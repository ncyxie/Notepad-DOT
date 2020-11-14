using System;
using System.IO;
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
                try
                {
                    if (Path.GetExtension(path) == ".txt")
                    {
                        notepad.textBox.LoadFile(path, RichTextBoxStreamType.PlainText);
                    }
                    else if (Path.GetExtension(path) != ".txt")
                    {
                        notepad.textBox.LoadFile(path, RichTextBoxStreamType.PlainText);
                        MessageBox.Show("WARNING: You just opened file with Notepad DOT unsupported file format. Your file text may look corrupted or incorrectly displayed. Use this file with Notepad DOT at your own risk.", "Notepad DOT", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    notepad.Text = Path.GetFileName(path) + " - Notepad DOT";

                    notepad.textBox.SelectionStart = notepad.textBox.Text.Length;
                    notepad.textBox.SelectionLength = 0;

                    notepad.isFileAlreadySaved = true;
                    notepad.isFileDirty = false;
                    notepad.currentOpenFileName = path;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            Application.Run(notepad);
        }
    }
}