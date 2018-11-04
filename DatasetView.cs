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
    public partial class DatasetView : Form
    {
        public AttributeVariable[] attributes;
        public AttributeVariable target_attribute;
        public Item[] instances;
        public Dictionary<string, Dictionary<double, string>> codifier;
        public DatasetView(Form1 f1)
        {
            attributes = f1.attributes;
            target_attribute = f1.target_attribute;
            instances = f1.instances;
            codifier = f1.codifier;

            InitializeComponent();
        }

        private void DatasetView_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < attributes.Length; ++i)
            {
                dataGridView1.Columns.Add("attribute" + i, attributes[i].Name);
                dataGridView1.Columns[i].Width = (dataGridView1.Width - 17) / (attributes.Length + 1);
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dataGridView1.Columns.Add("attribute" + attributes.Length, target_attribute.Name);
            dataGridView1.Columns[attributes.Length].Width = (dataGridView1.Width - 17) / (attributes.Length + 1);
            dataGridView1.Columns[attributes.Length].SortMode = DataGridViewColumnSortMode.NotSortable;

            foreach (var item in instances)
            {
                string[] row = new string[item.Values.Length];

                for (int i = 0; i < item.Values.Length; ++i)
                    row[i] = codifier[item.Values[i].Attribute.Name][item.Values[i].Value];

                dataGridView1.Rows.Add(row);
            }
        }
    }
}
