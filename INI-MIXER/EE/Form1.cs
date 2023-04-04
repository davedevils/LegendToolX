using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ini_Mixer
{
    public partial class Form1 : Form
    {
        GeneralFunc BasicFunc = new GeneralFunc();
        Thread t = new Thread(() => Console.WriteLine());

        List<string> fileslistOri;
        List<string> fileslistNew;
        public Form1()
        {
            InitializeComponent();

            // Welcome Message
            Console.WriteLine("-------------------------------");
            Console.WriteLine("|  Ini-Mixer Edited for EE |");
            Console.WriteLine("-------------------------------");
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (t.IsAlive == true)
            {
                string boxmsg = "You need wait other file finish before !";
                string boxtitle = "ERROR";
                MessageBox.Show(boxmsg, boxtitle);
            }
            else
            {
                //Small check for dumb
                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    string boxmsg = "You have forget to put a path !";
                    string boxtitle = "ERROR";
                    MessageBox.Show(boxmsg, boxtitle);
                    return;
                }




                ThreadWorker Tred = new ThreadWorker(
               textBox1.Text, textBox2.Text, fileslistOri, fileslistNew, security.Checked, jump.Checked);
                t = new Thread(new ThreadStart(Tred.ThreadMain));

                t.Start();
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    fileslistOri = new List<string>(Directory.GetFiles(fbd.SelectedPath));
                    textBox1.Text = fbd.SelectedPath;


                }
            }
            /*
            this.openFileDialog1.Filter = "XLegend ini (*.ini)|*.ini";
            this.openFileDialog1.Multiselect = false;
            this.openFileDialog1.Title = "Find Original Ini File";
            this.openFileDialog1.FileName = "Any X-Legend DB ini file";
            DialogResult ininode_files = this.openFileDialog1.ShowDialog();
            if (ininode_files == System.Windows.Forms.DialogResult.OK)
            {

                string nameoutput = openFileDialog1.FileName;
                textBox1.Text = nameoutput;
            }
            */
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    fileslistNew = new List<string>(Directory.GetFiles(fbd.SelectedPath));
                    textBox2.Text = fbd.SelectedPath;


                }
            }
            /*
            this.openFileDialog2.Filter = "XLegend ini (*.ini)|*.ini";
            this.openFileDialog2.Multiselect = false;
            this.openFileDialog2.Title = "Find Ini with more data";
            this.openFileDialog2.FileName = "Any X-Legend DB ini file";
            DialogResult ininode_files = this.openFileDialog2.ShowDialog();
            if (ininode_files == System.Windows.Forms.DialogResult.OK)
            {

                string nameoutput = openFileDialog2.FileName;
                textBox2.Text = nameoutput;
            }
            */
        }

        private void security_CheckedChanged(object sender, EventArgs e)
        {

        }
    }

    class GFG : IComparer<string[]>
    {
        public int Compare(string[] x, string[] y)
        {
            if (x == null || y == null)
            {
                return 0;
            }
            return Convert.ToUInt32(x[0]).CompareTo(Convert.ToUInt32(y[0]));

        }
    }

    public class ThreadWorker
    {
        private string Path1;
        private string Path2;
        private string Path1o;
        private string Path2o;
        private bool RenameSecure;
        string HeaderOriginal;
        string HeaderTransOriginal;
        string HeaderData;
        int ColNumber;
        int ColNumberOri;
        List<string> FileListOld;
        List<string> FileListNew;
        bool allowedjump;



        public ThreadWorker(string original, string data, List<string> oldlist, List<string> newlist, bool rename, bool allowjump)
        {
            Path1 = original.ToLower();
            Path2 = data.ToLower();
            Path1o = original;
            Path2o = data;
            RenameSecure = rename;
            FileListOld = oldlist;
            FileListNew = newlist;
            allowedjump = allowjump;
        }

        public void ThreadMain()
        {
            for (int i = 0; i < FileListOld.Count; i++)
            {
                FileListOld[i] = FileListOld[i].ToLower();
                if (FileListOld[i].Contains("c_") || FileListOld[i].Contains("s_") || FileListOld[i].Contains("t_"))
                {
                    FileListOld[i] = FileListOld[i].Replace(Path1, "").Replace("\\", "");

                    if (FileListOld[i].StartsWith("c_") || FileListOld[i].StartsWith("s_") || FileListOld[i].StartsWith("t_"))
                    {
                        // Bon fichier
                    }
                    else
                    { 
                    FileListOld[i] = ""; 
                    }
                }
                else
                    FileListOld[i] = "";
            }
            FileListOld.RemoveAll(str => string.IsNullOrEmpty(str));

            for (int i = 0; i < FileListNew.Count; i++)
            {
                FileListNew[i] = FileListNew[i].ToLower();
                if (FileListNew[i].Contains("c_") || FileListNew[i].Contains("s_") || FileListNew[i].Contains("t_"))
                {

                    FileListNew[i] = FileListNew[i].Replace(Path2, "").Replace("\\", "");

                    if (FileListNew[i].StartsWith("c_") || FileListNew[i].StartsWith("s_") || FileListNew[i].StartsWith("t_"))
                    { 
                        // Bon fichier
                    }
                    else
                    {
                        FileListNew[i] = "";
                    }

                }
                else
                    FileListNew[i] = "";
            }
            FileListNew.RemoveAll(str => string.IsNullOrEmpty(str));

            bool fail = false;

            foreach (string newTextfile in FileListOld)
            {
                fail = false;
                int exists = FileListNew.FindIndex(x => x.StartsWith(newTextfile));

                if (exists < 0)
                    continue;

                Console.Write("new file - " + newTextfile);

                List<string[]> OriginalRow = new List<string[]>();
                List<string[]> DataRow = new List<string[]>();
                GeneralFunc BasicFunc = new GeneralFunc();
                //Ok now we need data bro
                Encoding big5 = Encoding.GetEncoding("big5");
                int lignenumber = 1;
                Console.Write("Triming Original file");
                List<string> file = BasicFunc.TrimFileList(Path1o + "/" + newTextfile, allowedjump);
                bool istranslatefile = false;
                foreach (string line in file)
                {

                    if (newTextfile.Contains("t_"))
                        istranslatefile = true;

                    if (line.Count(c => c == '|') != 3 && lignenumber == 1 && istranslatefile == false)
                    {
                        string boxmsg = "Original file is not a valid Xlegend ini file. \nSorry i will not open it!";
                        string boxtitle = "Error in the INI file";
                        //MessageBox.Show(boxmsg, boxtitle);
                        Console.Write(boxmsg);
                        fail = true;
                        continue;
                    }

                    if (istranslatefile == false)
                    {
                        if (lignenumber != 1 && line.Contains('|') == true)
                        {
                            OriginalRow.Add(BasicFunc.CutLineVertical(line));
                        }
                        else if (lignenumber == 1)
                        {
                            string[] returndata = BasicFunc.CutLineVertical(line);
                            ColNumberOri = Int32.Parse(returndata[2]);
                            HeaderOriginal = line;
                        }
                    }
                    else
                    {
                        if (lignenumber != 1 && lignenumber != 2 && line.Contains('|') == true)
                        {
                            OriginalRow.Add(BasicFunc.CutLineVertical(line));
                        }
                        else if (lignenumber == 1)
                        {
                            string[] returndata = BasicFunc.CutLineVertical(line);
                            ColNumberOri = Int32.Parse(returndata[0]);
                            HeaderOriginal = line;
                        }
                        else if (lignenumber == 2)
                        {
                            HeaderTransOriginal = line;
                        }
                    }
                    lignenumber++;
                }
                Console.Write(" - done \n");

                if (fail == true)
                    continue;

                lignenumber = 1;
                Console.Write("Triming Data file");
                file = BasicFunc.TrimFileList(Path2o + "/" + newTextfile, allowedjump);
                foreach (string line in file)
                {

                    if (newTextfile.Contains("t_"))
                        istranslatefile = true;

                    if (line.Count(c => c == '|') != 3 && lignenumber == 1 && istranslatefile == false)
                    {
                        string boxmsg = "Data File is not a valid Xlegend ini file. \nSorry i will not open it!";
                        string boxtitle = "Error in the INI file";
                        // MessageBox.Show(boxmsg, boxtitle);
                        Console.Write(boxmsg);
                        fail = true;
                    }


                    if (istranslatefile == false)
                    {
                        if (lignenumber != 1 && line.Contains('|') == true)
                        {
                            DataRow.Add(BasicFunc.CutLineVertical(line));
                        }
                        else if (lignenumber == 1)
                        {
                            string[] returndata = BasicFunc.CutLineVertical(line);
                            ColNumber = Int32.Parse(returndata[2]);
                            HeaderData = line;
                        }
                    }
                    else
                    {
                        if (lignenumber != 1 && lignenumber != 2 && line.Contains('|') == true)
                        {
                            DataRow.Add(BasicFunc.CutLineVertical(line));
                        }
                        else if (lignenumber == 1)
                        {
                            string[] returndata = BasicFunc.CutLineVertical(line);
                            ColNumber = Int32.Parse(returndata[0]);
                            HeaderData = line;
                        }
                        else if (lignenumber == 2)
                        {
                            HeaderTransOriginal = line;
                        }
                    }


                    lignenumber++;
                }
                Console.Write(" - done \n");

                if (fail == true)
                    continue;

                Console.Write("We Enter on the Most long waiting ... Please waiting !\n");

                if(DataRow.Count() < 1)
                    continue;

                if (OriginalRow.Count() < 1)
                    continue;


                //Attend stop - We have data now we need be smart
                //First we need ordershit
                Console.Write("Step 1 : Order");

                // Remove duplicated data
                //if (delduplicate == true)
                {
                    OriginalRow = OriginalRow.Distinct().ToList();
                    DataRow = DataRow.Distinct().ToList();
                }

                GFG gg = new GFG();
                Console.Write(" .");
                DataRow.Sort(0, DataRow.Count(), gg);
                Console.Write(".");
                OriginalRow.Sort(0, OriginalRow.Count(), gg);
                Console.Write(".");

                Console.Write(" - Ok \n");
                // Ok On est cramée dans les bytes chico
                Console.Write("Apply of range - ");
                int MinNumber = Int32.Parse(OriginalRow[0][0]);

                if (Int32.Parse(DataRow[0][0]) < MinNumber)
                    MinNumber = Int32.Parse(DataRow[0][0]);

                Console.Write("Min : " + MinNumber.ToString());

                int MaxNumber = Int32.Parse(OriginalRow[OriginalRow.Count() - 1][0]);

                if (Int32.Parse(DataRow[DataRow.Count() - 1][0]) > MaxNumber)
                    MaxNumber = Int32.Parse(DataRow[DataRow.Count() - 1][0]);

                Console.Write(" - Max : " + MaxNumber.ToString() + " \n");
                // Ok now just apply
                string lineout = "";
                Console.Write("Final Step - apply to file\n");

                string OuputFile = "";
                if (RenameSecure == false)
                {
                    OuputFile = Path1o + "/" + newTextfile + ".patched";
                }
                else
                {
                    if (!Directory.Exists(Path1o + "\\..\\OUT"))
                    {
                        Directory.CreateDirectory(Path1o + "\\..\\OUT");
                    }

                    OuputFile = Path1o + "\\..\\OUT\\" + newTextfile;
                }

                if (File.Exists(OuputFile))
                    File.Delete(OuputFile);

                using (System.IO.StreamWriter fileoutput = new System.IO.StreamWriter(OuputFile, true, big5))
                {
                    fileoutput.WriteLine(HeaderData);

                    if (istranslatefile == true)
                        fileoutput.WriteLine(HeaderTransOriginal);

                    for (int i = MinNumber; i < MaxNumber + 1; i++)
                    {
                        Console.Write("Id " + i);
                        string[] stringin = OriginalRow.Find(x => x[0].Equals(i.ToString()));
                        string[] stringout = DataRow.Find(x => x[0].Equals(i.ToString()));

                        if (stringout != null || stringin != null) 
                        {
                            Console.Write(" - Found");
                            lineout = "";
                            for (int j = 0; j < ColNumber; j++)
                            {
                                if (stringin != null && stringout != null)
                                {
                                    if (j < stringin.Length && stringin[j] != null)
                                        lineout += stringin[j] + "|";
                                    else if (j < stringout.Length && stringout[j] != null)
                                        lineout += stringout[j] + "|";
                                    else
                                        lineout += "|";
                                }
                                else if (stringin == null)
                                {
                                    if (j < stringout.Length && stringout[j] != null)
                                        lineout += stringout[j] + "|";
                                    else
                                        lineout += "|";
                                }
                                else if (stringout == null)
                                {
                                    if (j < stringin.Length && stringin[j] != null)
                                        lineout += stringin[j] + "|";
                                    else
                                        lineout += "|";
                                }
                            }

                            if (i + 1 < MaxNumber + 1)
                                fileoutput.WriteLine(lineout);
                            else
                                fileoutput.Write(lineout);

                            Console.Write(" - Writed.\n");
                        }
                        else
                        {
                            Console.Write(" - NotFound\n");
                        }

                    }
                }

                if (allowedjump == true)
                {
                    string text = File.ReadAllText(OuputFile, big5);
                    text = text.Replace("/?/?/?", "\r\n");
                    File.WriteAllText(OuputFile, text, big5);
                }
                Console.Write("Finish !\n");
            }
            string boxmsg1 = "Mix have been finish, i hope you will love it !";
            string boxtitle1 = "Mix finished";
            MessageBox.Show(boxmsg1, boxtitle1);
        }
    }
}
