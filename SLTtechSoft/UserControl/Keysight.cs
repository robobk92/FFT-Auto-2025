using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ByYou
{
    public partial class Keysight : UserControl
    {
        public Keysight()
        {
            InitializeComponent();
        }
        public KeysightPSU KeysightPSU;
        
        public bool isConnect = false;
        
        public void btnConnect_Click(object sender, EventArgs e)
        {
            if(btnConnect.BackColor!=Color.Green)
            {
                KeysightPSU = new KeysightPSU();
                if (KeysightPSU.ScanAndOpen().Item2)
                {
                    btnConnect.BackColor = Color.Green;
                    btnOffPower_Click(null, null);
                    isConnect = true;
                }
            }
            else
            {
                if(KeysightPSU.Stop())
                {
                    btnConnect.BackColor = SystemColors.Control;
                    isConnect = false;
                }    
            }
        }
        

        public void btnSetVol_Click(object sender, EventArgs e)
        {
            if (btnConnect.BackColor != Color.Green) return;
            double vol = -1.00;
            if(double.TryParse(txtSetVol.Text,out vol))
            {
                if(KeysightPSU.SetVoltage(vol))
                {
                    //MessageBox.Show("Gửi thành công");
                }
            }
            
            Vol_value.Text = KeysightPSU.GetVoltage();
            Current_value.Text = KeysightPSU.GetCurrent();

           
        }

        public void btnSetCurrent_Click(object sender, EventArgs e)
        {
            if (btnConnect.BackColor != Color.Green) return;
            double cur = -1.00;
            if (double.TryParse(txtSetCurent.Text, out cur))
                KeysightPSU.SetCurrent(cur);
            Vol_value.Text = KeysightPSU.GetVoltage();
            Current_value.Text = KeysightPSU.GetCurrent();
        }

        public void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch 
            {

              
            }
            
        }

        public void btnOnPower_Click(object sender, EventArgs e)
        {
            KeysightPSU.ON();
            btnOnPower.BackColor = Color.Green;
            btnOffPower.BackColor = SystemColors.Control;
        }
        
        public void btnOffPower_Click(object sender, EventArgs e)
        {
            if (btnConnect.BackColor != Color.Green) return;
            KeysightPSU.OFF();
            btnOnPower.BackColor = SystemColors.Control;
            btnOffPower.BackColor = Color.Green;
        }

        public void btnDisplay_Click(object sender, EventArgs e)
        {
            if (btnConnect.BackColor != Color.Green) return;
            Vol_value.Text = KeysightPSU.GetVoltage();
            Current_value.Text = KeysightPSU.GetCurrent();
        }
        public double Read_Current()

        {
            try
            {
                string currentString = KeysightPSU.GetCurrent();
                double current = double.Parse(currentString);
                return current;

            }
            catch (Exception)

            {
                return 0;
            }
        }


        public double Read_Voltage()

        {
            try
            {
                string voltageString = KeysightPSU.GetVoltage();
                double voltage = double.Parse(voltageString);
                return voltage;
            }

            catch (Exception)

            {
                return 0;
            }
        }
    }
}
