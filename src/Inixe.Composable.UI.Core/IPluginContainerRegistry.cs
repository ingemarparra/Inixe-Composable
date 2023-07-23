// -----------------------------------------------------------------------
// <copyright file="IPluginContainerRegistry.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.UI.Core
{
    using System;

    /// <summary>
    /// IoC life cycle types for all of the objects.
    /// </summary>
    [Flags]
    public enum ContainerLifeCycle
    {
        /// <summary>
        /// Default instancing life cycle. A new instance is created per resolution.
        /// </summary>
        Transient = 0,

        /// <summary>
        /// The instance can be activated upon IoC is built.
        /// </summary>
        AutoActivate = 1,

        /// <summary>
        /// Scoped instance. This will require a scope to be resolved.
        /// </summary>
        Scoped = 2,

        /// <summary>
        /// Singleton instance definition.
        /// </summary>
        Singleton = 4,
    }

    /// <summary>
    /// Container Type registry for a plug-in. All Types composition should be registered using the provided interface.
    /// </summary>
    public interface IPluginContainerRegistry
    {
        /// <summary>
        /// Registers  <typeparamref name="T"/> with a transient scope.
        /// </summary>
        /// <typeparam name="T">The type to be registered.</typeparam>
        /// <returns>Fluent implementation of the registry.</returns>
        IPluginContainerRegistry RegisterType<T>();

        /// <summary>
        /// Registers the type <typeparamref name="T"/> with the provided <paramref name="containerLifeCycle"/> options.
        /// </summary>
        /// <typeparam name="T">The type to be registered.</typeparam>
        /// <param name="containerLifeCycle">The container life cycle options.</param>
        /// <returns>Fluent implementation of the registry.</returns>
        IPluginContainerRegistry RegisterType<T>(ContainerLifeCycle containerLifeCycle);

        /// <summary>
        /// Registers  <typeparamref name="T"/> as an implementation of <typeparamref name="TService"/> with a transient scope.
        /// </summary>
        /// <typeparam name="T">The type to be registered.</typeparam>
        /// <typeparam name="TService">The service type that will be resolved.</typeparam>
        /// <returns>Fluent implementation of the registry.</returns>
        IPluginContainerRegistry RegisterType<T, TService>();

        /// <summary>
        /// Registers  <typeparamref name="T"/> as an implementation of <typeparamref name="TService"/> with the provided <paramref name="containerLifeCycle"/> options.
        /// </summary>
        /// <typeparam name="T">The type to be registered.</typeparam>
        /// <typeparam name="TService">The service type that will be resolved.</typeparam>
        /// <param name="containerLifeCycle">The container life cycle options.</param>
        /// <returns>Fluent implementation of the registry.</returns>
        IPluginContainerRegistry RegisterType<T, TService>(ContainerLifeCycle containerLifeCycle);

        /// <summary>
        /// Registers  <paramref name="implementation"/> as an implementation of <paramref name="service"/> with the provided <paramref name="containerLifeCycle"/> options.
        /// </summary>
        /// <param name="implementation">The type to be registered.</param>
        /// <param name="service">The service type that will be resolved.</param>
        /// <param name="containerLifeCycle">The container life cycle options.</param>
        /// <returns>Fluent implementation of the registry.</returns>
        IPluginContainerRegistry RegisterType(Type implementation, Type service, ContainerLifeCycle containerLifeCycle);

        /// <summary>
        /// Registers the type <paramref name="implementation"/> with the provided <paramref name="containerLifeCycle"/> options.
        /// </summary>
        /// <param name="implementation">The type to be registered.</param>
        /// <param name="containerLifeCycle">The container life cycle options.</param>
        /// <returns>Fluent implementation of the registry.</returns>
        IPluginContainerRegistry RegisterType(Type implementation, ContainerLifeCycle containerLifeCycle);

        /// <summary>
        /// Registers an instance of an object to be provided with <typeparamref name="T"/> type.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="instance">The instance to be registered.</param>
        /// <returns>Fluent implementation of the registry.</returns>
        IPluginContainerRegistry RegisterInstance<T>(T instance)
            where T : class;

        /// <summary>
        /// Registers an instance of <typeparamref name="T"/> an object to be provided with <typeparamref name="TService"/> type.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="instance">The instance to be registered.</param>
        /// /// <typeparam name="TService">The service type that will be resolved.</typeparam>
        /// <returns>Fluent implementation of the registry.</returns>
        IPluginContainerRegistry RegisterInstance<T, TService>(T instance)
            where T : class, TService;

        /// <summary>
        /// Registers an object factory that will be called upon each object creation.
        /// </summary>
        /// <typeparam name="T">The type to be registered.</typeparam>
        /// <typeparam name="TService">The type of the service which will be resolved.</typeparam>
        /// <param name="context">The a plug-in context for performing dynamic dependency resolution.</param>
        /// <param name="containerLifeCycle">The container life cycle options.</param>
        /// <returns>Fluent implementation of the registry.</returns>
        IPluginContainerRegistry RegisterFactory<T, TService>(Func<IPluginContainerContext, T> context, ContainerLifeCycle containerLifeCycle)
            where T : TService;

        /// <summary>
        /// Registers an object factory that will be called upon each object creation.
        /// </summary>
        /// <typeparam name="T">The type to be registered.</typeparam>
        /// <param name="context">The a plug-in context for performing dynamic dependency resolution.</param>
        /// <param name="containerLifeCycle">The container life cycle options.</param>
        /// <returns>Fluent implementation of the registry.</returns>
        IPluginContainerRegistry RegisterFactory<T>(Func<IPluginContainerContext, T> context, ContainerLifeCycle containerLifeCycle);

        IPluginContainerRegistry RegisterDecorator<T, TService>(Func<IPluginContainerContext, T> context, ContainerLifeCycle containerLifeCycle)
            where T : TService;
    }
}
