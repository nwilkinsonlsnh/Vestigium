# PR03b — Test Suite

Tests land in Tests/Logging/Vestigium.Logging.Tests. A step is not done until its cases exist and pass.

| Step | Proves | Done |
|---|---|---|
| PR03b.01 | Second Initialize with the same options is a no-op. Different options throws. Second Shutdown is a no-op. | [x] |
| PR03b.02 | Parallel writes produce intact lines. No torn JSON. Flood and recent buffer stay consistent. | [x] |
| PR03b.03 | A Mini constructed while the host is shutting down does not throw. The marker is absent. | [x] |
