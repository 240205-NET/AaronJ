using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;


namespace BattleShip
{
    // add cheats
    // Improve display
        // Clear console and populate a new map each time an entry is made
        // Add color to text 
        // Improve Ship Status information
    // Smarter opponent
    // Make save map(as an option)
    // Fire position 0
    // Load Last Save
    class Program
    {
        static void Main(string[] args)
        {   
            //**********HERE**********
            //LOOK HERE CHANGE THIS FOR DEMO
            bool showShips = false;//(black sheep wall)
            //LOOK HERE CHANGE THIS FOR DEMO
            //**********HERE**********

            Ships MyShip = new Ships();
            Ships EnemyShip = new Ships();



            Dictionary<char, int> Coordinates = PopulateDictionary();
            PrintHeader();
            for (int h = 0; h < 19; h++)
            {
                Console.Write(" ");
            }


            PrintMap(MyShip.FirePositions, MyShip, EnemyShip, showShips);

            int Game;
            for (Game = 1; Game < 101; Game++)
            {
                MyShip.StepsTaken++;

                Node position = new Node();

                Console.WriteLine("e(x)it \t (n)ew game \t(s)ave map");
                Console.WriteLine("Enter firing position (e.g. B6)");
                string? input = Console.ReadLine();
                position = AnalyzeInput(input, Coordinates);

                if (position.x == -1 || position.y == -1)
                {
                    Console.WriteLine("Invalid coordinates!");
                    Game--;
                    continue;
                }
                
                if (MyShip.FirePositions.Any(EFP => EFP.x == position.x && EFP.y == position.y))
                {
                    Console.WriteLine("A trout jumps out of the water and slaps you around a bit. You've already fired here! Go pick on another fish!");
                    Game--;
                    continue;
                }

                


                EnemyShip.Fire();
                


                var index = MyShip.FirePositions.FindIndex(p => p.x == position.x && p.y == position.y);

                if (index == -1)
                {
                    MyShip.FirePositions.Add(position);
                }
                    
                //Clear();
               


                MyShip.AllShipsPosition.OrderBy(o => o.x).ThenBy(n => n.y).ToList();
                MyShip.CheckShipStatus(EnemyShip.FirePositions);

                EnemyShip.AllShipsPosition.OrderBy(o => o.x).ThenBy(n => n.y).ToList();
                EnemyShip.CheckShipStatus(MyShip.FirePositions);

                PrintHeader();
                for (int h = 0; h < 19; h++)
                {
                    Console.Write(" ");
                }



                PrintMap(MyShip.FirePositions, MyShip, EnemyShip, showShips);

                Commentator(MyShip, true);
                Commentator(EnemyShip, false);
                if (EnemyShip.IsObliteratedAll || MyShip.IsObliteratedAll) { break; }


            }


            if (EnemyShip.IsObliteratedAll && !MyShip.IsObliteratedAll)
            {
                Console.WriteLine("You win.");
            }
            else if (!EnemyShip.IsObliteratedAll && MyShip.IsObliteratedAll)
            {
                Console.WriteLine("You lose.");
            }
            else
            {
                Console.WriteLine("The game ended in a draw.");
            }

            Console.WriteLine("You won in:{0} turns!", Game);
            Console.ReadLine();


        }

        static void PrintStatistic(int x, int y, Ships Ship)
        {
            if (x == 1 && y == 10)
            {
                Console.Write("Ship Status:  ");
            }


            if (x == 2 && y == 10)
            {
                if (Ship.IsCarrierSunk)
                {
                    Console.Write("Carrier [5]   ");
                }
                else
                {
                    Console.Write("Carrier [5]   ");
                }

            }

            if (x == 3 && y == 10)
            {
                if (Ship.IsBattleshipSunk)
                {
                    Console.Write("Battleship [4]");
                }
                else
                {
                    Console.Write("Battleship [4]");
                }
            }

            if (x == 4 && y == 10)
            {

                if (Ship.IsDestroyerSunk)
                {
                    Console.Write("Destroyer [3] ");
                }
                else
                {
                    Console.Write("Destroyer [3] ");
                }
            }

            if (x == 5 && y == 10)
            {

                if (Ship.IsSubmarineSunk)
                {
                    Console.Write("Submarine [3] ");
                }
                else
                {
                    Console.Write("Submarine [3] ");
                }
            }

            if (x == 6 && y == 10)
            {

                if (Ship.IsPatrolBoatSunk)
                {
                    Console.Write("PatrolBoat [2]");
                }
                else
                {
                    Console.Write("PatrolBoat [2]");
                }

            }


            if (x > 6 && y == 10)
            {
                for (int i = 0; i < 14; i++)
                {
                    Console.Write(" ");
                }
            }

        }

