using System;
using Microsoft.Xrm.Sdk;

namespace EntireSamples.Plugins
{
    /// <summary>
    /// Pre-operation plugin for the Account Create message.
    /// Validates that the account name is not empty and sets the
    /// account number to "ACC-" followed by today's date (yyyyMMdd).
    /// 
    /// Registration:
    ///   Entity:   account
    ///   Message:  Create
    ///   Stage:    20 (Pre-Operation)
    ///   Mode:     Synchronous
    /// </summary>
    public class AccountPreCreatePlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            if (context.MessageName != "Create" || context.PrimaryEntityName != "account")
                return;

            if (context.Stage != 20)
                return;

            if (!(context.InputParameters["Target"] is Entity target))
                return;

            tracingService.Trace("AccountPreCreatePlugin: executing for account Create.");

            ValidateAccountName(target);
            SetAccountNumberPrefix(target);

            tracingService.Trace("AccountPreCreatePlugin: completed successfully.");
        }

        private static void ValidateAccountName(Entity target)
        {
            var name = target.GetAttributeValue<string>("name");

            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidPluginExecutionException("Account name cannot be empty.");
        }

        private static void SetAccountNumberPrefix(Entity target)
        {
            var dateSuffix = DateTime.UtcNow.ToString("yyyyMMdd");
            target["accountnumber"] = $"ACC-{dateSuffix}";
        }
    }
}
