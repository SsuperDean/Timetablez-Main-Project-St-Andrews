using System.Data;
using Microsoft.Data.SqlClient;
using Timetablez.Classes;
using Timetablez.Models;



namespace Timetablez
{
    public partial class TimetableBrowser : Form
    {
        // CLASS VARIABLES DECLARATION

        // Gets the default connection string selected in splash
        string cStr = AppGlobals.dbLocal;

        // Bespoke TableLayoutPanel with class to control flickering on scroll
        private NoFlickerPanel tlpMain = new NoFlickerPanel();

        // Used to identify starting and ending period during drag and drop
        private int actualPeriodStart, actualPeriodEnd;

        // The button for drag and drop
        private Button dragSourceButton;
        private bool dragStarted = false;
        private Point mouseDownLocation;

        // A reference to the last button clicked
        private Button contextButton;

        private Color btnBackgroundColor = Color.WhiteSmoke;
        private Color panelBackColor = Color.WhiteSmoke;

        private DataTable lessons;

        private int totalDaysOfWeek = AppGlobals.TotalWeekDays;
        private int totalPeriodsPerDay = AppGlobals.TotalPeriods;

        private string lessonView = AppGlobals.lessonView;
        private string lessonTable = AppGlobals.lessonTable;

        //Design to assist slider for monitor size
        private int firstRowHeight = 20;
        private int firstColumnWidth = 20;
        private static int containerColumnWidth = 120;
        private static int flpMinWidth = (int)((double)containerColumnWidth * 0.95);
        private int flpMinHeight = 95;
        private int btnWidth = (int)((double)flpMinWidth * 0.95);
        private int btnHeight = 20;

        public TimetableBrowser()
        {
            InitializeComponent();
            SetupTableLayoutPanel();
            ShowErrors();
        }

        private void SetupTableLayoutPanel()
        {
            tlpMain.RowCount = totalPeriodsPerDay + 1; // Header and Period1-4
            tlpMain.ColumnCount = totalDaysOfWeek + 1; // Header and Mon-Fri

            tlpMain.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tlpMain.AutoSize = true;
            tlpMain.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            // Resets the rows styling
            tlpMain.RowStyles.Clear();

            // Creates the first row for the weekday values. Be absolute in size (index 0 row)
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, firstRowHeight));

            // Creates the rows periods 1-4 container for buttons. Needs autosize to shrink and grow as needed.
            for (int i = 1; i < totalPeriodsPerDay; i++)
                tlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Clears the column styling
            tlpMain.ColumnStyles.Clear();

