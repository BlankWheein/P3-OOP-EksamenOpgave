using System;
using EksamenOpgave.Exceptions;
namespace EksamenOpgave
{
    public class BuyTransaction : Transaction, ITransaction
    {
        public BuyTransaction(int id, User user, DateTime date, Product product) : base(id, user, date)
        {
            Product = product;
        }
        public Product Product { get; set; }
        public override string ToString()
        {
            return $"Køb: {Product}, {base.ToString()}";
        }
        public void Execute() {
            if (Product.IsActive == false) throw new ProductNotActiveException();
            if (Product.CanBeBoughtOnCredit == true)
            {
                User.Balance -= Product.Price;
                return;
            } else if (User.Balance - Product.Price < 0)
                throw new InsufficientCreditsException();
            User.Balance -= Product.Price;
        }
        

    }
}
