# PR03 — Requirements

**Application:** Vestigium.Logging  
**Status:** Draft  
**Release:** PR03  
**Slices:** a implementation, b test suite, c documentation

## 1. Summary

Harden the host logger against concurrent use. Initialize and Shutdown become idempotent and locked. Concurrent writes stay safe. A Mini marker dropped during host shutdown does not throw.

## 2. Motivation

The host is a singleton. Tests that call Initialize and Shutdown in parallel race on that one instance. Production has one process, but two threads can still call Initialize twice, or a Mini can emit a marker while the host is shutting down. The library should make those edges safe instead of relying on callers to serialize.

## 3. Specification

| ID | Requirement |
|---|---|
| R-01 | Initialize is idempotent. A second call with the same options is a no-op. A second call with different options throws. |
| R-02 | Shutdown is idempotent. A second call is a no-op. |
| R-03 | Initialize and Shutdown are mutually exclusive. A lock guards the transition. |
| R-04 | Concurrent VestigiumLog writes from multiple threads do not corrupt a line or lose a line that passed the flood gate. |
| R-05 | A Mini marker emitted while the host is shutting down is dropped. It does not throw. |
| R-06 | The flood tracker and recent-line buffer are safe for concurrent observe and enqueue. |

## 4. Slices

- [ ] PR03a -- Implementation.md — the change
- [ ] PR03b -- TestSuite.md — what proves it
- [ ] PR03c -- Documentation.md — requirements and design updates

## 5. Path

| Step | Delivers | Requirements |
|---|---|---|
| PR03a.01 | Lock around Initialize and Shutdown | R-01, R-02, R-03 |
| PR03a.02 | Concurrent write safety audit and fix | R-04, R-06 |
| PR03a.03 | Mini marker drop during shutdown | R-05 |

## 6. Out of scope

- Replacing the singleton with an instanced host.
- Test collection pinning. That stays. It is a test concern.

## 7. Revision History

| Revision | Date | Author | Description |
|---|---|---|---|
| 1 | 2026-11-19 | Vestigium | Initial draft from concurrency discussion. |
