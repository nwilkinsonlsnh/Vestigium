# Vestigium.Helpers.Csv — Requirements Specification

**Document ID:** VEST-HLP-CSV-SRS-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Target:** .NET 10 LTS / `net10.0`  
**Companions:** `DesignSpec_Csv_Rev-1a.md`  
**Package:** `Vestigium.Helpers.Csv`  
**APPID:** `Csv`  
**EventIds:** 11500–11999

Binding contract for Csv as staged in the Vestigium monorepo.

---

## 0. Purpose

RFC 4180 CSV / TSV for Vestigium hosts. One session owns one rectangle. ClosedXml stays .xlsx. This package is delimited text only.

---

## 1. Surface

- CsvHelper.Create.
- WriteTable(CsvTable).
- Read back a rectangle.
- Delimiter is caller-supplied.

---

## 2. Rules that do not move

- Delimited text only. Not .xlsx.
- One session owns one rectangle.
- RFC 4180 quoting.

---

## 3. Logging

The library never calls `VestigiumLogger.Initialize` and never chooses a log folder. Hosts that want a disk trace call `Initialize` and the library catalog `Register`.

Writes are no-ops until the host initializes. JSONL lands at `%ProgramData%\Vestigium\Logs\{host-APPID}\`.

---

## 4. Packaging

| Item | Value |
|---|---|
| TFM | `net10.0` |
| Depends on | Vestigium.Helpers.Analytics, Vestigium.Logging |
| License | MIT |

---

## 5. Tests

Unit tests live under `Tests/Helpers/` in the matching folder for `Vestigium.Helpers.Csv.Tests`.

---

## 6. Non-goals

- Excel workbooks.
- Streaming sketches of unbounded files as a product feature.

---

## 7. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
