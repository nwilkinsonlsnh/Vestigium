# Vestigium.Helpers.PerfMon — Requirements Specification

**Document ID:** VEST-HLP-PERFMON-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0-windows`  
**Companions:** `DesignSpec_PerfMon_Rev-1a.md`  
**Package:** `Vestigium.Helpers.PerfMon`  
**APPID:** `PerfMon`  
**EventIds:** reserved

Binding contract for PerfMon as staged in the Vestigium monorepo.

---

## 0. Purpose

Shared performance-counter core. SampleJob and CounterSet live here. Probe libraries (Cpu, Disk, Gpu, Memory, Network, PageFile) sit on top.

---

## 1. Surface

- SampleJob.
- CounterSet.
- Typed catalogs.

---

## 2. Rules that do not move

- Windows only.
- Probes are siblings.
- Does not draw.

---

## 3. Logging

The library never calls `VestigiumLogger.Initialize` and never chooses a log folder. Hosts that want a disk trace call `Initialize` and the library catalog `Register`.

Writes are no-ops until the host initializes. JSONL lands at `%ProgramData%\Vestigium\Logs\{host-APPID}\`.

---

## 4. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0-windows` |
| Depends on | Vestigium.Logging |
| License | MIT |

---

## 5. Tests

Unit tests live under `Tests/Helpers/` in the matching folder for `Vestigium.Helpers.PerfMon.Tests`.

---

## 6. Non-goals

- Charting.
- Alerting product.

---

## 7. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
