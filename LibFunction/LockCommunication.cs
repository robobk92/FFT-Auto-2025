using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;
using System.Xml.Serialization;
using System.Collections;
using System.Reflection.Emit;
using System.Net.Http.Headers;
using System.ComponentModel;
using static LibFunction.LockCommunication;
using System.Diagnostics;
using System.Configuration;
using System.Globalization;
using System.Web;
namespace LibFunction
{
    public class LockCommunication
    {

        public PowerOn Power = new PowerOn();
        public ReadParameterDoor ReadParameter = new ReadParameterDoor();
        public Readversion ReadVersion = new Readversion();
        public DataKey ReadKey = new DataKey();
        public CheckLockInputAddstatus CheckLockInput = new CheckLockInputAddstatus();
        public communicationLock Communication = new communicationLock();
        public Password_Card_Figer PasswordCardFiger = new Password_Card_Figer();
        public languageClass language = new languageClass();
        public OpenUIDclass UID = new OpenUIDclass();
        public IDCard IdCard = new IDCard();
        public NameLock Name = new NameLock();

        public HistoryLock history = new HistoryLock();
        public Speak speak = new Speak();
        public OnLed Led = new OnLed();
        public Alarm AlarmBz = new Alarm();
        public BLE BLE_APP = new BLE();
        public RTC RTC = new RTC();
        public Protocol1 Protocol1 = new Protocol1();
        public ADC_Check ADCCheck = new ADC_Check();
        public External_device External = new External_device();
        public EndCheckLock EndCheck = new EndCheckLock();
        public SerialPort Serial = new SerialPort();
        public bool Mode_TCP_Serial = false;
        //Parameter for TCPIP
        public int tcpClient_ReceiveTimeout = 3000;
        public bool RTUOverTCP = false;
        static TcpClient tcpClient;
        public string m_ipAddress = "127.0.0.1";
        public int m_tcpPort = 9007;
        public int ID;
        static DateTime dtDisconnect = new DateTime();
        static DateTime dtNow = new DateTime();
        public bool NetworkIsOk = false;
        public string Datasending, DataRecived;
        public int numberOfTX;
        public string ExceptionCode = "";
        public string Status = "";
        public bool ReadHoldRegisterIsError, ReadCoilsIsError, WriteSingleCoilIsError, WriteMultiRegisterIsError;
        public string TX_cmd, RX_cmd;

        private Stopwatch stopwatch = new Stopwatch();

        static readonly string Digits = "0123456789ABCDEF";

        public string ByteTo1Hex(byte b)
        {
            char[] chars = new char[2];
            chars[0] = Digits[b / 16];
            chars[1] = Digits[b % 16];
            return new string(chars);
        }

        public byte[] CRC16(byte[] data)
        {
            byte[] checksum = new byte[2];
            ushort reg_crc = 0xFFFF;
            for (int i = 0; i < data.Length - 2; i++)
            {
                reg_crc ^= data[i];
                for (int j = 0; j < 8; j++)
                {
                    if ((reg_crc & 0x01) == 1)

                    {
                        reg_crc = (ushort)((reg_crc >> 1) ^ 0xA001);
                    }

                    else
                    {
                        reg_crc = (ushort)(reg_crc >> 1);
                    }
                }

            }
            checksum[1] = (byte)((reg_crc >> 8) & 0xFF);
            checksum[0] = (byte)(reg_crc & 0xFF);
            return checksum;

        }

        public string IntToHex(int b)
        {

            string result = "0";
            if (b >= 0 && b < 16)
            {
                switch (b)
                {
                    case 0: { result = "0"; break; }
                    case 1: { result = "1"; break; }
                    case 2: { result = "2"; break; }
                    case 3: { result = "3"; break; }
                    case 4: { result = "4"; break; }
                    case 5: { result = "5"; break; }
                    case 6: { result = "6"; break; }
                    case 7: { result = "7"; break; }
                    case 8: { result = "8"; break; }
                    case 9: { result = "9"; break; }
                    case 10: { result = "A"; break; }
                    case 11: { result = "B"; break; }
                    case 12: { result = "C"; break; }
                    case 13: { result = "D"; break; }
                    case 14: { result = "E"; break; }
                    case 15: { result = "F"; break; }
                    default: { result = "0"; break; }
                }
            }
            return result;
        }
        public string IntToHex2(int b)
        {
            string result = "0";
            if (b >= 0 && b < 256)
            {
                int b1 = b % 16;
                int b2 = b / 16;
                result = IntToHex(b2) + IntToHex(b1);
            }
            return result;
        }
        public bool[] Int8ToBools(int value)
        {
            bool[] bits = new bool[8];
            int remainder = 0;
            int i = 0;
            while (value > 0)
            {

                remainder = value % 2;
                value /= 2;
                bits[i] = Convert.ToBoolean(remainder);
                i++;
            }
            return bits;
        }

        public bool[] Int16ToBools(int value)
        {
            bool[] bits = new bool[16];
            int remainder = 0;
            int i = 0;
            while (value > 0)
            {

                remainder = value % 2;
                value /= 2;
                bits[i] = Convert.ToBoolean(remainder);
                i++;
            }
            return bits;
        }
        public bool[] Int32ToBools(int value)
        {
            bool[] bits = new bool[32];
            int remainder = 0;
            int i = 0;
            while (value > 0)
            {

                remainder = value % 2;
                value /= 2;
                bits[i] = Convert.ToBoolean(remainder);
                i++;
            }
            return bits;
        }

        public bool[] Hex8ToBools(string value)
        {
            int decvalue = HexToInt8(value);
            bool[] result = Int8ToBools(decvalue);
            return result;
        }

        public int HexToInt8(string Hexstring)
        {
            int result;
            if (Hexstring.Length == 0 || Hexstring.Length > 2)
            {
                result = 0;
            }
            else
            {

                if (Hexstring.Length == 2)
                {
                    string highstring = Hexstring.Substring(0, 1);
                    string lowstring = Hexstring.Substring(1, 1);
                    int highword = HexToInt4(highstring);
                    int lowword = HexToInt4(lowstring);
                    result = highword * 16 + lowword;
                }
                else
                {
                    result = HexToInt4(Hexstring);
                }

            }
            return result;
        }

        public int HexToInt4(string HexString)
        {
            int result;
            switch (HexString)
            {
                case "0": { result = 0; break; }
                case "1": { result = 1; break; }
                case "2": { result = 2; break; }
                case "3": { result = 3; break; }
                case "4": { result = 4; break; }
                case "5": { result = 5; break; }
                case "6": { result = 6; break; }
                case "7": { result = 7; break; }
                case "8": { result = 8; break; }
                case "9": { result = 9; break; }
                case "A": { result = 10; break; }
                case "B": { result = 11; break; }
                case "C": { result = 12; break; }
                case "D": { result = 13; break; }
                case "E": { result = 14; break; }
                case "F": { result = 15; break; }
                default: { result = 0; break; }
            }
            return result;
        }

        public bool ConnectSerial()
        {
            Mode_TCP_Serial = true;
            bool result;
            try
            {
                if (!Serial.IsOpen)
                {
                    Serial.Open();
                }
                ExceptionCode = "";
                result = true;
            }
            catch (Exception ex)
            {
                ExceptionCode = ex.ToString();
                result = false;
            }
            return result;
        }

        public void Disconnect()
        {

            tcpClient.Close();

        } //class Disconnect
        public ushort CombineBytes16(byte[] bytes, int count)
        {
            ushort result = 0;
            for (int i = 0; i < count && i < bytes.Length; i++)
            {
                result |= (ushort)(bytes[i] << (8 * (count - 1 - i)));
            }
            return result;
        }
        public uint CombineBytes32(byte[] bytes, int count)
        {
            uint result = 0;
            for (int i = 0; i < count && i < bytes.Length; i++)
            {
                result |= (uint)(bytes[i] << (8 * (count - 1 - i)));
            }
            return result;
        }
        static char[] ConvertToCharArray(uint value, int bitLength)
        {
            char[] bits = new char[bitLength];
            for (int i = 0; i < bitLength; i++)
            {
                bits[bitLength - 1 - i] = ((value & (1U << i)) != 0) ? '1' : '0';
            }
            return bits;
        }
        //public byte[] Merge4Bits(byte value)
        //{
        //    // Extract higher 4 bits and lower 4 bits
        //    byte highNibble = (byte)((value >> 4) & 0x0F); // Top 4 bits
        //    byte lowNibble = (byte)(value & 0x0F);         // Bottom 4 bits
        //    return new byte[] { highNibble, lowNibble };
        //}
        public char[] GetBits(byte value)
        {
            char[] bits = new char[8];
            for (int i = 0; i < 8; i++)
            {
                bits[7 - i] = ((value & (1 << i)) != 0) ? '1' : '0';
            }
            return bits;
        }

        private void ArrayByteToString_TX(ref string Output, byte[] input)
        {
            Output = "";
            for (int i = 0; i < input.Length; i++)
            {
                string hex = IntToHex2((int)input[i]);
                Output = Output + hex + " ";
            }
        }
        private void ArrayByteToString_RX(ref string Output, byte[] input, int length)
        {
            byte[] newbtye = new byte[length];
            for (int i = 0; i < length; i++)
            {
                newbtye[i] = input[i];
            }
            Output = BitConverter.ToString(newbtye).Replace("-", " ");
        }
        private void CheckByteToRead(ref int m, int TimeOut)
        {
            int lastbytes = 0;
            int ByteWait = 0;
            //200,000 =~ 1.51s
            //129870 - 1000ms
            int TimeOutCal = TimeOut / 1000 * 129870;
            for (int i = 0; i < TimeOutCal; i++)
            {
                int bytesNow = Serial.BytesToRead;
                if (bytesNow > 2)
                {
                    if (bytesNow != lastbytes)
                    {
                        lastbytes = bytesNow;
                        ByteWait = 0;
                    }
                    else
                    {
                        ByteWait++;
                    }
                    if (ByteWait > 10000)
                    {
                        break;
                    }
                }
            }
            m = Serial.BytesToRead;
        }
        public PowerOn ReadPowerOn(int TimeOut)
        {
            PowerOn Datarecived = new PowerOn();
            try
            {
                byte[] hi = new byte[1000];
                int m = 0;
                CheckByteToRead(ref m, TimeOut);

                if (m > 0)
                {
                    Datarecived.Fisrtscan = new byte[m];
                    Datarecived.Data = new byte[m];
                    Serial.Read(hi, 0, m);
                    for (int i = 0; i < m; i++)
                    {
                        Datarecived.Fisrtscan[i] = hi[i];
                    }
                    if (Datarecived.Fisrtscan[0] == 0x44 && Datarecived.Fisrtscan[1] == 0xB0 && Datarecived.Fisrtscan[3]==0x1A
                         && Datarecived.Fisrtscan[4] == 0x70 && Datarecived.Fisrtscan[5] == 0x71)
                    {
                        Datarecived.Power_On = true;
                    }
                    else
                    {
                        Datarecived.Power_On = false;
                    }
                    RX_cmd = "";
                    RX_cmd = BitConverter.ToString(Datarecived.Fisrtscan).Replace("-", " ");
                    OnEventTransferData(true, "Initial", RX_cmd);
                }
                else
                {
                    throw new Exception("Recived Timeout!");
                }
            }
            catch (Exception ex)
            {
                OnEventException("Initial", ex.Message);
                return null;
            }
            return Datarecived;
        }

