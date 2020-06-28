
using System;

using System.Collections.Generic;

using System.Globalization;

using System.Linq;

using System.Runtime.InteropServices.WindowsRuntime;

using System.Security.Cryptography.X509Certificates;

using System.Text;

using System.Threading.Tasks;

using System.Xml.Schema;


namespace BattleShipApp
{

    public abstract class Ship

    {

        public int length;

        public ConsoleColor color = ConsoleColor.DarkGray;

        public string content;

    }



    public class AircraftCarrier : Ship

    {

        public AircraftCarrier()

        {

            length = 5;

            content = "A";

        }

    }



    public class BattleShip : Ship

    {

        public BattleShip()

        {

            length = 4;

            content = "B";

        }

    }



    public class Cruiser : Ship

    {

        public Cruiser()

        {

            length = 3;

            content = "C";

        }

    }



    public class Destroyer : Ship

    {

        public Destroyer()

        {

            length = 2;

            content = "D";

        }

    }



    public class Submarine : Ship

    {

        public Submarine()

        {

            length = 1;

            content = "S";

        }

    }





    struct Tile

    {

        public int x;

        public int y;

        public string content;

        public ConsoleColor color;

        public Tile(int x, int y, string content, ConsoleColor color)

        {

            this.x = x; this.y = y; this.content = content; this.color = color;

        }



        public bool IsFree()

        {

            return (content == ".") ? true : false;

        }

    }



    class Player

    {

        Random rand;

        List<Tile> playermap;

        List<Tile> enemymap;

        int score;

        string name;

        Player enemy;

        public Player(string name)

        {

            score = 0;

            this.name = name;

        }



        public void InitPlayer()

        {

            CreateMapBoard();

            SetUpUnits();

        }

        void CreateMapBoard()

        {

            playermap = new List<Tile>();

            enemymap = new List<Tile>();

            for (int iterA = 0; iterA < 10; iterA++)

            {

                for (int iterB = 0; iterB < 10; iterB++)

                {

                    playermap.Add(new Tile(iterA, iterB, ".", ConsoleColor.Blue));

                    enemymap.Add(new Tile(iterA, iterB, ".", ConsoleColor.Blue));

                }

            }

        }

        void SetUpUnits()

        {

            rand = new Random(DateTime.Now.Millisecond);



            // create ships

            AircraftCarrier a = new AircraftCarrier();

            BattleShip b = new BattleShip();

            Cruiser c = new Cruiser();

            Destroyer d = new Destroyer();

            Submarine s = new Submarine();



            List<Ship> shipList = new List<Ship>() { a, b, c, d, s };



            bool placementOK = false;

            foreach (Ship ship in shipList)

            {

                int x, y, direction;

                do

                {

                    x = rand.Next(10);

                    y = rand.Next(10);

                    direction = (rand.Next(10)) % 2; // 0 horizontal, 1 = vertical

                    if (direction == 0)

                    {

                        if (x + ship.length <= 10)

                        {

                            for (int iterX = x; iterX < ship.length; iterX++)

                            {

                                placementOK = true;

                                if (!playermap.ElementAt<Tile>(y * 10 + iterX).IsFree())

                                {

                                    placementOK = false;

                                }

                            }

                        }

                    }

                    else

                    {

                        if (y + ship.length <= 10)

                        {

                            for (int iterY = y; iterY < ship.length; iterY++)

                            {

                                placementOK = true;

                                if (!playermap.ElementAt<Tile>(iterY * 10 + x).IsFree())

                                {

                                    placementOK = false;

                                }

                            }

                        }

                    }



                } while (!placementOK);

                // set ship on board

                if (direction == 0)

                {

                    for (int iterXX = 0; iterXX < ship.length; iterXX++)

                    {

                        int position = (y * 10) + (x + iterXX);

                        ChangeMapTile(playermap, position, ship.color, ship.content);

                    }

                }

                else

                {

                    for (int iterYY = 0; iterYY < ship.length; iterYY++)

                    {

                        int position = ((y + iterYY) * 10) + x;

                        ChangeMapTile(playermap, position, ship.color, ship.content);

                    }

                }



                placementOK = false;

            }

        }



        public void SetEnemy(Player enemy)

        {

            this.enemy = enemy;

            // For testing only...

            //ConsoleColor dummycolor;

            //string dummycontent;

            //for (int iter = 0; iter < 100; iter++)

            //{

            //    dummycolor = enemy.playermap.ElementAt<Tile>(iter).color;

            //    dummycontent = enemy.playermap.ElementAt<Tile>(iter).content;

            //    ChangeMapTile(enemymap, iter, dummycolor, dummycontent);

            //}

        }



