using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLDifferenceCalculator
{
    internal class Program
    {
        class SuccessTracker {
            public List<List<float>> History = new List<List<float>>();

            public bool CheckForRepetition(List<float> NewSuccess) {
                bool repeated = false;
                NewSuccess.Sort();
                for (int i = 0; i < History.Count; i++) {
                    repeated = Enumerable.SequenceEqual<float>(History[i], NewSuccess);
                    if (repeated) {
                        break;
                    }
                }
                if (!repeated)
                {
                    History.Add(NewSuccess);
                }
                return repeated;
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("What is the difference in your GL?");
            float Difference = float.Parse(Console.ReadLine());

            Console.WriteLine("What are the all entry amounts (space or comma delimited) for the side of the GL that is greater?");
            List<float> Entries = Console.ReadLine().Split(' ', ',').OrderByDescending(c => c).Select(n => float.Parse(n)).ToList();
            for (int i = 0; i < Entries.Count; i++)
            {
                if (Entries[i] > Difference)
                {
                    Entries = Entries.GetRange(i, Entries.Count - 1);
                }
            }
            Console.WriteLine("Here are the possibilities that are making the difference in your GL:");
            RecurseEntries(Difference, Entries, new SuccessTracker());
            Console.WriteLine("Press Enter to close...");
            Console.ReadLine();
        }

        static void RecurseEntries(float Difference, List<float> Entries, SuccessTracker Successes, string Possibility = "", float Sum = 0)
        {
            if (Sum == Difference) {
                List<float> NewSuccess = Possibility.Split('+').Select(n => float.Parse(n)).ToList();
                bool repeated = Successes.CheckForRepetition(NewSuccess);
                if (!repeated)
                {
                    Console.WriteLine(Possibility + "= " + Difference);
                }
            } else if(Sum < Difference) {
                if (Possibility != "")
                {
                    Possibility = Possibility + "+ ";
                }
                for(int i = 0; i < Entries.Count; i++)
                {
                    float Entry = Entries[i];
                    List<float> SubEntries = new List<float>(Entries);
                    SubEntries.RemoveAt(i);
                    RecurseEntries(Difference, SubEntries, Successes, Possibility + Entry + " ", Sum + Entry);
                }
            }
        }
    }
}
