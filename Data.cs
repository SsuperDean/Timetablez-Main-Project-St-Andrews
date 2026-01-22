using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Reflection;
using Timetablez.Classes;
using Timetablez.Models;

namespace Timetablez
{
    public partial class DataForm : Form
    {        
        /*
         * The user can dynamically perform any CRUD operation to any of the tables loaded. 
         * Add a Table with CRUD operations in 2 steps:
         *  1. Create the table in sql server with a primary key 
         *  2. Create the class model in c# and set the key attribute
         */

        private string classModelLocation = "Timetablez.Models.";
        private static string connectionStr = AppGlobals.dbLocal;   
        private static string currentTable;                         
        private static DataTable? currentData;  // what is loaded in the gridview
        private Type? currentDataType;          // get the model class type
        private ContextMenuWrapper contextMenu; // Instantiate dynamic context menu 

        public DataForm()
        {
            InitializeComponent();
        }

        private void DataForm_Load(object sender, EventArgs e)
        {
            LoadAvailableTables();            
        }

        private void LoadAvailableTables()
        {

            // Run stored procedure to get only table names based on specifications
            string spName = "GetAllTablesAndViews";
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@tableType", "BASE TABLE");

            DataTable dt = DataRead.GetDataAsDataTable(
                connectionStr,
                "TablesList",
                storedProcedure: spName,
                parameters: param
            );

            // For each row returned, create a button
            foreach (DataRow row in dt.Rows)
            {
                string tableName = row["FullName"].ToString();

                Button btn = new Button();
                btn.Text = tableName;
                btn.Width = flowLayoutPanel1.Width - 5;
                btn.Height = 25;
                btn.FlatStyle = FlatStyle.Flat;
                btn.BackColor = Color.LightSteelBlue;
                btn.Tag = tableName;
                btn.Click += TableButton_Click;

                flowLayoutPanel1.Controls.Add(btn);
            }
        }

        // show visually to the user which button has been clicked
        private void TableButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            currentTable = btn.Tag.ToString();

            // Reset all buttons to default color
            foreach (Button b in flowLayoutPanel1.Controls.OfType<Button>())
            {
                b.BackColor = Color.LightSteelBlue;
            }

            // highlight clicked button 
            btn.BackColor = Color.WhiteSmoke;  

            ShowData(connectionStr, currentTable);
        }

        private void ShowData(string connectionStr, string currentTable)
        {

            /* 
             * the datagrid datasource could be assigned directly to the currentData table but
             * using a view makes it easier to filter without additional sql command calls
            */ 

            string sqlString = "SELECT * FROM " + currentTable;
            currentData = DataRead.GetDataAsDataTable(connectionStr, null, sqlString);
            DataGrid.DataSource = currentData;

            /* assign the table data type which is a generic approach to handling
             * table processing using their data model. This saves from a lot of code repetition
            */
            currentDataType = Type.GetType(classModelLocation + currentTable);

            SetTheme.MorphGridView(DataGrid, "Data", true);

        }

        // CREATE CONTEXT MENU AND OPTIONS ON DATAGRIDVIEW ONLY
        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // Only allow if it is right click and not outside the valid cells
            DataGridView grid = sender as DataGridView;
            if (grid == null || e.Button != MouseButtons.Right)
                return;


            /* 
             When clicking on the grid the row and column number are >= 0
             we do not want this activated for row or column headers
            */

            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            /*
             * e.RowIndex: the row number of the table, from e arguments
             * Unlike left click, right click does not refresh row selection 
             * so this forces the row under the mouse pointer to be selected
             */

            grid.Rows[e.RowIndex].Selected = true;  

