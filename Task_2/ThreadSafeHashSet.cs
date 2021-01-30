using System.Collections.Generic;
using System.Linq;

namespace Task_2
{
    class ThreadSafeHashSet<T> : HashSet<T>
    {
        private static readonly object Marker = new object();

        public ThreadSafeHashSet() {}

        public void SafeAdd(T item)
        {
            lock (Marker)
            {
                Add(item);
            }
        }

        public T[] AsSortedArray()
        {
            return this.OrderBy(x => x).ToArray();
        }
    }
}
