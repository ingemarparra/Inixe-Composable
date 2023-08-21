// -----------------------------------------------------------------------
// <copyright file="RelayCommand.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.UI.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    /// <summary>
    /// Command Relay Implementation.
    /// </summary>
    /// <seealso cref="System.Windows.Input.ICommand" />
    public class RelayCommand : ICommand
    {
        private readonly Action simpleAction;
        private readonly Func<bool> assessCanExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="simpleAction">The action to execute by this command.</param>
        /// <param name="assessCanExecute">The assessment for whether or not the command can be executed..</param>
        /// <exception cref="ArgumentNullException">simpleAction.</exception>
        public RelayCommand(Action simpleAction, Func<bool> assessCanExecute)
        {
            Func<bool> defaultAssesment = () => true;

            this.simpleAction = simpleAction ?? throw new ArgumentNullException(nameof(simpleAction));
            this.assessCanExecute = assessCanExecute ?? defaultAssesment;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="simpleAction">The simple action.</param>
        public RelayCommand(Action simpleAction)
            : this(simpleAction, null)
        {
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        /// <returns>
        ///   <see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            var result = this.assessCanExecute();
            return result;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        public void Execute(object parameter)
        {
            if (this.CanExecute(parameter))
            {
                this.simpleAction();
            }
        }

        /// <summary>
        /// Called when can execute has changed.
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            var handler = this.CanExecuteChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }
    }
}
