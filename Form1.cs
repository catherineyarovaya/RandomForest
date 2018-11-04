using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Decision_Tree;
using System.Runtime.Serialization.Formatters.Binary;

namespace RandomForest
{
    public partial class Form1 : Form
    {
        public AttributeVariable[] attributes;
        public AttributeVariable target_attribute;
        public Item[] instances;
        public Dictionary<string, Dictionary<double, string>> codifier;    // имя атрибута, номер-значение
        public DecisionForest forest;
        public Form1()
        {
            InitializeComponent();
        }

        void DataLoad(string file_text)
        {
            codifier = new Dictionary<string, Dictionary<double, string>>();

            string[] lines = file_text.Split(new string[] { "\n", "\r", "\n\r" }, StringSplitOptions.RemoveEmptyEntries);
            string[] attributes_names = lines[0].Split(';');
            lines = lines.Skip(1).ToArray();
            attributes = new AttributeVariable[attributes_names.Length - 1];
            List<Item> instances_buf = new List<Item>();

            target_attribute = new AttributeVariable(attributes_names.Last(), 2);
            codifier.Add(attributes_names.Last(), new Dictionary<double, string>());

            for (int i = 0; i < attributes.Length; ++i)
            {
                attributes[i] = new AttributeVariable(attributes_names[i], DecisionVariableKind.Continuous);
                codifier.Add(attributes_names[i], new Dictionary<double, string>());
            }

            foreach (string line in lines)
            {
                string[] values = line.Split(';');
                AttributeValue[] item_values = new AttributeValue[attributes.Length + 1];

                for (int i = 0; i < attributes.Length; ++i)
                {
                    double num;
                    // если значения дискретные
                    if (attributes[i].Nature == DecisionVariableKind.Continuous && !double.TryParse(values[i], out num))
                        attributes[i] = new AttributeVariable(attributes[i].Name, DecisionVariableKind.Discrete);
                    Dictionary<double, string> cur_dict = codifier[attributes[i].Name];

                    if (attributes[i].Nature == DecisionVariableKind.Discrete)
                    {
                        if (!cur_dict.ContainsValue(values[i]))
                            cur_dict.Add(cur_dict.Count, values[i]);
                    }
                    else
                    {
                        if (!cur_dict.ContainsValue(values[i]))
                            cur_dict.Add(Convert.ToDouble(values[i]), values[i]);
                    }

                    item_values[i] = new AttributeValue(attributes[i], cur_dict.FirstOrDefault(x => x.Value == values[i]).Key);
                }

                if (!codifier[target_attribute.Name].ContainsValue(values.Last()))
                    codifier[target_attribute.Name].Add(codifier[target_attribute.Name].Count, values.Last());
                item_values[attributes.Length] = new AttributeValue(target_attribute, codifier[target_attribute.Name].FirstOrDefault(x => x.Value == values.Last()).Key);

                instances_buf.Add(new Item(item_values));
            }

            for (int i = 0; i < attributes.Length; ++i)
            {
                if (attributes[i].Nature == DecisionVariableKind.Discrete)
                    attributes[i] = new AttributeVariable(attributes[i].Name, codifier[attributes[i].Name].Count);
                else
                {
                    List<double> range = new List<double>();
                    foreach (var elem in codifier[attributes[i].Name])
                        if (!range.Contains(elem.Key))
                            range.Add(elem.Key);

                    attributes[i] = new AttributeVariable(attributes[i].Name, range.ToArray());
                }

                for (int j = 0; j < instances_buf.Count; ++j)
                    instances_buf[j].Values[i].Attribute = attributes[i];
            }
            instances = instances_buf.ToArray();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            label2.Enabled = false;
            numericUpDown1.Enabled = false;
            button3.Enabled = false;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Files|*.csv";
            DialogResult result = openFileDialog1.ShowDialog();
            string file_text;

            if (result == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                this.Enabled = false;

                string file = openFileDialog1.FileName;
                using (var reader = new StreamReader(file))
                {
                    file_text = reader.ReadToEnd();
                    reader.Close();
                }

                DataLoad(file_text);

                this.Enabled = true;
                this.Cursor = Cursors.Default;

                button2.Enabled = true;
                label2.Enabled = true;
                numericUpDown1.Enabled = true;
                button3.Enabled = true;

                MessageBox.Show("Data loaded successfully");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new DatasetView(this).Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.Enabled = false;

            forest = new DecisionForest(instances, target_attribute, attributes, Convert.ToInt32(numericUpDown1.Text));

            this.Enabled = true;
            this.Cursor = Cursors.Default;
            ForestForm forest_form = new ForestForm(this);
            forest_form.Show();
            forest_form.label1.Text = "The forest has been built!";
            forest_form.label1.Text += "\nF-Measure is " + Math.Round(Analysis.FMeasure(instances, forest, target_attribute), 2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            label2.Enabled = false;
            numericUpDown1.Enabled = false;
            button3.Enabled = false;

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Files|*.dat";
            DialogResult result = openFileDialog1.ShowDialog();
            BinaryFormatter formatter = new BinaryFormatter();

            if (result == DialogResult.OK)
            {
                using (FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open))
                {
                    ExternalForest extForest = (ExternalForest)formatter.Deserialize(fs);
                    forest = extForest.forest;
                    codifier = extForest.codifier;
                    attributes = extForest.attributes;
                    target_attribute = extForest.target_attribute;
                }

                ForestForm forest_form = new ForestForm(this);
                forest_form.Show();
                forest_form.label1.Text = "The forest loaded!";
            }
        }
    }

    [Serializable]
    public class ExternalForest
    {
        public DecisionForest forest;
        public Dictionary<string, Dictionary<double, string>> codifier;
        public AttributeVariable[] attributes;
        public AttributeVariable target_attribute;

        public ExternalForest(
            DecisionForest _forest, 
            Dictionary<string, Dictionary<double, string>> _codifier,
            AttributeVariable[] _attributes,
            AttributeVariable _target_attribute )
        {
            forest = _forest;
            codifier = _codifier;
            attributes = _attributes;
            target_attribute = _target_attribute;
        }
    }
}
