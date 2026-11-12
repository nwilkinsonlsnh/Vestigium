# Vestigium.Helpers.Network.RawSocket — Design Specification

**Document ID:** VEST-HLP-NETWORK-RAWSOCKET-DSN-000  
**Version:** 1a  
**Status:** Staging companion to Requirements 1a  
**Date:** 11 November 2026  
**Binding:** `Requirements_Network.RawSocket-Rev1a.md` wins on conflict

Why the library is shaped this way. It does not add requirements.

---

## 1. Intent

Raw socket helpers for hosts that need them. Not a packet sniffer product.

---

## 2. Locked decisions

| Decision | Why |
|---|---|
| Requires elevation where the OS demands it | Locked. |
| Depends on the Network core | Locked. |
| Logging APPID stamps library identity | Folder follows the host. |
| No host adapter | Orchestration is a different project. |

---

## 3. Dependencies

| Package | Role |
|---|---|
| Vestigium.Helpers.Network | Primary |
| Vestigium.Logging | JSONL writer. This library never initializes it. |

---

## 4. Still out

- Full capture UI.
- Protocol decode suite.

---

## 5. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
