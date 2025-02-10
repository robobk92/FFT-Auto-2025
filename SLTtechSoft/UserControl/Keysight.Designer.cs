
namespace ByYou
{
    partial class Keysight
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
            this.Vol_value = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Current_value = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSetVol = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSetCurent = new System.Windows.Forms.TextBox();
            this.btnSetVol = new System.Windows.Forms.Button();
            this.btnSetCurrent = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnOffPower = new System.Windows.Forms.Button();
            this.btnOnPower = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDisplay = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Vol_value
            // 
            this.Vol_value.Location = new System.Drawing.Point(106, 54);
            this.Vol_value.Name = "Vol_value";
            this.Vol_value.ReadOnly = true;
            this.Vol_value.Size = new System.Drawing.Size(66, 24);
            this.Vol_value.TabIndex = 0;
            this.Vol_value.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Voltage";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "Current";
            // 
            // Current_value
            // 
            this.Current_value.Location = new System.Drawing.Point(107, 79);
            this.Current_value.Name = "Current_value";
            this.Current_value.ReadOnly = true;
            this.Current_value.Size = new System.Drawing.Size(65, 24);
            this.Current_value.TabIndex = 3;
            this.Current_value.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 18);
            this.label4.TabIndex = 6;
            this.label4.Text = "Set voltage";
            // 
            // txtSetVol
            // 
            this.txtSetVol.Location = new System.Drawing.Point(107, 104);
            this.txtSetVol.Name = "txtSetVol";
            this.txtSetVol.Size = new System.Drawing.Size(65, 24);
            this.txtSetVol.TabIndex = 5;
            this.txtSetVol.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 138);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 18);
            this.label5.TabIndex = 8;
            this.label5.Text = "Set current";
            // 
            // txtSetCurent
            // 
            this.txtSetCurent.Location = new System.Drawing.Point(107, 132);
            this.txtSetCurent.Name = "txtSetCurent";
            this.txtSetCurent.Size = new System.Drawing.Size(65, 24);
            this.txtSetCurent.TabIndex = 7;
            this.txtSetCurent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnSetVol
            // 
            this.btnSetVol.Location = new System.Drawing.Point(178, 105);
            this.btnSetVol.Name = "btnSetVol";
            this.btnSetVol.Size = new System.Drawing.Size(73, 24);
            this.btnSetVol.TabIndex = 9;
            this.btnSetVol.Text = "Set";
            this.btnSetVol.UseVisualStyleBackColor = true;
            this.btnSetVol.Click += new System.EventHandler(this.btnSetVol_Click);
            // 
            // btnSetCurrent
            // 
            this.btnSetCurrent.Location = new System.Drawing.Point(178, 131);
            this.btnSetCurrent.Name = "btnSetCurrent";
            this.btnSetCurrent.Size = new System.Drawing.Size(73, 24);
            this.btnSetCurrent.TabIndex = 10;
            this.btnSetCurrent.Text = "Set";
            this.btnSetCurrent.UseVisualStyleBackColor = true;
            this.btnSetCurrent.Click += new System.EventHandler(this.btnSetCurrent_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(9, 20);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(101, 28);
            this.btnConnect.TabIndex = 11;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnOffPower
            // 
            this.btnOffPower.Location = new System.Drawing.Point(197, 20);
            this.btnOffPower.Name = "btnOffPower";
            this.btnOffPower.Size = new System.Drawing.Size(54, 28);
            this.btnOffPower.TabIndex = 13;
            this.btnOffPower.Text = "OFF";
            this.btnOffPower.UseVisualStyleBackColor = true;
            this.btnOffPower.Click += new System.EventHandler(this.btnOffPower_Click);
            // 
            // btnOnPower
            // 
            this.btnOnPower.Location = new System.Drawing.Point(116, 20);
            this.btnOnPower.Name = "btnOnPower";
            this.btnOnPower.Size = new System.Drawing.Size(54, 28);
            this.btnOnPower.TabIndex = 12;
            this.btnOnPower.Text = "ON";
            this.btnOnPower.UseVisualStyleBackColor = true;
            this.btnOnPower.Click += new System.EventHandler(this.btnOnPower_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDisplay);
            this.groupBox1.Controls.Add(this.btnOffPower);
            this.groupBox1.Controls.Add(this.Vol_value);
            this.groupBox1.Controls.Add(this.btnOnPower);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnConnect);
            this.groupBox1.Controls.Add(this.Current_value);
            this.groupBox1.Controls.Add(this.btnSetCurrent);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnSetVol);
            this.groupBox1.Controls.Add(this.txtSetVol);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtSetCurent);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(270, 167);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Keysight";
            // 
            // btnDisplay
            // 
            this.btnDisplay.Location = new System.Drawing.Point(178, 51);
            this.btnDisplay.Name = "btnDisplay";
            this.btnDisplay.Size = new System.Drawing.Size(73, 54);
            this.btnDisplay.TabIndex = 14;
            this.btnDisplay.Text = "Display";
            this.btnDisplay.UseVisualStyleBackColor = true;
            this.btnDisplay.Click += new System.EventHandler(this.btnDisplay_Click);
            // 
            // Keysight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Keysight";
            this.Size = new System.Drawing.Size(270, 167);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSetVol;
        private System.Windows.Forms.Button btnSetCurrent;
        public System.Windows.Forms.TextBox Vol_value;
        public System.Windows.Forms.TextBox Current_value;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDisplay;
        public System.Windows.Forms.TextBox txtSetVol;
        public System.Windows.Forms.TextBox txtSetCurent;
        public System.Windows.Forms.Button btnConnect;
        public System.Windows.Forms.Button btnOffPower;
        public System.Windows.Forms.Button btnOnPower;
    }
}
