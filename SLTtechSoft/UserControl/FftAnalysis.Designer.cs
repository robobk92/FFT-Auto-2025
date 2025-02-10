
namespace Io_JigTest_Motor
{
    partial class FftAnalysis
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblPeak = new System.Windows.Forms.Label();
            this.cbDevices = new System.Windows.Forms.ComboBox();
            this.lbPower = new System.Windows.Forms.Label();
            this.cbPeak = new System.Windows.Forms.CheckBox();
            this.cbDecibel = new System.Windows.Forms.CheckBox();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.cbAutoAxis = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnOnOff = new System.Windows.Forms.Button();
            this.lblCaption = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblPeak);
            this.panel1.Controls.Add(this.cbDevices);
            this.panel1.Controls.Add(this.lbPower);
            this.panel1.Controls.Add(this.cbPeak);
            this.panel1.Controls.Add(this.cbDecibel);
            this.panel1.Controls.Add(this.txtLog);
            this.panel1.Controls.Add(this.cbAutoAxis);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.formsPlot1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(384, 188);
            this.panel1.TabIndex = 153;
            // 
            // lblPeak
            // 
            this.lblPeak.AutoSize = true;
            this.lblPeak.Location = new System.Drawing.Point(7, 90);
            this.lblPeak.Name = "lblPeak";
            this.lblPeak.Size = new System.Drawing.Size(131, 13);
            this.lblPeak.TabIndex = 175;
            this.lblPeak.Text = "Peak Frequency: 1234 Hz";
            // 
            // cbDevices
            // 
            this.cbDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDevices.FormattingEnabled = true;
            this.cbDevices.Location = new System.Drawing.Point(3, 60);
            this.cbDevices.Name = "cbDevices";
            this.cbDevices.Size = new System.Drawing.Size(135, 21);
            this.cbDevices.TabIndex = 171;
            this.cbDevices.SelectedIndexChanged += new System.EventHandler(this.cbDevices_SelectedIndexChanged);
            // 
            // lbPower
            // 
            this.lbPower.AutoSize = true;
            this.lbPower.Location = new System.Drawing.Point(7, 113);
            this.lbPower.Name = "lbPower";
            this.lbPower.Size = new System.Drawing.Size(82, 13);
            this.lbPower.TabIndex = 179;
            this.lbPower.Text = "Power: 1234 db";
            // 
            // cbPeak
            // 
            this.cbPeak.AutoSize = true;
            this.cbPeak.Checked = true;
            this.cbPeak.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPeak.Location = new System.Drawing.Point(10, 129);
            this.cbPeak.Name = "cbPeak";
            this.cbPeak.Size = new System.Drawing.Size(51, 17);
            this.cbPeak.TabIndex = 178;
            this.cbPeak.Text = "Peak";
            this.cbPeak.UseVisualStyleBackColor = true;
            // 
            // cbDecibel
            // 
            this.cbDecibel.AutoSize = true;
            this.cbDecibel.Checked = true;
            this.cbDecibel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbDecibel.Location = new System.Drawing.Point(10, 163);
            this.cbDecibel.Name = "cbDecibel";
            this.cbDecibel.Size = new System.Drawing.Size(39, 17);
            this.cbDecibel.TabIndex = 177;
            this.cbDecibel.Text = "dB";
            this.cbDecibel.UseVisualStyleBackColor = true;
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(264, 54);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(111, 81);
            this.txtLog.TabIndex = 176;
            this.txtLog.Text = "";
            // 
            // cbAutoAxis
            // 
            this.cbAutoAxis.AutoSize = true;
            this.cbAutoAxis.Checked = true;
            this.cbAutoAxis.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAutoAxis.Location = new System.Drawing.Point(10, 146);
            this.cbAutoAxis.Name = "cbAutoAxis";
            this.cbAutoAxis.Size = new System.Drawing.Size(70, 17);
            this.cbAutoAxis.TabIndex = 173;
            this.cbAutoAxis.Text = "Auto-Axis";
            this.cbAutoAxis.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 172;
            this.label1.Text = "Audio Device";
            // 
            // formsPlot1
            // 
            this.formsPlot1.BackColor = System.Drawing.Color.Transparent;
            this.formsPlot1.Location = new System.Drawing.Point(109, 25);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(174, 150);
            this.formsPlot1.TabIndex = 170;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gainsboro;
            this.panel2.Controls.Add(this.btnOnOff);
            this.panel2.Controls.Add(this.lblCaption);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(382, 24);
            this.panel2.TabIndex = 169;
            // 
            // btnOnOff
            // 
            this.btnOnOff.BackColor = System.Drawing.Color.Black;
            this.btnOnOff.Location = new System.Drawing.Point(1, 0);
            this.btnOnOff.Name = "btnOnOff";
            this.btnOnOff.Size = new System.Drawing.Size(26, 24);
            this.btnOnOff.TabIndex = 119;
            this.btnOnOff.UseVisualStyleBackColor = false;
            // 
            // lblCaption
            // 
            this.lblCaption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCaption.BackColor = System.Drawing.Color.Gainsboro;
            this.lblCaption.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCaption.ForeColor = System.Drawing.Color.DarkBlue;
            this.lblCaption.Location = new System.Drawing.Point(27, 2);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(356, 20);
            this.lblCaption.TabIndex = 155;
            this.lblCaption.Text = "FFT ANALYSIS";
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timer1
            // 
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FftAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "FftAnalysis";
            this.Size = new System.Drawing.Size(384, 188);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnOnOff;
        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.Label lblPeak;
        private System.Windows.Forms.CheckBox cbAutoAxis;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbDevices;
        private ScottPlot.FormsPlot formsPlot1;
        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox cbPeak;
        private System.Windows.Forms.CheckBox cbDecibel;
        private System.Windows.Forms.Label lbPower;
    }
}
