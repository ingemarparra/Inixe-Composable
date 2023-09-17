// -----------------------------------------------------------------------
// <copyright file="PluginsListMenuItemViewModel.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.ViewModels
{
    using System;
    using Inixe.Composable.App.Composition.PluginFramework;
    using Inixe.Composable.UI.Core;
    using Inixe.Composable.UI.Core.Commands;

    internal class PluginsListMenuItemViewModel : MenuItemViewModel
    {
        private const string MenuCaption = "Plugins";

        public PluginsListMenuItemViewModel(PluginRegistry plugins, ICommandFactory commandFactory)
            : base(MenuCaption, PluginsMenuItemViewModelCollection.Create(plugins, commandFactory))
        {
        }
    }
}
