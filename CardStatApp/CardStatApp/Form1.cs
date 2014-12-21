using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CardStatApp.Parsers;

namespace CardStatApp
{
    public partial class Form1 : Form
    {
        class HandHistoryFile
        {
            public string FileTitle { get; set; }
            public string FilePath { get; set; }
            public HandHistoryFile(string filePath)
            {
                this.FilePath = filePath;
                this.FileTitle = System.IO.Path.GetFileName(filePath);
            }
        }
        #region fields
        string resultDir = @"C:\Users\rroberts\AppData\Local\PokerStars\TournSummary\RobRoberts82";
        string handDir = @"C:\Users\rroberts\AppData\Local\PokerStars\HandHistory\RobRoberts82";
        #endregion

        #region ctor
        public Form1()
        {
            InitializeComponent();

            LoadList();
        }
        void LoadList()
        {
            lstHandFiles.Items.Clear();

            lstHandFiles.DisplayMember = "FileTitle";

            foreach (string resultFile in System.IO.Directory.EnumerateFiles(this.handDir))
            {
                lstHandFiles.Items.Add(new HandHistoryFile(resultFile));
            }
        }
        #endregion

        #region tourney parser
        private void button1_Click(object sender, EventArgs e)
        {
            RunTourneyResultParserDirectory(resultDir);
        }
        void RunTourneyResultParserDirectory(string tourneyHistoryDirectory)
        {
            try
            {
                TourneyResultParser p = new TourneyResultParser(tourneyHistoryDirectory);
                p.ParseResults();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        #endregion

        #region hand history parser
        private void button2_Click(object sender, EventArgs e)
        {
            RunHandHistoryParseDirectory(handDir);
        }
        void RunHandHistoryParseDirectory(string handHistoryDirectory)
        {
            try
            {
                HandHistoryParser h = new HandHistoryParser(handHistoryDirectory);
                h.ParseResults();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                HandHistoryParser h = new HandHistoryParser(handDir);
                foreach (HandHistoryFile hhFile in lstHandFiles.SelectedItems)
                {
                    h.ParseResultFile(hhFile.FilePath);
                }
                LoadList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        #endregion

        #region hand parser
        private void button3_Click(object sender, EventArgs e)
        {
            //RunHandParseSingle(@"C:\Users\rroberts\AppData\Local\PokerStars\HandHistory\RobRoberts82\Archive\HH20141215 T1075110967 No Limit Hold'em 4,500 + 500.txt");
            RunHandParseSingle(txtSingle.Text);
            LoadList();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            RunHandParseDirectory(handDir);
            LoadList();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (HandHistoryFile hhFile in lstHandFiles.SelectedItems)
                {
                    RunHandParseSingle(hhFile.FilePath);
                }
                LoadList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        void RunHandParseDirectory(string handHistoryDirectory)
        {
            try
            {
                HandParser h = new HandParser(handHistoryDirectory);
                h.ParseResults();
                DisplayOutput(h);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        void RunHandParseSingle(string fileName)
        {
            RunHandParseSingle(handDir, fileName, chkMoveFile.Checked);
        }
        void RunHandParseSingle(string handHistoryDirectory, string fileName, bool moveFile)
        {
            try
            {
                HandParser h = new HandParser(handHistoryDirectory);
                h.MoveFile = moveFile;
                h.ParseFile(fileName);
                DisplayOutput(h);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        void DisplayOutput(HandParser h)
        {
            txtOutput.AppendText("-------------------------------------" + Environment.NewLine);
            if (chkMessages.Checked && h.Messages.Count > 0)
            {
                txtOutput.AppendText("*** MESSAGES ***" + Environment.NewLine);
                foreach (string message in h.Messages)
                {
                    txtOutput.AppendText(message + Environment.NewLine);
                }
            }

            if (chkErrors.Checked && h.Errors.Count > 0)
            {
                txtOutput.AppendText("*** ERRORS ***" + Environment.NewLine);
                foreach (string errorMessage in h.Errors)
                {
                    txtOutput.AppendText(errorMessage + Environment.NewLine);
                }
            }
            if (chkWarnings.Checked && h.Warnings.Count > 0)
            {
                txtOutput.AppendText("*** WARNINGS ***" + Environment.NewLine);
                foreach (string warningMessage in h.Warnings)
                {
                    txtOutput.AppendText(warningMessage + Environment.NewLine);
                }
            }
            txtOutput.AppendText("*** DONE ***" + Environment.NewLine);
        }
        #endregion

        #region HandParser parse line
        private void button6_Click(object sender, EventArgs e)
        {
            RunHandParserLine(txtLine.Text);
        }
        void RunHandParserLine(string line)
        {
            try
            {
                HandParser h = new HandParser();
                h.MoveFile = false;
                h.ProcessLine(line);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        #endregion
               
    }
}
