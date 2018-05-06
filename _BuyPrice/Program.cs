using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicallyMe.RobinhoodNet;
using BasicallyMe.RobinhoodNet.Raw;
using Newtonsoft.Json.Linq;
using Shared;

namespace _BuyPrice
{
    class Program
    {
        static void Main(string[] args)
        {
            Setup.Login().Wait();
            Trade.WatchSymbol();
        }
    }
}
