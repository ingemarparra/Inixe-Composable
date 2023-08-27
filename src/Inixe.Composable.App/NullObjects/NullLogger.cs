// -----------------------------------------------------------------------
// <copyright file="NullLogger.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.NullObjects
{
    using System;
    using Microsoft.Extensions.Logging;

    internal class NullLogger : ILogger, IDisposable
    {
        public IDisposable BeginScope<TState>(TState state)
            where TState : notnull
        {
            return this;
        }

        public void Dispose()
        {
            // Null object implementation
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            // Null object implementation
        }
    }
}
