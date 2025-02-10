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



namespace SLTtechSoft
{
    public partial class FilmStrip : UserControl
    {
        public PictureBox pictureBox = new PictureBox();
        public FilmStrip()
        {
            InitializeComponent();
            pictureBox = new PictureBox();
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            panel3.Controls.Add(pictureBox);
        }
    }
}
