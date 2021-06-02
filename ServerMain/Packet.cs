using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer
{
    [Serializable]
    public class Packet
    {
        Sender proprietary;
        PacketType type;
        object payload;

        public Packet()
        {
        }

        public Packet(Sender proprietary_, PacketType type_, object payload_)
        {
            this.type = type_;
            this.payload = payload_;
            this.proprietary = proprietary_;
        }


        public T GetPayload<T>()
        {
            return (T)this.payload;
        }

        public PacketType GetPacketType()
        { return this.type; }

        public Sender GetSender()
        { return this.proprietary; }

        public bool Empty()
        {
            return payload == null;
        }

    }


    public enum PacketType
    {

        Connection,
        MapTransfer,
        Ready,
        KeyDown,
        KeyUp,
        GameState,
        Pause,
        LoadGame,
        CloseConnection
    }
    public enum Sender
    {
        Server,
        Player1,
        Player2

    }

}