        static void PrintMap(List<Node> positions, Ships MyShip, Ships EnemyMyShip, bool showEnemyShips)
        {
            PrintHeader();
            Console.WriteLine();
            if (!showEnemyShips)
                showEnemyShips = MyShip.IsObliteratedAll;
           
            List<Node> SortedLFirePositions = positions.OrderBy(o => o.x).ThenBy(n => n.y).ToList();
            List<Node> SortedShipsPositions = EnemyMyShip.AllShipsPosition.OrderBy(o => o.x).ThenBy(n => n.y).ToList();

            SortedShipsPositions = SortedShipsPositions.Where(FP => !SortedLFirePositions.Exists(ShipPos => ShipPos.x == FP.x && ShipPos.y == FP.y)).ToList();


            int hitCounter = 0;
            int EnemyshipCounter = 0;
            int myShipCounter = 0;
            int enemyHitCounter = 0;

            char row = 'A';
            try
            {
                for (int x = 1; x < 11; x++)
                {
                    for (int y = 1; y < 11; y++)
                    {
                        bool keepGoing = true;

                        
                        if (y == 1)
                        {
                            Console.Write("[" + row + "]");
                            row++;
                        }
                       


                        if (SortedLFirePositions.Count != 0 && SortedLFirePositions[hitCounter].x == x && SortedLFirePositions[hitCounter].y == y)
                        {

                            if (SortedLFirePositions.Count - 1 > hitCounter)
                                hitCounter++;

                            if (EnemyMyShip.AllShipsPosition.Exists(ShipPos => ShipPos.x == x && ShipPos.y == y))
                            {

                                Console.Write("[*]");

                               
                                keepGoing = false;
                                

                            }
                            else
                            {

                                Console.Write("[X]");

                                keepGoing = false;
                                
                            }

                        }

                        if (keepGoing && showEnemyShips && SortedShipsPositions.Count != 0 && SortedShipsPositions[EnemyshipCounter].x == x && SortedShipsPositions[EnemyshipCounter].y == y)

                        {

                            if (SortedShipsPositions.Count - 1 > EnemyshipCounter)
                                EnemyshipCounter++;

                            Console.Write("[O]"); 
                            keepGoing = false; 
                        }

                        if (keepGoing)
                        {
                            Console.Write("[~]");
                        }


                        PrintStatistic(x, y, MyShip);


                        if (y == 10)
                        {
                            Console.Write("      ");

                            PrintMapOfEnemy(x, row, MyShip, EnemyMyShip, ref myShipCounter, ref enemyHitCounter);
                        } 
                    }

                    Console.WriteLine();
                }

            }
            catch (Exception e)
            {
                string error = e.Message.ToString();
            }
        }

