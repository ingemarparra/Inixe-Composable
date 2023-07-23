// -----------------------------------------------------------------------
// <copyright file="IPluginContainerContext.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.UI.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The IPluginContainerContext is the IoC instance that is responsible of handling the Plug-in's dependency tree.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IPluginContainerContext : IDisposable
    {
        /// <summary>
        /// Resolves an instance of the service <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The service which is registered into the container.</typeparam>
        /// <returns>An instance of <typeparamref name="T"/> if registered.</returns>
        T ResolveService<T>();

        /// <summary>
        /// Resolves a set of instances of all available service <typeparamref name="T"/> registered implementations.
        /// </summary>
        /// <typeparam name="T">The service which is registered into the container.</typeparam>
        /// <returns>A list of <typeparamref name="T"/> instances.</returns>
        IList<T> ResolveServices<T>();

        /// <summary>
        /// Resolves an instance of the service <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The service which is registered into the container.</param>
        /// <returns>An instance of <paramref name="type"/> if registered.</returns>
        object ResolveService(Type type);

        /// <summary>
        /// Creates a IoC scope.
        /// </summary>
        /// <param name="name">The name for the scope.</param>
        /// <returns>A new instance of the scope</returns>
        IPluginContainerContext CreateScope(string name);
    }
}
