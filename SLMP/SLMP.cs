using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics.CodeAnalysis;


namespace SLMP
{
    public class SLMPTCP
    {
   
        public bool ModeFrame = false; // false = 3E, true = 4E frame
     
        public int tcpClient_ReceiveTimeout = 2000;
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
        public bool FailExecute;
        public string TX_cmd, RX_cmd;
        public bool IsLanConnect;


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

        public bool ConnectTCP(string ipAddress, int tcpPort)
        {
          
            return Connect1(ipAddress, tcpPort);
        }


        private static bool CheckInternet()
        {
            //http://msdn.microsoft.com/en-us/library/windows/desktop/aa384702(v=vs.85).aspx
            // InternetConnectionState flag = InternetConnectionState.INTERNET_CONNECTION_LAN;
            return true;
        }

        private bool Connect1(string ipAddress, int tcpPort)
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
                    ExceptionCode = "ERROR" + DateTime.Now.ToString() + ":Connect process " + ex.StackTrace + "==>" + ex.Message;
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

        public void ErrorCodeDetected(byte higher, byte lower)
        {

            bool otherErrorCode = true;
            if (higher == 85 && lower == 00) //C052H 192 82
            {
                ExceptionCode = "CPU module requested other device to write data during RUN when write was not permitted during RUN";
                otherErrorCode = false;
            }
            if (higher == 82 && lower == 192) //C052H 192 82
            {
                ExceptionCode = "Maximum number of word devices for which data can be read/written all at once is outside the allowable range.";
                otherErrorCode = false;
            }
            if (higher == 83 && lower == 192) // C053H 
            {
                ExceptionCode = "Maximum number of bit devices for which data can be random read/written all at once is outside the allowable range";
                otherErrorCode = false;
            }
            if (higher == 84 && lower == 192) //C054H  192 84
            {
                ExceptionCode = "Maximum number of word devices for which data can be random read/written all at once is outside the allowable range";
                otherErrorCode = false;
            }
            if (higher == 86 && lower == 192) //C056H 192 86
            {
                ExceptionCode = "Read or write request exceeds maximum address.";
                otherErrorCode = false;
            }
            if (higher == 89 && lower == 192) //C059H 192 89
            {
                ExceptionCode = "- Error in command or subcommand specification." + Environment.NewLine +
                "- There is a command or subcommand that cannot be used by the CPU module";
                otherErrorCode = false;
            }
            if (higher == 91 && lower == 192) //C05BH 192 91
            {
                ExceptionCode = "CPU module cannot read or write from/to specified device";
                otherErrorCode = false;
            }
            if (higher == 92 && lower == 192) //C05CH 192 92
            {
                ExceptionCode = "Error in request contents. (Reading or writing by bit unit for word device, etc.)";
                otherErrorCode = false;
            }
            if (higher == 95 && lower == 192) //C05FH 192 95
            {
                ExceptionCode = "There is a request that cannot be executed for the target CPU module.";
                otherErrorCode = false;
            }
            if (higher == 96 && lower == 192) //C060H 96
            {
                ExceptionCode = "Error in request contents. (Error in specification of data for bit device, etc.)";
                otherErrorCode = false;
            }
            if (higher == 97 && lower == 192) //C061H 97
            {
                ExceptionCode = "Request data length does not match the number of data in the character section (part of text).";
                otherErrorCode = false;
            }
            if (higher == 00 && lower == 194) //C200H 194 00
            {
                ExceptionCode = "Error in remote password.";
                otherErrorCode = false;
            }
            if (higher == 04 && lower == 194) //C204H 194 04
            {
                ExceptionCode = "Different device requested remote password to be unlocked";
                otherErrorCode = false;
            }
            if (otherErrorCode == true)
            {
                ExceptionCode = "Errors detected by CPU module." + Environment.NewLine +
                                "(Errors that occurred in other than SLMP communication function)";
            }
            FailExecute = true;

        }

        public bool CheckLan()
        {
            bool flag;
            flag = (tcpClient.Client.Poll(10000, SelectMode.SelectWrite));
            return flag;
        }


