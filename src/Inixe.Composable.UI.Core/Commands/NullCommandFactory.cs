// -----------------------------------------------------------------------
// <copyright file="NullCommandFactory.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.UI.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Input;

    /// <summary>
    /// Null Object Implementation for <see cref="ICommandFactory"/>.
    /// </summary>
    /// <seealso cref="Inixe.Composable.UI.Core.Commands.ICommandFactory" />
    public sealed class NullCommandFactory : ICommandFactory
    {
        public static readonly ICommand NoCommand = new NullCommand();

        /// <summary>
        /// Determines whether this instance can create a command from the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if this instance can create a command from the specified name; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>As part of the <c>NullCommandFactory</c> no other command but <see cref="NullCommand"/> and this method will always return <c>False</c>.</remarks>
        public bool CanCreateCommand(string name)
        {
            return false;
        }

        /// <summary>
        /// Creates a command using the specified name for the name implementation.
        /// </summary>
        /// <param name="name">The command name.</param>
        /// <returns>
        /// The command implementation if found.
        /// </returns>
        /// <remarks>This implementation always returns <see cref="NullCommand"/>.</remarks>
        public ICommand CreateCommand(string name)
        {
            return NoCommand;
        }

        /// <summary>
        /// Creates a command using the specified name for the name implementation. The returned command monitors the specified object for changes and raises it's <see cref="ICommand.CanExecuteChanged" /> event in consequence.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="monitoredInstance">The monitored instance.</param>
        /// <returns>
        /// The command implementation if found. Otherwise an exception is thrown.
        /// </returns>
        /// <remarks>This implementation always returns <see cref="NullCommand"/>.</remarks>
        public ICommand CreateCommand(string name, INotifyPropertyChanged monitoredInstance)
        {
            return NoCommand;
        }

        /// <summary>
        /// Gets a command permissions.
        /// </summary>
        /// <param name="name">The command name.</param>
        /// <returns>The command's permissions.</returns>
        public IEnumerable<string> GetCommandPermissions(string name)
        {
            return Array.Empty<string>();
        }
    }
}
