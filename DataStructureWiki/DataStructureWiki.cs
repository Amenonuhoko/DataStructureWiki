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

        // TODO COMMENTS
        #region Variables
        static int row = 12;
        static int col = 4;
        string[,] DataTable = new string[row, col];
        static string fileName = "definitions.dat";
        int ptr = 0;
        int index = -1;
        #endregion

        // TODO COMMENTS
        #region Events
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            Add();
        }
        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
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
        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            BinarySearch();
        }
        private void BtnSort_Click(object sender, EventArgs e)
        {
            BubbleSort();
            DisplayData();
        }
        private void dataListView_Click(object sender, EventArgs e)
        {
            ClickData();
        }
        #endregion

        // TODO COMMENTS ON METHODS
        #region Method
        private void Add()
        {
            // Check if fields are empty
            if (String.IsNullOrEmpty(txtBoxName.Text) || String.IsNullOrEmpty(txtBoxCategory.Text) || String.IsNullOrEmpty(txtBoxDefinition.Text))
            {
                MessageBox.Show("Please Input all the required fields.");
            } else
            {
                // Make sure the pointer is not out of bounds
                if (ptr < row)
                {
                    DataTable[ptr, 0] = txtBoxName.Text;
                    DataTable[ptr, 1] = txtBoxCategory.Text;
                    DataTable[ptr, 2] = radioButtonLinear.Checked == true ? "Linear" : "Non-Linear";
                    DataTable[ptr, 3] = txtBoxDefinition.Text;
                    ptr++;
                    

                }
                // Check if there are more than 12
                else
                {
                    MessageBox.Show("Too many entries");
                }
                // Clear fields
                ClearFields();
                // Focus
                txtBoxName.Focus();
                // Refresh Data
                DisplayData();
            }
        }
        // TODO
        private void BinarySearch()
        {
            if (!String.IsNullOrEmpty(txtBoxSearch.Text))
            {
                BubbleSort();
                DisplayData();

                string target = txtBoxSearch.Text;
                int first = 0;
                int last = row - 1;
                int position = -1;

                while (first <= last)
                {
                    int middle = (first + last) / 2;
                    Debug.WriteLine("first: " + first);
                    Debug.WriteLine("last: " + last);
                    Debug.WriteLine(middle);

                    if (DataTable[middle, 0] == target)
                    {
                        position = middle;
                        dataListView.Items[position].Selected = true;
                        dataListView.Items[position].Focused = true;

                        Debug.WriteLine("here");

                        break;
                    }
                    else if (String.CompareOrdinal(DataTable[middle, 0], target) < 0)
                    {
                        first = middle + 1;
                    }
                    else if (String.CompareOrdinal(DataTable[middle, 0], target) > 0)
                    {
                        last = middle - 1;
                    }
                }

                if (position == -1)
                {
                    MessageBox.Show("Does not exist");
                }
            }
        }
        private void BubbleSort()
        {
            // Loop through outer set of data
            for (int i = 0; i < row; i++)
            {
                // Loop through inner set of data
                for (int j = 0; j < row - 1; j++)
                {
                    // Check if selected item is less value than the one next to it
                    if (String.CompareOrdinal(DataTable[j, 0], DataTable[j + 1, 0]) > 0)
                        
                    {
                        // Swap if true
                        Swap(j);
                    }
                }
            }
        }
        private void ClearFields()
        {
            // Clear all fields
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
            // Check if the entry is empty or if index is in the wrong place
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
            // Check if index is in the correct place
            if (index > -1)
            {
                // Show confirmation
                DialogResult confirm = MessageBox.Show("Confirm Delete?", "Delete", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    // Set all data to comparable value
                    DataTable[index, 0] = "zzz";
                    DataTable[index, 1] = "zzz";
                    DataTable[index, 2] = "zzz";
                    DataTable[index, 3] = "zzz";
                    // Reduce pointer
                    ptr--;
                    BubbleSort();
                    DisplayData();
                }
            }
        }
        private void DisplayData()
        {
            // Clear list
            dataListView.Items.Clear();
            // Loop through data
            for (int i = 0; i < row; i++)
            {
                // Hide entries that are deleted or empty
                if (DataTable[i, 0] != "zzz" && DataTable[i, 0] != null)
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
            // Error trappings
            if (!String.IsNullOrEmpty(txtBoxName.Text) || !String.IsNullOrEmpty(txtBoxCategory.Text) || !String.IsNullOrEmpty(txtBoxDefinition.Text))
            {
                DataTable[index, 0] = txtBoxName.Text;
                DataTable[index, 1] = txtBoxCategory.Text;
                if (radioButtonLinear.Checked) DataTable[index, 2] = "Linear";
                else DataTable[index, 2] = "Non-Linear";
                DataTable[index, 3] = txtBoxDefinition.Text;
                // Refresh
                DisplayData();
            }
        }
        private void LoadData()
        {
            // File dialog config
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Data Structure Wiki",
                Filter = "DAT files (*.dat)|*.dat|All files (*.*)|*.*",
                InitialDirectory = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"..\"))
            };
            // Set filename
            ofd.FileName = fileName;
            if (File.Exists(fileName) && ofd.ShowDialog() == DialogResult.OK)
            {
                using (var stream = File.Open(ofd.FileName, FileMode.Open))
                using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                {
                    // Loop through and read entries
                    for (int i = 0; i < row; i++)
                    {
                        DataTable[i, 0] = reader.ReadString();
                        DataTable[i, 1] = reader.ReadString();
                        DataTable[i, 2] = reader.ReadString();
                        DataTable[i, 3] = reader.ReadString();
                        ptr++;
                    }
                    reader.Close();
                    // Refresh
                    DisplayData();
                    // Display success
                    MessageBox.Show("Load Successful");
                }
            }
        }
        private void SaveData()
        {
            // File dialog config
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Title = "Data Structure Wiki",
                Filter = "DAT files (*.dat)|*.dat|All files (*.*)|*.*",
                InitialDirectory = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), @"..\"))
            };
            // Set filename
            sfd.FileName = fileName;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (Stream stream = File.Open(sfd.FileName, FileMode.Create))
                using (var writer = new BinaryWriter(stream))
                {
                    // Loop through 4 sets of data for each column
                    for (int i = 0; i < row; i++)
                    {
                        writer.Write(DataTable[i, 0]);
                        writer.Write(DataTable[i, 1]);
                        writer.Write(DataTable[i, 2]);
                        writer.Write(DataTable[i, 3]);
                    }
                    writer.Close();
                    // Display success
                    MessageBox.Show("Save Successful");
                }
            }
        }
        private void Swap(int j)
        {
            // Swap for every element in the columns
            for (int x = 0; x < col; x++)
            {
                string temp = DataTable[j + 1, x];
                DataTable[j + 1, x] = DataTable[j, x];
                DataTable[j, x] = temp;
            }
        }
        #endregion


    }
}