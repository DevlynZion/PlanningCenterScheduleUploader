# Planning Center Schedule Uploader

A Windows desktop application that reads a church service schedule from an Excel file and uploads team assignments to [Planning Center Online](https://www.planningcenteronline.com) via the Planning Center API.

---

## Download & Install

1. Go to the [Releases page](../../releases) and download the latest `PlanningCenterScheduleUploader.exe`
2. Save it anywhere on your computer — no installation required
3. Double-click to run

> **Windows SmartScreen warning:** Because the app is not code-signed, Windows may show a "Windows protected your PC" message on first run. Click **"More info"** then **"Run anyway"** to proceed.

---

## First-Time Setup

On first launch the app will ask for your Planning Center API credentials.

### Getting your credentials

1. Log into Planning Center and go to: **https://api.planningcenteronline.com/oauth/applications**
2. Click **"New Personal Access Token"**
3. Give it a name (e.g. "Schedule Uploader") and click **Create**
4. Copy the **Application ID** and **Secret**

### Entering credentials in the app

1. Click the **Settings** button in the top-right corner of the app
2. Paste your **Application ID** and **Secret** into the fields
3. Click **Save**

Credentials are stored locally on your computer and are never shared.

---

## Excel File Format

The app reads scheduling data from an Excel file with two tabs: **Setup** and **Schedule**.

### Setup tab

| Column A (Key) | Column B (Value) |
|---|---|
| Service Type | _Exact name of the service type in Planning Center_ |
| Team | _Exact name of the team in Planning Center_ |

### Schedule tab

- **Row 1 — Headers:** The first column must be named `date`. Each additional column is a role name that must match a Planning Center team position exactly.
- **Rows 2+ — Data:** The `date` column must contain a valid Excel date. Each role column contains the full name of the person to assign — the name must match their name in Planning Center exactly (case-sensitive).

**Example:**

| date | Sound | Presenter | Worship Leader |
|---|---|---|---|
| 2024-06-02 | Jane Smith | Bob Jones | Alice Brown |
| 2024-06-09 | Jane Smith | | Alice Brown |

### After upload

If any errors occur, the app saves a copy of your file named `<original file> (Errors).xlsx` in the same folder, with problem cells highlighted:
- **Red** — errors that prevented upload
- **Yellow** — warnings
- **Blue** — informational notices

---

## Reporting Issues

After an upload, click the **Copy Logs** button at the bottom of the app to copy the log to your clipboard. Paste the log into the feedback form (link to be provided by your administrator) when reporting an issue.

---

## Updating the App

The app checks for updates automatically when it starts. If a newer version is available, a link will appear at the bottom of the window. Click it to open the Releases page and download the new version — replace your existing `PlanningCenterScheduleUploader.exe` with the downloaded file.

---

## License

GNU General Public License v2.0 — see [LICENSE](LICENSE) for details.
