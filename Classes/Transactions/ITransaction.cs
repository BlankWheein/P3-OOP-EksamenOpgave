using System;

namespace EksamenOpgave
{
    public interface ITransaction
    {
        void Execute();
        public int Id { get; set; }
        User User { get; set; }
        public DateTime Date { get; set; }


    }
}
