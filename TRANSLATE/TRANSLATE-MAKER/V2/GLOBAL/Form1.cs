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
        public Form1()
        {
            InitializeComponent();

            // Welcome Message
            Console.WriteLine("----------------------------");
            Console.WriteLine("|  Translate Maker by DD's |");
            Console.WriteLine("----------------------------");
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
                if (textBox1.Text == "")
                {
                    string boxmsg = "You have forget to put a path !";
                    string boxtitle = "ERROR";
                    MessageBox.Show(boxmsg, boxtitle);
                    return;
                }


                fileslistOri = new List<string>(Directory.GetFiles(textBox1.Text));

                ThreadWorker Tred = new ThreadWorker(
               textBox1.Text, fileslistOri, security.Checked, true, removeduplicate.Checked);
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

        }

        private void security_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
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
            return Convert.ToInt64(x[0]).CompareTo(Convert.ToInt64(y[0]));

        }
    }

    public class ThreadWorker
    {
        private string Path1;
        private string Path1o;
        private bool RenameSecure;
        private bool delduplicate;
        string HeaderOriginal;
        string HeaderTrans;
        string ColHeaderTrans;
        int ColNumberOri;
        List<string> FileListOld;
        int[] TransColKeep;
        bool allowjumped;


        public ThreadWorker(string original, List<string> oldlist, bool rename, bool allowjump, bool removeduplicate)
        {
            Path1 = original.ToLower();
            Path1o = original;
            RenameSecure = rename;
            FileListOld = oldlist;
            allowjumped = allowjump;
            delduplicate = removeduplicate;
        }

        public void ThreadMain()
        {
            for (int i = 0; i < FileListOld.Count; i++)
            {
                FileListOld[i] = FileListOld[i].ToLower();
                if (FileListOld[i].Contains("c_") || FileListOld[i].Contains("s_"))
                {
                    FileListOld[i] = FileListOld[i].Replace(Path1, "").Replace("\\", "");

                    if (FileListOld[i].StartsWith("c_") || FileListOld[i].StartsWith("s_") )
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


            bool fail = false;

            foreach (string newTextfile in FileListOld)
            {
                fail = false;
                int exists = FileListOld.FindIndex(x => x.StartsWith(newTextfile));

                if (exists < 0)
                    continue;

                Console.Write("new file - " + newTextfile);

                List<string[]> OriginalRow = new List<string[]>();
                GeneralFunc BasicFunc = new GeneralFunc();
                //Ok now we need data bro
                Encoding big5 = Encoding.GetEncoding("big5");
                //bool istranslatefile = false;
                int lignenumber = 1;
                Console.Write("Triming Original file");
                List<string> file = BasicFunc.TrimFileList(Path1o + "/" + newTextfile, allowjumped);
                foreach (string line in file)
                {

                    if (line.Count(c => c == '|') != 3 && lignenumber == 1)
                    {
                        string boxmsg = "Original file is not a valid Xlegend ini file. \nSorry i will not open it!";
                        //string boxtitle = "Error in the INI file";
                        //MessageBox.Show(boxmsg, boxtitle);
                        Console.Write(boxmsg);
                        fail = true;
                        continue;
                    }

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
                    

                    lignenumber++;
                }
                Console.Write(" - done \n");

                if (fail == true)
                    continue;

                Console.Write("We Enter on the Most long waiting ... Please waiting !\n");

                if(OriginalRow.Count() < 1)
                    continue;

                //Attend stop - We have data now we need be smart
                //First we need ordershit
                Console.Write("Step 1 : Order");

                // Remove duplicated data
                if (delduplicate == true)
                {
                    OriginalRow = OriginalRow.Distinct().ToList();
                }

                GFG gg = new GFG();
                Console.Write(".");
                OriginalRow.Sort(0, OriginalRow.Count(), gg);
                Console.Write(".");

                Console.Write(" - Ok \n");
                // Ok On est cramée dans les bytes chico
                Console.Write("Apply of range - ");
                int MinNumber = Int32.Parse(OriginalRow[0][0]);

                if (Int32.Parse(OriginalRow[0][0]) < MinNumber)
                    MinNumber = Int32.Parse(OriginalRow[0][0]);

                Console.Write("Min : " + MinNumber.ToString());

                int MaxNumber = Int32.Parse(OriginalRow[OriginalRow.Count() - 1][0]);

                if (Int32.Parse(OriginalRow[OriginalRow.Count() - 1][0]) > MaxNumber)
                    MaxNumber = Int32.Parse(OriginalRow[OriginalRow.Count() - 1][0]);

                Console.Write(" - Max : " + MaxNumber.ToString() + " \n");

                // Now check if T file exist

                string TNewName = newTextfile;

                if (newTextfile.Split('_').Count() > 1)
                    TNewName = newTextfile.Split('_')[1];

                TNewName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\TModel\\T_" + TNewName;

                if (!File.Exists(TNewName))
                {
                    Console.Write(" - Skipped File\n");
                    continue;
                }

                 System.IO.StreamReader Transfile = new System.IO.StreamReader(TNewName);
                string transline;
                int Z = 1;
                while ((transline = Transfile.ReadLine()) != null)
                {
                    if (Z == 1)
                    {
                        string[] returndata = BasicFunc.CutLineVertical(transline);
                        if(returndata[0].Contains('v'))
                            HeaderTrans = returndata[1] + "|";
                        else
                            HeaderTrans = returndata[0] + "|";
                    }
                    else if (Z == 2)
                    {
                        ColHeaderTrans = transline;
                        string[] splitted = BasicFunc.CutLineVertical(ColHeaderTrans);

                        TransColKeep = new int[splitted.Length];

                        for (int i = 0; i < splitted.Length; i++)
                        {
                            if (splitted[i] != "")
                                TransColKeep[i] = int.Parse(splitted[i]);
                            else
                                TransColKeep[i] = 999999;
                        }
                    }
                    Z++;
                }
                Transfile.Close();

                // Ok now just apply
                string lineout = "";
                Console.Write("Final Step - apply to file\n");

                string OuputFile = "";
                if (RenameSecure == false)
                {
                    OuputFile = Path1o + "/" + newTextfile;
                }
                else
                {
                    if (!Directory.Exists(Path1o + "\\..\\OUT"))
                    {
                        Directory.CreateDirectory(Path1o + "\\..\\OUT");
                    }

                    OuputFile = Path1o + "\\..\\OUT\\" + newTextfile;
                }

                OuputFile = OuputFile.Replace("c_", "t_").Replace("s_", "t_");

                if (File.Exists(OuputFile))
                    File.Delete(OuputFile);

                using (System.IO.StreamWriter fileoutput = new System.IO.StreamWriter(OuputFile, true, big5))
                {
                    fileoutput.WriteLine(HeaderTrans);
                    fileoutput.WriteLine(ColHeaderTrans);

                    int i = 0;
                    foreach (string[] Row in OriginalRow)
                    {
                        int idnumber = Int32.Parse(Row[0]);
                        Console.Write("Id " + idnumber);
                        string[] stringout = Row;// OriginalRow.Find(x => x[0].Equals(i.ToString()));

                        if (stringout != null) 
                        {
                            Console.Write(" - Found");
                            lineout = "";
                            for (int j = 0; j < ColNumberOri; j++)
                            {
                                if (TransColKeep.Contains(j+1))
                                {
                                    if (j < stringout.Length && stringout[j] != null)
                                        lineout += stringout[j] + "|";
                                    else
                                        lineout += "|";
                                }
                            }

                            if (OriginalRow.Count > i + 1)
                                fileoutput.WriteLine(lineout);
                            else
                                fileoutput.Write(lineout);

                            Console.Write(" - Writed.\n");
                        }
                        else
                        {
                            Console.Write(" - NotFound\n");
                        }
                        i++;
                    }
                }

                if (allowjumped == true)
                {
                    string text = File.ReadAllText(OuputFile, big5);
                    text = text.Replace("/?/?/?", "\r\n");
                    File.WriteAllText(OuputFile, text, big5);
                }

                Console.Write("Finish !\n");

            }
            string boxmsg1 = "Cleaning Finish !";
            string boxtitle1 = "Cleaning Finish";
            MessageBox.Show(boxmsg1, boxtitle1);
        }
    }
}
