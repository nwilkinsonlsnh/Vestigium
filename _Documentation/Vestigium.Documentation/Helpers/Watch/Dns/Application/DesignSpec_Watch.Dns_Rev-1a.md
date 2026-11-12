# Vestigium.Helpers.Watch.Dns — Design Specification

**Document ID:** VEST-HLP-WATCH-DNS-DSN-000  
**Version:** 1a  
**Status:** Staging companion to Requirements 1a  
**Date:** 11 November 2026  
**Binding:** `Requirements_Watch.Dns-Rev1a.md` wins on conflict

Why the library is shaped this way. It does not add requirements.

---

## 1. Intent

DNS watch. Resolves a name on a cadence and reports changes. Does not draw.

---

## 2. Locked decisions

| Decision | Why |
|---|---|
| Windows targeting | Locked. |
| Does not own the timer in a host product | Locked. |
| Does not draw | Locked. |
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

- Full DNS server.
- Charting.

---

## 5. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
