using Vestigium.Logging;
using Vestigium.Helpers.Analytics;
using Vestigium.Helpers.FileIo;

namespace Vestigium.Helpers;

/// <summary>Test-only stand-in. Hosts initialize Vestigium.Logging.</summary>
public static class TestLog
{
    public const string Category = "Helpers";

    public static class AppIds
    {
        public const string Core = "Core";
        public const string Analytics = "Analytics";
        public const string Charts = "Charts";
        public const string ClosedXml = "ClosedXml";
        public const string Csv = "Csv";
        public const string Encryption = "Encryption";
        public const string FileIo = "FileIo";
        public const string Hashing = "Hashing";
        public const string Json = "Json";
        public const string Kql = "Kql";
        public const string Network = "Network";
        public const string Processes = "Processes";
        public const string Services = "Services";
        public const string WinReg = "WinReg";
        public const string Xml = "Xml";
    }

    public static class Subcategories
    {
        public const string Probe = "Probe";
        public const string Guard = "Guard";
        public const string Identity = "Identity";
        public const string Job = "Job";
        public const string Recon = "Recon";
        public const string Copy = "Copy";
        public const string Move = "Move";
        public const string Delete = "Delete";
        public const string Mirror = "Mirror";
        public const string Index = "Index";
        public const string Progress = "Progress";
        public const string Compare = "Compare";
        public const string Prune = "Prune";
        public const string SecureDelete = "SecureDelete";
        public const string Session = "Session";
        public const string Document = "Document";
        public const string Query = "Query";
        public const string Snapshot = "Snapshot";
        public const string Diff = "Diff";
        public const string Commit = "Commit";
        public const string Save = "Save";
        public const string Jsonl = "Jsonl";
        public const string Stats = "Stats";
    }

    public static readonly TaxonomyGate Taxonomy = new();
    public static bool IsInitialized => VestigiumLogger.IsInitialized;
    public static IReadOnlyList<string> RecentJsonLines => VestigiumLogger.RecentJsonLines;
    public static string? LogDirectory { get; private set; }
    public static void Shutdown() => VestigiumLogger.Shutdown();

    public static void InitializeHost(string appId, Action<VestigiumLoggerOptions>? configure = null)
    {
        var dir = Path.Combine(Path.GetTempPath(), "VestigiumHelpersTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        VestigiumLogger.Initialize(cfg =>
        {
            cfg.AppId = appId;
            cfg.LogDirectory = dir;
            cfg.MinimumDiskLevel = VestigiumLogLevel.Debug;
            cfg.OperationsLogEnabled = false;
            configure?.Invoke(cfg);
            LogDirectory = cfg.LogDirectory;
            FileIoCatalog.Register(cfg);
            AnalyticsCatalog.Register(cfg);
        });
    }

    public static void Information(string appId, VestigiumStatus status, string subcategory, string message)
    {
        if (!VestigiumLogger.IsInitialized) return;
        VestigiumLog.Information(10000, status, Category, subcategory, message, appId: appId);
    }

    public sealed class TaxonomyGate
    {
        public bool IsSubcategoryRegistered(string category, string subcategory) =>
            !string.IsNullOrWhiteSpace(category) && !string.IsNullOrWhiteSpace(subcategory);
    }
}
