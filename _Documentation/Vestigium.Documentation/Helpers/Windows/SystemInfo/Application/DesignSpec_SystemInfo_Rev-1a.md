# Vestigium.Helpers.SystemInfo — Design Specification

**Document ID:** VEST-HLP-SYSTEMINFO-DSN-000  
**Version:** 1a  
**Status:** Staging companion to Requirements 1a  
**Date:** 11 November 2026  
**Binding:** `Requirements_SystemInfo-Rev1a.md` wins on conflict

Why the library is shaped this way. It does not add requirements.

---

## 1. Intent

Machine facts and inventory reads. Native readers and CIM doors live here. No text, no files. A failed section is Fact<T>.Unavailable, not a throw.

---

## 2. Locked decisions

| Decision | Why |
|---|---|
| Windows only | Locked. |
| A failed section is unavailable, not a throw | Locked. |
| No text files | Locked. |
| Logging APPID stamps library identity | Folder follows the host. |
| No host adapter | Orchestration is a different project. |

---

## 3. Dependencies

| Package | Role |
|---|---|
| System.Management | Primary |
| Vestigium.Logging | JSONL writer. This library never initializes it. |

---

## 4. Still out

- Remote WMI as a product.
- Inventory database.

---

## 5. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
