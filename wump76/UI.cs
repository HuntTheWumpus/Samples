using System;

namespace wump76
   {
    public class ConsoleUI
    {
        public ConsoleUI()
        {
            Console.WriteLine();
            Console.Write("Instructions (Y-N)? " );
            if (GetInput()=="y") {
                ShowInstructions();
                Console.WriteLine("Press any key to start game");
                Console.Read();
            }
            _gc = new GameControl();
        }

        private void ShowPlayerLocation()
        {
            Console.WriteLine();
            Console.WriteLine("You are in room "+_gc.map.GetPlayerLocation());
            
            string connection_string =  "Tunnels lead to ";
            int i=0;
            foreach (int room in _gc.map.GetConnectingRooms())
            {
                if (i++!=0)
                    connection_string += ", ";
                connection_string += room.ToString();    
            }
            Console.WriteLine(connection_string);
        }

        private void ShowWarnings()
        {
            if (_gc.map.IsPlayerNearBat())
                Console.WriteLine("Bats Nearby!");
            if (_gc.map.IsPlayerNearPit())
                Console.WriteLine("I feel a draft!");
            if (_gc.map.IsPlayerNearWumpus())
                Console.WriteLine("I smell a Wumpus!");
        }
        
        public void Start() 
        {
            Console.WriteLine();
            Console.WriteLine("HUNT THE WUMPUS");

            ShowPlayerLocation();
            ShowWarnings();

            Console.WriteLine();
            Console.Write("Shoot or Move (S-M)? ");  
            while (1==1) {
                string answer=GetInput();
                if (answer=="quit")  
                    break;
                else if (answer=="m")
                {
                    Console.Write("Where to? ");
                    int loc=Convert.ToInt32(GetInput());
                    while (!_gc.MovePlayer(loc))
                    {
                        Console.WriteLine("Cant move to "+loc+", try again");
                        Console.Write("Where to? ");
                        loc=Convert.ToInt32(GetInput());
                    }
                }
                else 
                {
                    Console.WriteLine("Can't do that, try again");
                }
                ShowPlayerLocation(); 
                ShowWarnings();
                Console.WriteLine();
                Console.Write("Shoot or Move (S-M)? ");  
            }
        }

        private String GetInput()
        {
            try {
                string answer=Console.ReadLine().ToLower();
                //Console.WriteLine("DEBUG: answer="+answer);
                return answer;
                
            } catch (Exception) {}
            return "";
            
        }

        private void ShowInstructions()
        {
            Console.WriteLine();
            Console.WriteLine (@"
The Wumpus lives in a cave of 20 rooms. Each room has 3 tunnels leading to other rooms. (Look at a Dodecahedron to see how this works - If you don't know what a dodecahedron is, ask someone)

  HAZARDS:
    BOTTOMLESS PITS - Two rooms have bottomless pits in them. If you go there, you fall into the pit (& lose!)
    SUPER BATS - Two other rooms have super bats. If you go there, a bat grabs you and takes you to some other room at random. (Which might be troublesome)

  WUMPUS: 
  The Wumpus is not bothered by the hazards (he has sucker feet and is too big for a bat to lift). 
  Usually he is asleep. Two things wake him up: Your entering his room or your shooting an arrow.
  If the wumpus wakes, he moves (P=0.75) one room or stays still (P=0.25). After that, if he is where you are, he eats you up (& you lose!)

  YOU:
  Each turn you may move or shoot a crooked arrow.
    MOVING: You can go one room (thru one tunnel)
    ARROWS: You have 5 arrows. You lose when you run out. 
    Each arrow can go from 1 to 5 rooms. You aim by telling the computer the room #5 you want the arrow to go to. If the arrow can't go that way (i.e., no tunnel) it moves at random to the next room.
    If the arrow hits the wumpus, you win. 
    If the arrow hits you, you lose.

  WARNINGS:
  When you are one room away from Wumpus or hazard, the computer says:
    WUMPUS - 'I smell a wumpus'
    BAT    - 'Bats nearby'
    PIT    - 'I feel a draft'

  QUIT:
  Responding QUIT for any question will end the game immediately. 
            ");
        }
        private GameControl _gc; // game controler
    }
}