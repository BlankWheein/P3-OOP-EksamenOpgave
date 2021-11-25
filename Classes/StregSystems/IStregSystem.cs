using System;
using System.Collections.Generic;

namespace EksamenOpgave
{
    public interface IStregSystem
    {
        BuyTransaction BuyProduct(User user, Product product);
        InsertCashTransaction AddCreditsToAccount(User user, decimal amount);
        Product GetProductById(int id);
        User GetUser(Func<User, bool> predicate);
        User GetUserByUsername(string username);
        IEnumerable<Transaction> GetTransactions(User user, int count);
        IEnumerable<Product> ActiveProducts();
    }
}
