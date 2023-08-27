// -----------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;

    using System.Windows.Input;
    using Inixe.Composable.App.Commands;
    using Inixe.Composable.App.Composition.PluginFramework;
    using Inixe.Composable.App.NullObjects;
    using Inixe.Composable.UI.Core;
    using Inixe.Composable.UI.Core.Commands;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class MainViewModel : IAppHost, INotifyPropertyChanged
    {
        private readonly ObservableCollection<TabItemViewModelBase> tabs;
        private readonly Lazy<ObservableCollection<MenuItemViewModel>> menuItemsLazy;
        private readonly StatusBarViewModel statusBarViewModel;
        private readonly ILogger logger;
        private readonly IConfiguration configuration;
        private readonly ICommandFactory commandFactory;
        private readonly PluginRegistry pluginRegistry;
        private readonly ICommand refreshPluginRegistryCommand;

        public MainViewModel()
            : this(new NullCommandFactory(), new NullConfiguration(), new NullLogger<MainViewModel>(), new PluginRegistry())
        {
        }

        public MainViewModel(ICommandFactory commandFactory, IConfiguration configuration, ILogger<MainViewModel> logger, PluginRegistry pluginRegistry)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.pluginRegistry = pluginRegistry ?? throw new ArgumentNullException(nameof(pluginRegistry));
            this.commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));

            this.tabs = new ObservableCollection<TabItemViewModelBase>();
            this.menuItemsLazy = new Lazy<ObservableCollection<MenuItemViewModel>>(this.CreateInitialMenus);
            this.statusBarViewModel = new StatusBarViewModel();

            this.refreshPluginRegistryCommand = this.commandFactory.CreateCommand(Inixe.Composable.App.Commands.RefreshPluginRegistryCommand.CommandName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand RefreshPluginRegistryCommand
        {
            get
            {
                return this.refreshPluginRegistryCommand;
            }
        }

        public IList<TabItemViewModelBase> Tabs
        {
            get
            {
                return this.tabs;
            }
        }

        public IList<MenuItemViewModel> MenuItems
        {
            get
            {
                return this.menuItemsLazy.Value;
            }
        }

        public StatusBarViewModel StatusBar
        {
            get
            {
                return this.statusBarViewModel;
            }
        }

        public ILogger Logger
        {
            get
            {
                return this.logger;
            }
        }

        public IConfiguration Configuration
        {
            get
            {
                return this.configuration;
            }
        }

        private ObservableCollection<MenuItemViewModel> CreateInitialMenus()
        {
            var menus = new ObservableCollection<MenuItemViewModel>();
            menus.Add(new MainMenuItemViewModel(this.commandFactory));

            return menus;
        }

        private class MainMenuItemViewModel : MenuItemViewModel
        {
            public MainMenuItemViewModel(ICommandFactory commandFactory)
                : base("File", CreateFileMenuContents(commandFactory))
            {
            }

            private MainMenuItemViewModel(string caption, ICommand command)
                : base(caption, command)
            {
            }

            private static IList<MenuItemViewModel> CreateFileMenuContents(ICommandFactory commandFactory)
            {
                var items = new List<MenuItemViewModel>();
                items.Add(new MainMenuItemViewModel("Exit", commandFactory.CreateCommand(ExitApplicationCommand.CommandName)));
                return items;
            }
        }
    }
}
