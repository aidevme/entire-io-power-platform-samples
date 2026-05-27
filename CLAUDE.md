# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository Purpose

Companion code samples for a [blog series](https://aidevme.com/entire-io-for-power-platform-developers-why-ai-coding-sessions-need-a-system-of-record/) on using [Entire.io](https://entire.io) across the full Power Platform code-first development stack. Each sample folder is self-contained — there is no repo-level build or test command. Navigate to a folder and follow its README.

## Prerequisites

- Node.js 22+, npm
- .NET 8 SDK + .NET Framework 4.6.2 (for Dataverse plugins)
- Power Platform CLI: `dotnet tool install -g Microsoft.PowerApps.CLI.Tool`
- Entire CLI: `npm install -g entire` then `entire login`
- Azure Functions Core Tools v4 (for `src/azurefunctions/`)
- PCF CLI: `npm install -g pcf-scripts` (for `src/pcfs/`)

## Build & Test Commands

Each folder below has its own README with detailed steps. Common entry points:

| Component | Build | Test |
|-----------|-------|------|
| Plugins (C#/.NET 4.6.2) | `dotnet build src/plugins/` | xUnit in `tests/plugins/` |
| Custom APIs (C#/.NET) | See `src/customapis/README.md` | xUnit in `tests/customapis/` |
| Web Resources (TypeScript) | Webpack — see `src/webresources/README.md` | Jest in `tests/webresources/` |
| PCF Controls | `pac pcf push` after `npm install` — see `src/pcfs/README.md` | — |
| Azure Functions | Azure Functions Core Tools — see `src/azurefunctions/README.md` | — |

Python utilities require:
```bash
pip install --upgrade azure-identity
```

## Environment Setup

Copy `.env` at the repo root and fill in values (`.env` is gitignored):

```
DATAVERSE_URL=https://<org>.crm.dynamics.com/
TENANT_ID=<tenant-id>
MCP_CLIENT_ID=<app-registration-id>
CLIENT_ID=<sp-client-id>              # enables service principal auth
CLIENT_SECRET=<sp-secret>             # enables service principal auth
SOLUTION_NAME=<solution-name>
PUBLISHER_PREFIX=<prefix>
PAC_AUTH_PROFILE=<pac-profile-name>
```

`scripts/auth.py` loads `.env` automatically (searches repo root, then cwd).

## Architecture Overview

| Folder | Technology | Description |
|--------|-----------|-------------|
| `src/webresources/` | TypeScript, Webpack | Form scripts for model-driven apps; one Webpack entry point per entity |
| `src/plugins/` | C#, .NET Framework 4.6.2 | Dataverse `IPlugin` implementations (pre/post operation hooks) |
| `src/customapis/` | C#, .NET | Custom REST endpoints exposed as Dataverse Custom APIs |
| `src/pcfs/` | TypeScript, Node.js | PCF dataset/field controls; packaged via `pac pcf push` |
| `src/azurefunctions/` | C#, Azure Functions v4 | HTTP + Service Bus triggers that call Dataverse via managed identity; includes OpenAPI custom connector definition |
| `terraform/` | Terraform | Azure Function App, App Service Plan, user-assigned managed identity, Dataverse role assignment |
| `scripts/` | Python | Dataverse auth utility + MCP client registration |
| `entire-config/` | Entire CLI config | Session redaction rules and security/governance settings |
| `tests/` | xUnit (.NET) + Jest (TS) | Unit tests mirroring `src/` structure |

## Python Utilities

### Authentication (`scripts/auth.py`)

Auth priority: service principal (`CLIENT_ID` + `CLIENT_SECRET`) → device code flow (interactive, token persisted to OS credential store).

```python
# PREFERRED — SDK for all supported CRUD operations
from auth import get_credential, load_env
from PowerPlatform.Dataverse.client import DataverseClient
load_env()
client = DataverseClient(os.environ["DATAVERSE_URL"], get_credential())

# ONLY for operations the SDK does NOT support (form XML, saved queries, $ref, $apply):
from auth import get_token, load_env
token = get_token()
```

### MCP Client Registration (`scripts/enable-mcp-client.py`)

```bash
python scripts/enable-mcp-client.py
```

Registers the app from `MCP_CLIENT_ID` in the Dataverse `allowedmcpclient` table.

## Key Conventions

- **SDK over raw HTTP** — use `DataverseClient` for all record CRUD. Only use `get_token()` + raw Web API for operations the SDK doesn't support: form XML, saved queries, `$ref`, `$apply`.
- **PAC CLI profile** — `PAC_AUTH_PROFILE` in `.env` controls which named profile `pac` uses. Authenticate once with `pac auth create --name <profile>`.
- **Solution prefix** — set `PUBLISHER_PREFIX` in `.env` before creating any Dataverse customizations to ensure correct schema name prefixing.

## Entire CLI Integration

This repository is Entire-enabled. Checkpoints are pushed automatically to `aidevme/entire-io-power-platform-samples`.

```bash
entire checkpoint list          # view session history
entire search --json "<query>"  # search checkpoints (always use --json flag)
```

> **Never** run `entire search` without `--json` — it opens an interactive TUI.

The `entire-search` sub-agent (`.claude/agents/entire-search.md`) is available for historical session search. Use it proactively when the user asks about previous work, commits, sessions, prompts, or historical context.
