// Class for control Keysight E36103B, 102B, 104B
// for initial KeysightPSU _PSU = KeysightPSU.Instance;
// Ver 1.00 
// Date 14-11-2022
// Thinv


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Ivi.Visa;
using Ivi.Visa.FormattedIO;


public class KeysightPSU
{
    //Create Lock Object
    private object LockObject = new object();

    //begin Singleton pattern
    static readonly KeysightPSU instance = new KeysightPSU();

    // true : Power On, failse : Power Off
    public bool PSUOnOff_state = false;
    // output status
    public bool PowerOn = false;
    // 
    private bool PSUOPEN = false;
    //
    public double CurrentVoltage = 0.0;
    //
    public double CurrentCurrent = 0.0;

    //private static readonly KeysightVisa
    public IMessageBasedSession session;



    private string _visaDevice = "";
    public KeysightPSU()
    {
        // for initial class DevPsu
        PSUOnOff_state = false;
        (_visaDevice, PSUOPEN) = ScanAndOpen();
        if (PSUOPEN == false) MessageBox.Show("PSU Keysight không có! Hãy kiểm tra lại.");

    }

    static KeysightPSU()
    {
    }

    public static KeysightPSU Instance
    {
        get
        {
            return instance;
        }
    }

    public string[] Scan(string searchString = "USB?*INSTR")
    {
        IEnumerable<string> devices;

        string[] dev = new string[5];

        lock (LockObject)
        {
            try
            {
                devices = GlobalResourceManager.Find(searchString);
                int i = 0;
                foreach (string device in devices)
                {
                    dev[i] = GlobalResourceManager.Parse(device).AliasIfExists;
                    i++;
                }

            }
            catch (VisaException ex)
            {
                Log("Scan Keysight E36103B Error + {0}", ex.Message);
            }
            return dev;
        }
    }

    /// <summary>
    /// Check all Keysight E36103B attack to PC
    /// session = this Keysight which attacked to PC
    /// More than one Keysight please use Open(visaName) bellow
    /// </summary>
    /// <returns></returns>
    public (string, bool) ScanAndOpen()
    {
        IEnumerable<string> devices;
        string visaAddress = string.Empty;
        bool OpenSuccess = false;
        string searchString = "USB?*INSTR";

        lock (LockObject)
        {
            try
            {
                //Log.Information("Finding Keysight E36103 with Name \"" + searchString + "\"");
                devices = GlobalResourceManager.Find(searchString);
            }
            catch (VisaException ex)
            {
                Log("Find Keysight E36103B Error + {0}", ex.Message);
                OpenSuccess = false;
                return (visaAddress, false);
            }

            foreach (string device in devices)
            {
                visaAddress = device;

                try
                {
                    session = GlobalResourceManager.Open(visaAddress) as IMessageBasedSession;
                    if (session != null)
                    {
                        _visaDevice = device;
                        PSUOnOff_state = true;
                        OpenSuccess = true;
                        Log("Keysight Name {0} open OK!", device);
                    }
                }
                catch (VisaException ex)
                {
                    OpenSuccess = false;
                    //Log.Error("Open Keysight E36103B Error + {0}", ex.Message);
                    continue;
                }

                break;
            }
            return (visaAddress, OpenSuccess);
        }
    }

    /// <summary>
    /// Open the specific device name "visaAddress"
    /// </summary>
    /// <param name="visaAddress"></param>
    /// <returns></returns>
    public bool Open(string visaAddress)
    {
        bool result = false;

        lock (LockObject)
        {
            try
            {
                session = GlobalResourceManager.Open(visaAddress) as IMessageBasedSession;

                PSUOnOff_state = true;

                Log("Keysight E36103B address : {0} open OK!", visaAddress);
                result = true;
            }
            catch (VisaException ex)
            {
                Log("Open Keysight E36103B Error + {0}", ex.Message);
                result = false;

            }

            return result;
        }

    }

    public bool Stop()
    {

        bool flag = false;

        lock (LockObject)
        {
            if (this.session == null)
                return flag;
            try
            {
                ((IDisposable)this.session).Dispose();


                flag = true;
            }
            catch (NativeVisaException ex)
            {
                Log("Stop Keysight E36103B Error + {0}", ex.Message);
            }

            PSUOnOff_state = false;

            return flag;
        }
    }
    public bool OnOffState()
    {
        return PSUOnOff_state;
    }

    //Voltage from 0.00VDC ~ 20.00VDC 
    //Scale 0.01V
    //Cannot setup Voltage when voltage > 6.0V
    public bool SetVoltage(double voltage)
    {
        bool flag = false;

        lock (LockObject)
        {

            if (this.session == null || (double)voltage < 0.0 || (double)voltage > 9.0)
                return flag;

            try
            {
                new MessageBasedFormattedIO(this.session).WriteLine(string.Format(":Voltage {0}", (object)voltage));
                CurrentVoltage = voltage;
                flag = true;
            }
            catch (NativeVisaException ex)
            {
                Log("Set Voltage Keysight E36103B Error + {0}", ex.Message);

            }
            return flag;
        }
    }
    public bool LockPanel()
    {
        bool flag = false;

        lock (LockObject)
        {
            if (this.session == null)
                return flag;
            try
            {
                MessageBasedFormattedIO basedFormattedIo = new MessageBasedFormattedIO(this.session);
                basedFormattedIo.WriteLine(string.Format(":SYStem:RWlock"));
                flag = true;
            }
            catch (NativeVisaException ex)
            {
                Log("Lock Keysight E36103B Error + {0}", ex.Message);
            }
            return flag;
        }
    }

