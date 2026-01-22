using System;
using System.Windows.Forms;
using Timetablez.Models;

namespace Timetablez
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Show splash first
            using (var splash = new Splash())
            {
                splash.ShowDialog();
            }

            // Then open the main form
            Application.Run(new MainForm());
        }
    }
}
