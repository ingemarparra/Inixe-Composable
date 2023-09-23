// -----------------------------------------------------------------------
// <copyright file="HealthPluginProvisioner.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.Health
{
    using System;
    using System.Windows;
    using System.Windows.Markup;
    using System.Xml;
    using Inixe.Composable.Health.ViewModels;
    using Inixe.Composable.UI.Core;

    public class HealthPluginProvisioner : IPluginProvisioner
    {
        public IPluginProvisioner AttachResources(ResourceDictionary resourceDictionary)
        {
            var t = typeof(ViewModels.HealthViewModel);
            var rd = new ResourceDictionary();
            rd.Source = new Uri("pack://application:,,,/Inixe.Composable.Health;component/Resources/DataTemplates.xaml");

            resourceDictionary.MergedDictionaries.Add(rd);
            return this;
        }

        public void DetachResources(ResourceDictionary resourceDictionary)
        {
        }

        public IPluginProvisioner RegisterTypes(IPluginContainerRegistry pluginContainerRegistry)
        {
            pluginContainerRegistry.RegisterType<HealthPlugin, IPlugin>();
            pluginContainerRegistry.RegisterType<HealthViewModel>();

            return this;
        }
    }
}
