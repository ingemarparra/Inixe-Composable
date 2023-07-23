// -----------------------------------------------------------------------
// <copyright file="IPlugin.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.UI.Core
{
    using Inixe.Composable.UI.Core.Commands;

    public interface IPlugin
    {
        bool IsRunning { get; }

        string Name { get; }

        void Start(IAppHost host);

        void Stop(IAppHost host);
    }
}
