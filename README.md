# Entire.io for Power Platform Developers — Code Samples

Companion code for the [9-part blog series](https://aidevme.com/entire-io-for-power-platform-developers-why-ai-coding-sessions-need-a-system-of-record/)
on using Entire.io across the full Power Platform code-first development stack.

## Prerequisites

- Node.js 22+, npm
- .NET 8 SDK
- Power Platform CLI (PAC CLI): `dotnet tool install -g Microsoft.PowerApps.CLI.Tool`
- Entire CLI: `npm install -g entire` then `entire login`
- Azure Functions Core Tools v4 (for 41-azure-functions)
- PCF CLI: `npm install -g pcf-scripts` (for 40-pcf-controls)

## How to Use

Each folder is self-contained. Navigate to a folder and follow its README.

The repository is Entire-enabled. After `entire enable` in the repo root,
any agent sessions you run while modifying the samples will be captured as checkpoints.
Run `entire checkpoint list` to see the existing session history.

## Series Articles

| # | Topic | Folder |
|---|---|---|
| 1 | [Why AI Sessions Need a System of Record](https://aidevme.com/...) | — |
| 2 | [Installation & Getting Started](https://aidevme.com/...) | — |
| 3 | [Form Scripts & Web Resources](https://aidevme.com/...) | `38-form-scripts/` |
| 4 | [Dataverse Plugins & Custom APIs](https://aidevme.com/...) | `39-plugins-custom-apis/` |
| 5 | [PCF Controls](https://aidevme.com/...) | `40-pcf-controls/` |
| 6 | [Azure Functions](https://aidevme.com/...) | `41-azure-functions/` |
| 7 | [The Web Interface](https://aidevme.com/...) | — |
| 8 | [Entire Skills](https://aidevme.com/...) | — |
| 9 | [Security & Governance](https://aidevme.com/...) | `entire-config/` |