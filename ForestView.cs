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
    public partial class ForestView : Form
    {
        Dictionary<string, Dictionary<double, string>> codifier;
        DecisionForest forest;
        int index;
        void BuildTree(DecisionTree tree)
        {
            treeView1.Nodes.Add(BuildTreeNode(tree.Root, String.Empty));
        }
        TreeNode BuildTreeNode(DecisionNode node, string attribute_name)
        {
            if (node.Decision != null)
                return new TreeNode(ComparisonExtensions.ToString(node.Comparison) + " " + codifier[attribute_name][Convert.ToDouble(node.Value)] + " THEN " + codifier.First().Value[Convert.ToDouble(node.Decision)]);

            List<TreeNode> child_nodes = new List<TreeNode>();

            for (int i = 0; i < node.Branches.Count(); ++i)
                child_nodes.Add(BuildTreeNode(node.Branches[i], node.Attribute.Name));

            if (node.Value != null)
                return new TreeNode(ComparisonExtensions.ToString(node.Comparison) + " " + codifier[attribute_name][Convert.ToDouble(node.Value)] + " THEN IF " + node.Attribute.Name, child_nodes.ToArray());
            else
                return new TreeNode("IF " + node.Attribute.Name, child_nodes.ToArray());
        }
        public ForestView(ForestForm f)
        {
            forest = f.forest;
            codifier = f.codifier;
            InitializeComponent();
        }

        private void ForestView_Load(object sender, EventArgs e)
        {
            index = 0;
            BuildTree(forest.Trees[index]);
            label1.Text = "Tree №" + index;
            treeView1.ExpandAll();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            ++index;
            if (index == forest.Trees.Length)
                index = 0;
            BuildTree(forest.Trees[index]);
            label1.Text = "Tree №" + index;
            treeView1.ExpandAll();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            --index;
            if (index == -1)
                index = forest.Trees.Length - 1;
            BuildTree(forest.Trees[index]);
            label1.Text = "Tree №" + index;
            treeView1.ExpandAll();
        }
    }
}
