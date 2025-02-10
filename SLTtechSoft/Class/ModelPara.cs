
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SLTtechSoft
{
    public class CamPara
    {
        public int Exposure = 5;
        public double Gain = 0;
        public string VisionFile = "";
      
    }
    public class ModelParameter
    {
        public string ModelName = "";
        public CamPara[] Cam = new CamPara[3];
        public string Product = "";
        public string Line = "";
        public string StationTest = "";
        public string index = "";
        public string AreaTest = "";
        public string ModeTest = "Auto";
        public string TypeTest = "Full";
        public DataTable ListFFTTest = new DataTable();
        public string MotionFile = "";
        public PLCModelPara pLCModelPara = new PLCModelPara();
        public ClassParaKeySight KeySight = new ClassParaKeySight();
    }
    public class CurrentModelInfomation
    {
        public string PathFileModel = "";
        public string MachineNo = "0";
        public int QualityCount = 0;
        public DataTable ModelInfo = new DataTable();
        public int ModelNo = 0;
        public PLCInternalPara pLCInternalPara = new PLCInternalPara();
        public ClassModeMachine ModelMachine = new ClassModeMachine();
    }
    public class ClassModeMachine
    { 
        public bool cbSkipTestFail { get; set; }
        public bool cbSkipRetry { get; set; }
        public bool cbSkipStartButton { get; set; }
        public bool cbSkipMotion { get; set; }
        public int cb_Cycles { get; set; }
        public int tbRounds { get; set; }
        public bool cbAskSupply { get; set; }
        public bool cbResetWhenSetDefault { get; set; }
        public bool cbUseMacMes { get; set; }
        public bool cbManualMac { get; set; }
        public bool cbCloseDoorFFT { get; set; }
        public bool cbTestAgain { get; set; }
        public bool cbTurnOnVoice { get; set; }

    }
    public class PLCInternalPara
    { 
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
    public class PLCModelPara
    { 
        public int nudPoint1 { get; set; }
        public int nudPoint2 { get; set; }
        public int nudPoint3 { get; set; }
        public int nudPoint4 { get; set; }
        public int nudPoint5 { get; set; }
        public int nudPoint6 { get; set; }
        public int nudSpeed { get; set; }
        public int nudRPos1 { get; set; }
        public int nudRPos2 { get; set; }
        public int nudRPos3 { get; set; }
        public int nudRSpeed { get; set; }
    }
    public class ClassParaKeySight
    { 
        public double nudVoltage6V { get; set; }
        public double nudCurrent6V { get; set; }
        public double nudVoltage9V { get; set; }
        public double nudCurrent9V { get; set; }

    }
}
