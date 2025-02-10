using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLTtechSoft
{
    public partial class ModelSetting : UserControl
    {
        public ModelSetting()
        {
            InitializeComponent();
        }

        private void btnLoadFileListTest_Click(object sender, EventArgs e)
        {
            try
            {
                dgvFFT.Rows.Clear();
                System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
                dlg.CheckFileExists = true;
                dlg.CheckPathExists = true;
                dlg.Filter = "csv files (*.csv)|txt files(*.txt)|All files (*.*)|*.*";
                dlg.FilterIndex = 0;
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string fileName;
                    fileName = dlg.FileName;
                    if (!File.Exists(fileName)) return;
                    string[] ReadText = File.ReadAllLines(fileName);
                    for (int i = 1; i < ReadText.Length; i++)
                    {
                        string[] thisLine = ReadText[i].Split(',');
                        dgvFFT.Rows.Add(thisLine);
                    }
                    
                }
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load Model File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
