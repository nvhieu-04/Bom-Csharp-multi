using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BombermanMultiplayer
{
    public class Client
    {
        TcpClient sender;
        IFormatter formatter;
        NetworkStream stream;
        
        public Client(string endPointIP, int endPointPort)
        {

            try
            {
                sender = new TcpClient(endPointIP, endPointPort);
                stream = sender.GetStream();
                formatter = new BinaryFormatter();
            }
            catch (IOException se)
            {
                MessageBox.Show("A connection error occured, connection closed, please restart the game");
                this.sender.Close();
            }
            catch (SocketException se)
            {
                MessageBox.Show("Unable to connect");
                this.sender.Close();
            }
            catch (Exception) { }
        }

        public void sendData(Packet obj)
        {
            try
            {
               formatter.Serialize(stream, obj);
               stream.Flush();
            }
            catch (IOException se)
            {
                MessageBox.Show("A connection error occured, connection close, please restart the game");
                this.sender.Close();
                
            }
            catch (SocketException se)
            {
                MessageBox.Show("A connection error occured, connection close, please restart the game");
                this.sender.Close();
            }
            catch (Exception) { }
        }

        public void RecvData(ref Packet obj)
        {
            while (stream.DataAvailable)
            {
                obj = (Packet)formatter.Deserialize(stream);
            }
        }

        public void Disconnect()
        {
            this.sender.Close();
        }

        public bool GetConnectionState()
        {
            return this.sender.Connected;
        }

    }
}
