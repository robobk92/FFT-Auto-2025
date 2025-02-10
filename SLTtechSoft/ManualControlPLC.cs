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
    public partial class ManualControlPLC : UserControl
    {
        public ManualControlPLC()
        {
            InitializeComponent();
        }
        private bool IsAutoUp = false;
        private void cbAutoUp_CheckedChanged(object sender, EventArgs e)
        {
            IsAutoUp = !IsAutoUp;
            if (IsAutoUp)
            {
                CylinderFinger.TypeSwitch = ToggleSwitch.type.Momentary;
                CylinderKeyAsterisk.TypeSwitch = ToggleSwitch.type.Momentary;
                CylinderKeySharp.TypeSwitch = ToggleSwitch.type.Momentary;
                CylinderKey0.TypeSwitch = ToggleSwitch.type.Momentary;
                CylinderKey1.TypeSwitch = ToggleSwitch.type.Momentary;
                CylinderKey2.TypeSwitch = ToggleSwitch.type.Momentary;
                CylinderKey3.TypeSwitch = ToggleSwitch.type.Momentary;
                CylinderKey4.TypeSwitch = ToggleSwitch.type.Momentary;
                CylinderKey5.TypeSwitch = ToggleSwitch.type.Momentary;
                CylinderKey6.TypeSwitch = ToggleSwitch.type.Momentary;
                CylinderKey7.TypeSwitch = ToggleSwitch.type.Momentary;
                CylinderKey8.TypeSwitch = ToggleSwitch.type.Momentary;
                CylinderKey9.TypeSwitch = ToggleSwitch.type.Momentary;

            }
            else
            {
                CylinderFinger.TypeSwitch = ToggleSwitch.type.Toggle;
                CylinderKeyAsterisk.TypeSwitch = ToggleSwitch.type.Toggle;
                CylinderKeySharp.TypeSwitch = ToggleSwitch.type.Toggle;
                CylinderKey0.TypeSwitch = ToggleSwitch.type.Toggle;
                CylinderKey1.TypeSwitch = ToggleSwitch.type.Toggle;
                CylinderKey2.TypeSwitch = ToggleSwitch.type.Toggle;
                CylinderKey3.TypeSwitch = ToggleSwitch.type.Toggle;
                CylinderKey4.TypeSwitch = ToggleSwitch.type.Toggle;
                CylinderKey5.TypeSwitch = ToggleSwitch.type.Toggle;
                CylinderKey6.TypeSwitch = ToggleSwitch.type.Toggle;
                CylinderKey7.TypeSwitch = ToggleSwitch.type.Toggle;
                CylinderKey8.TypeSwitch = ToggleSwitch.type.Toggle;
                CylinderKey9.TypeSwitch = ToggleSwitch.type.Toggle;

            }
        }
    }
}
