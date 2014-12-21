namespace CardStatApp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.lstHandFiles = new System.Windows.Forms.ListBox();
            this.button5 = new System.Windows.Forms.Button();
            this.chkMoveFile = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkMessages = new System.Windows.Forms.CheckBox();
            this.chkWarnings = new System.Windows.Forms.CheckBox();
            this.chkErrors = new System.Windows.Forms.CheckBox();
            this.button6 = new System.Windows.Forms.Button();
            this.txtLine = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.button7 = new System.Windows.Forms.Button();
            this.txtSingle = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(168, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Tourney Result Parser";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(8, 32);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(168, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Hand History Parser - All";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(8, 160);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(168, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "Hand Parser - Single";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(8, 88);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(168, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "Hand Parser - All";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // lstHandFiles
            // 
            this.lstHandFiles.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstHandFiles.FormattingEnabled = true;
            this.lstHandFiles.Location = new System.Drawing.Point(0, 0);
            this.lstHandFiles.Name = "lstHandFiles";
            this.lstHandFiles.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstHandFiles.Size = new System.Drawing.Size(440, 336);
            this.lstHandFiles.TabIndex = 4;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(176, 88);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(168, 23);
            this.button5.TabIndex = 5;
            this.button5.Text = "Hand Parser - Selected";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // chkMoveFile
            // 
            this.chkMoveFile.AutoSize = true;
            this.chkMoveFile.Location = new System.Drawing.Point(360, 96);
            this.chkMoveFile.Name = "chkMoveFile";
            this.chkMoveFile.Size = new System.Drawing.Size(72, 17);
            this.chkMoveFile.TabIndex = 6;
            this.chkMoveFile.Text = "Move File";
            this.chkMoveFile.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtSingle);
            this.panel1.Controls.Add(this.button7);
            this.panel1.Controls.Add(this.chkMessages);
            this.panel1.Controls.Add(this.chkWarnings);
            this.panel1.Controls.Add(this.chkErrors);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.txtLine);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.chkMoveFile);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(443, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(447, 336);
            this.panel1.TabIndex = 7;
            // 
            // chkMessages
            // 
            this.chkMessages.AutoSize = true;
            this.chkMessages.Location = new System.Drawing.Point(152, 312);
            this.chkMessages.Name = "chkMessages";
            this.chkMessages.Size = new System.Drawing.Size(74, 17);
            this.chkMessages.TabIndex = 11;
            this.chkMessages.Text = "Messages";
            this.chkMessages.UseVisualStyleBackColor = true;
            // 
            // chkWarnings
            // 
            this.chkWarnings.AutoSize = true;
            this.chkWarnings.Checked = true;
            this.chkWarnings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWarnings.Location = new System.Drawing.Point(72, 312);
            this.chkWarnings.Name = "chkWarnings";
            this.chkWarnings.Size = new System.Drawing.Size(71, 17);
            this.chkWarnings.TabIndex = 10;
            this.chkWarnings.Text = "Warnings";
            this.chkWarnings.UseVisualStyleBackColor = true;
            // 
            // chkErrors
            // 
            this.chkErrors.AutoSize = true;
            this.chkErrors.Checked = true;
            this.chkErrors.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkErrors.Location = new System.Drawing.Point(8, 312);
            this.chkErrors.Name = "chkErrors";
            this.chkErrors.Size = new System.Drawing.Size(53, 17);
            this.chkErrors.TabIndex = 9;
            this.chkErrors.Text = "Errors";
            this.chkErrors.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(8, 216);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(168, 23);
            this.button6.TabIndex = 8;
            this.button6.Text = "Hand History Parser - Line";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // txtLine
            // 
            this.txtLine.Location = new System.Drawing.Point(8, 192);
            this.txtLine.Name = "txtLine";
            this.txtLine.Size = new System.Drawing.Size(424, 20);
            this.txtLine.TabIndex = 7;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(440, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 336);
            this.splitter1.TabIndex = 8;
            this.splitter1.TabStop = false;
            // 
            // txtOutput
            // 
            this.txtOutput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtOutput.Location = new System.Drawing.Point(0, 336);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutput.Size = new System.Drawing.Size(890, 209);
            this.txtOutput.TabIndex = 9;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(176, 32);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(168, 23);
            this.button7.TabIndex = 12;
            this.button7.Text = "Hand History Parser - Selected";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // txtSingle
            // 
            this.txtSingle.Location = new System.Drawing.Point(8, 136);
            this.txtSingle.Name = "txtSingle";
            this.txtSingle.Size = new System.Drawing.Size(424, 20);
            this.txtSingle.TabIndex = 13;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 545);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.lstHandFiles);
            this.Controls.Add(this.txtOutput);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ListBox lstHandFiles;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.CheckBox chkMoveFile;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox txtLine;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.CheckBox chkMessages;
        private System.Windows.Forms.CheckBox chkWarnings;
        private System.Windows.Forms.CheckBox chkErrors;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox txtSingle;
    }
}

