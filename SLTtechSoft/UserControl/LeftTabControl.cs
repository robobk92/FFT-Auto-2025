using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibFunction;
namespace SLTtechSoft
{
    public partial class LeftTabControl : UserControl
    {
        public LabelStatus LabelStatus = new LabelStatus();
        public LeftTabControl()
        {
            InitializeComponent();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {

        }
        public void SetlStatus(string status)
        {
            lblStatus.Text = status;
            if (status == LabelStatus.Wait)
            {
                lblStatus.BackColor = Color.FromArgb(192, 255, 255);
            }
            else
            {
                if (status == LabelStatus.Pass)
                {
                    lblStatus.BackColor = Color.Green;
                }
                else
                {
                    lblStatus.BackColor = Color.Red;
                }
            }


        }
    }
}
