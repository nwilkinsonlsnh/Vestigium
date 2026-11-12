# Vestigium.Helpers.ClosedXml — Requirements Specification

**Document ID:** VEST-HLP-CLOSEDXML-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0`  
**Companions:** `DesignSpec_ClosedXml_Rev-1a.md`  
**Package:** `Vestigium.Helpers.ClosedXml`  
**APPID:** `ClosedXml`  
**EventIds:** 11000–11499

Binding contract for ClosedXml as staged in the Vestigium monorepo.

---

## 0. Purpose

ClosedXML write-first Excel (.xlsx) for Vestigium hosts. One session owns one workbook. Sheets, tables, named ranges, pictures, and queued charts go through that session. It does not spawn Excel. ClosedXML types stay behind the session.

---

## 1. Surface

- Create / Open session.
- WriteTable, named ranges, pictures, queued charts.
- Save to path. ClosedXML types are not public.

---

## 2. Rules that do not move

- One session owns one workbook.
- Does not spawn Excel.
- ClosedXML types stay internal.
- Charts paints WPF; this library writes workbooks.

---

## 3. Logging

The library never calls `VestigiumLogger.Initialize` and never chooses a log folder. Hosts that want a disk trace call `Initialize` and the library catalog `Register`.

Writes are no-ops until the host initializes. JSONL lands at `%ProgramData%\Vestigium\Logs\{host-APPID}\`.

---

## 4. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0` |
| Depends on | ClosedXML 0.105.1, Vestigium.Helpers.Analytics, Vestigium.Logging |
| License | MIT |

---

## 5. Tests

Unit tests live under `Tests/Helpers/` in the matching folder for `Vestigium.Helpers.ClosedXml.Tests`.

---

## 6. Non-goals

- Spawning Excel.
- Public ClosedXML types.
- Drawing WPF charts.

---

## 7. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
