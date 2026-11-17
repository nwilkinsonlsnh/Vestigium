# PR01a.08 — Dispose

**Step:** 08  
**Delivers:** Flush, trailer, end marker, no host services  
**Requirements:** R-11, R-12

## Change

Dispose flushes, writes a seal/trailer if sealing is enabled for that Mini, and writes the end marker to the host log if the host is running. Mini does not start disk-space trip, operations log, janitor, or archive.

## Technical detail

- Flush before close.
- Trailer only when sealing is enabled for that Mini.
- End marker only when the host is initialized.
- No host services are started by Mini.

## Done when

- [ ] Dispose flushes the Mini file.
- [ ] Trailer is written when sealing is enabled.
- [ ] End marker is written when the host is running.
- [ ] Mini does not start disk trip, ops log, janitor, or archive.
