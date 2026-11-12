# Vestigium.Helpers.LogParser — Requirements Specification

**Document ID:** VEST-HLP-LOGPARSER-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0`  
**Companions:** `DesignSpec_LogParser_Rev-1a.md`  
**Package:** `Vestigium.Helpers.LogParser`  
**APPID:** `LogParser`

Binding contract for LogParser as staged in the Vestigium monorepo.

---

## 0. Purpose

Log line parser core. Sibling parsers (Har, Url) project specific formats into the same row shape.

---

## 1. Surface

- `LogFormat` — format enum.
- `LogHost` / `LogHostSource` — host identity.
- `LogReadResult` — the read outcome.
- `LogParserCatalog.Register` — host-only.

---

## 2. Rules that do not move

- One row shape. Format-specific parsers are siblings.
- The library never calls `VestigiumLogger.Initialize`.

---

## 3. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0` |
| Depends on | `Vestigium.Logging` |
| License | MIT |

---

## 4. Non-goals

Log shipping. Retention. Fetch.

## Revision History

Initial monorepo release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial monorepo release. |
