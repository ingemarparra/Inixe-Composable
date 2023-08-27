// -----------------------------------------------------------------------
// <copyright file="WpfModule.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.Composition
{
    using System;
    using System.Linq;
    using System.Windows.Input;
    using Autofac;
    using Autofac.Core.Registration;
    using Inixe.Composable.UI.Core;
    using Inixe.Composable.UI.Core.Commands;

    /// <summary>
    /// Loads and registers the WPF related classes.
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    internal class WpfModule : Module
    {
        /// <summary>
        /// The application name.
        /// </summary>
        public const string ApplicationName = "Inixe.Composable.App";

        /// <summary>
        /// The application owner name.
        /// </summary>
        public const string OwnerName = "Inixe";

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
            var commandType = typeof(ICommand);

            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .Where(x => x.Name.EndsWith("ViewModel") && !x.Name.StartsWith("Null"))
                .Named(t => t.Name, typeof(object))
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterAssemblyTypes(this.ThisAssembly)
                .Where(x => commandType.IsAssignableFrom(x) && !x.Name.StartsWith("Null"))
                .Named(t => t.Name, typeof(object))
                .AsImplementedInterfaces()
                .AsSelf();

            var mainViewModelType = typeof(Inixe.Composable.App.ViewModels.MainViewModel);
            builder.RegisterType(mainViewModelType)
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance()
                .Named(mainViewModelType.Name, typeof(object));

            builder.RegisterType<CommandFlyweightFactory>()
                .AsImplementedInterfaces();

            builder.Register<IViewModelLocator>(ctx => ctx.Resolve<App>());
            builder.Register(ResolveAppHost);

            builder.RegisterBuildCallback(AttachToViewModelFactory);

            base.Load(builder);
        }

        private static IAppHost ResolveAppHost(IComponentContext componentContext)
        {
            var viewModelLocator = componentContext.Resolve<IViewModelLocator>();
            return (IAppHost)viewModelLocator.GetViewModel(nameof(ViewModels.MainViewModel));
        }

        private static void AttachToViewModelFactory(ILifetimeScope componentContext)
        {
            var app = componentContext.Resolve<App>();

            var allFactories = app.Resources.MergedDictionaries.SelectMany(x => x.Values.OfType<ViewModelFlyweightFactory>());
            var factory = allFactories.FirstOrDefault();

            var locator = componentContext.Resolve<IViewModelLocator>();
            factory?.Initialize(locator);
        }
    }
}
