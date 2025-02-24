using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Reflection;
using Serilog;
using System.Diagnostics;
//VisionPro
using Cognex.VisionPro;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.Exceptions;
using System.Drawing.Imaging;

// PLC Connection
using SLMP;
using LibFunction;

using static SLTtechSoft.Welcome;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using GUISampleMultiCam;
using static SLTtechSoft.TopControl;
using Basler.Pylon;
using System.Windows.Forms.DataVisualization.Charting;
using Newtonsoft.Json.Linq;
using ByYou;
using static ByYou.ApiMes;
using static System.Net.WebRequestMethods;
using System.Text.RegularExpressions;
using System.Text.Json;
using AASTV_Auto_Test;

namespace SLTtechSoft
{
    public partial class Form1 : Form
    {

        #region Variables
        //Interface
        public LeftTabControl leftTabControl = new LeftTabControl();
        public TopControl topControl = new TopControl();

        //Các biến test máy
        private static string[] ThisMac = new string[]
        {
        "48F17F565DD3", //My PC
        "6CB311361BC8", //Nex+ 1
        "6CB311361E34", //Nex+ 2
        "6CB311361B90"  //Nex+ 3
        };

        public static bool IsTestNoPLC = true;
        public static bool[] IsTestNoCam = { true, false, false };

        //public static bool IsTestNoPLC = false;
        //public static bool[] IsTestNoCam = { false, false, false };

        public static bool IsTestNoPrint = true;
        public static bool IsTestNoScanner = false;
        public static bool IsTestNoMes = true;
        public static bool IstestNoLock = false;

        // Các form
        public Main formMain;
        public Model formModel;
        public FormIO formIO;
        public FormOption formOption;
        public ApiMes apiMes;
        bool Language = false;

        //Các thiết bị
        //Khai báo PLC
        public PLCCommunication PLC = new PLCCommunication();
        SLMPTCP Fx5uPLC = new SLMPTCP();
        private bool IsConnectPLC = false;
        public static int threadSleepTime = 5;
        private static int[] DataWritePLC = new int[40];
        private Stopwatch[] StopwatchReadPLC = new Stopwatch[2];
        private Stopwatch[] StopwatchWritePLC = new Stopwatch[2];
        private string[] TimeReadPLC = new string[2];
        private string[] TimeWritePLC = new string[2];
        private bool interval = false;
        public bool[] IsPLCConnected = new bool[2];
        //Khai Báo Scanner
        public SerialPort ScannerSerial = new SerialPort();
        //Khai Báo Lock
        public LockCommunication LockASSA = new LockCommunication();
        //Khai báo KeySight
        public enum LogType
        {
            Main,
            Mes,
            PLC,
            Vision,
            KeySight,
            FFT,
            LockSend,
            LockRecived
        }
        //Các biến khác
        private Thread trd;
        private Thread threadCam1;
        public WordConvert _WordConvert = new WordConvert();
        public int IndexFormShowing = 0;
        //Sản Lượng
        public string MachineNo = "0";

        //Camera
        public GUICamera GUICamera = new GUICamera();

        public int NumberOfCamera = 1;
        private int CountImageBasler = 0;

        public int ProcessIndex;
        public string ProcessStepName = "";
        public string TimeVision = "";

        int ProcessIndex2;
        public string ProcessStepName2 = "";
        public string TimeVision2 = "";
        //ApiMes
        public string line = "";
        public string LINE_INFOR = "";
        private GetProductInfoResponse _productInfo = null;
        public string No_WO = "";
        public int Total_WO, Remaining_WO, Passed = 0;
        public string station_name = "";
        IniFile WOFile;
       

        public enum ResultVision
        {
            Wait,
            Error,
            NG,
            OK
        }

        public class ResultColumn
        {
            public int No = 0;
            public int Name = 1;
            public int Tolerance = 2;
            public int ReqSize = 3;
            public int Result = 4;
        }
        public class ImageInfo
        {
            public CogImage24PlanarColor Image { get; set; }

        }
        public class VisionSystem
        {
            public CogStopwatch StopWatchVision = new CogStopwatch();
            public ICogAcqTrigger TriggerOperator;
            public CogAcqFifoTool CAM = new CogAcqFifoTool();
            public bool IsTriggerStart = false;
            public bool IsCamConnected = false;
            public CogToolBlock[] cogToolBlock;
            public int[] CogToolBlockNo;
            public bool IsToolBlockInitial = false;

            public ImageInfo[] imageInfos;
            public ResultVision Result = ResultVision.Wait;
            public string Code = "";
            public int ResultTriggerOne = 0;
            public bool ErrorVision = false;
            public int[] ResultArray;
            public int numAcq = 0;
            public int TimeVision;
            public bool FinishTrigger;
            public bool TriggerOne = false;
            public bool[] LedResult = new bool[17];
            public int[] LedValue = new int[17];
        }



        public VisionSystem[] _VisionSystem = new VisionSystem[3];
        private ICogAcqTrigger[] mTriggers;
        public int ModelNoInternal = 999;
        //AutoMode
        public enum ModeMachine
        {
            Manual,
            Teaching,
            Auto
        }
        public ModeMachine MachineMode = ModeMachine.Manual;
        private int CountCheckPLC_On = 0;
        private int CountCheckPLC_Off = 0;
        public bool[] IsCamRunContinous = { false, false, false };
        public bool[] IsLiveCam = { false, false, false };

        private int[] ErrorListCount = new int[2];
        private bool[] IsReadErrorList = new bool[2];
        private string[,] ErrorListContent1 = new string[2, 300];
        private string[,] ErrorListContent2 = new string[2, 300];
        public int[] CurrentErrorWord1 = new int[14];
        public int[] CurrentErrorWord2 = new int[14];

        TestMode _TestMode;
        #endregion Variables

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, int count);

