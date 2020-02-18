using System;

namespace BlueDragon
{
    static class Player
    {
        public static int spirit { get; set; } = 0;
        public static float combatRegenMod { get; set; } = 0;
        public static float combatTime { get; set; } = 0;

        public static int realTrinketTicks { get; set; } = 0;
        public static float extraMana { get; set; } = 0;
        public static float extraMana3pc { get; set; } = 0;
        public static int trinketProcCount { get; set; } = 0;

        public static float trinketTimeRemaining { get; set; } = 0;
        public static float castTimeRemaining { get; set; } = 0;
        public static int spellsCast { get; set; } = 0;

        private static int Heal { get; set; }
        private static int Renew { get; set; }
        private static int FlashHeal { get; set; }
        private static int Prayer { get; set; }

        private const float baseRegen = 0.15f;
        private const float baseRegen3pc = 0.3f;

        private static Random rng = new Random();

        public static void Reset()
        {
            extraMana = 0;
            extraMana3pc = 0;
            combatTime = 0;
            realTrinketTicks = 0;
            castTimeRemaining = 0;
            trinketProcCount = 0;
            spellsCast = 0;
        }

        public static void Tick()
        {
            if(trinketTimeRemaining > 0)
            {
                trinketTimeRemaining -= 0.5f;
            }

            castTimeRemaining -= 0.5f;
            combatTime += 0.5f;

           // PrintDebug();
        }

        public static bool IsBusy()
        {
            if(castTimeRemaining > 0) { return true; }
            return false;
        }
        
        public static void TickBlueDragon()
        {
            realTrinketTicks++;
            extraMana += GetDeltaRegen(baseRegen);
            extraMana3pc += GetDeltaRegen(baseRegen3pc);
        }

        public static bool IsBlueDragonActive()
        {
            if(trinketTimeRemaining > 0) { return true; }
            return false;
        }

        public static float GetFullRegen()
        {
            return spirit / 4;
        }

        public static float GetPassiveRegen(float regenMod)
        {
            return (spirit / 4) * regenMod;
        }

        public static float GetDeltaRegen(float regenMod)
        {
            return GetFullRegen() - GetPassiveRegen(regenMod);
        }

        public static float GetLostTicks()
        {
            return (trinketProcCount * 7.5f) - realTrinketTicks;
        }

        public static float GetEffectiveMP5()
        {
            return (extraMana / combatTime) * 5f;
        }

        public static float GetEffective3pcMP5()
        {
            return (extraMana3pc / combatTime) * 5f;
        }
         
        public static float GetRealProcRate()
        {
            return ((float)trinketProcCount / (float)spellsCast);
        }

        public static void TryProcTheDragon()
        {
            int trinketProcRng = rng.Next(100);

            if (trinketProcRng < 2)
            {
                trinketTimeRemaining = 15.0f;
                trinketProcCount++;
            }
        }

        public static void SetProfile(string profile)
        {
            switch (profile)
            {
                case "heal":
                    Heal = 70; Renew = 10; FlashHeal = 10; Prayer = 10;
                    break;
                case "flash":
                    Heal = 30; Renew = 10; FlashHeal = 50; Prayer = 5;
                    break;
                case "renew":
                    Heal = 20; Renew = 60; FlashHeal = 10; Prayer = 5;
                    break;
                case "eco":
                    Heal = 65; Renew = 30; FlashHeal = 0; Prayer = 5;
                    break;
            }
        }

        public static void CastNextHeal()
        {
            int rngValue = rng.Next(100);

            // Console.WriteLine($"RNG: {rngValue}");

            if (rngValue < Heal)
            {
                castTimeRemaining = 2.5f; // Cast Heal
            }
            else if (rngValue >= Heal && rngValue < (Heal + Renew))
            {
                castTimeRemaining = 1.5f; // Renew
            }
            else if (rngValue >= (Heal + Renew) && rngValue < (Heal + Renew + FlashHeal))
            {
                castTimeRemaining = 1.5f; // Flash Heal
            }
            else if (rngValue >= (Heal + Renew + FlashHeal))
            {
                castTimeRemaining = 3.0f; // Prayer
            }
            else
            {
                castTimeRemaining = 2.5f; // Failsafe
            }

            spellsCast++;
            TryProcTheDragon();
        }

        private static void PrintDebug()
        {
            Console.WriteLine($" -- Tick {combatTime} -- ");
            Console.WriteLine($" Cast Time Remaining:  {castTimeRemaining}");
            Console.WriteLine($" Trinket Time Remaining:  {trinketTimeRemaining}");
        }
    }
}
