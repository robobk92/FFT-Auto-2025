using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace AASTV_Auto_Test
{
    /// <summary>
    /// Create a New INI file to store or load data
    /// </summary>
    public class IniFile
    {
        public string path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public IniFile(string INIPath)
        {
            path = INIPath;
        }

        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }

        public string IniReadValue(string Section, string Key)
        {
            StringBuilder retVal = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", retVal, 255, this.path);
            return retVal.ToString();
        }

        public string IniReadValue(string Section, string Key, string Default_Value)
        {
            string retValue = IniReadValue(Section, Key);
            if (retValue == "") retValue = Default_Value;
            return retValue;
        }
        public List<string> ReadSection(string section)
        {
            List<string> values = new List<string>();
            StringBuilder temp = new StringBuilder(4096); // Tăng buffer để đọc nhiều dữ liệu
            int size = GetPrivateProfileString(section, null, "", temp, temp.Capacity, this.path);

            if (size > 0)
            {
                string[] keys = temp.ToString().Split('\0'); // Chia thành từng dòng
                foreach (string key in keys)
                {
                    if (!string.IsNullOrEmpty(key) && !key.Contains("=")) // Bỏ qua key-value
                    {
                        values.Add(key);
                    }
                }
            }

            return values;
        }
    }
}