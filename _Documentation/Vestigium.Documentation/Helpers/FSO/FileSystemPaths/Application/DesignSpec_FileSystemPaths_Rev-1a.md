# Vestigium.Helpers.FileSystemPaths — Design Specification

**Document ID:** VEST-HLP-FILESYSTEMPATHS-DSN-000  
**Version:** 1a  
**Status:** Staging companion to Requirements 1a  
**Date:** 11 November 2026  
**Binding:** `Requirements_FileSystemPaths-Rev1a.md` wins on conflict

Why the library is shaped this way. It does not add requirements.

---

## 1. Intent

Path identity and normalization for Vestigium hosts. Resolves, compares, and classifies filesystem paths without touching file contents.

---

## 2. Locked decisions

| Decision | Why |
|---|---|
| Does not read file contents | Locked. |
| Path identity is the contract | Locked. |
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

- File IO.
- Content hashing.

---

## 5. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
