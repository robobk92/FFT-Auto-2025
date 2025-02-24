using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cognex.VisionPro.Exceptions;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static SLTtechSoft.Form1;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.Implementation.Internal;
using Cognex.VisionPro.Implementation;
using Cognex.VisionPro.ImageProcessing;
using Cognex.VisionPro.Caliper;
using static System.Windows.Forms.AxHost;
using static SLTtechSoft.ModelTable;
using Io_JigTest_Motor;
using ByYou;
using System.Linq.Expressions;

namespace SLTtechSoft
{
    public partial class Model : Form
    {
        #region Data Declarations
        Form1 _form1;

        List<TextBox> ListVppFile = new List<TextBox>();
        public CurrentModelInfomation currentModelInfomation = new CurrentModelInfomation();
        public ModelParameter modelParameter = new ModelParameter();
        public CogToolBlockEditV2 cogToolBlockEditV21;
        public int IndexImageChoosing = 0;
        public CogRecordDisplay mLiveDisplay = new CogRecordDisplay();
        private int panel1FirstWidth = 0;
        public FftAnalysis FftAnalysis;
        public Keysight Keysight;
        public ModelSetting modelSetting = new ModelSetting();
        public PLCControl PLCControl;

        public int[] PLCProgramNo;
        public string[] PLCProgramName;
        #endregion


        public Model()
        {
            InitializeComponent();
            radioUseCam.Checked = true;
            panel1FirstWidth = panel1.Width;
        }

        /// <summary>
        /// Initial UI
        /// </summary>
        public void InitializeUI(Form1 obj)
        {
            _form1 = obj;
            InitialPLCControl();
            InitialModelUI();
            InitialVisionUI();
            InitialFFTUI();
            InitialKeysightUI();


            ReadModelInfomation();

            LoadAModel(currentModelInfomation.ModelNo);
        }
        private void InitialPLCControl()
        {

            PLCControl = new PLCControl();
            PLCControl.InitialUI(_form1, this);
            PLCControl.Dock = DockStyle.Fill;
            panel13.Controls.Add(PLCControl);
        }
        private void InitialModelUI()
        {
            modelSetting = new ModelSetting();
            modelSetting.Dock = DockStyle.Fill;
            tabPageModel.Controls.Add(modelSetting);
            //modelSetting.modelTable = new ModelTable();

            modelSetting.modelTable.Save.Click += ModelTableSave_Click;
            modelSetting.modelTable.Call.Click += ModelTalbeCall_Click;

        }
        private void InitialVisionUI()
        {

            cogToolBlockEditV21 = new CogToolBlockEditV2();
            cogToolBlockEditV21.AllowDrop = true;
            cogToolBlockEditV21.ContextMenuCustomizer = null;
            cogToolBlockEditV21.MinimumSize = new Size(489, 0);
            cogToolBlockEditV21.ShowNodeToolTips = true;
            cogToolBlockEditV21.SuspendElectricRuns = false;

            panel3.Controls.Add(cogToolBlockEditV21);
            cogToolBlockEditV21.Dock = DockStyle.Fill;

            mLiveDisplay = new CogRecordDisplay();
            mLiveDisplay.Location = new Point(0, 0);
            mLiveDisplay.Size = new Size(900, 700);
            mLiveDisplay.Dock = DockStyle.Fill;
            panelLiveCam.Controls.Add(mLiveDisplay);
            panelLiveCam.Width = 0;

        }
        private void InitialFFTUI()
        {
            FftAnalysis = new FftAnalysis();
            FftAnalysis.Dock = DockStyle.Fill;
            panelFFTAnalysis.Controls.Add(FftAnalysis);

        }
        private void InitialKeysightUI()
        {
            Keysight = new Keysight();
            Keysight.Dock = DockStyle.Fill;
            panelKeysight.Controls.Add(Keysight);
        }
        private void ModelTableSave_Click(object sender, EventArgs e)
        {
            SaveAModel();
            SaveCurrentModelInfomation(true);

        }

        private void ModelTalbeCall_Click(object sender, EventArgs e)
        {
            //if (!_form1.formOption.Parameters.Option.CallModelManual)
            //{
            //    MessageBox.Show("Please Use call Model Manual.","Call Model Fail");
            //    return;
            //}

            ModelColumns Col = new ModelColumns();
            int ModelNo = modelSetting.modelTable.Table.CurrentCell.RowIndex;
            if (ModelNo >= modelSetting.modelTable.Table.Rows.Count - 1) return;

            LoadAModel(ModelNo);
        }

        #region EventHandle
        private void numbericEdit1_Click_1(object sender, EventArgs e)
        {
            NumbericEdit ThisnumbericEdit = sender as NumbericEdit;

            NumPad numPad = new NumPad();
            numPad.InitialUI(ThisnumbericEdit, this, _form1);
            numPad.Show();
        }

