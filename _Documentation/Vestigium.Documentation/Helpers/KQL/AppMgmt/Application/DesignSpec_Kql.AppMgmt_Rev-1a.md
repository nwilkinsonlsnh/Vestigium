# Vestigium.Helpers.Kql.AppMgmt — Design Specification

**Document ID:** VEST-HLP-KQL-APPMGMT-DSN-000  
**Version:** 1a  
**Status:** Staging companion to Requirements 1a  
**Date:** 11 November 2026  
**Binding:** `Requirements_Kql.AppMgmt-Rev1a.md` wins on conflict

Why the library is shaped this way. It does not add requirements.

---

## 1. Intent

KQL helpers for installed-application inventory. Reads uninstall keys and projects them as rows.

---

## 2. Locked decisions

| Decision | Why |
|---|---|
| Windows only | Locked. |
| Read-only | Locked. |
| Depends on the Kql core | Locked. |
| Logging APPID stamps library identity | Folder follows the host. |
| No host adapter | Orchestration is a different project. |

---

## 3. Dependencies

| Package | Role |
|---|---|
| Vestigium.Helpers.Kql | Primary |
| Vestigium.Logging | JSONL writer. This library never initializes it. |

---

## 4. Still out

- Uninstall.
- Host UI.

---

## 5. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
