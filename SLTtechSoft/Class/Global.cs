//using DevExpress.XtraEditors.Internal;
//using DevExpress.XtraPrinting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AASTV_Auto_Test
{
    public static class Global
    {
        public static string pathProgram = Application.StartupPath + @"\\Program\\CsvFile\\Auto";
        public static string pathProgram_M = Application.StartupPath + @"\\Program\\CsvFile\\Manual";
        public static string pathOnline = Application.StartupPath + @"\\Program\\Online\\AutoStepList.csv";
        public static string pathJson_Online = Application.StartupPath + @"\\Program\\Online\\Json_Config.json";
        public static string pathJSON = Application.StartupPath + @"\\Program\\JsonFile";


        public static string path = AppDomain.CurrentDomain.BaseDirectory + "\\Setting\\AppConfig.ini";
        public static string path_USB_BLE = AppDomain.CurrentDomain.BaseDirectory + "\\Setting\\Terraforma_Scripts\\ratatosk\\";
        public static string path_USB_BLE_cfg = AppDomain.CurrentDomain.BaseDirectory + "\\Setting\\Terraforma_Scripts\\ratatosk\\cfg.yaml";
        public static string path_USB_BLE_variables = AppDomain.CurrentDomain.BaseDirectory + "\\Setting\\Terraforma_Scripts\\ratatosk\\variables.yaml";

        public static string pathJsonByYou = pathJSON + "\\factory_settings_A3HM-B701B-V1(ByYou_lever_lock).json";
        public static string pathJsonByYou_TW = pathJSON + "\\factory_settings_A3HM-B701B-V4(Byyou Pro TW)_B.json";
        public static string pathJsonRimLock_H = pathJSON + "\\factory_settings_A3HM-H701G-V1(ByYou_rim_lock_H).json";
        public static string pathJsonRimLock_V = pathJSON + "\\factory_settings_A3HM-V701G-V1(ByYou_rim_lock_V).json";
        public static string pathJsonRimLock_D = pathJSON + "\\factory_settings_A3HM-D701G-V1(ByYou_rim_lock_D).json";
        public static string pathJsonRimLock_G = pathJSON + "\\factory_settings_A3HM-G701G-V1(ByYou_rim_lock_G).json";
        public static string pathJsonE_Escutcheon = pathJSON + "\\factory_settings_A3HM-L230B-V0(e-escutcheon).json";
        public static string pathJsonYDR453 = pathJSON + "\\factory_settings_A3HM-H453A-V1(YDR453A).json";

        //AUTO MODE
        public static string pathProgramByYou = pathProgram + "\\ByYou_LeverLock_listTest.csv";
        public static string pathProgramRimLock_Hor = pathProgram + "\\RimLock_V+H_listTest.csv";
        public static string pathProgramRimLock_Ver = pathProgram + "\\RimLock_V+H_listTest.csv";
        public static string pathProgramRimLock_D = pathProgram + "\\RimLock_D_listTest.csv";
        public static string pathProgramRimLock_G = pathProgram + "\\RimLock_G_listTest.csv";
        public static string pathProgramE_Escutcheon = pathProgram + "\\E_Escutcheon_listTest.csv";
        public static string pathProgramPandora = pathProgram + "\\Pandora_listTest.csv";
        public static string pathProgramTerra = pathProgram + "\\Terraforma_listTest.csv";
        public static string pathProgramPrisma = pathProgram + "\\Prisma_listTest.csv";
        public static string pathProgramPrismaS = pathProgram + "\\PrismaS_listTest.csv";
        public static string pathProgramYDR453 = pathProgram + "\\YDR453_listTest.csv";
        public static string pathProgramMercury = pathProgram + "\\Mercury_listTest.csv";

        //MANUAL MODE
        public static string pathProgramByYou_M = pathProgram_M + "\\ByYou_LeverLock_listTest.csv";
        public static string pathProgramRimLock_Hor_M = pathProgram_M + "\\RimLock_V+H_listTest.csv";
        public static string pathProgramRimLock_Ver_M = pathProgram_M + "\\RimLock_V+H_listTest.csv";
        public static string pathProgramRimLock_D_M = pathProgram_M + "\\RimLock_D_listTest.csv";
        public static string pathProgramRimLock_G_M = pathProgram_M + "\\RimLock_G_listTest.csv";
        public static string pathProgramE_Escutcheon_M = pathProgram_M + "\\E_Escutcheon_listTest.csv";
        public static string pathProgramPandora_M = pathProgram_M + "\\Pandora_listTest.csv";
        public static string pathProgramTerra_M = pathProgram_M + "\\Terraforma_listTest.csv";
        public static string pathProgramPrisma_M = pathProgram_M + "\\Prisma_listTest.csv";
        public static string pathProgramPrismaS_M = pathProgram_M + "\\PrismaS_listTest.csv";
        public static string pathProgramYDR453_M = pathProgram_M + "\\YDR453_listTest.csv";
        public static string pathProgramMercury_M = pathProgram_M + "\\Mercury_listTest.csv";

        public static string ApplicationFullPathName()
        {
            return Assembly.GetExecutingAssembly().Location;
        }

        public static string ApplicationPath()
        {
            return Path.GetDirectoryName(ApplicationFullPathName());
        }

        public static Image image;
        public static Image Image_Array_R;
        public static Image Image_Array_G;
        public static Image Image_Array_B;
        public static Image[] Image_Terrafoma = new Image[7];
        public static void delay_ms(int ms_count)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            while (stopWatch.ElapsedMilliseconds < ms_count)
            { 
                Application.DoEvents();
                System.Threading.Thread.Sleep(10);
            }
        }
        public static string DutPort = "";
        public static string Wifi_ssid = "";
        public static string Wifi_psk = "";
        public static string DutBaurd = "115200";

        public static string MAC = "";
        public static string dataRevCom = "";
        public static string port_pyakta = "COM1";
        public static string port_pmtt = "";

        // Parameter
        public static string line = "";
        public static string station_name = "";
        public static string index = "";
        public static string Device = "";
        public static string Scanner_Port = "";
        public static string port_plc = "";
        public static string scan_port = "";
        public static string magnet_port = "";

        public static string MagnetPWM = "";
        public static string MagnetDuty = "";


        //ID
        public static string url = "";
        public static string userID = "";
        public static string pathLogfile = "";
        public static bool PlcEnable = true;
        public static bool Camera = true;

        public static string User = "";
        public static string Pass = "";

        // LEVELLOCK
        public static string SN_LENGHT = "";
        public static string SN_CONTAINS = "";
        public static string FRONT_CONTAINS = "";
        public static string FRONT_LENGHT = "";
        public static string MAIN_CONTAINS = "";
        public static string MAIN_LENGHT = "";
        public static string Sample_OK = "";
        public static string Sample_NG = "";

        public static string WO = "", PO ="", SKU = "", Line = "", Rule_Part_Main = "", Rule_Part_Front = "", Rule_SN = "", Mes_Model = "", Mes_Model_Name = "";
        public static bool No_Need_Link = false, Online = false;
        public static int Q_ty = 0, Qty_Part_Link = 0;
        public static string ENTER = "\u001b[1;44mENTERu001b[0m";
        public static string SPACE = "\u001b[1;31mSPACEu001b[0m";

        //// RIMLOCK_H
        //public static string RIMLOCK_H_SN_LENGHT = "";
        //public static string RIMLOCK_H_SN_CONTAINS = "";
        //public static string RIMLOCK_H_FRONT_CONTAINS = "";
        //public static string RIMLOCK_H_FRONT_LENGHT = "";
        //public static string RIMLOCK_H_MAIN_CONTAINS = "";
        //public static string RIMLOCK_H_MAIN_LENGHT = "";

        //// RIMLOCK_V
        //public static string RIMLOCK_V_SN_LENGHT = "";
        //public static string RIMLOCK_V_SN_CONTAINS = "";
        //public static string RIMLOCK_V_FRONT_CONTAINS = "";
        //public static string RIMLOCK_V_FRONT_LENGHT = "";
        //public static string RIMLOCK_V_MAIN_CONTAINS = "";
        //public static string RIMLOCK_V_MAIN_LENGHT = "";

        //// RIMLOCK_D
        //public static string RIMLOCK_D_SN_LENGHT = "";
        //public static string RIMLOCK_D_SN_CONTAINS = "";
        //public static string RIMLOCK_D_FRONT_CONTAINS = "";
        //public static string RIMLOCK_D_FRONT_LENGHT = "";
        //public static string RIMLOCK_D_MAIN_CONTAINS = "";
        //public static string RIMLOCK_D_MAIN_LENGHT = "";

        //// RIMLOCK_G
        //public static string RIMLOCK_G_SN_LENGHT = "";
        //public static string RIMLOCK_G_SN_CONTAINS = "";
        //public static string RIMLOCK_G_FRONT_CONTAINS = "";
        //public static string RIMLOCK_G_FRONT_LENGHT = "";
        //public static string RIMLOCK_G_MAIN_CONTAINS = "";
        //public static string RIMLOCK_G_MAIN_LENGHT = "";

        //// EE
        //public static string EE_SN_LENGHT = "";
        //public static string EE_SN_CONTAINS = "";
        //public static string EE_FRONT_CONTAINS = "";
        //public static string EE_FRONT_LENGHT = "";
        //public static string EE_MAIN_CONTAINS = "";
        //public static string EE_MAIN_LENGHT = "";

        //// MERCURY
        //public static string MERCURY_SN_LENGHT = "";
        //public static string MERCURY_SN_CONTAINS = "";
        //public static string MERCURY_FRONT_CONTAINS = "";
        //public static string MERCURY_FRONT_LENGHT = "";
        //public static string MERCURY_MAIN_CONTAINS = "";
        //public static string MERCURY_MAIN_LENGHT = "";

        //// PANDORA
        //public static string PANDORA_SN_LENGHT = "";
        //public static string PANDORA_SN_CONTAINS = "";
        //public static string PANDORA_FRONT_CONTAINS = "";
        //public static string PANDORA_FRONT_LENGHT = "";
        //public static string PANDORA_MAIN_CONTAINS = "";
        //public static string PANDORA_MAIN_LENGHT = "";

        //// ALLEGRO
        //public static string ALLEGRO_SN_LENGHT = "";
        //public static string ALLEGRO_SN_CONTAINS = "";
        //public static string ALLEGRO_FRONT_CONTAINS = "";
        //public static string ALLEGRO_FRONT_LENGHT = "";
        //public static string ALLEGRO_MAIN_CONTAINS = "";
        //public static string ALLEGRO_MAIN_LENGHT = "";

        //Tag
        public static string Power_On = "00";
        public static string Test_Mode = "01";
        public static string Get_Forced_Lock = "02";
        public static string Set_Default = "03";
        public static string Set_RTC = "04";
        public static string Get_RTC = "05";
        public static string Check_Flash = "06";
        public static string Check_Sound = "07";
        public static string Check_Led = "08";
        public static string Check_Motor = "09";
        public static string Check_M_Sensor = "0A";
        public static string Check_ADC = "0B";
        public static string Check_DPS = "0C";
        public static string Check_Keypad = "0D";
        public static string Check_Button = "0E";
        public static string Check_RF = "0F";
        public static string Check_Fringer = "10";
        public static string Check_Broken = "11";
        public static string Check_IED = "12";
        public static string Check_Handing = "13";

        //Data_Send_Event
        public static string Send_Test_Mode = "F0A0CN020100CS";
        public static string Send_Get_SW = "02";
        public static string Send_Set_Default = "F0A0CN03030113CS";//"F0A0CN03030000CS"; //F0A0010303011343
                                                                   // public static string Send_Set_RTC = "F0A0CN09040720230519151800CS";
        public static string Send_Get_RTC = "F0A0CN03050000CS";
        public static string Send_Check_Flash = "F0A0CN020600CS";
        public static string Send_Check_Sound_High = "F0A0CN03070103CS";
        public static string Send_Check_Sound_High_Prisma = "F0A0CN0407010203CS";
        public static string Send_Check_Sound_Low = "F0A0CN03070102CS";
        public static string Send_Check_Sound_Mute = "F0A0CN03070101CS";

        public static string Send_Check_Led = "F0A0CN03080103CS";
        public static string Send_Check_Motor_Sensor_Open = "F0A0CN03090100CS";
        public static string Send_Check_Motor_Sensor_Close = "F0A0CN030901FFCS";
        // public static string Send_Check_M_Sensor = "0A";
        public static string Send_Get_Forced_Lock = "F0B0CN020200CS";
        public static string Send_Get_Forced_Lock_Prisma = "F0A0CN020200CS";
        public static string Send_Check_ADC = "F0A0CN020B00CS";//       "F0B0CN020A00CS";
        public static string Send_Check_Door_Mode_In = "F0A0CN030C01FFCS";
        public static string Send_Check_Door_Close_Event = "F0A1CN030C0100CS";
        public static string Send_Check_Keypad_On = "F0A0CN030D01FFCS";
        public static string Send_Check_Keypad_Off = "F0A0CN030D0100CS";
        public static string Send_Check_Button_In = "F0A0CN030E01FFCS";//F0A0CN030E01FFCS
        public static string Send_Check_Button_Out = "F0A0CN030E0100CS";
        public static string Send_Check_DPS_Mode_In = "F0A1CN030C01FFCS";
        public static string Send_Check_DPS_Mode_Out = "F0A1CN030C0100CS";
        public static string Send_Check_RF_Mode_in = "F0A0CN030F01FFCS";
        public static string Send_Check_RF_Mode_out = "F0A0CN030F0100CS";
        public static string Send_Check_Fringer_Mode_In = "F0A0CN031001FFCS";
        public static string Send_Check_Fringer_Mode_Out = "F0A0CN03100100CS";
        public static string Send_Check_Fringer_1 = "F0A1CN03100101CS";
        public static string Send_Check_Fringer_2 = "F0A1CN03100102CS";
        public static string Send_Check_Broken_Mode_In =  "F0A0CN031101FFCS";
        public static string Send_Check_Broken_Ack = "F0A1CN031101FFCS";
        public static string Send_Check_Broken_Mode_Out = "F0A0CN03110100CS";
        public static string Send_Check_Broken_Mode_Out_Prisma = "F0A0CN03110100CS";
        public static string Send_Check_IDE = "F0A0CN021200CS";
        public static string Send_Set_Left_Hand_Set = "F0A0CN031301FFCS";
        public static string Send_Set_Right_Hand_Set = "F0A0CN03130100CS";
        public static string Send_Check_L_Sensor = "F0A0CN020A00CS";

        //Source
        public static string T_P_E = "A0";
        public static string Lock_ACK = "B1";
        public static string Lock_Event = "B0";

        public static string Program_ACK = "A1";



        public static string Pan_Led_On = "0400";
        public static string Pan_Arr_Red = "2b01";

        public static string Language = "";
        public static int AutoLock = 0;
        public static bool Rfid = false;
        public static bool Fingerprint = false;
        public static bool PinCode = false;
        public static bool SoundSw = false;
        public static int DefaultLevel = 0;
        public static bool DpsSw = false;
        public static bool Registration = false;
        public static bool AutoLockBtn = false;
        public static bool OpenClose = false;
        public static bool FactoryReset = false;
        public static bool InnerForcedLockSw = false;
        public static bool FingerprintDoubleCheck = false;
        public static bool HandingLock = false;
        public static bool BrokenDoubleCheck = false;
        public static bool LoadJsonData(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("File not found: " + filePath);
                    return false;
                }

                string json = File.ReadAllText(filePath);
                JObject jsonObj = JObject.Parse(json);

               
                var defaultSettings = jsonObj["default"];
                Language = defaultSettings.First(x => x["id"].ToString() == "language")["value"].ToString();
                AutoLock = (bool)defaultSettings.First(x => x["id"].ToString() == "autolock")["value"] ? 1 : 0;

                var credential = jsonObj["credential"][0];
                Rfid = (bool)credential["rfid"];
                Fingerprint = (bool)credential["fingerprint"];
                PinCode = (bool)credential["pincode"];

                var sound = jsonObj["sound"][0];
                SoundSw = (bool)sound["sound_sw"];
                DefaultLevel = (int)sound["default_level"];

                var dps = jsonObj["dps"][0];
                DpsSw = (bool)dps["dps_sw"];

                var buttonSwitch = jsonObj["button_switch"][0];
                Registration = (bool)buttonSwitch["registration"];
                AutoLockBtn = (bool)buttonSwitch["autolock"];
                OpenClose = (bool)buttonSwitch["open_close"];
                FactoryReset = (bool)buttonSwitch["factory_reset"];

                var innerforcedLock = jsonObj["innerforced_lock"][0];
                InnerForcedLockSw = (bool)innerforcedLock["innerforced_lock_SW"];

                var specialsFunctions = jsonObj["specials_Functions"][0];
                FingerprintDoubleCheck = (bool)specialsFunctions["fingerprint_dubble_check"];
                HandingLock = (bool)specialsFunctions["handing_lock"];
                BrokenDoubleCheck = (bool)specialsFunctions["broken_dubble_check"];

                Send_Set_Default = $"F0A0CN03{Set_Default}01{AutoLock}{Language}CS";
                return true;
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Không tìm thấy file Json " + ex.Message);
                return false;
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Quyền truy cập file Json bị từ chối: " + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đọc file Json: " + ex.Message);
                return false;
            }
        }
        public static bool Yaml_Varriables_Write_Value(string Serial_Number)
        {
            bool _flag = false;

            string key = "serial_number";

            string[] lines = File.ReadAllLines(Global.path_USB_BLE_variables);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (line.ToLower().Contains(key))
                {
                    int _index = line.IndexOf(':');
                    string updatedLine = line.Substring(0, _index + 1) + " " + Serial_Number;
                    lines[i] = updatedLine;
                    break;
                }
            }
            try
            {
                File.WriteAllLines(Global.path_USB_BLE_variables, lines);
                _flag = true;
            }
            catch (Exception ex)
            {
                _flag = false;

            }
            return _flag;
        }
        public enum CheckPosition_Nest
        {
            None,
            Home,
            Led,
            P1,
            P2,
            P3,
            P4,
            P5,
            P_RS,
        }
        public enum CheckPosition_Terrafoma
        {
            None,
            Home,
            Led,
            P5
        }

        //For Pandora

        public const string PAN_I2C_READ = "00"; //0x00
        public const string PAN_I2C_READ_ERROR = "01"; //0x01
        public const string PAN_I2C_WRITE = "02"; //0x02
        public const string PAN_I2C_WRITE_ERROR = "03"; //0x03

        public const string PAN_LED_CHECK_ON = "04"; //0x04 //전체 LED On
        public const string PAN_LED_CHECK_OFF = "05"; //0x04 //전체 LED Off
        public const string PAN_AUDIO_PLAY = "06";
        public const string PAN_KEY_SCAN_ON = "07";
        public const string PAN_KEY_SCAN_OFF = "08";
        public const string PAN_CAPSENSE_VALUE = "09";
        public const string PAN_GET_VOLTAGE = "0A";
        public const string PAN_GET_CURRENT = "0B";
        public const string PAN_READ_FW_VER = "0C";
        public const string PAN_READ_BT_ADDR = "0D";
        public const string PAN_WRITE_BT_ADDR = "0E";
        public const string PAN_READ_CURR_BT_ADDR = "0F";
        public const string PAN_READ_CHIRP_VAL = "10";
        public const string PAN_READ_ALS_VALUE = "11"; //조도
        public const string PAN_BUTTON_ON = "12";
        public const string PAN_BUTTON_OFF = "13";
        public const string PAN_FINGERPRINT_ON = "14";
        public const string PAN_FINGERPRINT_OFF = "15";
        public const string PAN_READ_NFC = "16";
        public const string PAN_READ_RSSI = "17";
        public const string PAN_WRITE_SERIAL = "20";
        public const string PAN_WRITE_HK_LOW = "21";
        public const string PAN_WRITE_HK_HIGH = "22";
        public const string PAN_WRITE_RESET_CODE = "23";
        public const string PAN_READ_SERIAL = "24";
        public const string PAN_READ_HK_LOW = "25";
        public const string PAN_READ_HK_HIGH = "26";
        public const string PAN_READ_RESET_CODE = "27";
        public const string PAN_MODE_EXIT = "28"; //0x28

        public const string PAN_ORANGE_LED_CHECK = "2A"; //State LED - 느낌표, MFG_TEST_CMD_ORANGE_LED_CHECK, 0x01 -> On , MFG_TEST_CMD_ORANGE_LED_CHECK, 0x00 -> Off
        public const string PAN_RED_LED_CHECK = "2B"; //Indicator Red LED - 느낌표, MFG_TEST_CMD_RED_LED_CHECK, 0x01 -> On , MFG_TEST_CMD_RED_LED_CHECK, 0x00 -> Off
        public const string PAN_GREEN_LED_CHECK = "2C"; //Indicator Green LED - 느낌표, MFG_TEST_CMD_GREEN_LED_CHECK, 0x01 -> On , MFG_TEST_CMD_GREEN_LED_CHECK, 0x00 -> Off
        public const string PAN_BLUE_LED_CHECK = "2D"; //Indicator Blue LED - 느낌표, MFG_TEST_CMD_BLUE_LED_CHECK, 0x01 -> On , MFG_TEST_CMD_BLUE_LED_CHECK, 0x00 -> Off
        public const string PAN_RIGHT_RED_LED_CHECK = "2E"; //State LED - 느낌표, MFG_TEST_CMD_RIGHT_RED_LED_CHECK, 0x01 -> On , MFG_TEST_CMD_RIGHT_RED_LED_CHECK, 0x00 -> Off
        public const string PAN_ALL_LED_CHECK = "2F"; //MFG_TEST_CMD_ALL_LED_CHECK, 0x01 -> On , MFG_TEST_CMD_ALL_LED_CHECK, 0x00 -> Off
        public const string PAN_BATTERY_CHECK = "30"; //MFG_TEST_CMD_LEFT_RED_LED_CHECK, 0x01 -> On , MFG_TEST_CMD_LEFT_RED_LED_CHECK, 0x00 -> Off

        //thinv 2023-02-27 add FFT-Command V5
        public const string PAN_AUDIO_ASSET = "31";
        public const string PAN_AUDIO_SET_VOLUME = "32";
        public const string PAN_CHIRP_THRESHOLDS = "33";
        public const string PAN_CHIRP_RANGES = "34";
        public const string PAN_CHIRP_SETTINGS = "35";
        public const string PAN_CHIRP_STREAM_ON = "36";
        public const string PAN_CHIRP_STREAM_OFF = "37";





        public const string Mer_All_Led_On = "5555";
        public const string Mer_Led_Off = "06";
        public const string Mer_Buzzer_On = "07";
        public const string Mer_Buzzer_Off = "08";
        public const string Mer_Key_Check_On = "09";
        public const string Mer_Key_Check_Off = "0A";
        public const string Mer_ALS = "15";
        public const string Mer_IR = "16";
        public const string Mer_Sensor_On = "17";
        public const string Mer_Sensor_Off = "18";
        public const string Mer_FW_Read = "10";
        public const string Mer_Switch_Mode = "28";
    }





}
