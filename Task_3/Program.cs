﻿using System;
using System.Threading;
using System.Collections.Concurrent;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace Task_3
{
    class Program
    {
        private static ConcurrentQueue<KeyValuePair<string, string>> concurrentQueue;
        private static CountdownEvent waiter;
        private static int threadsCount;
        private static int successfullLoginCount = 0;
        private static int failedLoginCount = 0;

        private static int GetThreadsCount()
        {
            Console.Write("Enter threads count -> ");
            try
            {
                var result = int.Parse(Console.ReadLine());
                if (result <= 0) throw new ArgumentOutOfRangeException();
                return result;
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid value. Try again");
                return GetThreadsCount();
            }
        }

        private static void AddToQueue(string[] pair)
        {
            concurrentQueue.Enqueue(new KeyValuePair<string, string>(pair[0], pair[1]));
        }

        private static void TryLogin(object obj)
        {
            concurrentQueue.TryDequeue(out KeyValuePair<string, string> pair);
            var isLogined = LoginClient.Login(pair.Key, pair.Value);
            if (isLogined == null)
            {
                Interlocked.Increment(ref failedLoginCount);
            }
            else
            {
                Interlocked.Increment(ref successfullLoginCount);
            }
            waiter.Signal();
        }

        private static void ThreadsWork()
        {
            while (!concurrentQueue.IsEmpty)
            {
                if (concurrentQueue.Count > threadsCount)
                {
                    waiter = new CountdownEvent(threadsCount);
                }
                else
                {
                    waiter = new CountdownEvent(concurrentQueue.Count);
                }
                for (int i = 0; i < threadsCount && !concurrentQueue.IsEmpty; i++)
                {
                    ThreadPool.QueueUserWorkItem(TryLogin, new object());
                }
                waiter.Wait();
            }
        }

        private static void SaveResult()
        {
            var result = new Result(successfullLoginCount, failedLoginCount);
            var json = JsonSerializer.Serialize(result, new JsonSerializerOptions() { WriteIndented = true });
            File.WriteAllText("result.json", json);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Service for issuing unique logins\nby Andrey Basystyi");
            concurrentQueue = new ConcurrentQueue<KeyValuePair<string, string>>();
            threadsCount = GetThreadsCount();
            foreach (var i in File.ReadAllLines("logins.csv"))
            {
                AddToQueue(i.Split(';'));
            }
            ThreadsWork();
            SaveResult();
        }
    }
}
