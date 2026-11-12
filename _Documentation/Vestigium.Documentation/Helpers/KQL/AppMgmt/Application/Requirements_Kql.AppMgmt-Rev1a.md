# Vestigium.Helpers.Kql.AppMgmt — Requirements Specification

**Document ID:** VEST-HLP-KQLAPPMGMT-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0-windows`  
**Companions:** `DesignSpec_Kql.AppMgmt_Rev-1a.md`  
**Package:** `Vestigium.Helpers.Kql.AppMgmt`  
**APPID:** `Kql.AppMgmt`

Binding contract for Kql.AppMgmt as staged in the Vestigium monorepo.

---

## 0. Purpose

Field catalogs and sessions for AppMgmtIQ. The dialect stays in `Vestigium.Helpers.Kql`.

Two sessions. The package page binds `app.*`. The Windows Features page binds `feature.*`.

---

## 1. Surface

- `AppMgmtKql.Packages()` — package session.
- `PackageCatalog` / `PackageFacts` / `PackageAdapter.ToRow`.
- `FeatureCatalog` / `FeatureFacts` / `FeatureAdapter.ToRow`.
- `StartupCatalog` / `StartupFacts` / `StartupAdapter.ToRow`.
- `FootprintsCatalog` / `FootprintsFacts` / `FootprintsAdapter.ToRow`.
- `BloatwareCatalog` / `BloatwareFacts` / `BloatwareAdapter.ToRow`.

---

## 2. Rules that do not move

- Dialect stays in Kql. This library projects rows.
- Windows only.
- Read-only inventory.
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

Uninstall. Host UI. Writing the dialect.

## Revision History

Initial monorepo release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial monorepo release. |
