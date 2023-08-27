// -----------------------------------------------------------------------
// <copyright file="NullLogger{T}.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.NullObjects
{
    using System;
    using Microsoft.Extensions.Logging;

    internal sealed class NullLogger<T> : NullLogger, ILogger<T>
    {
    }
}
