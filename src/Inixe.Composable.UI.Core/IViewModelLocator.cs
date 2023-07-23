// -----------------------------------------------------------------------
// <copyright file="IViewModelLocator.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.UI.Core
{
    /// <summary>
    /// Locates view models into the Corresponding View Model Factory.
    /// </summary>
    public interface IViewModelLocator
    {
        /// <summary>
        /// Determines whether the specified view model name is registered.
        /// </summary>
        /// <param name="name">The view model type name.</param>
        /// <returns>
        ///   <c>true</c> if the specified view model type name is registered; otherwise, <c>false</c>.
        /// </returns>
        bool IsRegistered(string name);

        /// <summary>
        /// Gets an instance of the view model with type name specified by <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Instance of the requested view model.</returns>
        object GetViewModel(string name);
    }
}
