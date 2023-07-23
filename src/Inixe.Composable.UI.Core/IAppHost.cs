// -----------------------------------------------------------------------
// <copyright file="IAppHost.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.UI.Core
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public interface IAppHost
    {
        IList<TabItemViewModelBase> Tabs { get; }

        IList<MenuItemViewModel> MenuItems { get; }

        StatusBarViewModel StatusBar { get; }

        ILogger Logger { get; }

        IConfiguration Configuration { get; }

        ICommand RefreshPluginRegistryCommand { get; }
    }
}
