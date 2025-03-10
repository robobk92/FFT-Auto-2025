﻿using Cognex.VisionPro.Exceptions;
using LibFunction;
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
    public partial class PLCControl : UserControl
    {
        Form1 _form1;
        Model _model;
        public ManualControlPLC ManualControlPLC;
        public PLCControl()
        {
            InitializeComponent();
            ManualControlPLC  = new ManualControlPLC();
            ManualControlPLC.Dock = DockStyle.Fill;
            tabPageManualControl.Controls.Add(ManualControlPLC);

        }
        public void InitialUI(Form1 obj_Form1, Model obj_Model)
        {

            this._form1 = obj_Form1;
            this._model = obj_Model;
        }

        private void btnOpenProgram_Click(object sender, EventArgs e)
        {
            _model.ReadPLCProgramFromFile(tbMotionFile.Texts);
        }

        private void btnSaveProgam_Click(object sender, EventArgs e)
        {
            _model.modelParameter.MotionFile = tbMotionFile.Texts;
            _model.SaveAModel();
        }

        private void btnChangeFileMotion_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
                dlg.CheckFileExists = true;
                dlg.CheckPathExists = true;
                dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                dlg.FilterIndex = 0;
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string fileName;
                    fileName = dlg.FileName;
                    tbMotionFile.Texts = fileName;
                    _model.ReadPLCProgramFromFile(tbMotionFile.Texts);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load Model File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SetPosPointPLC()
        { 
        
        
        }

        private void btnSavePos1_Click(object sender, EventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                string ButtonName = button.Name;
                int ButtonNo = Convert.ToInt32(ButtonName.Replace("btnSavePos", ""));
                switch (ButtonNo)
                {
                    case 1:
                        {
                            _model.modelParameter.pLCModelPara.nudPoint1 = Convert.ToInt32(nudPoint1.Value);
                            break;
                        }
                    case 2:
                        {
                            short sigervalue = (short)nudPoint2.Value;
                            ushort value = (ushort)sigervalue;
                            _model.modelParameter.pLCModelPara.nudPoint2 = Convert.ToInt32(value);
                            break;
                        }
                    case 3:
                        {
                            _model.modelParameter.pLCModelPara.nudPoint3 = Convert.ToInt32(nudPoint3.Value);
                            break;
                        }
                    case 4:
                        {
                            _model.modelParameter.pLCModelPara.nudPoint4 = Convert.ToInt32(nudPoint4.Value);
                            break;
                        }
                    case 5:
                        {
                            _model.modelParameter.pLCModelPara.nudPoint5 = Convert.ToInt32(nudPoint5.Value);
                            break;
                        }
                    case 6:
                        {
                            _model.modelParameter.pLCModelPara.nudPoint6 = Convert.ToInt32(nudPoint6.Value);
                            break;
                        }
                    default:break;
                }
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.Message);
            }
           
            
        }
        private void btnGotoPos1_Click(object sender, EventArgs e)
        {
            
            
        }

        private void btnRgetPos1_Click(object sender, EventArgs e)
        {
            try
            {
                Button button = (Button)sender;
                string ButtonName = button.Name;
                int ButtonNo = Convert.ToInt32(ButtonName.Replace("btnRgetPos", ""));
                switch (ButtonNo)
                {
                    case 1:
                        {
                            _model.modelParameter.pLCModelPara.nudRPos1 = Convert.ToInt32(nudRPos1.Value);
                            break;
                        }
                    case 2:
                        {
                            _model.modelParameter.pLCModelPara.nudRPos2 = Convert.ToInt32(nudRPos3.Value);
                            break;
                        }
                    case 3:
                        {
                            _model.modelParameter.pLCModelPara.nudRPos3 = Convert.ToInt32(nudRPos3.Value);
                            break;
                        }
                    
                    default: break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        private void AlarmPlc ()
        {
            List<string> errorMessages = new List<string>();
            if(_form1.PLC.Read.Alarm.AlarmALL)
            {
                if (_form1.PLC.Read.Alarm.Alarm_Cyl_Door_X14_not_on)
                    errorMessages.Add("Alarm_Cyl_Door_X14_not_on");
                if (_form1.PLC.Read.Alarm.Alarm_Cyl_Door_X15_not_on)
                    errorMessages.Add("Alarm_Cyl_Door_X15_not_on");
                if (_form1.PLC.Read.Alarm.Alarm_Cyl_Up_Tool_X16_not_on)
                    errorMessages.Add("Alarm_Cyl_Up_Tool_X16_not_on");
                if (_form1.PLC.Read.Alarm.Alarm_Cyl_DW_Tool_X17_not_on)
                    errorMessages.Add("Alarm_Cyl_DW_Tool_X17_not_on");
                if (_form1.PLC.Read.Alarm.Alarm_Cyl_FW_Tool_X23_not_on)
                    errorMessages.Add("Alarm_Cyl_FW_Tool_X23_not_on");
                if (_form1.PLC.Read.Alarm.Alarm_Cyl_BW_Tool_X24_not_on)
                    errorMessages.Add("Alarm_Cyl_BW_Tool_X24_not_on");
                if (_form1.PLC.Read.Alarm.Alarm_CylCheckCard_Dw_X25_not_on)
                    errorMessages.Add("Alarm_CylCheckCard_Dw_X25_not_on");
                if (_form1.PLC.Read.Alarm.Alarm_Cyl_CheckStuck_FW_X26_not_on)
                    errorMessages.Add("Alarm_Cyl_CheckStuck_FW_X26_not_on");
                if (_form1.PLC.Read.Alarm.Alarm_Cyl_PushLock_Dw_X30_not_off)
                    errorMessages.Add("Alarm_Cyl_PushLock_Dw_X30_not_off");
                if (_form1.PLC.Read.Alarm.Alarm_Cyl_PushLock_Dw_X30_not_on)
                    errorMessages.Add("Alarm_Cyl_PushLock_Dw_X30_not_on");
                if (_form1.PLC.Read.Alarm.Alarm_CylSpring_Open_X31_not_on)
                    errorMessages.Add("Alarm_CylSpring_Open_X31_not_on");
                if (_form1.PLC.Read.Alarm.Alarm_CylSpring_Close_X32_not_on)
                    errorMessages.Add("Alarm_CylSpring_Close_X32_not_on");
                if (_form1.PLC.Read.Alarm.Alarm_CylSpring_Up_X33_not_on)
                    errorMessages.Add("Alarm_CylSpring_Up_X33_not_on");
                if (_form1.PLC.Read.Alarm.Alarm_CylSpring_Dw_X34_not_on)
                    errorMessages.Add("Alarm_CylSpring_Dw_X34_not_on");

            }
            if (errorMessages.Count > 0)
                lbStepName.Text = "StepName: " + string.Join(", ", errorMessages);
            else
                lbStepName.Text = "StepName: ";
            //if (_form1.PLC.Read.Alarm.Alarm_Cyl_Door_X14_not_on) lbStepName.Text = "StepName:Alarm_Cyl_Door_X14_not_on";
            //else if (_form1.PLC.Read.Alarm.Alarm_Cyl_Door_X15_not_on) lbStepName.Text = "StepName:Alarm_Cyl_Door_X15_not_on";
            //else if (_form1.PLC.Read.Alarm.Alarm_Cyl_Up_Tool_X16_not_on) lbStepName.Text = "StepName:Alarm_Cyl_Up_Tool_X16_not_on";
            //else if (_form1.PLC.Read.Alarm.Alarm_Cyl_DW_Tool_X17_not_on) lbStepName.Text = "StepName:Alarm_Cyl_DW_Tool_X17_not_on";
            //else if (_form1.PLC.Read.Alarm.Alarm_Cyl_FW_Tool_X23_not_on) lbStepName.Text = "StepName:Alarm_Cyl_FW_Tool_X23_not_on";
            //else if (_form1.PLC.Read.Alarm.Alarm_Cyl_BW_Tool_X24_not_on) lbStepName.Text = "StepName:Alarm_Cyl_BW_Tool_X24_not_on";
            //else if (_form1.PLC.Read.Alarm.Alarm_CylCheckCard_Dw_X25_not_on) lbStepName.Text = "StepName:Alarm_CylCheckCard_Dw_X25_not_on";
            //else if (_form1.PLC.Read.Alarm.Alarm_Cyl_CheckStuck_FW_X26_not_on) lbStepName.Text = "StepName:Alarm_Cyl_CheckStuck_FW_X26_not_on";
            //else if (_form1.PLC.Read.Alarm.Alarm_Cyl_PushLock_Dw_X30_not_off) lbStepName.Text = "StepName:Alarm_Cyl_PushLock_Dw_X30_not_off";
            //else if (_form1.PLC.Read.Alarm.Alarm_Cyl_PushLock_Dw_X30_not_on) lbStepName.Text = "StepName:Alarm_Cyl_PushLock_Dw_X30_not_on";
            //else if (_form1.PLC.Read.Alarm.Alarm_CylSpring_Open_X31_not_on) lbStepName.Text = "StepName:Alarm_CylSpring_Open_X31_not_on";
            //else if (_form1.PLC.Read.Alarm.Alarm_CylSpring_Close_X32_not_on) lbStepName.Text = "StepName:Alarm_CylSpring_Close_X32_not_on";
            //else if (_form1.PLC.Read.Alarm.Alarm_CylSpring_Up_X33_not_on) lbStepName.Text = "StepName:Alarm_CylSpring_Up_X33_not_on";
            //else if (_form1.PLC.Read.Alarm.Alarm_CylSpring_Dw_X34_not_on) lbStepName.Text = "StepName:Alarm_CylSpring_Dw_X34_not_on";
            //else lbStepName.Text = "StepName:";
        }
        private void btnWriteJogParameter_Click(object sender, EventArgs e)
        {
            _model.currentModelInfomation.pLCInternalPara.nudStepSizeX = Convert.ToInt32(nudStepSizeX.Value);
            _model.currentModelInfomation.pLCInternalPara.nudFeedRateX = Convert.ToInt32(nudFeedRateX.Value);
            _model.currentModelInfomation.pLCInternalPara.nudStepSizeR = Convert.ToInt32(nudStepSizeR.Value);
            _model.currentModelInfomation.pLCInternalPara.nudFeedRateR = Convert.ToInt32(nudFeedRateR.Value);
        }

        private void btnWriteSpeedXAuto_Click(object sender, EventArgs e)
        {
            _model.modelParameter.pLCModelPara.nudSpeed = Convert.ToInt32(nudSpeed.Value);
        }

        private void btnSaveRPos_Click(object sender, EventArgs e)
        {
            _model.modelParameter.pLCModelPara.nudRSpeed = Convert.ToInt32(nudSpeed.Value);
        }

        private void btnHomeDataWrite_Click(object sender, EventArgs e)
        {
            _model.currentModelInfomation.pLCInternalPara.tbOPRCreepSpeedX = Convert.ToInt32(tbOPRCreepSpeedX.Text);
            _model.currentModelInfomation.pLCInternalPara.tbOprSpeedX = Convert.ToInt32(tbOprSpeedX.Text);
            _model.currentModelInfomation.pLCInternalPara.tbStartAddressX = Convert.ToInt32(tbStartAddressX.Text);
            _model.currentModelInfomation.pLCInternalPara.tbOprStartSpeedX = Convert.ToInt32(tbOprStartSpeedX.Text);
            _model.currentModelInfomation.pLCInternalPara.nudRCreepSpeed = Convert.ToInt32(nudRCreepSpeed.Value);
            _model.currentModelInfomation.pLCInternalPara.nudROprSpeed = Convert.ToInt32(nudROprSpeed.Value);
            _model.currentModelInfomation.pLCInternalPara.nudRStartAddress = Convert.ToInt32(nudRStartAddress.Value);
            _model.currentModelInfomation.pLCInternalPara.nudRStartSpeed = Convert.ToInt32(nudRStartSpeed.Value);
            _model.currentModelInfomation.pLCInternalPara.nudACC_X = Convert.ToInt32(nudACC_X.Text);
            _model.currentModelInfomation.pLCInternalPara.nudDEC_X = Convert.ToInt32(nudDEC_X.Text);
        }
      
        private Color On = Color.Lime;
        private Color Off = Color.Transparent;
        private void Timer1_Tick(object sender, EventArgs e)
        {

            //if (_form1.PLC.Read.Auto.Stop)
            //{
            //    _form1.PLC.Write.Auto.Auto = false;
            //    _form1.PLC.Write.Auto.Stop = false;
            //    _form1.PLC.Write.Auto.ResetAll = true;
            //}
            //else
            //{

            //    if (!_form1.PLC.Read.Auto.HomeAll)
            //    {
            //        _form1.PLC.Write.Auto.ResetAll = false;
            //        _form1.PLC.Write.Auto.HomeAll = true;

            //    }
            //    else
            //    {
            //        _form1.PLC.Write.Auto.HomeAll = false;
            //        _form1.PLC.Write.Auto.Auto = true;
            //    }
            //}
            //if (_form1.MachineMode != Form1.ModeMachine.Manual) return;



            AlarmPlc();
            //Input
            ManualControlPLC.LimitNegAxis1.BackColor = _form1.PLC.Read.Input.LimitNegAxis1 ? On : Off;
            ManualControlPLC.HomeAxis1.BackColor = _form1.PLC.Read.Input.HomeAxis1 ? On : Off;
            ManualControlPLC.LimitPosAxis1.BackColor = _form1.PLC.Read.Input.LimitPosAxis1 ? On : Off;

            ManualControlPLC.OP_Start1.BackColor = _form1.PLC.Read.Input.OP_Start1 ? On : Off;
            ManualControlPLC.OP_Start2.BackColor = _form1.PLC.Read.Input.OP_Start2 ? On : Off;
            ManualControlPLC.OP_EMG.BackColor = _form1.PLC.Read.Input.OP_EMG ? On : Off;
            ManualControlPLC.DoorUp.BackColor = _form1.PLC.Read.Input.DoorUp ? On : Off;
            ManualControlPLC.DoorDw.BackColor = _form1.PLC.Read.Input.DoorDw ? On : Off;
            ManualControlPLC.ToolsUp.BackColor = _form1.PLC.Read.Input.ToolsUp ? On : Off;
            ManualControlPLC.ToolsDw.BackColor = _form1.PLC.Read.Input.ToolsDw ? On : Off;
            ManualControlPLC.LockUp.BackColor = _form1.PLC.Read.Input.LockUp ? On : Off;
            ManualControlPLC.LockDw.BackColor = _form1.PLC.Read.Input.LockDw ? On : Off;
            ManualControlPLC.CheckSpringOpen.BackColor = _form1.PLC.Read.Input.CheckSpringOpen ? On : Off;
            ManualControlPLC.CheckSpringClose.BackColor = _form1.PLC.Read.Input.CheckSpringClose ? On : Off;

            ManualControlPLC.CheckSpring1.BackColor = _form1.PLC.Read.Input.CheckSpring1 ? On : Off;
            ManualControlPLC.CheckSpring2.BackColor = _form1.PLC.Read.Input.CheckSpring2 ? On : Off;
            ManualControlPLC.CheckSpring3.BackColor = _form1.PLC.Read.Input.CheckSpring3 ? On : Off;
            ManualControlPLC.TransToolFW.BackColor = _form1.PLC.Read.Input.TransToolFW ? On : Off;
            ManualControlPLC.TransToolBW.BackColor = _form1.PLC.Read.Input.TransToolBW ? On : Off;
            ManualControlPLC.Push1Dw.BackColor = _form1.PLC.Read.Input.Push1Dw ? On : Off;
            ManualControlPLC.Push2FW.BackColor = _form1.PLC.Read.Input.Push2FW ? On : Off;
            ManualControlPLC.Check1.BackColor = _form1.PLC.Read.Input.Check1 ? On : Off;
            ManualControlPLC.Check2.BackColor = _form1.PLC.Read.Input.Check2 ? On : Off;
            ManualControlPLC.Check3.BackColor = _form1.PLC.Read.Input.Check3 ? On : Off;
            ManualControlPLC.SafetySensor.BackColor = _form1.PLC.Read.Input.SafetySensor ? On : Off;
            ManualControlPLC.CylDownSpring.BackColor = _form1.PLC.Read.Output.CylinderDownSpring ? On : Off;
            ManualControlPLC.CylinderCheckSpring.BackColor = _form1.PLC.Read.Output.CylinderCheckSpring ? On : Off;
            ManualControlPLC.CylinderDoorClose.BackColor = _form1.PLC.Read.Output.CylinderDoorClose? On : Off;
            ManualControlPLC.CylinderToolsDown.BackColor = _form1.PLC.Read.Output.CylinderToolsDown ? On : Off;
            ManualControlPLC.CylinderTransTools.BackColor = _form1.PLC.Read.Output.CylinderTransTools? On : Off;
            ManualControlPLC.CylinderPush1.BackColor = _form1.PLC.Read.Output.CylinderPush1 ? On : Off;
            ManualControlPLC.CylinderPushLock.BackColor = _form1.PLC.Read.Output.CylinderPushLock? On : Off;
            ManualControlPLC.CylinderPush2.BackColor = _form1.PLC.Read.Output.CylinderPush2 ? On : Off;
            ManualControlPLC.CylinderTools3.BackColor = _form1.PLC.Read.Output.CylinderTools3 ? On : Off;
            ManualControlPLC.CylinderTools4.BackColor  = _form1.PLC.Read.Output.CylinderTools4 ? On : Off;
            ManualControlPLC.CylinderTools2.BackColor = _form1.PLC.Read.Output.CylinderTools2 ? On : Off;
            ManualControlPLC.CylinderTools1.BackColor = _form1.PLC.Read.Output.CylinderTools1 ? On : Off;
            ManualControlPLC.Reverse9V.BackColor = _form1.PLC.Read.Output.Reverse9V ? On : Off;
            ManualControlPLC.Change9Vor6V.BackColor = _form1.PLC.Read.Output.Change9Vor6V ? On : Off;
            ManualControlPLC.Supply6VToBattery.BackColor = _form1.PLC.Read.Output.Supply6VToBattery ? On : Off;
            ManualControlPLC.Supply9VToFront.BackColor = _form1.PLC.Read.Output.Supply9VToFront ? On : Off;
            ManualControlPLC.Broken.BackColor = _form1.PLC.Read.Output.Broken ? On : Off;
            ManualControlPLC.AutoCmd.BackColor = _form1.PLC.Read.Auto.Auto ? On : Off;
            //Auto
            _form1.PLC.Write.Auto.Auto = ManualControlPLC.AutoCmd.State;
            _form1.PLC.Write.Auto.Stop = Stop.State;
            _form1.PLC.Write.Auto.ResetAll = ManualControlPLC.Reset.State;
            _form1.PLC.Write.Auto.Test.FinishProcessTest = ManualControlPLC.TestFinish.State;
            _form1.PLC.Write.Auto.MotionReady = ManualControlPLC.MotionReady.State;
            _form1.PLC.Write.Auto.StepXNeg = toggleSwitch1.State;
            _form1.PLC.Write.Auto.StepXPos =  btnJogXup.State;
            _form1.PLC.Write.Auto.HomeX = btnHomeX.State;
            _form1.PLC.Write.Auto.HomeAll = btnHomeAll.State;
            _form1.PLC.Write.Auto.Step_Mode = btnJogMode.State;
            _form1.PLC.Write.Auto.PC_Simulation_Mode =  Simulation.State;
            _form1.PLC.Write.Auto.MovePosX1 = btnGotoPos1.State;
            _form1.PLC.Write.Auto.MovePosX2 = btnGotoPos2.State;
            _form1.PLC.Write.Auto.MovePosX3 = btnGotoPos3.State;
            _form1.PLC.Write.Auto.MovePosX4 = btnGotoPos4.State;
            _form1.PLC.Write.Auto.MovePosX5 = btnGotoPos5.State;
            _form1.PLC.Write.Auto.MovePosX6 = btnGotoPos6.State;
            


            //Word 
            _form1.PLC.Write.Word.nudStepSizeX = Convert.ToInt32(nudStepSizeX.Value);
            _form1.PLC.Write.Word.nudFeedRateX = Convert.ToInt32(nudFeedRateX.Value);
            _form1.PLC.Write.Word.tbOPRCreepSpeedX = Convert.ToInt32(_model.currentModelInfomation.pLCInternalPara.tbOPRCreepSpeedX);
            _form1.PLC.Write.Word.tbOprSpeedX = Convert.ToInt32(_model.currentModelInfomation.pLCInternalPara.tbOprSpeedX);
            _form1.PLC.Write.Word.tbStartAddressX = Convert.ToInt32(_model.currentModelInfomation.pLCInternalPara.tbStartAddressX);
            _form1.PLC.Write.Word.tbOprStartSpeedX = Convert.ToInt32(_model.currentModelInfomation.pLCInternalPara.tbOprStartSpeedX);
            _form1.PLC.Write.Word.nudACC_X = Convert.ToInt32(_model.currentModelInfomation.pLCInternalPara.nudACC_X);
            _form1.PLC.Write.Word.nudDEC_X = Convert.ToInt32(_model.currentModelInfomation.pLCInternalPara.nudDEC_X);
            _form1.PLC.Write.Word.SpeedXAuto = _model.modelParameter.pLCModelPara.nudSpeed;
            _form1.PLC.Write.Word.PosCamera = _model.modelParameter.pLCModelPara.nudPoint1;
            _form1.PLC.Write.Word.PosInput = _model.modelParameter.pLCModelPara.nudPoint2;
            _form1.PLC.Write.Word.PosGoOriginal = _model.modelParameter.pLCModelPara.nudPoint3;
            _form1.PLC.Write.Word.PosDoor = _model.modelParameter.pLCModelPara.nudPoint4;
            _form1.PLC.Write.Word.Pos5 = _model.modelParameter.pLCModelPara.nudPoint5;
            _form1.PLC.Write.Word.Pos6 = _model.modelParameter.pLCModelPara.nudPoint6;

            //outputnudStepSizeX
            _form1.PLC.Write.Output.CylinderDoorClose = ManualControlPLC.CylinderDoorClose.State;
            _form1.PLC.Write.Output.CylinderToolsDown = ManualControlPLC.CylinderToolsDown.State;
            _form1.PLC.Write.Output.CylinderPushLock = ManualControlPLC.CylinderPushLock.State;
            _form1.PLC.Write.Output.CylinderCheckSpring = ManualControlPLC.CylinderCheckSpring.State;
            _form1.PLC.Write.Output.CylinderDownSpring = ManualControlPLC.CylDownSpring.State;
            _form1.PLC.Write.Output.CylinderTools1 = ManualControlPLC.CylinderTools1.State;
            _form1.PLC.Write.Output.CylinderTools2 = ManualControlPLC.CylinderTools2.State;
            _form1.PLC.Write.Output.CylinderTransTools = ManualControlPLC.CylinderTransTools.State;
            _form1.PLC.Write.Output.CylinderPush1 = ManualControlPLC.CylinderPush1.State;
            _form1.PLC.Write.Output.CylinderPush2 = ManualControlPLC.CylinderPush2.State;
            _form1.PLC.Write.Output.CylinderTools3 = ManualControlPLC.CylinderTools3.State;
            _form1.PLC.Write.Output.CylinderTools4 = ManualControlPLC.CylinderTools4.State;

            _form1.PLC.Write.Output.CylinderFinger = ManualControlPLC.CylinderFinger.State;
            _form1.PLC.Write.Output.CylinderKeyAsterisk = ManualControlPLC.CylinderKeyAsterisk.State;
            _form1.PLC.Write.Output.CylinderKeySharp = ManualControlPLC.CylinderKeySharp.State;
            _form1.PLC.Write.Output.CylinderKey0 = ManualControlPLC.CylinderKey0.State;
            _form1.PLC.Write.Output.CylinderKey1 = ManualControlPLC.CylinderKey1.State;
            _form1.PLC.Write.Output.CylinderKey2 = ManualControlPLC.CylinderKey2.State;
            _form1.PLC.Write.Output.CylinderKey3 = ManualControlPLC.CylinderKey3.State;
            _form1.PLC.Write.Output.CylinderKey4 = ManualControlPLC.CylinderKey4.State;
            _form1.PLC.Write.Output.CylinderKey5 = ManualControlPLC.CylinderKey5.State;
            _form1.PLC.Write.Output.CylinderKey6 = ManualControlPLC.CylinderKey6.State;
            _form1.PLC.Write.Output.CylinderKey7 = ManualControlPLC.CylinderKey7.State;
            _form1.PLC.Write.Output.CylinderKey8 = ManualControlPLC.CylinderKey8.State;
            _form1.PLC.Write.Output.CylinderKey9 = ManualControlPLC.CylinderKey9.State;
            _form1.PLC.Write.Output.Reverse9V = ManualControlPLC.Reverse9V.State;
            _form1.PLC.Write.Output.Supply6VToBattery = ManualControlPLC.Supply6VToBattery.State;
            _form1.PLC.Write.Output.Supply9VToFront = ManualControlPLC.Supply9VToFront.State;
            _form1.PLC.Write.Output.Change9Vor6V = ManualControlPLC.Change9Vor6V.State;
            _form1.PLC.Write.Output.Broken = ManualControlPLC.Broken.State;
            //read word 
            tbXPosition.Text = _form1.PLC.Read.Word.CoordinateX.ToString();
            //read output
            lbTowerLightGreen.BackColor = _form1.PLC.Read.Output.TL_Green ? On : Off;
            lbTowerLightRed.BackColor = _form1.PLC.Read.Output.TL_Red ? On : Off;
            lbTowerLightOrange.BackColor = _form1.PLC.Read.Output.TL_Yellow ? On : Off;
            lbTowerLightOrange .BackColor = _form1.PLC.Read.Output.TL_Buzzer ? On : Off;
        }

        private void toggleSwitch3_Click(object sender, EventArgs e)
        {
            _form1.PLC.Write.Auto.StepXNeg = false;
            _form1.PLC.Write.Auto.StepXPos = false;

        }

       
    }
}
