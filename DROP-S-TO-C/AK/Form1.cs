using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DropRateChanger
{
    public partial class Form1 : Form
    {
        System.Globalization.NumberStyles style;
        System.Globalization.CultureInfo culture;
        public Form1()
        {
            style = System.Globalization.NumberStyles.Number | System.Globalization.NumberStyles.AllowCurrencySymbol;
            culture = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");
            InitializeComponent();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
        public string[] CutLine(string line)
        {
            string[] returndata = { "0", "0" };
            if (!string.IsNullOrEmpty(line))
            {
                returndata = line.Split('|');

            }

            return returndata;
        }

        public bool CreatePath(string path)
        {
            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {
                    return true;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
                return false;
            }
        }

        private void Trimit(string filename)
        {
            string line = "";
            Encoding big5 = Encoding.GetEncoding("big5");

            System.IO.StreamReader file = new System.IO.StreamReader(filename, big5, true);
            string nameoutput = Path.GetFileName(filename);

            if (File.Exists("tmp.conv"))
            {
                File.Delete("tmp.conv");
            }

            //Now Read line by Line
            int lignenumber = 1;
            int maxint = 0;
            while ((line = file.ReadLine()) != null)
            {

                if (line.Count(c => c == '|') < 3 && lignenumber == 1)
                    break;

                using (System.IO.StreamWriter fileoutput = new System.IO.StreamWriter("tmp.conv", true, big5))
                {
                    if (lignenumber != 1 && line.Contains('|') == true && maxint != 0)
                    {
                        string[] returndata = CutLine(line);
                        line = "";

                        int maxline = returndata.Count() - 1;

                        for (int i = 0; i < maxline; i++)
                        {

                            if (i == 0)
                            {
                                int no = 0;
                                float nof = 0.0f;
                                if (int.TryParse(returndata[i], out no))
                                {
                                    //it's number OK no problem
                                    line += "\r\n";
                                }
                                else if (Single.TryParse(returndata[i], style, culture, out nof))
                                {
                                    //it's number OK no problem
                                    line += "\r\n";
                                }
                                else
                                {
                                    returndata[i] = returndata[i].Replace("\r\n", "");
                                }
                            }

                            line += returndata[i] + "|";
                        }
                    }
                    else if (lignenumber == 1)
                    {

                        string[] returndata = CutLine(line);
                        line = "";

                        for (int i = 0; i < 3; i++)
                        {
                            if (i == 2)
                            {
                                maxint = int.Parse(returndata[i]) - 1;
                            }

                            line += returndata[i] + "|";
                        }
                    }

                    fileoutput.Write(line);
                }

                lignenumber++;
            }
            file.Close();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "S_DropItem.ini|*.ini";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    MessageBox.Show("Fix the file wait please");
                    string line = "";
                    Encoding big5 = Encoding.GetEncoding("big5");
                    string path = Path.GetDirectoryName(openFileDialog1.FileName.ToString());
                    CreatePath(path);
                    Trimit(openFileDialog1.FileName.ToString());
                    System.IO.StreamReader file = new System.IO.StreamReader("tmp.conv", big5, true);
                    string nameoutput = Path.GetFileName(openFileDialog1.FileName.ToString());

                    if (File.Exists(path + "\\C_DropItem.ini"))
                    {
                        File.Delete(path + "\\C_DropItem.ini");
                    }

                    //Now Read line by Line
                    int lignenumber = 1;
                    int whilelimit = 1;
                    while ((line = file.ReadLine()) != null)
                    {
                        using (System.IO.StreamWriter fileoutput = new System.IO.StreamWriter(path + "\\C_DropItem.ini", true, big5))
                        {
                            //DropLuck
                            int DropCol = 14;
                            if (lignenumber == 1)
                            {
                                string[] returndata = CutLine(line);
                                int.TryParse(returndata[2], out whilelimit);
                            }

                            if (lignenumber != 1 && line.Contains('|') == true)
                            {
                                string[] returndata = CutLine(line);
                                line = "";
                                for (int i = 0; i < whilelimit; i++)
                                {
                                    //match with Coll of drop
                                    if (i == DropCol)
                                    {
                                        if (returndata[i].Trim() != "")
                                        {

                                            DateTime date = new DateTime();
                                            System.Random random = new Random(i + (int)date.Ticks + lignenumber);
                                            System.Random random2 = new Random(6969 + i + (int)date.Ticks + lignenumber);
                                            Decimal Content = Decimal.Parse(returndata[i], CultureInfo.InvariantCulture.NumberFormat);
                                            Content = random2.Next(5, 79) * (decimal)random.NextDouble();
                                            returndata[i] = Content.ToString("0.00", CultureInfo.InvariantCulture.NumberFormat);

                                        }
                                        // check in 4 coll
                                        DropCol = DropCol + 4;
                                    }
                                    line += returndata[i] + "|";
                                }

                            }

                            fileoutput.WriteLine(line);
                        }

                        lignenumber++;
                    }
                    file.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Files can be read ... error: " + ex.Message);
                }

                if (File.Exists("tmp.conv"))
                {
                    File.Delete("tmp.conv");
                }

                MessageBox.Show("File have been protect !");
            }
        }

    }
}
