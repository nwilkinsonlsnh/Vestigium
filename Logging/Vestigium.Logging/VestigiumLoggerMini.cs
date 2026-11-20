namespace Vestigium.Logging;

public sealed class VestigiumLoggerMini : IDisposable
{
    private readonly FloodTracker _flood;
    private readonly VestigiumJsonlWriter _writer;
    private readonly int _pid = Environment.ProcessId;
    private int _disposed;

    public string AppId { get; }
    public string MiniId { get; }
    public string? ActivePath => _writer.ActivePath;

    public string? ApplicationCorrelationId { get; }
    public string MiniCorrelationId { get; }

    public VestigiumLoggerMini(string appId, string miniId, string? directory = null,
        int floodThreshold = 5, TimeSpan? floodWindow = null,
        string? applicationCorrelationId = null, string? miniCorrelationId = null)
    {
        AppId = LogId.Validate(appId, nameof(appId));
        MiniId = LogId.Validate(miniId, nameof(miniId));
        ApplicationCorrelationId = string.IsNullOrWhiteSpace(applicationCorrelationId) ? null : applicationCorrelationId;
        MiniCorrelationId = string.IsNullOrWhiteSpace(miniCorrelationId) ? Guid.NewGuid().ToString("N") : miniCorrelationId;
        var dir = string.IsNullOrWhiteSpace(directory) ? ResolveDirectory(AppId) : directory;
        _flood = new FloodTracker(floodThreshold, floodWindow ?? TimeSpan.FromSeconds(30));
        _writer = new VestigiumJsonlWriter(dir, Stem, fileSizeLimitBytes: 20L * 1024 * 1024);
    }

    public string Stem => $"{AppId}.{MiniId}";

    public void Write(int eventId, VestigiumLogLevel level, VestigiumStatus status,
        string category, string subcategory, string message, string? correlationId = null)
    {
        ObjectDisposedException.ThrowIf(Volatile.Read(ref _disposed) != 0, this);
        var now = DateTimeOffset.UtcNow;
        var identity = new FloodIdentity(Stem, category, level.ToString(), message);
        var (write, _) = _flood.Observe(identity, now, subcategory);
        if (!write) return;
        var props = new Dictionary<string, string?> { ["MiniCorrelationId"] = MiniCorrelationId };
        var evt = new VestigiumLogEvent(now, _pid, Environment.CurrentManagedThreadId,
            level, status, AppId, category, subcategory, message, null,
            correlationId ?? ApplicationCorrelationId, VestigiumPropertyBag.Sanitize(props), eventId, null);
        _writer.Enqueue(evt.ToJsonLine());
    }

    public void Information(int eventId, string category, string subcategory, string message) =>
        Write(eventId, VestigiumLogLevel.Information, VestigiumStatus.None, category, subcategory, message);

    public bool Flush(TimeSpan timeout) => _writer.Flush(timeout);

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _disposed, 1) == 1) return;
        _writer.Dispose();
    }

    private static string ResolveDirectory(string appId)
    {
        var root = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        if (string.IsNullOrWhiteSpace(root)) root = Path.Combine(Path.GetTempPath(), "Vestigium");
        return Path.Combine(root, "Vestigium", "Logging", appId);
    }
}
