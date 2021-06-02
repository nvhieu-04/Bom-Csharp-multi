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

namespace ServerMain
{
    class ServerSocket
    {
        string currentData;
        const int _buffer = 1024;
        Socket server;
        List<Player> Player;
        public void Start()
        {
            
            server = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            Player = new List<Player>();
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, 25000);
            try
            {
                server.Bind(iep);

            }
            catch {
            
            }
            Thread Listen = new Thread(() =>
            {
                try { 
                while(true)
                    {
                        server.Listen(500);
                        Socket client = server.Accept();
                        Player player = new Player();
                        player.client = client;
                        Player.Add(player);
                        player.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                        Thread receive = new Thread(Receive);
                        receive.IsBackground = true;
                        receive.Start(player);
                    }

                }
                catch { }
            });
            Listen.IsBackground = true;
            Listen.Start();
            
        }

        public void Receive(object obj)
        {
            Player player = obj as Player;
            currentData = "";
            try
            {
                while(true)
                {
                    byte[] buffer = new byte[_buffer * 5];
                    player.client.Receive(buffer);
                    currentData = (string) Deserialize(buffer);
                    string Data = currentData as string;
                    char[] b = {';' };
                    Int32 count = 5;
                    String[] strList = Data.Split(b, count, StringSplitOptions.RemoveEmptyEntries);
                    if(strList[0].Equals("newroom"))
                    {
                        if(strList[1].Equals("yes"))
                        {
                            Send("newroom;yes;ok",player.client);
                            player.port = Int32.Parse(strList[3]);
                            player.Username = strList[4];

                        }
                        if (strList[1].Equals("no"))
                        {
                            foreach(Player item in Player)
                            {
                                if(item.Username.Equals(strList[2]))
                                {

                                    Send("newroom;no;ok;"+item.port, player.client);
                                    break;
                                }
                            }

                        }

                    }    
                }
            }
            catch
            { }

        }
        public void Send(string data_need_to_be_sent, Socket client)
        {
            try { client.Send(Serialize(data_need_to_be_sent)); }
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
    public class Player
    {
        public string Username;
        public Socket client;
        public int port;
        public Room room;

    }
    public class Room
    {
        string Keyuser;
        bool isEmpty;
    }
    
}
