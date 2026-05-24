## Repo Structure

```
entire-io-power-platform-samples/
├── README.md                          ← Series overview, prerequisites, how to use
├── CONTRIBUTING.md                    ← How to run samples, submit issues, style guide
├── .gitignore
├── .entire/
│   └── settings.json                  ← Entire enabled; sample redaction rules
├── .github/
│   ├── ISSUE_TEMPLATE/
│   │   └── bug_report.md
│   └── pull_request_template.md
│
├── entire-config/
│   ├── README.md                      ← Links to Article 44; redaction setup guide
│   ├── settings.json.example          ← Entire settings with redaction rules
│   └── custom-redaction.json.example  ← PP-specific redaction: env GUIDs, connection refs
│
├── terraform/
│   ├── README.md                      ← Prerequisites, terraform init/plan/apply instructions
│   ├── main.tf                        ← Root module: resource group, providers
│   ├── variables.tf                   ← Input variables (location, prefix, env)
│   ├── outputs.tf                     ← Output values (function app URL, storage connection)
│   ├── terraform.tfvars.example       ← Example values; never commit real tfvars
│   └── modules/
│       ├── function-app/              ← Azure Function App + App Service Plan
│       │   ├── main.tf
│       │   ├── variables.tf
│       │   └── outputs.tf
│       └── managed-identity/          ← User-assigned MI + Dataverse role assignment
│           ├── main.tf
│           ├── variables.tf
│           └── outputs.tf
│
└── src/
    ├── webresources/
    │   ├── README.md                  ← Links to Article 38; setup + webpack entry map
    │   ├── package.json               ← Single build for all entities
    │   ├── tsconfig.json
    │   ├── webpack.config.js          ← Entry per entity; outputs dist/opportunity.js etc.
    │   ├── shared/                    ← Utilities shared across all entities
    │   │   └── ClientApiUtils.ts
    │   ├── opportunity/               ← Scripts registered on Opportunity forms
    │   │   └── FormScripts.ts             ← onSave validation handler
    │   └── account/                   ← Scripts registered on Account forms
    │       └── FormScripts.ts             ← onLoad handler example
    │
    ├── plugins/
    │   ├── README.md                  ← Links to Article 39; registration notes
    │   ├── Plugins.sln                ← Solution referencing Plugins.csproj
    │   ├── Plugins.csproj             ← Single assembly; all plugin classes compiled together
    │   ├── opportunity/               ← Namespace: MyCompany.Plugins.Opportunity
    │   │   └── OpportunityScoringPlugin.cs
    │   └── account/                   ← Namespace: MyCompany.Plugins.Account
    │       └── AccountValidationPlugin.cs
    │
    ├── customapis/
    │   ├── README.md                  ← Links to Article 39; registration notes
    │   ├── CustomApis.sln             ← Solution referencing CustomApis.csproj
    │   ├── CustomApis.csproj          ← Single assembly; all custom API handlers compiled together
    │   └── quote/                    ← Namespace: MyCompany.CustomApis.Quote
    │       └── CalculateQuoteTotalHandler.cs
    │
    ├── pcfs/
    │   ├── README.md                  ← Links to Article 40; pac pcf push notes
    │   └── AccountOpportunityGrid/    ← Dataset PCF control (opportunities grid)
    │       ├── AccountOpportunityGrid/
    │       │   ├── ControlManifest.Input.xml
    │       │   ├── index.ts
    │       │   └── css/
    │       │       └── AccountOpportunityGrid.css
    │       ├── package.json
    │       ├── tsconfig.json
    │       └── pcfconfig.json
    │
    ├── codeapps/
    │   └── README.md                  ← Placeholder; no code app sample in this series
    │
    └── azurefunctions/
        ├── README.md                  ← Links to Article 41; Managed Identity setup
        ├── DocumentGenerationFunction/  ← HTTP-triggered, calls Dataverse Web API
        │   ├── DocumentGenerationFunction.csproj
        │   ├── DocumentGenerationFunction.cs
        │   ├── host.json
        │   └── local.settings.json.example
        ├── ServiceBusProcessor/       ← PeekLock pattern, Power Automate integration
        │   ├── ServiceBusProcessor.csproj
        │   └── ServiceBusTriggeredProcessor.cs
        └── custom-connector/
            └── openapi-definition.yaml  ← Custom connector definition for the functions

tests/
    ├── webresources/
    │   ├── package.json               ← Single Jest project mirroring src/webresources/
    │   ├── tsconfig.json
    │   ├── jest.config.js
    │   ├── shared/
    │   │   └── ClientApiUtils.test.ts
    │   └── opportunity/
    │       └── FormScripts.test.ts
    │
    ├── plugins/
    │   └── Plugins.Tests/             ← Single xUnit project for all plugin classes
    │       ├── Plugins.Tests.csproj
    │       ├── opportunity/
    │       │   ├── OpportunityScoringPluginTests.cs
    │       │   └── Fakes/
    │       │       └── FakeOrganizationService.cs  ← IOrganizationService stub
    │       └── account/
    │           └── AccountValidationPluginTests.cs
    │
    ├── customapis/
    │   └── CustomApis.Tests/          ← Single xUnit project for all custom API handlers
    │       ├── CustomApis.Tests.csproj
    │       └── quote/
    │           └── CalculateQuoteTotalHandlerTests.cs
    │
    ├── pcfs/
    │   └── AccountOpportunityGrid.test/  ← Jest tests for the PCF control
    │       ├── index.test.ts
    │       ├── package.json
    │       ├── tsconfig.json
    │       └── jest.config.js
    │
    └── azurefunctions/
        └── AzureFunctions.Tests/      ← xUnit tests for both Azure Functions
            ├── AzureFunctions.Tests.csproj
            ├── DocumentGenerationFunctionTests.cs
            └── ServiceBusProcessorTests.cs
```