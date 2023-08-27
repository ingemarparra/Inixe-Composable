// -----------------------------------------------------------------------
// <copyright file="RestoreWindowCommand.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.Commands
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Command implementation for restoring a window.
    /// </summary>
    /// <seealso cref="System.Windows.Input.ICommand" />
    internal class RestoreWindowCommand : ICommand
    {
        /// <summary>
        /// Command instance used during window initialization.
        /// </summary>
        public static readonly RestoreWindowCommand Command = new RestoreWindowCommand();

        /// <summary>
        /// Initializes a new instance of the <see cref="RestoreWindowCommand"/> class.
        /// </summary>
        public RestoreWindowCommand()
        {
        }

        /// <inheritdoc/>
        public event EventHandler CanExecuteChanged;

        /// <inheritdoc/>
        public bool CanExecute(object parameter)
        {
            return parameter is Window w ? w.WindowState != WindowState.Normal : true;
        }

        /// <inheritdoc/>
        public void Execute(object parameter)
        {
            if (parameter is Window w)
            {
                SystemCommands.RestoreWindow(w);
            }
        }
    }
}
