using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SLTtechSoft
{
    public partial class ModelTable : UserControl
    {
        public class ModelColumns
        {
            public int No = 0;
            public int Name = 1;
            public int File = 2;
            
        }
        public ModelColumns Col = new ModelColumns();
        public ModelTable()
        {
            InitializeComponent();
        }

        private void Table_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            RefreshValueTable();
        }
        public void RefreshValueTable()
        {
            for (int i = 0; i < Table.RowCount - 1; i++)
            {
                Table.Rows[i].Height = 25;
                Table.Rows[i].Cells[Col.No].Value = i.ToString();
            }
        }

        private void Table_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Table.CurrentCell.ColumnIndex == Col.File)
            {
                System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
                TextBox ThisTextBox = sender as TextBox;
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string fileName;
                    fileName = dlg.FileName;
                    Table.Rows[Table.CurrentCell.RowIndex].Cells[Col.File].Value = fileName;


                }
            }
        }

        private void Call_Click(object sender, EventArgs e)
        {

        }
    }
}
