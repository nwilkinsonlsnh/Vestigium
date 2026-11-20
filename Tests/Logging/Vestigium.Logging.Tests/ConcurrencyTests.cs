namespace Vestigium.Logging.Tests;

[Collection("VestigiumLogger")]
public sealed class ConcurrencyTests : IDisposable
{
    private readonly string _dir = Path.Combine(Path.GetTempPath(), "vestigium-conc", Guid.NewGuid().ToString("N"));

    public ConcurrencyTests()
    {
        Directory.CreateDirectory(_dir);
        VestigiumLogger.Shutdown();
    }

    [Fact]
    public void Second_initialize_same_options_is_a_no_op()
    {
        VestigiumLogger.Initialize(o => { o.AppId = "PingIQ"; o.LogDirectory = _dir; o.OperationsLogEnabled = false; });
        VestigiumLog.Information(1, VestigiumStatus.None, "System", "IO", "first");
        VestigiumLogger.Flush(TimeSpan.FromSeconds(2));
        VestigiumLogger.Initialize(o => { o.AppId = "PingIQ"; o.LogDirectory = _dir; o.OperationsLogEnabled = false; });
        VestigiumLogger.Flush(TimeSpan.FromSeconds(2));
        var text = SharedRead.Text(Directory.GetFiles(_dir, "PingIQ.jsonl").Single());
        Assert.Contains("first", text);
        VestigiumLogger.Shutdown();
        VestigiumLogger.Shutdown();
    }

    [Fact]
    public void Second_initialize_different_options_throws()
    {
        VestigiumLogger.Initialize(o => { o.AppId = "PingIQ"; o.LogDirectory = _dir; o.OperationsLogEnabled = false; });
        var other = Path.Combine(_dir, "other");
        Directory.CreateDirectory(other);
        Assert.Throws<InvalidOperationException>(() =>
            VestigiumLogger.Initialize(o => { o.AppId = "PingIQ"; o.LogDirectory = other; }));
        VestigiumLogger.Shutdown();
    }

    [Fact]
    public void Parallel_writes_produce_intact_lines()
    {
        VestigiumLogger.Initialize(o => { o.AppId = "PingIQ"; o.LogDirectory = _dir; o.OperationsLogEnabled = false; });
        Parallel.For(0, 50, i =>
            VestigiumLog.Information(1, VestigiumStatus.None, "System", "IO", $"line-{i}"));
        VestigiumLogger.Flush(TimeSpan.FromSeconds(3));
        var text = SharedRead.Text(Directory.GetFiles(_dir, "PingIQ.jsonl").Single());
        var lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        Assert.True(lines.Length >= 50);
        foreach (var line in lines)
            Assert.True(line.StartsWith("{") && line.EndsWith("}"), line);
        VestigiumLogger.Shutdown();
    }

    [Fact]
    public void Mini_during_shutdown_does_not_throw()
    {
        VestigiumLogger.Initialize(o => { o.AppId = "PingIQ"; o.LogDirectory = _dir; o.OperationsLogEnabled = false; });
        VestigiumLogger.Shutdown();
        var mini = new VestigiumLoggerMini("PingIQ", "PathPing", _dir);
        mini.Dispose();
        Assert.True(File.Exists(Path.Combine(_dir, "PingIQ.PathPing.jsonl")));
    }

    public void Dispose()
    {
        VestigiumLogger.Shutdown();
        try { Directory.Delete(_dir, true); } catch { }
    }
}
