using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicallyMe.RobinhoodNet;
using BasicallyMe.RobinhoodNet.Raw;
using Newtonsoft.Json.Linq;
using Shared;


namespace _Buy
{
    class Trade
    {
        public static Instrument CurrentStockSymbol;
        private static int Quantity = 0;
        private static decimal Price = 0m;
        private static TimeInForce OrderDuration = TimeInForce.Unknown;

        public static void WatchSymbol()
        {
            Console.Clear();

            //Search for symbol
            try
            {
                CurrentStockSymbol = Setup.Client.FindInstrument(Setup.CurrentStockSymbol).Result.First(i => i.Symbol == Setup.CurrentStockSymbol);
                Start();
            }
            catch
            {
                Console.WriteLine("Cannot find stock symbol, please check symbol");
                Console.ReadKey();
            }
        }

        public static void Start()
        {
            Console.Clear();

            string discriptionText = "Press enter to buy at current price" + "\n";
            Console.WriteLine(discriptionText);

            var input = Console.ReadKey();
            if (input.Key == ConsoleKey.Enter) Confirmation();
            else Start();
        }

        static void Confirmation()
        {
            Console.Clear();
            Setup.UpdateAccountInfo();

            //Info
            var quote = Setup.Client.DownloadQuote(CurrentStockSymbol.Symbol).Result;

            decimal buyingPower = (decimal)Setup.UserAccount.MarginBalances["unallocated_margin_cash"];
            decimal currentPrice;
            if (Tools.TimeBetween(DateTime.Now.TimeOfDay, "06:30", "13:00")) currentPrice = quote.LastTradePrice;
            else currentPrice = (decimal)quote.LastExtendedHoursTradePrice;

            //Set settings
            Quantity = (int)Math.Floor(buyingPower / currentPrice);
            Price = currentPrice;
            OrderDuration = TimeInForce.GoodTillCancel;

            //Confirmation
            string confirmationText =
                "Symbol: " + CurrentStockSymbol.Symbol + "\n" +
                "Side: " + "BUY" + "\n" +
                "Price: " + Price + "\n" +
                "Quantity: " + Quantity + "\n" +
                "Duration: " + OrderDuration + "\n" +
                "\n" +
                "\n" +
                "Are you sure? Press enter to confirm" + "\n" +
                "Press escape to cancel" + "\n";
            Console.WriteLine(confirmationText);

            var input = Console.ReadKey();
            if (input.Key == ConsoleKey.Enter) PlaceOrder();
            else if (input.Key == ConsoleKey.Escape) Start();
        }

        static void PlaceOrder()
        {
            //Order settings
            NewOrderSingle newOrder = new NewOrderSingle(CurrentStockSymbol);
            newOrder.AccountUrl = Setup.UserAccount.AccountUrl;
            newOrder.Quantity = Math.Abs(Quantity);
            newOrder.Side = Side.Buy;
            newOrder.TimeInForce = OrderDuration;

            if (Price == 0) newOrder.OrderType = OrderType.Market;
            else
            {
                newOrder.OrderType = OrderType.Limit;
                newOrder.Price = Price;
            }

            //Places the order
            OrderSnapshot order = new OrderSnapshot();
            try
            {
                order = Setup.Client.PlaceOrder(newOrder).Result;
                Console.WriteLine("Order Placed");
                Console.ReadKey();

            }
            catch
            {
                Console.WriteLine("There was a problem placing the order, might be insufficent funds or shares");
                Console.ReadKey();
            }
        }
    }
}
