using System;

namespace wump76
{
    public class GameControl
    {
        public Map map; // public member map

        public GameControl()
        {
            map = new Map();
        }


        private bool IsValidMove(int room)
        {
             foreach (int connecting_room in map.GetConnectingRooms())
                if (room==connecting_room) 
                    return true;
            return false;
        }

        public bool MovePlayer(int room)
        {
            if (IsValidMove(room))
                return map.MovePlayer(room);
            return false;
        }
    }
}