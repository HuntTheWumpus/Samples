using System;

namespace wump76
{
    public class GameControl
    {
        public Map map; // public member map
        private Random rand; //random nubmer generator used for game interactions

        public GameControl()
        {
            map = new Map();
            rand = new Random();
        }

        public bool MovePlayer(int room)
        {
            foreach (int connecting_room in map.GetConnectingRooms())
                if (room==connecting_room) 
                    return map.MovePlayer(room);
            return false;
        }

        private bool MoveWumpus()
        {
            int[] connecting_rooms = map.GetConnectingRooms();
            int rand_idx = rand.Next(connecting_rooms.Length-1);
            // Console.WriteLine("DEBUG: Random index "+rand_idx.ToString()+"; Wumpus moved to "+connecting_rooms[rand_idx].ToString());
            return map.MoveWumpus(connecting_rooms[rand_idx]);
        }


        public bool InteractWithBats(int room)
        {
            if (map.IsBatInRoom(room))
            {
                int new_room = rand.Next(map.MAX_ROOM);
                while (new_room==room)
                    new_room = rand.Next(map.MAX_ROOM); // handle unlikely event of random seleciton = current room one or more times! 
                map.MovePlayer(new_room);
                return true;
            }
            return false;
        }

        public bool DoesWumpusMove()
        {
            int wumpus_action = rand.Next(3); 
            // action will=0 approx. with probablity of 25% (1 out of 4 equally probably values 0-3
            // use this to decide wumpus's action  
            // Console.WriteLine("DEBUG: action="+wumpus_action.ToString());
            if (wumpus_action==0)
            {
                MoveWumpus();
                return true; // wumpus woke, but was startled and moved to a different room  
            }
            return false; // wumpus woke and stayed in the room
        }

        public bool DoesRoomHavePit(int room)
        {
            return map.IsPitInRoom(room);
        }
    }
}