    public bool UnLockPanel()
    {
        bool flag = false;

        lock (LockObject)
        {
            if (this.session == null)
                return flag;
            try
            {
                MessageBasedFormattedIO basedFormattedIo = new MessageBasedFormattedIO(this.session);
                basedFormattedIo.WriteLine(string.Format(":SYStem:LOCal"));
                flag = true;
            }
            catch (NativeVisaException ex)
            {
                Log("Unlock Keysight E36103B Error + {0}", ex.Message);
            }
            return flag;
        }
    }

    //Current from 0.000A ~ 2.000A 
    //Scale 1mA
    //Cannot set up current when I > 2A
    public bool SetCurrent(double current)
    {
        bool flag = false;

        lock (LockObject)
        {
            if (this.session == null || current < (byte)0 || current > (byte)2)
                return flag;
            try
            {
                new MessageBasedFormattedIO(this.session).WriteLine(string.Format(":CURRent {0}", (object)current));
                CurrentCurrent = current;
                flag = true;
            }
            catch (NativeVisaException ex)
            {
                Log("Set Current Keysight E36103B Error + {0}", ex.Message);
            }
            return flag;
        }
    }
    public string GetVoltage()
    {
        string voltage = "0";
        double temp = 0;

        lock (LockObject)
        {
            if (this.session == null)
                return voltage;
            try
            {
                MessageBasedFormattedIO basedFormattedIo = new MessageBasedFormattedIO(this.session);
                basedFormattedIo.WriteLine(string.Format(":MEASure:VOLTage?"));
                voltage = basedFormattedIo.ReadLine();
                voltage = voltage.Replace("\n", "");
                temp = Convert.ToDouble(voltage);
                voltage = temp.ToString("0.##");
                //Log.Information("Keysight PSU voltage : {0}V", voltage);
            }
            catch (NativeVisaException ex)
            {
                Log("Get Voltage Keysight E36103B Error + {0}", ex.Message);

            }
            return voltage;
        }
    }
    public string GetCurrent()
    {
        string current = "0";
        double temp = 0;

        lock (LockObject)
        {
            if (this.session == null)
                return current;
            try
            {
                MessageBasedFormattedIO basedFormattedIo = new MessageBasedFormattedIO(this.session);
                basedFormattedIo.WriteLine(string.Format(":MEASure:CURRent?"));
                current = basedFormattedIo.ReadLine();
                current = current.Replace("\n", "");

                temp = Convert.ToDouble(current) * 1000;
                current = temp.ToString("0.###");

            }
            catch (NativeVisaException ex)
            {
                Log("Get Current Keysight E36103B Error + {0}", ex.Message);
            }
            return current;
        }
    }
    public bool ON()
    {
        bool flag = false;

        lock (LockObject)
        {
            if (this.session == null)
                return flag;
            try
            {
                new MessageBasedFormattedIO(this.session).WriteLine(string.Format("OUTP ON"));
                PowerOn = true;
            }
            catch (NativeVisaException ex)
            {
                Log("Power ON Keysight E36103B Error + {0}", ex.Message);
            }
            return flag;
        }
    }

    public bool OFF()
    {
        bool flag = false;

        lock (LockObject)
        {
            if (this.session == null)
                return flag;
            try
            {
                new MessageBasedFormattedIO(this.session).WriteLine(string.Format("OUTP OFF"));
                PowerOn = false;


            }
            catch (NativeVisaException ex)
            {
                Log("Power OFF Keysight E36103B Error + {0}", ex.Message);
            }
            return flag;
        }
    }

    #region Display Voltage by convert value
    public string DisplayVol(double value)
    {
        string result = "";

        if (value < 1)
        {
            result = (value * 1000).ToString("0.###") + " mV";
        }
        else
        {
            result = value.ToString("0.###") + " V";
        }

        return result;
    }
    #endregion

    #region Display Current by convert value
    public string DisplayCur(double value)
    {
        string result = "";

        if (value < 1)
        {
            result = (value * 1000).ToString("0.###") + " mA";
        }
        else
        {
            result = value.ToString("0.###") + " A";
        }

        return result;
    }
    #endregion
    public bool aLiveorNot()
    {
        if (this.session == null)
        {
            Log("Information","Keysight E36103B is died.");
            return false;
        }
        else
            return true;
    }

    public event KeySightLog OnKeySightLog;
    public delegate void KeySightLog(string Content, string Message);
    public void Log(string Content, string Message)
    {
        if (this.OnKeySightLog != null)
        {
            this.OnKeySightLog(Content, Message);
        }

    }


}

