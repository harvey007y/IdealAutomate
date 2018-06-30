using System;
using System.Runtime.InteropServices;

namespace GetCursorPosDemo
{
    class Program
    {
        static int _x, _y;
        
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            while (!Console.KeyAvailable)
            {
                ShowMousePosition();
            }
            Console.CursorVisible = true;
        }

        static void ShowMousePosition()
        {
            POINT point;
            if (GetCursorPos(out point) && point.X != _x && point.Y != _y)
            {
                Console.Clear();
                Console.WriteLine("({0},{1})", point.X, point.Y);
                _x = point.X;
                _y = point.Y;
            }
        }
    }
}
