// -----------------------------------------------------------------------
// <copyright file="PluginsMenuItemViewModelCollection.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows.Input;
    using Inixe.Composable.App.Composition.PluginFramework;
    using Inixe.Composable.UI.Core;
    using Inixe.Composable.UI.Core.Commands;

    internal class PluginsMenuItemViewModelCollection : ObservableCollection<MenuItemViewModel>
    {
        private readonly PluginRegistry plugins;
        private readonly ICommandFactory commandFactory;

        private PluginsMenuItemViewModelCollection(PluginRegistry plugins, ICommandFactory commandFactory)
        {
            this.commandFactory = commandFactory;
            this.plugins = plugins;

            this.plugins.CollectionChanged += this.PluginRegistry_CollectionChanged;
        }

        public static IList<MenuItemViewModel> Create(PluginRegistry plugins, ICommandFactory commandFactory)
        {
            ArgumentNullException.ThrowIfNull(plugins, nameof(plugins));
            ArgumentNullException.ThrowIfNull(commandFactory, nameof(commandFactory));

            var list = new PluginsMenuItemViewModelCollection(plugins, commandFactory);
            foreach (var instance in plugins)
            {
                list.AddMenu(instance);
            }

            return list;
        }

        private void PluginRegistry_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                e.NewItems.Cast<PluginInstance>()
                    .ToList()
                    .ForEach(this.AddMenu);
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                e.OldItems.Cast<PluginInstance>()
                    .ToList()
                    .ForEach(this.RemoveMenu);
            }
        }

        private void RemoveMenu(PluginInstance instance)
        {
            var menu = this.OfType<PluginMenuViewModel>()
                .SingleOrDefault(x => x.Id == instance.Manifest.Id);

            if (menu != null)
            {
                this.Remove(menu);
            }
        }

        private void AddMenu(PluginInstance instance)
        {
            var vm = this.CreateNewItem(instance);
            this.Add(vm);
        }

        private MenuItemViewModel CreateNewItem(PluginInstance instance)
        {
            var command = this.commandFactory.CreateCommand(Commands.TogglePluginCommand.CommandName);
            return new PluginMenuViewModel(instance.Manifest, command, instance.Manifest);
        }

        private class PluginMenuViewModel : MenuItemViewModel
        {
            public PluginMenuViewModel(IPluginManifest manifest, ICommand command, object parameter)
                : base(manifest.Name, command)
            {
                this.Id = manifest.Id;
                this.Parameter = parameter;
            }

            public Guid Id { get; }
        }
    }
}
