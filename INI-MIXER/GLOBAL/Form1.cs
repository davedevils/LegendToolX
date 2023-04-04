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

        public Form1()
        {
            InitializeComponent();

            // Welcome Message
            Console.WriteLine("----------------------------");
            Console.WriteLine("|  Ini-Mixer by DaveDevils |");
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
                if (textBox1.Text == "" || textBox2.Text == "")
                {
                    string boxmsg = "You have forget to put a path or a file !";
                    string boxtitle = "ERROR";
                    MessageBox.Show(boxmsg, boxtitle);
                    return;
                }
                ThreadWorker Tred = new ThreadWorker(
               textBox1.Text, textBox2.Text, security.Checked, textBox3.Text);
                t = new Thread(new ThreadStart(Tred.ThreadMain));

                t.Start();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
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
        }

        private void button2_Click(object sender, EventArgs e)
        {
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
        private bool RenameSecure;
        string HeaderOriginal;
        int ColNumber;
        int onlyrow;

        public ThreadWorker(string original, string data, bool rename, string rownum)
        {
            Path1 = original;
            Path2 = data;
            RenameSecure = rename;
            onlyrow = int.Parse(rownum);
        }

        public void ThreadMain()
        {
            List<string[]> OriginalRow = new List<string[]>();
            List<string[]> DataRow = new List<string[]>();
            GeneralFunc BasicFunc = new GeneralFunc();
            //Ok now we need data bro
            Encoding big5 = Encoding.GetEncoding("big5");
            int lignenumber = 1;
            Console.Write("Triming Original file");
            List<string> file = BasicFunc.TrimFileList(Path1);
            foreach (string line in file)  
            {

                if (line.Count(c => c == '|') != 3 && lignenumber == 1)
                {
                    string boxmsg = "Original file is not a valid Xlegend ini file. \nSorry i will not open it!";
                    string boxtitle = "Error in the INI file";
                    MessageBox.Show(boxmsg, boxtitle);
                    return;
                }

                if (lignenumber != 1 && line.Contains('|') == true)
                {
                    OriginalRow.Add(BasicFunc.CutLineVertical(line));
                }
                else if (lignenumber == 1)
                {
                    string[] returndata = BasicFunc.CutLineVertical(line);
                    ColNumber = Int32.Parse(returndata[2]);
                    HeaderOriginal = line;
                }
                lignenumber++;
            }
            Console.Write(" - done \n");

            lignenumber = 1;
            Console.Write("Triming Data file");
            file = BasicFunc.TrimFileList(Path2);
            foreach (string line in file)
            {

                if (line.Count(c => c == '|') != 3 && lignenumber == 1)
                {
                    string boxmsg = "Data File is not a valid Xlegend ini file. \nSorry i will not open it!";
                    string boxtitle = "Error in the INI file";
                    MessageBox.Show(boxmsg, boxtitle);
                    return;
                }

                if (lignenumber != 1 && line.Contains('|') == true)
                {
                    DataRow.Add(BasicFunc.CutLineVertical(line));
                }
                lignenumber++;
            }
            Console.Write(" - done \n");

            Console.Write("We Enter on the Most long waiting ... Please waiting !\n");

            //Attend stop - We have data now we need be smart
            //First we need ordershit
            Console.Write("Step 1 : Order");
            GFG gg = new GFG();
            Console.Write(" .");
            DataRow.Sort(0, DataRow.Count() - 1, gg);
            Console.Write(".");
            OriginalRow.Sort(0, OriginalRow.Count() - 1, gg);
            Console.Write(".");

            Console.Write(" - Ok \n");
            // Ok On est cramée dans les bytes chico
            Console.Write("Apply of range - ");
            int MinNumber = Int32.Parse(OriginalRow[0][0]);

            if (Int32.Parse(DataRow[0][0]) < MinNumber)
                MinNumber = Int32.Parse(DataRow[0][0]);

            Console.Write("Min : "+ MinNumber.ToString());

            int MaxNumber = Int32.Parse(OriginalRow[OriginalRow.Count() - 1][0]);

            if (Int32.Parse(DataRow[DataRow.Count() - 1][0]) > MaxNumber)
                MaxNumber = Int32.Parse(DataRow[DataRow.Count() - 1][0]);

            Console.Write(" - Max : " + MaxNumber.ToString() + " \n");
            // Ok now just apply
            string lineout = "";
            Console.Write("Final Step - apply to file\n");

            string OuputFile = Path1;

            if (RenameSecure == false)
                OuputFile += ".patched";

            if (File.Exists(OuputFile))
                File.Delete(OuputFile);

            using (System.IO.StreamWriter fileoutput = new System.IO.StreamWriter(OuputFile, true, big5))
            {
                fileoutput.WriteLine(HeaderOriginal);
                for (int i = MinNumber; i < MaxNumber+1; i++)
                {
                    Console.Write("Id " + i);
                    string[] stringout = OriginalRow.Find(x => x[0].Equals(i.ToString()));

                    if (stringout == null )
                        stringout = DataRow.Find(x => x[0].Equals(i.ToString()));

                    if (stringout != null) //match
                    {
                          Console.Write(" - Found");
                          lineout = "";
                          for (int j = 0; j < ColNumber; j++)
                          {
                              if (j == onlyrow)
                              {
                                      string[] stringsure = DataRow.Find(x => x[0].Equals(i.ToString()));
                                      if (stringsure != null) // 75
                                      { 
                                          if (j < stringsure.Length && stringsure[j] != null)
                                              lineout += stringsure[j] + "|";
                                          else
                                              lineout += "|";
                                      }
                                      else
                                      {
                                          if (j < stringout.Length && stringout[j] != null)
                                              lineout += stringout[j] + "|";
                                          else
                                              lineout += "|";
                                      }
                              }
                              else
                              {
                                      if (j < stringout.Length && stringout[j] != null)
                                          lineout += stringout[j] + "|";
                                      else
                                          lineout += "|";
                              }
                          }

                          if (i + 1 < MaxNumber+1)
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
                Console.Write("Finish !\n");
            }
            string boxmsg1 = "Mix have been finish, i hope you will love it !";
            string boxtitle1 = "Mix finished";
            MessageBox.Show(boxmsg1, boxtitle1);
        }
    }
}
