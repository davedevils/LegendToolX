using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TranslateErrorFinder
{
    public partial class Form1 : Form
    {
        Thread t = new Thread(() => Console.WriteLine());


        public Form1()
        {
            InitializeComponent();

            // Welcome Message
            Console.WriteLine("-----------------------------------------");
            Console.WriteLine("|  Translate Error finder by DaveDevils |");
            Console.WriteLine("---------------*************-------------");
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

                ThreadWorker Tred = new ThreadWorker(
               textBox1.Text, maskedTextBox1.Text);
                t = new Thread(new ThreadStart(Tred.ThreadMain));

                t.Start();
            }
        }

        public class ThreadWorker
        {
            int numbofrow;
            string urlfile;
            public ThreadWorker(string urlpath, string numberofrow)
            {
                urlfile = urlpath;
                numbofrow = int.Parse(numberofrow);
            }

            public void ThreadMain()
            {
                int line = 1;
                foreach (var myString in File.ReadAllLines(urlfile))
                {
                    var count = myString.Count(x => x == '|');

                    if (count != numbofrow)
                        Console.WriteLine("ERROR FOUND : Line "+ line + " - There is " + count + " | founded" );

                    line++;
                }
                string boxmsg1 = "Checking Finish !";
                string boxtitle1 = "Checking Finish";
                MessageBox.Show(boxmsg1, boxtitle1);
            }
        }
    }
}
