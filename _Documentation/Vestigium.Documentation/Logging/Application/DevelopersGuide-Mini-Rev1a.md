# Vestigium.Logging — Mini Developer Guide

**Application:** Logging  
**Document ID:** VEST-LOG-DX-MINI  
**Version:** 1a  
**Status:** Staging  
**Date:** 19 November 2026  
**Target:** .NET 10 LTS  
**Companions:** `Requirements-Rev1a.md`, `DesignSpec-Rev-1a.md`

How to use `VestigiumLoggerMini`. An instanced logger for a scoped series of events that should not land in the application log.

---

## 1. Construct

```csharp
using var mini = new VestigiumLoggerMini("PingIQ", "PathPing");
mini.Information(1, "Network", "Probe", "hop 1");
```

`AppId` and `MiniId` each need at least 4 letters. Digits are allowed. Pure-digit and separators are rejected.

Mini is not a singleton. Dispose it when the scope ends.

---

## 2. Files

```text
%ProgramData%\Vestigium\Logging\<AppId>\<AppId>.<MiniId>.jsonl
%ProgramData%\Vestigium\Logging\<AppId>\<AppId>.<MiniId>.001.jsonl
```

Host file is `<AppId>.jsonl`. Mini file is `<AppId>.<MiniId>.jsonl`. Rolled parts are `.001`, `.002`. No date in the name.

Pass a directory to override the default.

```csharp
var mini = new VestigiumLoggerMini("PingIQ", "PathPing", directory: @"D:\logs");
```

---

## 3. Correlation

```csharp
var mini = new VestigiumLoggerMini("PingIQ", "PathPing",
    applicationCorrelationId: "job-42",
    miniCorrelationId: "fixed");
```

`applicationCorrelationId` lands in `CORRELATIONID`. `miniCorrelationId` is generated if you omit it, and lands in `PROPERTIES.MiniCorrelationId`. Both appear on every Mini line.

---

## 4. Markers

When the host is running, Mini writes a start marker on construct and an end marker on dispose. Both carry the correlation ids and the Mini file path. The marker uses `System/Configuration`.

If the host shuts down between the check and the write, the marker is dropped. It does not throw.

---

## 5. Standalone

Mini runs when the host is not initialized. It writes a warning line into its own file and does not throw.

```csharp
VestigiumLogger.Shutdown();
using var mini = new VestigiumLoggerMini("PingIQ", "PathPing");
// writes: Mini started with no host logger.
```

---

## 6. Catalog overlay

Mini uses the host catalog by default. Opt in to an overlay for that Mini only. Ids must be `>= 10000`.

```csharp
var overlay = new[]
{
    new VestigiumEventDefinition(10001, "PathPing.Hop", "Network", "Probe")
};
var mini = new VestigiumLoggerMini("PingIQ", "PathPing", overlay: overlay);
```

The overlay is visible only to that Mini.

---

## 7. Flood

Mini has its own flood tracker. It does not share state with the host or with other Mini instances.

```csharp
var mini = new VestigiumLoggerMini("PingIQ", "PathPing",
    floodThreshold: 5, floodWindow: TimeSpan.FromSeconds(10));
```

---

## 8. Disk tripwire

Dual gate: 10 percent free or 5 GB. When tripped, writes stop and one warning line records the trip.

```csharp
var mini = new VestigiumLoggerMini("PingIQ", "PathPing",
    tripwireEnabled: true, freePercent: 10, bytesFloor: 5L * 1024 * 1024 * 1024);

if (mini.IsDiskTripped) { /* stopped */ }
```

Disable with `tripwireEnabled: false`.

---

## 9. Sealing

Opt-in. Own key ring per Mini. HMAC-SHA256 trailer on dispose.

```csharp
var mini = new VestigiumLoggerMini("PingIQ", "PathPing",
    sealEnabled: true, sealKeyPath: @"D:\logs\mini.key");
mini.Information(1, "Network", "Probe", "sealed");
mini.Dispose(); // writes the trailer
```

Verify with the host API. Results match: `Valid`, `NoTrailer`, `Torn`, `HashMismatch`, `BadSignature`, `MissingFile`, `KeyMismatch`.

---

## 10. Queue

```csharp
mini.Information(1, "Network", "Probe", "queued");
var pending = mini.PendingCount;
mini.Flush(TimeSpan.FromSeconds(2));
```

`PendingCount` is the depth of the Mini queue. `Flush` waits up to the timeout.

---

## 11. Head and Tail

```csharp
var head = mini.Head(20);
var tail = mini.Tail(20);
```

Complete lines only. The seal trailer is skipped. Same cap as the host.

---

## 12. Janitor

Mini has its own janitor. It deletes only its own aged parts. The application log is untouched. The host janitor does not delete Mini files.

```csharp
VestigiumLogJanitor.DeleteMiniLogs(_dir, "PingIQ", "PathPing", TimeSpan.FromDays(7));
```

---

## 13. Archive

Opt-in. Copies aged Mini parts, writes a SHA256 sidecar, deletes the source. Does not touch the application log.

When sealing is enabled, archive proceeds only on `Valid` or `NoTrailer`.

```csharp
VestigiumLogArchive.ArchiveMiniLogs(
    TimeSpan.FromDays(7), archiveDir, _dir, "PingIQ", "PathPing", ring);
```

---

## 14. Several at once

Multiple Mini instances may run against the same AppId. Each owns its file, flood tracker, correlation, and janitor. They do not share state.

```csharp
using var a = new VestigiumLoggerMini("PingIQ", "PathPing");
using var b = new VestigiumLoggerMini("PingIQ", "TraceRt");
```

---

## 15. Dispose

```csharp
mini.Dispose();
```

Dispose flushes, writes the trailer if sealing is on, writes the end marker if the host is running, and releases the writer. A second dispose returns.

---

## 16. Revision History

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 19 Nov 2026 | Vestigium | Initial developer guide for Mini. |
