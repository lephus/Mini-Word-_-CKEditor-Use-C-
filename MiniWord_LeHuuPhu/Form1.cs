using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniWord_LeHuuPhu
{
    public partial class Form1 : Form
    {
        LeHuuPhu clsPhu = new LeHuuPhu();
        //string startupPath = Directory.GetParent(Environment.CurrentDirectory.TrimEnd(Path.DirectorySeparatorChar)).FullName;
        public Form1()
        {
            InitializeComponent();
            ReadFont();
            clsPhu.readData();
            updatePartFile();
            if(clsPhu.PATH_SAVE != "")
            {
                richTextBox.LoadFile(clsPhu.PATH_SAVE);
            }
            SetFontAndSizeRichTextBox();
            DrawIconEmojiBoard();
            timer2.Start();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ButtonOpen_Click();
        }

        // READ FONT SYSTEM
        private void ReadFont()
        {
            foreach (FontFamily item in FontFamily.Families)
            {
                toolStripComboBoxFont.Items.Add(item.Name);
            }
        }
        // DRAW ICON EMOJI
        void DrawIconEmojiBoard()
        {
            clsPhu.matrixButton = new List<List<Button>>();
            Button oldButton = new Button() { Width = 0, Location = new Point(0, 0) };
            int count = -1;
            for (int i = 0; i < 6; i++)
            {
                clsPhu.matrixButton.Add(new List<Button>());
                for (int j = 0; j < 9; j++)
                {
                    count++;
                    //Console.WriteLine(count);
                    Button btn = new Button()
                    {
                        Tag = i.ToString(),
                        Width = clsPhu.width_button,
                        Height = clsPhu.height_button,
                        Location = new Point(oldButton.Location.X + oldButton.Width, oldButton.Location.Y)
                    };
                    btn.BackColor = Color.White;
                    btn.BackgroundImage = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject(count+"");
                    flowLayoutPanel.Controls.Add(btn);
                    btn.Tag = count;
                    btn.Click += btnActionEvenClick;
                    oldButton = btn;
                    clsPhu.matrixButton[i].Add(btn);
                }
                oldButton.Location = new Point(0, oldButton.Location.Y + clsPhu.height_button);
                oldButton.Width = 0;
                oldButton.Height = 0;
            }
        }
        private void btnActionEvenClick(object sender, EventArgs e)
        {
            timer1.Start();
            Button button = sender as Button;
            //Console.WriteLine(button.Tag);
            Clipboard.SetImage((System.Drawing.Image)Properties.Resources.ResourceManager.GetObject(button.Tag + ""));
            richTextBox.Paste();
        }

        // OPEN FILE
        private void ButtonOpen_Click()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    clsPhu.PATH_SAVE = openFileDialog.FileName;
                    updatePartFile();
                    Console.WriteLine(openFileDialog.FileName);
                    richTextBox.LoadFile(openFileDialog.FileName);
                    clsPhu.writeData(openFileDialog.FileName);
                }
            }
            catch (Exception)
            {
                string message = "Định dạng file không được hỗ trợ!";
                string title = "THÔNG BÁO LỖI";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
            }
        }
        // Shows the use of a SaveFileDialog to save a MemoryStream to a file.
        private void ButtonSave_Click()
        {
            if (clsPhu.PATH_SAVE == "")
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "Rich Text format (*.RTF)|*.rtf|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.FileName = "MiniWord_Phu.rtf";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    clsPhu.PATH_SAVE = saveFileDialog1.FileName;
                    updatePartFile();
                    Console.WriteLine(saveFileDialog1.FileName);
                    richTextBox.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.RichText);
                    clsPhu.writeData(saveFileDialog1.FileName);
                }

            }
            else
            {
                Console.WriteLine(clsPhu.PATH_SAVE);
                updatePartFile();
                richTextBox.SaveFile(clsPhu.PATH_SAVE, RichTextBoxStreamType.RichText);
            }
        }
       // SAVE FILE
        private void saveCtrlSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ButtonSave_Click();
        }
        // SAVE AS FILE
        private void ButtonSaveAs_Click()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Rich Text format (*.RTF)|*.rtf|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = "MiniWord_Phu.rtf";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                clsPhu.PATH_SAVE = saveFileDialog1.FileName;
                updatePartFile();
                Console.WriteLine(saveFileDialog1.FileName);
                richTextBox.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.RichText);
                clsPhu.writeData(saveFileDialog1.FileName);
            }
        }

        private void saveAsCtrlShiftSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ButtonSaveAs_Click();
        }
        // UPDATE VIEW PATH
        private void updatePartFile()
        {
            timer1.Start();
            toolStripStatusLabel3.Text = "      |     "+clsPhu.PATH_SAVE+ "      |     " ;
            toolStripStatusLabel3.ForeColor = Color.Blue;
            if (clsPhu.PATH_SAVE == "")
            {
                toolStripStatusLabel3.Text = "      |     " + "Chưa lưu" + "      |     ";
                toolStripStatusLabel3.ForeColor = Color.Red;
            }
        }
        // SAVE FILE
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ButtonSave_Click();
        }
        // SET UP MER
        private void timer1_Tick(object sender, EventArgs e)
        {
            runProcess();
        }

        // ACTIVITY TIMER
        private void runProcess()
        {
            toolStripProgressBar1.Value += 5;
            if(toolStripProgressBar1.Value >= toolStripProgressBar1.Maximum)
            {
                timer1.Stop();
                toolStripProgressBar1.Value = 0;
            }
        }

        // NEW FILE
        private void newCtrlNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Bạn có muốn lưu thay đổi file soạn thảo trước khi new file mới không?";
            string title = "THÔNG BÁO";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
            if(result == DialogResult.Yes)
            {
                ButtonSave_Click();
                clsPhu.PATH_SAVE = "";
                clsPhu.writeData("");
                richTextBox.Clear();
                updatePartFile();
            }
            else
            {
                clsPhu.PATH_SAVE = "";
                clsPhu.writeData("");
                richTextBox.Clear();
                updatePartFile();
            }
        }

        // CLOSE FILE
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Bạn có muốn lưu thay đổi trước khi đóng file?";
            string title = "THÔNG BÁO";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                ButtonSave_Click();
                clsPhu.PATH_SAVE = "";
                clsPhu.writeData("");
                richTextBox.Clear();
                updatePartFile();
            }
            else
            {
                clsPhu.PATH_SAVE = "";
                clsPhu.writeData("");
                richTextBox.Clear();
                updatePartFile();
            }
        }

        // EXIT FILE
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "Bạn có muốn lưu thay đổi trước khi đóng ứng dụng?";
            string title = "THÔNG BÁO";
            MessageBoxButtons buttons = MessageBoxButtons.YesNoCancel;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                ButtonSave_Click();
                Application.Exit();
            }
            if (result == DialogResult.No)
            {
                Application.Exit();
            }
        }


        // ACTION UNDO
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.Undo();
        }
        
        // ACTION REDO
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.Redo();
        }

        private void contr(object sender, EventArgs e)
        {

        }

        // SHOW STRIP
        private void contextMenuStrip_Action(object sender, EventArgs e)
        {
            contextMenuStrip.Show();
        }
        // ACTION COPPY
        private void toolStripButtonCoppy_Click(object sender, EventArgs e)
        {
            ActionCoppy();
        }
        // ACTION CUT
        private void toolStripButtonCut_Click(object sender, EventArgs e)
        {
            ActionCut();
        }

        // ACTION PASTE
        private void toolStripButtonPaste_Click(object sender, EventArgs e)
        {
            ActionPaste();
        }

        private void coppyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActionCoppy();
        }
      
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActionPaste();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActionCut();
        }




        // SET FONT AND SIZE
        private void SetFontAndSizeRichTextBox()
        {
            int fontSize = 12;
            try
            {
                fontSize = int.Parse(toolStripComboBoxSize.Text);
            }
            catch (Exception)
            {
                string message = "Bạn nhập không đúng định dạng số?";
                string title = "CẢNH BÁO";
                MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
            }

            try
            {
                richTextBox.SelectionFont = new Font(toolStripComboBoxFont.Text, fontSize);
            }
            catch (Exception)
            {
                string message = "Không tìm thấy font chữ này?";
                string title = "CẢNH BÁO";
                MessageBoxButtons buttons = MessageBoxButtons.OKCancel;
                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
            }        
        }
        private void ActionCoppy()
        {
            try { 
                toolStripButtonPaste.Enabled = true;
                pasteToolStripMenuItem.Enabled = true;
                Console.WriteLine(richTextBox.SelectedText);
                Clipboard.SetText(richTextBox.SelectedText);
            }
            catch (Exception) { }
        }
        private void ActionCut()
        {
            try
            {
                toolStripButtonPaste.Enabled = true;
                pasteToolStripMenuItem.Enabled = true;
                Clipboard.SetText(richTextBox.SelectedText);
                richTextBox.Cut();
            }
            catch (Exception){}
            
        }
        private void ActionPaste()
        {
            try
            {
                richTextBox.Paste();
            }
            catch (Exception){}
}

        private void SelectedIndexChange(object sender, EventArgs e)
        {
            SetFontAndSizeRichTextBox();
        }

        // SET FONT BOLD
        private void toolStripButtonBold_Click(object sender, EventArgs e)
        {
            if (richTextBox.SelectionFont != null)
            {
                System.Drawing.Font currentFont = richTextBox.SelectionFont;
                System.Drawing.FontStyle newFontStyle;

                if (richTextBox.SelectionFont.Bold == true)
                {
                    newFontStyle = FontStyle.Regular;
                }
                else
                {
                    newFontStyle = FontStyle.Bold;
                }

                richTextBox.SelectionFont = new Font(
                   currentFont.FontFamily,
                   currentFont.Size,
                   newFontStyle
                );
            }
        }

        // SET FONT ITALIC
        private void toolStripButtonItalic_Click(object sender, EventArgs e)
        {
            if (richTextBox.SelectionFont != null)
            {
                System.Drawing.Font currentFont = richTextBox.SelectionFont;
                System.Drawing.FontStyle newFontStyle;

                if (richTextBox.SelectionFont.Italic == true)
                {
                    newFontStyle = FontStyle.Regular;
                }
                else
                {
                    newFontStyle = FontStyle.Italic;
                }

                richTextBox.SelectionFont = new Font(
                   currentFont.FontFamily,
                   currentFont.Size,
                   newFontStyle
                );
            }
        }


        // SET FONT UNDERLINE
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (richTextBox.SelectionFont != null)
            {
                System.Drawing.Font currentFont = richTextBox.SelectionFont;
                System.Drawing.FontStyle newFontStyle;

                if (richTextBox.SelectionFont.Underline == true)
                {
                    newFontStyle = FontStyle.Regular;
                }
                else
                {
                    newFontStyle = FontStyle.Underline;
                }

                richTextBox.SelectionFont = new Font(
                   currentFont.FontFamily,
                   currentFont.Size,
                   newFontStyle
                );
            }
        }


        //SET TEXT COLOR
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.ShowDialog();
            if(richTextBox.SelectedText == "")
            {
                richTextBox.ForeColor = dlg.Color;
            }
            else
            {
                richTextBox.SelectionColor = dlg.Color;
            }
        }

        // SET HIGHT LINE COLOR
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.ShowDialog();
            richTextBox.SelectionBackColor = dlg.Color;
        }

        // SET BACKGROUD COLOR IN RICH TEXT BOX
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.ShowDialog();
            richTextBox.BackColor = dlg.Color;
        }


        // INSERT IMAGE
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.png; *.bmp)|*.jpg; *.jpeg; *.gif; *.png; *.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Clipboard.SetImage(Image.FromFile(ofd.FileName));
                    richTextBox.Paste();
                }
            }
        }
        // SET FONT COLOR
        private void textColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.ShowDialog();
            if (richTextBox.SelectedText == "")
            {
                richTextBox.ForeColor = dlg.Color;
            }
            else
            {
                richTextBox.SelectionColor = dlg.Color;
            }
        }
        //INSERT IMAGE
        private void insertImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.png; *.bmp)|*.jpg; *.jpeg; *.gif; *.png; *.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Clipboard.SetImage(Image.FromFile(ofd.FileName));
                    richTextBox.Paste();
                }
            }
        }
        // ACTION FIND 
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Nhập nội dung bạn muốn tìm kiếm", "Tìm kiếm", "Tìm gì đó...", 300, 300);
            //Console.WriteLine(input);
            int resp = clsPhu.Find(richTextBox, input, Color.Yellow);
            string message = resp + " kết quả được tìm thấy !";
            string title = "KẾT QUẢ TÌM KIẾM";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Information);
        }
        // REPLACE TEXT
        private void toolStripButtonReaplace_Click(object sender, EventArgs e)
        {
           clsPhu.FindAndReplace(richTextBox, toolStripTextBoxFindWhat.Text, toolStripTextBoxReplaceWith.Text, Color.Yellow);
            string message = "'"+toolStripTextBoxFindWhat.Text + "' đã được thay thế thành '"+ toolStripTextBoxReplaceWith.Text+"'";
            string title = "KẾT QUẢ THAY THẾ";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Information);
        }
        // SELECT ALL TEXT
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            richTextBox.SelectAll();
            richTextBox.SelectionBackColor = Color.Transparent;
        }
        // SELECT ALL TEXT
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            richTextBox.SelectAll();
        }
        // ZOOM IN
        private void ActionZoomIn()
        {
            
            float zoom = richTextBox.ZoomFactor;
            if (zoom * 2 < 64)
                richTextBox.ZoomFactor = zoom * 2;
            if (hScrollBar2.Value < 200)
            {
                hScrollBar2.Value += 20;
                label3.Text = hScrollBar2.Value + "%";
            }
        }
        // ZOOM OUT
        private void ActionZoomOut()
        {
            float zoom = richTextBox.ZoomFactor;
            if (zoom / 2 > 0.015625)
                richTextBox.ZoomFactor = zoom / 2;
            if(hScrollBar2.Value >= 20)
            {
                hScrollBar2.Value -= 20;
                label3.Text = hScrollBar2.Value + "%";
            }
               
        }
        // CLICK BUTTON ZOOM IN
        private void ZoomIn_Click(object sender, EventArgs e)
        {
            ActionZoomIn();
        }
        // CLICK BUTTON ZOOM OUT
        private void ZoomOut_Click(object sender, EventArgs e)
        {
            ActionZoomOut();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                if (richTextBox.SelectedText == "")
                {
                    if (toolStripTextBoxFindWhat.Text == "" && toolStripTextBoxReplaceWith.Text == "")
                    {
                        toolStripButtonReaplace.Enabled = false;
                    }
                    else
                    {
                        toolStripButtonReaplace.Enabled = true;
                    }

                    if (richTextBox.SelectionFont.Underline == true)
                    {
                        toolStripButton1.BackColor = Color.Blue;
                    }
                    if (richTextBox.SelectionFont.Underline == false)
                    {
                        toolStripButton1.BackColor = Color.Transparent;
                    }
                    if (richTextBox.SelectionFont.Bold == true)
                    {
                        toolStripButtonBold.BackColor = Color.Blue;
                    }
                    if (richTextBox.SelectionFont.Bold == false)
                    {
                        toolStripButtonBold.BackColor = Color.Transparent;
                    }
                    if (richTextBox.SelectionFont.Italic == true)
                    {
                        toolStripButtonItalic.BackColor = Color.Blue;
                    }
                    if (richTextBox.SelectionFont.Italic == false)
                    {
                        toolStripButtonItalic.BackColor = Color.Transparent;
                    }
                }


                if (richTextBox.Text == "" || richTextBox.SelectedText == "")
                {
                    toolStripButtonCut.Enabled = false;
                    toolStripButtonCoppy.Enabled = false;
                    coppyToolStripMenuItem.Enabled = false;
                    cutToolStripMenuItem.Enabled = false;
                }
                if (richTextBox.SelectedText != "")
                {
                    toolStripButtonCut.Enabled = true;
                    toolStripButtonCoppy.Enabled = true;
                    coppyToolStripMenuItem.Enabled = true;
                    cutToolStripMenuItem.Enabled = true;
                }
               
            } catch (Exception){}
        }
    }
}
