using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robotics
{
    class oddsprocessing
    {
        public oddsprocessing() { FileList = new List<string>(); }
        public List<string> FileList;

        public void ReadFile(string FileName)
        {
            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file =
               new System.IO.StreamReader(FileName);
            while ((line = file.ReadLine()) != null)
            {
                if (line.Length != 0)
                    FormatFile(line);
            }

            file.Close();
        }

        public void FormatFile(string line)
        {
            //format of game from text file
            /*
            2003-09-04 Jets at Redskins
            Team Name	\tSpread	\tOver/Under
            Jets	\t3	\t39
            Redskins	\t-3	\t39
            */
            var a = line.Split(' ');

            foreach (var token in a)
            {
                //if token contains /t split and add to list, else add to list
                string[] b;
                if (token.Contains('\t'))
                {
                    b = token.Split('\t');

                    foreach (var token2 in b)
                    { FileList.Add(token2); }
                }
                else
                    FileList.Add(token);
            }
        }
    }
}
