using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robotics
{
    class oddsprocessing
    {
        public oddsprocessing() { FileList = new List<string>(); TeamNickname = new List<string>(); AddNickname(); }
        public List<string> FileList;
        public List<string> TeamNickname;

        public void AddNickname() {

            System.IO.StreamReader fileNN =
            new System.IO.StreamReader("C:\\Users\\Lance\\Source\\Repos\\NFLPicks\\NFLNickname.txt");

            string l;
            while ((l = fileNN.ReadLine()) != null) {
                TeamNickname.Add(l);
            }

            fileNN.Close();
        }

        public void ReadFile(string FileName)
        {
            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(FileName);
            while ((line = file.ReadLine()) != null)
            {
                if (line.Length != 0)
                    FileList.Add(FormatFile(line));
                    
            }

            file.Close();
        }

        public string FormatFile(string line)
        {
            //format of game from text file
            /*
            2003-09-04 Jets at Redskins
            Team Name	\tSpread	\tOver/Under
            Jets	\t3	\t39
            Redskins	\t-3	\t39
            */

            string[] b;
            b = line.Split('\t');
            
            line = string.Join(" ",b);
            return line;
        }

        public void ConvertToGame() {
            OddsData game = new OddsData();
            decimal oddsCheck = new decimal();

            /*
            2003-09-04 Jets at Redskins
            Team Name	\tSpread	\tOver/Under
            Jets	\t3	\t39
            Redskins	\t-3	\t39
            */

            foreach (var g in FileList) {
                
                DateTime gameTime = new DateTime();

                var splitLine = g.Split(' ');
                int i = 0;  //Reset counter
                

                while (i < splitLine.Count()) {
                    //firstline(2003-09-04 Jets at Redskins)
                    if ((DateTime.TryParse(splitLine[i], out gameTime))) {
                        //Date Parsed
                        game.Date = gameTime;
                        ++i;    //go to next token
                        
                        var temp = i;   //peek variable
                        ++temp; //peek
                        var vTeamFound = TeamNickname.Find(n => n.TrimEnd() == splitLine[i]);

                        if (temp < splitLine.Count() && splitLine[temp] == "at" && vTeamFound != null) { //if at is peek away team found

                            game.VisTeam = vTeamFound.TrimEnd();
                            ++i;    //actual count catch up to peek(temp)
                        }
                        
                        ++i;    //next token past at on to hometeam
                        var hTeamFound = TeamNickname.Find(n => n.TrimEnd() == splitLine[i]);

                        if (hTeamFound != null) {

                            game.HomeTeam = hTeamFound.TrimEnd();
                        }   
                    }
                    
                    //second line(Team Name	\tSpread	\tOver/Under)
                    if (i == 0 && splitLine[i] == "Team")
                        i = splitLine.Count();  //skip, line not needed


                    //third line(Jets	\t3	\t39)
                    if (i == 0 && splitLine[i] == game.VisTeam) {

                        ++i;    //visteam found, add spread
                        game.VisSpread = splitLine[i];
                        ++i;    //nextToken
                        oddsCheck = Convert.ToDecimal(splitLine[i]);
                    }

                    //FourthLine(Redskins	\t-3	\t39)
                    if (i == 0 && splitLine[i] == game.HomeTeam) {
                        ++i;     //hometeam found, add spread, check odds and add
                        game.HomeSpread = splitLine[i];
                        ++i;    //nextToken
                        if (oddsCheck == Convert.ToDecimal(splitLine[i]))
                            game.OverUnder = oddsCheck;
                    }

                    i++;  //prevent inf loop.
                }

                

            }
        }
    }
}
