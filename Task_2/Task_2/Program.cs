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
        private static ThreadSafeList<int> safeList = new ThreadSafeList<int>();
        private static readonly object Marker = new object();

        static void FindSafePrimes(int from, int to)
        {
            for (var i = from; i < to; i++)
            {
                if (IsPrime(i))
                {
                    lock (Marker)
                    {
                        safeList.SafeAdd(i);
                    }
                }
            }
        }

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

        static void PrintPrimesSafe(object obj)
        {
            var range = (KeyValuePair<int, int>)obj;
            FindSafePrimes(range.Key, range.Value);
        }

        static void Main(string[] args)
        {
            var pair = new List<KeyValuePair<int, int>>();
            pair.Add(new KeyValuePair<int, int>(1, 1000));
            pair.Add(new KeyValuePair<int, int>(1, 1000));
            pair.Add(new KeyValuePair<int, int>(1, 1000));
            var t1 = new Thread(PrintPrimesSafe);
            t1.Start(pair[0]);
            var t2 = new Thread(PrintPrimesSafe);
            t2.Start(pair[1]);
            var t3 = new Thread(PrintPrimesSafe);
            t3.Start(pair[2]);
            t1.Join();
            t2.Join();
            t3.Join();
            Console.WriteLine(safeList.Count);
            //Console.WriteLine(list.Count);
            //list.ForEach(x => Console.Write($"{x} "));
            //Console.WriteLine(list.Count);
            //Console.WriteLine(safeList.Count);
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
