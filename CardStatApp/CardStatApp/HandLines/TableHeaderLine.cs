using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardStatApp.HandLines
{
    class TableHeaderLine : ILine
    {
        //Table '1066955653 1' 9-max Seat #7 is the button
        //Table 'NLHE 5/10 6 max' 6-max Seat #1 is the button
        public String TableId { get; set; }
        public string Description { get; set; }
        public int ButtonSeat { get; set; }

        public TableHeaderLine(string line)
        {
            TableId = line.Split('\'')[1];
            Description = line.Split('\'')[2].Split('S')[0].Trim();
            ButtonSeat = Convert.ToInt32(line.Substring(line.IndexOf('#') + 1, 1));
        }
    }
}
