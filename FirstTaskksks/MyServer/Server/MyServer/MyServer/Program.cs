using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Runtime.InteropServices;
namespace CSCS1
{
    class ReceivingProgram
    {
        static void Main(string[] args)
        {
            ReceiveMessage();
        }
        private static void ReceiveMessage()
        {
            IntPtr handle = GetStdHandle(-11);
            GetConsoleMode(handle, out int mode);
            SetConsoleMode(handle, mode | 0x4);
            int port = 1001;
            Commands commands = new Commands();
            UdpClient receiver = new UdpClient(port);
            IPEndPoint remoteIp = new IPEndPoint(IPAddress.Any, 0);
            IPEndPoint iPEndPoint;
            byte commandNum;
            byte command;
            Int16 x0, y0;
            Int16 x1, y1;
            Int16 radius;
            string text;
            string hexcolor;
            Int16 rotation;
            try
            {
                while (true)
                {
                    byte[] data = receiver.Receive(ref remoteIp);
                    commandNum = data[0];
                    switch (commandNum)
                    {
                        case 1:
                            commands.ClearDisplayDecode(data, out command, out
                            hexcolor);
                            Console.WriteLine($"Recieved command: clear display; color: 0x{ hexcolor}; ");
                    break;
                        case 2:
                            commands.PixelDecode(data, out command, out x0, out
                            y0, out hexcolor);
                            Console.WriteLine($"Recieved command: draw pixel; x: { x0}; y: { y0}; color: 0x{ hexcolor}; ");
                    break;
                        case 3:
                            commands.FourNumbersDecode(data, out command, out x0,
                            out y0, out x1, out y1, out hexcolor);
                            Console.WriteLine($"Recieved command: draw line; x0: { x0}; y0: { y0}; x1: { x1}; y1: { y1}; color: 0x{ hexcolor}; ");
                    break;
                        case 4:
                            commands.FourNumbersDecode(data, out command, out x0,
                            out y0, out x1, out y1, out hexcolor);
                            Console.WriteLine($"Recieved command: draw rectangle; x: { x0}; y: { y0}; width: { x1}; height: { y1}; color: 0x{ hexcolor}; ");
                    break;
                        case 5:
                            commands.FourNumbersDecode(data, out command, out x0,
                            out y0, out x1, out y1, out hexcolor);
                            Console.WriteLine($"Recieved command: fill rectangle; x: { x0}; y: { y0}; width: { x1}; height: { y1}; color: 0x{ hexcolor}; ");
                    break;
                        case 6:
                            commands.FourNumbersDecode(data, out command, out x0,
                            out y0, out x1, out y1, out hexcolor); Console.WriteLine($"Recieved command: draw ellipse; x: { x0}; y: { y0}; radius x: { x1}; radius y: { y1}; color: 0x{ hexcolor}; ");
                    break;
                        case 7:
                            commands.FourNumbersDecode(data, out command, out x0,
                            out y0, out x1, out y1, out hexcolor);
                            Console.WriteLine($"Recieved command: fill ellipse; x: { x0}; y: { y0}; radius x: { x1}; radius y: { y1}; color: 0x{ hexcolor}; ");
                    break;
                        case 8:
                            commands.CircleDecode(data, out command, out x0, out
                            y0, out radius, out hexcolor);
                            Console.WriteLine($"Recieved command: draw circle; x: { x0}; y: { y0}; radius: { radius}; color: 0x{ hexcolor}; ");
                    break;
                        case 9:
                            commands.CircleDecode(data, out command, out x0, out
                            y0, out radius, out hexcolor);
                            Console.WriteLine($"Recieved command: fill circle; x: { x0}; y: { y0}; radius: { radius}; color: 0x{ hexcolor}; ");
                    break;
                        case 10:
                            commands.RoundedRectDecode(data, out command, out x0,
                            out y0, out x1, out y1, out radius, out hexcolor);
                            Console.WriteLine($"Recieved command: draw rounded rectangle; x: { x0}; y: { y0}; width: { x1}; height: { y1}; radius: { radius}; color:0x{ hexcolor}; ");
                    break;
                        case 11:
                            commands.RoundedRectDecode(data, out command, out x0,
                            out y0, out x1, out y1, out radius, out hexcolor);
                            Console.WriteLine($"Recieved command: fill rounded rectangle; x: { x0}; y: { y0}; width: { x1}; height: { y1}; radius: { radius}; color: 0x{ hexcolor}; ");
                    break;
                        case 12:
                            commands.TextDecode(data, out command, out x0, out y0,
                            out hexcolor, out x1, out y1, out text);
                            Console.WriteLine($"Recieved command: draw text; x: { x0}; y: { y0}; color: 0x{ hexcolor}; font number: { x1}; length: { y1}; text: { text}; ");
                    break;
                        case 13:
                            Color[,] colors;
                            commands.ImageDecode(data, out command, out x0, out
                            y0, out x1, out y1, out colors);
                            Console.WriteLine($"Recieved command: draw image; x: { x0}; y: { y0}; width: { x1}; height: { y1}; colors: ");
                    for (int col = 0; col < y1; col++)
                            {
                                for (int row = 0; row < x1; row++)
                                {
                                    Console.Write("\x1b[38;2;" + colors[row,
                                    col].R + ";" + colors[row, col].G + ";" + colors[row, col].B + "m");
                                    Console.Write("██");
                                }
                                Console.WriteLine("");
                            }
                            Console.WriteLine("");
                            Console.ResetColor();
                            break;
                        case 14:
                            rotation =
                            BitConverter.ToInt16(data.Skip(1).Take(2).ToArray(), 0);
                            Console.WriteLine($"Recieved command: set orientation; rotation angle: { rotation}; ");
                                break;
                    case 15:
                            Console.WriteLine($"Recieved command: get width;");
                            data =
                            BitConverter.GetBytes(Convert.ToInt16(Console.WindowWidth));
                            iPEndPoint = new IPEndPoint(remoteIp.Address,
                            remoteIp.Port);
                            Console.WriteLine($"Send: {Console.WindowWidth};");
                            receiver.Send(data, data.Length, iPEndPoint);
                            break;
                        case 16:
                            Console.WriteLine($"Recieved command: get height;");
                            data =
                            BitConverter.GetBytes(Convert.ToInt16(Console.WindowHeight));
                            iPEndPoint = new IPEndPoint(remoteIp.Address,
                            remoteIp.Port);
                            Console.WriteLine($"Send: {Console.WindowHeight};");
                            receiver.Send(data, data.Length, iPEndPoint);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
            finally
            {
                receiver.Close();
            }
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr handle, out int mode);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int handle);
    }
}
