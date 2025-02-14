namespace SLTtechSoft
{
    partial class ModelSetting
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dgvFFT = new System.Windows.Forms.DataGridView();
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLoadFileListTest = new System.Windows.Forms.Button();
            this.cbTypeTest = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbModeTest = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbArea = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.cbProduct = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbIndex = new System.Windows.Forms.ComboBox();
            this.cbLine = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbStationTest = new System.Windows.Forms.ComboBox();
            this.modelTable = new SLTtechSoft.ModelTable();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFFT)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1358, 570);
            this.panel1.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dgvFFT);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(254, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1104, 570);
            this.panel4.TabIndex = 21;
            // 
            // dgvFFT
            // 
            this.dgvFFT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFFT.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
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
            this.dgvFFT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFFT.Location = new System.Drawing.Point(0, 0);
            this.dgvFFT.Name = "dgvFFT";
            this.dgvFFT.Size = new System.Drawing.Size(1104, 570);
            this.dgvFFT.TabIndex = 18;
            // 
            // Column_ID
            // 
            this.Column_ID.FillWeight = 32.69474F;
            this.Column_ID.HeaderText = "ID";
            this.Column_ID.Name = "Column_ID";
            // 
            // Column_Name
            // 
            this.Column_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_Name.FillWeight = 32.69474F;
            this.Column_Name.HeaderText = "Name";
            this.Column_Name.Name = "Column_Name";
            // 
            // Column_Detail
            // 
            this.Column_Detail.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column_Detail.FillWeight = 32.69474F;
            this.Column_Detail.HeaderText = "Detail";
            this.Column_Detail.Name = "Column_Detail";
            // 
            // Column_Value
            // 
            this.Column_Value.FillWeight = 32.69474F;
            this.Column_Value.HeaderText = "Value";
            this.Column_Value.Name = "Column_Value";
            this.Column_Value.Width = 90;
            // 
            // Column_Min
            // 
            this.Column_Min.FillWeight = 32.69474F;
            this.Column_Min.HeaderText = "Min";
            this.Column_Min.Name = "Column_Min";
            this.Column_Min.Width = 50;
            // 
            // Column_Max
            // 
            this.Column_Max.FillWeight = 32.69474F;
            this.Column_Max.HeaderText = "Max";
            this.Column_Max.Name = "Column_Max";
            this.Column_Max.Width = 50;
            // 
            // Column_Unit
            // 
            this.Column_Unit.FillWeight = 32.69474F;
            this.Column_Unit.HeaderText = "Unit";
            this.Column_Unit.Name = "Column_Unit";
            this.Column_Unit.Width = 50;
            // 
            // Column_Timer
            // 
            this.Column_Timer.FillWeight = 144.5108F;
            this.Column_Timer.HeaderText = "Time(s)";
            this.Column_Timer.Name = "Column_Timer";
            this.Column_Timer.Width = 50;
            // 
            // Column_Result
            // 
            this.Column_Result.FillWeight = 111.1621F;
            this.Column_Result.HeaderText = "Result";
            this.Column_Result.Name = "Column_Result";
            this.Column_Result.Width = 50;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Enable";
            this.Column2.Name = "Column2";
            this.Column2.Width = 50;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Retry";
            this.Column1.Name = "Column1";
            this.Column1.Width = 50;
            // 
            // Column_Timeout
            // 
            this.Column_Timeout.FillWeight = 515.4639F;
            this.Column_Timeout.HeaderText = "Timeout(ms)";
            this.Column_Timeout.Name = "Column_Timeout";
            this.Column_Timeout.Width = 50;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.modelTable);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(254, 570);
            this.panel3.TabIndex = 20;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Location = new System.Drawing.Point(3, 150);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(243, 199);
            this.panel2.TabIndex = 19;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLoadFileListTest);
            this.groupBox1.Controls.Add(this.cbTypeTest);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cbModeTest);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cbArea);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.cbProduct);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbIndex);
            this.groupBox1.Controls.Add(this.cbLine);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbStationTest);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(243, 199);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Model infomation";
            // 
            // btnLoadFileListTest
            // 
            this.btnLoadFileListTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadFileListTest.Location = new System.Drawing.Point(186, 22);
            this.btnLoadFileListTest.Name = "btnLoadFileListTest";
            this.btnLoadFileListTest.Size = new System.Drawing.Size(51, 64);
            this.btnLoadFileListTest.TabIndex = 20;
            this.btnLoadFileListTest.Text = "Load List";
            this.btnLoadFileListTest.UseVisualStyleBackColor = true;
            this.btnLoadFileListTest.Click += new System.EventHandler(this.btnLoadFileListTest_Click);
            // 
            // cbTypeTest
            // 
            this.cbTypeTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTypeTest.FormattingEnabled = true;
            this.cbTypeTest.Items.AddRange(new object[] {
            "Full",
            "Front Only",
            "Main Only\t"});
            this.cbTypeTest.Location = new System.Drawing.Point(84, 164);
            this.cbTypeTest.Name = "cbTypeTest";
            this.cbTypeTest.Size = new System.Drawing.Size(96, 21);
            this.cbTypeTest.TabIndex = 22;
            this.cbTypeTest.Text = "Full";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(10, 167);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Mục tiêu Test";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbModeTest
            // 
            this.cbModeTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbModeTest.FormattingEnabled = true;
            this.cbModeTest.Items.AddRange(new object[] {
            "Auto",
            "Manual"});
            this.cbModeTest.Location = new System.Drawing.Point(84, 141);
            this.cbModeTest.Name = "cbModeTest";
            this.cbModeTest.Size = new System.Drawing.Size(96, 21);
            this.cbModeTest.TabIndex = 20;
            this.cbModeTest.Text = "Auto";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(10, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Chế độ test";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbArea
            // 
            this.cbArea.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbArea.FormattingEnabled = true;
            this.cbArea.Items.AddRange(new object[] {
            "AASTV",
            "AANZ",
            "TOPVINA",
            "UNIONVINA",
            "DONGKWANG"});
            this.cbArea.Location = new System.Drawing.Point(84, 118);
            this.cbArea.Name = "cbArea";
            this.cbArea.Size = new System.Drawing.Size(96, 21);
            this.cbArea.TabIndex = 18;
            this.cbArea.Text = "AASTV";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(10, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Khu Vực";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label15.Location = new System.Drawing.Point(10, 26);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(44, 13);
            this.label15.TabIndex = 8;
            this.label15.Text = "Product";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbProduct
            // 
            this.cbProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbProduct.FormattingEnabled = true;
            this.cbProduct.Items.AddRange(new object[] {
            "By You Pro",
            "By You Pro1",
            "By You Pro2",
            "By You Pro3",
            "By You Pro4",
            "Prisma",
            "Prisma S"});
            this.cbProduct.Location = new System.Drawing.Point(84, 23);
            this.cbProduct.Name = "cbProduct";
            this.cbProduct.Size = new System.Drawing.Size(96, 21);
            this.cbProduct.TabIndex = 10;
            this.cbProduct.Text = "By You Pro";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(10, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Line";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbIndex
            // 
            this.cbIndex.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbIndex.FormattingEnabled = true;
            this.cbIndex.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cbIndex.Location = new System.Drawing.Point(84, 94);
            this.cbIndex.Name = "cbIndex";
            this.cbIndex.Size = new System.Drawing.Size(96, 21);
            this.cbIndex.TabIndex = 16;
            this.cbIndex.Text = "1";
            // 
            // cbLine
            // 
            this.cbLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbLine.FormattingEnabled = true;
            this.cbLine.Items.AddRange(new object[] {
            "L1",
            "L2",
            "L3",
            "L4",
            "L5",
            "L6",
            "L7"});
            this.cbLine.Location = new System.Drawing.Point(84, 46);
            this.cbLine.Name = "cbLine";
            this.cbLine.Size = new System.Drawing.Size(96, 21);
            this.cbLine.TabIndex = 12;
            this.cbLine.Text = "L2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(10, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Index";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(10, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Station Test";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbStationTest
            // 
            this.cbStationTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbStationTest.FormattingEnabled = true;
            this.cbStationTest.Items.AddRange(new object[] {
            "FFT1",
            "FFT2",
            "FFT3",
            "FFT4"});
            this.cbStationTest.Location = new System.Drawing.Point(84, 70);
            this.cbStationTest.Name = "cbStationTest";
            this.cbStationTest.Size = new System.Drawing.Size(96, 21);
            this.cbStationTest.TabIndex = 14;
            this.cbStationTest.Text = "FFT1";
            // 
            // modelTable
            // 
            this.modelTable.Font = new System.Drawing.Font("Segoe UI", 8.142858F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modelTable.Location = new System.Drawing.Point(3, 3);
            this.modelTable.Name = "modelTable";
            this.modelTable.Size = new System.Drawing.Size(243, 141);
            this.modelTable.TabIndex = 0;
            // 
            // ModelSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "ModelSetting";
            this.Size = new System.Drawing.Size(1358, 570);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFFT)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel4;
        public ModelTable modelTable;
        public System.Windows.Forms.DataGridView dgvFFT;
        public System.Windows.Forms.ComboBox cbProduct;
        public System.Windows.Forms.ComboBox cbLine;
        public System.Windows.Forms.ComboBox cbStationTest;
        public System.Windows.Forms.ComboBox cbIndex;
        public System.Windows.Forms.ComboBox cbArea;
        public System.Windows.Forms.ComboBox cbModeTest;
        public System.Windows.Forms.ComboBox cbTypeTest;
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
        private System.Windows.Forms.Button btnLoadFileListTest;
    }
}
