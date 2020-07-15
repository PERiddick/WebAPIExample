using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Text;

namespace WebApiExample
{
    class Program
    {
        static void Main(string[] args)
        {

            // retrieve daily prices for MSFT
            var dailyPrices = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=MSFT&apikey=7NYTTIF0YYUKXWIQ&datatype=csv"
                       .GetStringFromUrl().FromCsv<List<AlphaVantageData>>();


            var dateCriteria = DateTime.Now.Date.AddDays(-7);

            var sumWeek = (from d in dailyPrices
                               where d.Timestamp >= DateTime.Now.Date.AddDays(-7)
                               select d.Volume);

            var averageWeek = sumWeek.Sum()/ sumWeek.Count();

            Console.WriteLine(averageWeek.ToString());
            Console.ReadLine();



        }
    }


}
