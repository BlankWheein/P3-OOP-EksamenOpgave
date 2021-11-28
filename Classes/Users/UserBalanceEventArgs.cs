using System;


namespace EksamenOpgave
{
    public class UserBalanceEventArgs : EventArgs
    {
        public User User { get; set; }
        public decimal Balance { get; set; }
    }
}
