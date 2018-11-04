using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decision_Tree
{
    /// <summary>
    /// Вид сравнения, который нужно произвести с узлом-потомком
    /// </summary>
    public enum ComparisonKind
    {
        None,
        Equal,
        NotEqual,
        GreaterThanOrEqual,
        GreaterThan,
        LessThan,
        LessThanOrEqual
    }
    public static class ComparisonExtensions
    {
        public static bool Compare(ComparisonKind comparison, double x, double y)
        {
            switch (comparison)
            {
                case ComparisonKind.Equal:
                    return x == y;
                case ComparisonKind.GreaterThan:
                    return x > y;
                case ComparisonKind.GreaterThanOrEqual:
                    return x >= y;
                case ComparisonKind.LessThan:
                    return x < y;
                case ComparisonKind.LessThanOrEqual:
                    return x <= y;
                case ComparisonKind.NotEqual:
                    return x != y;
                case ComparisonKind.None:
                    throw new InvalidOperationException("Node comparison type not specified");
                default:
                    throw new InvalidOperationException("Unexpected node comparison type");
            }
        }
        public static string ToString(ComparisonKind comparison)
        {
            switch (comparison)
            {
                case ComparisonKind.Equal:
                    return "=";

                case ComparisonKind.GreaterThan:
                    return ">";

                case ComparisonKind.GreaterThanOrEqual:
                    return ">=";

                case ComparisonKind.LessThan:
                    return "<";

                case ComparisonKind.LessThanOrEqual:
                    return "<=";

                case ComparisonKind.NotEqual:
                    return "!=";

                case ComparisonKind.None:
                    return "";

                default:
                    throw new InvalidOperationException("Unexpected node comparison type.");
            }
        }
    }

    /// <summary>
    /// Узел дерева принятия решений
    /// </summary>
    [Serializable]
    public class DecisionNode
    {
        /// <summary>
        /// Узел-родитель. Если узел - корень, то null
        /// </summary>
        public double? Value { get; set; }
        /// <summary>
        /// Атрибут, соответсвующий узлу
        /// </summary>
        public AttributeVariable Attribute { get; set; }
        /// <summary>
        /// Вид сравнения, который нужно произвести с узлом-потомком
        /// </summary>
        public ComparisonKind Comparison { get; set; }
        /// <summary>
        /// Решение, которое должно быть принято. Если узел не лист - null
        /// </summary>
        public int? Decision { get; set; }
        /// <summary>
        /// Узлы-потомки
        /// </summary>
        public DecisionNode[] Branches { get; set; }

        /// <summary>
        /// Создание корня
        /// </summary>
        /// <param name="comparison"></param>
        public DecisionNode(AttributeVariable attribute)
        {
            this.Value = null;
            this.Comparison = ComparisonKind.None;
            this.Attribute = attribute;
            this.Decision = null;
        }
        /// <summary>
        /// создание узла
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="value"></param>
        /// <param name="comparison"></param>
        public DecisionNode(ComparisonKind comparison, double value, AttributeVariable attribute)
        {
            this.Value = value;
            this.Comparison = comparison;
            this.Attribute = attribute;
            this.Decision = null;
        }
        /// <summary>
        /// Создание листа
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="value"></param>
        /// <param name="decision"></param>
        public DecisionNode(ComparisonKind comparison, double value, int decision)
        {
            this.Value = value;
            this.Comparison = comparison;
            this.Attribute = null;
            this.Decision = decision;
        }
        /// <summary>
        /// Создание корня, являющимя решением
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decision"></param>
        public DecisionNode(int decision)
        {
            this.Value = null;
            this.Comparison = ComparisonKind.None;
            this.Attribute = null;
            this.Decision = decision;
        }

        /// <summary>
        /// Выбор ветки по заданному значению
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public DecisionNode Compute(double x)
        {
            DecisionNode current_node;
            for (int i = 0; i < Branches.Count(); ++i)
            {
                current_node = Branches[i];
                if (ComparisonExtensions.Compare(current_node.Comparison, x, (double)current_node.Value))
                    return current_node;
            }
            throw new InvalidOperationException();
        }

        public override string ToString()
        {
            return Attribute.Name;
        }
    }
}
