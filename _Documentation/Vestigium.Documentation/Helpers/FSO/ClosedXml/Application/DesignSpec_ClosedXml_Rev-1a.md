# Vestigium.Helpers.ClosedXml — Design Specification

**Document ID:** VEST-HLP-CLOSEDXML-DSN-000  
**Version:** 1a  
**Status:** Staging companion to Requirements 1a  
**Date:** 11 November 2026  
**Binding:** `Requirements_ClosedXml-Rev1a.md` wins on conflict

Why the library is shaped this way. It does not add requirements.

---

## 1. Intent

ClosedXML write-first Excel (.xlsx) for Vestigium hosts. One session owns one workbook. Sheets, tables, named ranges, pictures, and queued charts go through that session. It does not spawn Excel. ClosedXML types stay behind the session.

---

## 2. Locked decisions

| Decision | Why |
|---|---|
| One session owns one workbook | Locked. |
| Does not spawn Excel | Locked. |
| ClosedXML types stay internal | Locked. |
| Charts paints WPF; this library writes workbooks | Locked. |
| Logging APPID stamps library identity | Folder follows the host. |
| No host adapter | Orchestration is a different project. |

---

## 3. Dependencies

| Package | Role |
|---|---|
| ClosedXML 0.105.1 | Primary |
| Vestigium.Logging | JSONL writer. This library never initializes it. |

---

## 4. Still out

- Spawning Excel.
- Public ClosedXML types.
- Drawing WPF charts.

---

## 5. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
