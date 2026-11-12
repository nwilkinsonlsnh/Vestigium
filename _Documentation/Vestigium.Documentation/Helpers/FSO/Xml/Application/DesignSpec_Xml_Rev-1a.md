# Vestigium.Helpers.Xml — Design Specification

**Document ID:** VEST-HLP-XML-DSN-000  
**Version:** 1a  
**Status:** Staging companion to Requirements 1a  
**Date:** 11 November 2026  
**Binding:** `Requirements_Xml-Rev1a.md` wins on conflict

Why the library is shaped this way. It does not add requirements.

---

## 1. Intent

XML codec for Vestigium hosts. Load, query, and write. Not an XSLT engine.

---

## 2. Locked decisions

| Decision | Why |
|---|---|
| One options contract | Locked. |
| Does not execute XSLT | Locked. |
| Logging APPID stamps library identity | Folder follows the host. |
| No host adapter | Orchestration is a different project. |

---

## 3. Dependencies

| Package | Role |
|---|---|
| Vestigium.Logging | Primary |
| Vestigium.Logging | JSONL writer. This library never initializes it. |

---

## 4. Still out

- XSLT.
- Schema validation as a product.

---

## 5. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
