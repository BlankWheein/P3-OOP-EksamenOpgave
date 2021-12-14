using EksamenOpgave;
using System.Collections.Generic;

namespace EksamenOpgave.CLI
{
    public interface IStregsystemUI
    {
        void DisplayUserNotFound(string username);
        void DisplayProductNotFound(string product);
        void DisplayUserInfo(User user);
        void DisplayTooManyArgumentsError(string command);
        void DisplayAdminCommandNotFoundMessage(string adminCommand);
        void DisplayUserBuysProduct(BuyTransaction transaction);
        void DisplayUserBuysProduct(int count, BuyTransaction transaction);
        void DisplayProductNotActive(Product product);
        void Close();
        void DisplayInsufficientCash(User user, Product product);
        void DisplayInsufficientCash(User user, List<(Product, int)> product);
        void DisplayGeneralError(string errorString);
        void Start();
        void Reset();
        void Show();
    }
}
