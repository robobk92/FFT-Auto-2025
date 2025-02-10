using Cognex.VisionPro;
using SLTtechSoft.Properties;
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
   
    public partial class PartOfCam : UserControl
    {
        
        public List<Label> labelResults = new List<Label>();
        public PartOfCam()
        {
            InitializeComponent();
            labelResults.Add(lbResult1);
            labelResults.Add(lbResult2);
            labelResults.Add(lbResult3);
            labelResults.Add(lbResult4);
            labelResults.Add(lbResult5);
            labelResults.Add(lbResult6);
            labelResults.Add(lbResult7);
            labelResults.Add(lbResult8);
            for (int i = 0; i < labelResults.Count; i++)
            {
                labelResults[i].Text = "";
            }
        }
    }
}
