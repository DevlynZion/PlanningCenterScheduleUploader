---
name: verify
description: How to verify changes to PlanningCenterScheduleUploader without hitting the live Planning Center API
---

# Verifying changes in this repo

The app is a WinForms exe (`PlanningCenterScheduleUploader`) over a class library
(`PlanningCenterScheduleUploaderLib`). A full GUI run requires real Planning Center
credentials (`%AppData%\PlanningCenterScheduleUploader\credentials.json`) and makes
live API calls from `PlanningCenterScheduler.DoChecks()` onward — do not drive that
live for verification.

## Excel-processing changes (ExcelProcessor, ScheduleContextFactory, validation-free paths)

Drive the library boundary exactly as the app does, no credentials needed:

```csharp
var processor = new ExcelProcessor(path);          // same ctor the UI uses
var context = processor.CreateScheduleModel();     // read + parse
processor.ProcessErrors(context);                  // writes "<name> (Errors).xlsx" to CWD
```

Recipe:
1. `dotnet new console` in the scratchpad, `dotnet add reference .../PlanningCenterScheduleUploaderLib.csproj`.
2. Generate adversarial workbooks with ClosedXML (already a transitive dependency):
   `Setup` tab (A=key, B=value; needs `Service Type` + `Team` rows) and `Schedule` tab
   (row 1 headers incl. `date`; data rows below).
3. Run cases, print `context.Errors` / `Assignments` / `Configs` **before and after**
   `ProcessErrors` — `ProcessErrors` can append errors (e.g. save-failure warning).
4. Inspect the emitted `(Errors).xlsx` by unzipping it (`Expand-Archive`) and reading
   `xl/worksheets/*.xml` — ClosedXML-written parts use `x:` prefixes (`<x:row`).

Useful adversarial inputs: out-of-range date serial (> 2958465, e.g. year 20206 typo),
text date, blank date with names present, fully blank row, numeric cell where a name
belongs, extra unrelated tabs (Google Sheets exports have them).

`Document/Scheduling (2).xlsx` is a real user file with an out-of-range date in
Schedule!A12 (serial 6686379) — good regression input.

## Gotchas

- ClosedXML `SaveAs` throws `ArgumentException: Not a legal OleAut date` on
  out-of-range date cells and finalizes a **truncated but well-formed** xlsx —
  a readable file is not proof the save succeeded; count rows in the sheet XML.
- `ProcessErrors` writes the errors copy to `Directory.GetCurrentDirectory()`,
  not next to the source file.
- PowerShell mangles `dotnet run` output through `Select-String`; pipe to a file
  (`Out-File run.txt`) and filter the file instead.
