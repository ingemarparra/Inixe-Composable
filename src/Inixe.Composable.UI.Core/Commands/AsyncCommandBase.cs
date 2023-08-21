// -----------------------------------------------------------------------
// <copyright file="AsyncCommandBase.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.UI.Core.Commands
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Performs Async capable execution of commands.
    /// </summary>
    /// <seealso cref="Inixe.Composable.UI.Core.Commands.IAsyncCommand" />
    public abstract class AsyncCommandBase : IAsyncCommand
    {
        private bool isExecuting;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncCommandBase"/> class.
        /// </summary>
        protected AsyncCommandBase()
        {
            this.isExecuting = false;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Gets the command name.
        /// </summary>
        /// <value>
        /// The command name.
        /// </value>
        public abstract string Name { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is executing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is executing; otherwise, <c>false</c>.
        /// </value>
        public bool IsExecuting
        {
            get
            {
                return this.isExecuting;
            }

            private set
            {
                if (this.isExecuting != value)
                {
                    this.isExecuting = value;
                    this.OnCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        /// <returns>
        ///   <see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return !this.IsExecuting && this.CanExecuteWhenIdle(parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        public void Execute(object parameter)
        {
            if (this.CanExecute(parameter))
            {
                this.IsExecuting = true;

                this.ExecuteAsync(parameter)
                .ContinueWith(this.OnTaskCommandExecuted, TaskContinuationOptions.ExecuteSynchronously)
                .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Asynchronously executes the command.
        /// </summary>
        /// <param name="parameter">The command parameter.</param>
        /// <returns>The command execution task.</returns>
        public abstract Task ExecuteAsync(object parameter);

        /// <summary>
        /// Determines whether this instance can execute when idle the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>
        ///   <c>true</c> if this instance can execute when idle the specified parameter; otherwise, <c>false</c>.
        /// </returns>
        protected abstract bool CanExecuteWhenIdle(object parameter);

        /// <summary>
        /// Called when can the execute state changed.
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            var handler = this.CanExecuteChanged;

            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        private void OnTaskCommandExecuted(Task task)
        {
            this.IsExecuting = false;
        }
    }
}
