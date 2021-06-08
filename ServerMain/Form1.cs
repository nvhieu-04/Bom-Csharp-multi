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
    public partial class Form1 : Form
    {
        string currentData;
        const int _buffer = 1024;
        Socket server;
        public List<Player> listPlayer;
        int[] portServer = { 8000, 8050, 8080 };

        int i = 0;
        int _idRoom = 0;
        public Form1()
        {
            InitializeComponent();
            
        }
        public void Start()
        {

            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listPlayer = new List<Player>();
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, 25000);
            try
            {
                server.Bind(iep);

            }
            catch
            {

            }
            Thread Listen = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        server.Listen(500);
                        Socket client = server.Accept();
                        Player _player = new Player();
                        _player.client = client;

                        listPlayer.Add(_player);

                        _player.client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                        Thread receive = new Thread(() => Receive(_player));
                        receive.IsBackground = true;
                        receive.Start();
                    }

                }   
                catch { }
            });
            Listen.IsBackground = true;
            Listen.Start();

        }
        public void Receive(Player playerInfo)
        {
           
            currentData = "";
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[_buffer * 5];
                    playerInfo.client.Receive(buffer);
                    currentData = (string)Deserialize(buffer);
                    string Data = currentData as string;

                    string[] strList = Data.Split(';');
                    if (strList[0].Equals("newroom"))
                    {
                        if (strList[1].Equals("yes"))
                        {
                            _idRoom++;
                            playerInfo.port = portServer[i];
                            playerInfo.Room = strList[3];
                            playerInfo.Username = strList[4];
                            playerInfo.idRoom = _idRoom;
                            string send = "yes" + ';' + playerInfo.port.ToString();
                            string getInfo ="Phòng" +"[" + _idRoom + "]" + ": "  + strList[3]+ "- Tên chủ phòng: " + strList[4];
                            Send(send, playerInfo.client);
                            UpdateChatHistoryThreadSafe(getInfo);
                        }
                        i++;
                    }

                    if (strList[0].Equals("find"))
                    {
                        int h = 0; //h : dung de nhan biet de gui cai nao
                        int idRoomForFindClient = 0;
                        for (int j = 0; j < listPlayer.Count; j++)
                        {
                            if (strList[1] == listPlayer[j].Room)
                            {
                                h++;
                                idRoomForFindClient = listPlayer[j].idRoom;
                                string send = "existRoom; " + listPlayer[j].port.ToString();
                                Send(send, playerInfo.client);
                                break;
                            }
                        }
                        if (h == 0)
                        {
                            string send = "notExistRoom;";
                            string getInfo = strList[1] + strList[2];
                            Send(send, playerInfo.client);
                        }
                        else
                        {
                            string getInfo ="Phòng" +"[" + idRoomForFindClient+"]"+": "+ strList[1] + "##Tên người chơi: " + strList[2];
                            UpdateChatHistoryThreadSafe(getInfo);
                        }
                    }
                }
            }
            catch
            { }
        }
        private delegate void SafeCallDelegate(string text);

        private void UpdateChatHistoryThreadSafe(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                var d = new SafeCallDelegate(UpdateChatHistoryThreadSafe);
                richTextBox1.Invoke(d, new object[] { text });
            }
            else
            {
                richTextBox1.Text += text+'\n';
            }
        }
        public void Send(string data_need_to_be_sent, Socket client)
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
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Start();
            button1.Enabled = false;
        }
      
    }
    public class Player
    {
        public string Username;
        public string Room;
        public Socket client;
        public int port;
        public int idRoom = 0;
    }
}
