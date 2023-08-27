// -----------------------------------------------------------------------
// <copyright file="PluginRegistry.cs" company="Inixe S.A.">
// Copyright All Rights reserved. Inixe S.A. 2023
// </copyright>
// -----------------------------------------------------------------------

namespace Inixe.Composable.App.Composition.PluginFramework
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Instance collection.
    /// </summary>
    /// <seealso cref="System.Collections.ObjectModel.KeyedCollection{TKey, TValue}" />
    public class PluginRegistry : KeyedCollection<Guid, PluginInstance>, INotifyCollectionChanged, INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Occurs when the collection changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc/>
        public void Dispose()
        {
            foreach (var item in this.ToArray())
            {
                item.Dispose();
            }

            this.ClearItems();
        }

        /// <summary>
        /// Called when the collection is changed.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="NotSupportedException">Could not notify action</exception>
        protected virtual void OnCollectionChanged(PluginInstance instance, NotifyCollectionChangedAction action)
        {
            NotifyCollectionChangedEventArgs args;

            switch (action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                    args = new NotifyCollectionChangedEventArgs(action, instance);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    args = new NotifyCollectionChangedEventArgs(action);
                    break;
                default:
                    throw new NotSupportedException("Could not notify action");
            }

            this.CollectionChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Called when a property has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// When implemented in a derived class, extracts the key from the specified element.
        /// </summary>
        /// <param name="item">The element from which to extract the key.</param>
        /// <returns>
        /// The key for the specified element.
        /// </returns>
        protected override Guid GetKeyForItem(PluginInstance item)
        {
            return item.Manifest.Id;
        }

        /// <summary>
        /// Inserts an element into the <see cref="T:System.Collections.ObjectModel.KeyedCollection`2" /> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        protected override void InsertItem(int index, PluginInstance item)
        {
            base.InsertItem(index, item);
            this.OnPropertyChanged(nameof(this.Count));
            this.OnCollectionChanged(item, NotifyCollectionChangedAction.Add);
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="T:System.Collections.ObjectModel.KeyedCollection`2" />.
        /// </summary>
        /// <param name="index">The index of the element to remove.</param>
        protected override void RemoveItem(int index)
        {
            var instance = this[index];
            base.RemoveItem(index);

            this.OnPropertyChanged(nameof(this.Count));
            this.OnCollectionChanged(instance, NotifyCollectionChangedAction.Remove);
        }

        /// <summary>
        /// Removes all elements from the <see cref="T:System.Collections.ObjectModel.KeyedCollection`2" />.
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();
            this.OnPropertyChanged(nameof(this.Count));
            this.OnCollectionChanged(null, NotifyCollectionChangedAction.Reset);
        }
    }
}
