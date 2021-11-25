using System;
using System.Collections.Generic;
using System.Linq;

namespace EksamenOpgave
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            StregSystem stregSystem = new();
            User user = stregSystem.GetUser(p => p.FirstName == "Mads");
            Console.WriteLine(user);
            stregSystem.AddCreditsToAccount(user, 1000);
            Console.WriteLine(user);
            Product p = stregSystem.GetProductById(1);
            Console.WriteLine(p);
            stregSystem.BuyProduct(user, p);
            Console.WriteLine(user);
            foreach(ITransaction t in stregSystem.GetTransactions(user, 0)) {
                Console.WriteLine(t);
            }

            foreach(var i in stregSystem.ActiveProducts())
            {
                Console.WriteLine(i);
            }

        }
    }
}