        public bool initializationLock(int TimeOut)
        {
            byte[] cmd = new byte[31];
            cmd[0] = 0x44;
            cmd[1] = 0xA1;
            cmd[2] = 0x01;
            cmd[3] = 0x1A;
            cmd[4] = 0xFE;
            cmd[5] = 0XFE;

            for (int i = 0; i < 23; i++)
            {
                cmd[6 + i] = 0x00;
            }
            cmd[30] = 0xFE;
            ArrayByteToString_TX(ref TX_cmd, cmd);
            try
            {
                Serial.Write(cmd, 0, cmd.Length);
                OnEventTransferData(false, "Initial", TX_cmd);

                byte[] hi = new byte[500];
                int m = 0;
                CheckByteToRead(ref m, TimeOut);
                if (m > 0)
                {
                    Serial.Read(hi, 0, m);

                    byte[] newbtye = new byte[m];
                    for (int i = 0; i < m; i++)
                    {
                        newbtye[i] = hi[i];
                    }
                    RX_cmd = BitConverter.ToString(newbtye).Replace("-", " ");
                    OnEventTransferData(true, "Initial", RX_cmd);
                }
                else
                {
                    throw new Exception("Recived Timeout!");
                }
            }
            catch (Exception ex)
            {
                OnEventException("Initial", ex.Message);
                return false;
            }
            return true;
        }
        public Readversion readVersion(int TimeOut)
        {
            Readversion DataRecivedVersion = new Readversion();
            byte[] cmd = new byte[5];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x00;
            cmd[3] = 0x00;
            cmd[4] = 0x03;

            ArrayByteToString_TX(ref TX_cmd, cmd);
            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd, 0, cmd.Length);
                OnEventTransferData(false, "Readversion", TX_cmd);
                CheckByteToRead(ref m, TimeOut);

                DataRecivedVersion.DataRecived = new byte[m];
                if (m > 0)
                {
                    Serial.Read(hi, 0, m);
                    for (int i = 0; i < m; i++)
                    {
                        DataRecivedVersion.DataRecived[i] = hi[i];
                    }
                    ArrayByteToString_RX(ref RX_cmd, hi, m);
                    OnEventTransferData(true, "Readversion", RX_cmd);
                }
                else
                {
                    throw new Exception("Recived Timeout!");
                }


