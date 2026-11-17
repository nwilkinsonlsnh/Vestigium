# PR01a — Implementation

Steps live in this table. Do not create a file per step. Check a row when that step is done.

| Step | Delivers | Requirements | Done |
|---|---|---|---|
| PR01a.01 | AppId and MiniId rules. At least 4 letters, digits allowed, pure-digit rejected. Throws on failure. | R-06 | [ ] |
| PR01a.02 | Size-based names under `%ProgramData%\Vestigium\Logging\<AppId>\`. Main `<AppId>.jsonl`, Mini `<AppId>.<MiniId>.jsonl`. Rolled `.001`, `.002`. Highest under-limit part reopened. No date. | R-05, R-07 | [ ] |
| PR01a.03 | Instanced `VestigiumLoggerMini`. Own file, own flood tracker. Host lines never in Mini file. | R-01, R-02, R-03 | [ ] |
| PR01a.04 | Application correlation id (caller) and Mini correlation id (generated or accepted) on every Mini line. | R-04 | [ ] |
| PR01a.05 | Automatic start marker on construct and end marker on dispose in the host log. Both carry the two correlation ids and the Mini file path. | R-08 | [ ] |
| PR01a.06 | Host catalog and taxonomy by default. Optional overlay visible only to that Mini. | R-09 | [ ] |
| PR01a.07 | Runs when host is not initialized. Warning line in the Mini file. No throw. No markers. | R-10 | [ ] |
| PR01a.08 | Dispose flushes, writes trailer if sealing is enabled, writes end marker if host is running. Does not start disk trip, ops log, janitor, or archive. | R-11, R-12 | [ ] |

## Out of scope

- Host multi-instance.
- Automatic janitor or archive on Mini dispose.
- Separate package.
