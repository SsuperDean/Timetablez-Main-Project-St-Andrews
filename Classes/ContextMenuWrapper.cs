using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Timetablez
{
    public class ContextMenuWrapper : ContextMenuStrip
    {

        // main constractor to build menu based on list passed
        public ContextMenuWrapper(List<GridMenuItem> items)
        {
            this.Items.Clear();

            foreach (var item in items)
            {
                if (item.IsSeparator)
                {
                    this.Items.Add(new ToolStripSeparator());
                }
                else if (item.IsHeader)
                {
                    var header = new ToolStripMenuItem(item.Text)
                    {
                        Enabled = false,
                        Font = new Font(SystemFonts.DefaultFont, FontStyle.Bold),
                    };
                    this.Items.Add(header);
                }
                else
                {
                    this.Items.Add(item.Text, null, item.Handler);
                }
            }
        }

        public void EnableItem(string itemText, bool enabled)
        {
            foreach (ToolStripItem item in this.Items)
            {
                if (item.Text == itemText)
                {
                    item.Enabled = enabled;
                    break;
                }
            }
        }

        public void AddItem(string text, EventHandler action)
        {
            // in this overload null is for image not used.
            this.Items.Add(text, null, action);
        }

        public void Clear()
        {
            this.Items.Clear();
        }

        public void ShowAt(Control control, Point location)
        {
            this.Show(control, location);
        }

        // this is a pseudo override because the real tag is to ContextMenuStrip which
        // is not externally accessible and the wrapper class is not inheritting from a context menu class

    }

    // Data model for each context menu item behaviour (Header, Line, or Label/Event item)
    public class GridMenuItem
    {
        public string Text { get; set; }

        // Event that will run when an item is clicked. The EventHandler attaches the 
        // (Object sender, EvetnArgs e) to a method
        public EventHandler? Handler { get; set; }

        public bool IsSeparator { get; set; }

        public bool IsHeader { get; set; }

        // Constructor 
        public GridMenuItem(
            string text, 
            EventHandler? handler = null, 
            bool isSeparator = false, 
            bool isHeader = false)
        {
            Text = text;
            Handler = handler;
            IsSeparator = isSeparator;
            IsHeader = isHeader;
        }
    }
}
