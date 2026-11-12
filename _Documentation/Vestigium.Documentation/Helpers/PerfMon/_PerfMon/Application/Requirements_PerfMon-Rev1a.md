# Vestigium.Helpers.PerfMon — Requirements Specification

**Document ID:** VEST-HLP-PERFMON-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0-windows`  
**Companions:** `DesignSpec_PerfMon_Rev-1a.md`  
**Package:** `Vestigium.Helpers.PerfMon` 0.1.3  
**APPID:** `PerfMon`  
**EventIds:** 17000–17499

Binding contract for PerfMon as staged in the Vestigium monorepo.

---

## 0. Purpose

Shared performance sample contract, job runner, and counter source. Not a CLI. Not `perfmon.exe`. Not a plot package.

Probe libraries (Cpu, Disk, Gpu, Memory, Network, PageFile) sit on top.

---

## 1. Surface

- `SampleJob` — runs a sample against a source.
- `CounterSet` — typed catalog of counters.
- `CachedPdhSource` — the one public source. Hosts do not subclass it.
- `ListInstances` uses `PdhCounterInventory.Shared`.

---

## 2. Rules that do not move

- The library never calls `VestigiumLogger.Initialize`.
- Missing counter categories are `Unavailable`, not a fake zero.
- The first read of a rate counter is Unavailable. It is the prime, not a sample.
- No Demo project.
- `SampleJob` with no `Source` constructs `CachedPdhSource` and disposes it. A host-passed source is not disposed.
- `PerformanceCounterSource` stays internal.
- `ListInstances` does not walk PDH a second time.

---

## 3. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0-windows` |
| Depends on | `Vestigium.Logging` |
| License | MIT |

---

## 4. Non-goals

Charting. Alerting product. `perfmon.exe`.

## Revision History

Initial monorepo release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial monorepo release. |
