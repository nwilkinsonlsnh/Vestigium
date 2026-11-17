# PR01a — Implementation

## Change

Add an instanced Mini logger that reuses the host catalog by default, owns its own flood tracker and size-based file, and writes automatic start/end markers to the main log when the host is running.

## Technical detail

- New type `VestigiumLoggerMini` (name final in implementation). Constructed with AppId, MiniId, optional application correlation id, optional Mini correlation id, and optional catalog overlay.
- AppId and MiniId validated at construction: at least 4 letters (A–Z / a–z), digits allowed, pure-digit rejected.
- File path resolved under `%ProgramData%\Vestigium\Logging\<AppId>\`. Active Mini file is `<AppId>.<MiniId>.jsonl`. Rolled parts are `<AppId>.<MiniId>.001.jsonl` and so on.
- Writer opens the highest existing part under the size limit and appends. Rolls to the next part when the next line would exceed the limit. No date stamp in the name.
- Own `FloodTracker`. Does not share state with the host or other Mini instances.
- Start marker written to the host log on construct when `VestigiumLogger.IsInitialized`. End marker written on dispose. Both carry the application correlation id, the Mini correlation id, and the Mini file path.
- If the host is not initialized, Mini still runs and writes a warning line into its own file. No exception.
- Catalog: host catalog by default. Optional overlay merged for that instance only.
- Dispose flushes and, if sealing is enabled for that Mini, writes the trailer. Does not start disk trip, ops log, janitor, or archive.

## Out of scope

- Host multi-instance.
- Automatic janitor/archive on Mini dispose.
- Separate package.
