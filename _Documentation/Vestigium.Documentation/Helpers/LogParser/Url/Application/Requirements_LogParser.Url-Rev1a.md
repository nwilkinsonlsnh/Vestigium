# Vestigium.Helpers.LogParser.Url — Requirements Specification

**Document ID:** VEST-HLP-LOGPARSER-URL-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0`  
**Companions:** `DesignSpec_LogParser.Url_Rev-1a.md`  
**Package:** `Vestigium.Helpers.LogParser.Url`  
**APPID:** `LogParser.Url`  
**EventIds:** reserved

Binding contract for LogParser.Url as staged in the Vestigium monorepo.

---

## 0. Purpose

URL and access-log parser. Projects URL fields into the LogParser row shape.

---

## 1. Surface

- Parse a URL or access-log line.
- Project fields.

---

## 2. Rules that do not move

- Depends on the LogParser core.
- Does not fetch.

---

## 3. Logging

The library never calls `VestigiumLogger.Initialize` and never chooses a log folder. Hosts that want a disk trace call `Initialize` and the library catalog `Register`.

Writes are no-ops until the host initializes. JSONL lands at `%ProgramData%\Vestigium\Logs\{host-APPID}\`.

---

## 4. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0` |
| Depends on | Vestigium.Helpers.LogParser, Vestigium.Logging |
| License | MIT |

---

## 5. Tests

Unit tests live under `Tests/Helpers/` in the matching folder for `Vestigium.Helpers.LogParser.Url.Tests`.

---

## 6. Non-goals

- HTTP client.

---

## 7. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
