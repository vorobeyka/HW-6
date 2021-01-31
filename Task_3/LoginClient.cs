using System;
using System.Threading;

namespace Task_3
{
    class LoginClient
    {
        public static string Login(string login, string password)
        {
            var rnd = new Random();
            Thread.Sleep((int)(rnd.NextDouble() * 1000));
            return rnd.Next(2) == 1 ? Guid.NewGuid().ToString() : null;
        }
    }
}
