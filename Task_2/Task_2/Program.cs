using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text.Json;
using System.IO;
using System.Threading;
using System.Collections;
using System.Diagnostics;

namespace Task_2
{
    class Program
    {
        private static ThreadSafeList<int> _safeList = new ThreadSafeList<int>();
        private static bool _success = true;
        private static string _error = null;
        private static string _duration = "";

        private static void FindPrimesInRange(object obj)
        {
            var primes = new List<int>();
            var stopWatch = new Stopwatch();
            var range = (Settings)obj;
            var from = (int)range.PrimesFrom;
            var to = (int)range.PrimesTo;
            try
            {
                var numbers = Enumerable.Range(from, to - from);
                stopWatch.Start();
                primes = numbers.Where(x => x >= 2 && Enumerable.Range(2, (int)Math.Sqrt(x))
                                                              .All(n => x % n != 0 || x == 2))
                                                              .ToList();
            }
            catch (Exception) { }
            AddToSafe(primes);
        }

        private static void AddToSafe(object list)
        {
            var primes = (List<int>)list;
            primes.ForEach(x => _safeList.Add(x));
        }


        private static void FindPrimes()
        {
            var json = File.ReadAllText("settings.json");
            var settings = JsonSerializer.Deserialize<List<Settings>>(json);
            var threads = new List<Thread>();
            foreach (var i in settings)
            {
                threads.Add(new Thread(FindPrimesInRange));
                threads.Last().Start(i);
                //threads.Last().Join();
            }
            
            //threads.ForEach(t => t.Join());
            Console.WriteLine(_safeList.Count);
        }

        static void SaveResult()
        {
            var primes = _safeList.GetDistinct().ToArray();
            var result = new Result(_success, _error, _duration, primes);
            var json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("result.json", json);
        }

        static void Main(string[] args)
        {
            var stopWatch = new Stopwatch();
            try
            {
                stopWatch.Start();
                FindPrimes();
            }
            catch (Exception ex)
            {
                _error = ex.Message;
                _success = false;
            }
            finally
            {
                stopWatch.Stop();
                _duration = stopWatch.Elapsed.ToString();
            }
            SaveResult();
        }
    }
}
