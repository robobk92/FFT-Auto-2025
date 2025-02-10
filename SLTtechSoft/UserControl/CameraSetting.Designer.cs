namespace SLTtechSoft
{
    partial class CameraSetting
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
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.TriggerAuto = new SLTSoft.RJControl.RJRadio();
            this.TriggerManual = new SLTSoft.RJControl.RJRadio();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnConnectCam = new System.Windows.Forms.Button();
            this.RefreshFileImage = new System.Windows.Forms.Button();
            this.RefreshFolderImage = new System.Windows.Forms.Button();
            this.FileDirector = new System.Windows.Forms.TextBox();
            this.FolderDirector = new System.Windows.Forms.TextBox();
            this.ModeFile = new SLTSoft.RJControl.RJRadio();
            this.ModeFolder = new SLTSoft.RJControl.RJRadio();
            this.ModeCam = new SLTSoft.RJControl.RJRadio();
            this.label29 = new System.Windows.Forms.Label();
            this.IPAddress = new System.Windows.Forms.TextBox();
            this.Use = new System.Windows.Forms.CheckBox();
            this.SaveGraphic = new System.Windows.Forms.CheckBox();
            this.SaveOrigin = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.SaveOriginSeparate = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.groupBox6.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.TriggerAuto);
            this.groupBox6.Controls.Add(this.TriggerManual);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox6.Location = new System.Drawing.Point(0, 0);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBox6.Size = new System.Drawing.Size(849, 113);
            this.groupBox6.TabIndex = 24;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Trigger Mode";
            // 
            // TriggerAuto
            // 
            this.TriggerAuto.AutoSize = true;
            this.TriggerAuto.BackColor = System.Drawing.Color.White;
            this.TriggerAuto.CheckedColor = System.Drawing.Color.RoyalBlue;
            this.TriggerAuto.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.TriggerAuto.Location = new System.Drawing.Point(5, 71);
            this.TriggerAuto.Margin = new System.Windows.Forms.Padding(4);
            this.TriggerAuto.MinimumSize = new System.Drawing.Size(0, 27);
            this.TriggerAuto.Name = "TriggerAuto";
            this.TriggerAuto.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.TriggerAuto.Size = new System.Drawing.Size(132, 27);
            this.TriggerAuto.TabIndex = 7;
            this.TriggerAuto.Text = "Hardware Auto";
            this.TriggerAuto.UnCheckedColor = System.Drawing.Color.Black;
            this.TriggerAuto.UseVisualStyleBackColor = false;
            // 
            // TriggerManual
            // 
            this.TriggerManual.AutoSize = true;
            this.TriggerManual.BackColor = System.Drawing.Color.White;
            this.TriggerManual.CheckedColor = System.Drawing.Color.RoyalBlue;
            this.TriggerManual.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.TriggerManual.Location = new System.Drawing.Point(5, 37);
            this.TriggerManual.Margin = new System.Windows.Forms.Padding(4);
            this.TriggerManual.MinimumSize = new System.Drawing.Size(0, 27);
            this.TriggerManual.Name = "TriggerManual";
            this.TriggerManual.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.TriggerManual.Size = new System.Drawing.Size(85, 27);
            this.TriggerManual.TabIndex = 6;
            this.TriggerManual.Text = "Manual";
            this.TriggerManual.UnCheckedColor = System.Drawing.Color.Black;
            this.TriggerManual.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnConnectCam);
            this.groupBox1.Controls.Add(this.RefreshFileImage);
            this.groupBox1.Controls.Add(this.RefreshFolderImage);
            this.groupBox1.Controls.Add(this.FileDirector);
            this.groupBox1.Controls.Add(this.FolderDirector);
            this.groupBox1.Controls.Add(this.ModeFile);
            this.groupBox1.Controls.Add(this.ModeFolder);
            this.groupBox1.Controls.Add(this.ModeCam);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.groupBox1.Size = new System.Drawing.Size(849, 172);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mode";
            // 
            // btnConnectCam
            // 
            this.btnConnectCam.Location = new System.Drawing.Point(597, 34);
            this.btnConnectCam.Name = "btnConnectCam";
            this.btnConnectCam.Size = new System.Drawing.Size(114, 39);
            this.btnConnectCam.TabIndex = 42;
            this.btnConnectCam.Text = "Connect";
            this.btnConnectCam.UseVisualStyleBackColor = true;
            // 
            // RefreshFileImage
            // 
            this.RefreshFileImage.Location = new System.Drawing.Point(597, 127);
            this.RefreshFileImage.Name = "RefreshFileImage";
            this.RefreshFileImage.Size = new System.Drawing.Size(114, 39);
            this.RefreshFileImage.TabIndex = 41;
            this.RefreshFileImage.Text = "Refresh";
            this.RefreshFileImage.UseVisualStyleBackColor = true;
            // 
            // RefreshFolderImage
            // 
            this.RefreshFolderImage.Location = new System.Drawing.Point(597, 82);
            this.RefreshFolderImage.Name = "RefreshFolderImage";
            this.RefreshFolderImage.Size = new System.Drawing.Size(114, 39);
            this.RefreshFolderImage.TabIndex = 40;
            this.RefreshFolderImage.Text = "Refresh";
            this.RefreshFolderImage.UseVisualStyleBackColor = true;
            // 
            // FileDirector
            // 
            this.FileDirector.BackColor = System.Drawing.Color.White;
            this.FileDirector.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.FileDirector.Location = new System.Drawing.Point(211, 128);
            this.FileDirector.Margin = new System.Windows.Forms.Padding(4);
            this.FileDirector.Name = "FileDirector";
            this.FileDirector.ReadOnly = true;
            this.FileDirector.Size = new System.Drawing.Size(370, 26);
            this.FileDirector.TabIndex = 39;
            this.FileDirector.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FolderDirector
            // 
            this.FolderDirector.BackColor = System.Drawing.Color.White;
            this.FolderDirector.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.FolderDirector.Location = new System.Drawing.Point(209, 82);
            this.FolderDirector.Margin = new System.Windows.Forms.Padding(4);
            this.FolderDirector.Name = "FolderDirector";
            this.FolderDirector.ReadOnly = true;
            this.FolderDirector.Size = new System.Drawing.Size(370, 26);
            this.FolderDirector.TabIndex = 38;
            this.FolderDirector.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ModeFile
            // 
            this.ModeFile.AutoSize = true;
            this.ModeFile.BackColor = System.Drawing.Color.White;
            this.ModeFile.CheckedColor = System.Drawing.Color.RoyalBlue;
            this.ModeFile.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ModeFile.Location = new System.Drawing.Point(12, 126);
            this.ModeFile.Margin = new System.Windows.Forms.Padding(4);
            this.ModeFile.MinimumSize = new System.Drawing.Size(0, 27);
            this.ModeFile.Name = "ModeFile";
            this.ModeFile.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.ModeFile.Size = new System.Drawing.Size(97, 27);
            this.ModeFile.TabIndex = 6;
            this.ModeFile.Text = "ImageFile";
            this.ModeFile.UnCheckedColor = System.Drawing.Color.Black;
            this.ModeFile.UseVisualStyleBackColor = false;
            // 
            // ModeFolder
            // 
            this.ModeFolder.AutoSize = true;
            this.ModeFolder.BackColor = System.Drawing.Color.White;
            this.ModeFolder.CheckedColor = System.Drawing.Color.RoyalBlue;
            this.ModeFolder.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ModeFolder.Location = new System.Drawing.Point(12, 81);
            this.ModeFolder.Margin = new System.Windows.Forms.Padding(4);
            this.ModeFolder.MinimumSize = new System.Drawing.Size(0, 27);
            this.ModeFolder.Name = "ModeFolder";
            this.ModeFolder.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.ModeFolder.Size = new System.Drawing.Size(115, 27);
            this.ModeFolder.TabIndex = 5;
            this.ModeFolder.Text = "ImageFolder";
            this.ModeFolder.UnCheckedColor = System.Drawing.Color.Black;
            this.ModeFolder.UseVisualStyleBackColor = false;
            // 
            // ModeCam
            // 
            this.ModeCam.AutoSize = true;
            this.ModeCam.BackColor = System.Drawing.Color.White;
            this.ModeCam.CheckedColor = System.Drawing.Color.RoyalBlue;
            this.ModeCam.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.ModeCam.Location = new System.Drawing.Point(12, 35);
            this.ModeCam.Margin = new System.Windows.Forms.Padding(4);
            this.ModeCam.MinimumSize = new System.Drawing.Size(0, 27);
            this.ModeCam.Name = "ModeCam";
            this.ModeCam.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.ModeCam.Size = new System.Drawing.Size(86, 27);
            this.ModeCam.TabIndex = 4;
            this.ModeCam.Text = "Camera";
            this.ModeCam.UnCheckedColor = System.Drawing.Color.Black;
            this.ModeCam.UseVisualStyleBackColor = false;
            // 
            // label29
            // 
            this.label29.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label29.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label29.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label29.Location = new System.Drawing.Point(12, 11);
            this.label29.Margin = new System.Windows.Forms.Padding(4);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(191, 39);
            this.label29.TabIndex = 18;
            this.label29.Text = "IP Addess";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IPAddress
            // 
            this.IPAddress.Dock = System.Windows.Forms.DockStyle.Left;
            this.IPAddress.Location = new System.Drawing.Point(12, 11);
            this.IPAddress.Margin = new System.Windows.Forms.Padding(4);
            this.IPAddress.Name = "IPAddress";
            this.IPAddress.Size = new System.Drawing.Size(272, 25);
            this.IPAddress.TabIndex = 19;
            this.IPAddress.Text = "192.168.1.10";
            this.IPAddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Use
            // 
            this.Use.AutoSize = true;
            this.Use.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Use.Location = new System.Drawing.Point(15, 10);
            this.Use.Margin = new System.Windows.Forms.Padding(4);
            this.Use.Name = "Use";
            this.Use.Size = new System.Drawing.Size(51, 23);
            this.Use.TabIndex = 20;
            this.Use.Text = "Use";
            this.Use.UseVisualStyleBackColor = true;
            // 
            // SaveGraphic
            // 
            this.SaveGraphic.AutoSize = true;
            this.SaveGraphic.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SaveGraphic.Location = new System.Drawing.Point(109, 10);
            this.SaveGraphic.Margin = new System.Windows.Forms.Padding(4);
            this.SaveGraphic.Name = "SaveGraphic";
            this.SaveGraphic.Size = new System.Drawing.Size(107, 23);
            this.SaveGraphic.TabIndex = 21;
            this.SaveGraphic.Text = "Save Graphic";
            this.SaveGraphic.UseVisualStyleBackColor = true;
            // 
            // SaveOrigin
            // 
            this.SaveOrigin.AutoSize = true;
            this.SaveOrigin.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SaveOrigin.Location = new System.Drawing.Point(321, 10);
            this.SaveOrigin.Margin = new System.Windows.Forms.Padding(4);
            this.SaveOrigin.Name = "SaveOrigin";
            this.SaveOrigin.Size = new System.Drawing.Size(98, 23);
            this.SaveOrigin.TabIndex = 22;
            this.SaveOrigin.Text = "Save Origin";
            this.SaveOrigin.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Location = new System.Drawing.Point(2, 2);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(849, 61);
            this.panel3.TabIndex = 0;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.IPAddress);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(215, 0);
            this.panel6.Margin = new System.Windows.Forms.Padding(2);
            this.panel6.Name = "panel6";
            this.panel6.Padding = new System.Windows.Forms.Padding(12, 11, 12, 11);
            this.panel6.Size = new System.Drawing.Size(634, 61);
            this.panel6.TabIndex = 20;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label29);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(12, 11, 12, 11);
            this.panel1.Size = new System.Drawing.Size(215, 61);
            this.panel1.TabIndex = 19;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.panel3);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Controls.Add(this.panel4);
            this.flowLayoutPanel1.Controls.Add(this.panel5);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(875, 427);
            this.flowLayoutPanel1.TabIndex = 27;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.SaveOriginSeparate);
            this.panel2.Controls.Add(this.SaveOrigin);
            this.panel2.Controls.Add(this.Use);
            this.panel2.Controls.Add(this.SaveGraphic);
            this.panel2.Location = new System.Drawing.Point(2, 67);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(849, 53);
            this.panel2.TabIndex = 1;
            // 
            // SaveOriginSeparate
            // 
            this.SaveOriginSeparate.AutoSize = true;
            this.SaveOriginSeparate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SaveOriginSeparate.Location = new System.Drawing.Point(492, 10);
            this.SaveOriginSeparate.Margin = new System.Windows.Forms.Padding(4);
            this.SaveOriginSeparate.Name = "SaveOriginSeparate";
            this.SaveOriginSeparate.Size = new System.Drawing.Size(155, 23);
            this.SaveOriginSeparate.TabIndex = 23;
            this.SaveOriginSeparate.Text = "Save Origin Separate";
            this.SaveOriginSeparate.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.groupBox1);
            this.panel4.Location = new System.Drawing.Point(2, 124);
            this.panel4.Margin = new System.Windows.Forms.Padding(2);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(849, 172);
            this.panel4.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.groupBox6);
            this.panel5.Location = new System.Drawing.Point(2, 300);
            this.panel5.Margin = new System.Windows.Forms.Padding(2);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(849, 113);
            this.panel5.TabIndex = 3;
            // 
            // CameraSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.857143F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CameraSetting";
            this.Size = new System.Drawing.Size(927, 451);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label29;
        public SLTSoft.RJControl.RJRadio TriggerAuto;
        public SLTSoft.RJControl.RJRadio TriggerManual;
        public System.Windows.Forms.TextBox FileDirector;
        public System.Windows.Forms.TextBox FolderDirector;
        public SLTSoft.RJControl.RJRadio ModeFile;
        public SLTSoft.RJControl.RJRadio ModeFolder;
        public SLTSoft.RJControl.RJRadio ModeCam;
        public System.Windows.Forms.TextBox IPAddress;
        public System.Windows.Forms.CheckBox SaveGraphic;
        public System.Windows.Forms.CheckBox SaveOrigin;
        public System.Windows.Forms.CheckBox Use;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel6;
        public System.Windows.Forms.Button RefreshFolderImage;
        public System.Windows.Forms.Button RefreshFileImage;
        public System.Windows.Forms.Button btnConnectCam;
        public System.Windows.Forms.CheckBox SaveOriginSeparate;
    }
}
