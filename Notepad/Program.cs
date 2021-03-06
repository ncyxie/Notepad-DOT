using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Notepad
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Notepad notepad = new Notepad();
            if (args.Length > 0)
            {
                string path = args[0];

                StreamReader sr = new StreamReader(path, Encoding.UTF8);
                notepad.textBox.Text = sr.ReadToEnd();
                sr.Close();

                if (Path.GetExtension(path) != ".txt")
                {
                    MessageBox.Show("WARNING: You just opened file with Notepad DOT unsupported file format. Your file text may look corrupted or incorrectly displayed. Use this file with Notepad DOT at your own risk.", "Notepad DOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                notepad.Text = Path.GetFileName(path) + " - Notepad DOT";

                notepad.textBox.SelectionStart = notepad.textBox.Text.Length;
                notepad.textBox.SelectionLength = 0;

                notepad.currentOpenFileName = path;
            }
            Application.Run(notepad);
        }
    }
}