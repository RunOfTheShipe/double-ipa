
namespace DoubleIPA.Model.ItemSelection
{
    /// <summary>
    /// Interface describing an item that may be selected. Responsible for keeping track of how
    /// many times the item has been selected
    /// </summary>
    public interface ISelectable
    {
        /// <summary>
        /// Current count of the number of times the item has been selected
        /// </summary>
        int SelectedCount { get; }

        /// <summary>
        /// Called when an item has been selected
        /// </summary>
        void Selected();
    }

    /// <summary>
    /// Base implementation of the ISelectable interface
    /// </summary>
    public abstract class Selectable : ISelectable
    {
        /// <summary>
        /// Default constructor; sets SelectedCount to 0 by default
        /// </summary>
        public Selectable()
            : this(0)
        { }

        /// <summary>
        /// Allows for initial selection count for the item
        /// </summary>
        /// <param name="initialCount"></param>
        public Selectable(int initialCount)
        {
            _selectedCount = initialCount;
        }

        /// <summary>
        /// Accesses the number of times this item has been selected
        /// </summary>
        public int SelectedCount
        {
            get { return _selectedCount; }
        }

        /// <summary>
        /// Called when the item has been selected
        /// </summary>
        public void Selected()
        {
            _selectedCount++;
            OnSelection();
        }

        /// <summary>
        /// Method by be overridden by derived classes; called when the item has been selected, but
        /// after SelectedCount has been updated. No default behavior in the base class.
        /// </summary>
        protected virtual void OnSelection()
        { }

        #region Privates

        private int _selectedCount;

        #endregion
    }
}