            // get the coordinates of the cell clicked
            grid.CurrentCell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex];

            // get the column header name (field name) and the value in cell
            string columnHeader = grid.CurrentCell.OwningColumn.HeaderText;
            object? cellValue = grid.CurrentCell.Value;

            /*
             Instantiates a list of type GridMenuItem data model in ContextMenuWrapper class.
             The second arguiment is the function to execute when the option is clicked
             */

            List<GridMenuItem> rc = new List<GridMenuItem>();

            rc.Add(new GridMenuItem("ACTIONS (CS)", isHeader: true));
            rc.Add(new GridMenuItem("Update", OnUpdateClicked));
            rc.Add(new GridMenuItem("Delete", OnDeleteClicked));
            rc.Add(new GridMenuItem("Validate", OnValidateClicked));
            rc.Add(new GridMenuItem(null, isSeparator: true));
            rc.Add(new GridMenuItem("UTILITIES", isHeader: true));
            rc.Add(new GridMenuItem("Backup", OnBackupClicked));
            rc.Add(new GridMenuItem("Restore", OnRestoreClicked));

            // Create a new context menu instance and pass the list of menu items
            contextMenu = new ContextMenuWrapper(rc);

            /*
             When the user clicks a menu option, Row and Column index is lost, so must be kept as
             metadata in the context menu
             */

            contextMenu.Tag = new Tuple<string, object>(columnHeader, cellValue);

            // create here a list of cm options to disable based on user role or table needs
            contextMenu.EnableItem("Validate", currentTable=="WeeklySchedule");

            contextMenu.ShowAt(DataGrid, DataGrid.PointToClient(Cursor.Position));
        }

        private void OnUpdateClicked(object sender, EventArgs e)
        {
            if (currentData == null || string.IsNullOrEmpty(currentTable))
            {
                MessageBox.Show(
                    "No data loaded.", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                return;
            }

            DataBulkUpdate.BulkUpdateAllData(
                connectionStr, 
                currentTable, 
                currentData 
                );

            ShowData(connectionStr, currentTable);
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            if (currentData == null || DataGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "No data selected for deletion.", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                return;
            }

            /* 
             * The method will loop through the data model for a [Key] attribute in any property 
             * and if one exists will return the column name. 
            */

            string keyColumn = GetPrimaryKeyName(currentDataType);

            if (keyColumn == null)
            {
                MessageBox.Show(
                    "Deletion is not allowed because no primary key is defined for this table.", 
                    "Error", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Warning);
                return;
            }

            var selectedIds = new List<object>();

            // it will create a list of all primary key id from any selected rows

            foreach (DataGridViewRow row in DataGrid.SelectedRows)
            {
                if (row.Cells[keyColumn].Value != null)
                    selectedIds.Add(row.Cells[keyColumn].Value);
            }


            DataDelete.DeleteSelectedRows(
                connectionStr, 
                currentTable, 
                selectedIds, 
                keyColumn);

            ShowData(connectionStr, currentTable);
        }

        public static string? GetPrimaryKeyName(Type modelType)
        {
            PropertyInfo keyProperty = null;
            
            /*
             * The type is being passed from the currentDataType which contains everything about the data model class.
             * Build a propertyinfo type array to search all the public, non-static (Instance) properties in the class. 
             * excluding constructors
             */

            PropertyInfo[] properties = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                // search and exit the loop if the [Key] attribute in a property is found
                if (Attribute.IsDefined(property, typeof(KeyAttribute)))
                {
                    keyProperty = property;
                    break;
                }
            }

            return keyProperty?.Name;
        }

        private void OnRestoreClicked(object sender, EventArgs e)
        {
            SQLTablesHelper.CopyTable(connectionStr, currentTable + "Bak", currentTable, "spCopyTable");
            ShowData(connectionStr, currentTable);
        }

        private void OnBackupClicked(object sender, EventArgs e)
        {
            SQLTablesHelper.CopyTable(connectionStr, currentTable, currentTable + "Bak", "spCopyTable");
        }

        private void OnValidateClicked(object sender, EventArgs e)
        {
            Validations.DataSource = DataRead.GetDataAsDataTable(connectionStr, currentTable, null, "spWSValidations");
            SetTheme.MorphGridView(Validations,"Validations");
            
        }
    }

}

