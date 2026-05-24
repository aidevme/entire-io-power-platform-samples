# Copilot Instructions

## Repository Overview

Companion code samples for a blog series on using [Entire.io](https://entire.io) across the full Power Platform code-first development stack.

| Folder | Topic |
|---|---|
| `src/webresources/` | Form Scripts & Web Resources (Article 38) |
| `src/plugins/` | Dataverse Plugins (.NET Framework 4.6.2 / C#) (Article 39) |
| `src/customapis/` | Custom APIs (.NET/C#) (Article 39) |
| `src/pcfs/` | PCF Controls (Node.js) (Article 40) |
| `src/codeapps/` | Code Apps — placeholder, no sample in this series |
| `src/azurefunctions/` | Azure Functions (Article 41) |
| `terraform/` | Infrastructure — Function App, Managed Identity |
| `entire-config/` | Security & Governance (Article 44) |
| `tests/` | Unit tests mirroring `src/` (xUnit + Jest) |

## Prerequisites

- Node.js 22+, npm
- .NET 8 SDK
- .NET Framework 4.6.2 (for Dataverse plugins)
- Power Platform CLI: `dotnet tool install -g Microsoft.PowerApps.CLI.Tool`
- Entire CLI: `npm install -g entire` then `entire login`
- Azure Functions Core Tools v4 (for `41-azure-functions/`)
- PCF CLI: `npm install -g pcf-scripts` (for `40-pcf-controls/`)

## Environment Setup

Copy `.env` in the repo root and fill in required values:

```
DATAVERSE_URL=https://<org>.crm.dynamics.com/
TENANT_ID=<tenant-id>
MCP_CLIENT_ID=<app-registration-id>   # for MCP server access
CLIENT_ID=<sp-client-id>              # optional — enables service principal auth
CLIENT_SECRET=<sp-secret>             # optional — enables service principal auth
SOLUTION_NAME=<solution-name>
PUBLISHER_PREFIX=<prefix>
PAC_AUTH_PROFILE=<pac-profile-name>
```

`.env` is gitignored. `scripts/auth.py` loads it automatically (searches repo root, then cwd).

## Python Utilities (`scripts/`)

### Authentication (`scripts/auth.py`)

Auth priority: service principal (CLIENT_ID + CLIENT_SECRET) → device code flow (interactive, token persisted to OS credential store).

```python
# PREFERRED — use SDK for all supported operations
from auth import get_credential, load_env
from PowerPlatform.Dataverse.client import DataverseClient
load_env()
client = DataverseClient(os.environ["DATAVERSE_URL"], get_credential())

# ONLY for operations the SDK does NOT support (forms, views, $ref, $apply):
from auth import get_token, load_env
token = get_token()
```

### MCP Client Registration (`scripts/enable-mcp-client.py`)

Registers the app from `MCP_CLIENT_ID` in the Dataverse `allowedmcpclient` table:

```
python scripts/enable-mcp-client.py
```

## Entire CLI Integration

This repository is Entire-enabled. AI session checkpoints are captured automatically during agent sessions. Checkpoints are pushed to `aidevme/pp-session-records`.

```bash
entire checkpoint list          # view session history
entire search --json "<query>"  # search checkpoints and transcripts (always use --json)
```

> **Never** run `entire search` without `--json` — it opens an interactive TUI.

The `entire-search` sub-agent (`.claude/agents/entire-search.md`) is available in Claude Code for historical session search. Use it proactively when the user asks about previous work, sessions, or prompts.

## Key Conventions

- **Each sample folder is self-contained** — follow the folder's own README for build/test steps; there is no repo-level build or test command.
- **SDK over raw HTTP** — use `DataverseClient` (Python SDK) for all record CRUD. Only drop to raw `get_token()` + Web API for operations the SDK doesn't support: form XML, saved queries, `$ref`, `$apply`.
- **PAC CLI profile** — the `PAC_AUTH_PROFILE` env var controls which named profile `pac` uses. Authenticate once with `pac auth create --name <profile>`.
- **Solution prefix** — set `PUBLISHER_PREFIX` in `.env` before creating any Dataverse customizations to ensure correct schema name prefixing.
