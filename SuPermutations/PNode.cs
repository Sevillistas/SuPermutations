using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuPermutations
{
    class PNode
    {
        public string Permutation { get; set; }

        public int Level { get; set; }

        public PNode[] Descendants { get; set; }

        public PNode(int n)
        {
            Level = 0;
            Permutation = "";
            Descendants = new PNode[n];
            for (int i = 0; i < n; i++)
            {
                Descendants[i] = new PNode(n, this, (i + 1).ToString());
            }
        }

        private PNode(int n, PNode ancestor, string symbol)
        {
            Level = ancestor.Level + 1;
            Permutation = ancestor.Permutation + symbol; 
            if (Level < 8)
            {
                Descendants = new PNode[n];
                for (int i = 0; i < n; i++)
                {
                    Descendants[i] = new PNode(n, this, (i + 1).ToString());
                }
            }
        }

        public PNode FindByPermutation(string permutation)
        {
            if (Permutation.Equals(permutation))
            {
                return this;
            }
            string prefix = permutation.Substring(0, Level + 1);
            foreach (PNode descendant in Descendants)
            {
                if (descendant.Permutation.Substring(0, Level + 1).Equals(prefix))
                {
                    return descendant.FindByPermutation(permutation);
                }
            }

            return new PNode(0);
        }
    }
}
