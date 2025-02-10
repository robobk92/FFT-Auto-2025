namespace SLTtechSoft
{
    partial class CogDisplayNumpad
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CogDisplayNumpad));
            this.mDisplay = new Cognex.VisionPro.CogRecordDisplay();
            ((System.ComponentModel.ISupportInitialize)(this.mDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // mDisplay
            // 
            this.mDisplay.ColorMapLowerClipColor = System.Drawing.Color.Black;
            this.mDisplay.ColorMapLowerRoiLimit = 0D;
            this.mDisplay.ColorMapPredefined = Cognex.VisionPro.Display.CogDisplayColorMapPredefinedConstants.None;
            this.mDisplay.ColorMapUpperClipColor = System.Drawing.Color.Black;
            this.mDisplay.ColorMapUpperRoiLimit = 1D;
            this.mDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mDisplay.DoubleTapZoomCycleLength = 2;
            this.mDisplay.DoubleTapZoomSensitivity = 2.5D;
            this.mDisplay.Location = new System.Drawing.Point(2, 2);
            this.mDisplay.Margin = new System.Windows.Forms.Padding(0);
            this.mDisplay.MouseWheelMode = Cognex.VisionPro.Display.CogDisplayMouseWheelModeConstants.Zoom1;
            this.mDisplay.MouseWheelSensitivity = 1D;
            this.mDisplay.Name = "mDisplay";
            this.mDisplay.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("mDisplay.OcxState")));
            this.mDisplay.Size = new System.Drawing.Size(146, 146);
            this.mDisplay.TabIndex = 6;
            // 
            // CogDisplayNumpad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Controls.Add(this.mDisplay);
            this.Name = "CogDisplayNumpad";
            this.Padding = new System.Windows.Forms.Padding(2);
            ((System.ComponentModel.ISupportInitialize)(this.mDisplay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public Cognex.VisionPro.CogRecordDisplay mDisplay;
    }
}
