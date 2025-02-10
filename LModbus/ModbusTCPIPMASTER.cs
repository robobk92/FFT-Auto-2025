using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using System.IO.Ports;
namespace LModbus
{
    public class ModbusTCPIPMASTER
    {
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
        public  bool NetworkIsOk = false;
        public string Datasending, DataRecived;
        public int numberOfTX;
        public string ExceptionCode = "";
        public string Status = "";
        public bool ReadHoldRegisterIsError, ReadCoilsIsError, WriteSingleCoilIsError, WriteMultiRegisterIsError;
        public string TX_cmd, RX_cmd;

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

        public  int HexToInt4(string HexString)
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

        public bool ConnectTCP(string ipAddress, int tcpPort)
        {
            Mode_TCP_Serial = false;
            return Connect1(ipAddress, tcpPort);
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

        private static bool CheckInternet()
        {
            //http://msdn.microsoft.com/en-us/library/windows/desktop/aa384702(v=vs.85).aspx
            // InternetConnectionState flag = InternetConnectionState.INTERNET_CONNECTION_LAN;
            return true;
        }

        private  bool Connect1(string ipAddress, int tcpPort)
        {
            m_ipAddress = ipAddress;
            m_tcpPort = tcpPort;
            if (tcpClient != null)
                tcpClient.Close();
            tcpClient = new TcpClient();
            if (CheckInternet())
            {
                try
                {
                        IAsyncResult asyncResult = tcpClient.BeginConnect(ipAddress, tcpPort, null, null);
                        asyncResult.AsyncWaitHandle.WaitOne(2000, true); //wait for 3 sec
                        if (!asyncResult.IsCompleted)
                        {
                            tcpClient.Close();
                            ExceptionCode = "ERROR" + DateTime.Now.ToString() + ":Cannot connect to server.";
                            Console.WriteLine(ExceptionCode);
                            return false;
                        }
                        else
                        { 
                        ExceptionCode = "";
                        Status = DateTime.Now.ToString() + ":Connected to server.";
                        Console.WriteLine(Status);
                        NetworkIsOk = true;
                        return true;
                        }
                        

                }
                catch (Exception ex)
                {
                    ExceptionCode = "ERROR" +  DateTime.Now.ToString() + ":Connect process " + ex.StackTrace + "==>" + ex.Message;
                    Console.WriteLine(DateTime.Now.ToString() + ":Connect process " + ex.StackTrace + "==>" + ex.Message);
                    NetworkIsOk = false;
                    return false;
                }
            }
            else return false;
        }
        private void Reconnect()
        {
            try
            {
                if (NetworkIsOk)
                {
                }
                else
                {
                    dtNow = DateTime.Now;
                    if ((dtNow - dtDisconnect) > TimeSpan.FromSeconds(10))
                    {
                        Console.WriteLine(DateTime.Now.ToString() + ":Start connecting");

                        NetworkIsOk = Connect1(m_ipAddress, m_tcpPort);
                        if (!NetworkIsOk)
                        {
                            ExceptionCode = "ERROR" + DateTime.Now.ToString() + ":Connecting fail. Wait for retry";
                            Console.WriteLine(ExceptionCode);
                            dtDisconnect = DateTime.Now;
                        }
                    }
                    else
                    {
                        Console.WriteLine(DateTime.Now.ToString() + ":Wait for retry connecting");
                    }

                }

            }
            catch (Exception exception)
            {
                if (exception.Source.Equals("System"))
                {
                    NetworkIsOk = false;
                    Console.WriteLine(exception.Message);
                    dtDisconnect = DateTime.Now;
                }
            }
        }

        public void Disconnect()
        {

            tcpClient.Close();

        } //class Disconnect

        public int[] ReadHoldingRegistersTCPIP(int startAddress, int numberOfPoints)
        {
            int[] value1 = new int[numberOfPoints];
            int temp2 = startAddress / 256;
            int temp1 = startAddress % 256;
            int temp3 = numberOfPoints % 256;
            int temp4 = numberOfPoints / 256;
            int temp5 = numberOfTX % 256;
            int temp6 = numberOfTX / 256;
            byte[] cmd = new byte[12];
            cmd[0] = Convert.ToByte(temp6);  
            cmd[1] = Convert.ToByte(temp5);
            cmd[2] = 0x00;
            cmd[3] = 0x00;
            cmd[4] = 0x00;
            cmd[5] = 0x06;
            cmd[6] = Convert.ToByte(ID);
            cmd[7] = 0x03;
            cmd[8] = Convert.ToByte(temp2);
            cmd[9] = Convert.ToByte(temp1);
            cmd[10] = Convert.ToByte(temp4);
            cmd[11] = Convert.ToByte(temp3);
            TX_cmd = "";
            for (int i = 0; i < 12; i++)
            {
                string hex = IntToHex2((int)cmd[i]);
                TX_cmd = TX_cmd + hex + " ";
            }
                try
            {
                tcpClient.ReceiveTimeout = tcpClient_ReceiveTimeout;
                tcpClient.Client.Send(cmd);
                byte[] hi = new byte[500];
                int m = tcpClient.Client.Receive(hi);
                //DataRecived = Encoding.ASCII.GetString(hi, 1, m - 1);
                RX_cmd = "";
                for (int i = 0; i < m; i++)
                {

                    string hex = IntToHex2((int)hi[i]);
                    RX_cmd = RX_cmd + hex + " ";
                }

                if (m > 2)
                {
                        if (hi[0] == cmd[0] && hi[1] == cmd[1] && hi[6] == cmd[6] && hi[7] == cmd[7] && hi[8] != 83 && hi[9] != 02)
                        {
                            numberOfTX++;
                            if (numberOfTX > 65535) numberOfTX = 0;
                            ReadHoldRegisterIsError = false;
                            Status = "ReadHoldingRegisters is OK " + numberOfTX.ToString();
                            for (int i = 0; i < numberOfPoints; i++)
                            {
                                value1[i] = hi[9 + i * 2] * 256 + hi[10 + (i * 2)];
                            }
                            ExceptionCode = "";
                           
                        }
                        else
                        {
                            if (hi[0] != cmd[0] || hi[1] != cmd[1])
                            {
                                int temptxrx = hi[0] * 256 + hi[1];
                                ExceptionCode = "ERROR! Modbus ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                                "Number of TX is " + numberOfTX.ToString() + Environment.NewLine +
                                                "Number of RX is " + temptxrx.ToString() + Environment.NewLine +
                                                "No Matching";
                            }

                            if (hi[5] == 0 && m == 6)
                                ExceptionCode = "ERROR! Modbus ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                                "Modbus have sent a wrong address ID to Slave device";
                            //if (hi[6] != cmd[6] || hi[7] != cmd[7])
                            //    ExceptionCode = "ERROR! Modbus ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                            //                    "Not Matching command";
                            if (hi[7] == 131 && hi[8] == 01)
                                ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                                "ILLEGAL FUNCTION";
                            if (hi[7] == 131 && hi[8] == 02)
                                ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                                "ILLEGAL DATA ADDRESS";
                            if (hi[7] == 131 && hi[8] == 03)
                                ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                                "ILLEGAL DATA VALUE";
                            if (hi[7] == 131 && hi[8] == 04)
                                ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                                "SLAVE DEVICE FAILURE";
                            ReadHoldRegisterIsError = true;
                         
                        }
                    }
            }
            catch (Exception ex)
            {
                ExceptionCode = ex.ToString();
                NetworkIsOk = false;
                Reconnect();
                ReadHoldRegisterIsError = true;
            }
            return value1;
        }

        public int[] ReadHoldingRegistersRTU(int startAddress, int numberOfPoints)
        {
            int[] value1 = new int[numberOfPoints];
            int temp2 = startAddress / 256;
            int temp1 = startAddress % 256;
            int temp3 = numberOfPoints % 256;
            int temp4 = numberOfPoints / 256;
            int temp5 = numberOfTX % 256;
            int temp6 = numberOfTX / 256;

            byte[] cmd = new byte[8];

                cmd[0] = Convert.ToByte(ID);
                cmd[1] = 0x03;
                cmd[2] = Convert.ToByte(temp2);
                cmd[3] = Convert.ToByte(temp1);
                cmd[4] = Convert.ToByte(temp4);
                cmd[5] = Convert.ToByte(temp3);
                byte[] checksum = CRC16(cmd);
                cmd[6] = checksum[0];
                cmd[7] = checksum[1];
                TX_cmd = "";
                for (int i = 0; i < 8; i++)
                {
                    string hex = IntToHex2((int)cmd[i]);
                    TX_cmd = TX_cmd + hex + " ";
                }


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
                    int lastbytes = 0;
                    int ByteWait = 0;
                    for (int i = 0; i < 200000; i++)
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
                    if (m > 0)
                    {
                        Serial.Read(hi, 0, m);
                    }
                    else
                    {
                        throw new Exception("Recived Timeout!");
                    }
                    
                }

                //DataRecived = Encoding.ASCII.GetString(hi, 1, m - 1);

                RX_cmd = "";
                for (int i = 0; i < m; i++)
                {

                    string hex = IntToHex2((int)hi[i]);
                    RX_cmd = RX_cmd + hex + " ";
                }
                if (m > 2)
                {
                        if (hi[0] == cmd[0] && hi[1] == cmd[1])
                        {
                            numberOfTX++;
                            if (numberOfTX > 65535) numberOfTX = 0;
                            ReadHoldRegisterIsError = false;
                            Status = "ReadHoldingRegisters is OK " + numberOfTX.ToString();
                            for (int i = 0; i < numberOfPoints; i++)
                            {
                                value1[i] = hi[3 + i * 2] * 256 + hi[4 + (i * 2)];
                            }
                            ExceptionCode = "";
                           
                        }
                        else
                        {

                            if (hi[1] == 131 && hi[2] == 01)
                                ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                                "ILLEGAL FUNCTION";
                            if (hi[1] == 131 && hi[2] == 02)
                                ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                                "ILLEGAL DATA ADDRESS";
                            if (hi[1] == 131 && hi[2] == 03)
                                ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                                "ILLEGAL DATA VALUE";
                            if (hi[1] == 131 && hi[2] == 04)
                                ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                                "SLAVE DEVICE FAILURE";
                            ReadHoldRegisterIsError = true;
                          
                        }
                    }
            }
            catch (Exception ex)
            {
                ExceptionCode = ex.ToString();
                NetworkIsOk = false;
                if (Mode_TCP_Serial == false)
                Reconnect();
                ReadHoldRegisterIsError = true;
              
            }
            return value1;
        }

