using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robotics
{
    class Program
    {
        static void Main(string[] args)
        {
            oddsprocessing a = new oddsprocessing();
            a.ReadFile("O:\\lance.wessale\\Source\\Repos\\NFLPicks\\20032016NFLBettingLines.txt");
            a.FileList.ForEach(delegate(string line) { Console.WriteLine(line); });

            string date = "2016-02-09";
            DateTime dt = Convert.ToDateTime(date);
            // Suspend the screen.
            Console.ReadLine();
        }
    }
}
