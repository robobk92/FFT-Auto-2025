using FunctionLoock.Usercontrol;
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
    public partial class FormIO : Form
    {
        Form1 _form1;
        public FunctionLock functionLock;

        Color On = Color.Lime;
        Color Off = Color.Red;

        public FormIO()
        {
            InitializeComponent();
            functionLock = new FunctionLock();
            functionLock.Dock = DockStyle.Fill;
            this.Controls.Add(functionLock);
            
        }

        public void InitializeUI(Form1 obj)
        {
            _form1 = obj;
            functionLock.InitialUI(_form1);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //MAIN PLC
            

        }

     
    }
}
