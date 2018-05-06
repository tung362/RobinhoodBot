using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BasicallyMe.RobinhoodNet;
using BasicallyMe.RobinhoodNet.Raw;
using Newtonsoft.Json.Linq;

namespace Shared
{
    public class Setup
    {
        public static RobinhoodClient Client;
        public static Account UserAccount;
        public static string Username = "";
        public static string Password = "";
        public static string CurrentStockSymbol = "NVCN";
        public static decimal StopLossPrice = 0.0610m;

        public static async Task Login()
        {
            //Read login credentials
            StreamReader loginFile = File.OpenText("Login.login");
            Username = loginFile.ReadLine();
            Password = loginFile.ReadLine();

            Client = new RobinhoodClient();
            await Client.Authenticate(Username, Password);
            UpdateAccountInfo();
        }

        public static void UpdateAccountInfo()
        {
            UserAccount = Client.DownloadAllAccounts().Result.First();
        }
    }
}
