using Cognex.VisionPro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SLTtechSoft
{
    public partial class TopControl : UserControl
    {
        public Dictionary<string, CogDisplayNumpad> VisionDisplay = new Dictionary<string, CogDisplayNumpad>();
        //private Color BackColorCogDisplay = Color.FromArgb(255, 255, 223);
        private Color BackColorCogDisplay = Color.Black;
        public class NumPadName
        {
            public string No0 = "No0";
            public string No1 = "No1";
            public string No2 = "No2";
            public string No3 = "No3";
            public string No4 = "No4";
            public string No5 = "No5";
            public string No6 = "No6";
            public string No7 = "No7";
            public string No8 = "No8";
            public string No9 = "No9";
            public string Asterisk = "Asterisk";
            public string Sharp = "Sharp";
            public string Logo = "Logo";
            public string Card = "Card";
        }
        private NumPadName numPadName = new NumPadName();
        public TopControl()
        {
            InitializeComponent();
            InitialUI();


        }
        private void InitialUI()
        {
            
            for (int i = 0; i < 10; i++)
            {
                string thisName = $"No{i}";
                CogDisplayNumpad cogDisplayNumpad = new CogDisplayNumpad();
                VisionDisplay.Add(thisName, cogDisplayNumpad);
                tlp_VisionNumPad.Controls.Add(VisionDisplay[thisName]);
            }

            CogDisplayNumpad Asterisk = new CogDisplayNumpad();
            Asterisk.Dock = DockStyle.Fill;
            VisionDisplay.Add(numPadName.Asterisk, Asterisk);
            tlp_VisionNumPad.Controls.Add(VisionDisplay[numPadName.Asterisk]);

            CogDisplayNumpad Sharp = new CogDisplayNumpad();
            Sharp.Dock = DockStyle.Fill;
            VisionDisplay.Add(numPadName.Sharp, Sharp);
            tlp_VisionNumPad.Controls.Add(VisionDisplay[numPadName.Sharp]);

            CogDisplayNumpad Logo = new CogDisplayNumpad();
            Logo.Dock = DockStyle.Fill;
            VisionDisplay.Add(numPadName.Logo, Logo);
            tlp_VisionLogo.Controls.Add(VisionDisplay[numPadName.Logo]);

            CogDisplayNumpad Card = new CogDisplayNumpad();
            Card.Dock = DockStyle.Fill;
            VisionDisplay.Add(numPadName.Card, Card);
            tlp_VisionLogo.Controls.Add(VisionDisplay[numPadName.Card]);
        }
      

       
    }
}
