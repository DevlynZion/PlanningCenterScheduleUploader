# PlanningCenterScheduleUploader — Claude Context

## Project Purpose

Windows Forms desktop application (.NET 8) that reads a church service schedule from an Excel file and uploads team assignments to Planning Center Online via its JSON API 1.0. Targets non-technical church staff as end users.

## Repository Structure

```
PlanningCenterScheduleUploader/          ← repo root
├── .github/workflows/release.yml        ← CI/CD: builds & publishes on git tag
├── Project/PlanningCenterScheduleUploader/
│   ├── PlanningCenterScheduleUploader.sln
│   ├── PlanningCenterScheduleUploader/  ← WinForms UI (WinExe, net8.0-windows)
│   ├── PlanningCenterAPI/               ← HTTP client for Planning Center API
│   ├── PlanningCenterScheduleUploaderLib/ ← Core business logic
│   ├── PlanningCenterAPI.Test/          ← Manual test harness (not automated)
│   └── ProcessSourceFile/               ← Incomplete utility, unused
└── Document/
    ├── ExampleData/Nkele/               ← Sample Excel schedules
    └── Planning Centre JSON API Structure.xlsx
```

## Architecture

### Data Flow
1. User selects an Excel file via the UI
2. `ExcelProcessor` reads the `Setup` and `Schedule` tabs into a `ScheduleContext`
3. `PlanningCenterScheduler.DoChecks()` runs 8 validation steps (pipeline pattern)
4. If no errors: `ClearPlans()` removes existing assignments, `SubmitScheduling()` uploads new ones
5. `ExcelProcessor.ProcessErrors()` writes errors back to a copy of the Excel file

### Key Patterns
- **Pipeline validation**: `IPipelineStep<T>` in `PlanningCenterScheduleUploaderLib/Pipeline/`; 8 steps each validate one aspect (service type, team, plans, roles, people, blockout days, duplicate assignments)
- **Rate limiter**: `RateLimiter.cs` in `PlanningCenterAPI/Core/` throttles all HTTP requests to avoid Planning Center API limits
- **Error tracking**: `ScheduleErrors` tracks errors by cell coordinate (tab name, row, column) for Excel annotation

### Excel File Format
- **Setup tab**: Two columns — key in A, value in B. Required keys: `Service Type`, `Team`
- **Schedule tab**: Row 1 = headers (`date` column required; other columns = role names matching Planning Center team position names exactly). Rows 2+ = data (date must be an Excel date value; cells contain person full names matching Planning Center exactly)
- On error: a `<filename> (Errors).xlsx` file is saved with color-coded cells

## Planning Center API

- Spec: JSON API 1.0 — https://api.planningcenteronline.com/docs/overview/json-api
- Apps reference: https://api.planningcenteronline.com/docs/apps
- Rate limiting: https://api.planningcenteronline.com/docs/overview/rate-limiting
- Authentication: HTTP Basic Auth with Base64-encoded `AppId:Secret` (Personal Access Tokens)

## Authentication

**Current approach (beta):** Personal Access Tokens stored in `%AppData%\PlanningCenterScheduleUploader\credentials.json`.
- Users enter credentials via a Settings dialog in the app (auto-opens on first launch)
- `AuthenticationHelper.cs` reads/writes this file
- `Client.cs` calls `AuthenticationHelper.GetCredentials()` in its constructor — returns `"AppId:Secret"` which is Base64-encoded for the `Authorization: Basic` header

**Future (not yet implemented):** OAuth 2 — requires admin rights in Planning Center to register an OAuth application. Track this as a future feature.

## Credentials Setup (for testers)
1. Log into https://api.planningcenteronline.com/oauth/applications
2. Create a Personal Access Token
3. Copy the Application ID and Secret
4. Open the app → click Settings → paste credentials → Save

## Building & Releasing

**Local build:**
```
dotnet build Project/PlanningCenterScheduleUploader/PlanningCenterScheduleUploader.sln
```

**Create a release:**
```
git tag v0.1.0
git push --tags
```
GitHub Actions will build a self-contained single-file `win-x64` exe and attach it to a GitHub Release automatically.

**Version:** Defined in `PlanningCenterScheduleUploader.csproj` as `<Version>`. Bump this before tagging.

## Branching & PR Policy

- Every new feature or change goes into its own branch
- PRs target `main` using **rebase** strategy (not merge)
- PRs are reviewed by Qodo AI before merging

## Known Limitations (beta)

- Executable is unsigned — users see a Windows SmartScreen warning on first run ("More info → Run anyway")
- No auto-update mechanism; app notifies when update is available, user must download manually
- No persistent log file — logs shown in the UI listbox only
- No automated tests; `PlanningCenterAPI.Test` is a manual test harness

## Future Features

- OAuth 2 authentication (replaces Personal Access Tokens)
- Code signing (removes SmartScreen warning)
- Automated unit/integration tests
- MSIX installer
- Persistent log file on disk
