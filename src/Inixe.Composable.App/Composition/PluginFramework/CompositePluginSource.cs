// -----------------------------------------------------------------------
// <copyright file="CompositePluginSource.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.Composition.PluginFramework
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Inixe.Composable.UI.Core;

    /// <summary>
    /// Aggregator for plugin sources.
    /// </summary>
    /// <seealso cref="Inixe.Composable.UI.Core.IPluginSource" />
    /// <remarks>This acts as a decorator that will eventually call any module registered <see cref="IPluginSource"/>.</remarks>
    internal sealed class CompositePluginSource : IPluginSource
    {
        private readonly IList<IPluginSource> sources;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositePluginSource"/> class.
        /// </summary>
        /// <param name="sources">The sources.</param>
        public CompositePluginSource(IList<IPluginSource> sources)
        {
            this.sources = sources ?? (IList<IPluginSource>)new List<IPluginSource> { new NullPluginSource() };
        }

        /// <inheritdoc/>
        public IList<IPluginManifest> FindManifests()
        {
            var allManifests = this.sources.SelectMany(x => x.FindManifests())
                .ToList();

            return allManifests;
        }
    }
}
