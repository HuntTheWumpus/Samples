using System;

namespace wump76
{
    public class Map 
    {
        private int[][] _connections;
        private int _wumpus;
        private int _player;
        private int[] _bats;
        private int[] _pits;
        public int MAX_ROOM; 
        
        public Map()
        {
            _connections = new int[][] {
                new int[] {1, 3, 4}, 
                new int[] {0, 2, 5}, 
                new int[] {1, 3, 6},
                new int[] {0, 2, 7},
                new int[] {0, 5, 7},
                new int[] {1, 4, 6},
                new int[] {2, 5, 7},
                new int[] {3, 4, 6}
                };
            _wumpus = 6;
            _player = 0;
            _bats = new int[] {3, 4};
            _pits = new int[] {5};
            MAX_ROOM = _connections.Length-1;
        }

        public int GetPlayerLocation()
        {
            return _player;
        }

        public bool IsWumpusInRoom(int room)
        {
            return (_wumpus==room);
        }
        
        public bool IsBatInRoom(int room)
        {
            foreach (int bat in _bats)
                if (bat==room)
                    return true;
            return false;
        }

        public bool IsPitInRoom(int room)
        {
            foreach (int p in _pits)
                if (p==room)
                    return true;
            return false;
        }
        
        public bool IsPlayerNearPit()
        {
             foreach (int room in GetConnectingRooms(_player)) 
               if (IsPitInRoom(room))
                   return true;
            return false;
        }

        public bool IsConnectingRoom(int some_room)
        {
            foreach (int room in GetConnectingRooms(_player)) 
               if (some_room==room) return true;
            return false;
        }

        public bool IsPlayerNearWumpus()
        {
             foreach (int room in GetConnectingRooms(_player)) 
               if (_wumpus==room) return true;
            return false;
        }

        public bool IsPlayerNearBat()
        {
            foreach (int room in GetConnectingRooms(_player)) 
                if (IsBatInRoom(room)) return true;
            return false;
        }

        public bool MovePlayer(int room)
        {
            if (room>=0 && room<=MAX_ROOM)
            {
                _player = room;
                return true;
            }
            return false;
        }

        public bool MoveWumpus(int room)
        {
            if (room>=0 && room<=MAX_ROOM)
            {
                _wumpus = room;
                return true;
            }
            return false;
        }

        public int[] GetConnectingRooms()
        {
            return GetConnectingRooms(_player);
        }

        private int[] GetConnectingRooms(int room) 
        {
            if (room>MAX_ROOM || room<0)
                return new int[0]; // invalid room; no connections
            return _connections[room];
        }
    }

}
