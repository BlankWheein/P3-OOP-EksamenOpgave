using System;
using System.Linq;
using System.Collections.Generic;
using EksamenOpgave.Exceptions;
using System.IO;
using System.Text.RegularExpressions;

namespace EksamenOpgave
{
    public class StregSystem 
    {
        public List<ITransaction> Transactions { get; set; } = new();
        public List<User> Users { get; set; } = new();
        public List<Product> Products { get; set; } = new();

        private void ReadProducts()
        {
            int index = 0;
            foreach (string line in File.ReadLines(@"../../../Data/products.csv"))
            {
                if (index == 0)
                {
                    index++;
                    continue;
                }
                List<string> Lines = line.Split(";").ToList();
                int Id = int.Parse(Lines[0]);
                string Name = StripHTML(Lines[1]);
                decimal Price = decimal.Parse(Lines[2]);
                bool Active = Lines[3] == "0" ? false : true;
                if (Lines[4] != "")
                {
                    Lines[4] = Lines[4].Replace("\"", "");
                    DateTime Date = DateTime.Parse(Lines[4]);
                    Products.Add(new SeasonalProduct(Id, Name, Price, Active, false, Date));
                }
                else
                {
                    Products.Add(new Product(Id, Name, Price, Active, false));
                }
                index++;
            }
        }
        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
        private void ReadUsers()
        {
            int index = 0;
            foreach (string line in File.ReadLines(@"../../../Data/users.csv"))
            {
                if (index == 0)
                {
                    index++;
                    continue;
                }
                List<string> Lines = line.Split(",").ToList();
                int Id = int.Parse(Lines[0]);
                string FirstName = Lines[1];
                string LastName = Lines[2];
                string UserName = Lines[3];
                decimal Balance = decimal.Parse(Lines[4]);
                string Email = Lines[5];
                Users.Add(new User(Id, FirstName, LastName, UserName, Email, Balance));
                index++;
            }
        }
        public StregSystem()
        {
            ReadProducts();
            ReadUsers();
        }
        public IEnumerable<Product> ActiveProducts()
        {
            foreach (var product in Products.FindAll(p => p.IsActive == true))
                yield return product;
        }

        public void AddCreditsToAccount(User user, decimal amount)
        {
            InsertCashTransaction t;
            try
            {
                t = new(Transactions.Count + 1, user, DateTime.Now, amount);
                ExecuteTransaction(t);
            } catch
            {
                throw;
            }
        }

        public void BuyProduct(User user, Product product)
        {
            BuyTransaction t;
            try
            {
                t = new(Transactions.Count + 1, user, DateTime.Now, product);
                ExecuteTransaction(t);
            }
            catch
            {
                throw;
            }
        }

        public void ExecuteTransaction(ITransaction t)
        {
            try
            {
                t.Execute();
                Transactions.Add(t);
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<ITransaction> GetTransactions(User user, int count)
        {
            List<ITransaction> UserTransactions = Transactions.OrderByDescending(p => p.Date).ToList().FindAll(p => p.User.Id == user.Id);
            if (UserTransactions.Count > count)
            UserTransactions.RemoveRange(count, UserTransactions.Count - 1);
            foreach (ITransaction t in UserTransactions)
                yield return t;
        }
        public Product GetProductById(int id)
        {
            Product p = Products.Find(p => p.Id == id);
            if (p == null)
                throw new ProductDoesNotExistException();
            return p;
        }

        public User GetUser(Func<User, bool> predicate)
        {
            User user = Users.Where(predicate).FirstOrDefault();
            if (user == null)
                throw new UserNotFoundException();
            return user;
        }
        #region Why
        public User GetUserByUsername(string username)
        {
            return GetUser(p => p.UserName == username);
        }
        #endregion
    }
}
