// -----------------------------------------------------------------------
// <copyright file="TogglePluginCommand.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.Commands
{
    using System;
    using Inixe.Composable.App.Composition.PluginFramework;
    using Inixe.Composable.UI.Core;
    using Inixe.Composable.UI.Core.Commands;

    /// <summary>
    /// Starts or stops a plugin execution.
    /// </summary>
    /// <seealso cref="Inixe.Composable.UI.Core.Commands.INamedCommand" />
    internal class TogglePluginCommand : INamedCommand
    {
        /// <summary>
        /// The command name which then can be used to get this command.
        /// </summary>
        public const string CommandName = "StartPlugin";

        private readonly PluginRegistry registry;

        /// <summary>
        /// Initializes a new instance of the <see cref="TogglePluginCommand"/> class.
        /// </summary>
        /// <param name="registry">The registry.</param>
        /// <exception cref="ArgumentNullException">When registry is <c>null</c>.</exception>
        public TogglePluginCommand(PluginRegistry registry)
        {
            this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        /// <inheritdoc />
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc />
        public string Name
        {
            get
            {
                return CommandName;
            }
        }

        /// <inheritdoc />
        public bool CanExecute(object parameter)
        {
            return parameter is IPluginManifest m ? this.registry.Contains(m.Id) : false;
        }

        /// <inheritdoc />
        public void Execute(object parameter)
        {
            if (parameter is IPluginManifest m && this.registry.Contains(m.Id))
            {
                if (!this.registry[m.Id].IsLoaded)
                {
                    this.registry[m.Id].Load();
                }

                if (!this.registry[m.Id].Plugin.IsRunning)
                {
                    this.registry[m.Id].Start();
                }
                else
                {
                    this.registry[m.Id].Stop();
                }
            }
        }
    }
}
