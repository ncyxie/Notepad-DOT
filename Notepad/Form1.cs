using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad
{
    public partial class Notepad : Form
    {
        public frmAbout frmabout;
        string path;
        public Notepad()
        {
            InitializeComponent();
            if (Properties.Settings.Default.Theme == "light")
            {
                this.BackColor = Color.White;
            }
            else
            {
                this.BackColor = Color.FromArgb(30, 30, 30);
            }

            statusBar1.Panels[0].Text = "";
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            path = string.Empty;
            textBox.Clear();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Text Documents|*.txt", ValidateNames = true, Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamReader sr = new StreamReader(ofd.FileName))
                        {
                            path = ofd.FileName;
                            Task<string> text = sr.ReadToEndAsync();
                            textBox.Text = text.Result;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(path))
            {
                using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Text Documents|*.txt", ValidateNames = true })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            path = sfd.FileName;
                            using (StreamWriter sw = new StreamWriter(sfd.FileName))
                            {
                                await sw.WriteLineAsync(textBox.Text);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            else 
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(path))
                    {
                        await sw.WriteLineAsync(textBox.Text);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "Text Documents|*.txt", ValidateNames = true })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter sw = new StreamWriter(sfd.FileName))
                        {
                            await sw.WriteLineAsync(textBox.Text);
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(frmAbout frm=new frmAbout())
            {
                frm.ShowDialog();
            }
        }

        private void Notepad_Load(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void darkModeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        bool CanApplicationClose = false;

        private void Notepad_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CanApplicationClose == false)
            {
                e.Cancel = true;

                DialogResult confirm = MessageBox.Show("Are you sure you want to exit application?", "Exit", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    CanApplicationClose = true;
                    Application.Exit();
                }
            }
        }
        private void textBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void darkModeToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            darkModeToolStripMenuItem.Checked = true;
            lightModeToolStripMenuItem.Checked = false;
            blueModeToolStripMenuItem.Checked = false;
            oliveModeToolStripMenuItem.Checked = false;
            pinkModeToolStripMenuItem.Checked = false;

            Properties.Settings.Default.Theme = "dark";

            this.textBox.BackColor = Color.FromArgb(30, 30, 30);
            this.textBox.ForeColor = Color.White;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.Undo();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.Paste();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.Cut();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.SelectAll();
        }

        private void dateTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.Text = System.DateTime.Now.ToString();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog op = new FontDialog();
            if (op.ShowDialog() == DialogResult.OK)
                textBox.Font = op.Font;
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog op = new ColorDialog();
            if (op.ShowDialog() == DialogResult.OK)
                textBox.ForeColor = op.Color;
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString(this.textBox.Text, this.textBox.Font, Brushes.Black, 10, 25);
        }
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
                printDocument1.Print();
        }

        private void telegramSupportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://t.me/ncyxie");
        }

        private void GithubRepoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/ncyxie/Notepad-Dot");
        }

        private void searchWithGoogleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.google.com/search?q=" + textBox.SelectedText);
        }

        private void searchWithBingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.bing.com/search?q=" + textBox.SelectedText);
        }

        private void searchWithDuckDuckGoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://duckduckgo.com/?q=" + textBox.SelectedText);
        }

        private void lightModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            darkModeToolStripMenuItem.Checked = false;
            lightModeToolStripMenuItem.Checked = true;
            blueModeToolStripMenuItem.Checked = false;
            oliveModeToolStripMenuItem.Checked = false;
            pinkModeToolStripMenuItem.Checked = false;

            Properties.Settings.Default.Theme = "light";

            this.textBox.BackColor = Color.White;
            this.textBox.ForeColor = Color.Black;
        }

        private void blueModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            darkModeToolStripMenuItem.Checked = false;
            lightModeToolStripMenuItem.Checked = false;
            blueModeToolStripMenuItem.Checked = true;
            oliveModeToolStripMenuItem.Checked = false;
            pinkModeToolStripMenuItem.Checked = false;

            Properties.Settings.Default.Theme = "blue";

            this.textBox.BackColor = Color.FromArgb(0, 103, 179);
            this.textBox.ForeColor = Color.White;
        }

        private void oliveModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            darkModeToolStripMenuItem.Checked = false;
            lightModeToolStripMenuItem.Checked = false;
            blueModeToolStripMenuItem.Checked = false;
            oliveModeToolStripMenuItem.Checked = true;
            pinkModeToolStripMenuItem.Checked = false;

            Properties.Settings.Default.Theme = "olive";

            this.textBox.BackColor = Color.FromArgb(107, 142, 35);
            this.textBox.ForeColor = Color.White;
        }

        private void pinkModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            darkModeToolStripMenuItem.Checked = false;
            lightModeToolStripMenuItem.Checked = false;
            blueModeToolStripMenuItem.Checked = false;
            oliveModeToolStripMenuItem.Checked = false;
            pinkModeToolStripMenuItem.Checked = true;

            Properties.Settings.Default.Theme = "pink";

            this.textBox.BackColor = Color.FromArgb(255, 192, 203);
            this.textBox.ForeColor = Color.Black;
        }

        private void gitHubReleasesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/ncyxie/Notepad-Dot/releases");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (hourClockToolStripMenuItem.Checked)
            {
                statusBar1.Panels[0].Text = DateTime.Now.ToString("hh:mm:ss");
            }
            else if (hourClockToolStripMenuItem1.Checked)
            {
                statusBar1.Panels[0].Text = DateTime.Now.ToString("HH:mm:ss");
            }
        }

        private void hourClockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hourClockToolStripMenuItem.Checked = true;
            hourClockToolStripMenuItem1.Checked = false;
            offToolStripMenuItem.Checked = false;

            timer1.Start();
        }

        private void hourClockToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            hourClockToolStripMenuItem.Checked = false;
            hourClockToolStripMenuItem1.Checked = true;
            offToolStripMenuItem.Checked = false;

            timer1.Start();
        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hourClockToolStripMenuItem.Checked = false;
            hourClockToolStripMenuItem1.Checked = false;
            offToolStripMenuItem.Checked = true;

            timer1.Stop();
            statusBar1.Panels[0].Text = "";
        }

    }
 }

