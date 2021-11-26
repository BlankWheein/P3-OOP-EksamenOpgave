using System;
using EksamenOpgave.Exceptions;


namespace EksamenOpgave
{
    public class SeasonalProduct : Product
    {
        public DateTime SeasonStartDate { get; set; }
        public DateTime SeasonEndDate { get; set; }
        public SeasonalProduct(int id, string name, decimal price, bool isActive, bool canBeBoughtOnCredit, DateTime Date) :base(id, name, price, isActive, canBeBoughtOnCredit)
        { 
            this.SeasonEndDate = Date;
        }

    }
}
