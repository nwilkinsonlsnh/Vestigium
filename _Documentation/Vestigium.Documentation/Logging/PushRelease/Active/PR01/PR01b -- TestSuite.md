# PR01b — Test Suite

Tests land in `Tests/Logging/Vestigium.Logging.Tests`. No new test project. A step is not done until its cases exist and pass.

| Step | Proves | Done |
|---|---|---|
| PR01b.01 | Fewer than 4 letters rejected. Pure-digit rejected. Mixed (`0AB1C2D`) accepted. | [x] |
| PR01b.02 | Rolls to `.001`, `.002` on size. No date in the name. Highest under-limit part reopened. | [x] |
| PR01b.03 | Mini writes only to its own file. Own flood does not affect host or other Mini instances. | [x] |
| PR01b.04 | Application correlation id and Mini correlation id appear on every Mini line. | [x] |
| PR01b.05 | Start and end markers appear in the host log with both correlation ids and the Mini file path. | [x] |
| PR01b.06 | Default uses host catalog. Opt-in overlay is visible only to that Mini. | [x] |
| PR01b.07 | Host not initialized: Mini runs and writes a warning line into its own file. No throw. | [x] |
| PR01b.08 | Dispose flushes, writes end marker when host is running, writes trailer when sealing is enabled. | [ ] |
| PR01b.09 | Mini janitor deletes only its own aged parts. Application log is untouched. Host janitor does not delete Mini files. | [ ] |
| PR01b.10 | Two Mini instances against one AppId do not share file, flood, correlation, or janitor state. | [ ] |
| PR01b.11 | Mini archive copies only its own aged parts, writes a SHA256 sidecar, deletes the source, and leaves the application log untouched. Disabled by default. | [ ] |

## Not covered

- Live tail.
