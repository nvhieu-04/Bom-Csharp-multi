using BombermanMultiplayer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer
{
    [Serializable]
    public class GameState
    {

        //If the game is paused
        public bool Paused = false;

        //Player
        public short[] XY_Position_Player1 = new short[2];
        public short[] XY_Position_Player2 = new short[2];

        public short framePlayer1;
        public short framePlayer2;

        public Player.MovementDirection orientationPlayer1;
        public Player.MovementDirection orientationPlayer2;

        public short NbBomb_Player1;
        public short NbBomb_Player2;

        public bool deadPlayer1;
        public bool deadPlayer2;

        public BonusType[] BonusSlotPlayer1;
        public BonusType[] BonusSlotPlayer2;

        public short[] BonusTimerPlayer1;
        public short[] BonusTimerPlayer2;

        public string NamePlayer1;
        public string NamePlayer2;

        public byte Winner = 0;
        public bool Over;

        //Bomb list
        public List<Bomb> bombsList;

        public Byte[,] map;

    }
}
