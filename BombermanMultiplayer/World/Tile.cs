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
using BombermanMultiplayer.Objects;

namespace BombermanMultiplayer
{
    [Serializable]
    public class Tile : GameObject
    {
        public bool Walkable = false;
        public bool Destroyable = false;
        public bool Occupied = false;
        public bool Fire = false;

        [NonSerialized]
        public Bonus BonusHere = null;
        [NonSerialized]
        public Bomb bomb = null;

        public int FireTime = 500; 

        #region Accessors


        #endregion


        public Tile(int x_, int y_, int totalFrame_, int frameWidth_, int frameHeigt_,  bool walkable, bool destroyable)
            : base(x_, y_, totalFrame_, frameWidth_, frameHeigt_)
        {
            Walkable = walkable;
            Destroyable = destroyable;
        }


        public void SpawnBonus()
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            int num = r.Next(0, 4);

            if (num == 0)
            {
                this.BonusHere = new Bonus(this.Source.X, this.Source.Y, 1, this.Source.Width, this.Source.Height, BonusType.PowerBomb);
                this.BonusHere.LoadSprite(Properties.Resources.SuperBomb);
            }
            else if (num == 1 )
            {
                this.BonusHere = new Bonus(this.Source.X, this.Source.Y, 1, this.Source.Width, this.Source.Height, BonusType.SpeedBoost);
                this.BonusHere.LoadSprite(Properties.Resources.SpeedUp);
            }
            else if (num == 2)
            {
                this.BonusHere = new Bonus(this.Source.X, this.Source.Y, 1, this.Source.Width, this.Source.Height, BonusType.Desamorce);
                this.BonusHere.LoadSprite(Properties.Resources.Deactivate);
            }

            else if (num ==  3)
            {
                this.BonusHere = new Bonus(this.Source.X, this.Source.Y, 1, this.Source.Width, this.Source.Height, BonusType.Armor);
                this.BonusHere.LoadSprite(Properties.Resources.Armor);
            }

            if (this.BonusHere != null)
            {
                this.BonusHere.CheckCasePosition(this.Source.Width, this.Source.Height);
            }
        }

        public new void Draw(Graphics gr)
        {
            if (this.Sprite != null)
            {
                gr.DrawImage(this.Sprite, Source, frameindex * Source.Width, 0, Source.Width, Source.Height, GraphicsUnit.Pixel);
                gr.DrawRectangle(Pens.Red, this.Source);
               
            }
            if (BonusHere != null)
            {
                this.BonusHere.Draw(gr);

            }
        }

    }

    

}
