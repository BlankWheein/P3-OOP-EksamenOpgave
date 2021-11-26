using System;
using System.Collections.Generic;
using System.Linq;
using EksamenOpgave.CLI;
using EksamenOpgave.Controller;

namespace EksamenOpgave
{
    internal partial class Program
    {
        static void Main(string[] args)
        {
            StregSystem stregSystem = new StregSystem();
            StregSystemCLI CLI = new(stregSystem);
            StregSystemController Controller = new(stregSystem, CLI);
            Controller.Start();
        }
    }
}
