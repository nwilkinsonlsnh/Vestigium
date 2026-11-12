# Vestigium.Helpers.PerfMon.Gpu — Design Specification

**Document ID:** VEST-HLP-PERFMON-GPU-DSN-000  
**Version:** 1a  
**Status:** Staging companion to Requirements 1a  
**Date:** 11 November 2026  
**Binding:** `Requirements_PerfMon.Gpu-Rev1a.md` wins on conflict

Why the library is shaped this way. It does not add requirements.

---

## 1. Intent

GPU performance-counter probe. Reads GPU engine counters through the PerfMon core.

---

## 2. Locked decisions

| Decision | Why |
|---|---|
| Windows only | Locked. |
| Depends on the PerfMon core | Locked. |
| Absent GPU is a miss, not a throw | Locked. |
| Logging APPID stamps library identity | Folder follows the host. |
| No host adapter | Orchestration is a different project. |

---

## 3. Dependencies

| Package | Role |
|---|---|
| Vestigium.Helpers.PerfMon | Primary |
| Vestigium.Logging | JSONL writer. This library never initializes it. |

---

## 4. Still out

- Rendering.
- Driver install.

---

## 5. Revision History

Initial release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial release. |
