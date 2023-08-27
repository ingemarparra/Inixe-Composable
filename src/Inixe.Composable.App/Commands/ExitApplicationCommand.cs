// -----------------------------------------------------------------------
// <copyright file="ExitApplicationCommand.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.Commands
{
    using System;
    using Inixe.Composable.UI.Core.Commands;

    internal class ExitApplicationCommand : INamedCommand
    {
        public const string CommandName = "ExitApplication";

        private readonly App app;

        public ExitApplicationCommand(App app)
        {
            this.app = app ?? throw new ArgumentNullException(nameof(app));
        }

        public event EventHandler CanExecuteChanged;

        public string Name
        {
            get
            {
                return CommandName;
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.app.Shutdown();
        }
    }
}
