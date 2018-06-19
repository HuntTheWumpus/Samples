using System;

namespace wump76
{
    public class Map 
    {
        private int[][] _connections;
        private int _wumpus;
        private int _player;
        private int _bats;
        private int _pits;
        
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
            _bats = 3;
            _pits = 5;
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
            return (_bats==room);
        }

        public bool IsPitInRoom(int room)
        {
            return (_pits==room);
        }
        
        public bool IsPlayerNearPit()
        {
             foreach (int room in GetConnectingRooms(_player)) 
               if (_pits==room) return true;
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
                if (_bats==room) return true;
            return false;
        }

        public bool MovePlayer(int room)
        {
            if (room>=0 && room<_connections.Length)
            {
                _player = room;
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
            if (room>=_connections.Length)
                return new int[0]; // invalid room; no connections
            return _connections[room];
        }
    }

}
