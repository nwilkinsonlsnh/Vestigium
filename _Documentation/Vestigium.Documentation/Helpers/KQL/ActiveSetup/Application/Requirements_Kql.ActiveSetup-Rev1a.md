# Vestigium.Helpers.Kql.ActiveSetup — Requirements Specification

**Document ID:** VEST-HLP-KQL-ACTIVESETUP-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0-windows`  
**Companions:** `DesignSpec_Kql.ActiveSetup_Rev-1a.md`  
**Package:** `Vestigium.Helpers.Kql.ActiveSetup`  
**APPID:** `Kql.ActiveSetup`  
**EventIds:** reserved

Binding contract for Kql.ActiveSetup as staged in the Vestigium monorepo.

---

## 0. Purpose

KQL helpers for Active Setup inventory. Reads the Active Setup keys and projects them as rows the Kql host can filter.

---

## 1. Surface

- Enumerate Active Setup entries.
- Project to Kql rows.

---

## 2. Rules that do not move

- Windows only.
- Read-only.
- Depends on the Kql core.

---

## 3. Logging

The library never calls `VestigiumLogger.Initialize` and never chooses a log folder. Hosts that want a disk trace call `Initialize` and the library catalog `Register`.

Writes are no-ops until the host initializes. JSONL lands at `%ProgramData%\Vestigium\Logs\{host-APPID}\`.

---

## 4. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0-windows` |
| Depends on | Vestigium.Helpers.Kql, Vestigium.Logging |
| License | MIT |

---

## 5. Tests

Unit tests live under `Tests/Helpers/` in the matching folder for `Vestigium.Helpers.Kql.ActiveSetup.Tests`.

---

## 6. Non-goals

- Writing Active Setup.
- Host UI.

---

## 7. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
