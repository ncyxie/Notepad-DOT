using System;
using System.IO;
using System.Windows.Forms;

namespace Notepad
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main(string[] args)
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
                        notepad.textBox.LoadFile(path, RichTextBoxStreamType.PlainText);

                    if (Path.GetExtension(path) == ".rtf")
                        notepad.textBox.LoadFile(path, RichTextBoxStreamType.RichText);

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