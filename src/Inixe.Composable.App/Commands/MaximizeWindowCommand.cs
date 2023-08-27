// -----------------------------------------------------------------------
// <copyright file="MaximizeWindowCommand.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.Commands
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Command implementation that maximizes a window.
    /// </summary>
    /// <seealso cref="System.Windows.Input.ICommand" />
    internal class MaximizeWindowCommand : ICommand
    {
        /// <summary>
        /// Command instance used during window initialization.
        /// </summary>
        public static readonly MaximizeWindowCommand Command = new MaximizeWindowCommand();

        /// <summary>
        /// Initializes a new instance of the <see cref="MaximizeWindowCommand"/> class.
        /// </summary>
        public MaximizeWindowCommand()
        {
        }

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute(object parameter)
        {
            return parameter is Window w ? w.WindowState != WindowState.Maximized : true;
        }

        /// <inheritdoc/>
        public void Execute(object parameter)
        {
            if (parameter is Window w)
            {
                SystemCommands.MaximizeWindow(w);
            }
        }
    }
}
