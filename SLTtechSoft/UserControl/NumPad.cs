using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SLTtechSoft.Form1;

namespace SLTtechSoft
{
    public partial class NumPad : Form
    {
        Model _formModel;
        Form1 _formLog;
      
        private NumbericEdit _NumbericEdit;
        private string LastValue = "";
        public string CurText;
        private bool firstTimeEdit = true;
        public NumPad()
        {
            InitializeComponent();
            this.KeyPreview = true;
            firstTimeEdit = true;
            TopMost = true;
            this.StartPosition = FormStartPosition.Manual;

            Rectangle resolution = Screen.PrimaryScreen.Bounds;
            int PosX = Cursor.Position.X;
            int PosY = Cursor.Position.Y;
            if (PosX + this.Width > resolution.Width)
            {
                PosX = PosX - this.Width;
            }
            if (PosY + this.Height > resolution.Height)
            {
                PosY = PosY - this.Height;
            }

            this.SetDesktopLocation(PosX, PosY);
        }
        public void InitialUI(NumbericEdit obj, Model form, Form1 form1)
        {
            _NumbericEdit = obj;
            if (form != null)
            {
                _formModel = form;
                _formModel.Enabled = false;
            }
            if (form1 != null)
            _formLog = form1;
         

            this.Text = _NumbericEdit.Name;
            LastValue = _NumbericEdit.Text;
            if (obj.Text.Length == 0 )
            {
                switch (_NumbericEdit.DeviceRightOfDec)
                {
                    case 0:
                        {
                            lbResult.Text = "0";
                            break;
                        }
                    case 1:
                        {
                            lbResult.Text = "0.0";
                            break;
                        }
                    case 2:
                        {
                            lbResult.Text = "0.00";
                            break;
                        }
                    case 3:
                        {
                            lbResult.Text = "0.000";
                            break;
                        }
                }
            }
            else
            {
                lbResult.Text = obj.Text;
            }
            
        }
   

        private void btnESC_Click_1(object sender, EventArgs e)
        {
            if (_formModel != null)
            {
                _formModel.Enabled = true;
            }
            
            this.Close();
        }

        private void HandleNumberic(string key)
        {

            string lastValue = lbResult.Text;
            string NewValueString = lbResult.Text;
            double LastValue_double = 0;
            double NewValueDouble = LastValue_double;

            if (key == "PlusOne" || key == "SubOne") firstTimeEdit = false;
            if (firstTimeEdit)
            {
                lastValue = "0";
                NewValueString = "0";
            }
            firstTimeEdit = false;
            if ((lbResult.Text.Contains("-") && lbResult.Text.Length == 1) ||
                lbResult.Text == "0."
                )
            {

            }
            else
            {
                LastValue_double = Convert.ToDouble(lastValue);
                NewValueDouble = LastValue_double;

                if (key != "Dot" && LastValue_double == 0 && NewValueString != "-0." && NewValueString != "-0.0" && NewValueString != "0." && NewValueString != "0.0")
                {
                    NewValueString = "";
                }

            }

            switch (key)
            {
                case "Clr":
                    {
                        NewValueString = "0";
                        break;
                    }
                case "Bs":
                    {
                        if (NewValueString == "-") NewValueString = "0";
                        if (LastValue_double == 0) break;
                        if (NewValueString.Length == 1) NewValueString = "0";
                        else
                        {
                            NewValueString = NewValueString.Substring(0, NewValueString.Length - 1);
                        }
                        break;
                    }
                case "Del":
                    {
                        NewValueString = "0";
                        break;

                    }
                case "PlusOne":
                    {
                        if (_NumbericEdit.DeviceRightOfDec == 0)
                        {
                            NewValueDouble += 1;
                        }
                        else
                        {
                            NewValueDouble += 0.1;
                        }
                        NewValueString = NewValueDouble.ToString();
                        break;
                    }
                case "SubOne":
                    {
                        if (_NumbericEdit.DeviceRightOfDec == 0)
                        {
                            NewValueDouble -= 1;
                        }
                        else
                        {
                            NewValueDouble -= 0.1;
                        }
                        NewValueString = NewValueDouble.ToString();
                        break;
                    }

                case "Neg":
                    {
                        if (_NumbericEdit.DeviceLow < 0) NewValueString = "-";

                        break;
                    }
                case "Dot":
                    {
                        if (_NumbericEdit.DeviceRightOfDec > 0)
                        {
                            if (!NewValueString.Contains("."))
                                NewValueString += ".";
                        }
                        break;
                    }
                case "0":
                    {
                        if (NewValueDouble != 0 || NewValueString.Contains("."));
                        {
                            NewValueString += "0";
                        }
                        break;
                    }
                case "1":
                    {
                        NewValueString += "1";
                        break;
                    }
                case "2":
                    {
                        NewValueString += "2";
                        break;
                    }
                case "3":
                    {
                        NewValueString += "3";
                        break;
                    }
                case "4":
                    {
                        NewValueString += "4";
                        break;
                    }
                case "5":
                    {
                        NewValueString += "5";
                        break;
                    }
                case "6":
                    {
                        NewValueString += "6";
                        break;
                    }
                case "7":
                    {
                        NewValueString += "7";

                        break;
                    }
                case "8":
                    {
                        NewValueString += "8";
                        break;
                    }
                case "9":
                    {
                        NewValueString += "9";
                        break;
                    }
            }
            if (NewValueString == "-")
            {
                lbResult.Text = NewValueString;
                return;
            }
            if (NewValueString == "-.")
            {
                lbResult.Text = "-0.";
                return;
            }
            if (NewValueString == "0.0")
            {
                lbResult.Text = NewValueString;
                return;
            }
            if (NewValueString.Length == 0) NewValueString = "0";
            if (NewValueString == ".")
            {
                NewValueString = "0.";
                lbResult.Text = NewValueString;
                return;
            }
            if (NewValueString.Contains("."))
            {
                string[] Filter1 = NewValueString.Split('.');
                if (Filter1[1].Length > _NumbericEdit.DeviceRightOfDec)
                {
                    NewValueString = lastValue;
                }
                else
                {
                    NewValueDouble = Convert.ToDouble(NewValueString);
                    if (NewValueDouble > _NumbericEdit.DeviceHigh || NewValueDouble < _NumbericEdit.DeviceLow)
                    {
                        NewValueString = lastValue;
                    }

                }
            }
            else
            {
                NewValueDouble = Convert.ToDouble(NewValueString);
                if (NewValueDouble > _NumbericEdit.DeviceHigh || NewValueDouble < _NumbericEdit.DeviceLow)
                {
                    NewValueString = lastValue;
                }
            }
            lbResult.Text = NewValueString;

            btnEnter.Focus();
        }
        private void EnterNewValue()
        {
            string NewValueString = lbResult.Text;

            if (NewValueString == "-" || NewValueString == "0.")
            {
                NewValueString = "0";
            }
            if (_NumbericEdit.DeviceRightOfDec > 0)
            {
                if (NewValueString.Contains("."))
                {
                    string[] Filter1 = NewValueString.Split('.');
                    if (Filter1[1].Length == _NumbericEdit.DeviceRightOfDec)
                    {

                    }
                    else
                    {
                        int devi = _NumbericEdit.DeviceRightOfDec - Filter1[1].Length;
                        for (int i = 0; i < devi; i++)
                        {
                            NewValueString += "0";
                        }
                    }
                }
                else
                {
                    NewValueString += ".";
                    for (int i = 0; i < _NumbericEdit.DeviceRightOfDec; i++)
                    {
                        NewValueString += "0";
                    }

                }

            }

            _NumbericEdit.Text = NewValueString;
            if (_formModel != null && _formLog != null)
            {
                //LOG
                this.Invoke(new Action(() => {
                    _formLog.WriteLogPC(LogType.Main, "Setting", $"Model: {_formModel.modelParameter.ModelName}. Change Value {_NumbericEdit.Name} From {LastValue} To {NewValueString}");
                }));
              
            }
            this.Close();

        }

