// -----------------------------------------------------------------------
// <copyright file="AsyncDispatcher.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using Inixe.Composable.UI.Core;

    /// <summary>
    /// WPF Dispatcher for Context Switching calls.
    /// </summary>
    /// <seealso cref="Inixe.Composable.UI.Core.IAsyncDispatcher" />
    public sealed class AsyncDispatcher : IAsyncDispatcher
    {
        /// <summary>
        /// Invokes the invokes an action on the UI Thread.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>The operation task.</returns>
        public async Task InvokeAsync(Action action)
        {
            await Application.Current.Dispatcher.BeginInvoke(action);
        }
    }
}
