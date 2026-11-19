# PR01a — Implementation

Steps live in this table. Do not create a file per step. Check a row when that step is done.

| Step | Delivers | Requirements | Done |
|---|---|---|---|
| PR01a.01 | AppId and MiniId rules. At least 4 letters, digits allowed, pure-digit rejected. Throws on failure. | R-06 | [x] |
| PR01a.02 | Size-based names under `%ProgramData%\Vestigium\Logging\<AppId>\`. Main `<AppId>.jsonl`, Mini `<AppId>.<MiniId>.jsonl`. Rolled `.001`, `.002`. Highest under-limit part reopened. No date. | R-05, R-07 | [x] |
| PR01a.03 | Instanced `VestigiumLoggerMini`. Own file, own flood tracker. Host lines never in Mini file. | R-01, R-02, R-03 | [ ] |
| PR01a.04 | Application correlation id (caller) and Mini correlation id (generated or accepted) on every Mini line. | R-04 | [ ] |
| PR01a.05 | Automatic start marker on construct and end marker on dispose in the host log. Both carry the two correlation ids and the Mini file path. | R-08 | [ ] |
| PR01a.06 | Host catalog and taxonomy by default. Optional overlay visible only to that Mini. | R-09 | [ ] |
| PR01a.07 | Runs when host is not initialized. Warning line in the Mini file. No throw. No markers. | R-10 | [ ] |
| PR01a.08 | Dispose flushes, writes trailer if sealing is enabled, writes end marker if host is running. | R-11 | [ ] |
| PR01a.09 | Mini janitor subscription. Own retention age and count. Does not touch the application log. Host janitor does not touch Mini files. | R-12 | [ ] |
| PR01a.10 | Multiple Mini instances may run against the same AppId. Each has its own file, flood, correlation, and janitor. | R-13 | [ ] |
| PR01a.11 | Mini archive opt-in. `MiniArchiveEnabled` + `MiniArchiveDirectory`. Copies aged Mini parts, writes SHA256 sidecar, deletes source. Does not touch the application log. | R-14 | [ ] |

## Out of scope

- Live tail of a Mini file.
- Separate package.
