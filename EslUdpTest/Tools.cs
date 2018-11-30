using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace EslUdpTest
{
    public class Tools
    {
        public Tools()
        {
        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder stringBuilder = new StringBuilder((int)ba.Length * 2);
            byte[] numArray = ba;
            for (int i = 0; i < (int)numArray.Length; i++)
            {
                stringBuilder.AppendFormat("{0:X2}", numArray[i]);
            }
            return stringBuilder.ToString();
        }

        public static string ConvertBinaryToHex(string strBinary)
        {
            return Convert.ToInt32(strBinary, 2).ToString("x8");
        }

        public static int ConvertHexToInt(string hex)
        {
            return int.Parse(hex, NumberStyles.HexNumber);
        }

        public static string ConvertHexToString(byte[] HexValue)
        {
            return Encoding.UTF8.GetString(HexValue);
        }

        public static string ConvertHexToString(string HexValue)
        {
            string str = "";
            while (HexValue.Length > 0)
            {
                char chr = Convert.ToChar(Convert.ToUInt32(HexValue.Substring(0, 2), 16));
                str = string.Concat(str, chr.ToString());
                HexValue = HexValue.Substring(2, HexValue.Length - 2);
            }
            return str;
        }

        public static string ConvertStringToHex(string text)
        {
            return Tools.ByteArrayToString(Encoding.UTF8.GetBytes(text));
        }

        public static string ConvertCharToHex(string text)
        {
            char[] values = text.ToCharArray();
            string result = "";
            foreach (char letter in values)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(letter);
                // Convert the decimal value to a hexadecimal value in string form.
                string hexOutput = String.Format("{0:X}", value);
                result = result + hexOutput;
                Console.WriteLine("Hexadecimal value of {0} is {1}", letter, hexOutput);
            }

            return result;
        }
        
        public static byte[] iCheckSum(byte[] data)
        {
            byte[] numArray = new byte[2];
            int num = 0;
            for (int i = 0; i < (int)data.Length; i++)
            {
                num += data[i];
            }
            byte[] bytes = BitConverter.GetBytes(num);
            Array.Reverse(bytes);
            numArray[0] = bytes[(int)bytes.Length - 2];
            numArray[1] = bytes[(int)bytes.Length - 1];
            return numArray;
        }

        public static string IntToHex(int iValue, int len)
        {
            string str = null;
            if (len == 1)
            {
                str = iValue.ToString("X");
            }
            else if (len == 2)
            {
                str = iValue.ToString("X2");
            }
            else if (len == 3)
            {
                str = iValue.ToString("X3");
            }
            else if (len == 4)
            {
                str = iValue.ToString("X4");
            }
            else if (len == 5)
            {
                str = iValue.ToString("X5");
            }
            else if (len == 6)
            {
                str = iValue.ToString("X6");
            }
            return str;
        }

        public void SNC_GetAP_Info()
        {
            List<AP_Information> old = new List<AP_Information> { };
            /* byte[] data = new byte[4]; //broadcast data
             data[0] = 0xff;
             data[1] = 0x01;
             data[2] = 0x01;
             data[3] = 0x02;*/
            // byte[] data = new byte[] { 0x77, 0x77, 0x77, 0x2E, 0x75, 0x73, 0x72, 0x2E, 0x63, 0x6E };
            byte[] data = Encoding.ASCII.GetBytes("HF-A11ASSISTHREAD");
            IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, 48899); //braodcast IP address, and corresponding port
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces(); //get all network interfaces of the computer
            foreach (NetworkInterface adapter in nics)
            {
                // Only select interfaces that are Ethernet type and support IPv4 (important to minimize waiting time)
                if (adapter.NetworkInterfaceType != NetworkInterfaceType.Wireless80211) { continue; }
                if (adapter.Supports(NetworkInterfaceComponent.IPv4) == false) { continue; }
                try
                {
                    IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                    foreach (var ua in adapterProperties.UnicastAddresses)
                    {
                        if (ua.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            //SEND BROADCAST IN THE ADAPTER
                            Console.WriteLine("FFFFFFFFFFFFF" + ua.Address);
                            Socket bcSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp); //broadcast socket
                            bcSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                            bcSocket.ReceiveTimeout = 200; //receive timout 200ms
                            IPEndPoint myLocalEndPoint = new IPEndPoint(ua.Address, 48899);
                            bcSocket.Bind(myLocalEndPoint);
                            bcSocket.SendTo(data, ip);
                            //RECEIVE BROADCAST IN THE ADAPTER
                            int BUFFER_SIZE_ANSWER = 1024;
                            byte[] bufferAnswer = new byte[BUFFER_SIZE_ANSWER];

                            do
                            {
                                try
                                {
                                    IPEndPoint sssss = new IPEndPoint(IPAddress.Any, 0);
                                    EndPoint Remote = (EndPoint)(sssss);
                                    var recv = bcSocket.Receive(bufferAnswer);
                                    var redata = new byte[recv];
                                    Array.Copy(bufferAnswer, 0, redata, 0, recv);
                                    Console.WriteLine("recv" + recv);
                                    Console.WriteLine("redata" + redata);
                                    Console.WriteLine("SSSSSSSSSSS" + Tools.ByteArrayToString(redata));
                                    Console.WriteLine("AAAAAAAAAAA" + Tools.ConvertHexToString(Tools.ByteArrayToString(redata)));
                                    if (recv == 27)
                                    {
                                        string str = Tools.ByteArrayToString(redata);
                                        Console.WriteLine("str" + str);
                                        string data2 = Tools.ConvertHexToString(str);
                                        string[] IPMAC = data2.Split(',');
                                        string str1 = IPMAC[0];
                                        string str2 = IPMAC[1];

                                        AP_Information mAP_Information = new AP_Information();
                                        mAP_Information.AP_IP = str1;
                                        mAP_Information.AP_MAC_Address = str2;

                                        old.Add(mAP_Information);
                                    }
                                }
                                catch (Exception e)
                                {
                                    // Console.Write(e.ToString() + Environment.NewLine);
                                    // bcSocket.Close();
                                    break;
                                }
                            } while (bcSocket.ReceiveTimeout != 0); //fixed receive timeout for each adapter that supports our broadcast
                            bcSocket.Close();
                        }
                    }
                }

                catch { }
            }
            ApScanEventArgs obj = new ApScanEventArgs();
            obj.data = old;
            onApScanEvent(this, obj);
        }

        public static byte[] StringToByteArray(string hex)
        {
            int length = hex.Length;
            byte[] num = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                num[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return num;
        }

        public event EventHandler onApScanEvent;

        public class AP_Information
        {
            public string AP_IP = "";

            public string AP_MAC_Address = "";

            public string AP_Name = "";

            public AP_Information()
            {
            }
        }

        public class ApScanEventArgs : EventArgs
        {
            public List<Tools.AP_Information> data;

            public ApScanEventArgs()
            {
            }
        }
    }
}