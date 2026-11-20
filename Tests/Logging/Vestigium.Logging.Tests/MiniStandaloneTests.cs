namespace Vestigium.Logging.Tests;

public sealed class MiniStandaloneTests : IDisposable
{
    private readonly string _dir = Path.Combine(Path.GetTempPath(), "vestigium-mini-solo", Guid.NewGuid().ToString("N"));

    public MiniStandaloneTests()
    {
        Directory.CreateDirectory(_dir);
        VestigiumLogger.Shutdown();
    }

    [Fact]
    public void Runs_and_warns_when_host_is_not_initialized()
    {
        using var mini = new VestigiumLoggerMini("PingIQ", "PathPing", _dir);
        Assert.True(mini.Flush(TimeSpan.FromSeconds(2)));
        var text = SharedRead.Text(mini.ActivePath!);
        Assert.Contains("Mini started with no host logger.", text);
        Assert.Contains("Warning", text);
        Assert.False(File.Exists(Path.Combine(_dir, "PingIQ.jsonl")));
    }

    [Fact]
    public void Does_not_throw_when_host_is_not_initialized()
    {
        var ex = Record.Exception(() =>
        {
            using var mini = new VestigiumLoggerMini("PingIQ", "PathPing", _dir);
            mini.Information(1, "Network", "Probe", "still-works");
            mini.Flush(TimeSpan.FromSeconds(2));
        });
        Assert.Null(ex);
    }

    public void Dispose()
    {
        try { Directory.Delete(_dir, true); } catch { }
    }
}
