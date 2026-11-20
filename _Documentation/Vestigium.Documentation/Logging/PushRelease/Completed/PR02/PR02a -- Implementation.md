# PR02a — Implementation

Steps live in this table. Do not create a file per step. Check a row when that step is done.

| Step | Delivers | Requirements | Done |
|---|---|---|---|
| PR02a.01 | Disk tripwire. Dual gate 10 percent or 5 GB unless disabled. Mini stops writing and records the trip. | R-01 | [x] |
| PR02a.02 | Sealing opt-in. Own key ring per Mini. HMAC-SHA256 trailer. Verify results match the host. | R-02 | [x] |
| PR02a.03 | Archive seal gate. When sealing is enabled, archive proceeds only on Valid or NoTrailer. | R-03 | [x] |
| PR02a.04 | Head and Tail. Complete lines only. Skip trailer. Same cap as the host. | R-04 | [x] |
| PR02a.05 | Queue visibility. Pending count, peek, flush for the Mini queue. | R-05 | [x] |
| PR02a.06 | Uncatalogued Thrown. EVENTID 3 plus DEBUG hint. Recommend catalog after 20 distinct unknown types. | R-06 | [x] |
| PR02a.07 | Caps. Exception detail max 8192. Properties max 16 keys. | R-07 | [x] |

## Out of scope

- Subscribers, channel, and pump.
- Mini operations log.
- Live tail.