        private void btnModel_New_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to make new Model? Current Parameters will be clear", "New Model", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                currentModelInfomation.PathFileModel = "";
                lbModelFile.Text = "Not set";
                modelParameter = new ModelParameter();

            }
            //LOG
            this.Invoke(new Action(() => {
                _form1.WriteLogPC(LogType.Main, "Model", "New Model is initial");
            }));
        }
        private void btnModel_Save_Click(object sender, EventArgs e)
        {
            SaveAModel();

        }
        public string GetPathFolder(string file)
        {
            string result = "";
            string[] array = file.Split('\\');
            for (int i = 0; i < array.Length - 1; i++)
            {
                result += array[i] + "\\";
            }

            return result;
        }
        private DataTable GetDataGridViewAsDataTable(DataGridView _DataGridView)
        {
            try
            {
                if (_DataGridView.ColumnCount == 0) return null;
                DataTable dtSource = new DataTable();
                //////create columns
                foreach (DataGridViewColumn col in _DataGridView.Columns)
                {
                    if (col.ValueType == null) dtSource.Columns.Add(col.Name, typeof(string));
                    else dtSource.Columns.Add(col.Name, col.ValueType);
                    dtSource.Columns[col.Name].Caption = col.HeaderText;
                }
                ///////insert row data
                foreach (DataGridViewRow row in _DataGridView.Rows)
                {
                    DataRow drNewRow = dtSource.NewRow();
                    foreach (DataColumn col in dtSource.Columns)
                    {
                        drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value;
                    }
                    dtSource.Rows.Add(drNewRow);
                }
                return dtSource;
            }
            catch
            {
                return null;
            }
        }
        public void SaveAModel()
        {
            //Save Camera Setting
            if (modelParameter.Cam == null) modelParameter.Cam = new CamPara[3];
            modelParameter.Cam[0].VisionFile = txbFileVision1.Text;
            try
            {
                modelParameter.Cam[0].Exposure = Convert.ToInt32(tbExposure.Text);
                if (_form1.GUICamera != null)
                {
                    _form1.GUICamera.SetExposure(modelParameter.Cam[0].Exposure);
                }
            }
            catch
            {
                MessageBox.Show("Can't Save Exposure");
            }

            modelParameter.MotionFile = PLCControl.tbMotionFile.Texts;

            //Save Model Setting
            modelParameter.ModelName = modelSetting.modelTable.Table.Rows[_form1.ModelNoInternal].Cells[1].Value.ToString();
            currentModelInfomation.PathFileModel = modelSetting.modelTable.Table.Rows[_form1.ModelNoInternal].Cells[2].Value.ToString();
            modelParameter.ListFFTTest = GetDataGridViewAsDataTable(modelSetting.dgvFFT);
            modelParameter.Product = modelSetting.cbProduct.Text;
            modelParameter.Line = modelSetting.cbLine.Text;
            modelParameter.StationTest = modelSetting.cbStationTest.Text;
            modelParameter.index = modelSetting.cbIndex.Text;
            modelParameter.AreaTest = modelSetting.cbArea.Text;
            modelParameter.ModeTest = modelSetting.cbModeTest.Text;
            modelParameter.TypeTest = modelSetting.cbTypeTest.Text;
            modelParameter.KeySight.nudVoltage6V = Convert.ToDouble(nudVoltage6V.Value);
            modelParameter.KeySight.nudCurrent6V = Convert.ToDouble(nudCurrent6V.Value);
            modelParameter.KeySight.nudVoltage9V = Convert.ToDouble(nudVoltage9V.Value);
            modelParameter.KeySight.nudCurrent9V = Convert.ToDouble(nudCurrent9V.Value);
            ShowFFTTableInMainScreen();

            if (!File.Exists(currentModelInfomation.PathFileModel))
            {

                System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
                dlg.CheckFileExists = false;
                dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                dlg.FilterIndex = 0;
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string fileName;
                    fileName = dlg.FileName;

                    if (!fileName.Contains(".txt")) fileName = fileName + ".txt";

                    SaveAModelParameter(modelParameter, fileName);
                    lbModelFile.Text = fileName;
                    currentModelInfomation.PathFileModel = fileName;
                }

            }
            else
            {
                string json = JsonConvert.SerializeObject(modelParameter);
                File.WriteAllText(currentModelInfomation.PathFileModel, json);
            }
            //LOG
            this.Invoke(new Action(() => {
                _form1.WriteLogPC(LogType.Main, "Model", $"Model: {modelParameter.ModelName} is Save to {currentModelInfomation.PathFileModel}");
            }));
            MessageBox.Show("Save Model Parameters is Successfull", "Save Model Parameter", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnModel_Load_Click(object sender, EventArgs e)
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
                    LoadAModel(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Load Model File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void LoadAModel(int ModelNo)
        {
            ModelColumns Col = new ModelColumns();
            if (modelSetting.modelTable.Table.Rows[ModelNo].Cells[Col.File].Value == null) return;
            string fileName = modelSetting.modelTable.Table.Rows[ModelNo].Cells[Col.File].Value.ToString();

            _form1.ModelNoInternal = ModelNo;
            currentModelInfomation.ModelNo = ModelNo;

            ReadAModelParameter(fileName);
            ShowAModelParameter(modelParameter);
            ShowFFTTableInMainScreen();
            _form1.InitialAllCameras();

            cogToolBlockEditV21.Subject = null;
            for (int i = 0; i < modelSetting.modelTable.Table.RowCount - 1; i++)
            {
                if (i == ModelNo)
                {
                    modelSetting.modelTable.Table.Rows[i].DefaultCellStyle.BackColor = Color.Lime;
                }
                else
                {
                    modelSetting.modelTable.Table.Rows[i].DefaultCellStyle.BackColor = Color.White;
                }
            }
        }
        private void btnSaveAll_Click(object sender, EventArgs e)
        {
            SaveCurrentModelInfomation(false);
        }
        #endregion

        #region Read and Save Parameter
        private string MyDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
        public string PathFileParameter = "D:\\InitialData\\ModelInfomation.txt";
        public void ReadModelToSetModelTable(DataTable dataTable)
        {
            modelSetting.modelTable.Table.Rows.Clear();
            if (dataTable == null) return;
            foreach (DataRow row in dataTable.Rows)
            {
                if (row.ItemArray[0] == null) return;
                string thisdata1 = row.ItemArray[0].ToString();

                modelSetting.modelTable.Table.Rows.Add(row.ItemArray);
            }

        }
        public void ReadFFTTableToSetFFTDataGridView(DataTable dataTable)
        {
            modelSetting.dgvFFT.Rows.Clear();
            if (dataTable == null) return;
            foreach (DataRow row in dataTable.Rows)
            {
                if (row.ItemArray[0] == null) return;
                string thisdata1 = row.ItemArray[0].ToString();

                modelSetting.dgvFFT.Rows.Add(row.ItemArray);
            }
        }
        
        public void ShowFFTTableInMainScreen()
        {
            _form1.formMain.dataGridView1.Rows.Clear();
            if (modelParameter == null) return;
            if (modelParameter.ListFFTTest == null) return;
            DataTable dataTable = modelParameter.ListFFTTest;

            foreach (DataRow row in dataTable.Rows)
            {
                if (row.ItemArray[0] == null) return;
                _form1.formMain.dataGridView1.Rows.Add(row.ItemArray);
            }

        }
        /// <summary>
        /// Model Information
        /// </summary>
        private void ReadModelInfomation()
        {
            //PathFileParameter = "D:\\InitialData\\ModelInfomation.txt";
            if (File.Exists(PathFileParameter))
            {
                string ReadText = File.ReadAllText(PathFileParameter);
                currentModelInfomation = JsonConvert.DeserializeObject<CurrentModelInfomation>(ReadText);
                ShowFixModelInfomation(currentModelInfomation);
            }
        }
        private void ShowFixModelInfomation(CurrentModelInfomation Model)
        {
            num_MachineNo.Text = Model.MachineNo;
            lbModelFile.Text = Model.PathFileModel;
            PLCControl.nudStepSizeX.Value = Model.pLCInternalPara.nudStepSizeX;
            PLCControl.nudFeedRateX.Value = Model.pLCInternalPara.nudFeedRateX;
            PLCControl.nudStepSizeR.Value = Model.pLCInternalPara.nudStepSizeR;
            PLCControl.nudFeedRateR.Value = Model.pLCInternalPara.nudFeedRateR;
            PLCControl.tbOPRCreepSpeedX.Text = Model.pLCInternalPara.tbOPRCreepSpeedX.ToString();
            PLCControl.tbOprSpeedX.Text = Model.pLCInternalPara.tbOprSpeedX.ToString();
            PLCControl.tbStartAddressX.Text = Model.pLCInternalPara.tbStartAddressX.ToString();
            PLCControl.tbOprStartSpeedX.Text = Model.pLCInternalPara.tbOprStartSpeedX.ToString();
            PLCControl.nudRCreepSpeed.Value = Model.pLCInternalPara.nudRCreepSpeed;
            PLCControl.nudROprSpeed.Value = Model.pLCInternalPara.nudROprSpeed;
            PLCControl.nudRStartAddress.Value = Model.pLCInternalPara.nudRStartAddress;
            PLCControl.nudRStartSpeed.Value = Model.pLCInternalPara.nudRStartSpeed;
            PLCControl.nudACC_X.Text = Model.pLCInternalPara.nudACC_X.ToString();
            PLCControl.nudDEC_X.Text = Model.pLCInternalPara.nudDEC_X.ToString();
            ReadModelToSetModelTable(Model.ModelInfo);

            _form1.topControl.cbSkipTestFail.Checked = Model.ModelMachine.cbSkipTestFail;
            _form1.topControl.cbSkipRetry.Checked = Model.ModelMachine.cbSkipRetry;
            _form1.topControl.cbSkipStartButton.Checked = Model.ModelMachine.cbSkipStartButton;
            _form1.topControl.cbSkipMotion.Checked = Model.ModelMachine.cbSkipMotion;
            _form1.topControl.cb_Cycles.Text = Model.ModelMachine.cb_Cycles.ToString();
            _form1.topControl.tbRounds.Text = Model.ModelMachine.tbRounds.ToString();

            _form1.formMain.cbAskSupply.Checked = Model.ModelMachine.cbAskSupply;
            _form1.formMain.cbResetWhenSetDefault.Checked = Model.ModelMachine.cbResetWhenSetDefault;
            _form1.formMain.cbUseMacMes.Checked = Model.ModelMachine.cbUseMacMes;
            _form1.formMain.cbManualMac.Checked = Model.ModelMachine.cbManualMac;
            _form1.formMain.cbCloseDoorFFT.Checked = Model.ModelMachine.cbCloseDoorFFT;
            _form1.formMain.cbTestAgain.Checked = Model.ModelMachine.cbTestAgain;
            _form1.formMain.cbTurnOnVoice.Checked = Model.ModelMachine.cbTurnOnVoice;

           
        }
        public void SaveCurrentModelInfomation(bool silent)
        {
            try
            {
                currentModelInfomation.MachineNo = num_MachineNo.Text;
                currentModelInfomation.pLCInternalPara.nudStepSizeX = Convert.ToInt32(PLCControl.nudStepSizeX.Value);
                currentModelInfomation.pLCInternalPara.nudFeedRateX = Convert.ToInt32(PLCControl.nudFeedRateX.Value);
                currentModelInfomation.pLCInternalPara.nudStepSizeR = Convert.ToInt32(PLCControl.nudStepSizeR.Value);
                currentModelInfomation.pLCInternalPara.nudFeedRateR = Convert.ToInt32(PLCControl.nudFeedRateR.Value);
                currentModelInfomation.pLCInternalPara.tbOPRCreepSpeedX = Convert.ToInt32(PLCControl.tbOPRCreepSpeedX.Text);
                currentModelInfomation.pLCInternalPara.tbOprSpeedX = Convert.ToInt32(PLCControl.tbOprSpeedX.Text);
                currentModelInfomation.pLCInternalPara.tbStartAddressX = Convert.ToInt32(PLCControl.tbStartAddressX.Text);
                currentModelInfomation.pLCInternalPara.tbOprStartSpeedX = Convert.ToInt32(PLCControl.tbOprStartSpeedX.Text);
                currentModelInfomation.pLCInternalPara.nudRCreepSpeed = Convert.ToInt32(PLCControl.nudRCreepSpeed.Value);
                currentModelInfomation.pLCInternalPara.nudROprSpeed = Convert.ToInt32(PLCControl.nudROprSpeed.Value);
                currentModelInfomation.pLCInternalPara.nudRStartAddress = Convert.ToInt32(PLCControl.nudRStartAddress.Value);
                currentModelInfomation.pLCInternalPara.nudRStartSpeed = Convert.ToInt32(PLCControl.nudRStartSpeed.Value);

                currentModelInfomation.ModelMachine.cbSkipTestFail = _form1.topControl.cbSkipTestFail.Checked;
                currentModelInfomation.ModelMachine.cbSkipRetry = _form1.topControl.cbSkipRetry.Checked;
                currentModelInfomation.ModelMachine.cbSkipStartButton = _form1.topControl.cbSkipStartButton.Checked;
                currentModelInfomation.ModelMachine.cbSkipMotion = _form1.topControl.cbSkipMotion.Checked;
                currentModelInfomation.ModelMachine.cb_Cycles = Convert.ToInt32(_form1.topControl.cb_Cycles.Text);
                currentModelInfomation.ModelMachine.tbRounds = Convert.ToInt32(_form1.topControl.tbRounds.Text);

                currentModelInfomation.ModelMachine.cbAskSupply = _form1.formMain.cbAskSupply.Checked;
                currentModelInfomation.ModelMachine.cbResetWhenSetDefault = _form1.formMain.cbResetWhenSetDefault.Checked;
                currentModelInfomation.ModelMachine.cbUseMacMes = _form1.formMain.cbUseMacMes.Checked;
                currentModelInfomation.ModelMachine.cbManualMac = _form1.formMain.cbManualMac.Checked;
                currentModelInfomation.ModelMachine.cbCloseDoorFFT = _form1.formMain.cbCloseDoorFFT.Checked;
                currentModelInfomation.ModelMachine.cbTestAgain = _form1.formMain.cbTestAgain.Checked;
                currentModelInfomation.ModelMachine.cbTurnOnVoice = _form1.formMain.cbTurnOnVoice.Checked;


                currentModelInfomation.ModelInfo = GetDataGridViewAsDataTable(modelSetting.modelTable.Table);




                //string PathFileParameter = MyDirectory() + @"\ModelInfomation.txt";
                //PathFileParameter = "D:\\InitialData\\ModelInfomation.txt";
                string json = JsonConvert.SerializeObject(currentModelInfomation);
                File.WriteAllText(PathFileParameter, json);
                if (!silent)
                    MessageBox.Show("Save Current Model Infomation is Successfull", "Save Model Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //LOG
                this.Invoke(new Action(() => {
                    _form1.WriteLogPC(LogType.Main, "Model", "Current Model Information is Save");
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "A Fix Parameter Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// Read ans Save A Model
        /// </summary>
        /// <param name="Path"></param>
        private void ReadAModelParameter(string Path)
        {
            ModelParameter Model = new ModelParameter();
            try
            {
                if (!File.Exists(Path)) throw new Exception("Model File is not exist!");
                string json = File.ReadAllText(Path);
                Model = JsonConvert.DeserializeObject<ModelParameter>(json);
                modelParameter = Model;
                lbModelFile.Text = Path;
                currentModelInfomation.PathFileModel = Path;

                //LOG
                this.Invoke(new Action(() => {
                    _form1.WriteLogPC(LogType.Main, "Model", $"Read OKE Model: {modelParameter.ModelName} From file: {Path}. OKE");
                }));
            }
            catch (Exception ex)
            {
                //LOG
                this.Invoke(new Action(() => {
                    _form1.WriteLogPC(LogType.Main, "Model", $"Read Error Model From file: {Path}. Error");
                }));
                MessageBox.Show(ex.Message, "Read a model parameters Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        public void ShowAModelParameter(ModelParameter Model)
        {

            for (int i = 0; i < _form1.NumberOfCamera; i++)
            {
                if (Model.Cam == null) break;
                if (_form1.formMain != null)
                {
                    tbExposure.Text = Model.Cam[0].Exposure.ToString();

                }
                txbFileVision1.Text = Model.Cam[0].VisionFile;
            }
            modelSetting.cbProduct.Text = Model.Product;
            modelSetting.cbLine.Text = Model.Line;
            modelSetting.cbStationTest.Text = Model.StationTest;
            modelSetting.cbIndex.Text = Model.index;
            modelSetting.cbArea.Text = Model.AreaTest;
            modelSetting.cbModeTest.Text = Model.ModeTest;
            modelSetting.cbTypeTest.Text = Model.TypeTest;
            ReadFFTTableToSetFFTDataGridView(Model.ListFFTTest);
            PLCControl.tbMotionFile.Texts = Model.MotionFile;
            ReadPLCProgramFromFile(Model.MotionFile);
            short pos2 = (short)Model.pLCModelPara.nudPoint2;
            PLCControl.nudPoint1.Value = Model.pLCModelPara.nudPoint1;
            PLCControl.nudPoint2.Value = pos2;
            PLCControl.nudPoint3.Value = Model.pLCModelPara.nudPoint3;
            PLCControl.nudPoint4.Value = Model.pLCModelPara.nudPoint4;
            PLCControl.nudPoint5.Value = Model.pLCModelPara.nudPoint5;
            PLCControl.nudPoint6.Value = Model.pLCModelPara.nudPoint6;
            PLCControl.nudSpeed.Value = Model.pLCModelPara.nudSpeed;
            PLCControl.nudRPos1.Value = Model.pLCModelPara.nudRPos1;
            PLCControl.nudRPos2.Value = Model.pLCModelPara.nudRPos2;
            PLCControl.nudRPos3.Value = Model.pLCModelPara.nudRPos3;
            PLCControl.nudRSpeed.Value = Model.pLCModelPara.nudRSpeed;
            
            nudVoltage6V.Value = Convert.ToDecimal(Model.KeySight.nudVoltage6V);
            nudCurrent6V.Value = Convert.ToDecimal(Model.KeySight.nudCurrent6V);
            nudVoltage9V.Value = Convert.ToDecimal(Model.KeySight.nudVoltage9V);
            nudCurrent9V.Value = Convert.ToDecimal(Model.KeySight.nudCurrent9V);
        }
        
        public void ReadPLCProgramFromFile(string Path)
        {
            try
            {
               
                if (!File.Exists(Path)) throw new Exception($"File PLC Program No Exists, {Path}");
                // Đọc nội dung của file và hiển thị nó trên RichTextBox
                string fileContents = File.ReadAllText(Path);
                PLCControl.rtbProgram.Text = fileContents;
                if (fileContents != "" || fileContents != null) return;
                string fileContents1 = fileContents.Replace("\r","");
                string[] Rows = fileContents1.Split('\n');
                if (Rows.Length <= 1)
                {
                    throw new Exception($"PLC Program is error from file {Path}");
                }
                int[] PLCProgramNoTemp = new int[Rows.Length];
                string[] PLCProgramNameTemp = new string[Rows.Length];
                for (int i = 0; i < Rows.Length; i++)
                {
                    if (!Rows[i].Contains(",")) throw new Exception($"PLC Program is error in Row {i} from file {Path}");
                    string[] dataRow = Rows[i].Split(',');
                    PLCProgramNoTemp[i] = Convert.ToInt32(dataRow[0]);
                    PLCProgramNameTemp[i] = dataRow[1].Replace("//", "");
                }
                PLCProgramNo = PLCProgramNoTemp;
                PLCProgramName = PLCProgramNameTemp;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }
        }
        private void SaveAModelParameter(ModelParameter Model, string Path)
        {
            try
            {
                string json = JsonConvert.SerializeObject(Model);
                File.WriteAllText(Path, json);
                //LOG
                this.Invoke(new Action(() => {
                    _form1.WriteLogPC(LogType.Main, "Model", $"Save Model: {Model.ModelName} To file: {Path}. OK");
                }));
                MessageBox.Show("Save Model is Successfull", "Save A Model", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                //LOG
                this.Invoke(new Action(() => {
                    _form1.WriteLogPC(LogType.Main, "Model", $"Save Model: {Model.ModelName} To file: {Path}. Fail");
                }));
                MessageBox.Show(ex.Message, "Read a model parameters Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Vision
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="IsInitial"></param>
        /// <returns></returns>
        public CogToolBlock LoadCogToolBlockFromFile(string FileName, bool IsInitial)
        {
            CogToolBlock cogToolBlock = new CogToolBlock();
            try
            {
                if (FileName == "" || FileName == null) return new CogToolBlock();
                this.Cursor = Cursors.AppStarting;

                if (!File.Exists(FileName))
                {
                    cogToolBlock = new CogToolBlock();
                    CogImage8Grey InputImage = new CogImage8Grey();
                    CogToolBlockTerminal cogToolBlockTerminalInput = new CogToolBlockTerminal("InputImage", InputImage);
                    CogImage8Grey OutputImage = new CogImage8Grey();
                    CogToolBlockTerminal cogToolBlockTerminalOutputImage = new CogToolBlockTerminal("OutputImage", OutputImage);
                    string DisplayImage = "DisplayImage";
                    string DataResults = "DataResults";
                    int Result = 2;
                    CogToolBlockTerminal terminalDisplayImage = new CogToolBlockTerminal(DisplayImage, DisplayImage);
                    CogToolBlockTerminal terminalDataResults = new CogToolBlockTerminal(DataResults, DataResults);
                    CogToolBlockTerminal terminalDataResult = new CogToolBlockTerminal("Result", Result);
                    cogToolBlock.Inputs.Add(cogToolBlockTerminalInput);
                    cogToolBlock.Outputs.Add(cogToolBlockTerminalOutputImage);
                    cogToolBlock.Outputs.Add(terminalDisplayImage);
                    cogToolBlock.Outputs.Add(terminalDataResults);
                    cogToolBlock.Outputs.Add(terminalDataResult);

                    CogFindLineTool cogFindLineTool1 = new CogFindLineTool();
                    CogToolBlockTerminal cogToolBlockTerminalLine = new CogToolBlockTerminal("cogFindLineTool1", cogFindLineTool1);
                    cogToolBlock.Tools.Add(cogFindLineTool1);
                    
                    CogFindLineResults cogFindLineResults1 = new CogFindLineResults();
                    CogToolBlockTerminal cogToolBlockTerminalLineResults = new CogToolBlockTerminal("Line", cogFindLineResults1);
                    cogToolBlock.Outputs.Add(cogToolBlockTerminalLineResults);

                    if (IsInitial) CogSerializer.SaveObjectToFile(cogToolBlock, FileName);

                }
                else
                {
                    cogToolBlock = CogSerializer.LoadObjectFromFile(FileName) as CogToolBlock;
                }

                this.Cursor = Cursors.Default;

            }
            catch (CogException cogex)
            {
                MessageBox.Show(cogex.ToString());
            }
            return cogToolBlock;

        }
        public void LoadCogToolBlockEditer(string FileName, bool IsInitial)
        {
            //if (cogToolBlockEditV21.Subject != null)
            //{
            //    cogToolBlockEditV21.Subject = null;
            //}
            cogToolBlockEditV21.Subject = LoadCogToolBlockFromFile(FileName, IsInitial);
            lbToolOpening.Text = FileName;
        }

        public CogToolBlock LoadCogToolBlockIndex(int index, bool ShowOnEditer)
        {
            CogToolBlock cogToolBlock = new CogToolBlock();
            string Filename = modelParameter.Cam[index].VisionFile;
            //string Filename = GetPathFolder(_form1.formModel.currentModelInfomation.PathFileModel);
            //Filename += $"CogBlockTool_{index}.vpp";
            cogToolBlock = LoadCogToolBlockFromFile(Filename, true);
            if (ShowOnEditer)
            {
                cogToolBlockEditV21.Subject = cogToolBlock;
                lbToolOpening.Text = Filename;
            }
            return cogToolBlock;
        }
        #endregion

        private void txbFileVision_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            TextBox ThisTextBox = sender as TextBox;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName;
                fileName = dlg.FileName;

                switch (ThisTextBox.Name)
                {
                    case "txbFileVision1":
                        {
                            txbFileVision1.Text = fileName;
                            break;
                        }
                    //case "txbFileVision2":
                    //    {
                    //        txbFileVision2.Text = fileName;
                    //        break;
                    //    }
                    //case "txbFileVision3":
                    //    {
                    //        txbFileVision3.Text = fileName;
                    //        break;
                    //    }
                    default: break;
                }

                //LoadCogToolBlockEditer(txbFileVision1.Text);
            }
        }

        private void txbFileVision_TextChanged(object sender, EventArgs e)
        {
            TextBox ThisTextBox = sender as TextBox;
            switch (ThisTextBox.Name)
            {
                case "txbFileVision1":
                    {
                        if (modelParameter.Cam[0] == null) modelParameter.Cam[0] = new CamPara();
                        modelParameter.Cam[0].VisionFile = txbFileVision1.Text;
                        break;
                    }
                //case "txbFileVision2":
                //    {
                //        if (modelParameter.Cam[1] == null) modelParameter.Cam[1] = new CamPara();
                //        modelParameter.Cam[1].VisionFile = txbFileVision2.Text;
                //        break;
                //    }
                //case "txbFileVision3":
                //    {
                //        if (modelParameter.Cam[2] == null) modelParameter.Cam[2] = new CamPara();
                //        modelParameter.Cam[2].VisionFile = txbFileVision3.Text;
                //        break;
                //    }
                default: break;
            }


            //LoadCogToolBlockEditer(txbFileVision1.Text);
        }

        private string DirectorImageFolder = "";
        private string[] FolderImageFiles;
        private int ImageFolderCount = 0;
        private bool ImageFolderOrFile = false;
        private void txbRunTextFolder_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            System.Windows.Forms.FolderBrowserDialog fldDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (fldDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txbRunTextFolder.Text = fldDialog.SelectedPath;
            }

        }

        private void txbRunTextFolder_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DirectorImageFolder = txbRunTextFolder.Text;
                if (DirectorImageFolder == "") return;
                if (!Directory.Exists(DirectorImageFolder)) return;
                FolderImageFiles = Directory.GetFiles(@DirectorImageFolder);
                ImageFolderCount = 0;
                ImageFolderOrFile = false;
            }
            catch (IOException ex)
            { 
                
            }
          
        }
        private string FileImageOpening = "";
        private void txbRunTestFile_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txbRunTestFile.Text = dlg.FileName;
            }
        }

        private void txbRunTestFile_TextChanged(object sender, EventArgs e)
        {
            FileImageOpening = txbRunTestFile.Text;
            IsImageFileToolRunOnce = false;
        }
        CogImageFileTool _imageFileTool = new CogImageFileTool();
        private bool IsImageFileToolRunOnce = false;
        
        private void btnRunOnce_Click(object sender, EventArgs e)
        {
            if (radioUseCam.Checked)
            {
                
                _form1.GUICamera.StartSingleShotGrabbing();
                //_form1.TriggerCamManual(0, true, true);
            }
            else
            {
                RunOneCogToolBlockEdit(GetImage());
            }
            
        }

        public void RunOneCogToolBlockEdit(CogImage24PlanarColor thisImageRun)
        {
            try
            {
                if (thisImageRun == null) return;
                if (cogToolBlockEditV21.Subject == null) return;
                if (cogToolBlockEditV21.Subject.Inputs.Count == 0) return;
                for (int i = 0; i < cogToolBlockEditV21.Subject.Inputs.Count; i++)
                {
                    if (cogToolBlockEditV21.Subject.Inputs[i].Name == "InputImage")
                    {
                        cogToolBlockEditV21.Subject.Inputs["InputImage"].Value = thisImageRun;
                        cogToolBlockEditV21.Subject.Run();
                        break;
                    }
                }
            }
            catch (CogException ex)
            {

            }
        }
        public CogImage24PlanarColor GetImage()
        {
            CogImage24PlanarColor thisImageRun = null;
            try
            {
                if (cogToolBlockEditV21.Subject == null)
                {
                    MessageBox.Show("Please Load Vpp File fisrt!");
                    return null;
                }
                // neu dung su dung cam. thi lay du lieu cua Cam de Run test
                if (radioUseCam.Checked == true)
                {
                    //_form1.BaslerOneShot();
                    //if (_form1.formMain.CameraVisionDisplays[0] != null)
                    //{
                    //    _form1._VisionSystem[CamSelectedIndex].cogAcqFifoTool.Run();
                    //    _form1._VisionSystem[CamSelectedIndex].IsTriggerStart = false;
                    //    _form1._VisionSystem[CamSelectedIndex].cogAcqFifoTool.Run();
                    //    // thisImageRun = _form1._VisionSystem[CamSelectedIndex].cogAcqFifoTool.OutputImage as CogImage8Grey;
                    //}
                    
                }

                if (radioUseImageSetting.Checked == true)
                {

                }
                // neu dung su dung Folder. thi lay du lieu cua Cam de Run test
                if (radioUseFolder.Checked == true)
                {
                    if (FolderImageFiles == null) return null;
                    if (FolderImageFiles.Length == 0)
                    {
                        if (FolderImageFiles[ImageFolderCount] == null)
                        {
                            //MessageBox.Show("Please Choose File Image first! Or File must be Image File");
                            return null;
                        }
                        else
                        {
                            if (!FolderImageFiles[ImageFolderCount].Contains("bmp"))
                            {
                                // MessageBox.Show("Please Choose File Image first! Or File must be Image File");
                                return null;
                            }

                        }

                    }
                    _imageFileTool = new CogImageFileTool();
                    if (ImageFolderCount >= FolderImageFiles.Length)
                    {
                        ImageFolderCount = 0;
                    }
                    lbFolderImageIndex.Text = ImageFolderCount.ToString();
                    _imageFileTool.Operator.Open(@FolderImageFiles[ImageFolderCount], CogImageFileModeConstants.Read);
                    _imageFileTool.Run();
                    thisImageRun = _imageFileTool.OutputImage as CogImage24PlanarColor;
                    ImageFolderCount++;
                }

                // neu dung su dung File. thi lay du lieu cua Cam de Run test
                if (radioFile.Checked == true)
                {
                    if (FileImageOpening.Length == 0)
                    {
                        //MessageBox.Show("Please Choose File Image first! Or File must be Image File");
                        return null;
                    }
                    if (!FileImageOpening.Contains("idb") || !IsImageFileToolRunOnce)
                    {
                        _imageFileTool = new CogImageFileTool();
                        _imageFileTool.Operator.Open(@FileImageOpening, CogImageFileModeConstants.Read);
                        IsImageFileToolRunOnce = true;
                    }
                    _imageFileTool.Run();
                    thisImageRun = _imageFileTool.OutputImage as CogImage24PlanarColor;
                }

            }
            catch (CogException cogex)
            {
                MessageBox.Show(cogex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return thisImageRun;
        }

        public void SaveCogToolBlock1()
        {
            try
            {
                if (cogToolBlockEditV21.Subject == null) return;
                if (!File.Exists(@lbToolOpening.Text)) return;
                if (CamSelectedIndex != 0) return;
                _form1._VisionSystem[CamSelectedIndex].cogToolBlock[0] = cogToolBlockEditV21.Subject;
                CogSerializer.SaveObjectToFile(cogToolBlockEditV21.Subject, @lbToolOpening.Text);
                MessageBox.Show($"Save CogToolblock Done. Cam{CamSelectedIndex}. CogToolBlock FileName: {@lbToolOpening.Text}" , "Save CogToolBlock");
            }
            catch (CogException ex)
            {
                MessageBox.Show(ex.Message, "Save CogToolBlock Fail");
                //Log
                this.Invoke(new Action(() =>
                {
                    _form1.WriteLogPC(LogType.Vision, $"SaveCogToolBlock", ex.Message);
                }));
            }

        }
        public void SaveCogToolBlock2()
        {
            try
            {
                if (cogToolBlockEditV21.Subject == null) return;
                if (!File.Exists(@lbToolOpening.Text)) return;
                if (CamSelectedIndex != 1) return;
                _form1._VisionSystem[CamSelectedIndex].cogToolBlock[0] = cogToolBlockEditV21.Subject;
                CogSerializer.SaveObjectToFile(cogToolBlockEditV21.Subject, @lbToolOpening.Text);
                MessageBox.Show($"Save CogToolblock Done. Cam{CamSelectedIndex}. CogToolBlock FileName: {@lbToolOpening.Text}", "Save CogToolBlock");
            }
            catch (CogException ex)
            {
                MessageBox.Show(ex.Message, "Save CogToolBlock Fail");
                //Log
                this.Invoke(new Action(() =>
                {
                    _form1.WriteLogPC(LogType.Vision, $"SaveCogToolBlock", ex.Message);
                }));
            }

        }
        private int CamSelectedIndex = 0;
        private void btnLoadVppFile1_Click(object sender, EventArgs e)
        {
            CamSelectedIndex = 0;
            LoadCogToolBlockEditer(txbFileVision1.Text, false);
        }

     
        private int CountSave;
        private int CountMov;
        private void timer1_Tick(object sender, EventArgs e)
        {
            

        }
        public bool IsliveCamModel = false;
        private void btnLiveCam_Click(object sender, EventArgs e)
        {
            
            if (_form1.MachineMode == ModeMachine.Auto) return;
            if (_form1.IsCamRunContinous[0]) return;

            _form1.IsLiveCam[0] = !_form1.IsLiveCam[0];
            IsliveCamModel = _form1.IsLiveCam[0];
            LiveCamInModelForm(IsliveCamModel);
            if (_form1.IsLiveCam[0])
            {
                _form1.GUICamera.StartContinuousShotGrabbing();
            }
            else
            {
                _form1.GUICamera.StopGrabbing();
            }
        }
        public void LiveCamInModelForm(bool Enable)
        {
            if (Enable)
            {
                panelLiveCam.Width = panel22.Width;
            }
            else
            {
                panelLiveCam.Width = 0;
            }
        
        }

        private void btnPanelViewPlus_Click(object sender, EventArgs e)
        {
            if (panel1.Width < this.Width - 100)
                panel1.Width += 20;
        }

        private void btnPanelViewSub_Click(object sender, EventArgs e)
        {

            if (panel1.Width > 100)
                panel1.Width -= 20;
        }

        private void btnViewPanelRefresh_Click(object sender, EventArgs e)
        {
            panel1.Width = panel1FirstWidth;
        }

        private void btnRefreshFolderImage_Click(object sender, EventArgs e)
        {
            ImageFolderCount = 0;
        }
        public string ConvertIntToStringNumberic(int Value, int RighOfDecimal)
        {
            string Result = "0";
            string TempValue = Value.ToString();

            switch (RighOfDecimal)
            {
                case 0:
                    {
                        Result = Value.ToString();
                        break;
                    }
                case 1:
                    {
                        if (!TempValue.Contains("-"))
                        {

                            if (TempValue.Length == 1)
                            {
                                TempValue = "0" + TempValue;
                            }
                        }
                        else
                        {

                            if (TempValue.Length == 1)
                            {
                                TempValue = TempValue.Replace("-", "");
                                TempValue = "-0" + TempValue;
                            }
                        }
                        string LeftValue = TempValue.Substring(0, TempValue.Length - 1);
                        string RightValue = TempValue.Substring(TempValue.Length - 1);
                        Result = LeftValue + "." + RightValue;
                        break;
                    }
                case 2:
                    {
                        if (!TempValue.Contains("-"))
                        {
                            if (TempValue.Length == 1)
                            {
                                TempValue = "00" + TempValue;
                            }
                            else
                            {
                                if (TempValue.Length == 2)
                                {
                                    TempValue = "0" + TempValue;
                                }
                            }
                        }
                        else
                        {

                            if (TempValue.Length == 2)
                            {
                                TempValue = TempValue.Replace("-", "");
                                TempValue = "-00" + TempValue;
                            }
                            else
                            {
                                if (TempValue.Length == 3)
                                {
                                    TempValue = TempValue.Replace("-", "");
                                    TempValue = "-0" + TempValue;
                                }
                            }
                        }
                        string LeftValue = TempValue.Substring(0, TempValue.Length - 2);
                        string RightValue = TempValue.Substring(TempValue.Length - 2);
                        Result = LeftValue + "." + RightValue;
                        break;
                    }
                case 3:
                    {
                        if (!TempValue.Contains("-"))
                        {
                            if (TempValue.Length == 1)
                            {
                                TempValue = "000" + TempValue;
                            }
                            else
                            {
                                if (TempValue.Length == 2)
                                {
                                    TempValue = "00" + TempValue;
                                }
                                else
                                {
                                    if (TempValue.Length == 3)
                                    {
                                        TempValue = "0" + TempValue;
                                    }
                                }
                            }
                        }
                        else
                        {

                            if (TempValue.Length == 2)
                            {
                                TempValue = TempValue.Replace("-", "");
                                TempValue = "-000" + TempValue;
                            }
                            else
                            {
                                if (TempValue.Length == 3)
                                {
                                    TempValue = TempValue.Replace("-", "");
                                    TempValue = "-00" + TempValue;
                                }
                                else
                                {
                                    if (TempValue.Length == 4)
                                    {
                                        TempValue = TempValue.Replace("-", "");
                                        TempValue = "-0" + TempValue;
                                    }
                                }
                            }
                        }

                        string LeftValue = TempValue.Substring(0, TempValue.Length - 3);
                        string RightValue = TempValue.Substring(TempValue.Length - 3);
                        Result = LeftValue + "." + RightValue;
                        break;
                    }
            }
            return Result;

        }
        private void btnSaveCogToolBlock_Click(object sender, EventArgs e)
        {
            SaveCogToolBlock1();
        }

        private bool HideShowPanelFFTAnalize = true;
        private void btnExtendFFTAnalize_Click(object sender, EventArgs e)
        {
            HideShowPanelFFTAnalize = !HideShowPanelFFTAnalize;
            panelFFTAnalize.Width = HideShowPanelFFTAnalize ? 384 : 39;
        }

        private void btnFftStart_Click(object sender, EventArgs e)
        {
            FftAnalysis.Start();
        }

        private void btnFftStop_Click(object sender, EventArgs e)
        {
            FftAnalysis.Stop();
        }

        private void btnFftClear_Click(object sender, EventArgs e)
        {
            FftAnalysis.Clear();
        }
    }
}
