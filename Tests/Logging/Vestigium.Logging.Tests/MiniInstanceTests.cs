namespace Vestigium.Logging.Tests;

[Collection("VestigiumLogger")]
public sealed class MiniInstanceTests : IDisposable
{
    private readonly string _dir = Path.Combine(Path.GetTempPath(), "vestigium-mini", Guid.NewGuid().ToString("N"));

    public MiniInstanceTests() => Directory.CreateDirectory(_dir);

    [Fact]
    public void Writes_only_to_its_own_file()
    {
        using var mini = new VestigiumLoggerMini("PingIQ", "PathPing", _dir);
        mini.Information(0, "Network", "Probe", "mini-line");
        Assert.True(mini.Flush(TimeSpan.FromSeconds(2)));
        var path = Path.Combine(_dir, "PingIQ.PathPing.jsonl");
        Assert.Equal(path, mini.ActivePath);
        var text = SharedRead.Text(path);
        Assert.Contains("mini-line", text);
        Assert.False(File.Exists(Path.Combine(_dir, "PingIQ.jsonl")));
    }

    [Fact]
    public void Host_lines_do_not_enter_mini_file()
    {
        using var mini = new VestigiumLoggerMini("PingIQ", "PathPing", _dir);
        mini.Information(0, "Network", "Probe", "mini-only");
        mini.Flush(TimeSpan.FromSeconds(2));
        var text = SharedRead.Text(mini.ActivePath!);
        Assert.DoesNotContain("host-line", text);
        Assert.Contains("mini-only", text);
    }

    [Fact]
    public void Own_flood_does_not_affect_another_mini()
    {
        using var a = new VestigiumLoggerMini("PingIQ", "PathPing", _dir, floodThreshold: 1);
        using var b = new VestigiumLoggerMini("PingIQ", "TraceRt", _dir, floodThreshold: 5);
        for (var i = 0; i < 10; i++)
            a.Information(0, "Network", "Probe", "same");
        b.Information(0, "Network", "Probe", "same");
        Assert.True(a.Flush(TimeSpan.FromSeconds(2)));
        Assert.True(b.Flush(TimeSpan.FromSeconds(2)));
        var bText = SharedRead.Text(Path.Combine(_dir, "PingIQ.TraceRt.jsonl"));
        Assert.Contains("same", bText);
    }

    [Fact]
    public void Rejects_short_ids()
    {
        Assert.Throws<ArgumentException>(() => new VestigiumLoggerMini("AB", "PathPing", _dir));
        Assert.Throws<ArgumentException>(() => new VestigiumLoggerMini("PingIQ", "001", _dir));
    }

    public void Dispose()
    {
        try { Directory.Delete(_dir, true); } catch { }
    }
}
