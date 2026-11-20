# PR02b — Test Suite

Tests land in `Tests/Logging/Vestigium.Logging.Tests`. No new test project. A step is not done until its cases exist and pass.

| Step | Proves | Done |
|---|---|---|
| PR02b.01 | Tripwire stops Mini writes at the gate and records the trip. Disabled gate does not trip. | [x] |
| PR02b.02 | Seal writes a trailer. Verify returns Valid, NoTrailer, Torn, HashMismatch, BadSignature, MissingFile, KeyMismatch as appropriate. | [x] |
| PR02b.03 | Archive proceeds on Valid or NoTrailer and refuses otherwise when sealing is enabled. | [x] |
| PR02b.04 | Head and Tail return complete lines, skip the trailer, and honor the cap. | [x] |
| PR02b.05 | Pending count, peek, and flush reflect the Mini queue. | [x] |
| PR02b.06 | Uncatalogued Thrown writes EVENTID 3 and a DEBUG hint. Recommendation appears after 20 distinct types. | [x] |
| PR02b.07 | Exception detail is capped at 8192. Properties are capped at 16 keys. | [x] |

## Not covered

- Subscribers and pump.
- Mini operations log.
- Live tail.
