
namespace Timetablez
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void dataManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form existing = GetOpenForm("DataForm");
            if (existing != null)
            {
                existing.Activate();
                return;
            }

            DataForm frm = new DataForm();
            frm.MdiParent = this;
            frm.Show();
        }


        private void timetableGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form existing = GetOpenForm("TimeTableGen");
            if (existing != null)
            {
                existing.Activate();
                return;
            }

            TimeTableGen frm = new TimeTableGen();
            frm.MdiParent = this;
            frm.Show();
        }

        private void timetableBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form existing = GetOpenForm("TimetableBrowser");
            if (existing != null)
            {
                existing.Activate();
                return;
            }
            TimetableBrowser frm = new TimetableBrowser();
            frm.MdiParent = this;
            frm.Show();
        }

        // Loop through all open forms. Returns null if the form is not already open
        private Form GetOpenForm(string formName)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.Name == formName)
                    return f;
            }
            return null;
        }
    }
}
