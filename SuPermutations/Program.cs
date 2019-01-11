using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuPermutations
{
    class Program
    {
        private static readonly int[] Fact = {0, 1, 2, 6, 24, 120, 720, 5040, 40320};

        private static string[] superpermutations = {"1", "121", "", "", "", "", ""};

        private static PNode pTree;

        static void Main(string[] args)
        {
            Console.Write("Enter n (<8): ");
            int n = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Creating permutations tree...");
            pTree = new PNode(n);
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

        //TODO: build new superpermutation by algorithm from paper.
        //TODO: make it parallel 
        private static string BuildPermFromPrev(string prevPerm, int prevN)
        {
            int iterations = prevPerm.Length - prevN + 1;
            //Extract all permutations from (n-1)-supepermutation saving its order.
            List<string> perms = new List<string>();
            for (int i = 0; i < iterations; i++)
            {
                string prefix = prevPerm.Substring(i, prevN);
                if (pTree.FindByPermutation(prefix).Level == prevN)
                {
                    perms.Add(prefix);
                }
            }
            Console.WriteLine("Permutations found: {0} from {1}.", perms.Count, Fact[prevN]);

            StringBuilder expPerms = new StringBuilder();
            //make ROL
            foreach (string perm in perms)
            {
                //Computed analitically
                string newShiftedClipedPerm = perm + (prevN + 1) + perm;
                expPerms.Append(newShiftedClipedPerm);
            }
            string result = expPerms.ToString();
            Console.WriteLine(result);

            return result;
        }
    }
}