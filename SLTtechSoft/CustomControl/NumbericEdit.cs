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
    public partial class NumbericEdit : TextBox
    {
        public int DeviceLeftOfDec { get; set; }
        public int DeviceRightOfDec { get; set; }
        public double DeviceHigh { get; set; }
        public double DeviceLow { get; set; }
        public enum Format
        { 
            Unsigned16,
            Unsigned32,
            Signed16,
            Signed32
        }
        public Format DeviveFormat { get; set; }
        public NumbericEdit()
        {
            InitializeComponent();

            DeviceLeftOfDec = 5;
            DeviceRightOfDec = 2;
            DeviceHigh = 999999;
            DeviceLow = 0;
            DeviveFormat = Format.Unsigned16;
            switch (DeviceRightOfDec)
            {

                case 0:
                    {
                        this.Text = "0";
                        break;
                    }
                case 1:
                    {
                        this.Text = "0.0";
                        break;
                    }
                case 2:
                    {
                        this.Text = "0.00";
                        break;
                    }
                case 3:
                    {
                        this.Text = "0.000";
                        break;
                    }
                default:
                    {
                        this.Text = "0";
                        break;
                    }

            }
          
           
            this.TextAlign = HorizontalAlignment.Right;
            this.ReadOnly = true;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            
        }
    }
}
