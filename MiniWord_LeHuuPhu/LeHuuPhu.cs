using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniWord_LeHuuPhu
{
    class LeHuuPhu
    {
        public String PATH_DATABASE = @"E:\databaseMiniWord.txt";
        public String PATH_SAVE = "";
        public bool PASTE_IS_READY = false;
        public List<List<Button>> matrixButton;
        public int width_button = 40;
        public int height_button = 40;
        public void readData()
        {
            try
            {
                if (File.Exists(PATH_DATABASE))
                {
                    using (StreamReader sr = File.OpenText(PATH_DATABASE))
                    {
                        String s = "";
                        while ((s = sr.ReadLine()) != null)
                        {
                            Console.WriteLine("part : " + s);
                            PATH_SAVE = s;
                        }
                    }
                }
                else
                {
                    PATH_SAVE = "";
                    FileStream fs = File.Create(PATH_DATABASE);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }
        public void writeData(String partNew)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(PATH_DATABASE))
                {
                    writer.Write(PATH_SAVE);
                }
            }
            catch (Exception exp)
            {
                Console.Write(exp.Message);
            }
        }

        // FUNCTION FIND
        public int Find(RichTextBox rtb, String textBox1, Color color)
        {
            int count = 0;
            string[] words = textBox1.Split(',');
            foreach (string word in words)
            {
                int startindex = 0;
                while (startindex < rtb.TextLength)
                {
                    int wordstartIndex = rtb.Find(word, startindex, RichTextBoxFinds.None);
                    if (wordstartIndex != -1)
                    {
                        rtb.SelectionStart = wordstartIndex;
                        rtb.SelectionLength = word.Length;
                        rtb.SelectionBackColor = Color.Yellow;
                        count++;
                    }
                    else
                        break;
                    startindex += wordstartIndex + word.Length;
                }
            }
            return count;
        }

        // FUNCTION REPLACE
        public void FindAndReplace(RichTextBox rtb, String textBox1,String Replace, Color color)
        {
            rtb.Text = rtb.Text.Replace(textBox1, Replace);
        }
    }
}
