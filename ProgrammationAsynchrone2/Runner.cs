using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammationAsynchrone2
{
    class Runner
    {
        int MinReflexTime = 200;
        int MaxReflexTime = 1001;
        int MinRunTime = 200;
        int MaxRunTime = 1001;
        public string Name;
        Random rand;

        public Runner(string name)
        {
            Name = name;
            rand = new Random(NameToInt()+(int)DateTime.Now.Ticks);
        }

        int GetReflexTime()
        {
            return rand.Next(MinReflexTime,MaxReflexTime);
        }

        int GetRunTime()
        {
            return rand.Next(MinRunTime, MaxRunTime);
        }

        public async Task Run()
        {
            await Task.Delay(GetReflexTime());
            Console.WriteLine("{0} démarre !", Name);
            await Task.Delay(GetRunTime());
            Console.WriteLine("{0} atteint la ligne d'arrivée !", Name);
        }

        public async Task<string> Run2()
        {
            await Task.Delay(GetReflexTime());
            Console.WriteLine("{0} démarre !", Name);
            await Task.Delay(GetRunTime());
            Console.WriteLine("{0} atteint la ligne d'arrivée !", Name);
            return Name;
        }

        int NameToInt()
        {
            int result = 0;
            foreach (char c in Name)
            {
                result += c;
            }

            return result;
        }
    }
}
