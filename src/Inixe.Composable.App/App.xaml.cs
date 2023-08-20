// -----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App
{
    using System;
    using System.Linq;
    using System.Windows;
    using Autofac;
    using Inixe.Composable.App.Composition;
    using Inixe.Composable.UI.Core;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application, IViewModelLocator
    {
        private IContainer container;

        /// <inheritdoc/>
        bool IViewModelLocator.IsRegistered(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Invalid View Model name", nameof(name));
            }

            return this.container.IsRegisteredWithName<object>(name);
        }

        /// <inheritdoc/>
        object IViewModelLocator.GetViewModel(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Invalid View Model name", nameof(name));
            }

            return this.container.ResolveNamed<object>(name);
        }

        /// <inheritdoc/>
        protected override void OnStartup(StartupEventArgs e)
        {
            this.InitializeContainer(e.Args);

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            this.container.Dispose();
            base.OnExit(e);
        }

        private void InitializeContainer(string[] args)
        {
            var builder = new ContainerBuilder();
            var applicationType = typeof(App);

            builder.RegisterInstance(this);
            builder.RegisterModule(new BaseServicesModule(args));
            builder.RegisterAssemblyModules(applicationType.Assembly);

            this.container = builder.Build();
        }
    }
}
