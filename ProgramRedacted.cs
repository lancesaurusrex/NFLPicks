using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;


namespace Robotics
{
    class ProgramRedacted
    {
        static void Redacted() 
        //static void Main(string[] args)
        {
            string FileNameOdds;
            string FileNameStats;
            string projdir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;

            DataProcessing dp = new DataProcessing();

            //fileName nflodds2015weekX and nflstats2015weekX
            int currentWeek = 13;

            for (int i = 1; i < currentWeek + 1; ++i)
            {
                FileNameOdds = "nflodds2015week" + i + ".csv";
                FileNameStats = "nflstats2015week" + i + ".csv";

                string FullPath = projdir + "\\" + FileNameOdds;      //ROOT

                using (var csvC = new CsvReader(File.OpenText(FullPath)))
                {
                    csvC.Configuration.IgnoreHeaderWhiteSpace = true;
                    csvC.Configuration.TrimFields = true;

                    dp.addOdds(csvC.GetRecords<OddsData>().ToList());
                }

                FullPath = projdir + "\\" + FileNameStats;        //ROOT

                using (var csvC = new CsvReader(File.OpenText(FullPath)))
                {
                    csvC.Configuration.IgnoreHeaderWhiteSpace = true; ;
                    csvC.Configuration.TrimFields = true;
                    csvC.Configuration.RegisterClassMap<CustomClassMap>();

                    dp.addStats(csvC.GetRecords<GameData>().ToList());
                }
            }



            dp.getoddsandscore();
            for (int i = 0; i < dp.ListAllStats.Count(); ++i) {
                foreach (GameData g in dp.ListAllStats[i]) {
                    Console.WriteLine("{0} {1, -2} {2, -10} {3,-4} {4, -6} {5, -7} {6, -10} {7} {8} {9}", "Week", i + 1, g.Home.HomeName, g.Home.HScore, g.HomeSpread, g.Home.HTrueOdds, g.Visitor.VisName, g.Visitor.VScore, g.VisSpread, g.Visitor.VTrueOdds);
                }

                  
            }
            dp.SortWinLost(currentWeek);     //Do Sort for Weeks 1-11 HINT:Array based 0 = week 1
            List<Rank> TR = new List<Rank>();
            TR = dp.PowerRankings(currentWeek);
            Console.WriteLine("{0, -10} {1, -5} {2, -5} {3, -5} {4, -5} {5, -5} {6, -7} {7}", "TeamName", "RYA", "AYA", "IPA", "TPAA", "TE", "TrueScore", "Total");

            List<Rank> PRTotal = new List<Rank>();  //total in list with teamname and total
            foreach (Rank r in TR)
            {
                Console.Write("{0, -10} {1:0.000} {2:0.000} {3:0.000} {4:0.000} {5,-6:0.000} {6, 5} {7}", r.mTeamName, r.mRYA, r.mAYA, r.mIPA, r.mTPAA, r.mTE, r.mTrueScore, "  ");
                double total = r.mRYA + r.mAYA + r.mIPA + r.mTPAA + r.mTE;
                Console.WriteLine("{0:0.000}", total);

                Rank t = new Rank();
                t.mTeamName = r.mTeamName;
                t.mTrueScore = (decimal)total;
                PRTotal.Add(t);
            }



            string fileName13 = "nflodds2015week13.csv";
            string path13 = projdir + "\\" + fileName13;
            List<OddsData> week13Data = new List<OddsData>();
            //week 13 odds
            using (var csvC = new CsvReader(File.OpenText(path13)))
            {
                csvC.Configuration.IgnoreHeaderWhiteSpace = true;
                csvC.Configuration.TrimFields = true;

                while (csvC.Read())
                    week13Data.Add(csvC.GetRecord<OddsData>());
            }

            //List<Rank> LastWeek = new List<Rank>();
            //LastWeek = dp.GetLastWeekTrueScore(11, 1);

            //var home = from a in PRTotal
            //             join b in week13Data on a.mTeamName equals b.HomeTeam
            //             select new { a, b };   //common a truescore, common b spread

            //var away = from a in PRTotal
            //                      join b in week13Data on a.mTeamName equals b.VisTeam
            //                      select new { a, b };   //common a truescore, common b spread
            Console.WriteLine("{0, -10} {1, 5:0.000} {2, 5:0.000} {3, 5:0.000} {4,5:0.000} {5,-6:0.000} {6, 5:0.000} {7:0.000}",
                        "HomeTeam", "HPwrRate", "HomeSprd", "VisTeam", "VPRate", "VisSprd", "TPRate", "totPwrRate+3");
            foreach (OddsData o in week13Data)
            {
                if (o != null)
                {
                    var homeTotal = PRTotal.Find(h => h.mTeamName == o.HomeTeam);
                    var awayTotal = PRTotal.Find(v => v.mTeamName == o.VisTeam);
                    var homeweek13 = week13Data.Find(h => h.HomeTeam == o.HomeTeam);
                    var awayweek13 = week13Data.Find(v => v.VisTeam == o.VisTeam);

                    var homePR = homeTotal.mTrueScore;
                    var awayPR = awayTotal.mTrueScore;
                    var homespread = Convert.ToDecimal(homeweek13.HomeSpread);
                    var awayspread = Convert.ToDecimal(awayweek13.VisSpread);

                    decimal totalPR;
                    decimal totalPR3 = 0;
                    if (homePR > awayPR)
                    {
                        totalPR = homePR - awayPR;
                        totalPR3 = totalPR + 3;
                    }
                    else
                        totalPR = awayPR - homePR;
                    
                    Console.WriteLine("{0, -10} {1,5:0.000} {2,-8:0.000} {3,-8:0.000} {4,5:0.000} {5,-6:0.000} {6, -9:0.000} {7,-8:0.000}",
                        o.HomeTeam, homePR, homespread, o.VisTeam, awayPR, awayspread, totalPR, totalPR3);
                }
            }
        }
    }
}
