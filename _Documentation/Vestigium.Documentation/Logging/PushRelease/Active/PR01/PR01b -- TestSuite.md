# PR01b — Test Suite

## Coverage

- Mini writes only to its own file. Host file does not contain Mini lines except start/end markers.
- Own flood tracker: one Mini flooding does not suppress another Mini or the host.
- Correlation: application correlation id and Mini correlation id appear on Mini lines and on start/end markers.
- Naming: AppId/MiniId with fewer than 4 letters rejected; pure-digit rejected; mixed (`0AB1C2D`) accepted.
- Size roll: file rolls to `.001`, `.002` when the size limit would be exceeded. No date in the name.
- Host not initialized: Mini runs and writes a warning line into its own file. No throw.
- Catalog: default uses host catalog; opt-in overlay is visible only to that Mini.
- Dispose: flushes, writes end marker when host is running, writes trailer when sealing is enabled.

## Projects

`Tests/Logging/Vestigium.Logging.Tests`. New cases alongside the existing logger tests. No new test project.

## Not covered

- Disk-space trip, janitor, and archive behavior for Mini (host concerns; not started by Mini).
- Live tail.
