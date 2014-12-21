namespace CardStatApp.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CardStatApp.Data;
    using CardStatApp.Models;

    class HandHistoryParser
    {
        public event EventHandler TourneyResultParsed;
        public event ProgressChangedEvent ProgressChangedx;

        string playerName = "RobRoberts82";
        CardStatsContext context = new CardStatsContext();

        public string ResultDirectory { get; set; }

        public string ArchiveDirectory
        {
            get
            {
                return System.IO.Path.Combine(this.ResultDirectory, "Archive");
            }
        }

        public HandHistoryParser(string directory)
        {
            this.ResultDirectory = directory;
        }

        public void ParseResults()
        {
            if (!Directory.Exists(this.ArchiveDirectory))
                Directory.CreateDirectory(this.ArchiveDirectory);

            foreach (string resultFile in Directory.EnumerateFiles(this.ResultDirectory))
            {
                if (ParseResultFile(resultFile))
                    MoveParsedFile(resultFile);
                else
                    break;
            }

            Console.WriteLine("Done");
        }

        public void ParseFile(string resultFile)
        {
            ParseResultFile(resultFile);
        }

        public bool ParseResultFile(string resultFile)
        {
            if (!File.Exists(resultFile)) return false;

            Console.WriteLine("Parsing file " + resultFile);
            try
            {
                string[] lines = File.ReadAllLines(resultFile);

                IList<string> handLines = new List<String>();
                bool lastLineBlank = false;

                foreach (string line in lines)
                {
                    if (String.IsNullOrEmpty(line.Trim()))
                    {
                        lastLineBlank = true;
                    }
                    else
                    {
                        if (lastLineBlank)
                        {
                            if (!ProcessHand(handLines))
                                return false;
                            handLines = new List<String>();
                            handLines.Add(line);
                        }
                        else
                        {
                            handLines.Add(line);
                        }
                        lastLineBlank = false;
                    }
                }
                if (!ProcessHand(handLines))
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        bool ProcessHand(IList<String> handLines)
        {
            HandHistoryModel handHistory = new HandHistoryModel();
            try
            {
                int handState = 0;

                List<String> holeCards = new List<String>();

                foreach (string line in handLines)
                {
                    if (line.StartsWith("PokerStars Hand #"))
                    {
                        if (line.Contains("Tournament "))
                        {
                            handHistory.HandNo = line.Split(':')[0].Split('#')[1];
                            handHistory.TourneyNo = line.Split(':')[1].Split('#')[1].Split(',')[0];
                        }
                        else
                        {
                            handHistory.HandNo = line.Split(':')[0].Split('#')[1];
                            handHistory.TourneyNo = String.Empty;
                        }
                    }
                    else if (line.StartsWith("PokerStars Zoom Hand #"))
                    {
                            handHistory.HandNo = line.Split(':')[0].Split('#')[1];
                            handHistory.TourneyNo = String.Empty;
                    }
                    if (handState == 0)
                    {
                        if (line.Contains("is the button"))
                        {
                            int poundIdx = line.IndexOf('#');
                            handHistory.ButtonSeat = Convert.ToInt32(line.Substring(poundIdx + 1, 1));
                        }
                        else if (line.Contains(playerName) && (line.Contains("in chips")))
                        {
                            handHistory.StartChips = Convert.ToInt32(line.Substring(line.IndexOf('(') + 1, line.IndexOf(' ', line.IndexOf('(')) - line.IndexOf('(')));
                            handHistory.PlayerSeat = Convert.ToInt32(line.Substring(5, 1));
                            handHistory.IsButton = (handHistory.PlayerSeat == handHistory.ButtonSeat);
                        }
                        else if (line.StartsWith(playerName))
                        {
                            if (line.Contains("posts the ante"))
                            {
                                handHistory.IsAnte = true;
                                handHistory.Ante = GetBlindOrAnteAmount(line);
                            }
                            else if (line.Contains("posts big blind"))
                            {
                                handHistory.IsBigBlind = true;
                                handHistory.Blind = GetBlindOrAnteAmount(line);
                            }
                            else if (line.Contains("posts small blind"))
                            {
                                handHistory.IsSmallBlind = true;
                                handHistory.Blind = GetBlindOrAnteAmount(line);
                            }
                        }
                    }
                    else if (handState == 1)
                    {
                        if (line.StartsWith("Dealt to") && line.Contains(playerName) & line.Contains('[') && (line.Contains(playerName) & line.Contains('[')))
                        {
                            holeCards.AddRange(line.Substring(line.LastIndexOf('[') + 1, line.Length - line.LastIndexOf('[') - 2).Split(' '));
                            handHistory.HoleCard1 = holeCards[0];
                            handHistory.HoleCard2 = holeCards[1];
                        }
                        if (line.StartsWith(playerName) && (line.Contains("bets") || line.Contains("calls") || line.Contains("raises")))
                            handHistory.VIP += GetActionAmount(line);
                    }
                    else if (handState == 6)
                    {
                        if (line.Contains(playerName) && (line.Contains("collected") || line.Contains("won")))
                            handHistory.Won = Convert.ToInt32(line.Substring(line.LastIndexOf('(') + 1, line.LastIndexOf(')') - line.LastIndexOf('(') - 1));
                        else if (line.Contains("Total pot"))
                            handHistory.TotalPot = Convert.ToInt32(line.Split(' ')[2]);
                        else if (line.Contains("Board"))
                        {
                            handHistory.Board = line.Substring(6);
                        }
                    }
                    else if (line.StartsWith(playerName) && (line.Contains("bets") || line.Contains("calls") || line.Contains("raises")))
                        handHistory.VIP += GetActionAmount(line);

                    if (line.StartsWith("***"))
                    {
                        if (line == "*** SUMMARY ***")
                            handState = 6;
                        else
                            handState += 1;
                    }
                }            //Console.WriteLine("Start: {0}, NET: {1}", startChips, (chipsIn - chipsOut));

                handHistory.Net = handHistory.Won - (handHistory.VIP + handHistory.Ante + handHistory.Blind);
                handHistory.EndChips = handHistory.StartChips + handHistory.Net;
                context.HandHistories.Add(handHistory);
                System.Diagnostics.Debug.Assert(!String.IsNullOrEmpty(handHistory.HandNo));               
                context.SaveChanges();

                return true;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var eve in ex.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine(String.Format("Validation Error: Property {0}; Message: {1}", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        int GetBlindOrAnteAmount(string line)
        {
            int blind = 0;
            Int32.TryParse(line.Split(' ')[4], out blind);
            return blind;
        }

        int GetActionAmount(string line)
        {
            int actionAmount = 0;
            string[] actionLine = line.Split(':')[1].Split(' ');
            Int32.TryParse(actionLine[2], out actionAmount);
            return actionAmount;
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
