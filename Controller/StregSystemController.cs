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

            decimal TotalPrice = products.Where(p => p.Item1.CanBeBoughtOnCredit == false).Sum(p => p.Item1.Price * p.Item2);
            if (TotalPrice > user.Balance)
            {
                Delay = 5000;
                CLI.DisplayInsufficientCash(user, products);
                return;
            }
            Delay = products.Count * 2500;
            foreach (var product in products)
            {
                ITransaction t = null;
                for (int i = 0; i < product.Item2; i++)
                {
                    StregSystem.BuyProduct(user, product.Item1);
                    t = StregSystem.GetTransactions(user, 1).ToList()[0];
                }
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
            Command = command;
            if (command.StartsWith(":"))
            {
                Action a = AdminCommands.Where(p => p.Key == command.Split(" ").ToList()[0].ToLower()).First().Value;
                if (a != null)
                    a();
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
            CLI.Close();
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
        #endregion
    }
}
