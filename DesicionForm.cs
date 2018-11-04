using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Decision_Tree;

namespace RandomForest
{
    public partial class DesicionForm : Form
    {
        Dictionary<string, Dictionary<double, string>> codifier;
        DecisionForest forest;
        AttributeVariable[] attributes;
        AttributeVariable target_attribute;

        public DesicionForm(ForestForm f)
        {
            forest = f.forest;
            codifier = f.codifier;
            attributes = f.attributes;
            target_attribute = f.target_attribute;

            InitializeComponent();
        }

        private void DesicionForm_Load(object sender, EventArgs e)
        {
            if (attributes.Length < 5)
            {
                dataGridView1.Columns[0].Width = 133;
                dataGridView1.Columns[1].Width = 134;
            }

            foreach(var attribute in attributes)
            {
                Dictionary<double, string> item = codifier[attribute.Name];
                DataGridViewRow row = new DataGridViewRow();
                DataGridViewCell cell = new DataGridViewTextBoxCell();
                cell.Value = attribute.Name;

                row.Cells.Add(cell);

                if (attribute.Nature == DecisionVariableKind.Discrete)
                {
                    DataGridViewComboBoxCell comboBox = new DataGridViewComboBoxCell();
                    foreach (var value in item)
                        comboBox.Items.Add(value.Value);

                    row.Cells.Add(comboBox);
                }
                else
                    row.Cells.Add(new DataGridViewTextBoxCell());

                dataGridView1.Rows.Add(row);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AttributeValue[] item_values = new AttributeValue[dataGridView1.Rows.Count];

            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                if (dataGridView1.Rows[i].Cells[1].Value == null)
                {
                    MessageBox.Show("Fill in all the fields!");
                    return;
                }

                double value;
                if (attributes[i].Nature == DecisionVariableKind.Continuous)
                    value = Convert.ToDouble(dataGridView1.Rows[i].Cells[1].Value);
                else
                    value = codifier[attributes[i].Name].FirstOrDefault(x => x.Value == Convert.ToString(dataGridView1.Rows[i].Cells[1].Value)).Key;
                item_values[i] = new AttributeValue(attributes[i], value);
            }

            Item item = new Item(item_values);
            double decision = forest.Decide(item) * 100;

            answer.Text = "'" + codifier.First().Value[0] + "' " + Convert.ToString(100 - decision) + "%\n";
            answer.Text += "'" + codifier.First().Value[1] + "' " + Convert.ToString(decision) + '%';
        }
    }
}
