# Vestigium.Logging — Requirements Specification

**Document ID:** VEST-LOG-SRS-000
**Version:** 1a
**Status:** Staging — monorepo release
**Date:** 15 November 2026
**Target:** .NET 10 LTS / `net10.0`
**Companions:** `DesignSpec_Logging_Rev-1a.md`
**Project:** `Logging/Vestigium.Logging/`
**Package:** `Vestigium.Logging` 2.0.0

Binding contract for the logger as staged in the Vestigium monorepo. Source material is the Vestigium.Logging SRS 2.3, rewritten to this library format.

---

## 0. Purpose

One process-wide JSON Lines logger. A host calls `Initialize` once. Libraries call `VestigiumLog`. The library answers:

- a required `EVENTID` on every line;
- a catalog that maps ids, exception types, and levels;
- flood protection so a hot path cannot fill the disk;
- an owned JSONL writer with optional HMAC seal;
- a WPF-safe event stream for a host that wants to subscribe.

It does not own the host UI. It does not assign host event ids below 10000 except the reserved ranges. The host owns the log folder.

```csharp
VestigiumLogger.Initialize(cfg =>
{
    cfg.AppId = "PingIQ";
    cfg.LogDirectory = logDir;
    cfg.MinimumDiskLevel = VestigiumLogLevel.Information;
});
VestigiumLog.Information("PingIQ", VestigiumStatus.Success, "Probe", "Run", "probe complete");
```

---

## 1. Binding language

| Term | Meaning |
|---|---|
| **Host** | The one `VestigiumLogger` created by `Initialize`. |
| **Line** | One JSON object, one line, required `EVENTID`. |
| **Catalog** | Embedded rows plus an optional host overlay at `>= 10000`. |
| **Flood** | A repeating call site is collapsed after the threshold. |
| **Seal** | Optional HMAC trailer on a closed file. |
| **Operations log** | The 5000–9999 stream. Hosts must not assign these ids. |

---

## 2. Scope

### 2.1 In scope (1a)

| ID | Capability |
|---|---|
| S-01 | `Initialize` / `Shutdown`. One host per process. |
| S-02 | Required `EVENTID`, `STATUS`, `CATEGORY`, `SUBCATEGORY`, `MESSAGE`. |
| S-03 | Catalog resolve: explicit id, else exception type, else level 0–4. |
| S-04 | Embedded catalog plus host overlay `>= 10000`. |
| S-05 | Flood protection on the write path. |
| S-06 | Owned JSONL writer. Disk level gate. |
| S-07 | Optional HMAC seal, trailer, and verify. |
| S-08 | Operations log stream when enabled. |
| S-09 | `Events` observable and channel reader for a host subscriber. |
| S-10 | `UninitializedBehavior`: throw, or no-op for library calls before `Initialize`. |

### 2.2 Out of scope (1a)

- A host UI. The demo is a sample, not the contract.
- Assigning host product event ids. Those are `>= 10000` and belong to the host catalog.
- A remote collector. The file is local.

---

## 3. Event ranges

| IDs | Owner |
|---|---|
| 0–4 | General messaging |
| 5–99 | General operations helpers |
| 100–4999 | Embedded .NET exceptions and engine |
| 5000–9999 | Vestigium.Logging operations log. Hosts must not assign these. |
| `>= 10000` | Host custom overlay |

`LoadDefault` seeds 0–14, merges exception shards, and loads the operations catalog for 5000–9999. A custom `FullName` at `>= 10000` wins over an embedded `FullName`.

---

## 4. Host contract

| Rule | Behavior |
|---|---|
| One host | A second `Initialize` replaces or rejects per the staged host rules. `Shutdown` drains writers. |
| Required fields | A line without `EVENTID` does not emit. |
| Resolve order | Explicit id, else exception `FullName` then bases, else level → 0 / 1 / 2 / 3 / 4. |
| Overlay | `LoadCustomCatalog` adds `>= 10000`. `UnloadCustomCatalog` drops it. |
| Disk gate | A line below `MinimumDiskLevel` stays off disk. |
| Seal is opt-in | No key, no trailer. |
| Libraries before init | `UninitializedBehavior = NoOp` lets a library call drop. `Throw` is the default. |

---

## 5. Non-goals

A UI. Remote shipping. Host event ids inside this package. Replacing the catalog ranges above.

---

## 6. Revision History

Initial monorepo release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 15 Nov 2026 | Nathaniel Wilkinson | Initial monorepo release. Rewritten from Vestigium.Logging SRS 2.3. |
