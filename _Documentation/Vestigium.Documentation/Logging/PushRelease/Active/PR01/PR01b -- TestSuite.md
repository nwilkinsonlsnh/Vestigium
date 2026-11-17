# PR01b — Test Suite

Tests land in `Tests/Logging/Vestigium.Logging.Tests`. No new test project. A step is not done until its cases exist and pass.

| Step | Proves | Done |
|---|---|---|
| 01 | AppId/MiniId with fewer than 4 letters rejected. Pure-digit rejected. Mixed (`0AB1C2D`) accepted. | [ ] |
| 02 | File rolls to `.001`, `.002` on size. No date in the name. Highest under-limit part is opened and appended. | [ ] |
| 03 | Mini writes only to its own file. Own flood tracker does not affect host or other Mini instances. | [ ] |
| 04 | Application correlation id and Mini correlation id appear on every Mini line. | [ ] |
| 05 | Start and end markers appear in the host log with both correlation ids and the Mini file path. | [ ] |
| 06 | Default uses host catalog. Opt-in overlay is visible only to that Mini. | [ ] |
| 07 | Host not initialized: Mini runs and writes a warning line into its own file. No throw. | [ ] |
| 08 | Dispose flushes, writes end marker when host is running, writes trailer when sealing is enabled. Does not start disk trip, janitor, or archive. | [ ] |

## Not covered

- Disk-space trip, janitor, and archive behavior for Mini (host concerns).
- Live tail.
