// -----------------------------------------------------------------------
// <copyright file="TabItemViewModelBase.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.UI.Core
{
    using System;
    using System.ComponentModel;

    public class TabItemViewModelBase : INotifyPropertyChanged
    {
        private string caption;

        public TabItemViewModelBase(string caption)
        {
            this.caption = caption;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Caption
        {
            get
            {
                return this.caption;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
