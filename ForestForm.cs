using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Decision_Tree;

namespace RandomForest
{
    public partial class ForestForm : Form
    {
        public Dictionary<string, Dictionary<double, string>> codifier;
        public DecisionForest forest;
        public AttributeVariable[] attributes;
        public AttributeVariable target_attribute;
        public ForestForm(Form1 f1)
        {
            codifier = f1.codifier;
            forest = f1.forest;
            attributes = f1.attributes;
            target_attribute = f1.target_attribute;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new ForestView(this).Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new DesicionForm(this).Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Files|*.dat";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                BinaryFormatter formatter = new BinaryFormatter();

                using (FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile())
                {
                    ExternalForest extForest = new ExternalForest(forest, codifier, attributes, target_attribute);
                    formatter.Serialize(fs, extForest);
                }

                MessageBox.Show("Forest saved!");
            }
        }
    }
}
