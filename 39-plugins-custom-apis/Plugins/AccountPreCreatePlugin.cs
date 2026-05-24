using System;
using Microsoft.Xrm.Sdk;

namespace AccountPlugins
{
    /// <summary>
    /// Pre-operation Create plugin for the Account entity.
    /// Validates that the account name is not empty and sets the account number
    /// to "ACC-" followed by today's date (yyyyMMdd).
    ///
    /// Registration:
    ///   Entity:   account
    ///   Message:  Create
    ///   Stage:    PreOperation (20)
    ///   Mode:     Synchronous
    /// </summary>
    public class AccountPreCreatePlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            tracingService.Trace("AccountPreCreatePlugin: Entered.");

            if (context.Stage != 20)
                throw new InvalidPluginExecutionException("This plugin must be registered as a PreOperation (stage 20) plugin.");

            if (context.MessageName != "Create")
                throw new InvalidPluginExecutionException("This plugin must be registered on the Create message.");

            if (!context.InputParameters.TryGetValue("Target", out object targetObj) || targetObj is not Entity target)
            {
                tracingService.Trace("AccountPreCreatePlugin: No Target found in InputParameters. Exiting.");
                return;
            }

            ValidateAccountName(target);
            SetAccountNumber(target, tracingService);

            tracingService.Trace("AccountPreCreatePlugin: Completed.");
        }

        private static void ValidateAccountName(Entity target)
        {
            var name = target.GetAttributeValue<string>("name");
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidPluginExecutionException("Account Name is required and cannot be empty.");
        }

        private static void SetAccountNumber(Entity target, ITracingService tracingService)
        {
            string dateSuffix = DateTime.UtcNow.ToString("yyyyMMdd");
            string accountNumber = $"ACC-{dateSuffix}";
            target["accountnumber"] = accountNumber;
            tracingService.Trace($"AccountPreCreatePlugin: accountnumber set to '{accountNumber}'.");
        }
    }
}
