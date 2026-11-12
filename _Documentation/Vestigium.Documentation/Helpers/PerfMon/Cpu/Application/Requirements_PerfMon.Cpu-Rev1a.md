# Vestigium.Helpers.PerfMon.Cpu — Requirements Specification

**Document ID:** VEST-HLP-PERFMON-CPU-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0-windows`  
**Companions:** `DesignSpec_PerfMon.Cpu_Rev-1a.md`  
**Package:** `Vestigium.Helpers.PerfMon.Cpu`  
**APPID:** `PerfMon.Cpu`  
**EventIds:** reserved

Binding contract for PerfMon.Cpu as staged in the Vestigium monorepo.

---

## 0. Purpose

CPU performance-counter probe. Reads processor counters through the PerfMon core.

---

## 1. Surface

- Sample CPU counters.
- Project to the shared sample shape.

---

## 2. Rules that do not move

- Windows only.
- Depends on the PerfMon core.

---

## 3. Logging

The library never calls `VestigiumLogger.Initialize` and never chooses a log folder. Hosts that want a disk trace call `Initialize` and the library catalog `Register`.

Writes are no-ops until the host initializes. JSONL lands at `%ProgramData%\Vestigium\Logs\{host-APPID}\`.

---

## 4. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0-windows` |
| Depends on | Vestigium.Helpers.PerfMon, Vestigium.Logging |
| License | MIT |

---

## 5. Tests

Unit tests live under `Tests/Helpers/` in the matching folder for `Vestigium.Helpers.PerfMon.Cpu.Tests`.

---

## 6. Non-goals

- Scheduling.
- Affinity changes.

---

## 7. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
