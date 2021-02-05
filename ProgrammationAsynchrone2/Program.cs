using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammationAsynchrone2
{
    class Program
    {
        #region variables
        static int minReflexTime = 200;
        static int maxReflexTime = 1001;
        static int minRunTime = 1000;
        static int maxRunTime = 3001;

        public static List<string> names = new List<string> { "Pierre", "Paul", "Jacques" };
        #endregion

        static async Task Main(string[] args)
        {
            await RaceQ1();
        }

        #region Question1

        static async Task RaceQ1()
        {
            Console.WriteLine("DEPART");
            Runner r = new Runner("Maradonna");
            await r.Run();

            Console.WriteLine("ARRIVEE");
        }

        static async Task Runner(string name)
        {
            Random rand = new Random();
            int reflexTime = rand.Next(minReflexTime, maxReflexTime);
            await Task.Delay(reflexTime);
            Console.WriteLine("{0} démarre !", name);

            int runTime = rand.Next(minRunTime,maxRunTime);
            await Task.Delay(runTime);
            Console.WriteLine("{0} atteint la ligne d'arrivée", name);
        }
        #endregion

        #region Question2

        static async Task Race()
        {
            Console.WriteLine("DEPART");

            Task runner1 = Runner("Maradonna");
            Task runner2 = Runner("Evra");
            Task runner3 = Runner("Ronaldo");

            List<Task> runners = new List<Task> { runner1, runner2, runner3 };

            await Task.WhenAll(runners);

            Console.WriteLine("ARRIVEE");
        }

        #endregion
    }
}