                //int number = Convert.ToInt16(DataRecivedVersion.DataRecived[6]); 
                //string hexValue = number.ToString("X");
                //int lastnumber =Convert.ToInt16(DataRecivedVersion.DataRecived[7]);
                //string hexLast = lastnumber.ToString("X");
                //DataRecivedVersion.version = hexValue+hexLast;
                string formattedNumber = "." + DataRecivedVersion.DataRecived[7].ToString().PadLeft(2, '0');
                double result = DataRecivedVersion.DataRecived[6] / 10.0;
                string[] Bytes = { result.ToString("0.0"), formattedNumber };
                DataRecivedVersion.version = string.Join("", Array.ConvertAll(Bytes, b => b.ToString()));

            }
            catch (Exception ex)
            {
                OnEventException("Readversion", ex.Message);
                return null;
            }
            ReadVersion = DataRecivedVersion;
            return DataRecivedVersion;
        }
        //4. Kiểm tra thông số chức năng khóa cửa
        public ReadParameterDoor CheckParameterDoor(int TimeOut)
        {
            ReadParameterDoor parameterDoor = new ReadParameterDoor();
            byte[] cmd = new byte[5];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x14;
            cmd[3] = 0x00;
            cmd[4] = 0x03;
            ArrayByteToString_TX(ref TX_cmd, cmd);
            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd, 0, cmd.Length);
                OnEventTransferData(false, "CheckParameterDoor", TX_cmd);
                CheckByteToRead(ref m, TimeOut);
                parameterDoor.DataRecived = new byte[m];
                if (m > 0)
                {
                    Serial.Read(hi, 0, m);
                    for (int i = 0; i < m; i++)
                    {
                        parameterDoor.DataRecived[i] = hi[i];
                    }
                    ArrayByteToString_RX(ref RX_cmd, hi, m);
                    OnEventTransferData(true, "CheckParameterDoor", RX_cmd);
                }
                else
                {
                    throw new Exception("Recived Timeout!");
                }
                parameterDoor.button = new byte[2];
                parameterDoor.sensor = new byte[3];
                parameterDoor.degree = new byte[2];
                parameterDoor.config = new byte[4];
                parameterDoor.None = new byte[3];
                if (parameterDoor.DataRecived != null)
                {
                    switch (parameterDoor.DataRecived[4])
                    {
                        case 00:
                            parameterDoor.Region = "Asian";
                            break;
                        case 01:
                            parameterDoor.Region = "EMEA";
                            break;
                        case 02:
                            parameterDoor.Region = "US";
                            break;
                        case 03:
                            parameterDoor.Region = "Unknow";
                            break;
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        parameterDoor.button[i] = parameterDoor.DataRecived[5 + i];
                    }
                    ushort value16buton = CombineBytes16(parameterDoor.button, 2);
                    char[] bits16buton = ConvertToCharArray(value16buton, 8 * parameterDoor.button.Length);
                    if (bits16buton != null)
                    {
                        parameterDoor.key_cham = false; parameterDoor.key_xo = false;
                        parameterDoor.buttonregister = false; parameterDoor.buttonUnlock_Lock = false;
                        parameterDoor.doublecheck = false; parameterDoor.Silent = false; parameterDoor.Checkbutton = false;
                        if (bits16buton[0] != 0) parameterDoor.key_cham = true;
                        if (bits16buton[1] != 0) parameterDoor.key_xo = true;
                        if (bits16buton[11] != 0) parameterDoor.Checkbutton = true;
                        if (bits16buton[12] != 0) parameterDoor.Silent = true;
                        if (bits16buton[13] != 0) parameterDoor.doublecheck = true;
                        if (bits16buton[14] != 0) parameterDoor.buttonUnlock_Lock = true;
                        if (bits16buton[15] != 0) parameterDoor.buttonregister = true;
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        parameterDoor.sensor[i] = parameterDoor.DataRecived[7 + i];
                    }
                    uint value24sensor = CombineBytes32(parameterDoor.sensor, parameterDoor.sensor.Length);
                    char[] bits24sensor = ConvertToCharArray(value24sensor, 8 * parameterDoor.sensor.Length);
                    if (bits24sensor != null)
                    {
                        parameterDoor.location = false; parameterDoor.checkpos1_lock = false; parameterDoor.checkpos2_lock = false;
                        parameterDoor.motor1_lock = false; parameterDoor.motor2_lock = false; parameterDoor.check_sensor = false;
                        parameterDoor.unlock_lock1 = false; parameterDoor.unlock_lock2 = false;
                        if (bits24sensor[11] != 0) parameterDoor.check_sensor = true;
                        if (bits24sensor[12] != 0) parameterDoor.unlock_lock2 = true;
                        if (bits24sensor[14] != 0) parameterDoor.unlock_lock1 = true;
                        if (bits24sensor[16] != 0) parameterDoor.motor1_lock = true;
                        if (bits24sensor[17] != 0) parameterDoor.motor2_lock = true;
                        if (bits24sensor[21] != 0) parameterDoor.checkpos2_lock = true;
                        if (bits24sensor[22] != 0) parameterDoor.checkpos1_lock = true;
                        if (bits24sensor[23] != 0) parameterDoor.location = true;
                    }
                    parameterDoor.method = parameterDoor.DataRecived[10];
                    char[] bitmethod = GetBits(parameterDoor.method);
                    if (bitmethod != null)
                    {
                        parameterDoor.exalt = false;
                        if (bitmethod[6] != 0) parameterDoor.exalt = true;
                    }
                    for (int i = 0; i < 2; i++)
                    {
                        parameterDoor.degree[i] = parameterDoor.DataRecived[11 + i];
                    }
                    ushort value24degree = CombineBytes16(parameterDoor.degree, 2);
                    char[] bits24degree = ConvertToCharArray(value24sensor, 8 * parameterDoor.degree.Length);
                    if (bits24degree != null)
                    {
                        parameterDoor.Pin_code = false; parameterDoor.fingerprint = false;
                        parameterDoor.card = false; parameterDoor.button_i = false;
                        if (bits24degree[12] != 0) parameterDoor.button_i = true;
                        if (bits24degree[13] != 0) parameterDoor.fingerprint = true;
                        if (bits24degree[14] != 0) parameterDoor.card = true;
                        if (bits24degree[15] != 0) parameterDoor.Pin_code = true;
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        parameterDoor.config[i] = parameterDoor.DataRecived[13 + i];
                    }
                    uint value24config = CombineBytes32(parameterDoor.config, parameterDoor.config.Length);
                    char[] bits24config = ConvertToCharArray(value24config, 8 * parameterDoor.config.Length);
                    if (bits24config != null)
                    {
                        parameterDoor.R_autolock = false; parameterDoor.L_autolock = false; parameterDoor.R_sound = false;
                        parameterDoor.L_sound = false; parameterDoor.Speaker = false; parameterDoor.R_manuallock = false;
                        parameterDoor.L_manuallock = false; parameterDoor.doublecheck_unlock_lock = false;
                        if (bits24config[24] != 0) parameterDoor.doublecheck_unlock_lock = true;
                        if (bits24config[25] != 0) parameterDoor.L_manuallock = true;
                        if (bits24config[26] != 0) parameterDoor.R_manuallock = true;
                        if (bits24config[27] != 0) parameterDoor.Speaker = true;
                        if (bits24config[28] != 0) parameterDoor.L_sound = true;
                        if (bits24config[29] != 0) parameterDoor.R_sound = true;
                        if (bits24config[30] != 0) parameterDoor.L_autolock = true;
                        if (bits24config[31] != 0) parameterDoor.R_autolock = true;
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        parameterDoor.None[i] = parameterDoor.DataRecived[17 + i];
                    }
                }
            }
            catch (Exception ex)
            {
                OnEventException("CheckParameterDoor", ex.Message);
                return null;
            }
            return parameterDoor;
        }
        //5. Cài đặt mặc định khóa cửa
        public SetdeafautLock SetDefaultDoor(byte Value, int TimeOut)
        {
           // byte Value = 0;
            //switch (Type)
            //{
            //    case SetDefaultDoorType.GreateManual: { Value = 0xC5; break; }
            //    case SetDefaultDoorType.LeftNumManual: { Value = 0xD2; break; }
            //    case SetDefaultDoorType.GreatAuto: { Value = 0xCC; break; }
            //    case SetDefaultDoorType.LeftNumAuto: { Value = 0xDD; break; }
            //    case SetDefaultDoorType.WaitAutoLeftRight: { Value = 0x55; break; }
            //    case SetDefaultDoorType.NoSetLeftRight: { Value = 0xFF; break; }
            //    default: { Value = 0xFF; break; }
            //}

            SetdeafautLock DataDefaut = new SetdeafautLock();
            byte[] cmd = new byte[21];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x03;
            cmd[3] = 0x10;
            cmd[4] = 0x12;
            cmd[5] = 0x34;
            cmd[6] = 0x56;
            cmd[7] = 0x78;
            cmd[8] = 0x90;
            cmd[9] = 0xFF;
            cmd[10] = 0xFF;
            cmd[11] = 0x0A;
            cmd[12] = Value; //RightMan 0xC5 LeftMan 0xD2 RightAuto 0xCC LeftAuto 0xDD L&RAuto 0x55 None 0x00/0xFF
            for (int i = 0; i < 7; i++)
            {
                cmd[13 + i] = 0xFF;
            }
            cmd[20] = 0x03;

            ArrayByteToString_TX(ref TX_cmd, cmd);

            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd, 0, cmd.Length);
                OnEventTransferData(false, "SetDefaultDoor", TX_cmd);
                CheckByteToRead(ref m, TimeOut);
                DataDefaut.DataRecived = new byte[m];
                if (m > 0)
                {
                    Serial.Read(hi, 0, m);
                    for (int i = 0; i < m; i++)
                    {
                        DataDefaut.DataRecived[i] = hi[i];
                    }
                    ArrayByteToString_RX(ref RX_cmd, hi, m);
                    OnEventTransferData(true, "SetDefaultDoor", RX_cmd);
                }
                else
                {
                    throw new Exception("Recived Timeout!");
                }
                byte[] Bytes = { DataDefaut.DataRecived[12], DataDefaut.DataRecived[13] };
                DataDefaut.temperature = string.Join("", Array.ConvertAll(Bytes, b => b.ToString()));
                if (DataDefaut.DataRecived[9] == 0xFF && DataDefaut.DataRecived[10] == 0xFF && DataDefaut.DataRecived[11] == 0xFF)
                {

                    DataDefaut.checkFlash = true;
                }
                else
                {
                    DataDefaut.checkFlash = false;
                }
               
            }
            catch (Exception ex)
            {

                OnEventException("SetDefaultDoor", ex.Message);
                return null;
            }
            return DataDefaut;
        }

        //6. Kiểm tra trạng thái đầu vào khóa cử
        // Key: kiểm tra nút nào đang nhấn
        // SensorDoor: gạt khóa của ở giữa. Đẩy ra là mở. nhấn vào là Close
        // FingerPrint: Chạm vào là Open. bor ra là LidClose

        public DataKey checkDataKey(int TimeOut)
        {
            DataKey Datakey = new DataKey();
            byte[] cmd = new byte[5];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x0E;
            cmd[3] = 0x00;
            cmd[4] = 0x03;
            ArrayByteToString_TX(ref TX_cmd, cmd);

            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd, 0, cmd.Length);
                OnEventTransferData(false, "checkDataKey", TX_cmd);
                CheckByteToRead(ref m, TimeOut);
                Datakey.DataRecived = new byte[m];
                if (m > 0)
                {
                    Serial.Read(hi, 0, m);
                    for (int i = 0; i < m; i++)
                    {
                        Datakey.DataRecived[i] = hi[i];
                    }
                    ArrayByteToString_RX(ref RX_cmd, hi, m);
                    OnEventTransferData(true, "checkDataKey", RX_cmd);
                }
                else
                {
                    throw new Exception("Recived Timeout!");
                }
                if (Datakey.DataRecived != null)
                {
                    int power = Datakey.DataRecived[4]; int voltage = Datakey.DataRecived[5]; int SensorMortise = Datakey.DataRecived[6];
                    int key = Datakey.DataRecived[7]; int sensorDoor = Datakey.DataRecived[8]; int RTC = Datakey.DataRecived[9];
                    int L_R = Datakey.DataRecived[10]; int Auto_man = Datakey.DataRecived[11]; int incident = Datakey.DataRecived[12];
                    int lid = Datakey.DataRecived[13]; int UART = Datakey.DataRecived[14]; int figerlid = Datakey.DataRecived[15];
                    int ICTouch = Datakey.DataRecived[16]; int Volume = Datakey.DataRecived[17]; int implement = Datakey.DataRecived[19];
                    switch (power)
                    {
                        case 48: { Datakey.SourceUsed = "external source"; break; }
                        case 49: { Datakey.SourceUsed = "Battery"; break; }
                        default: { Datakey.SourceUsed = power.ToString(); break; }
                    }
                    switch (voltage)
                    {
                        case 48: { Datakey.Voltage = "Voltage Normal"; break; }
                        case 49: { Datakey.Voltage = "Voltage Low"; break; }
                        default: { Datakey.Voltage = voltage.ToString(); break; }
                    }
                    switch (SensorMortise)
                    {
                        case 0: { Datakey.SensorMortise = "Open"; break; }
                        case 1: { Datakey.SensorMortise = "Open and neutral sensor detection status"; break; }
                        case 2: { Datakey.SensorMortise = "Open and internal forced lock sensor detection status"; break; }
                        case 3: { Datakey.SensorMortise = "Lock sensor detection status"; break; }
                        case 4: { Datakey.SensorMortise = "Sensor detection status locked and neutral"; break; }
                        case 5: { Datakey.SensorMortise = "Internal locked and forced lock sensor detection status"; break; }
                        case 6: { Datakey.SensorMortise = "State Sensor Neutral"; break; }
                        case 7: { Datakey.SensorMortise = "Neutral and internal forced lock sensor detection statusl"; break; }
                        case 8: { Datakey.SensorMortise = "No Sensor"; break; }
                        case 9: { Datakey.SensorMortise = "Internal forced lock sensor detection status"; break; }
                        case 10: { Datakey.SensorMortise = "Sensor detection error status"; break; }
                        case 64: { Datakey.SensorMortise = "Lock sensor detection status"; break; }
                        case 131: { Datakey.SensorMortise = "Close"; break; }
                        case 128: { Datakey.SensorMortise = "Open"; break; }
                        default: { Datakey.SensorMortise = SensorMortise.ToString(); break; }
                    }
                    switch (key)
                    {
                        case 0: { Datakey.Key = "Press_0"; break; }
                        case 1: { Datakey.Key = "Press_1"; break; }
                        case 2: { Datakey.Key = "Press_2"; break; }
                        case 3: { Datakey.Key = "Press_3"; break; }
                        case 4: { Datakey.Key = "Press_4"; break; }
                        case 5: { Datakey.Key = "Press_5"; break; }
                        case 6: { Datakey.Key = "Press_6"; break; }
                        case 7: { Datakey.Key = "Press_7"; break; }
                        case 8: { Datakey.Key = "Press_8"; break; }
                        case 9: { Datakey.Key = "Press_9"; break; }
                        case 10: { Datakey.Key = "Press_*"; break; }
                        case 11: { Datakey.Key = "11"; break; }
                        case 12: { Datakey.Key = "Press_V"; break; }
                        case 13: { Datakey.Key = "Key Open/Lock"; break; }
                        case 14: { Datakey.Key = "Key Register"; break; }
                        case 15: { Datakey.Key = "Key Menu"; break; }
                        case 17: { Datakey.Key = "Key unknow"; break; }
                        case 19: { Datakey.Key = "Key protective"; break; }
                        case 255: { Datakey.Key = "Key input error"; break; }
                        default: { Datakey.Key = key.ToString(); break; }
                    }
                    switch (sensorDoor)
                    {
                        case 48: { Datakey.SensorDoor = "Close"; break; }
                        case 49: { Datakey.SensorDoor = "Open"; break; }
                        case 255: { Datakey.SensorDoor = "No sensor door"; break; }
                        default: { Datakey.SensorDoor = sensorDoor.ToString(); break; }

                    }
                    switch (RTC)
                    {
                        case 48: { Datakey.RTC = "IC RTC OK"; break; }
                        case 49: { Datakey.RTC = "IC RTC NG"; break; }
                        case 255: { Datakey.RTC = "No IC RTC"; break; }
                        default: { Datakey.RTC = RTC.ToString(); break; }
                    }
                    switch (L_R)
                    {
                        case 48: { Datakey.Left_Right = "Setup Right Status"; break; }
                        case 49: { Datakey.Left_Right = "Setup Left Status"; break; }
                        case 255: { Datakey.Left_Right = "No setup left and right"; break; }
                        default: { Datakey.Left_Right = L_R.ToString(); break; }
                    }
                    switch (Auto_man)
                    {
                        case 48: { Datakey.Auto_manual = "Setup Right Status"; break; }
                        case 49: { Datakey.Auto_manual = "Setup Left Status"; break; }
                        case 255: { Datakey.Auto_manual = "No setup left and right"; break; }
                        default: { Datakey.Auto_manual = Auto_man.ToString(); break; }
                    }
                    switch (incident)
                    {
                        case 48: { Datakey.Incident = "Condition: No damage detected"; break; }
                        case 49: { Datakey.Incident = "Condition: found damaged"; break; }
                        case 255: { Datakey.Incident = "There are no broken functions or unsupported functions"; break; }
                        default: { Datakey.Incident = incident.ToString(); break; }
                    }
                    switch (lid)
                    {
                        case 48: { Datakey.lid = "Lid close state"; break; }
                        case 49: { Datakey.lid = "Lid open state"; break; }
                        case 255: { Datakey.lid = "There is no lid switch or this function is not supported"; break; }
                        default: { Datakey.lid = lid.ToString(); break; }
                    }
                    switch (UART)
                    {
                        case 48: { Datakey.Communicate_UARTfingerprint = "No problem with fingerprint uart communication"; break; }
                        case 49: { Datakey.Communicate_UARTfingerprint = "Fingerprint UART communication error"; break; }
                        case 255: { Datakey.Communicate_UARTfingerprint = "Fingerprints UART communication are not supported"; break; }
                        default: { Datakey.Communicate_UARTfingerprint = UART.ToString(); break; }
                    }
                    switch (figerlid)
                    {
                        case 48: { Datakey.fingerprintLid = "Close"; break; }
                        case 49: { Datakey.fingerprintLid = "Open"; break; }
                        case 50: { Datakey.fingerprintLid = "Lid close auto state"; break; }
                        case 51: { Datakey.fingerprintLid = "Lid open auto state"; break; }
                        case 255: { Datakey.fingerprintLid = "There is no lid switch or this function is not supported"; break; }
                        default: { Datakey.fingerprintLid = figerlid.ToString(); break; }
                    }
                    switch (ICTouch)
                    {
                        case 48: { Datakey.IC_touch = "There is no error in Touch I2C communication"; break; }
                        case 49: { Datakey.IC_touch = "There is an error in Touch I2C communication"; break; }
                        case 255: { Datakey.IC_touch = "Touch is not supported"; break; }
                        default: { Datakey.IC_touch = ICTouch.ToString(); break; }
                    }
                    switch (Volume)
                    {
                        case 48: { Datakey.Volume = "Toggle silent volume"; break; }
                        case 49: { Datakey.Volume = "Toggle low volume"; break; }
                        case 50: { Datakey.Volume = "Toggle hight volume"; break; }
                        case 51: { Datakey.Volume = "Silent programmable volume"; break; }
                        case 52: { Datakey.Volume = "Low programmable volume"; break; }
                        case 53: { Datakey.Volume = "Hight programmable volume"; break; }
                        case 255: { Datakey.Volume = "Volume is not supported"; break; }
                        default: { Datakey.Volume = Volume.ToString(); break; }
                    }
                    switch (implement)
                    {
                        case 0: { Datakey.implement = "None"; break; }
                        case 1: { Datakey.implement = "Additional parameters"; break; }
                        case 10: { Datakey.implement = "parameters available"; break; }
                        case 255: { Datakey.implement = "implement is not supported"; break; }
                        default: { Datakey.implement = implement.ToString(); break; }
                    }
                }

            }
            catch (Exception ex)
            {
                OnEventException("checkDataKey", ex.Message);
                return null;
            }
            return Datakey;
        }
        //7. Kiểm tra trạng thái thêm đầu vào khóa cửa
        public CheckLockInputAddstatus CheckInputADoor(int TimeOut)
        {
            CheckLockInputAddstatus DataAdditional = new CheckLockInputAddstatus();
            byte[] cmd = new byte[6];

            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x0E;
            cmd[3] = 0x01;
            cmd[4] = 0x01;
            cmd[5] = 0x03;
            ArrayByteToString_TX(ref TX_cmd, cmd);

            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd, 0, cmd.Length);
                OnEventTransferData(false, "CheckInputADoor", TX_cmd);
                CheckByteToRead(ref m, TimeOut);
                DataAdditional.DataRecived = new byte[m];
                if (m > 0)
                {
                    Serial.Read(hi, 0, m);
                    for (int i = 0; i < m; i++)
                    {
                        DataAdditional.DataRecived[i] = hi[i];
                    }
                    ArrayByteToString_RX(ref RX_cmd, hi, m);
                    OnEventTransferData(true, "CheckInputADoor", RX_cmd);
                }
                else
                {
                    throw new Exception("Recived Timeout!");
                }
                if (DataAdditional.DataRecived != null)
                {
                    int hardware = DataAdditional.DataRecived[5]; int sensor2 = DataAdditional.DataRecived[6];
                    int EMPROM = DataAdditional.DataRecived[7]; int sortfware = DataAdditional.DataRecived[8];
                    int Additional = DataAdditional.DataRecived[19];
                    switch (hardware)
                    {
                        case 1: { DataAdditional.hardware = "check 9v front"; break; }
                        case 2: { DataAdditional.hardware = "Front reset needs to be confirmed"; break; }
                        case 4: { DataAdditional.hardware = "None"; break; }
                        case 8: { DataAdditional.hardware = "None"; break; }
                        case 16: { DataAdditional.hardware = "None"; break; }
                        case 32: { DataAdditional.hardware = "None"; break; }
                        case 64: { DataAdditional.hardware = "None"; break; }
                        case 128: { DataAdditional.hardware = "None"; break; }
                        case 255: { DataAdditional.hardware = "Front reset needs to be confirmed"; break; }
                        default: { DataAdditional.hardware = hardware.ToString(); break; }
                    }

                    switch (sensor2)
                    {
                        case 0: { DataAdditional.sensor_2 = "Open sensor detection status"; break; }
                        case 1: { DataAdditional.sensor_2 = "Open and neutral sensor detection status"; break; }
                        case 2: { DataAdditional.sensor_2 = "Open and internal forced lock sensor detection status"; break; }
                        case 3: { DataAdditional.sensor_2 = "Lock sensor detection status"; break; }
                        case 4: { DataAdditional.sensor_2 = "Sensor detection status locked and neutral"; break; }
                        case 5: { DataAdditional.sensor_2 = "Internal locked and forced lock sensor detection status"; break; }
                        case 6: { DataAdditional.sensor_2 = "State Sensor Neutral"; break; }
                        case 7: { DataAdditional.sensor_2 = "Neutral and internal forced lock sensor detection statusl"; break; }
                        case 8: { DataAdditional.sensor_2 = "No Sensor"; break; }
                        case 9: { DataAdditional.sensor_2 = "Internal forced lock sensor detection status"; break; }
                        case 10: { DataAdditional.sensor_2 = "Sensor detection error status"; break; }
                        case 64: { DataAdditional.sensor_2 = "Lock sensor detection status"; break; }
                        case 255: { DataAdditional.sensor_2 = "No sensor check2"; break; }
                        default: { DataAdditional.sensor_2 = sensor2.ToString(); break; }
                    }
                    switch (EMPROM)
                    {
                        case 48: { DataAdditional.EEPROM = "IC EEPROM OK"; break; }
                        case 49: { DataAdditional.EEPROM = "IC EEPROM NG"; break; }
                        case 255: { DataAdditional.EEPROM = "No IC EEPROM"; break; }
                        default: { DataAdditional.EEPROM = EMPROM.ToString(); break; }
                    }
                    switch (sortfware)
                    {
                        case 1: { DataAdditional.Sorftware = "Requires installation of FW Protect"; break; }
                        case 2: { DataAdditional.Sorftware = "Requires installation of FW Protect"; break; }
                        case 4: { DataAdditional.Sorftware = "Set the card's floating ID function"; break; }
                        case 8: { DataAdditional.Sorftware = "Set the card's floating ID function"; break; }
                        case 16: { DataAdditional.Sorftware = "Internal locked and forced lock sensor detection status"; break; }
                        case 32: { DataAdditional.Sorftware = "State Sensor Neutral"; break; }
                        case 64: { DataAdditional.Sorftware = "Neutral and internal forced lock sensor detection statusl"; break; }
                        case 128: { DataAdditional.Sorftware = "No Sensor"; break; }
                        case 255: { DataAdditional.Sorftware = "No sensor check2"; break; }
                        default: { DataAdditional.Sorftware = sortfware.ToString(); break; }
                    }

                }
            }
            catch (Exception ex)
            {
                OnEventException("CheckInputADoor", ex.Message);
                return null;
            }
            return DataAdditional;
        }
        //8. Kiểm tra trạng thái gói truyền thông cơ bản
        public communicationLock communication(int TimeOut)
        {
            communicationLock Data = new communicationLock();
            byte[] cmd = new byte[5];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x0E;
            cmd[3] = 0x00;
            cmd[4] = 0x03;
            ArrayByteToString_TX(ref TX_cmd, cmd);
            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd, 0, cmd.Length);
                OnEventTransferData(false, "communication", TX_cmd);
                CheckByteToRead(ref m, TimeOut);
                Data.DataRecived = new byte[m];
                if (m > 0)
                {
                    Serial.Read(hi, 0, m);
                    for (int i = 0; i < m; i++)
                    {
                        Data.DataRecived[i] = hi[i];
                    }
                    ArrayByteToString_RX(ref RX_cmd, hi, m);
                    OnEventTransferData(true, "communication", RX_cmd);
                }
                else
                {
                    throw new Exception("Recived Timeout!");
                }
                if (Data.DataRecived[4] == 0x02)
                {
                    int Byte1 = Data.DataRecived[12]; int Byte2 = Data.DataRecived[13];
                    switch (Byte1)
                    {
                        case 1: { Data.Byte1 = "xác nhận thành công"; break; }
                        case 2: { Data.Byte1 = "Xác minh không thành công"; break; }
                        case 3: { Data.Byte1 = "lỗi ID"; break; }
                    }
                    switch (Byte2)
                    {
                        case 1: { Data.Byte2 = "Gói giao tiếp iRevo (TRX/RX)"; break; }
                        case 2: { Data.Byte2 = "Gói giao tiếp BLE-N"; break; }
                        case 3: { Data.Byte2 = "Gói giao tiếp ZWave/Zigbee"; break; }
                    }

                }

            }
            catch (Exception ex)
            {
                OnEventException("communication", ex.Message);
                return null;
            }
            return Data;
        }
        //9. Xác nhận nhập mật khẩu
        public Password_Card_Figer ConfirmPass(int TimeOut)
        {
            Password_Card_Figer Data = new Password_Card_Figer();
            byte[] cmd = new byte[5];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x0E;
            cmd[3] = 0x00;
            cmd[4] = 0x03;
            ArrayByteToString_TX(ref TX_cmd, cmd);
            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                if (Mode_TCP_Serial == false)
                {
                    tcpClient.ReceiveTimeout = tcpClient_ReceiveTimeout;
                    tcpClient.Client.Send(cmd);
                    m = tcpClient.Client.Receive(hi);
                }
                else
                {
                    Serial.Write(cmd, 0, cmd.Length);
                    OnEventTransferData(false, "ConfirmPass", TX_cmd);
                    CheckByteToRead(ref m, TimeOut);
                    Data.DataRecived_Pass = new byte[m];
                    if (m > 0)
                    {
                        Serial.Read(hi, 0, m);
                        ArrayByteToString_RX(ref RX_cmd, hi, m);
                        OnEventTransferData(true, "Initial", RX_cmd);
                        for (int i = 0; i < m; i++)
                        {
                            Data.DataRecived_Pass[i] = hi[i];
                        }
                        Data.DataPass = BitConverter.ToString(Data.DataRecived_Pass, 12).Replace("-", " ");
                    }
                    else
                    {
                        throw new Exception("Recived Timeout!");
                    }
                }
                if (Data.DataRecived_Pass[4] == 0x01)
                {
                    if (Data.DataPass == "12 34 56 78 90 FF FF FF 03")
                    {
                        Data.DataPass_GetTrue = true;
                    }
                    else
                    {
                        Data.DataPass_GetTrue = false;
                    }
                }

            }
            catch (Exception ex)
            {
                OnEventException("ConfirmPass", ex.Message);
                return null;

            }
            return Data;
        }
        //10. Xác nhận nhập thẻ
        public Password_Card_Figer ConfirmCard(int TimeOut)
        {
            Password_Card_Figer Data = new Password_Card_Figer();
            byte[] cmd = new byte[5];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x0E;
            cmd[3] = 0x00;
            cmd[4] = 0x03;

            ArrayByteToString_TX(ref TX_cmd, cmd);
            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd, 0, cmd.Length);
                OnEventTransferData(false, "ConfirmCard", TX_cmd);
                CheckByteToRead(ref m, TimeOut);
                Data.DataRecived_ConfirmCard = new byte[m];
                if (m > 0)
                {
                    Serial.Read(hi, 0, m);
                    ArrayByteToString_RX(ref RX_cmd, hi, m);
                    OnEventTransferData(true, "ConfirmCard", RX_cmd);
                    for (int i = 0; i < m; i++)
                    {
                        Data.DataRecived_ConfirmCard[i] = hi[i];
                    }
                    Data.DataCard = BitConverter.ToString(Data.DataRecived_ConfirmCard, 12).Replace("-", " ");
                }
                else
                {
                    throw new Exception("Recived Timeout!");
                }

                if (Data.DataRecived_ConfirmCard[4] == 0x00)
                {
                    if (Data.DataCard == "01 01 FF FF FF FF FF FF 03")
                    {
                        Data.DataCard_GetTrue = true;
                    }
                    else
                    {
                        Data.DataCard_GetTrue = false;
                    }
                }

            }
            catch (Exception ex)
            {
                OnEventException("CheckInputADoor", ex.Message);
                return null;

            }
            return Data;

        }
        //11. Xác nhận nhập dấu vân tay
        public Password_Card_Figer ConfirmFingerPrint(int TimeOut)
        {
            Password_Card_Figer Data = new Password_Card_Figer();
            byte[] cmd = new byte[5];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x0E;
            cmd[3] = 0x00;
            cmd[4] = 0x03;

            ArrayByteToString_TX(ref TX_cmd, cmd);
            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd, 0, cmd.Length);
                OnEventTransferData(false, "ConfirmFingerPrint", TX_cmd);
                CheckByteToRead(ref m, TimeOut);
                Data.DataRecived_ConfirmFinger = new byte[m];
                if (m > 0)
                {
                    Serial.Read(hi, 0, m);
                    ArrayByteToString_RX(ref RX_cmd, hi, m);
                    OnEventTransferData(true, "ConfirmFingerPrint", RX_cmd);
                    for (int i = 0; i < m; i++)
                    {
                        Data.DataRecived_ConfirmFinger[i] = hi[i];
                    }
                    Data.DataFingerPrint = BitConverter.ToString(Data.DataRecived_ConfirmFinger, 12).Replace("-", " ");
                }
                else
                {
                    throw new Exception("Recived Timeout!");
                }
                if (Data.DataRecived_ConfirmFinger[4] == 0x00)
                {
                    if (Data.DataFingerPrint == "01 01 FF FF FF FF FF FF 03")
                    {
                        Data.DataFingerPrint_GetTrue = true;
                    }
                    else
                    {
                        Data.DataFingerPrint_GetTrue = false;
                    }
                }

            }
            catch (Exception ex)
            {
                OnEventException("ConfirmFingerPrint", ex.Message);
                return null;

            }
            return Data;

        }
        //12. Yêu cầu vận hành động cơ
        public bool RunMotorDoor(int TimeOut)
        {
            byte[] cmd = new byte[5];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x0F;
            cmd[3] = 0x00;
            cmd[4] = 0x03;

            ArrayByteToString_TX(ref TX_cmd, cmd);

            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd, 0, cmd.Length);
                OnEventTransferData(false, "RunMotorDoor", TX_cmd);
                //CheckByteToRead(ref m, TimeOut);
                return true;
            }
            catch (Exception ex)
            {
                OnEventException("RunMotorDoor", ex.Message);
                return false;
            }

        }
        // //13. Kết thúc kiểm tra khóa cửa
        public bool SleepMode(int TimeOut)
        {
            byte[] cmd = new byte[5];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x0C;
            cmd[3] = 0x00;
            cmd[4] = 0x03;
            ArrayByteToString_TX(ref TX_cmd, cmd);

            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd, 0, cmd.Length);
                OnEventTransferData(false, "Initial", TX_cmd);
                CheckByteToRead(ref m, TimeOut);
                return true;
            }
            catch (Exception ex)
            {
                OnEventException("CheckInputADoor", ex.Message);
                return false;
            }
        }
        //14. Cài đặt ngôn ngữ
        public languageClass ChangeLanguage(byte language, int TimeOut)
        {
            languageClass DataLanguage = new languageClass();
            byte[] cmd = new byte[10];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x12;
            cmd[3] = 0x05;
            cmd[4] = 0x01;
            cmd[5] = 0x01;
            cmd[6] = 0x00;
            cmd[7] = 0x07;
            cmd[8] = language; //Korean 0x01 china 0x02 Eng 0x03 Spain 0x04 Portugal 0x05 Taiwan 0x06
            cmd[9] = 0x03;

            ArrayByteToString_TX(ref TX_cmd, cmd);

            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                if (Mode_TCP_Serial == false)
                {
                    tcpClient.ReceiveTimeout = tcpClient_ReceiveTimeout;
                    tcpClient.Client.Send(cmd);
                    m = tcpClient.Client.Receive(hi);
                }
                else
                {
                    Serial.Write(cmd, 0, cmd.Length);
                    OnEventTransferData(false, "ChangeLanguage", TX_cmd);
                    CheckByteToRead(ref m, TimeOut);

                    DataLanguage.DataRecived = new byte[m];
                    if (m > 0)
                    {
                        Serial.Read(hi, 0, m);

                        ArrayByteToString_RX(ref RX_cmd, hi, m);
                        OnEventTransferData(true, "ChangeLanguage", RX_cmd);
                        for (int i = 0; i < m; i++)
                        {
                            DataLanguage.DataRecived[i] = hi[i];
                        }
                        DataLanguage.Data = BitConverter.ToString(DataLanguage.DataRecived).Replace("-", " ");
                    }
                    else
                    {
                        throw new Exception("Recived Timeout!");
                    }

                }
                if (DataLanguage.Data == "02 00 8D 04 01 01 00 07 03")
                {
                    DataLanguage.DataChageLanguageTrue = true;
                }
                else
                {
                    DataLanguage.DataChageLanguageTrue = false;
                }


            }
            catch (Exception ex)
            {
                OnEventException("CheckInputADoor", ex.Message);
                return null;
            }
            return DataLanguage;
        }
        //
        public languageClass OptionLanguage(byte language, int TimeOut)
        {
            languageClass OptionLanguage = new languageClass();
            byte[] cmd = new byte[10];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x12;
            cmd[3] = 0x05;
            cmd[4] = 0x01;
            cmd[5] = 0x01;
            cmd[6] = 0x1F;
            cmd[7] = 0xCD;
            cmd[8] = language; //NoOption 0x00 China 0x33
            cmd[9] = 0x03;
            ArrayByteToString_TX(ref TX_cmd, cmd);

            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd, 0, cmd.Length);
                OnEventTransferData(false, "OptionLanguage", TX_cmd);
                CheckByteToRead(ref m, TimeOut);

                OptionLanguage.Option_DataRecived = new byte[m];
                if (m > 0)
                {
                    Serial.Read(hi, 0, m);
                    ArrayByteToString_RX(ref RX_cmd, hi, m);
                    OnEventTransferData(true, "OptionLanguage", RX_cmd);

                    for (int i = 0; i < m; i++)
                    {
                        OptionLanguage.Option_DataRecived[i] = hi[i];
                    }
                    OptionLanguage.Option_Data = BitConverter.ToString(OptionLanguage.Option_DataRecived).Replace("-", " ");
                }
                else
                {
                    throw new Exception("Recived Timeout!");
                }

                if (OptionLanguage.Option_Data == "02 00 8D 04 01 01 1F CD 03")
                {
                    OptionLanguage.Option_DataGetTrue = true;
                }
                else
                {
                    OptionLanguage.Option_DataGetTrue = false;
                }
            }
            catch (Exception ex)
            {
                OnEventException("OptionLanguage", ex.Message);
                return null;
            }
            return OptionLanguage;
        }
        //16. MỞ cài đặt UID
        //Đây là quá trình cài đặt OPEN UID cho từng sản phẩm trong khóa cửa
        public OpenUIDclass OpenUID(byte uid, int TimeOut)
        {
            OpenUIDclass DataUID = new OpenUIDclass();
            byte[] cmd = new byte[10];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x12;
            cmd[3] = 0x05;
            cmd[4] = 0x01;
            cmd[5] = 0x01;
            cmd[6] = 0x00;
            cmd[7] = 0x0D;
            cmd[8] = uid; //Defaut 0xFF UseUID 0x74
            cmd[9] = 0x03;
            ArrayByteToString_TX(ref TX_cmd, cmd);
            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd, 0, cmd.Length);

                OnEventTransferData(false, "OpenUID", TX_cmd);
                CheckByteToRead(ref m, TimeOut);
                DataUID.DataRecived = new byte[m];
                if (m > 0)
                {
                    Serial.Read(hi, 0, m);
                    ArrayByteToString_RX(ref RX_cmd, hi, m);
                    OnEventTransferData(true, "OpenUID", RX_cmd);
                    for (int i = 0; i < m; i++)
                    {
                        DataUID.DataRecived[i] = hi[i];
                    }
                    DataUID.Data = BitConverter.ToString(DataUID.DataRecived).Replace("-", " ");
                }
                else
                {
                    throw new Exception("Recived Timeout!");
                }
                if (DataUID.Data == "02 00  8D 04 01 01 00 0D 03")
                {
                    DataUID.DataOpenUID_GetTrue = true;
                }
                else
                {
                    DataUID.DataOpenUID_GetTrue = false;
                }
            }
            catch (Exception ex)
            {
                OnEventException("OpenUID", ex.Message);
                return null;
            }
            return DataUID;
        }
        //17. Cài đặt ID nổi thẻ
        //Đây là quá trình cài đặt chức năng ID nổi của thẻ cho từng sản phẩm trong khóa cửa
        public IDCard SetupIDFloating(byte IDcard, int TimeOut)
        {
            IDCard DataIDFloating = new IDCard();
            byte[] cmd = new byte[10];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x12;
            cmd[3] = 0x05;
            cmd[4] = 0x01;
            cmd[5] = 0x01;
            cmd[6] = 0x1D;
            cmd[7] = 0x00;
            cmd[8] = IDcard; //Defaut 0xFF IDCard 0x3A
            cmd[9] = 0x03;
            ArrayByteToString_TX(ref TX_cmd, cmd);

            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                if (Mode_TCP_Serial == false)
                {
                    tcpClient.ReceiveTimeout = tcpClient_ReceiveTimeout;
                    tcpClient.Client.Send(cmd);
                    m = tcpClient.Client.Receive(hi);
                }
                else
                {
                    Serial.Write(cmd, 0, cmd.Length);
                    OnEventTransferData(false, "SetupIDFloating", TX_cmd);
                    CheckByteToRead(ref m, TimeOut);
                    DataIDFloating.FloatingID_DataRecived = new byte[m];
                    if (m > 0)
                    {
                        Serial.Read(hi, 0, m);
                        ArrayByteToString_RX(ref RX_cmd, hi, m);
                        OnEventTransferData(true, "SetupIDFloating", RX_cmd);
                        for (int i = 0; i < m; i++)
                        {
                            DataIDFloating.FloatingID_DataRecived[i] = hi[i];
                        }
                        DataIDFloating.FloatingID_Data = BitConverter.ToString(DataIDFloating.FloatingID_DataRecived).Replace("-", " ");
                    }
                    else
                    {
                        throw new Exception("Recived Timeout!");
                    }
                }
                if (DataIDFloating.FloatingID_Data == "02 00 8D 04 01 01 1D 00 03")
                {
                    DataIDFloating.DataFloatingID_GetTrue = true;
                }
                else
                {
                    DataIDFloating.DataFloatingID_GetTrue = false;
                }

            }
            catch (Exception ex)
            {
                OnEventException("CheckInputADoor", ex.Message);
                return null;
            }
            return DataIDFloating;
        }
        //18. Cài đặt thông tin sản phẩm
        //Đây là quá trình thiết lập thông tin duy nhất cho từng sản phẩm trong khóa cửa
        public IDCard SetupIdLock(byte IDdoor, int TimeOut)
        {
            IDCard IDLockData = new IDCard();
            byte[] cmd = new byte[14];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x12;
            cmd[3] = 0x05;
            cmd[4] = 0x01;
            cmd[5] = 0x05;
            cmd[6] = 0x1F;
            cmd[7] = 0xC8;
            cmd[8] = 0x12;
            cmd[9] = 0x34;
            cmd[10] = 0x56;
            cmd[11] = 0x78;
            cmd[12] = IDdoor; //UseID 0x1D UnusedID 0xFF
            cmd[13] = 0x03;
            ArrayByteToString_TX(ref TX_cmd, cmd);
            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                if (Mode_TCP_Serial == false)
                {
                    tcpClient.ReceiveTimeout = tcpClient_ReceiveTimeout;
                    tcpClient.Client.Send(cmd);
                    m = tcpClient.Client.Receive(hi);
                }
                else
                {
                    Serial.Write(cmd, 0, cmd.Length);
                    OnEventTransferData(false, "Initial", TX_cmd);
                    CheckByteToRead(ref m, TimeOut);
                    IDLockData.IDLock_DataRecived = new byte[m];
                    if (m > 0)
                    {
                        Serial.Read(hi, 0, m);
                        ArrayByteToString_RX(ref RX_cmd, hi, m);
                        OnEventTransferData(true, "Initial", RX_cmd);
                        for (int i = 0; i < m; i++)
                        {
                            IDLockData.IDLock_DataRecived[i] = hi[i];
                        }
                        IDLockData.IDLock_Data = BitConverter.ToString(IDLockData.IDLock_DataRecived).Replace("-", " ");
                    }
                    else
                    {
                        throw new Exception("Recived Timeout!");
                    }
                }

                if (IDLockData.IDLock_Data == "02 00 8D 04 01 05 1F C8 03")
                {
                    IDLockData.IDLock_GetTrue = true;
                }
                else
                {
                    IDLockData.IDLock_GetTrue = false;
                }

            }
            catch (Exception ex)
            {
                OnEventException("CheckInputADoor", ex.Message);
                return null;

            }
            return IDLockData;
        }
        //19. Cài đặt tên sản phẩm
        public NameLock NameDoor(int TimeOut)
        {
            NameLock NameLockData = new NameLock();
            //8bitHight
            byte[] cmd = new byte[17];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x12;
            cmd[3] = 0x05;
            cmd[4] = 0x01;
            cmd[5] = 0x08;
            cmd[6] = 0x1F;
            cmd[7] = 0xF0;
            cmd[8] = 0x12;
            cmd[9] = 0x34;
            cmd[10] = 0x56;
            cmd[11] = 0x78;
            cmd[12] = 0x9A;
            cmd[13] = 0xBC;
            cmd[14] = 0xDE;
            cmd[15] = 0xF0;
            cmd[16] = 0x03;
            ArrayByteToString_TX(ref TX_cmd, cmd);

            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd, 0, cmd.Length);
                OnEventTransferData(false, "NameDoor", TX_cmd);
                CheckByteToRead(ref m, TimeOut);
                NameLockData.HightbyteDataRecived = new byte[m];
                if (m > 0)
                {
                    Serial.Read(hi, 0, m);
                    ArrayByteToString_RX(ref RX_cmd, hi, m);
                    OnEventTransferData(true, "NameDoor", RX_cmd);
                    for (int i = 0; i < m; i++)
                    {
                        NameLockData.HightbyteDataRecived[i] = hi[i];
                    }
                    NameLockData.HightbyteData = BitConverter.ToString(NameLockData.HightbyteDataRecived).Replace("-", " ");
                }
                else
                {
                    throw new Exception("Recived Timeout!");
                }
                if (NameLockData.HightbyteData == "02 00 8D 04 01 08 1F F0 03")
                {
                    NameLockData.Hightbyte_GetTrue = true;
                }
                else
                {
                    NameLockData.Hightbyte_GetTrue = false;
                }
            }
            catch (Exception ex)
            {
                OnEventException("NameDoor", ex.Message);
                return null;
            }

            //8bitlow
            byte[] cmd2 = new byte[17];
            cmd2[0] = 0x02;
            cmd2[1] = 0x00;
            cmd2[2] = 0x12;
            cmd2[3] = 0x05;
            cmd2[4] = 0x01;
            cmd2[5] = 0x08;
            cmd2[6] = 0x1F;
            cmd2[7] = 0xF8;
            cmd2[8] = 0x21;
            cmd2[9] = 0x43;
            cmd2[10] = 0x65;
            cmd2[11] = 0x87;
            cmd2[12] = 0xA9;
            cmd2[13] = 0xCB;
            cmd2[14] = 0xED;
            cmd2[15] = 0x0F;
            cmd2[16] = 0x03;
            ArrayByteToString_TX(ref TX_cmd, cmd);

            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd2, 0, cmd2.Length);

                OnEventTransferData(false, "Initial", TX_cmd);
                CheckByteToRead(ref m, TimeOut);
                NameLockData.LowbyteDataRecived = new byte[m];
                if (m > 0)
                {
                    Serial.Read(hi, 0, m);
                    ArrayByteToString_RX(ref RX_cmd, hi, m);
                    OnEventTransferData(true, "Initial", RX_cmd);
                    for (int i = 0; i < m; i++)
                    {
                        NameLockData.LowbyteDataRecived[i] = hi[i];
                    }
                    NameLockData.LowbyteData = BitConverter.ToString(NameLockData.LowbyteDataRecived).Replace("-", " ");
                }
                else
                {
                    throw new Exception("Recived Timeout!");
                }
                if (NameLockData.LowbyteData == "02 00 8D 04 01 08 1F F8 03")
                {
                    NameLockData.Lowbyte_GetTrue = true;
                }
                else
                {
                    NameLockData.Lowbyte_GetTrue = false;
                }
            }
            catch (Exception ex)
            {
                OnEventException("CheckInputADoor", ex.Message);
                return null;
            }
            return NameLockData;
        }
        //20. Cài đặt số sê-ri
        public SerialLock SetSerialDoor(int TimeOut)
        {
            SerialLock Data = new SerialLock();
            byte[] cmd = new byte[21];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x01;
            cmd[3] = 0x10;
            cmd[4] = 0x12;
            cmd[5] = 0x34;
            cmd[6] = 0x56;
            cmd[7] = 0x78;
            cmd[8] = 0x9A;
            cmd[9] = 0xBC;
            cmd[10] = 0xDE;
            cmd[11] = 0xF0;
            cmd[12] = 0x21;
            cmd[13] = 0x43;
            cmd[14] = 0x65;
            cmd[15] = 0x87;
            cmd[16] = 0xA9;
            cmd[17] = 0xCB;
            cmd[18] = 0xED;
            cmd[19] = 0x0F;
            cmd[20] = 0x03;
            Data.SerialDoorTX = BitConverter.ToString(cmd, 4).Replace("-", " ");
            ArrayByteToString_TX(ref TX_cmd, cmd);
            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd, 0, cmd.Length);
                OnEventTransferData(false, "SeriDoor", TX_cmd);
                //CheckByteToRead(ref m, TimeOut);


            }
            catch (Exception ex)
            {
                OnEventException("SeriDoor", ex.Message);
                return null;
            }
            return Data;
        }
        //21. Truy vấn số sê-ri
        public SerialLock QuerySerialDoor(int TimeOut)
        {
            SerialLock DataSeri = new SerialLock();
            byte[] cmd = new byte[5];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x02;
            cmd[3] = 0x00;
            cmd[4] = 0x03;

            ArrayByteToString_TX(ref TX_cmd, cmd);

            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd, 0, cmd.Length);
                OnEventTransferData(false, "SeriDoor", TX_cmd);
                CheckByteToRead(ref m, TimeOut);

                DataSeri.DataRecived = new byte[m];
                if (m > 0)
                {
                    Serial.Read(hi, 0, m);
                    ArrayByteToString_RX(ref RX_cmd, hi, m);
                    OnEventTransferData(true, "SeriDoor", RX_cmd);

                    for (int i = 0; i < m; i++)
                    {
                        DataSeri.DataRecived[i] = hi[i];
                    }
                }
                else
                {
                    throw new Exception("Recived Timeout!");
                }
                DataSeri.DataquerySeri = BitConverter.ToString(DataSeri.DataRecived, 4).Replace("-", " ");
                if (DataSeri.DataquerySeri == DataSeri.SerialDoorTX)
                {
                    DataSeri.quertySeti_GetTrue = true;
                }
                else
                {
                    DataSeri.quertySeti_GetTrue = false;
                }
            }
            catch (Exception ex)
            {
                ExceptionCode = ex.ToString();
                NetworkIsOk = false;
                WriteMultiRegisterIsError = true;
                return null;
            }
            return DataSeri;
        }
        //22. Kiểm tra lịch sử kiểm tra
        public HistoryLock History(int TimeOut)
        {
            HistoryLock DataHis = new HistoryLock();
            byte[] cmd = new byte[5];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x13;
            cmd[3] = 0x00;
            cmd[4] = 0x03;

            ArrayByteToString_TX(ref TX_cmd, cmd);
            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                if (Mode_TCP_Serial == false)
                {
                    tcpClient.ReceiveTimeout = tcpClient_ReceiveTimeout;
                    tcpClient.Client.Send(cmd);
                    m = tcpClient.Client.Receive(hi);
                }
                else
                {
                    Serial.Write(cmd, 0, cmd.Length);
                    OnEventTransferData(false, "History", TX_cmd);
                    CheckByteToRead(ref m, TimeOut);
                    DataHis.DataRecived = new byte[m];
                    if (m > 0)
                    {
                        Serial.Read(hi, 0, m);
                        ArrayByteToString_RX(ref RX_cmd, hi, m);
                        OnEventTransferData(true, "History", RX_cmd);
                        for (int i = 0; i < m; i++)
                        {
                            DataHis.DataRecived[i] = hi[i];
                        }
                    }
                    else
                    {
                        throw new Exception("Recived Timeout!");
                    }

                }
                int Byte1 = DataHis.DataRecived[4]; int Byte2 = DataHis.DataRecived[5]; int Byte3 = DataHis.DataRecived[6]; int Byte4 = DataHis.DataRecived[7];
                int Byte5 = DataHis.DataRecived[8]; int Byte6 = DataHis.DataRecived[9]; int Byte7 = DataHis.DataRecived[10]; int Byte8 = DataHis.DataRecived[11];
                switch (Byte1)
                {
                    case 1: { DataHis.Byte1 = "Được khởi tạo bằng Factory Reset"; break; }
                    case 2: { DataHis.Byte1 = "Khởi tạo bằng PC (chương trình jig thử nghiệm sản xuất)"; break; }
                    case 3: { DataHis.Byte1 = "Được khởi tạo bởi JigPack N1 (gói bên trái và bên phải)"; break; }
                    case 4: { DataHis.Byte1 = "Được khởi tạo bởi JigPack N2 (Gói khởi tạo)"; break; }
                    default: { DataHis.Byte1 = "Không có dữ liệu"; break; }
                }
                switch (Byte2)
                {
                    case 1: { DataHis.Byte2 = "Được khởi tạo bằng Factory Reset"; break; }
                    case 2: { DataHis.Byte2 = "Khởi tạo bằng PC (chương trình jig thử nghiệm sản xuất)"; break; }
                    case 3: { DataHis.Byte2 = "Được khởi tạo bởi JigPack N1 (gói bên trái và bên phải)"; break; }
                    case 4: { DataHis.Byte2 = "Được khởi tạo bởi JigPack N2 (Gói khởi tạo)"; break; }
                    default: { DataHis.Byte2 = "Không có dữ liệu"; break; }
                }
                switch (Byte3)
                {
                    case 1: { DataHis.Byte3 = "Được khởi tạo bằng Factory Reset"; break; }
                    case 2: { DataHis.Byte3 = "Khởi tạo bằng PC (chương trình jig thử nghiệm sản xuất)"; break; }
                    case 3: { DataHis.Byte3 = "Được khởi tạo bởi JigPack N1 (gói bên trái và bên phải)"; break; }
                    case 4: { DataHis.Byte3 = "Được khởi tạo bởi JigPack N2 (Gói khởi tạo)"; break; }
                    default: { DataHis.Byte3 = "Không có dữ liệu"; break; }
                }
                switch (Byte4)
                {
                    case 1: { DataHis.Byte4 = "Được khởi tạo bằng Factory Reset"; break; }
                    case 2: { DataHis.Byte4 = "Khởi tạo bằng PC (chương trình jig thử nghiệm sản xuất)"; break; }
                    case 3: { DataHis.Byte4 = "Được khởi tạo bởi JigPack N1 (gói bên trái và bên phải)"; break; }
                    case 4: { DataHis.Byte4 = "Được khởi tạo bởi JigPack N2 (Gói khởi tạo)"; break; }
                    default: { DataHis.Byte4 = "Không có dữ liệu"; break; }
                }
                switch (Byte5)
                {
                    case 1: { DataHis.Byte5 = "Được khởi tạo bằng Factory Reset"; break; }
                    case 2: { DataHis.Byte5 = "Khởi tạo bằng PC (chương trình jig thử nghiệm sản xuất)"; break; }
                    case 3: { DataHis.Byte5 = "Được khởi tạo bởi JigPack N1 (gói bên trái và bên phải)"; break; }
                    case 4: { DataHis.Byte5 = "Được khởi tạo bởi JigPack N2 (Gói khởi tạo)"; break; }
                    default: { DataHis.Byte5 = "Không có dữ liệu"; break; }
                }
                switch (Byte6)
                {
                    case 1: { DataHis.Byte6 = "Được khởi tạo bằng Factory Reset"; break; }
                    case 2: { DataHis.Byte6 = "Khởi tạo bằng PC (chương trình jig thử nghiệm sản xuất)"; break; }
                    case 3: { DataHis.Byte6 = "Được khởi tạo bởi JigPack N1 (gói bên trái và bên phải)"; break; }
                    case 4: { DataHis.Byte6 = "Được khởi tạo bởi JigPack N2 (Gói khởi tạo)"; break; }
                    default: { DataHis.Byte6 = "Không có dữ liệu"; break; }
                }
                switch (Byte7)
                {
                    case 1: { DataHis.Byte7 = "Được khởi tạo bằng Factory Reset"; break; }
                    case 2: { DataHis.Byte7 = "Khởi tạo bằng PC (chương trình jig thử nghiệm sản xuất)"; break; }
                    case 3: { DataHis.Byte7 = "Được khởi tạo bởi JigPack N1 (gói bên trái và bên phải)"; break; }
                    case 4: { DataHis.Byte7 = "Được khởi tạo bởi JigPack N2 (Gói khởi tạo)"; break; }
                    default: { DataHis.Byte7 = "Không có dữ liệu"; break; }
                }
                switch (Byte8)
                {
                    case 1: { DataHis.Byte8 = "Được khởi tạo bằng Factory Reset"; break; }
                    case 2: { DataHis.Byte8 = "Khởi tạo bằng PC (chương trình jig thử nghiệm sản xuất)"; break; }
                    case 3: { DataHis.Byte8 = "Được khởi tạo bởi JigPack N1 (gói bên trái và bên phải)"; break; }
                    case 4: { DataHis.Byte8 = "Được khởi tạo bởi JigPack N2 (Gói khởi tạo)"; break; }
                    default: { DataHis.Byte8 = "Không có dữ liệu"; break; }
                }
                RX_cmd = "";
                for (int i = 0; i < m; i++)
                {
                    string hex = IntToHex2((int)hi[i]);
                    RX_cmd = RX_cmd + hex + " ";
                }
            }
            catch (Exception ex)
            {
                OnEventException("History", ex.Message);
                return null;
            }

            return DataHis;
        }
        //23. Yêu cầu đầu ra im lặng: 02 00 05 02 01 01 03
        //24. Yêu cầu đầu ra âm trầm: 02 00 05 02 01 21 03
        //25. Yêu cầu đầu ra treble:  02 00 05 02 01 41 03
        //26. Yêu cầu dừng in
        public Speak Speaker(SpeakerType type, int TimeOut)
        {
            byte speak = 0;
            switch (type)
            {
                case SpeakerType.Quiet: { speak = 0x01; break; }
                case SpeakerType.Low: { speak = 0x21; break; }
                case SpeakerType.High: { speak = 0x41; break; }
                case SpeakerType.Silent: { speak = 0x5E; break; }
                default: { speak = 0x01; break; }
            }
            Speak DataSpeak = new Speak();
            byte[] cmd = new byte[7];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x05;
            cmd[3] = 0x02;
            cmd[4] = 0x01;
            cmd[5] = speak; //Sinlent 01 Low 21 High 41 Stop 5E
            cmd[6] = 0x03;
            ArrayByteToString_TX(ref TX_cmd, cmd);

            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd, 0, cmd.Length);

                OnEventTransferData(false, "Speaker", TX_cmd);
                CheckByteToRead(ref m, TimeOut);
                DataSpeak.Datarecived = new byte[m];
                if (m > 0)
                {
                    Serial.Read(hi, 0, m);
                    ArrayByteToString_RX(ref RX_cmd, hi, m);
                    OnEventTransferData(true, "Speaker", RX_cmd);
                    for (int i = 0; i < m; i++)
                    {
                        DataSpeak.Datarecived[i] = hi[i];
                    }
                    DataSpeak.DataSpeak = BitConverter.ToString(DataSpeak.Datarecived).Replace("-", " ");
                }
                else
                {
                    throw new Exception("Recived Timeout!");
                }
                if (DataSpeak.DataSpeak == "02 00 8E 00 03")
                {
                    DataSpeak.DataSpeak_GetTrue = true;
                }
                else
                {
                    DataSpeak.DataSpeak_GetTrue = false;
                }
               
            }
            catch (Exception ex)
            {
                OnEventException("Speaker", ex.Message);
                return null;
            }
            return DataSpeak;
        }
        //27. Yêu cầu BẬT đèn LED BÀN PHÍM
        public OnLed LEDKeyOn(int TimeOut)
        {
            OnLed DataOnLed = new OnLed();
            byte[] cmd = new byte[7];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x05;
            cmd[3] = 0x02;
            cmd[4] = 0x01;
            cmd[5] = 0x45;
            cmd[6] = 0x03;
            ArrayByteToString_TX(ref TX_cmd, cmd);

            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                if (Mode_TCP_Serial == false)
                {
                    tcpClient.ReceiveTimeout = tcpClient_ReceiveTimeout;
                    tcpClient.Client.Send(cmd);
                    m = tcpClient.Client.Receive(hi);
                }
                else
                {
                    Serial.Write(cmd, 0, cmd.Length);
                    OnEventTransferData(false, "LEDKeyOn", TX_cmd);
                    CheckByteToRead(ref m, TimeOut);
                    DataOnLed.Datarecived = new byte[m];
                    if (m > 0)
                    {
                        Serial.Read(hi, 0, m);
                        ArrayByteToString_RX(ref RX_cmd, hi, m);
                        OnEventTransferData(true, "LEDKeyOn", RX_cmd);
                        for (int i = 0; i < m; i++)
                        {
                            DataOnLed.Datarecived[i] = hi[i];
                        }
                        DataOnLed.DataOnled = BitConverter.ToString(DataOnLed.Datarecived).Replace("-", " ");
                    }
                    else
                    {
                        throw new Exception("Recived Timeout!");
                    }

                }
                if (DataOnLed.DataOnled == "02 00 8E 00 03")
                {
                    DataOnLed.DataOnled_GetTrue = true;
                }
                else
                {
                    DataOnLed.DataOnled_GetTrue = false;
                }
              
            }
            catch (Exception ex)
            {
                OnEventException("LEDKeyOn", ex.Message);
                return null;
            }
            return DataOnLed;
        }
        //28. Yêu cầu tạo cảnh báo
        //29. Yêu cầu dừng báo động
        public Alarm WarningOnOff(WarningType On_Off, int TimeOut)
        {
            byte value = Convert.ToByte(On_Off);
            Alarm AlarmData = new Alarm();
            byte[] cmd = new byte[7];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x05;
            cmd[3] = 0x02;
            cmd[4] = 0x02;
            cmd[5] = value; //On 21 Off 41
            cmd[6] = 0x03;
            ArrayByteToString_TX(ref TX_cmd, cmd);
            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                if (Mode_TCP_Serial == false)
                {
                    tcpClient.ReceiveTimeout = tcpClient_ReceiveTimeout;
                    tcpClient.Client.Send(cmd);
                    m = tcpClient.Client.Receive(hi);
                }
                else
                {
                    Serial.Write(cmd, 0, cmd.Length);
                    OnEventTransferData(false, "WarningOnOff", TX_cmd);
                    CheckByteToRead(ref m, TimeOut);
                    AlarmData.Datarecived = new byte[m];
                    if (m > 0)
                    {
                        Serial.Read(hi, 0, m);
                        ArrayByteToString_RX(ref RX_cmd, hi, m);
                        OnEventTransferData(true, "WarningOnOff", RX_cmd);
                        for (int i = 0; i < m; i++)
                        {
                            AlarmData.Datarecived[i] = hi[i];
                        }
                        AlarmData.DataAlarm = BitConverter.ToString(AlarmData.Datarecived).Replace("-", " ");
                    }
                    else
                    {
                        throw new Exception("Recived Timeout!");
                    }

                }
                if (AlarmData.DataAlarm == "02 00 8E 00 03")
                {
                    AlarmData.DataAlarm_GetTrue = true;
                }
                else
                {
                    AlarmData.DataAlarm_GetTrue = false;
                }
               
            }
            catch (Exception ex)
            {

                OnEventException("WarningOnOff", ex.Message);
                return null;
            }
            return AlarmData;
        }
        //30. Yêu cầu thực hiện chế độ kiểm tra BLE APP
        //32. Yêu cầu thực hiện chế độ kiểm tra BLE RF
        public BLE TestBLEAPP_RF(byte BLE, int TimeOut)
        {
            BLE Data = new BLE();
            byte[] cmd = new byte[7];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x05;
            cmd[3] = 0x02;
            cmd[4] = 0x03;
            cmd[5] = BLE; //APP 88 RF 89
            cmd[6] = 0x03;
            ArrayByteToString_TX(ref TX_cmd, cmd);

            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd, 0, cmd.Length);
                OnEventTransferData(false, "TestBLEAPP_RF", TX_cmd);
                CheckByteToRead(ref m, TimeOut);
                Data.Datarecived_RF_APP = new byte[m];
                if (m > 0)
                {
                    Serial.Read(hi, 0, m);
                    ArrayByteToString_RX(ref RX_cmd, hi, m);
                    OnEventTransferData(true, "TestBLEAPP_RF", RX_cmd);
                    for (int i = 0; i < m; i++)
                    {
                        Data.Datarecived_RF_APP[i] = hi[i];
                    }
                    Data.Data_RF_APP = BitConverter.ToString(Data.Datarecived_RF_APP).Replace("-", " ");
                }
                else
                {
                    throw new Exception("Recived Timeout!");
                }
                if (Data.Data_RF_APP == "02 00 8E 00 03")
                {
                    Data.DataRF_APP_GetTrue = true;
                }
                else
                {
                    Data.DataRF_APP_GetTrue = false;
                }
               
            }
            catch (Exception ex)
            {
                OnEventException("TestBLEAPP_RF", ex.Message);
                return null;
            }
            return Data;
        }
        //31. Yêu cầu thực hiện chế độ kiểm tra BLE APP với giá trị khóa được thêm vào
        public BLE TestBLEAPPKeyPad(byte key, int TimeOut)
        {
            BLE Data = new BLE();
            byte[] cmd = new byte[8];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x05;
            cmd[3] = 0x03;
            cmd[4] = 0x03;
            cmd[5] = 0x87;
            cmd[6] = key;
            cmd[7] = 0x03;
            ArrayByteToString_TX(ref TX_cmd, cmd);

            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                if (Mode_TCP_Serial == false)
                {
                    tcpClient.ReceiveTimeout = tcpClient_ReceiveTimeout;
                    tcpClient.Client.Send(cmd);
                    m = tcpClient.Client.Receive(hi);
                }
                else
                {
                    Serial.Write(cmd, 0, cmd.Length);
                    OnEventTransferData(false, "TestBLEAPPKeyPad", TX_cmd);
                    CheckByteToRead(ref m, TimeOut);
                    Data.Datarecived_Keypad = new byte[m];
                    if (m > 0)
                    {
                        Serial.Read(hi, 0, m);

                        ArrayByteToString_RX(ref RX_cmd, hi, m);
                        OnEventTransferData(true, "TestBLEAPPKeyPad", RX_cmd);
                        for (int i = 0; i < m; i++)
                        {
                            Data.Datarecived_Keypad[i] = hi[i];
                        }
                        Data.Data_Keypad = BitConverter.ToString(Data.Datarecived_Keypad).Replace("-", " ");
                    }
                    else
                    {
                        throw new Exception("Recived Timeout!");
                    }

                }

                if (Data.Data_Keypad == "02 00 8E 00 03")
                {
                    Data.Data_Keypad_GetTrue = true;
                }
                else
                {
                    Data.Data_Keypad_GetTrue = false;
                }
            }
            catch (Exception ex)
            {
                OnEventException("TestBLEAPPKeyPad", ex.Message);
                return null;
            }
            return Data;
        }
        //32. Yêu cầu thực hiện chế độ kiểm tra BLE RF
        public BLE TestBLECBA(int TimeOut)
        {
            BLE Data = new BLE();
            byte[] cmd = new byte[8];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x15;
            cmd[3] = 0x03;
            cmd[4] = 0x01;
            cmd[5] = 0x22;
            cmd[6] = 0x33;
            cmd[7] = 0x03;
            ArrayByteToString_TX(ref TX_cmd, cmd);

            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                if (Mode_TCP_Serial == false)
                {
                    tcpClient.ReceiveTimeout = tcpClient_ReceiveTimeout;
                    tcpClient.Client.Send(cmd);
                    m = tcpClient.Client.Receive(hi);
                }
                else
                {
                    Serial.Write(cmd, 0, cmd.Length);
                    OnEventTransferData(false, "TestBLECBA", TX_cmd);
                    CheckByteToRead(ref m, TimeOut);
                    Data.Datarecived_CBA = new byte[m];
                    if (m > 0)
                    {
                        Serial.Read(hi, 0, m);
                        ArrayByteToString_RX(ref RX_cmd, hi, m);
                        OnEventTransferData(true, "TestBLECBA", RX_cmd);
                        for (int i = 0; i < m; i++)
                        {
                            Data.Datarecived_CBA[i] = hi[i];
                        }
                        Data.Data_CBA = BitConverter.ToString(Data.Datarecived_CBA).Replace("-", " ");
                    }
                    else
                    {
                        throw new Exception("Recived Timeout!");
                    }

                }
                if (Data.Data_CBA == "02 00 8E 00 03")
                {
                    Data.Data_CBA_GetTrue = true;
                }
                else
                {
                    Data.Data_CBA_GetTrue = false;
                }
              

            }
            catch (Exception ex)
            {
                OnEventException("TestBLECBA", ex.Message);
                return null;
            }
            return Data;
        }
        //33. Yêu cầu giao đơn hàng
        public External_device ExternalDevice(int TimeOut)
        {
            External_device Data = new External_device();
            byte[] cmd = new byte[8];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x15;
            cmd[3] = 0x03;
            cmd[4] = 0x01;
            cmd[5] = 0x22;
            cmd[6] = 0x33;
            cmd[7] = 0x03;
            ArrayByteToString_TX(ref TX_cmd, cmd);

            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                if (Mode_TCP_Serial == false)
                {
                    tcpClient.ReceiveTimeout = tcpClient_ReceiveTimeout;
                    tcpClient.Client.Send(cmd);
                    m = tcpClient.Client.Receive(hi);
                }
                else
                {
                    Serial.Write(cmd, 0, cmd.Length);
                    OnEventTransferData(false, "ExternalDevice", TX_cmd);
                    CheckByteToRead(ref m, TimeOut);
                    Data.DataRecived = new byte[m];
                    if (m > 0)
                    {
                        Serial.Read(hi, 0, m);
                        ArrayByteToString_RX(ref RX_cmd, hi, m);
                        OnEventTransferData(true, "ExternalDevice", RX_cmd);
                        for (int i = 0; i < m; i++)
                        {
                            Data.DataRecived[i] = hi[i];
                        }
                        Data.DataExternal = BitConverter.ToString(Data.DataRecived).Replace("-", " ");
                    }
                    else
                    {
                        throw new Exception("Recived Timeout!");
                    }

                }

            }
            catch (Exception ex)
            {
                OnEventException("ExternalDevice", ex.Message);
                return null;
            }
            return Data;
        }
        //34. Yêu cầu cài đặt Bảo vệ
        public bool SettingSecurity(ProtectType Protect, int TimeOut)
        {

            byte value = Convert.ToByte(Protect);

            byte[] cmd = new byte[7];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x17;
            cmd[3] = 0x01;
            cmd[4] = 0x01;
            cmd[5] = value; //NOSetup 0x00 Level1 0x01 Level2 0x02
            cmd[6] = 0x03;
            ArrayByteToString_TX(ref TX_cmd, cmd);

            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd, 0, cmd.Length);

                OnEventTransferData(false, "SeriDoor", TX_cmd);
                CheckByteToRead(ref m, TimeOut);
                if (m > 0)
                {
                    Serial.Read(hi, 0, m);
                    ArrayByteToString_RX(ref RX_cmd, hi, m);
                    OnEventTransferData(true, "SeriDoor", RX_cmd);
                }
                else
                {
                    throw new Exception("Recived Timeout!");
                }
                return true;
            }
            catch (Exception ex)
            {
                OnEventException("SeriDoor", ex.Message);
                return false;

            }
        }
        //34.1 
        public Protocol1 CheckPCBBLE(CheckProtocol Protocol,int TimeOut)
        {
            Protocol1 Data = new Protocol1();
            //CheckProtocol Protocol
            byte value = Convert.ToByte(Protocol);

            byte[] cmd = new byte[7];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x15;
            cmd[3] = 0x02;
            cmd[4] = 0x01;
            cmd[5] = value; //CheckRTC 0x00 Check_Vol 0x01 Check_Cur 0x02 Check_IOS 0x03 Check_VolBLE 0x04 Check BleRx 0x05 CheckSleepmode 0x06
            cmd[6] = 0x03;
            ArrayByteToString_TX(ref TX_cmd, cmd);
            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd, 0, cmd.Length);

                OnEventTransferData(false, "Protocol", TX_cmd);
                CheckByteToRead(ref m, TimeOut);
                if (m > 0)
                {
                    Serial.Read(hi, 0, m);
                    ArrayByteToString_RX(ref RX_cmd, hi, m);
                    OnEventTransferData(true, "Protocol", RX_cmd);
                    for (int i = 0; i<m; i++)
                    {
                        Data.Datarecived_Protocol[i] = hi[i];
                    }
                    Data.Data_Protocol = BitConverter.ToString(Data.Datarecived_Protocol).Replace("-", " ");
                }
                else
                {
                    throw new Exception("Recived Timeout!");
                }
                
            }
            catch (Exception ex)
            {
                OnEventException("Protocol", ex.Message);
                return null;
            }
            return Data;
        }
        //34.4 Set RT
        public RTC SetRTC (string date,int TimeOut )
        {
          
            DateTime dateset = DateTime.ParseExact(date, "yyyyMMdd", CultureInfo.InvariantCulture);
            // Tách các phần của chuỗi
            byte year = Convert.ToByte(date.Substring(2, 2)); // Lấy 2 số cuối của năm (23)
            byte month = Convert.ToByte(date.Substring(4, 2)); // Lấy tháng (10)
            byte day = Convert.ToByte(date.Substring(6, 2)); // Lấy ngày (11)
            //byte DayOfWeek = Convert.ToByte(dateset.DayOfWeek.ToString()); // Lấy thứ của ngày trên
            byte DayOfWeek = (byte)dateset.DayOfWeek;
            RTC Data = new RTC();
            byte[] cmd = new byte[9];
            cmd[0] = 0x00;
            cmd[1] = 0x00;
            cmd[2] = year;
            cmd[3] = month;
            cmd[4] = day;
            cmd[5] = DayOfWeek; 
            cmd[6] = 0x00; //hour
            cmd[7] = 0x00; //Minute
            cmd[8] = 0x00; //second
            ArrayByteToString_TX(ref TX_cmd, cmd);
            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd, 0, cmd.Length);

                OnEventTransferData(false, "SetRTC", TX_cmd);
                CheckByteToRead(ref m, TimeOut);
                if (m > 0)
                {
                    Serial.Read(hi, 0, m);
                    ArrayByteToString_RX(ref RX_cmd, hi, m);
                    OnEventTransferData(true, "SetRTC", RX_cmd);
                    for (int i = 0; i < m; i++)
                    {
                        Data.Datarecived_Set_RTC[i] = hi[i];
                    }
                    if (Data.Datarecived_Set_RTC[2] == cmd[0] && Data.Datarecived_Set_RTC[3] == cmd[1] && Data.Datarecived_Set_RTC[4] == cmd[2] &&
                        Data.Datarecived_Set_RTC[5] == cmd[3] && Data.Datarecived_Set_RTC[6] == cmd[4] && Data.Datarecived_Set_RTC[7] == cmd[5] &&
                        Data.Datarecived_Set_RTC[8] == cmd[6] && Data.Datarecived_Set_RTC[9] == cmd[7] && Data.Datarecived_Set_RTC[10] == cmd[8])
                    {
                        Data.Data_Set_RTC_GetTrue = true;
                    }
                    else
                    {
                        Data.Data_Set_RTC_GetTrue = false;
                    }
                    Data.Data_Set_RTC = BitConverter.ToString(Data.Datarecived_Set_RTC).Replace("-", " ");
                }
                else
                {
                    throw new Exception("Recived Timeout!");
                }
                
            }
            catch (Exception ex)
            {
                OnEventException("SetRTC", ex.Message);
                return null;

            }
            return Data;
        }
        //Get RTC
        public RTC GetRTC(int TimeOut)
        {
            RTC Data = new RTC();
            byte[] cmd = new byte[2];
            cmd[0] = 0x00;
            cmd[1] = 0xFF;
            ArrayByteToString_TX(ref TX_cmd, cmd);
            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd, 0, cmd.Length);

                OnEventTransferData(false, "GetRTC", TX_cmd);
                CheckByteToRead(ref m, TimeOut);
                if (m > 0)
                {
                    Serial.Read(hi, 0, m);
                    ArrayByteToString_RX(ref RX_cmd, hi, m);
                    OnEventTransferData(true, "GetRTC", RX_cmd);
                    Data.Datarecived_Get_RTC = new byte[m];
                    for (int i = 0; i < m; i++)
                    {
                        Data.Datarecived_Get_RTC[i] = hi[i];
                    }
                    if (Data.Datarecived_Set_RTC[2] == Data.Datarecived_Get_RTC[2] && Data.Datarecived_Set_RTC[3] == Data.Datarecived_Get_RTC[3] && Data.Datarecived_Set_RTC[4] == Data.Datarecived_Get_RTC[4] &&
                        Data.Datarecived_Set_RTC[5] == Data.Datarecived_Get_RTC[5] && Data.Datarecived_Set_RTC[6] == Data.Datarecived_Get_RTC[6] && Data.Datarecived_Set_RTC[7] == Data.Datarecived_Get_RTC[7] &&
                        Data.Datarecived_Set_RTC[8] == Data.Datarecived_Get_RTC[8] && Data.Datarecived_Set_RTC[9] == Data.Datarecived_Get_RTC[9] && Data.Datarecived_Set_RTC[10]== Data.Datarecived_Get_RTC[10])
                    {
                        Data.Data_Get_RTC_GetTrue =true;
                    }
                    else
                    {
                        Data.Data_Get_RTC_GetTrue = false;
                    }
                    Data.Data_Get_RTC = BitConverter.ToString(Data.Datarecived_Get_RTC).Replace("-", " ");
                }
                else
                {
                    throw new Exception("Recived Timeout!");
                }

            }
            catch (Exception ex)
            {
                OnEventException("GetRTC", ex.Message);
                return null;

            }
            return Data;
        }
        //34.5 VolPin
        public ADC_Check ADC_Check (int TimeOut)
        {
            ADC_Check Data = new ADC_Check();
            byte[] cmd = new byte[2];
            cmd[0] = 0x01;
            cmd[1] = 0x00;
            ArrayByteToString_TX(ref TX_cmd, cmd);
            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                Serial.Write(cmd, 0, cmd.Length);

                OnEventTransferData(false, "ADC_Check", TX_cmd);
                CheckByteToRead(ref m, TimeOut);
                if (m > 0)
                {
                    Serial.Read(hi, 0, m);
                    ArrayByteToString_RX(ref RX_cmd, hi, m);
                    OnEventTransferData(true, "ADC_Check", RX_cmd);
                    for (int i = 0; i < m; i++)
                    {
                        Data.Datarecived[i] = hi[i];
                    }
                    int[] Bytes = { Data.Datarecived[4], Data.Datarecived[5] };
                    Data.Data_ADC = string.Join("", Array.ConvertAll(Bytes, b => b.ToString()));

                }
                else
                {
                    throw new Exception("Recived Timeout!");
                }

            }
            catch (Exception ex)
            {
                OnEventException("ADC_Check", ex.Message);
                return null;

            }
            return Data;
        }
        public EndCheckLock DoneCheckLock(int TimeOut)
        {
            EndCheckLock Data = new EndCheckLock();
            byte[] cmd = new byte[7];
            cmd[0] = 0x02;
            cmd[1] = 0x00;
            cmd[2] = 0x05;
            cmd[3] = 0x02;
            cmd[4] = 0x03;
            cmd[5] = 0xFF;
            cmd[6] = 0x03;
            ArrayByteToString_TX(ref TX_cmd, cmd);
            try
            {
                byte[] hi = new byte[500];
                int m = 0;
                if (Mode_TCP_Serial == false)
                {
                    tcpClient.ReceiveTimeout = tcpClient_ReceiveTimeout;
                    tcpClient.Client.Send(cmd);
                    m = tcpClient.Client.Receive(hi);
                }
                else
                {
                    Serial.Write(cmd, 0, cmd.Length);
                    OnEventTransferData(false, "DoneCheckLock", TX_cmd);
                    CheckByteToRead(ref m, TimeOut);
                    Data.Datarecived = new byte[m];
                    if (m > 0)
                    {
                        Serial.Read(hi, 0, m);
                        ArrayByteToString_RX(ref RX_cmd, hi, m);
                        OnEventTransferData(true, "DoneCheckLock", RX_cmd);
                        for (int i = 0; i < m; i++)
                        {
                            Data.Datarecived[i] = hi[i];
                        }
                        Data.DataCheckEnd = BitConverter.ToString(Data.Datarecived).Replace("-", " ");
                    }
                    else
                    {
                        throw new Exception("Recived Timeout!");
                    }

                }
                if (Data.DataCheckEnd == "02 00 8E 00 03")
                {
                    Data.DataCheckEnd_GetTrue = true;
                }
                else
                {
                    Data.DataCheckEnd_GetTrue = false;
                }
                RX_cmd = "";
                for (int i = 0; i < m; i++)
                {
                    string hex = IntToHex2((int)hi[i]);
                    RX_cmd = RX_cmd + hex + " ";
                }
            }
            catch (Exception ex)
            {

                OnEventException("DoneCheckLock", ex.Message);
                return null;
            }
            return Data;
        }
        //

        //Event Send Data
        public event EventTransferData EventTransferDataLog;
        public delegate void EventTransferData(bool Type, string command, string content);
        public void OnEventTransferData(bool Type, string command, string content)
        {
            if (this.EventTransferDataLog != null)
            {
                this.EventTransferDataLog(Type, command, content);
            }
        }
        //Event Exception
        public event EventException EventExceptionLog;
        public delegate void EventException(string command, string content);
        public void OnEventException(string command, string content)
        {
            if (this.EventExceptionLog != null)
            {
                this.EventExceptionLog(command, content);
            }
        }

    }
    
} //class ModbusTCPIP

