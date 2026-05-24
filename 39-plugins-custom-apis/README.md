# 39 — Dataverse Plugins & Custom APIs

Dataverse plugin sample for the Account entity.

## Plugin: `AccountPreCreatePlugin`

**File:** `Plugins/AccountPreCreatePlugin.cs`

Executes on **Account → Create (PreOperation, Stage 20, Synchronous)** and:

1. Validates that `name` (Account Name) is not empty — throws `InvalidPluginExecutionException` if blank.
2. Sets `accountnumber` to `"ACC-"` followed by today's UTC date (`yyyyMMdd`), e.g. `ACC-20260524`.

## Build

```bash
cd Plugins
dotnet build
```

Output: `Plugins/bin/Debug/net462/AccountPlugins.dll`

## Register with PAC CLI

```bash
# Authenticate (once)
pac auth create --name <profile> --url $DATAVERSE_URL

# Upload assembly
pac plugin push --pluginFile Plugins/bin/Debug/net462/AccountPlugins.dll

# Then register the step in Power Apps Plug-in Registration tool:
#   Assembly:  AccountPlugins
#   Type:      AccountPlugins.AccountPreCreatePlugin
#   Message:   Create
#   Entity:    account
#   Stage:     PreOperation (20)
#   Mode:      Synchronous
```