        public int[] ReadRegister(int startAddress, int numberOfPoints)
        {
            int[] value1 = new int[numberOfPoints];
            int temp2 = startAddress / 256;
            int temp1 = startAddress % 256;
            int temp3 = numberOfPoints % 256;
            int temp4 = numberOfPoints / 256;
            byte[] cmd = new byte[21];
            //subheader
            cmd[0] = 0x50;
            cmd[1] = 0x00;
            //request destination network no.
            cmd[2] = 0x00;
            //request destination station no.
            cmd[3] = 255;
            //request destination Module I/O no.
            cmd[4] = 255;
            cmd[5] = 0x03;
            //request destination multidrop station no.
            cmd[6] = 0x00;
            //request datalenght
            cmd[7] = 0x0C;
            cmd[8] = 0x00;
            //monitoring timer
            cmd[9] = 0x10;
            cmd[10] = 0x00;
            //command
            cmd[11] = 0x01;
            cmd[12] = 0x04;
            //subcommand for 
            // for MELSEC-Q/L 00[]1 or 00[0]
            // for MELSEC iQ-R 0002 or 0003
            cmd[13] = 0x00;
            cmd[14] = 0x00;
            //Head device No
            cmd[15] = Convert.ToByte(temp1);
            cmd[16] = Convert.ToByte(temp2);
            cmd[17] = 0x00;
            //Device Code
            //for D register is A8
            cmd[18] = 168;
            //No of device point
            cmd[19] = Convert.ToByte(temp3);
            cmd[20] = Convert.ToByte(temp4);
            TX_cmd = "";
            for (int i = 0; i < 21; i++)
            {
                string hex = IntToHex2((int)cmd[i]);
                TX_cmd = TX_cmd + hex + " ";
            }
            try
            {
                if (NetworkIsOk == false)
                {
                    throw new Exception("No Ethernet Connection");

                }

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
                    if (hi[9] == 0 && cmd[10] == 0)
                    {
                        numberOfTX++;
                        if (numberOfTX > 65535) numberOfTX = 0;

                        Status = "ReadDRegister is OK " + numberOfTX.ToString();
                        for (int i = 0; i < numberOfPoints; i++)
                        {
                            value1[i] = hi[12 + i * 2] * 256 + hi[11 + (i * 2)];
                        }
                        FailExecute = false;
                        ExceptionCode = "";

                    }
                    else
                        ErrorCodeDetected(hi[9], hi[10]);
                }
                else
                    ExceptionCode = "Data Recived error!";

            }
            catch (Exception ex)
            {
                ExceptionCode = ex.ToString();
                NetworkIsOk = false;
                //Reconnect();
                FailExecute = true;
            }
            return value1;
        }
        public bool[] ReadRelay(int startAddress, int numberOfPoints)
        {
            bool[] value1 = new bool[numberOfPoints];
            int temp2 = startAddress / 256;
            int temp1 = startAddress % 256;
            int temp3 = numberOfPoints % 256;
            int temp4 = numberOfPoints / 256;
            byte[] cmd = new byte[21];
            //subheader
            cmd[0] = 0x50;
            cmd[1] = 0x00;
            //request destination network no.
            cmd[2] = 0x00;
            //request destination station no.
            cmd[3] = 255;
            //request destination Module I/O no.
            cmd[4] = 255;
            cmd[5] = 0x03;
            //request destination multidrop station no.
            cmd[6] = 0x00;
            //request datalenght
            cmd[7] = 0x0C;
            cmd[8] = 0x00;
            //monitoring timer
            cmd[9] = 0x10;
            cmd[10] = 0x00;
            //command
            cmd[11] = 0x01;
            cmd[12] = 0x04;
            //subcommand for 
            // for MELSEC-Q/L 00[]1 or 00[0]
            // for MELSEC iQ-R 0002 or 0003
            cmd[13] = 0x01;
            cmd[14] = 0x00;
            //Head device No
            cmd[15] = Convert.ToByte(temp1);
            cmd[16] = Convert.ToByte(temp2);
            cmd[17] = 0x00;
            //Device Code
            //for D register is A8
            cmd[18] = 0x90;
            //No of device point
            cmd[19] = Convert.ToByte(temp3);
            cmd[20] = Convert.ToByte(temp4);
            TX_cmd = "";
            for (int i = 0; i < 21; i++)
            {
                string hex = IntToHex2((int)cmd[i]);
                TX_cmd = TX_cmd + hex + " ";
            }
            try
            {
                if (NetworkIsOk == false)
                {
                    throw new Exception("No Ethernet Connection");

                }

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
                    if (hi[9] == 0 && cmd[10] == 0)
                    {
                        numberOfTX++;
                        if (numberOfTX > 65535) numberOfTX = 0;

                        Status = "ReadDRegister is OK " + numberOfTX.ToString();
                        for (int i = 0; i < hi[7]-2; i++)
                        {
                            bool[] value2 = new bool[2];
                            value2 = Int8ToBools(hi[i + 9]);
                            for (int j = 0; j < 8; j++)
                            {
                                value1[j + 8 * i] = value2[j];
                            }
                        }
                        FailExecute = false;
                        ExceptionCode = "";

                    }
                    else
                        ErrorCodeDetected(hi[9], hi[10]);
                }
            }
            catch (Exception ex)
            {
                ExceptionCode = ex.ToString();
                NetworkIsOk = false;
               // Reconnect();
                FailExecute = true;
            }
            return value1;
        }
        public bool[] WriteRegister(int startAddress, int numberOfPoints, int startAdressArray, int[] dataArray)
        {
            bool[] value1 = new bool[numberOfPoints];
            int temp2 = startAddress / 256;
            int temp1 = startAddress % 256;
            int temp3 = numberOfPoints % 256;
            int temp4 = numberOfPoints / 256;
            byte[] cmd = new byte[21 + numberOfPoints * 2];
            int numberOfcmd = 12 + numberOfPoints * 2;
            int temp10 = numberOfcmd % 256;
            int temp11 = numberOfcmd / 256;

            //subheader
            cmd[0] = 0x50;
            cmd[1] = 0x00;
            //request destination network no.
            cmd[2] = 0x00;
            //request destination station no.
            cmd[3] = 255;
            //request destination Module I/O no.
            cmd[4] = 255;
            cmd[5] = 0x03;
            //request destination multidrop station no.
            cmd[6] = 0x00;
            //request datalenght
            cmd[7] = Convert.ToByte(temp10);
            cmd[8] = Convert.ToByte(temp11);
            //monitoring timer
            cmd[9] = 0x10;
            cmd[10] = 0x00;
            //command
            cmd[11] = 0x01;
            cmd[12] = 0x14;
            //subcommand for 
            // for MELSEC-Q/L 00[]1 or 00[0]
            // for MELSEC iQ-R 0002 or 0003

            cmd[13] = 0x00;

            cmd[14] = 0x00;
            //Head device No
            cmd[15] = Convert.ToByte(temp1);
            cmd[16] = Convert.ToByte(temp2);
            cmd[17] = 0x00;
            //Device Code
            //for D register is A8
            cmd[18] = 0xA8;
            //No of device point
            cmd[19] = Convert.ToByte(temp3);
            cmd[20] = Convert.ToByte(temp4);

            for (int i = 0; i < numberOfPoints; i++)
            {
                int temp5 = dataArray[i + startAdressArray] % 256;
                int temp6 = dataArray[i + startAdressArray] / 256;
                cmd[21 + i * 2] = Convert.ToByte(temp5);
                cmd[22 + i * 2] = Convert.ToByte(temp6);
            }



            TX_cmd = "";
            for (int i = 0; i < cmd.Length; i++)
            {
                string hex = IntToHex2((int)cmd[i]);
                TX_cmd = TX_cmd + hex + " ";
            }
            try
            {
                if (NetworkIsOk == false)
                {
                    throw new Exception("No Ethernet Connection");

                }

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
                    if (hi[9] == 0 && cmd[10] == 0)
                    {
                        numberOfTX++;
                        if (numberOfTX > 65535) numberOfTX = 0;

                        Status = "WriteDRegister is OK " + numberOfTX.ToString();
                        FailExecute = false;
                        ExceptionCode = "";

                    }
                    else
                        ErrorCodeDetected(hi[9], hi[10]);
                }
            }
            catch (Exception ex)
            {
                ExceptionCode = ex.ToString();
                NetworkIsOk = false;
               // Reconnect();
                FailExecute = true;
            }
            return value1;
        }


    }
}
