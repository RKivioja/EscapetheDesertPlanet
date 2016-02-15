using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextAdventure
{
    class Enemy
    {
        private static string name;
        private static int health;
        private static int damage;

        public static string Name
        {
            get { return name; }
            set { name = value; }
        }

        public static int Health
        {
            get { return health; }
            set { health = value; }
        }

        public static int Damage
        {
            get { return damage; }
            set { damage = value; }
        }
    }
}
