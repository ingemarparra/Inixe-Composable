// -----------------------------------------------------------------------
// <copyright file="AutofacPluginContainerContext.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.Composition.PluginFramework
{
    using System;
    using System.Collections.Generic;
    using Autofac;
    using Inixe.Composable.UI.Core;

    /// <summary>
    /// Autofac implementation of <see cref="IPluginContainerContext"/>.
    /// </summary>
    /// <seealso cref="Inixe.Composable.UI.Core.IPluginContainerContext" />
    internal class AutofacPluginContainerContext : IPluginContainerContext, IViewModelLocator
    {
        private readonly ILifetimeScope componentContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacPluginContainerContext"/> class.
        /// </summary>
        /// <param name="componentContext">The component context.</param>
        /// <exception cref="ArgumentNullException">componentContext.</exception>
        internal AutofacPluginContainerContext(ILifetimeScope componentContext)
        {
            this.componentContext = componentContext ?? throw new ArgumentNullException(nameof(componentContext));
        }

        /// <inheritdoc/>
        public IPluginContainerContext CreateScope(string name)
        {
            return new AutofacPluginContainerContext(this.componentContext.BeginLifetimeScope(name));
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.componentContext.Dispose();
        }

        /// <inheritdoc/>
        public object ResolveService(Type type)
        {
            return this.componentContext.Resolve(type);
        }

        /// <inheritdoc/>
        public T ResolveService<T>()
        {
            return this.componentContext.Resolve<T>();
        }

        /// <inheritdoc/>
        public IList<T> ResolveServices<T>()
        {
            return this.componentContext.Resolve<IList<T>>();
        }

        /// <inheritdoc/>
        object IViewModelLocator.GetViewModel(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Invalid View Model name", nameof(name));
            }

            return this.componentContext.ResolveNamed<object>(name);
        }

        /// <inheritdoc/>
        bool IViewModelLocator.IsRegistered(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Invalid View Model name", nameof(name));
            }

            return this.componentContext.IsRegisteredWithName<object>(name);
        }
    }
}
