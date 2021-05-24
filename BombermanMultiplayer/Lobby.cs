using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BombermanMultiplayer
{
    public partial class Lobby : Form
    {
        //Server part
        Server server;
        Task runServer;
        //Used to cancel a task during her execution
        CancellationTokenSource cts;


        Client client;
        bool GameRunning = false;

        Packet TX_Packet;
        Packet RX_Packet;

        Regex ipParser = new Regex(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b");


        //Store the variable defining the state of the game
        GameState gamestate;
        
        //Identify who sends  packet
        Sender Station = 0;

        //Game Components
        Game game;
        Rectangle[] BonusSlot;
        private BufferedGraphics bufferG = null;
        private Graphics gr;
        private System.Timers.Timer TimerDelayKeyDown = new System.Timers.Timer(40);
        bool DelayKey = false;


        public Lobby()
        {
            InitializeComponent();



        }

        private void btnServer_Click(object sender, EventArgs e)
        {
            if (GameRunning == true)
            {
                return;
            }

            int port = 30000;

            try
            {
                int.TryParse(tbPortConnect.Text, out port);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error : " + ex.Message, "Problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }


            server = new Server(port);

            cts = new CancellationTokenSource();
            string fileName = tbGameToLoad.Text;

            //If there's a game to load
            if (fileName.Length > 0)
            {
                runServer = Task.Run(() => server.Launch(cts.Token, fileName), cts.Token);
            }
            else
            {
                //Default
                runServer = Task.Run(() => server.Launch(cts.Token), cts.Token);
            }

            lbServerOnline.Visible = true;
            PanelConnections.Visible = false;
            

            //Make a local connection the server
            client = new Client("127.0.0.1", 3000);

            Station = Sender.Player1;

            RX_Packet = new Packet();

            //Wait till data
            while (RX_Packet.Empty())
            {
                client.RecvData(ref RX_Packet);
            }

            List<string> PlayersInfos = RX_Packet.GetPayload<List<string>>();

            
            lbConnected.Items.Clear();

            for (int i = 0; i < PlayersInfos.Count; i++)
            {
                lbConnected.Items.Add(PlayersInfos[i]);

            }
            


            //Start timer to check for incoming packet on the server
            ConnectionTimer.Start();
            

        }



        private void btnClient_Click(object sender, EventArgs e)
        {

            if (GameRunning == true)
            {
                return;

            }

            int port = 0;

            
            if (!ipParser.IsMatch(tbAddressConnect.Text))
            {
                MessageBox.Show("No valid IP address", "Problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            try
            {
                int.TryParse(tbPortConnect.Text, out port);

                //Connexion
                client = new Client(tbAddressConnect.Text, port);

            }
            catch (Exception ex)
            {

                return;
            }



            RX_Packet = new Packet();

            while (RX_Packet.Empty())
            {
                client.RecvData(ref RX_Packet);
            }

            List<string> PlayersInfos = RX_Packet.GetPayload<List<string>>();


            lbConnected.Items.Clear();

            for (int i = 0; i < PlayersInfos.Count; i++)
            {
                lbConnected.Items.Add(PlayersInfos[i]);

            }
            PanelConnections.Visible = false;

            Station = Sender.Player2;

            ConnectionTimer.Start();
            TimerDelayKeyDown.Elapsed += TimerDelayKeyDown_Elapsed;

        }


        private void ConnectionTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                client.RecvData(ref RX_Packet);

                //If there's a data received 
                if (!RX_Packet.Empty())
                {
                   //If game haven't been launched yet
                    if (!GameRunning)
                    {
                        //If it's the player list incoming
                        if (RX_Packet.GetPacketType() == PacketType.Connection)
                        {
                            List<string> PlayersInfos = RX_Packet.GetPayload<List<string>>();
                            lbConnected.Items.Clear();
                            for (int i = 0; i < PlayersInfos.Count; i++)
                            {
                                lbConnected.Items.Add(PlayersInfos[i]);

                            }
                            
                        }
                        if (RX_Packet.GetPacketType() == PacketType.MapTransfer)
                        {

                            //transfering the random generated map
                            //this.pbGame.SizeMode = PictureBoxSizeMode.StretchImage;
                            
                            this.pbGame.Visible = this.panelGame.Visible = true;

                            
                            //this.panelGame.Size = new Size(3 * (this.pbGame.ClientSize.Width / 3), 3 * (this.pbGame.ClientSize.Width / 3));
                            
                            this.panelGame.Location = this.PanelConnections.Location;
                            this.ClientSize = new Size(this.ClientSize.Width , this.ClientSize.Height + 528);

                            this.pbGame.ClientSize = new Size(528, 528);
                            //Center picture box
                            pbGame.Left = (panelGame.Width - pbGame.Width) / 2;
                            //pbGame.Top = (panelGame.Height - pbGame.Height) / 2;



                            //Initialize the game
                            game = new Game(this.pbGame.Width, this.pbGame.Height);
                            game.world.MapGrid = RX_Packet.GetPayload<Tile[,]>();
                            LoadGameComponents();
                            GameRunning = true;
                            
                            //Send the player name
                            TX_Packet = new Packet(Station, PacketType.Ready, tbNamePlayer.Text);
                            client.sendData(TX_Packet);
                            
                        }
                    }
                    else
                    {
                        //Process game datas
                        if (RX_Packet.GetPacketType() == PacketType.GameState)
                        {
                            gamestate = RX_Packet.GetPayload<GameState>();

                            game.Paused = gamestate.Paused;

                            game.Over = gamestate.Over;
                            game.Winner = gamestate.Winner;
                            

                            game.player1.ChangeLocation(gamestate.XY_Position_Player1[0], gamestate.XY_Position_Player1[1]);
                            game.player2.ChangeLocation(gamestate.XY_Position_Player2[0], gamestate.XY_Position_Player2[1]);

                            game.player1.frameindex = gamestate.framePlayer1;
                            game.player2.frameindex = gamestate.framePlayer2;

                            game.player1.Orientation = gamestate.orientationPlayer1;
                            game.player2.Orientation = gamestate.orientationPlayer2;

                            game.player1.BonusSlot = gamestate.BonusSlotPlayer1;
                            game.player1.BonusTimer = gamestate.BonusTimerPlayer1;

                            game.player2.BonusSlot = gamestate.BonusSlotPlayer2;
                            game.player2.BonusTimer = gamestate.BonusTimerPlayer2;

                            game.player1.Name = gamestate.NamePlayer1;
                            game.player2.Name = gamestate.NamePlayer2;


                            game.BombsOnTheMap = gamestate.bombsList;



                            //Map mask

                            //0 = free tile
                            //1 hard block
                            //2 destructible block
                            //3 fire

                            //Bonus
                            //10 PowerBomb and empty 
                            //11 SpeedBoost and empty 
                            //12 Desamorce and empty 
                            //13 Armor and empty 

                            //30 PowerBomb and Fire 
                            //31 SpeedBoost and Fire 
                            //32 Desamorce and Fire 
                            //33 Armor and Fire 

                            //Map
                            for (int i = 0; i < gamestate.map.GetLength(0); i++) //Ligne
                            {
                                for (int j = 0; j < gamestate.map.GetLength(1); j++) //Colonne
                                {
                                    if (gamestate.map[i, j] < 10 && game.world.MapGrid[i, j].BonusHere != null)
                                    {
                                        game.world.MapGrid[i, j].BonusHere = null;
                                    }

                                    switch (gamestate.map[i,j])
                                    {
                                        case 0:
                                            game.world.MapGrid[i, j].Walkable = true;
                                            game.world.MapGrid[i, j].Destroyable = game.world.MapGrid[i, j].Fire = false;
                                            break;
                                        case 1:
                                            game.world.MapGrid[i, j].Walkable = game.world.MapGrid[i, j].Destroyable = false;
                                            break;
                                        case 2:

                                            break;
                                        case 3:
                                            game.world.MapGrid[i, j].Walkable = game.world.MapGrid[i, j].Fire = true;
                                            game.world.MapGrid[i, j].Destroyable = false;
                                            break;

                                            //Bonus
                                        case 10:
                                            game.world.MapGrid[i, j].BonusHere =
                                                new Objects.Bonus(game.world.MapGrid[i, j].Source.X, game.world.MapGrid[i, j].Source.Y, 1,
                                                game.world.MapGrid[i, j].Source.Width, game.world.MapGrid[i, j].Source.Height, Objects.BonusType.PowerBomb);
                                            game.world.MapGrid[i, j].Destroyable = game.world.MapGrid[i, j].Fire = false;
                                            break;
                                        case 11:
                                            game.world.MapGrid[i, j].BonusHere =
                                                new Objects.Bonus(game.world.MapGrid[i, j].Source.X, game.world.MapGrid[i, j].Source.Y, 1,
                                                game.world.MapGrid[i, j].Source.Width, game.world.MapGrid[i, j].Source.Height, Objects.BonusType.SpeedBoost);
                                            game.world.MapGrid[i, j].Destroyable = game.world.MapGrid[i, j].Fire = false;
                                            break;
                                        case 12:
                                            game.world.MapGrid[i, j].BonusHere =
                                                new Objects.Bonus(game.world.MapGrid[i, j].Source.X, game.world.MapGrid[i, j].Source.Y, 1,
                                                game.world.MapGrid[i, j].Source.Width, game.world.MapGrid[i, j].Source.Height, Objects.BonusType.Desamorce);
                                            game.world.MapGrid[i, j].Destroyable = game.world.MapGrid[i, j].Fire = false;
                                            break;
                                        case 13:
                                            game.world.MapGrid[i, j].BonusHere =
                                                new Objects.Bonus(game.world.MapGrid[i, j].Source.X, game.world.MapGrid[i, j].Source.Y, 1,
                                                game.world.MapGrid[i, j].Source.Width, game.world.MapGrid[i, j].Source.Height, Objects.BonusType.Armor);
                                            game.world.MapGrid[i, j].Destroyable = game.world.MapGrid[i, j].Fire = false;
                                            break;

                                        case 30:
                                            game.world.MapGrid[i, j].BonusHere =
                                                new Objects.Bonus(game.world.MapGrid[i, j].Source.X, game.world.MapGrid[i, j].Source.Y, 1,
                                                game.world.MapGrid[i, j].Source.Width, game.world.MapGrid[i, j].Source.Height, Objects.BonusType.PowerBomb);
                                            game.world.MapGrid[i, j].Walkable = game.world.MapGrid[i, j].Fire = true;
                                            game.world.MapGrid[i, j].Destroyable = false;
                                            break;
                                        case 31:
                                            game.world.MapGrid[i, j].BonusHere =
                                                new Objects.Bonus(game.world.MapGrid[i, j].Source.X, game.world.MapGrid[i, j].Source.Y, 1,
                                                game.world.MapGrid[i, j].Source.Width, game.world.MapGrid[i, j].Source.Height, Objects.BonusType.SpeedBoost);
                                            game.world.MapGrid[i, j].Walkable = game.world.MapGrid[i, j].Fire = true;
                                            game.world.MapGrid[i, j].Destroyable = false;
                                            break;
                                        case 32:
                                            game.world.MapGrid[i, j].BonusHere =
                                                new Objects.Bonus(game.world.MapGrid[i, j].Source.X, game.world.MapGrid[i, j].Source.Y, 1,
                                                game.world.MapGrid[i, j].Source.Width, game.world.MapGrid[i, j].Source.Height, Objects.BonusType.Desamorce);
                                            game.world.MapGrid[i, j].Walkable = game.world.MapGrid[i, j].Fire = true;
                                            game.world.MapGrid[i, j].Destroyable = false;
                                            break;
                                        case 33:
                                            game.world.MapGrid[i, j].BonusHere =
                                                new Objects.Bonus(game.world.MapGrid[i, j].Source.X, game.world.MapGrid[i, j].Source.Y, 1,
                                                game.world.MapGrid[i, j].Source.Width, game.world.MapGrid[i, j].Source.Height, Objects.BonusType.Armor);
                                            game.world.MapGrid[i, j].Walkable = game.world.MapGrid[i, j].Fire = true;
                                            game.world.MapGrid[i, j].Destroyable = false;
                                            break;
                                    }
                                }
                            }
                            return;
                        }
                        else if (RX_Packet.GetPacketType() == PacketType.CloseConnection)
                        {
                            this.ConnectionTimer.Stop();
                            this.client.Disconnect();

                            if (server != null)
                            {
                                if (server.IsRunning && !cts.IsCancellationRequested)
                                {
                                    cts.Cancel();
                                    try
                                    {
                                        runServer.Wait();
                                    }
                                    catch (AggregateException ex)
                                    { }
                                    finally
                                    {
                                        cts.Dispose();
                                    }
                                }

                            }

                            MessageBox.Show("Other player left the game, back to menu...");
                            this.Close();

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("An error has occured : "+  ex.Message);
                //ConnectionTimer.Stop();
                //panelClient.Enabled = true;
            }
            
        }

        //Load all sprite 
        public void LoadAllMapSprites()
        {
            for (int i = 0; i < game.world.MapGrid.GetLength(0); i++) //Ligne
            {
                for (int j = 0; j < game.world.MapGrid.GetLength(1); j++) //Colonne
                {
                    if (!game.world.MapGrid[i,j].Destroyable && game.world.MapGrid[i, j].Walkable)
                    {
                        game.world.MapGrid[i, j].UnloadSprite();
                    }

                    if (game.world.MapGrid[i, j].Fire)
                        game.world.MapGrid[i, j].LoadSprite(Properties.Resources.Fire);
                    else if (!game.world.MapGrid[i, j].Fire && game.world.MapGrid[i, j].Walkable && !game.world.MapGrid[i, j].Destroyable)
                        game.world.MapGrid[i, j].UnloadSprite();

                    if (game.world.MapGrid[i, j].BonusHere != null)
                    {
                        switch (game.world.MapGrid[i,j].BonusHere.Type)
                        {
                            case Objects.BonusType.None:
                                break;
                            case Objects.BonusType.PowerBomb:
                                game.world.MapGrid[i, j].BonusHere.LoadSprite(Properties.Resources.SuperBomb);
                                break;
                            case Objects.BonusType.SpeedBoost:
                                game.world.MapGrid[i, j].BonusHere.LoadSprite(Properties.Resources.SpeedUp);
                                break;
                            case Objects.BonusType.Desamorce:
                                game.world.MapGrid[i, j].BonusHere.LoadSprite(Properties.Resources.Deactivate);
                                break;
                            case Objects.BonusType.Armor:
                                game.world.MapGrid[i, j].BonusHere.LoadSprite(Properties.Resources.Armor);
                                break;
                            default:
                                break;
                        }
                    }

                }
            }
            foreach (Bomb bomb in game.BombsOnTheMap)
            {
                if (bomb != null)
                    bomb.LoadSprite(Properties.Resources.Bombe);
            }
        }


        private void LoadGameComponents()
        {

            BonusSlot = new Rectangle[4];

            for (int i = 0; i < 4; i++)
            {
                BonusSlot[i] = new Rectangle(i * pbGame.Width / 18, pbGame.Height / 25, pbGame.Width / 20, pbGame.Height / 20);
                if (i > 1)
                {
                    BonusSlot[i] = new Rectangle(3 * pbGame.Width / 4 + i * pbGame.Width / 18, pbGame.Height / 25, pbGame.Width / 20, pbGame.Height / 20);
                }
            }

            //game.world.loadBackground(Properties.Resources.World);
            game.world.loadSpriteTile(Properties.Resources.BlockDestructible, Properties.Resources.BlockNonDestructible);
            game.player1.LoadSprite(Properties.Resources.AT_DOWN);
            game.player2.LoadSprite(Properties.Resources.T_UP);


            bufferG = BufferedGraphicsManager.Current.Allocate(pbGame.CreateGraphics(), pbGame.DisplayRectangle);
            gr = bufferG.Graphics;
            

            this.refreshGraphics.Start();

        }

        public void Draw()
        {

            if (!game.player1.Dead)
            {
                switch (game.player1.Orientation)
                {
                    case Player.MovementDirection.UP:
                        game.player1.LoadSprite(Properties.Resources.AT_UP);
                        break;
                    case Player.MovementDirection.DOWN:
                        game.player1.LoadSprite(Properties.Resources.AT_DOWN);
                        break;
                    case Player.MovementDirection.LEFT:
                        game.player1.LoadSprite(Properties.Resources.AT_LEFT);
                        break;
                    case Player.MovementDirection.RIGHT:
                        game.player1.LoadSprite(Properties.Resources.AT_RIGHT);
                        break;
                    case Player.MovementDirection.NONE:
                        break;
                    default:
                        break;
                }
            }
            else
                game.player1.LoadSprite(Properties.Resources.Blood);

            if (!game.player2.Dead)
            {
                switch (game.player2.Orientation)
                {
                    case Player.MovementDirection.UP:
                        game.player2.LoadSprite(Properties.Resources.T_UP);
                        break;
                    case Player.MovementDirection.DOWN:
                        game.player2.LoadSprite(Properties.Resources.T_DOWN);
                        break;
                    case Player.MovementDirection.LEFT:
                        game.player2.LoadSprite(Properties.Resources.T_LEFT);
                        break;
                    case Player.MovementDirection.RIGHT:
                        game.player2.LoadSprite(Properties.Resources.T_RIGHT);
                        break;
                    case Player.MovementDirection.NONE:
                        break;
                    default:
                        break;
                }
            }
            else
                game.player2.LoadSprite(Properties.Resources.Blood);

            gr.Clear(pbGame.BackColor);

            game.world.Draw(gr);

            game.player1.Draw(gr);
            game.player1.DrawPosition(gr);

            game.player2.Draw(gr);
            game.player2.DrawPosition(gr);

            foreach (Bomb bomb in game.BombsOnTheMap)
            {
                try
                {
                    bomb.Draw(gr);

                }
                catch (Exception)
                {

                }
            }

            DrawInterface();

            bufferG.Render();

        }

        public void DrawInterface()
        {

            if (game.Paused && !game.Over)
            {
                tslMenu.Visible = true;
                gr.DrawString("PAUSED", new System.Drawing.Font("Arial", 30), Brushes.White, pbGame.Width / 2, pbGame.Height / 2);

            }
            else if (!game.Paused && !game.Over)
            {
                tslMenu.Visible = false;
            }
            else
            {
                tslMenu.Visible = true;
            }


            if (game.Over)
            {
                gr.DrawString("GAME OVER", new Font("Stencil", (float)(this.pbGame.Height / 10), System.Drawing.FontStyle.Bold),
                     new SolidBrush(Color.WhiteSmoke), 0, this.pbGame.Height / 2 - this.pbGame.Height / 8);
                switch (game.Winner)
                {
                    case 1:
                        gr.DrawString("Soldier wins", new Font("Stencil", (float)(this.pbGame.Height / 10), System.Drawing.FontStyle.Bold),
                            new SolidBrush(Color.WhiteSmoke), 0, this.pbGame.Height / 2 - this.pbGame.Height / 8 + this.pbGame.Height / 9);
                        break;
                    case 2:
                        gr.DrawString("Terrorist wins", new Font("Stencil", (float)(this.pbGame.Height / 10), System.Drawing.FontStyle.Bold),
                            new SolidBrush(Color.WhiteSmoke), 0, this.pbGame.Height / 2 - this.pbGame.Height / 8 + this.pbGame.Height / 9);
                        break;
                    default:
                        gr.DrawString("Draw", new Font("Stencil", (float)(this.pbGame.Height / 10), System.Drawing.FontStyle.Bold),
                            new SolidBrush(Color.WhiteSmoke), 0, this.pbGame.Height / 2 - this.pbGame.Height / 8 + this.pbGame.Height / 9);
                        break;
                }

            }

            for (int j = 0; j < 2; j++)
            {

                //Bonus
                gr.DrawString("Player " + (int)(j + 1) + " : ", new System.Drawing.Font("Arial", 10), Brushes.White, BonusSlot[j * 2].X, BonusSlot[j * 2].Y - BonusSlot[j + 1].Width / 2);
                for (int i = 0; i < game.player1.BonusSlot.Length; i++)
                {
                    switch (game.player1.BonusSlot[i])
                    {
                        case Objects.BonusType.PowerBomb:
                            gr.DrawImage(Properties.Resources.SuperBomb, BonusSlot[i]);
                            gr.DrawString((game.player1.BonusTimer[i] / 1000).ToString() + "s", new System.Drawing.Font("Arial", 10), Brushes.White, BonusSlot[i].X, BonusSlot[i].Y + BonusSlot[i].Height);
                            break;
                        case Objects.BonusType.SpeedBoost:
                            gr.DrawImage(Properties.Resources.SpeedUp, BonusSlot[i]);
                            gr.DrawString((game.player1.BonusTimer[i] / 1000).ToString() + "s", new System.Drawing.Font("Arial", 10), Brushes.White, BonusSlot[i].X, BonusSlot[i].Y + BonusSlot[i].Height);
                            break;
                        case Objects.BonusType.Desamorce:
                            gr.DrawImage(Properties.Resources.Deactivate, BonusSlot[i]);
                            gr.DrawString((game.player1.BonusTimer[i] / 1000).ToString() + "s", new System.Drawing.Font("Arial", 10), Brushes.White, BonusSlot[i].X, BonusSlot[i].Y + BonusSlot[i].Height);
                            break;
                        case Objects.BonusType.Armor:
                            gr.DrawImage(Properties.Resources.Armor, BonusSlot[i]);
                            gr.DrawString((game.player1.BonusTimer[i] / 1000).ToString() + "s", new System.Drawing.Font("Arial", 10), Brushes.White, BonusSlot[i].X, BonusSlot[i].Y + BonusSlot[i].Height);
                            break;
                        case Objects.BonusType.None:
                            break;
                        default:
                            break;
                    }
                    switch (game.player2.BonusSlot[i])
                    {
                        case Objects.BonusType.PowerBomb:
                            gr.DrawImage(Properties.Resources.SuperBomb, BonusSlot[i + 2]);
                            gr.DrawString((game.player2.BonusTimer[i] / 1000).ToString() + "s", new System.Drawing.Font("Arial", 10), Brushes.White, BonusSlot[i + 2].X, BonusSlot[i + 2].Y + BonusSlot[i + 2].Height);
                            break;
                        case Objects.BonusType.SpeedBoost:
                            gr.DrawImage(Properties.Resources.SpeedUp, BonusSlot[i + 2]);
                            gr.DrawString((game.player2.BonusTimer[i] / 1000).ToString() + "s", new System.Drawing.Font("Arial", 10), Brushes.White, BonusSlot[i + 2].X, BonusSlot[i + 2].Y + BonusSlot[i + 2].Height);
                            break;
                        case Objects.BonusType.Desamorce:
                            gr.DrawImage(Properties.Resources.Deactivate, BonusSlot[i + 2]);
                            gr.DrawString((game.player2.BonusTimer[i] / 1000).ToString() + "s", new System.Drawing.Font("Arial", 10), Brushes.White, BonusSlot[i + 2].X, BonusSlot[i + 2].Y + BonusSlot[i + 2].Height);
                            break;
                        case Objects.BonusType.Armor:
                            gr.DrawImage(Properties.Resources.Armor, BonusSlot[i + 2]);
                            gr.DrawString((game.player2.BonusTimer[i] / 1000).ToString() + "s", new System.Drawing.Font("Arial", 10), Brushes.White, BonusSlot[i + 2].X, BonusSlot[i + 2].Y + BonusSlot[i + 2].Height);
                            break;
                        case Objects.BonusType.None:
                            break;
                        default:
                            break;
                    }

                }


            }
        }

        //Redraw everything each tick
        private void refreshGraphics_Tick(object sender, EventArgs e)
        {
            LoadAllMapSprites();
            Draw();
        }

        //Close properly
        private void Lobby_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (this.DialogResult == DialogResult.Cancel)
            {
                //Stop trying to receive datas
                ConnectionTimer.Stop();
                //cancel server task
                if (client != null)
                    if (this.client.GetConnectionState())
                    {
                        this.client.sendData(new Packet(Sender.Server, PacketType.CloseConnection, 1));
                        Thread.Sleep(50);
                        this.client.Disconnect();
                    }
                if (server != null)
                {
                    if (server.IsRunning && !cts.IsCancellationRequested)
                    {
                        cts.Cancel();
                        try
                        {
                            runServer.Wait();
                        }
                        catch (AggregateException ex) { }
                        finally { cts.Dispose(); }
                    }
                }
            }

            //Exited via game exit button
            if (e.CloseReason == CloseReason.UserClosing)
            { MessageBox.Show("Lobby exited."); }
            else
            {
                //Stop trying to receive datas
                ConnectionTimer.Stop();
                //cancel server task
                this.client.sendData(new Packet(Station, PacketType.CloseConnection, 1));

                Thread.Sleep(50);
                this.client.Disconnect();

                if (server != null)
                {
                    if (server.IsRunning && !cts.IsCancellationRequested)
                    {
                        this.server.SendData(new Packet(Sender.Server, PacketType.CloseConnection, 1));
                        cts.Cancel();
                        try
                        {
                            runServer.Wait();
                        }
                        catch (AggregateException ex){ }
                        finally { cts.Dispose();}
                    }
                }
            }
                   
        }

        private void Lobby_KeyDown(object sender, KeyEventArgs e)
        {          
            if (GameRunning && !game.Over)
            {
                TX_Packet = new Packet(Station, PacketType.KeyDown, e.KeyCode);
                client.sendData(TX_Packet);

                DelayKey = true;
                TimerDelayKeyDown.Start();
                
            }
           
        }

        private void Lobby_KeyUp(object sender, KeyEventArgs e)
        {
            if (GameRunning && !game.Over)
            {
                TX_Packet = new Packet(Station, PacketType.KeyUp, e.KeyCode);
                client.sendData(TX_Packet);
            }
            
        }

        private void TimerDelayKeyDown_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DelayKey = false;
            TimerDelayKeyDown.Stop();
        }

        private void Lobby_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (DelayKey)
            {
                e.IsInputKey = false;
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (game.Over)
            {
                MessageBox.Show("You can't save the game now, the game is over !");
                return;
            }
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "Bomberman savegame | *.bmb";
                dlg.AddExtension = true;
                dlg.FileName = "save.bmb";
                dlg.DefaultExt = ".bmb";

                if (dlg.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                else
                {
                    try
                    {
                        game.SaveGame(dlg.FileName);
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("An error has occured : " + ex.Message + " \n please try again");
                    }

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Bomberman savegame | *.bmb";
                dlg.AddExtension = true;
                dlg.FileName = "save.bmb";
                dlg.DefaultExt = ".bmb";

                if (dlg.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                else
                {
                    try
                    {
                        tbGameToLoad.Text = dlg.FileName;

                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("An error has occured : " + ex.Message + " \n please try again");
                    }

                }

            }
        }

        private void tlsbExit_Click(object sender, EventArgs e)
        {
            //Stop trying to receive datas
            ConnectionTimer.Stop();
            //cancel server task
            this.client.sendData(new Packet(Station, PacketType.CloseConnection, 1));
            this.client.Disconnect();

            if (server != null)
            {
                if (server.IsRunning && !cts.IsCancellationRequested)
                {

                    server.SendData(new Packet(Sender.Server, PacketType.CloseConnection, 1));
                    cts.Cancel();
                    try
                    {
                        runServer.Wait();
                    }
                    catch (AggregateException ex)
                    { }
                    finally
                    {
                        cts.Dispose();
                    }
                }

            }

            this.Close();
        
        }

        private void panelServer_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
