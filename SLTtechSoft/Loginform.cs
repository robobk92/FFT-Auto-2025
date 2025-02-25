using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ByYou;

namespace SLTtechSoft
{
    public partial class Loginform : Form
    {
        ApiMes apiMes;
        string URL = "";
        public Loginform()
        {
            InitializeComponent();
            InitialMES();
        }
        public void InitialMES ()
        {
            URL = txtURL.Text;
            apiMes = new ApiMes(URL);
        }
        public async void getempoy ()
        {
            string result = await apiMes.employeeAuthApi(txtUser.Text,txtPassWord.Text);
            txtResult.Text = result;
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            getempoy();
            if (txtResult.Text=="PASS")
            {
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
            }
        }
    }
}
