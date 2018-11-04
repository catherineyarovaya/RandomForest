using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decision_Tree
{
    /// <summary>
    /// Дерево принятий решений
    /// </summary>
    [Serializable]
    public class DecisionTree
    {
        /// <summary>
        /// Корень дерева
        /// </summary>
        public DecisionNode Root { get; set; }

        public DecisionTree(DecisionNode root)
        {
            this.Root = root;
        }

        /// <summary>
        /// Найти решение
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Decide(Item item)
        {
            DecisionNode current_node = Root;
            while (current_node.Decision == null)
            {
                for (int i = 0; i < item.Values.Count(); ++i)
                {
                    if (current_node.Attribute == item.Values[i].Attribute)
                    {
                        current_node = current_node.Compute(item.Values[i].Value);
                        break;
                    }
                }
            }

            return (int)current_node.Decision;
        }
    }
}
