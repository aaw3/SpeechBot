namespace SpeechBot
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
            this.HelpButton = new System.Windows.Forms.Button();
            this.CommandNameTBox = new System.Windows.Forms.TextBox();
            this.CommandDataTBox = new System.Windows.Forms.TextBox();
            this.CategoriesCoBox = new System.Windows.Forms.ComboBox();
            this.CreateCommand = new System.Windows.Forms.Button();
            this.ReloadButton = new System.Windows.Forms.Button();
            this.ReloadCreateChBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ShowChart = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.SaveTimeLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // HelpButton
            // 
            this.HelpButton.Location = new System.Drawing.Point(2, 2);
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(104, 34);
            this.HelpButton.TabIndex = 0;
            this.HelpButton.Text = "Show Commands";
            this.HelpButton.UseVisualStyleBackColor = true;
            this.HelpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // CommandNameTBox
            // 
            this.CommandNameTBox.Location = new System.Drawing.Point(42, 123);
            this.CommandNameTBox.Name = "CommandNameTBox";
            this.CommandNameTBox.Size = new System.Drawing.Size(326, 20);
            this.CommandNameTBox.TabIndex = 1;
            // 
            // CommandDataTBox
            // 
            this.CommandDataTBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CommandDataTBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CommandDataTBox.Location = new System.Drawing.Point(374, 123);
            this.CommandDataTBox.Multiline = true;
            this.CommandDataTBox.Name = "CommandDataTBox";
            this.CommandDataTBox.Size = new System.Drawing.Size(434, 270);
            this.CommandDataTBox.TabIndex = 2;
            // 
            // CategoriesCoBox
            // 
            this.CategoriesCoBox.FormattingEnabled = true;
            this.CategoriesCoBox.Location = new System.Drawing.Point(42, 96);
            this.CategoriesCoBox.Name = "CategoriesCoBox";
            this.CategoriesCoBox.Size = new System.Drawing.Size(170, 21);
            this.CategoriesCoBox.TabIndex = 3;
            this.CategoriesCoBox.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox1_DrawItem);
            this.CategoriesCoBox.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.CategoriesCoBox.DropDownClosed += new System.EventHandler(this.comboBox1_DropDownClosed);
            // 
            // CreateCommand
            // 
            this.CreateCommand.Location = new System.Drawing.Point(279, 165);
            this.CreateCommand.Name = "CreateCommand";
            this.CreateCommand.Size = new System.Drawing.Size(89, 42);
            this.CreateCommand.TabIndex = 4;
            this.CreateCommand.Text = "Create Command";
            this.CreateCommand.UseVisualStyleBackColor = true;
            this.CreateCommand.Click += new System.EventHandler(this.CreateCommand_Click);
            // 
            // ReloadButton
            // 
            this.ReloadButton.Location = new System.Drawing.Point(279, 237);
            this.ReloadButton.Name = "ReloadButton";
            this.ReloadButton.Size = new System.Drawing.Size(89, 42);
            this.ReloadButton.TabIndex = 5;
            this.ReloadButton.Text = "Reload Dictionary";
            this.ReloadButton.UseVisualStyleBackColor = true;
            this.ReloadButton.Click += new System.EventHandler(this.Reload_Button);
            // 
            // ReloadCreateChBox
            // 
            this.ReloadCreateChBox.AutoSize = true;
            this.ReloadCreateChBox.Location = new System.Drawing.Point(162, 174);
            this.ReloadCreateChBox.Name = "ReloadCreateChBox";
            this.ReloadCreateChBox.Size = new System.Drawing.Size(111, 17);
            this.ReloadCreateChBox.TabIndex = 6;
            this.ReloadCreateChBox.Text = "Reload On Create";
            this.ReloadCreateChBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(374, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(434, 36);
            this.label1.TabIndex = 7;
            this.label1.Text = "You can access Form class called \'form\' to access the SpeechBot Application\'s int" +
    "ernals.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Visible = false;
            // 
            // ShowChart
            // 
            this.ShowChart.Location = new System.Drawing.Point(112, 2);
            this.ShowChart.Name = "ShowChart";
            this.ShowChart.Size = new System.Drawing.Size(104, 34);
            this.ShowChart.TabIndex = 8;
            this.ShowChart.Text = "Show Chart";
            this.ShowChart.UseVisualStyleBackColor = true;
            this.ShowChart.Click += new System.EventHandler(this.ShowChart_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(222, 2);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(104, 34);
            this.SaveButton.TabIndex = 9;
            this.SaveButton.Text = "Save Commands";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // SaveTimeLabel
            // 
            this.SaveTimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveTimeLabel.Location = new System.Drawing.Point(332, 5);
            this.SaveTimeLabel.Name = "SaveTimeLabel";
            this.SaveTimeLabel.Size = new System.Drawing.Size(135, 28);
            this.SaveTimeLabel.TabIndex = 10;
            this.SaveTimeLabel.Text = "Last Save:\r\nNone";
            this.SaveTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 450);
            this.Controls.Add(this.SaveTimeLabel);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.ShowChart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ReloadCreateChBox);
            this.Controls.Add(this.ReloadButton);
            this.Controls.Add(this.CreateCommand);
            this.Controls.Add(this.CategoriesCoBox);
            this.Controls.Add(this.CommandDataTBox);
            this.Controls.Add(this.CommandNameTBox);
            this.Controls.Add(this.HelpButton);
            this.Name = "Form1";
            this.Text = "SpeechBot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button HelpButton;
        private System.Windows.Forms.TextBox CommandNameTBox;
        private System.Windows.Forms.ComboBox CategoriesCoBox;
        private System.Windows.Forms.Button ReloadButton;
        private System.Windows.Forms.CheckBox ReloadCreateChBox;
        private System.Windows.Forms.Button CreateCommand;
        private System.Windows.Forms.TextBox CommandDataTBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ShowChart;
        private System.Windows.Forms.Button SaveButton;
        public System.Windows.Forms.Label SaveTimeLabel;
    }
}

