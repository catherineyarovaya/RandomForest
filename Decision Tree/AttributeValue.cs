using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decision_Tree
{
    /// <summary>
    /// Значение атрибута
    /// </summary>
    [Serializable]
    public class AttributeValue
    {
        public AttributeVariable Attribute { get; set; }
        public double Value { get; set; }

        public AttributeValue(AttributeVariable attribute, double value)
        {
            this.Attribute = attribute;
            this.Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
    /// <summary>
    /// Элемент классификации
    /// </summary>
    [Serializable]
    public class Item
    {
        public AttributeValue[] Values;
        public Item(AttributeValue[] values)
        {
            this.Values = values;
        }
        /// <summary>
        /// Находит значение заданного атрибута
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public double? FindAttributeValue(AttributeVariable attribute)
        {
            for (int i = 0; i < Values.Count(); ++i)
            {
                if (Values[i].Attribute.Name == attribute.Name)
                    return Values[i].Value;
            }

            return null;
        }
    }
}
