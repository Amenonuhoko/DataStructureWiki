using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        static string fileName = "definitions.dat";
        int ptr = 0;
        int index = -1;
        #endregion


        #region Event 

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            Add();
        }
        private void BtnDel_Click(object sender, EventArgs e)
        {
            Delete();
        }
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            Edit();
        }
        private void BtnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        private void dataListView_Click(object sender, EventArgs e)
        {
            ClickData();
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void BtnSort_Click(object sender, EventArgs e)
        {
            BubbleSort();
        }
        #endregion


        #region Method

        private void Add()
        {
            // Check if fields are empty
            if (String.IsNullOrEmpty(txtBoxName.Text) || String.IsNullOrEmpty(txtBoxCategory.Text) || String.IsNullOrEmpty(txtBoxDefinition.Text))
            {
                MessageBox.Show("Please Input all the required fields.");
            } else
            {
                // Make sure the pointer is in the correct place
                if (ptr < row)
                {
                    DataTable[ptr, 0] = txtBoxName.Text;
                    DataTable[ptr, 1] = txtBoxCategory.Text;
                    DataTable[ptr, 2] = radioButtonLinear.Checked == true ? "Linear" : "Non-Linear";
                    DataTable[ptr, 3] = txtBoxDefinition.Text;
                    ptr++;
                    toolStripStatusLabel1.Text = ptr.ToString();

                } // Check if there are more than 12
                else 
                {
                    MessageBox.Show("Too many entries");
                }
                // Clear everything
                ClearFields();
                // Focus
                txtBoxName.Focus();
                // Refresh Data
                DisplayData();
            }
        }
        private void BubbleSort()
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < row - 1; j++)
                {
                    if (String.CompareOrdinal(DataTable[j, 0], DataTable[j + 1, 0]) < 0)
                    {
                        Swap(j);
                    }
                }
            }
        }
        private void ClearFields()
        {
            txtBoxName.Clear();
            txtBoxCategory.Clear();
            radioButtonLinear.Checked = false;
            radioButtonNonLinear.Checked = false;
            txtBoxDefinition.Clear();
        }
        private void ClickData()
        {
            // Add selected item to index
            index = dataListView.SelectedIndices[0];
            toolStripStatusLabel1.Text = DataTable[index, 0] + " " + index;
            if (!String.IsNullOrEmpty(DataTable[index, 0]) && index > -1)
            {
                // Fill boxes with data
                txtBoxName.Text = DataTable[index, 0];
                txtBoxCategory.Text = DataTable[index, 1];
                // Check radio buttons
                if (DataTable[index, 2] == "Linear") radioButtonLinear.Checked = true;
                else radioButtonNonLinear.Checked = true;
                txtBoxDefinition.Text = DataTable[index, 3];
            }            
        }

        private void Delete()
        {
            if (index != -1 && DataTable[index, 0] != null)
            {
                // Set all data to comparable value
                DataTable[index, 0] = "~";
                DataTable[index, 1] = "~";
                DataTable[index, 2] = "~";
                DataTable[index, 3] = "~";
                // Reduce pointer
                ptr--;
                index = -1;
                BubbleSort();
                DisplayData();
            }

            
        }

        private void DisplayData()
        {
            // Clear list
            dataListView.Items.Clear();
            for (int i = 0; i < row; i++)
            {
                if (DataTable[i, 0] != "" && DataTable[i, 0] != null)
                {
                    // Create object to fill
                    ListViewItem data1 = new ListViewItem(DataTable[i, 0]);
                    data1.SubItems.Add(DataTable[i, 1]);
                    data1.SubItems.Add(DataTable[i, 2]);
                    data1.SubItems.Add(DataTable[i, 3]);
                    // Fill into box
                    dataListView.Items.Add(data1);
                }            
            }
        }

        private void Edit()
        {
            int i = index;
            toolStripStatusLabel1.Text = index.ToString();
            DataTable[i, 0] = txtBoxName.Text;
            DataTable[i, 1] = txtBoxCategory.Text;
            if (radioButtonLinear.Checked) DataTable[i, 2] = "Linear";
            else DataTable[i, 2] = "Non-Linear";

            DataTable[i, 3] = txtBoxDefinition.Text;

            DisplayData();
        }
        private void LoadData()
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Data Structure Wiki",
                Filter = "DAT files (*.dat)|*.dat|All files (*.*)|*.*",
                InitialDirectory = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"..\"))
            };
            ofd.FileName = fileName;
            if (File.Exists(fileName) && ofd.ShowDialog() == DialogResult.OK)
            {
                using (var stream = File.Open(ofd.FileName, FileMode.Open))
                {
                    using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                    {
                        for (int i = 0; i < row; i++)
                        {
                            DataTable[i, 0] = reader.ReadString();
                            DataTable[i, 1] = reader.ReadString();
                            DataTable[i, 2] = reader.ReadString();
                            DataTable[i, 3] = reader.ReadString();
                        }
                        reader.Close();

                        DisplayData();
                    }
                }
            }
        }

        private void SaveData()
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Title = "Data Structure Wiki",
                Filter = "DAT files (*.dat)|*.dat|All files (*.*)|*.*",
                InitialDirectory = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"..\"))
            };
            sfd.FileName = fileName;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (Stream stream = File.Open(sfd.FileName, FileMode.Create))
                using (var writer = new BinaryWriter(stream))
                {
                    for (int i = 0; i < row; i++)
                    {
                        writer.Write(DataTable[i, 0]);
                        writer.Write(DataTable[i, 1]);
                        writer.Write(DataTable[i, 2]);
                        writer.Write(DataTable[i, 3]);
                    }
                    writer.Close();


                    MessageBox.Show("Data Saved");
                }
            }
        }

        private void Swap(int j)
        {
            for (int x = 0; x < col - 1; x++)
            {
                string temp = DataTable[j + 1, x];
                DataTable[j + 1, x] = DataTable[j, x];
                DataTable[j, x] = temp;
            }
        }





        #endregion


    }
}