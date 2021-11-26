using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EksamenOpgave.CLI;
using EksamenOpgave;
using EksamenOpgave.Exceptions;

namespace EksamenOpgave.Controller
{
    public class StregSystemController
    {
        StregSystem StregSystem { get; set; }
        IStregsystemUI CLI { get; set; }
        public StregSystemController(StregSystem stregSystem, IStregsystemUI CLI)
        {
            this.CLI = CLI;
            StregSystem = stregSystem;
        }
        public void Start()
        {
            CLI.Start();
            WaitForInput();

        }
        public void WaitForInput()
        {
            ParseCommand(Console.ReadLine());
        }
        public void ParseCommand(string command)
        {
            List<string> args = command.Split(" ").ToList();
            User user;
            try
            {
                user = StregSystem.GetUserByUsername(args[0]);
            } catch (UserNotFoundException ex)
            {
                CLI.DisplayUserNotFound(args[0]);
            }
        }
    }
}
