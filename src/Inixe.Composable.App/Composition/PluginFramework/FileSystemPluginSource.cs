// -----------------------------------------------------------------------
// <copyright file="FileSystemPluginSource.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.Composition.PluginFramework
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Inixe.Composable.UI.Core;
    using Newtonsoft.Json;

    /// <summary>
    /// Default file system plugin source. It will scan all plugin manifests from a directory.
    /// </summary>
    /// <seealso cref="Inixe.Composable.UI.Core.IPluginSource" />
    internal class FileSystemPluginSource : IPluginSource
    {
        private readonly string sourceDirectoryPath;

        private FileSystemPluginSource(string sourceDirectoryPath)
        {
            this.sourceDirectoryPath = sourceDirectoryPath;
        }

        public static IPluginSource Create(string sourceDirectoryPath)
        {
            if (string.IsNullOrWhiteSpace(sourceDirectoryPath))
            {
                throw new ArgumentException("Invalid source directory path", nameof(sourceDirectoryPath));
            }

            if (!Directory.Exists(sourceDirectoryPath))
            {
                throw new ArgumentException("Invalid path. The source directory must exist", nameof(sourceDirectoryPath));
            }

            return new FileSystemPluginSource(sourceDirectoryPath);
        }

        /// <inheritdoc/>
        public IList<IPluginManifest> FindManifests()
        {
            const string SearchPattern = "*.manifest";

            var files = Directory.EnumerateFiles(this.sourceDirectoryPath, SearchPattern, SearchOption.AllDirectories);

            var manifests = files.Select(x => new ManifestFile(x, File.ReadAllText(x)))
                .Select(LoadPluginManifestFromFile);

            return manifests.ToList();
        }

        private static IPluginManifest LoadPluginManifestFromFile(ManifestFile manifestFile)
        {
            var manifest = JsonConvert.DeserializeObject<FileBasedPluginManifest>(manifestFile.Contents);
            return manifest;
        }

        private record ManifestFile(string SourcePath, string Contents);

        private class FileBasedPluginManifest : IPluginManifest
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public string SourcePath { get; set; }

            public IReadOnlyDictionary<string, object> Properties { get; set; }

            public string PluginFactoryTypeName { get; set; }

            public string Description { get; set; }
        }
    }
}
