# Vestigium.Helpers.PerfMon.Cpu — Requirements Specification

**Document ID:** VEST-HLP-PERFMON-CPU-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0-windows`  
**Companions:** `DesignSpec_PerfMon.Cpu_Rev-1a.md`  
**Package:** `Vestigium.Helpers.PerfMon.Cpu`  
**APPID:** `PerfMon.Cpu`

Binding contract for PerfMon.Cpu as staged in the Vestigium monorepo.

---

## 0. Purpose

CPU performance-counter probe. Reads processor counters through the PerfMon core.

---

## 1. Surface

- Sample cpu counters.
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
