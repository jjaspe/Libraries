using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterSystemLibrary.Classes
{
    public class Globals
    {
        private Globals() { }
        public static int SKILL_LIMIT = 100;
        public static int ATTRIBUTE_LIMIT = 100;
        public static int WEAPON_LIMIT = 100;
        public static int EFFECT_LIMIT = 1000;
        public static int CHARACTER_LIMIT = 100;
        public static int ACTION_LIMIT = 300;
        public static int STAT_LIMIT = 10000;
        public static int PROPERTY_LIMIT = ATTRIBUTE_LIMIT + SKILL_LIMIT + STAT_LIMIT;

        public static int RandomNumber(int Min, int Max)
        {
            Random random = new Random();
            return random.Next(Min, Max);
        }
    }
}
