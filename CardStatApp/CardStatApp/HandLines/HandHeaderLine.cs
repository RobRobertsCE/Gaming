using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardStatApp.HandLines
{
    class HandHeaderLine : ILine
    {
        //PokerStars Hand #126747213121: Tournament #1066955653, 4500+500 Hold'em No Limit - Level XVI (1250/2500) - 2014/12/12 16:36:57 ET
        //PokerStars Zoom Hand #127128892645:  Hold'em No Limit (5/10) - 2014/12/19 12:51:06 ET
        public string Site { get; set; }
        public string HandNo { get; set; }
        public string TourneyNo { get; set; }
        public string Game { get; set; }
        public string Level { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsZoom { get; set; }
        public bool IsTournament { get; set; }

        public HandHeaderLine(string line)
        {
            if (line.Contains("Tournament "))
            {
                IsTournament = true;
                HandNo = line.Split(':')[0].Split('#')[1];
                TourneyNo = line.Split(':')[1].Split('#')[1].Split(',')[0];
            }
            else
            {
                HandNo = line.Split(':')[0].Split('#')[1];
                TourneyNo = String.Empty;
            }           

            if (line.Contains("Zoom"))
            {
                IsZoom = true;
                // PokerStars Zoom Hand #127128892645:  Hold'em No Limit (5/10) - 2014/12/19 12:51:06 ET
                Game = "Zoom " + line.Split(':')[1].Split('-')[0].Trim();
                Level = "(" + line.Split('(')[1].Split(')')[0] + ")";
                Timestamp = DateTime.Parse(line.Split('-')[1].Replace("ET", "").Trim());
            }
            else
            {
                if (line.Contains("Tournament"))
                {
                    IsTournament = true;
                    string[] buffer = line.Split(',')[1].Split('-');
                    Game = buffer[0].Trim();
                    Level = buffer[1].Trim();
                    Timestamp = DateTime.Parse(buffer[2].Replace("ET", "").Trim());
                }
                else
                {
                    // PokerStars Hand #127121837188:  Hold'em No Limit (1/2) - 2014/12/19 10:24:36 ET
                    Game = line.Split(':')[1].Split('-')[0].Trim();
                    Level = "(" + line.Split('(')[1].Split(')')[0] + ")";
                    Timestamp = DateTime.Parse(line.Split('-')[1].Replace("ET", "").Trim());
                }               
            }           
        }
    }
}
