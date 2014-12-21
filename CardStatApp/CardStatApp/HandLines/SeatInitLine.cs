using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardStatApp.HandLines
{
    class SeatInitLine : ILine
    {
        //Seat 1: sticks828 (27361 in chips) 
        //Seat 7: SenSej19 (26391 in chips) is sitting out
        //Seat 5: EMG (Bs As) (2208 in chips)
        //Seat 2: rubi8787 (4000 in chips) is sitting out
        //Seat 5: pepillo1940 (1500 in chips) out of hand (moved from another table into small blind)
        public int SeatNumber { get; set; }
        public string PlayerName { get; set; }
        public int ChipCount { get; set; }
        public bool IsSittingOut { get; set; }
        public bool IsOutOfHand { get; set; }

        public SeatInitLine(string line)
        {
            IsSittingOut = line.Contains("is sitting out");
            SeatNumber = Convert.ToInt32(line.Split(':')[0].Split(' ')[1]);
            PlayerName = line.Split(':')[1].Split('(')[0].Trim();
            IsOutOfHand = (line.Contains(" out of hand"));
            if (IsOutOfHand )
            {
                line = line.Replace(" out of hand (moved from another table into small blind)", "");
            }
            else
            {
                int chipStart = line.LastIndexOf("(");
                int chipEnd = line.IndexOf(")", chipStart);
                int chipLen = chipEnd - chipStart;
                string chipString = line.Substring(chipStart + 1, chipLen).Replace(" in chips)", "").Trim();
                ChipCount = Convert.ToInt32(chipString);
            }           
        }
    }
}
