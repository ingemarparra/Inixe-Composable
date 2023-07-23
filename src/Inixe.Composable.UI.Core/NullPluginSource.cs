// -----------------------------------------------------------------------
// <copyright file="NullPluginSource.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.UI.Core
{
    using System.Collections.Generic;

    public sealed class NullPluginSource : IPluginSource
    {
        public IList<IPluginManifest> FindManifests()
        {
            return new List<IPluginManifest>();
        }
    }
}
