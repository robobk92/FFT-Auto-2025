using SLTtechSoft;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using  LibFunction;
using static SLTtechSoft.Form1;

namespace FunctionLoock.Usercontrol
{
    public partial class FunctionLock : UserControl
    {
        Form1 _form1;
        public FunctionLock()
        {
            InitializeComponent();
        }
        public void InitialUI(Form1 obj)
        {
            _form1 = obj;
        }

        public void WriteLogList(string code, string content)
        {
            string DateTimenow = DateTime.Now.ToString("HH:mm:ss");
            ListViewItem item = new ListViewItem(DateTimenow);
            item.SubItems.Add(code);
            item.SubItems.Add(content);
            listView_LogData.Items.Add(item);
        }
        string date = "20250215";
        private void btnExecute_Click(object sender, EventArgs e)
        {
            string thisFunctionTest = cbFunctionTest.Text;
            int timeout = Convert.ToInt32(tbTimeOut.Text);
            string command = thisFunctionTest;
            string[] Content = new string[1];
            switch (thisFunctionTest)
            {
                case "initializationLock":
                    {
                        Content = new string[1];
                        if (_form1.LockASSA.initializationLock(10)) Content[0] = "OK";
                        else Content[0] = "Fail";
                        break;
                    }
                case "readVersion":
                    {
                        Content = new string[1];
                        Readversion data = new Readversion();
                        data = _form1.LockASSA.readVersion(timeout);
                        if (data != null) Content[0] = data.version;
                        else Content[0] = "Null";
                        break;
                    }
                case "CheckParameterDoor": { Content = CheckParameterDoor(); break; }
                case "SetDefaultDoor": { Content = SetDefaultDoor(); break; }
                case "checkDataKey": { Content = checkDataKey(); break; }
                case "CheckInputADoor": { Content = CheckInputADoor(); break; }
                case "SetSerialDoor": { Content = SetSerialDoor(); break; }
                case "QuerySerialDoor": { Content = QuerySerialDoor(); break; }
                case "RunMotorDoor": { Content = RunMotorDoor(); break; }
                case "SleepMode": { Content = SleepMode(); break; }
                case "Speaker_Quiet": { Content = Speaker_Quiet(); break; }
                case "Speaker_Low": { Content = Speaker_Low(); break; }
                case "Speaker_High": { Content = Speaker_High(); break; }
                case "Speaker_Silent": { Content = Speaker_Silent(); break; }
                case "LEDKeyOn": { Content = LEDKeyOn(); break; }
                case "WarningOn": { Content = WarningOn(); break; }
                case "WarningOff": { Content = WarningOff(); break; }
                case "Set_RTC": { Content = SetRTC(); break; }
                case "Get_RTC": { Content = GetRTC(); break; }
                case "CheckProtocol": { Content = Check_Protocol(); break; }
                default: break;
            }
            listView_LogData.Items.Clear();
            for (int i = 0; i < Content.Length; i++)
            {
                WriteLogList(command, Content[i]);
            }
            
        }
        private string[] CheckParameterDoor()
        {
            string[] Content = new string[30];
            int timeout = Convert.ToInt32(tbTimeOut.Text);
            ReadParameterDoor data = new ReadParameterDoor();
            data = _form1.LockASSA.CheckParameterDoor(timeout);
            if (data != null)
            {
                Content = new string[30];
                Content[0] = $"Region: {data.Region}";
                Content[1] = $"value: {data.value}";
                Content[2] = $"buttonregister: {data.buttonregister}";
                Content[3] = $"buttonUnlock_Lock: {data.buttonUnlock_Lock}";
                Content[4] = $"doublecheck: {data.doublecheck}";
                Content[5] = $"Silent: {data.Silent}";
                Content[6] = $"Checkbutton: {data.Checkbutton}";
                Content[7] = $"key_xo: {data.key_xo}";
                Content[8] = $"key_cham: {data.key_cham}";
                Content[9] = $"location: {data.location}";
                Content[10] = $"checkpos1_lock: {data.checkpos1_lock}";
                Content[11] = $"checkpos2_lock: {data.checkpos2_lock}";
                Content[12] = $"motor1_lock: {data.motor1_lock}";
                Content[13] = $"motor2_lock: {data.motor2_lock}";
                Content[14] = $"unlock_lock1: {data.unlock_lock1}";
                Content[15] = $"unlock_lock2: {data.unlock_lock2}";
                Content[16] = $"check_sensor: {data.check_sensor}";
                Content[17] = $"exalt: {data.exalt}";
                Content[18] = $"Pin_code: {data.Pin_code}";
                Content[19] = $"card: {data.card}";
                Content[20] = $"fingerprint: {data.fingerprint}";
                Content[21] = $"button_i: {data.button_i}";
                Content[22] = $"R_autolock: {data.R_autolock}";
                Content[23] = $"L_autolock: {data.L_autolock}";
                Content[24] = $"R_sound: {data.R_sound}";
                Content[25] = $"L_sound: {data.L_sound}";
                Content[26] = $"Speaker: {data.Speaker}";
                Content[27] = $"R_manuallock: {data.R_manuallock}";
                Content[28] = $"L_manuallock: {data.L_manuallock}";
                Content[29] = $"doublecheck_unlock_lock: {data.doublecheck_unlock_lock}";
            }

            else
            {
                Content = new string[1];
                Content[0] = "null";
            }
            return Content ;
        }
        private string[] SetDefaultDoor()
        {
            string[] Result = new string[2];
            int timeout = Convert.ToInt32(tbTimeOut.Text);
            SetdeafautLock data = new SetdeafautLock();
            data = _form1.LockASSA.SetDefaultDoor(0xFF, timeout);
            if (data != null)
            {
                Result[0] = data.temperature;
                Result[1] = data.checkFlash.ToString();
            }
            else
            {
                Result[0] = "null";
            }
            return Result;
        }
        private string[] checkDataKey()
        {
            string[] Content = new string[30];
            int timeout = Convert.ToInt32(tbTimeOut.Text);
            DataKey data = new DataKey();
            data = _form1.LockASSA.checkDataKey(timeout);
            if (data != null)
            {
                Content = new string[16];
                Content[0] = $"SourceUsed: {data.SourceUsed}";
                Content[1] = $"Voltage: {data.Voltage}";
                Content[2] = $"Sensor: {data.SensorMortise}";
                Content[3] = $"Key: {data.Key}";
                Content[4] = $"SensorDoor: {data.SensorDoor}";
                Content[5] = $"RTC: {data.RTC}";
                Content[6] = $"Left_Right: {data.Left_Right}";
                Content[7] = $"Auto_manual: {data.Auto_manual}";
                Content[8] = $"Incident: {data.Incident}";
                Content[9] = $"lid: {data.lid}";
                Content[10] = $"Communicate_UARTfingerprint: {data.Communicate_UARTfingerprint}";
                Content[11] = $"fingerprintLid: {data.fingerprintLid}";
                Content[12] = $"IC_touch: {data.IC_touch}";
                Content[13] = $"Volume: {data.Volume}";
                Content[14] = $"screet: {data.screet}";
                Content[15] = $"implement: {data.implement}";
            }

            else
            {
                Content = new string[1];
                Content[0] = "null";
            }
            return Content;
        }
        private string[] CheckInputADoor()
        {
            string[] Content = new string[30];
            int timeout = Convert.ToInt32(tbTimeOut.Text);
            CheckLockInputAddstatus data = new CheckLockInputAddstatus();
            data = _form1.LockASSA.CheckInputADoor(timeout);
            if (data != null)
            {
                Content = new string[5];
                Content[0] = $"hardware: {data.hardware}";
                Content[1] = $"sensor_2: {data.sensor_2}";
                Content[2] = $"EEPROM: {data.EEPROM}";
                Content[3] = $"Sorftware: {data.Sorftware}";
                Content[4] = $"Additional: {data.Additional}";
            }
            else
            {
                Content = new string[1];
                Content[0] = "null";
            }
            return Content;
        }