        public Form1()
        {
            CheckLastProcessRunning(false);
            //CheckMacID();
            CheckFirstRun();
            InitializeComponent();

            IsCamRunContinous = new bool[NumberOfCamera];
            IsLiveCam = new bool[NumberOfCamera];
            mTriggers = new ICogAcqTrigger[NumberOfCamera];

        }
        /// <summary>
        /// INITIAL
        /// </summary>
        #region Check Last running process
        private void CheckFirstRun()
        {

            DialogResult result = MessageBox.Show("Yes : Normal Machine Mode \nNo: Test Mode (No connect to PLC and Camera) \nCancel: Exit", "Mode Software", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                IsTestNoPLC = false;
                IsTestNoCam = new bool[] { false, false, false };
                IsTestNoScanner = false;
                IstestNoLock = false;
                IsTestNoMes = false;
            }
            if (result == DialogResult.No)
            {
                IsTestNoPLC = true;
                IsTestNoCam = new bool[] { true, true, true };
                IsTestNoScanner = true;
                IstestNoLock = true;
                IsTestNoMes = true;
            }
            if (result == DialogResult.Cancel)
            {
                this.Close();
                Environment.Exit(0);

            }


        }
        private void CheckLastProcessRunning(bool bShowMessage)
        {
            if (System.Diagnostics.Process.GetProcessesByName(System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                if (bShowMessage)
                {
                    MessageBox.Show("Another process is running, Please End last process", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                this.Close();
                Environment.Exit(0);
            }
        }
        private void CheckMacID()
        {
            string[] macAddr =
                (
                 from nic in NetworkInterface.GetAllNetworkInterfaces()
                 where nic.OperationalStatus == OperationalStatus.Up
                 select nic.GetPhysicalAddress().ToString()
                ).ToArray();
            bool IsfindOut = false;
            for (int i = 0; i < macAddr.Length; i++)
            {
                if (IsfindOut) break;
                for (int j = 0; j < ThisMac.Length; j++)
                {
                    if (macAddr[i] == ThisMac[j])
                    {
                        IsfindOut = true;
                        break;
                    }
                }
            }
            if (!IsfindOut)
            {
                MessageBox.Show("This Computer is Not Register! Close.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                Environment.Exit(0);
            }

        }
        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeVision();
            InitialDevice();
            InitialUI();
            GetPOInformation();
            timer1.Enabled = true;
        }
        private void InitialDevice()
        {
            trd = new Thread(new ThreadStart(this.ThreadTask));
            trd.IsBackground = true;

            ScannerSerial = new SerialPort();
            ScannerSerial.DataReceived += Scanner_DataReceived;

            LockASSA = new LockCommunication();
            LockASSA.Mode_TCP_Serial = true;
            LockASSA.EventTransferDataLog += LockASSA_EventTransferDataLog;
            LockASSA.EventExceptionLog += LockASSA_EventExceptionLog;



            ErrorListContent1 = ReadErrorListContent(0);
            ErrorListContent2 = ReadErrorListContent(1);

        }
        private void LockASSA_EventTransferDataLog(bool Type, string command, string content)
        {
            if (!Type) WriteLogPC(LogType.LockSend, command, content);
            else WriteLogPC(LogType.LockRecived, command, content);

        }
        private void LockASSA_EventExceptionLog(string command, string content)
        {
            WriteLogPC(LogType.Main, command, content);
        }
        private void InitialUI()
        {
            leftTabControl.Dock = DockStyle.Fill;
            this.panelLeftControl.Controls.Add(leftTabControl);
            leftTabControl.btnRun.Click += BtnRun_Click;
            leftTabControl.btnStop.Click += BtnStop_Click;

            topControl.Dock = DockStyle.Fill;
            panelTopControl.Controls.Add(topControl);
            topControl.btnShutdown.Click += Shutdown;
            topControl.btnReset.Click += BtnReset_Click;


            formIO = new FormIO();
            formIO.TopLevel = false;
            this.panelMother.Controls.Add(formIO);

            formOption = new FormOption();
            formOption.TopLevel = false;
            this.panelMother.Controls.Add(formOption);

            formModel = new Model();
            formModel.TopLevel = false;
            this.panelMother.Controls.Add(formModel);

            formMain = new Main();
            formMain.TopLevel = false;
            this.panelMother.Controls.Add(formMain);

            formOption.InitializeUI(this);
            formModel.InitializeUI(this);
            formIO.InitializeUI(this);
            formMain.InitializeUI(this);

            formModel.timer1.Enabled = false;

            ScannerConnect();
            LockConnect();
            ConnectKeySightPSU();
            IndexFormShowing = 0;
            formMain.Show();
            btnMain.BackColor = TabControlBackColorON;
            btnModel.BackColor = TabControlBackColorOFF;
            btnIO.BackColor = TabControlBackColorOFF;
            btnOption.BackColor = TabControlBackColorOFF;
            formModel.btnRunOnce.Enabled = GUICamera.IsSingleShotSupported();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
          
            DialogResult result = MessageBox.Show("Do you want Reset Testing", "Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                formModel.ShowFFTTableInMainScreen();
                formMain.StartTesting = false;
                MachineMode = ModeMachine.Manual;
                formMain.ProcessTestIndex = 0;
                CurrentSerial_Number = "dacb1";
                CurrentQRCodeRecived = "123";
                formMain.IsLedDoorOn = false;
                _VisionSystem[0].FinishTrigger = false;
                PLC.Write.Auto.Test.FinishProcessTest =true;
            }
         
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            MachineMode = ModeMachine.Manual;
        }
            
        private void BtnRun_Click(object sender, EventArgs e)
        {
            
            MachineMode = ModeMachine.Auto;
        }

        private void RefreshSizeForm()
        {

            if (formMain != null && IndexFormShowing == 0)
            {
                formMain.WindowState = FormWindowState.Normal;
                formMain.WindowState = FormWindowState.Maximized;
            }
            if (formModel != null && IndexFormShowing == 1)
            {
                formModel.WindowState = FormWindowState.Normal;
                formModel.WindowState = FormWindowState.Maximized;
            }
            if (formIO != null && IndexFormShowing == 2)
            {
                formIO.WindowState = FormWindowState.Normal;
                formIO.WindowState = FormWindowState.Maximized;
            }
            if (formOption != null && IndexFormShowing == 3)
            {
                formOption.WindowState = FormWindowState.Normal;
                formOption.WindowState = FormWindowState.Maximized;
            }

        }
        Color TabControlBackColorON = Color.FromArgb(128, 128, 255);
        Color TabControlBackColorOFF = Color.FromArgb(45, 45, 55);
        /// <summary>
        /// Menu Click
        /// </summary>
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            RefreshSizeForm();
        }
        private void btnMain_Click(object sender, EventArgs e)
        {
            if (IndexFormShowing != 0)
            {
                IndexFormShowing = 0;
                formMain.Show();
                formModel.Hide();
                formIO.Hide();
                formOption.Hide();
                btnMain.BackColor = TabControlBackColorON;
                btnModel.BackColor = TabControlBackColorOFF;
                btnIO.BackColor = TabControlBackColorOFF;
                btnOption.BackColor = TabControlBackColorOFF;

                formModel.timer1.Enabled = false;
                //formIO.timer1.Enabled = false;
            }
            formModel.LiveCamInModelForm(false);
            formModel.IsliveCamModel = false;
            IsLiveCam = new bool[3];
            RefreshSizeForm();
        }
        private void btnModel_Click(object sender, EventArgs e)
        {
            IsLiveCam = new bool[3];
            if (IndexFormShowing != 1)
            {
                IndexFormShowing = 1;
                formMain.Hide();
                formModel.Show();
                formIO.Hide();
                formOption.Hide();
                btnMain.BackColor = TabControlBackColorOFF;
                btnModel.BackColor = TabControlBackColorON;
                btnIO.BackColor = TabControlBackColorOFF;
                btnOption.BackColor = TabControlBackColorOFF;
                //formModel.timer1.Enabled = true;
                //formIO.timer1.Enabled = false;
            }

            RefreshSizeForm();
        }
        private void btnIO_Click(object sender, EventArgs e)
        {
            IsLiveCam = new bool[3];
            if (IndexFormShowing != 2)
            {
                IndexFormShowing = 2;
                formMain.Hide();
                formModel.Hide();
                formIO.Show();
                formOption.Hide();
                btnMain.BackColor = TabControlBackColorOFF;
                btnModel.BackColor = TabControlBackColorOFF;
                btnIO.BackColor = TabControlBackColorON;
                btnOption.BackColor = TabControlBackColorOFF;
                formModel.timer1.Enabled = false;
                //formIO.timer1.Enabled = true;
            }
            formModel.LiveCamInModelForm(false);
            formModel.IsliveCamModel = false;
            RefreshSizeForm();
        }
        private void btnOption_Click(object sender, EventArgs e)
        {
            IsLiveCam = new bool[3];
            if (IndexFormShowing != 3)
            {
                IndexFormShowing = 3;
                formMain.Hide();
                formModel.Hide();
                formIO.Hide();
                formOption.Show();
                btnMain.BackColor = TabControlBackColorOFF;
                btnModel.BackColor = TabControlBackColorOFF;
                btnIO.BackColor = TabControlBackColorOFF;
                btnOption.BackColor = TabControlBackColorON;
                formModel.timer1.Enabled = false;
                formModel.IsliveCamModel = false;
                //formIO.timer1.Enabled = false;
            }
            formModel.LiveCamInModelForm(false);
            RefreshSizeForm();
        }
        private void Shutdown(object sender, EventArgs e)
        {
            formModel.SaveCurrentModelInfomation(true);
            DialogResult result = MessageBox.Show("Do you want close this application", "Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DestroyCamera();
                DisposeAllDevice();
                Environment.Exit(0);
                this.Close();
            }

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            formModel.SaveCurrentModelInfomation(true);
            DialogResult result = MessageBox.Show("Do you want close this application", "Close", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                DestroyCamera();
                DisposeAllDevice();
                Environment.Exit(0);
                this.Close();
            }
            else
            {
                e.Cancel = true;

            }
        }
        public void MES_TransferLog(object sender, string Type, string Data)
        {
            //Log
            this.Invoke(new Action(() =>
            {
                WriteLogPC(LogType.Mes, Type, Data);
            }));
        }
        private void DisposeAllDevice()
        {
            trd.Abort();
            trd = null;

            GUICamera.DestroyCamera();
        }
        /// <summary>
        /// Thread PLC Communication
        /// </summary>

        private void ThreadTask()
        {
            while (true)
            {

                interval = !interval;

                if (!IsTestNoPLC)
                {

                    if (formOption != null)
                    {
                        if (formOption.Parameters != null)
                        {
                            string IpAddress = formOption.Parameters.PLC[0].HostIP;
                            int Port = formOption.Parameters.PLC[0].Port;
                            if (!Fx5uPLC.NetworkIsOk)
                            {
                                Fx5uPLC.ConnectTCP(IpAddress, Port);
                                IsConnectPLC = false;
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                if (formOption.Parameters != null)
                                {
                                    threadSleepTime = formOption.Parameters.PLC[0].ThreadInterval;
                                }
                                ReadDataPLC1();
                                //Thread.Sleep(threadSleepTime);
                                WriteDataPLC1();
                                //Thread.Sleep(threadSleepTime);
                            }

                        }
                    }
                }
            }
        }
        private void ReadDataPLC1()
        {
            //if (IsLiveCam[0]) return;
            StopwatchReadPLC[0] = new Stopwatch();
            StopwatchReadPLC[0].Start();
            try
            {
                int varCount = 40;
                int[] dataReadPlc = Fx5uPLC.ReadRegister(5000, varCount);
                if (dataReadPlc.Length != varCount) return;
                if (dataReadPlc[20] == 0) return;

                bool[] dataReadPLC_bools = _WordConvert.WordTo16Bit(dataReadPlc[0]);
                bool[] dataReadPLC_bools1 = _WordConvert.WordTo16Bit(dataReadPlc[1]);
                bool[] dataReadPLC_bools5 = _WordConvert.WordTo16Bit(dataReadPlc[5]);
                bool[] dataReadPLC_bools6 = _WordConvert.WordTo16Bit(dataReadPlc[6]);
                bool[] dataReadPLC_bools7 = _WordConvert.WordTo16Bit(dataReadPlc[7]);

                bool[] dataReadPLC_bools17 = _WordConvert.WordTo16Bit(dataReadPlc[17]);
                bool[] dataReadPLC_bools18 = _WordConvert.WordTo16Bit(dataReadPlc[18]);
                bool[] dataReadPLC_bools19 = _WordConvert.WordTo16Bit(dataReadPlc[19]);
                bool[] dataReadPLC_bools20 = _WordConvert.WordTo16Bit(dataReadPlc[20]);

                PLC.Read.Input.LimitNegAxis1 = dataReadPLC_bools[0];
                PLC.Read.Input.HomeAxis1 = dataReadPLC_bools[1];
                PLC.Read.Input.LimitPosAxis1 = dataReadPLC_bools[2];

                PLC.Read.Input.OP_Start1 = dataReadPLC_bools[4];
                PLC.Read.Input.OP_Start2 = dataReadPLC_bools[5];
                PLC.Read.Input.OP_EMG = dataReadPLC_bools[6];
               
                PLC.Read.Input.Check1 = dataReadPLC_bools[8];
                PLC.Read.Input.Check2 = dataReadPLC_bools[9];
                PLC.Read.Input.Check3 = dataReadPLC_bools[10];
                PLC.Read.Input.SafetySensor = dataReadPLC_bools[11];
                PLC.Read.Input.DoorUp = dataReadPLC_bools[12];
                PLC.Read.Input.DoorDw = dataReadPLC_bools[13];
                PLC.Read.Input.ToolsUp = dataReadPLC_bools[14];
                PLC.Read.Input.ToolsDw = dataReadPLC_bools[15];

                PLC.Read.Input.CheckSpring1 = dataReadPLC_bools1[0];
                PLC.Read.Input.CheckSpring2 = dataReadPLC_bools1[1];
                PLC.Read.Input.CheckSpring3 = dataReadPLC_bools1[2];
                PLC.Read.Input.TransToolFW = dataReadPLC_bools1[3];
                PLC.Read.Input.TransToolBW = dataReadPLC_bools1[4];
                PLC.Read.Input.Push1Dw = dataReadPLC_bools1[5];
                PLC.Read.Input.Push2FW = dataReadPLC_bools1[6];
                PLC.Read.Input.LockUp = dataReadPLC_bools1[7];
                PLC.Read.Input.LockDw = dataReadPLC_bools1[8];
                PLC.Read.Input.CheckSpringOpen = dataReadPLC_bools1[9];
                PLC.Read.Input.CheckSpringClose = dataReadPLC_bools1[10];

                PLC.Read.Output.PulseAxis1 = dataReadPLC_bools5[0];
                PLC.Read.Output.Reverse9V = dataReadPLC_bools5[2];
                PLC.Read.Output.Change9Vor6V = dataReadPLC_bools5[3];
                PLC.Read.Output.Supply6VToBattery = dataReadPLC_bools5[4];
                PLC.Read.Output.Supply9VToFront = dataReadPLC_bools5[5];
                PLC.Read.Output.DirAxis1 = dataReadPLC_bools5[6];
                PLC.Read.Output.Broken = dataReadPLC_bools5[7];
                PLC.Read.Output.TL_Green = dataReadPLC_bools5[8];
                PLC.Read.Output.TL_Red = dataReadPLC_bools5[9];
                PLC.Read.Output.TL_Yellow = dataReadPLC_bools5[10];
                PLC.Read.Output.TL_Buzzer = dataReadPLC_bools5[11];
                //PLC.Read.Output.CylinderPush2 = dataReadPLC_bools5[12];
                PLC.Read.Output.CylinderDownSpring = dataReadPLC_bools5[12];

                PLC.Read.Output.CylinderKeyAsterisk = dataReadPLC_bools6[0];
                PLC.Read.Output.CylinderKeySharp = dataReadPLC_bools6[1];
                PLC.Read.Output.CylinderKey0 = dataReadPLC_bools6[2];
                PLC.Read.Output.CylinderKey1 = dataReadPLC_bools6[3];
                PLC.Read.Output.CylinderKey2 = dataReadPLC_bools6[4];
                PLC.Read.Output.CylinderKey3 = dataReadPLC_bools6[5];
                PLC.Read.Output.CylinderKey4 = dataReadPLC_bools6[6];
                PLC.Read.Output.CylinderKey5 = dataReadPLC_bools6[7];
                PLC.Read.Output.CylinderKey6 = dataReadPLC_bools6[8];
                PLC.Read.Output.CylinderKey7 = dataReadPLC_bools6[9];
                PLC.Read.Output.CylinderKey8 = dataReadPLC_bools6[10];
                PLC.Read.Output.CylinderKey9 = dataReadPLC_bools6[11];
                PLC.Read.Output.CylinderTools3 = dataReadPLC_bools6[12];
                PLC.Read.Output.CylinderTools4 = dataReadPLC_bools6[13];
                PLC.Read.Output.CylinderFinger = dataReadPLC_bools6[14];
                PLC.Read.Output.CylinderDoorClose = dataReadPLC_bools6[15];

                PLC.Read.Output.CylinderToolsDown = dataReadPLC_bools7[0];
                PLC.Read.Output.CylinderPushLock = dataReadPLC_bools7[1];
                PLC.Read.Output.CylinderCheckSpring = dataReadPLC_bools7[2];
                PLC.Read.Output.CylinderTools1 = dataReadPLC_bools7[3];
                PLC.Read.Output.CylinderTools2 = dataReadPLC_bools7[4];
                PLC.Read.Output.CylinderTransTools = dataReadPLC_bools7[5];
                PLC.Read.Output.CylinderPush1 = dataReadPLC_bools7[6];
                PLC.Read.Output.CylinderPush2 = dataReadPLC_bools7[7];

                PLC.Read.Auto.Test.ReadyToOpenPower6V = dataReadPLC_bools17[0];
                PLC.Read.Auto.Test.ResultSpringOne = dataReadPLC_bools17[1];
                PLC.Read.Auto.Test.ResultSpringTwo = dataReadPLC_bools17[2];
                PLC.Read.Auto.Test.ResultSpringThree = dataReadPLC_bools17[3];
                PLC.Read.Auto.Test.ReadyCheckDoorPositionSensor_Close = dataReadPLC_bools17[4];
                PLC.Read.Auto.Test.ReadyCheckDoorPositionSensor_Auto_Lock = dataReadPLC_bools17[5];
                PLC.Read.Auto.Test.ReadyCheckDoorPositionSensor_Open = dataReadPLC_bools17[6];
                PLC.Read.Auto.Test.ReadyCheckDoorPositionSensor_Alarm = dataReadPLC_bools17[7];
                PLC.Read.Auto.Test.ReadyCheckPress[0] = dataReadPLC_bools17[8];
                PLC.Read.Auto.Test.ReadyCheckPress[1] = dataReadPLC_bools17[9];
                PLC.Read.Auto.Test.ReadyCheckPress[2] = dataReadPLC_bools17[10];
                PLC.Read.Auto.Test.ReadyCheckPress[3] = dataReadPLC_bools17[11];
                PLC.Read.Auto.Test.ReadyCheckPress[4] = dataReadPLC_bools17[12];
                PLC.Read.Auto.Test.ReadyCheckPress[5] = dataReadPLC_bools17[13];
                PLC.Read.Auto.Test.ReadyCheckPress[6] = dataReadPLC_bools17[14];
                PLC.Read.Auto.Test.ReadyCheckPress[7] = dataReadPLC_bools17[15];
                PLC.Read.Auto.Test.ReadyCheckPress[8] = dataReadPLC_bools18[0];
                PLC.Read.Auto.Test.ReadyCheckPress[9] = dataReadPLC_bools18[1];
                PLC.Read.Auto.Test.ReadyCheckPress[10] = dataReadPLC_bools18[2];
                PLC.Read.Auto.Test.ReadyCheckPress[11] = dataReadPLC_bools18[3];
                PLC.Read.Auto.Test.ReadyLedCheck = dataReadPLC_bools18[4];
                PLC.Read.Auto.Test.ReadyFingerprint_Check_Contact_1 = dataReadPLC_bools18[5];
                PLC.Read.Auto.Test.ReadyFingerprint_Check_Touch_1 = dataReadPLC_bools18[6];
                PLC.Read.Auto.Test.ReadyRF_Card_Check = dataReadPLC_bools18[7];
                PLC.Read.Auto.Test.ReadyButton_Check_Register_P = dataReadPLC_bools18[8];
                PLC.Read.Auto.Test.ReadyButton_Check_Register_N = dataReadPLC_bools18[9];
                PLC.Read.Auto.Test.ReadyButton_Check_Lock_P = dataReadPLC_bools18[10];
                PLC.Read.Auto.Test.ReadyButton_Check_Lock_N = dataReadPLC_bools18[11];
                PLC.Read.Auto.Test.ReadyMotor_Check_Close = dataReadPLC_bools18[12];
                PLC.Read.Auto.Test.ReadyMotor_Check_Mortise_Close = dataReadPLC_bools18[13];
                PLC.Read.Auto.Test.ReadyMotor_Check_Open = dataReadPLC_bools18[14];
                PLC.Read.Auto.Test.ReadyMotor_Check_Mortise_Open = dataReadPLC_bools18[15];
                PLC.Read.Auto.Test.IsMortise_Close = dataReadPLC_bools19[0];
                PLC.Read.Auto.Test.IsMortise_Open = dataReadPLC_bools19[1];
                PLC.Read.Auto.Test.ReadyExternal_Power_Check_9V = dataReadPLC_bools19[2];
                PLC.Read.Auto.Test.ReadyExternal_Power_Check_9V_REV = dataReadPLC_bools19[3];
                PLC.Read.Auto.Test.StartProcessTest = dataReadPLC_bools19[4];
                PLC.Read.Auto.Test.Ready_check_ADCVoltage = dataReadPLC_bools19[5];
                PLC.Read.Auto.Test.Ready_Check_IDE_Current = dataReadPLC_bools19[6];
                PLC.Read.Auto.Test.Broken_Disconnect = dataReadPLC_bools19[7];
                PLC.Read.Auto.Test.StartCyl_Check_Stuck = dataReadPLC_bools19[8];
                PLC.Read.Auto.Test.StartCyl_Resset_Default = dataReadPLC_bools19[9];

                PLC.Read.Auto.Auto = dataReadPLC_bools20[0];
                PLC.Read.Auto.Run = dataReadPLC_bools20[1];
                PLC.Read.Auto.Stop = dataReadPLC_bools20[2];
                PLC.Read.Auto.ResetAlarm = dataReadPLC_bools20[3];
                PLC.Read.Auto.ResetAll = dataReadPLC_bools20[4];
                PLC.Read.Auto.HomeAll = dataReadPLC_bools20[5];
                PLC.Read.Auto.HomeX = dataReadPLC_bools20[6];
                PLC.Read.Auto.Bypass = dataReadPLC_bools20[7];
                PLC.Read.Auto.ComFlash = dataReadPLC_bools20[14];
                PLC.Read.Auto.ComOn = dataReadPLC_bools20[15];

                PLC.Read.Word.CoordinateX = _WordConvert.Signed32Toint(dataReadPlc[22], dataReadPlc[23]);
                PLC.Read.Word.CoordinateR = _WordConvert.Signed32Toint(dataReadPlc[24], dataReadPlc[25]);
                PLC.Read.Word.CycleTime = dataReadPlc[26];
                PLC.Read.Word.ProcessNo = dataReadPlc[27];
                PLC.Read.Word.TactTime = dataReadPlc[28];
                PLC.Read.Word.TactTimeLow = dataReadPlc[30];
                PLC.Read.Word.TactTimeHigh = dataReadPlc[31];
                PLC.Read.Word.TactTimeMean = dataReadPlc[32];
                PLC.Read.Word.PassCount = dataReadPlc[33];
                PLC.Read.Word.FailCount = dataReadPlc[34];
                PLC.Read.Word.MachineWorkTime = dataReadPlc[35];
                PLC.Read.Word.HandlingTime = dataReadPlc[36];





                for (int i = 0; i < 10; i++)
                {
                    bool[] ThisBools = _WordConvert.WordTo16Bit(dataReadPlc[10 + i]);
                    PLC.Read.Auto.ErrorWord1[i] = dataReadPlc[10 + i];
                    for (int y = 0; y < 16; y++)
                    {
                        PLC.Read.Auto.ErrorList1[i * 16 + y] = ThisBools[y];
                    }
                }
            }
            catch (Exception ex)
            {
                //this.Invoke(new Action(() => {
                //    WriteLogPC(LogType.Main, "PLC", ex.ToString());
                //}));
            }
            StopwatchReadPLC[0].Stop();

            // Format and display the TimeSpan value.
            TimeSpan ts = StopwatchReadPLC[0].Elapsed;
            TimeReadPLC[0] = $"{ts.Seconds}.{PadLeftZeros(ts.Milliseconds, 3)}";
        }
        private void WriteDataPLC1()
        {
            //if (IsLiveCam[0]) return;
            StopwatchWritePLC[0] = new Stopwatch();
            StopwatchWritePLC[0].Start();

            DataWritePLC[0] = _WordConvert.Bit16ToWord(
                PLC.Write.Output.PulseAxis1,
                false,
                PLC.Write.Output.Reverse9V,
                PLC.Write.Output.Change9Vor6V,
                PLC.Write.Output.Supply6VToBattery,
                PLC.Write.Output.Supply9VToFront,
                PLC.Write.Output.DirAxis1,
                PLC.Write.Output.Broken,
                PLC.Write.Output.TL_Green,
                PLC.Write.Output.TL_Red,
                PLC.Write.Output.TL_Yellow,
                PLC.Write.Output.TL_Buzzer,
                PLC.Write.Output.CylinderDownSpring,
                false,
                false,
                false
                );
            DataWritePLC[1] = _WordConvert.Bit16ToWord(
                PLC.Write.Output.CylinderKeyAsterisk,
                PLC.Write.Output.CylinderKeySharp,
                PLC.Write.Output.CylinderKey0,
                PLC.Write.Output.CylinderKey1,
                PLC.Write.Output.CylinderKey2,
                PLC.Write.Output.CylinderKey3,
                PLC.Write.Output.CylinderKey4,
                PLC.Write.Output.CylinderKey5,
                PLC.Write.Output.CylinderKey6,
                PLC.Write.Output.CylinderKey7,
                PLC.Write.Output.CylinderKey8,
                PLC.Write.Output.CylinderKey9,
                PLC.Write.Output.CylinderTools3,
                PLC.Write.Output.CylinderTools4,
                PLC.Write.Output.CylinderFinger,
                PLC.Write.Output.CylinderDoorClose
                );
            DataWritePLC[2] = _WordConvert.Bit16ToWord(
                PLC.Write.Output.CylinderToolsDown,
                PLC.Write.Output.CylinderPushLock,
                PLC.Write.Output.CylinderCheckSpring,
                PLC.Write.Output.CylinderTools1,
                PLC.Write.Output.CylinderTools2,
                PLC.Write.Output.CylinderTransTools,
                PLC.Write.Output.CylinderPush1,
                PLC.Write.Output.CylinderPush2,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false
                );
            DataWritePLC[3] = _WordConvert.Bit16ToWord(
                PLC.Write.Auto.Auto,
                PLC.Write.Auto.Run,
                PLC.Write.Auto.Stop,
                PLC.Write.Auto.ResetAlarm,
                PLC.Write.Auto.ResetAll,
                PLC.Write.Auto.HomeAll,
                PLC.Write.Auto.HomeX,
                PLC.Write.Auto.Bypass,
                PLC.Write.Auto.TestFinish,
                PLC.Write.Auto.MotionReady,
                PLC.Write.Auto.Step_Mode,
                PLC.Write.Auto.PC_Simulation_Mode,
                false,
                false,
                false,
                PLC.Write.Auto.ComFlash
                );
            DataWritePLC[4] = _WordConvert.Bit16ToWord(
                PLC.Write.Auto.StepXNeg,
                PLC.Write.Auto.StepXPos,
                PLC.Write.Auto.StepXStop,
                PLC.Write.Auto.StepMode,
                PLC.Write.Auto.MovePosX1,
                PLC.Write.Auto.MovePosX2,
                PLC.Write.Auto.MovePosX3,
                PLC.Write.Auto.MovePosX4,
                PLC.Write.Auto.MovePosX5,
                PLC.Write.Auto.MovePosX6,
                PLC.Write.Auto.MovePosR1,
                PLC.Write.Auto.MovePosR2,
                PLC.Write.Auto.MovePosR3,
                false,
                false,
                false
                );

            DataWritePLC[6] = _WordConvert.Bit16ToWord(
                PLC.Write.Auto.Test.StartCheckVoltage6V,
                PLC.Write.Auto.Test.StartCheckDoorPositionSensor_Close,
                PLC.Write.Auto.Test.StartCheckDoorPositionSensor_Auto_Lock,
                PLC.Write.Auto.Test.StartCheckDoorPositionSensor_Open,
                PLC.Write.Auto.Test.StartCheckDoorPositionSensor_Alarm,
                PLC.Write.Auto.Test.StartCheckPress[0],
                PLC.Write.Auto.Test.StartCheckPress[1],
                PLC.Write.Auto.Test.StartCheckPress[2],
                PLC.Write.Auto.Test.StartCheckPress[3],
                PLC.Write.Auto.Test.StartCheckPress[4],
                PLC.Write.Auto.Test.StartCheckPress[5],
                PLC.Write.Auto.Test.StartCheckPress[6],
                PLC.Write.Auto.Test.StartCheckPress[7],
                PLC.Write.Auto.Test.StartCheckPress[8],
                PLC.Write.Auto.Test.StartCheckPress[9],
                PLC.Write.Auto.Test.StartCheckPress[10]
                );
            DataWritePLC[7] = _WordConvert.Bit16ToWord(
                PLC.Write.Auto.Test.StartCheckPress[11],
                PLC.Write.Auto.Test.StartLedCheck,
                PLC.Write.Auto.Test.StartFingerprint_Check_Contact_1,
                PLC.Write.Auto.Test.StartFingerprint_Check_Touch_1,
                PLC.Write.Auto.Test.StartRF_Card_Check,
                PLC.Write.Auto.Test.StartButton_Check_Register_P,
                PLC.Write.Auto.Test.StartButton_Check_Register_N,
                PLC.Write.Auto.Test.StartButton_Check_Lock_P,
                PLC.Write.Auto.Test.StartButton_Check_Lock_N,
                PLC.Write.Auto.Test.StartMotor_Check_Close,
                PLC.Write.Auto.Test.StartMotor_Check_Mortise_Close,
                PLC.Write.Auto.Test.StartMotor_Check_Open,
                PLC.Write.Auto.Test.StartMotor_Check_Mortise_Open,
                PLC.Write.Auto.Test.StartExternal_Power_Check_9V,
                PLC.Write.Auto.Test.StartExternal_Power_Check_9V_REV,
                PLC.Write.Auto.Test.FinishProcessTest

               );
            DataWritePLC[8] = _WordConvert.Bit16ToWord(
                PLC.Write.Auto.Test.Set_Check_ADC_Voltage,
                PLC.Write.Auto.Test.Set_Check_IDE_Current,
                PLC.Write.Auto.Test.Broken_Disconnect,
                PLC.Write.Auto.Test.Cyl_Check_Stuck,  
                PLC.Write.Auto.Test.Cyl_Resset_Default,
                false, 
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false
                );
           
           
            DataWritePLC[10] = PLC.Write.Word.nudStepSizeX;
            DataWritePLC[11] = PLC.Write.Word.nudFeedRateX;
            DataWritePLC[12] = PLC.Write.Word.nudStepSizeR;
            DataWritePLC[13] = PLC.Write.Word.nudFeedRateR;
            DataWritePLC[14] = PLC.Write.Word.tbOPRCreepSpeedX;
            DataWritePLC[15] = PLC.Write.Word.tbOprSpeedX;
            DataWritePLC[16] = PLC.Write.Word.tbStartAddressX;
            DataWritePLC[17] = PLC.Write.Word.tbOprStartSpeedX;
            DataWritePLC[18] = PLC.Write.Word.nudRCreepSpeed;
            DataWritePLC[19] = PLC.Write.Word.nudROprSpeed;
            DataWritePLC[20] = PLC.Write.Word.nudRStartAddress;
            DataWritePLC[21] = PLC.Write.Word.nudRStartSpeed;
            DataWritePLC[22] = PLC.Write.Word.PosCamera;
            DataWritePLC[23] = PLC.Write.Word.PosInput;
            DataWritePLC[24] = PLC.Write.Word.PosGoOriginal;
            DataWritePLC[25] = PLC.Write.Word.PosDoor;
            DataWritePLC[26] = PLC.Write.Word.Pos5;
            DataWritePLC[27] = PLC.Write.Word.Pos6;
            DataWritePLC[28] = PLC.Write.Word.SpeedXAuto;

            DataWritePLC[29] = PLC.Write.Word.nudRPos1;
            DataWritePLC[30] = PLC.Write.Word.nudRPos2;
            DataWritePLC[31] = PLC.Write.Word.nudRPos3;
            DataWritePLC[32] = PLC.Write.Word.SpeedRAuto;
            DataWritePLC[33] = PLC.Write.Word.nudACC_X;
            DataWritePLC[34] = PLC.Write.Word.nudDEC_X;


            Fx5uPLC.WriteRegister(5500, 40, 0, DataWritePLC);

            StopwatchWritePLC[0].Stop();
            TimeSpan ts2 = StopwatchWritePLC[0].Elapsed;
            // Format and display the TimeSpan value.
            TimeWritePLC[0] = $"{ts2.Seconds}.{PadLeftZeros(ts2.Milliseconds, 3)}";
        }

        /// <summary>
        /// Timer1
        /// </summary>
        private int CountStartThread = 0;
        private int CountTimeOverCamBasler = 0;
        private int CountIntervalTimer1 = 0;
        private bool isFailBaslerShot = false;
        private bool ExportingFile1 = false;
        private bool ExportingFile2 = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if(MachineMode==ModeMachine.Auto)
            {
                if (PLC.Read.Input.OP_Start1 && PLC.Read.Input.OP_Start2)
                {
                    if (!PLC.Read.Auto.Test.StartProcessTest)
                    {
                        formMain.StartTesting = false;
                        CurrentSerial_Number = "dacb1";
                        CurrentQRCodeRecived = "123";
                        
                        
                        formMain.ProcessTestIndex = 0;
                        formMain.IsLedDoorOn = false;
                        _VisionSystem[0].FinishTrigger = false;
                    }
                        
                }
            }
           
               
            formMain.DoorTestProcess();
            Timer1_ShowProductivity();
            //LiveCam();
            //CallModelAutoPLC();
            if (CountIntervalTimer1++ > 5)
            {
                if (formOption != null)
                {
                    if (formOption.Parameters != null)
                    {
                        if (formOption.Parameters.CommonPath != null)
                        {
                            DeleteOldFolderImage(formOption.Parameters.CommonPath);
                        }
                    }
                }
                RunContinueCam();
                PLC.Write.Auto.ComFlash = !PLC.Write.Auto.ComFlash;
                CountIntervalTimer1 = 0;
            }
            InitialOneTimeTimer();

            //if (!IsTestNoPLC)
            //{
            //    UpdateErrorListPLC1();
            //}
           
            lbTimeReadPLC.Text = TimeReadPLC[0];
            lbTimeWritePLC.Text = TimeWritePLC[0];
            // chu trình tự động
            if (PLC.Read.Auto.ComFlash)
            {
                CountCheckPLC_On++;
                CountCheckPLC_Off = 0;
            }
            else
            {
                CountCheckPLC_Off++;
                CountCheckPLC_On = 0;
            }
            if (CountCheckPLC_On > 50 || CountCheckPLC_Off > 50)
            {
                CountCheckPLC_On = 0;
                CountCheckPLC_Off = 0;
            }
            Timer1_ShowUI();
           
        }
        private void Timer1_ShowProductivity()
        {
            topControl.TactTime.Text = PLC.Read.Word.TactTime.ToString();
            topControl.TactTimeLow.Text = PLC.Read.Word.TactTimeLow.ToString();
            topControl.TactTimeHigh.Text = PLC.Read.Word.TactTimeHigh.ToString();
            topControl.TactTimeMean.Text = PLC.Read.Word.TactTimeMean.ToString();
            topControl.PassCount.Text = PLC.Read.Word.PassCount.ToString();
            topControl.FailCount.Text = PLC.Read.Word.FailCount.ToString();
            topControl.MachineWorkTime.Text = PLC.Read.Word.MachineWorkTime.ToString();
            topControl.HandlingTime.Text = PLC.Read.Word.HandlingTime.ToString();
            topControl.QualityLastHour.Text = PLC.Read.Word.QualityLastHour.ToString();
            topControl.QualityThisHour.Text = PLC.Read.Word.QualityThisHour.ToString();

            UpdateChartOKNG(PLC.Read.Word.PassCount, PLC.Read.Word.FailCount);
            UpdateChartPerformance(PLC.Read.Word.MachineWorkTime, PLC.Read.Word.HandlingTime);
        }

        private void UpdateChartOKNG(int Value1, int Value2)
        {
            topControl.chart_RateFirst.Series.Clear();
            topControl.chart_RateFirst.Titles.Clear();

            // Tạo Series mới
            Series series = new Series("Quality");
            series.ChartType = SeriesChartType.Pie;

            // Thêm dữ liệu OK và NG
            int okCount = Value1;  // Số lượng sản phẩm OK
            int ngCount = Value2;  // Số lượng sản phẩm NG
            int OKPercent = 0;
            if (Value1 == 0 && Value2 == 0)
            {
                okCount = 5;
                ngCount = 5;
            }
            OKPercent = okCount / (okCount + ngCount);
            topControl.lblPercentOK_NG.Text = OKPercent.ToString();

            series.Points.AddXY("OK", okCount);
            series.Points.AddXY("NG", ngCount);
            // Thêm Series vào Chart
            topControl.chart_RateFirst.Series.Add(series);
            // Tuỳ chỉnh hiển thị
            series.Points[0].Color = System.Drawing.Color.Green;  // Màu xanh cho OK
            series.Points[1].Color = System.Drawing.Color.Red;    // Màu đỏ cho NG
        }
        private void UpdateChartPerformance(int Value1, int Value2)
        {
            topControl.chart_Performance.Series.Clear();
            topControl.chart_Performance.Titles.Clear();

            // Tạo Series mới
            Series series = new Series("Performance");
            series.ChartType = SeriesChartType.Pie;

            // Thêm dữ liệu OK và NG
            int okCount = Value1;  // Số lượng sản phẩm OK
            int ngCount = Value2;  // Số lượng sản phẩm NG
            int OKPercent = 0;
            if (Value1 == 0 && Value2 == 0)
            {
                okCount = 5;
                ngCount = 5;
            }
            OKPercent = okCount / (okCount + ngCount);
            topControl.lblPercentMC_Performance.Text = OKPercent.ToString();

            series.Points.AddXY("OK", okCount);
            series.Points.AddXY("NG", ngCount);
            // Thêm Series vào Chart
            topControl.chart_Performance.Series.Add(series);
            // Tuỳ chỉnh hiển thị
            series.Points[0].Color = System.Drawing.Color.Green;  // Màu xanh cho OK
            series.Points[1].Color = System.Drawing.Color.HotPink;    // Màu đỏ cho NG
        }
        private void InitialOneTimeTimer()
        {
            if (CountStartThread <= 30)
            {
                CountStartThread++;
            }
            if (CountStartThread == 10)
            {

                trd.Start();

            }


        }
        private void Timer1_ShowUI()
        {
            leftTabControl.btnRun.BackColor = MachineMode == ModeMachine.Auto ? Color.Green : Color.AntiqueWhite;
            leftTabControl.btnStop.BackColor = MachineMode == ModeMachine.Manual ? Color.Orange : Color.AntiqueWhite;
            //Hiển thị các trạng thái kết nối
            lbPLCConected.BackColor = PLC.Read.Auto.ComFlash ? Color.Green : Color.IndianRed;
            // lbPLC2Conected.BackColor = PLC.Read.Auto.PLC2_Com ? Color.Green : Color.IndianRed;
            if (_VisionSystem != null)
            {
                if (_VisionSystem[0] != null)
                    lbCam1Connected.BackColor = GUICamera.IsOpen ? Color.Green : Color.IndianRed;
            }
            if (_VisionSystem != null)
            {
                if (_VisionSystem[0] != null)
                {
                    lbCountImageBasler.Text = _VisionSystem[0].TimeVision.ToString();
                }
            }
            //lbProcessStepNo.Text = ProcessIndex.ToString();
            //lbProcessStepName.Text = ProcessStepName;
        }

        /// <summary>
        /// Initial VISION COGNEX
        /// </summary>
        /// 
        public bool InitialCamDahua(int cam, int index)
        {


            return true;
        }
        private void CameraHandle1(object sender, Bitmap bitmap)
        {
            int cam = 0;
            if (_VisionSystem[0].TimeVision++ > 9999)
            {
                _VisionSystem[0].TimeVision = 0;
            }

            CogImage24PlanarColor cogImage8 = new CogImage24PlanarColor(bitmap);

            // Do some processing while acquiring next image
            if (cogImage8 != null)
            {
                ImageInfo ImageGrap = new ImageInfo();

                ImageGrap.Image = new CogImage24PlanarColor(cogImage8);

                if (IsLiveCam == null) return;
                if (formModel == null) return;


                if (!IsLiveCam[cam] && !formModel.IsliveCamModel)
                {

                    switch (MachineMode)
                    {
                        case ModeMachine.Auto:
                            {
                                _VisionSystem[cam].ResultTriggerOne = TriggerCamAuto(0, ImageGrap, true, true);
                                _VisionSystem[cam].FinishTrigger = true;

                                break;
                            }
                        case ModeMachine.Manual:
                            {
                                TriggerCamAuto(cam, ImageGrap, false, false);
                                formModel.RunOneCogToolBlockEdit(ImageGrap.Image);
                                break;
                            }
                        case ModeMachine.Teaching:
                            {
                                formModel.RunOneCogToolBlockEdit(ImageGrap.Image);

                                break;
                            }
                        default: break;
                    }


                }
                else
                {
                    if (IsLiveCam[cam])
                    {
                        formModel.mLiveDisplay.Image = ImageGrap.Image;
                    }
                   
                }

            }
        }
        public void InitializeVision()
        {

            _VisionSystem = new VisionSystem[NumberOfCamera];
            _VisionSystem[0] = new VisionSystem();
            GUICamera = new GUICamera();
            InitializeCamera(0);
        }
        public void InitializeCamera(int index) 
        {
            _VisionSystem[index].IsCamConnected = false;

            if (IsTestNoCam[index]) return;
            try
            {
                _VisionSystem[index].StopWatchVision = new CogStopwatch();
                GUICamera.CreateCameraByIndex(0);
                GUICamera.OpenCamera();
                GUICamera.HandleImageGrabbed += new GUICamera.ImageGrabbedHandle(CameraHandle1);
                GUICamera.GuiCameraGrabStarted += GUICamera_GuiCameraGrabStarted;
                GUICamera.GuiCameraGrabStopped += GUICamera_GuiCameraGrabStopped;
            }
            catch (Exception ex)
            {
                //Log
                this.Invoke(new Action(() =>
                {
                    WriteLogPC(LogType.Vision, $"Cam Connect", $"Index:{index}, {ex.Message}");
                }));
            }
        }

        private void GUICamera_GuiCameraGrabStopped(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                BeginInvoke(new EventHandler<GrabStopEventArgs>(GUICamera_GuiCameraGrabStopped), sender, e);
                return;
            }
           
            GUICamera.Stopwatch.Stop();
            TimeSpan ts = GUICamera.Stopwatch.Elapsed;
            lbTimeCamCapture.Text = $"{ts.Seconds}.{PadLeftZeros(ts.Milliseconds, 3)}";
            timer1.Enabled = true;
        }

