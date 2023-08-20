// -----------------------------------------------------------------------
// <copyright file="Constants.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.Composition
{
    internal static class Constants
    {
        public const string LocalPluginsDirectory = nameof(LocalPluginsDirectory);

        public static string CurrentPath
        {
            get
            {
                var thisType = typeof(PluginCompositionModule);
                var asm = thisType.Assembly;
                return System.IO.Path.GetDirectoryName(asm.Location);
            }
        }

        public static string DefaultPluginDirectory
        {
            get
            {
                return System.IO.Path.Combine(CurrentPath, "plugins");
            }
        }
    }
}
