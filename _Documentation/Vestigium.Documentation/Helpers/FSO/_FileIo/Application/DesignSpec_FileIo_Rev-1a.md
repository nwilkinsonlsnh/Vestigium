# Vestigium.Helpers.FileIo — Design Specification

**Document ID:** VEST-HLP-FILEIO-DSN-000  
**Version:** 1a  
**Status:** Staging companion to Requirements 1a  
**Date:** 11 November 2026  
**Binding:** `Requirements_FileIo-Rev1a.md` wins on conflict

Why the library is shaped this way. It does not add requirements.

---

## 1. Intent

Validated file jobs. Robocopy is the behavior reference; FileIo is the record. This package does not spawn robocopy.exe. Recon fills five size buckets. Default collision is UniqueName (.##). Cap is NameCap — the original dest is never overwritten unless the caller picks Overwrite.

---

## 2. Locked decisions

| Decision | Why |
|---|---|
| Does not spawn robocopy | Locked. |
| Default collision is UniqueName | Locked. |
| Original dest is never overwritten unless Overwrite is chosen | Locked. |
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

- Spawning robocopy.exe.
- Silent overwrite.

---

## 5. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
