using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFunction
{
    public class ClassDoorTestData
    {
        public int RowIndex {  get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public string Value { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
        public string Unit { get; set; }
        public string Time { get; set; }
        public string Result { get; set; }
        public bool Enable { get; set; }
        public int retry { get; set; }
        public int TimeOut { get; set; }
    }
    public class PowerOn
    {
        public byte[] Fisrtscan { get; set; }
        public byte[] Data { get; set; }
        public bool Power_On { get; set; }
        public string VersionComunication { get; set; }
    }
    public class ReadParameterDoor
    {
        public byte[] DataRecived { get; set; }
        public string Region { get; set; }
        public byte[] button { get; set; }
        public byte[] sensor { get; set; }
        public byte method { get; set; }
        public byte[] degree { get; set; }
        public byte[] config { get; set; }
        public byte[] None { get; set; }
        //bit type
        public string value { get; set; }
        //bit button
        public bool buttonregister { get; set; }
        public bool buttonUnlock_Lock { get; set; }
        public bool doublecheck { get; set; }
        public bool Silent { get; set; }
        public bool Checkbutton { get; set; }
        public bool key_xo { get; set; }
        public bool key_cham { get; set; }
        //bit sensor
        public bool location { get; set; }
        public bool checkpos1_lock { get; set; }
        public bool checkpos2_lock { get; set; }
        public bool motor1_lock { get; set; }
        public bool motor2_lock { get; set; }
        public bool unlock_lock1 { get; set; }
        public bool unlock_lock2 { get; set; }
        public bool check_sensor { get; set; }
        //bit method
        public bool exalt { get; set; }
        //bit degree
        public bool Pin_code { get; set; }
        public bool card { get; set; }
        public bool fingerprint { get; set; }
        public bool button_i { get; set; }
        //bit config
        public bool R_autolock { get; set; }
        public bool L_autolock { get; set; }
        public bool R_sound { get; set; }
        public bool L_sound { get; set; }
        public bool Speaker { get; set; }
        public bool R_manuallock { get; set; }
        public bool L_manuallock { get; set; }
        public bool doublecheck_unlock_lock { get; set; }

    }
    public class Readversion
    {
        public byte[] DataRecived { get; set; }
        public string version { get; set; }
    }
    public class SetdeafautLock
    {
        public byte[] DataRecived { get; set; }
        public string temperature { get; set; }
        public bool checkFlash { get; set; }
    }
    public class DataKey
    {
        public byte[] DataRecived { get; set; }
        public string SourceUsed { get; set; }
        public string Voltage { get; set; }
        public string SensorMortise { get; set; }
        public string Key { get; set; }
        public string SensorDoor { get; set; }
        public string RTC { get; set; }
        public string Left_Right { get; set; }
        public string Auto_manual { get; set; }
        public string Incident { get; set; }
        public string lid { get; set; }
        public string Communicate_UARTfingerprint { get; set; }
        public string fingerprintLid { get; set; }
        public string IC_touch { get; set; }
        public string Volume { get; set; }
        public string screet { get; set; }
        public string implement { get; set; }
    }
    public class CheckLockInputAddstatus
    {
        public byte[] DataRecived { set; get; }
        public string hardware { get; set; }
        public string sensor_2 { get; set; }
        public string EEPROM { get; set; }
        public string Sorftware { get; set; }
        public string Additional { get; set; }
    }
    public class communicationLock
    {
        public byte[] DataRecived { get; set; }
        public string Byte1 { get; set; }
        public string Byte2 { get; set; }
    }
    public class Password_Card_Figer
    {
        public byte[] DataRecived_Pass { set; get; }
        public byte[] DataRecived_ConfirmCard { set; get; }
        public byte[] DataRecived_ConfirmFinger { set; get; }
        public string DataPass { get; set; }
        public string DataCard { get; set; }
        public string DataFingerPrint { get; set; }
        public bool DataPass_GetTrue { get; set; }
        public bool DataCard_GetTrue { get; set; }
        public bool DataFingerPrint_GetTrue { get; set; }
    }
    public class languageClass
    {
        public byte[] DataRecived { get; set; }
        public string Data { get; set; }
        public bool DataChageLanguageTrue { get; set; }
        public byte[] Option_DataRecived { get; set; }
        public string Option_Data { get; set; }
        public bool Option_DataGetTrue { get; set; }
    }
    public class OpenUIDclass
    {
        public byte[] DataRecived { get; set; }
        public string Data { get; set; }
        public bool DataOpenUID_GetTrue { get; set; }
    }
    public class IDCard
    {
        public byte[] FloatingID_DataRecived { get; set; }
        public string FloatingID_Data { get; set; }
        public bool DataFloatingID_GetTrue { get; set; }
        public byte[] IDLock_DataRecived { get; set; }
        public string IDLock_Data { get; set; }
        public bool IDLock_GetTrue { get; set; }
    }
    public class NameLock
    {
        public byte[] HightbyteDataRecived { get; set; }
        public byte[] LowbyteDataRecived { get; set; }
        public string HightbyteData { get; set; }
        public string LowbyteData { get; set; }
        public bool Hightbyte_GetTrue { get; set; }
        public bool Lowbyte_GetTrue { get; set; }
    }
    public class SerialLock
    {
        public byte[] DataRecived { get; set; }
        public string SerialDoorTX { get; set; }
        public string DataquerySeri { get; set; }
        public bool quertySeti_GetTrue { get; set; }

    }
    public class HistoryLock
    {
        public byte[] DataRecived { get; set; }
        public string Byte1 { get; set; }
        public string Byte2 { get; set; }
        public string Byte3 { get; set; }
        public string Byte4 { get; set; }
        public string Byte5 { get; set; }
        public string Byte6 { get; set; }
        public string Byte7 { get; set; }
        public string Byte8 { get; set; }
    }
    public class Speak
    {
        public byte[] Datarecived { get; set; }
        public string DataSpeak { get; set; }
        public bool DataSpeak_GetTrue { get; set; }
    }
    public class OnLed
    {
        public byte[] Datarecived { get; set; }
        public string DataOnled { get; set; }
        public bool DataOnled_GetTrue { get; set; }
    }
    public class Alarm
    {
        public byte[] Datarecived { get; set; }
        public string DataAlarm { get; set; }
        public bool DataAlarm_GetTrue { get; set; }

    }
    public class BLE
    {
        public byte[] Datarecived_RF_APP { get; set; }
        public string Data_RF_APP { get; set; }
        public bool DataRF_APP_GetTrue { get; set; }
        public byte[] Datarecived_Keypad { get; set; }
        public string Data_Keypad { get; set; }
        public bool Data_Keypad_GetTrue { get; set; }
        public byte[] Datarecived_CBA { get; set; }
        public string Data_CBA { get; set; }
        public bool Data_CBA_GetTrue { get; set; }

    }
    public class Protocol1
    {
        public byte[] Datarecived_Protocol { get; set; }
        public string Data_Protocol { get; set; }
    }
    public class RTC
    {
        public byte[] Datarecived_Set_RTC { get; set; }
        public string Data_Set_RTC { get;set; }
        public bool Data_Set_RTC_GetTrue { get; set; }
        public byte[] Datarecived_Get_RTC { get; set; }
        public string Data_Get_RTC { get; set; }
        public bool Data_Get_RTC_GetTrue {get; set; }
    }
    public class ADC_Check
    {
        public byte[] Datarecived { get; set; }
        public string Data_ADC { get; set; }

    }
    public class External_device
    {
        public byte[] DataRecived { get; set; }
        public string DataExternal { get; set; }

    }
    public class EndCheckLock
    {
        public byte[] Datarecived { get; set; }
        public string DataCheckEnd { get; set; }
        public bool DataCheckEnd_GetTrue { get; set; }
    }
    public enum SetDefaultDoorType
    {
        GreateManual,
        LeftNumManual,
        GreatAuto,
        LeftNumAuto,
        WaitAutoLeftRight,
        NoSetLeftRight
    }
    public enum SpeakerType
    {
        Quiet,
        Low,
        High,
        Silent
    }
    public enum WarningType
    { 
        On = 0x21,
        Off = 0x41
    }
    public enum ProtectType
    {
        None = 0x00,
        Level1 = 0x01,
        Level2 = 0x02
    }
    public enum CheckProtocol
    {
        Check_RTC   = 0x00,
        Check_ADC   = 0x01,
        Check_tuKe  = 0x02,
        Check_IOS   = 0x03,
        Check_Current_BLE_Tx = 0x04,
        Check_BlE_Rx = 0x05,
        Check_Sleep_Mode = 0x06


    }
    public class FFTTableColumnClass
    {
        public int ID = 0;
        public int Name = 1;
        public int Detail = 2;
        public int Value = 3;
        public int Min = 4;
        public int Max = 5;
        public int Unit = 6;
        public int Time = 7;
        public int Result = 8;
        public int Enable = 9;
        public int Retry = 10;
        public int Timeout = 11;
    }
    public class FFTResultClass
    {
        public string Empty = "";
        public string Pass = "PASS";
        public string Fail = "FAIL";
    }
    public class LabelStatus
    {
        public string Wait = "...Waiting...";
        public string Pass = "PASS";
        public string Fail = "Fail";
    }
    

}
