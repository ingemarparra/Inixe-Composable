// -----------------------------------------------------------------------
// <copyright file="MinimizeWindowCommand.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.Commands
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Command implementation for minimizing a window.
    /// </summary>
    /// <seealso cref="System.Windows.Input.ICommand" />
    internal class MinimizeWindowCommand : ICommand
    {
        /// <summary>
        /// Command instance used during window initialization.
        /// </summary>
        public static readonly MinimizeWindowCommand Command = new MinimizeWindowCommand();

        /// <summary>
        /// Initializes a new instance of the <see cref="MinimizeWindowCommand"/> class.
        /// </summary>
        public MinimizeWindowCommand()
        {
        }

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute(object parameter)
        {
            return parameter is Window w ? w.WindowState != WindowState.Minimized : true;
        }

        /// <inheritdoc/>
        public void Execute(object parameter)
        {
            if (parameter is Window w)
            {
                SystemCommands.MinimizeWindow(w);
            }
        }
    }
}
