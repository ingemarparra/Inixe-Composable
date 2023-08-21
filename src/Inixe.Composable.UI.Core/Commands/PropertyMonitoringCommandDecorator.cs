// -----------------------------------------------------------------------
// <copyright file="PropertyMonitoringCommandDecorator.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.UI.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;

    /// <summary>
    /// Monitors an object in order to auto evaluate whether or not the command execution status changes.
    /// </summary>
    /// <seealso cref="System.Windows.Input.ICommand" />
    internal class PropertyMonitoringCommandDecorator : ICommand
    {
        private readonly INotifyPropertyChanged parent;
        private readonly ICommand decorated;

        private bool isExecutable;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMonitoringCommandDecorator"/> class.
        /// </summary>
        /// <param name="decorated">The decorated.</param>
        /// <param name="parent">The parent.</param>
        /// <exception cref="ArgumentNullException">
        /// decorated
        /// or
        /// parent.
        /// </exception>
        public PropertyMonitoringCommandDecorator(ICommand decorated, INotifyPropertyChanged parent)
        {
            this.decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
            this.decorated.CanExecuteChanged += this.Decorated_CanExecuteChanged;
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
            this.parent.PropertyChanged += this.Parent_PropertyChanged;
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
            this.isExecutable = this.decorated.CanExecute(parameter);
            return this.isExecutable;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
        public void Execute(object parameter)
        {
            this.decorated.Execute(parameter);
        }

        /// <summary>
        /// Called when the Command execution has changed.
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            var handler = this.CanExecuteChanged;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        private void Decorated_CanExecuteChanged(object sender, EventArgs e)
        {
            this.OnCanExecuteChanged();
        }

        private void Parent_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var lastAssessment = this.decorated.CanExecute(this.parent);
            if (lastAssessment != this.isExecutable)
            {
                this.isExecutable = lastAssessment;
                this.OnCanExecuteChanged();
            }
        }
    }
}
