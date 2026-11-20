# PR04 — Requirements

**Application:** Vestigium.Logging  
**Status:** Draft  
**Release:** PR04  
**Slices:** a implementation, b test suite, c documentation

## 1. Summary

Review of the logging test suite found no production logic living in the tests. The library owns the behavior. This PR tracks the thin spots: duplicated test helpers, and test seams that live in the library only so tests can force an edge.

## 2. Findings

| Item | Where | Verdict |
|---|---|---|
| `SharedRead.Text` | Tests | Stays in tests. Reads a file the writer still holds. Not library behavior. |
| `ReadShared` | `JsonlWriterTests`, `LoggerBehaviorTests` | Duplicate of `SharedRead`. Consolidate in the test project. |
| `Touch` | `MiniJanitorTests`, `MiniArchiveTests`, `MiniMultiInstanceTests` | Duplicate. Ages a file for janitor and archive tests. Consolidate in the test project. |
| SHA256 in tests | Seal and archive tests | Stays. Tests recompute a hash to check the library. The library owns the real hash. |
| `TripwireOverride` | `VestigiumLoggerMini` | Test seam in the library. Internal. Keep, but document. |
| `HashOverride` | `VestigiumLogArchive` | Test seam. Internal. Keep, but document. |
| `ReadyOverride` | `DiskSpaceMonitor` | Test seam. Internal. Keep, but document. |
| `OverrideDiskPressure` | `VestigiumLogger` | Public. Used by tests and available to a host. Keep. |
| Direct internal calls | `VestigiumPropertyBag`, `UncataloguedExceptionAdvisor` | Tests reach internal types. Acceptable for a unit suite. No logic to move. |

## 3. Specification

| ID | Requirement |
|---|---|
| R-01 | Consolidate `ReadShared` and `Touch` into shared test helpers. Do not move them to the library. |
| R-02 | Document the three internal test seams and why they exist. |
| R-03 | No production logic moves from tests to the library. The review found none. |

## 4. Slices

- [ ] PR04a -- Implementation.md
- [ ] PR04b -- TestSuite.md
- [ ] PR04c -- Documentation.md

## 5. Path

| Step | Delivers | Requirements |
|---|---|---|
| PR04a.01 | Shared test helpers for file read and aging | R-01 |
| PR04a.02 | Comments on the three internal seams | R-02 |

## 6. Out of scope

- Moving test helpers into the library.
- Removing the seams. They are how the suite forces disk and hash edges.
- Changing public API.

## 7. Revision History

| Revision | Date | Author | Description |
|---|---|---|---|
| 1 | 2026-11-19 | Vestigium | Review of the logging test suite. No library logic found in tests. |
