﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicallyMe.RobinhoodNet;
using BasicallyMe.RobinhoodNet.Raw;
using Newtonsoft.Json.Linq;
using Shared;


namespace _StopLoss
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
            Setup.UpdateAccountInfo();

            string discriptionText = "Stop loss set at $" + Setup.StopLossPrice + "\n";
            Console.WriteLine(discriptionText);

            CheckLoop();
        }

        static void CheckLoop()
        {
            //Info
            var quote = Setup.Client.DownloadQuote(CurrentStockSymbol.Symbol).Result;
            decimal currentPrice;
            if (Tools.TimeBetween(DateTime.Now.TimeOfDay, "06:30", "13:00")) currentPrice = quote.LastTradePrice;
            else currentPrice = (decimal)quote.LastExtendedHoursTradePrice;

            if (currentPrice <= Setup.StopLossPrice) Confirmation();
            else CheckLoop();
        }

        static void Confirmation()
        {
            Console.Clear();
            Setup.UpdateAccountInfo();

            //Info
            var quote = Setup.Client.DownloadQuote(CurrentStockSymbol.Symbol).Result;
            var positions = Setup.Client.DownloadPositions(Setup.UserAccount.PositionsUrl.ToString());

            //Find position Index
            int positionIndex = -1;
            for (int i = 0; i < positions.Count; i++)
            {
                if (positions[i].InstrumentUrl.ToString() == CurrentStockSymbol.InstrumentUrl.ToString())
                {
                    positionIndex = i;
                    break;
                }
            }

            decimal currentPrice;
            if (Tools.TimeBetween(DateTime.Now.TimeOfDay, "06:30", "13:00")) currentPrice = quote.LastTradePrice;
            else currentPrice = (decimal)quote.LastExtendedHoursTradePrice;

            //Set settings
            Quantity = (int)positions[positionIndex].Quantity;
            Price = currentPrice;
            OrderDuration = TimeInForce.GoodTillCancel;

            PlaceOrder();
        }

        static void PlaceOrder()
        {
            //Order settings
            NewOrderSingle newOrder = new NewOrderSingle(CurrentStockSymbol);
            newOrder.AccountUrl = Setup.UserAccount.AccountUrl;
            newOrder.Quantity = Math.Abs(Quantity);
            newOrder.Side = Side.Sell;
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
