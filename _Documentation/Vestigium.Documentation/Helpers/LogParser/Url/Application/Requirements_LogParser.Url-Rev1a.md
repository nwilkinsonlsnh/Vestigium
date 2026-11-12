# Vestigium.Helpers.LogParser.Url — Requirements Specification

**Document ID:** VEST-HLP-URL-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0`  
**Companions:** `DesignSpec_LogParser.Url_Rev-1a.md`  
**Package:** `Vestigium.Helpers.LogParser.Url`  
**APPID:** `LogParser.Url`

Binding contract for LogParser.Url as staged in the Vestigium monorepo.

---

## 0. Purpose

URL and access-log parser. Projects URL fields into the LogParser row shape.

---

## 1. Surface

- `UrlReader.ReadFile(path)` / `Read(stream)` → `LogReadResult`.
- `UrlCatalog.Register` — host-only.

---

## 2. Rules that do not move

- Depends on the LogParser core.
- Does not fetch.
- The library never calls `VestigiumLogger.Initialize`.

---

## 3. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0` |
| Depends on | `Vestigium.Helpers.LogParser`, `Vestigium.Logging` |
| License | MIT |

---

## 4. Non-goals

HTTP client.

## Revision History

Initial monorepo release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial monorepo release. |
