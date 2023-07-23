// -----------------------------------------------------------------------
// <copyright file="MenuItemViewModel.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.UI.Core
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows.Input;
    using Inixe.Composable.UI.Core.Commands;

    public class MenuItemViewModel : INotifyPropertyChanged
    {
        private readonly string caption;
        private readonly string hotKey;
        private readonly ICommand command;
        private readonly IList<MenuItemViewModel> subMenus;
        private readonly bool isCheckable;

        private bool isChecked;

        protected MenuItemViewModel(string caption)
            : this(caption, string.Empty, null)
        {
        }

        protected MenuItemViewModel(string caption, ICommand command)
            : this(caption, string.Empty, command)
        {
        }

        protected MenuItemViewModel(string caption, IList<MenuItemViewModel> subMenus)
            : this(caption, string.Empty, null, ValidateSubMenus(subMenus), false, false)
        {
        }

        protected MenuItemViewModel(string caption, string hotKey, ICommand command)
            : this(caption, hotKey, command, false, false)
        {
        }

        protected MenuItemViewModel(string caption, string hotKey, ICommand command, bool isCheckable, bool isChecked)
            : this(caption, hotKey, command, null, isCheckable, isChecked)
        {
        }

        private MenuItemViewModel(string caption, string hotKey, ICommand command, IList<MenuItemViewModel> subMenus, bool isCheckable, bool isChecked)
        {
            if (string.IsNullOrWhiteSpace(caption))
            {
                throw new ArgumentException("Invalid menu name", nameof(caption));
            }

            this.isCheckable = isCheckable;
            this.caption = caption;
            this.hotKey = hotKey;
            this.command = command;
            this.subMenus = subMenus;
            this.isChecked = isChecked;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public object Parameter { get; set; }

        public bool IsChecked
        {
            get
            {
                return this.isChecked;
            }

            set
            {
                if (this.isChecked != value)
                {
                    this.isChecked = value;
                    this.OnPropertyChanged(nameof(this.IsChecked));
                }
            }
        }

        public bool IsCheckable
        {
            get
            {
                return this.isCheckable;
            }
        }

        public string Caption
        {
            get
            {
                return this.caption;
            }
        }

        public string HotKey
        {
            get
            {
                return this.hotKey;
            }
        }

        public ICommand Command
        {
            get
            {
                return this.command;
            }
        }

        public IList<MenuItemViewModel> SubMenus
        {
            get
            {
                return this.subMenus;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("Invalid property name", nameof(propertyName));
            }

            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private static IList<MenuItemViewModel> ValidateSubMenus(IList<MenuItemViewModel> items)
        {
            return items ?? throw new ArgumentNullException(nameof(items));
        }
    }
}
