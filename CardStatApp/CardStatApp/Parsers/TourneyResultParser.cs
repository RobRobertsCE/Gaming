namespace CardStatApp.Parsers
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Globalization;
    using CardStatApp.Data;
    using CardStatApp.Models;

    public delegate void ProgressChangedEvent(object sender, int percentComplete);

    class TourneyResultParser
    {
        public event EventHandler TourneyResultParsed;
        public event ProgressChangedEvent ProgressChangedx;

        string playerName = "RobRoberts82";

        public string ResultDirectory { get; set; }

        public string ArchiveDirectory
        {
            get
            {
                return System.IO.Path.Combine(this.ResultDirectory, "Archive");
            }
        }

        public TourneyResultParser(string directory)
        {
            this.ResultDirectory = directory;
        }

        public void ParseResults()
        {
            if (!Directory.Exists(this.ArchiveDirectory))
            {
                Directory.CreateDirectory(this.ArchiveDirectory);
            }

            foreach (string resultFile in Directory.EnumerateFiles(this.ResultDirectory))
            {
                if (ParseResultFile(resultFile)) 
                     MoveParsedFile(resultFile);
            }
            Console.WriteLine("Done");
        }

        public void ParseFile(string resultFile)
        {
            ParseResultFile(resultFile);
        }

        bool ParseResultFile(string resultFile)
        {
            if (!File.Exists(resultFile)) return false;

            Console.WriteLine("Parsing file " + resultFile);
            try
            {                
                string[] lines = File.ReadAllLines(resultFile);
                TourneyResultModel t = new TourneyResultModel();
                string[] l = lines[0].Split(',');
                string[] l1 = l[0].Split(' ');
                t.SiteName = l1[0].Trim();
                t.TourneyNo = l1[2].Replace(',', ' ').Replace('#', ' ').Trim();
                t.GameType = l[1].Trim();

                string buyInLine = lines[1].Split(':')[1].Trim();
                t.BuyIn = buyInLine;

                if (buyInLine.Contains('/'))
                {
                    string[] b = buyInLine.Split('/');
                    int b1 = 0;
                    Int32.TryParse(b[0],out b1);
                    int b2=0;
                    Int32.TryParse(b[1],out b2);
                    t.BuyInTotal = b1+b2;
                }

                int pc = 0;
                if (!Int32.TryParse(lines[2].Split(' ')[0].Trim(), out pc))
                {
                    return false;
                }
                t.PlayerCount = pc;

                int pp = 0;
                if (!Int32.TryParse(lines[3].Split(':')[1].Trim(), out pp))
                {
                    return false;
                }
                t.TotalPrizePool = pp;

                t.StartTime = lines[4].Substring(19);

                if (ParseFinish(t, lines))
                {
                    SaveResult(t);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        bool ParseFinish(TourneyResultModel t, string[] lines)
        {
            foreach (string line in lines.Skip(5).ToArray())
            {
                if (line.Contains(playerName))
                {
                    int fp = 0;
                    if (!Int32.TryParse(line.Split(':')[0], out fp))
                    {
                        return false;
                    }
                    t.FinishPosition = fp;

                    if (line.Contains('%'))
                    {
                        int firstComma = line.IndexOf(',');
                        string payout = line.Substring(firstComma + 2); 
                        int firstParen = payout.IndexOf('(');

                        int winnings = 0;
                        if (!Int32.TryParse(payout.Substring(0, firstParen - 1), NumberStyles.Number, CultureInfo.InvariantCulture, out winnings))
                        {
                            return false;
                        }
                        t.Winnings = winnings;

                        int percent = 0;
                        if (!Int32.TryParse(payout.Substring(firstParen + 1, payout.IndexOf('%') - firstParen  - 1), out percent))
                        {
                            return false;
                        }
                        t.WinPercent = percent;
                    }
                    break;
                }
            }

            return true;
        }

        void SaveResult(TourneyResultModel t)
        {
            using (CardStatsContext context = new CardStatsContext())
            {
                context.TourneyResults.Add(t);
                context.SaveChanges();
            }
        }

        void MoveParsedFile(string resultFile)
        {
            File.Move(resultFile, GetArchiveFileName(resultFile));
        }

        string GetArchiveFileName(string resultFile)
        {
            string fileName = System.IO.Path.GetFileName(resultFile);
            return System.IO.Path.Combine(this.ArchiveDirectory, fileName);
        }
    }
}
