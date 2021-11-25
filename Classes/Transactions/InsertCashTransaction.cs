using System;
using EksamenOpgave.Exceptions;

namespace EksamenOpgave
{
    public class InsertCashTransaction : Transaction, ITransaction
    {
        public override string ToString()
        {
            return $"Indbetaling: {base.ToString()}";
        }
        public void Execute() {
            User.Balance += Amount;
        }
        public InsertCashTransaction(int id, User user, DateTime date, decimal amount) : base(id, user, date, amount)
        {
        }

    }
}
