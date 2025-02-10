using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLTtechSoft
{
    public class MachineSetting
    {
        public string MCTitle = "";
        public string Password = "";
    }
    public class MesSetting
    {
        public string HostIP = "0.0.0.0";
        public int Port = 4000;
        public string McCode = "";
        public string CheckCount = "";
    }
    public class CamSetting
    {
        public string HostIP = "0.0.0.0";
        public bool Use = false;
        public bool SaveGraphic = false;
        public bool SaveOrigin = false;
        public bool SaveOriginSeparate = false;
        public ImageSource ImageSource = ImageSource.Cam;
        public TriggerMode TriggerMode = TriggerMode.Manual;
        public string ImageFolder = "";
        public string ImageFile = "";

    }
    public enum ImageSource
    {
        Cam,
        Folder,
        File
    }
    public enum TriggerMode
    { 
        Manual,
        HardwareAuto
    }
    public class PrinterSetting
    {
        public string PortName = "COM1";
        public int BaudRate = 9600;
        public int DataBits = 0;
        public string Parity = "None";
        public string StopBits = "One";
    }
    public class ScannerSetting
    {
        public string HostIP = "0.0.0.0";
        public int Port = 4000;

        public string PortName = "COM1";
        public int BaudRate = 9600;
        public int DataBits = 0;
        public string Parity = "None";
        public string StopBits = "One";
    }
    public class PLCSetting
    {
        public string HostIP = "0.0.0.0";
        public int Port = 4000;
        public int ThreadInterval = 5;


        public string PortName = "COM1";
        public int BaudRate = 9600;
        public int DataBits = 0;
        public string Parity = "None";
        public string StopBits = "One";

    }
    public class OptionSetting
    {
        public bool CallModelManual { get; set; }
     
    }
  
    public class Parameter
    { 
        public MachineSetting Machine  = new MachineSetting();
        public PLCSetting[] PLC = new PLCSetting[2];
        public MesSetting MES = new MesSetting();
        public PrinterSetting Lock = new PrinterSetting();
        public ScannerSetting Scanner = new ScannerSetting();
        public CamSetting[] Cam = new CamSetting[3];

        public OptionSetting Option = new OptionSetting();
        public string CommonPath = "";
    }
    
}
