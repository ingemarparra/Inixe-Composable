// -----------------------------------------------------------------------
// <copyright file="HealthPlugin.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.Health
{
    using System.Windows;
    using Inixe.Composable.UI.Core;

    public class HealthPlugin : IPlugin
    {
        private bool isRunning;

        public HealthPlugin()
        {
            this.isRunning = false;
        }

        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
        }

        public string Name
        {
            get
            {
                return Constants.PluginName;
            }
        }

        public void Start(IAppHost host)
        {
            host.Tabs.Add(new ViewModels.HealthViewModel());
            this.isRunning = true;
        }

        public void Stop(IAppHost host)
        {
            this.isRunning = false;
        }

        private class HealthMenuItemViewModel : MenuItemViewModel
        {
            public HealthMenuItemViewModel(string caption)
                : base(caption)
            {
            }
        }
    }
}
