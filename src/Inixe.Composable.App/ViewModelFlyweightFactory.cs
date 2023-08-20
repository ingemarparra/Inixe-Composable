// -----------------------------------------------------------------------
// <copyright file="ViewModelFlyweightFactory.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using Inixe.Composable.UI.Core;

    /// <summary>
    /// Factory for View Models that is hosted in resource dictionaries.
    /// </summary>
    /// <seealso cref="System.Dynamic.DynamicObject" />
    public sealed class ViewModelFlyweightFactory : DynamicObject
    {
        private IViewModelLocator locator;
        private Dictionary<string, object> viewModels;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelFlyweightFactory"/> class.
        /// </summary>
        public ViewModelFlyweightFactory()
        {
            this.viewModels = new Dictionary<string, object>();
            this.locator = null;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is initialized.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is initialized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitialized
        {
            get
            {
                return this.locator != null;
            }
        }

        /// <summary>
        /// Provides the implementation for operations that get member values. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations such as getting a value for a property.
        /// </summary>
        /// <param name="binder">Provides information about the object that called the dynamic operation. The <c>binder.Name</c> property provides the name of the member on which the dynamic operation is performed. For example, for the <c>Console.WriteLine(sampleObject.SampleProperty)</c> statement, where <c>sampleObject</c> is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, <c>binder.Name</c> returns "SampleProperty". The <c>binder.IgnoreCase</c> property specifies whether the member name is case-sensitive.</param>
        /// <param name="result">The result of the get operation. For example, if the method is called for a property, you can assign the property value to <paramref name="result" />.</param>
        /// <returns>
        ///   <see langword="true" /> if the operation is successful; otherwise, <see langword="false" />. If this method returns <see langword="false" />, the run-time binder of the language determines the behavior. (In most cases, a run-time exception is thrown.)
        /// </returns>
        /// <exception cref="ArgumentNullException">binder.</exception>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            this.CheckInitialized();

            if (binder == null)
            {
                throw new ArgumentNullException(nameof(binder));
            }

            var isKnown = this.viewModels.ContainsKey(binder.Name);
            if (!isKnown)
            {
                if (!this.locator.IsRegistered(binder.Name))
                {
                    return base.TryGetMember(binder, out result);
                }

                var vm = this.locator.GetViewModel(binder.Name);
                this.viewModels.Add(binder.Name, vm);
            }

            result = this.viewModels[binder.Name];
            return true;
        }

        /// <summary>
        /// Initializes the specified locator.
        /// </summary>
        /// <param name="locator">The locator.</param>
        /// <exception cref="ArgumentNullException">locator.</exception>
        internal void Initialize(IViewModelLocator locator)
        {
            this.locator = locator ?? throw new ArgumentNullException(nameof(locator));
        }

        private void CheckInitialized()
        {
            if (!this.IsInitialized)
            {
                throw new InvalidOperationException("The resolver has not been initialized");
            }
        }
    }
}
