using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SLTtechSoft.Form1;

namespace SLTtechSoft
{
    public class WordConvert
    {
        public Dword IntTo2Word(int Value)
        {
            Dword result = new Dword();
            if (Value > 0)
            {
                result.Upper = (ushort)(Value / 65536);
                result.Lowwer = (ushort)(Value % 65536);
            }
            else
            {
                int ValueRev = (2147483647 + Value + 1);
                int HighWord = ValueRev / 65536 + 32768;
                int LowWord = ValueRev % 65536;

                result.Upper = (ushort)HighWord;
                result.Lowwer = (ushort)LowWord;

            }
            return result;
        }
        public bool[] WordTo16Bit(ushort Word)
        {
            bool[] result = new bool[16];
            var bools = new BitArray(new int[] { Word }).Cast<bool>().ToArray();
            result = bools;
            return result;
        }
        public bool[] WordTo16Bit(int Word)
        {
            bool[] result = new bool[16];
            var bools = new BitArray(new int[] { Word }).Cast<bool>().ToArray();
            result = bools;
            return result;
        }

        public int USigned32Toint(ushort Lowwer, ushort Upper)
        {
            int result = 0;
            result = Upper * 65536 + Lowwer;
            return result;
        }
        public int Signed32Toint(ushort Lowwer, ushort Upper)
        {
            int result = 0;
            bool[] BitUpper = WordTo16Bit(Upper);
            bool[] BitLower = WordTo16Bit(Lowwer);
            // if > 0
            if (!BitUpper[15])
            {
                result = Upper * 65536 + Lowwer;
            }
            else
            {
                int NewUpper = Upper - 32768;
                int NewValue = 2147483647 - (NewUpper * 65536) - Lowwer + 1;
                result = -NewValue;
            }
            return result;
        }
        public int Signed32Toint(int Lowwer, int Upper)
        {
            int result = 0;
            bool[] BitUpper = WordTo16Bit(Upper);
            bool[] BitLower = WordTo16Bit(Lowwer);
            // if > 0
            if (!BitUpper[15])
            {
                result = Upper * 65536 + Lowwer;
            }
            else
            {
                int NewUpper = Upper - 32768;
                int NewValue = 2147483647 - (NewUpper * 65536) - Lowwer + 1;
                result = -NewValue;
            }
            return result;
        }
        public ushort Bit16ToWord(bool b0, bool b1, bool b2, bool b3, bool b4, bool b5, bool b6, bool b7, bool b8, bool b9, bool b10, bool b11, bool b12, bool b13, bool b14, bool b15)
        {
            ushort result = 0;
            int D0 = b0 ? 1 : 0;
            int D1 = b1 ? 1 : 0;
            int D2 = b2 ? 1 : 0;
            int D3 = b3 ? 1 : 0;
            int D4 = b4 ? 1 : 0;
            int D5 = b5 ? 1 : 0;
            int D6 = b6 ? 1 : 0;
            int D7 = b7 ? 1 : 0;
            int D8 = b8 ? 1 : 0;
            int D9 = b9 ? 1 : 0;
            int D10 = b10 ? 1 : 0;
            int D11 = b11 ? 1 : 0;
            int D12 = b12 ? 1 : 0;
            int D13 = b13 ? 1 : 0;
            int D14 = b14 ? 1 : 0;
            int D15 = b15 ? 1 : 0;
            int ResultInt = D0 + D1*2 + D2*4 + D3*8 + D4*16 + D5*32 + D6*64 + D7*128 + D8*256 + D9*512 + D10*1024 + D11*2048 + D12*4096 + D13*8192 + D14*16384 + D15* 32768;
            result = (ushort)ResultInt;
            return result;
        }

    }
    public class Dword
    {
        public ushort Lowwer { get; set; }
        public ushort Upper { get; set; }
    }


}
