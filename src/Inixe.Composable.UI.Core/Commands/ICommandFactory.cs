// -----------------------------------------------------------------------
// <copyright file="ICommandFactory.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.UI.Core.Commands
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Input;

    /// <summary>
    /// Factory for commands.
    /// </summary>
    public interface ICommandFactory
    {
        /// <summary>
        /// Determines whether this instance can create a command from the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if this instance can create a command from the specified name; otherwise, <c>false</c>.
        /// </returns>
        bool CanCreateCommand(string name);

        /// <summary>
        /// Creates a command using the specified name for the name implementation.
        /// </summary>
        /// <param name="name">The command name.</param>
        /// <returns>The command implementation if found. Otherwise an exception is thrown.</returns>
        ICommand CreateCommand(string name);

        /// <summary>
        /// Creates a command using the specified name for the name implementation. The returned command monitors the specified object for changes and raises it's <see cref="ICommand.CanExecuteChanged"/> event in consequence.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="monitoredInstance">The monitored instance.</param>
        /// <returns>The command implementation if found. Otherwise an exception is thrown.</returns>
        ICommand CreateCommand(string name, INotifyPropertyChanged monitoredInstance);

        /// <summary>
        /// Gets a command permissions.
        /// </summary>
        /// <param name="name">The command name.</param>
        /// <returns>The command's permissions.</returns>
        IEnumerable<string> GetCommandPermissions(string name);
    }
}
