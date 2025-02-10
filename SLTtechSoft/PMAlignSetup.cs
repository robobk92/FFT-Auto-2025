using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.PatInspect;
using Cognex.VisionPro.PMAlign;
using Cognex.VisionPro.Exceptions;
using Cognex.VisionPro.Display;
using System.Runtime.InteropServices;
using SLTtechSoft.Properties;

namespace SLTtechSoft
{
    public partial class PMAlignSetup : Form
    {
        //private Cognex.VisionPro.PMAlign.CogPMAlignEditV2 CogPMAlignEdit1;
        private Cognex.VisionPro.CogRecordDisplay CogDisplay1;
        private Cognex.VisionPro.CogRecordDisplay CogDisplay_TrainImage;
        CogPMAlignTool PatMaxTool;
        CogImageFileTool ImageFileTool;
        CogAcqFifoTool AcqFifoTool;
        Model _model;
        private CogImage8Grey InputImage = new CogImage8Grey();

        class PMAlignRunParars
        { 
            public string TrainImageFile { get; set; }
        
        }



        public PMAlignSetup()
        {
            InitializeComponent();
        }

        public void InitialUI(Model formModel)
        {
            _model = formModel;
            PatMaxTool = new CogPMAlignTool();
            PatMaxTool.Changed += PatMaxTool_Changed;
            CogDisplay1 = new CogRecordDisplay();
            panel1.Controls.Add(CogDisplay1);
            
            CogDisplay1.Dock = DockStyle.Fill;
        }

        private void cmdImageAcquisitionLiveOrOpenCommand_Click(System.Object sender, System.EventArgs e)
        {
            CogDisplay1.InteractiveGraphics.Clear();
            CogDisplay1.StaticGraphics.Clear();
            InputImage = _model.GetImage();
            CogDisplay1.Image = InputImage;
            PatMaxTool.InputImage = InputImage;
            PatMaxTool.Run();
            if ((PatMaxTool.RunStatus.Exception != null))
            {
                MessageBox.Show(PatMaxTool.RunStatus.Exception.Message, "PatMax Run Error");
            }

        }

        private void PMAlignSetup_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }
        bool SettingUp;
        private void cmdPatMaxSetupCommand_Click(object sender, EventArgs e)
        {
            //PatMax Setup button has been pressed, Entering SettingUp mode.
            if (!SettingUp)
            {
                //Copy InputImage to TrainImage, If no ImputImage then display an
                //error message
                if (PatMaxTool.InputImage == null)
                {
                    MessageBox.Show("No InputImage available for setup.", "PatMax Setup Error");
                    return;
                }
                PatMaxTool.Pattern.TrainImage = PatMaxTool.InputImage;
                //While setting up PMAlign, disable other GUI controls.
                SettingUp = true;
                DisableAll();
                //Add TrainRegion to display's interactive graphics
                //Add SearchRegion to display's static graphics for display only.
                CogDisplay1.InteractiveGraphics.Clear();
                CogDisplay1.StaticGraphics.Clear();
                
                CogDisplay1.InteractiveGraphics.Add(PatMaxTool.Pattern.TrainRegion as ICogGraphicInteractive, "test", false);
               
                if ((PatMaxTool.SearchRegion != null))
                {
                    CogDisplay1.StaticGraphics.Add(PatMaxTool.SearchRegion as ICogGraphic, "test");
                }

                //OK has been pressed, completing Setup.
            }
            else
            {
                SettingUp = false;
                CogDisplay1.InteractiveGraphics.Clear();
                CogDisplay1.StaticGraphics.Clear();
                //Make sure we catch errors from Train, since they are likely.  For example,
                //No InputImage, No Pattern Features, etc.
                PatMatTrain();
                EnableAll();
            }
        }
        private void DisableAll()
        {
            //Disable all of the frames (Disables controls within frame)
            cmdImageAcquisitionLiveOrOpenCommand.Enabled = false;
            cmdPatMaxSetupCommand.Text = "OK";
        }
        private void EnableAll()
        {
            //Disable all of the frames (Disables controls within frame)
            cmdImageAcquisitionLiveOrOpenCommand.Enabled = true;
            cmdPatMaxSetupCommand.Text = "Setup\r\nTrain Image";
        }
        //If PMAlign results have changed then update the Score & Region graphic.
        //Handles PatMaxTool.Changed
        private void PatMaxTool_Changed(object sender, Cognex.VisionPro.CogChangedEventArgs e)
        {
            try
            {
                //If FunctionalArea And cogFA_Tool_Results Then
                if ((Cognex.VisionPro.Implementation.CogToolBase.SfCreateLastRunRecord
               | Cognex.VisionPro.Implementation.CogToolBase.SfRunStatus) > 0)
                {
                    CogDisplay1.StaticGraphics.Clear();
                    //Note, Results will be nothing if Run failed.
                    if (PatMaxTool.Results == null)
                    {
                        txtPatMaxScoreValue.Text = "N/A";
                    }
                    else if (PatMaxTool.Results.Count > 0)
                    {
                        //Passing result does not imply Pattern is found, must check count.
                        txtPatMaxScoreValue.Text = PatMaxTool.Results[0].Score.ToString("g3");
                        txtPatMaxScoreValue.Refresh();
                        CogCompositeShape resultGraphics = default(CogCompositeShape);
                        resultGraphics = PatMaxTool.Results[0].CreateResultGraphics(CogPMAlignResultGraphicConstants.MatchRegion);
                        CogDisplay1.InteractiveGraphics.Add(resultGraphics, "test", false);
                    }
                    else
                    {
                        txtPatMaxScoreValue.Text = "N/A";
                    }
                }
            }
            catch (CogException cogex)
            {
                MessageBox.Show("Following Specific Cognex Error Occured:" + cogex.Message);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PatMax Run Error");
            }

        }

        private void PatMatTrain()
        {
            try
            {
                PatMaxTool.Pattern.Train();
                txbTrainOriginX.Text = PatMaxTool.Pattern.Origin.TranslationX.ToString();
                txbTrainOriginY.Text = PatMaxTool.Pattern.Origin.TranslationY.ToString();
               
            }
            catch (CogException cogex)
            {
                MessageBox.Show("Following Specific Cognex Error Occured:" + cogex.Message);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "PatMax Setup Error");
            }
        }


        private void LoadPatMatParams()
        {
            if (PatMaxTool == null) return;
          
        }
    }
}
