using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SLTtechSoft
{
    public partial class ToggleSwitch : Button
    {
        public enum type
        { 
            SetOn,
            SetOff,
            Toggle,
            Momentary
        }
        public type TypeSwitch { get; set; }
        public bool State { get; set; }
        public bool ReadSameRelay { get; set; }
        public ToggleSwitch()
        {
            InitializeComponent();
            TypeSwitch = type.Toggle;
            State = false;
            ReadSameRelay = true;
        }
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            switch (TypeSwitch)
            { 
                case type.SetOn:
                    {
                        State = true;
                        break;
                    }
                case type.SetOff:
                    {
                        State = false;
                        break;
                    }
                case type.Toggle:
                    {
                        State = !State;
                        break;
                    }
                case type.Momentary:
                    {
                        State = true;
                        break;
                    }
            }
            this.BackColor = State ? Color.Lime : Color.WhiteSmoke;
            
            base.OnMouseDown(mevent);
        }
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (TypeSwitch == type.Momentary)
            {
                State = false;
            }
            this.BackColor = State ? Color.Lime : Color.WhiteSmoke;
            base.OnMouseUp(mevent);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            if (TypeSwitch == type.Momentary)
            {
                State = false;
            }
            this.BackColor = State ? Color.Lime : SystemColors.Control;
            base.OnMouseLeave(e);
        }
        public void SetState(bool Value)
        {
            State = Value;
            this.BackColor = State ? Color.Lime : Color.WhiteSmoke;
        }
      
    }
}
