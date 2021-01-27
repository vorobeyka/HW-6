using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Diagnostics;

namespace Task_1
{
    class Program
    {
        static private int _min;
        static private int _max;

        static int GetValue()
        {
            try
            {
                return int.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                Console.Write("Invalid value. Try again\n-> ");
                return GetValue();
            }
        }

        static void SetRange()
        {
            while (true)
            {
                Console.Write("Enter minimum value\n-> ");
                _min = GetValue();
                Console.Write("Enter maximum value\n-> ");
                _max = GetValue();
                if (_max > _min) break;
                Console.WriteLine("Maximum value must be more than minimum value. Try again");
            }
        }

        static void FindWithLINQ()
        {
            var range = Enumerable.Range(_min, _max - _min);
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var primes = range.Where(x => x >= 2 && Enumerable.Range(2, (int)Math.Sqrt(x))
                                                              .All(n => x % n != 0 || x == 2)).ToList();
            stopWatch.Stop();
            Console.WriteLine("------------------------");
            Console.WriteLine($"Count: {primes.Count}\nTime: {stopWatch.Elapsed}");
            Console.WriteLine("------------------------");
        }

        static void FindWithPLINQ()
        {
            var range = Enumerable.Range(_min, _max - _min).AsParallel();
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var primes = range.Where(x => x >= 2 && Enumerable.Range(2, (int)Math.Sqrt(x))
                                                              .All(n => x % n != 0 || x == 2)).ToList();
            stopWatch.Stop();
            Console.WriteLine("------------------------");
            Console.WriteLine($"Count: {primes.Count}\nTime: {stopWatch.Elapsed}");
            Console.WriteLine("------------------------");
        }

        static void Menu()
        {
            while (true)
            {
                Console.Write("1. Find with LINQ\n2. Find with PLINQ\n3. Set new range\n4. Exit\n->");
                var key = Console.ReadKey().KeyChar;
                Console.WriteLine();
                switch (key)
                {
                    case '1': FindWithLINQ();
                        break;
                    case '2': FindWithPLINQ();
                        break;
                    case '3': SetRange();
                        break;
                    case '4': return;
                    default: Console.WriteLine("Invalid menu number. Try again");
                        break;
                }

            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Task_1. Finding prime numbers with LINQ and PLINQ\nby Andrey Basystyi");
            SetRange();
            Menu();
            Console.WriteLine("Bye!");
        }
    }
}