        private void GUICamera_GuiCameraGrabStarted(object sender, EventArgs e)
        {
            
            
            timer1.Enabled = false;
            if (InvokeRequired)
            {
                // If called from a different thread, we must use the Invoke method to marshal the call to the proper thread.
                BeginInvoke(new EventHandler<EventArgs>(GUICamera_GuiCameraGrabStarted), sender, e);
                return;
            }
            GUICamera.Stopwatch = new Stopwatch();
            GUICamera.Stopwatch.Start();
        }

        public void InitialAllCameras()
        {

            if (formOption.Parameters == null) return;

            for (int i = 0; i < NumberOfCamera; i++)
            {
                if (formOption.Parameters.Cam[i] == null) break;
                if (_VisionSystem[i] == null) break;
                if (formModel.modelParameter.Cam[i] == null) break;
                if (formOption.Parameters.Cam[i].Use)
                {
                    ChangeParameterCam(0);
                    InitialCogToolBlock(i, formModel.modelParameter.Cam[i].VisionFile);
                }

            }

        }
        public void InitialCogToolBlock(int Cam, string CogToolBlockFile)
        {
            _VisionSystem[Cam].IsToolBlockInitial = false;
            //Initial CogToolBlock Cam 1
            try
            {
                if (_VisionSystem[Cam] == null) return;
                _VisionSystem[Cam].cogToolBlock = new CogToolBlock[1];
                _VisionSystem[Cam].CogToolBlockNo = new int[1];
                _VisionSystem[Cam].ResultArray = new int[1];
                _VisionSystem[Cam].imageInfos = new ImageInfo[1];
                _VisionSystem[Cam].CogToolBlockNo[0] = 0;
                _VisionSystem[Cam].cogToolBlock[0] = formModel.LoadCogToolBlockIndex(Cam, false);
                _VisionSystem[Cam].IsToolBlockInitial = true;
            }
            catch (CogException ex)
            {
                //Log
                this.Invoke(new Action(() =>
                {
                    WriteLogPC(LogType.Vision, $"CogToolBlock{Cam} Initial", ex.ToString());
                }));
            }
            catch (Exception ex)
            {
                //Log
                this.Invoke(new Action(() =>
                {
                    WriteLogPC(LogType.Vision, $"CogToolBlock{Cam}", ex.ToString());
                }));
            }
        }
        public void ChangeParameterCam(int cam)
        {
            try
            {

                if (_VisionSystem[cam].CAM == null) return;
                if (_VisionSystem[cam].CAM.Operator == null) return;

                if (formOption.Parameters.Cam[cam].ImageSource == ImageSource.Cam)
                {
                    GUICamera.SetExposure(formModel.modelParameter.Cam[cam].Exposure);
                }

            }
            catch (Exception ex)
            {
                //Log
                this.Invoke(new Action(() =>
                {
                    WriteLogPC(LogType.Vision, $"ChangeParameterCam({cam})", ex.ToString());
                }));
            }
        }
        /// <summary>
        /// Cam Basler
        /// </summary>
        private void LiveCam()
        {
            if (MachineMode == ModeMachine.Auto) return;

            for (int i = 0; i < NumberOfCamera; i++)
            {
                if (IsCamRunContinous[i]) return;
                if (_VisionSystem == null) return;
                if (_VisionSystem[i] == null) return;
                if (_VisionSystem[i].CAM == null) return;
                if (IsLiveCam[i])
                {
                    GUICamera.StartContinuousShotGrabbing();

                }
                if (!IsCamRunContinous[i] && !IsLiveCam[i])
                {
                    GUICamera.StopGrabbing();
                }

            }
        }
        private void RunContinueCam()
        {
            if (MachineMode == ModeMachine.Auto) return;

            for (int i = 0; i < NumberOfCamera; i++)
            {
                if (IsLiveCam[i]) return;
                if (_VisionSystem[i] == null) return;
                if (_VisionSystem[i].CAM == null) return;
                if (_VisionSystem[i].cogToolBlock == null) return;
                if (_VisionSystem[i].cogToolBlock[0] == null) return;

                if (IsCamRunContinous[i])
                {

                    if (formOption.Parameters.Cam[i].ImageSource == ImageSource.Cam)
                    {

                        GUICamera.StartSingleShotGrabbing();
                    }
                    else
                    {
                        TriggerCamManual(i, false, false);
                    }
                }

            }


        }
        private void DestroyCamera()
        {
            GUICamera.DestroyCamera();
        }
        /// <summary>
        /// ThreadCam_Task1
        /// </summary>
        public int TriggerCamAuto(int cam, ImageInfo cogImage8Grey, bool IsFitImage, bool IsFilmStrip)
        {
            int result = 0;
            if (IsTestNoCam[cam]) return 0;
            //if (!IsCamConnected) return 0;
            if (!_VisionSystem[cam].IsToolBlockInitial) return 0;
            try
            {
                int indexExport = 0;

                switch (cam)
                {
                    case 0:
                        {
                            result = VisionRun(cam, cogImage8Grey, IsFitImage);
                            break;
                        }

                    default: break;
                }



                CreatFilmStripOne(cam, indexExport, result, false);
            }
            catch (CogException ex)
            {
                result = 0;
                //Log
                this.Invoke(new Action(() =>
                {
                    WriteLogPC(LogType.Vision, $"Trigger Auto", ex.Message);
                }));
            }
            catch (Exception ex)
            {
                //Log
                this.Invoke(new Action(() =>
                {
                    WriteLogPC(LogType.Vision, $"Trigger Auto", ex.Message);
                }));
                result = 0;
            }
            return result;
        }
        public int TriggerCamManual(int cam, bool IsFitImage, bool IsFilmStrip)
        {
            Stopwatch stopWatch = new Stopwatch();

            int result = 0;
            if (!_VisionSystem[cam].IsToolBlockInitial)
            {
                stopWatch.Stop();
                return 0;
            }
            try
            {
                stopWatch.Start();
                switch (cam)
                {
                    case 0:
                        {
                            result = VisionRun(cam, GetImageVision(cam), IsFitImage);
                            break;
                        }

                    default: break;
                }

                //CreatFilmStripOne(cam, 0, result, IsFilmStrip);

                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                TimeVision = $"{ts.Seconds}.{PadLeftZeros(ts.Milliseconds, 3)}";
            }
            catch (CogException ex)
            {
                stopWatch.Stop();
                //Log
                this.Invoke(new Action(() =>
                {
                    WriteLogPC(LogType.Vision, $"TriggerManual", ex.Message);
                }));
            }
            catch (Exception ex)
            {
                stopWatch.Stop();
                //Log
                this.Invoke(new Action(() =>
                {
                    WriteLogPC(LogType.Vision, $"TriggerManual", ex.ToString());
                }));
            }
            return result;
        }
        static CogImageFileTool cogImageFileTool_GetImageFolder = new CogImageFileTool();
        public ImageInfo GetImageVision(int cam)
        {
            ImageInfo thisImageRun = new ImageInfo();

            switch (formOption.Parameters.Cam[cam].ImageSource)
            {
                case ImageSource.Cam:
                    {

                        break;
                    }
                case ImageSource.Folder:
                    {
                        if (formOption.FolderImageFiles[cam] == null) return null;
                        if (formOption.IndexImageFolder[cam] >= formOption.FolderImageFiles[cam].Length)
                        {
                            formOption.IndexImageFolder[cam] = 0;
                        }
                        cogImageFileTool_GetImageFolder.Operator.Open(@formOption.FolderImageFiles[cam][formOption.IndexImageFolder[cam]], CogImageFileModeConstants.Read);
                        cogImageFileTool_GetImageFolder.Run();
                        thisImageRun.Image = cogImageFileTool_GetImageFolder.OutputImage as CogImage24PlanarColor;
                        formOption.IndexImageFolder[cam]++;
                        break;
                    }
                case ImageSource.File:
                    {
                        if (formOption.FileCogImageTools[cam].Operator == null) return null;
                        formOption.FileCogImageTools[cam].Run();
                        thisImageRun.Image = formOption.FileCogImageTools[cam].OutputImage as CogImage24PlanarColor;

                        break;
                    }
                default: break;
            }
            return thisImageRun;
        }

