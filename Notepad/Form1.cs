using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Notepad
{
    public partial class Notepad : Form
    {
        #region fields
        public bool isFileAlreadySaved;
        public bool isFileDirty;
        public string currentOpenFileName;
        #endregion

        bool CanApplicationClose = false;

        public static string FindText = "";
        public static Boolean MatchCase;
        public static string ReplaceText = "";
        int d;

        public frmAbout frmabout;
        public Discord discord;
        public Find find;
        public Replace replace;

        public Notepad()
        {
            InitializeComponent();

            if (!Properties.Settings.Default.windowPosition.IsEmpty)
            {
                Bounds = Properties.Settings.Default.windowPosition;
                StartPosition = FormStartPosition.Manual;
            }

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en");

            statusBar1.Panels[0].Text = "";
            statusBar1.Panels[1].Text = "";
            statusBar1.Panels[2].Text = "";
            statusBar1.Panels[3].Text = "";

            offToolStripMenuItem.Checked = true;
            hourClockToolStripMenuItem.Checked = false;
            hourClockToolStripMenuItem1.Checked = false;
            verticalToolStripMenuItem.Checked = false;
            horizontalToolStripMenuItem.Checked = false;
            bothToolStripMenuItem.Checked = true;
            noneToolStripMenuItem.Checked = false;
            offToolStripMenuItem3.Checked = true;
            onToolStripMenuItem2.Checked = false;
            offToolStripMenuItem4.Checked = true;
            onToolStripMenuItem3.Checked = false;
            onToolStripMenuItem6.Checked = false;
            offToolStripMenuItem7.Checked = true;
            statusBar1.Hide();
            timeToolStripMenuItem.Enabled = false;
            wordCounterToolStripMenuItem.Enabled = false;
            characterCounterToolStripMenuItem.Enabled = false;
            fontToolStripMenuItem2.Enabled = false;
            columnsToolStripMenuItem.Enabled = false;
            capsTrackerToolStripMenuItem.Enabled = false;

            bothToolStripMenuItem.Checked = true;
            textBox.WordWrap = false;

            statusBar1.AutoSize = true;
            statusBar1.Panels[0].AutoSize = StatusBarPanelAutoSize.Contents;
            statusBar1.Panels[1].AutoSize = StatusBarPanelAutoSize.Contents;
            statusBar1.Panels[2].AutoSize = StatusBarPanelAutoSize.Contents;
            statusBar1.Panels[3].AutoSize = StatusBarPanelAutoSize.Contents;

            statusBar1.Panels[0].BorderStyle = StatusBarPanelBorderStyle.None;
            statusBar1.Panels[1].BorderStyle = StatusBarPanelBorderStyle.None;
            statusBar1.Panels[2].BorderStyle = StatusBarPanelBorderStyle.None;
            statusBar1.Panels[3].BorderStyle = StatusBarPanelBorderStyle.None;

            onToolStripMenuItem5.Checked = true;
            offToolStripMenuItem6.Checked = false;

            toolStripSeparator1.Visible = true;
            toolStripSeparator2.Visible = true;
            toolStripSeparator3.Visible = true;
            toolStripSeparator4.Visible = true;
            toolStripSeparator5.Visible = true;
            toolStripSeparator6.Visible = true;
            toolStripSeparator7.Visible = true;

            onToolStripMenuItem4.Checked = false;
            columnsToolStripMenuItem1.Enabled = true;
            offToolStripMenuItem5.Checked = true;
            toolStrip1.Hide();

            onToolStripMenuItem7.Checked = true;
            offToolStripMenuItem8.Checked = false;

            textBox.ContextMenuStrip = contextMenuStrip1;
        }

        public void GetSettings()
        {
            textBox.Font = Properties.Settings.Default.Font;
            textBox.ForeColor = Properties.Settings.Default.Color;
            textBox.BackColor = Properties.Settings.Default.Mode;
            menuStrip1.BackColor = Properties.Settings.Default.menuStripMode;
            menuStrip1.Font = Properties.Settings.Default.menuStripFont;
            menuStrip1.ForeColor = Properties.Settings.Default.menuStripColor;
            textBox.ScrollBars = Properties.Settings.Default.scrollBars;
            statusBar1.Font = Properties.Settings.Default.statusBarFont;
            statusBarPanel1.BorderStyle = Properties.Settings.Default.statusBarColumns;
            statusBarPanel2.BorderStyle = Properties.Settings.Default.statusBarColumns;
            statusBarPanel3.BorderStyle = Properties.Settings.Default.statusBarColumns;
            statusBarPanel4.BorderStyle = Properties.Settings.Default.statusBarColumns;

            onToolStripMenuItem2.Checked = Properties.Settings.Default.statusBar;

            if (onToolStripMenuItem2.Checked == true)
            {
                onToolStripMenuItem2.Checked = true;
                offToolStripMenuItem3.Checked = false;
                statusBar1.Show();
                timeToolStripMenuItem.Enabled = true;
                wordCounterToolStripMenuItem.Enabled = true;
                characterCounterToolStripMenuItem.Enabled = true;
                fontToolStripMenuItem2.Enabled = true;
                columnsToolStripMenuItem.Enabled = true;
                capsTrackerToolStripMenuItem.Enabled = true;
            }

            else
            {
                onToolStripMenuItem2.Checked = false;
                offToolStripMenuItem3.Checked = true;
                statusBar1.Hide();
                timeToolStripMenuItem.Enabled = false;
                wordCounterToolStripMenuItem.Enabled = false;
                characterCounterToolStripMenuItem.Enabled = false;
                fontToolStripMenuItem2.Enabled = false;
                columnsToolStripMenuItem.Enabled = false;
                capsTrackerToolStripMenuItem.Enabled = false;
            }

            hourClockToolStripMenuItem.Checked = Properties.Settings.Default.twelveHours;
            hourClockToolStripMenuItem1.Checked = Properties.Settings.Default.twentyFourHours;
            offToolStripMenuItem.Checked = Properties.Settings.Default.timerOff;

            if (hourClockToolStripMenuItem.Checked == true)
            {
                hourClockToolStripMenuItem.Checked = true;
                hourClockToolStripMenuItem1.Checked = false;
                offToolStripMenuItem.Checked = false;

                timer1.Start();
            }

            else if (hourClockToolStripMenuItem1.Checked == true)
            {
                hourClockToolStripMenuItem.Checked = false;
                hourClockToolStripMenuItem1.Checked = true;
                offToolStripMenuItem.Checked = false;

                timer1.Start();
            }

            else if (offToolStripMenuItem.Checked == true)
            {
                hourClockToolStripMenuItem.Checked = false;
                hourClockToolStripMenuItem1.Checked = false;
                offToolStripMenuItem.Checked = true;

                timer1.Stop();
            }

            onToolStripMenuItem.Checked = Properties.Settings.Default.statusBarWordCounter;

            if (onToolStripMenuItem.Checked == true)
            {
                onToolStripMenuItem.Checked = true;
                offToolStripMenuItem1.Checked = false;
                textBox.TextChanged += WordCounter;
            }

            else
            {
                onToolStripMenuItem.Checked = false;
                offToolStripMenuItem1.Checked = true;
                textBox.TextChanged -= WordCounter;
            }

            onToolStripMenuItem1.Checked = Properties.Settings.Default.statusBarCharCounter;

            if (onToolStripMenuItem1.Checked == true)
            {
                onToolStripMenuItem1.Checked = true;
                offToolStripMenuItem2.Checked = false;
                textBox.TextChanged += CharCounter;
            }

            else
            {
                onToolStripMenuItem1.Checked = false;
                offToolStripMenuItem2.Checked = true;
                textBox.TextChanged -= CharCounter;
            }

            onToolStripMenuItem3.Checked = Properties.Settings.Default.columnOn;

            if (onToolStripMenuItem3.Checked == true)
            {
                onToolStripMenuItem3.Checked = true;
                offToolStripMenuItem4.Checked = false;
            }

            else
            {
                onToolStripMenuItem3.Checked = false;
                offToolStripMenuItem4.Checked = true;
            }

            verticalToolStripMenuItem.Checked = Properties.Settings.Default.verticalScroll;
            horizontalToolStripMenuItem.Checked = Properties.Settings.Default.horizontalScroll;
            bothToolStripMenuItem.Checked = Properties.Settings.Default.bothScroll;
            noneToolStripMenuItem.Checked = Properties.Settings.Default.noneScroll;

            if (verticalToolStripMenuItem.Checked == true)
            {
                noneToolStripMenuItem.Checked = false;
                bothToolStripMenuItem.Checked = false;
                horizontalToolStripMenuItem.Checked = false;
                verticalToolStripMenuItem.Checked = true;

                textBox.WordWrap = true;
            }
            else if (horizontalToolStripMenuItem.Checked == true)
            {
                noneToolStripMenuItem.Checked = false;
                bothToolStripMenuItem.Checked = false;
                horizontalToolStripMenuItem.Checked = true;
                verticalToolStripMenuItem.Checked = false;

                textBox.WordWrap = false;
            }
            else if (bothToolStripMenuItem.Checked == true)
            {
                noneToolStripMenuItem.Checked = false;
                bothToolStripMenuItem.Checked = true;
                horizontalToolStripMenuItem.Checked = false;
                verticalToolStripMenuItem.Checked = false;

                textBox.WordWrap = false;
            }
            else if (noneToolStripMenuItem.Checked == true)
            {
                noneToolStripMenuItem.Checked = true;
                bothToolStripMenuItem.Checked = false;
                horizontalToolStripMenuItem.Checked = false;
                verticalToolStripMenuItem.Checked = false;

                textBox.WordWrap = true;
            }

            lightModeToolStripMenuItem.Checked = Properties.Settings.Default.lightMode;
            darkModeToolStripMenuItem.Checked = Properties.Settings.Default.darkMode;
            blueModeToolStripMenuItem.Checked = Properties.Settings.Default.blueMode;
            pinkModeToolStripMenuItem.Checked = Properties.Settings.Default.pinkMode;
            oliveModeToolStripMenuItem.Checked = Properties.Settings.Default.oliveMode;
            colorModeToolStripMenuItem.Checked = Properties.Settings.Default.colorMode;
            followStripMenuToolStripMenuItem.Checked = Properties.Settings.Default.followStrip;

            if (lightModeToolStripMenuItem.Checked == true)
            {
                darkModeToolStripMenuItem.Checked = false;
                lightModeToolStripMenuItem.Checked = true;
                blueModeToolStripMenuItem.Checked = false;
                oliveModeToolStripMenuItem.Checked = false;
                pinkModeToolStripMenuItem.Checked = false;
                colorModeToolStripMenuItem.Checked = false;
                followStripMenuToolStripMenuItem.Checked = false;

            }
            else if (darkModeToolStripMenuItem.Checked == true)
            {
                darkModeToolStripMenuItem.Checked = true;
                lightModeToolStripMenuItem.Checked = false;
                blueModeToolStripMenuItem.Checked = false;
                oliveModeToolStripMenuItem.Checked = false;
                pinkModeToolStripMenuItem.Checked = false;
                colorModeToolStripMenuItem.Checked = false;
                followStripMenuToolStripMenuItem.Checked = false;
            }
            else if (blueModeToolStripMenuItem.Checked == true)
            {
                darkModeToolStripMenuItem.Checked = false;
                lightModeToolStripMenuItem.Checked = false;
                blueModeToolStripMenuItem.Checked = true;
                oliveModeToolStripMenuItem.Checked = false;
                pinkModeToolStripMenuItem.Checked = false;
                colorModeToolStripMenuItem.Checked = false;
                followStripMenuToolStripMenuItem.Checked = false;
            }
            else if (pinkModeToolStripMenuItem.Checked == true)
            {
                darkModeToolStripMenuItem.Checked = false;
                lightModeToolStripMenuItem.Checked = false;
                blueModeToolStripMenuItem.Checked = false;
                oliveModeToolStripMenuItem.Checked = false;
                pinkModeToolStripMenuItem.Checked = true;
                colorModeToolStripMenuItem.Checked = false;
                followStripMenuToolStripMenuItem.Checked = false;
            }
            else if (oliveModeToolStripMenuItem.Checked == true)
            {
                darkModeToolStripMenuItem.Checked = false;
                lightModeToolStripMenuItem.Checked = false;
                blueModeToolStripMenuItem.Checked = false;
                oliveModeToolStripMenuItem.Checked = true;
                pinkModeToolStripMenuItem.Checked = false;
                colorModeToolStripMenuItem.Checked = false;
                followStripMenuToolStripMenuItem.Checked = false;
            }
            else if (colorModeToolStripMenuItem.Checked == true)
            {
                darkModeToolStripMenuItem.Checked = false;
                lightModeToolStripMenuItem.Checked = false;
                blueModeToolStripMenuItem.Checked = false;
                oliveModeToolStripMenuItem.Checked = false;
                pinkModeToolStripMenuItem.Checked = false;
                colorModeToolStripMenuItem.Checked = true;
                followStripMenuToolStripMenuItem.Checked = false;
            }
            else if (followStripMenuToolStripMenuItem.Checked == true)
            {
                darkModeToolStripMenuItem.Checked = false;
                lightModeToolStripMenuItem.Checked = false;
                blueModeToolStripMenuItem.Checked = false;
                oliveModeToolStripMenuItem.Checked = false;
                pinkModeToolStripMenuItem.Checked = false;
                colorModeToolStripMenuItem.Checked = false;
                followStripMenuToolStripMenuItem.Checked = true;
            }

            lightModeToolStripMenuItem1.Checked = Properties.Settings.Default.lightStrip;
            darkModeToolStripMenuItem1.Checked = Properties.Settings.Default.darkStrip;
            blueModeToolStripMenuItem1.Checked = Properties.Settings.Default.blueStrip;
            pinkModeToolStripMenuItem1.Checked = Properties.Settings.Default.pinkStrip;
            oliveModeToolStripMenuItem1.Checked = Properties.Settings.Default.oliveStrip;
            colorModeToolStripMenuItem1.Checked = Properties.Settings.Default.colorStrip;
            followTextBoxToolStripMenuItem.Checked = Properties.Settings.Default.followTextBox;

            if (lightModeToolStripMenuItem1.Checked == true)
            {
                darkModeToolStripMenuItem1.Checked = false;
                lightModeToolStripMenuItem1.Checked = true;
                blueModeToolStripMenuItem1.Checked = false;
                oliveModeToolStripMenuItem1.Checked = false;
                pinkModeToolStripMenuItem1.Checked = false;
                colorModeToolStripMenuItem1.Checked = false;
                followTextBoxToolStripMenuItem.Checked = false;
            }
            else if (darkModeToolStripMenuItem1.Checked == true)
            {
                darkModeToolStripMenuItem1.Checked = true;
                lightModeToolStripMenuItem1.Checked = false;
                blueModeToolStripMenuItem1.Checked = false;
                oliveModeToolStripMenuItem1.Checked = false;
                pinkModeToolStripMenuItem1.Checked = false;
                colorModeToolStripMenuItem1.Checked = false;
                followTextBoxToolStripMenuItem.Checked = false;
            }
            else if (blueModeToolStripMenuItem1.Checked == true)
            {
                darkModeToolStripMenuItem1.Checked = false;
                lightModeToolStripMenuItem1.Checked = false;
                blueModeToolStripMenuItem1.Checked = true;
                oliveModeToolStripMenuItem1.Checked = false;
                pinkModeToolStripMenuItem1.Checked = false;
                colorModeToolStripMenuItem1.Checked = false;
                followTextBoxToolStripMenuItem.Checked = false;
            }
            else if (pinkModeToolStripMenuItem1.Checked == true)
            {
                darkModeToolStripMenuItem1.Checked = false;
                lightModeToolStripMenuItem1.Checked = false;
                blueModeToolStripMenuItem1.Checked = false;
                oliveModeToolStripMenuItem1.Checked = false;
                pinkModeToolStripMenuItem1.Checked = true;
                colorModeToolStripMenuItem1.Checked = false;
                followTextBoxToolStripMenuItem.Checked = false;
            }
            else if (oliveModeToolStripMenuItem1.Checked == true)
            {
                darkModeToolStripMenuItem1.Checked = false;
                lightModeToolStripMenuItem1.Checked = false;
                blueModeToolStripMenuItem1.Checked = false;
                oliveModeToolStripMenuItem1.Checked = true;
                pinkModeToolStripMenuItem1.Checked = false;
                colorModeToolStripMenuItem1.Checked = false;
                followTextBoxToolStripMenuItem.Checked = false;
            }
            else if (colorModeToolStripMenuItem1.Checked == true)
            {
                darkModeToolStripMenuItem1.Checked = false;
                lightModeToolStripMenuItem1.Checked = false;
                blueModeToolStripMenuItem1.Checked = false;
                oliveModeToolStripMenuItem1.Checked = false;
                pinkModeToolStripMenuItem1.Checked = false;
                colorModeToolStripMenuItem1.Checked = true;
                followTextBoxToolStripMenuItem.Checked = false;
            }
            else if (followTextBoxToolStripMenuItem.Checked == true)
            {
                darkModeToolStripMenuItem1.Checked = false;
                lightModeToolStripMenuItem1.Checked = false;
                blueModeToolStripMenuItem1.Checked = false;
                oliveModeToolStripMenuItem1.Checked = false;
                pinkModeToolStripMenuItem1.Checked = false;
                colorModeToolStripMenuItem1.Checked = false;
                followTextBoxToolStripMenuItem.Checked = true;
            }

            onToolStripMenuItem4.Checked = Properties.Settings.Default.toolStrip;

            if (onToolStripMenuItem4.Checked == true)
            {
                onToolStripMenuItem4.Checked = true;
                columnsToolStripMenuItem1.Enabled = true;
                offToolStripMenuItem5.Checked = false;
                toolStrip1.Show();
            }

            else
            {
                onToolStripMenuItem4.Checked = false;
                columnsToolStripMenuItem1.Enabled = false;
                offToolStripMenuItem5.Checked = true;
                toolStrip1.Hide();
            }

            onToolStripMenuItem5.Checked = Properties.Settings.Default.toolColumnOn;

            if (onToolStripMenuItem5.Checked == true)
            {
                onToolStripMenuItem5.Checked = true;
                offToolStripMenuItem6.Checked = false;

                toolStripSeparator1.Visible = true;
                toolStripSeparator2.Visible = true;
                toolStripSeparator3.Visible = true;
                toolStripSeparator4.Visible = true;
                toolStripSeparator5.Visible = true;
                toolStripSeparator6.Visible = true;
                toolStripSeparator7.Visible = true;
            }

            else
            {
                onToolStripMenuItem5.Checked = false;
                offToolStripMenuItem6.Checked = true;

                toolStripSeparator1.Visible = false;
                toolStripSeparator2.Visible = false;
                toolStripSeparator3.Visible = false;
                toolStripSeparator4.Visible = false;
                toolStripSeparator5.Visible = false;
                toolStripSeparator6.Visible = false;
                toolStripSeparator7.Visible = false;
            }

            onToolStripMenuItem6.Checked = Properties.Settings.Default.capsTracker;

            if (onToolStripMenuItem6.Checked == true)
            {
                onToolStripMenuItem6.Checked = true;
                offToolStripMenuItem7.Checked = false;

                if (Control.IsKeyLocked(Keys.CapsLock))
                {
                    statusBarPanel4.Text = "CAPS ON";
                }

                else
                {
                    statusBarPanel4.Text = "caps off";
                }
            }

            else
            {
                onToolStripMenuItem6.Checked = false;
                offToolStripMenuItem7.Checked = true;

                statusBarPanel4.Text = "";
            }

            onToolStripMenuItem7.Checked = Properties.Settings.Default.contextMenu;

            if (onToolStripMenuItem7.Checked == true)
            {
                onToolStripMenuItem7.Checked = true;
                offToolStripMenuItem8.Checked = false;

                textBox.ContextMenuStrip = contextMenuStrip1;
            }

            else
            {
                onToolStripMenuItem7.Checked = false;
                offToolStripMenuItem8.Checked = true;

                textBox.ContextMenuStrip = new ContextMenuStrip();
            }
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.windowPosition = Bounds;

            Properties.Settings.Default.Font = textBox.Font;
            Properties.Settings.Default.Color = textBox.ForeColor;
            Properties.Settings.Default.Mode = textBox.BackColor;
            Properties.Settings.Default.menuStripMode = menuStrip1.BackColor;
            Properties.Settings.Default.menuStripFont = menuStrip1.Font;
            Properties.Settings.Default.menuStripColor = menuStrip1.ForeColor;
            Properties.Settings.Default.scrollBars = textBox.ScrollBars;
            Properties.Settings.Default.statusBarFont = statusBar1.Font;
            Properties.Settings.Default.statusBarColumns = statusBarPanel1.BorderStyle;
            Properties.Settings.Default.statusBarColumns = statusBarPanel2.BorderStyle;
            Properties.Settings.Default.statusBarColumns = statusBarPanel3.BorderStyle;
            Properties.Settings.Default.statusBarColumns = statusBarPanel4.BorderStyle;

            Properties.Settings.Default.statusBar = onToolStripMenuItem2.Checked;

            Properties.Settings.Default.twelveHours = hourClockToolStripMenuItem.Checked;
            Properties.Settings.Default.twentyFourHours = hourClockToolStripMenuItem1.Checked;
            Properties.Settings.Default.timerOff = offToolStripMenuItem.Checked;

            Properties.Settings.Default.statusBarWordCounter = onToolStripMenuItem.Checked;

            Properties.Settings.Default.statusBarCharCounter = onToolStripMenuItem1.Checked;

            Properties.Settings.Default.columnOn = onToolStripMenuItem3.Checked;

            Properties.Settings.Default.verticalScroll = verticalToolStripMenuItem.Checked;
            Properties.Settings.Default.horizontalScroll = horizontalToolStripMenuItem.Checked;
            Properties.Settings.Default.bothScroll = bothToolStripMenuItem.Checked;
            Properties.Settings.Default.noneScroll = noneToolStripMenuItem.Checked;

            Properties.Settings.Default.lightMode = lightModeToolStripMenuItem.Checked;
            Properties.Settings.Default.darkMode = darkModeToolStripMenuItem.Checked;
            Properties.Settings.Default.blueMode = blueModeToolStripMenuItem.Checked;
            Properties.Settings.Default.pinkMode = pinkModeToolStripMenuItem.Checked;
            Properties.Settings.Default.oliveMode = oliveModeToolStripMenuItem.Checked;
            Properties.Settings.Default.colorMode = colorModeToolStripMenuItem.Checked;
            Properties.Settings.Default.followStrip = followStripMenuToolStripMenuItem.Checked;

            Properties.Settings.Default.lightStrip = lightModeToolStripMenuItem1.Checked;
            Properties.Settings.Default.darkStrip = darkModeToolStripMenuItem1.Checked;
            Properties.Settings.Default.blueStrip = blueModeToolStripMenuItem1.Checked;
            Properties.Settings.Default.pinkStrip = pinkModeToolStripMenuItem1.Checked;
            Properties.Settings.Default.oliveStrip = oliveModeToolStripMenuItem1.Checked;
            Properties.Settings.Default.colorStrip = colorModeToolStripMenuItem1.Checked;
            Properties.Settings.Default.followTextBox = followTextBoxToolStripMenuItem.Checked;

            Properties.Settings.Default.toolStrip = onToolStripMenuItem4.Checked;

            Properties.Settings.Default.toolColumnOn = onToolStripMenuItem5.Checked;

            Properties.Settings.Default.capsTracker = onToolStripMenuItem6.Checked;

            Properties.Settings.Default.contextMenu = onToolStripMenuItem7.Checked;

            Properties.Settings.Default.Save();
        }

        private void Notepad_Load(object sender, EventArgs e)
        {
            isFileAlreadySaved = false;
            isFileDirty = false;
            currentOpenFileName = "";

            GetSettings();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            isFileDirty = true;
            undoToolStripMenuItem.Enabled = true;
            toolStripButton7.Enabled = true;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFile();
        }

        private void NewFile()
        {
            if (isFileDirty)
            {
                DialogResult result = MessageBox.Show("Do you want to save text file / changes?", "Notepad DOT", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

                switch (result)
                {
                    case DialogResult.Yes:
                        SaveFileMenu();
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        return;
                }
            }
            ClearScreen();
            isFileAlreadySaved = false;
            currentOpenFileName = "";
        }

        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewWindowMethod();
        }

        private static void NewWindowMethod()
        {
            System.Diagnostics.Process.Start(Application.ExecutablePath);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf";

            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (Path.GetExtension(openFileDialog.FileName) == ".txt")
                    textBox.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.PlainText);

                if (Path.GetExtension(openFileDialog.FileName) == ".rtf")
                    textBox.LoadFile(openFileDialog.FileName, RichTextBoxStreamType.RichText);

                this.Text = Path.GetFileName(openFileDialog.FileName) + " - Notepad DOT";

                isFileAlreadySaved = true;
                isFileDirty = false;
                currentOpenFileName = openFileDialog.FileName;
            }
        }

        internal ColorDialog ColorDialog()
        {
            throw new NotImplementedException();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileMenu();
        }

        private void SaveFileMenu()
        {
            if (isFileAlreadySaved)
            {
                if (Path.GetExtension(currentOpenFileName) == ".rtf")
                    textBox.SaveFile(currentOpenFileName, RichTextBoxStreamType.RichText);

                if (Path.GetExtension(currentOpenFileName) == ".txt")
                    textBox.SaveFile(currentOpenFileName, RichTextBoxStreamType.PlainText);

                isFileDirty = false;
            }

            else
            {
                if (isFileDirty)
                {
                    SaveAsFileMenu();
                }
                else
                {
                    ClearScreen();
                }
            }
        }

        private void ClearScreen()
        {
            textBox.Clear();
            this.Text = "Untitled - Notepad DOT";
            isFileDirty = false;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAsFileMenu();
        }

        private void SaveAsFileMenu()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf";

            DialogResult result = saveFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (Path.GetExtension(saveFileDialog.FileName) == ".txt")
                    textBox.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.PlainText);

                if (Path.GetExtension(saveFileDialog.FileName) == ".rtf")
                    textBox.SaveFile(saveFileDialog.FileName, RichTextBoxStreamType.RichText);

                this.Text = Path.GetFileName(saveFileDialog.FileName) + " - Notepad DOT";

                isFileAlreadySaved = true;
                isFileDirty = false;
                currentOpenFileName = saveFileDialog.FileName;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isFileDirty)
            {
                if (CanApplicationClose == false)
                {
                    DialogResult result = MessageBox.Show("Do you want to save text file / changes?", "Notepad DOT", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

                    switch (result)
                    {
                        case DialogResult.Yes:
                            SaveFileMenu();
                            break;
                        case DialogResult.No:
                            break;
                        case DialogResult.Cancel:
                            return;
                    }
                }
            }
            CanApplicationClose = true;
            isFileAlreadySaved = false;
            Application.Exit();

            SaveSettings();
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

        private void Notepad_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isFileDirty)
            {
                if (CanApplicationClose == false)
                {
                    e.Cancel = true;
                    DialogResult result = MessageBox.Show("Do you want to save text file / changes?", "Notepad DOT", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

                    switch (result)
                    {
                        case DialogResult.Yes:
                            SaveFileMenu();
                            break;
                        case DialogResult.No:
                            break;
                        case DialogResult.Cancel:
                            return;
                    }
                }
            }
            CanApplicationClose = true;
            isFileAlreadySaved = false;
            Application.Exit();

            SaveSettings();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UndoMethod();
        }

        private void UndoMethod()
        {
            textBox.Undo();
            redoToolStripMenuItem.Enabled = true;
            toolStripButton7.Enabled = false;
            toolStripButton8.Enabled = true;
            undoToolStripMenuItem.Enabled = false;
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RedoMethod();
        }

        private void RedoMethod()
        {
            textBox.Redo();
            redoToolStripMenuItem.Enabled = false;
            toolStripButton7.Enabled = true;
            toolStripButton8.Enabled = false;
            undoToolStripMenuItem.Enabled = true;
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

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteMethod();
        }

        private void DeleteMethod()
        {
            textBox.SelectedText = string.Empty;
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox.SelectAll();
        }

        private void dateTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTimeMethod();
        }

        private void DateTimeMethod()
        {
            textBox.SelectedText = System.DateTime.Now.ToString();
            textBox.SelectionStart = textBox.Text.Length;
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontMenu();
        }

        private void FontMenu()
        {
            FontDialog op = new FontDialog();
            op.Font = textBox.Font;
            if (op.ShowDialog() == DialogResult.OK)
                textBox.Font = op.Font;
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorMenu();
        }

        private void ColorMenu()
        {
            ColorDialog op = new ColorDialog();
            op.Color = textBox.ForeColor;
            if (op.ShowDialog() == DialogResult.OK)
                textBox.ForeColor = op.Color;
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintPreview();
        }

        private void PrintPreview()
        {
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString(this.textBox.Text, this.textBox.Font, Brushes.Black, new PointF(25, 25));
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintMenu();
        }

        private void PrintMenu()
        {
            if (printDialog1.ShowDialog() == DialogResult.OK)
                printDocument1.Print();
        }

        private void telegramSupportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://t.me/ncyxie");
        }

        private void discordSupportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Discord r = new Discord();
            r.ShowDialog();
        }

        private void GithubRepoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/ncyxie/Notepad-DOT");
        }

        private void searchWithGoogleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GoogleMethod();
        }

        private void GoogleMethod()
        {
            System.Diagnostics.Process.Start("https://www.google.com/search?q=" + textBox.SelectedText);
        }

        private void searchWithBingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BingMethod();
        }

        private void BingMethod()
        {
            System.Diagnostics.Process.Start("https://www.bing.com/search?q=" + textBox.SelectedText);
        }

        private void searchWithDuckDuckGoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DuckDuckGoMethod();
        }

        private void DuckDuckGoMethod()
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
            followStripMenuToolStripMenuItem.Checked = false;

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
            followStripMenuToolStripMenuItem.Checked = false;

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
            followStripMenuToolStripMenuItem.Checked = false;

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
            followStripMenuToolStripMenuItem.Checked = false;

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
            followStripMenuToolStripMenuItem.Checked = false;

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
            followStripMenuToolStripMenuItem.Checked = false;

            Properties.Settings.Default.Theme = "color";

            ColorDialog MyDialog = new ColorDialog();

            MyDialog.Color = textBox.BackColor;

            ColorDialog op = new ColorDialog();

            op.Color = textBox.BackColor;
            if (MyDialog.ShowDialog() == DialogResult.OK)
                textBox.BackColor = MyDialog.Color;

        }
        private void followStripMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            darkModeToolStripMenuItem.Checked = false;
            lightModeToolStripMenuItem.Checked = false;
            blueModeToolStripMenuItem.Checked = false;
            oliveModeToolStripMenuItem.Checked = false;
            pinkModeToolStripMenuItem.Checked = false;
            colorModeToolStripMenuItem.Checked = false;
            followStripMenuToolStripMenuItem.Checked = true;

            textBox.BackColor = menuStrip1.BackColor;
        }

        private void gitHubReleasesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/ncyxie/Notepad-DOT/releases");
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

            textBox.ScrollBars = RichTextBoxScrollBars.None;
            textBox.WordWrap = true;
        }

        private void bothToolStripMenuItem_Click(object sender, EventArgs e)
        {
            noneToolStripMenuItem.Checked = false;
            bothToolStripMenuItem.Checked = true;
            horizontalToolStripMenuItem.Checked = false;
            verticalToolStripMenuItem.Checked = false;

            textBox.ScrollBars = RichTextBoxScrollBars.Both;
            textBox.WordWrap = false;
        }

        private void horizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            noneToolStripMenuItem.Checked = false;
            bothToolStripMenuItem.Checked = false;
            horizontalToolStripMenuItem.Checked = true;
            verticalToolStripMenuItem.Checked = false;

            textBox.ScrollBars = RichTextBoxScrollBars.Horizontal;
            textBox.WordWrap = false;
        }

        private void verticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            noneToolStripMenuItem.Checked = false;
            bothToolStripMenuItem.Checked = false;
            horizontalToolStripMenuItem.Checked = false;
            verticalToolStripMenuItem.Checked = true;

            textBox.ScrollBars = RichTextBoxScrollBars.Vertical;
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
            fontToolStripMenuItem2.Enabled = false;
            columnsToolStripMenuItem.Enabled = false;
            capsTrackerToolStripMenuItem.Enabled = false;
        }

        private void onToolStripMenuItem2_Click_1(object sender, EventArgs e)
        {
            offToolStripMenuItem3.Checked = false;
            onToolStripMenuItem2.Checked = true;
            statusBar1.Show();
            timeToolStripMenuItem.Enabled = true;
            wordCounterToolStripMenuItem.Enabled = true;
            characterCounterToolStripMenuItem.Enabled = true;
            fontToolStripMenuItem2.Enabled = true;
            columnsToolStripMenuItem.Enabled = true;
            capsTrackerToolStripMenuItem.Enabled = true;
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

        private void fontToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FontDialog op = new FontDialog();
            op.Font = menuStrip1.Font;
            op.MinSize = 8;
            op.MaxSize = 24;
            if (op.ShowDialog() == DialogResult.OK)
                menuStrip1.Font = op.Font;
        }

        private void fontColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog op = new ColorDialog();
            op.Color = menuStrip1.ForeColor;
            if (op.ShowDialog() == DialogResult.OK)
                menuStrip1.ForeColor = op.Color;
        }

        private void lightModeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            darkModeToolStripMenuItem1.Checked = false;
            lightModeToolStripMenuItem1.Checked = true;
            blueModeToolStripMenuItem1.Checked = false;
            oliveModeToolStripMenuItem1.Checked = false;
            pinkModeToolStripMenuItem1.Checked = false;
            colorModeToolStripMenuItem1.Checked = false;
            followTextBoxToolStripMenuItem.Checked = false;

            menuStrip1.BackColor = Color.White;
            menuStrip1.ForeColor = Color.Black;
        }

        private void darkModeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            darkModeToolStripMenuItem1.Checked = true;
            lightModeToolStripMenuItem1.Checked = false;
            blueModeToolStripMenuItem1.Checked = false;
            oliveModeToolStripMenuItem1.Checked = false;
            pinkModeToolStripMenuItem1.Checked = false;
            colorModeToolStripMenuItem1.Checked = false;
            followTextBoxToolStripMenuItem.Checked = false;

            menuStrip1.BackColor = Color.FromArgb(30, 30, 30);
            menuStrip1.ForeColor = Color.White;
        }

        private void blueModeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            darkModeToolStripMenuItem1.Checked = false;
            lightModeToolStripMenuItem1.Checked = false;
            blueModeToolStripMenuItem1.Checked = true;
            oliveModeToolStripMenuItem1.Checked = false;
            pinkModeToolStripMenuItem1.Checked = false;
            colorModeToolStripMenuItem1.Checked = false;
            followTextBoxToolStripMenuItem.Checked = false;

            menuStrip1.BackColor = Color.FromArgb(0, 103, 179);
            menuStrip1.ForeColor = Color.White;
        }

        private void pinkModeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            darkModeToolStripMenuItem1.Checked = false;
            lightModeToolStripMenuItem1.Checked = false;
            blueModeToolStripMenuItem1.Checked = false;
            oliveModeToolStripMenuItem1.Checked = false;
            pinkModeToolStripMenuItem1.Checked = true;
            colorModeToolStripMenuItem1.Checked = false;
            followTextBoxToolStripMenuItem.Checked = false;

            menuStrip1.BackColor = Color.FromArgb(255, 192, 203);
            menuStrip1.ForeColor = Color.Black;
        }

        private void oliveModeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            darkModeToolStripMenuItem1.Checked = false;
            lightModeToolStripMenuItem1.Checked = false;
            blueModeToolStripMenuItem1.Checked = false;
            oliveModeToolStripMenuItem1.Checked = true;
            pinkModeToolStripMenuItem1.Checked = false;
            colorModeToolStripMenuItem1.Checked = false;
            followTextBoxToolStripMenuItem.Checked = false;

            menuStrip1.BackColor = Color.FromArgb(107, 142, 35);
            menuStrip1.ForeColor = Color.White;
        }

        private void colorModeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            darkModeToolStripMenuItem1.Checked = false;
            lightModeToolStripMenuItem1.Checked = false;
            blueModeToolStripMenuItem1.Checked = false;
            oliveModeToolStripMenuItem1.Checked = false;
            pinkModeToolStripMenuItem1.Checked = false;
            colorModeToolStripMenuItem1.Checked = true;
            followTextBoxToolStripMenuItem.Checked = false;

            ColorDialog MyDialog = new ColorDialog();

            MyDialog.Color = menuStrip1.BackColor;

            ColorDialog op = new ColorDialog();

            op.Color = menuStrip1.BackColor;
            if (MyDialog.ShowDialog() == DialogResult.OK)
                menuStrip1.BackColor = MyDialog.Color;

        }

        private void followTextBoxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            darkModeToolStripMenuItem1.Checked = false;
            lightModeToolStripMenuItem1.Checked = false;
            blueModeToolStripMenuItem1.Checked = false;
            oliveModeToolStripMenuItem1.Checked = false;
            pinkModeToolStripMenuItem1.Checked = false;
            colorModeToolStripMenuItem1.Checked = false;
            followTextBoxToolStripMenuItem.Checked = true;

            menuStrip1.BackColor = textBox.BackColor;
        }

        private void fontToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            FontDialog op = new FontDialog();
            op.Font = statusBar1.Font;
            op.MinSize = 8;
            op.MaxSize = 16;
            if (op.ShowDialog() == DialogResult.OK)
                statusBar1.Font = op.Font;
        }

        private void columnsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void onToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            onToolStripMenuItem3.Checked = true;
            offToolStripMenuItem4.Checked = false;
            statusBar1.Panels[0].BorderStyle = StatusBarPanelBorderStyle.Sunken;
            statusBar1.Panels[1].BorderStyle = StatusBarPanelBorderStyle.Sunken;
            statusBar1.Panels[2].BorderStyle = StatusBarPanelBorderStyle.Sunken;
            statusBar1.Panels[3].BorderStyle = StatusBarPanelBorderStyle.Sunken;
        }

        private void offToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            onToolStripMenuItem3.Checked = false;
            offToolStripMenuItem4.Checked = true;
            statusBar1.Panels[0].BorderStyle = StatusBarPanelBorderStyle.None;
            statusBar1.Panels[1].BorderStyle = StatusBarPanelBorderStyle.None;
            statusBar1.Panels[2].BorderStyle = StatusBarPanelBorderStyle.None;
            statusBar1.Panels[3].BorderStyle = StatusBarPanelBorderStyle.None;
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindMethod();
        }

        private void FindMethod()
        {
            Find r = new Find();
            r.ShowDialog();

            if (FindText != "")
            {
                d = textBox.Find(FindText);
            }

            if (d < 0)
            {
                MessageBox.Show("No results found", "Find", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void findNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindNextMethod();
        }

        private void FindNextMethod()
        {
            if (FindText != "")
            {
                if (MatchCase == true)
                {
                    d = textBox.Find(FindText, (d + 1), textBox.Text.Length, RichTextBoxFinds.MatchCase);

                }
                else
                {
                    d = textBox.Find(FindText, (d + 1), textBox.Text.Length, RichTextBoxFinds.None);
                }

            }

            if (d <= 0)
            {
                MessageBox.Show("No results found", "Find Next", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplaceMethod();
        }

        private void ReplaceMethod()
        {
            Replace r = new Replace();
            r.ShowDialog();

            if (FindText != "")
            {
                if (MatchCase == true)
                {
                    d = textBox.Find(FindText, RichTextBoxFinds.MatchCase);
                    textBox.SelectedText = ReplaceText;
                }
                else
                {
                    d = textBox.Find(FindText, RichTextBoxFinds.None);
                    textBox.SelectedText = ReplaceText;
                }
            }

            if (d < 0)
            {
                MessageBox.Show("No results found", "Replace", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void replaceAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplaceAllMethod();
        }

        private void ReplaceAllMethod()
        {
            if (ReplaceText != "")
            {
                textBox.Text = textBox.Text.Replace(FindText, ReplaceText);
            }

            else
            {
                MessageBox.Show("No results found", "Replace All", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void onToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            onToolStripMenuItem4.Checked = true;
            offToolStripMenuItem5.Checked = false;
            columnsToolStripMenuItem1.Enabled = true;
            toolStrip1.Show();
        }

        private void offToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            onToolStripMenuItem4.Checked = false;
            offToolStripMenuItem5.Checked = true;
            columnsToolStripMenuItem1.Enabled = false;
            toolStrip1.Hide();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            NewFile();
        }

        private void toolStripButton26_Click(object sender, EventArgs e)
        {
            NewWindowMethod();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            SaveFileMenu();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            SaveAsFileMenu();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            PrintMenu();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            PrintPreview();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            UndoMethod();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            RedoMethod();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            textBox.Cut();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            textBox.Copy();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            textBox.Paste();
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            DeleteMethod();
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            FindMethod();
        }

        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            FindNextMethod();
        }

        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            ReplaceMethod();
        }

        private void toolStripButton17_Click(object sender, EventArgs e)
        {
            ReplaceAllMethod();
        }

        private void toolStripButton18_Click(object sender, EventArgs e)
        {
            FontMenu();
        }

        private void toolStripButton19_Click(object sender, EventArgs e)
        {
            ColorMenu();
        }

        private void toolStripButton20_Click(object sender, EventArgs e)
        {
            textBox.SelectAll();
        }

        private void toolStripButton21_Click(object sender, EventArgs e)
        {
            DateTimeMethod();
        }

        private void columnsToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void onToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            onToolStripMenuItem5.Checked = true;
            offToolStripMenuItem6.Checked = false;

            toolStripSeparator1.Visible = true;
            toolStripSeparator2.Visible = true;
            toolStripSeparator3.Visible = true;
            toolStripSeparator4.Visible = true;
            toolStripSeparator5.Visible = true;
            toolStripSeparator6.Visible = true;
            toolStripSeparator7.Visible = true;
        }

        private void offToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            onToolStripMenuItem5.Checked = false;
            offToolStripMenuItem6.Checked = true;

            toolStripSeparator1.Visible = false;
            toolStripSeparator2.Visible = false;
            toolStripSeparator3.Visible = false;
            toolStripSeparator4.Visible = false;
            toolStripSeparator5.Visible = false;
            toolStripSeparator6.Visible = false;
            toolStripSeparator7.Visible = false;
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                statusBarPanel4.Text = "CAPS ON";
            }

            else
            {
                statusBarPanel4.Text = "caps off";
            }
        }

        private void capsTrackerToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void onToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            onToolStripMenuItem6.Checked = true;
            offToolStripMenuItem7.Checked = false;

            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                statusBarPanel4.Text = "CAPS ON";
            }

            else
            {
                statusBarPanel4.Text = "caps off";
            }
        }

        private void offToolStripMenuItem7_Click(object sender, EventArgs e)
        {
            onToolStripMenuItem6.Checked = false;
            offToolStripMenuItem7.Checked = true;

            statusBarPanel4.Text = "";
        }

        private void regularToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Regular();
        }

        private void Regular()
        {
            textBox.SelectionFont = new Font(textBox.Font, FontStyle.Regular);
        }

        private void boldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bold();
        }

        private void Bold()
        {
            textBox.SelectionFont = new Font(textBox.Font, FontStyle.Bold);
        }

        private void italicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Italic();
        }

        private void Italic()
        {
            textBox.SelectionFont = new Font(textBox.Font, FontStyle.Italic);
        }

        private void underlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Underline();
        }

        private void Underline()
        {
            textBox.SelectionFont = new Font(textBox.Font, FontStyle.Underline);
        }

        private void strikeoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Strikeout();
        }

        private void Strikeout()
        {
            textBox.SelectionFont = new Font(textBox.Font, FontStyle.Strikeout);
        }

        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            Bold();
        }

        private void toolStripButton22_Click(object sender, EventArgs e)
        {
            Italic();
        }

        private void toolStripButton23_Click(object sender, EventArgs e)
        {
            Underline();
        }

        private void toolStripButton24_Click(object sender, EventArgs e)
        {
            Strikeout();
        }

        private void toolStripButton25_Click(object sender, EventArgs e)
        {
            Regular();
        }

        private void undoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            UndoMethod();
        }

        private void redoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RedoMethod();
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            textBox.Cut();
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            textBox.Copy();
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            textBox.Paste();
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DeleteMethod();
        }

        private void selectAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            textBox.SelectAll();
        }

        private void dateTimeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DateTimeMethod();
        }

        private void regularToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Regular();
        }

        private void boldToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Bold();
        }

        private void italicToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Italic();
        }

        private void underlineToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Underline();
        }

        private void strikeoutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Strikeout();
        }

        private void searchWithGoogleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GoogleMethod();
        }

        private void searchWithBingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BingMethod();
        }

        private void searchWithDuckDuckGoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DuckDuckGoMethod();
        }

        private void onToolStripMenuItem7_Click(object sender, EventArgs e)
        {
            onToolStripMenuItem7.Checked = true;
            offToolStripMenuItem8.Checked = false;

            textBox.ContextMenuStrip = contextMenuStrip1;
        }

        private void offToolStripMenuItem8_Click(object sender, EventArgs e)
        {
            onToolStripMenuItem7.Checked = false;
            offToolStripMenuItem8.Checked = true;

            textBox.ContextMenuStrip = new ContextMenuStrip();
        }
    }
}

