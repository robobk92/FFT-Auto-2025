using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using Newtonsoft.Json;
using SLTSoft.RJControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace SLTtechSoft
{
    public partial class FormOption : Form
    {
        // Khai bao form chinh
        Form1 _form1;
        //Khai bao bien
        public Parameter Parameters { get; set; }
        public CameraSetting[] cameraSettings = new CameraSetting[3];
        public string[][] FolderImageFiles = new string[3][];
        public int[] IndexImageFolder = new int[3];
        public CogImageFileTool[] FileCogImageTools = new CogImageFileTool[3];
        public FormOption()
        {
            InitializeComponent();
        }
        public void InitializeUI(Form1 obj)
        {
            _form1 = obj;
            string[] myPort = System.IO.Ports.SerialPort.GetPortNames();
            cbbScannerPort.Items.AddRange(myPort);
            cbLockPort.Items.AddRange(myPort);
            InitialListControl();
            ReadParameter();

        }
        private void InitialListControl()
        {
            cameraSettings = new CameraSetting[_form1.NumberOfCamera];
            FolderImageFiles = new string[_form1.NumberOfCamera][];
            IndexImageFolder = new int[_form1.NumberOfCamera];
            FileCogImageTools = new CogImageFileTool[_form1.NumberOfCamera];

            for (int i = 0; i < _form1.NumberOfCamera; i++)
            {
                cameraSettings[i] = new CameraSetting();
                cameraSettings[i].Dock = DockStyle.Fill;
                cameraSettings[i].FolderDirector.TextChanged += FolderDirector_TextChanged;
                cameraSettings[i].FolderDirector.Tag = i.ToString();
                cameraSettings[i].FileDirector.TextChanged += FileDirector_TextChanged;
                cameraSettings[i].FileDirector.Tag = i.ToString();
                cameraSettings[i].RefreshFolderImage.Click += RefreshFolderImage_Click; ;
                cameraSettings[i].RefreshFolderImage.Tag = i.ToString();
                cameraSettings[i].RefreshFileImage.Click += RefreshFileImage_Click; ;
                cameraSettings[i].RefreshFileImage.Tag = i.ToString();

                cameraSettings[i].FolderDirector.DoubleClick += FolderDirector_DoubleClick; ;
                cameraSettings[i].FileDirector.DoubleClick += FileDirector_DoubleClick;
                cameraSettings[i].btnConnectCam.Click += BtnConnectCam_Click;
                cameraSettings[i].btnConnectCam.Tag = i.ToString();
                
            }
            tabPageCam1.Controls.Add(cameraSettings[0]);
          
        }

        private void BtnConnectCam_Click(object sender, EventArgs e)
        {
            Button textBoxCam = sender as Button;
            int cam = Convert.ToInt16(textBoxCam.Tag);
            //_form1.InitializeCamera(cam);
          

        }

        private void FileDirector_DoubleClick(object sender, EventArgs e)
        {
            TextBox textBoxCam = sender as TextBox;
            int cam = Convert.ToInt16(textBoxCam.Tag);
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxCam.Text = dlg.FileName;
            }
        }

        private void FolderDirector_DoubleClick(object sender, EventArgs e)
        {
            TextBox textBoxCam = sender as TextBox;
            int cam = Convert.ToInt16(textBoxCam.Tag);
            System.Windows.Forms.FolderBrowserDialog fldDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (fldDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxCam.Text = fldDialog.SelectedPath;
            }
        }

        private void RefreshFileImage_Click(object sender, EventArgs e)
        {
            Button textBoxCam = sender as Button;
            int cam = Convert.ToInt16(textBoxCam.Tag);
            FileCogImageTools[cam] = new CogImageFileTool();
            if (cameraSettings[cam].FileDirector.Text == "" || cameraSettings[cam].FileDirector.Text == null) return;
            FileCogImageTools[cam].Operator.Open(cameraSettings[cam].FileDirector.Text, CogImageFileModeConstants.Read);

        }

        private void RefreshFolderImage_Click(object sender, EventArgs e)
        {
            Button textBoxCam = sender as Button;
            int cam = Convert.ToInt16(textBoxCam.Tag);
            IndexImageFolder[cam] = 0;

        }

        private void FileDirector_TextChanged(object sender, EventArgs e)
        {
            TextBox textBoxCam = sender as TextBox;
            int cam = Convert.ToInt16(textBoxCam.Tag);
            FileCogImageTools[cam] = new CogImageFileTool();
            if (textBoxCam.Text == "" || textBoxCam.Text == null) return;
            if (File.Exists(textBoxCam.Text))
            FileCogImageTools[cam].Operator.Open(textBoxCam.Text, CogImageFileModeConstants.Read);
        }

        private void FolderDirector_TextChanged(object sender, EventArgs e)
        {
            TextBox textBoxCam = sender as TextBox;
            int cam = Convert.ToInt16(textBoxCam.Tag);
            if (textBoxCam.Text == "" || textBoxCam.Text == null) return;
            if (Directory.Exists(textBoxCam.Text))
            {
                FolderImageFiles[cam] = Directory.GetFiles(textBoxCam.Text);
            }
            IndexImageFolder[cam] = 0;
        }
        private string MyDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public string PathFileParameter = "D:\\InitialData\\Parameter.txt";
        public void ReadParameter()
        {
            //string PathFileParameter = MyDirectory() + @"\Parameter.txt";
            try
            {
                if (File.Exists(PathFileParameter))
                {
                    string ReadText = File.ReadAllText(PathFileParameter);
                    Parameters = JsonConvert.DeserializeObject<Parameter>(ReadText);
                    ShowParameterInterface(Parameters);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ReadParameter");
            }
           
            
        }

        public void ShowParameterInterface(Parameter parameter)
        {
            try
            {
                txbMCTittle.Text = parameter.Machine.MCTitle;
                txbPassWord.Text = parameter.Machine.Password;

                txbMES_HostIP.Text = parameter.MES.HostIP;
                txbMES_Port.Text = parameter.MES.Port.ToString();
                txbMES_MCCode.Text = parameter.MES.McCode;
                txbMES_CheckCount.Text = parameter.MES.CheckCount;

                txbScanner_HostIP.Text = parameter.Scanner.HostIP;
                txbScanner_Port.Text = parameter.Scanner.Port.ToString();

                cbbScannerPort.Text = parameter.Scanner.PortName;
                cbbScannerBaudrate.Text = parameter.Scanner.BaudRate.ToString();
                cbbScannerDataBits.Text = parameter.Scanner.DataBits.ToString();
                cbbScannerParity.Text = parameter.Scanner.Parity;
                cbbScannerStopbits.Text = parameter.Scanner.StopBits;

                txbPLC_HostIP.Text = parameter.PLC[0].HostIP;
                txbPLC_Port.Text = parameter.PLC[0].Port.ToString();

                cbbPLC1PortName.Text = parameter.PLC[0].PortName;
                cbbPLC1Baudrate.Text = parameter.PLC[0].BaudRate.ToString();
                cbbPLC1Databits.Text = parameter.PLC[0].DataBits.ToString();
                cbbPLC1Parity.Text = parameter.PLC[0].Parity;
                cbbPLC1Stopbits.Text = parameter.PLC[0].StopBits;
                tbThreadInterval.Text = parameter.PLC[0].ThreadInterval.ToString() ;

                cbLockPort.Text = parameter.Lock.PortName;
                cbLockBaudrate.Text = parameter.Lock.BaudRate.ToString();
                cbLockDataBits.Text = parameter.Lock.DataBits.ToString();
                cbLockParity.Text = parameter.Lock.Parity;
                cbLockStopBits.Text = parameter.Lock.StopBits;

                txbCommonPath.Text = parameter.CommonPath;
                

                for (int i = 0; i < _form1.NumberOfCamera; i++)
                {
                    if (parameter.Cam[i] == null) break;
                    cameraSettings[i].IPAddress.Text = parameter.Cam[i].HostIP;
                    cameraSettings[i].Use.Checked = parameter.Cam[i].Use;
                    cameraSettings[i].SaveOrigin.Checked = parameter.Cam[i].SaveOrigin;
                    cameraSettings[i].SaveGraphic.Checked = parameter.Cam[i].SaveGraphic;
                    cameraSettings[i].SaveOriginSeparate.Checked = parameter.Cam[i].SaveOriginSeparate;
                    switch (parameter.Cam[i].ImageSource)
                    {
                        case ImageSource.Cam:
                            {
                                cameraSettings[i].ModeCam.Checked = true;
                                break;
                            }
                        case ImageSource.Folder:
                            {
                                cameraSettings[i].ModeFolder.Checked = true;
                                break;
                            }
                        case ImageSource.File:
                            {
                                cameraSettings[i].ModeFile.Checked = true;
                                break;
                            }
                        default: break;
                    }

                    switch (parameter.Cam[i].TriggerMode)
                    {
                        case TriggerMode.Manual:
                            {
                                cameraSettings[i].TriggerManual.Checked = true;
                                break;
                            }
                        case TriggerMode.HardwareAuto:
                            {
                                cameraSettings[i].TriggerAuto.Checked = true;
                                break;
                            }

                        default: break;
                    }

                    cameraSettings[i].FolderDirector.Text = parameter.Cam[i].ImageFolder;
                    cameraSettings[i].FileDirector.Text = parameter.Cam[i].ImageFile;
                }

            }
            catch (Exception ex)
            { 
            
            }
            
        }

        public bool CheckIPAddress(string IP)
        {
            
            try
            {
                if (!IP.Contains(".")) return false;
                string[] filter = IP.Split('.');
                if(filter.Length !=4 ) return false;

                foreach (string filter2 in filter)
                {
                    int thisValue = Convert.ToInt32(filter2);
                    if (thisValue < 0 || thisValue > 255) return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            
        }
        public Parameter SaveAllParameter()
        {
            Parameter parameter = new Parameter();
            try
            {
                parameter.Machine.MCTitle = txbMCTittle.Text;
                parameter.Machine.Password = txbPassWord.Text;

                if (!CheckIPAddress(txbMES_HostIP.Text))
                    throw new Exception("MES HOST IP FORMAT IS NOT CORRECT! SAVE PARAMETER FAIL");

                parameter.MES.HostIP = txbMES_HostIP.Text;
                parameter.MES.Port = Convert.ToInt32(txbMES_Port.Text);
                parameter.MES.McCode = txbMES_MCCode.Text;
                parameter.MES.CheckCount = txbMES_CheckCount.Text;

                if (!CheckIPAddress(txbScanner_HostIP.Text))
                    throw new Exception("SCANNER HOST IP FORMAT IS NOT CORRECT! SAVE PARAMETER FAIL");

                parameter.Scanner.HostIP = txbScanner_HostIP.Text;
                parameter.Scanner.Port = Convert.ToInt32(txbScanner_Port.Text);
                parameter.Scanner.PortName = cbbScannerPort.Text;
                parameter.Scanner.BaudRate = Convert.ToInt32(cbbScannerBaudrate.Text);
                parameter.Scanner.DataBits = Convert.ToInt32(cbbScannerDataBits.Text);
                parameter.Scanner.Parity = cbbScannerParity.Text;
                parameter.Scanner.StopBits = cbbScannerStopbits.Text;

                parameter.Lock.PortName = cbLockPort.Text;
                parameter.Lock.BaudRate = Convert.ToInt32(cbLockBaudrate.Text);
                parameter.Lock.DataBits = Convert.ToInt32(cbLockDataBits.Text);
                parameter.Lock.Parity = cbLockParity.Text;
                parameter.Lock.StopBits = cbLockStopBits.Text;

                parameter.PLC[0] = new PLCSetting();
                parameter.PLC[1] = new PLCSetting();
                if (!CheckIPAddress(txbPLC_HostIP.Text))
                {
                    txbPLC_HostIP.Text = "0.0.0.0";
                    parameter.PLC[0].HostIP = "0.0.0.0";
                }
                else
                {
                    parameter.PLC[0].HostIP = txbPLC_HostIP.Text;
                }
                parameter.PLC[0].Port = Convert.ToInt32(txbPLC_Port.Text);

                parameter.PLC[0].PortName = cbbPLC1PortName.Text;
                parameter.PLC[0].BaudRate = Convert.ToInt32(cbbPLC1Baudrate.Text);
                parameter.PLC[0].DataBits = Convert.ToInt32(cbbPLC1Databits.Text);
                parameter.PLC[0].Parity = cbbPLC1Parity.Text;
                parameter.PLC[0].StopBits = cbbPLC1Stopbits.Text;

                parameter.CommonPath = txbCommonPath.Text;

                for (int i = 0; i < _form1.NumberOfCamera; i++)
                {
                    parameter.Cam[i] = new CamSetting();

                    if (!CheckIPAddress(cameraSettings[i].IPAddress.Text))
                    {
                        cameraSettings[i].IPAddress.Text = "0.0.0.0";
                        parameter.Cam[i].HostIP = "0.0.0.0";
                    }
                    else
                    {
                        parameter.Cam[i].HostIP = cameraSettings[i].IPAddress.Text;
                    }
                    parameter.Cam[i].Use = cameraSettings[i].Use.Checked;
                    parameter.Cam[i].SaveGraphic = cameraSettings[i].SaveGraphic.Checked;
                    parameter.Cam[i].SaveOrigin = cameraSettings[i].SaveOrigin.Checked;
                    parameter.Cam[i].SaveOriginSeparate = cameraSettings[i].SaveOriginSeparate.Checked;
                    if (cameraSettings[i].ModeCam.Checked) parameter.Cam[i].ImageSource = ImageSource.Cam;
                    if (cameraSettings[i].ModeFolder.Checked) parameter.Cam[i].ImageSource = ImageSource.Folder;
                    if (cameraSettings[i].ModeFile.Checked) parameter.Cam[i].ImageSource = ImageSource.File;

                    if (cameraSettings[i].TriggerManual.Checked) parameter.Cam[i].TriggerMode = TriggerMode.Manual;
                    if (cameraSettings[i].TriggerAuto.Checked) parameter.Cam[i].TriggerMode = TriggerMode.HardwareAuto;
                    parameter.Cam[i].ImageFolder = cameraSettings[i].FolderDirector.Text;
                    parameter.Cam[i].ImageFile = cameraSettings[i].FileDirector.Text;
                }
                parameter.PLC[0].ThreadInterval = Convert.ToInt32(tbThreadInterval.Text);

            }
            catch (Exception ex) 
            {
                parameter = null;
                MessageBox.Show(ex.Message, "Save Parameter Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return parameter;
        }

        private void btnSaveAll_Click(object sender, EventArgs e)
        {
           
            Parameter parameter = SaveAllParameter();
           
            if (parameter != null) 
            {
                Parameters = parameter;
                string json = JsonConvert.SerializeObject(Parameters);
                //string PathFileParameter = MyDirectory() + @"\Parameter.txt";
                File.WriteAllText(PathFileParameter, json);
            }
            MessageBox.Show("SAVE PARAMETER SUCCESSFULL!", "Parameter", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
        }

        private void btnPLCConnect_Click(object sender, EventArgs e)
        {
            //_form1.ConnectQPLC(Parameters.PLC[0].HostIP, Parameters.PLC[0].Port);
        }

        private void btnPLCDisconnect_Click(object sender, EventArgs e)
        {
         
        }

        private void btnPrinterConnect_Click(object sender, EventArgs e)
        {
          
        }

        private void btnPrinterDisConnect_Click(object sender, EventArgs e)
        {
            
        }

        private void btnScannerConnect_Click(object sender, EventArgs e)
        {
           
        }

        private void btnScannerDisconnect_Click(object sender, EventArgs e)
        {
        
        }

        private void txbCommonPath_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
            
          
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txbCommonPath.Text = dlg.SelectedPath;
                

            }
        }

        private void btnRefreshPortPrinter_Click(object sender, EventArgs e)
        {
            string[] myPort = System.IO.Ports.SerialPort.GetPortNames();
          
        }

        private void btnRefeshPortScanner_Click(object sender, EventArgs e)
        {
            string[] myPort = System.IO.Ports.SerialPort.GetPortNames();
            cbbScannerPort.Items.Clear();
            cbbScannerPort.Items.AddRange(myPort);
        }

      

        private void numBcodeText3_Click(object sender, EventArgs e)
        {
            NumbericEdit ThisnumbericEdit = sender as NumbericEdit;
            NumPad numPad = new NumPad();
            numPad.InitialUI(ThisnumbericEdit, null, _form1);
            numPad.Show();
        }

        private void btnMesConnect_Click(object sender, EventArgs e)
        {
            //_form1.MES.ConnectTCP(Parameters.MES.HostIP, Parameters.MES.Port);
        }

        private void btnMesDisconnect_Click(object sender, EventArgs e)
        {
            //_form1.MES.Disconnect();
        }

        private void btnMesTest_Click(object sender, EventArgs e)
        {
            //_form1.MES.DataInformationRequest(txbMES_CheckCount.Text);
        }

        private void btnRefreshPLC1Port_Click(object sender, EventArgs e)
        {
            string[] myPort = System.IO.Ports.SerialPort.GetPortNames();
            cbbPLC1PortName.Items.Clear();
            cbbPLC1PortName.Items.AddRange(myPort);
        }

        private void btnRefreshPLC2Port_Click(object sender, EventArgs e)
        {
            string[] myPort = System.IO.Ports.SerialPort.GetPortNames();
            
        }
    }
}
