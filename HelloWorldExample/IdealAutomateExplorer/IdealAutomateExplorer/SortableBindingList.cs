using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Windows.Forms.Samples { 
    public class SortableBindingList<T> : BindingList<T> {
        private static readonly Dictionary<string, Comparison<T>> PropertyLookup;
        private readonly Action<IList<T>, Comparison<T>> _sortDelegate;

        private bool _isSorted;
        private ListSortDirection _sortDirection;
        private PropertyDescriptor _sortProperty;

        //A Dictionary<TKey, TValue> is thread safe on reads so we only need to make the dictionary once per type.
        static SortableBindingList() {
            PropertyLookup = new Dictionary<string, Comparison<T>>();
            foreach (PropertyInfo property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
                Type propertyType = property.PropertyType;
                bool usingNonGenericInterface = false;

                //First check to see if it implments the generic interface.
                Type compareableInterface = propertyType.GetInterfaces()
                    .FirstOrDefault(a => a.Name == "IComparable`1" &&
                                         a.GenericTypeArguments[0] == propertyType);

                //If we did not find a generic interface then use the non-generic interface.
                if (compareableInterface == null) {
                    compareableInterface = propertyType.GetInterface("IComparable");
                    usingNonGenericInterface = true;
                }

                if (compareableInterface != null) {
                    ParameterExpression x = Expression.Parameter(typeof(T), "x");
                    ParameterExpression y = Expression.Parameter(typeof(T), "y");

                    MemberExpression xProp = Expression.Property(x, property.Name);
                    Expression yProp = Expression.Property(y, property.Name);

                    MethodInfo compareToMethodInfo = compareableInterface.GetMethod("CompareTo");

                    //If we are not using the generic version of the interface we need to 
                    // cast to object or we will fail when using structs.
                    if (usingNonGenericInterface) {
                        yProp = Expression.TypeAs(yProp, typeof(object));
                    }

                    MethodCallExpression call = Expression.Call(xProp, compareToMethodInfo, yProp);

                    Expression<Comparison<T>> lambada = Expression.Lambda<Comparison<T>>(call, x, y);
                    PropertyLookup.Add(property.Name, lambada.Compile());
                }
            }
        }

        public SortableBindingList() : base(new List<T>()) {
            _sortDelegate = (list, comparison) => ((List<T>)list).Sort(comparison);
        }

        public SortableBindingList(IList<T> list) : base(list) {
            MethodInfo sortMethod = list.GetType().GetMethod("Sort", new[] { typeof(Comparison<T>) });
            if (sortMethod == null || sortMethod.ReturnType != typeof(void)) {
                throw new ArgumentException(
                    "The passed in IList<T> must support a \"void Sort(Comparision<T>)\" call or you must provide one using the other constructor.",
                    "list");
            }

            _sortDelegate = CreateSortDelegate(list, sortMethod);
        }

        public SortableBindingList(IList<T> list, Action<IList<T>, Comparison<T>> sortDelegate)
            : base(list) {
            _sortDelegate = sortDelegate;
        }

        protected override bool IsSortedCore {
            get { return _isSorted; }
        }

        protected override ListSortDirection SortDirectionCore {
            get { return _sortDirection; }
        }

        protected override PropertyDescriptor SortPropertyCore {
            get { return _sortProperty; }
        }

        protected override bool SupportsSortingCore {
            get { return true; }
        }

        private static Action<IList<T>, Comparison<T>> CreateSortDelegate(IList<T> list, MethodInfo sortMethod) {
            ParameterExpression sourceList = Expression.Parameter(typeof(IList<T>));
            ParameterExpression comparer = Expression.Parameter(typeof(Comparison<T>));
            UnaryExpression castList = Expression.TypeAs(sourceList, list.GetType());
            MethodCallExpression call = Expression.Call(castList, sortMethod, comparer);
            Expression<Action<IList<T>, Comparison<T>>> lambada =
                Expression.Lambda<Action<IList<T>, Comparison<T>>>(call,
                    sourceList, comparer);
            Action<IList<T>, Comparison<T>> sortDelegate = lambada.Compile();
            return sortDelegate;
        }

        protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction) {
            Comparison<T> comparison;

            if (PropertyLookup.TryGetValue(property.Name, out comparison)) {
                if (direction == ListSortDirection.Descending) {
                    _sortDelegate(Items, (x, y) => comparison(y, x));
                } else {
                    _sortDelegate(Items, comparison);
                }

                _isSorted = true;
                _sortProperty = property;
                _sortDirection = direction;

                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, property));
            }
        }

        protected override void RemoveSortCore() {
            _isSorted = false;
        }
    }
}