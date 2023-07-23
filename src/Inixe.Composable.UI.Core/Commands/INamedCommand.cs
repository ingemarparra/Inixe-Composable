// -----------------------------------------------------------------------
// <copyright file="INamedCommand.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.UI.Core.Commands
{
    using System.Windows.Input;

    /// <summary>
    /// Extends the <see cref="ICommand"/> with a name for identification.
    /// </summary>
    /// <seealso cref="System.Windows.Input.ICommand" />
    public interface INamedCommand : ICommand
    {
        /// <summary>
        /// Gets the command name.
        /// </summary>
        /// <value>
        /// The command name.
        /// </value>
        string Name { get; }
    }
}
