using System;
using System.Collections.Generic;
using System.Threading;
using DoubleIPA.Utils;

namespace DoubleIPA.Model.ItemSelection
{
    /// <summary>
    /// Public interface describing selection configuration
    /// </summary>
    /// <typeparam name="T">Must be ISelectable</typeparam>
    public interface ISelectionConfig<T> where T : ISelectable
    {
        /// <summary>
        /// How many items to select per selection
        /// - must be greater than 0
        /// </summary>
        int ItemsToSelect { get; }

        /// <summary>
        /// How many times may an item be selected
        /// - must be greater than 0
        /// </summary>
        int MaxSelectsPerItem { get; }

        /// <summary>
        /// How frequently does the selection occur?
        /// - user initiated
        /// - timespan
        /// </summary>
        WaitHandle SelectionFrequency { get; }

        /// <summary>
        /// At what point does the selection end?
        /// - user defined
        /// - timespan
        /// - number of items selected
        /// </summary>
        WaitHandle HaltingCriteria { get; }

        /// <summary>
        /// Set of items to select from
        /// - Whole set
        /// - Remaining set from last selection?
        /// </summary>
        IEnumerable<T> ItemsSource { get; }
    }

    /// <summary>
    /// Class responsible for performing the selection
    /// </summary>
    /// <typeparam name="T">Must be ISelectable</typeparam>
    public class Selector<T> where T : ISelectable
    {
        public Selector(ISelectionConfig<T> config)
        {
            Arg.NotNull(config, "config");
            _config = config;
            _stopLoop = new ManualResetEvent(false);
        }

        /// <summary>
        /// Fired when a selection is made. EventArgs contains the items selected
        /// </summary>
        public event EventHandler<SelectedItemsArgs<T>> OnSelection;

        /// <summary>
        /// Run the selector; method blocks until the ISelectionConfig's HaltingCriteria has been
        /// triggered.
        /// </summary>
        public void Run()
        {
            // Set of WaitHandles the loop depends on
            WaitHandle[] handles = new WaitHandle[] {
                    _config.SelectionFrequency,
                    _config.HaltingCriteria,
                    _stopLoop
                };

            bool keepGoing = true;
            while (keepGoing)
            {
                // Wait for any of the handles to come back. Wait a maximum of 500ms so the thread
                // can stay responsive to external requests
                int selectedHandle = WaitHandle.WaitAny(handles, 500);
                switch (selectedHandle)
                {
                    // KeepAlive
                    case WaitHandle.WaitTimeout:
                        break;

                    // Selection frequency
                    case 0:

                        // TODO: Actually select something

                        // Fire the event that we've selected something
                        OnSelection(this, SelectedItemsArgs<T>.Create());
                        break;

                    // HaltingCriteria has been met or Stop() has been called on this object. Break
                    // out of this loop.
                    case 1: // HaltingCriteria
                    case 2: // Stop()
                        keepGoing = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Call to stop the looping.
        /// </summary>
        public void Stop()
        {
            _stopLoop.Set();
        }

        private readonly ISelectionConfig<T> _config;
        private ManualResetEvent _stopLoop;
    }

    /// <summary>
    /// EventArgs for selected items
    /// </summary>
    /// <typeparam name="T">Must be ISelectable</typeparam>
    public class SelectedItemsArgs<T> : EventArgs where T : ISelectable
    {
        #region Object Management

        /// <summary>
        /// Creates from an IEnumerable
        /// </summary>
        /// <param name="items">May be null</param>
        /// <returns></returns>
        internal static SelectedItemsArgs<T> Create(IEnumerable<T> items)
        {
            return new SelectedItemsArgs<T>(items);
        }

        /// <summary>
        /// Creates from a parameterized list
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        internal static SelectedItemsArgs<T> Create(params T[] items)
        {
            return new SelectedItemsArgs<T>(items);
        }

        // Constructor
        private SelectedItemsArgs(IEnumerable<T> items)
        {
            _selectedItems = new List<T>();
            if (items != null)
            {
                _selectedItems.AddRange(items);
            }
        }

        #endregion

        /// <summary>
        /// Set of items selected; may be empty, but never null
        /// </summary>
        public IReadOnlyList<T> SelectedItems
        {
            get { return _selectedItems; }
        }

        #region Private

        private List<T> _selectedItems;

        #endregion
    }
}
