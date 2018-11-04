using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decision_Tree
{
    /// <summary>
    /// Вид атрибута
    /// </summary>
    public enum DecisionVariableKind
    {
        /// <summary>
        /// дискретный
        /// </summary>
        Discrete,
        /// <summary>
        /// непрерывный
        /// </summary>
        Continuous
    }

    /// <summary>
    /// Атрибут
    /// </summary>
    [Serializable]
    public class AttributeVariable
    {
        public string Name { get; set; }
        public DecisionVariableKind Nature { get; set; }
        public double[] Range { get; set; }

        /// <summary>
        /// Атрибут с непрерывными значениями в заданном интервале
        /// </summary>
        /// <param name="name"></param>
        /// <param name="range"></param>
        public AttributeVariable(string name, double[] range)
        {
            this.Name = name;
            this.Nature = DecisionVariableKind.Continuous;
            this.Range = range;
        }

        /// <summary>
        /// Атрибут с заданным видом, значение по умолчанию [0,1]
        /// </summary>
        /// <param name="name"></param>
        /// <param name="nature"></param>
        public AttributeVariable(string name, DecisionVariableKind nature)
        {
            this.Name = name;
            this.Nature = nature;
            this.Range = new double[] { 0, 1 };
        }

        /// <summary>
        /// Атрибут с дискретными значениями с заданным количеством значений
        /// </summary>
        /// <param name="name"></param>
        /// <param name="symbols"></param>
        public AttributeVariable(string name, int symbols)
        {
            this.Name = name;
            this.Nature = DecisionVariableKind.Discrete;
            double[] _range = new double[symbols];
            for (int i = 0; i < symbols; ++i)
                _range[i] = i;
            this.Range = _range;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
