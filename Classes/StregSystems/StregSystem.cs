using System;
using System.Linq;
using System.Collections.Generic;
using EksamenOpgave.Exceptions;


namespace EksamenOpgave
{
    public class StregSystem 
    {
        public List<ITransaction> Transactions { get; set; } = new();
        public List<User> Users { get; set; } = new() { 
            new(1, "Mads", "Morten", "mm", "mm@mm.dk", 0) 
        };
        public List<Product> Products { get; set; } = new() { 
            new Product(1, "Kage", 10, true, true),
            new Product(2, "Mælk", 420, true, false),
            new Product(3, "Agurk", 666, false, true),
            new Product(4, "Pizza", 1337, false, false)
        };
        public StregSystem()
        {

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
