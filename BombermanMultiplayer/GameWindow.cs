using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BombermanMultiplayer
{
    public partial class GameWindow : Form
    {
        Game game;

        private BufferedGraphics bufferG = null;
        private Graphics gr;
        private Rectangle[] BonusSlot;
        public GameWindow()
        {
            InitializeComponent();
            this.pbGame.ClientSize = new Size(528, 528);

            game = new Game(this.pbGame.Width, this.pbGame.Height);

            BonusSlot = new Rectangle[4];

            for (int i = 0; i < 4; i++)
            {
                BonusSlot[i] = new Rectangle(i * pbGame.Width / 18, pbGame.Height / 25, pbGame.Width / 20, pbGame.Height / 20);
                if (i  > 1 )
                {
                    BonusSlot[i] = new Rectangle(3 * pbGame.Width / 4 + i * pbGame.Width / 18, pbGame.Height / 25, pbGame.Width / 20, pbGame.Height / 20);
                }
            }

            game.world.loadBackground(Properties.Resources.World);
            game.world.loadSpriteTile(Properties.Resources.BlockDestructible, Properties.Resources.BlockNonDestructible);
            game.player1.LoadSprite(Properties.Resources.AT_DOWN);
            game.player2.LoadSprite(Properties.Resources.T_UP);

            bufferG = BufferedGraphicsManager.Current.Allocate(pbGame.CreateGraphics(), pbGame.DisplayRectangle);
            gr = bufferG.Graphics;

            this.game.LogicTimer.Start();



            this.refreshGraphics.Start();
        }


        public void Draw()
        {
            gr.Clear(pbGame.BackColor);
            game.world.refreshTileSprites();

            game.world.Draw(gr);

            game.player1.Draw(gr);
            game.player1.DrawPosition(gr);

            game.player2.Draw(gr);
            game.player2.DrawPosition(gr);

            try
            {

                foreach (Bomb bomb in game.BombsOnTheMap)
                {

                bomb.Draw(gr);

                }

            }
            catch (Exception)
            {}


            DrawInterface();




            bufferG.Render();

        }

        public void DrawInterface()
        {

            if (game.Paused && !game.Over)
            {
                tlsMenu.Visible = true;
                gr.DrawString("PAUSED", new System.Drawing.Font("Arial", 30), Brushes.White, pbGame.Width / 2, pbGame.Height / 2);

            }
            else if (!game.Paused && !game.Over)
            {
                tlsMenu.Visible = false;
            }
            else
            {
                tlsMenu.Visible = true;
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


        private void Game_KeyDown(object sender, KeyEventArgs e)
        {
            game.Game_KeyDown(e.KeyCode);

           
        }

        

        private void Game_KeyUp(object sender, KeyEventArgs e)
        {
            game.Game_KeyUp(e.KeyCode);
        }


        private void Game_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
        }

        private void refreshGraphics_Tick(object sender, EventArgs e)
        {

                Draw();

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

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
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
                        game.LoadGame(dlg.FileName);
                        game.world.loadBackground(Properties.Resources.World);
                        game.world.loadSpriteTile(Properties.Resources.BlockDestructible, Properties.Resources.BlockNonDestructible);
                        game.player1.LoadSprite(Properties.Resources.AT_DOWN);
                        game.player2.LoadSprite(Properties.Resources.T_UP);
                        Draw();

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
            this.Close();
        }

        private void pbGame_Click(object sender, EventArgs e)
        {

        }

        private void GameWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
