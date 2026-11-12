# Vestigium.Helpers.Kql.ActiveSetup — Requirements Specification

**Document ID:** VEST-HLP-KQLSETUP-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0-windows`  
**Companions:** `DesignSpec_Kql.ActiveSetup_Rev-1a.md`  
**Package:** `Vestigium.Helpers.Kql.ActiveSetup`  
**APPID:** `Kql.ActiveSetup`

Binding contract for Kql.ActiveSetup as staged in the Vestigium monorepo.

---

## 0. Purpose

Field catalog and session for Active Setup. The dialect stays in `Vestigium.Helpers.Kql`.

One session. The Active Setup page binds `setup.*`.

---

## 1. Surface

- `ActiveSetupKql.Components()` — session.
- `ActiveSetupCatalog` / `ActiveSetupFacts` / `ActiveSetupAdapter.ToRow`.

---

## 2. Rules that do not move

- Dialect stays in Kql.
- Windows only.
- Read-only.
- The library never calls `VestigiumLogger.Initialize`.

---

## 3. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0-windows` |
| Depends on | `Vestigium.Helpers.Kql`, `Vestigium.Logging` |
| License | MIT |

---

## 4. Non-goals

Writing Active Setup. Host UI.

## Revision History

Initial monorepo release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial monorepo release. |
