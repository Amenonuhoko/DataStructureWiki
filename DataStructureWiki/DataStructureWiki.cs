using System;
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
        int index = 0;
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
            Click();
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveData();
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
                    if (radioButtonLinear.Checked)
                    {
                        DataTable[ptr, 2] = "Linear";
                    }
                    else
                    {
                        DataTable[ptr, 2] = "Non-Linear";
                    }
                    DataTable[ptr, 3] = txtBoxDefinition.Text;
                    ptr++;
                } else // Check if there are more than 12
                {
                    MessageBox.Show("Too many entries");
                }
                // Refresh Data
                DisplayData();
            }
        }

        // TODO: Errors, comments, clean up, link to edit
        private void Click()
        {
            // Add selected item to index
            index = dataListView.SelectedIndices[0];
            
            // Check if empty
            if (String.IsNullOrEmpty(DataTable[index, 0]))
            {
                MessageBox.Show("This field is empty");
            } else
            {
                txtBoxName.Text = DataTable[index, 0];
                txtBoxCategory.Text = DataTable[index, 1];
                if (DataTable[index, 2] == "Linear")
                {
                    radioButtonLinear.Checked = true;
                }
                else
                {
                    radioButtonNonLinear.Checked = true;
                }
                txtBoxDefinition.Text = DataTable[index, 3];
            }
            
        }

        private void Delete()
        {
            for (int i = index; i < row - index - 1; i++)
            {
                DataTable[i, 0] = DataTable[i + 1, 0];
                DataTable[i, 1] = DataTable[i + 1, 1];
                DataTable[i, 2] = DataTable[i + 1, 2];
                DataTable[i, 3] = DataTable[i + 1, 3];
            }

            DataTable[row - 1, 0] = "";
            DataTable[row - 1, 1] = ""; 
            DataTable[row - 1, 2] = "";
            DataTable[row - 1, 3] = "";

            

            DisplayData();
        }


        // TODO: Errors, comments, clean up
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

        private void Edit()
        {
            DataTable[index, 0] = txtBoxName.Text;
            DataTable[index, 1] = txtBoxCategory.Text;
            if (radioButtonLinear.Checked)
            {
                DataTable[index, 2] = "Linear";
            }
            else
            {
                DataTable[index, 2] = "Non-Linear";
            }
            DataTable[index, 3] = txtBoxDefinition.Text;

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




        #endregion


    }
}