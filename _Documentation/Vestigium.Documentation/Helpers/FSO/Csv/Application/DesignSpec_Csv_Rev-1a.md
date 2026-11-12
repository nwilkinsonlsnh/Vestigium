# Vestigium.Helpers.Csv — Design Specification

**Document ID:** VEST-HLP-CSV-DSN-000  
**Version:** 1a  
**Status:** Staging companion to Requirements 1a  
**Date:** 11 November 2026  
**Binding:** `Requirements_Csv-Rev1a.md` wins on conflict

Why the library is shaped this way. It does not add requirements.

---

## 1. Intent

RFC 4180 CSV / TSV for Vestigium hosts. One session owns one rectangle. ClosedXml stays .xlsx. This package is delimited text only.

---

## 2. Locked decisions

| Decision | Why |
|---|---|
| Delimited text only | Locked. |
| One session owns one rectangle | Locked. |
| RFC 4180 quoting | Locked. |
| Logging APPID stamps library identity | Folder follows the host. |
| No host adapter | Orchestration is a different project. |

---

## 3. Dependencies

| Package | Role |
|---|---|
| Vestigium.Helpers.Analytics | Primary |
| Vestigium.Logging | JSONL writer. This library never initializes it. |

---

## 4. Still out

- Excel workbooks.
- Streaming sketches of unbounded files as a product feature.

---

## 5. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
