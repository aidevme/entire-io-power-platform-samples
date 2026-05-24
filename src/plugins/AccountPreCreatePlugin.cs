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
        /// <summary>
        /// Entry point called by the Dataverse plugin pipeline.
        /// Resolves execution context and tracing service, guards against incorrect
        /// registration, then delegates to <see cref="ValidateAccountName"/> and
        /// <see cref="SetAccountNumberPrefix"/>.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider supplied by the Dataverse runtime, used to resolve
        /// <see cref="IPluginExecutionContext"/> and <see cref="ITracingService"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="serviceProvider"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="InvalidPluginExecutionException">
        /// Thrown when account name validation fails or an unexpected error occurs.
        /// </exception>
        public void Execute(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            try
            {
                var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

                if (context.MessageName != "Create" || context.PrimaryEntityName != "account")
                    return;

                if (context.Stage != 20)
                    return;

                if (!context.InputParameters.ContainsKey("Target") ||
                    !(context.InputParameters["Target"] is Entity target))
                {
                    tracingService.Trace("AccountPreCreatePlugin: Target parameter missing or invalid — skipping.");
                    return;
                }

                tracingService.Trace("AccountPreCreatePlugin: executing for account Create.");

                ValidateAccountName(target);
                SetAccountNumberPrefix(target);

                tracingService.Trace("AccountPreCreatePlugin: completed successfully.");
            }
            catch (InvalidPluginExecutionException)
            {
                // Re-throw business rule violations directly — they surface as user-facing errors.
                throw;
            }
            catch (Exception ex)
            {
                tracingService.Trace($"AccountPreCreatePlugin: unexpected error — {ex.GetType().Name}: {ex.Message}");
                throw new InvalidPluginExecutionException(
                    $"An unexpected error occurred in AccountPreCreatePlugin: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Ensures the account's <c>name</c> attribute is not null, empty, or whitespace.
        /// </summary>
        /// <param name="target">The target <see cref="Entity"/> from the plugin input parameters.</param>
        /// <exception cref="InvalidPluginExecutionException">
        /// Thrown when <c>name</c> is null, empty, or whitespace — surfaces as a user-facing error.
        /// </exception>
        private static void ValidateAccountName(Entity target)
        {
            var name = target.GetAttributeValue<string>("name");

            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidPluginExecutionException("Account name cannot be empty.");
        }

        /// <summary>
        /// Sets the <c>accountnumber</c> attribute to <c>ACC-yyyyMMdd</c> using the current UTC date.
        /// Any value previously set by the user is overwritten.
        /// </summary>
        /// <param name="target">The target <see cref="Entity"/> from the plugin input parameters.</param>
        private static void SetAccountNumberPrefix(Entity target)
        {
            var dateSuffix = DateTime.UtcNow.ToString("yyyyMMdd");
            target["accountnumber"] = $"ACC-{dateSuffix}";
        }
    }
}
