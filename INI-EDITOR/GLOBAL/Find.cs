using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ini_Editor
{
    public partial class Find : Form
    {
        public static Form1 MainForm;
        private List<Form1.MemoryFind> FindList;
        private int LastFind = 0;
        public Find(Form1 MainF)
        {
            MainForm = MainF;
            InitializeComponent();
            this.Opacity = 100;
            CollList.Items.Add("All Columns");

            if(Form1.ColumnsHeader != null)
                foreach (String ColumnsName in Form1.ColumnsHeader)
                    CollList.Items.Add(ColumnsName);

            CollList.SelectedIndex = 0;

            CollList2.Items.Add("All Columns");

            if (Form1.ColumnsHeader != null)
                foreach (String ColumnsName in Form1.ColumnsHeader)
                    CollList2.Items.Add(ColumnsName);

            CollList2.SelectedIndex = 0;
        }

        private void SetTransparancy()
        {
            float transparancy = Convert.ToSingle(trackBar1.Value) / 100;
            if (transparancy > 0.10)
                this.Opacity = transparancy;
            else
                this.Opacity = 0.10;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox2.Checked = true;
                SetTransparancy();
            }
            else
            {
                checkBox2.Checked = false;
                this.Opacity = 100;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox1.Checked = true;
                SetTransparancy();
            }
            else
            {
                checkBox1.Checked = false;
                this.Opacity = 100;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            trackBar2.Value = trackBar1.Value;
            if (checkBox1.Checked == true)
            {
                SetTransparancy();
            }
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            trackBar1.Value = trackBar2.Value;
            if (checkBox2.Checked == true)
            {
                SetTransparancy();
            }
        }
        private void loopreplace_CheckedChanged(object sender, EventArgs e)
        {
            if (loopreplace.Checked == true)
                loopfind.Checked = true;
            else
                loopfind.Checked = false;
        }

        private void loopfind_CheckedChanged(object sender, EventArgs e)
        {
            if (loopfind.Checked == true)
                loopreplace.Checked = true;
            else
                loopreplace.Checked = false;
        }

        private void casefind_CheckedChanged(object sender, EventArgs e)
        {
            if (casefind.Checked == true)
                casereplace.Checked = true;
            else
                casereplace.Checked = false;
        }

        private void casereplace_CheckedChanged(object sender, EventArgs e)
        {
            if (casereplace.Checked == true)
                casefind.Checked = true;
            else
                casefind.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FindText();
        }

        private void Find_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void searchtextbox_TextChanged(object sender, EventArgs e)
        {
            //Empty The list
            FindList = new List<Form1.MemoryFind>();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CountText();
        }

        private void CountText()
        {
            if (FindList == null || FindList.Count() < 1)
            {
                FindText();
                resultfind.Text = "Value found  " + FindList.Count() + " time";
            }
            else
            {
                resultfind.Text = "Value found  " + FindList.Count() + " time";
            }

        }
        private void FindText()
        {
            if(FindList == null || FindList.Count() < 1)
            {
                String searchValue = searchtextbox.Text;
                FindList = MainForm.FindAllInCell(searchValue, CollList.Text, casefind.Checked);

                if (FindList.Count() > 0)
                {
                    //focus on cell
                    MainForm.FocusOnCell(FindList[0].Row, FindList[0].Coll);
                    resultfind.Text = "Value found on Row " + (FindList[0].Row + 1);
                    LastFind = 0;
                }
                else
                    resultfind.Text = "Value not found :( ...";
            }
            else
            {
                if (FindList.Count() > LastFind)
                {
                    MainForm.FocusOnCell(FindList[LastFind].Row, FindList[LastFind].Coll);
                    resultfind.Text = "Value found on Row " + (FindList[LastFind].Row + 1);
                    LastFind++;
                }
                else
                {
                    //Reset
                    if (loopfind.Checked == true)
                    {
                        MainForm.FocusOnCell(FindList[0].Row, FindList[0].Coll);
                        resultfind.Text = "Value found on Row " + (FindList[0].Row + 1);
                        LastFind = 0;
                    }
                    else
                    {
                        resultfind.Text = "You hit the end of file, without find other value.";
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String searchValue = searchtextbox.Text;
            FindList = MainForm.FindAllInCell(searchValue, CollList.Text, casefind.Checked);

            if (FindList.Count() > 0)
            {
                //Generate list
                string box_msg = "This function is not allowed on this version ! \nSorry !";
                string box_title = "Registration Error";
                MessageBox.Show(box_msg, box_title);
            }
            else
                resultfind.Text = "Value not found :( ...";
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FindText();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //replace
            string box_msg = "This function is not allowed on this version ! \nSorry !";
            string box_title = "Registration Error";
            MessageBox.Show(box_msg, box_title);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //replace all
            string box_msg = "This function is not allowed on this version ! \nSorry !";
            string box_title = "Registration Error";
            MessageBox.Show(box_msg, box_title);
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {
            if(tabControl1.SelectedIndex == 0) 
                this.Text = "Find";
            else
                this.Text = "Replace";
        }
    }
}
