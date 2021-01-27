using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Task_2
{
    class ThreadSafeList<T> : List<T>
    {
        private static readonly object Marker;

        public void SafeAdd(T item)
        {
            lock (Marker)
            {
                Add(item);
            }
        }
    }
}
