using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EksamenOpgave.CLI;
using EksamenOpgave;
using EksamenOpgave.Exceptions;
using System.Threading;

namespace EksamenOpgave.Controller
{
    public class StregSystemController
    {
        private string Command;
        public Dictionary<string, Action> AdminCommands = new();
        
        StregSystem StregSystem { get; set; }
        IStregsystemUI CLI { get; set; }
        private int Delay;
        private bool _running;
        public StregSystemController(StregSystem stregSystem, IStregsystemUI CLI)
        {
            this.CLI = CLI;
            StregSystem = stregSystem;
            AdminCommands.Add(":activate", ActivateAction);
            AdminCommands.Add(":deactivate", DeActivateAction);
            AdminCommands.Add(":quit", QuitAction);
            AdminCommands.Add(":q", QuitAction);
            AdminCommands.Add(":crediton", CreditOnAction);
            AdminCommands.Add(":creditoff", CreditOffAction);
            AdminCommands.Add(":addcredits", AddCreditsAction);
        }

        

        public void Start()
        {
            _running = true;
            CLI.Start();
            while (_running)
            {
                CLI.Reset();
                CLI.Show();
                WaitForInput();
                Thread.Sleep(Delay);
            }
        }
        public void WaitForInput()
        {
            ParseCommand(Console.ReadLine());
        }
        private void Buy(User user, List<(Product, int)> products)
        {
            
            CLI.Reset();
            if (products.Any(p => p.Item2 < 1))
            {
                CLI.DisplayGeneralError("Count cant be less than 1");
                return;
            }
            decimal TotalPrice = products.Where(p => p.Item1.CanBeBoughtOnCredit == false).Sum(p => p.Item1.Price * p.Item2);
            if (TotalPrice > user.Balance)
            {
                Delay = 5000;
                CLI.DisplayInsufficientCash(user, products);
                return;
            }
            Delay = products.Count * 2500;
            foreach (var product in products)
            {   if (product.Item2 < 1)
                {
                    CLI.DisplayGeneralError("Count cant be less than 1");
                    return;
                }
                ITransaction t = null;
                for (int i = 0; i < product.Item2; i++)
                {
                    StregSystem.BuyProduct(user, product.Item1);
                }
                t = StregSystem.GetTransactions(user, 1).ToList()[0];
                if (t != null)
                CLI.DisplayUserBuysProduct(product.Item2, (BuyTransaction)t);
            }
        }
        
        private User GetUserByUsername(string username)
        {
            try
            {
                return StregSystem.GetUserByUsername(username);
            }
            catch (UserNotFoundException)
            {
                CLI.DisplayUserNotFound(username);
                return null;
            }
        }
        private Product GetProductById(int Id)
        {
            try
            {
                return StregSystem.GetProductById(Id);
            }
            catch (ProductDoesNotExistException)
            {
                CLI.DisplayProductNotFound(Id.ToString());
                return null;
            }
        }
        private void ParseCommand(string command)
        {
            Delay = 2500;
            command = command.Trim();
            Command = command;
            if (command.StartsWith(":"))
            {
                Action a = AdminCommands.Where(p => p.Key == command.Split(" ").ToList()[0].ToLower()).FirstOrDefault().Value ?? null;
                if (a != null)
                    a();
                else
                    CLI.DisplayAdminCommandNotFoundMessage(command.Split(" ").ToList()[0]);
            }
            else
                BuyParse();
        }
        private void BuyParse()
        {
            List<string> args = Command.Split(" ").ToList();
            User user = GetUserByUsername(args[0]);
            if (user == null) return;
            args.RemoveAt(0);
            List<(Product, int)> products = new();
            if (args.Count == 0)
            {
                Delay = 5000;
                CLI.Reset();
                CLI.DisplayUserInfo(user);
                return;
            }
            foreach (string s in args)
            {
                int count = 1;
                Product product;
                if (s.Contains(":"))
                {
                    product = GetProductById(int.Parse(s.Split(":").ToList()[0]));
                    count = int.Parse(s.Split(":").ToList()[1]);
                } else
                {
                    product = GetProductById(int.Parse(s));
                }
                if (product == null) return;
                products.Add((product, count));
            }
            Buy(user, products);
            
        }
        #region AdminCommands
        private void QuitAction()
        {
            Delay = 0;
            CLI.Close();
            StregSystem.Close();
            _running = false;
        }
        public void ActivateAction()
        {
            Product product = StregSystem.GetProductById(int.Parse(Command.Split(" ").ToList()[1]));
            SetProductActive(product, true);
        }
        public void DeActivateAction()
        {
            Product product = StregSystem.GetProductById(int.Parse(Command.Split(" ").ToList()[1]));
            SetProductActive(product, false);
        }
        private void SetProductActive(Product product, bool Active)
        {
            product.IsActive = Active;
            Delay = 0;
        }
        private void SetProductCredit(Product product, bool Credit)
        {
            product.CanBeBoughtOnCredit = Credit;
            Delay = 0;
        }
        private void CreditOffAction()
        {
            Product product = StregSystem.GetProductById(int.Parse(Command.Split(" ").ToList()[1]));
            SetProductCredit(product, false);
        }

        private void CreditOnAction()
        {
            Product product = StregSystem.GetProductById(int.Parse(Command.Split(" ").ToList()[1]));
            SetProductCredit(product, true);
        }
        private void AddCreditsAction()
        {
            User user = StregSystem.GetUserByUsername(Command.Split(" ").ToList()[1]);
            decimal Balance = decimal.Parse(Command.Split(" ").ToList()[2]);
            StregSystem.AddCreditsToAccount(user, Balance);
        }
        #endregion
    }
}
