using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robotics
{
    public class DataProcessing
    {
        public List<List<OddsData>> ListAllOdds;
        public List<List<GameData>> ListAllStats;
        public List<HomeTeam> HomeWin;
        public List<VisTeam> VisWin;
        public List<HomeTeam> HomeLost;
        public List<VisTeam> VisLost;
        public List<string> TeamNames;
        public List<Rank> TeamRankings;

        public const int WEEKS = 12;

        public DataProcessing()
        {
            ListAllOdds = new List<List<OddsData>>();
            ListAllStats = new List<List<GameData>>();
            HomeWin = new List<HomeTeam>();
            VisWin = new List<VisTeam>();
            HomeLost = new List<HomeTeam>();
            VisLost = new List<VisTeam>();
            TeamNames = new List<String>();
            TeamRankings = new List<Rank>();
        }
        public void addOdds(List<OddsData> a)
        {
            ListAllOdds.Add(a);
        }

        public void addStats(List<GameData> a)
        {
            ListAllStats.Add(a);
        }

        public void addallTeamNames()
        {
            foreach (GameData g in ListAllStats[0])
            {
                TeamNames.Add(g.Home.HomeName);
                TeamNames.Add(g.Visitor.VisName);
            }
        }

        public void getoddsandscore()
        {
            int length = ListAllStats.Count;
            for (int z = 0; z < length; ++z) //(RLR was here pete)
            {
                var teamsscore = ListAllStats[z].Select(x => new { x.Home.HomeName, x.Home.HScore, x.Visitor.VisName, x.Visitor.VScore });
                //var oddsscore = ListAllOdds[z].Select(x => new OddsData { HomeTeam = x.HomeTeam, HomeSpread = x.HomeSpread, VisTeam = x.VisTeam, VisSpread = x.VisSpread });
                //3 tie games since 2012, one each in 12, 13, 14

                foreach (var b in teamsscore)
                {

                    var oddsFound = ListAllOdds[z].Where(x => b.HomeName == x.HomeTeam).ToList();     //find current game in oddscore

                    if (oddsFound == null || oddsFound.Count != 1)
                        throw new NullReferenceException("OddsFound is null or found more than 1.");

                    foreach (var odds in oddsFound)
                    {
                        decimal hspDec = Convert.ToDecimal(odds.HomeSpread);
                        decimal vspDec = Convert.ToDecimal(odds.VisSpread);
                        decimal htrueScore;
                        decimal vtrueScore;
                        bool homeWins = false;
                        int marginOfVictory;

                        marginOfVictory = b.VScore - b.HScore;
                        if (b.HScore > b.VScore)
                        {
                            homeWins = true;
                            marginOfVictory = b.HScore - b.VScore;
                        }

                        if (homeWins && hspDec != 0)
                        {
                            htrueScore = hspDec + marginOfVictory; //(-spread + mov)
                            vtrueScore = htrueScore * -1;
                        }
                        else if (!homeWins && hspDec != 0)
                        {
                            vtrueScore = vspDec + marginOfVictory;
                            htrueScore = vtrueScore * -1;
                        }
                        else if (hspDec == 0)
                        {
                            htrueScore = marginOfVictory;
                            vtrueScore = marginOfVictory;
                        }
                        else if (marginOfVictory == 0)
                            throw new Exception("TIE!!");
                        else
                            throw new Exception("I haven't accounted for this scenario.");

                        decimal TrueTotal = (b.HScore + b.VScore);

                        //Find In odds list and add in TrueOdds/total
                        ListAllStats[z].Where(w => w.Home.HomeName == odds.HomeTeam).ToList().ForEach(s =>
                            {
                                s.TrueTotal = TrueTotal; s.VisSpread = odds.VisSpread; s.HomeSpread = odds.HomeSpread;
                                s.OverUnder = odds.OverUnder; s.Home.HSpread = hspDec; s.Home.OverUnder = odds.OverUnder;
                                s.Home.TrueTotal = TrueTotal; s.Home.HTrueOdds = htrueScore; s.Visitor.VSpread = vspDec;
                                s.Visitor.TrueTotal = TrueTotal; s.Visitor.VTrueOdds = vtrueScore; s.Visitor.OverUnder = odds.OverUnder;
                            });
                    }
                }
            }
        }
        public void SortWinLost(int weeksLength)
        {
            for (int i = 0; i < weeksLength; ++i)
            {
                foreach (GameData g in ListAllStats[i])
                {
                    //Home Team Wins, Vis Team Lost
                    if (g.Home.HScore > g.Visitor.VScore)
                    {
                        HomeTeam tempH = g.Home;
                        HomeWin.Add(tempH);
                        VisTeam tempV = g.Visitor;
                        VisLost.Add(tempV);
                    }
                    //Home Team Lost, ViS Team Win
                    else if (g.Home.HScore < g.Visitor.VScore)
                    {
                        HomeTeam tempH = g.Home;
                        HomeLost.Add(tempH);
                        VisTeam tempV = g.Visitor;
                        VisWin.Add(tempV);
                    }
                    else { throw new Exception("We have a tie!"); }
                }
            }
        }

        public List<Rank> PowerRankings(int weeksLength)    //must match sortWinLose
        {
            addallTeamNames();
            List<ITeam> PowerR = new List<ITeam>();

            if (TeamNames.Count != 32) { throw new Exception("There has to be 32 teams!"); }

            //Find each game from one team put into list and get power ranking, store that into a list 
            foreach (string team in TeamNames)
            {
                var temp = HomeWin.FindAll(t => t.HomeName == team);
                ListObjtoInterface(temp, PowerR);
                var temp2 = VisWin.FindAll(t => t.VisName == team);
                ListObjtoInterface(temp2, PowerR);
                var temp3 = HomeLost.FindAll(t => t.HomeName == team);
                ListObjtoInterface(temp3, PowerR);
                var temp4 = VisLost.FindAll(t => t.VisName == team);
                ListObjtoInterface(temp4, PowerR);
                //Checking to see if there are the right amount of games with/without byeweek 
                // The range is count < 11 & count > 12, if WEEKS = 12, should be 11 or 12 games
                //if (PowerR.Count < (weeksLength - 1) || PowerR.Count > weeksLength)
                    //throw new Exception("Too few games for current team");

                Rank r = new Rank();
                /*
                The "advanced" stats work as 
                RYA - Rushing yards per att, higher # the better
                AYA - Passing yards per att, higher # the better
                IPA - Interceptions per comp, higher # the worse
                TPAA - Turnovers per att (fumbleslost+int)/totalatt, rush&pass, higher # the worse
                TE - (score/totatt) * FirstDowns, higher the better 
                */
                foreach (ITeam t in PowerR)
                {
                    var te = t.GetType();
                    double RYA;
                    double AYA;
                    double IPA;
                    double TPAA;
                    double TE;
                    decimal trueOdds;
                    string teamName;

                    if (te.Name == "HomeTeam")
                    {
                        HomeTeam a = new HomeTeam();
                        a = t as HomeTeam;
                        double totalAttempts = (a.HRushingAttempts + a.HPassingAttempts);

                        RYA = (double)a.HRushingYards / a.HRushingAttempts; //Rushing Yards per attempt
                        AYA = (double)a.HPassingYards / a.HPassingAttempts; //Pass yard per attempt
                        IPA = (double)a.HInterceptionsThrown / a.HPassingCompletions;    //Int per comp
                        TPAA = (double)(a.HFumblesLost + a.HInterceptionsThrown) / totalAttempts;   //Turnovers per attempt
                        TE = (double)(a.HScore / totalAttempts) * a.HFirstDowns;    //play efficiency, (score/top) * FirstDowns

                        trueOdds = a.HTrueOdds;
                        teamName = a.HomeName;
                    }
                    else
                    {
                        VisTeam a = new VisTeam();
                        a = t as VisTeam;
                        double totalAttempts = (a.VRushingAttempts + a.VPassingAttempts);

                        RYA = (double)a.VRushingYards / a.VRushingAttempts; //Rushing Yards per attempt
                        AYA = (double)a.VPassingYards / a.VPassingAttempts; //Pass yard per attempt
                        IPA = (double)a.VInterceptionsThrown / a.VPassingCompletions;    //Int per comp
                        TPAA = (double)(a.VFumblesLost + a.VInterceptionsThrown) / totalAttempts;   //Turnovers per attempt
                        TE = (double)(a.VScore / totalAttempts) * a.VFirstDowns;    //play efficiency, (score/top) * FirstDowns

                        trueOdds = a.VTrueOdds;
                        teamName = a.VisName;
                    }

                    r.mTeamName = teamName;
                    r.mRYA += RYA;
                    r.mAYA += AYA;
                    r.mIPA += IPA;
                    r.mTPAA += TPAA;
                    r.mTE += TE;
                    r.mTrueScore += trueOdds;
                }
                r.mRYA /= PowerR.Count();
                r.mAYA /= PowerR.Count();
                r.mIPA /= PowerR.Count();
                r.mTPAA /= PowerR.Count();
                r.mTE /= PowerR.Count();

                TeamRankings.Add(r);
                PowerR.Clear();
            }

            return TeamRankings;
        }

        public List<Rank> GetLastWeekTrueScore(int week, int howmany)   //what week to get, can go back x weeks, how many do you go back?
        {
            List<Rank> LastTS = new List<Rank>();
            if (TeamNames.Count == 0)
                addallTeamNames();
            foreach (string name in TeamNames)
            {
                for (int i = week - howmany; i < week; ++i)
                {
                    GameData homeTemp = ListAllStats[i].Find(w => w.Home.HomeName == name);
                    GameData awayTemp = ListAllStats[i].Find(w => w.Visitor.VisName == name); ;
                    Rank r = new Rank();

                    if (homeTemp == null && awayTemp == null) {
                        r.mTrueScore = 0;
                    }
                    else if (homeTemp == null) {
                        r.mTeamName = awayTemp.Visitor.VisName;
                        r.mTrueScore = awayTemp.Visitor.VSpread; }
                    else {
                        r.mTeamName = homeTemp.Home.HomeName;
                        r.mTrueScore = homeTemp.Home.HSpread; }
                        
                    LastTS.Add(r);
                }
                
            }
            return LastTS;
        }

        public List<ITeam> ListObjtoInterface<T>(List<T> a, List<ITeam> o)
        {
            foreach (object obj in a)
            {
                ITeam b = obj as ITeam;
                o.Add(b);
            }
            return o;
        }

    }
}
