// -----------------------------------------------------------------------
// <copyright file="CommandFactoryRegistry.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.Composition.PluginFramework
{
    using System;
    using System.Collections.ObjectModel;
    using Inixe.Composable.UI.Core.Commands;

    /// <summary>
    /// Registry for all command factories.
    /// </summary>
    internal class CommandFactoryRegistry : KeyedCollection<string, CommandFactoryInstance>
    {
        /// <inheritdoc/>
        protected override string GetKeyForItem(CommandFactoryInstance item)
        {
            return item.Name;
        }
    }

    internal record CommandFactoryInstance(string Name, ICommandFactory CommandFactory);
}
