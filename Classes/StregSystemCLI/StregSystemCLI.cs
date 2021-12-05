using EksamenOpgave;
using EksamenOpgave.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace EksamenOpgave.CLI
{
    public class StregSystemCLI : IStregsystemUI
    {
        private StregSystem StregSystem { get; set; }
        public StregSystemCLI(StregSystem stregSystem)
        {
            StregSystem = stregSystem;
        }
        public void Start()
        {
            foreach (User u in StregSystem.Users)
            {
                u.BalanceLow += OnUserBalanceLow;
            }
            Show();
        }
        public void Reset()
        {
            Console.Clear();
        }
        public void Show()
        {
            foreach(Product product in StregSystem.ActiveProducts())
            {
                Console.WriteLine(product);
            }
            Console.WriteLine();
            Console.Write("Input command: ");
        }

        public void OnUserBalanceLow(object source, UserBalanceEventArgs e)
        {
            Reset();
            Console.WriteLine($"{e.User.UserName}'s balance is low :(");
        }

        public void DisplayUserNotFound(string username)
        {
            Console.WriteLine($"User [{username}] was not found");
        }

        public void DisplayProductNotFound(string product)
        {
            Console.WriteLine($"Product [{product}] was not found");
        }

        public void DisplayUserInfo(User user)
        {
            Console.WriteLine(user);
            if (user.Balance < 50)
                Console.WriteLine("User balance is low");
            foreach (ITransaction t in StregSystem.GetTransactions(user, 10))
            {
                Console.WriteLine(t);
            }
        }

        public void DisplayTooManyArgumentsError(string command)
        {
            Console.WriteLine(command);
        }

        public void DisplayAdminCommandNotFoundMessage(string adminCommand)
        {
            Console.WriteLine(adminCommand);
        }

        public void DisplayUserBuysProduct(BuyTransaction transaction)
        {
            Console.WriteLine(transaction);
        }

        public void DisplayUserBuysProduct(int count, BuyTransaction transaction)
        {
            Console.WriteLine($"{count}x {transaction}");
        }

        public void Close()
        {

        }

        public void DisplayInsufficientCash(User user, Product product)
        {
            Console.WriteLine(new InsufficientCreditsException($"{user}, {product}"));
        }
        public void DisplayInsufficientCash(User user, List<(Product, int)> product)
        {
            Console.WriteLine(new InsufficientCreditsException($"{user.UserName}'s current balance: {user.Balance}kr\nNeeds {product.Sum(p => p.Item1.Price * p.Item2) - user.Balance}kr to buy products"));
        }

        public void DisplayGeneralError(string errorString)
        {
            Console.WriteLine(errorString);
        }
    }
}
