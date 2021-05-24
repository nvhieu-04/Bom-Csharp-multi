using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace BombermanMultiplayer
{
    /// <summary>
    /// Game components 
    /// </summary>
    public class Game
    {
        public bool Paused = false;
        public bool Over = false;
        public byte Winner = 0;

        public World world;
        public Player player1, player2;

        public List<Bomb> BombsOnTheMap;
        public System.Timers.Timer LogicTimer;

        //ctor when picture box size is determined
        public Game(int hebergeurWidth, int hebergeurHeight)
        {
            this.world = new World(hebergeurWidth, hebergeurHeight, 48, 48, 1);

            player1 = new Player(1, 2, 33, 33, 1, 1, 48, 48, 80, 1);
            player2 = new Player(1, 2, 33, 33, this.world.MapGrid.GetLength(0) - 2, this.world.MapGrid.GetLength(0) - 2, 48, 48, 80, 2);

            this.BombsOnTheMap = new List<Bomb>();
            this.LogicTimer = new System.Timers.Timer(40);
            this.LogicTimer.Elapsed += LogicTimer_Elapsed;
        }

        //ctor when loading a game
        public Game(int hebergeurWidth, int hebergeurHeight, SaveGameData save)
        {
            this.world = new World(hebergeurWidth, hebergeurHeight, save.MapGrid);

            player1 = save.player1;
            player2 = save.player2;

            this.BombsOnTheMap = save.bombsOnTheMap;
            this.LogicTimer = new System.Timers.Timer(40);
            this.LogicTimer.Elapsed += LogicTimer_Elapsed;
        }
        //default ctor
        public Game()
        {
            this.world = new World();
            this.LogicTimer = new System.Timers.Timer(40);
            this.LogicTimer.Elapsed += LogicTimer_Elapsed;
        }

        //Use a mask to represente the placement of different objects on the map
        public Byte[,] BuildMapMask()
        {
            Byte[,] mask = new Byte[world.MapGrid.GetLength(0), world.MapGrid.GetLength(1)];

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

            for (int i = 0; i < world.MapGrid.GetLength(0); i++) //Ligne
            {
                for (int j = 0; j < world.MapGrid.GetLength(1); j++) //Colonne
                {
                    if (world.MapGrid[i, j].BonusHere != null)
                    {
                        switch (world.MapGrid[i, j].BonusHere.Type)
                        {
                            case Objects.BonusType.PowerBomb:
                                if (world.MapGrid[i, j].Fire)
                                    mask[i, j] = 30;
                                else mask[i, j] = 10;
                                break;
                            case Objects.BonusType.SpeedBoost:
                                if (world.MapGrid[i, j].Fire)
                                    mask[i, j] = 31;
                                else mask[i, j] = 11;
                                break;
                            case Objects.BonusType.Desamorce:
                                if (world.MapGrid[i, j].Fire)
                                    mask[i, j] = 32;
                                else mask[i, j] = 12;
                                break;
                            case Objects.BonusType.Armor:
                                if (world.MapGrid[i, j].Fire)
                                    mask[i, j] = 33;
                                else mask[i, j] = 13;
                                break;
                            case Objects.BonusType.None:
                                if (world.MapGrid[i, j].Fire)
                                    mask[i, j] = 3;
                                else mask[i, j] = 0;
                                break;
                        }
                    }
                    else
                    {
                        if (world.MapGrid[i, j].Walkable && !world.MapGrid[i, j].Fire)
                        {
                            mask[i, j] = 0;
                        }
                        else if (!world.MapGrid[i, j].Walkable && !world.MapGrid[i, j].Destroyable)
                        {
                            mask[i, j] = 1;
                        }
                        else if (!world.MapGrid[i, j].Walkable && world.MapGrid[i, j].Destroyable)
                        {
                            mask[i, j] = 2;
                        }
                        else if (world.MapGrid[i, j].Walkable && world.MapGrid[i, j].Fire)
                        {
                            mask[i, j] = 3;
                        }
                    }
                }
            }
            return mask;
        }

        //Manage key pushed for local game
        public void Game_KeyDown(Keys key)
        {
            switch (key)
            {
                case Keys.Z:
                    if (player1.Dead)
                        break;
                    player1.Orientation = Player.MovementDirection.UP;
                    player1.LoadSprite(Properties.Resources.AT_UP);
                    break;
                case Keys.S:
                    if (player1.Dead)
                        break;
                    player1.Orientation = Player.MovementDirection.DOWN;
                    player1.LoadSprite(Properties.Resources.AT_DOWN);
                    break;
                case Keys.Q:
                    if (player1.Dead)
                        break;
                    player1.Orientation = Player.MovementDirection.LEFT;
                    player1.LoadSprite(Properties.Resources.AT_LEFT);
                    break;
                case Keys.D:
                    if (player1.Dead)
                        break;
                    player1.Orientation = Player.MovementDirection.RIGHT;
                    player1.LoadSprite(Properties.Resources.AT_RIGHT);
                    break;
                case Keys.Space:
                    if (player1.Dead)
                        break;
                    player1.DropBomb(this.world.MapGrid, this.BombsOnTheMap, player2);
                    break;
                case Keys.A:
                    if (player1.Dead)
                        break;
                    player1.Deactivate(this.world.MapGrid, BombsOnTheMap, player2);
                    break;
                case Keys.Up:
                    if (player2.Dead)
                        break;
                    player2.Orientation = Player.MovementDirection.UP;
                    player2.LoadSprite(Properties.Resources.T_UP);
                    break;
                case Keys.Down:
                    if (player2.Dead)
                        break;
                    player2.Orientation = Player.MovementDirection.DOWN;
                    player2.LoadSprite(Properties.Resources.T_DOWN);
                    break;
                case Keys.Left:
                    if (player2.Dead)
                        break;
                    player2.Orientation = Player.MovementDirection.LEFT;
                    player2.LoadSprite(Properties.Resources.T_LEFT);
                    break;
                case Keys.Right:
                    if (player2.Dead)
                        break;
                    player2.Orientation = Player.MovementDirection.RIGHT;
                    player2.LoadSprite(Properties.Resources.T_RIGHT);
                    break;
                case Keys.ControlKey:
                    if (player2.Dead)
                        break;
                    player2.DropBomb(this.world.MapGrid, this.BombsOnTheMap, player1);
                    break;
                case Keys.Shift:
                    if (player2.Dead)
                        break;
                    player2.Deactivate(this.world.MapGrid, BombsOnTheMap, player1);
                    break;
                case Keys.Escape:
                    Pause();
                    break;
            }
        }

        //Manage key push for server side
        public void Game_KeyDownWithoutSprite(Keys key, Sender Station)
        {
            Player sender = null;
            Player otherPlayer = null;

            switch (Station)
            {
                case Sender.Player1:
                    sender = player1;
                    otherPlayer = player2;
                    if (sender.Dead)
                        return;
                    break;
                case Sender.Player2:
                    sender = player2;
                    otherPlayer = player1;
                    if (sender.Dead)
                        return;
                    break;
                default:
                    break;
            }

            switch (key)
            {
                case Keys.Up:
                    sender.Orientation = Player.MovementDirection.UP;
                    break;
                case Keys.Down:
                    sender.Orientation = Player.MovementDirection.DOWN;
                    break;
                case Keys.Left:
                    sender.Orientation = Player.MovementDirection.LEFT;
                    break;
                case Keys.Right:
                    sender.Orientation = Player.MovementDirection.RIGHT;
                    break;
                case Keys.Space:
                    sender.DropBomb(this.world.MapGrid, this.BombsOnTheMap, otherPlayer);
                    break;
                case Keys.ControlKey:
                    sender.Deactivate(this.world.MapGrid, BombsOnTheMap, otherPlayer);
                    break;
                case Keys.Escape:
                    Pause();
                    break;
            }
        }

        //Manage release of key for server side
        public void Game_KeyUpWithoutSprite(Keys key, Sender Station)
        {
            Player sender = null;

            switch (Station)
            {
                case Sender.Player1:
                    sender = player1;
                    break;
                case Sender.Player2:
                    sender = player2;
                    break;
                default:
                    break;
            }
            switch (key)
            {
                case Keys.Up:
                    sender.Orientation = Player.MovementDirection.NONE;
                    break;
                case Keys.Down:
                    sender.Orientation = Player.MovementDirection.NONE;
                    break;
                case Keys.Left:
                    sender.Orientation = Player.MovementDirection.NONE;
                    break;
                case Keys.Right:
                    sender.Orientation = Player.MovementDirection.NONE;
                    break;
            }
        }

        //Manage the release of the keys
        public void Game_KeyUp(Keys key)
        {
            switch (key)
            {
                case Keys.Z:
                    player1.Orientation = Player.MovementDirection.NONE;
                    break;
                case Keys.S:
                    player1.Orientation = Player.MovementDirection.NONE;
                    break;
                case Keys.Q:
                    player1.Orientation = Player.MovementDirection.NONE;
                    break;
                case Keys.D:
                    player1.Orientation = Player.MovementDirection.NONE;
                    break;
                case Keys.Up:
                    player2.Orientation = Player.MovementDirection.NONE;
                    break;
                case Keys.Down:
                    player2.Orientation = Player.MovementDirection.NONE;
                    break;
                case Keys.Left:
                    player2.Orientation = Player.MovementDirection.NONE;
                    break;
                case Keys.Right:
                    player2.Orientation = Player.MovementDirection.NONE;
                    break;
            }
        }

        private void GameOver()
        {
            if (player1.Dead || player2.Dead)
            {
                this.Over = true;
                this.Paused = true;
                if (player1.Dead && player2.Dead)
                    Winner = 0;
                else if (player2.Dead)
                    Winner = 2;
                else if (player1.Dead)
                    Winner = 1;
            }
        }

        //Manage interactions between worlds and objects
        private void InteractionLogic()
        {
            for (int i = 0; i < world.MapGrid.GetLength(0); i++) //Ligne
            {
                for (int j = 0; j < world.MapGrid.GetLength(1); j++) //Colonne
                {

                    if (world.MapGrid[i, j].Fire)
                    {
                        if (player1.CasePosition[0] == i && player1.CasePosition[1] == j
                            && player1.BonusSlot[0] != Objects.BonusType.Armor && player1.BonusSlot[1] != Objects.BonusType.Armor)
                        {
                            player1.Dead = true;
                            player1.LoadSprite(Properties.Resources.Blood);
                        }
                        if (player2.CasePosition[0] == i && player2.CasePosition[1] == j
                            && player2.BonusSlot[0] != Objects.BonusType.Armor && player2.BonusSlot[1] != Objects.BonusType.Armor)
                        {
                            player2.Dead = true;
                            player2.LoadSprite(Properties.Resources.Blood);
                        }
                        if (world.MapGrid[i, j].FireTime <= 0)
                        {
                            world.MapGrid[i, j].Fire = false;
                            world.MapGrid[i, j].FireTime = 500;
                        }
                        else
                        {
                            world.MapGrid[i, j].FireTime -= (int)LogicTimer.Interval;
                        }
                    }
                }
            }
        }
        private void BombsLogic()
        {
            List<int> ToRemove = new List<int>();

            //Check for bomb explosion
            for (int i = 0; i < BombsOnTheMap.Count; i++)
            {
                BombsOnTheMap[i].UpdateFrame((int)LogicTimer.Interval);
                BombsOnTheMap[i].TimingExplosion((int)LogicTimer.Interval);
                if (BombsOnTheMap[i].Explosing == true)
                {
                    BombsOnTheMap[i].Explosion(this.world.MapGrid, player1, player2);
                    ToRemove.Add(i);
                }
            }

            //Remove exploded bombs
            for (int i = 0; i < ToRemove.Count; i++)
            {
                try
                {
                    BombsOnTheMap.RemoveAt(ToRemove[i]);
                }
                catch (Exception){}
            }
        }

        private void BonusLogic(Player player)
        {
            int freeSlot = -1;
            for (int i = 0; i < player.BonusSlot.Length; i++)
            {
                if (player.BonusSlot[i] != Objects.BonusType.None)
                {
                    if (player.BonusTimer[i] <= 0)
                    {
                        if (player.BonusSlot[i] == Objects.BonusType.SpeedBoost)
                        {
                            player.Vitesse /= 2;
                        }

                        player.BonusSlot[i] = Objects.BonusType.None;
                    }
                    else
                    {
                        player.BonusTimer[i] -= (short)LogicTimer.Interval;
                    }
                }
                else
                {
                    freeSlot = i;
                }
            }

            if (this.world.MapGrid[player.CasePosition[0], player.CasePosition[1]].BonusHere != null)
            {
                //If Player already have the bonus
                if (player.BonusSlot[0] == this.world.MapGrid[player.CasePosition[0], player.CasePosition[1]].BonusHere.Type ||
                    player.BonusSlot[1] == this.world.MapGrid[player.CasePosition[0], player.CasePosition[1]].BonusHere.Type)
                    return;

                if (freeSlot != -1)
                {
                    switch (this.world.MapGrid[player.CasePosition[0], player.CasePosition[1]].BonusHere.Type)
                    {
                        case Objects.BonusType.PowerBomb:
                            player.BonusSlot[freeSlot] = Objects.BonusType.PowerBomb;
                            player.BonusTimer[freeSlot] = 5000;
                            break;
                        case Objects.BonusType.SpeedBoost:
                            player.BonusSlot[freeSlot] = Objects.BonusType.SpeedBoost;
                            player.Vitesse *= 2;
                            player.BonusTimer[freeSlot] = 5000;
                            break;
                        case Objects.BonusType.Desamorce:
                            player.BonusSlot[freeSlot] = Objects.BonusType.Desamorce;
                            player.BonusTimer[freeSlot] = 10000;
                            break;
                        case Objects.BonusType.Armor:
                            player.BonusSlot[freeSlot] = Objects.BonusType.Armor;
                            player.BonusTimer[freeSlot] = 5000;
                            break;
                        default:
                            break;
                    }
                    this.world.MapGrid[player.CasePosition[0], player.CasePosition[1]].BonusHere = null;
                }
            }
        }

        private void PlayersLogic()
        {
            player1.LocationCheck(48, 48);
            player2.LocationCheck(48, 48);

            BonusLogic(player1);
            BonusLogic(player2);

            if (player1.Orientation != Player.MovementDirection.NONE)
            {
                if (CheckCollisionPlayer(player1, player2, world.MapGrid, player1.Orientation))
                {
                    player1.Move();

                }
                player1.UpdateFrame((int)LogicTimer.Interval);
            }
            else
                player1.frameindex = 1;

            if (player2.Orientation != Player.MovementDirection.NONE)
            {
                if (CheckCollisionPlayer(player2, player1, world.MapGrid, player2.Orientation))
                {
                    player2.Move();
                }
                player2.UpdateFrame((int)LogicTimer.Interval);
            }
            else
                player2.frameindex = 1;
        }
        //Collision management

        //Check collision between two rectangles
        public bool CheckCollisionRectangle(Rectangle Object1, Rectangle Object2)
        {

            if ((Object2.X >= Object1.X + Object1.Width)      // trop à droite
                || (Object2.X + Object2.Width <= Object1.X) // trop à gauche
                || (Object2.Y >= Object1.Y + Object1.Height) // trop en bas
                || (Object2.Y + Object2.Height <= Object1.Y))  // trop en haut
                return false;
            else
                return true;
            //True if there's a collision

        }
        public bool CheckCollisionPlayer(Player movingPlayer, Player player2, Tile[,] map, Player.MovementDirection direction)
        {
            int lig = movingPlayer.CasePosition[0];
            int col = movingPlayer.CasePosition[1];

            //Check for environnement collision (map)
            switch (direction)
            {
                case Player.MovementDirection.UP:
                    {
                        //UP
                        //Temporary version of player collision box with expected position after deplacement
                        Rectangle rect = new Rectangle(movingPlayer.Source.X, movingPlayer.Source.Y - movingPlayer.Vitesse, movingPlayer.Source.Width, movingPlayer.Source.Height);

                        if (!map[lig - 1, col - 1].Walkable || map[lig - 1, col - 1].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig - 1, col - 1].Source))
                                return false;
                        }
                        if (!map[lig - 1, col].Walkable || map[lig - 1, col].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig - 1, col].Source))
                                return false;
                        }
                        if (!map[lig - 1, col + 1].Walkable || map[lig - 1, col + 1].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig - 1, col + 1].Source))
                                return false;
                        }
                        if (CheckCollisionRectangle(rect, player2.Source))
                            return false;
                    }
                    break;
                case Player.MovementDirection.DOWN:
                    {
                        //DOWN
                        Rectangle rect = new Rectangle(movingPlayer.Source.X, movingPlayer.Source.Y + movingPlayer.Vitesse, movingPlayer.Source.Width, movingPlayer.Source.Height);

                        if (!map[lig + 1, col - 1].Walkable || map[lig + 1, col - 1].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig + 1, col - 1].Source))
                                return false;
                        }
                        if (!map[lig + 1, col].Walkable || map[lig + 1, col].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig + 1, col].Source))
                                return false;
                        }
                        if (!map[lig + 1, col + 1].Walkable || map[lig + 1, col + 1].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig + 1, col + 1].Source))
                                return false;
                        }
                        if (CheckCollisionRectangle(rect, player2.Source))
                            return false;
                    }
                    break;
                case Player.MovementDirection.LEFT:
                    {
                        //LEFT
                        Rectangle rect = new Rectangle(movingPlayer.Source.X - movingPlayer.Vitesse, movingPlayer.Source.Y, movingPlayer.Source.Width, movingPlayer.Source.Height);
                        if (!map[lig - 1, col - 1].Walkable || map[lig - 1, col - 1].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig - 1, col - 1].Source))
                                return false;
                        }
                        if (!map[lig, col - 1].Walkable || map[lig, col - 1].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig, col - 1].Source))
                                return false;
                        }
                        if (!map[lig + 1, col - 1].Walkable || map[lig + 1, col - 1].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig + 1, col - 1].Source))
                                return false;
                        }
                        if (CheckCollisionRectangle(rect, player2.Source))
                            return false;
                    }
                    break;
                case Player.MovementDirection.RIGHT:
                    {
                        Rectangle rect = new Rectangle(movingPlayer.Source.X + movingPlayer.Vitesse, movingPlayer.Source.Y, movingPlayer.Source.Width, movingPlayer.Source.Height);
                        //RIGHT
                        if (!map[lig - 1, col + 1].Walkable || map[lig - 1, col + 1].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig - 1, col + 1].Source))
                                return false;
                        }
                        if (!map[lig, col + 1].Walkable || map[lig, col + 1].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig, col + 1].Source))
                                return false;
                        }
                        if (!map[lig + 1, col + 1].Walkable || map[lig + 1, col + 1].Occupied)
                        {
                            if (CheckCollisionRectangle(rect, map[lig + 1, col + 1].Source))
                                return false;
                        }
                        if (CheckCollisionRectangle(rect, player2.Source))
                            return false;
                    }
                    break;
                default:
                    break;
            }
            return true;
        }
        private void LogicTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            InteractionLogic();
            PlayersLogic();
            BombsLogic();
            GameOver();
        }

        public void SaveGame(string fileName)
        {
            System.Runtime.Serialization.IFormatter formatter = new BinaryFormatter();
            System.IO.FileStream filestream = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
            try
            {
                formatter.Serialize(filestream, new SaveGameData(BombsOnTheMap, world.MapGrid, player1, player2));
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occured : " + ex.Message);
                return;
            }
            MessageBox.Show("File " + fileName + " saved successfuly !");
        }

        public void LoadGame(string fileName)
        {
            SaveGameData save;

            System.Runtime.Serialization.IFormatter formatter = new BinaryFormatter();
            System.IO.FileStream filestream = new System.IO.FileStream(fileName, System.IO.FileMode.Open);
            try
            {
                save = (SaveGameData)formatter.Deserialize(filestream);

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error has occured : " + ex.Message);
                return;
            }
            this.BombsOnTheMap = save.bombsOnTheMap;
            this.world.MapGrid = save.MapGrid;
            this.player1 = save.player1;
            this.player2 = save.player2;

            this.Paused = true;
            this.LogicTimer.Stop();

            if (this.Over)
            {
                this.Over = false;
                this.Winner = 0;
            }
        }
        
        public void Pause()
        {
            //If the game is already over, no need for pause
            if (!Over)
            {
                if (Paused)
                {
                    LogicTimer.Start();
                    Paused = false;
                }
                else
                {
                    LogicTimer.Stop();
                    Paused = true;
                }
            }
        }
    }
}