        private string[] SetSerialDoor()
        {
            string[] Content = new string[30];
            int timeout = Convert.ToInt32(tbTimeOut.Text);
            SerialLock data = new SerialLock();
            data = _form1.LockASSA.SetSerialDoor(timeout);
            if (data != null)
            {
                Content = new string[3];
                Content[0] = $"SerialDoorTX: {data.SerialDoorTX}";
                Content[1] = $"DataquerySeri: {data.DataquerySeri}";
                Content[2] = $"quertySeti_GetTrue: {data.quertySeti_GetTrue}";
            }
            else
            {
                Content = new string[1];
                Content[0] = "null";
            }
            return Content;
        }
        private string[] QuerySerialDoor()
        {
            string[] Content = new string[30];
            int timeout = Convert.ToInt32(tbTimeOut.Text);
            SerialLock data = new SerialLock();
            data = _form1.LockASSA.QuerySerialDoor(timeout);
            if (data != null)
            {
                Content = new string[3];
                Content[0] = $"SerialDoorTX: {data.SerialDoorTX}";
                Content[1] = $"DataquerySeri: {data.DataquerySeri}";
                Content[2] = $"quertySeti_GetTrue: {data.quertySeti_GetTrue}";
            }
            else
            {
                Content = new string[1];
                Content[0] = "null";
            }
            return Content;
        }
        private string[] RunMotorDoor()
        {
            string[] Content = new string[1];
            int timeout = Convert.ToInt32(tbTimeOut.Text);
            bool data = new bool();
            data = _form1.LockASSA.RunMotorDoor(timeout);
            Content[0] = data.ToString();
         
            return Content;
        }
        private string[] SleepMode()
        {
            string[] Content = new string[1];
            int timeout = Convert.ToInt32(tbTimeOut.Text);
            bool data = new bool();
            data = _form1.LockASSA.SleepMode(timeout);
            Content[0] = data.ToString();

            return Content;
        }
        private string[] Speaker_Quiet()
        {
            string[] Content = new string[2];
            int timeout = Convert.ToInt32(tbTimeOut.Text);
            Speak data = new Speak();
            data = _form1.LockASSA.Speaker(SpeakerType.Quiet,timeout);
            if (data != null)
            {
                Content = new string[3];
                Content[0] = $"DataSpeak: {data.DataSpeak}";
                Content[1] = $"DataSpeak_GetTrue: {data.DataSpeak_GetTrue}";
            }
            else
            {
                Content = new string[1];
                Content[0] = "null";
            }
            return Content;
        }
        private string[] Speaker_Low()
        {
            string[] Content = new string[2];
            int timeout = Convert.ToInt32(tbTimeOut.Text);
            Speak data = new Speak();
            data = _form1.LockASSA.Speaker(SpeakerType.Low, timeout);
            if (data != null)
            {
                Content = new string[3];
                Content[0] = $"DataSpeak: {data.DataSpeak}";
                Content[1] = $"DataSpeak_GetTrue: {data.DataSpeak_GetTrue}";
            }
            else
            {
                Content = new string[1];
                Content[0] = "null";
            }
            return Content;
        }
        private string[] Speaker_High()
        {
            string[] Content = new string[2];
            int timeout = Convert.ToInt32(tbTimeOut.Text);
            Speak data = new Speak();
            data = _form1.LockASSA.Speaker(SpeakerType.High, timeout);
            if (data != null)
            {
                Content = new string[3];
                Content[0] = $"DataSpeak: {data.DataSpeak}";
                Content[1] = $"DataSpeak_GetTrue: {data.DataSpeak_GetTrue}";
            }
            else
            {
                Content = new string[1];
                Content[0] = "null";
            }
            return Content;
        }
        private string[] Speaker_Silent()
        {
            string[] Content = new string[2];
            int timeout = Convert.ToInt32(tbTimeOut.Text);
            Speak data = new Speak();
            data = _form1.LockASSA.Speaker(SpeakerType.Silent, timeout);
            if (data != null)
            {
                Content = new string[3];
                Content[0] = $"DataSpeak: {data.DataSpeak}";
                Content[1] = $"DataSpeak_GetTrue: {data.DataSpeak_GetTrue}";
            }
            else
            {
                Content = new string[1];
                Content[0] = "null";
            }
            return Content;
        }
        private string[] LEDKeyOn()
        {
            string[] Content = new string[2];
            int timeout = Convert.ToInt32(tbTimeOut.Text);
            OnLed data = new OnLed();
            data = _form1.LockASSA.LEDKeyOn(timeout);
            if (data != null)
            {
                Content = new string[3];
                Content[0] = $"DataOnled: {data.DataOnled}";
                Content[1] = $"DataOnled_GetTrue: {data.DataOnled_GetTrue}";
            }
            else
            {
                Content = new string[1];
                Content[0] = "null";
            }
            return Content;
        }
        private string[] WarningOn()
        {
            string[] Content = new string[2];
            int timeout = Convert.ToInt32(tbTimeOut.Text);
            Alarm data = new Alarm();
            data = _form1.LockASSA.WarningOnOff(WarningType.On, timeout);
            if (data != null)
            {
                Content = new string[3];
                Content[0] = $"DataAlarm: {data.DataAlarm}";
                Content[1] = $"DataAlarm_GetTrue: {data.DataAlarm_GetTrue}";
            }
            else
            {
                Content = new string[1];
                Content[0] = "null";
            }
            return Content;
        }
        private string[] WarningOff()
        {
            string[] Content = new string[2];
            int timeout = Convert.ToInt32(tbTimeOut.Text);
            Alarm data = new Alarm();
            data = _form1.LockASSA.WarningOnOff(WarningType.Off, timeout);
            if (data != null)
            {
                Content = new string[3];
                Content[0] = $"DataAlarm: {data.DataAlarm}";
                Content[1] = $"DataAlarm_GetTrue: {data.DataAlarm_GetTrue}";
            }
            else
            {
                Content = new string[1];
                Content[0] = "null";
            }
            return Content;
        }
        private string[] Check_Protocol()
        {
            string[] Content = new string[2];
            int timeout = Convert.ToInt32(tbTimeOut.Text);
            Protocol1 data = new Protocol1();
            data = _form1.LockASSA.CheckPCBBLE(CheckProtocol.Check_RTC,timeout);
            if (data != null)
            {
                Content = new string[3];
                Content[0] = $"DataProtocol: {data.Data_Protocol}";
                Content[1] = $"DataProtocol_GetTrue: ";
            }
            else
            {
                Content = new string[1];
                Content[0] = "null";
            }
            return Content;
        }
        private string[] SetRTC()
        {
            string[] Content = new string[2];
            int timeout = Convert.ToInt32(tbTimeOut.Text);
            RTC data = new RTC();
            data = _form1.LockASSA.SetRTC(date, timeout);
            if (data != null)
            {
                Content = new string[3];
                Content[0] = $"DataSetRTC: {data.Data_Set_RTC}";
                Content[1] = $"DataSetRTC_GetTrue: {data.Data_Set_RTC_GetTrue}";
            }
            else
            {
                Content = new string[1];
                Content[0] = "null";
            }
            return Content;
        }
        private string[] GetRTC()
        {
            string[] Content = new string[2];
            int timeout = Convert.ToInt32(tbTimeOut.Text);
            RTC data = new RTC();
            data = _form1.LockASSA.GetRTC(timeout);
            if (data != null)
            {
                Content = new string[3];
                Content[0] = $"DataGetRTC: {data.Data_Get_RTC}";
                Content[1] = $"DataGetRTC_GetTrue: {data.Data_Get_RTC_GetTrue}";
            }
            else
            {
                Content = new string[1];
                Content[0] = "null";
            }
            return Content;
        }
    }
}
