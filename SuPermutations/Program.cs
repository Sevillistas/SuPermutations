using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuPermutations
{
    class Program
    {
        private static readonly int[] Fact = {0, 1, 2, 6, 24, 120, 720, 5040, 40320};

        private static string[] superpermutations = {"1", "121", "", "", "", "", ""};

        static void Main(string[] args)
        {
            Console.Write("Enter n (<8): ");
            int n = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Creating permutations tree...");
            PNode tree = new PNode(n);
            Console.WriteLine("Permutations tree created.");

            Console.WriteLine(FindSuperpermutation(n));

            //ParallelLoopResult plResult = Parallel.For(0, n, (l, state) =>
            //{
            //    Console.WriteLine($"Iteration number {Fact[l]}");
            //});
        }

        private static string FindSuperpermutation(int n)
        {
            if (superpermutations[n - 1].Equals(""))
            {
                string prevPerm = FindSuperpermutation(n - 1);
                superpermutations[n - 1] = BuildPermFromPrev(prevPerm, n - 1);
            }

            return superpermutations[n - 1];
        }

        private static string BuildPermFromPrev(string prevPerm, int prevN)
        {
            for (int i = 0; i < Fact[prevN]; i++)
            {
                string prefix = prevPerm.Substring(i, prevN);
                //TODO: build new superpermutation by algorithm from paper.
                //TODO: make it parallel 
            }
            return "";
        }
    }
}