// -----------------------------------------------------------------------
// <copyright file="IPluginSource.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.UI.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Source from which plug-ins are located.
    /// </summary>
    public interface IPluginSource
    {
        /// <summary>
        /// Finds the plug-in manifests contained or provided by this source.
        /// </summary>
        /// <returns>A list of Manifests.</returns>
        IList<IPluginManifest> FindManifests();
    }
}
