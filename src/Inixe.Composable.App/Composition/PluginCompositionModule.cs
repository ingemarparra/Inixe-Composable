// -----------------------------------------------------------------------
// <copyright file="PluginCompositionModule.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.Composition
{
    using System;
    using Autofac;
    using Inixe.Composable.App.Composition.PluginFramework;
    using Inixe.Composable.UI.Core;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Loads and registers the WPF related classes.
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    internal class PluginCompositionModule : Module
    {
        /// <summary>
        /// Override to add registrations to the container.
        /// </summary>
        /// <param name="builder">The builder through which components can be
        /// registered.</param>
        /// <remarks>
        /// Note that the ContainerBuilder parameter is unique to this module.
        /// </remarks>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PluginRegistry>()
                .SingleInstance();

            builder.RegisterType<CommandFactoryRegistry>()
                .SingleInstance();

            builder.RegisterType<RoutingCommandFactory>()
                .AsImplementedInterfaces();

            builder.RegisterType<AsyncDispatcher>()
                .SingleInstance()
                .AsImplementedInterfaces();

            builder.Register(this.CreateFilePluginSource)
                .AsImplementedInterfaces();

            builder.RegisterType<PluginInstance>();

            builder.RegisterDecorator<PluginViewModelLocatorDecorator, IViewModelLocator>();
            builder.RegisterComposite<CompositePluginSource, IPluginSource>();

            base.Load(builder);
        }

        private IPluginSource CreateFilePluginSource(IComponentContext context)
        {
            try
            {
                var config = context.Resolve<IConfiguration>();
                var pluginPath = config.GetValue(Constants.LocalPluginsDirectory, Constants.DefaultPluginDirectory);

                return FileSystemPluginSource.Create(pluginPath);
            }
            catch (ArgumentException)
            {
                return new NullPluginSource();
            }
        }
    }
}
