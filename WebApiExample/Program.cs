using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ServiceStack;
using ServiceStack.Text;

namespace WebApiExample
{
    class Program
    {
        static void Main(string[] args)
        {
            question1();
            question2();
            question3();
            question4();

            Console.ReadLine();



        }

        // retrieve daily prices for MSFT
        static void question1()
        {
            var dailyPrices = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=MSFT&apikey=7NYTTIF0YYUKXWIQ&datatype=csv&outputsize=full"
                           .GetStringFromUrl().FromCsv<List<AlphaVantageData>>();

            var dateCriteria = DateTime.Now.Date.AddDays(-7);

            var sumWeek = (from d in dailyPrices
                           where d.Timestamp >= DateTime.Now.Date.AddDays(-7)
                           select d.Volume);

            var averageWeek = sumWeek.Sum() / sumWeek.Count();

            Console.WriteLine("Average volume of MSFT in the past 7 days: " + averageWeek.ToString());
        }

        //Find the highest closing price of AAPL in the past 6 months 
        static void question2()
        {
            var dailyPrices = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=AAPL&apikey=7NYTTIF0YYUKXWIQ&datatype=csv&outputsize=full"
                       .GetStringFromUrl().FromCsv<List<AlphaVantageData>>();

            var closingPrices = (from d in dailyPrices
                                 where d.Timestamp >= DateTime.Now.Date.AddMonths(-6)
                                 select d.Close);

            var highestClosingPrice = closingPrices.Max();
            Console.WriteLine("Highest closing price of AAPL in the past 6 months: " + highestClosingPrice.ToString());
        }

        //Find the difference between open and close price for BA for every day in the last month
        static void question3()
        {
            var dailyPrices = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=BA&apikey=7NYTTIF0YYUKXWIQ&datatype=csv&outputsize=full"
           .GetStringFromUrl().FromCsv<List<AlphaVantageData>>();


            var differencePrices = (from d in dailyPrices
                                    where d.Timestamp >= DateTime.Now.Date.AddMonths(-1)
                                    select d.Open - d.Close);
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("Difference between open and close price for BA for every day in the last month: ");
            foreach (var price in differencePrices)
            {
                Console.WriteLine(price.ToString());
            }
            Console.WriteLine("----------------------------------------------------------");
        }

        //Given a list of stock symbols, find the symbol with the largest return over the past month
        static void question4()
        {
            var symbols = new List<string> { "MSFT", "AAPL", "BA" };
            List<Symbol> returns = new List<Symbol>();

            foreach (var symbol in symbols)
            {

                var url = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=" + symbol + "&apikey=7NYTTIF0YYUKXWIQ&datatype=csv&outputsize=full";
                var Prices = url.GetStringFromUrl().FromCsv<List<AlphaVantageData>>();

                var returnValue = (from d in Prices
                                   where d.Timestamp >= DateTime.Now.Date.AddMonths(-1)
                                   select d.Volume).Sum();

                returns.Add(new Symbol { name = symbol, returnValue = returnValue });

            }
            Symbol highestReturn = returns.OrderByDescending(i => i.returnValue).FirstOrDefault();
            Console.WriteLine("Given a list of stock symbols, find the symbol with the largest return over the past month: " + highestReturn.name + " " + highestReturn.returnValue);
        }
    }




}
