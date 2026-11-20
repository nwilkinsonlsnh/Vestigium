namespace Vestigium.Logging.Tests;

[Collection("VestigiumLogger")]
public sealed class MiniParityTests : IDisposable
{
    private readonly string _dir = Path.Combine(Path.GetTempPath(), "vestigium-mini-par", Guid.NewGuid().ToString("N"));

    public MiniParityTests()
    {
        Directory.CreateDirectory(_dir);
        VestigiumLogger.Shutdown();
        VestigiumLoggerMini.TripwireOverride = null;
    }

    [Fact]
    public void Tripwire_stops_writes_and_records_the_trip()
    {
        using var mini = new VestigiumLoggerMini("PingIQ", "PathPing", _dir, tripwireEnabled: true);
        VestigiumLoggerMini.TripwireOverride = true;
        mini.Information(1, "Network", "Probe", "blocked");
        Assert.True(mini.Flush(TimeSpan.FromSeconds(2)));
        Assert.True(mini.IsDiskTripped);
        var text = SharedRead.Text(mini.ActivePath!);
        Assert.Contains("Mini disk tripwire tripped.", text);
        Assert.DoesNotContain("blocked", text);
        VestigiumLoggerMini.TripwireOverride = null;
    }

    [Fact]
    public void Disabled_tripwire_does_not_trip()
    {
        using var mini = new VestigiumLoggerMini("PingIQ", "PathPing", _dir, tripwireEnabled: false);
        VestigiumLoggerMini.TripwireOverride = true;
        mini.Information(1, "Network", "Probe", "passes");
        Assert.True(mini.Flush(TimeSpan.FromSeconds(2)));
        Assert.Contains("passes", SharedRead.Text(mini.ActivePath!));
        VestigiumLoggerMini.TripwireOverride = null;
    }

    [Fact]
    public void Seal_writes_trailer_and_verifies_valid()
    {
        var key = Path.Combine(_dir, "mini.key");
        var mini = new VestigiumLoggerMini("PingIQ", "PathPing", _dir, sealEnabled: true, sealKeyPath: key);
        mini.Information(1, "Network", "Probe", "sealed");
        Assert.True(mini.Flush(TimeSpan.FromSeconds(2)));
        var path = mini.ActivePath!;
        mini.Dispose();
        var text = SharedRead.Text(path);
        Assert.Contains("VESTIGIUM_TRAILER", text);
        var ring = VestigiumSealKeyRing.Open(key);
        Assert.Equal(VestigiumSealVerifyResult.Valid, VestigiumLogSeal.Verify(path, ring).Result);
        Assert.Equal(VestigiumSealVerifyResult.MissingFile, VestigiumLogSeal.Verify(path + ".gone", ring).Result);
    }

    [Fact]
    public void Archive_refuses_a_tampered_sealed_file()
    {
        var key = Path.Combine(_dir, "mini.key");
        var mini = new VestigiumLoggerMini("PingIQ", "PathPing", _dir, sealEnabled: true, sealKeyPath: key);
        mini.Information(1, "Network", "Probe", "clean");
        Assert.True(mini.Flush(TimeSpan.FromSeconds(2)));
        var path = mini.ActivePath!;
        mini.Dispose();
        File.WriteAllText(path, File.ReadAllText(path).Replace("clean", "TAMPER"));
        File.SetLastWriteTimeUtc(path, DateTime.UtcNow.AddDays(-30));
        var dst = Path.Combine(_dir, "arc");
        var ring = VestigiumSealKeyRing.Open(key);
        var result = VestigiumLogArchive.ArchiveMiniLogs(TimeSpan.FromDays(7), dst, _dir, "PingIQ", "PathPing", ring);
        Assert.Equal(0, result.Archived);
        Assert.True(File.Exists(path));
    }

    [Fact]
    public void Head_and_tail_skip_trailer_and_honor_cap()
    {
        var key = Path.Combine(_dir, "mini.key");
        var mini = new VestigiumLoggerMini("PingIQ", "PathPing", _dir, sealEnabled: true, sealKeyPath: key);
        for (var i = 0; i < 5; i++)
            mini.Information(1, "Network", "Probe", $"line-{i}");
        Assert.True(mini.Flush(TimeSpan.FromSeconds(2)));
        var head = mini.Head(3);
        Assert.True(head.Count >= 2);
        Assert.Contains(head, l => l.Contains("line-0"));
        var tail = mini.Tail(2);
        Assert.True(tail.Count <= 2);
        Assert.DoesNotContain(tail, l => l.Contains("VESTIGIUM_TRAILER"));
        mini.Dispose();
    }

    [Fact]
    public void Pending_count_and_flush_reflect_the_queue()
    {
        using var mini = new VestigiumLoggerMini("PingIQ", "PathPing", _dir);
        mini.Information(1, "Network", "Probe", "queued");
        Assert.True(mini.PendingCount >= 0);
        Assert.True(mini.Flush(TimeSpan.FromSeconds(2)));
        Assert.Equal(0, mini.PendingCount);
        Assert.Contains("queued", SharedRead.Text(mini.ActivePath!));
    }

    [Fact]
    public void Properties_are_capped_at_sixteen_keys()
    {
        var bag = new Dictionary<string, string?>();
        for (var i = 0; i < 30; i++) bag[$"k{i}"] = "v";
        var clean = VestigiumPropertyBag.Sanitize(bag);
        Assert.NotNull(clean);
        Assert.True(clean!.Count <= 16);
    }

    public void Dispose()
    {
        VestigiumLoggerMini.TripwireOverride = null;
        VestigiumLogger.Shutdown();
        try { Directory.Delete(_dir, true); } catch { }
    }
}
