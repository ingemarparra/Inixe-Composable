// -----------------------------------------------------------------------
// <copyright file="RoutingCommandFactory.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.Composition.PluginFramework
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;
    using Inixe.Composable.UI.Core.Commands;

    /// <summary>
    /// Global command factory that allows to share commands among plug-ins.
    /// </summary>
    /// <seealso cref="Inixe.Composable.UI.Core.Commands.ICommandFactory" />
    internal class RoutingCommandFactory : ICommandFactory
    {
        private readonly Lazy<CommandFactoryRegistry> commandFactoryInstancesLazy;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutingCommandFactory"/> class.
        /// </summary>
        /// <param name="registryResolver">The registry resolver.</param>
        public RoutingCommandFactory(Func<CommandFactoryRegistry> registryResolver)
        {
            ArgumentNullException.ThrowIfNull(registryResolver, nameof(registryResolver));
            this.commandFactoryInstancesLazy = new Lazy<CommandFactoryRegistry>(registryResolver);
        }

        private CommandFactoryRegistry Registry
        {
            get
            {
                return this.commandFactoryInstancesLazy.Value;
            }
        }

        /// <summary>
        /// Determines whether this instance can create a command from the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// <c>true</c> if this instance can create a command from the specified name; otherwise, <c>false</c>.
        /// </returns>
        public bool CanCreateCommand(string name)
        {
            return this.Registry.Any(x => x.CommandFactory.CanCreateCommand(name));
        }

        /// <summary>
        /// Creates a command using the specified name for the name implementation.
        /// </summary>
        /// <param name="name">The command name.</param>
        /// <returns>
        /// The command implementation if found. Otherwise an exception is thrown.
        /// </returns>
        public ICommand CreateCommand(string name)
        {
            var factory = this.FindFactory(name);
            return factory.CreateCommand(name);
        }

        /// <summary>
        /// Creates a command using the specified name for the name implementation. The returned command monitors the specified object for changes and raises it's <see cref="E:System.Windows.Input.ICommand.CanExecuteChanged" /> event in consequence.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="monitoredInstance">The monitored instance.</param>
        /// <returns>
        /// The command implementation if found. Otherwise an exception is thrown.
        /// </returns>
        public ICommand CreateCommand(string name, INotifyPropertyChanged monitoredInstance)
        {
            var factory = this.FindFactory(name);
            return factory.CreateCommand(name, monitoredInstance);
        }

        /// <summary>
        /// Gets a command permissions.
        /// </summary>
        /// <param name="name">The command name.</param>
        /// <returns>The command's permissions.</returns>
        public IEnumerable<string> GetCommandPermissions(string name)
        {
            var factory = this.FindFactory(name);
            return factory.GetCommandPermissions(name);
        }

        private ICommandFactory FindFactory(string name)
        {
            var factoryInstance = this.Registry.SingleOrDefault(x => x.CommandFactory.CanCreateCommand(name));
            var factory = factoryInstance?.CommandFactory;
            if (factory == null)
            {
                throw new ArgumentException("Invalid command name", nameof(name));
            }

            return factory;
        }
    }
}