        public bool[] ReadCoilsTCPIP(int startAddress, int numberOfPoints)
        {
            bool[] value1 = new bool[numberOfPoints];
            int temp2 = startAddress / 256;
            int temp1 = startAddress % 256;
            int temp3 = numberOfPoints % 256;
            int temp4 = numberOfPoints / 256;
            int temp5 = numberOfTX % 256;
            int temp6 = numberOfTX / 256;
            byte[] cmd = new byte[12];
            cmd[0] = Convert.ToByte(temp6);  
            cmd[1] = Convert.ToByte(temp5);
            cmd[2] = 0x00;
            cmd[3] = 0x00;
            cmd[4] = 0x00;
            cmd[5] = 0x06;
            cmd[6] = Convert.ToByte(ID);
            cmd[7] = 0x01;
            cmd[8] = Convert.ToByte(temp2);
            cmd[9] = Convert.ToByte(temp1);
            cmd[10] = Convert.ToByte(temp4);
            cmd[11] = Convert.ToByte(temp3);
            TX_cmd = "";
            for (int i = 0; i < 12; i++)
            {
                string hex = IntToHex2((int)cmd[i]);
                TX_cmd = TX_cmd + hex + " ";
            }
            try
            {
                tcpClient.ReceiveTimeout = tcpClient_ReceiveTimeout;
                tcpClient.Client.Send(cmd);
                byte[] hi = new byte[500];
                int m = tcpClient.Client.Receive(hi);
                RX_cmd = "";
                for (int i = 0; i < m; i++)
                {

                    string hex = IntToHex2((int)hi[i]);
                    RX_cmd = RX_cmd + hex + " ";
                }
                if (m > 2)
                {
                    if (hi[0] == cmd[0] && hi[1] == cmd[1] && hi[5] != 0 && hi[6] == cmd[6] && hi[7] == cmd[7] && hi[8] != 83 && hi[9] != 02)
                    {
                        numberOfTX++;
                        if (numberOfTX > 65535) numberOfTX = 0;
                        ReadCoilsIsError = false;
                        Status = "ReadCoils is OK " + numberOfTX.ToString();
                        for (int i = 0; i < hi[8]; i++)
                        {
                            bool[] value2 = new bool[8];
                            value2 = Int8ToBools(hi[i+9]);
                            for (int j = 0; j < 8; j++)
                            {
                                value1[j + 8 * i] = value2[j];
                            }
                        }
                        ExceptionCode = "";
                    }
                    else
                    {
                        if (hi[0] != cmd[0] || hi[1] != cmd[1])
                        {
                            int temptxrx = hi[0] * 256 + hi[1];
                            ExceptionCode = "ERROR! Modbus ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "Number of TX is " + numberOfTX.ToString() + Environment.NewLine +
                                            "Number of RX is " + temptxrx.ToString() + Environment.NewLine +
                                            "No Matching";
                        }

                        if (hi[7] == 131 && hi[8] == 01)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "ILLEGAL FUNCTION";
                        if (hi[7] == 131 && hi[8] == 02)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "ILLEGAL DATA ADDRESS";
                        if (hi[7] == 131 && hi[8] == 03)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "ILLEGAL DATA VALUE";
                        if (hi[7] == 131 && hi[8] == 04)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "SLAVE DEVICE FAILURE";
                        ReadCoilsIsError = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionCode = ex.ToString();
                NetworkIsOk = false;
                Reconnect();
                ReadCoilsIsError = true;
            }
            return value1;
        }

        public bool[] ReadCoilsRTU(int startAddress, int numberOfPoints)
        {
            bool[] value1 = new bool[numberOfPoints];
            int temp2 = startAddress / 256;
            int temp1 = startAddress % 256;
            int temp3 = numberOfPoints % 256;
            int temp4 = numberOfPoints / 256;
            byte[] cmd = new byte[8];
            cmd[0] = Convert.ToByte(ID);
            cmd[1] = 0x01;
            cmd[2] = Convert.ToByte(temp2);
            cmd[3] = Convert.ToByte(temp1);
            cmd[4] = Convert.ToByte(temp4);
            cmd[5] = Convert.ToByte(temp3);
            byte[] checksum = CRC16(cmd);
            cmd[6] = checksum[0];
            cmd[7] = checksum[1];

            TX_cmd = "";
            for (int i = 0; i < 8; i++)
            {
                string hex = IntToHex2((int)cmd[i]);
                TX_cmd = TX_cmd + hex + " ";
            }
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
                    for (int i = 0; i < 10000; i++)
                    {
                        if (Serial.BytesToRead > 2)
                        {
                            break;
                        }
                    }
                    m = Serial.BytesToRead;
                    if (m > 0)
                    {
                        Serial.Read(hi, 0, m);
                    }
                    else
                    {
                        throw new Exception("Recived Timeout!");
                    }

                }


                //----------------------------
                RX_cmd = "";
                for (int i = 0; i < m; i++)
                {

                    string hex = IntToHex2((int)hi[i]);
                    RX_cmd = RX_cmd + hex + " ";
                }
                if (m > 2)
                {
                    if (hi[0] == cmd[0] && hi[1] == cmd[1])
                    {
                        //numberOfTX++;
                        //if (numberOfTX > 65535) numberOfTX = 0;
                        ReadCoilsIsError = false;
                        Status = "ReadCoils is OK " + numberOfTX.ToString();
                        for (int i = 0; i < hi[2]; i++)
                        {
                            bool[] value2 = new bool[8];
                            value2 = Int8ToBools(hi[i + 3]);
                            for (int j = 0; j < 8; j++)
                            {
                                value1[j + 8 * i] = value2[j];
                            }
                        }
                        ExceptionCode = "";
                    }
                    else
                    {

                        if (hi[1] == 131 && hi[2] == 01)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "ILLEGAL FUNCTION";
                        if (hi[1] == 131 && hi[2] == 02)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "ILLEGAL DATA ADDRESS";
                        if (hi[1] == 131 && hi[2] == 03)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "ILLEGAL DATA VALUE";
                        if (hi[1] == 131 && hi[2] == 04)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "SLAVE DEVICE FAILURE";
                        ReadCoilsIsError = true;

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionCode = ex.ToString();
                NetworkIsOk = false;
                if (Mode_TCP_Serial == false)
                    Reconnect();
                ReadCoilsIsError = true;
            }
            return value1;
        }

        public void WriteSingleCoilTCPIP(int coilAddress, bool value)
        {
            int temp2 = coilAddress / 256;
            int temp1 = coilAddress % 256;
            int temp3 = 0;
            int temp4;
            if (value) temp4 = 255; else temp4 = 0;
            int temp5 = numberOfTX % 256;
            int temp6 = numberOfTX / 256;

            byte[] cmd = new byte[12];
            cmd[0] = Convert.ToByte(temp6);  
            cmd[1] = Convert.ToByte(temp5);
            cmd[2] = 0x00;
            cmd[3] = 0x00;
            cmd[4] = 0x00;
            cmd[5] = 0x06;
            cmd[6] = Convert.ToByte(ID);
            cmd[7] = 0x05;
            cmd[8] = Convert.ToByte(temp2);
            cmd[9] = Convert.ToByte(temp1);
            cmd[10] = Convert.ToByte(temp4);
            cmd[11] = Convert.ToByte(temp3);
            TX_cmd = "";
            for (int i = 0; i < 12; i++)
            {
                string hex = IntToHex2((int)cmd[i]);
                TX_cmd = TX_cmd + hex + " ";
            }

            try
            {
                tcpClient.ReceiveTimeout = 3000;
                tcpClient.Client.Send(cmd);
                byte[] hi = new byte[500];
                int m = tcpClient.Client.Receive(hi);
                //DataRecived = Encoding.ASCII.GetString(hi, 1, m - 1);
                RX_cmd = "";
                for (int i = 0; i < m; i++)
                {

                    string hex = IntToHex2((int)hi[i]);
                    RX_cmd = RX_cmd + hex + " ";
                }

                if (m > 2)
                {
                    if (hi[0] == cmd[0] && hi[1] == cmd[1] && hi[5] != 0 && hi[6] == cmd[6] && hi[7] == cmd[7] && hi[8] != 83 && hi[9] != 02)
                    {
                        numberOfTX++;
                        if (numberOfTX > 65535) numberOfTX = 0;
                        WriteSingleCoilIsError = false;
                        Status = "WriteSingleCoil is OK " + numberOfTX.ToString();
                        ExceptionCode = "";

                    }
                    else
                    {
                        if (hi[0] != cmd[0] || hi[1] != cmd[1])
                        {
                            int temptxrx = hi[0] * 256 + hi[1];
                            ExceptionCode = "ERROR! Modbus ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "Number of TX is " + numberOfTX.ToString() + Environment.NewLine +
                                            "Number of RX is " + temptxrx.ToString() + Environment.NewLine +
                                            "No Matching";
                        }

                        if (hi[5] == 0 && m == 6)
                            ExceptionCode = "ERROR! Modbus ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "Modbus have sent a wrong address ID to Slave device";
                        
                        if (hi[7] == 131 && hi[8] == 01)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "ILLEGAL FUNCTION";
                        if (hi[7] == 131 && hi[8] == 02)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "ILLEGAL DATA ADDRESS";
                        if (hi[7] == 131 && hi[8] == 03)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "ILLEGAL DATA VALUE";
                        if (hi[7] == 131 && hi[8] == 04)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "SLAVE DEVICE FAILURE";
                        WriteSingleCoilIsError = true;

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionCode = ex.ToString();
                NetworkIsOk = false;
                Reconnect();
                WriteSingleCoilIsError = true;
            }

        }

        public void WriteSingleCoilRTU(int coilAddress, bool value)
        {
            int temp2 = coilAddress / 256;
            int temp1 = coilAddress % 256;
            int temp3 = 0;
            int temp4;
            if (value) temp4 = 255; else temp4 = 0;


            byte[] cmd = new byte[8];
            cmd[0] = Convert.ToByte(ID);
            cmd[1] = 0x05;
            cmd[2] = Convert.ToByte(temp2);
            cmd[3] = Convert.ToByte(temp1);
            cmd[4] = Convert.ToByte(temp4);
            cmd[5] = Convert.ToByte(temp3);
            byte[] checksum = CRC16(cmd);
            cmd[6] = checksum[0];
            cmd[7] = checksum[1];

            TX_cmd = "";
            for (int i = 0; i < 8; i++)
            {
                string hex = IntToHex2((int)cmd[i]);
                TX_cmd = TX_cmd + hex + " ";
            }
            Console.WriteLine(TX_cmd);
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
                    int lastbytes = 0;
                    int ByteWait = 0;
                    for (int i = 0; i < 200000; i++)
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
                    if (m > 0)
                    {
                        Serial.Read(hi, 0, m);
                    }
                    else
                    {
                        throw new Exception("Recived Timeout!");
                    }

                }

                //DataRecived = Encoding.ASCII.GetString(hi, 1, m - 1);
                RX_cmd = "";
                for (int i = 0; i < m; i++)
                {

                    string hex = IntToHex2((int)hi[i]);
                    RX_cmd = RX_cmd + hex + " ";
                }

                if (m > 2)
                {
                    if (hi[0] == cmd[0] && hi[1] == cmd[1])
                    {
                        numberOfTX++;
                        if (numberOfTX > 65535) numberOfTX = 0;
                        WriteSingleCoilIsError = false;
                        Status = "WriteSingleCoil is OK " + numberOfTX.ToString();
                        ExceptionCode = "";

                    }
                    else
                    {
                        if (hi[1] == 131 && hi[2] == 01)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "ILLEGAL FUNCTION";
                        if (hi[1] == 131 && hi[2] == 02)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "ILLEGAL DATA ADDRESS";
                        if (hi[1] == 131 && hi[2] == 03)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "ILLEGAL DATA VALUE";
                        if (hi[1] == 131 && hi[2] == 04)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "SLAVE DEVICE FAILURE";
                        WriteSingleCoilIsError = true;

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionCode = ex.ToString();
                NetworkIsOk = false;
                if (Mode_TCP_Serial == false)
                    Reconnect();
                WriteSingleCoilIsError = true;
            }

        }

        public void WriteMultipleRegisters(int startAddress, int[] data)
        {
            int datalength = data.Length * 2 + 7;

            int temp2 = startAddress / 256;
            int temp1 = startAddress % 256;

            int temp3 = data.Length % 256;
            int temp4 = data.Length / 256;

            int temp5 = numberOfTX % 256;
            int temp6 = numberOfTX / 256;

            int temp7 = datalength % 256;
            int temp8 = datalength / 256;

            byte[] cmd = new byte[13+ data.Length * 2];
            cmd[0] = Convert.ToByte(temp6);  // 00 00 00 00 00 06 01 03 00 00 00 0A
            cmd[1] = Convert.ToByte(temp5);
            cmd[2] = 0x00;
            cmd[3] = 0x00;
            cmd[4] = 0x00;
            cmd[5] = Convert.ToByte(temp7);
            cmd[6] = Convert.ToByte(ID);
            cmd[7] = 0x10;
            cmd[8] = Convert.ToByte(temp2);
            cmd[9] = Convert.ToByte(temp1);
            cmd[10] = Convert.ToByte(temp4);
            cmd[11] = Convert.ToByte(temp3);
            cmd[12] = Convert.ToByte(data.Length * 2);
            for (int i = 0; i < data.Length; i++)
            {
               
                int temp9 = data[i] % 256;
                int temp10 = data[i] / 256;

                cmd[13+i*2] = Convert.ToByte(temp10);
                cmd[14+i * 2] = Convert.ToByte(temp9);
            }

            TX_cmd = "";
            for (int i = 0; i < cmd.Length; i++)
            {
                string hex = IntToHex2((int)cmd[i]);
                TX_cmd = TX_cmd + hex + " ";
            }

            try
            {
                tcpClient.ReceiveTimeout = 3000;
                tcpClient.Client.Send(cmd);
                byte[] hi = new byte[500];
                int m = tcpClient.Client.Receive(hi);
                RX_cmd = "";
                for (int i = 0; i < m; i++)
                {

                    string hex = IntToHex2((int)hi[i]);
                    RX_cmd = RX_cmd + hex + " ";
                }

                if (m > 2)
                {
                    if (hi[0] == cmd[0] && hi[1] == cmd[1] && hi[5] != 0 && hi[6] == cmd[6] && hi[7] == cmd[7] && hi[8] != 83 && hi[9] != 02)
                    {
                        numberOfTX++;
                        if (numberOfTX > 65535) numberOfTX = 0;
                        WriteMultiRegisterIsError = false;
                        Status = "WriteMultiRegister is OK " + numberOfTX.ToString();
                        ExceptionCode = "";
                    }
                    else
                    {
                        if (hi[0] != cmd[0] || hi[1] != cmd[1])
                        {
                            int temptxrx = hi[0] * 256 + hi[1];
                            ExceptionCode = "ERROR! Modbus ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "Number of TX is " + numberOfTX.ToString() + Environment.NewLine +
                                            "Number of RX is " + temptxrx.ToString() + Environment.NewLine +
                                            "No Matching";
                        }

                        if (hi[5] == 0 && m == 6)
                            ExceptionCode = "ERROR! Modbus ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "Modbus have sent a wrong address ID to Slave device";

                        if (hi[7] == 131 && hi[8] == 01)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "ILLEGAL FUNCTION";
                        if (hi[7] == 131 && hi[8] == 02)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "ILLEGAL DATA ADDRESS";
                        if (hi[7] == 131 && hi[8] == 03)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "ILLEGAL DATA VALUE";
                        if (hi[7] == 131 && hi[8] == 04)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "SLAVE DEVICE FAILURE";
                        WriteMultiRegisterIsError = true;

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionCode = ex.ToString();
                NetworkIsOk = false;
                 if (Mode_TCP_Serial == false)
                Reconnect();
                WriteMultiRegisterIsError = true;
            }

        }

        public void WriteMultipleRegistersRTU(int startAddress, int[] data)
        {
            int datalength = data.Length * 2 + 7;

            int temp2 = startAddress / 256;
            int temp1 = startAddress % 256;

            int temp3 = data.Length % 256;
            int temp4 = data.Length / 256;


            byte[] cmd = new byte[9 + data.Length * 2];
            cmd[0] = Convert.ToByte(ID);
            cmd[1] = 0x10;
            cmd[2] = Convert.ToByte(temp2);
            cmd[3] = Convert.ToByte(temp1);
            cmd[4] = Convert.ToByte(temp4);
            cmd[5] = Convert.ToByte(temp3);
            cmd[6] = Convert.ToByte(data.Length * 2);
            for (int i = 0; i < data.Length; i++)
            {

                int temp9 = data[i] % 256;
                int temp10 = data[i] / 256;

                cmd[7 + i * 2] = Convert.ToByte(temp10);
                cmd[8 + i * 2] = Convert.ToByte(temp9);
            }

            byte[] checksum = CRC16(cmd);
            cmd[cmd.Length-2] = checksum[0];
            cmd[cmd.Length - 1] = checksum[1];

            TX_cmd = "";

            for (int i = 0; i < cmd.Length; i++)
            {
                string hex = IntToHex2((int)cmd[i]);
                TX_cmd = TX_cmd + hex + " ";
            }

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
                    int lastbytes = 0;
                    int ByteWait = 0;
                    for (int i = 0; i < 200000; i++)
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
                    if (m > 0)
                    {
                        Serial.Read(hi, 0, m);
                    }
                    else
                    {
                        throw new Exception("Recived Timeout!");
                    }

                }

                RX_cmd = "";
                for (int i = 0; i < m; i++)
                {
                    string hex = IntToHex2((int)hi[i]);
                    RX_cmd = RX_cmd + hex + " ";
                }

                if (m > 2)
                {
                    if (hi[0] == cmd[0] && hi[1] == cmd[1])
                    {
                        numberOfTX++;
                        if (numberOfTX > 65535) numberOfTX = 0;
                        WriteMultiRegisterIsError = false;
                        Status = "WriteMultiRegister is OK " + numberOfTX.ToString();
                        ExceptionCode = "";
                    }
                    else
                    {

                        if (hi[1] == 131 && hi[2] == 01)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "ILLEGAL FUNCTION";
                        if (hi[1] == 131 && hi[2] == 02)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "ILLEGAL DATA ADDRESS";
                        if (hi[1] == 131 && hi[2] == 03)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "ILLEGAL DATA VALUE";
                        if (hi[1] == 131 && hi[2] == 04)
                            ExceptionCode = "ERROR! Modbus error ReadHoldingRegisters! SlaveID: " + ID.ToString() + Environment.NewLine +
                                            "SLAVE DEVICE FAILURE";
                        WriteMultiRegisterIsError = true;

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionCode = ex.ToString();
                NetworkIsOk = false;
                if (Mode_TCP_Serial == false)
                    Reconnect();
                WriteMultiRegisterIsError = true;
            }

        }
        //public ushort[] ReadInputRegisters(ushort startAddress, ushort numberOfPoints)
        //{ }
        //public bool[] ReadInputs(ushort startAddress, ushort numberOfPoints)
        //{ }
        //public ushort[] ReadWriteMultipleRegisters(ushort startReadAddress, ushort numberOfPointsToRead, ushort startWriteAddress, ushort[] writeData)
        //{ }
        //public void WriteMultipleCoils(ushort startAddress, bool[] data)
        //{ }


        //public void WriteSingleRegister(ushort registerAddress, ushort value)
        //{ }

    }

} //class ModbusTCPIP

