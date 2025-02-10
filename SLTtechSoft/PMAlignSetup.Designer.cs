namespace SLTtechSoft
{
    partial class PMAlignSetup
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmdPatMaxSetupCommand = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtPatMaxScoreValue = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdImageAcquisitionLiveOrOpenCommand = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txbTrainOriginX = new System.Windows.Forms.TextBox();
            this.txbTrainOriginY = new System.Windows.Forms.TextBox();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(476, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(583, 537);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(476, 537);
            this.panel2.TabIndex = 1;
            // 
            // cmdPatMaxSetupCommand
            // 
            this.cmdPatMaxSetupCommand.Location = new System.Drawing.Point(10, 6);
            this.cmdPatMaxSetupCommand.Name = "cmdPatMaxSetupCommand";
            this.cmdPatMaxSetupCommand.Size = new System.Drawing.Size(74, 48);
            this.cmdPatMaxSetupCommand.TabIndex = 4;
            this.cmdPatMaxSetupCommand.Text = "Setup\r\nTrain Image";
            this.cmdPatMaxSetupCommand.Click += new System.EventHandler(this.cmdPatMaxSetupCommand_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtPatMaxScoreValue);
            this.panel3.Controls.Add(this.Label1);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.cmdImageAcquisitionLiveOrOpenCommand);
            this.panel3.Controls.Add(this.textBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(476, 102);
            this.panel3.TabIndex = 0;
            // 
            // txtPatMaxScoreValue
            // 
            this.txtPatMaxScoreValue.Location = new System.Drawing.Point(147, 49);
            this.txtPatMaxScoreValue.Multiline = true;
            this.txtPatMaxScoreValue.Name = "txtPatMaxScoreValue";
            this.txtPatMaxScoreValue.Size = new System.Drawing.Size(80, 40);
            this.txtPatMaxScoreValue.TabIndex = 6;
            this.txtPatMaxScoreValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Label1
            // 
            this.Label1.Location = new System.Drawing.Point(93, 53);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(48, 32);
            this.Label1.TabIndex = 7;
            this.Label1.Text = "Score";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Tool Name";
            // 
            // cmdImageAcquisitionLiveOrOpenCommand
            // 
            this.cmdImageAcquisitionLiveOrOpenCommand.Location = new System.Drawing.Point(12, 49);
            this.cmdImageAcquisitionLiveOrOpenCommand.Name = "cmdImageAcquisitionLiveOrOpenCommand";
            this.cmdImageAcquisitionLiveOrOpenCommand.Size = new System.Drawing.Size(75, 40);
            this.cmdImageAcquisitionLiveOrOpenCommand.TabIndex = 6;
            this.cmdImageAcquisitionLiveOrOpenCommand.Text = "Run";
            this.cmdImageAcquisitionLiveOrOpenCommand.Click += new System.EventHandler(this.cmdImageAcquisitionLiveOrOpenCommand_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(76, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(253, 20);
            this.textBox1.TabIndex = 7;
            this.textBox1.Text = "PMAlignTool1";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 102);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(476, 435);
            this.panel4.TabIndex = 5;
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(476, 70);
            this.panel5.TabIndex = 0;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.tabControl1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(0, 70);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(476, 365);
            this.panel6.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(476, 365);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txbTrainOriginY);
            this.tabPage1.Controls.Add(this.txbTrainOriginX);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.cmdPatMaxSetupCommand);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(468, 339);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(192, 74);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(102, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Origin Y:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(102, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Origin X:";
            // 
            // txbTrainOriginX
            // 
            this.txbTrainOriginX.Location = new System.Drawing.Point(155, 6);
            this.txbTrainOriginX.Name = "txbTrainOriginX";
            this.txbTrainOriginX.Size = new System.Drawing.Size(100, 20);
            this.txbTrainOriginX.TabIndex = 10;
            // 
            // txbTrainOriginY
            // 
            this.txbTrainOriginY.Location = new System.Drawing.Point(155, 38);
            this.txbTrainOriginY.Name = "txbTrainOriginY";
            this.txbTrainOriginY.Size = new System.Drawing.Size(100, 20);
            this.txbTrainOriginY.TabIndex = 11;
            // 
            // PMAlignSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1059, 537);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "PMAlignSetup";
            this.Text = "PMAlignSetup";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.PMAlignSetup_FormClosed);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button cmdImageAcquisitionLiveOrOpenCommand;
        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.TextBox txtPatMaxScoreValue;
        private System.Windows.Forms.Button cmdPatMaxSetupCommand;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txbTrainOriginY;
        private System.Windows.Forms.TextBox txbTrainOriginX;
    }
}