using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SLTtechSoft
{
    public class PLCCommunication
    {
        public Read Read = new Read();
        public Write Write = new Write();
    }
    public class Read
    {
        public ClassInput Input = new ClassInput();
        public ClassOutput Output = new ClassOutput();
        public BitReadAuto Auto = new BitReadAuto();
        public WordRead Word = new WordRead();
    }
    public class Write
    {
        public ClassOutput Output = new ClassOutput();
        public BitWriteAuto Auto = new BitWriteAuto();
        public WordWrite Word = new WordWrite();
    }
    public class ClassInput
    {
        public bool LimitNegAxis1 { get; set; }
        public bool HomeAxis1 { get; set; }
        public bool LimitPosAxis1 { get; set; }
        public bool OP_Start1 { get; set; }
        public bool OP_Start2 { get; set; }
        public bool OP_EMG { get; set; }
        public bool DoorUp { get; set; }
        public bool DoorDw { get; set; }
        public bool ToolsUp { get; set; }
        public bool ToolsDw { get; set; }
        public bool LockUp { get; set; }
        public bool LockDw { get; set; }
        public bool CheckSpringOpen { get; set; }
        public bool CheckSpringClose { get; set; }
        public bool CheckSpring1 { get; set; }
        public bool CheckSpring2 { get; set; }
        public bool CheckSpring3 { get; set; }
        public bool TransToolFW { get; set; }
        public bool TransToolBW { get; set; }
        public bool Push1Dw { get; set; }
        public bool Push2FW { get; set; }
        public bool Check1 { get; set; }
        public bool Check2 { get; set; }
        public bool Check3 { get; set; }
        public bool SafetySensor { get; set; }
    }

    public class ClassOutput
    {
        public bool PulseAxis1 { get; set; }
        public bool CylinderDoorClose { get; set; }
        public bool CylinderToolsDown { get; set; }
        public bool CylinderPushLock { get; set; }
        public bool CylinderCheckSpring { get; set; }
        public bool DirAxis1 { get; set; }
        public bool Broken {  get; set; }
        public bool TL_Green { get; set; }
        public bool TL_Red { get; set; }
        public bool TL_Yellow { get; set; }
        public bool TL_Buzzer { get; set; }
        public bool CylinderDownSpring { get; set; }
        public bool CylinderTools1 { get; set; }
        public bool CylinderTools2 { get; set; }
        public bool CylinderTransTools { get; set; }
        public bool CylinderPush1 { get; set; }
        public bool CylinderPush2 { get; set; }
        public bool CylinderFinger { get; set; }
        public bool CylinderKeyAsterisk { get; set; }
        public bool CylinderKeySharp { get; set; }
        public bool CylinderKey0 { get; set; }
        public bool CylinderKey1 { get; set; }
        public bool CylinderKey2 { get; set; }
        public bool CylinderKey3 { get; set; }
        public bool CylinderKey4 { get; set; }
        public bool CylinderKey5 { get; set; }
        public bool CylinderKey6 { get; set; }
        public bool CylinderKey7 { get; set; }
        public bool CylinderKey8 { get; set; }
        public bool CylinderKey9 { get; set; }
        public bool CylinderTools3 { get; set; }
        public bool CylinderTools4 { get; set; }

        public bool Reverse9V { get; set; }
        public bool Change9Vor6V { get; set; }

        public bool Supply6VToBattery { get; set; }
        public bool Supply9VToFront { get; set; }


    }
    public class BitReadAuto
    {
        public bool Auto { get; set; }
        public bool Run { get; set; }
        public bool Stop { get; set; }
        public bool ResetAlarm { get; set; }
        public bool ResetAll { get; set; }
        public bool HomeAll { get; set; }
        public bool HomeX { get; set; }
        public bool Bypass { get; set; }

        public bool ComFlash { get; set; }
        public bool ComOn { get; set; }


        public bool[] ErrorList1 = new bool[300];
        public bool[] ErrorList2 = new bool[300];
        public int[] ErrorWord1 = new int[10];
        public int[] ErrorWord2 = new int[10];

        public TestAutoRead Test = new TestAutoRead();
    }
    public class BitWriteAuto
    {
        public bool Auto { get; set; }
        public bool Run { get; set; }
        public bool Stop { get; set; }
        public bool ResetAlarm { get; set; }
        public bool ResetAll { get; set; }
        public bool HomeAll { get; set; }
        public bool HomeX { get; set; }
        public bool Bypass { get; set; }
        public bool Step_Mode { get; set; }
        public bool PC_Simulation_Mode { get; set; }
        public bool TestFinish { get; set; }
        public bool MotionReady { get; set; }
        public bool ComFlash { get; set; }

        public bool StepXNeg { get; set; }
        public bool StepXPos { get; set; }
        public bool StepXStop { get; set; }
        public bool StepMode { get; set; }
        public bool MovePosX1 { get; set; }
        public bool MovePosX2 { get; set; }
        public bool MovePosX3 { get; set; }
        public bool MovePosX4 { get; set; }
        public bool MovePosX5 { get; set; }
        public bool MovePosX6 { get; set; }
        public bool MovePosR1 { get; set; }
        public bool MovePosR2 { get; set; }
        public bool MovePosR3 { get; set; }
        public TestAutoWrite Test = new TestAutoWrite();
    }

    public class TestAutoWrite
    {
        //Check Voltage
        public bool StartCheckVoltage6V { get; set; }
        public bool StartCheckDoorPositionSensor_Close { get; set; }
        public bool StartCheckDoorPositionSensor_Auto_Lock { get; set; }
        public bool StartCheckDoorPositionSensor_Open { get; set; }
        public bool StartCheckDoorPositionSensor_Alarm { get; set; }
        public bool[] StartCheckPress = new bool[12];
        public bool StartLedCheck {  get; set; }
        public bool StartFingerprint_Check_Contact_1 { get; set; }
        public bool StartFingerprint_Check_Touch_1 { get; set; }
        public bool StartRF_Card_Check { get; set; }
        public bool StartButton_Check_Register_P { get; set; }
        public bool StartButton_Check_Register_N { get; set; }
        public bool StartButton_Check_Lock_P { get; set; }
        public bool StartButton_Check_Lock_N { get; set; }
        public bool StartMotor_Check_Close { get; set; }
        public bool StartMotor_Check_Mortise_Close { get; set; }
        public bool StartMotor_Check_Open { get; set; }
        public bool StartMotor_Check_Mortise_Open { get; set; }
        public bool StartExternal_Power_Check_9V { get; set; }
        public bool StartExternal_Power_Check_9V_REV { get; set; }
        public bool FinishProcessTest { get; set; }
        public bool Set_Check_ADC_Voltage { get; set; }
        public bool Set_Check_IDE_Current {  get; set; }
        public bool Broken_Disconnect { get; set; }
    }
    public class TestAutoRead
    {
        //Check Voltage
        public bool ReadyToOpenPower6V { get; set; }
        public bool ResultSpringOne { get; set; }
        public bool ResultSpringTwo { get; set; }
        public bool ResultSpringThree { get; set; }
        public bool ReadyCheckDoorPositionSensor_Close { get; set; }
        public bool ReadyCheckDoorPositionSensor_Auto_Lock { get; set; }
        public bool ReadyCheckDoorPositionSensor_Open { get; set; }
        public bool ReadyCheckDoorPositionSensor_Alarm { get; set; }
        public bool[] ReadyCheckPress = new bool[12];
        public bool ReadyLedCheck {  get; set; }
        public bool ReadyFingerprint_Check_Contact_1 { get; set; }
        public bool ReadyFingerprint_Check_Touch_1 { get; set; }
        public bool ReadyRF_Card_Check { get; set; }
        public bool ReadyButton_Check_Register_P { get; set; }
        public bool ReadyButton_Check_Register_N { get; set; }
        public bool ReadyButton_Check_Lock_P { get; set; }
        public bool ReadyButton_Check_Lock_N { get; set; }
        public bool ReadyMotor_Check_Close { get; set; }
        public bool ReadyMotor_Check_Mortise_Close { get; set; }
        public bool ReadyMotor_Check_Open { get; set; }
        public bool ReadyMotor_Check_Mortise_Open { get; set; }
        public bool IsMortise_Close { get; set; }
        public bool IsMortise_Open { get; set; }
        public bool ReadyExternal_Power_Check_9V { get; set; }
        public bool ReadyExternal_Power_Check_9V_REV { get; set; }
        public bool StartProcessTest { get; set; }
        public bool Ready_check_ADCVoltage { get; set; }
        public bool Ready_Check_IDE_Current {  get; set; }

        public bool Broken_Disconnect { get; set; }
    }
    public class WordRead
    {
     
        public int CoordinateX { get; set; }
        public int CoordinateR { get; set; }
        public int CycleTime { get; set; }
        public int ProcessNo { get; set; }
        public int TactTime { get; set; }
        public int TactTimeLow { get; set; }
        public int TactTimeHigh { get; set; }
        public int TactTimeMean { get; set; }
        public int PassCount { get; set; }
        public int FailCount { get; set; }
        public int MachineWorkTime { get; set; }
        public int HandlingTime { get; set; }
        public int QualityLastHour { get; set; }
        public int QualityThisHour { get; set; }

    }
    public class WordWrite
    {
        public int PosCamera { get; set; }
        public int PosInput { get; set; }
        public int PosGoOriginal { get; set; }
        public int PosDoor { get; set; }
        public int Pos5 { get; set; }
        public int Pos6 { get; set; }
        public int SpeedXAuto { get; set; }
        public int nudRPos1 { get; set; }
        public int nudRPos2 { get; set; }
        public int nudRPos3 { get; set; }
        public int SpeedRAuto { get; set; }
        public int nudStepSizeX { get; set; }
        public int nudFeedRateX { get; set; }
        public int nudStepSizeR { get; set; }
        public int nudFeedRateR { get; set; }
        public int tbOPRCreepSpeedX { get; set; }
        public int tbOprSpeedX { get; set; }
        public int tbStartAddressX { get; set; }
        public int tbOprStartSpeedX { get; set; }
        public int nudRCreepSpeed { get; set; }
        public int nudROprSpeed { get; set; }
        public int nudRStartAddress { get; set; }
        public int nudRStartSpeed { get; set; }

    }
    public class Coordinate3D
    {
        public int X {  get; set; }
        public int Y { get; set; }
        public int R { get; set; }
       
    }

}
