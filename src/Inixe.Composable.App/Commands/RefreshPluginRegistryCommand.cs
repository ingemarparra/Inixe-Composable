// -----------------------------------------------------------------------
// <copyright file="RefreshPluginRegistryCommand.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.Commands
{
    using System;
    using System.Threading.Tasks;
    using Inixe.Composable.App.Composition.PluginFramework;
    using Inixe.Composable.UI.Core;
    using Inixe.Composable.UI.Core.Commands;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Refreshes the known plugins list.
    /// </summary>
    /// <seealso cref="Inixe.Composable.UI.Core.Commands.AsyncCommandBase" />
    /// <remarks>
    /// This command can be used either for initial phases during the application initialization or for on-demand discovery of new plugins.
    /// </remarks>
    internal class RefreshPluginRegistryCommand : AsyncCommandBase
    {
        /// <summary>
        /// The command name which then can be used to get this command.
        /// </summary>
        public const string CommandName = "RefreshPluginRegistry";

        private readonly IPluginSource pluginSource;
        private readonly PluginInstance.PluginInstanceFactory pluginInstanceFactory;
        private readonly ILogger<RefreshPluginRegistryCommand> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshPluginRegistryCommand"/> class.
        /// </summary>
        /// <param name="pluginSource">The plugin source.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="pluginInstanceFactory">The plugin instance factory.</param>
        /// <exception cref="ArgumentNullException">
        /// pluginInstanceFactory
        /// or
        /// pluginSource
        /// or
        /// logger.
        /// </exception>
        public RefreshPluginRegistryCommand(IPluginSource pluginSource, ILogger<RefreshPluginRegistryCommand> logger, PluginInstance.PluginInstanceFactory pluginInstanceFactory)
        {
            this.pluginInstanceFactory = pluginInstanceFactory ?? throw new ArgumentNullException(nameof(pluginInstanceFactory));
            this.pluginSource = pluginSource ?? throw new ArgumentNullException(nameof(pluginSource));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public override string Name
        {
            get
            {
                return CommandName;
            }
        }

        private PluginInstance.PluginInstanceFactory LoadPluginFromManifest
        {
            get
            {
                return this.pluginInstanceFactory;
            }
        }

        /// <inheritdoc />
        public override Task ExecuteAsync(object parameter)
        {
            if (parameter is PluginRegistry registry)
            {
                var manifests = this.pluginSource.FindManifests();
                foreach (var manifest in manifests)
                {
                    this.logger.LogDebug("Processing: {@Manifest}", manifest);
                    if (registry.Contains(manifest.Id))
                    {
                        continue;
                    }

                    this.TryAddFromManifest(registry, manifest);
                }
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        protected override bool CanExecuteWhenIdle(object parameter)
        {
            return parameter is PluginRegistry;
        }

        private void TryAddFromManifest(PluginRegistry registry, IPluginManifest manifest)
        {
            this.logger.LogInformation("Registering plugin {PluginName} - {PluginID}", manifest.Name, manifest.Id);

            try
            {
                var instance = this.LoadPluginFromManifest(manifest);
                registry.Add(instance);

                this.logger.LogInformation("Successfully registered plugin {PluginName} - {PluginID}", manifest.Name, manifest.Id);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Could not load {PluginName} - {PluginID}. Reason: {Reason}", manifest.Name, manifest.Id, ex.Message);
            }
        }
    }
}
