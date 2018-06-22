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

        private void NewTurn()
        {
            Console.WriteLine();
        }

        private void ShowPlayerLocation()
        {
            int loc = _gc.map.GetPlayerLocation();
            Console.WriteLine("You are in room "+loc);
        }
        private void ShowBatMoveLocation()
        {
            int loc = _gc.map.GetPlayerLocation();
            Console.WriteLine("Bats moved you to room "+loc);
        }

        private GameState HandleRoomInteraction()
        {
            int loc = _gc.map.GetPlayerLocation();
            if (_gc.InteractWithBats(loc))
            {
                Console.WriteLine("ZAP! Suffer Bat Snatch! Elsewhereville for you!");
                ShowBatMoveLocation();
                // call recursively to handle involuntary move to new room  
                // exit immediately after as we don't need to handle any other hazards in room player was just moved out of
                return HandleRoomInteraction(); 
            }
            if (_gc.DoesRoomHavePit(loc))
            {
                Console.WriteLine("YYYIIIIEEEE... You fell in a pit!");
                return GameState.Loose;
            }
            if (_gc.map.IsWumpusInRoom(loc))
            {
                Console.WriteLine("You entered room with the Wunpus!");
                if(_gc.DoesWumpusMove())
                {
                    Console.WriteLine("LUCKY! Wumpus was startled and moved to nearby room!");
                    return GameState.Continue;
                }
                else
                {
                    Console.WriteLine("Your are Wumpus food!");
                    return GameState.Loose;
                } 
            }
            return GameState.Continue;
        }

        private void ShowConnectingRooms()
        {
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
        
        private string GetUserAction()
        {
            ShowConnectingRooms();
            ShowWarnings();
            Console.Write("Shoot (S), Move (M) or Quit (Q)? ");
            return GetInput();
        }

        private void HandleMoveAction()
        {
            Console.Write("Where to? ");
            int loc=Convert.ToInt32(GetInput());
            while (_gc.MovePlayer(loc)==ActionResult.Invalid)
            {
                Console.WriteLine("Cant move to "+loc+", try again");
                Console.Write("Where to? ");
                loc=Convert.ToInt32(GetInput());
            }
        }

        private GameState HandleShootAction()
        {
            Console.Write("Aim Where? ");
            int loc=Convert.ToInt32(GetInput());
            ActionResult result = _gc.ShootAction(loc);
            if (result==ActionResult.Invalid)
            {
                Console.WriteLine("You can't shoot into room "+loc+", try again");
                return GameState.Continue;
            }
            else if (result==ActionResult.KilledWumpus)
            {
                Console.WriteLine("Aargh... you shot the wumpus!");
                return GameState.Win;
            }
            else if (result==ActionResult.NoArrowsLeft) 
            {
                Console.WriteLine("Ha, Ha - you've run out of arrows, Wumpus will get you!");
                return GameState.Loose;
            }
            else //ActionResult.Done
            {
                Console.WriteLine("You missed! Wumpus is not in room "+loc);
                Console.WriteLine("You have "+_gc.GetArrowsLeft()+" arrows left");
            }
            return GameState.Continue;
        }

        public void Start() 
        {
            Console.WriteLine();
            Console.WriteLine("HUNT THE WUMPUS");

            while (1==1) {
                NewTurn();
                ShowPlayerLocation();
                
                string answer;
                GameState state = GameState.Continue; 
                answer=GetUserAction();
                if (answer=="q")
                {  
                    Console.WriteLine("HA - quitter! run along, then!");
                    Console.WriteLine("");
                    break;
                }
                else if (answer=="m")
                {
                    HandleMoveAction();
                    state = HandleRoomInteraction();
                }
                else if (answer=="s")
                    state=HandleShootAction();
                else 
                    Console.WriteLine("Can't do that, try again");
                if (state==GameState.Win) // exit loop if  game has been won or lost
                {
                    Console.WriteLine("You Won! Wumpus will get you next time!");
                    Console.WriteLine("");
                    break;
                }
                else if (state==GameState.Loose)
                {
                    Console.WriteLine("Ha, Ha, Ha - You Loose!");
                    Console.WriteLine("");
                    break;
                }
                
            }
        }

        private String GetInput()
        {
            try {
                string answer=Console.ReadLine().ToLower();
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
    PIT    - 'I feel a draft'");
        }
        private GameControl _gc; // game controler
    }
}