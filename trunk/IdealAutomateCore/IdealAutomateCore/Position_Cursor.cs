using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace IdealAutomate.Core
{
    static class Position_Cursor
    {

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetCursorPos(int X, int Y);
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy,
                               uint dwData, int dwExtraInfo);
        [DllImport("user32.dll", EntryPoint = "keybd_event", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern void keybd_event(byte vk, byte scan, int flags, int extrainfo);

        /// <summary>
        /// Not sure if we're just supposed to create our own point class.
        /// </summary>
        struct Point
        {
            public int x;
            public int y;
        }


        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;
        public const byte KEYBDEVENTF_SHIFTVIRTUAL = 0x10;
        public const byte KEYBDEVENTF_SHIFTSCANCODE = 0x2A;
        public const int KEYBDEVENTF_KEYDOWN = 0;
        public const int KEYBDEVENTF_KEYUP = 2;


        public static void DoMouseClick(UInt32 myX, UInt32 myY)
        {
            UInt32 myFlags = MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP;
            UInt32 myButtons = 0;
            UIntPtr myExtraInfo; 
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, myX, myY, 0, 0);
            
        }

        public static void DoMouseShiftClick(UInt32 myX, UInt32 myY)
        {
            UInt32 myFlags = MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP;
            UInt32 myButtons = 0;
            UIntPtr myExtraInfo;
            // shift down
            keybd_event(KEYBDEVENTF_SHIFTVIRTUAL, KEYBDEVENTF_SHIFTSCANCODE, KEYBDEVENTF_KEYDOWN, 0);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, myX, myY, 0, 0);
            // shift up
            keybd_event(KEYBDEVENTF_SHIFTVIRTUAL, KEYBDEVENTF_SHIFTSCANCODE, KEYBDEVENTF_KEYUP, 0);
        }

        public static void DoMouseRightClick(UInt32 myX, UInt32 myY)
        {
            UInt32 myFlags = MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP;
            UInt32 myButtons = 0;
            UIntPtr myExtraInfo;
            mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, myX, myY, 0, 0);

        }
               
        public static void MoveMouse(int x, int y)
        {
            SetCursorPos(x, y);
        }

    }
}
