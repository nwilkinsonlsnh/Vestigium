# Vestigium.Helpers.LogParser — Requirements Specification

**Document ID:** VEST-HLP-LOGPARSER-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0`  
**Companions:** `DesignSpec_LogParser_Rev-1a.md`  
**Package:** `Vestigium.Helpers.LogParser`  
**APPID:** `LogParser`  
**EventIds:** reserved

Binding contract for LogParser as staged in the Vestigium monorepo.

---

## 0. Purpose

Log line parser core. Sibling parsers (Har, Url) project specific formats into the same row shape.

---

## 1. Surface

- Parse a line.
- Project fields.
- Reject a malformed line without aborting the batch when asked.

---

## 2. Rules that do not move

- One row shape.
- Format-specific parsers are siblings.

---

## 3. Logging

The library never calls `VestigiumLogger.Initialize` and never chooses a log folder. Hosts that want a disk trace call `Initialize` and the library catalog `Register`.

Writes are no-ops until the host initializes. JSONL lands at `%ProgramData%\Vestigium\Logs\{host-APPID}\`.

---

## 4. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0` |
| Depends on | Vestigium.Logging |
| License | MIT |

---

## 5. Tests

Unit tests live under `Tests/Helpers/` in the matching folder for `Vestigium.Helpers.LogParser.Tests`.

---

## 6. Non-goals

- Log shipping.
- Retention.

---

## 7. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
