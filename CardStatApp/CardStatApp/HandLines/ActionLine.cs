namespace CardStatApp.HandLines
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CardStatApp.Logic;
    using CardStatApp.Models;
    using CardStatApp.Parsers;

    

    enum CommentType
    {
        TimedOut,
        Disconnected,
        Comment,
        UncalledBet
    }

    class RoundLine : ILine
    {
        public HandRound Round { get; set; }
        public BoardCards Cards { get; set; }

        public RoundLine(string line)
        {
            Cards = new BoardCards();

            if (line.Contains("HOLE CARDS"))
                Round = HandRound.HoleCards;
            else if (line.Contains("FLOP"))
                Round = HandRound.Flop;
            else if (line.Contains("TURN"))
                Round = HandRound.Turn;
            else if (line.Contains("RIVER"))
                Round = HandRound.River;
            else if (line.Contains("SHOWDOWN"))
                Round = HandRound.Showdown;
            else if (line.Contains("SUMMARY"))
                Round = HandRound.Summary;

            switch (Round)
            {
                case HandRound.Flop:
                    {
                        IList<CardModel> cards = CardParser.ParseCards(line.Substring(line.LastIndexOf("***") + 3).Trim());
                        Cards = new BoardCards(cards);
                        break;
                    }
                case HandRound.Turn:
                    {
                        IList<CardModel> cards = CardParser.ParseCards(line.Substring(line.LastIndexOf("***") + 3).Trim());
                        Cards.Turn = cards[3];
                        break;
                    }
                case HandRound.River:
                    {
                        IList<CardModel> cards = CardParser.ParseCards(line.Substring(line.LastIndexOf("***") + 3).Trim());
                        Cards.River = cards[4];
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
    class CommentLine : ILine
    {
        public string PlayerName { get; set; }
        public CommentType CommentType { get; set; }
        public string Comment { get; set; }

        public CommentLine(string line)
        {
            if (line.Contains("has timed out while being disconnected"))
            {
                PlayerName = line.Split(' ')[0];
            }
            else if (line.Contains(" is disconnected") || line.Contains(" is connected"))
            {
                PlayerName = line.Split(' ')[0];
            }
            else if (line.Contains(" said, "))
            {
                PlayerName = line.Split(' ')[0];
                Comment = line.Split(',')[1].Trim();
            }
        }
    }
    class DealtToLine : ILine
    {
        public string PlayerName { get; set; }
        public HoleCards HoleCards { get; set; }

        public DealtToLine(string line)
        {
            PlayerName = line.Split(' ')[2].Trim();
            HoleCards = new HoleCards(CardParser.ParseCards(line.Substring(line.LastIndexOf('['))));
        }
    }
    class ActionLine : ILine
    {
        //sticks828: posts the ante 250
        //yves2711: posts small blind 1250
        //RobRoberts82: posts big blind 2500
        //sticks828: folds
        //Arsen78x: raises 2500 to 5000
        //SenSej19 has timed out while being disconnected
        //yves2711: calls 3750
        //yves2711: bets 2500
        //SenSej19 is disconnected 
        //---- > Uncalled bet (2500) returned to yves2711
        //---- > yves2711 collected 14500 from pot
        //yves2711: doesn't show hand 
        //RobRoberts82: raises 30515 to 45515 and is all-in
        //RobRoberts82: shows [Qd Kh] (two pair, Queens and Fives)
        //Arsen78x: shows [Kc Qh] (two pair, Queens and Fives)
        //---- > Seat 8: yves2711 showed [Th Js] and lost with a pair of Queens
        //---- > Seat 9: RobRoberts82 showed [Kc Ad] and won (63080) with two pair, Kings and Queens
        //RobRoberts82 said, "that was exciting"
        //yves2711: checks 
        public string PlayerName { get; set; }
        public GameAction Action { get; set; }
        public int? Amount { get; set; }
        public int? Raise { get; set; }
        public bool IsAllIn { get; set; }

        public ActionLine(string line)
        {
            PlayerName = line.Split(':')[0];
            IsAllIn = line.Contains("is all-in");

            if (line.Contains("posts small blind"))
            {
                this.Action = GameAction.SmallBlind;
                this.Amount = GetActionAmount(line);
            }
            else if (line.Contains("posts big blind"))
            {
                this.Action = GameAction.BigBlind;
                this.Amount = GetActionAmount(line);
            }
            else if (line.Contains("posts the ante"))
            {
                this.Action = GameAction.Ante;
                this.Amount = GetActionAmount(line);
            }
            else if (line.Contains("folds"))
            {
                this.Action = GameAction.Fold;
            }
            else if (line.Contains("bets"))
            {
                this.Action = GameAction.Bet;
                this.Amount = GetActionAmount(line);
            }
            else if (line.Contains("calls"))
            {
                this.Action = GameAction.Call;
                this.Amount = GetActionAmount(line);
            }
            else if (line.Contains("raises"))
            {
                this.Action = GameAction.Raise;
                this.Amount = GetActionAmount(line);
                this.Raise = GetRaiseAmount(line);
            }
            else if (line.Contains("checks"))
            {
                this.Action = GameAction.Check;
            }
            else if (line.Contains("mucks"))
            {
                this.Action = GameAction.MucksHand;
            }
            else if (line.Contains("shows"))
            {
                this.Action = GameAction.ShowsHand;
            }
            else if (line.Contains("doesn't show"))
            {
                this.Action = GameAction.DoesntShowHand;
            }
            else
            {
                Console.WriteLine("Unparsed ActionLine: " + line);
            }
        }

        int GetActionAmount(string line)
        {
            string amountLine = String.Empty;
            if (line.Contains("and is all-in"))
                amountLine = line.Replace("and is all-in", "").Trim();
            else
                amountLine = line;

            string[] buffer = amountLine.Split(' ');
            // TRICKLE UP: raises 1480 to 1500 and is all-in
            if (line.Contains("raises"))
            {
                string amt = amountLine.Split(':')[1].Split(' ')[4].Trim();
                return Convert.ToInt32(amt);
            }
            else
                return Convert.ToInt32(buffer.Last());
        }

        int GetRaiseAmount(string line)
        {
            // TRICKLE UP: raises 1480 to 1500 and is all-in
            string amountLine = String.Empty;
            if (line.Contains("and is all-in"))
                amountLine = line.Replace("and is all-in", "").Trim();
            else
                amountLine = line;

            string[] buffer = amountLine.Split(':')[1].Split(' ');
            return Convert.ToInt32(buffer[2].Trim());
        }
    }
    class ShowsLine : ILine
    {
        //romank275: shows [6c Ac] (a pair of Queens)
        //Paulo Pitt: shows [4d As] (a pair of Queens)
        public string PlayerName { get; set; }
        public HoleCards HoleCards { get; set; }
        public string HandDescription { get; set; }

        public ShowsLine(string line)
        {
            PlayerName = line.Split(':')[0];
            HoleCards = new HoleCards(CardParser.ParseCards(line.Split(':')[1].Trim().Split('[')[1].Split(']')[0]));
            HandDescription = line.Split(':')[1].Trim().Split('(')[1].Split(')')[0].Trim();
        }
    }
    class FinishedTourneyLine : ILine
    {
        // lowlander66 finished the tournament in 78th place
        // RobRoberts82 finished the tournament in 3rd place and received 161200.
        public string PlayerName { get; set; }
        public string Position { get; set; }
        public int? Winnings { get; set; }
        public FinishedTourneyLine(string line)
        {
            PlayerName = line.Substring(0, line.IndexOf(" finished the")).Trim();
            string[] buffer = line.Split(' ');
            for (int i = 0; i < buffer.Count(); i++)
            {
                if (buffer[i] == "in")
                {
                    Position = buffer[i + 1];
                }
                if (buffer[i] == "received")
                {
                    Winnings = Convert.ToInt32(buffer[i + 1].TrimEnd('.'));
                }
            }
        }
    }
    class TimedOutLine : ILine
    {
        public string PlayerName { get; set; }

        public TimedOutLine(string line)
        {
            PlayerName = line.Split(' ')[0].Trim();
        }
    }
    class UncalledBetLine : ILine
    {
        public string PlayerName { get; set; }
        public int Amount { get; set; }

        public UncalledBetLine(string line)
        {
            int startReturnedTo = line.IndexOf(" returned to ");
            int startName = startReturnedTo + " returned to ".Length;
            PlayerName = line.Substring(startName);

            string amt = line.Split('(')[1].Split(')')[0].Trim();
            Amount = Convert.ToInt32(amt);
        }
    }
    class CollectedLine : ILine
    {
        //romank275 collected 1105 from pot
        //Paulo Pitt collected 1105 from pot
        public string PlayerName { get; set; }
        public int Amount { get; set; }

        public CollectedLine(string line)
        {
            int c = line.IndexOf(" collected");
            int endc = c + " collected".Length + 1;
            int startFrom = line.IndexOf(" from");
            PlayerName = line.Substring(0, c - 1);

            string amt = line.Substring(endc, startFrom - endc);
            Amount = Convert.ToInt32(amt);
        }
    }
    class SummarySeatLine : ILine
    {
        //Seat 1: watersmk (button) folded before Flop (didn't bet)
        //Seat 2: Dick_Liquor (small blind) folded before Flop
        //Seat 3: RobRoberts82 (big blind) folded before Flop
        //Seat 4: sbonino folded before Flop
        //Seat 5: romank275 showed [6c Ac] and won (1105) with a pair of Queens
        //Seat 6: Tiago3385 folded before Flop (didn't bet)
        //Seat 7: serega55599 folded before Flop (didn't bet)
        //Seat 8: ProVolme folded before Flop
        //Seat 9: Paulo Pitt showed [4d As] and won (1105) with a pair of Queens

        // collected = won when everyone folded
        //Seat 3: Gerbosci (big blind) collected (20)
        //Seat 3: Gerbosci (big blind) mucked [6c Ac]

        public string PlayerName { get; set; }
        public HoleCards HoleCards { get; set; }
        public string HandDescription { get; set; }
        public int? Amount { get; set; }
        public int SeatNumber { get; set; }
        public bool DidntBet { get; set; }
        public bool IsWinner { get; set; }
        public bool IsLoser { get; set; }
        public bool ShowsHand { get; set; }

        public SummarySeatLine(string line)
        {
            SeatNumber = Convert.ToInt32(line.Split(':')[0].Split(' ')[1]);
            PlayerName = line.Split(':')[1].Split('(')[0].Trim();
            DidntBet = line.EndsWith("(didn't bet)");
            IsLoser = line.Contains(" and lost ");

            if (line.Contains("showed ["))
            {
                ShowsHand = true;
                string cardBuffer = line.Split('[')[1].Split(']')[0].Trim();
                HoleCards = new HoleCards(CardParser.ParseCards(cardBuffer));
            }

            if (line.Contains("and won ("))
            {
                IsWinner = true;
                string wonBuffer = line.Split(']')[1].Split('(')[1].Split(')')[0].Trim();
                Amount = Convert.ToInt32(wonBuffer);
            }
            else if (line.Contains(" collected ("))
            {
                IsWinner = true;
                string collectedBuffer = line.Substring(line.LastIndexOf('(') + 1).Replace(")", "").Trim();
                Amount = Convert.ToInt32(collectedBuffer);
            }
            else if (line.Contains(" mucked "))
            {
                ShowsHand = true;
                string cardBuffer = line.Split('[')[1].Split(']')[0].Trim();
                HoleCards = new HoleCards(CardParser.ParseCards(cardBuffer));
            }

            if (line.Contains(" with "))
            {
                HandDescription = line.Substring(line.IndexOf(" with ") + 6).Trim();
            }

        }
    }
    class SummaryBoardLine : ILine
    {
        //Board [Qs Kh 9c 2d Qc]

        public BoardCards Cards { get; set; }

        public SummaryBoardLine(string line)
        {
            string cardBuffer = line.Split('[')[1].Split(']')[0].Trim();
            Cards = new BoardCards(CardParser.ParseCards(cardBuffer));
        }
    }
    class SummaryTotalLine : ILine
    {
        //Total pot 2210 | Rake 0 

        public int TotalPot { get; set; }
        public int Rake { get; set; }

        public SummaryTotalLine(string line)
        {
            string[] buffer = line.Split(' ');
            TotalPot = Convert.ToInt32(buffer[2].Trim());
            Rake = Convert.ToInt32(buffer[5].Trim());
        }
    }
}