            // Creates the first column absolute size to hold label for periods
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, firstColumnWidth));

            for (int i = 1; i < totalDaysOfWeek; i++)
                tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, containerColumnWidth));

            String[] daysOfWeek = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            // Header labels (columns)
            for (int i = 0; i < totalDaysOfWeek; i++)
                tlpMain.Controls.Add(CreateHeaderLabel(daysOfWeek[i]), i + 1, 0);

            // Header labels (rows)
            for (int i = 1; i <= totalPeriodsPerDay; i++)
                tlpMain.Controls.Add(CreateRowLabel("P" + i.ToString()), 0, i);

            /* 
             * A table cell can only contain one control. A FlowLayoutPanel works best as container 
             * for multiple controls. Configure a FlowLayoutPanel and populate each TableLayoutPanel cell
             */

            for (int row = 1; row < totalPeriodsPerDay + 1; row++)
            {
                for (int col = 1; col < totalDaysOfWeek + 1; col++)
                {
                    FlowLayoutPanel flowPanel = new FlowLayoutPanel
                    {
                        Dock = DockStyle.Fill,
                        BackColor = panelBackColor,
                        AllowDrop = true,
                        AutoSize = true,
                        WrapContents = false,
                        AutoSizeMode = AutoSizeMode.GrowAndShrink,
                        FlowDirection = FlowDirection.TopDown,
                        MinimumSize = new Size(flpMinWidth, flpMinHeight),
                        Tag = new Point(col, row)
                    };

                    // Assign drag drop events to each panel
                    flowPanel.DragEnter += flowPanel_DragEnter;
                    flowPanel.DragDrop += flowPanel_DragDrop;

                    tlpMain.Controls.Add(flowPanel, col, row);
                }
            }
        }

        // Method to check for errors in time table
        private void ShowErrors()
        {
            dgvErrors.DataSource = DataRead.GetDataAsDataTable(cStr, "errors", null, "spWSValidations");
            dgvErrors.Tag = "Errors";
            SetTheme.MorphGridView(dgvErrors, "Validations");
        }

        private void TimetableBrowser_Load(object sender, EventArgs e)
        {
            // Load data source options
            cboDataSource.Items.Add("LessonView");
            cboDataSource.Items.Add("LessonViewTemp");
            cboDataSource.SelectedText = lessonView;

            // Wrap tlpMain in a scrollable panel
            Panel scrollWrapper = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                AllowDrop = true // Ensure drag events aren't blocked
            };

            // Add the timetable into the panel to ensure scroll
            scrollWrapper.Controls.Add(tlpMain);

            // place the scrollable panel inside the left panel of the split schedule panel
            splitSchedule.Panel1.Controls.Add(scrollWrapper);

            // lesson view converts period and day to row and col
            string sql = "SELECT DISTINCT GroupName, TeacherName, CourseName, RoomName, ColNo, RowNo FROM " + lessonView;
            lessons = DataRead.GetDataAsDataTable(cStr, "Lessons", sql, null, null);


            foreach (DataRow lesson in lessons.Rows)
            {
                string groupName = lesson["GroupName"].ToString();
                string roomName = lesson["RoomName"].ToString();
                string teacherName = lesson["TeacherName"].ToString();
                string courseName = lesson["CourseName"].ToString();

                var buttonTagValues = new Dictionary<string, string>
                {
                    { "GroupName", groupName },
                    { "RoomName", roomName },
                    { "TeacherName", teacherName },
                    { "CourseName", courseName }
                };
                AddButtonToCell((int)lesson["ColNo"], (int)lesson["RowNo"], buttonTagValues);
            }

            // delay positioning splitter until after the tlp has fully rendered to get its final width
            this.BeginInvoke(new Action(() =>
            {
                splitSchedule.SplitterDistance = tlpMain.PreferredSize.Width + 30;
            }));
        }

        private Label CreateHeaderLabel(string text)
        {
            return CreateLabel(text, Color.CornflowerBlue, Color.White);
        }

        private Label CreateRowLabel(string text)
        {
            return CreateLabel(text, Color.LightGray, Color.Black);
        }

        private Label CreateLabel(string text, Color bg, Color fg)
        {
            Label lbl = new Label();
            lbl.Text = text;
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lbl.BackColor = bg;
            lbl.ForeColor = fg;
            lbl.Font = new Font(lbl.Font, FontStyle.Bold);
            return lbl;
        }

        private void ResetButtons(object sender, EventArgs e)
        {
            tlpMain.SuspendLayout();
            dgvDetails.DataSource = null;

            foreach (Control control in tlpMain.Controls)
            {
                FlowLayoutPanel panel = control as FlowLayoutPanel;
                if (panel != null)
                {
                    panel.SuspendLayout();
                    panel.Controls.Clear();
                    panel.ResumeLayout();
                }
            }

            string sql = "SELECT DISTINCT GroupName, TeacherName, CourseName, RoomName, ColNo, RowNo FROM " + lessonView;
            lessons = DataRead.GetDataAsDataTable(cStr, "Lessons", sql, null, null);

            foreach (DataRow lesson in lessons.Rows)
            {
                string groupName = lesson["GroupName"].ToString();
                string roomName = lesson["RoomName"].ToString();
                string teacherName = lesson["TeacherName"].ToString();
                string courseName = lesson["CourseName"].ToString();

                var buttonTagValues = new Dictionary<string, string>
                {
                    { "GroupName", groupName },
                    { "RoomName", roomName },
                    { "TeacherName", teacherName },
                    { "CourseName", courseName }
                };

                AddButtonToCell((int)lesson["ColNo"], (int)lesson["RowNo"], buttonTagValues);
            }

            if (lessonTable == "WeeklyScheduleTemp")
                dgvErrors.DataSource = null;
            else
                ShowErrors();

                tlpMain.ResumeLayout();
        }

        private void ShowContextMenuForButton(Button clickedButton, Point location)
        {

            List<GridMenuItem> rc = new List<GridMenuItem>();

            rc.Add(new GridMenuItem("ACTIONS", isHeader: true));
            rc.Add(new GridMenuItem("Reset", ResetButtons));
            rc.Add(new GridMenuItem("", isSeparator: true));
            rc.Add(new GridMenuItem("HIGHLIGHT SIMILAR", isHeader: true));
            rc.Add(new GridMenuItem("Room", HighlightSimilarRoomName_Click));
            rc.Add(new GridMenuItem("Group", HighlightSimilarGroupName_Click));
            rc.Add(new GridMenuItem("Teacher", HighlightSimilarTeacherName_Click));
            rc.Add(new GridMenuItem("Course", HighlightSimilarCourseName_Click));
            rc.Add(new GridMenuItem("", isSeparator: true));
            rc.Add(new GridMenuItem("BROWSE", isHeader: true));
            rc.Add(new GridMenuItem("Day", BrowseDay_Click));
            rc.Add(new GridMenuItem("Period", BrowsePeriod_Click));

            var contextMenu = new ContextMenuWrapper(rc);
            contextMenu.Tag = clickedButton.Tag;
            contextButton = clickedButton;
            contextMenu.ShowAt(clickedButton, location);
        }

        private void BrowseDay_Click(object sender, EventArgs e)
        {
            Browse(sender, "Day");
        }

        private void BrowsePeriod_Click(object sender, EventArgs e)
        {
            Browse(sender, "Period");
        }

        private void HighlightSimilarRoomName_Click(object sender, EventArgs e)
        {
            HighlightSimilarByField(sender, "RoomName");
        }

        private void HighlightSimilarGroupName_Click(object sender, EventArgs e)
        {
            HighlightSimilarByField(sender, "GroupName");
        }

        private void HighlightSimilarTeacherName_Click(object sender, EventArgs e)
        {
            HighlightSimilarByField(sender, "TeacherName");
        }

        private void HighlightSimilarCourseName_Click(object sender, EventArgs e)
        {
            HighlightSimilarByField(sender, "CourseName");
        }

        private void Browse(object sender, string detail)
        {
            if (contextButton != null)
            {
                FlowLayoutPanel parentPanel = contextButton.Parent as FlowLayoutPanel;
                if (parentPanel != null)
                {
                    int col = tlpMain.GetColumn(parentPanel);
                    int row = tlpMain.GetRow(parentPanel);

                    string colStr = detail == "Day" ? col.ToString() : null;
                    string rowStr = detail == "Period" ? row.ToString() : null;

                    BrowseFilterBy(null, colStr, rowStr);
                }
            }
        }

        private void HighlightSimilarByField(object sender, string fieldName)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem == null || menuItem.Owner == null)
                return;

            var data = menuItem.Owner.Tag as Dictionary<string, string>;
            if (data == null || !data.ContainsKey(fieldName))
                return;

            string tagValue = data[fieldName];

            ButtonGroupColour(tagValue, tlpMain, fieldName);

            // populate gridview with resuling content
            string sql = "SELECT DISTINCT GroupName, RoomName, StudentName, TeacherName, CourseName, PeriodName " +
                         $"FROM " + lessonView + $" WHERE {fieldName} = '{tagValue}'";
            DataTable lessons = DataRead.GetDataAsDataTable(cStr, "Lessons", sql, null, null);
            dgvDetails.DataSource = lessons;
            dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDetails.RowHeadersVisible = false;
            dgvDetails.AllowUserToOrderColumns = true;
        }

        private void ButtonGroupColour(string highlightValue, TableLayoutPanel tlp, string fldName = null)
        {
            // Highlight all buttons with the same tag
            foreach (Control ctrl in tlp.Controls)
            {
                //This is to skip labels for rows and columns
                FlowLayoutPanel panel = ctrl as FlowLayoutPanel;
                if (panel == null)
                    continue;

                //Loop through each button
                foreach (Control innerCtrl in panel.Controls)
                {
                    panel.BackColor = panelBackColor;
                    Button btn = innerCtrl as Button;
                    if (btn != null && btn.Tag != null)
                    {
                        btn.BackColor = btnBackgroundColor;
                        var data = btn.Tag as Dictionary<string, string>;
                        string fieldName = fldName == null ? "GroupName" : fldName;

                        if (data[fieldName] == highlightValue)
                        {
                            btn.BackColor = Color.LightGreen; // or any color you prefer
                        }
                    }

                }
            }
        }

        private void ResetButtonColour(TableLayoutPanel tlp)
        {
            foreach (Control ctrl in tlp.Controls)
            {
                FlowLayoutPanel panel = ctrl as FlowLayoutPanel;
                if (panel == null)
                    continue;

                foreach (Control innerCtrl in panel.Controls)
                {
                    panel.BackColor = panelBackColor;
                    Button btn = innerCtrl as Button;
                    btn.BackColor = btnBackgroundColor;
                }
            }

        }

        private void btn_MouseClick(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                FlowLayoutPanel parentPanel = clickedButton.Parent as FlowLayoutPanel;
                if (parentPanel != null)
                {
                    // Retrieve the tag
                    var data = clickedButton.Tag as Dictionary<string, string>;
                    if (data != null && data.ContainsKey("GroupName"))
                    {
                        string tagValue = data["GroupName"];

                        // Get column and row from the TableLayoutPanel
                        int col = tlpMain.GetColumn(parentPanel);
                        int row = tlpMain.GetRow(parentPanel);

                        // Call your method with the tag
                        BrowseFilterBy(tagValue, col.ToString(), row.ToString());

                        // Group colour
                        ButtonGroupColour(tagValue, tlpMain);
                    }
                }
            }
        }

        private void BrowseFilterBy(string tag = null, string col = null, string row = null)
        {

            //if (tag == null) return;

            var parameters = new Dictionary<string, object>()
            {
                { "@LessonView", lessonView },
                { "@SelectFields", "GroupName, RoomName, StudentName, TeacherName, CourseName, PeriodName" },
                { "@Tag", tag },
                { "@Col", col },
                { "@Row", row }
            };

            DataTable lessons = DataRead.GetDataAsDataTable(cStr, "Lessons", null, "spBrowse", parameters);
            dgvDetails.DataSource = lessons;
            SetTheme.MorphGridView(dgvDetails, "Details");
        }

        // A custom TableLayoutPanel with double buffering enabled to reduce flickering
        public class NoFlickerPanel : TableLayoutPanel
        {
            public NoFlickerPanel()
            {
                this.DoubleBuffered = true;
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
                this.UpdateStyles();
            }
        }

        private void cboDataSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            lessonView = cboDataSource.SelectedItem.ToString();
            if (lessonView.Contains("Temp"))
                lessonTable = AppGlobals.lessonTable + "Temp";
            else
                lessonTable = AppGlobals.lessonTable;

            ResetButtons(this, EventArgs.Empty);
        }

        private void ApplyScaleChanges()
        {
            // Resize column widths
            for (int col = 1; col < totalDaysOfWeek; col++)
            {
                tlpMain.ColumnStyles[col].Width = containerColumnWidth;
            }

            // Resize each FlowLayoutPanel + its buttons
            foreach (Control ctrl in tlpMain.Controls)
            {
                FlowLayoutPanel flp = ctrl as FlowLayoutPanel;
                if (flp != null)
                {
                    flp.MinimumSize = new Size(flpMinWidth, flpMinHeight);

                    foreach (Control inner in flp.Controls)
                    {
                        Button btn = inner as Button;
                        if (btn != null)
                        {
                            btn.Width = btnWidth;
                        }
                    }
                }
            }

            tlpMain.PerformLayout();
            splitSchedule.SplitterDistance = tlpMain.PreferredSize.Width + 30;
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            int step = 20;
            int val = trackBar1.Value;

            // Snap "UP"
            int snapped = ((val + step - 1) / step) * step;

            // Clamp min/max just in case
            if (snapped < trackBar1.Minimum) snapped = trackBar1.Minimum;
            if (snapped > trackBar1.Maximum) snapped = trackBar1.Maximum;

            // Only assign if changed
            if (snapped != trackBar1.Value)
                trackBar1.Value = snapped;

            // Now resize your UI
            containerColumnWidth = trackBar1.Value;
            flpMinWidth = (int)(containerColumnWidth * 0.95);
            btnWidth = (int)(flpMinWidth * 0.95);

            ApplyScaleChanges();

        }

        // ========================== MOUSE DRAG DROP =============================

        private void btn_MouseDown(object sender, MouseEventArgs e)
        {
            // Only allow for left click
            if (e.Button == MouseButtons.Left)
            {
                // Only accepts a button, otherwise null
                dragSourceButton = sender as Button;

                if (dragSourceButton != null)
                {
                    // Get the flow panel that the button is in
                    FlowLayoutPanel flowPanel = dragSourceButton.Parent as FlowLayoutPanel;
                    if (flowPanel != null)
                    {
                        // Get column and row to calculate the period 
                        int col = tlpMain.GetColumn(flowPanel);
                        int row = tlpMain.GetRow(flowPanel);
                        actualPeriodStart = ((col - 1) * 4) + row;

                    }

                }

                // Store for future reference i.e mousemove
                mouseDownLocation = e.Location;
                dragStarted = false;
            }
        }

        private void btn_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // If not drag, then execute a normal click
                if (!dragStarted)
                {
                    btn_MouseClick(sender, e);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                // Gets position of mouse and data about button to build context menu
                Button clickedButton = sender as Button;
                btn_MouseClick(sender, e);
                ShowContextMenuForButton(clickedButton, new Point(e.X, e.Y));
            }
        }

        private void btn_MouseMove(object sender, MouseEventArgs e)
        {
            // Must be left click
            if (e.Button != MouseButtons.Left)
                return;

            // Distance mouse moved from click position
            int dx = Math.Abs(e.X - mouseDownLocation.X);
            int dy = Math.Abs(e.Y - mouseDownLocation.Y);

            // 5 pixel move causes drag
            // If drag not already started to not constantly repeat
            if (!dragStarted && (dx > 5 || dy > 5))
            {
                dragStarted = true;
                ResetButtonColour(tlpMain);
                dragSourceButton.BackColor = Color.Orange;
                // Execute drag drop on specific button clicked
                dragSourceButton.DoDragDrop(dragSourceButton, DragDropEffects.Move);
            }
        }

        private void flowPanel_DragEnter(object sender, DragEventArgs e)
        {
            // Only allows a button to be dragged
            if (e.Data.GetDataPresent(typeof(Button)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        private void flowPanel_DragDrop(object sender, DragEventArgs e)
        {
            // Value stored will be null if not button
            Button draggedButton = e.Data.GetData(typeof(Button)) as Button;
            FlowLayoutPanel targetPanel = sender as FlowLayoutPanel;    // Panel dragging into
            string tagValue = null;
            Dictionary<string, string> data = new Dictionary<string, string>();

            // Cant drag anything but a button and only into a panel that the button is not already in
            if (draggedButton != null && targetPanel != null && draggedButton.Parent != targetPanel)
            {
                targetPanel.SuspendLayout();    //stop flickering

                targetPanel.Controls.Add(draggedButton);
                targetPanel.PerformLayout();    //refresh

                // Deconstruct tag holding group, course, teacher and room
                data = draggedButton.Tag as Dictionary<string, string>;
                tagValue = data["GroupName"];

                //highlight same group
                ButtonGroupColour(tagValue, tlpMain);

                targetPanel.ResumeLayout();
            }
            else
            {
                return;
            }

            if (targetPanel != null)
            {
                Point? cell = targetPanel.Tag as Point?; // Used to calculate row and col
                if (cell.HasValue)
                {
                    int col = cell.Value.X;
                    int row = cell.Value.Y;

                    // If result returns rows, then group already exists in that period
                    var parameters = new Dictionary<string, object>()
                    {
                        { "@Tag", data["GroupName"] },
                        { "@Col", col },
                        { "@Row", row }
                    };
                    DataTable result = DataRead.GetDataAsDataTable(cStr, "Lessons", null, "spBrowse", parameters);
                    if (result != null && result.Rows.Count != 0)
                    {
                        MessageBox.Show("Cannot drag group into period where group already exists.", "Schedule Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ResetButtons(this, EventArgs.Empty);
                        return;
                    }

                    // Needed to know where to store the button
                    actualPeriodEnd = ((col - 1) * 4) + row;

                    data = draggedButton?.Tag as Dictionary<string, string>;
                    // Just in case...
                    if (data != null)
                    {
                        parameters = new Dictionary<string, object>
                        {
                            { "@GroupName", data["GroupName"] },
                            { "@periodStart", actualPeriodStart },
                            { "@periodEnd", actualPeriodEnd },
                            { "@LessonView", lessonView },
                            { "@scheduleTable", lessonTable }
                        };

                        DataRead.ExecuteNonQuery(cStr, "spMoveGroup", parameters, true);
                        ShowErrors();
                        BrowseFilterBy(tagValue, col.ToString(), row.ToString());
                    }
                }
            }
            
        }

        private void AddButtonToCell(int col, int row, Dictionary<string, string> tagValue)
        {
            // Loop through all cell
            foreach (Control control in tlpMain.Controls)
            {
                //Check if it is the target cell
                if (tlpMain.GetColumn(control) == col && tlpMain.GetRow(control) == row)
                {
                    FlowLayoutPanel flow = control as FlowLayoutPanel;
                    if (flow != null)
                    {
                        // Build the button
                        Button btn = new Button
                        {
                            Text = tagValue["GroupName"] + '-' + tagValue["RoomName"],
                            Tag = tagValue,
                            Width = btnWidth,
                            AutoSize = false,
                            //TextAlign = ContentAlignment.MiddleLeft,
                            TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                            FlatStyle = FlatStyle.Standard,
                            Enabled = true,
                            BackColor = btnBackgroundColor,
                            Padding = new Padding(0),
                            Margin = new Padding(0),
                            Height = btnHeight,
                            Font = new Font("Calibri", 8f, FontStyle.Regular)
                        };

                        // Assign events
                        btn.MouseDown += btn_MouseDown;
                        btn.MouseUp += btn_MouseUp;
                        btn.MouseClick += btn_MouseClick;
                        btn.MouseMove += btn_MouseMove;

                        //Add the button
                        flow.Controls.Add(btn);
                        break;
                    }
                }
            }
        }
         
    }
}
