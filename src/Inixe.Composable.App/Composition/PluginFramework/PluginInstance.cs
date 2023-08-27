// -----------------------------------------------------------------------
// <copyright file="PluginInstance.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.Composition.PluginFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Loader;
    using Autofac;
    using Autofac.Core.Lifetime;
    using Inixe.Composable.UI.Core;
    using Inixe.Composable.UI.Core.Commands;

    /// <summary>
    /// Represents the root and loading context of a plugin.
    /// </summary>
    /// <seealso cref="System.Runtime.Loader.AssemblyLoadContext" />
    /// <seealso cref="System.IDisposable" />
    /// <remarks>
    /// <para>
    /// All plugins are loaded into the Application by using their own context. This means that they are isolated from the main application dependencies.
    /// On demand loading is possible, although unloading is not guaranteed.
    /// </para>
    /// <para>
    /// Loading a plugin does not execute any code other than the plugin types registration into the DI IoC and initial plugin tasks that may happen at construction of the provided <see cref="IPlugin"/> implementation.
    /// Although it's not recommended to do any initialization steps during that phase.
    /// </para>
    /// <para>
    /// Starting a plugin is the actual work for making initialization tasks register objects and make them available in the IU or the Application host model.
    /// At stop a plugin must take care and clean all of the provided objects into the Application host model.
    /// </para>
    /// </remarks>
    public class PluginInstance : AssemblyLoadContext, IDisposable
    {
        private readonly IPluginManifest manifest;
        private readonly Func<ILifetimeScope> lifetimeScopeFactory;
        private IAppHost appHost;
        private App appInstance;
        private AutofacPluginContainerContext context;
        private IPlugin plugin;
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginInstance"/> class.
        /// </summary>
        /// <param name="manifest">The manifest.</param>
        /// <param name="lifetimeScopeFactory">The lifetime scope factory.</param>
        /// <exception cref="ArgumentNullException">
        /// manifest
        /// or
        /// manifest is <c>null</c>.
        /// </exception>
        public PluginInstance(IPluginManifest manifest, Func<ILifetimeScope> lifetimeScopeFactory)
            : base(GetIdFromManifest(manifest), isCollectible: true)
        {
            this.manifest = manifest ?? throw new ArgumentNullException(nameof(manifest));
            this.lifetimeScopeFactory = lifetimeScopeFactory ?? throw new ArgumentNullException(nameof(manifest));

            this.Unloading += this.PluginInstance_Unloading;
            this.disposedValue = false;
            this.plugin = null;
            this.context = null;
            this.appHost = null;
            this.appInstance = null;
        }

        /// <summary>
        /// This delegate is used for constructing the plugin instance from a <see cref="IPluginManifest"/>.
        /// It will be automatically picked up by Autofac and will use the default constructor filling the remaining parameters.
        /// </summary>
        /// <param name="manifest">The manifest.</param>
        /// <returns>A valid plugin instance.</returns>
        public delegate PluginInstance PluginInstanceFactory(IPluginManifest manifest);

        /// <summary>
        /// Gets the plugin manifest from which this instance is loaded.
        /// </summary>
        /// <value>
        /// The manifest.
        /// </value>
        public IPluginManifest Manifest
        {
            get
            {
                return this.manifest;
            }
        }

        /// <summary>
        /// Gets the plugin.
        /// </summary>
        /// <value>
        /// The plugin.
        /// </value>
        public IPlugin Plugin
        {
            get
            {
                return this.plugin ?? new NullPlugin(this.manifest);
            }
        }

        /// <summary>
        /// Gets the container context used by this plugin.
        /// </summary>
        /// <value>
        /// The container context used by this plugin.
        /// </value>
        public IPluginContainerContext ContainerContext
        {
            get
            {
                return this.IsLoaded ? this.context : new NullPluginContainerContext();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this plugin instance is loaded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this plugin instance is loaded; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoaded
        {
            get
            {
                return this.plugin != null && !this.disposedValue;
            }
        }

        private ILifetimeScope RootLifetimeScope
        {
            get
            {
                return this.lifetimeScopeFactory();
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <exception cref="InvalidOperationException">The plugin is not loaded and not ready for use.</exception>
        public void Start()
        {
            if (!this.IsLoaded)
            {
                throw new InvalidOperationException("The plugin is not loaded and not ready for use");
            }

            if (!this.plugin.IsRunning)
            {
                this.plugin.Start(this.appHost);
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        /// <exception cref="InvalidOperationException">The plugin is not loaded and not ready for use.</exception>
        /// <remarks>All resources added or modified in the application need to be removed.</remarks>
        public void Stop()
        {
            if (!this.IsLoaded)
            {
                throw new InvalidOperationException("The plugin is not loaded and not ready for use");
            }

            if (this.plugin.IsRunning)
            {
                this.plugin.Stop(this.appHost);
            }
        }

        /// <summary>
        /// Loads this instance into the application host.
        /// </summary>
        /// <exception cref="TypeLoadException">When initial plugin resolution fails.</exception>
        public void Load()
        {
            if (!this.IsLoaded)
            {
                var rootScope = this.RootLifetimeScope;

                this.appInstance = rootScope.Resolve<App>();
                this.appHost = rootScope.Resolve<IAppHost>();

                var scope = rootScope.BeginLoadContextLifetimeScope(this, this.RegisterPluginTypes);

                this.context = new AutofacPluginContainerContext(scope);
                scope.CurrentScopeEnding += this.Scope_CurrentScopeEnding;

                this.plugin = scope.ResolveOptional<IPlugin>();
                if (this.plugin == null)
                {
                    throw new TypeLoadException("Could not load plugin");
                }
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            const bool Disposing = true;

            this.Dispose(Disposing);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.context?.Dispose();
                    this.Unload();
                }

                this.disposedValue = true;
            }
        }

        private static string GetIdFromManifest(IPluginManifest manifest)
        {
            var id = manifest?.Id ?? Guid.Empty;
            return id.ToString();
        }

        private static bool IsPluginProviderType(Type type, IPluginManifest manifest)
        {
            var pluginBaseType = typeof(IPluginProvisioner);
            return type.FullName == manifest.PluginFactoryTypeName && pluginBaseType.IsAssignableFrom(type);
        }

        private void PluginInstance_Unloading(AssemblyLoadContext loadContext)
        {
            this.context?.Dispose();
        }

        private void Scope_CurrentScopeEnding(object sender, LifetimeScopeEndingEventArgs e)
        {
            this.plugin = null;
            this.context = null;
        }

        private void RegisterPluginTypes(ContainerBuilder builder)
        {
            var providerType = this.LoadProviderType();

            if (providerType != null)
            {
                var provider = Activator.CreateInstance(providerType) as IPluginProvisioner;
                if (provider != null)
                {
                    this.RegisterContextBasedInstances(builder, provider);
                    this.ProvisionPlugin(builder, provider);
                }
            }
        }

        private void ProvisionPlugin(ContainerBuilder builder, IPluginProvisioner provider)
        {
            var containerRegistry = new AutofacPluginContainerRegistry(builder);

            provider.RegisterTypes(containerRegistry)
                .AttachResources(this.appInstance.Resources);
        }

        private void RegisterContextBasedInstances(ContainerBuilder builder, IPluginProvisioner provider)
        {
            builder.RegisterInstance(provider)
                .Keyed<IPluginProvisioner>(this.manifest.Id);

            builder.Register(ctx => new AutofacPluginContainerContext(ctx.Resolve<ILifetimeScope>()))
                .As<IPluginContainerContext>()
                .Keyed<IPluginContainerContext>(this.manifest.Id);

            builder.RegisterBuildCallback(ctx => this.ImportCommands(ctx));
        }

        private void ImportCommands(ILifetimeScope componentContext)
        {
            if (componentContext.TryResolveKeyed<ICommandFactory>(this.manifest.Id, out var commandFactory))
            {
                var registry = componentContext.Resolve<CommandFactoryRegistry>();
                var instance = new CommandFactoryInstance(this.manifest.Name, commandFactory);

                registry.Add(instance);
            }
        }

        private Type LoadProviderType()
        {
            var asm = this.LoadFromAssemblyPath(this.manifest.SourcePath);
            var types = asm.GetExportedTypes();
            var providerType = asm.GetExportedTypes()
                .SingleOrDefault(x => IsPluginProviderType(x, this.manifest));

            return providerType;
        }

        private class NullPluginContainerContext : IPluginContainerContext
        {
            public IPluginContainerContext CreateScope(string name)
            {
                return this;
            }

            public void Dispose()
            {
                // No action Null Object pattern
            }

            public T ResolveService<T>()
            {
                return default(T);
            }

            public object ResolveService(Type type)
            {
                return null;
            }

            public IList<T> ResolveServices<T>()
            {
                return new List<T>();
            }
        }

        private class NullPlugin : IPlugin
        {
            private readonly IPluginManifest pluginManifest;

            public NullPlugin(IPluginManifest pluginManifest)
            {
                this.pluginManifest = pluginManifest;
            }

            public bool IsRunning
            {
                get
                {
                    return false;
                }
            }

            public string Name
            {
                get
                {
                    return this.pluginManifest.Name;
                }
            }

            public void Start(IAppHost host)
            {
                // No action Null Object pattern
            }

            public void Stop(IAppHost host)
            {
                // No action Null Object pattern
            }
        }
    }
}
