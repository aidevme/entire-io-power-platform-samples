# .NET Agent Skills

This repository uses .NET for Dataverse plugins and custom APIs. AI coding agents can be enhanced with the official .NET team's curated skill set from [dotnet/skills](https://github.com/dotnet/skills/).

## dotnet/skills

**Repository:** <https://github.com/dotnet/skills/>

The `dotnet/skills` repository contains the .NET team's curated set of core skills and custom agents for coding agents, following the [agentskills.io](https://agentskills.io/) open standard.

### Available Plugins

| Plugin | Description |
|--------|-------------|
| `dotnet` | Core .NET skills for common .NET coding tasks |
| `dotnet-data` | .NET data access and Entity Framework tasks |
| `dotnet-diag` | .NET performance investigations and debugging |
| `dotnet-msbuild` | MSBuild and .NET build skills: diagnosis, optimization, and modernization |
| `dotnet-nuget` | NuGet and .NET package management |
| `dotnet-upgrade` | Migrating and upgrading .NET projects across framework versions |
| `dotnet-test` | Running, diagnosing, and migrating .NET tests |
| `dotnet-aspnet` | ASP.NET Core web development skills |
| `dotnet-ai` | AI and ML skills for .NET: LLM integration, MCP, and ML.NET |

### Installation

**VS Code (Copilot Chat):**

```json
// settings.json
{
  "chat.plugins.enabled": true,
  "chat.plugins.marketplaces": ["dotnet/skills"]
}
```

Then type `/plugins` in Copilot Chat to browse and install plugins.

**Copilot CLI / Claude Code:**

```bash
/plugin marketplace add dotnet/skills
/plugin install dotnet@dotnet-agent-skills
```

## Relevance to This Repository

The `src/plugins/` and `src/customapis/` folders target .NET Framework 4.6.2 and .NET 8 respectively. The `dotnet`, `dotnet-upgrade`, `dotnet-msbuild`, and `dotnet-nuget` plugins are particularly relevant when working on those components.
