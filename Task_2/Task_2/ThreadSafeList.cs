using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Task_2
{
    class ThreadSafeList<T> : List<T>
    {
        private static readonly object Marker = new object();

        public ThreadSafeList() {}

        public void SafeAdd(T item)
        {
            lock (Marker)
            {
                if (!Contains(item))
                {
                    Add(item);
                }
            }
        }

        public List<T> GetDistinct()
        {
            Sort();
            return this.Distinct().ToList();
        }
    }
}
