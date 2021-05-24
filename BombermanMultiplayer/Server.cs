using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BombermanMultiplayer
{
    /// <summary>
    /// Server of a bomberman Game
    /// </summary>
    public class Server
    {

       
        //List the connections with which the server is connected
        public List<Connection> connections = new List<Connection>();
        //Socket of the server
        TcpListener server;
        //Determine if the game is running or not
        public bool IsRunning = false;
        //If all players are not connected
        private bool WaitingPlayers = true;

        private int port = 3000;

        //The variable describing the state of the game, who'll be sended throught network
        GameState gamestate;

        //All logic game elements
        Game game;



        //Send Packet
        Packet TX_Packet;
        //Receive Packet
        Packet RX_Packet;
        //Determine the person who's sending a packet
        Sender Station = Sender.Server;

        public Server()
        {

        }

        public Server(int port_)
        {
            port = port_;
        }

        /// <summary>
        /// Launch the listening of connections and after the treatement of game data
        /// </summary>
        /// <param name="token"> the token to cancel the task (server running)</param>
        public void Launch(CancellationToken token, string fileName = null)
        {
            server = new TcpListener(IPAddress.Any, 3000);
            server.Start();
            IsRunning = true;

            while (WaitingPlayers)
            {
                AcceptConnections();

                //When there's two players, no need to wait anymore
                if(connections.Count == 2)
                {
                    WaitingPlayers = false;
                    Thread.Sleep(1000);
                }

                //SHutdown
                if (token.IsCancellationRequested)
                {
                    //Disconnect all clients
                    foreach (var item in connections)
                    {
                        item.sock.Close();
                    }

                    server.Stop();
                    token.ThrowIfCancellationRequested();
                }



                Thread.Sleep(5);
            }


            //If we load and old savegame
            if (fileName != null)
            {
                //Load an existing game
                game = new Game();
                game.LoadGame(fileName);
            }
            else
            {
                //Start new game game
                game = new Game(528, 528);
            }

            gamestate = new GameState();            
            int PlayersReady = 0;
           
            System.Timers.Timer GameStateTime = new System.Timers.Timer(120);
            GameStateTime.Elapsed += GameStateTime_Elapsed;
            


            //Game initialized, need to transfer the map to all players now
            this.SendData(TX_Packet = new Packet(Station, PacketType.MapTransfer, game.world.MapGrid));

            RX_Packet = new Packet();

            //Wait till all players have received the map
            while (PlayersReady < 2)
            {
                
                //Try to get a packet
                this.RecvData(ref RX_Packet);

                //If there's a packet
                if (RX_Packet.GetPacketType() == PacketType.Ready)
                {
                    PlayersReady++;
                    switch (RX_Packet.GetSender())
                    {
                        case Sender.Player1:
                            game.player1.Name = RX_Packet.GetPayload<string>();
                            break;
                        case Sender.Player2:
                            game.player2.Name = RX_Packet.GetPayload<string>();
                            break;
                    }

                    RX_Packet = new Packet();

                }


            }

            //In cas of a loaded save
            if (!game.Paused)
            {
                game.LogicTimer.Start();
            }

            //Start sending datas about the state of the game
            GameStateTime.Start();

            //Server Loop
            while (true)
            {
                //Try to get a packet
                this.RecvData(ref RX_Packet);

                //If there's a packet
                if (!RX_Packet.Empty())
                {
                    switch (RX_Packet.GetPacketType())
                    {
                        case PacketType.Connection:
                            break;
                        case PacketType.KeyDown:
                            game.Game_KeyDownWithoutSprite(RX_Packet.GetPayload<Keys>(), RX_Packet.GetSender());
                            break;
                        case PacketType.KeyUp:
                            game.Game_KeyUpWithoutSprite(RX_Packet.GetPayload<Keys>(), RX_Packet.GetSender());
                            break;
                        case PacketType.CloseConnection:
                            this.SendData(new Packet(Sender.Server, PacketType.CloseConnection, 1));
                            break;
                        default:
                            break;
                    }
                    RX_Packet = new Packet();
                }


                Thread.Sleep(1);


                //Shutdown
                if (token.IsCancellationRequested)
                {
                    GameStateTime.Stop();
                    try
                    {
                        this.server.Server.Shutdown(SocketShutdown.Both);
                        this.server.Server.Close();

                        //Disconnect all clients
                        foreach (var item in connections)
                        {
                            item.sock.Client.Shutdown(SocketShutdown.Both);
                            item.sock.Client.Close();
                        }

                    }
                    catch (Exception)
                    {

                        //throw;
                    }


                    server.Stop();
                    token.ThrowIfCancellationRequested();

                }


            }
            

        }


        /// <summary>
        /// Callback from the timer, send the game state to every clients 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameStateTime_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            gamestate.Paused = this.game.Paused;

            gamestate.Over = this.game.Over;
            gamestate.Winner = this.game.Winner;

            //Send game state to everyone
            gamestate.XY_Position_Player1 = new short[2] { (short)this.game.player1.Source.X, (short)this.game.player1.Source.Y };
            gamestate.XY_Position_Player2 = new short[2] { (short)this.game.player2.Source.X, (short)this.game.player2.Source.Y };

            gamestate.framePlayer1 = (short)game.player1.frameindex;
            gamestate.framePlayer2 = (short)game.player2.frameindex;

            gamestate.orientationPlayer1 = game.player1.Orientation;
            gamestate.orientationPlayer2 = game.player2.Orientation;

            gamestate.BonusSlotPlayer1 = game.player1.BonusSlot;
            gamestate.BonusTimerPlayer1 = game.player1.BonusTimer;

            gamestate.BonusSlotPlayer2 = game.player2.BonusSlot;
            gamestate.BonusTimerPlayer2 = game.player2.BonusTimer;

            gamestate.NamePlayer1 = game.player1.Name;
            gamestate.NamePlayer2 = game.player2.Name;


            gamestate.bombsList = game.BombsOnTheMap;


            //Use a mask of short to represent the map
            gamestate.map = game.BuildMapMask();


            TX_Packet = new Packet(Station, PacketType.GameState, gamestate);

            this.SendData(TX_Packet);

        }




        /// <summary>
        /// Accept a incoming connection
        /// </summary>
        private void AcceptConnections()
        {
            if (server.Pending())
            {
                Connection conn = new Connection();
                conn.sock = server.AcceptTcpClient();
                conn.stream = conn.sock.GetStream();
                conn.formatter = new BinaryFormatter();
                connections.Add(conn);

                //Send Player list to all players
                SendPlayersList();

                
            }
        }

        /// <summary>
        /// Send the list of the client with which the server has established a connection
        /// </summary>
        private void SendPlayersList()
        {
            List<string> infosPlayers = new List<string>(connections.Count);

            StringBuilder s = new StringBuilder();

            for (int i = 0; i < connections.Count; i++)
            {
                infosPlayers.Add(s.Append("Player " + i + " : " + connections[i].sock.Client.RemoteEndPoint.ToString()).ToString());
                s.Clear();
            }


            Packet PlayersList = new Packet(Station, PacketType.Connection, infosPlayers);
            this.SendData(PlayersList);
        }

        /// <summary>
        /// Send data to all clients, using a packet class to encapsulate the datas
        /// </summary>
        /// <param name="obj">Class which encapsulates the datas</param>
        public void SendData(Packet obj)
        {
            try
            {
                // send to all
                foreach (Connection c in connections)
                {
                    c.formatter.Serialize(c.stream, obj);
                    c.stream.Flush();
                }
            }
            catch(Exception) { }

        }
        /// <summary>
        /// Receive data from the clients 
        /// </summary>
        /// <param name="obj">Class which encapsulates the datas</param>
        public void RecvData(ref Packet obj)
        {
            foreach (Connection c in connections)
            {
                while (c.stream.DataAvailable)
                {
                    obj = (Packet)c.formatter.Deserialize(c.stream);
                }
            }
        }

    }
    /// <summary>
    /// Represent a connection with a client
    /// </summary>
    public class Connection
    {

        public TcpClient sock;
        public IFormatter formatter = new BinaryFormatter();
        public NetworkStream stream;
    }
}
