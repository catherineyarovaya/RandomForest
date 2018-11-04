using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decision_Tree
{
    public static class Bootstrap
    {
        private static Random rnd = new Random();
        public static Item[] Aggregate (Item[] instances)
        {
            int N = instances.Length;
            Item[] result = new Item[N];

            for (int i = 0; i < N; ++i)
                    result[i] = instances[rnd.Next(N)];

            return result;
        }
    }
}
