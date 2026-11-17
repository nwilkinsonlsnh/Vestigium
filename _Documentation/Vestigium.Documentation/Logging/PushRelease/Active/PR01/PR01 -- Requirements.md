# PR01 — Requirements

**Application:** Vestigium.Logging  
**Status:** Draft  
**Release:** PR01  
**Slices:** a implementation, b test suite, c documentation

## 1. Summary

Add `Vestigium.Logging.Mini`, an instanced logger for a scoped series of events that must not land in the application main log. Mini reuses the host catalog and taxonomy by default, owns its own flood tracker and file, and writes automatic start/end markers to the main log when the host is running. File names are size-based, not date-based, so the janitor has a stable set of parts to retain.

## 2. Motivation

The current writer names files `vestigium-{appId}-{yyyyMMdd}.json`. A new file appears on every initialize because the date is in the name. Scoped work (a PathPing, a job, a diagnostic session) has no place to go except the main log, which mixes it with host traffic. Mini gives that work its own file under the same application directory, and size-based rolling lets the janitor do its job.

## 3. Specification

| ID | Requirement |
|---|---|
| R-01 | Mini is an instanced class. It is not a static singleton. |
| R-02 | Mini writes only to its own file. Host lines never appear in a Mini file. Mini lines never appear in the host file except the automatic start and end markers. |
| R-03 | Mini has its own flood tracker. It does not share flood state with the host or with other Mini instances. |
| R-04 | Caller may pass an application correlation id. Mini also has its own correlation id (generated or accepted). Both appear on every Mini line and on the start/end markers. |
| R-05 | Layout is `%ProgramData%\Vestigium\Logging\<AppId>\`. Main file is `<AppId>.jsonl`. Mini file is `<AppId>.<MiniId>.jsonl`. Rolled parts are `<AppId>.001.jsonl` and `<AppId>.<MiniId>.001.jsonl`. |
| R-06 | AppId and MiniId must contain at least 4 letters (A–Z / a–z), case-insensitive. Digits are allowed. Pure-digit ids are rejected. The part number is always pure digits, so a scanner can distinguish rolled main files from Mini files. |
| R-07 | Rotation is by size. The writer opens the highest existing part that is under the size limit and appends. When the active file would exceed the limit, it rolls to the next part number. No date in the name. |
| R-08 | Start and end markers in the main log are automatic when the host is initialized. They include both correlation ids and the Mini file path. |
| R-09 | Mini uses the host catalog and taxonomy by default. A caller may opt in to a catalog overlay for that Mini. |
| R-10 | Mini can run when the main logger is not initialized. In that case it writes a warning line into its own file and does not throw. |
| R-11 | Dispose flushes, writes a seal/trailer if sealing is enabled for that Mini, and writes the end marker to the main log if the host is running. |
| R-12 | Disk-space trip, operations log, janitor, and archive remain host concerns. Mini does not start them by default. |

## 4. Slices

- [x] `PR01a -- Implementation.md` — the change
- [x] `PR01b -- TestSuite.md` — what proves it
- [x] `PR01c -- Documentation.md` — requirements and design updates

## 5. Out of scope

- Changing the host to a multi-instance model.
- Automatic janitor or archive on Mini dispose.
- Live tail of a Mini file.
- A separate NuGet package for Mini.

## 6. Path

Work lands in `Vestigium.Logging`. Tests land in `Tests/Logging/Vestigium.Logging.Tests`. Requirements and design specs under `_Documentation/Vestigium.Documentation/Logging/Application` gain a revision row. No package version bump until the change is accepted.

## 7. Revision History

| Revision | Date | Author | Description |
|---|---|---|---|
| 1 | 2026-11-16 | Vestigium | Initial draft from Mini brainstorm. |
