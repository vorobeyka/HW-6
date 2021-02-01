using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace Task_2
{
    class Program
    {
        private static readonly ThreadSafeHashSet<int> _safeHashSet = new ThreadSafeHashSet<int>();
        private static readonly Stopwatch _stopwatch = new Stopwatch();
        private static bool _success = true;
        private static string _error = null;
        private static string _duration = "";

        private static void FindPrimesInRange(object obj)
        {
            var stopWatch = new Stopwatch();
            var from =((Settings)obj).PrimesFrom;
            var to = ((Settings)obj).PrimesTo;
            try
            {
                var numbers = Enumerable.Range(from, to - from);
                stopWatch.Start();
                var primes = numbers.Where(x => x >= 2 && Enumerable.Range(2, (int)Math.Sqrt(x))
                                                              .All(n => x % n != 0 || x == 2));
                foreach (var i in primes)
                {
                    _safeHashSet.SafeAdd(i);
                }
            }
            catch (ArgumentException) { }
        }

        private static void FindPrimes()
        {
            var json = "";
            try
            {
                json = File.ReadAllText("settings.json");
            }
            catch (Exception)
            {
                throw new Exception("settings.json are missing or corrupted");
            }
            var settings = JsonSerializer.Deserialize<List<Settings>>(json);
            var threads = new List<Thread>();
            _stopwatch.Start();
            foreach (var i in settings)
            {
                threads.Add(new Thread(FindPrimesInRange));
                threads.Last().Start(i);
            }
            threads.ForEach(t => t.Join());
            _stopwatch.Stop();
        }

        private static void SaveResult()
        {
            var primes = _safeHashSet.AsSortedArray();
            var result = new Result(_success, _error, _duration, primes);
            var json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("result.json", json);
        }

        static int Main(string[] args)
        {
            var returnValue = 0;
            try
            {
                FindPrimes();
            }
            catch (Exception ex)
            {
                _error = ex.Message;
                _success = false;
                returnValue = -1;
            }
            finally
            {
                _duration = _stopwatch.Elapsed.ToString();
                SaveResult();
            }
            return returnValue;
        }
    }
}