        NumPadName numPadName = new NumPadName();
        private int VisionRun(int cam, ImageInfo ImageInput, bool IsFitImage)
        {
            // Khởi tạo biến
            int Result = 0;
            int Result1 = 0;
            int Result2 = 0;
            int index = 0;
            ICogRecord cogRecord = null;
            CogImage8Grey OutputImage = null;
            if (ImageInput == null) return 1;
            if (ImageInput.Image == null) return 1;

            try
            {
                //Kiếm tra Khởi tạo
                if (_VisionSystem[cam].cogToolBlock[index] == null) throw new Exception($"_VisionSystem[0].cogToolBlock[{index}] == null");
                //Gán ảnh đầu vào
                _VisionSystem[cam].cogToolBlock[index].Inputs["InputImage"].Value = ImageInput.Image;
                if (MachineMode != ModeMachine.Teaching)
                {
                    _VisionSystem[cam].imageInfos[index] = ImageInput;

                    //Check Tool Block Output Variable
                    //int FindProductLocation = (int)_VisionSystem[cam].cogToolBlock[0].Outputs["FindProductLocation"].Value;
                    //if (FindProductLocation == 0) throw new Exception($"don't Exist Card Results");


                    _VisionSystem[cam].cogToolBlock[index].Run();
                    CogToolBlock CogToolBlock_Logo = _VisionSystem[cam].cogToolBlock[0].Tools["CogToolBlock_Logo"] as CogToolBlock;
                    CogToolBlock CogToolBlock_Card = _VisionSystem[cam].cogToolBlock[0].Tools["CogToolBlock_Card"] as CogToolBlock;
                    CogToolBlock CogToolBlock_No0 = _VisionSystem[cam].cogToolBlock[0].Tools["CogToolBlock_No0"] as CogToolBlock;
                    CogToolBlock CogToolBlock_No1 = _VisionSystem[cam].cogToolBlock[0].Tools["CogToolBlock_No1"] as CogToolBlock;
                    CogToolBlock CogToolBlock_No2 = _VisionSystem[cam].cogToolBlock[0].Tools["CogToolBlock_No2"] as CogToolBlock;
                    CogToolBlock CogToolBlock_No3 = _VisionSystem[cam].cogToolBlock[0].Tools["CogToolBlock_No3"] as CogToolBlock;
                    CogToolBlock CogToolBlock_No4 = _VisionSystem[cam].cogToolBlock[0].Tools["CogToolBlock_No4"] as CogToolBlock;
                    CogToolBlock CogToolBlock_No5 = _VisionSystem[cam].cogToolBlock[0].Tools["CogToolBlock_No5"] as CogToolBlock;
                    CogToolBlock CogToolBlock_No6 = _VisionSystem[cam].cogToolBlock[0].Tools["CogToolBlock_No6"] as CogToolBlock;
                    CogToolBlock CogToolBlock_No7 = _VisionSystem[cam].cogToolBlock[0].Tools["CogToolBlock_No7"] as CogToolBlock;
                    CogToolBlock CogToolBlock_No8 = _VisionSystem[cam].cogToolBlock[0].Tools["CogToolBlock_No8"] as CogToolBlock;
                    CogToolBlock CogToolBlock_No9 = _VisionSystem[cam].cogToolBlock[0].Tools["CogToolBlock_No9"] as CogToolBlock;
                    CogToolBlock CogToolBlock_Asterisk = _VisionSystem[cam].cogToolBlock[0].Tools["CogToolBlock_Asterisk"] as CogToolBlock;
                    CogToolBlock CogToolBlock_Sharp = _VisionSystem[cam].cogToolBlock[0].Tools["CogToolBlock_Sharp"] as CogToolBlock;
                    CogToolBlock CogToolBlock_Lock = _VisionSystem[cam].cogToolBlock[0].Tools["CogToolBlock_Lock"] as CogToolBlock;
                    CogToolBlock CogToolBlock_Finger = _VisionSystem[cam].cogToolBlock[0].Tools["CogToolBlock_Finger"] as CogToolBlock;
                    CogToolBlock CogToolBlock_Battery = _VisionSystem[cam].cogToolBlock[0].Tools["CogToolBlock_Battery"] as CogToolBlock;

                    if (!GUICamera.UserLighting)
                    {
                        _VisionSystem[cam].LedValue[0] = Convert.ToInt32((double)CogToolBlock_No0.Outputs["HistoValue"].Value);
                        _VisionSystem[cam].LedValue[1] = Convert.ToInt32((double)CogToolBlock_No1.Outputs["HistoValue"].Value);
                        _VisionSystem[cam].LedValue[2] = Convert.ToInt32((double)CogToolBlock_No2.Outputs["HistoValue"].Value);
                        _VisionSystem[cam].LedValue[3] = Convert.ToInt32((double)CogToolBlock_No3.Outputs["HistoValue"].Value);
                        _VisionSystem[cam].LedValue[4] = Convert.ToInt32((double)CogToolBlock_No4.Outputs["HistoValue"].Value);
                        _VisionSystem[cam].LedValue[5] = Convert.ToInt32((double)CogToolBlock_No5.Outputs["HistoValue"].Value);
                        _VisionSystem[cam].LedValue[6] = Convert.ToInt32((double)CogToolBlock_No6.Outputs["HistoValue"].Value);
                        _VisionSystem[cam].LedValue[7] = Convert.ToInt32((double)CogToolBlock_No7.Outputs["HistoValue"].Value);
                        _VisionSystem[cam].LedValue[8] = Convert.ToInt32((double)CogToolBlock_No8.Outputs["HistoValue"].Value);
                        _VisionSystem[cam].LedValue[9] = Convert.ToInt32((double)CogToolBlock_No9.Outputs["HistoValue"].Value);
                        _VisionSystem[cam].LedValue[10] = Convert.ToInt32((double)CogToolBlock_Asterisk.Outputs["HistoValue"].Value);
                        _VisionSystem[cam].LedValue[11] = Convert.ToInt32((double)CogToolBlock_Sharp.Outputs["HistoValue"].Value);

                        _VisionSystem[cam].LedValue[14] = Convert.ToInt32((double)CogToolBlock_Lock.Outputs["HistoValue"].Value);
                        _VisionSystem[cam].LedValue[15] = Convert.ToInt32((double)CogToolBlock_Finger.Outputs["HistoValue"].Value);
                        _VisionSystem[cam].LedValue[16] = Convert.ToInt32((double)CogToolBlock_Battery.Outputs["HistoValue"].Value);


                        _VisionSystem[cam].LedResult[0] = (bool)CogToolBlock_No0.Outputs["ResultAll"].Value;
                        _VisionSystem[cam].LedResult[1] = (bool)CogToolBlock_No1.Outputs["ResultAll"].Value;
                        _VisionSystem[cam].LedResult[2] = (bool)CogToolBlock_No2.Outputs["ResultAll"].Value;
                        _VisionSystem[cam].LedResult[3] = (bool)CogToolBlock_No3.Outputs["ResultAll"].Value;
                        _VisionSystem[cam].LedResult[4] = (bool)CogToolBlock_No4.Outputs["ResultAll"].Value;
                        _VisionSystem[cam].LedResult[5] = (bool)CogToolBlock_No5.Outputs["ResultAll"].Value;
                        _VisionSystem[cam].LedResult[6] = (bool)CogToolBlock_No6.Outputs["ResultAll"].Value;
                        _VisionSystem[cam].LedResult[7] = (bool)CogToolBlock_No7.Outputs["ResultAll"].Value;
                        _VisionSystem[cam].LedResult[8] = (bool)CogToolBlock_No8.Outputs["ResultAll"].Value;
                        _VisionSystem[cam].LedResult[9] = (bool)CogToolBlock_No9.Outputs["ResultAll"].Value;
                        _VisionSystem[cam].LedResult[10] = (bool)CogToolBlock_Asterisk.Outputs["ResultAll"].Value;
                        _VisionSystem[cam].LedResult[11] = (bool)CogToolBlock_Sharp.Outputs["ResultAll"].Value;

                        _VisionSystem[cam].LedResult[14] = (bool)CogToolBlock_Lock.Outputs["ResultAll"].Value;
                        _VisionSystem[cam].LedResult[15] = (bool)CogToolBlock_Finger.Outputs["ResultAll"].Value;
                        _VisionSystem[cam].LedResult[16] = (bool)CogToolBlock_Battery.Outputs["ResultAll"].Value;
                    }
                    else
                    {
                        _VisionSystem[cam].LedValue[12] = Convert.ToInt32((double)CogToolBlock_Logo.Outputs["HistoValue"].Value);
                        _VisionSystem[cam].LedValue[13] = Convert.ToInt32((double)CogToolBlock_Card.Outputs["HistoValue"].Value);

                        _VisionSystem[cam].LedResult[12] = (bool)CogToolBlock_Logo.Outputs["ResultLogo"].Value;
                        _VisionSystem[cam].LedResult[13] = (bool)CogToolBlock_Card.Outputs["ResultLogo"].Value;
                    }

                    var Record = _VisionSystem[cam].cogToolBlock[index].CreateLastRunRecord();
                    ICogRecord cogRecordAll = Record.SubRecords["CogAffineTransformTool_AllProduct.OutputImage"];
                    ICogRecord cogRecord0 = Record.SubRecords["CogToolBlock_No0.CogAffineTransformTool_Logo.OutputImage"];
                    ICogRecord cogRecord1 = Record.SubRecords["CogToolBlock_No1.CogAffineTransformTool_Logo.OutputImage"];
                    ICogRecord cogRecord2 = Record.SubRecords["CogToolBlock_No2.CogAffineTransformTool_Logo.OutputImage"];
                    ICogRecord cogRecord3 = Record.SubRecords["CogToolBlock_No3.CogAffineTransformTool_Logo.OutputImage"];
                    ICogRecord cogRecord4 = Record.SubRecords["CogToolBlock_No4.CogAffineTransformTool_Logo.OutputImage"];
                    ICogRecord cogRecord5 = Record.SubRecords["CogToolBlock_No5.CogAffineTransformTool_Logo.OutputImage"];
                    ICogRecord cogRecord6 = Record.SubRecords["CogToolBlock_No6.CogAffineTransformTool_Logo.OutputImage"];
                    ICogRecord cogRecord7 = Record.SubRecords["CogToolBlock_No7.CogAffineTransformTool_Logo.OutputImage"];
                    ICogRecord cogRecord8 = Record.SubRecords["CogToolBlock_No8.CogAffineTransformTool_Logo.OutputImage"];
                    ICogRecord cogRecord9 = Record.SubRecords["CogToolBlock_No9.CogAffineTransformTool_Logo.OutputImage"];
                    ICogRecord cogRecordAsterisk = Record.SubRecords["CogToolBlock_Asterisk.CogAffineTransformTool_Logo.OutputImage"];
                    ICogRecord cogRecordSharp = Record.SubRecords["CogToolBlock_Sharp.CogAffineTransformTool_Logo.OutputImage"];
                    ICogRecord cogRecordLogo = Record.SubRecords["CogToolBlock_Logo.CogAffineTransformTool_Logo.OutputImage"];
                    ICogRecord cogRecordCard = Record.SubRecords["CogToolBlock_Card.CogAffineTransformTool_Logo.OutputImage"];
                    ICogRecord cogRecordLock = Record.SubRecords["CogToolBlock_Lock.CogAffineTransformTool_Logo.OutputImage"];
                    ICogRecord cogRecordFinger = Record.SubRecords["CogToolBlock_Finger.CogAffineTransformTool_Logo.OutputImage"];
                    ICogRecord cogRecordBattery = Record.SubRecords["CogToolBlock_Battery.CogAffineTransformTool_Logo.OutputImage"];

                    //Lấy ảnh đầu ra hiển thị
                    leftTabControl.CamDisplay.BackColor = Color.Lime;
                    leftTabControl.CamDisplay.mDisplay.InteractiveGraphics.Clear();
                    //leftTabControl.CamDisplay.mDisplay.Image = OutputImage;
                    if (cogRecordAll != null) leftTabControl.CamDisplay.mDisplay.Record = cogRecordAll;
                    if (IsFitImage) leftTabControl.CamDisplay.mDisplay.Fit(false);

                    //Hiển thị ảnh lên các part
                    if (!GUICamera.UserLighting)
                    {  
                        // No0
                        topControl.VisionDisplay[numPadName.No0].BackColor = _VisionSystem[cam].LedResult[0] ? Color.Lime : Color.Red;
                        topControl.VisionDisplay[numPadName.No0].mDisplay.InteractiveGraphics.Clear();
                        if (cogRecord0 != null) topControl.VisionDisplay[numPadName.No0].mDisplay.Record = cogRecord0;
                        topControl.VisionDisplay[numPadName.No0].mDisplay.Fit(false);

                        // No1
                        topControl.VisionDisplay[numPadName.No1].BackColor = _VisionSystem[cam].LedResult[1] ? Color.Lime : Color.Red;
                        topControl.VisionDisplay[numPadName.No1].mDisplay.InteractiveGraphics.Clear();
                        if (cogRecord1 != null) topControl.VisionDisplay[numPadName.No1].mDisplay.Record = cogRecord1;
                        topControl.VisionDisplay[numPadName.No1].mDisplay.Fit(false);

                        // No2
                        topControl.VisionDisplay[numPadName.No2].BackColor = _VisionSystem[cam].LedResult[2] ? Color.Lime : Color.Red;
                        topControl.VisionDisplay[numPadName.No2].mDisplay.InteractiveGraphics.Clear();
                        if (cogRecord2 != null) topControl.VisionDisplay[numPadName.No2].mDisplay.Record = cogRecord2;
                        topControl.VisionDisplay[numPadName.No2].mDisplay.Fit(false);

                        // No3
                        topControl.VisionDisplay[numPadName.No3].BackColor = _VisionSystem[cam].LedResult[3] ? Color.Lime : Color.Red;
                        topControl.VisionDisplay[numPadName.No3].mDisplay.InteractiveGraphics.Clear();
                        if (cogRecord3 != null) topControl.VisionDisplay[numPadName.No3].mDisplay.Record = cogRecord3;
                        topControl.VisionDisplay[numPadName.No3].mDisplay.Fit(false);

                        // No4
                        topControl.VisionDisplay[numPadName.No4].BackColor = _VisionSystem[cam].LedResult[4] ? Color.Lime : Color.Red;
                        topControl.VisionDisplay[numPadName.No4].mDisplay.InteractiveGraphics.Clear();
                        if (cogRecord4 != null) topControl.VisionDisplay[numPadName.No4].mDisplay.Record = cogRecord4;
                        topControl.VisionDisplay[numPadName.No4].mDisplay.Fit(false);

                        // No5
                        topControl.VisionDisplay[numPadName.No5].BackColor = _VisionSystem[cam].LedResult[5] ? Color.Lime : Color.Red;
                        topControl.VisionDisplay[numPadName.No5].mDisplay.InteractiveGraphics.Clear();
                        if (cogRecord5 != null) topControl.VisionDisplay[numPadName.No5].mDisplay.Record = cogRecord5;
                        topControl.VisionDisplay[numPadName.No5].mDisplay.Fit(false);

                        // No6
                        topControl.VisionDisplay[numPadName.No6].BackColor = _VisionSystem[cam].LedResult[6] ? Color.Lime : Color.Red;
                        topControl.VisionDisplay[numPadName.No6].mDisplay.InteractiveGraphics.Clear();
                        if (cogRecord6 != null) topControl.VisionDisplay[numPadName.No6].mDisplay.Record = cogRecord6;
                        topControl.VisionDisplay[numPadName.No6].mDisplay.Fit(false);

                        // No7
                        topControl.VisionDisplay[numPadName.No7].BackColor = _VisionSystem[cam].LedResult[7] ? Color.Lime : Color.Red;
                        topControl.VisionDisplay[numPadName.No7].mDisplay.InteractiveGraphics.Clear();
                        if (cogRecord7 != null) topControl.VisionDisplay[numPadName.No7].mDisplay.Record = cogRecord7;
                        topControl.VisionDisplay[numPadName.No7].mDisplay.Fit(false);

                        // No8
                        topControl.VisionDisplay[numPadName.No8].BackColor = _VisionSystem[cam].LedResult[8] ? Color.Lime : Color.Red;
                        topControl.VisionDisplay[numPadName.No8].mDisplay.InteractiveGraphics.Clear();
                        if (cogRecord8 != null) topControl.VisionDisplay[numPadName.No8].mDisplay.Record = cogRecord8;
                        topControl.VisionDisplay[numPadName.No8].mDisplay.Fit(false);

                        // No9
                        topControl.VisionDisplay[numPadName.No9].BackColor = _VisionSystem[cam].LedResult[9] ? Color.Lime : Color.Red;
                        topControl.VisionDisplay[numPadName.No9].mDisplay.InteractiveGraphics.Clear();
                        if (cogRecord9 != null) topControl.VisionDisplay[numPadName.No9].mDisplay.Record = cogRecord9;
                        topControl.VisionDisplay[numPadName.No9].mDisplay.Fit(false);
                        // Asterisk
                        topControl.VisionDisplay[numPadName.Asterisk].BackColor = _VisionSystem[cam].LedResult[10] ? Color.Lime : Color.Red;
                        topControl.VisionDisplay[numPadName.Asterisk].mDisplay.InteractiveGraphics.Clear();
                        if (cogRecordAsterisk != null) topControl.VisionDisplay[numPadName.Asterisk].mDisplay.Record = cogRecordAsterisk;
                        topControl.VisionDisplay[numPadName.Asterisk].mDisplay.Fit(false);
                        // Sharp
                        topControl.VisionDisplay[numPadName.Sharp].BackColor = _VisionSystem[cam].LedResult[11] ? Color.Lime : Color.Red;
                        topControl.VisionDisplay[numPadName.Sharp].mDisplay.InteractiveGraphics.Clear();
                        if (cogRecordSharp != null) topControl.VisionDisplay[numPadName.Sharp].mDisplay.Record = cogRecordSharp;
                        topControl.VisionDisplay[numPadName.Sharp].mDisplay.Fit(false);

                        // Lock
                        topControl.VisionDisplay[numPadName.Lock].BackColor = _VisionSystem[cam].LedResult[14] ? Color.Lime : Color.Red;
                        topControl.VisionDisplay[numPadName.Lock].mDisplay.InteractiveGraphics.Clear();
                        if (cogRecordLock != null) topControl.VisionDisplay[numPadName.Lock].mDisplay.Record = cogRecordLock;
                        topControl.VisionDisplay[numPadName.Lock].mDisplay.Fit(false);
                        // Finger
                        topControl.VisionDisplay[numPadName.Finger].BackColor = _VisionSystem[cam].LedResult[15] ? Color.Lime : Color.Red;
                        topControl.VisionDisplay[numPadName.Finger].mDisplay.InteractiveGraphics.Clear();
                        if (cogRecordFinger != null) topControl.VisionDisplay[numPadName.Finger].mDisplay.Record = cogRecordFinger;
                        topControl.VisionDisplay[numPadName.Finger].mDisplay.Fit(false);
                        // Battery
                        topControl.VisionDisplay[numPadName.Battery].BackColor = _VisionSystem[cam].LedResult[16] ? Color.Lime : Color.Red;
                        topControl.VisionDisplay[numPadName.Battery].mDisplay.InteractiveGraphics.Clear();
                        if (cogRecordBattery != null) topControl.VisionDisplay[numPadName.Battery].mDisplay.Record = cogRecordBattery;
                        topControl.VisionDisplay[numPadName.Battery].mDisplay.Fit(false);

                    }
                    else
                    {
                        // Logo
                        topControl.VisionDisplay[numPadName.Logo].BackColor = _VisionSystem[cam].LedResult[12] ? Color.Lime : Color.Red;
                        topControl.VisionDisplay[numPadName.Logo].mDisplay.InteractiveGraphics.Clear();
                        if (cogRecordLogo != null) topControl.VisionDisplay[numPadName.Logo].mDisplay.Record = cogRecordLogo;
                        topControl.VisionDisplay[numPadName.Logo].mDisplay.Fit(false);
                        // Card
                        topControl.VisionDisplay[numPadName.Card].BackColor = _VisionSystem[cam].LedResult[13] ? Color.Lime : Color.Red;
                        topControl.VisionDisplay[numPadName.Card].mDisplay.InteractiveGraphics.Clear();
                        if (cogRecordCard != null) topControl.VisionDisplay[numPadName.Card].mDisplay.Record = cogRecordCard;
                        topControl.VisionDisplay[numPadName.Card].mDisplay.Fit(false);
                    }

                    //ShowCogRecord1(true, CurrentMdisplaySHow, PLC.Write.Word.ResultCam1a.X, PLC.Write.Word.ResultCam1a.Y, PLC.Write.Word.ResultCam1a.R);
                    ////Log
                    //this.Invoke(new Action(() =>
                    //{
                    //    string Cassette = PLC.Read.Auto.Cam1_TableAorB ? "B" : "A";
                    //    WriteLogPC(LogType.Vision, $"Vision{cam}", $"LotNo: {PLC.Read.Word.Cam1_LotNo}, Floor: {PLC.Read.Word.Cam1_FloorIndex}, Cassette: {Cassette}, Result: OK, X:{PLC.Write.Word.ResultCam1a.X}, Y: {PLC.Write.Word.ResultCam1a.Y}, R: {PLC.Write.Word.ResultCam1a.R}");
                    //}));

                    if (IsFitImage) leftTabControl.CamDisplay.mDisplay.Fit(false);

                }
            }
            catch (CogException ex)
            {
                Result = 1;
                //Log
                this.Invoke(new Action(() =>
                {
                    WriteLogPC(LogType.Vision, $"Trigger1", ex.Message);
                }));

            }
            catch (Exception ex)
            {
                Result = 1;
                leftTabControl.CamDisplay.BackColor = Color.Red;
                leftTabControl.CamDisplay.mDisplay.InteractiveGraphics.Clear();
                leftTabControl.CamDisplay.mDisplay.Image = _VisionSystem[cam].imageInfos[index].Image;

                //Log
                this.Invoke(new Action(() =>
                {
                    WriteLogPC(LogType.Vision, $"Trigger1", ex.ToString());
                }));
            }
            _VisionSystem[cam].ResultArray[index] = Result;
            _VisionSystem[cam].ErrorVision = Result == 1;
            return Result;
        }
        public void RefreshCogDisplayImage()
        {
            leftTabControl.CamDisplay.BackColor = Color.Yellow;
            leftTabControl.CamDisplay.mDisplay.InteractiveGraphics.Clear();
            leftTabControl.CamDisplay.mDisplay.Image = null;

            int cam = 0;
            // No0
            topControl.VisionDisplay[numPadName.No0].BackColor = Color.Yellow;
            topControl.VisionDisplay[numPadName.No0].mDisplay.InteractiveGraphics.Clear();
            topControl.VisionDisplay[numPadName.No0].mDisplay.Image = null;

            // No1
            topControl.VisionDisplay[numPadName.No1].BackColor = Color.Yellow;
            topControl.VisionDisplay[numPadName.No1].mDisplay.InteractiveGraphics.Clear();
            topControl.VisionDisplay[numPadName.No1].mDisplay.Image = null;
            // No2
            topControl.VisionDisplay[numPadName.No2].BackColor = Color.Yellow;
            topControl.VisionDisplay[numPadName.No2].mDisplay.InteractiveGraphics.Clear();
            topControl.VisionDisplay[numPadName.No2].mDisplay.Image = null;
            // No3
            topControl.VisionDisplay[numPadName.No3].BackColor = Color.Yellow;
            topControl.VisionDisplay[numPadName.No3].mDisplay.InteractiveGraphics.Clear();
            topControl.VisionDisplay[numPadName.No3].mDisplay.Image = null;
            // No4
            topControl.VisionDisplay[numPadName.No4].BackColor = Color.Yellow;
            topControl.VisionDisplay[numPadName.No4].mDisplay.InteractiveGraphics.Clear();
            topControl.VisionDisplay[numPadName.No4].mDisplay.Image = null;
            // No5
            topControl.VisionDisplay[numPadName.No5].BackColor = Color.Yellow;
            topControl.VisionDisplay[numPadName.No5].mDisplay.InteractiveGraphics.Clear();
            topControl.VisionDisplay[numPadName.No5].mDisplay.Image = null;
            // No6
            topControl.VisionDisplay[numPadName.No6].BackColor = Color.Yellow;
            topControl.VisionDisplay[numPadName.No6].mDisplay.InteractiveGraphics.Clear();
            topControl.VisionDisplay[numPadName.No6].mDisplay.Image = null;
            // No7
            topControl.VisionDisplay[numPadName.No7].BackColor = Color.Yellow;
            topControl.VisionDisplay[numPadName.No7].mDisplay.InteractiveGraphics.Clear();
            topControl.VisionDisplay[numPadName.No7].mDisplay.Image = null;
            // No8
            topControl.VisionDisplay[numPadName.No8].BackColor = Color.Yellow;
            topControl.VisionDisplay[numPadName.No8].mDisplay.InteractiveGraphics.Clear();
            topControl.VisionDisplay[numPadName.No8].mDisplay.Image = null;
            // No9
            topControl.VisionDisplay[numPadName.No9].BackColor = Color.Yellow;
            topControl.VisionDisplay[numPadName.No9].mDisplay.InteractiveGraphics.Clear();
            topControl.VisionDisplay[numPadName.No9].mDisplay.Image = null;
            // Asterisk
            topControl.VisionDisplay[numPadName.Asterisk].BackColor = Color.Yellow;
            topControl.VisionDisplay[numPadName.Asterisk].mDisplay.InteractiveGraphics.Clear();
            topControl.VisionDisplay[numPadName.Asterisk].mDisplay.Image = null;
            // Sharp
            topControl.VisionDisplay[numPadName.Sharp].BackColor = Color.Yellow;
            topControl.VisionDisplay[numPadName.Sharp].mDisplay.InteractiveGraphics.Clear();
            topControl.VisionDisplay[numPadName.Sharp].mDisplay.Image = null;
            // Lock
            topControl.VisionDisplay[numPadName.Lock].BackColor = Color.Yellow;
            topControl.VisionDisplay[numPadName.Lock].mDisplay.InteractiveGraphics.Clear();
            topControl.VisionDisplay[numPadName.Lock].mDisplay.Image = null;
            // Finger
            topControl.VisionDisplay[numPadName.Finger].BackColor = Color.Yellow;
            topControl.VisionDisplay[numPadName.Finger].mDisplay.InteractiveGraphics.Clear();
            topControl.VisionDisplay[numPadName.Finger].mDisplay.Image = null;
            // Battery
            topControl.VisionDisplay[numPadName.Battery].BackColor = Color.Yellow;
            topControl.VisionDisplay[numPadName.Battery].mDisplay.InteractiveGraphics.Clear();
            topControl.VisionDisplay[numPadName.Battery].mDisplay.Image = null;


            // Logo
            topControl.VisionDisplay[numPadName.Logo].BackColor = Color.Yellow;
            topControl.VisionDisplay[numPadName.Logo].mDisplay.InteractiveGraphics.Clear();
            topControl.VisionDisplay[numPadName.Logo].mDisplay.Image = null;
            // Card
            topControl.VisionDisplay[numPadName.Card].BackColor = Color.Yellow;
            topControl.VisionDisplay[numPadName.Card].mDisplay.InteractiveGraphics.Clear();
            topControl.VisionDisplay[numPadName.Card].mDisplay.Image = null;

        }
        /// <summary>
        /// FilmStrip
        /// </summary>
        private CogImageFileTool CIFT_SaveNGFile = new CogImageFileTool();
        public string FirstTimeTrigger = "";
        private Bitmap ImageShowFilmStip = null;
        private bool IsGettingResult = false;
        private void CreatFilmStripOne(int cam, int index, int result, bool Enable)
        {
            if (leftTabControl.CamDisplay.mDisplay == null) return;
            string Datetimenow = DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
            string FileName = $"{formMain.tbSerialNumber.Texts} {Datetimenow}";
            string CamNoString = (cam + 1).ToString();
            string ResultString = result == 2 ? "Pass" : "Fail";
            string FolderName = DateTime.Now.ToString("yyyy-MM-dd");
            string subpathOrigin = formOption.Parameters.CommonPath + $"\\Cam{CamNoString}\\Origin\\{FolderName}\\{formModel.modelParameter.ModelName}\\{ResultString}\\{index}";

            string subpathOriginSeparate = formOption.Parameters.CommonPath + $"\\Cam{CamNoString}\\OriginSeparate\\{FolderName}\\{formModel.modelParameter.ModelName}\\{ResultString}\\{index}";

            string subpathGraphic = formOption.Parameters.CommonPath + $"\\Cam{CamNoString}\\Graphic\\{FolderName}\\{formModel.modelParameter.ModelName}\\{ResultString}\\{index}";

            bool IsExitFolderOrigin = Directory.Exists(subpathOrigin);
            if (!IsExitFolderOrigin) Directory.CreateDirectory(subpathOrigin);

            bool IsExitFolderOriginSeparate = Directory.Exists(subpathOriginSeparate);
            if (!IsExitFolderOriginSeparate) Directory.CreateDirectory(subpathOriginSeparate);

            bool IsExitFolderGraphic = Directory.Exists(subpathGraphic);
            if (!IsExitFolderGraphic) Directory.CreateDirectory(subpathGraphic);

            string thisPathFileOrigin = subpathOrigin + "\\" + FileName + ".jpg";

            string thisPathFileOriginSeparate = subpathOriginSeparate + "\\" + FileName + ".jpg";

            string thisPathFileGraphic = subpathGraphic + "\\" + FileName + ".bmp";

            //SaveImageGraphic(thisPathFileGraphic, cam, index);

            if (formOption.Parameters.Cam[cam].SaveOrigin)
            {
                var thisImage = _VisionSystem[cam].cogToolBlock[0].Inputs["InputImage"].Value as CogImage24PlanarColor;
                if (thisImage != null)
                {
                    CIFT_SaveNGFile = new CogImageFileTool();
                    CIFT_SaveNGFile.InputImage = _VisionSystem[cam].cogToolBlock[0].Inputs["InputImage"].Value as CogImage24PlanarColor;
                    CIFT_SaveNGFile.Operator.Open(thisPathFileOrigin, CogImageFileModeConstants.Write);
                    CIFT_SaveNGFile.Run();
                }
            }


        }
        private void DeleteOldFolderImage(string CommonPathDic)
        {
            try
            {
                string FolderDate = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd");
                string subpathOrigin1 = CommonPathDic + "\\Cam1\\Origin\\" + FolderDate;
                string subpathGraphic1 = CommonPathDic + "\\Cam1\\Graphic\\" + FolderDate;
                if (Directory.Exists(subpathOrigin1)) Directory.Delete(subpathOrigin1, true);
                if (Directory.Exists(subpathGraphic1)) Directory.Delete(subpathGraphic1, true);
            }
            catch (Exception) { }

        }
        static Bitmap bmp_img_Display = null;
        private void SaveImageGraphic(CogRecordDisplay display, string Path, int cam, int index)
        {
            try
            {
                int indexSave = 0;
                indexSave = index;
                if (formOption.Parameters.Cam[cam].SaveGraphic)
                {
                    if (System.IO.File.Exists(Path)) throw new Exception($"{Path}, Graphic Image file already Exists.");
                    if (display.Image == null) throw new Exception($"formMain.CameraVisionDisplays[cam].ListPartOfCam[{index}].mDisplay.Image == null");

                    bmp_img_Display = display.CreateContentBitmap(Cognex.VisionPro.Display.CogDisplayContentBitmapConstants.Display) as Bitmap;
                    ImageShowFilmStip = bmp_img_Display;
                    bmp_img_Display.Save(Path, ImageFormat.Bmp);
                    bmp_img_Display = null;
                }
            }
            catch (AccessViolationException ex)
            {
                //Log
                this.Invoke(new Action(() =>
                {
                    WriteLogPC(LogType.Vision, $"SaveImageGraphic", ex.Message);
                }));
            }
            catch (Exception ex)
            {
                //Log
                this.Invoke(new Action(() =>
                {
                    WriteLogPC(LogType.Vision, $"SaveImageGraphic", ex.Message);
                }));
            }
        }
        private static Color OKColor = Color.FromArgb(57, 196, 129);
        private static Color NGColor = Color.FromArgb(255, 130, 130);
        public void ExportFileExcel(bool Pass)
        {
            try
            {
                string Result = Pass ? "Pass" : "Fail";
                string thisYear = DateTime.Now.ToString("yyyy");
                string thisMonth = DateTime.Now.ToString("MM");
                string thisDay = DateTime.Now.ToString("dd");
                string thisDate = DateTime.Now.ToString("yyyy-MM-dd");
                string thisDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;
                string subPath = $"{formOption.Parameters.CommonPath}\\HistoryData\\{thisYear}\\Month {thisMonth}\\{formModel.modelParameter.ModelName}\\{Result}";
                string pathFile = $"{subPath}\\{thisDate}.csv";
                if (!Directory.Exists(subPath))
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(subPath);
                }
                ExportDataGridViewToCsv(formMain.dataGridView1, pathFile);
            }
            catch (Exception ex)
            {
                //Log
                this.Invoke(new Action(() =>
                {
                    WriteLogPC(LogType.Main, $"ExportExcel", ex.Message);
                }));
            }
        }
        public void ExportDataGridViewToCsv(DataGridView dataGridView, string filePath)
        {
            try
            {
                StringBuilder csvContent = new StringBuilder();

                // Ghi tiêu đề cột
                for (int i = 0; i < dataGridView.ColumnCount; i++)
                {
                    csvContent.Append(dataGridView.Columns[i].HeaderText + ",");
                }
                csvContent.AppendLine();

                // Ghi dữ liệu từng dòng
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (!row.IsNewRow) // Bỏ qua dòng trống cuối cùng
                    {
                        for (int i = 0; i < dataGridView.ColumnCount; i++)
                        {
                            object cellValue = row.Cells[i].Value;
                            csvContent.Append($"\"{cellValue?.ToString()}\""); // Đặt trong dấu ngoặc kép để tránh lỗi dấu phẩy
                            csvContent.Append(",");
                        }
                        csvContent.AppendLine();
                    }
                }

                // Ghi vào file
                System.IO.File.WriteAllText(filePath, csvContent.ToString(), Encoding.UTF8);
                //LOG
                this.Invoke(new Action(() => {
                    WriteLogPC(LogType.Main, "ExportCSV", "Done");
                }));
            }
            catch (Exception ex)
            {
                //LOG
                this.Invoke(new Action(() => {
                    WriteLogPC(LogType.Main, "ExportCSV", ex.Message);
                }));
            }
        }


        public string PadLeftZeros(int Data, int Count)
        {
            string text = "0000000000" + Math.Abs(Data);
            if (Data < 0)
            {
                return "-" + text.Substring(text.Length - Count + 1);
            }
            string result = text.Substring(text.Length - Count);
            return result;
        }
        /// <summary>
        /// Scanner
        /// </summary>
        public void ScannerConnect()
        {
            try
            {
                if (IsTestNoScanner) return;
                if (formOption == null) return;
                if (formOption.Parameters == null) return;
                if (formOption.Parameters.Scanner.PortName == "" || formOption.Parameters.Scanner.PortName == null) return;
                if (ScannerSerial.IsOpen)
                {
                    lbScannerConected.BackColor = Color.IndianRed;
                    ScannerSerial.Close();
                }

                if (!ScannerSerial.IsOpen)
                {

                    ScannerSerial.PortName = formOption.Parameters.Scanner.PortName;
                    ScannerSerial.BaudRate = formOption.Parameters.Scanner.BaudRate;
                    ScannerSerial.DataBits = formOption.Parameters.Scanner.DataBits;
                    Parity sParity = (Parity)Enum.Parse(typeof(Parity), formOption.Parameters.Scanner.Parity, true);
                    StopBits sStopBits = (StopBits)Enum.Parse(typeof(StopBits), formOption.Parameters.Scanner.StopBits, true);
                    ScannerSerial.Parity = sParity;
                    ScannerSerial.StopBits = sStopBits;
                    ScannerSerial.Open();
                    lbScannerConected.BackColor = ScannerSerial.IsOpen ? Color.Green : Color.IndianRed;
                }

            }
            catch (Exception ex)
            {
                //LOG
                this.Invoke(new Action(() => {
                    WriteLogPC(LogType.Main, "Scanner", "Connect Fail");
                }));
                MessageBox.Show(ex.ToString(), "Scanner", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public void ScannerDisconnect()
        {
            if (ScannerSerial.IsOpen)
            {
                ScannerSerial.Close();
            }
            lbScannerConected.BackColor = ScannerSerial.IsOpen ? Color.Green : Color.IndianRed;
        }
        private void Scanner_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string BarcodeReceived = ScannerSerial.ReadLine();

            if (BarcodeReceived.Contains("\r"))
            {
                BarcodeReceived = BarcodeReceived.Replace("\r", "");
            }
            if (BarcodeReceived.Contains("\n"))
            {
                BarcodeReceived = BarcodeReceived.Replace("\n", "");
            }
            CurrentQRCodeRecived = BarcodeReceived;

            //this.Invoke(new EventHandler(DisplayBarcodeRecived));
        }

        /// <summary>
        /// Lock Assa
        /// </summary>
        public void LockConnect()
        {
            try
            {
                if (IstestNoLock) return;
                if (formOption == null) return;
                if (formOption.Parameters == null) return;
                if (formOption.Parameters.Lock.PortName == "" || formOption.Parameters.Lock.PortName == null) return;
                if (LockASSA.Serial.IsOpen)
                {
                    lbLockConnectStatus.BackColor = Color.IndianRed;
                    LockASSA.Serial.Close();
                }

                if (!LockASSA.Serial.IsOpen)
                {

                    LockASSA.Serial.PortName = formOption.Parameters.Lock.PortName;
                    LockASSA.Serial.BaudRate = formOption.Parameters.Lock.BaudRate;
                    LockASSA.Serial.DataBits = formOption.Parameters.Lock.DataBits;
                    Parity sParity = (Parity)Enum.Parse(typeof(Parity), formOption.Parameters.Lock.Parity, true);
                    StopBits sStopBits = (StopBits)Enum.Parse(typeof(StopBits), formOption.Parameters.Lock.StopBits, true);
                    LockASSA.Serial.Parity = sParity;
                    LockASSA.Serial.StopBits = sStopBits;
                    LockASSA.Serial.Open();
                    lbLockConnectStatus.BackColor = LockASSA.Serial.IsOpen ? Color.Green : Color.IndianRed;
                }

            }
            catch (Exception ex)
            {
                //LOG
                this.Invoke(new Action(() => {
                    WriteLogPC(LogType.FFT, "Lock", "Connect Fail");
                }));
                MessageBox.Show(ex.ToString(), "Lock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        /// <summary>
        /// KeySight PSU
        /// </summary>
        public void ConnectKeySightPSU()
        {
            try
            {
               if (formModel.Keysight.btnConnect.BackColor != Color.Green)
                {
                    formModel.Keysight.KeysightPSU = new KeysightPSU();
                    if (formModel.Keysight.KeysightPSU.ScanAndOpen().Item2)
                    {
                        formModel.Keysight.btnConnect.BackColor = Color.Green;
                        formModel.Keysight.btnOffPower_Click(null, null);
                        formModel.Keysight.isConnect = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //LOG
                this.Invoke(new Action(() => {
                    WriteLogPC(LogType.KeySight, "Connect", ex.Message);
                }));
                MessageBox.Show(ex.ToString(), "Connect", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public void KeySightPSU_ON()
        {
            try
            {
                if (!formModel.Keysight.isConnect) throw new Exception("KeySight is Closing");
                formModel.Keysight.btnOnPower_Click(null, null);

            }
            catch (Exception ex)
            {
                //LOG
                this.Invoke(new Action(() => {
                    WriteLogPC(LogType.KeySight, "ON", ex.Message);
                }));
            }

        }
        public void KeySightPSU_Off()
        {
            try
            {
                if (!formModel.Keysight.isConnect) throw new Exception("KeySight is Closing");
                formModel.Keysight.btnOffPower_Click(null, null);

            }
            catch (Exception ex)
            {
                //LOG
                this.Invoke(new Action(() => {
                    WriteLogPC(LogType.KeySight, "Off", ex.Message);
                }));
            }
        }
        public bool KeySightPSU_SetVol(double Vol)
        {
            try
            {
                if (!formModel.Keysight.isConnect) throw new Exception("KeySight is Closing");
               
                if (formModel.Keysight.KeysightPSU.SetVoltage(Vol))
                {
                    string KeysightReadVol_String = formModel.Keysight.KeysightPSU.GetVoltage();
                    formModel.Keysight.Vol_value.Text = KeysightReadVol_String;
                    double KeysightReadVol_double = Convert.ToDouble(KeysightReadVol_String);
                    //if (Vol != KeysightReadVol_double)
                    //    throw new Exception($"Set Voltage fail. SetVol = {Vol}. GetVol = {KeysightReadVol_String}");

                }
                return true;
            }
            catch (Exception ex)
            {

                //LOG
                this.Invoke(new Action(() => {
                    WriteLogPC(LogType.KeySight, "SetVoltage", ex.Message);
                }));
                return false;
            }
        }
        public bool KeySightPSU_SetCurrent(double Current)
        {
            try
            {
                if (!formModel.Keysight.isConnect) throw new Exception("KeySight is Closing");

                if (formModel.Keysight.KeysightPSU.SetCurrent(Current))
                { 

                }
                return true;
            }
            catch (Exception ex)
            {

                //LOG
                this.Invoke(new Action(() => {
                    WriteLogPC(LogType.KeySight, "SetCurrent", ex.Message);
                }));
                return false;
            }
        }


        /// <summary>
        /// Alarm - Log - Error List
        /// </summary>
        private string MyDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
        private void WriteLogError(string LogType, string ErrorCode, string ErrorContent)
        {
            if (formOption == null) return;
            if (formOption.Parameters == null) return;
            if (formOption.Parameters.CommonPath == null) return;
            string DayTime = DateTime.Now.ToString("MM-dd-yyyy HH:mm:ss");
            string DateTimeNowFoder = DateTime.Now.ToString("MM-dd-yyyy");
            string DateTimeNowFile = DateTime.Now.ToString("MM-dd-yyyy HH");
            string thisPath = formOption.Parameters.CommonPath + "\\" + LogType + "Log\\" + DateTimeNowFoder + "\\";
            string thisLogFileName = thisPath + LogType + "Log_" + DateTimeNowFile + ".csv";
            bool IsExitFolderOrigin = Directory.Exists(thisPath);
            if (!IsExitFolderOrigin) Directory.CreateDirectory(thisPath);
            try
            {
                if (!System.IO.File.Exists(thisLogFileName))
                {
                    string header = "STT,DayTime,Code,Content";
                    System.IO.File.WriteAllText(thisLogFileName, header);
                }
                if (System.IO.File.Exists(thisLogFileName))
                {
                    string[] lines = System.IO.File.ReadAllLines(thisLogFileName, Encoding.UTF8);
                    string text = System.IO.File.ReadAllText(thisLogFileName, Encoding.UTF8);
                    int thisStt = 0;
                    if (ErrorCode.Contains("\r"))
                    {
                        ErrorCode = ErrorCode.Replace("\r", " ");
                    }
                    if (ErrorCode.Contains("\n"))
                    {
                        ErrorCode = ErrorCode.Replace("\n", " ");
                    }
                    string thislineupdate = thisStt.ToString() + "," +
                                            DayTime + "," +
                                            ErrorCode + "," +
                                            ErrorContent;
                    text = text + Environment.NewLine + thislineupdate;
                    System.IO.File.WriteAllText(thisLogFileName, text);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

            }

        }
        public void WriteLogPC(LogType logType, string code, string content)
        {
            string DateTimenow = DateTime.Now.ToString("HH:mm:ss");
            ListViewItem item = new ListViewItem(DateTimenow);
            item.SubItems.Add(code);
            item.SubItems.Add(content);
            switch (logType)
            {
                case LogType.Main:
                    {

                        leftTabControl.listViewMainLog.Items.Add(item);
                        leftTabControl.listViewMainLog.Items[leftTabControl.listViewMainLog.Items.Count - 1].EnsureVisible();
                        //if (leftTabControl.listViewMainLog.Items.Count > 20) leftTabControl.listViewMainLog.Items.RemoveAt(0);
                        //leftTabControl.listViewMainLog.Items[leftTabControl.listViewMainLog.Items.Count - 1].EnsureVisible();
                        WriteLogError("MAIN", code, content);
                        break;
                    }
                case LogType.Mes:
                    {
                        leftTabControl.listViewMesLog.Items.Add(item);
                        leftTabControl.listViewMesLog.Items[leftTabControl.listViewMesLog.Items.Count - 1].EnsureVisible();
                        WriteLogError("MES", code, content);
                        break;
                    }
                case LogType.Vision:
                    {
                        leftTabControl.listViewVisionLog.Items.Add(item);
                        leftTabControl.listViewVisionLog.Items[leftTabControl.listViewVisionLog.Items.Count - 1].EnsureVisible();
                        WriteLogError("Vision", code, content);
                        break;
                    }
                case LogType.KeySight:
                    {
                        leftTabControl.listViewKeysightLog.Items.Add(item);
                        leftTabControl.listViewKeysightLog.Items[leftTabControl.listViewKeysightLog.Items.Count - 1].EnsureVisible();
                        WriteLogError("MAIN", code, content);
                        break;
                    }
                case LogType.FFT:
                    {

                        leftTabControl.listViewMainLog.Items.Add(item);
                        leftTabControl.listViewMainLog.Items[leftTabControl.listViewMainLog.Items.Count - 1].EnsureVisible();
                        WriteLogError("MAIN", code, content);
                        break;
                    }
                case LogType.LockSend:
                    {
                        leftTabControl.ListViewDataLog.Items.Add(item);
                        int RowCount = leftTabControl.ListViewDataLog.Items.Count;
                        leftTabControl.ListViewDataLog.Items[RowCount - 1].ForeColor = Color.Blue;
                        leftTabControl.ListViewDataLog.Items[leftTabControl.ListViewDataLog.Items.Count - 1].EnsureVisible();
                        WriteLogError("Lock", code, content);
                        break;
                    }
                case LogType.LockRecived:
                    {
                        leftTabControl.ListViewDataLog.Items.Add(item);
                        int RowCount = leftTabControl.ListViewDataLog.Items.Count;
                        leftTabControl.ListViewDataLog.Items[RowCount - 1].ForeColor = Color.IndianRed;
                        leftTabControl.ListViewDataLog.Items[leftTabControl.ListViewDataLog.Items.Count - 1].EnsureVisible();
                        WriteLogError("Lock", code, content);
                        break;
                    }
                default: { break; }

            }
        }
        private string[,] ReadErrorListContent(int index)
        {
            string[,] result = new string[2, 300];
            try
            {
                string PathFileParameter = $"D:\\InitialData\\ErrorListPLC{index}.csv";
                if (!System.IO.File.Exists(PathFileParameter)) return null;
                string[] lines = System.IO.File.ReadAllLines(PathFileParameter, Encoding.UTF8);
                if (lines.Length == 0) return null;
                ErrorListCount[index] = lines.Length;
                result = new string[2, lines.Length];
                for (int i = 0; i < lines.Length; i++)
                {
                    result[0, i] = lines[i].Split(',')[0];
                    result[1, i] = lines[i].Split(',')[1];
                }
                IsReadErrorList[index] = true;
            }
            catch (IOException ex)
            {
                result = null;
                MessageBox.Show(ex.ToString(), "Read Error List", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }
        private void UpdateErrorListPLC1()
        {
            if (!IsReadErrorList[0]) return;

            bool IsErrorPLCTrigger = false;
            bool IsErrorEmpty = true;
            for (int i = 0; i < 14; i++)
            {
                if (PLC.Read.Auto.ErrorWord1[i] != 0)
                {
                    IsErrorEmpty = false;
                }
                if (CurrentErrorWord1[i] != PLC.Read.Auto.ErrorWord1[i])
                {
                    IsErrorPLCTrigger = true;
                }
            }
            if (IsErrorPLCTrigger && !IsErrorEmpty)
            {
                for (int i = 0; i < ErrorListCount[0]; i++)
                {
                    if (PLC.Read.Auto.ErrorList1[i])
                    {
                        //LOG
                        this.Invoke(new Action(() => {
                            WriteLogPC(LogType.Main, ErrorListContent1[0, i], ErrorListContent1[1, i]);
                        }));
                    }
                }
                for (int i = 0; i < 14; i++)
                {
                    CurrentErrorWord1[i] = PLC.Read.Auto.ErrorWord1[i];
                }
            }
        }
        private void UpdateErrorListPLC2()
        {
            if (!IsReadErrorList[1]) return;

            bool IsErrorPLCTrigger = false;
            bool IsErrorEmpty = true;
            for (int i = 0; i < 14; i++)
            {
                if (PLC.Read.Auto.ErrorWord2[i] != 0)
                {
                    IsErrorEmpty = false;
                }
                if (CurrentErrorWord2[i] != PLC.Read.Auto.ErrorWord2[i])
                {
                    IsErrorPLCTrigger = true;
                }
            }
            if (IsErrorPLCTrigger && !IsErrorEmpty)
            {
                for (int i = 0; i < ErrorListCount[1]; i++)
                {
                    if (PLC.Read.Auto.ErrorList2[i])
                    {
                        //LOG
                        this.Invoke(new Action(() => {
                            WriteLogPC(LogType.Main, ErrorListContent2[0, i], ErrorListContent2[1, i]);
                        }));
                    }
                }
                for (int i = 0; i < 14; i++)
                {
                    CurrentErrorWord2[i] = PLC.Read.Auto.ErrorWord2[i];
                }
            }
        }
        public void ControlIconInvoke(Control control, string Text, Color Backcolor, Color ForeColor)
        {
            if (control == null) return;
            this.Invoke(new Action(delegate {
                if (Text != null)
                    control.Text = Text;
                if (Backcolor != null)
                    control.BackColor = Backcolor;
                if (ForeColor != null)
                    control.ForeColor = ForeColor;
            }));


        }
        private void btnStop_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Door Test
        /// </summary>
        #region API Connect
        private void UpdateUI(Action updateAction)
        {
            if (InvokeRequired)
            {
                Invoke(updateAction);
                LINE_INFOR = line + "_" + station_name;
            }
            else
            {
                updateAction();
            }
        }
        public async void GetPOInformation()
        {
            if (formMain !=null)
            {
                try
                {
                    (bool result, string data) response = await apiMes.getProductInfoApi(LINE_INFOR);
                    if (response.result == false)
                    {
                        throw new Exception(response.data);
                    }
                    GetProductInfoResponse productInfoResponse = new GetProductInfoResponse();
                    productInfoResponse = JsonSerializer.Deserialize<GetProductInfoResponse>(response.data);

                    _productInfo = productInfoResponse;

                    //lbWO.Text = $"WO: {_productInfo.data.WONumber}";
                    //lbPO.Text = $"PO: {_productInfo.data.PO.PONumber}";
                    No_WO = _productInfo.data.WONumber;
                    formMain.tbWO.Text = No_WO;
                    formMain.tbSoPo.Text = _productInfo.data.PO.PONumber;
                    //lbSkuCode.Text = $"SKU: {_productInfo.data.Product.SkuCode}";
                    formMain.txtSKUCode.Text = _productInfo.data.Product.SkuCode;
                    formMain.lblTongWo.Text = $"Tổng WO: {_productInfo.data.TotalQuantity}";
                    Total_WO = _productInfo.data.TotalQuantity;
                    Count_PO();

                    (bool result, string msg, ProductLabelResponse data) _productLabel;
                    _productLabel = await apiMes.getProductLabel(productInfoResponse.data.Product.SkuCode, station_name);
                    if (!_productLabel.result)
                    {
                        throw new Exception(_productLabel.msg);
                    }

                    // Open Template File in ProductLabel (table)
                    //foreach (var item in _productLabel.data.results)
                    //{
                    //    if (engine == null)
                    //        continue;

                    //    //MessageBox.Show($"PrkioductLabel: {item.TemplateFile}");
                    //    string tmp_file_path = "";
                    //    // Download file label
                    //    tmp_file_path = _Mes.DownloadFileByUrl(item.TemplateFile);
                    //    // Load Label to Engine

                    //    if (item.Printer != null && item.Printer != "") // Have config Printer
                    //        format = engine.Documents.Open(tmp_file_path, item.Printer);    // Load to Engine
                    //    else
                    //    {
                    //        format = engine.Documents.Open(tmp_file_path, cb_Printer.SelectedItem.ToString());

                    //    }
                    //}

                }
                catch (Exception ex)
                {
                    int line = (new StackTrace(ex, true)).GetFrame(0).GetFileLineNumber();
                    MessageBox.Show($"[GetPOInformation][ERROR][Line {line}] {ex.Message}", "GetPOInformation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Application.Exit();
                }
            }
            else
            {
                Load_WO_Offline();
                //lbWO.Text = $"WO: {No_WO}";
                //Total_WO = frmLogin.Mode.Q_ty;
                formMain.lblTongWo.Text = $"Total WO: {Total_WO}";
                formMain.lblDaDat.Text = $"Passed: {Passed}";
                Remaining_WO = Total_WO - Passed;
                formMain.lblConlai.Text = $"Remaining: {Remaining_WO}";
                formMain.lblWO.Text = $"WO: {No_WO}";
                //lb_name_machine.Text = $"{Frm_Login.Mode.Location} Final Functions Test";
                //lbPO.Text = $"PO: {_productInfo.data.PO.PONumber}";
                //No_WO = _productInfo.data.WONumber;
                //lbSkuCode.Text = $"SKU: {_productInfo.data.Product.SkuCode}";


            }

        }
        private void Load_WO_Offline()
        {
            try
            {
                //string _WO = WOFile.IniReadValue("Previos", "Work_Order");
                //No_WO = _WO;
                //Total_WO = int.Parse(WOFile.IniReadValue(_WO, "Total"));
                //Passed = int.Parse(WOFile.IniReadValue(_WO, "Passed"));
                No_WO = "";
                Total_WO = 0;
                Passed = 0;
            }
            catch { }
        }
        private async void Count_WO()
        {

            (bool result, string data) Data;
            //string Station_ID = Global.LINE_INFO;
            Data = await apiMes.getCompleteQtyByStationApi(LINE_INFOR, No_WO);
            int count = Regex.Matches(Data.data, "LPO").Count;
            Log.Information(count.ToString());
            formMain.lblDaDat.Text = $"Đã Đạt: {count}";
            formMain.lblConlai.Text = $"Còn Lại: {(Total_WO - count)}";

        }
        private async void Count_PO()
        {
            if (formMain!= null)
            {

                (bool result, string data) Data;
                string Station_ID = LINE_INFOR;
                Data = await apiMes.getCompleteQtyByStationApi(Station_ID, No_WO);
                int pre = Regex.Matches(Data.data, ",").Count;
                int count = pre - 2;
                Log.Information(count.ToString());
                UpdateUI(() =>
                {
                    formMain.lblDaDat.Text = $"Đã Đạt: {count}";
                    formMain.lblConlai.Text = $"Còn Lại: {(Total_WO - count)}";
                    //if (English)
                    //{
                    //    lb_passed.Text = $"Passed: {count}";
                    //    lb_remaining.Text = $"Remaining: {(Total_WO - count)}";
                    //}
                    //else
                    //{
                        
                    //}
                });
            }
            else
            {
                WOFile.IniWriteValue(No_WO, "Passed", Passed.ToString());
                Remaining_WO = Total_WO - Passed;
                formMain.lblDaDat.Text = $"Đã Đạt: {Passed}";
                formMain.lblConlai.Text = $"Còn Lại: {Remaining_WO}";
                //if (English)
                //{
                //    lb_passed.Text = $"Passed: {Passed}";
                //    lb_remaining.Text = $"Remaining: {Remaining_WO}";
                //}
                //else
                //{
                   
                //}
            }

        }
        #endregion
        public ClassDoorTestData CurrentDoorTestData = new ClassDoorTestData();
        public string CurrentSerial_Number = "dacb1";
        public string CurrentQRCodeRecived = "123";
        
        
    }
}
