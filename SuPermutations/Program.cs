using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;

namespace SuPermutations
{
    class Program
    {
        private static readonly int[] Fact = {0, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880, 3628800};

        private static string[] superpermutations = {"1", "121", "123121321", "", "", "", "", "", "", ""};

        private static PNode pTree;

        static void Main(string[] args)
        {
            Console.Write("Enter n (<8): ");
            int n = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Creating permutations tree...");
            pTree = new PNode(n);
            Console.WriteLine("Permutations tree created.");

            string result = FindSuperpermutation(n);

            Console.WriteLine("{0}: " + result, result.Length);
            using (FileStream fs = new FileStream("out.txt", FileMode.Create))
            {
                StreamWriter sw = new StreamWriter(fs);
                sw.Write("{0}: " + result, result.Length);
                sw.Flush();
            }

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

        //TODO: make it parallel 
        private static string BuildPermFromPrev(string prevPerm, int prevN)
        {
            int iterations = prevPerm.Length - prevN + 1;
            //Extract all permutations from (n-1)-supepermutation saving its order.
            List<string> perms = new List<string>();

            Stopwatch timer = new Stopwatch();
            timer.Start();

            for (int i = 0; i < iterations; i++)
            {
                string prefix = prevPerm.Substring(i, prevN);
                if (pTree.FindByPermutation(prefix).Level == prevN)
                {
                    perms.Add(prefix);
                }
            }
            timer.Stop();
            Console.WriteLine("Permutations found: {0} from {1}.", perms.Count, Fact[prevN]);
            Console.WriteLine("Time consumed: {0}", timer.Elapsed);

            //make ROL
            timer.Restart();
            StringBuilder expPerms = new StringBuilder();
            foreach (string perm in perms)
            {
                //Computed analitically
                string newShiftedClipedPerm = perm + (prevN + 1) + perm;
                expPerms.Append(newShiftedClipedPerm);
            }
            timer.Stop();
            Console.WriteLine("Time consumed on ROL: {0}", timer.Elapsed);

            timer.Restart();

            string result;
            if (prevN > 4)
            {
                string expPermsStr = expPerms.ToString();
                int expPermsStrLen = expPermsStr.Length;
                int parallelizm = 4;
                List<string> parts = new List<string>();
                for (int i = 0; i < parallelizm; i++)
                    parts.Add(expPermsStr.Substring(i * expPermsStrLen / parallelizm, expPermsStrLen / parallelizm));
                var partsRes = parts.AsParallel().Select(x => EliminateOverlaps(x, prevN));
                string strRes = "";
                foreach (string part in partsRes)
                {
                    strRes += part;
                }
                result = EliminateOverlaps(strRes, prevN);
            }
            else
            {
                result = EliminateOverlaps(expPerms.ToString(), prevN);
            }
            timer.Stop();
            Console.WriteLine("Time consumed on eliminating overlaps: {0}", timer.Elapsed);

            return result;
        }

        private static string EliminateOverlaps(string source, int frameLength)
        {
            string trimmed = source;
            for (int i = frameLength; i < trimmed.Length; i++)
            {
                for (int j = 1; j <= frameLength; j++)
                {
                    if (trimmed[i].Equals(trimmed[i - j]))
                    {
                        if (trimmed.Substring(i - j, j).Equals(trimmed.Substring(i, j)))
                        {
                            trimmed = trimmed.Remove(i - j, j);
                            break;
                        }
                    }
                }
            }
            return trimmed;
        }
    }
}