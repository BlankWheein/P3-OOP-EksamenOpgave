using System;
using System.Collections.Generic;

namespace EksamenOpgave
{
    public interface IStregSystem
    {
        List<Product> Products { get; set; }
        List<ITransaction> Transactions { get; set; }
        List<User> Users { get; set; }

        IEnumerable<Product> ActiveProducts();
        void AddCreditsToAccount(User user, decimal amount);
        void BuyProduct(User user, Product product);
        void Close();
        void ExecuteTransaction(ITransaction t);
        Product GetProductById(int id);
        IEnumerable<ITransaction> GetTransactions(User user, int count);
        User GetUser(Func<User, bool> predicate);
        User GetUserByUsername(string username);
    }
}