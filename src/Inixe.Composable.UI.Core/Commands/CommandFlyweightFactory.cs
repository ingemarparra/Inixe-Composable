// -----------------------------------------------------------------------
// <copyright file="CommandFlyweightFactory.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.UI.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Input;

    /// <summary>
    /// Flyweight factory for commands.
    /// </summary>
    /// <seealso cref="Inixe.Composable.UI.Core.Commands.ICommandFactory" />
    public sealed class CommandFlyweightFactory : ICommandFactory
    {
        private readonly CommandCollection commands;
        private readonly IDictionary<string, IEnumerable<string>> permissionsCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandFlyweightFactory"/> class.
        /// </summary>
        /// <param name="commands">The commands.</param>
        public CommandFlyweightFactory(IList<INamedCommand> commands)
        {
            this.commands = new CommandCollection(commands);
            this.permissionsCache = new Dictionary<string, IEnumerable<string>>();
        }

        /// <summary>
        /// Determines whether this instance can create a command from the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if this instance can create a command from the specified name; otherwise, <c>false</c>.
        /// </returns>
        public bool CanCreateCommand(string name)
        {
            return this.commands.Contains(name);
        }

        /// <summary>
        /// Creates a command using the specified name for the name implementation.
        /// </summary>
        /// <param name="name">The command name.</param>
        /// <returns>
        /// The command implementation if found. Otherwise an exception is thrown.
        /// </returns>
        /// <exception cref="ArgumentException">Invalid command name - name.</exception>
        public ICommand CreateCommand(string name)
        {
            if (!this.CanCreateCommand(name))
            {
                throw new ArgumentException("Invalid command name", nameof(name));
            }

            return this.commands[name];
        }

        /// <summary>
        /// Creates a command using the specified name for the name implementation. The returned command monitors the specified object for changes and raises it's <see cref="ICommand.CanExecuteChanged" /> event in consequence.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="monitoredInstance">The monitored instance.</param>
        /// <returns>
        /// The command implementation if found. Otherwise an exception is thrown.
        /// </returns>
        public ICommand CreateCommand(string name, INotifyPropertyChanged monitoredInstance)
        {
            var cmd = this.CreateCommand(name);
            return new PropertyMonitoringCommandDecorator(cmd, monitoredInstance);
        }

        public IEnumerable<string> GetCommandPermissions(string name)
        {
            if (!this.CanCreateCommand(name))
            {
                throw new ArgumentException("Invalid command name", nameof(name));
            }

            if (!this.permissionsCache.ContainsKey(name))
            {
                var atts = TypeDescriptor.GetAttributes(this.commands[name]);
                var permissions = atts.OfType<CommandPermissionAttribute>()
                    .Select(x => x.PermissionName)
                    .ToArray();

                this.permissionsCache.Add(name, permissions);
            }

            return this.permissionsCache[name];
        }

        private class CommandCollection : System.Collections.ObjectModel.KeyedCollection<string, INamedCommand>
        {
            public CommandCollection(IList<INamedCommand> commands)
            {
                if (commands == null)
                {
                    throw new ArgumentNullException("Invalid commands list");
                }

                foreach (var item in commands)
                {
                    this.Add(item);
                }
            }

            protected override string GetKeyForItem(INamedCommand item)
            {
                return item.Name;
            }
        }
    }
}
