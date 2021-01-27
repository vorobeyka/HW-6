using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;
using System.Threading;


namespace Task_2
{
    class Program
    {
        private static List<int> list = new List<int>();
        private static ThreadSafeList<object> safeList;

        static void FindPrimes(int from, int to)
        {
            for (var i = from; i < to; i++)
            {
                if (IsPrime(i))
                {
                    list.Add(i);
                }
            }
        }

        private static bool IsPrime(int n)
        {
            if (n <= 1)
                return false;

            // Check from 2 to n-1
            for (var i = 2; i < n; i++)
            {
                if (n % i == 0) return false;
            }
            return true;
        }

        static void PrintPrimes(object obj)
        {
            var range = (KeyValuePair<int, int>)obj;
            FindPrimes(range.Key, range.Value);
        }

        static void Main(string[] args)
        {
            var pair = new List<KeyValuePair<int, int>>();
            pair.Add(new KeyValuePair<int, int>(1, 10));
            pair.Add(new KeyValuePair<int, int>(1, 10));
            pair.Add(new KeyValuePair<int, int>(1, 10));
            var t1 = new Thread(PrintPrimes);
            t1.Start(pair[0]);
            var t2 = new Thread(PrintPrimes);
            t2.Start(pair[1]);
            var t3 = new Thread(PrintPrimes);
            t3.Start(pair[2]);
            t1.Join();
            t2.Join();
            t3.Join();
            list.Distinct().ToList().ForEach(x => Console.Write($"{x} "));
            Console.WriteLine();
            /*var pair = new List<KeyValuePair<int, int>>();
            pair.Add(new KeyValuePair<int, int>(1, 80_000));
            pair.Add(new KeyValuePair<int, int>(1, 80_000));
            pair.Add(new KeyValuePair<int, int>(1, 80_000));
            *//*var t1 = new Thread(PrintPrimes);
            t1.Start(pair[0]);
            var t2 = new Thread(PrintPrimes);
            t2.Start(pair[1]);
            var t3 = new Thread(PrintPrimes);
            t3.Start(pair[2]);
            t1.Join();
            t2.Join();
            t3.Join();*//*
            for (int i = 0; i < 3; i++)
            {
                var thread = new Thread(PrintPrimes)
                {
                    Name = $"T{i}",
                    IsBackground = false
                };
                thread.Start(pair[i]);
                //thread.Join();
            }
            Console.WriteLine("pivet");*/
        }
    }
}