        void ChangeMapTile(List<Tile> map, int position, ConsoleColor color, string content)

        {

            Tile dummy = map.ElementAt<Tile>(position);

            dummy.color = color;

            dummy.content = content;

            map.RemoveAt(position);

            map.Insert(position, dummy);

        }



        public void DrawMap(string message = "")

        {

            Console.Clear();



            int baseX = 2, baseY = 1;

            Console.Write("Player is " + name + " with score " + score + ".");



            baseX = 2; baseY = 3;



            int x, y;

            for (int iter = 0; iter < 100; iter++)

            {

                x = playermap.ElementAt<Tile>(iter).x;

                y = playermap.ElementAt<Tile>(iter).y;

                Console.SetCursorPosition(baseX + x, baseY + y);

                Console.BackgroundColor = playermap.ElementAt<Tile>(iter).color;

                Console.Write(playermap.ElementAt<Tile>(iter).content);

            }



            baseX = 15; baseY = 3;

            for (int iter = 0; iter < 100; iter++)

            {



                x = enemymap.ElementAt<Tile>(iter).x;

                y = enemymap.ElementAt<Tile>(iter).y;

                Console.SetCursorPosition(baseX + x, baseY + y);

                Console.BackgroundColor = enemymap.ElementAt<Tile>(iter).color;

                Console.Write(enemymap.ElementAt<Tile>(iter).content);

            }



            baseX = 2; baseY = 17;

            Console.SetCursorPosition(baseX, baseY);

            Console.BackgroundColor = ConsoleColor.Black;

            Console.WriteLine(message);

        }



        public void Fire()

        {

            Console.WriteLine("Fire Now!");



            bool validResponse = false;

            int responseX, responseY;

            do

            {

                Console.WriteLine("X...");

                string guessInput = Console.ReadLine();



                // convert string to number

                validResponse = int.TryParse(guessInput, out responseX);

                if (validResponse)

                {

                    if (responseX < 0 || responseX > 9) { validResponse = false; }

                }



            } while (!validResponse);



            do

            {

                Console.WriteLine("Y...");

                string guessInput = Console.ReadLine();



                // convert string to number

                validResponse = int.TryParse(guessInput, out responseY);

                if (validResponse)

                {

                    if (responseY < 0 || responseY > 9) { validResponse = false; }

                }

            } while (!validResponse);



            int position = (responseX * 10) + responseY;

            // evaluate if hit or miss

            // If miss

            if (enemy.playermap.ElementAt<Tile>(position).content == ".")

            {

                ChangeMapTile(enemymap, position, ConsoleColor.Blue, "!");

            }

            else if (

                enemymap.ElementAt<Tile>(position).content != "A" &&

                enemymap.ElementAt<Tile>(position).content != "B" &&

                enemymap.ElementAt<Tile>(position).content != "C" &&

                enemymap.ElementAt<Tile>(position).content != "D" &&

                enemymap.ElementAt<Tile>(position).content != "S"

                )

            {

                score++;

                string content = enemy.playermap.ElementAt<Tile>(position).content;

                DrawMap("You hit enemy! Fire again...");

                Fire();

            }



            Console.WriteLine("<Return> to Change Player...");

            Console.ReadLine();

        }



        public bool DidWin()

        {

            bool winner = (score >= 15) ? true : false;

            if (winner)

            {

                Console.WriteLine("Winner is " + name);

            }

            return winner;

        }



    }



    class Game

    {

        Player player1;

        Player player2;

        public Game()

        {

            // initiate players

            player1 = new Player("Player 1");

            player2 = new Player("Player 2");



            // initiate players and setup units

            player1.InitPlayer();

            player2.InitPlayer();



            // Player 1's enemymap is player2 map and vice versa

            player1.SetEnemy(player2);

            player2.SetEnemy(player1);

        }



        public void PlayGame()

        {

            // run game loop

            bool didWin = false;

            do

            {

                // player1

                player1.DrawMap();

                player1.Fire();

                player1.DrawMap();

                didWin = player1.DidWin();



                // player2

                player2.DrawMap();

                player2.Fire();

                player2.DrawMap();

                didWin = player2.DidWin();



            } while (!didWin);

        }



    }





    class Program

    {

        static void Main(string[] args)

        {

            Game game = new Game();

            game.PlayGame();



            Console.ReadLine();

        }

    }

}