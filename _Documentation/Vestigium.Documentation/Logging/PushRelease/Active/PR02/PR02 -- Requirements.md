# PR02 — Requirements

**Application:** Vestigium.Logging
**Status:** Draft
**Release:** PR02
**Slices:** a implementation, b test suite, c documentation
**Depends on:** PR01

## 1. Summary

Bring Mini to parity with the host on the features that protect and inspect a log. Disk tripwire, sealing, Head/Tail, queue visibility, the uncatalogued-Thrown hint, and the exception/properties caps. Subscribers and a Mini operations log are not in this release.

## 2. Motivation

PR01 gives Mini its own file, flood, janitor, and archive. Without the tripwire a Mini can fill the disk. Without sealing there is no integrity trailer. Without Head/Tail and queue visibility there is no way to inspect a Mini the way a host log is inspected. These are host features today. Mini should match.

## 3. Specification

| ID | Requirement |
|---|---|
| R-01 | Mini disk tripwire matches the host. Dual gate: 10 percent or 5 GB unless disabled. When tripped, Mini stops writing and records the trip. |
| R-02 | Mini sealing is opt-in. Own key ring per Mini. HMAC-SHA256 trailer on the last line. Verify results match the host: Valid, NoTrailer, Torn, HashMismatch, BadSignature, MissingFile, KeyMismatch. |
| R-03 | Mini archive requires Valid or NoTrailer when sealing is enabled, matching the host gate. |
| R-04 | Mini Head and Tail read complete persisted lines only. Skip the seal trailer. Same cap as the host. |
| R-05 | Mini exposes pending count, peek, and flush for its own queue. |
| R-06 | Uncatalogued Thrown writes EVENTID 3 plus a DEBUG hint. After 20 distinct unknown types, recommend a custom catalog. |
| R-07 | Exception detail and properties caps match the host. Exception detail max 8192. Properties max 16 keys. |

## 4. Slices

- [x] `PR02a -- Implementation.md`
- [x] `PR02b -- TestSuite.md`
- [x] `PR02c -- Documentation.md`

## 5. Path

Steps live in the tables inside `PR02a`, `PR02b`, and `PR02c`. Do not create a file per step.

| Step | Delivers | Requirements |
|---|---|---|
| PR02a.01 | Disk tripwire | R-01 |
| PR02a.02 | Sealing and verify | R-02 |
| PR02a.03 | Archive seal gate | R-03 |
| PR02a.04 | Head and Tail | R-04 |
| PR02a.05 | Queue visibility | R-05 |
| PR02a.06 | Uncatalogued Thrown hint | R-06 |
| PR02a.07 | Exception and properties caps | R-07 |

## 6. Out of scope

- Subscribers, channel, and pump for Mini.
- A Mini operations log.
- Live tail.
- Changing the host tripwire, seal, or reader.

## 7. Revision History

| Revision | Date | Author | Description |
|---|---|---|---|
| 1 | 2026-11-16 | Vestigium | Initial draft. Symmetry gaps from the host. |
