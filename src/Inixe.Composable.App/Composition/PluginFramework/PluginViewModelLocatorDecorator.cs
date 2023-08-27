// -----------------------------------------------------------------------
// <copyright file="PluginViewModelLocatorDecorator.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.Composition.PluginFramework
{
    using System;
    using System.Linq;
    using Inixe.Composable.UI.Core;

    /// <summary>
    /// Decorator for <see cref="IViewModelLocator"/> that enables plugin instances to benefit from the design time capabilities.
    /// </summary>
    /// <seealso cref="Inixe.Composable.UI.Core.IViewModelLocator" />
    /// <remarks>
    /// Each plugin is responsible of implementing it's own copy of <see cref="IViewModelLocator"/>. Once registered in the plugin's context this will be picked up by the decorator
    /// and will serve the corresponding ViewModel to the plugin. It's also possible to reuse view models from the root application.
    /// </remarks>
    internal class PluginViewModelLocatorDecorator : IViewModelLocator
    {
        private readonly IViewModelLocator decorated;
        private readonly PluginRegistry registry;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginViewModelLocatorDecorator"/> class.
        /// </summary>
        /// <param name="decorated">The decorated.</param>
        /// <param name="registry">The registry.</param>
        /// <exception cref="ArgumentNullException">
        /// decorated
        /// or
        /// registry is <c>null</c>.
        /// </exception>
        public PluginViewModelLocatorDecorator(IViewModelLocator decorated, PluginRegistry registry)
        {
            this.decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
            this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        /// <inheritdoc/>
        public object GetViewModel(string name)
        {
            object viewModel;

            if (!this.decorated.IsRegistered(name))
            {
                viewModel = this.registry.Select(x => x.ContainerContext)
                    .Cast<IViewModelLocator>()
                    .Single(x => x.IsRegistered(name))
                    .GetViewModel(name);
            }
            else
            {
                viewModel = this.decorated.GetViewModel(name);
            }

            return viewModel;
        }

        /// <inheritdoc/>
        public bool IsRegistered(string name)
        {
            var isRegisteredAtRoot = this.decorated.IsRegistered(name);
            var isRegisteredAtPlugin = this.registry.Select(x => x.ContainerContext)
                    .Cast<IViewModelLocator>()
                    .Any(x => x.IsRegistered(name));

            return isRegisteredAtPlugin || isRegisteredAtRoot;
        }
    }
}
