using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextAdventure
{
    class Player
    {
        private static char location;
        private static string name;
        private static int gold;
        private static int health;
        private static bool defence;
        private static bool eligible;
        private static int level;
        private static bool medkit;

        public static string Name
        {
            get { return name; }
            set { name = value; }
        }

        public static char Location
        {
            get { return location; }
            set { location = value; }
        }

        public static int Gold
        {
            get { return gold; }
            set { gold = value; }
        }

        public static int Health
        {
            get { return health; }
            set { health = value; }
        }

        public static bool Defence
        {
            get { return defence; }
            set { defence = value; }
        }

        public static bool Eligible
        {
            get { return eligible; }
            set { eligible = value; }
        }

        public static int Level
        {
            get { return level; }
            set { if(value < MainProgram.Enemies.GetLength(0)) level = value; }
        }

        public static bool Medkit
        {
            get { return medkit; }
            set { medkit = value; }
        }
    }
}
