# Vestigium.Helpers.SystemInfo — Requirements Specification

**Document ID:** VEST-HLP-SYSTEMINFO-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0-windows`  
**Companions:** `DesignSpec_SystemInfo_Rev-1a.md`  
**Package:** `Vestigium.Helpers.SystemInfo`  
**APPID:** `SystemInfo`  
**EventIds:** reserved

Binding contract for SystemInfo as staged in the Vestigium monorepo.

---

## 0. Purpose

Machine facts and inventory reads. Native readers and CIM doors live here. No text, no files. A failed section is Fact<T>.Unavailable, not a throw.

---

## 1. Surface

- CpuFacts.Host / Live.
- MemoryFacts.Read.
- SystemInventory.ReadBase / ReadStorage / ReadNetwork / ReadSecurity / ReadSoftware.

---

## 2. Rules that do not move

- Windows only.
- A failed section is unavailable, not a throw.
- No text files.

---

## 3. Logging

The library never calls `VestigiumLogger.Initialize` and never chooses a log folder. Hosts that want a disk trace call `Initialize` and the library catalog `Register`.

Writes are no-ops until the host initializes. JSONL lands at `%ProgramData%\Vestigium\Logs\{host-APPID}\`.

---

## 4. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0-windows` |
| Depends on | System.Management, System.ServiceProcess.ServiceController |
| License | MIT |

---

## 5. Tests

Unit tests live under `Tests/Helpers/` in the matching folder for `Vestigium.Helpers.SystemInfo.Tests`.

---

## 6. Non-goals

- Remote WMI as a product.
- Inventory database.

---

## 7. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
