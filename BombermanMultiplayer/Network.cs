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
using System.Text;

namespace BombermanMultiplayer
{
    public class Network
    {
        public int _portRoom;
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

        public void Receive()
        {
            _currentData = new object();
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[_buffer * 5];
                    _client.Receive(buffer);
                    _currentData = Deserialize(buffer);
                    string getPort = _currentData as string;
                    string[] arrGetPort = getPort.Split(';');
                    if(arrGetPort[0] == "yes")
                    {
                        _portRoom = int.Parse(arrGetPort[1]);
                    }
                    else if(arrGetPort[0] == "existRoom")
                    {
                        _portPEER = int.Parse(arrGetPort[1]);
                    }
                    else if(arrGetPort[0] == "noExistRoom")
                    {
                        _portPEER = 0;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Cann't receive");
            }
        }
        public void CreateNewServer(int port)
        {
            
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
                    MessageBox.Show("Loi o create server");
                }
            }
            _serverPEER.Dispose();

            
        }


        public void Send(string data_need_to_be_sent)
        {   
            try 
            {
               
                _client.Send(Serialize(data_need_to_be_sent));
            }
            catch
            {
                MessageBox.Show("Loi o send network ");
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

