# Vestigium.Helpers.Json — Design Specification

**Document ID:** VEST-HLP-JSON-DSN-000  
**Version:** 1a  
**Status:** Staging companion to Requirements 1a  
**Date:** 11 November 2026  
**Binding:** `Requirements_Json-Rev1a.md` wins on conflict

Why the library is shaped this way. It does not add requirements.

---

## 1. Intent

JSON codec for Vestigium hosts. Serialize and deserialize with a single options contract. Not a document database.

---

## 2. Locked decisions

| Decision | Why |
|---|---|
| One options contract | Locked. |
| Does not persist | Locked. |
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

- Document store.
- Schema registry.

---

## 5. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
