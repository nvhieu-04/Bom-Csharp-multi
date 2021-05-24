using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Objects
{
    public class Bonus : GameObject
    {

        public BonusType Type = BonusType.None;


        public Bonus(int x, int y, int frameNumber, int frameWidth, int frameHeight, BonusType type ) 
            : base(x, y, frameNumber, frameWidth, frameHeight)
        {
            this.Type = type;

        }

        public void CheckCasePosition(int TileWidth, int TileHeight)
        {
            this.CasePosition[0] = this.Source.Y / TileWidth; //Ligne
            this.CasePosition[1] = this.Source.X / TileWidth; //Colonne
        }

    }

    public enum BonusType
    {
        None,
        PowerBomb,
        SpeedBoost,
        Desamorce,
        Armor

    }
}
