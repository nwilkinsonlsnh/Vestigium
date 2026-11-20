# Vestigium.Logging — Application Developer Guide

**Application:** Logging  
**Document ID:** VEST-LOG-DX-APP  
**Version:** 1a  
**Status:** Staging  
**Date:** 19 November 2026  
**Target:** .NET 10 LTS  
**Companions:** `Requirements-Rev1a.md`, `DesignSpec-Rev-1a.md`

How to use the host logger. One process, one `VestigiumLogger`. Libraries call `VestigiumLog`.

---

## 1. Start

Call `Initialize` once, at startup, before any library writes.

```csharp
VestigiumLogger.Initialize(cfg =>
{
    cfg.AppId = "PingIQ";
    cfg.LogDirectory = @"C:\ProgramData\Vestigium\Logging\PingIQ";
    cfg.MinimumDiskLevel = VestigiumLogLevel.Information;
});
```

`AppId` needs at least 4 letters. Digits are allowed. Pure-digit and path separators are rejected.

A second `Initialize` with the same AppId and directory returns. Different options throws. Call `Shutdown` first if you need a new host.

```csharp
VestigiumLogger.Shutdown();
```

`Shutdown` is idempotent. A second call returns.

---

## 2. Write

```csharp
VestigiumLog.Information(1, VestigiumStatus.Success, "Network", "Probe", "probe complete");
VestigiumLog.Warning(2, VestigiumStatus.None, "System", "IO", "retry");
VestigiumLog.Error(3, VestigiumStatus.Failed, "System", "IO", "write failed", ex);
```

Every line needs an `EVENTID`. Category and subcategory must be registered in the taxonomy, or the host rewrites them to `Uncategorized/Unregistered` and emits a warning.

Levels: `Verbose`, `Debug`, `Information`, `Warning`, `Error`, `Fatal`.

---

## 3. Files

```text
%ProgramData%\Vestigium\Logging\<AppId>\<AppId>.jsonl
%ProgramData%\Vestigium\Logging\<AppId>\<AppId>.001.jsonl
%ProgramData%\Vestigium\Logging\<AppId>\<AppId>.002.jsonl
```

Rotation is by size. The writer reopens the highest part under the limit. No date in the name.

---

## 4. Correlation and properties

```csharp
VestigiumLog.Information(1, VestigiumStatus.None, "Network", "Probe", "hop",
    correlationId: "job-42",
    properties: new Dictionary<string, string?> { ["rtt"] = "12" });
```

`correlationId` lands in `CORRELATIONID`. Properties land in `PROPERTIES`. Keys must match `^[A-Za-z][A-Za-z0-9_]*$`. Max 16 keys. Values truncate at 256.

---

## 5. Exceptions

```csharp
try { /* ... */ }
catch (Exception ex)
{
    VestigiumLog.Thrown(ex, VestigiumStatus.Failed, "System", "IO", "probe failed");
}
```

An uncatalogued exception writes EVENTID 3 plus a DEBUG hint. After 20 distinct unknown types, the host recommends a custom catalog. Exception detail caps at 8192 characters.

---

## 6. Catalog

Register host events at or above 10000.

```csharp
VestigiumLogger.Initialize(cfg =>
{
    cfg.AppId = "PingIQ";
    cfg.RegisterEvent("ProbeDone", "App.ProbeDone", "Network", "Probe");
});
```

Load a custom catalog from a directory of JSON shards. `UnloadCustomCatalog` drops it.

---

## 7. Flood

The same message from the same app, category, and level is suppressed after the threshold. A summary line records the count. Tune with `FloodThresholdCount` and `FloodWindow`.

---

## 8. Disk tripwire

Dual gate: 10 percent free or 5 GB. When tripped, lines at or below `Debug` stop. Force it in a test or a host with `OverrideDiskPressure`.

```csharp
VestigiumLogger.OverrideDiskPressure(true);
VestigiumLogger.OverrideDiskPressure(null); // clear
```

---

## 9. Sealing

Opt-in. HMAC-SHA256 trailer on the last line.

```csharp
cfg.SealEnabled = true;
cfg.SealKeyPath = @"C:\ProgramData\Vestigium\Logging\PingIQ\host.key";
```

Verify returns `Valid`, `NoTrailer`, `Torn`, `HashMismatch`, `BadSignature`, `MissingFile`, or `KeyMismatch`.

---

## 10. Read

```csharp
var head = VestigiumLogReader.Head(20);
var tail = VestigiumLogReader.Tail(20);
```

Complete lines only. The seal trailer is skipped.

---

## 11. Janitor and archive

```csharp
VestigiumLogJanitor.DeleteHostLogs();
VestigiumLogArchive.ArchiveHostLogs(TimeSpan.FromDays(30), archiveDir);
```

The host janitor and archive touch only host files. Mini files are left alone.

---

## 12. Shutdown

```csharp
VestigiumLogger.Flush(TimeSpan.FromSeconds(2));
VestigiumLogger.Shutdown();
```

`Shutdown` drains the writers, appends the trailer if sealing is on, and disposes the channel.

---

## 13. Before init

Default is `Throw`. A library that logs before `Initialize` throws. Set `UninitializedBehavior = NoOp` to drop those calls.

---

## 14. Revision History

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 19 Nov 2026 | Vestigium | Initial developer guide for the host logger. |
