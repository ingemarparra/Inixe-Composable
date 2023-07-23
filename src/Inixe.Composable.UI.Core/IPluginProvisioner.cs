// -----------------------------------------------------------------------
// <copyright file="IPluginProvisioner.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.UI.Core
{
    using System.Windows;

    public interface IPluginProvisioner
    {
        IPluginProvisioner AttachResources(ResourceDictionary resourceDictionary);

        IPluginProvisioner RegisterTypes(IPluginContainerRegistry pluginContainerRegistry);

        void DetachResources(ResourceDictionary resourceDictionary);
    }
}
