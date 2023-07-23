// -----------------------------------------------------------------------
// <copyright file="IAsyncCommand.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.UI.Core.Commands
{
    using System.Threading.Tasks;
    using System.Windows.Input;

    /// <summary>
    /// Asynchronous Command Implementation.
    /// </summary>
    /// <seealso cref="System.Windows.Input.ICommand" />
    public interface IAsyncCommand : INamedCommand
    {
        /// <summary>
        /// Gets a value indicating whether this instance is executing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is executing; otherwise, <c>false</c>.
        /// </value>
        bool IsExecuting { get; }

        /// <summary>
        /// Executes the command in asynchronous mode.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>The command task.</returns>
        Task ExecuteAsync(object parameter);
    }
}
