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


// Mauriza Arianne, 08/03/2023
// A wiki for all data structure types

namespace DataStructureWiki
{
    public partial class DataStructureWiki : Form
    {
        public DataStructureWiki()
        {
            InitializeComponent();
        }

        #region Variables
        // 9.1	Create a global 2D string array, use static variables for the dimensions (row = 4, column = 12),

        // Static should not be changed
        static int row = 12;
        static int col = 4;
        // Two dimensionals arrays
        string[,] DataTable = new string[row, col];
        // Default filename for saving and loading
        static string fileName = "definitions.dat";
        int ptr = 0;
        int index = -1;
        #endregion

        #region Events
        // Events that call the methods
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

        #region Method

        // 9.2	Create an ADD button that will store the information from the 4 text boxes into the 2D array,
        // Adds an entry into the table
        private void Add()
        {
            // Check if fields are empty
            if (String.IsNullOrEmpty(txtBoxName.Text) || String.IsNullOrEmpty(txtBoxCategory.Text) || String.IsNullOrEmpty(txtBoxDefinition.Text))
            {
                MessageBox.Show("Please Input all the required fields.", "Warning", 0, MessageBoxIcon.Warning);
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
                    MessageBox.Show("Too many entries.", "Warning", 0, MessageBoxIcon.Warning);
                }
                // Clear fields
                ClearFields();
                // Refresh Data
                DisplayData();
                // Focus
                txtBoxName.Focus();
            }
        }

        //9.7	Write the code for a Binary Search for the Name in the 2D array and display the information in the other textboxes when found, add suitable feedback if the search in not successful and clear the search textbox (do not use any built-in array methods),
        // Searches for an entry using binary search algorithm
        private void BinarySearch()
        {
            // Check if textbox is filled
            if (!String.IsNullOrEmpty(txtBoxSearch.Text))
            {
                // Sort the data and refresh it
                BubbleSort();
                DisplayData();
                // Create the target to search
                string target = txtBoxSearch.Text;
                int first = 0;
                int last = row - 1;
                int position = -1;
                // Entry and exit condition for loop
                while (first <= last)
                {
                    int middle = (first + last) / 2;
                    // If target is found
                    if (String.CompareOrdinal(DataTable[middle, 0],target) == 0)
                    {
                        // Change position and highlight the entry
                        position = middle;
                        index = position;
                        dataListView.SelectedItems.Clear();
                        dataListView.Items[position].Selected = true;
                        dataListView.Items[position].Focused = true;
                        dataListView.Select();
                        // Add into fields
                        FillData(position);
                        // Exit out of loop
                        break;
                    }
                    // Change the middle if entry is higher or lower than the current position
                    else if (String.CompareOrdinal(DataTable[middle, 0], target) < 0)
                    {
                        first = middle + 1;
                    }
                    else if (String.CompareOrdinal(DataTable[middle, 0], target) > 0)
                    {
                        last = middle - 1;
                    }
                }
                // If position has not changed then it does not exist
                if (position == -1)
                {
                    txtBoxSearch.Clear();
                    MessageBox.Show("Does not exist", "Warning", 0, MessageBoxIcon.Warning);
                }
            }
        }

        // 9.6	Write the code for a Bubble Sort method to sort the 2D array by Name ascending, ensure you use a separate swap method that passes the array element to be swapped (do not use any built-in array methods),
        // Sorts the entries in alphabetic order ascending
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

        // 9.5	Create a CLEAR method to clear the four text boxes so a new definition can be added,
        // Clears the fields
        private void ClearFields()
        {
            txtBoxName.Clear();
            txtBoxCategory.Clear();
            radioButtonLinear.Checked = false;
            radioButtonNonLinear.Checked = false;
            txtBoxDefinition.Clear();
        }

        // 9.9	Create a method so the user can select a definition (Name) from the ListView and all the information is displayed in the appropriate Textboxes,
        // Add data into fields from the table using a click
        private void ClickData()
        {
            // Add selected item to index
            index = dataListView.SelectedIndices[0];
            // Check if the entry is empty or if index is in the wrong place
            if (!String.IsNullOrEmpty(DataTable[index, 0]) && index > -1)
            {
                FillData(index);
            }            
        }

        // 9.4	Create a DELETE button that removes all the information from a single entry of the array; the user must be prompted before the final deletion occurs, 
        // Deletes an entry from the table
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
                    // Clear 
                    ClearFields();
                    // Focus
                    txtBoxName.Focus();
                    // Sort after
                    BubbleSort();
                    DisplayData();
                }
            }
        }

        // 9.8	Create a display method that will show the following information in a ListView: Name and Category,
        // Adds the data from the array into the table
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

        // 9.3	Create an EDIT button that will allow the user to modify any information from the 4 text boxes into the 2D array,
        // Edits an entry in the table with current data from the fields
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
                // Clear
                ClearFields();
                txtBoxName.Focus();
            }
        }

        // Fills fields with selected data
        private void FillData(int index)
        {
            // Fill boxes with data
            txtBoxName.Text = DataTable[index, 0];
            txtBoxCategory.Text = DataTable[index, 1];
            // Check radio buttons
            if (DataTable[index, 2] == "Linear") radioButtonLinear.Checked = true;
            else radioButtonNonLinear.Checked = true;
            txtBoxDefinition.Text = DataTable[index, 3];
        }

        // 9.11	Create a LOAD button that will read the information from a binary file called definitions.dat into the 2D array, ensure the user has the option to select an alternative file. Use a file stream and BinaryReader to complete this task.
        // Loads a table from a binary file
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

        // 9.10	Create a SAVE button so the information from the 2D array can be written into a binary file called definitions.dat which is sorted by Name, ensure the user has the option to select an alternative file. Use a file stream and BinaryWriter to create the file.
        // Saves a table into a binary file
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

        // Swap function for sorting
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