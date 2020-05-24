using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
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

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en");

            statusBar1.Panels[0].Text = "";
            statusBar1.Panels[1].Text = "";
            statusBar1.Panels[2].Text = "";

            offToolStripMenuItem3.Checked = true;
            onToolStripMenuItem2.Checked = false;
            statusBar1.Hide();
            timeToolStripMenuItem.Enabled = false;
            wordCounterToolStripMenuItem.Enabled = false;
            characterCounterToolStripMenuItem.Enabled = false;

            bothToolStripMenuItem.Checked = true;
            textBox.WordWrap = false;
        }

        public void GetSettings()
        {
            textBox.Font = Properties.Settings.Default.Font;
            textBox.ForeColor = Properties.Settings.Default.Color;
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.Font = textBox.Font;
            Properties.Settings.Default.Color = textBox.ForeColor;
            Properties.Settings.Default.Save();
        }

        private void Notepad_Load(object sender, EventArgs e)
        {
            GetSettings();
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

        internal ColorDialog ColorDialog()
        {
            throw new NotImplementedException();
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
                    catch (Exception ex)
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
            using (frmAbout frm = new frmAbout())
            {
                frm.ShowDialog();
            }
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

                SaveSettings();
            }
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
            colorModeToolStripMenuItem.Checked = false;

            Properties.Settings.Default.Theme = "light";

            this.textBox.BackColor = Color.White;
            this.textBox.ForeColor = Color.Black;
        }

        private void darkModeToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            darkModeToolStripMenuItem.Checked = true;
            lightModeToolStripMenuItem.Checked = false;
            blueModeToolStripMenuItem.Checked = false;
            oliveModeToolStripMenuItem.Checked = false;
            pinkModeToolStripMenuItem.Checked = false;
            colorModeToolStripMenuItem.Checked = false;

            Properties.Settings.Default.Theme = "dark";

            this.textBox.BackColor = Color.FromArgb(30, 30, 30);
            this.textBox.ForeColor = Color.White;
        }

        private void blueModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            darkModeToolStripMenuItem.Checked = false;
            lightModeToolStripMenuItem.Checked = false;
            blueModeToolStripMenuItem.Checked = true;
            oliveModeToolStripMenuItem.Checked = false;
            pinkModeToolStripMenuItem.Checked = false;
            colorModeToolStripMenuItem.Checked = false;

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
            colorModeToolStripMenuItem.Checked = false;

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
            colorModeToolStripMenuItem.Checked = false;

            Properties.Settings.Default.Theme = "pink";

            this.textBox.BackColor = Color.FromArgb(255, 192, 203);
            this.textBox.ForeColor = Color.Black;
        }
        private void colorModeToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            darkModeToolStripMenuItem.Checked = false;
            lightModeToolStripMenuItem.Checked = false;
            blueModeToolStripMenuItem.Checked = false;
            oliveModeToolStripMenuItem.Checked = false;
            pinkModeToolStripMenuItem.Checked = false;
            colorModeToolStripMenuItem.Checked = true;

            Properties.Settings.Default.Theme = "color";

            ColorDialog MyDialog = new ColorDialog();

            MyDialog.Color = textBox.BackColor;

            if (MyDialog.ShowDialog() == DialogResult.OK)
                textBox.BackColor = MyDialog.Color;

        }

        private void gitHubReleasesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/ncyxie/Notepad-Dot/releases");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (hourClockToolStripMenuItem.Checked)
            {
                statusBar1.Panels[0].Text = DateTime.Now.ToString("hh:mm tt");
            }
            else if (hourClockToolStripMenuItem1.Checked)
            {
                statusBar1.Panels[0].Text = DateTime.Now.ToString("HH:mm");
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

        private void WordCounter(object sender, EventArgs e)
        {
            string txt = textBox.Text;
            char[] separator = { ' ' };

            int wordsCount = txt.Split(separator, StringSplitOptions.RemoveEmptyEntries).Length;

            statusBar1.Panels[1].Text = "Words: " + wordsCount.ToString();
        }

        private void onToolStripMenuItem_Click(object sender, EventArgs e)
        {
            onToolStripMenuItem.Checked = true;
            offToolStripMenuItem1.Checked = false;
            textBox.TextChanged += WordCounter;
        }

        private void offToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            onToolStripMenuItem.Checked = false;
            offToolStripMenuItem1.Checked = true;
            textBox.TextChanged -= WordCounter;
            statusBar1.Panels[1].Text = "";
        }


        private void CharCounter(object sender, EventArgs e)
        {
            string txt = textBox.Text;

            int charCount = txt.Length;
            statusBar1.Panels[2].Text = "Characters: " + charCount.ToString();
        }

        private void onToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            onToolStripMenuItem1.Checked = true;
            offToolStripMenuItem2.Checked = false;
            textBox.TextChanged += CharCounter;
        }

        private void offToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            onToolStripMenuItem1.Checked = false;
            offToolStripMenuItem2.Checked = true;
            textBox.TextChanged -= CharCounter;
            statusBar1.Panels[2].Text = "";
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            noneToolStripMenuItem.Checked = true;
            bothToolStripMenuItem.Checked = false;
            horizontalToolStripMenuItem.Checked = false;
            verticalToolStripMenuItem.Checked = false;

            textBox.ScrollBars = ScrollBars.None;
            textBox.WordWrap = true;
        }

        private void bothToolStripMenuItem_Click(object sender, EventArgs e)
        {
            noneToolStripMenuItem.Checked = false;
            bothToolStripMenuItem.Checked = true;
            horizontalToolStripMenuItem.Checked = false;
            verticalToolStripMenuItem.Checked = false;

            textBox.ScrollBars = ScrollBars.Both;
            textBox.WordWrap = false;
        }

        private void horizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            noneToolStripMenuItem.Checked = false;
            bothToolStripMenuItem.Checked = false;
            horizontalToolStripMenuItem.Checked = true;
            verticalToolStripMenuItem.Checked = false;

            textBox.ScrollBars = ScrollBars.Horizontal;
            textBox.WordWrap = false;
        }

        private void verticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            noneToolStripMenuItem.Checked = false;
            bothToolStripMenuItem.Checked = false;
            horizontalToolStripMenuItem.Checked = false;
            verticalToolStripMenuItem.Checked = true;

            textBox.ScrollBars = ScrollBars.Vertical;
            textBox.WordWrap = true;
        }

        private void offToolStripMenuItem3_Click_1(object sender, EventArgs e)
        {
            offToolStripMenuItem3.Checked = true;
            onToolStripMenuItem2.Checked = false;
            statusBar1.Hide();
            timeToolStripMenuItem.Enabled = false;
            wordCounterToolStripMenuItem.Enabled = false;
            characterCounterToolStripMenuItem.Enabled = false;
        }

        private void onToolStripMenuItem2_Click_1(object sender, EventArgs e)
        {
            offToolStripMenuItem3.Checked = false;
            onToolStripMenuItem2.Checked = true;
            statusBar1.Show();
            timeToolStripMenuItem.Enabled = true;
            wordCounterToolStripMenuItem.Enabled = true;
            characterCounterToolStripMenuItem.Enabled = true;
        }

        private void timeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void wordCounterToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void characterCounterToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}

