using System;
using EksamenOpgave.Exceptions;


namespace EksamenOpgave
{
    public class Product
    {

        #region Private Properties
        private int _id;
        private string _name;
        private decimal _price;

        #endregion
        public Product(int id, string name, decimal price, bool isActive, bool canBeBoughtOnCredit)
        {
            Id = id;
            Name = name;
            Price = price;
            IsActive = isActive;
            CanBeBoughtOnCredit = canBeBoughtOnCredit;
        }
        #region Public Properties
        public int Id
        {
            get => _id; set
            {
                if (value < 1) throw new ArgumentException();
                _id = value;
            }
        }
        public string Name
        {
            get => _name; set
            {
                _name = value ?? throw new NullReferenceException();
            }
        }
        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0) throw new ArgumentException();
                _price = value;
            }
        }
        public bool IsActive { get; set; }
        public bool CanBeBoughtOnCredit { get; set; }
        #endregion
        public override string ToString()
        {
            string PriceString = String.Format("{0:.0}", Price);
            string IdString = $"{Id}".PadRight(15, ' ');
            string NameString = $"{Name}".PadRight(60, ' ');
            PriceString = $"{PriceString}".PadRight(8, ' ');
            return $"{IdString} {NameString} {PriceString}";
        }
    }
}
