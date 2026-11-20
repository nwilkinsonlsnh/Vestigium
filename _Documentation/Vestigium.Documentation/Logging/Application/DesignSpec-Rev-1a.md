# Vestigium.Logging — Design Specification

**Application:** Logging
**Document ID:** VEST-LOG-SDS-000
**Version:** 1a
**Status:** Staging — monorepo release
**Date:** 15 November 2026
**Target:** .NET 10 LTS / `net10.0`
**Companions:** `Requirements-Rev1a.md`
**Project:** `Logging/Vestigium.Logging/`

How the staged logger meets the 1a requirements. Source material is Design 2.2.

---

## 0. Shape

One `VestigiumLogger` host per process. `Initialize` builds options, the catalog, the taxonomy snapshot, the flood tracker, the disk monitor, the host JSONL writer, and, when enabled, the operations writer and the seal ring. `Shutdown` drains both writers, appends trailers if sealing is on, and disposes the channel.

Libraries call `VestigiumLog`. Catalog, `Events`, and housekeeping require a host.

---

## 1. Write path

```text
VestigiumLog.*
  → VestigiumLogger.Emit
      → require EVENTID / STATUS / CATEGORY / SUBCATEGORY / MESSAGE
      → taxonomy normalize
      → catalog resolve (explicit id, else exception type, else general 0–4)
      → flood Observe
      → format JSON line
      → channel + recent buffer
      → disk queue if LEVEL >= MinimumDiskLevel and disk is not dropping Verbose/Debug
      → VestigiumJsonlWriter.Enqueue
```

---

## 2. Catalog

Embedded shards cover 0–14, exception types, and the operations range 5000–9999. `LoadCustomCatalog` overlays `>= 10000`. Resolve prefers an explicit id, then exception `FullName` (a custom `FullName` at `>= 10000` wins), then the level fallback.

---

## 3. Disk and seal

The writer is owned. Retention is count-based. Seal is opt-in: a key ring, an in-file trailer, and a verify peel. No key means no trailer.

The operations log is a second writer with its own destination when the host enables it. Archive and janitor are opt-in and use separate host and operations destinations.

---

## 4. Subscribers

`Events` is an observable. `EventReader` is the channel reader. A WPF host marshals to the dispatcher itself. The library does not take a dispatcher.

---

## 5. Projects

| Project | Role |
|---|---|
| `Vestigium.Logging` | The library. Packable. `InternalsVisibleTo` the test assembly. |
| `Vestigium.Logging.Tests` | xUnit. References the library. |
| `Vestigium.Logging.Demo` | WPF sample. Not part of the contract. |

Other Vestigium projects keep the `Vestigium.Logging` 2.0.0 package reference. These three do not take that package.

---

## 6. Decisions

| Decision | Why |
|---|---|
| Required `EVENTID` | A line without an id is not a Vestigium line. |
| Reserved 5000–9999 | Operations stays distinguishable from host ids. |
| Overlay at `>= 10000` | Hosts extend without editing the embedded catalog. |
| Owned writer | The library controls rotation, seal, and shutdown drain. |
| No-op before init | A library can log before the host has started, when the host allows it. |

---

## 7. Non-goals

No host UI in the library. No remote sink. No host event ids in this package.

---


---

## 9. Mini

`VestigiumLoggerMini` is constructed with an AppId and a MiniId. It owns a `VestigiumJsonlWriter` pointed at `<AppId>.<MiniId>` and a private `FloodTracker`.

| Concern | Design |
|---|---|
| Naming | `LogId.Validate` enforces the 4-letter rule. The writer stem is `AppId.MiniId`. |
| Rotation | Size-based. Highest under-limit part is reopened. No date in the name. |
| Correlation | `ApplicationCorrelationId` on `CORRELATIONID`. `MiniCorrelationId` generated or accepted, stored in `PROPERTIES`. |
| Markers | `WriteHostMarker` calls `VestigiumLog.Information` with `System/Configuration` and both ids. Automatic on construct and dispose when the host is running. |
| Catalog | Reads the host catalog when initialized. Overlay dictionary checked first. Ids below 10000 rejected. |
| Standalone | If the host is not initialized, the constructor writes a warning line into the Mini file. |
| Dispose | Flush, optional seal trailer via `VestigiumSealKeyRing`, end marker, writer dispose. |
| Janitor | `DeleteMiniLogs(directory, appId, miniId, age)` matches only `<AppId>.<MiniId>` parts. Host janitor skips Mini files via `IsHostFile`. |
| Archive | `ArchiveMiniLogs` copies aged Mini parts, writes a SHA256 sidecar, deletes the source. Host archive skips Mini files. When sealing is enabled, archive proceeds only on Valid or NoTrailer. |
| Tripwire | Dual gate: 10 percent free or 5 GB. When tripped, writes stop and one warning line records the trip. Disable with `tripwireEnabled: false`. |
| Sealing | Opt-in. Own key ring per Mini. HMAC-SHA256 trailer on dispose. Verify results match the host. |

---

## 8. Revision History

Initial monorepo release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 15 Nov 2026 | Nathaniel Wilkinson | Initial monorepo release. Rewritten from Vestigium.Logging SRS 2.3 and Design 2.2. |
| 1a.1 | 19 Nov 2026 | Vestigium | Added Mini design: type, file, flood, correlation, markers, overlay, standalone, dispose, janitor, multi-instance, archive. |
| 1a.2 | 19 Nov 2026 | Vestigium | Added Mini tripwire, sealing, and archive seal gate. |
