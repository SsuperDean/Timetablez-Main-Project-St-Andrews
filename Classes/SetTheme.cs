using Timetablez.Models;

namespace Timetablez.Classes
{
    public class SetTheme
    {
        public static void MorphGridView(DataGridView dgv, string themeName, bool EditBehaviour = false)
        {
            if (!AppGlobals.themes.ContainsKey(themeName))
                themeName = "Data";   // default fallback

            GridTheme theme = AppGlobals.themes[themeName];

            // Behaviour
            dgv.ReadOnly = !EditBehaviour;
            dgv.AllowUserToAddRows = EditBehaviour;
            dgv.AllowUserToDeleteRows = EditBehaviour;
            dgv.MultiSelect = EditBehaviour;
            dgv.RowHeadersVisible = EditBehaviour;

            // Row Selection 
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.DefaultCellStyle.SelectionBackColor = theme.SelectionBackColor;
            dgv.RowsDefaultCellStyle.BackColor = theme.RowBackColor;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = theme.AlternatingRowBackColor;
            dgv.BorderStyle = BorderStyle.None;

            // Column headers
            dgv.AllowUserToOrderColumns = true;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = theme.HeaderBackColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = theme.HeaderForeColor;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = theme.HeaderSelectionBackColor;
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = theme.HeaderSelectionForeColor;

            // Grid and column sizing
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;

            // wrap long text
            dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
        }

    }

}
