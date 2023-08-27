// -----------------------------------------------------------------------
// <copyright file="NullConfiguration.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.NullObjects
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Primitives;

    internal sealed class NullConfiguration : IConfiguration, IConfigurationSection, IChangeToken, IDisposable
    {
        public string this[string key]
        {
            get
            {
                return string.Empty;
            }

            set
            {
                // Null Object implementation
            }
        }

        public string Key
        {
            get
            {
                return string.Empty;
            }
        }

        public string Path
        {
            get
            {
                return string.Empty;
            }
        }

        public string Value
        {
            get
            {
                return string.Empty;
            }

            set
            {
                // Null Object implementation
            }
        }

        bool IChangeToken.HasChanged
        {
            get
            {
                return false;
            }
        }

        bool IChangeToken.ActiveChangeCallbacks
        {
            get
            {
                return false;
            }
        }

        public void Dispose()
        {
            // Null Object implementation
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return Array.Empty<IConfigurationSection>();
        }

        public IChangeToken GetReloadToken()
        {
            return this;
        }

        public IConfigurationSection GetSection(string key)
        {
            return this;
        }

        IDisposable IChangeToken.RegisterChangeCallback(Action<object> callback, object state)
        {
            return this;
        }
    }
}
