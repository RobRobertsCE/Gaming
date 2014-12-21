namespace CardStatApp.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CardStatApp.Data;
    using CardStatApp.HandLines;
    using CardStatApp.Models;
    
    public class HandParser
    {
        #region fields
        string currentResultFile = String.Empty;
        int lineIdx = 0;
        HandModel current = null;
        IDictionary<string, SeatModel> seatList = new Dictionary<string, SeatModel>();
        IDictionary<string, PlayerModel> playerList = new Dictionary<string, PlayerModel>();
        #endregion

        #region properties
        public List<string> Messages { get; set; }
        public List<string> Errors { get; set; }
        public List<string> Warnings { get; set; }
        public bool MoveFile { get; set; }
        public string ResultDirectory { get; set; }
        public string ArchiveDirectory
        {
            get
            {
                return System.IO.Path.Combine(this.ResultDirectory, "HandArchive");
            }
        }

        CardStatsContext context = null;
        #endregion

        #region ctor
        public HandParser()
        {
            MoveFile = false;
            Messages = new List<string>();
            Errors = new List<string>();
            Warnings = new List<string>();

            context = new CardStatsContext();
        }
        public HandParser(string directory)
            : this()
        {
            this.ResultDirectory = directory;
        }
        #endregion

        #region public
        public void ParseResults()
        {
            if (!Directory.Exists(this.ArchiveDirectory))
                Directory.CreateDirectory(this.ArchiveDirectory);

            StatusMessage("Begin Parsing Directory " + this.ResultDirectory);

            foreach (string resultFile in Directory.EnumerateFiles(this.ResultDirectory))
            {
                if (!ParseResultFile(resultFile))
                    break;
            }

            StatusMessage("Done Parsing Directory");
        }
        public bool ParseFile(string resultFile)
        {
            try
            {
                if (ParseResultFile(resultFile))
                {
                    if (MoveFile) MoveParsedFile(resultFile);
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.ToString());
                return false;
            }

        }
        #endregion

        #region parsers
        bool ParseResultFile(string resultFile)
        {
            if (!File.Exists(resultFile)) return false;

            currentResultFile = resultFile;

            StatusMessage("Begin Parsing file " + currentResultFile);

            try
            {
                foreach (string line in File.ReadAllLines(currentResultFile))
                {
                    ProcessLine(line);
                    lineIdx++;
                }

                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage(String.Format("Line: {0}; Error: {1}", lineIdx.ToString(), ex.ToString()));
                return false;
            }
            finally
            {
                StatusMessage("Done Parsing file " + currentResultFile);
            }
        }
        public void ProcessLine(string line)
        {
            ILine l = null;

            try
            {
                if (line.StartsWith("PokerStars"))
                {
                    StartNewHand();
                    l = ProcessInitializationLine(line);
                }
                else if (line.StartsWith("***"))
                {
                    l = ProcessRoundLine(line);
                }
                else if (line.Contains("said, ") || line.Contains("joins the table") || line.Contains("leaves the table") || line.Contains(" is connected "))
                {
                    l = new CommentLine(line);
                }
                else if (line.Contains("collected") && (line.Contains("from pot") || line.Contains("from side pot") || line.Contains("from main pot")))
                {
                    l = ProcessGameActionLine(line);
                }
                else if (line.Contains("finished the tournament"))
                {
                    l = ProcessGameActionLine(line);
                }
                else if (line.Contains("wins the tournament"))
                {
                    l = new CommentLine(line);
                }
                else if (line.Contains("has timed out"))
                {
                    l = new TimedOutLine(line);
                }
                else
                {
                    switch (current.Round)
                    {
                        case HandRound.Initialization:
                            {
                                l = ProcessInitializationLine(line);
                                break;
                            }
                        case HandRound.HoleCards:
                            {
                                l = ProcessHoleCardsLine(line);
                                break;
                            }
                        case HandRound.Flop:
                            {
                                l = ProcessFlopLine(line);
                                break;
                            }
                        case HandRound.Turn:
                            {
                                l = ProcessTurnLine(line);
                                break;
                            }
                        case HandRound.River:
                            {
                                l = ProcessRiverLine(line);
                                break;
                            }
                        case HandRound.Showdown:
                            {
                                l = ProcessShowdownLine(line);
                                break;
                            }
                        case HandRound.Summary:
                            {
                                l = ProcessSummaryLine(line);
                                break;
                            }
                        default:
                            {
                                ErrorMessage("Unrecognized Hand Round " + current.Round.ToString());
                                break;
                            }
                    }
                }

                if (l != null)
                {
                    ProcessLineData(l);
                }
                else
                {
                    if (!String.IsNullOrEmpty(line))
                        UnparsedLine(lineIdx + ": " + "Unparsed Line in ProcessGameActionLine: " + line);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(lineIdx + ": " + line + "   " + ex.ToString());
            }
        }

        ILine ProcessRoundLine(string line)
        {
            return new RoundLine(line);
        }
        ILine ProcessInitializationLine(string line)
        {
            if (line.StartsWith("PokerStars"))
                return new HandHeaderLine(line);
            else if (line.StartsWith("Table"))
                return new TableHeaderLine(line);
            else if (line.StartsWith("Seat"))
                return new SeatInitLine(line);
            else if (line.Contains(':'))
                return new ActionLine(line);
            else
                return null;
        }
        ILine ProcessHoleCardsLine(string line)
        {
            return ProcessGameActionLine(line);
        }
        ILine ProcessFlopLine(string line)
        {
            return ProcessGameActionLine(line);
        }
        ILine ProcessTurnLine(string line)
        {
            return ProcessGameActionLine(line);
        }
        ILine ProcessRiverLine(string line)
        {
            return ProcessGameActionLine(line);
        }
        ILine ProcessGameActionLine(string line)
        {
            if (line.StartsWith("Dealt to"))
                return new DealtToLine(line);
            else if (line.Contains(':'))
                return new ActionLine(line);
            else if (line.StartsWith("Uncalled bet"))
                return new UncalledBetLine(line);
            else if (line.Contains("collected"))
                return new CollectedLine(line);
            else if (line.Contains("said, "))
                return new CommentLine(line);
            else if (line.Contains("is disconnected"))
                return new CommentLine(line);
            else if (line.Contains("timed out"))
                return new TimedOutLine(line);
            else if (line.Contains("finished the tournament"))
                return new FinishedTourneyLine(line);
            else
                return null;
        }
        ILine ProcessShowdownLine(string line)
        {
            if (line.Contains("shows"))
                return new ShowsLine(line);
            else if (line.Contains("collected"))
                return new CollectedLine(line);
            else if (line.Contains("mucks"))
                return new ActionLine(line);
            else if (line.Contains("doesn't show"))
                return new ActionLine(line);
            else if (line.Contains("finished the tournament"))
                return new FinishedTourneyLine(line);
            else if (line.Contains("has timed out while disconnected") || line.Contains("congratulations"))
                return new CommentLine(line);
            else
                return null;
        }
        ILine ProcessSummaryLine(string line)
        {
            if (line.StartsWith("Total pot"))
                return new SummaryTotalLine(line);
            else if (line.StartsWith("Board"))
                return new SummaryBoardLine(line);
            else if (line.StartsWith("Seat"))
                return new SummarySeatLine(line);
            else
                return null;
        }

        void ProcessLineData(ILine l)
        {
            if (l is ActionLine)
                ProcessActionLine(l as ActionLine);
            else if (l is RoundLine)
                ProcessRoundLine(l as RoundLine);
            else if (l is CollectedLine)
                ProcessCollectedLine(l as CollectedLine);
            else if (l is CommentLine)
                ProcessCommentLine(l as CommentLine);
            else if (l is DealtToLine)
                ProcessDealtToLine(l as DealtToLine);
            else if (l is FinishedTourneyLine)
                ProcessFinishedTourneyLine(l as FinishedTourneyLine);
            else if (l is HandHeaderLine)
                ProcessHandHeaderLine(l as HandHeaderLine);
            else if (l is SeatInitLine)
                ProcessSeatInitLine(l as SeatInitLine);
            else if (l is ShowsLine)
                ProcessShowsLine(l as ShowsLine);
            else if (l is SummaryBoardLine)
                ProcessSummaryBoardLine(l as SummaryBoardLine);
            else if (l is SummarySeatLine)
                ProcessSummarySeatLine(l as SummarySeatLine);
            else if (l is SummaryTotalLine)
                ProcessSummaryTotalLine(l as SummaryTotalLine);
            else if (l is TableHeaderLine)
                ProcessTableHeaderLine(l as TableHeaderLine);
            else if (l is TimedOutLine)
                ProcessTimedOutLine(l as TimedOutLine);
            else if (l is UncalledBetLine)
                ProcessUncalledBetLine(l as UncalledBetLine);
            else
                ErrorMessage("Unrecognized Line Type: " + l.GetType().ToString());
        }
        #endregion

        #region ILine processors
        // TODO: 
        void ProcessCollectedLine(CollectedLine l)
        {

        }
        // TODO: 
        void ProcessCommentLine(CommentLine l)
        {

        }
        // TODO: 
        private void ProcessUncalledBetLine(UncalledBetLine uncalledBetLine)
        {

        }
        // TODO: 
        private void ProcessTimedOutLine(TimedOutLine timedOutLine)
        {

        }
        // TODO: 
        private void ProcessSummaryTotalLine(SummaryTotalLine summaryTotalLine)
        {

        }
        // TODO: 
        private void ProcessSummarySeatLine(SummarySeatLine summarySeatLine)
        {

        }
        // TODO: 
        private void ProcessSummaryBoardLine(SummaryBoardLine summaryBoardLine)
        {

        }
        // TODO: 
        private void ProcessFinishedTourneyLine(FinishedTourneyLine finishedTourneyLine)
        {

        }
        private void ProcessShowsLine(ShowsLine l)
        {
            SeatModel seat = GetSeat(l.PlayerName);
            seat.HoleCard0 = l.HoleCards.Card0;
            seat.HoleCard1 = l.HoleCards.Card1;
        }
        private void ProcessSeatInitLine(SeatInitLine l)
        {
            SeatModel playerSeat = null;

            if (!current.Seats.Any(s => s.Player.Name == l.PlayerName))
            {
                if (!seatList.ContainsKey(l.PlayerName))
                {
                    playerSeat = new SeatModel() { SeatNumber = l.SeatNumber, Chips = l.ChipCount };
                    playerSeat.Player = GetPlayer(l.PlayerName);
                    seatList.Add(l.PlayerName, playerSeat);
                }
                else
                {
                    playerSeat = seatList[l.PlayerName];
                    if (playerSeat.SeatNumber != l.SeatNumber)
                    {
                        seatList.Remove(l.PlayerName);
                        playerSeat = new SeatModel() { SeatNumber = l.SeatNumber, Chips = l.ChipCount };
                        playerSeat.Player = GetPlayer(l.PlayerName);
                        seatList.Add(l.PlayerName, playerSeat);
                    }
                    else
                    {
                        playerSeat.Chips = l.ChipCount;
                    }
                }
                current.Seats.Add(playerSeat);
            }
            else
            {
                playerSeat = current.Seats.Where(s => s.Player.Name == l.PlayerName).FirstOrDefault();

                if (playerSeat.SeatNumber != l.SeatNumber)
                {
                    current.Seats.Remove(playerSeat);
                    seatList.Remove(l.PlayerName);
                    playerSeat = new SeatModel() { SeatNumber = l.SeatNumber, Chips = l.ChipCount };
                    playerSeat.Player = GetPlayer(l.PlayerName);
                    seatList.Add(l.PlayerName, playerSeat);
                }
                else
                {
                    playerSeat.Chips = l.ChipCount;
                }
            }

        }
        private void ProcessHandHeaderLine(HandHeaderLine l)
        {
            current.Seats = new List<SeatModel>();
            current.HandId = Convert.ToInt64(l.HandNo);
            if (l.IsZoom)
            {
                current.TourneyNo = 0;
                current.SmallBlind = Convert.ToInt32(l.Level.Split('(')[1].Split(')')[0].Trim().Split('/')[0]);
                current.BigBlind = Convert.ToInt32(l.Level.Split('(')[1].Split(')')[0].Trim().Split('/')[1]);
            }
            else
            {
                if (l.IsTournament) current.TourneyNo = Convert.ToInt64(l.TourneyNo);
                current.SmallBlind = Convert.ToInt32(l.Level.Split('(')[1].Split(')')[0].Trim().Split('/')[0]);
                current.BigBlind = Convert.ToInt32(l.Level.Split('(')[1].Split(')')[0].Trim().Split('/')[1]);
            }
        }
        private void ProcessDealtToLine(DealtToLine l)
        {
            SeatModel seat = GetSeat(l.PlayerName);
            seat.HoleCard0 = l.HoleCards.Card0;
            seat.HoleCard1 = l.HoleCards.Card1;
        }
        private void ProcessTableHeaderLine(TableHeaderLine l)
        {
            current.ButtonSeat = l.ButtonSeat;
            current.TableId = l.TableId;
            current.Description = l.Description;
        }
        void ProcessRoundLine(RoundLine l)
        {
            current.Round = (HandRound)l.Round;

            switch (current.Round)
            {
                case HandRound.Flop:
                    {
                        current.FlopCard0 = l.Cards.Flop0;
                        current.FlopCard1 = l.Cards.Flop1;
                        current.FlopCard2 = l.Cards.Flop2;
                        break;
                    }
                case HandRound.Turn:
                    {
                        current.TurnCard = l.Cards.Turn;
                        break;
                    }
                case HandRound.River:
                    {
                        current.RiverCard = l.Cards.River;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        void ProcessActionLine(ActionLine l)
        {
            PlayerActionModel a = new PlayerActionModel();

            a.ActionKey = l.Action;
            a.Seat = GetSeat(l.PlayerName);
            if (l.Amount.HasValue) a.Amount = l.Amount.Value;

            current.Actions.Add(a);
        }
        #endregion

        #region data
        SeatModel GetSeat(string playerName)
        {
            SeatModel seat = null;

            if (current.Seats.Any(s => s.Player.Name == playerName))
                return current.Seats.Where(s => s.Player.Name == playerName).FirstOrDefault();
            else
            {
                if (seatList.ContainsKey(playerName))
                {
                    seat = seatList[playerName];
                    current.Seats.Add(seat);
                }
                else
                    ErrorMessage("Cant find seat for " + playerName);
            }

            return seat;
        }
        PlayerModel GetPlayer(string playerName)
        {
            PlayerModel player = null;

            if (!playerList.ContainsKey(playerName))
            {
                // TODO: Load from context 
                player = new PlayerModel() { Name = playerName };
                playerList.Add(playerName, player);
            }
            else
            {
                player = playerList[playerName];
            }
            return player;
        }
        #endregion

        #region new hand
        void StartNewHand()
        {
            if (null != current)
                SaveCurrentHand();

            current = new HandModel();
            current.Seats = new List<SeatModel>();
            current.Round = HandRound.Initialization;
        }
        void SaveCurrentHand()
        {
            try
            {
                context.SaveChanges();
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        #endregion

        #region move file
        void MoveParsedFile(string resultFile)
        {
            File.Move(resultFile, GetArchiveFileName(resultFile));
        }
        string GetArchiveFileName(string resultFile)
        {
            string fileName = System.IO.Path.GetFileName(resultFile);
            return System.IO.Path.Combine(this.ArchiveDirectory, fileName);
        }
        #endregion

        #region messages
        void StatusMessage(string message)
        {
            Messages.Add(message);
        }
        void ErrorMessage(string message)
        {
            Errors.Add(message);
        }
        void UnparsedLine(string line)
        {
            string msg = String.Format("UNPARSED LINE: {0}:{1}:{2}", currentResultFile, lineIdx, line);
            Warnings.Add(msg);
        }
        void WarningMessage(string message)
        {
            Warnings.Add(message);
        }
        #endregion
    }
}
