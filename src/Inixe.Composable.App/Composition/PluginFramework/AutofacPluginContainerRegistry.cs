// -----------------------------------------------------------------------
// <copyright file="AutofacPluginContainerRegistry.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.Composition.PluginFramework
{
    using System;
    using Autofac;
    using Autofac.Builder;
    using Inixe.Composable.UI.Core;

    /// <summary>
    /// Autofac implementation of <see cref="IPluginContainerRegistry"/>.
    /// </summary>
    /// <seealso cref="Inixe.Composable.UI.Core.IPluginContainerRegistry" />
    internal class AutofacPluginContainerRegistry : IPluginContainerRegistry
    {
        private readonly ContainerBuilder containerBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutofacPluginContainerRegistry"/> class.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <exception cref="ArgumentNullException">containerBuilder.</exception>
        public AutofacPluginContainerRegistry(ContainerBuilder containerBuilder)
        {
            this.containerBuilder = containerBuilder ?? throw new ArgumentNullException(nameof(containerBuilder));
        }

        /// <inheritdoc/>
        public IPluginContainerRegistry RegisterDecorator<T, TService>(Func<IPluginContainerContext, T> factory, ContainerLifeCycle containerLifeCycle)
            where T : TService
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public IPluginContainerRegistry RegisterFactory<T, TService>(Func<IPluginContainerContext, T> factory, ContainerLifeCycle containerLifeCycle)
            where T : TService
        {
            ArgumentNullException.ThrowIfNull(factory, nameof(factory));

            var registration = this.containerBuilder.Register<TService>(ctx =>
            {
                var scopeProvider = ctx.Resolve<ILifetimeScope>();
                var pluginContext = new AutofacPluginContainerContext(scopeProvider);
                return factory(pluginContext);
            });

            ConfigureRegistration(containerLifeCycle, registration);

            return this;
        }

        /// <inheritdoc/>
        public IPluginContainerRegistry RegisterFactory<T>(Func<IPluginContainerContext, T> factory, ContainerLifeCycle containerLifeCycle)
        {
            ArgumentNullException.ThrowIfNull(factory, nameof(factory));

            var registration = this.containerBuilder.Register(ctx =>
            {
                var scopeProvider = ctx.Resolve<ILifetimeScope>();
                var pluginContext = new AutofacPluginContainerContext(scopeProvider);
                return factory(pluginContext);
            });

            ConfigureRegistration(containerLifeCycle, registration);

            return this;
        }

        /// <inheritdoc/>
        public IPluginContainerRegistry RegisterInstance<T>(T instance)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(instance, nameof(instance));
            return this.RegisterInstance<T, T>(instance);
        }

        /// <inheritdoc/>
        public IPluginContainerRegistry RegisterInstance<T, TService>(T instance)
            where T : class, TService
        {
            ArgumentNullException.ThrowIfNull(instance, nameof(instance));

            IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> registration = this.containerBuilder.RegisterInstance<T>(instance);
            if (typeof(T) != typeof(TService))
            {
                registration.As<TService>();
            }

            return this;
        }

        /// <inheritdoc/>
        public IPluginContainerRegistry RegisterType<T>()
        {
            return this.RegisterType<T>(ContainerLifeCycle.Transient);
        }

        /// <inheritdoc/>
        public IPluginContainerRegistry RegisterType<T>(ContainerLifeCycle containerLifeCycle)
        {
            return this.RegisterType(typeof(T), typeof(T), containerLifeCycle);
        }

        /// <inheritdoc/>
        public IPluginContainerRegistry RegisterType<T, TService>()
        {
            return this.RegisterType<T, TService>(ContainerLifeCycle.Transient);
        }

        /// <inheritdoc/>
        public IPluginContainerRegistry RegisterType<T, TService>(ContainerLifeCycle containerLifeCycle)
        {
            return this.RegisterType(typeof(T), typeof(TService), containerLifeCycle);
        }

        /// <inheritdoc/>
        public IPluginContainerRegistry RegisterType(Type implementation, Type service, ContainerLifeCycle containerLifeCycle)
        {
            ArgumentNullException.ThrowIfNull(implementation, nameof(implementation));
            ArgumentNullException.ThrowIfNull(service, nameof(service));

            var registration = this.containerBuilder.RegisterType(implementation);
            if (!implementation.Equals(service))
            {
                registration.As(service);
            }

            ConfigureRegistration(containerLifeCycle, registration);
            return this;
        }

        /// <inheritdoc/>
        public IPluginContainerRegistry RegisterType(Type implementation, ContainerLifeCycle containerLifeCycle)
        {
            ArgumentNullException.ThrowIfNull(implementation, nameof(implementation));
            return this.RegisterType(implementation, implementation, containerLifeCycle);
        }

        private static void ConfigureRegistration<T>(ContainerLifeCycle containerLifeCycle, IRegistrationBuilder<T, SimpleActivatorData, SingleRegistrationStyle> registration)
        {
            if (!containerLifeCycle.HasFlag(ContainerLifeCycle.Scoped) && containerLifeCycle.HasFlag(ContainerLifeCycle.Singleton))
            {
                registration = registration.SingleInstance();
            }

            if (containerLifeCycle.HasFlag(ContainerLifeCycle.Scoped) && !containerLifeCycle.HasFlag(ContainerLifeCycle.Singleton))
            {
                registration = registration.OwnedByLifetimeScope()
                    .InstancePerLifetimeScope();
            }

            if (containerLifeCycle.HasFlag(ContainerLifeCycle.AutoActivate))
            {
                registration.AutoActivate();
            }
        }

        private static void ConfigureRegistration(ContainerLifeCycle containerLifeCycle, IRegistrationBuilder<object, ConcreteReflectionActivatorData, SingleRegistrationStyle> registration)
        {
            if (!containerLifeCycle.HasFlag(ContainerLifeCycle.Scoped) && containerLifeCycle.HasFlag(ContainerLifeCycle.Singleton))
            {
                registration = registration.SingleInstance();
            }

            if (containerLifeCycle.HasFlag(ContainerLifeCycle.Scoped) && !containerLifeCycle.HasFlag(ContainerLifeCycle.Singleton))
            {
                registration = registration.OwnedByLifetimeScope()
                    .InstancePerLifetimeScope();
            }

            if (containerLifeCycle.HasFlag(ContainerLifeCycle.AutoActivate))
            {
                registration.AutoActivate();
            }
        }
    }
}
