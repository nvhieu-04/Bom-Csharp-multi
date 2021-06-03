using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Threading;
using System.Data.SqlClient;
using System.Data;

namespace BombermanMultiplayer
{
    public class Network
    {
        public int _port;
        public Socket _clientPEER;
        public Socket _serverPEER;
        public Socket _client;
        public object _currentData;
        public int _portPEER;
 
        public const int _buffer = 1024;
        public void Start()
        {
            _client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 25000);
            _client.Connect(ipe);
            Thread listen = new Thread(Receive);
            listen.IsBackground = true;
            listen.Start();
            
        }

        private void Receive()
        {
            _currentData = new object();
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[_buffer * 5];
                    _client.Receive(buffer);
                    _currentData = Deserialize(buffer);
                    string data = _currentData as string;
                    char[] b = { ';' };
                    Int32 count = 4;
                    String[] strList = data.Split(b,count,StringSplitOptions.RemoveEmptyEntries);

                    if(strList[0].Equals("newroom"))
                    {
                        if(strList[1].Equals("yes")&&strList[2].Equals("ok"))
                        {
                            //int port = 30000;
                            //IPEndPoint iPEnd = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                            //_serverPEER = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            //while (true)
                            //{
                            //    try
                            //    {

                            //        _serverPEER.Bind(iPEnd);

                            //        break;

                            //    }
                            //    catch
                            //    {
                            //        port++;
                            //    }
                            //}
                            //_port = port;
                               

                        }
                        if (strList[1].Equals("no") && strList[2].Equals("ok"))
                        {
                            _portPEER = Int32.Parse(strList[3]);
                            

                        }
                    }    

                }
            }
            catch
            { 

            }
        }
        public bool CreateNewServer()
        {
            int port = 30000;
            IPEndPoint iPEnd = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            _serverPEER = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            while (true)
            {
                try
                {
                    
                    _serverPEER.Bind(iPEnd);
                    break;

                }
                catch
                {
                    port++;
                }
            }
            _port = port;
            _serverPEER.Dispose();

            return true;
        }


        public void Send(string data_need_to_be_sent)
        {   
            try { _client.Send(Serialize(data_need_to_be_sent)); }
            catch
            {
                MessageBox.Show(" ");
            }
        }
        byte[] Serialize(object o)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, o);
            return ms.ToArray();
        }
        object Deserialize(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryFormatter bf = new BinaryFormatter();
            return bf.Deserialize(ms);
        }
    }
}

