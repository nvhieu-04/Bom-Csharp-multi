using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Media;
using System.Diagnostics;

namespace BombermanMultiplayer
{
    [Serializable]
    public class World 
    {
        public Tile[,] MapGrid;
        [NonSerialized]
        private Image Background_;


        public Image Background
        {
            get
            {
                return Background_;
            }

            set
            {
                Background_ = value;
            }
        }


        public void Draw(Graphics gr)
        {
            if (Background != null)
             {
                gr.DrawImage(Background, gr.VisibleClipBounds.X, gr.VisibleClipBounds.Y, gr.VisibleClipBounds.Width, gr.VisibleClipBounds.Height);
             }

            for (int i = 0; i < MapGrid.GetLength(0); i++) //Ligne
            {
                for (int j = 0; j < MapGrid.GetLength(1); j++) //Colonne
                {
                    MapGrid[i, j].Draw(gr);
                }
            }

        }

        public void loadBackground(Image background)
        {
            this.Background = background;
        }
        
        public void loadSpriteTile(Image spriteDestroyableTile, Image spriteUndestroyableTile)
        {
            for (int i = 0; i < MapGrid.GetLength(0); i++) //Ligne
            {
                for (int j = 0; j < MapGrid.GetLength(1); j++) //Colonne
                {
                    if (MapGrid[i, j].Destroyable)
                    {
                        MapGrid[i, j].LoadSprite(spriteDestroyableTile);
                    }
                    else if (!MapGrid[i, j].Walkable && !MapGrid[i, j].Destroyable)
                    {
                        MapGrid[i, j].LoadSprite(spriteUndestroyableTile);
                    }
                }
            }

        }

        public void refreshTileSprites()
        {
            for (int i = 0; i < MapGrid.GetLength(0); i++) //Ligne
            {
                for (int j = 0; j < MapGrid.GetLength(1); j++) //Colonne
                {
                    if (MapGrid[i, j].Walkable && !MapGrid[i, j].Destroyable)
                    {
                        MapGrid[i, j].UnloadSprite();
                    }
                    if (MapGrid[i, j].Fire)
                    {
                        MapGrid[i, j].LoadSprite(Properties.Resources.Fire);
                    }
                    else if (MapGrid[i, j].Walkable && !MapGrid[i, j].Fire)
                    {
                        MapGrid[i, j].UnloadSprite();
                    }

                }
            }

        }


        public World(int hebergeurWidth, int hebergeurHeight, int TILE_WIDTH, int TILE_HEIGHT, int totalFrameTile)
        {
            CreateWorldGrid(hebergeurWidth, hebergeurHeight, TILE_WIDTH, TILE_HEIGHT, totalFrameTile);
        }

        public World(int hebergeurWidth, int hebergeurHeight, Tile[,] map)
        {
            this.MapGrid = map;
        }

        public World()
        {

        }

        public void CreateWorldGrid(int hebergeurWidth, int hebergeurHeight, int TILE_WIDTH, int TILE_HEIGHT, int totalFrameTile)
        {
            Random r = new Random();
            int rand = 0;
            //Génération grille du terrain
            MapGrid = new Tile[hebergeurWidth / TILE_WIDTH, hebergeurHeight / TILE_HEIGHT];

            for (int i = 0; i < MapGrid.GetLength(0); i++) //Ligne
            {
                for (int j = 0; j < MapGrid.GetLength(1); j++) //Colonne
                {

                    rand = r.Next(0,10);
                       
                    if (j == 0 || j == (MapGrid.GetLength(0) - 1) || i == 0 || i == (MapGrid.GetLength(1) - 1))
                        MapGrid[i, j] = new Tile(j * TILE_WIDTH, i * TILE_HEIGHT, totalFrameTile, TILE_WIDTH, TILE_HEIGHT, false, false);
                    else
                    {
                        if (i % 2 == 0 && j % 2 == 0)
                            MapGrid[i, j] = new Tile( j * TILE_WIDTH, i * TILE_HEIGHT, totalFrameTile, TILE_WIDTH, TILE_HEIGHT, false, false);
                        else
                        {
                            if (((i == 1 && (j == 1 || j == 2)) || (i == 2 && j == 1)
                                || (i == (MapGrid.GetLength(0) - 1) - 2 && j == (MapGrid.GetLength(0) - 1) - 1) || (i == (MapGrid.GetLength(0) - 1) - 1 && (j == (MapGrid.GetLength(0) - 1) - 1 || j == (MapGrid.GetLength(0) - 1) - 2)))) // les cases adjacentes au point de spawn du joueurs sont exemptes de blocks destructibles
                                MapGrid[i, j] = new Tile( j * TILE_WIDTH, i * TILE_HEIGHT, totalFrameTile, TILE_WIDTH, TILE_HEIGHT, true, false);
                            else if (rand >= 1) // Si le random est pair la case est remplie par un bloc destructible
                                MapGrid[i, j] = new Tile(j * TILE_WIDTH, i * TILE_HEIGHT, totalFrameTile, TILE_WIDTH, TILE_HEIGHT, false, true);
                            else // Le reste des cases est libre 
                                MapGrid[i, j] = new Tile(j * TILE_WIDTH, i * TILE_HEIGHT, totalFrameTile, TILE_WIDTH, TILE_HEIGHT, true, false);

                        }
                            

                    }
                    

                }
            }
        }


        



    }
}
