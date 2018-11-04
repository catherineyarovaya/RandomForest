using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decision_Tree
{
    public static class ID3algorithm
    {
        /// <summary>
        /// Построить дерево
        /// </summary>
        /// <param name="instances"> Примеры </param>
        /// <param name="target_attribute"> Атрибут, для которого нужно принять решение - да или нет </param>
        /// <param name="attributes"> Перечисление всех атрибутов </param>
        /// <returns></returns>
        public static DecisionTree CreateTree(Item[] instances, AttributeVariable target_attribute, AttributeVariable[] attributes)
        {
            return new DecisionTree(ID3(instances, target_attribute, attributes));
        }

        /// <summary>
        /// Вычисление энтропии заданного атрибута
        /// </summary>
        /// <param name="instances"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        static double Entrophy(Item[] instances, AttributeVariable target_attribute)
        {
            double[] range = target_attribute.Range;
            double entrophy = 0;

            foreach (double n in range)
            {
                int cnt = 0;
                for (int i = 0; i < instances.Count(); ++i)
                {
                    if (instances[i].FindAttributeValue(target_attribute) == n)
                        ++cnt;
                }

                double p = (double)cnt / (double)instances.Count();
                entrophy -= p * Math.Log(p, 2);
            }

            return entrophy;
        }
        /// <summary>
        /// Вычисление энтропии заданного атрибута, включая только определенные значения
        /// </summary>
        /// <param name="instances"></param>
        /// <param name="target_attribute"></param>
        /// <param name="attribute"></param>
        /// <param name="attribute_value"></param>
        /// <returns></returns>

        static double Entrophy(Item[] instances, AttributeVariable target_attribute, AttributeVariable attribute, double attribute_value)
        {
            double[] range = target_attribute.Range;
            double entrophy = 0;

            foreach(double n in range)
            {
                int cnt = 0, cnt_all = 0;
                for (int i = 0; i < instances.Count(); ++i)
                {
                    if (instances[i].FindAttributeValue(attribute) != attribute_value)
                        continue;
                    if (instances[i].FindAttributeValue(target_attribute) == n)
                        ++cnt;
                    ++cnt_all;
                }

                if (cnt != 0)
                {
                    double p = (double)cnt / (double)cnt_all;
                    entrophy -= p * Math.Log(p, 2);
                }
            }

            return entrophy;
        }

        static double Gain(Item[] instances, AttributeVariable target_attribute, AttributeVariable attribute)
        {
            double[] range = attribute.Range;
            double gain = Entrophy(instances, target_attribute);

            foreach(double n in range)
            {
                int cnt = 0;
                for (int i = 0; i < instances.Count(); ++i)
                    if (instances[i].FindAttributeValue(attribute) == n)
                        ++cnt;

                gain -= (double)cnt / (double)instances.Count() * Entrophy(instances, target_attribute, attribute, n);
            }

            return gain;
        }

        static DecisionNode ID3(Item[] instances, AttributeVariable target_attribute, AttributeVariable[] attributes)
        {
            int yes_cnt = 0, no_cnt = 0;
            for (int i = 0; i < instances.Count(); ++i)
            {
                double? value = instances[i].FindAttributeValue(target_attribute);
                if (value == null)
                    throw new Exception("Не инициализировано значение атрибута " + target_attribute + " в " + i + " примере");
                if (value == 0)
                    ++no_cnt;
                else
                    if (value == 1)
                    ++yes_cnt;
                else
                    throw new Exception("Значение атрибута " + target_attribute + " должно быть 0 или 1");
            }

            // если все значения target_attribute - ноль, дерево будет состоять из единственного узла, приволящим к решению 0
            if (no_cnt == instances.Count())
                return new DecisionNode(0);
            // аналогично, если все значения - 1
            if (yes_cnt == instances.Count())
                return new DecisionNode(1);

            // Если список атрибутов пуст - выводится узел, состоящий из наиболее встречающегося значения
            if (attributes.Count() == 0)
                if (yes_cnt > no_cnt)
                    return new DecisionNode(1);
                else
                    return new DecisionNode(0);

            // Ищем атрибут, который лучше всего классифицирует примеры
            double max_gain = 0, current_gain;
            AttributeVariable best_attribute = attributes[0];
            for (int i = 0; i < attributes.Count(); ++i)
            {
                current_gain = Gain(instances, target_attribute, attributes[i]);
                if (current_gain > max_gain)
                {
                    best_attribute = attributes[i];
                    max_gain = current_gain;
                }
            }

            List<double?> node_values = new List<double?>();

            foreach (double n in best_attribute.Range)
                node_values.Add(n);

            List<DecisionNode> Branches = new List<DecisionNode>();
            int k = 0;
            // для каждого значения атрибута добавляем ветку
            foreach (double n in best_attribute.Range)
            {
                List<Item> Examples = new List<Item>();
                // выделяем примеры, которые соответсвуют текущему значению атрибуты
                for (int i = 0; i < instances.Count(); ++i)
                    if (instances[i].FindAttributeValue(best_attribute) == n)
                        Examples.Add(instances[i]);
                // если не найдено примеров, добавляем лист с наиболее встречающимся значением
                if (Examples.Count() == 0)
                    if (yes_cnt > no_cnt)
                        Branches.Add(new DecisionNode(ComparisonKind.Equal, (double)node_values[k], 1));
                    else
                        Branches.Add(new DecisionNode(ComparisonKind.Equal, (double)node_values[k], 0));
                else
                {
                    List<AttributeVariable> new_attributes_list = attributes.ToList();
                    new_attributes_list.Remove(best_attribute);
                    Branches.Add(ID3(Examples.ToArray(), target_attribute, new_attributes_list.ToArray()));
                    Branches.Last().Value = node_values[k];
                    Branches.Last().Comparison = ComparisonKind.Equal;
                }
                ++k;
            }

            DecisionNode root = new DecisionNode(best_attribute);
            root.Branches = Branches.ToArray();
            return root;
        }
    }
}
