# Vestigium.Helpers.PerfMon.Disk — Requirements Specification

**Document ID:** VEST-HLP-PERFMON-DISK-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0-windows`  
**Companions:** `DesignSpec_PerfMon.Disk_Rev-1a.md`  
**Package:** `Vestigium.Helpers.PerfMon.Disk`  
**APPID:** `PerfMon.Disk`

Binding contract for PerfMon.Disk as staged in the Vestigium monorepo.

---

## 0. Purpose

Disk performance-counter probe. Reads logical and physical disk counters through the PerfMon core.

---

## 1. Surface

- Sample disk counters.
- Project to the shared sample shape.

---

## 2. Rules that do not move

- Windows only.
- Depends on the PerfMon core.
- Missing category is `Unavailable`, not a fake zero.
- First rate read is the prime, not a sample.
- The library never calls `VestigiumLogger.Initialize`.

---

## 3. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0-windows` |
| Depends on | `Vestigium.Helpers.PerfMon`, `Vestigium.Logging` |
| License | MIT |

---

## 4. Non-goals

Charting. Alerting. Writing counters.

## Revision History

Initial monorepo release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial monorepo release. |
