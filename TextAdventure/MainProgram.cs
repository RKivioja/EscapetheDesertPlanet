using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TextAdventure
{
    class MainProgram
    {
        private static string[] texts;
        private static string[] enemies;

        public static string[] Enemies
        {
            get { return enemies; }
            set { enemies = value; }
        }
        
        /// <summary>
        /// The main for the whole program. Contains the game loop.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            ReadTexts();
            PlayerInit();
            StartScreen();
            Intro();
            while(Player.Location == 'S') Settlement();
            Console.ReadKey();
        }

        /// <summary>
        /// Initializes values for the player-class.
        /// </summary>
        private static void PlayerInit()
        {
            Player.Health = 20;

            Player.Level = 1;

            Player.Eligible = false;
            
            Player.Medkit = false;
        }

        /// <summary>
        /// Start screen for the game.
        /// </summary>
        private static void StartScreen()
        {
            PrintLines(1, 7, "Q", false);
            for (int i = 0; i < 10; i++)
            {
                PrintLine(" ", false);
            }
            PrintLines(8, 9, "Q", false);
            
            ConsoleKeyInfo qinput = Console.ReadKey();

            if (qinput.Key == ConsoleKey.Escape)
            {
                Environment.Exit(0);
            }
            Console.Clear();
        }
        
        /// <summary>
        /// The intro sequence for the main game.
        /// </summary>
        private static void Intro()
        {
            Player.Location = 'I';
            PrintLines(1, 8, "I", true); //introtexts
            
            PrintLines(9, 13, "I", false); //ask occupation
            
            char input = PlayerPromptKey();
            
            bool occupationfound = false;

            while (occupationfound == false)
            {
                switch (input)
                {
                    case 'a':
                        PrintLine(PickLine("I.14"), true);
                        occupationfound = true;
                        break;
                    case 'b':
                        PrintLine(PickLine("I.14"), true);
                        occupationfound = true;
                        break;
                    case 'c':
                        PrintLine(PickLine("I.14"), true);
                        occupationfound = true;
                        break;
                    case 'd':
                        GameOver("I.15");
                        return;
                    default:
                        PrintLines(9, 13, "I", false);
                        input = PlayerPromptKey();
                        break;
                }
            }

            PrintLine(PickLine("I.16"), false); //name
            
            string name = PlayerPromptLine();
            
            bool passed = false;

            while (passed == false)
            {
                if (CheckName(name))
                {
                    Player.Name = name;
                    passed = true;
                }
                else
                {
                    PrintLine(PickLine("I.17"), false);
                    name = PlayerPromptLine();
                }
            }

            Player.Location = 'S';
        }

        /// <summary>
        /// The central hub for the game, labeled as "settlement".
        /// </summary>
        private static void Settlement()
        {
            PrintLine("Name: " + Player.Name + "                           Credit: " + Player.Gold.ToString(), false);
            PrintLine(" ", false);
            PrintLines(1, 8, "S", false);
            PrintLine(" ", false);

            char schoice = PlayerPromptKey();

            switch (schoice)
            {
                case 't':
                    Player.Location = 'T';
                    while(Player.Location == 'T') Tavern();
                    break;
                case 'a':
                    Player.Location = 'A';
                    while (Player.Location == 'A') Arena();
                    break;
                case 's':
                    Player.Location = 'R';
                    while (Player.Location == 'R') Store();
                    break;
                case 'p':
                    Player.Location = 'P';
                    while (Player.Location == 'P') Palace();
                    return;
                default:
                    Settlement();
                    break;
            }
        }

        /// <summary>
        /// The kalif's palace.
        /// </summary>
        private static void Palace()
        {
            if (Player.Eligible == true)
            {
                Outro();
            }
            else
            {
                PrintLines(1, 1, "P", false);
                char choice = PlayerPromptKey();
                Player.Location = 'S';
                Settlement();
            }
        }

        /// <summary>
        /// Arena where all the combat in the game takes place.
        /// </summary>
        private static void Arena()
        {
            PrintLines(1, 1, "A", false);
            PrintLine(" ", false);
            PrintLine("HEALTH: " + Player.Health, false);
            PrintLine(" ", false);
            PrintLines(2, 6, "A", false);
            
            char achoice = PlayerPromptKey();

            switch (achoice)
            {
                case 's':
                    Battle();
                    break;
                case 'l':
                    Player.Location = 'S';
                    Settlement();
                    break;
                default:
                    Arena();
                    break;
            }
        }

        /// <summary>
        /// The battle state of the game. Only accessible through Arena.
        /// </summary>
        private static void Battle()
        {
            CreateEnemy();

            bool inbattle = true;
            
            while (inbattle)
            {
                PrintLine("ENEMY: " + Enemy.Name + "                    ENEMY HEALTH: " + Enemy.Health, false);
                PrintLines(1, 4, "C", false);
                PrintLine("HEALTH: " + Player.Health, false);

                char action = PlayerPromptKey();

                switch (action)
                {
                    case 't':
                        //attack
                        Enemy.Health = Enemy.Health - 2;

                        if (Enemy.Health <= 0)
                        {
                            inbattle = false;
                            if (Player.Level < 5) Player.Level = Player.Level + 1;
                            if (Enemy.Name == "DRAGON") Player.Eligible = true;
                            Arena();
                        }
                        else
                        {
                            PrintLines(6, 6, "C", false);
                            PrintLine("The " + Enemy.Name + " whimpers in agony.", false);
                            PrintLine(" ", false);
                        }
                        break;
                    case 'a':
                        //defend
                        Player.Defence = true;
                        break;
                    case 's':
                        //use medkit
                        if (Player.Medkit)
                        {
                            Player.Health = Player.Health + 20;
                            PrintLines(9, 9, "C", false);
                            PrintLine(" ", false);
                        }
                        else
                        {
                            PrintLines(10, 10, "C", false);
                            PrintLine(" ", false);
                        }
                        break;
                    case 'p':
                        //flee
                        Arena();
                        break;
                    default:
                        //do nothing
                        break;
                }

                PrintLine(Enemy.Name + " attacks!", false);

                if (Player.Defence == true)
                {
                    PrintLines(5, 5, "C", false);
                    Player.Defence = false;
                }
                else
                {
                    Player.Health = Player.Health - Enemy.Damage;
                    PrintLine("You take " + Enemy.Damage + " damage.", false);
                    PrintLine(" ", false);

                    if (Player.Health <= 0)
                    {
                        inbattle = false;
                        GameOver("C8");
                    }
                }
            }
        }

        /// <summary>
        /// Creates an enemy based on the info in the text document.
        /// </summary>
        private static void CreateEnemy()
        {
            string enemy = enemies[Player.Level];

            string[] enemydata = enemy.Split('-');

            Enemy.Name = enemydata[0];
            Enemy.Health = Int32.Parse(enemydata[1]);
            Enemy.Damage = Int32.Parse(enemydata[2]);
        }

        /// <summary>
        /// Store where player can shop for gear.
        /// </summary>
        private static void Store()
        {
            PrintLines(1, 5, "R", false);
            char rchoice = PlayerPromptKey();

            switch (rchoice)
            {
                case 'b':
                    PrintLines(6, 13, "R", false);
                    char buy = PlayerPromptKey();

                    switch (rchoice)
                    {
                        case 'a':
                            Player.Medkit = true;
                            break;
                        default:
                            Store();
                            break;
                    }

                    break;
                case 'l':
                    Player.Location = 'S';
                    Settlement();
                    break;
                default:
                    Store();
                    break;
            }
        }

        /// <summary>
        /// Tavern where the player can go.
        /// </summary>
        private static void Tavern()
        {
            PrintLines(1, 6, "T", false);
            char tchoice = PlayerPromptKey();

            switch (tchoice)
            {
                case 's':
                    PrintLines(7, 7, "T", true);
                    PrintLines(8, 8, "T", true);
                    Player.Health = Player.Health + 10;
                    break;
                case 'r':
                    Random rndom = new Random();
                    int rumour = rndom.Next(10, 15);

                    PrintLines(rumour, rumour, "T", true);
                    break;
                case 'l':
                    Player.Location = 'S';
                    Settlement();
                    break;
                default:
                    Tavern();
                    break;
            }
        }

        /// <summary>
        /// The ending sequence for the game.
        /// </summary>
        private static void Outro()
        {
            PrintLine("YOU WIN THE GAME!", false);
        }

        /// <summary>
        /// Game over screen with customizable end message.
        /// </summary>
        /// <param name="ending">The line used in the ending message.</param>
        private static void GameOver(string ending)
        {
            Player.Location = 'G';
            PrintLine(PickLine(ending), true);
            PrintLines(1, 6, "G", false);
        }

        /// <summary>
        /// Prints an indicator for the player to input a single key press and records the key press.
        /// </summary>
        /// <returns>The key that was pressed.</returns>
        private static char PlayerPromptKey()
        {
            Console.Write(">");
            ConsoleKeyInfo input = Console.ReadKey();
            char choice = input.KeyChar;
            Console.Clear();

            return choice;
        }

        /// <summary>
        /// Prints an indicator for the player to input a single line of text and records the text.
        /// </summary>
        /// <returns>Player's input.</returns>
        private static string PlayerPromptLine()
        {
            Console.Write(">");
            string input = Console.ReadLine();
            Console.Clear();
            return input;
        }

        /// <summary>
        /// Reads the script for the game from the text file.
        /// </summary>
        private static void ReadTexts()
        {
            try
            {
                texts = System.IO.File.ReadAllLines(@"C:/Users/Roope/Documents/Koulutus/ajk/harjoitustyö/harjotustyo/TextAdventure/GameTexts.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to read game texts: " + e);
            }

            try
            {
                enemies = System.IO.File.ReadAllLines(@"C:/Users/Roope/Documents/Koulutus/ajk/harjoitustyö/harjotustyo/TextAdventure/Enemies.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to read enemy data: " + e);
            }
        }

        /// <summary>
        /// Picks a single line from the script 
        /// </summary>
        /// <param name="lineID">ID of the line, for example: "I.10"</param>
        /// <returns>The line from the given ID</returns>
        private static string PickLine(string lineID)
        {
            int location = 0;
            
            for (int i = 0; i < texts.Length; i++)
            {
                if (texts[i].Contains(lineID))
                {
                    location = i;
                }
            }

            string line = texts[location];
            line = line.Substring(line.IndexOf(" ") + 1);
            return line;
        }
        
        /// <summary>
        /// Prints a given string to the console screen.
        /// </summary>
        /// <param name="line">String that gets printed.</param>
        /// <param name="refresh">Dictates if screen is cleared after print and key press.</param>
        private static void PrintLine(string line, bool refresh)
        {
            Console.WriteLine(line);
            
            if (refresh)
            {
                Console.ReadKey();
                Console.Clear();            
            }
        }

        /// <summary>
        /// Prints multiple lines from the given range.
        /// </summary>
        /// <param name="start">Beginning of the printing area.</param>
        /// <param name="end">End of the printing area.</param>
        /// <param name="chapter">Chapter for the print.</param>
        /// <param name="refresh">Dictates if screen is cleared after print and key press.</param>
        private static void PrintLines(int start, int end, string chapter, bool refresh)
        {
            for (int i = start; i <= end; i++)
            {
                if (i < 10) PrintLine(PickLine(chapter + i.ToString()), refresh);
                else PrintLine(PickLine(chapter + "." + i.ToString()), refresh);
            }
        }

        /// <summary>
        /// Checks if the given string resembles a full name.
        /// </summary>
        /// <param name="input">String that is checked.</param>
        /// <returns>If the string is a full name or not.</returns>
        private static bool CheckName(string input)
        {
            string pattern = @"^[A-ZÅÄÖa-zåäö\s]+[A-ZÅÄÖa-zåäö\s]*$";

            if (Regex.IsMatch(input, pattern)) return true;
            else return false;
        }
    }
}
