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

    /// <summary>
    /// Application Main View Model. This also acts as the implementation of <see cref="IAppHost"/>.
    /// </summary>
    /// <seealso cref="Inixe.Composable.UI.Core.IAppHost" />
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
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

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <remarks>This constructor is only used by design time instances.</remarks>
        public MainViewModel()
            : this(new NullCommandFactory(), new NullConfiguration(), new NullLogger<MainViewModel>(), new PluginRegistry())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        /// <param name="commandFactory">The command factory.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="pluginRegistry">The plugin registry.</param>
        /// <exception cref="ArgumentNullException">When
        /// logger
        /// or
        /// configuration
        /// or
        /// pluginRegistry
        /// or
        /// commandFactory
        /// is <c>null</c>.
        /// </exception>
        /// <remarks>This is the actual view model constructor during runtime.</remarks>
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
            this.LoadPlugins();

            var menus = new ObservableCollection<MenuItemViewModel>();
            menus.Add(new MainMenuItemViewModel(this.commandFactory));
            menus.Add(new PluginsListMenuItemViewModel(this.pluginRegistry, this.commandFactory));

            return menus;
        }

        private void LoadPlugins()
        {
            if (this.RefreshPluginRegistryCommand is IAsyncCommand ac)
            {
                ac.ExecuteAsync(this.pluginRegistry);
            }
            else
            {
                this.RefreshPluginRegistryCommand.Execute(this.pluginRegistry);
            }
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
