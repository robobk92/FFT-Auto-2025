namespace SLTtechSoft
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.lblConlai = new System.Windows.Forms.Label();
            this.lblTongWo = new System.Windows.Forms.Label();
            this.tbSKUCode = new System.Windows.Forms.Panel();
            this.btnLoadSoPo = new System.Windows.Forms.Button();
            this.btnLoadWO = new System.Windows.Forms.Button();
            this.btnLoadSKUCode = new System.Windows.Forms.Button();
            this.tbSoPo = new SLTSoft.RJControl.RJTextBox();
            this.tbWO = new SLTSoft.RJControl.RJTextBox();
            this.rjTextBox1 = new SLTSoft.RJControl.RJTextBox();
            this.cbTurnOnVoice = new System.Windows.Forms.CheckBox();
            this.cbTestAgain = new System.Windows.Forms.CheckBox();
            this.cbCloseDoorFFT = new System.Windows.Forms.CheckBox();
            this.cbManualMac = new System.Windows.Forms.CheckBox();
            this.cbUseMacMes = new System.Windows.Forms.CheckBox();
            this.cbResetWhenSetDefault = new System.Windows.Forms.CheckBox();
            this.cbAskSupply = new System.Windows.Forms.CheckBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Detail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Min = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Max = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Timer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Timeout = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblDaDat = new System.Windows.Forms.Label();
            this.tbMacDevice = new SLTSoft.RJControl.RJTextBox();
            this.tbMainQR = new SLTSoft.RJControl.RJTextBox();
            this.tbFrontQR = new SLTSoft.RJControl.RJTextBox();
            this.tbSerialNumber = new SLTSoft.RJControl.RJTextBox();
            this.tbMacMes = new SLTSoft.RJControl.RJTextBox();
            this.panel27 = new System.Windows.Forms.Panel();
            this.btnskip = new System.Windows.Forms.Button();
            this.tbUSBScan = new SLTSoft.RJControl.RJTextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.panel25 = new System.Windows.Forms.Panel();
            this.panel26 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel22 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbSKUCode.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel27.SuspendLayout();
            this.panel25.SuspendLayout();
            this.panel26.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel22.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblConlai
            // 
            resources.ApplyResources(this.lblConlai, "lblConlai");
            this.lblConlai.Name = "lblConlai";
            // 
            // lblTongWo
            // 
            resources.ApplyResources(this.lblTongWo, "lblTongWo");
            this.lblTongWo.Name = "lblTongWo";
            // 
            // tbSKUCode
            // 
            this.tbSKUCode.Controls.Add(this.btnLoadSoPo);
            this.tbSKUCode.Controls.Add(this.btnLoadWO);
            this.tbSKUCode.Controls.Add(this.btnLoadSKUCode);
            this.tbSKUCode.Controls.Add(this.tbSoPo);
            this.tbSKUCode.Controls.Add(this.tbWO);
            this.tbSKUCode.Controls.Add(this.rjTextBox1);
            this.tbSKUCode.Controls.Add(this.cbTurnOnVoice);
            this.tbSKUCode.Controls.Add(this.cbTestAgain);
            this.tbSKUCode.Controls.Add(this.cbCloseDoorFFT);
            this.tbSKUCode.Controls.Add(this.cbManualMac);
            this.tbSKUCode.Controls.Add(this.cbUseMacMes);
            this.tbSKUCode.Controls.Add(this.cbResetWhenSetDefault);
            this.tbSKUCode.Controls.Add(this.cbAskSupply);
            this.tbSKUCode.Controls.Add(this.label30);
            this.tbSKUCode.Controls.Add(this.label31);
            this.tbSKUCode.Controls.Add(this.label32);
            resources.ApplyResources(this.tbSKUCode, "tbSKUCode");
            this.tbSKUCode.Name = "tbSKUCode";
            // 
            // btnLoadSoPo
            // 
            this.btnLoadSoPo.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadSoPo.BackgroundImage = global::SLTtechSoft.Properties.Resources.HandPointFinger;
            resources.ApplyResources(this.btnLoadSoPo, "btnLoadSoPo");
            this.btnLoadSoPo.ForeColor = System.Drawing.Color.White;
            this.btnLoadSoPo.Name = "btnLoadSoPo";
            this.btnLoadSoPo.UseVisualStyleBackColor = false;
            // 
            // btnLoadWO
            // 
            this.btnLoadWO.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadWO.BackgroundImage = global::SLTtechSoft.Properties.Resources.HandPointFinger;
            resources.ApplyResources(this.btnLoadWO, "btnLoadWO");
            this.btnLoadWO.ForeColor = System.Drawing.Color.White;
            this.btnLoadWO.Name = "btnLoadWO";
            this.btnLoadWO.UseVisualStyleBackColor = false;
            // 
            // btnLoadSKUCode
            // 
            this.btnLoadSKUCode.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadSKUCode.BackgroundImage = global::SLTtechSoft.Properties.Resources.HandPointFinger;
            resources.ApplyResources(this.btnLoadSKUCode, "btnLoadSKUCode");
            this.btnLoadSKUCode.ForeColor = System.Drawing.Color.White;
            this.btnLoadSKUCode.Name = "btnLoadSKUCode";
            this.btnLoadSKUCode.UseVisualStyleBackColor = false;
            // 
            // tbSoPo
            // 
            this.tbSoPo.BackColor = System.Drawing.SystemColors.Window;
            this.tbSoPo.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.tbSoPo.BorderFocusColor = System.Drawing.Color.HotPink;
            this.tbSoPo.BorderSize = 2;
            resources.ApplyResources(this.tbSoPo, "tbSoPo");
            this.tbSoPo.ForeColor = System.Drawing.Color.DimGray;
            this.tbSoPo.Multiline = false;
            this.tbSoPo.Name = "tbSoPo";
            this.tbSoPo.PasswordChar = false;
            this.tbSoPo.Texts = "";
            this.tbSoPo.UnderlinedStyle = true;
            // 
            // tbWO
            // 
            this.tbWO.BackColor = System.Drawing.SystemColors.Window;
            this.tbWO.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.tbWO.BorderFocusColor = System.Drawing.Color.HotPink;
            this.tbWO.BorderSize = 2;
            resources.ApplyResources(this.tbWO, "tbWO");
            this.tbWO.ForeColor = System.Drawing.Color.DimGray;
            this.tbWO.Multiline = false;
            this.tbWO.Name = "tbWO";
            this.tbWO.PasswordChar = false;
            this.tbWO.Texts = "";
            this.tbWO.UnderlinedStyle = true;
            // 
            // rjTextBox1
            // 
            this.rjTextBox1.BackColor = System.Drawing.SystemColors.Window;
            this.rjTextBox1.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.rjTextBox1.BorderFocusColor = System.Drawing.Color.HotPink;
            this.rjTextBox1.BorderSize = 2;
            resources.ApplyResources(this.rjTextBox1, "rjTextBox1");
            this.rjTextBox1.ForeColor = System.Drawing.Color.DimGray;
            this.rjTextBox1.Multiline = false;
            this.rjTextBox1.Name = "rjTextBox1";
            this.rjTextBox1.PasswordChar = false;
            this.rjTextBox1.Texts = "";
            this.rjTextBox1.UnderlinedStyle = true;
            // 
            // cbTurnOnVoice
            // 
            resources.ApplyResources(this.cbTurnOnVoice, "cbTurnOnVoice");
            this.cbTurnOnVoice.Name = "cbTurnOnVoice";
            this.cbTurnOnVoice.UseVisualStyleBackColor = true;
            // 
            // cbTestAgain
            // 
            resources.ApplyResources(this.cbTestAgain, "cbTestAgain");
            this.cbTestAgain.Name = "cbTestAgain";
            this.cbTestAgain.UseVisualStyleBackColor = true;
            // 
            // cbCloseDoorFFT
            // 
            resources.ApplyResources(this.cbCloseDoorFFT, "cbCloseDoorFFT");
            this.cbCloseDoorFFT.Name = "cbCloseDoorFFT";
            this.cbCloseDoorFFT.UseVisualStyleBackColor = true;
            // 
            // cbManualMac
            // 
            resources.ApplyResources(this.cbManualMac, "cbManualMac");
            this.cbManualMac.Name = "cbManualMac";
            this.cbManualMac.UseVisualStyleBackColor = true;
            // 
            // cbUseMacMes
            // 
            resources.ApplyResources(this.cbUseMacMes, "cbUseMacMes");
            this.cbUseMacMes.Name = "cbUseMacMes";
            this.cbUseMacMes.UseVisualStyleBackColor = true;
            // 
            // cbResetWhenSetDefault
            // 
            resources.ApplyResources(this.cbResetWhenSetDefault, "cbResetWhenSetDefault");
            this.cbResetWhenSetDefault.Name = "cbResetWhenSetDefault";
            this.cbResetWhenSetDefault.UseVisualStyleBackColor = true;
            // 
            // cbAskSupply
            // 
            resources.ApplyResources(this.cbAskSupply, "cbAskSupply");
            this.cbAskSupply.Name = "cbAskSupply";
            this.cbAskSupply.UseVisualStyleBackColor = true;
            // 
            // label30
            // 
            resources.ApplyResources(this.label30, "label30");
            this.label30.Name = "label30";
            // 
            // label31
            // 
            resources.ApplyResources(this.label31, "label31");
            this.label31.Name = "label31";
            // 
            // label32
            // 
            resources.ApplyResources(this.label32, "label32");
            this.label32.Name = "label32";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_ID,
            this.Column_Name,
            this.Column_Detail,
            this.Column_Value,
            this.Column_Min,
            this.Column_Max,
            this.Column_Unit,
            this.Column_Timer,
            this.Column_Result,
            this.Column2,
            this.Column1,
            this.Column_Timeout});
            resources.ApplyResources(this.dataGridView1, "dataGridView1");
            this.dataGridView1.Name = "dataGridView1";
            // 
            // Column_ID
            // 
            this.Column_ID.FillWeight = 32.69474F;
            resources.ApplyResources(this.Column_ID, "Column_ID");
            this.Column_ID.Name = "Column_ID";
            // 
            // Column_Name
            // 
            this.Column_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_Name.FillWeight = 32.69474F;
            resources.ApplyResources(this.Column_Name, "Column_Name");
            this.Column_Name.Name = "Column_Name";
            // 
            // Column_Detail
            // 
            this.Column_Detail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_Detail.FillWeight = 32.69474F;
            resources.ApplyResources(this.Column_Detail, "Column_Detail");
            this.Column_Detail.Name = "Column_Detail";
            // 
            // Column_Value
            // 
            this.Column_Value.FillWeight = 32.69474F;
            resources.ApplyResources(this.Column_Value, "Column_Value");
            this.Column_Value.Name = "Column_Value";
            // 
            // Column_Min
            // 
            this.Column_Min.FillWeight = 32.69474F;
            resources.ApplyResources(this.Column_Min, "Column_Min");
            this.Column_Min.Name = "Column_Min";
            // 
            // Column_Max
            // 
            this.Column_Max.FillWeight = 32.69474F;
            resources.ApplyResources(this.Column_Max, "Column_Max");
            this.Column_Max.Name = "Column_Max";
            // 
            // Column_Unit
            // 
            this.Column_Unit.FillWeight = 32.69474F;
            resources.ApplyResources(this.Column_Unit, "Column_Unit");
            this.Column_Unit.Name = "Column_Unit";
            // 
            // Column_Timer
            // 
            this.Column_Timer.FillWeight = 144.5108F;
            resources.ApplyResources(this.Column_Timer, "Column_Timer");
            this.Column_Timer.Name = "Column_Timer";
            // 
            // Column_Result
            // 
            this.Column_Result.FillWeight = 111.1621F;
            resources.ApplyResources(this.Column_Result, "Column_Result");
            this.Column_Result.Name = "Column_Result";
            // 
            // Column2
            // 
            resources.ApplyResources(this.Column2, "Column2");
            this.Column2.Name = "Column2";
            // 
            // Column1
            // 
            resources.ApplyResources(this.Column1, "Column1");
            this.Column1.Name = "Column1";
            // 
            // Column_Timeout
            // 
            this.Column_Timeout.FillWeight = 515.4639F;
            resources.ApplyResources(this.Column_Timeout, "Column_Timeout");
            this.Column_Timeout.Name = "Column_Timeout";
            // 
            // lblDaDat
            // 
            resources.ApplyResources(this.lblDaDat, "lblDaDat");
            this.lblDaDat.Name = "lblDaDat";
            // 
            // tbMacDevice
            // 
            this.tbMacDevice.BackColor = System.Drawing.SystemColors.Window;
            this.tbMacDevice.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.tbMacDevice.BorderFocusColor = System.Drawing.Color.HotPink;
            this.tbMacDevice.BorderSize = 2;
            resources.ApplyResources(this.tbMacDevice, "tbMacDevice");
            this.tbMacDevice.ForeColor = System.Drawing.Color.DimGray;
            this.tbMacDevice.Multiline = false;
            this.tbMacDevice.Name = "tbMacDevice";
            this.tbMacDevice.PasswordChar = false;
            this.tbMacDevice.Texts = "";
            this.tbMacDevice.UnderlinedStyle = true;
            // 
            // tbMainQR
            // 
            this.tbMainQR.BackColor = System.Drawing.SystemColors.Window;
            this.tbMainQR.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.tbMainQR.BorderFocusColor = System.Drawing.Color.HotPink;
            this.tbMainQR.BorderSize = 2;
            resources.ApplyResources(this.tbMainQR, "tbMainQR");
            this.tbMainQR.ForeColor = System.Drawing.Color.DimGray;
            this.tbMainQR.Multiline = false;
            this.tbMainQR.Name = "tbMainQR";
            this.tbMainQR.PasswordChar = false;
            this.tbMainQR.Texts = "";
            this.tbMainQR.UnderlinedStyle = true;
            // 
            // tbFrontQR
            // 
            this.tbFrontQR.BackColor = System.Drawing.SystemColors.Window;
            this.tbFrontQR.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.tbFrontQR.BorderFocusColor = System.Drawing.Color.HotPink;
            this.tbFrontQR.BorderSize = 2;
            resources.ApplyResources(this.tbFrontQR, "tbFrontQR");
            this.tbFrontQR.ForeColor = System.Drawing.Color.DimGray;
            this.tbFrontQR.Multiline = false;
            this.tbFrontQR.Name = "tbFrontQR";
            this.tbFrontQR.PasswordChar = false;
            this.tbFrontQR.Texts = "";
            this.tbFrontQR.UnderlinedStyle = true;
            // 
            // tbSerialNumber
            // 
            this.tbSerialNumber.BackColor = System.Drawing.SystemColors.Window;
            this.tbSerialNumber.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.tbSerialNumber.BorderFocusColor = System.Drawing.Color.HotPink;
            this.tbSerialNumber.BorderSize = 2;
            resources.ApplyResources(this.tbSerialNumber, "tbSerialNumber");
            this.tbSerialNumber.ForeColor = System.Drawing.Color.DimGray;
            this.tbSerialNumber.Multiline = false;
            this.tbSerialNumber.Name = "tbSerialNumber";
            this.tbSerialNumber.PasswordChar = false;
            this.tbSerialNumber.Texts = "";
            this.tbSerialNumber.UnderlinedStyle = true;
            // 
            // tbMacMes
            // 
            this.tbMacMes.BackColor = System.Drawing.SystemColors.Window;
            this.tbMacMes.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.tbMacMes.BorderFocusColor = System.Drawing.Color.HotPink;
            this.tbMacMes.BorderSize = 2;
            resources.ApplyResources(this.tbMacMes, "tbMacMes");
            this.tbMacMes.ForeColor = System.Drawing.Color.DimGray;
            this.tbMacMes.Multiline = false;
            this.tbMacMes.Name = "tbMacMes";
            this.tbMacMes.PasswordChar = false;
            this.tbMacMes.Texts = "";
            this.tbMacMes.UnderlinedStyle = true;
            // 
            // panel27
            // 
            this.panel27.Controls.Add(this.btnskip);
            this.panel27.Controls.Add(this.tbMacMes);
            this.panel27.Controls.Add(this.tbUSBScan);
            this.panel27.Controls.Add(this.tbMacDevice);
            this.panel27.Controls.Add(this.tbMainQR);
            this.panel27.Controls.Add(this.tbFrontQR);
            this.panel27.Controls.Add(this.tbSerialNumber);
            this.panel27.Controls.Add(this.label20);
            this.panel27.Controls.Add(this.label19);
            this.panel27.Controls.Add(this.label18);
            this.panel27.Controls.Add(this.label17);
            this.panel27.Controls.Add(this.label16);
            this.panel27.Controls.Add(this.label15);
            resources.ApplyResources(this.panel27, "panel27");
            this.panel27.Name = "panel27";
            // 
            // btnskip
            // 
            resources.ApplyResources(this.btnskip, "btnskip");
            this.btnskip.Name = "btnskip";
            this.btnskip.UseVisualStyleBackColor = true;
            this.btnskip.Click += new System.EventHandler(this.btnskip_Click);
            // 
            // tbUSBScan
            // 
            this.tbUSBScan.BackColor = System.Drawing.SystemColors.Window;
            this.tbUSBScan.BorderColor = System.Drawing.Color.MediumSlateBlue;
            this.tbUSBScan.BorderFocusColor = System.Drawing.Color.HotPink;
            this.tbUSBScan.BorderSize = 2;
            resources.ApplyResources(this.tbUSBScan, "tbUSBScan");
            this.tbUSBScan.ForeColor = System.Drawing.Color.DimGray;
            this.tbUSBScan.Multiline = false;
            this.tbUSBScan.Name = "tbUSBScan";
            this.tbUSBScan.PasswordChar = false;
            this.tbUSBScan.Texts = "";
            this.tbUSBScan.UnderlinedStyle = true;
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // panel25
            // 
            this.panel25.Controls.Add(this.panel27);
            this.panel25.Controls.Add(this.panel26);
            resources.ApplyResources(this.panel25, "panel25");
            this.panel25.Name = "panel25";
            // 
            // panel26
            // 
            this.panel26.Controls.Add(this.tableLayoutPanel2);
            resources.ApplyResources(this.panel26, "panel26");
            this.panel26.Name = "panel26";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.lblConlai, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblDaDat, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblTongWo, 0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // panel22
            // 
            this.panel22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel22.Controls.Add(this.panel25);
            this.panel22.Controls.Add(this.tbSKUCode);
            resources.ApplyResources(this.panel22, "panel22");
            this.panel22.Name = "panel22";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel22);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // Main
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(234)))), ((int)(((byte)(248)))));
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Main";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.Main_Shown);
            this.tbSKUCode.ResumeLayout(false);
            this.tbSKUCode.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel27.ResumeLayout(false);
            this.panel27.PerformLayout();
            this.panel25.ResumeLayout(false);
            this.panel26.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel22.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ToggleSwitch btnAuto;
        private ToggleSwitch btnStop;
        private ToggleSwitch btnBypass;
        private ToggleSwitch btnReset;
        private ToggleSwitch btnRun;
        private ToggleSwitch btnEMGStop;
        private ToggleSwitch btnHome;
        private ToggleSwitch btnPassNGScan;
        private ToggleSwitch btnBuzz;
        public ToggleSwitch btnNoPassToNextMc;
        public ToggleSwitch btnReScan;
        private ToggleSwitch btnManualScan;
        public System.Windows.Forms.Label lblConlai;
        public System.Windows.Forms.Label lblTongWo;
        private System.Windows.Forms.Panel tbSKUCode;
        public System.Windows.Forms.Button btnLoadSoPo;
        public System.Windows.Forms.Button btnLoadWO;
        public System.Windows.Forms.Button btnLoadSKUCode;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.Label lblDaDat;
        private System.Windows.Forms.Panel panel27;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Panel panel25;
        private System.Windows.Forms.Panel panel26;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel22;
        private System.Windows.Forms.Panel panel1;
        public SLTSoft.RJControl.RJTextBox tbSoPo;
        public SLTSoft.RJControl.RJTextBox tbWO;
        public SLTSoft.RJControl.RJTextBox rjTextBox1;
        public SLTSoft.RJControl.RJTextBox tbMacDevice;
        public SLTSoft.RJControl.RJTextBox tbMainQR;
        public SLTSoft.RJControl.RJTextBox tbFrontQR;
        public SLTSoft.RJControl.RJTextBox tbSerialNumber;
        public SLTSoft.RJControl.RJTextBox tbMacMes;
        public SLTSoft.RJControl.RJTextBox tbUSBScan;
        public System.Windows.Forms.CheckBox cbTurnOnVoice;
        public System.Windows.Forms.CheckBox cbTestAgain;
        public System.Windows.Forms.CheckBox cbCloseDoorFFT;
        public System.Windows.Forms.CheckBox cbManualMac;
        public System.Windows.Forms.CheckBox cbUseMacMes;
        public System.Windows.Forms.CheckBox cbResetWhenSetDefault;
        public System.Windows.Forms.CheckBox cbAskSupply;
        public System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Detail;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Min;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Max;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Timer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Result;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Timeout;
        private System.Windows.Forms.Button btnskip;
    }
}