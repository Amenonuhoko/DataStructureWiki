using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataStructureWiki
{
    public partial class DataStructureWiki : Form
    {
        public DataStructureWiki()
        {
            InitializeComponent();
        }

        #region Variable
        static int row = 12;
        static int col = 4;
        string[,] DataTable = new string[row, col];
        #endregion


        #region Event 

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            Add();
        }
        private void BtnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        // TODO
        private void dataListView_Click(object sender, EventArgs e)
        {
            var index = dataListView.SelectedIndices[0];

            nameTextBox.Text = DataTable[index, 0];
            categoryTextBox.Text = DataTable[index, 1];
            structureTextBox.Text = DataTable[index, 2];
            definitionTextBox.Text = DataTable[index, 3];
        }

        #endregion


        #region Method

        private void DisplayData()
        {
            dataListView.Items.Clear();

            for (int i = 0; i < row; i++)
            {
                ListViewItem data1 = new ListViewItem(DataTable[i, 0]);
                data1.SubItems.Add(DataTable[i, 1]);
                data1.SubItems.Add(DataTable[i, 2]);
                data1.SubItems.Add(DataTable[i, 3]);

                dataListView.Items.Add(data1);
            }

        }

        // TODO
        private void Add()
        {
            DataTable[0,0] = nameTextBox.Text;
            DataTable[0,1] = categoryTextBox.Text;
            DataTable[0,2] = structureTextBox.Text;
            DataTable[0,3] = definitionTextBox.Text;

            DisplayData();
        }

        private void LoadData()
        {

        }

        #endregion


    }
}