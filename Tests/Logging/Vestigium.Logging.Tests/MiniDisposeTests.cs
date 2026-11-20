namespace Vestigium.Logging.Tests;

public sealed class MiniDisposeTests : IDisposable
{
    private readonly string _dir = Path.Combine(Path.GetTempPath(), "vestigium-mini-disp", Guid.NewGuid().ToString("N"));

    public MiniDisposeTests()
    {
        Directory.CreateDirectory(_dir);
        VestigiumLogger.Shutdown();
    }

    [Fact]
    public void Dispose_flushes_and_writes_end_marker_when_host_runs()
    {
        VestigiumLogger.Initialize(o => { o.AppId = "PingIQ"; o.LogDirectory = _dir; o.OperationsLogEnabled = false; });
        var mini = new VestigiumLoggerMini("PingIQ", "PathPing", _dir);
        mini.Information(1, "Network", "Probe", "line");
        mini.Dispose();
        VestigiumLogger.Flush(TimeSpan.FromSeconds(2));
        var miniText = SharedRead.Text(Path.Combine(_dir, "PingIQ.PathPing.jsonl"));
        Assert.Contains("line", miniText);
        var host = SharedRead.Text(Path.Combine(_dir, "PingIQ.jsonl"));
        Assert.Contains("Mini ended.", host);
        VestigiumLogger.Shutdown();
    }

    [Fact]
    public void Dispose_writes_trailer_when_sealing_is_enabled()
    {
        var key = Path.Combine(_dir, "mini.key");
        var mini = new VestigiumLoggerMini("PingIQ", "PathPing", _dir, sealEnabled: true, sealKeyPath: key);
        mini.Information(1, "Network", "Probe", "sealed");
        Assert.True(mini.Flush(TimeSpan.FromSeconds(2)));
        var path = mini.ActivePath!;
        mini.Dispose();
        var text = SharedRead.Text(path);
        Assert.Contains("sealed", text);
        Assert.Contains("VESTIGIUM_TRAILER", text);
        var ring = VestigiumSealKeyRing.Open(key);
        Assert.Equal(VestigiumSealVerifyResult.Valid, VestigiumLogSeal.Verify(path, ring).Result);
    }

    [Fact]
    public void Dispose_writes_no_trailer_when_sealing_is_off()
    {
        var mini = new VestigiumLoggerMini("PingIQ", "PathPing", _dir);
        mini.Information(1, "Network", "Probe", "plain");
        Assert.True(mini.Flush(TimeSpan.FromSeconds(2)));
        var path = mini.ActivePath!;
        mini.Dispose();
        Assert.DoesNotContain("VESTIGIUM_TRAILER", SharedRead.Text(path));
    }

    public void Dispose()
    {
        VestigiumLogger.Shutdown();
        try { Directory.Delete(_dir, true); } catch { }
    }
}
