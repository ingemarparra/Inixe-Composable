// -----------------------------------------------------------------------
// <copyright file="IAsyncDispatcher.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.UI.Core
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Dispatcher for Context Switching calls.
    /// </summary>
    public interface IAsyncDispatcher
    {
        /// <summary>
        /// Invokes the invokes an action on the UI Thread.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The operation task.</returns>
        Task InvokeAsync(Action action);
    }
}
