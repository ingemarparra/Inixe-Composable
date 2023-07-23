// -----------------------------------------------------------------------
// <copyright file="IPluginManifest.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.UI.Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The manifest instance is responsible of declaring key plugin information to the consuming host.
    /// </summary>
    public interface IPluginManifest
    {
        /// <summary>
        /// Gets the unique plugin identifier.
        /// </summary>
        /// <value>
        /// The unique plugin identifier.
        /// </value>
        Guid Id { get; }

        string Name { get; }

        string Description { get; }

        string SourcePath { get; }

        IReadOnlyDictionary<string, object> Properties { get; }

        string PluginFactoryTypeName { get; }
    }
}
