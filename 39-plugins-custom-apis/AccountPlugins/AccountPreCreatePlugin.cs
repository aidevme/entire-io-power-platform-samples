using System;
using Microsoft.Xrm.Sdk;

namespace AccountPlugins
{
    /// <summary>
    /// Pre-operation plugin on Account Create.
    /// Validates that the account name is not empty and sets the account number
    /// to "ACC-" followed by today's date (yyyyMMdd).
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

            if (context.MessageName != "Create" || context.Stage != 20)
                return;

            if (!(context.InputParameters["Target"] is Entity target) || target.LogicalName != "account")
                return;

            tracingService.Trace("AccountPreCreatePlugin: executing for account create");

            ValidateAccountName(target);
            SetAccountNumber(target);

            tracingService.Trace("AccountPreCreatePlugin: completed successfully");
        }

        private static void ValidateAccountName(Entity target)
        {
            var name = target.GetAttributeValue<string>("name");

            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidPluginExecutionException(
                    "Account name is required and cannot be empty.");
        }

        private static void SetAccountNumber(Entity target)
        {
            var dateSuffix = DateTime.UtcNow.ToString("yyyyMMdd");
            target["accountnumber"] = $"ACC-{dateSuffix}";
        }
    }
}
