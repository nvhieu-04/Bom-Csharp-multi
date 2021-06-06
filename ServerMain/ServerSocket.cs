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
namespace ServerMain
{
    class ServerSocket
    {
        string currentData;
        const int _buffer = 1024;
        Socket server;
        public List<Player> Player;
        int[] portServer = { 8000, 8050, 8080 };
      
        int i = 0;
        
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
                    
                    string[] strList = Data.Split(';');
                    if (strList[0].Equals("newroom"))
                    {
                        if (strList[1].Equals("yes"))
                        {
                            player.port = portServer[i];
                            player.Username = strList[3];
                            string send = "yes" + ';' + player.port.ToString();
                            Send(send, player.client);

                        }
                        i++;
                    }
                        
                    if (strList[0].Equals("find"))
                    {
                        int h = 0; //h : dung de nhan biet khi nao gui
                        for(int j = 0; j < Player.Count; j++)
                        {
                            if(strList[1] == Player[j].Username)
                            {
                                h++;
                                if (h > 0)
                                {
                                    string send = "existRoom; " + Player[j].port.ToString();
                                    Send(send, player.client);
                                }
                            }
                        }
                        if( h == 0)
                        {
                            string send = "notExistRoom;";
                            Send(send, player.client);
                        }
                        
                    }
                 }
            }
            catch
            { }

        }
        public void Send(string data_need_to_be_sent,Socket client)
        {
            try 
            {
                client.Send(Serialize(data_need_to_be_sent));
            }
            catch
            {
                MessageBox.Show("loi o send server ");
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
