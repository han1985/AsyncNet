using HNetProtocal;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace HNet
{
    public class AsyncTool
    {
        public static byte[] PackLenInfo(byte[] data)
        {
            int len = data.Length;
            byte[] result = new byte[len + 4];
            byte[] head = BitConverter.GetBytes(len);
            head.CopyTo(result, 0);
            data.CopyTo(result, 4);
            return result;
        }

        public static byte[] Serialize(AsyncMsg msg)
        {
            byte[] data = null;
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                bf.Serialize(ms, msg);
                ms.Seek(0, SeekOrigin.Begin);
                data = ms.ToArray();
            }
            catch (SerializationException ex)
            {
                ErrorLog($"error Serialize:{ex.Message}");
            }
            finally
            {
                ms.Close();
            }

            return data;
        }

        public static AsyncMsg  DeSerialize(byte[] bytes)
        {
            AsyncMsg data = null;
            MemoryStream ms = new MemoryStream(bytes);
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                data = bf.Deserialize(ms) as AsyncMsg;

            }
            catch (SerializationException ex)
            {
                ErrorLog($"error DeSerialize:{ex.Message}");
            }
            finally
            {
                ms.Close();
            }

            return data;
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

        public static Action<string> LogFunc { get; set; }
        public static Action<string> ColorLogFunc { get; set; }
        public static Action<string> WarnFunc { get; set; }
        public static Action<string> ErrorFunc { get; set; }
        public static void Log(string msg, params object[] args)
        {
            msg = string.Format(msg, args);

            if(LogFunc != null)
            {
                LogFunc(msg);
            }
            else
            {
                Console.WriteLine(msg);
            }
        }

        public static void ColorLog(AsyncLogColor collor,string msg, params object[] args)
        {
            msg = string.Format(msg, args);

            if (ColorLogFunc != null)
            {
                ColorLogFunc(msg);
            }
            else
            {
                ConsoloeLog(msg, collor);
            }
        }
        public static void WarnLog(string msg, params object[] args)
        {
            msg = string.Format(msg, args);

            if (WarnFunc != null)
            {
                WarnFunc(msg);
            }
            else
            {
                ConsoloeLog(msg, AsyncLogColor.Yellow);
            }
        }

        public static void ErrorLog(string msg, params object[] args)
        {
            msg = string.Format(msg, args);

            if (ErrorFunc != null)
            {
                ErrorFunc(msg);
            }
            else
            {
                ConsoloeLog(msg, AsyncLogColor.Red);
            }
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
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    }
                case AsyncLogColor.Blue:
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    }
                case AsyncLogColor.Cyan:
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    }
                case AsyncLogColor.Magenta:
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    }
                case AsyncLogColor.Yellow:
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.Gray;
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
