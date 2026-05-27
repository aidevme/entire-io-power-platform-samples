using System;
using Microsoft.Xrm.Sdk;

namespace EntireSamples.Plugins
{
    /// <summary>
    /// Pre-operation plugin: account Create (stage 20, synchronous).
    /// Validates name is non-empty and sets accountnumber to ACC-yyyyMMdd (UTC).
    /// </summary>
    public class AccountPreCreatePlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            try
            {
                var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

                if (!string.Equals(context.MessageName, "Create", StringComparison.Ordinal) ||
                    !string.Equals(context.PrimaryEntityName, "account", StringComparison.Ordinal) ||
                    context.Stage != 20)
                    return;

                if (!context.InputParameters.TryGetValue("Target", out var raw) || !(raw is Entity target))
                {
                    tracingService?.Trace("AccountPreCreatePlugin: Target parameter missing or invalid — skipping.");
                    return;
                }

                tracingService?.Trace("AccountPreCreatePlugin: executing for account Create.");

                ValidateAccountName(target);
                SetAccountNumberPrefix(target);

                tracingService?.Trace("AccountPreCreatePlugin: completed successfully.");
            }
            catch (InvalidPluginExecutionException)
            {
                throw;
            }
            catch (Exception ex)
            {
                tracingService?.Trace($"AccountPreCreatePlugin: unexpected error — {ex.GetType().Name}: {ex.Message}");
                throw new InvalidPluginExecutionException(
                    $"An unexpected error occurred in AccountPreCreatePlugin: {ex.Message}", ex);
            }
        }

        private static void ValidateAccountName(Entity target)
        {
            if (string.IsNullOrWhiteSpace(target.GetAttributeValue<string>("name")))
                throw new InvalidPluginExecutionException("Account name cannot be empty.");
        }

        private static void SetAccountNumberPrefix(Entity target)
        {
            target["accountnumber"] = $"ACC-{DateTime.UtcNow:yyyyMMdd}";
        }
    }
}
