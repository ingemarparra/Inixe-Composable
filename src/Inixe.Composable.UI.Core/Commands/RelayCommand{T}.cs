// -----------------------------------------------------------------------
// <copyright file="RelayCommand{T}.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.UI.Core.Commands
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Generic Relay Command.
    /// </summary>
    /// <typeparam name="T">The command Type.</typeparam>
    /// <seealso cref="System.Windows.Input.ICommand" />
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> simpleAction;
        private readonly Predicate<T> assessCanExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="simpleAction">The action to execute by this command.</param>
        /// <param name="assessCanExecute">The assessment for whether or not the command can be executed..</param>
        /// <exception cref="ArgumentNullException">simpleAction.</exception>
        public RelayCommand(Action<T> simpleAction, Predicate<T> assessCanExecute)
        {
            Predicate<T> defaultAssesment = x => true;

            this.simpleAction = simpleAction ?? throw new ArgumentNullException(nameof(simpleAction));
            this.assessCanExecute = assessCanExecute ?? defaultAssesment;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="simpleAction">The simple action.</param>
        public RelayCommand(Action<T> simpleAction)
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
            var result = true;
            if (this.assessCanExecute != null && parameter is T t)
            {
                result = this.assessCanExecute(t);
            }

            return result;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        public void Execute(object parameter)
        {
            if (this.CanExecute(parameter) && parameter is T t)
            {
                this.simpleAction(t);
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
