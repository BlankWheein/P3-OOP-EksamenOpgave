using System;
using EksamenOpgave.Exceptions;


namespace EksamenOpgave
{
    public class Transaction : ITransaction
    {
        #region Private Properties
        private User _user;
        #endregion
        protected Transaction(int id, User user, DateTime date)
        {
            Id = id;
            User = user;
            Date = date;
        }
        public Transaction(int id, User user, DateTime date, decimal amount) : this(id, user, date)
        {
            Amount = amount;
        }
        public int Id { get; set; }
        public User User { 
            get => _user; 
            set => _user = value ?? throw new NullReferenceException(); 
        }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        public override string ToString()
        {
            return $"{Id}, {User}, {Amount}, {Date}";
        }
        public virtual void Execute() { }
    }
}
