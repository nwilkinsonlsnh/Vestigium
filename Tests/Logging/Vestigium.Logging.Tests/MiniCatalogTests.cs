namespace Vestigium.Logging.Tests;

[Collection("VestigiumLogger")]
public sealed class MiniCatalogTests : IDisposable
{
    private readonly string _dir = Path.Combine(Path.GetTempPath(), "vestigium-mini-cat", Guid.NewGuid().ToString("N"));

    public MiniCatalogTests()
    {
        Directory.CreateDirectory(_dir);
        VestigiumLogger.Shutdown();
    }

    [Fact]
    public void Default_resolves_from_host_catalog()
    {
        VestigiumLogger.Initialize(o => { o.AppId = "PingIQ"; o.LogDirectory = _dir; o.OperationsLogEnabled = false; });
        using var mini = new VestigiumLoggerMini("PingIQ", "PathPing", _dir);
        mini.Information(1, "System", "Lifecycle", "info");
        Assert.True(mini.Flush(TimeSpan.FromSeconds(2)));
        var text = SharedRead.Text(mini.ActivePath!);
        Assert.Contains("General.Information", text);
        VestigiumLogger.Shutdown();
    }

    [Fact]
    public void Overlay_is_visible_only_to_that_mini()
    {
        VestigiumLogger.Initialize(o => { o.AppId = "PingIQ"; o.LogDirectory = _dir; o.OperationsLogEnabled = false; });
        var overlay = new VestigiumEventDefinition(10000, "Probe.Start", "PingIQ.Probe.Start", "Network", "Probe", "Information", "Custom", true);
        using var mini = new VestigiumLoggerMini("PingIQ", "PathPing", _dir, overlay: [overlay]);
        mini.Information(10000, "Network", "Probe", "probe");
        mini.Flush(TimeSpan.FromSeconds(2));
        Assert.Contains("Probe.Start", SharedRead.Text(mini.ActivePath!));
        Assert.False(VestigiumLogger.Catalog.TryGetById(10000, out _));
        VestigiumLogger.Shutdown();
    }

    public void Dispose()
    {
        VestigiumLogger.Shutdown();
        try { Directory.Delete(_dir, true); } catch { }
    }
}