        private void btn_0_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            string btnName = button.Name.Split('_')[1];

            HandleNumberic(btnName);
        }

        private void btnEnter_Click(object sender, EventArgs e)
        {
            EnterNewValue();
        }

        private void NumPad_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                // Press 0
                case Keys.NumPad0:
                    {
                        HandleNumberic("0");
                        break;
                    }
                case Keys.D0:
                    {
                        HandleNumberic("0");
                        break;
                    }
                // Press 1
                case Keys.NumPad1:
                    {
                        HandleNumberic("1");
                        break;
                    }
                case Keys.D1:
                    {
                        HandleNumberic("1");
                        break;
                    }
                // Press 2
                case Keys.NumPad2:
                    {
                        HandleNumberic("2");
                        break;
                    }
                case Keys.D2:
                    {
                        HandleNumberic("2");
                        break;
                    }
                // Press 3
                case Keys.NumPad3:
                    {
                        HandleNumberic("3");
                        break;
                    }
                case Keys.D3:
                    {
                        HandleNumberic("3");
                        break;
                    }
                // Press 4
                case Keys.NumPad4:
                    {
                        HandleNumberic("4");
                        break;
                    }
                case Keys.D4:
                    {
                        HandleNumberic("4");
                        break;
                    }
                // Press 5
                case Keys.NumPad5:
                    {
                        HandleNumberic("5");
                        break;
                    }
                case Keys.D5:
                    {
                        HandleNumberic("5");
                        break;
                    }
                // Press 6
                case Keys.NumPad6:
                    {
                        HandleNumberic("6");
                        break;
                    }
                case Keys.D6:
                    {
                        HandleNumberic("6");
                        break;
                    }
                // Press 6
                case Keys.NumPad7:
                    {
                        HandleNumberic("7");
                        break;
                    }
                case Keys.D7:
                    {
                        HandleNumberic("7");
                        break;
                    }
                // Press 7
                case Keys.NumPad8:
                    {
                        HandleNumberic("8");
                        break;
                    }
                case Keys.D8:
                    {
                        HandleNumberic("8");
                        break;
                    }
                // Press 8
                case Keys.NumPad9:
                    {
                        HandleNumberic("9");
                        break;
                    }
                case Keys.D9:
                    {
                        HandleNumberic("9");
                        break;
                    }
                // Press 9
                case Keys.Escape:
                    {
                        this.Close();
                        break;
                    }
                case Keys.Enter:
                    {
                        EnterNewValue();
                        break;
                    }
                case Keys.Delete:
                    {
                        HandleNumberic("Del");
                        break;
                    }
                case Keys.Back:
                    {
                        HandleNumberic("Bs");
                        break;
                    }
                case Keys.Add:
                    {
                        HandleNumberic("PlusOne");
                        break;
                    }
                case Keys.Subtract:
                    {
                        HandleNumberic("SubOne");
                        break;
                    }
                case Keys.Decimal:
                    {
                        HandleNumberic("Dot");
                        break;
                    }
            }
        }

        private void NumPad_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_formModel != null)
            _formModel.Enabled = true;
        }
    }
}
