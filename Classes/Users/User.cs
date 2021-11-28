using System;
using System.Collections.Generic;
using System.Linq;
using EksamenOpgave.Exceptions;


namespace EksamenOpgave
{
    public class User : IComparable<User>
        {
        //public delegate void UserBalanceLow(object source, UserBalanceEventArgs e);
        public event EventHandler<UserBalanceEventArgs> BalanceLow;

        protected virtual void OnBalanceLow()
        {
            BalanceLow?.Invoke(this, new UserBalanceEventArgs() { User = this, Balance = Balance });
        }

        #region Private Fields
        private string _lastname;
        private string _firstname;
        private string _username;
        private string _email;
        private decimal _balance;
        #endregion
        public User(int id, string firstName, string lastName, string userName, string email, decimal balance)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            UserName = userName;
            Email = email;
            Balance = balance;
        }
        #region Public Properties
        public int Id { get; set; }
        public string FirstName
        {
            
            get => _firstname;
            set => _firstname = value ?? throw new NullReferenceException($"value was null {value}");
        }
        public string LastName
        {
            get => _lastname;
            set => _lastname = value ?? throw new NullReferenceException($"value was null {value}");
        }
        public string UserName {
            get => _username;
            set => _username = value.ToList().All(x => char.IsLetterOrDigit(x) || x == '_') ? value : throw new NullReferenceException();
        }
        public string Email
        {
            get => _email;
            set
            {
                if (value.ToList().FindAll(x => x == '@').Count != 1) throw new ArgumentException(null, nameof(value));
                List<char> localChars = new() { '_', '-', '.', ',' };
                List<char> domainChars = new() { '-', '.' };
                string localPart = value.Split("@")[0];
                string domain = value.Split("@")[1];

                if (!localPart.ToList().All(x => char.IsLetterOrDigit(x) || localChars.Contains(x))) throw new ArgumentException();
                if (!domain.ToList().All(x => char.IsLetterOrDigit(x) || domainChars.Contains(x))) throw new ArgumentException();
                if (domainChars.Contains(domain.ToList().First()) || domainChars.Contains(domain.ToList().Last())) throw new ArgumentException();
                if (!domain.Contains(".")) throw new ArgumentException();
                _email = value;
            }
        }
        public decimal Balance
        {
            get => _balance;
            set
            {
                _balance = value;
                if (_balance < 50)
                {
                    OnBalanceLow();
                }
            }
        }
        #endregion

        #region Public Overrides
        public override string ToString()
        {
            return $"{FirstName} {LastName} ({Email}) // {Balance}";
        }

        
        public override int GetHashCode()
        {
            return Id;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        #endregion

        #region Interface Implementations
        public int CompareTo(User other)
        {
            return Id.CompareTo(other.Id);
        }
        #endregion
    }
}
