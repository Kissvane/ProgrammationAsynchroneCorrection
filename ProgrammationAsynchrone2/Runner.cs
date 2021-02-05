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

        public async Task Run()
        {
            await Task.Delay(GetReflexTime());
            Console.WriteLine("{0} démarre !", Name);
            await Task.Delay(8000);
            Console.WriteLine("{0} atteint la ligne d'arrivée !", Name);
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
