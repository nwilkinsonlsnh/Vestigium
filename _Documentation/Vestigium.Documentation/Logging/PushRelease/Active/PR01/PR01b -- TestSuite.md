# PR01b — Test Suite

Tests land in `Tests/Logging/Vestigium.Logging.Tests`. No new test project. A step is not done until its cases exist and pass.

| Step | Proves | Done |
|---|---|---|
| PR01a.01 | Fewer than 4 letters rejected. Pure-digit rejected. Mixed (`0AB1C2D`) accepted. | [ ] |
| PR01a.02 | Rolls to `.001`, `.002` on size. No date in the name. Highest under-limit part reopened. | [ ] |
| PR01a.03 | Mini writes only to its own file. Own flood does not affect host or other Mini instances. | [ ] |
| PR01a.04 | Application correlation id and Mini correlation id appear on every Mini line. | [ ] |
| PR01a.05 | Start and end markers appear in the host log with both correlation ids and the Mini file path. | [ ] |
| PR01a.06 | Default uses host catalog. Opt-in overlay is visible only to that Mini. | [ ] |
| PR01a.07 | Host not initialized: Mini runs and writes a warning line into its own file. No throw. | [ ] |
| PR01a.08 | Dispose flushes, writes end marker when host is running, writes trailer when sealing is enabled. | [ ] |
| PR01a.09 | Mini janitor deletes only its own aged parts. Application log is untouched. Host janitor does not delete Mini files. | [ ] |
| PR01a.10 | Two Mini instances against one AppId do not share file, flood, correlation, or janitor state. | [ ] |
| PR01a.11 | Mini archive copies only its own aged parts, writes a SHA256 sidecar, deletes the source, and leaves the application log untouched. Disabled by default. | [ ] |

## Not covered

- Live tail.
