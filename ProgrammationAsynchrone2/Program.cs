using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProgrammationAsynchrone2
{
    class Program
    {
        #region variables
        static int minReflexTime = 200;
        static int maxReflexTime = 1001;
        static int minRunTime = 8000;
        static int maxRunTime = 16001;

        public static List<string> names = new List<string> { "Pierre", "Paul", "Jacques" };

        static readonly CancellationTokenSource RaceCancel = new CancellationTokenSource();

        static async Task Main(string[] args)
        {
            await RaceWithCancellableRunners_Q5();

        }

        static int NameToInt(string name)
        {
            int result = 0;
            foreach (char c in name)
            {
                result += c;
            }

            return result;
        }

        #endregion

        

        #region Question1

        static async Task RaceQ1()
        {
            Console.WriteLine("DEPART");

            await Runner("MARADONNA");

            Console.WriteLine("ARRIVEE");
        }

        static async Task RaceQ1_O()
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

        static async Task RaceQ2()
        {
            Console.WriteLine("DEPART");

            Task runner1 = Runner("Maradonna");
            Task runner2 = Runner("Evra");
            Task runner3 = Runner("Ronaldo");

            List<Task> runners = new List<Task> { runner1, runner2, runner3 };

            await Task.WhenAll(runners);

            Console.WriteLine("ARRIVEE");
        }

        static async Task RaceQ2_0()
        {
            Console.WriteLine("DEPART");

            List<Task> runs = new List<Task>();

            for (int i = 0; i < names.Count; i++)
            {
                Runner runner = new Runner(names[i]);
                runs.Add(runner.Run());
            }

            await Task.WhenAll(runs);

            Console.WriteLine("ARRIVEE");
        }

        #endregion

        #region Question3

        static async Task<string> Runner2(string name)
        {
            Random rand = new Random(NameToInt(name) + (int)DateTime.Now.Ticks);
            int reflexTime = rand.Next(minReflexTime, maxReflexTime);
            await Task.Delay(reflexTime);
            Console.WriteLine("{0} démarre !", name);

            int runTime = rand.Next(minRunTime, maxRunTime);
            await Task.Delay(runTime);
            Console.WriteLine("{0} atteint la ligne d'arrivée en {1} secondes", name, runTime);

            return name;
        }

        static async Task<List<string>> RaceQ3()
        {
            Console.WriteLine("DEPART");

            List<string> podium = new List<string>();

            Task<string> runner1 = Runner2("Maradonna");
            Task<string> runner2 = Runner2("Evra");
            Task<string> runner3 = Runner2("Ronaldo");

            List<Task<string>> runners = new List<Task<string>> { runner1, runner2, runner3 };

            while (runners.Count > 0)
            {
                Task<string> finishedTask = await Task.WhenAny(runners);
                podium.Add(finishedTask.Result);
                runners.Remove(finishedTask);
            }

            Console.WriteLine("ARRIVEE");

            return podium;
        }

        static async Task RaceQ3AndPodium()
        {
            Task<List<string>> race = RaceQ3();

            await race;

            List<string> podium = race.Result;

            for (int i = 0; i < podium.Count; i++)
            {
                Console.WriteLine("{0} : {1}", i + 1, podium[i]);
            }
        }

        #endregion

        #region Question4

        static async Task CancellableRaceAndPodium_Q4()
        {
            Task cancel = Task.Run(() => RaceCancellation());

            Task<List<string>> race = Task.Run(() => RaceQ3(), RaceCancel.Token);

            List<Task> tasks = new List<Task> { race, cancel };

            Task finished = await Task.WhenAny(tasks);

            if (finished == cancel)
            {
                Console.WriteLine("race is cancelled.");
            }
            else
            {
                List<string> podium = race.Result;

                for (int i = 0; i < podium.Count; i++)
                {
                    Console.WriteLine("{0} : {1}", i + 1, podium[i]);
                }
            }
        }

        private static async Task RaceCancellation()
        {
            Console.WriteLine("Press the " + ConsoleKey.C + " key to cancel the course...");
            while (Console.ReadKey(true).Key != ConsoleKey.C)
            {
                //Console.WriteLine("Press the any key to cancel...");
            }

            Console.WriteLine("key pressed : COVID alarm.");
            RaceCancel.Cancel();
        }

        #endregion

        #region Question5

        static async Task<string> Runner3(string name, CancellationTokenSource ct, ConsoleKey ck)
        {
            //cancellation token source of cancel task
            CancellationTokenSource CancelLoop = new CancellationTokenSource();
            //cancel Task
            Task cancel = Task.Run(() => GenericCancel(ct, ck, name), CancelLoop.Token);
            //random creation
            Random rand = new Random(NameToInt(name) + (int)DateTime.Now.Ticks);
            int reflexTime = rand.Next(minReflexTime, maxReflexTime);
            //reflex task

            Task delay1 = Task.Run(() => Task.Delay(reflexTime), ct.Token);

            List<Task> tasks = new List<Task> { cancel, delay1};

            //detecting which task finish first
            Task finished =  await Task.WhenAny(tasks);
            if (finished == delay1)
            {
                Console.WriteLine("{0} démarre !", name);

                int runTime = rand.Next(minRunTime, maxRunTime);
                Task delay2 = Task.Run(() => Task.Delay(runTime), ct.Token);

                tasks = new List<Task> { cancel, delay2 };

                Task finished2 = await Task.WhenAny(tasks);
                if (finished2 == delay2)
                {
                    CancelLoop.Cancel();
                    Console.WriteLine("{0} atteint la ligne d'arrivée en {1} secondes", name, (float) (runTime/1000.0f));
                    return name;
                }
                else
                {
                    Console.WriteLine("{0} abandonne la course", name);
                }
                
            }
            else
            {
                Console.WriteLine("{0} abandonne la course", name);
            }

            return null;
        }

        static async Task<List<string>> RaceWithCancellableRunners_Q5()
        {
            Console.WriteLine("DEPART");

            CancellationTokenSource Cancel1 = new CancellationTokenSource();
            CancellationTokenSource Cancel2 = new CancellationTokenSource();
            CancellationTokenSource Cancel3 = new CancellationTokenSource();

            List<string> podium = new List<string>();

            Task<string> runner1 = Task.Run(() => Runner3("Maradonna", Cancel1, ConsoleKey.A), Cancel1.Token);
            Task<string> runner2 = Task.Run(() => Runner3("Evra", Cancel2, ConsoleKey.Z), Cancel2.Token);
            Task<string> runner3 = Task.Run(() => Runner3("Ronaldo", Cancel3, ConsoleKey.E), Cancel3.Token);

            List<Task<string>> runners = new List<Task<string>> { runner1, runner2, runner3 };

            while (runners.Count > 0)
            {
                Task<string> finishedTask = await Task.WhenAny(runners);
                if (!string.IsNullOrEmpty(finishedTask.Result))
                {
                    podium.Add(finishedTask.Result);
                }
                runners.Remove(finishedTask);
            }

            

            Console.WriteLine("ARRIVEE");

            for (int i = 0; i < podium.Count; i++)
            {
                Console.WriteLine("{0} : {1}", i + 1, podium[i]);
            }

            return podium;
        }

        private static async Task GenericCancel(CancellationTokenSource ct, ConsoleKey key, string name)
        {
            Console.WriteLine("Press the " + key.ToString() + " key to cancel the course...");
            while (Console.ReadKey(true).Key != key)
            {
                //Console.WriteLine("Press the any key to cancel...");
            }

            Console.WriteLine("\nkey "+ key.ToString() +" pressed : Cancellation start for "+name);
            ct.Cancel();
        }
        #endregion
    }
}
