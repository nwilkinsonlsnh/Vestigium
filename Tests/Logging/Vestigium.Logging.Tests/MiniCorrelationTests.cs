namespace Vestigium.Logging.Tests;

[Collection("VestigiumLogger")]
public sealed class MiniCorrelationTests : IDisposable
{
    private readonly string _dir = Path.Combine(Path.GetTempPath(), "vestigium-mini-corr", Guid.NewGuid().ToString("N"));

    public MiniCorrelationTests() => Directory.CreateDirectory(_dir);

    [Fact]
    public void Both_ids_appear_on_every_line()
    {
        using var mini = new VestigiumLoggerMini("PingIQ", "PathPing", _dir,
            applicationCorrelationId: "app-123", miniCorrelationId: "mini-456");
        mini.Information(0, "Network", "Probe", "line");
        Assert.True(mini.Flush(TimeSpan.FromSeconds(2)));
        var text = SharedRead.Text(mini.ActivePath!);
        Assert.Contains("app-123", text);
        Assert.Contains("mini-456", text);
        Assert.Contains("MiniCorrelationId", text);
    }

    [Fact]
    public void Generates_mini_id_when_not_supplied()
    {
        using var mini = new VestigiumLoggerMini("PingIQ", "PathPing", _dir, applicationCorrelationId: "app-1");
        Assert.False(string.IsNullOrWhiteSpace(mini.MiniCorrelationId));
        mini.Information(0, "Network", "Probe", "line");
        mini.Flush(TimeSpan.FromSeconds(2));
        Assert.Contains(mini.MiniCorrelationId, SharedRead.Text(mini.ActivePath!));
    }

    [Fact]
    public void Caller_supplied_mini_id_is_stable()
    {
        using var mini = new VestigiumLoggerMini("PingIQ", "PathPing", _dir, miniCorrelationId: "fixed");
        Assert.Equal("fixed", mini.MiniCorrelationId);
        mini.Information(0, "Network", "Probe", "a");
        mini.Information(0, "Network", "Probe", "b");
        mini.Flush(TimeSpan.FromSeconds(2));
        var text = SharedRead.Text(mini.ActivePath!);
        Assert.Equal(3, text.Split("fixed").Length - 1);
    }

    public void Dispose()
    {
        try { Directory.Delete(_dir, true); } catch { }
    }
}
