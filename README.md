`Main.cs` needs `DBC_LOCATION` changed to the DB file location and `DBD_LOCATION` to the DBD files location.
The program expects the DB files to be in build named folders e.g. `.\0.5.3.3368\Map.dbc`.

Controls:
- Lock Scroll X/Y: both grid's scroll bars are synchronized and scroll by cell width/height when using the scroll arrows
- Matching Ids Only: only shows rows where the first column values intersect
- Rows/Cols: comparison direction
  - Row: green for full match, blue for first X match
  - Cols: matched cells are green, displays % match for each column
- Validate Def: validates every definition against every file and lists all errors

Errors:
- DEFINITION_ISSUE: definition is malformed or a build has multiple definitions
- FIELD_COUNT: definition field count mismatches DB header's field count
- INTASFLOAT: float field is potentially an int
- INVALID_IDS: ids are not all unique and positive
- INVALID_STRINGS: string not found in the stringtable
- MALFORMED: DB file's header is corrupt
- NO_FIELDS, NO_RECORDS: DB file's header has 0 records and/or fields
- UNUSED_STRINGS: stringtable has strings but the definition has no string fields

