using EksamenOpgave;
using EksamenOpgave.Exceptions;
using System;

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
            Show();
        }
        private void Show()
        {
            foreach(Product product in StregSystem.ActiveProducts())
            {
                Console.WriteLine(product);
            }
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
            Console.WriteLine(transaction);
        }

        public void Close()
        {
            throw new System.NotImplementedException();
        }

        public void DisplayInsufficientCash(User user, Product product)
        {
            Console.WriteLine(new InsufficientCreditsException($"{user}, {product}"));
        }

        public void DisplayGeneralError(string errorString)
        {
            Console.WriteLine(errorString);
        }
    }
}