        static void PrintMapOfEnemy(int x, char row, Ships MyShip, Ships EnemyShip, ref int MyshipCounter, ref int EnemyHitCounter)
        {
            List<Node> EnemyFirePositions = new List<Node>();
            row--;
            Random random = new Random();
            List<Node> SortedLFirePositions = EnemyShip.FirePositions.OrderBy(o => o.x).ThenBy(n => n.y).ToList();
            List<Node> SortedLShipsPositions = MyShip.AllShipsPosition.OrderBy(o => o.x).ThenBy(n => n.y).ToList();

            SortedLShipsPositions = SortedLShipsPositions.Where(FP => !SortedLFirePositions.Exists(ShipPos => ShipPos.x == FP.x && ShipPos.y == FP.y)).ToList();
              

            try
            {

                for (int y = 1; y < 11; y++)
                {
                    bool keepGoing = true;

                    
                    if (y == 1)
                    {
                        Console.Write("[" + row + "]");
                        row++;
                    }
                    


                    if (SortedLFirePositions.Count != 0 && SortedLFirePositions[EnemyHitCounter].x == x && SortedLFirePositions[EnemyHitCounter].y == y)
                    {

                        if (SortedLFirePositions.Count - 1 > EnemyHitCounter)
                            EnemyHitCounter++;

                        if (MyShip.AllShipsPosition.Exists(ShipPos => ShipPos.x == x && ShipPos.y == y))
                        {
                            Console.Write("[*]");

                            
                            keepGoing = false;
                           

                        }
                        else
                        {
                            Console.Write("[X]");

                            
                            keepGoing = false;
                            

                        }

                    }

                    if (keepGoing && SortedLShipsPositions.Count != 0 && SortedLShipsPositions[MyshipCounter].x == x && SortedLShipsPositions[MyshipCounter].y == y)

                    {

                        if (SortedLShipsPositions.Count - 1 > MyshipCounter)
                            MyshipCounter++;

                        Console.Write("[O]");

                        
                        keepGoing = false;
                        

                    }

                    if (keepGoing)
                    {
                        Console.Write("[~]");
                    }


                    PrintStatistic(x, y, EnemyShip);

                }
                 

            }
            catch (Exception e)
            {
                string error = e.Message.ToString();
            }
        }

        static Node AnalyzeInput(string input, Dictionary<char, int> Coordinates)
        {
            Node pos = new Node();

            char[] inputSplit = input.ToUpper().ToCharArray();

            if (inputSplit.Length < 2 || inputSplit.Length > 4)
            {

                return pos;
            }




            if (Coordinates.TryGetValue(inputSplit[0], out int value))
            {
                
                pos.x = value;
            }
            else
            {
                return pos;
            }




            if (inputSplit.Length == 3)
            {

                if (inputSplit[1] == '1' && inputSplit[2] == '0')
                {
                    
                    pos.y = 10;
                    return pos;
                }
                else
                {
                   return pos;
                }

            }

            


            if (inputSplit[1] - '0' > 9)
            {
                
                return pos;
            }
            else
            {
                pos.y = inputSplit[1] - '0';
            }

            

            return pos;
        }

        static void PrintHeader()
        {
            Console.Write("[ ]");
            for (int i = 1; i < 11; i++)
                Console.Write("[" + i + "]");


        }


        static Dictionary<char, int> PopulateDictionary()
        {
            Dictionary<char, int> Coordinate =
                     new Dictionary<char, int>
                     {
                         { 'A', 1 },
                         { 'B', 2 },
                         { 'C', 3 },
                         { 'D', 4 },
                         { 'E', 5 },
                         { 'F', 6 },
                         { 'G', 7 },
                         { 'H', 8 },
                         { 'I', 9 },
                         { 'J', 10 }
                     };

            return Coordinate;
        }

        static void Commentator(Ships Ship, bool isMyShip)
        {

            string title = isMyShip ? "Your" : "Enemy";

            if (Ship.CheckPBattleship && Ship.IsBattleshipSunk)
            {
                Console.WriteLine("{0} {1} sunk", title, nameof(Ship.Battleship));
                Ship.CheckPBattleship = false;
            }

            if (Ship.CheckCarrier && Ship.IsCarrierSunk)
            {
                Console.WriteLine("{0} {1} sunk", title, nameof(Ship.Carrier));
                Ship.CheckCarrier = false;
            }

            if (Ship.CheckDestroyer && Ship.IsDestroyerSunk)
            {
                Console.WriteLine("{0} {1} sunk", title, nameof(Ship.Destroyer));
                Ship.CheckDestroyer = false;
            }

            if (Ship.CheckPatrolBoat && Ship.IsPatrolBoatSunk)
            {
                Console.WriteLine("{0} {1} sunk", title, nameof(Ship.PatrolBoat));
                Ship.CheckPatrolBoat = false;
            }

            if (Ship.CheckSubmarine && Ship.IsSubmarineSunk)
            {
                Console.WriteLine("{0} {1} sunk", title, nameof(Ship.Submarine));
                Ship.CheckSubmarine = false;
            }

        }
    }
}