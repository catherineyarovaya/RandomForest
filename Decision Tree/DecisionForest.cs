using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decision_Tree
{
    [Serializable]
    public class DecisionForest
    {
        public DecisionTree[] Trees { get; }

        /// <summary>
        /// Построить ансамбль решающих деревьев
        /// </summary>
        /// <param name="instances"></param>
        /// <param name="target_attribute"></param>
        /// <param name="attributes"></param>
        /// <param name="N"> количество деревье </param>
        /// <returns></returns>
        public DecisionForest(Item[] instances, AttributeVariable target_attribute, AttributeVariable[] attributes, int N)
        {
            Trees = GrowTrees(instances, target_attribute, attributes, N);
        }

        private static DecisionTree[] GrowTrees(Item[] instances, AttributeVariable target_attribute, AttributeVariable[] attributes, int N)
        {
            Item[] cur_sample;
            DecisionTree[] forest = new DecisionTree[N];

            for (int i = 0; i < N; ++i)
            {
                // сгенерируем выборку с помощью бутстрэпа
                cur_sample = Bootstrap.Aggregate(instances);

                // построим решающее дерево по этой выборке
                forest[i] = C45algorithm.CreateTree(instances, target_attribute, attributes);
            }

            return forest;
        }

        public double Decide(Item item)
        {
            int decision = 0;

            foreach (DecisionTree tree in Trees)
                decision += tree.Decide(item);

            return (double)decision / (double)Trees.Length;
        }
    }
}
