# PR03a — Implementation

| Step | Change | Requirements | Done |
|---|---|---|---|
| PR03a.01 | Add a lock around Initialize and Shutdown. Second Initialize with same options returns. Different options throws. Second Shutdown returns. | R-01, R-02, R-03 | [ ] |
| PR03a.02 | Audit the flood tracker and recent-line buffer. Add locks or concurrent collections where shared state is mutated. | R-04, R-06 | [ ] |
| PR03a.03 | WriteHostMarker catches the shutdown race and drops the marker instead of throwing. | R-05 | [ ] |
