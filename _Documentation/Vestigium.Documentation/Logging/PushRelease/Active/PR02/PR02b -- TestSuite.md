# PR02b — Test Suite

Tests land in `Tests/Logging/Vestigium.Logging.Tests`. No new test project. A step is not done until its cases exist and pass.

| Step | Proves | Done |
|---|---|---|
| PR02a.01 | Tripwire stops Mini writes at the gate and records the trip. Disabled gate does not trip. | [ ] |
| PR02a.02 | Seal writes a trailer. Verify returns Valid, NoTrailer, Torn, HashMismatch, BadSignature, MissingFile, KeyMismatch as appropriate. | [ ] |
| PR02a.03 | Archive proceeds on Valid or NoTrailer and refuses otherwise when sealing is enabled. | [ ] |
| PR02a.04 | Head and Tail return complete lines, skip the trailer, and honor the cap. | [ ] |
| PR02a.05 | Pending count, peek, and flush reflect the Mini queue. | [ ] |
| PR02a.06 | Uncatalogued Thrown writes EVENTID 3 and a DEBUG hint. Recommendation appears after 20 distinct types. | [ ] |
| PR02a.07 | Exception detail is capped at 8192. Properties are capped at 16 keys. | [ ] |

## Not covered

- Subscribers and pump.
- Mini operations log.
- Live tail.
