using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MouseAccelerator
{
    public static class MouseHelper
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, SPIF fWinIni);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, uint pvParam, SPIF fWinIni);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, uint[] pvParam, SPIF fWinIni);

        [Flags]
        public enum SPIF
        {
            None = 0x00,
            SPIF_UPDATEINIFILE = 0x01,
            SPIF_SENDCHANGE = 0x02,
            SPIF_SENDWININICHANGE = 0x02
        }

        public const uint SPI_GETMOUSE = 0x0003;
        public const uint SPI_SETMOUSE = 0x0004;
        public const uint SPI_GETMOUSESPEED = 0x0070;
        public const uint SPI_SETMOUSESPEED = 0x0071;

        public static bool GetMouseSpeed(out int speed)
        {
            IntPtr pointer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
            bool b = SystemParametersInfo(SPI_GETMOUSESPEED, 0, pointer, SPIF.None);
            speed = Marshal.ReadInt32(pointer);
            Marshal.FreeHGlobal(pointer);
            return b;
        }

        public static bool SetMouseSpeed(uint speed)
        {
            return SystemParametersInfo(SPI_SETMOUSESPEED, 0, speed, SPIF.SPIF_SENDCHANGE);
        }

        public static int[] GetMouse()
        {
            int[] data = new int[3];
            unsafe
            {
                fixed (int* pArray = data)
                {
                    IntPtr intPtr = new IntPtr((void*)pArray);
                    SystemParametersInfo(SPI_GETMOUSE, 0, intPtr, SPIF.None);
                }
            }
            return data;
        }

        public static void SetMouse(int speed1, int speed2, bool accel)
        {
            SystemParametersInfo(SPI_SETMOUSE, 0, new uint[] 
            {
                (uint)speed1,
                (uint)speed2,
                (uint)(accel?1:0)
            }, SPIF.SPIF_SENDCHANGE);
        }
    }
}
