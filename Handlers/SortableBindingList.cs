using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace SoftwareShelf_Desktop
{
    // BindingList<T> on .NET 2.0 does not sort, so grid header clicks need this.
    internal class SortableBindingList<T> : BindingList<T>
    {
        private bool isSorted;
        private ListSortDirection sortDirection;
        private PropertyDescriptor sortProperty;

        public SortableBindingList(List<T> items)
            : base(items)
        {
        }

        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        protected override bool IsSortedCore
        {
            get { return isSorted; }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get { return sortDirection; }
        }

        protected override PropertyDescriptor SortPropertyCore
        {
            get { return sortProperty; }
        }

        protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
        {
            List<T> items = (List<T>)Items;
            items.Sort(delegate (T left, T right)
            {
                int compared = Comparer.Default.Compare(property.GetValue(left), property.GetValue(right));
                return direction == ListSortDirection.Descending ? -compared : compared;
            });

            sortProperty = property;
            sortDirection = direction;
            isSorted = true;
            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        protected override void RemoveSortCore()
        {
            isSorted = false;
            sortProperty = null;
        }
    }
}
