using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using Cognex.VisionPro.Implementation.Internal;
using LibFunction;
using Newtonsoft.Json.Linq;
using ScottPlot.Drawing.Colormaps;
using SLTSoft.RJControl;
using SLTtechSoft.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SLTtechSoft.Form1;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace SLTtechSoft
{
    public partial class Main : Form
    {
        Form1 _form1;

        public FunctionDoorClass functionDoorList = new FunctionDoorClass();
        public ClassDoorTestData CurrentDoorTestData = new ClassDoorTestData();
        public FFTTableColumnClass DoorTableCol = new FFTTableColumnClass();
        public FFTResultClass DoorResult = new FFTResultClass();
        public LabelStatus labelStatus = new LabelStatus();
        public Stopwatch StopWatchTestDoor = new Stopwatch();
        public Stopwatch StopWatchTestDoorTotal = new Stopwatch();
        public string[] ListItemStopWhenFail = new string[]
        {
            "Check_Voltage",
            "Check_Current",
            "Power_On",
        };
        //Delay Process 
        private int CountDelayProcess = 0;
        private int DelayProcessIndex = 0;
        private bool StartDelayProcess = false;
        private bool DelayProcessDone = false;
        //
        public int ProcessTestIndex = 0;
        public int DelayKeysightPowerOn = 10;
        private int CountKeySightPowerOn = 0;
        public int TestRetryTime = 0;
        public bool StopTestingByFail = false;
        public bool StartTesting = false;

        public Main()
        {
            InitializeComponent();

        }
        public void InitializeUI(Form1 obj)
        {
            _form1 = obj;

            if (_form1.formOption == null) return;
            if (_form1.formOption.Parameters == null) return;

            //_form1.formModel.ShowAModelParameter(_form1.formModel.modelParameter);
            tbSerialNumber.textBox1.KeyDown += tbSerialNumber_Keydown;
        }

        private void Main_Shown(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.WindowState = FormWindowState.Maximized;
        }


        private void btnskip_Click(object sender, EventArgs e)
        {
            ProcessTestIndex = 4;
        }
        private void tbSerialNumber_Keydown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string inputData = tbSerialNumber.Texts;
                tbSerialNumber.Texts = "";
                if (inputData == "") return;
                _form1.CurrentSerial_Number = inputData;

            }
        }
        public void FinishATest(bool Result, string Value)
        {
            dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Value].Value = Value;
            if (Result)
            {
                //Pass
                dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value = DoorResult.Pass;
            }
            else
            {
                //Fail
                dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value = DoorResult.Fail;
            }
          
            //ShowTime
            StopWatchTestDoor.Stop();
            TimeSpan ts = StopWatchTestDoor.Elapsed;
            string TimeTest = $"{ts.Seconds}.{_form1.PadLeftZeros(ts.Milliseconds, 3)}";

            dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Time].Value = TimeTest;

            ProcessTestIndex = 0;

        }
        public void DelayProcess(int TimeDelay)
        {
            if (!StartDelayProcess)
            {
                CountDelayProcess = 0;
                DelayProcessIndex = 0;
                DelayProcessDone = false;
            }

            switch (DelayProcessIndex)
            {
                case 0:
                    {
                        if (StartDelayProcess)
                        {
                            CountDelayProcess = 0;
                            DelayProcessIndex++;
                        }
                        break;
                    }
                case 1:
                    {
                        if (CountDelayProcess++ >= TimeDelay)
                        {
                            DelayProcessIndex++;
                        }
                        break;
                    }
                case 2:
                    {
                        DelayProcessDone = true;
                        break;
                    }
                default:
                    {

                        break;
                    }
            }

        }
        //Chu trình Kiểm tra Khóa cửa
        public void DoorTestProcess()
        {

            if (_form1.MachineMode != ModeMachine.Auto) return;
            if (!_form1.PLC.Read.Auto.Test.StartProcessTest) _form1.PLC.Write.Auto.Test.FinishProcessTest = false;

            if (!StartTesting)
            {
                Test_SerialNumber();
            }
            else
            {
                if (ProcessTestIndex == 0) {
                    CurrentDoorTestData = CheckTestTable();
                }
               
                Test_Front_QR();
                Test_Main_QR();
                Test_Check_Voltage();
                Test_Check_Current();
                Test_Power_On();
                Test_Load_Parameter();
                Test_Firmwave_Check();
                Test_Set_Default();
                Test_Set_RTC();
                Test_Get_RTC();
                Test_Check_Flash();
                Test_Door_Position_Sensor_Check_Close();
                Test_Door_Position_Sensor_Check_Auto_Lock();
                Test_Door_Position_Sensor_Check_Open();
                Test_Door_Position_Sensor_Check_Alarm();
                Test_Broken_Check_Disconnect();
                Test_Broken_Check_Alarm();
                Test_Speaker_Check();
                Test_Key_Check();
                Test_Led_Check();
                //Test_Label_Check();
                Test_Fingerprint_Check_Contact_1();
                Test_Fingerprint_Check_Touch_1();
                Test_RF_Card_Check();
                Test_Button_Check_Register_P();
                Test_Button_Check_Register_N();
                Test_Button_Check_Lock_P();
                Test_Button_Check_Lock_N();
                Test_Motor_Check_Close();
                Test_Motor_Check_Mortise_Close();
                Test_Motor_Check_Open();
                Test_Motor_Check_Mortise_Open();
                Test_ADC_Check();
                Test_IDE_Curent_Check();
                Test_Reset_defaut();
                Test_Check_Stuck();
                Test_9V_Battery();
                Test_9V_Battery_REV();
            }
            
        }
        //Quét bảng kết quả kiểm tra để biết hạng mục đang test là bạng mục nào
        public ClassDoorTestData CheckTestTable()
        {
           
            ClassDoorTestData classDoorTestData = new ClassDoorTestData();
            bool isFindOutFunctionTest = false;
            bool TotalResult = true;
            if (dataGridView1.ColumnCount == 0) return null;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value != null)
                {
                    string thisID = dataGridView1.Rows[i].Cells[DoorTableCol.ID].Value.ToString();
                    string thisResult = "";
                    string thisEnable = "";
                    if (thisID.Contains("FFT") && !StopTestingByFail)
                    {

                        if (dataGridView1.Rows[i].Cells[DoorTableCol.Result].Value == null) thisResult = "";
                        else
                        {
                            thisResult = dataGridView1.Rows[i].Cells[DoorTableCol.Result].Value.ToString();
                            if (thisResult == DoorResult.Fail)
                            {
                                TotalResult = false;
                            }
                        }
                        if (dataGridView1.Rows[i].Cells[DoorTableCol.Enable].Value == null) thisEnable = "";
                        else
                        {
                            thisEnable = dataGridView1.Rows[i].Cells[DoorTableCol.Enable].Value.ToString();
                        }
                        if (thisResult == DoorResult.Pass)
                        {
                            dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Green;
                        }
                        else
                        {
                            if (thisResult == DoorResult.Fail)
                            {
                                dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                                //Kiểm tra xem Chức năng có thuộc danh sách dừng Check Khi Fail hay không
                                //Nếu phải thì cho dừng Kiểm tra
                                //for (int k = 0; k <= ListItemStopWhenFail.Length; k++)
                                //{
                                //    if (dataGridView1.Rows[i].Cells[1].Value.ToString() == ListItemStopWhenFail[k])
                                //    {
                                //        StopTestingByFail = false;
                                //        break;
                                //    }
                                //}
                                //if (StopTestingByFail) break;
                            }
                            else
                            {
                                if (thisResult == "" && thisEnable != "No")
                                {
                                    if (ProcessTestIndex == 0)
                                    {
                                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                                        classDoorTestData.RowIndex = i;
                                        classDoorTestData.ID = dataGridView1.Rows[i].Cells[0].Value.ToString();
                                        classDoorTestData.Name = dataGridView1.Rows[i].Cells[1].Value.ToString();
                                        classDoorTestData.Detail = dataGridView1.Rows[i].Cells[2].Value.ToString();
                                        classDoorTestData.Value = dataGridView1.Rows[i].Cells[3].Value.ToString();
                                        classDoorTestData.Min = dataGridView1.Rows[i].Cells[4].Value.ToString();
                                        classDoorTestData.Max = dataGridView1.Rows[i].Cells[5].Value.ToString();
                                        classDoorTestData.Unit = dataGridView1.Rows[i].Cells[6].Value.ToString();
                                        classDoorTestData.Time = dataGridView1.Rows[i].Cells[7].Value.ToString();
                                        classDoorTestData.Result = dataGridView1.Rows[i].Cells[8].Value.ToString();
                                        classDoorTestData.Enable = dataGridView1.Rows[i].Cells[9].Value.ToString() != "No";
                                        classDoorTestData.retry = Convert.ToInt32(dataGridView1.Rows[i].Cells[10].Value.ToString());
                                        classDoorTestData.TimeOut = Convert.ToInt32(dataGridView1.Rows[i].Cells[11].Value.ToString());
                                    }
                                  
                                    isFindOutFunctionTest = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            //Nếu là dòng cuối cùng. Tổng hợp kết quả
            if (!isFindOutFunctionTest || StopTestingByFail)
            {
                classDoorTestData.RowIndex = dataGridView1.RowCount - 3 ;
                classDoorTestData.ID = "...Final";
                classDoorTestData.Name = "Result...";
                //ghi kết quả vào ô Final Result
                if (TotalResult && !StopTestingByFail)
                {
                    dataGridView1.Rows[dataGridView1.RowCount - 3].Cells[DoorTableCol.Value].Value = DoorResult.Pass;
                    _form1.leftTabControl.SetlStatus(labelStatus.Pass);
                }
                else
                {
                    dataGridView1.Rows[dataGridView1.RowCount - 3].Cells[DoorTableCol.Value].Value = DoorResult.Fail;
                    _form1.leftTabControl.SetlStatus(labelStatus.Fail);
                }

                //Bắt đầu đếm thời gian tổng
                StopWatchTestDoorTotal.Stop();
                TimeSpan ts = StopWatchTestDoor.Elapsed;
                string TimeTest = $"{ts.Seconds}.{_form1.PadLeftZeros(ts.Milliseconds, 3)}";
                //Ghi lại thời gian kiểm tra tổng.
                dataGridView1.Rows[dataGridView1.RowCount - 2].Cells[DoorTableCol.Time].Value = TimeTest;
                //Xuất file CSV
                _form1.ExportFileExcel(TotalResult);
                //Kết thúc chu trình
                _form1.PLC.Write.Auto.Test.FinishProcessTest = true;
                StartTesting = false;
            }
            //Hiện thị bước đang kiểm tra
            _form1.leftTabControl.lbProcessStatus.Text = $"{CurrentDoorTestData.ID}:{CurrentDoorTestData.Name}:{ProcessTestIndex}";
            
            return classDoorTestData;
        }
        //FFT-0.0 Serial_Number
        public void Test_SerialNumber()
        {
            //if (!CurrentDoorTestData.Enable) return;
            //if (CurrentDoorTestData.Name != functionDoorList.Serial_Number.Name) return;


            _form1.leftTabControl.SetlStatus(labelStatus.Wait);
            StopTestingByFail = false;
            if (_form1.CurrentSerial_Number != "")
            {
                //Khởi tạo các trạng thái kiểm tra
                _form1.formModel.ShowFFTTableInMainScreen();
                StartTesting = true;
                ProcessTestIndex = 0;
                _form1.RefreshCogDisplayImage();

                //Bắt đầu đếm thời gian kiểm thử 1 công đoạn
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                //Bắt đầu đếm thời gian tổng
                StopWatchTestDoorTotal = new Stopwatch();
                StopWatchTestDoorTotal.Start();

                dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Value].Value = _form1.CurrentSerial_Number;
                dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value = DoorResult.Pass;

                _form1.CurrentSerial_Number = "";

                StopWatchTestDoor.Stop();
                TimeSpan ts = StopWatchTestDoor.Elapsed;
                string TimeTest = $"{ts.Seconds}.{_form1.PadLeftZeros(ts.Milliseconds, 3)}";

                dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Time].Value = TimeTest;
                


            }
           



        }
        private DataKey datakeyRead;
        private CheckLockInputAddstatus DataLockread;
        public void Test_Front_QR()
        {
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Front_QR.Name) return;
            if (_form1.CurrentQRCodeRecived != "")
            {
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Value].Value = _form1.CurrentQRCodeRecived;
                dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value = DoorResult.Pass;
                //_form1.CurrentQRCodeRecived = "";

                StopWatchTestDoor.Stop();
                TimeSpan ts = StopWatchTestDoor.Elapsed;
                string TimeTest = $"{ts.Seconds}.{_form1.PadLeftZeros(ts.Milliseconds, 3)}";

               dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Time].Value = TimeTest;
            }
        }
        public void Test_Main_QR()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Main_QR.Name) return;

            //Execute
            if (_form1.CurrentQRCodeRecived != "")
            {
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Value].Value = _form1.CurrentQRCodeRecived;
                dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value = DoorResult.Pass;
                _form1.CurrentQRCodeRecived = "";

                //ShowTime
                StopWatchTestDoor.Stop();
                TimeSpan ts = StopWatchTestDoor.Elapsed;
                string TimeTest = $"{ts.Seconds}.{_form1.PadLeftZeros(ts.Milliseconds, 3)}";

                dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Time].Value = TimeTest;
            }
        }
        public void Test_Check_Voltage()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Check_Voltage.Name) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();
            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            } 
            //Execute
            switch (ProcessTestIndex)
            {
                case 1: 
                    {
                        bool SetVolDone = false, SetCurrentDone = false;
                        _form1.PLC.Write.Auto.Test.StartCheckVoltage6V = true;
                        if (_form1.formModel.Keysight.KeysightPSU.PowerOn && _form1.formModel.Keysight.KeysightPSU.CurrentVoltage == _form1.formModel.modelParameter.KeySight.nudVoltage6V)
                        {
                            ProcessTestIndex = 4;
                        }
                        else
                        {
                            SetVolDone = _form1.KeySightPSU_SetVol(_form1.formModel.modelParameter.KeySight.nudVoltage6V);
                            if (SetVolDone)
                                SetCurrentDone = _form1.KeySightPSU_SetCurrent(_form1.formModel.modelParameter.KeySight.nudCurrent6V);

                            if (SetVolDone && SetCurrentDone) ProcessTestIndex++;


                        }
                        break;
                    }
                case 2:
                    {
                        if (_form1.PLC.Read.Auto.Test.ReadyToOpenPower6V)
                        {
                            _form1.KeySightPSU_ON();
                            CountKeySightPowerOn = 0;
                            ProcessTestIndex++;
                        }
                        break;
                    }
                case 3:
                    {
                        if (CountKeySightPowerOn++ > DelayKeysightPowerOn)
                        {
                            ProcessTestIndex++;
                        }
                        break;
                    }
                case 4:
                    {
                        if (_form1.PLC.Read.Auto.Test.ReadyToOpenPower6V)
                        {
                            string Vol_string = _form1.formModel.Keysight.KeysightPSU.GetVoltage();
                            _form1.formModel.Keysight.Vol_value.Text = Vol_string;
                            double Vol = Convert.ToDouble(Vol_string);
                            double Max = Convert.ToDouble(CurrentDoorTestData.Max);
                            double Min = Convert.ToDouble(CurrentDoorTestData.Min);

                           
                            if (Vol >= Min && Vol <= Max)
                            {
                                FinishATest(true, Vol.ToString());

                            }
                            else
                            {
                                if (TestRetryTime++ >= CurrentDoorTestData.retry)
                                {
                                    FinishATest(false, Vol.ToString());
                                }
                            }

                        }
                        
                        break;
                    }
                

                default: { break; }
            }
        }
        public void Test_Check_Current()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Check_Current.Name) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();
            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute
            switch (ProcessTestIndex)
            {
                case 1:
                    {
                       
                        _form1.PLC.Write.Auto.Test.StartCheckVoltage6V = true;
                        if (_form1.formModel.Keysight.KeysightPSU.PowerOn && _form1.formModel.Keysight.KeysightPSU.CurrentVoltage == _form1.formModel.modelParameter.KeySight.nudVoltage6V)
                        {
                            ProcessTestIndex = 4;
                        }
                        else
                        {
                            bool SetVolDone = false, SetCurrentDone = false;
                            SetVolDone = _form1.KeySightPSU_SetVol(_form1.formModel.modelParameter.KeySight.nudVoltage6V);
                            if (SetVolDone)
                                SetCurrentDone = _form1.KeySightPSU_SetCurrent(_form1.formModel.modelParameter.KeySight.nudCurrent6V);

                            if (SetVolDone && SetCurrentDone) ProcessTestIndex++;


                        }
                        break;
                    }
                case 2:
                    {
                        if (_form1.PLC.Read.Auto.Test.ReadyToOpenPower6V)
                        {
                            _form1.KeySightPSU_ON();
                            CountKeySightPowerOn = 0;
                            ProcessTestIndex++;
                        }
                        break;
                    }
                case 3:
                    {
                        if (CountKeySightPowerOn++ > DelayKeysightPowerOn)
                        {
                            ProcessTestIndex++;
                        }
                        break;
                    }
                case 4:
                    {
                        if (_form1.PLC.Read.Auto.Test.ReadyToOpenPower6V)
                        {
                            string Cur_string = _form1.formModel.Keysight.KeysightPSU.GetCurrent();
                            _form1.formModel.Keysight.Current_value.Text = Cur_string;
                            double Cur = Convert.ToDouble(Cur_string);
                            double Max = Convert.ToDouble(CurrentDoorTestData.Max);
                            double Min = Convert.ToDouble(CurrentDoorTestData.Min);

                           
                            if (Cur >= Min && Cur <= Max)
                            {
                                FinishATest(true, Cur.ToString());

                                
                            }
                            else
                            {
                                if (TestRetryTime++ >= CurrentDoorTestData.retry)
                                {
                                    FinishATest(false, Cur.ToString());
                                   
                                }
                            }

                        }
                        break;
                    }

                default: { break; }
            }
        }
        public void Test_Power_On()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Power_On.Name) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();
            //kiểm tra lò xo. lấy dữ liệu lò xo từ plc
            string ResultSpringOne = _form1.PLC.Read.Auto.Test.ResultSpringOne ? "1" : "0";
            string ResultSpringTwo = _form1.PLC.Read.Auto.Test.ResultSpringTwo ? "1" : "0";
            string ResultSpringThree = _form1.PLC.Read.Auto.Test.ResultSpringThree ? "1" : "0";
            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute
           
            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        //kiểm tra Power On
                        PowerOn powerOn = _form1.LockASSA.ReadPowerOn(CurrentDoorTestData.TimeOut);
                        if (powerOn == null) return;
                        if (powerOn.Power_On)
                        {
                            
                            string Value = $"1-{ResultSpringOne}-{ResultSpringTwo}-{ResultSpringThree}";
                            //if (powerOn.Power_On)
                            //{
                            //    FinishATest(true, Value);
                            //}
                            //else
                            //{
                            //    FinishATest(false, Value);
                            //}
                            if (_form1.PLC.Read.Auto.Test.ResultSpringOne && _form1.PLC.Read.Auto.Test.ResultSpringTwo && _form1.PLC.Read.Auto.Test.ResultSpringThree)
                            {
                                FinishATest(true, Value);
                                _form1.PLC.Write.Auto.Test.StartCheckVoltage6V = false;
                                powerOn.Power_On = false;
                            }
                            else
                            {
                                FinishATest(false, Value);
                                _form1.PLC.Write.Auto.Test.StartCheckVoltage6V = false;
                                powerOn.Power_On = false;
                            }
                        }
                        else
                        {
                            ProcessTestIndex++;
                        }
                        break;
                    }
                case 2:
                    {
                        // khi không thấy PowerOn
                        if (TestRetryTime++ >= CurrentDoorTestData.retry)
                        {
                           

                            //Khi Vượt quá số lân Retry
                            string Value = $"0-{ResultSpringOne}-{ResultSpringTwo}-{ResultSpringThree}";
                            FinishATest(false, Value);
                        }
                        else
                        {
                            //Khi Chưa Vượt quá số lân Retry
                            ProcessTestIndex++;
                        }
                        break;
                    }
                case 3:
                    {
                        //Ngắt nguồn KeysightPSU
                        //_form1.KeySightPSU_Off();
                        StartDelayProcess = true;
                        ProcessTestIndex++;
                        break;
                    }
                case 4:
                    {
                        //Chờ 1 giây
                        DelayProcess(3);
                        if (DelayProcessDone)
                        {
                            StartDelayProcess = false;
                            ProcessTestIndex++;
                        }
                        break;
                    }
                case 5:
                    {
                        //Bật nguồn trở lại
                        //_form1.KeySightPSU_ON();
                        StartDelayProcess = true;
                        ProcessTestIndex++;
                        break;
                    }
                case 6:
                    {
                        //Chờ 1 giây
                        DelayProcess(3);
                        if (DelayProcessDone)
                        {
                            StartDelayProcess = false;
                            ProcessTestIndex++;
                        }
                        break;
                    }
                case 7:
                    {
                        //Quay trở lại check 1 lần nữa
                        ProcessTestIndex = 1;
                        break;
                    }
                default: { break; }
            }
        }
        public void Test_Load_Parameter()
        {

            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Load_Parameter.Name) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();
           
            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute

            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        _form1.LockASSA.initializationLock(CurrentDoorTestData.TimeOut);
                        if (  TestRetryTime < CurrentDoorTestData.retry)
                        {
                            TestRetryTime = 0;
                            ProcessTestIndex++;
                        }
                        else
                        {
                            if (TestRetryTime++ > CurrentDoorTestData.retry)
                            {
                                ProcessTestIndex = 3;
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        ReadParameterDoor CheckParameterDoor = _form1.LockASSA.CheckParameterDoor(CurrentDoorTestData.TimeOut);
                        if (CheckParameterDoor != null && TestRetryTime < CurrentDoorTestData.retry)
                        {

                            FinishATest(true, "");
                        }
                        else
                        {
                            if (TestRetryTime++ > CurrentDoorTestData.retry)
                            {
                                ProcessTestIndex = 3;

                            }
                        }
                        break;
                    }
                case 3:
                    {
                        FinishATest(false, "Null");
                        break;
                    }
               
                default: { break; }
            }
        }

        public void Test_Firmwave_Check()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Firmwave_Check.Name) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute

            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        Readversion readVersion = _form1.LockASSA.readVersion(CurrentDoorTestData.TimeOut);
                        if (readVersion != null && TestRetryTime < CurrentDoorTestData.retry)
                        {
                            FinishATest(true, readVersion.version);
                        }
                        else
                        {
                            if (TestRetryTime++ > CurrentDoorTestData.retry)
                            {
                                ProcessTestIndex = 2;

                            }
                        }
                        break;
                    }
                case 2:
                    {
                        FinishATest(false, "Null");
                        break;
                    }
                default: { break; }
            }
        }
        public void Test_Set_Default()
        {
            //Condition 
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Set_Default.Name) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute
            
            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        try 
                        {
                            string hexString = CurrentDoorTestData.Min;
                            string hexValue = hexString.Substring(2);
                            byte result = Convert.ToByte(hexValue,16);
                            SetdeafautLock SetDefaultDoor = _form1.LockASSA.SetDefaultDoor(result, CurrentDoorTestData.TimeOut);
                            if (SetDefaultDoor != null)
                            {
                                FinishATest(true, SetDefaultDoor.temperature);
                            }
                            else
                            {
                                if (TestRetryTime++ > CurrentDoorTestData.retry)
                                {
                                    FinishATest(false, "Null");
                                }
                            }
                        }
                        catch (Exception ex) 
                        {
                            FinishATest(false, "Error");
                            StopTestingByFail = true;
                            //LOG
                            this.Invoke(new Action(() => {
                                _form1.WriteLogPC(LogType.Main, "SetDefaultDoor", ex.Message);
                            }));

                        }
                        break;
                    }
               
                default: { break; }
            }
        }
        public void Test_Set_RTC()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Set_RTC.Name) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute

            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        try
                        {
                            string dateString = CurrentDoorTestData.Min;
                            //DateTime date = DateTime.ParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture);
                            //// Tách các phần của chuỗi
                            //string year = dateString.Substring(2, 2); // Lấy 2 số cuối của năm (23)
                            //string month = dateString.Substring(4, 2); // Lấy tháng (10)
                            //string day = dateString.Substring(6, 2); // Lấy ngày (11)
                            //string DayOfWeek = date.DayOfWeek.ToString(); // Lấy thứ của ngày trên

                            RTC Set_RTC = _form1.LockASSA.SetRTC(dateString, CurrentDoorTestData.TimeOut);
                            if (Set_RTC != null)
                            {
                                FinishATest(true, Set_RTC.Data_Set_RTC);
                            }
                            else
                            {
                                FinishATest(false, "Null");
                            }

                        }
                        catch (Exception ex)
                        {
                            FinishATest(false, "Error");
                            StopTestingByFail = true;
                            //LOG
                            this.Invoke(new Action(() => {
                                _form1.WriteLogPC(LogType.Main, "Set_RTC", ex.Message);
                            }));
                        }
                        break;
                    }
               
                default: { break; }
            }
        }
            public void Test_Get_RTC()
            {
                //Condition
                if (!CurrentDoorTestData.Enable) return;
                if (CurrentDoorTestData.Name != functionDoorList.Get_RTC.Name) return;
                string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

                if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
                {
                    TestRetryTime = 0;
                    //CountTime
                    StopWatchTestDoor = new Stopwatch();
                    StopWatchTestDoor.Start();

                    ProcessTestIndex = 1;
                }
                //Execute

                switch (ProcessTestIndex)
                {
                    case 1:
                        {
                            try
                            {
                                //string dateString = CurrentDoorTestData.Min;
                                //DateTime date = DateTime.ParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture);
                                //// Tách các phần của chuỗi
                                //string year = dateString.Substring(2, 2); // Lấy 2 số cuối của năm (23)
                                //string month = dateString.Substring(4, 2); // Lấy tháng (10)
                                //string day = dateString.Substring(6, 2); // Lấy ngày (11)
                                //string DayOfWeek = date.DayOfWeek.ToString(); // Lấy thứ của ngày trên

                                RTC Get_RTC = _form1.LockASSA.GetRTC(16);
                                if (Get_RTC != null)
                                {
                                    FinishATest(true, Get_RTC.Data_Get_RTC);
                                }
                                else
                                {
                                    FinishATest(false, "Null");
                                }
                            }
                            catch (Exception ex)
                            {
                                FinishATest(false, "Error");
                                StopTestingByFail = true;
                                //LOG
                                this.Invoke(new Action(() => {
                                    _form1.WriteLogPC(LogType.Main, "Get_RTC", ex.Message);
                                }));
                            }
                            break;
                        }

                    default: { break; }
                }
            }
        public void Test_Check_Flash()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Check_Flash.Name) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute

            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        try
                        {
                            string hexString = CurrentDoorTestData.Min;
                            byte result = Convert.ToByte(hexString.Replace("0x", ""), CurrentDoorTestData.TimeOut);
                            SetdeafautLock Check_flash = _form1.LockASSA.SetDefaultDoor(result, CurrentDoorTestData.TimeOut);
                            
                            if (Check_flash.checkFlash != null)
                            {
                                FinishATest(true, hexString);
                            }
                            else
                            {
                                FinishATest(false, "Null");
                            }
                        }
                        catch (Exception ex)
                        {
                            FinishATest(false, "Error");
                            StopTestingByFail = true;
                            //LOG
                            this.Invoke(new Action(() => {
                                _form1.WriteLogPC(LogType.Main, "Check_Flash", ex.Message);
                            }));
                        }
                        break;
                    }
                default: { break; }
            }
        }
       
        public void Test_Door_Position_Sensor_Check_Close()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Door_Position_Sensor_Check_Close.Name ||
                CurrentDoorTestData.Detail != functionDoorList.Door_Position_Sensor_Check_Close.Detail
                ) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute

            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        _form1.PLC.Write.Auto.Test.StartCheckDoorPositionSensor_Close = true;
                        ProcessTestIndex++;
                        break;
                    }
                case 2:
                    {
                        if (_form1.PLC.Read.Auto.Test.ReadyCheckDoorPositionSensor_Close)
                        {
                            datakeyRead = _form1.LockASSA.checkDataKey(CurrentDoorTestData.TimeOut);
                            DataLockread = _form1.LockASSA.CheckInputADoor(CurrentDoorTestData.TimeOut);
                            if (datakeyRead == null)
                            {
                                //Fail
                                ProcessTestIndex++;
                                //LOG
                                this.Invoke(new Action(() => {
                                    _form1.WriteLogPC(LogType.Main, "checkDataKey", "Null");
                                }));
                            }
                            if (datakeyRead != null)
                            {
                                if (datakeyRead.SourceUsed == "")
                                {
                                    //Fail
                                    ProcessTestIndex++;
                                    //LOG
                                    this.Invoke(new Action(() => {
                                        _form1.WriteLogPC(LogType.Main, "checkDataKey", "dataKey.SourceUsed = Empty");
                                    }));
                                }
                                else
                                {
                                    if (datakeyRead.SensorDoor == "Close")
                                    {
                                        //Pass
                                        ProcessTestIndex = 4;
                                    }
                                    else
                                    {
                                        //Fail
                                        ProcessTestIndex++;
                                    }
                                }
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        //Khi lỗi xảy ra, báo cho PLC kiểm tra lại
                        //_form1.PLC.Write.Auto.Test.StartCheckDoorPositionSensor_Close = false;
                        if (TestRetryTime++ < CurrentDoorTestData.retry)
                        {
                            ProcessTestIndex = 1;
                        }
                        else
                        {
                            //kết thúc kiểm tra Fail
                            if (datakeyRead != null)
                                FinishATest(false, datakeyRead.SensorDoor);
                            else
                                FinishATest(false, "Error");
                        }
                        break;
                    }
                case 4:
                    {
                        //kết thúc kiểm tra Pass
                        _form1.PLC.Write.Auto.Test.StartCheckDoorPositionSensor_Close = false;
                        if (!_form1.PLC.Read.Auto.Test.ReadyCheckDoorPositionSensor_Close)
                        {
                            FinishATest(true, "Close");
                        }
                        break;
                    }
                default: { break; }
            }
        }
        private ReadParameterDoor readParameterDoor;
        public void Test_Door_Position_Sensor_Check_Auto_Lock()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Door_Position_Sensor_Check_Auto_Lock.Name ||
                CurrentDoorTestData.Detail != functionDoorList.Door_Position_Sensor_Check_Auto_Lock.Detail
                ) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute

            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        _form1.PLC.Write.Auto.Test.StartCheckDoorPositionSensor_Auto_Lock = true;
                        ProcessTestIndex++;
                        break;
                    }
                case 2:
                    {
                        if (_form1.PLC.Read.Auto.Test.ReadyCheckDoorPositionSensor_Auto_Lock)
                        {
                            readParameterDoor = _form1.LockASSA.CheckParameterDoor(CurrentDoorTestData.TimeOut);
                            DataLockread = _form1.LockASSA.CheckInputADoor(CurrentDoorTestData.TimeOut);
                            if (readParameterDoor == null)
                            {
                                //LOG
                                this.Invoke(new Action(() => {
                                    _form1.WriteLogPC(LogType.Main, "CheckParameterDoor", "Null");
                                }));

                            }
                            if (readParameterDoor != null)
                            {
                                if (readParameterDoor.R_autolock && readParameterDoor.L_autolock)
                                {
                                    //Pass
                                    ProcessTestIndex = 4;
                                }
                                else
                                {
                                    //Fail
                                    ProcessTestIndex++;
                                }
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        //Khi lỗi xảy ra, báo cho PLC kiểm tra lại
                        _form1.PLC.Write.Auto.Test.StartCheckDoorPositionSensor_Auto_Lock = false;
                        if (!_form1.PLC.Read.Auto.Test.ReadyCheckDoorPositionSensor_Auto_Lock)
                        {
                            if (TestRetryTime++ < CurrentDoorTestData.retry)
                            {
                                ProcessTestIndex = 0;
                            }
                            else
                            {
                                //kết thúc kiểm tra Fail
                                if (readParameterDoor != null)
                                    FinishATest(false, $"{readParameterDoor.R_autolock}-{readParameterDoor.L_autolock}");
                                else
                                    FinishATest(false, "Error");
                            }
                        }
                        break;
                    }
                case 4:
                    {
                        //kết thúc kiểm tra Pass
                        _form1.PLC.Write.Auto.Test.StartCheckDoorPositionSensor_Auto_Lock = false;
                        if (!_form1.PLC.Read.Auto.Test.ReadyCheckDoorPositionSensor_Auto_Lock)
                        {
                            FinishATest(true, "Auto_Lock");
                        }
                        break;
                    }

                default: { break; }
            }
        }
        public void Test_Door_Position_Sensor_Check_Open()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Door_Position_Sensor_Check_Open.Name ||
                CurrentDoorTestData.Detail != functionDoorList.Door_Position_Sensor_Check_Open.Detail
                ) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute

            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        _form1.PLC.Write.Auto.Test.StartCheckDoorPositionSensor_Open = true;
                        ProcessTestIndex++;
                        break;
                    }
                case 2:
                    {
                        if (_form1.PLC.Read.Auto.Test.ReadyCheckDoorPositionSensor_Open)
                        {
                            datakeyRead = _form1.LockASSA.checkDataKey(CurrentDoorTestData.TimeOut);
                            DataLockread = _form1.LockASSA.CheckInputADoor(CurrentDoorTestData.TimeOut);
                            if (datakeyRead == null)
                            {
                                //Fail
                                ProcessTestIndex++;
                                //LOG
                                this.Invoke(new Action(() => {
                                    _form1.WriteLogPC(LogType.Main, "checkDataKey", "Null");
                                }));
                            }
                            if (datakeyRead != null)
                            {
                                if (datakeyRead.SourceUsed == "")
                                {
                                    //Fail
                                    ProcessTestIndex++;
                                    //LOG
                                    this.Invoke(new Action(() => {
                                        _form1.WriteLogPC(LogType.Main, "checkDataKey", "dataKey.SourceUsed = Empty");
                                    }));
                                }
                                else
                                {
                                    if (datakeyRead.SensorDoor == "Open")
                                    {
                                        //Pass
                                        ProcessTestIndex = 4;
                                    }
                                    else
                                    {
                                        //Fail
                                        ProcessTestIndex++;
                                    }
                                }
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        //Khi lỗi xảy ra, báo cho PLC kiểm tra lại
                        _form1.PLC.Write.Auto.Test.StartCheckDoorPositionSensor_Open = false;
                        if (!_form1.PLC.Read.Auto.Test.ReadyCheckDoorPositionSensor_Open)
                        {
                            if (TestRetryTime++ < CurrentDoorTestData.retry)
                            {
                                ProcessTestIndex = 0;
                            }
                            else
                            {
                                //kết thúc kiểm tra Fail
                                if (datakeyRead != null)
                                    FinishATest(false, datakeyRead.SensorDoor);
                                else
                                    FinishATest(false, "Error");
                            }
                        }
                        break;
                    }
                case 4:
                    {
                        //kết thúc kiểm tra Pass
                        _form1.PLC.Write.Auto.Test.StartCheckDoorPositionSensor_Open = false;
                        if (!_form1.PLC.Read.Auto.Test.ReadyCheckDoorPositionSensor_Open)
                        {
                            FinishATest(true, "Open");
                        }
                        break;
                    }

                default: { break; }
            }
        }
        public void Test_Door_Position_Sensor_Check_Alarm()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Door_Position_Sensor_Check_Alarm.Name ||
                CurrentDoorTestData.Detail != functionDoorList.Door_Position_Sensor_Check_Alarm.Detail
                ) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute

            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        _form1.formModel.FftAnalysis.Start();
                        ProcessTestIndex++;
                        break;
                    }
                case 2:
                    {
                        double min = Convert.ToDouble(CurrentDoorTestData.Min);
                        double max = Convert.ToDouble(CurrentDoorTestData.Max);

                        if (_form1.formModel.FftAnalysis.PeakPower >= min && _form1.formModel.FftAnalysis.PeakPower <= max)
                        {
                            FinishATest(true, _form1.formModel.FftAnalysis.PeakPower.ToString());
                        }
                        else
                        {
                            if (TestRetryTime++ < CurrentDoorTestData.retry)
                            {
                                ProcessTestIndex = 1;
                            }
                            else
                            {
                                FinishATest(false, _form1.formModel.FftAnalysis.PeakPower.ToString());
                            }
                        }
                        break;
                    }
                default: { break; }
            }
        }
        public void Test_Broken_Check_Disconnect()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Broken_Check_Disconnect.Name ||
                CurrentDoorTestData.Detail != functionDoorList.Broken_Check_Disconnect.Detail
                ) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute

            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        _form1.PLC.Write.Auto.Test.Broken_Disconnect = true;
                        ProcessTestIndex++;
                        break;
                    }
                case 2:
                    {
                        if (!_form1.PLC.Read.Auto.Test.Broken_Disconnect)
                        {
                            if (TestRetryTime++ < CurrentDoorTestData.retry)
                            {
                                ProcessTestIndex = 0;
                            }
                            else
                            {
                                    FinishATest(false, "False");
                            }
                        }
                        else
                        {
                            ProcessTestIndex = 3;
                        }
                        break;
                    }
                case 3:
                    {
                        if (_form1.PLC.Read.Auto.Test.Broken_Disconnect)
                        {
                            FinishATest(true, "True");
                        }
                        else
                        {
                            FinishATest(false, "false");
                        }
                        break;
                    }
                default: { break; }
            }
        }
        public void Test_Broken_Check_Alarm()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Broken_Check_Alarm.Name ||
                CurrentDoorTestData.Detail != functionDoorList.Broken_Check_Alarm.Detail
                ) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute

            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        StartDelayProcess = true;
                        ProcessTestIndex++;
                        break;
                    }
                case 2:
                    {
                        DelayProcess(50);
                        if (DelayProcessDone)
                        {
                            StartDelayProcess = false;
                            _form1.formModel.FftAnalysis.Start();
                            ProcessTestIndex++;
                        }
                        break;
                    }
                case 3:
                    {
                        StartDelayProcess = true;
                        ProcessTestIndex++;

                        break;
                    }
                case 4:
                    {
                        DelayProcess(20);
                        if (DelayProcessDone)
                        {
                            StartDelayProcess = false;
                            ProcessTestIndex++;
                        }
                        break;
                    }
                case 5:
                    {
                        Alarm alarm = _form1.LockASSA.WarningOnOff(WarningType.Off, CurrentDoorTestData.TimeOut);
                        
                        if (alarm.DataAlarm_GetTrue)
                        {
                            ProcessTestIndex++;
                        }
                        else
                        {
                            StopTestingByFail = true;
                            //LOG
                            this.Invoke(new Action(() => {
                                _form1.WriteLogPC(LogType.Main, "WarningOn", "Fail");
                            }));
                        }
                        break;
                    }
                case 6:
                    {
                        double min = Convert.ToDouble(CurrentDoorTestData.Min);
                        double max = Convert.ToDouble(CurrentDoorTestData.Max);

                        if (_form1.formModel.FftAnalysis.PeakPower >= min && _form1.formModel.FftAnalysis.PeakPower <= max)
                        {
                            FinishATest(true, _form1.formModel.FftAnalysis.PeakPower.ToString());
                        }
                        else
                        {
                            if (TestRetryTime++ < CurrentDoorTestData.retry)
                            {
                                ProcessTestIndex = 1;
                            }
                            else
                            {
                                FinishATest(false, _form1.formModel.FftAnalysis.PeakPower.ToString());
                            }
                           
                        }
                       
                        _form1.PLC.Write.Auto.Test.Broken_Disconnect = false;
                        break;
                    }
               
                default: { break; }
            }
        }
        public void Test_Speaker_Check()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Speaker_Check.Name ||
                CurrentDoorTestData.Detail != functionDoorList. Speaker_Check.Detail
                ) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute

            switch (ProcessTestIndex)
            {

                case 1:
                    {
                        Speak Speaker = _form1.LockASSA.Speaker(SpeakerType.Low, CurrentDoorTestData.TimeOut);
                        if (Speaker == null)
                        {
                            StopTestingByFail = true;
                            //LOG
                            this.Invoke(new Action(() => {
                                _form1.WriteLogPC(LogType.Main, "Speaker", "null");
                            }));
                        }
                        if (Speaker != null)
                        {
                            if (!Speaker.DataSpeak_GetTrue)
                            {
                                StopTestingByFail = true;
                                //LOG
                                this.Invoke(new Action(() => {
                                    _form1.WriteLogPC(LogType.Main, "Speaker", "Fail");
                                }));

                            }
                            else
                            {
                                ProcessTestIndex++;
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        _form1.formModel.FftAnalysis.Start();
                        StartDelayProcess = true;
                        ProcessTestIndex++;
                        break;
                    }
                case 3:
                    {
                        DelayProcess(30);
                        if (DelayProcessDone)
                        {
                            StartDelayProcess = false;
                            ProcessTestIndex++;
                        }
                        break;
                    }
                case 4:
                    {
                        double min = Convert.ToDouble(CurrentDoorTestData.Min);
                        double max = Convert.ToDouble(CurrentDoorTestData.Max);

                        if (_form1.formModel.FftAnalysis.PeakPower >= min && _form1.formModel.FftAnalysis.PeakPower <= max)
                        {
                            FinishATest(true, _form1.formModel.FftAnalysis.PeakPower.ToString());
                        }
                        else
                        {
                            if (TestRetryTime++ < CurrentDoorTestData.retry)
                            {
                                ProcessTestIndex = 2;
                            }
                            else
                            {
                                FinishATest(false, _form1.formModel.FftAnalysis.PeakPower.ToString());
                            }
                        }
                        
                        break;
                    }
               
                
                default: { break; }
            }
        }
        private int CheckKeyPressIndex = 0;
        
        public void Test_Key_Check()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            bool IsInFunctionList = false;
            for (int i = 0; i < functionDoorList.KeyChecks.Length; i++)
            {
                if (CurrentDoorTestData.Name == functionDoorList.KeyChecks[i].Name &&
                    CurrentDoorTestData.Detail == functionDoorList.KeyChecks[i].Detail
                   )
                {
                    IsInFunctionList = true;
                    CheckKeyPressIndex = i;
                }
            }
            if (!IsInFunctionList) return;

            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute

            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        _form1.PLC.Write.Auto.Test.StartCheckPress[CheckKeyPressIndex] = true;
                        ProcessTestIndex++;
                        break;
                    }
                case 2:
                    {
                        if (_form1.PLC.Read.Auto.Test.ReadyCheckPress[CheckKeyPressIndex])
                        {
                            datakeyRead = _form1.LockASSA.checkDataKey(CurrentDoorTestData.TimeOut);
                            DataLockread = _form1.LockASSA.CheckInputADoor(CurrentDoorTestData.TimeOut);
                            if (datakeyRead == null)
                            {
                                //Fail
                                ProcessTestIndex++;
                                //LOG
                                this.Invoke(new Action(() => {
                                    _form1.WriteLogPC(LogType.Main, "checkDataKey", "Null");
                                }));
                            }
                            if (datakeyRead != null)
                            {
                                if (datakeyRead.SourceUsed == "")
                                {
                                    //Fail
                                    ProcessTestIndex++;
                                    //LOG
                                    this.Invoke(new Action(() => {
                                        _form1.WriteLogPC(LogType.Main, "checkDataKey", "dataKey.SourceUsed = Empty");
                                    }));
                                }
                                else
                                {
                                    if (datakeyRead.Key == CurrentDoorTestData.Detail)
                                    {
                                        //Pass
                                        ProcessTestIndex = 4;
                                    }
                                    else
                                    {
                                        //Fail
                                        ProcessTestIndex++;
                                    }
                                }
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        //Khi lỗi xảy ra, báo cho PLC kiểm tra lại

                        if (TestRetryTime++ < CurrentDoorTestData.retry)
                        {
                            ProcessTestIndex = 1;
                        }
                        else
                        {
                            //kết thúc kiểm tra Fail
                            if (datakeyRead != null)
                                FinishATest(false, datakeyRead.Key);
                            else
                                FinishATest(false, "Error");
                        }

                        break;
                    }
                case 4:
                    {
                        //kết thúc kiểm tra Pass
                        _form1.PLC.Write.Auto.Test.StartCheckPress[CheckKeyPressIndex] = false;
                        if (!_form1.PLC.Read.Auto.Test.ReadyCheckPress[CheckKeyPressIndex])
                        {
                            FinishATest(true, CurrentDoorTestData.Min);
                        }
                        break;
                    }
                default: { break; }
            }
        }
        public bool IsLedDoorOn = false;
        public void Test_Led_Check()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != "Led_Check")
            {
                _form1._VisionSystem[0].FinishTrigger = false;
            }
            bool IsInFunctionList = false;
            for (int i = 0; i < functionDoorList.LedChecks.Length; i++)
            {
                
                if (CurrentDoorTestData.Name == functionDoorList.LedChecks[i].Name &&
                    CurrentDoorTestData.Detail == functionDoorList.LedChecks[i].Detail
                   )
                {
                    IsInFunctionList = true;
                    CheckKeyPressIndex = i;
                }
            }
            if (!IsInFunctionList) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute

            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        _form1.PLC.Write.Auto.Test.StartLedCheck = true;
                        if (!IsLedDoorOn)
                        {
                            _form1.LockASSA.LEDKeyOn(CurrentDoorTestData.TimeOut);
                            _form1.GUICamera.UserLighting = true;
                            IsLedDoorOn = true;

                        }
                        if (_form1.PLC.Read.Auto.Test.ReadyLedCheck)
                        {
                            _form1.PLC.Write.Auto.Test.StartLedCheck = false;
                            ProcessTestIndex++;
                        }
                       
                        break;
                    }
                case 2:
                    {
                        if (!_form1._VisionSystem[0].FinishTrigger) _form1.GUICamera.StartSingleShotGrabbing();
                        ProcessTestIndex++;
                        break;
                    }
                case 3:
                    {
                        if (_form1._VisionSystem[0].FinishTrigger)
                        {
                            if (_form1._VisionSystem[0].LedResult[CheckKeyPressIndex])
                            {
                                datakeyRead = _form1.LockASSA.checkDataKey(CurrentDoorTestData.TimeOut);
                                DataLockread = _form1.LockASSA.CheckInputADoor(CurrentDoorTestData.TimeOut);
                                int Result1 = _form1._VisionSystem[0].LedValue[CheckKeyPressIndex];
                                int Result2 = _form1._VisionSystem[0].LedValue[CheckKeyPressIndex];
                                int Min1 = Convert.ToInt32(CurrentDoorTestData.Min.Split('-')[0]);
                                int Min2 = Convert.ToInt32(CurrentDoorTestData.Min.Split('-')[1]);
                                int Max1 = Convert.ToInt32(CurrentDoorTestData.Max.Split('-')[0]);
                                int Max2 = Convert.ToInt32(CurrentDoorTestData.Max.Split('-')[1]);

                                if (Result1 >= Min1 && Result1 <= Max1 && Result2 >= Min2 && Result2 <= Max2)
                                {
                                    //Pass
                                    FinishATest(true, _form1._VisionSystem[0].LedValue[CheckKeyPressIndex].ToString());

                                }
                                else
                                {
                                    //Fail;
                                    
                                   
                                    ProcessTestIndex = 4;
                                }
                            }
                            else
                            {
                                //Fail;
                                
                                ProcessTestIndex = 4;
                               
                            }
                        }
                        
                        break;
                    }
                case 4:
                    {
                        //Xử lý chụp lại khi NG
                        //delay nhung chup van bi mo
                        if (TestRetryTime++ < CurrentDoorTestData.retry)
                        {
                            _form1._VisionSystem[0].FinishTrigger = false;
                            _form1.LockASSA.LEDKeyOn(CurrentDoorTestData.TimeOut);
                            StartDelayProcess = true;
                            DelayProcess(30);
                            if (DelayProcessDone)
                            {
                                StartDelayProcess = false;
                            }
                            ProcessTestIndex = 2;
                        }
                        else
                        {
                            FinishATest(false, _form1._VisionSystem[0].LedValue[CheckKeyPressIndex].ToString());
                        }
                        break;
                    }
                
                default: { break; }
            }
        }
        private int ChecklabelIndex = 0;
        public void Test_Label_Check()
        {
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != "Label_Check")
            {
                _form1._VisionSystem[0].FinishTrigger = false;
            }
            bool IsInFunctionList = false;
            for (int i = 0; i < functionDoorList.labelCheck.Length; i++)
            {

                if (CurrentDoorTestData.Name == functionDoorList.labelCheck[i].Name &&
                    CurrentDoorTestData.Detail == functionDoorList.labelCheck[i].Detail
                   )
                {
                    IsInFunctionList = true;
                    ChecklabelIndex = i;
                }
            }
            if (!IsInFunctionList) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute

            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        _form1.PLC.Write.Auto.Test.StartLedCheck = true;
                        if (!IsLedDoorOn)
                        {

                            IsLedDoorOn = true;

                        }
                        if (_form1.PLC.Read.Auto.Test.ReadyLedCheck)
                        {
                            _form1.PLC.Write.Auto.Test.StartLedCheck = false;
                            _form1.GUICamera.UserLighting = true;
                            ProcessTestIndex++;
                        }

                        break;
                    }
                case 2:
                    {
                        if (!_form1._VisionSystem[0].FinishTrigger) _form1.GUICamera.StartSingleShotGrabbing();
                        ProcessTestIndex++;
                        break;
                    }
                case 3:
                    {
                        if (_form1._VisionSystem[0].FinishTrigger)
                        {
                            if (_form1._VisionSystem[0].LedResult[CheckKeyPressIndex])
                            {
                                datakeyRead = _form1.LockASSA.checkDataKey(CurrentDoorTestData.TimeOut);
                                DataLockread = _form1.LockASSA.CheckInputADoor(CurrentDoorTestData.TimeOut);
                                int Result1 = _form1._VisionSystem[0].LedValue[CheckKeyPressIndex];
                                int Result2 = _form1._VisionSystem[0].LedValue[CheckKeyPressIndex];
                                int Min1 = Convert.ToInt32(CurrentDoorTestData.Min.Split('-')[0]);
                                int Min2 = Convert.ToInt32(CurrentDoorTestData.Min.Split('-')[1]);
                                int Max1 = Convert.ToInt32(CurrentDoorTestData.Max.Split('-')[0]);
                                int Max2 = Convert.ToInt32(CurrentDoorTestData.Max.Split('-')[1]);

                                if (Result1 >= Min1 && Result1 <= Max1 && Result2 >= Min2 && Result2 <= Max2)
                                {
                                    //Pass
                                    FinishATest(true, _form1._VisionSystem[0].LedValue[CheckKeyPressIndex].ToString());

                                }
                                else
                                {
                                    //Fail;


                                    ProcessTestIndex = 4;
                                }
                            }
                            else
                            {
                                //Fail;

                                ProcessTestIndex = 4;

                            }
                        }

                        break;
                    }
                case 4:
                    {
                        //Xử lý chụp lại khi NG
                        //delay nhung chup van bi mo
                        if (TestRetryTime++ < CurrentDoorTestData.retry)
                        {
                            _form1._VisionSystem[0].FinishTrigger = false;
                            ProcessTestIndex = 2;
                        }
                        else
                        {
                            FinishATest(false, _form1._VisionSystem[0].LedValue[CheckKeyPressIndex].ToString());
                        }
                        break;
                    }

                default: { break; }
            }
        }
        public void Test_Fingerprint_Check_Contact_1()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Fingerprint_Check_Contact_1.Name ||
                CurrentDoorTestData.Detail != functionDoorList.Fingerprint_Check_Contact_1.Detail
                ) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute
            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        _form1.PLC.Write.Auto.Test.StartFingerprint_Check_Contact_1 = true;
                        ProcessTestIndex++;
                        break;
                    }
                case 2:
                    {
                        if (_form1.PLC.Read.Auto.Test.ReadyFingerprint_Check_Contact_1)
                        {
                            Password_Card_Figer data = _form1.LockASSA.ConfirmFingerPrint(CurrentDoorTestData.TimeOut);
                            if (data == null)
                            {
                                //Fail
                                ProcessTestIndex++;
                                //LOG
                                this.Invoke(new Action(() => {
                                    _form1.WriteLogPC(LogType.Main, "checkDataKey", "Null");
                                }));
                            }
                            if (data != null)
                            {
                                if (data.DataFingerPrint_GetTrue == false )
                                {
                                    //Fail
                                    ProcessTestIndex++;
                                    //LOG
                                    this.Invoke(new Action(() => {
                                        _form1.WriteLogPC(LogType.Main, "checkDataKey", "dataKey.SourceUsed = Empty");
                                    }));
                                }
                                else
                                {
                                    if (data.DataFingerPrint_GetTrue == true)
                                    {
                                        //Pass
                                        ProcessTestIndex = 4;
                                    }
                                    else
                                    {
                                        //Fail
                                        ProcessTestIndex++;
                                    }
                                }
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        //Khi lỗi xảy ra, báo cho PLC kiểm tra lại
                        _form1.PLC.Write.Auto.Test.StartFingerprint_Check_Contact_1 = false;
                        if (!_form1.PLC.Read.Auto.Test.ReadyFingerprint_Check_Contact_1)
                        {
                            if (TestRetryTime++ < CurrentDoorTestData.retry)
                            {
                                ProcessTestIndex = 0;
                            }
                            else
                            {
                                //kết thúc kiểm tra Fail
                                if (readParameterDoor != null)
                                    FinishATest(false, datakeyRead.fingerprintLid.ToString());
                                else
                                    FinishATest(false, "Error");
                            }
                        }
                        break;
                       
                    }
                case 4:
                    {
                        //kết thúc kiểm tra Pass
                        _form1.PLC.Write.Auto.Test.StartFingerprint_Check_Contact_1 = false;
                        if (!_form1.PLC.Read.Auto.Test.ReadyFingerprint_Check_Contact_1)
                        {
                            FinishATest(true, CurrentDoorTestData.Min);
                        }
                        break;
                    }
                default: { break; }
            }
        }
        public void Test_Fingerprint_Check_Touch_1()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Fingerprint_Check_Touch_1.Name ||
                CurrentDoorTestData.Detail != functionDoorList.Fingerprint_Check_Touch_1.Detail
                ) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute
            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        _form1.PLC.Write.Auto.Test.StartFingerprint_Check_Touch_1 = true;
                        ProcessTestIndex++;
                        break;
                    }
                case 2:
                    {
                        if (_form1.PLC.Read.Auto.Test.ReadyFingerprint_Check_Touch_1)
                        {
                            datakeyRead = _form1.LockASSA.checkDataKey(CurrentDoorTestData.TimeOut);
                            if (datakeyRead == null)
                            {
                                //Fail
                                ProcessTestIndex++;
                                //LOG
                                this.Invoke(new Action(() => {
                                    _form1.WriteLogPC(LogType.Main, "checkDataKey", "Null");
                                }));
                            }
                            if (datakeyRead != null)
                            {
                                if (datakeyRead.fingerprintLid == "")
                                {
                                    //Fail
                                    ProcessTestIndex++;
                                    //LOG
                                    this.Invoke(new Action(() => {
                                        _form1.WriteLogPC(LogType.Main, "checkDataKey", "dataKey.SourceUsed = Empty");
                                    }));
                                }
                                else
                                {
                                    if (datakeyRead.fingerprintLid == "Close")
                                    {
                                        //Pass
                                        ProcessTestIndex = 4;
                                    }
                                    else
                                    {
                                        //Fail
                                        ProcessTestIndex++;
                                    }
                                }
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        //Khi lỗi xảy ra, báo cho PLC kiểm tra lại
                       
                        if (TestRetryTime++ < CurrentDoorTestData.retry)
                        {
                            ProcessTestIndex = 1;
                        }
                        else
                        {
                            //kết thúc kiểm tra Fail
                            if (datakeyRead != null)
                                FinishATest(false, datakeyRead.fingerprintLid);
                            else
                                FinishATest(false, "Error");
                        }
                        break;
                    }
                case 4:
                    {
                        //kết thúc kiểm tra Pass
                        _form1.PLC.Write.Auto.Test.StartFingerprint_Check_Touch_1 = false;
                        if (!_form1.PLC.Read.Auto.Test.ReadyFingerprint_Check_Touch_1)
                        {
                            FinishATest(true, CurrentDoorTestData.Min);
                        }
                        break;
                    }
                default: { break; }
            }
        }

        public void Test_RF_Card_Check()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.RF_Card_Check.Name ||
                CurrentDoorTestData.Detail != functionDoorList.RF_Card_Check.Detail
                ) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute
            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        _form1.PLC.Write.Auto.Test.StartRF_Card_Check = true;
                        ProcessTestIndex++;
                        break;
                    }
                case 2:
                    {
                        if (_form1.PLC.Read.Auto.Test.ReadyRF_Card_Check)
                        {
                            Password_Card_Figer  data = _form1.LockASSA.ConfirmCard(CurrentDoorTestData.TimeOut);
                            if (data == null)
                            {
                                //Fail
                                ProcessTestIndex++;
                                //LOG
                                this.Invoke(new Action(() => {
                                    _form1.WriteLogPC(LogType.Main, "readParameterDoor", "Null");
                                }));
                            }
                            if (data != null)
                            {
                                if (data.DataCard == "")
                                {
                                    //Fail
                                    ProcessTestIndex++;
                                    //LOG
                                    this.Invoke(new Action(() => {
                                        _form1.WriteLogPC(LogType.Main, "readParameterDoor", "data.DataCard = Empty");
                                    }));
                                }
                                else
                                {
                                    if (data.DataCard_GetTrue)
                                    {
                                        //Pass
                                        ProcessTestIndex = 4;
                                    }
                                    else
                                    {
                                        //Fail
                                        ProcessTestIndex++;
                                    }
                                }
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        //Khi lỗi xảy ra, báo cho PLC kiểm tra lại
                        _form1.PLC.Write.Auto.Test.StartRF_Card_Check = false;
                        if (!_form1.PLC.Read.Auto.Test.ReadyRF_Card_Check)
                        {
                            if (TestRetryTime++ < CurrentDoorTestData.retry)
                            {
                                ProcessTestIndex = 0;
                            }
                            else
                            {
                                //kết thúc kiểm tra Fail
                                if (readParameterDoor != null)
                                    FinishATest(false, readParameterDoor.card.ToString());
                                else
                                    FinishATest(false, "Error");
                            }
                        }
                        break;
                    }
                case 4:
                    {
                        //kết thúc kiểm tra Pass
                        _form1.PLC.Write.Auto.Test.StartRF_Card_Check = false;
                        if (!_form1.PLC.Read.Auto.Test.ReadyRF_Card_Check)
                        {
                            FinishATest(true, CurrentDoorTestData.Min);
                        }
                        break;
                    }
                default: { break; }
            }
        }
        public void Test_Button_Check_Register_P()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Button_Check_Register_P.Name ||
                CurrentDoorTestData.Detail != functionDoorList.Button_Check_Register_P.Detail
                ) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute
            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        _form1.PLC.Write.Auto.Test.StartButton_Check_Register_P = true;
                        ProcessTestIndex++;
                        break;
                    }
                case 2:
                    {
                        if (_form1.PLC.Read.Auto.Test.ReadyButton_Check_Register_P)
                        {
                            datakeyRead = _form1.LockASSA.checkDataKey(CurrentDoorTestData.TimeOut);
                            DataLockread = _form1.LockASSA.CheckInputADoor(CurrentDoorTestData.TimeOut);
                            if (datakeyRead == null)
                            {
                                //Fail
                                ProcessTestIndex++;
                                //LOG
                                this.Invoke(new Action(() => {
                                    _form1.WriteLogPC(LogType.Main, "datakeyRead", "Null");
                                }));
                            }
                            if (datakeyRead != null)
                            {
                                if (datakeyRead.SourceUsed == "")
                                {
                                    //Fail
                                    ProcessTestIndex++;
                                    //LOG
                                    this.Invoke(new Action(() => {
                                        _form1.WriteLogPC(LogType.Main, "datakeyRead", "datakeyRead.SourceUsed = Empty");
                                    }));
                                }
                                else
                                {
                                    if (datakeyRead.Key == "Key Register")
                                    {
                                        //Pass
                                        ProcessTestIndex = 4;
                                    }
                                    else
                                    {
                                        //Fail
                                        ProcessTestIndex++;
                                    }
                                }
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        //Khi lỗi xảy ra, báo cho PLC kiểm tra lại
                        if (TestRetryTime++ < CurrentDoorTestData.retry)
                        {
                            ProcessTestIndex = 0;
                        }
                        else
                        {
                            //kết thúc kiểm tra Fail
                            if (datakeyRead != null)
                                FinishATest(false, datakeyRead.Key);
                            else
                                FinishATest(false, "Error");
                        }
                        break;
                    }
                case 4:
                    {
                        //kết thúc kiểm tra Pass
                        _form1.PLC.Write.Auto.Test.StartButton_Check_Register_P = false;
                        if (!_form1.PLC.Read.Auto.Test.ReadyButton_Check_Register_P)
                        {
                            FinishATest(true, CurrentDoorTestData.Min);
                        }
                        break;
                    }
                default: { break; }
            }
        }
        public void Test_Button_Check_Register_N()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Button_Check_Register_N.Name ||
                CurrentDoorTestData.Detail != functionDoorList.Button_Check_Register_N.Detail
                ) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute
            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        _form1.PLC.Write.Auto.Test.StartButton_Check_Register_N = true;
                        ProcessTestIndex++;
                        break;
                    }
                case 2:
                    {
                        if (_form1.PLC.Read.Auto.Test.ReadyButton_Check_Register_N)
                        {
                            datakeyRead = _form1.LockASSA.checkDataKey(CurrentDoorTestData.TimeOut);
                            DataLockread = _form1.LockASSA.CheckInputADoor(CurrentDoorTestData.TimeOut);
                            if (datakeyRead == null)
                            {
                                //Fail
                                ProcessTestIndex++;
                                //LOG
                                this.Invoke(new Action(() => {
                                    _form1.WriteLogPC(LogType.Main, "datakeyRead", "Null");
                                }));
                            }
                            if (datakeyRead != null)
                            {
                                if (datakeyRead.SourceUsed == "")
                                {
                                    //Fail
                                    ProcessTestIndex++;
                                    //LOG
                                    this.Invoke(new Action(() => {
                                        _form1.WriteLogPC(LogType.Main, "datakeyRead", "datakeyRead.SourceUsed = Empty");
                                    }));
                                }
                                else
                                {
                                    if (datakeyRead.Key == "254")
                                    {
                                        //Pass
                                        ProcessTestIndex = 4;
                                    }
                                    else
                                    {
                                        //Fail
                                        ProcessTestIndex++;
                                    }
                                }
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        //Khi lỗi xảy ra, báo cho PLC kiểm tra lại
                        if (TestRetryTime++ < CurrentDoorTestData.retry)
                        {
                            ProcessTestIndex = 0;
                        }
                        else
                        {
                            //kết thúc kiểm tra Fail
                            if (datakeyRead != null)
                                FinishATest(false, datakeyRead.Key);
                            else
                                FinishATest(false, "Error");
                        }
                        break;
                    }
                case 4:
                    {
                        //kết thúc kiểm tra Pass
                        _form1.PLC.Write.Auto.Test.StartButton_Check_Register_N = false;
                        if (!_form1.PLC.Read.Auto.Test.ReadyButton_Check_Register_N)
                        {
                            FinishATest(true, CurrentDoorTestData.Min);
                        }
                        break;
                    }
                default: { break; }
            }
        }
        public void Test_Button_Check_Lock_P()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Button_Check_Lock_P.Name ||
                CurrentDoorTestData.Detail != functionDoorList.Button_Check_Lock_P.Detail
                ) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute
            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        _form1.PLC.Write.Auto.Test.StartButton_Check_Lock_P = true;
                        ProcessTestIndex++;
                        break;
                    }
                case 2:
                    {
                        if (_form1.PLC.Read.Auto.Test.ReadyButton_Check_Lock_P)
                        {
                            datakeyRead = _form1.LockASSA.checkDataKey(CurrentDoorTestData.TimeOut);
                            if (datakeyRead == null)
                            {
                                //Fail
                                ProcessTestIndex++;
                                //LOG
                                this.Invoke(new Action(() => {
                                    _form1.WriteLogPC(LogType.Main, "datakeyRead", "Null");
                                }));
                            }
                            if (datakeyRead != null)
                            {
                                if (datakeyRead.SourceUsed == "")
                                {
                                    //Fail
                                    ProcessTestIndex++;
                                    //LOG
                                    this.Invoke(new Action(() => {
                                        _form1.WriteLogPC(LogType.Main, "datakeyRead", "datakeyRead.SourceUsed = Empty");
                                    }));
                                }
                                else
                                {
                                    if (datakeyRead.Key == "Key Open/Lock")
                                    {
                                        //Pass
                                        ProcessTestIndex = 4;
                                    }
                                    else
                                    {
                                        //Fail
                                        ProcessTestIndex++;
                                    }
                                }
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        //Khi lỗi xảy ra, báo cho PLC kiểm tra lại
                        if (TestRetryTime++ < CurrentDoorTestData.retry)
                        {
                            ProcessTestIndex = 0;
                        }
                        else
                        {
                            //kết thúc kiểm tra Fail
                            if (datakeyRead != null)
                                FinishATest(false, datakeyRead.Key);
                            else
                                FinishATest(false, "Error");
                        }
                        break;
                    }
                case 4:
                    {
                        //kết thúc kiểm tra Pass
                        _form1.PLC.Write.Auto.Test.StartButton_Check_Lock_P = false;
                        if (!_form1.PLC.Read.Auto.Test.ReadyButton_Check_Lock_P)
                        {
                            FinishATest(true, CurrentDoorTestData.Min);
                        }
                        break;
                    }
                default: { break; }
            }
        }
        public void Test_Button_Check_Lock_N()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Button_Check_Lock_N.Name ||
                CurrentDoorTestData.Detail != functionDoorList.Button_Check_Lock_N.Detail
                ) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute
            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        _form1.PLC.Write.Auto.Test.StartButton_Check_Lock_N = true;
                        ProcessTestIndex++;
                        break;
                    }
                case 2:
                    {
                        if (_form1.PLC.Read.Auto.Test.ReadyButton_Check_Lock_N)
                        {
                            datakeyRead = _form1.LockASSA.checkDataKey(CurrentDoorTestData.TimeOut);
                            if (datakeyRead == null)
                            {
                                //Fail
                                ProcessTestIndex++;
                                //LOG
                                this.Invoke(new Action(() => {
                                    _form1.WriteLogPC(LogType.Main, "datakeyRead", "Null");
                                }));
                            }
                            if (datakeyRead != null)
                            {
                                if (datakeyRead.SourceUsed == "")
                                {
                                    //Fail
                                    ProcessTestIndex++;
                                    //LOG
                                    this.Invoke(new Action(() => {
                                        _form1.WriteLogPC(LogType.Main, "datakeyRead", "datakeyRead.SourceUsed = Empty");
                                    }));
                                }
                                else
                                {
                                    if (datakeyRead.Key == "254")
                                    {
                                        //Pass
                                        ProcessTestIndex = 4;
                                    }
                                    else
                                    {
                                        //Fail
                                        ProcessTestIndex++;
                                    }
                                }
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        //Khi lỗi xảy ra, báo cho PLC kiểm tra lại
                        //_form1.PLC.Write.Auto.Test.StartButton_Check_Lock_N = false;
                        if (TestRetryTime++ < CurrentDoorTestData.retry)
                        {
                            ProcessTestIndex = 0;
                        }
                        else
                        {
                            //kết thúc kiểm tra Fail
                            if (datakeyRead != null)
                                FinishATest(false, datakeyRead.Key);
                            else
                                FinishATest(false, "Error");
                        }
                        break;
                    }
                case 4:
                    {
                        //kết thúc kiểm tra Pass
                        _form1.PLC.Write.Auto.Test.StartButton_Check_Lock_N = false;
                        if (!_form1.PLC.Read.Auto.Test.ReadyButton_Check_Lock_N)
                        {
                            FinishATest(true, CurrentDoorTestData.Min);
                        }
                        break;
                    }
                default: { break; }
            }
        }
        public void Test_Motor_Check_Close()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Motor_Check_Close.Name ||
                CurrentDoorTestData.Detail != functionDoorList.Motor_Check_Close.Detail
                ) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute
            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        _form1.PLC.Write.Auto.Test.StartMotor_Check_Close = true;
                        _form1.PLC.Write.Auto.Test.StartCheckDoorPositionSensor_Close = true;

                        ProcessTestIndex++;
                        break;
                    }
                case 2:
                    {
                        _form1.LockASSA.RunMotorDoor(CurrentDoorTestData.TimeOut);
                        ProcessTestIndex++;
                        break;
                    }
                case 3:
                    {
                        if (_form1.PLC.Read.Auto.Test.ReadyMotor_Check_Close)
                        {
                            datakeyRead = _form1.LockASSA.checkDataKey(CurrentDoorTestData.TimeOut);
                            DataLockread = _form1.LockASSA.CheckInputADoor(CurrentDoorTestData.TimeOut);
                            if (datakeyRead == null)
                            {
                                //Fail
                                ProcessTestIndex++;
                                //LOG
                                this.Invoke(new Action(() => {
                                    _form1.WriteLogPC(LogType.Main, "datakeyRead", "Null");
                                }));
                            }
                            if (datakeyRead != null)
                            {
                                if (datakeyRead.SourceUsed == "")
                                {
                                    //Fail
                                    ProcessTestIndex++;
                                    //LOG
                                    this.Invoke(new Action(() => {
                                        _form1.WriteLogPC(LogType.Main, "datakeyRead", "datakeyRead.SourceUsed = Empty");
                                    }));
                                }
                                else
                                {
                                    if (datakeyRead.SensorMortise == "Close")
                                    {
                                        //Pass
                                        ProcessTestIndex = 5;
                                    }
                                    else
                                    {
                                        //Fail
                                        ProcessTestIndex++;
                                    }
                                }
                            }
                        }
                        break;
                    }
                case 4:
                    {
                        //Khi lỗi xảy ra, báo cho PLC kiểm tra lại
                        if (TestRetryTime++ < CurrentDoorTestData.retry)
                        {
                            ProcessTestIndex = 0;
                        }
                        else
                        {
                            //kết thúc kiểm tra Fail
                            if (datakeyRead != null)
                                FinishATest(false, datakeyRead.SensorMortise);
                            else
                                FinishATest(false, "Error");
                        }
                        break;
                    }
                case 5:
                    {
                        //kết thúc kiểm tra Pass
                        _form1.PLC.Write.Auto.Test.StartCheckDoorPositionSensor_Close = false;
                        _form1.PLC.Write.Auto.Test.StartMotor_Check_Close = false;
                        if (!_form1.PLC.Read.Auto.Test.ReadyMotor_Check_Close)
                        {
                            FinishATest(true, CurrentDoorTestData.Min);
                        }
                        break;
                    }
                default: { break; }
            }
        }
        public void Test_Motor_Check_Mortise_Close()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Motor_Check_Mortise_Close.Name ||
                CurrentDoorTestData.Detail != functionDoorList.Motor_Check_Mortise_Close.Detail
                ) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute
            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        
                       _form1.PLC.Write.Auto.Test.StartMotor_Check_Mortise_Close =true;
                        ProcessTestIndex++;
                        break;
                    }
                case 2:
                    {

                        if (_form1.PLC.Read.Auto.Test.IsMortise_Close)
                        {
                            FinishATest(true, CurrentDoorTestData.Min);
                        }
                        else
                        {
                            if (_form1.PLC.Read.Auto.Test.IsMortise_Close)
                            {
                                FinishATest(false, "Open");
                            }
                            else
                            {
                                FinishATest(false, "Error");
                            }
                        }
                        _form1.PLC.Write.Auto.Test.StartMotor_Check_Mortise_Close = false;
                        break;
                       
                    }
                default: { break; }
            }
        }
        public void Test_Motor_Check_Open()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Motor_Check_Open.Name ||
                CurrentDoorTestData.Detail != functionDoorList.Motor_Check_Open.Detail
                ) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute
            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        _form1.PLC.Write.Auto.Test.StartMotor_Check_Open = true;
                        ProcessTestIndex++;
                        break;
                    }
                case 2:
                    {
                        if (_form1.PLC.Read.Auto.Test.ReadyMotor_Check_Open)
                        {
                            bool RunMotor = _form1.LockASSA.RunMotorDoor(CurrentDoorTestData.TimeOut);
                            if (!RunMotor)
                            {
                                //Fail
                                datakeyRead = null;
                                ProcessTestIndex = 4;
                                //LOG
                                this.Invoke(new Action(() => {
                                    _form1.WriteLogPC(LogType.Main, "RunMotorDoor", "Fail");
                                }));
                            }
                            else
                            {
                                if (RunMotor) ProcessTestIndex++;

                            }
                        }
                        break;
                    }
                case 3:
                    {
                        if (_form1.PLC.Read.Auto.Test.ReadyMotor_Check_Open)
                        {
                            datakeyRead = _form1.LockASSA.checkDataKey(CurrentDoorTestData.TimeOut);
                            if (datakeyRead == null)
                            {
                                //Fail
                                ProcessTestIndex = 4;
                                //LOG
                                this.Invoke(new Action(() => {
                                    _form1.WriteLogPC(LogType.Main, "datakeyRead", "Null");
                                }));
                            }
                            if (datakeyRead != null)
                            {
                                if (datakeyRead.SourceUsed == "")
                                {
                                    //Fail
                                    ProcessTestIndex++;
                                    //LOG
                                    this.Invoke(new Action(() => {
                                        _form1.WriteLogPC(LogType.Main, "datakeyRead", "datakeyRead.SourceUsed = Empty");
                                    }));
                                }
                                else
                                {
                                    if (datakeyRead.SensorMortise == "Open")
                                    {
                                        //Pass
                                        ProcessTestIndex = 5;
                                    }
                                    else
                                    {
                                        //Fail
                                        ProcessTestIndex++;
                                    }
                                }
                            }
                        }
                        break;
                    }
                case 4:
                    {
                        //Khi lỗi xảy ra, báo cho PLC kiểm tra lại
                        if (TestRetryTime++ < CurrentDoorTestData.retry)
                        {
                            ProcessTestIndex = 0;
                        }
                        else
                        {
                            //kết thúc kiểm tra Fail
                            if (datakeyRead != null)
                                FinishATest(false, datakeyRead.SensorMortise);
                            else
                                FinishATest(false, "Error");
                        }
                        break;
                    }
                case 5:
                    {
                        //kết thúc kiểm tra Pass
                        _form1.PLC.Write.Auto.Test.StartMotor_Check_Open = false;
                        if (!_form1.PLC.Read.Auto.Test.ReadyMotor_Check_Open)
                        {
                            FinishATest(true, CurrentDoorTestData.Min);
                        }
                        break;
                    }
             


                default: { break; }
            }
        }
        public void Test_Motor_Check_Mortise_Open()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Motor_Check_Mortise_Open.Name ||
                CurrentDoorTestData.Detail != functionDoorList.Motor_Check_Mortise_Open.Detail
                ) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute
            switch (ProcessTestIndex)
            {
                case 1:
                    {
                       
                        _form1.PLC.Write.Auto.Test.StartMotor_Check_Mortise_Open = true;
                        ProcessTestIndex++;
                        break;
                    }
                case 2:
                    {
                        if (_form1.PLC.Read.Auto.Test.IsMortise_Open)
                        {
                            FinishATest(true, CurrentDoorTestData.Min);
                        }
                        else
                        {
                            ProcessTestIndex = 3;
                        }
                        _form1.PLC.Write.Auto.Test.StartMotor_Check_Mortise_Open = false;
                        break;
                    }
                case 3:
                    {
                        if (TestRetryTime++ < CurrentDoorTestData.retry)
                        {
                            ProcessTestIndex = 0;
                        }
                        else
                        {
                            //kết thúc kiểm tra Fail
                            if (_form1.PLC.Read.Auto.Test.IsMortise_Open)
                            {
                                FinishATest(false, "Close");
                            }
                            else
                            {
                                FinishATest(false, "Error");
                            }
                        }
                        break;
                    }
                    
                default: { break; }
            }
        }
        public void Test_ADC_Check()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.ADC_Check.Name) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();
            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute
            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        bool SetVolDone = false, SetCurrentDone = false;
                        _form1.PLC.Write.Auto.Test.Set_Check_ADC_Voltage = true;
                        if (_form1.formModel.Keysight.KeysightPSU.PowerOn && _form1.formModel.Keysight.KeysightPSU.CurrentVoltage == _form1.formModel.modelParameter.KeySight.nudVoltage6V)
                        {
                            ProcessTestIndex = 4;
                        }
                        else
                        {
                            SetVolDone = _form1.KeySightPSU_SetVol(_form1.formModel.modelParameter.KeySight.nudVoltage6V);
                            if (SetVolDone)
                                SetCurrentDone = _form1.KeySightPSU_SetCurrent(_form1.formModel.modelParameter.KeySight.nudCurrent6V);

                            if (SetVolDone && SetCurrentDone) ProcessTestIndex++;


                        }
                        break;
                    }
                case 2:
                    {
                        if (_form1.PLC.Read.Auto.Test.Ready_check_ADCVoltage)
                        {
                            _form1.KeySightPSU_ON();
                            CountKeySightPowerOn = 0;
                            ProcessTestIndex++;
                        }
                        break;
                    }
                case 3:
                    {
                        if (CountKeySightPowerOn++ > DelayKeysightPowerOn)
                        {
                            ProcessTestIndex++;
                        }
                        break;
                    }
                case 4:
                    {
                        if (_form1.PLC.Read.Auto.Test.Ready_check_ADCVoltage)
                        {

                            ADC_Check Vol_string = _form1.LockASSA.ADC_Check(CurrentDoorTestData.TimeOut);
                            
                            double Vol = Convert.ToDouble(Vol_string.Data_ADC);
                            double Max = Convert.ToDouble(CurrentDoorTestData.Max);
                            double Min = Convert.ToDouble(CurrentDoorTestData.Min);


                            if (Vol >= Min && Vol <= Max)
                            {
                                FinishATest(true, Vol.ToString());

                            }
                            else
                            {
                                if (TestRetryTime++ >= CurrentDoorTestData.retry)
                                {
                                    FinishATest(false, Vol.ToString());
                                }
                            }

                        }

                        break;
                    }

                default: { break; }
            }
        }
        public void Test_IDE_Curent_Check()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.IDE_Curent_Check.Name) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();
            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute
            switch (ProcessTestIndex)
            {
                case 1:
                    {

                        _form1.PLC.Write.Auto.Test.Set_Check_IDE_Current = true;
                        if (_form1.formModel.Keysight.KeysightPSU.PowerOn && _form1.formModel.Keysight.KeysightPSU.CurrentVoltage == _form1.formModel.modelParameter.KeySight.nudVoltage6V)
                        {
                            ProcessTestIndex = 4;
                        }
                        else
                        {
                            bool SetVolDone = false, SetCurrentDone = false;
                            SetVolDone = _form1.KeySightPSU_SetVol(_form1.formModel.modelParameter.KeySight.nudVoltage6V);
                            if (SetVolDone)
                                SetCurrentDone = _form1.KeySightPSU_SetCurrent(_form1.formModel.modelParameter.KeySight.nudCurrent6V);

                            if (SetVolDone && SetCurrentDone) ProcessTestIndex++;


                        }
                        break;
                    }
                case 2:
                    {
                        if (_form1.PLC.Read.Auto.Test.Ready_Check_IDE_Current)
                        {
                            _form1.KeySightPSU_ON();
                            CountKeySightPowerOn = 0;
                            ProcessTestIndex++;
                        }
                        break;
                    }
                case 3:
                    {
                        if (CountKeySightPowerOn++ > DelayKeysightPowerOn)
                        {
                            ProcessTestIndex++;
                        }
                        break;
                    }
                case 4:
                    {
                        if (_form1.PLC.Read.Auto.Test.Ready_Check_IDE_Current)
                        {
                            _form1.LockASSA.SleepMode(CurrentDoorTestData.TimeOut);
                            string Cur_string = _form1.formModel.Keysight.KeysightPSU.GetCurrent();
                            _form1.formModel.Keysight.Current_value.Text = Cur_string;
                            double Cur = Convert.ToDouble(Cur_string);
                            double Max = Convert.ToDouble(CurrentDoorTestData.Max);
                            double Min = Convert.ToDouble(CurrentDoorTestData.Min);


                            if (Cur >= Min && Cur <= Max)
                            {
                                FinishATest(true, Cur.ToString());
                            }
                            else
                            {
                                if (TestRetryTime++ >= CurrentDoorTestData.retry)
                                {
                                    FinishATest(false, Cur.ToString());

                                }
                            }
                        }
                        break;
                    }

                default: { break; }
            }
        }

        public void Test_Reset_defaut()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Reset_Defaut_Check.Name) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute

            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        _form1.PLC.Write.Auto.Test.Cyl_Resset_Default = true;
                        ProcessTestIndex++; 
                        break;
                    }

                case 2:

                    {
                        if (_form1.PLC.Read.Auto.Test.StartCyl_Resset_Default)
                        {
                            PowerOn powerOn = _form1.LockASSA.ReadPowerOn(CurrentDoorTestData.TimeOut);
                           
                            if (powerOn.Power_On)
                            {
                                TestRetryTime = 0;
                                ProcessTestIndex++;
                            }
                            else
                            {
                               
                                    ProcessTestIndex = 4;
                               
                            }
                        }
                       
                        break;
                    }
                case 3:
                    {
                       
                        _form1.LockASSA.initializationLock(CurrentDoorTestData.TimeOut);
                        if (TestRetryTime < CurrentDoorTestData.retry)
                        {
                            FinishATest(true, "Pass");
                        }
                        else
                        {
                            if (TestRetryTime++ > CurrentDoorTestData.retry)
                            {
                                ProcessTestIndex ++;

                            }
                        }
                        
                        _form1.PLC.Write.Auto.Test.Cyl_Resset_Default = false;
                        break;
                    }
                case 4:
                    {
                        if (TestRetryTime++ < CurrentDoorTestData.retry)
                        {
                            ProcessTestIndex = 0;
                        }
                        else
                        {
                            //kết thúc kiểm tra Fail
                           FinishATest(false, "null");
                            _form1.PLC.Write.Auto.Test.Cyl_Resset_Default = false;
                        }
                        break;
                    }
                default: { break; }
            }
        }
        public void Test_Check_Stuck()
        {
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.classCheck_Stuck.Name) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute

            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        _form1.PLC.Write.Auto.Test.Cyl_Check_Stuck = true;
                        ProcessTestIndex++;
                        break;
                    }

                case 2:

                    {
                        if (_form1.PLC.Read.Auto.Test.StartCyl_Check_Stuck)
                        {
                            if (_form1.PLC.Read.Input.Push2FW)
                            {
                                ProcessTestIndex++;
                            }
                            else
                            {
                                ProcessTestIndex = 4;
                            }
                        }

                        break;
                    }
                case 3:
                    {
                        _form1.PLC.Write.Auto.Test.Cyl_Check_Stuck = false;
                        if (!_form1.PLC.Read.Auto.Test.StartCyl_Check_Stuck)
                        {
                            FinishATest(true, "Pass");
                        }
                        break;

                    }
                case 4:
                    {
                        if (TestRetryTime++ < CurrentDoorTestData.retry)
                        {
                            ProcessTestIndex = 1;
                        }
                        else
                        {
                            //kết thúc kiểm tra Fail
                            
                            _form1.PLC.Write.Auto.Test.Cyl_Check_Stuck = false;
                            if(!_form1.PLC.Read.Auto.Test.StartCyl_Check_Stuck)
                            {
                                FinishATest(false, "Fail");
                            }
                        }
                        break;
                    }
                default: { break; }
            }
        }
        public void Test_9V_Battery()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.External_Power_Check_9V.Name ||
                CurrentDoorTestData.Detail != functionDoorList.External_Power_Check_9V.Detail
                ) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute

            switch (ProcessTestIndex)
            {
                case 1:// Turn Off Power
                    {
                        
                        _form1.KeySightPSU_Off();
                        ProcessTestIndex++;
                        break;
                    }
                case 2:// Xác Nhận cho PLC ngắt điện 6V ra khỏi chỗ pin
                    {
                        
                        _form1.PLC.Write.Auto.Test.StartExternal_Power_Check_9V = true;
                        // Cài đặt nguồn KeySignt lên 9V
                        bool SetVol = _form1.KeySightPSU_SetVol(_form1.formModel.modelParameter.KeySight.nudVoltage9V);
                        bool SetCur = _form1.KeySightPSU_SetCurrent(_form1.formModel.modelParameter.KeySight.nudCurrent9V);
                        if (!SetVol || !SetCur)
                        {
                            // Lỗi ghi điện áp hoặc dòng điện 9V
                            PowerOn powerOn = _form1.LockASSA.ReadPowerOn(CurrentDoorTestData.TimeOut);
                            if (powerOn.Power_On != false) return;
                        }
                        if (SetVol && SetCur)
                        {
                            //Ghi điện áp và dòng điện thành công
                            ProcessTestIndex++;
                        }
                      
                        break;
                    }
                case 3: // Xác nhận PLC đã mở relay 9V
                    {
                       
                        // Mở nguồn Keysight 9V
                        if (_form1.PLC.Read.Auto.Test.ReadyExternal_Power_Check_9V)
                        {
                            _form1.KeySightPSU_ON();
                            // Delay Sau khi mở nguồn
                            StartDelayProcess = true;
                            DelayProcess(30);
                            if (DelayProcessDone)
                            {
                                StartDelayProcess = false;
                                ProcessTestIndex++;
                            }
                        }
                        break;
                    }
                case 4:  // Kiểm tra khởi tạo
                    {

                        PowerOn powerOn = _form1.LockASSA.ReadPowerOn(CurrentDoorTestData.TimeOut);
                        //if (powerOn.Power_On == null) return;
                        if (powerOn.Power_On)
                        {
                            _form1.LockASSA.initializationLock(CurrentDoorTestData.TimeOut);
                            if (TestRetryTime++ > CurrentDoorTestData.retry)
                            {
                                ProcessTestIndex = 6;
                                powerOn.Power_On = false;
                            }
                            else
                            {
                                ProcessTestIndex = 5;
                                powerOn.Power_On = false;
                            }

                        }


                        break;
                    }
                case 5: //Pass
                    {
                       
                        _form1.PLC.Write.Auto.Test.StartExternal_Power_Check_9V = false;
                        if (!_form1.PLC.Read.Auto.Test.ReadyExternal_Power_Check_9V)
                        {
                            FinishATest(true, CurrentDoorTestData.Min);
                        }
                        break;
                    }
                case 6: //Fail
                    {
                       
                        _form1.PLC.Write.Auto.Test.StartExternal_Power_Check_9V = false;
                        if (!_form1.PLC.Read.Auto.Test.ReadyExternal_Power_Check_9V)
                        {
                            FinishATest(false, "Fail");
                        }
                        break;
                    }
                default: { break; }
            }
        }
        public void Test_9V_Battery_REV()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.External_Power_Check_9V_REV.Name ||
                CurrentDoorTestData.Detail != functionDoorList.External_Power_Check_9V_REV.Detail
                ) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute

            switch (ProcessTestIndex)
            {
                case 1:
                    {
                        StartDelayProcess = true;
                        DelayProcess(5);
                        if (DelayProcessDone)
                        {
                            _form1.PLC.Write.Auto.Test.StartExternal_Power_Check_9V_REV = true;
                            StartDelayProcess = false;
                            
                        }

                        PowerOn powerOn = _form1.LockASSA.ReadPowerOn(CurrentDoorTestData.TimeOut);
                        if (powerOn.Power_On)
                        {
                            ProcessTestIndex++;
                        }
                       
                        break;
                    }
                case 2:
                    {
                        if (_form1.PLC.Read.Auto.Test.ReadyExternal_Power_Check_9V_REV)
                        {
                            ProcessTestIndex++;
                        }
                       
                        break;
                    }
                case 3:
                    {
                        if (_form1.LockASSA.initializationLock(CurrentDoorTestData.TimeOut))
                        {
                            ProcessTestIndex = 4;
                            
                        }
                        else
                        {
                            if (TestRetryTime++ > CurrentDoorTestData.retry)
                            {
                                ProcessTestIndex = 5;
                                
                            }
                        }
                        break;
                    }
                case 4:
                    {
                        _form1.PLC.Write.Auto.Test.StartExternal_Power_Check_9V_REV = false;
                        if (!_form1.PLC.Read.Auto.Test.ReadyExternal_Power_Check_9V_REV)
                        {
                            FinishATest(true, CurrentDoorTestData.Min);
                        }
                        break;
                    }
                case 5:
                    {
                        _form1.PLC.Write.Auto.Test.StartExternal_Power_Check_9V_REV = false;
                        if (!_form1.PLC.Read.Auto.Test.ReadyExternal_Power_Check_9V_REV)
                        {
                            FinishATest(false, "Fail");
                        }
                        break;
                    }
                default: { break; }
            }
        }
        //Hàm Test Mẫu
        public void Test_()
        {
            //Condition
            if (!CurrentDoorTestData.Enable) return;
            if (CurrentDoorTestData.Name != functionDoorList.Broken_Check_Disconnect.Name ||
                CurrentDoorTestData.Detail != functionDoorList.Broken_Check_Disconnect.Detail
                ) return;
            string CurrentResult = dataGridView1.Rows[CurrentDoorTestData.RowIndex].Cells[DoorTableCol.Result].Value.ToString();

            if (ProcessTestIndex == 0 && CurrentResult == DoorResult.Empty)
            {
                TestRetryTime = 0;
                //CountTime
                StopWatchTestDoor = new Stopwatch();
                StopWatchTestDoor.Start();

                ProcessTestIndex = 1;
            }
            //Execute

            switch (ProcessTestIndex)
            {
                case 1:
                    {

                        break;
                    }
                case 2:
                    {

                        break;
                    }
                case 3:
                    {

                        break;
                    }
                case 4:
                    {

                        break;
                    }
                case 5:
                    {

                        break;
                    }
                case 6:
                    {


                        break;
                    }
                case 7:
                    {

                        break;
                    }
                default: { break; }
            }
        }

    }//
}
