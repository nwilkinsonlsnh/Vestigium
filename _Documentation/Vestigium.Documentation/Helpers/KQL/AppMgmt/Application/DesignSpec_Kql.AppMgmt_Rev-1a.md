# Vestigium.Helpers.Kql.AppMgmt — Design Specification

**Document ID:** VEST-HLP-KQL-APPMGMT-DSN-000  
**Version:** 1a  
**Status:** Staging companion to Requirements 1a  
**Date:** 11 November 2026  
**Binding:** `Requirements_Kql.AppMgmt-Rev1a.md` wins on conflict

Why the library is shaped this way. It does not add requirements.

---

## 1. Intent

See Requirements 1a. This document records locked decisions only.

---

## 2. Locked decisions

| Decision | Why |
|---|---|
| Library identity APPID | Folder follows the host. |
| No host adapter | Orchestration is a different project. |
| Fail closed | Corrupt or denied input is a result or a throw, never a silent success. |

---

## 3. Still out

Host adapters. Drawing. Persistence beyond what Requirements names.


## Revision History

Initial monorepo release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial monorepo release. |

