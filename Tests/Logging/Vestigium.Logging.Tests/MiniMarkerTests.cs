namespace Vestigium.Logging.Tests;

public sealed class MiniMarkerTests : IDisposable
{
    private readonly string _dir = Path.Combine(Path.GetTempPath(), "vestigium-mini-mark", Guid.NewGuid().ToString("N"));

    public MiniMarkerTests()
    {
        Directory.CreateDirectory(_dir);
        VestigiumLogger.Shutdown();
    }

    [Fact]
    public void Start_and_end_markers_land_in_host_log()
    {
        VestigiumLogger.Initialize(o =>
        {
            o.AppId = "PingIQ";
            o.LogDirectory = _dir;
            o.OperationsLogEnabled = false;
        });
        VestigiumLog.Information(1, VestigiumStatus.None, "System", "Lifecycle", "host-ready");
        VestigiumLogger.Flush(TimeSpan.FromSeconds(2));
        var mini = new VestigiumLoggerMini("PingIQ", "PathPing", _dir,
            applicationCorrelationId: "app-1", miniCorrelationId: "mini-1");
        VestigiumLogger.Flush(TimeSpan.FromSeconds(2));
        var hostPath = Directory.GetFiles(_dir, "PingIQ.jsonl").Single();
        var host = SharedRead.Text(hostPath);
        Assert.Contains("Mini started.", host);
        Assert.Contains("app-1", host);
        Assert.Contains("mini-1", host);
        Assert.Contains("PingIQ.PathPing.jsonl", host);

        mini.Dispose();
        VestigiumLogger.Flush(TimeSpan.FromSeconds(2));
        host = SharedRead.Text(Directory.GetFiles(_dir, "PingIQ.jsonl").Single());
        Assert.Contains("Mini ended.", host);
        VestigiumLogger.Shutdown();
    }

    [Fact]
    public void No_marker_when_host_is_not_initialized()
    {
        VestigiumLogger.Shutdown();
        using var mini = new VestigiumLoggerMini("PingIQ", "PathPing", _dir);
        Assert.False(File.Exists(Path.Combine(_dir, "PingIQ.jsonl")));
    }

    public void Dispose()
    {
        VestigiumLogger.Shutdown();
        try { Directory.Delete(_dir, true); } catch { }
    }
}
