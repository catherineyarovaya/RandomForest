using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decision_Tree
{
    public static class Analysis
    {
        public static double Precision(Item[] instances, DecisionForest forest, AttributeVariable target_attribute)
        {
            int tp = 0, fp = 0;

            foreach (var item in instances)
            {
                int true_value = (int)item.FindAttributeValue(target_attribute);
                int predict_value = (int)Math.Round(forest.Decide(item));

                if (predict_value == 1)
                {
                    if (true_value == 1)
                        ++tp;
                    else
                        ++fp;
                }
            }

            return (double)tp / (double)(tp + fp);
        }

        public static double Recall(Item[] instances, DecisionForest forest, AttributeVariable target_attribute)
        {
            int tp = 0, fn = 0;

            foreach (var item in instances)
            {
                int true_value = (int)item.FindAttributeValue(target_attribute);
                int predict_value = (int)Math.Round(forest.Decide(item));

                if (true_value == 1)
                {
                    if (predict_value == 1)
                        ++tp;
                    else
                        ++fn;
                }
            }

            return (double)tp / (double)(tp + fn);
        }

        public static double FMeasure(Item[] instances, DecisionForest forest, AttributeVariable target_attribute, double B = 1)
        {
            double precision = Precision(instances, forest, target_attribute);
            double recall = Recall(instances, forest, target_attribute);

            return (B * B + 1) * precision * recall / (B * B * precision + recall);

        }
    }
}
