using System;
using System.Linq;
using System.Collections.Generic;

namespace BlueDragon
{
    class Program
    {
        static List<Result> results = new List<Result>();

        static void Main(string[] args)
        {
            SetupPlayer();

            while (true)
            {
                GetInput();
            }
        }

        static void GetInput()
        {
            results.Clear();

            Console.Write("Expected Fight Length (Seconds): ");
            if(!Int32.TryParse(Console.ReadLine(), out int dur)) { return; }

            Console.WriteLine("Profile (heal, flash, renew, eco): ");
            string profile = Console.ReadLine().ToLower();

            Console.WriteLine("How many simulations shall we run???");
            if(!Int32.TryParse(Console.ReadLine(), out int count)) { return; }

            for (int i = 0; i < count; i++)
            {
                SimTheThing(dur, profile);
            }

            PrintResultsAverages();
        }

        static void SimTheThing(int fightDuration, string profile)
        {
            Player.SetProfile(profile);
            Player.Reset();

            float combatTime = 0;

            while(combatTime < fightDuration)
            {
                SimulateTick(combatTime);
                combatTime += 0.5f; // Simulating play at increments of 500ms
            }

            results.Add(new Result(Player.trinketProcCount, Player.realTrinketTicks, Player.GetLostTicks(),
                        Player.GetEffectiveMP5(), Player.GetEffective3pcMP5(), Player.GetRealProcRate()));

            // PrintResults(); // << Uncomment this to see individual sim results.
        }

        static void ManaTick()
        {
            if (Player.IsBlueDragonActive()) { Player.TickBlueDragon(); }
        }

        static void SimulateTick(float combatTime)
        {
            if (combatTime % 2 == 0) { ManaTick(); }
            if(!Player.IsBusy()) { Player.CastNextHeal(); }

            Player.Tick();   
        }

        static void SetupPlayer()
        {
            Player.spirit = 350; // <<< CHNAGE SPIRIT HERE!
        }

        static void PrintResults()
        {
            Console.WriteLine(" *** Results *** ");
            Console.WriteLine($" Proc Count: {Player.trinketProcCount}");
            Console.WriteLine($" Trinket Tick Count: {Player.realTrinketTicks}");
            Console.WriteLine($" Lost Ticks: {Player.GetLostTicks()}");
            Console.WriteLine($" Effective MP5: {Player.GetEffectiveMP5()}");
            Console.WriteLine($" Effective MP5 (3pc T2): {Player.GetEffective3pcMP5()}");
            Console.WriteLine($" Proc Rate: {Player.GetRealProcRate()}");
        }

        static void PrintResultsAverages()
        {
            float mp5avg = results.Average(r => r.effectiveMP5);
            float total = results.Sum(r => r.effectiveMP5);
            double mp5stddev = Math.Sqrt((total) / (results.Count() - 1));


            Console.WriteLine(" #### Averages ####");
            Console.WriteLine($" Proc Count: {results.Average(r => r.procCount)}");
            Console.WriteLine($" Trinket Tick Count: {results.Average(r => r.tickCount)}");
            Console.WriteLine($" Lost Ticks: {results.Average(r => r.lostTicks)}");
            Console.WriteLine($" Effective MP5: {mp5avg}");
            Console.WriteLine($" Effective MP5 (3pc T2): {results.Average(r => r.effective3pcMP5)}");
            Console.WriteLine($" Effective MP5 - Std Dev: {mp5stddev}");
            Console.WriteLine($" Proc Rate: {results.Average(r => r.procRate)}\n");
        }
    }
}
