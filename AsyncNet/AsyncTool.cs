using System;
using System.Collections.Generic;
using System.Text;

namespace HNet
{
    internal class AsyncTool
    {
        public AsyncTool() { 
        
        }

        public enum AsyncLogColor
        {
            None,
            Red,
            Green,
            Blue,
            Cyan,
            Magenta,
            Yellow,
        }

        public static void ConsoloeLog(string msg, AsyncLogColor color)
        {
            switch (color)
            {
                case AsyncLogColor.None:
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(msg);
                        Console.ForegroundColor= ConsoleColor.Gray;
                        break;
                    }
                case AsyncLogColor.Red:
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    }
                case AsyncLogColor.Green:
                    {
                        break;
                    }
                case AsyncLogColor.Blue:
                    {
                        break;
                    }
                case AsyncLogColor.Cyan:
                    {
                        break;
                    }
                case AsyncLogColor.Magenta:
                    {
                        break;
                    }
                case AsyncLogColor.Yellow:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }


    }
}
