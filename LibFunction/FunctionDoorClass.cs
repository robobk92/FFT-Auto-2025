using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibFunction
{
    public class FunctionDoorClass
    {
        public ClassSerial_Number Serial_Number = new ClassSerial_Number();
        public ClassFront_QR Front_QR = new ClassFront_QR();
        public ClassMain_QR Main_QR = new ClassMain_QR();
        public ClassCheck_Voltage Check_Voltage = new ClassCheck_Voltage();
        public ClassCheck_Current Check_Current = new ClassCheck_Current();
        public ClassPower_On Power_On = new ClassPower_On();
        public ClassLoad_Parameter Load_Parameter = new ClassLoad_Parameter();
        public ClassFirmware_Check Firmwave_Check = new ClassFirmware_Check();
        public ClassSet_Default Set_Default = new ClassSet_Default();
        public ClassSet_RTC Set_RTC = new ClassSet_RTC();
        public ClassGet_RTC Get_RTC = new ClassGet_RTC();
        public ClassCheck_Flash Check_Flash = new ClassCheck_Flash();

        public ClassDoor_Position_Sensor_Check_Close Door_Position_Sensor_Check_Close = new ClassDoor_Position_Sensor_Check_Close();
        public ClassDoor_Position_Sensor_Check_Auto_Lock Door_Position_Sensor_Check_Auto_Lock = new ClassDoor_Position_Sensor_Check_Auto_Lock();
        public ClassDoor_Position_Sensor_Check_Open Door_Position_Sensor_Check_Open = new ClassDoor_Position_Sensor_Check_Open();
        public ClassDoor_Position_Sensor_Check_Alarm Door_Position_Sensor_Check_Alarm = new ClassDoor_Position_Sensor_Check_Alarm();

        public ClassBroken_Check_Disconnect Broken_Check_Disconnect = new ClassBroken_Check_Disconnect();
        public ClassBroken_Check_Alarm Broken_Check_Alarm = new ClassBroken_Check_Alarm();

        public ClassSpeaker_Check Speaker_Check = new ClassSpeaker_Check();

        public ClassKeyCheck[] KeyChecks = new ClassKeyCheck[]
        {
        new ClassKeyCheck { ID = "FFT-9.0", Name = "Key_Check", Detail = "Press_1" },
        new ClassKeyCheck { ID = "FFT-9.1", Name = "Key_Check", Detail = "Press_2" },
        new ClassKeyCheck { ID = "FFT-9.2", Name = "Key_Check", Detail = "Press_3" },
        new ClassKeyCheck { ID = "FFT-9.3", Name = "Key_Check", Detail = "Press_4" },
        new ClassKeyCheck { ID = "FFT-9.4", Name = "Key_Check", Detail = "Press_5" },
        new ClassKeyCheck { ID = "FFT-9.5", Name = "Key_Check", Detail = "Press_6" },
        new ClassKeyCheck { ID = "FFT-9.6", Name = "Key_Check", Detail = "Press_7" },
        new ClassKeyCheck { ID = "FFT-9.7", Name = "Key_Check", Detail = "Press_8" },
        new ClassKeyCheck { ID = "FFT-9.8", Name = "Key_Check", Detail = "Press_9" },
        new ClassKeyCheck { ID = "FFT-9.9", Name = "Key_Check", Detail = "Press_0" },
        new ClassKeyCheck { ID = "FFT-9.10", Name = "Key_Check", Detail = "Press_V" },
        new ClassKeyCheck { ID = "FFT-9.11", Name = "Key_Check", Detail = "Press_*" }
        };

        public ClassLedCheck[] LedChecks = new ClassLedCheck[]
        {
        new ClassLedCheck { ID = "FFT-10.0", Name = "Led_Check", Detail = "Led_0" },
        new ClassLedCheck { ID = "FFT-10.1", Name = "Led_Check", Detail = "Led_1" },
        new ClassLedCheck { ID = "FFT-10.2", Name = "Led_Check", Detail = "Led_2" },
        new ClassLedCheck { ID = "FFT-10.3", Name = "Led_Check", Detail = "Led_3" },
        new ClassLedCheck { ID = "FFT-10.4", Name = "Led_Check", Detail = "Led_4" },
        new ClassLedCheck { ID = "FFT-10.5", Name = "Led_Check", Detail = "Led_5" },
        new ClassLedCheck { ID = "FFT-10.6", Name = "Led_Check", Detail = "Led_6" },
        new ClassLedCheck { ID = "FFT-10.7", Name = "Led_Check", Detail = "Led_7" },
        new ClassLedCheck { ID = "FFT-10.8", Name = "Led_Check", Detail = "Led_8" },
        new ClassLedCheck { ID = "FFT-10.9", Name = "Led_Check", Detail = "Led_9" },
        new ClassLedCheck { ID = "FFT-10.10", Name = "Led_Check", Detail = "Led_*" },
        new ClassLedCheck { ID = "FFT-10.11", Name = "Led_Check", Detail = "Led_#" },
        new ClassLedCheck { ID = "FFT-10.12", Name = "Led_Check", Detail = "Led_Bar" }
        };

        public ClassFingerprint_Check_Contact_1 Fingerprint_Check_Contact_1 = new ClassFingerprint_Check_Contact_1();
        public ClassFingerprint_Check_Touch_1 Fingerprint_Check_Touch_1 = new ClassFingerprint_Check_Touch_1();
        public ClassRF_Card_Check RF_Card_Check = new ClassRF_Card_Check();

        public ClassButton_Check_Register_P Button_Check_Register_P = new ClassButton_Check_Register_P();
        public ClassButton_Check_Register_N Button_Check_Register_N = new ClassButton_Check_Register_N();
        public ClassButton_Check_Lock_P Button_Check_Lock_P = new ClassButton_Check_Lock_P();
        public ClassButton_Check_Lock_N Button_Check_Lock_N = new ClassButton_Check_Lock_N();
        public ClassMotor_Check_Close Motor_Check_Close = new ClassMotor_Check_Close();
        public ClassMotor_Check_Mortise_Close Motor_Check_Mortise_Close = new ClassMotor_Check_Mortise_Close();
        public ClassMotor_Check_Open Motor_Check_Open = new ClassMotor_Check_Open();
        public ClassMotor_Check_Mortise_Open Motor_Check_Mortise_Open = new ClassMotor_Check_Mortise_Open();

        public ClassADC_Check ADC_Check = new ClassADC_Check();
        public ClassIDE_Current_Check IDE_Curent_Check = new ClassIDE_Current_Check();
        public ClassExternal_Power_Check_9V External_Power_Check_9V = new ClassExternal_Power_Check_9V();
        public ClassExternal_Power_Check_9V_REV External_Power_Check_9V_REV = new ClassExternal_Power_Check_9V_REV();
    }

    public class ClassSerial_Number
    {
        public string ID = "FFT-0.0";
        public string Name = "Serial_Number";
        public string Detail = "Scan";
    }

    public class ClassFront_QR
    {
        public string ID = "FFT-0.1";
        public string Name = "Front_QR";
        public string Detail = "Scan";
    }

    public class ClassMain_QR
    {
        public string ID = "FFT-0.2";
        public string Name = "Main_QR";
        public string Detail = "Scan";
    }

    public class ClassCheck_Voltage
    {
        public string ID = "FFT-1.0";
        public string Name = "Check_Voltage";
        public string Detail = "PSU_Read_Voltage";
    }

    public class ClassCheck_Current
    {
        public string ID = "FFT-1.1";
        public string Name = "Check_Current";
        public string Detail = "PSU_Read_Current";
    }

    public class ClassPower_On
    {
        public string ID = "FFT-2.0";
        public string Name = "Power_On";
        public string Detail = "";
    }

    public class ClassLoad_Parameter
    {
        public string ID = "FFT-2.1";
        public string Name = "Load_Parameter";
        public string Detail = "";
    }

    public class ClassFirmware_Check
    {
        public string ID = "FFT-2.2";
        public string Name = "Firmwave_Check";
        public string Detail = "";
    }

    public class ClassSet_Default
    {
        public string ID = "FFT-3";
        public string Name = "Set_Default";
        public string Detail = "Set_Default";
    }

    public class ClassSet_RTC
    {
        public string ID = "FFT-4.0";
        public string Name = "Set_RTC";
        public string Detail = "RTC";
    }

    public class ClassGet_RTC
    {
        public string ID = "FFT-4.1";
        public string Name = "Get_RTC";
        public string Detail = "RTC";
    }

    public class ClassCheck_Flash
    {
        public string ID = "FFT-5";
        public string Name = "Check_Flash";
        public string Detail = "";
    }

    public class ClassDoor_Position_Sensor_Check_Close
    {
        public string ID = "FFT-6.0";
        public string Name = "Door_Position_Sensor_Check";
        public string Detail = "Close";
    }
    public class ClassDoor_Position_Sensor_Check_Auto_Lock
    {
        public string ID = "FFT-6.1";
        public string Name = "Door_Position_Sensor_Check";
        public string Detail = "Auto_Lock";
    }
    public class ClassDoor_Position_Sensor_Check_Open
    {
        public string ID = "FFT-6.2";
        public string Name = "Door_Position_Sensor_Check";
        public string Detail = "Open";
    }
    public class ClassDoor_Position_Sensor_Check_Alarm
    {
        public string ID = "FFT-6.3";
        public string Name = "Door_Position_Sensor_Check";
        public string Detail = "Alarm";
    }
    public class ClassBroken_Check_Disconnect
    {
        public string ID = "FFT-7.0";
        public string Name = "Broken_Check";
        public string Detail = "Disconnect";
    }
    public class ClassBroken_Check_Alarm
    {
        public string ID = "FFT-7.1";
        public string Name = "Broken_Check";
        public string Detail = "Alarm";
    }


    public class ClassSpeaker_Check
    {
        public string ID = "FFT-8.0";
        public string Name = "Speaker_Check";
        public string Detail = "Volume";
    }
    // Key Check Press
    public class ClassKeyCheck
    {
        public string ID {  get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
    }
 
    // ------------- Led Check---------------
    public class ClassLedCheck
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
    }

    //
    public class ClassFingerprint_Check_Contact_1
    {
        public string ID = "FFT-11.0";
        public string Name = "Fingerprint_Check";
        public string Detail = "Contact_1";
    }
    public class ClassFingerprint_Check_Touch_1
    {
        public string ID = "FFT-11.1";
        public string Name = "Fingerprint_Check";
        public string Detail = "Touch_1";
    }

    public class ClassRF_Card_Check
    {
        public string ID = "FFT-12.0";
        public string Name = "RF_Card_Check";
        public string Detail = "RF_Card_Insert";
    }

    public class ClassButton_Check_Register_P
    {
        public string ID = "FFT-13.0";
        public string Name = "Button_Check";
        public string Detail = "Register_P";
    }

    public class ClassButton_Check_Register_N
    {
        public string ID = "FFT-13.1";
        public string Name = "Button_Check";
        public string Detail = "Register_N";
    }

    public class ClassButton_Check_Lock_P
    {
        public string ID = "FFT-13.2";
        public string Name = "Button_Check";
        public string Detail = "Lock_P";
    }

    public class ClassButton_Check_Lock_N
    {
        public string ID = "FFT-13.3";
        public string Name = "Button_Check";
        public string Detail = "Lock_N";
    }

    public class ClassMotor_Check_Close
    {
        public string ID = "FFT-14.0";
        public string Name = "Motor_Check";
        public string Detail = "Close";
    }

    public class ClassMotor_Check_Mortise_Close
    {
        public string ID = "FFT-14.1";
        public string Name = "Motor_Check";
        public string Detail = "Mortise_Close";
    }

    public class ClassMotor_Check_Open
    {
        public string ID = "FFT-14.2";
        public string Name = "Motor_Check";
        public string Detail = "Open";
    }

    public class ClassMotor_Check_Mortise_Open
    {
        public string ID = "FFT-14.3";
        public string Name = "Motor_Check";
        public string Detail = "Mortise_Open";
    }

    public class ClassADC_Check
    {
        public string ID = "FFT-15.0";
        public string Name = "ADC_Check";
        public string Detail = "Get_Battery_Voltage";
    }

    public class ClassIDE_Current_Check
    {
        public string ID = "FFT-16.0";
        public string Name = "IDE_Current_Check";
        public string Detail = "PSU_Read_Current";
    }

    public class ClassExternal_Power_Check_9V
    {
        public string ID = "FFT-17.0";
        public string Name = "External_Power_Check";
        public string Detail = "9V_Battery";
    }
    public class ClassExternal_Power_Check_9V_REV
    {
        public string ID = "FFT-17.1";
        public string Name = "External_Power_Check";
        public string Detail = "9V_Battery_Rev";
    }

}
