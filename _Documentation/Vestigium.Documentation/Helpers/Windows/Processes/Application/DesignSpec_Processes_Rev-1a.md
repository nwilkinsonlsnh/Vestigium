# Vestigium.Helpers.Processes — Design

**Document ID:** VEST-HLP-PROCESSES-DSN-000  
**Version:** 1a  
**Status:** Staging — monorepo release  
**Date:** 11 November 2026  
**Binding:** `Requirements_Processes-Rev1a.md` wins on conflict  

This page records *why* Processes is shaped this way. It does not add requirements.

---

## 1. Intent

One Windows façade over the process table. Snapshotter fills `ProcessInfo` plus `FieldAvailability`. Kql lives in a sibling; this package binds rows.

```
List / Get / Search(term)
Search(query) → KqlHelper.Compile(KqlPack.Process) → ProcessKqlRow
Watch / Campaign sample on an interval
Kill / Start go through dedicated types that apply the denylist
```

---

## 2. Locked decisions

| Decision | Why |
|---|---|
| Snapshotter + availability | Denied / unsupported fields must not look like empty values. |
| Two Search overloads | Term search is cheap. Kql search is a compiled predicate. |
| Slim vs Full | Kql upgrades Slim → Full when the AST names a Full field. |
| Watchers are pull | Interval sample. No ETW subscription in v1. |
| Campaigns in-process | No scheduler package. JSONL while a window is open. |
| Kill fail-closed | Confirm is not a privilege escalation. |
| Comment keys hashed | JSON must not store raw image paths as keys. |
| Never `Initialize` | Folder follows the host APPID. |

---

## 3. Shape

| File | Role |
|---|---|
| `ProcessHelper.cs` | List / Get / Search(term) / tree / threads / watch / start / kill |
| `ProcessHelper.Kql.cs` | Search(query), SearchThreads, MatchSystem, Watch(query) |
| `ProcessHelper.Campaigns.cs` | Create / Open campaign |
| `ProcessSnapshotter` / readers | Fill `ProcessInfo` |
| `ProcessKiller` / `ProcessStarter` | Lifetime |
| `ProcessCampaign` | Windows + JSONL |
| `ProcessKqlRow` | `IKqlRow` adapter |
| `ProcessesCatalog` / `ProcessesEvents` | Logging |

Tests live under `src/Vestigium.Helpers.Tests/`.

---

## 4. Exception policy

| Class | When |
|---|---|
| `ArgumentOutOfRangeException` | Bad PID, interval, maxResults, MaxMatches. |
| `ArgumentException` | Kql compile failed. |
| `InvalidOperationException` | GetTree / SetComment on a gone PID. |

`Get` / `TryGet` do not throw on a missing PID.

---

## 5. Still out

Linux, `schtasks`, ETW watchers, Autoruns-complete, dumping protected processes.

---

## 6. Document control

| Version | Date | Change |
|---|---|---|
| 1.3 | 15 Sep 2026 | As-built stub. |
| 1.3 | 21 Sep 2026 | Expanded to standalone Design. |

---

## Revision History

Initial monorepo release. Add a row when this document changes. Do not bump the revision until the change is accepted.

| Revision | Date | Author | Description |
|---|---|---|---|
| 1a | 11 Nov 2026 | Nathaniel Wilkinson | Initial monorepo release. Carried forward from the prior library contract. |
