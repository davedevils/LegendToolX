using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DDSParser;
// tool dev on 1 day BRAHAHAHAHAHAAHA
namespace Altra_Tool
{
    public partial class Main : Form
    {
        AltarMemory GMemory = new AltarMemory { };
        AltarMemory EMemory = new AltarMemory { };
        AltarMemory DMemory = new AltarMemory { };
        int selectedHeight = 1;
        int selectedWidth = 1;
        int currentAltar = 1;
        int autosave = 0;
        int showstats = 0;


        //splash
        Loading Loadingsplash = new Loading();
        int splash = 0;
        int swapaltar = 0;


        public Main()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;

            Loadingsplash.Start();
            this.Hide();
            splash = 1;

            GMemory.rows = new List<RowAltar>();
            GMemory.altarnum = 1;

            EMemory.rows = new List<RowAltar>();
            EMemory.altarnum = 2;

            DMemory.rows = new List<RowAltar>();
            DMemory.altarnum = 3;

            // Load Altar ALL from file
            currentAltar = 3;
            LoadAltarFromFile();
            currentAltar = 2;
            LoadAltarFromFile();
            currentAltar = 1;
            LoadAltarFromFile();

            // Select Altar 1
            radioButton1.Checked = true;

            // 1 - 1
            selectedHeight = 1;
            selectedWidth = 1;
            RefreshUiText();

            //Loading finish
            Loadingsplash.Stop();
            this.Show();
            splash = 0;

            this.WindowState = FormWindowState.Normal;
            //CenterForm(this);
        }

        private void CenterForm(Form Thisform)
        {
            foreach (var s in Screen.AllScreens)
            {
                if (s.WorkingArea.Contains(Thisform.Location))
                {
                    int screenH = s.WorkingArea.Height / 2;
                    int screenW = s.WorkingArea.Width / 2;

                    int top = (screenH + s.WorkingArea.Top) - Thisform.Width / 2;
                    int left = (screenW + s.WorkingArea.Left) - Thisform.Height / 2;
                    Thisform.Location = new Point(top, left);
                    break;
                }
            }
        }

        private void ShowThe6(bool showthem)
        {
            pictureBox1.Visible = showthem;
            pictureBox2.Visible = showthem;
            pictureBox3.Visible = showthem;
            pictureBox4.Visible = showthem;
            pictureBox5.Visible = showthem;
            pictureBox6.Visible = showthem;
            pictureBox7.Visible = showthem;
            pictureBox8.Visible = showthem;
            pictureBox9.Visible = showthem;
            pictureBox10.Visible = showthem;
            pictureBox11.Visible = showthem;
            pictureBox12.Visible = showthem;
        }
        private void GemAltar_CheckedChanged(object sender, EventArgs e)
        {
            if (swapaltar == 1)
                return;

            swapaltar = 1;

            if (splash == 0)
            {
                Loadingsplash.Start();
                this.Hide();
                splash = 2;
            }

            //Altar Gem
            currentAltar = 1;
            RefreshUiText();
            if (showstats == 1)
            {
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                        UpdateStats(x, y);
                }
            }
            LoadAltarPicture();

            //Loading finish
            if (splash == 2)
            {
                Loadingsplash.Stop();
                this.Show();
                this.BringToFront();
                splash = 0;
            }
            ShowThe6(true);
            swapaltar = 0;


        }

        private void EcAltar_CheckedChanged(object sender, EventArgs e)
        {
            if (swapaltar == 1)
                return;

            swapaltar = 1;

            if (splash == 0)
            {
                Loadingsplash.Start();
                this.Hide();
                splash = 2;
            }

            // Altar EC
            currentAltar = 2;
            RefreshUiText();
            if (showstats == 1)
            {
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                        UpdateStats(x, y);
                }
            }
            LoadAltarPicture();

            //Loading finish
            if (splash == 2)
            {
                Loadingsplash.Stop();
                this.Show();
                this.BringToFront();
                splash = 0;
            }
            ShowThe6(true);
            swapaltar = 0;
        }

        private void DiamondAltar_CheckedChanged(object sender, EventArgs e)
        {
            if (swapaltar == 1)
                return;

            swapaltar = 1;

            if (splash == 0)
            {
                Loadingsplash.Start();
                this.Hide();
                splash = 2;
            }

            // Diamond altar
            currentAltar = 3;
            RefreshUiText();
            if (showstats == 1)
            {
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                        UpdateStats(x, y);
                }
            }
            LoadAltarPicture();

            //Loading finish
            if (splash == 2)
            {
                Loadingsplash.Stop();
                this.Show();
                this.BringToFront();
                splash = 0;
            }
            ShowThe6(false);
            swapaltar = 0;
        }

        private void LoadAltarFromFile()
        {
            if (File.Exists("altar-"+ currentAltar + ".txt"))
            {
                string line = "";
                int counter = 0;
                System.IO.StreamReader file = new System.IO.StreamReader("altar-" + currentAltar + ".txt");
                // first line
                line = file.ReadLine();
                if(currentAltar == 1)
                    GMemory.altarnum = int.Parse(line);
                else if (currentAltar == 2)
                    EMemory.altarnum = int.Parse(line);
                else
                    DMemory.altarnum = int.Parse(line);

                while ((line = file.ReadLine()) != null)
                {
                    //Line cut
                    string[] x = line.Split('£');

                    if (currentAltar == 1)
                        GMemory.rows.Add(new RowAltar() { hauter = int.Parse(x[0]), longer = int.Parse(x[1]), itemid = x[2], quantity = x[3], luckrate = x[4], annouce = Convert.ToBoolean(x[5]), resetbonus = Convert.ToBoolean(x[6]), lockscroll = Convert.ToBoolean(x[7]) });
                    else if (currentAltar == 2)
                        EMemory.rows.Add(new RowAltar() { hauter = int.Parse(x[0]), longer = int.Parse(x[1]), itemid = x[2], quantity = x[3], luckrate = x[4], annouce = Convert.ToBoolean(x[5]), resetbonus = Convert.ToBoolean(x[6]), lockscroll = Convert.ToBoolean(x[7]) });
                    else
                        DMemory.rows.Add(new RowAltar() { hauter = int.Parse(x[0]), longer = int.Parse(x[1]), itemid = x[2], quantity = x[3], luckrate = x[4], annouce = Convert.ToBoolean(x[5]), resetbonus = Convert.ToBoolean(x[6]), lockscroll = Convert.ToBoolean(x[7]) });


                    // ICON
                    string path1 = "UI\\itemicon\\";
                    string path2 = "data\\db\\";
                    string Imgname = "";
                    //find per ID
                    IniReader Readers = new IniReader();
                    Imgname = Readers.ReadIconName(x[2].Trim(), path2 + "C_ItemMall.ini");

                    if (Imgname == "")
                        Imgname = Readers.ReadIconName(x[2].Trim(), path2 + "C_Item.ini");

                    //now we have name icon
                    Imgname = path1 + Imgname + ".dds";
                    if (File.Exists(Imgname))
                    {
                        UpdatePicture(int.Parse(x[0]), int.Parse(x[1]), Imgname);
                    }
                    else
                    {
                        ErasePicture(int.Parse(x[0]), int.Parse(x[1]));
                    }
                    counter++;
                }
                file.Close();
            }
        }

        private void LoadAltarPicture()
        {
            for (int H = 0; H < 8; H++)
            {
                for (int L = 0; L < 8; L++)
                {
                    RowAltar Row;
                    if (currentAltar == 1)
                        Row = GMemory.rows.SingleOrDefault(x => x.hauter == H && x.longer == L);
                    else if (currentAltar == 2)
                        Row = EMemory.rows.SingleOrDefault(x => x.hauter == H && x.longer == L);
                    else
                        Row = DMemory.rows.SingleOrDefault(x => x.hauter == H && x.longer == L);


                    if (Row != null)
                    {
                        // ICON
                        string path1 = "UI\\itemicon\\";
                        string path2 = "data\\db\\";
                        string Imgname = "";
                        //find per ID
                        IniReader Readers = new IniReader();
                        Imgname = Readers.ReadIconName(Row.itemid, path2 + "C_ItemMall.ini");

                        if (Imgname == "")
                            Imgname = Readers.ReadIconName(Row.itemid, path2 + "C_Item.ini");

                        //now we have name icon
                        Imgname = path1 + Imgname + ".dds";
                        if (File.Exists(Imgname))
                        {
                            UpdatePicture(H, L, Imgname);
                        }
                        else
                        {
                            ErasePicture(H, L);
                        }
                    }
                    else
                        ErasePicture(H, L);
                }
            }
        }
private void ItemFinder_Click(object sender, EventArgs e)
        {
            string message = "Give me time, i can't make all on day 1";
            MessageBox.Show(message);
        }

        private void CellR1C1_Click(object sender, EventArgs e)
        {
            // 1 - 1
            if(autosave == 1)
                SaveCaseChange();

            selectedHeight = 1;
            selectedWidth = 1;
            RefreshUiText();

        }

        private void CellR1C2_Click(object sender, EventArgs e)
        {
            // 1 - 2
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 1;
            selectedWidth = 2;
            RefreshUiText();
        }

        private void CellR1C3_Click(object sender, EventArgs e)
        {
            // 1 - 3
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 1;
            selectedWidth = 3;
            RefreshUiText();
        }

        private void CellR1C4_Click(object sender, EventArgs e)
        {
            // 1 - 4
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 1;
            selectedWidth = 4;
            RefreshUiText();
        }

        private void CellR1C5_Click(object sender, EventArgs e)
        {
            // 1 - 5
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 1;
            selectedWidth = 5;
            RefreshUiText();
        }

        private void CellR1C6_Click(object sender, EventArgs e)
        {
            // 1 - 6
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 1;
            selectedWidth = 6;
            RefreshUiText();
        }

        private void CellR2C1_Click(object sender, EventArgs e)
        {
            // 2 - 1
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 2;
            selectedWidth = 1;
            RefreshUiText();
        }

        private void CellR2C2_Click(object sender, EventArgs e)
        {
            // 2 - 2
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 2;
            selectedWidth = 2;
            RefreshUiText();
        }

        private void CellR2C3_Click(object sender, EventArgs e)
        {
            // 2 - 3
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 2;
            selectedWidth = 3;
            RefreshUiText();
        }

        private void CellR2C4_Click(object sender, EventArgs e)
        {
            // 2 - 4
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 2;
            selectedWidth = 4;
            RefreshUiText();
        }

        private void CellR2C5_Click(object sender, EventArgs e)
        {
            // 2 - 5
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 2;
            selectedWidth = 5;
            RefreshUiText();
        }

        private void CellR2C6_Click(object sender, EventArgs e)
        {
            // 2 - 6
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 2;
            selectedWidth = 6;
            RefreshUiText();
        }

        private void CellR3C1_Click(object sender, EventArgs e)
        {
            // 3 - 1
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 3;
            selectedWidth = 1;
            RefreshUiText();
        }

        private void CellR3C2_Click(object sender, EventArgs e)
        {
            // 3 - 2
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 3;
            selectedWidth = 2;
            RefreshUiText();
        }

        private void CellR3C3_Click(object sender, EventArgs e)
        {
            // 3 - 3
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 3;
            selectedWidth = 3;
            RefreshUiText();
        }

        private void CellR3C4_Click(object sender, EventArgs e)
        {
            // 3 - 4
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 3;
            selectedWidth = 4;
            RefreshUiText();
        }

        private void CellR3C5_Click(object sender, EventArgs e)
        {
            // 3 - 5
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 3;
            selectedWidth = 5;
            RefreshUiText();
        }

        private void CellR3C6_Click(object sender, EventArgs e)
        {
            // 3 - 6
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 3;
            selectedWidth = 6;
            RefreshUiText();
        }

        private void CellR4C1_Click(object sender, EventArgs e)
        {
            // 4 - 1
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 4;
            selectedWidth = 1;
            RefreshUiText();
        }

        private void CellR4C2_Click(object sender, EventArgs e)
        {
            // 4 - 2
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 4;
            selectedWidth = 2;
            RefreshUiText();
        }

        private void CellR4C3_Click(object sender, EventArgs e)
        {
            // 4 - 3
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 4;
            selectedWidth = 3;
            RefreshUiText();
        }

        private void CellR4C4_Click(object sender, EventArgs e)
        {
            // 4 - 4
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 4;
            selectedWidth = 4;
            RefreshUiText();
        }

        private void CellR4C5_Click(object sender, EventArgs e)
        {
            // 4 - 5
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 4;
            selectedWidth = 5;
            RefreshUiText();
        }

        private void CellR4C6_Click(object sender, EventArgs e)
        {
            // 4 - 6
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 4;
            selectedWidth = 6;
            RefreshUiText();
        }

        private void CellR5C1_Click(object sender, EventArgs e)
        {
            // 5 - 1
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 5;
            selectedWidth = 1;
            RefreshUiText();
        }

        private void CellR5C2_Click(object sender, EventArgs e)
        {
            // 5 - 2
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 5;
            selectedWidth = 2;
            RefreshUiText();
        }

        private void CellR5C3_Click(object sender, EventArgs e)
        {
            // 5 - 3
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 5;
            selectedWidth = 3;
            RefreshUiText();
        }

        private void CellR5C4_Click(object sender, EventArgs e)
        {
            // 5 - 4
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 5;
            selectedWidth = 4;
            RefreshUiText();
        }

        private void CellR5C5_Click(object sender, EventArgs e)
        {
            // 5 - 5
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 5;
            selectedWidth = 5;
            RefreshUiText();
        }

        private void CellR5C6_Click(object sender, EventArgs e)
        {
            // 5 - 6
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 5;
            selectedWidth = 6;
            RefreshUiText();
        }

        private void CellR6C1_Click(object sender, EventArgs e)
        {
            // 6 - 1
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 6;
            selectedWidth = 1;
            RefreshUiText();
        }

        private void CellR6C2_Click(object sender, EventArgs e)
        {
            // 6 - 2
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 6;
            selectedWidth = 2;
            RefreshUiText();
        }

        private void CellR6C3_Click(object sender, EventArgs e)
        {
            // 6 - 3
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 6;
            selectedWidth = 3;
            RefreshUiText();
        }

        private void CellR6C4_Click(object sender, EventArgs e)
        {
            // 6 - 4
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 6;
            selectedWidth = 4;
            RefreshUiText();
        }

        private void CellR6C5_Click(object sender, EventArgs e)
        {
            // 6 - 5
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 6;
            selectedWidth = 5;
            RefreshUiText();
        }

        private void CellR6C6_Click(object sender, EventArgs e)
        {
            // 6 - 6
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 6;
            selectedWidth = 6;
            RefreshUiText();
        }

        private void CellR7C1_Click(object sender, EventArgs e)
        {
            // 7 - 1
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 7;
            selectedWidth = 1;
            RefreshUiText();
        }

        private void CellR7C2_Click(object sender, EventArgs e)
        {
            // 7 - 2
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 7;
            selectedWidth = 2;
            RefreshUiText();
        }

        private void CellR7C3_Click(object sender, EventArgs e)
        {
            // 7 - 3
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 7;
            selectedWidth = 3;
            RefreshUiText();
        }

        private void CellR7C4_Click(object sender, EventArgs e)
        {
            // 7 - 4
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 7;
            selectedWidth = 4;
            RefreshUiText();
        }

        private void CellR7C5_Click(object sender, EventArgs e)
        {
            // 7 - 5
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 7;
            selectedWidth = 5;
            RefreshUiText();
        }

        private void CellR7C6_Click(object sender, EventArgs e)
        {
            // 7 - 6
            if (autosave == 1)
                SaveCaseChange();

            selectedHeight = 7;
            selectedWidth = 6;
            RefreshUiText();
        }


        public class AltarMemory
        {
            public int altarnum { get; set; }
            public List<RowAltar> rows { get; set; }
        }

        public class RowAltar
        {
            public int hauter { get; set; }
            public int longer { get; set; }
            public string itemid { get; set; }
            public string quantity { get; set; }
            public string luckrate { get; set; }
            public bool annouce { get; set; }
            public bool resetbonus { get; set; }
            public bool lockscroll { get; set; }
        }

        private void RefreshUiText()
        {
            casenum.Text = selectedHeight + " - " + selectedWidth;

            RowAltar Row;

            if (currentAltar == 1)
                Row = GMemory.rows.SingleOrDefault(x => x.hauter == selectedHeight && x.longer == selectedWidth);
            else if (currentAltar == 2)
                Row = EMemory.rows.SingleOrDefault(x => x.hauter == selectedHeight && x.longer == selectedWidth);
            else
                Row = DMemory.rows.SingleOrDefault(x => x.hauter == selectedHeight && x.longer == selectedWidth);

           
            if (Row != null)
            {
                itemidtext.Text = Row.itemid;
                quantitytext.Text = Row.quantity;
                luckratetext.Text = Row.luckrate;
                announcegain.Checked = Row.annouce;
                resetbonustext.Checked = Row.resetbonus;
                lockscrolltext.Checked = Row.lockscroll;
            }
            else
            {
                itemidtext.Text = "";
                quantitytext.Text = "";
                luckratetext.Text = "";
                announcegain.Checked = false;
                resetbonustext.Checked = false;
                lockscrolltext.Checked = false;
            }
        }

        private void ErasePicture(int hauter, int longer)
        {

            string Namecase = hauter + " - " + longer;
            switch (Namecase)
            {
                case "7 - 1":
                    pictureBox1.BackgroundImage = null;
                    break;
                case "7 - 2":
                    pictureBox2.BackgroundImage = null;
                    break;
                case "7 - 3":
                    pictureBox3.BackgroundImage = null;
                    break;
                case "7 - 4":
                    pictureBox4.BackgroundImage = null;
                    break;
                case "7 - 5":
                    pictureBox5.BackgroundImage = null;
                    break;
                case "7 - 6":
                    pictureBox6.BackgroundImage = null;
                    break;
                case "6 - 6":
                    pictureBox7.BackgroundImage = null;
                    break;
                case "6 - 5":
                    pictureBox8.BackgroundImage = null;
                    break;
                case "6 - 4":
                    pictureBox9.BackgroundImage = null;
                    break;
                case "6 - 3":
                    pictureBox10.BackgroundImage = null;
                    break;
                case "6 - 2":
                    pictureBox11.BackgroundImage = null;
                    break;
                case "6 - 1":
                    pictureBox12.BackgroundImage = null;
                    break;
                case "5 - 6":
                    pictureBox13.BackgroundImage = null;
                    break;
                case "5 - 5":
                    pictureBox14.BackgroundImage = null;
                    break;
                case "5 - 4":
                    pictureBox15.BackgroundImage = null;
                    break;
                case "5 - 3":
                    pictureBox16.BackgroundImage = null;
                    break;
                case "5 - 2":
                    pictureBox17.BackgroundImage = null;
                    break;
                case "5 - 1":
                    pictureBox18.BackgroundImage = null;
                    break;
                case "4 - 6":
                    pictureBox19.BackgroundImage = null;
                    break;
                case "4 - 5":
                    pictureBox20.BackgroundImage = null;
                    break;
                case "4 - 4":
                    pictureBox21.BackgroundImage = null;
                    break;
                case "4 - 3":
                    pictureBox22.BackgroundImage = null;
                    break;
                case "4 - 2":
                    pictureBox23.BackgroundImage = null;
                    break;
                case "4 - 1":
                    pictureBox24.BackgroundImage = null;
                    break;
                case "3 - 6":
                    pictureBox25.BackgroundImage = null;
                    break;
                case "3 - 5":
                    pictureBox26.BackgroundImage = null;
                    break;
                case "3 - 4":
                    pictureBox27.BackgroundImage = null;
                    break;
                case "3 - 3":
                    pictureBox28.BackgroundImage = null;
                    break;
                case "3 - 2":
                    pictureBox29.BackgroundImage = null;
                    break;
                case "3 - 1":
                    pictureBox30.BackgroundImage = null;
                    break;
                case "2 - 6":
                    pictureBox31.BackgroundImage = null;
                    break;
                case "2 - 5":
                    pictureBox32.BackgroundImage = null;
                    break;
                case "2 - 4":
                    pictureBox33.BackgroundImage = null;
                    break;
                case "2 - 3":
                    pictureBox34.BackgroundImage = null;
                    break;
                case "2 - 2":
                    pictureBox35.BackgroundImage = null;
                    break;
                case "2 - 1":
                    pictureBox36.BackgroundImage = null;
                    break;
                case "1 - 6":
                    pictureBox37.BackgroundImage = null;
                    break;
                case "1 - 5":
                    pictureBox38.BackgroundImage = null;
                    break;
                case "1 - 4":
                    pictureBox39.BackgroundImage = null;
                    break;
                case "1 - 3":
                    pictureBox40.BackgroundImage = null;
                    break;
                case "1 - 2":
                    pictureBox41.BackgroundImage = null;
                    break;
                case "1 - 1":
                    pictureBox42.BackgroundImage = null;
                    break;
            }
        }

        private void UpdatePicture(int hauter, int longer, string url)
        {
            var DDSImage = new DDSImage(File.ReadAllBytes(url));
            string Namecase = hauter + " - " + longer;

            switch (Namecase)
            {
                case "7 - 1":
                    pictureBox1.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "7 - 2":
                    pictureBox2.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "7 - 3":
                    pictureBox3.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "7 - 4":
                    pictureBox4.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "7 - 5":
                    pictureBox5.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "7 - 6":
                    pictureBox6.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "6 - 6":
                    pictureBox7.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "6 - 5":
                    pictureBox8.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "6 - 4":
                    pictureBox9.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "6 - 3":
                    pictureBox10.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "6 - 2":
                    pictureBox11.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "6 - 1":
                    pictureBox12.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "5 - 6":
                    pictureBox13.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "5 - 5":
                    pictureBox14.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "5 - 4":
                    pictureBox15.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "5 - 3":
                    pictureBox16.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "5 - 2":
                    pictureBox17.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "5 - 1":
                    pictureBox18.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "4 - 6":
                    pictureBox19.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "4 - 5":
                    pictureBox20.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "4 - 4":
                    pictureBox21.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "4 - 3":
                    pictureBox22.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "4 - 2":
                    pictureBox23.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "4 - 1":
                    pictureBox24.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "3 - 6":
                    pictureBox25.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "3 - 5":
                    pictureBox26.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "3 - 4":
                    pictureBox27.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "3 - 3":
                    pictureBox28.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "3 - 2":
                    pictureBox29.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "3 - 1":
                    pictureBox30.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "2 - 6":
                    pictureBox31.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "2 - 5":
                    pictureBox32.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "2 - 4":
                    pictureBox33.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "2 - 3":
                    pictureBox34.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "2 - 2":
                    pictureBox35.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "2 - 1":
                    pictureBox36.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "1 - 6":
                    pictureBox37.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "1 - 5":
                    pictureBox38.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "1 - 4":
                    pictureBox39.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "1 - 3":
                    pictureBox40.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "1 - 2":
                    pictureBox41.BackgroundImage = DDSImage.BitmapImage;
                    break;
                case "1 - 1":
                    pictureBox42.BackgroundImage = DDSImage.BitmapImage;
                    break;
            }
        }


        private void SaveCase_Click(object sender, EventArgs e)
        {
            SaveCaseChange();
        }

        private void SaveCaseChange()
        {
            IniReader Readers = new IniReader();
            if (currentAltar == 1)
            {
                if (GMemory.rows.Count > 0)
                    GMemory.rows.Remove(GMemory.rows.SingleOrDefault(x => x.hauter == selectedHeight && x.longer == selectedWidth));

                // Save row
                GMemory.rows.Add(new RowAltar() { hauter = selectedHeight, longer = selectedWidth, itemid = itemidtext.Text, quantity = quantitytext.Text, luckrate = luckratetext.Text, annouce = announcegain.Checked, resetbonus = resetbonustext.Checked, lockscroll = lockscrolltext.Checked });

            }
            else if (currentAltar == 2)
            {
                if (EMemory.rows.Count > 0)
                    EMemory.rows.Remove(EMemory.rows.SingleOrDefault(x => x.hauter == selectedHeight && x.longer == selectedWidth));
                // Save row
                EMemory.rows.Add(new RowAltar() { hauter = selectedHeight, longer = selectedWidth, itemid = itemidtext.Text, quantity = quantitytext.Text, luckrate = luckratetext.Text, annouce = announcegain.Checked, resetbonus = resetbonustext.Checked, lockscroll = lockscrolltext.Checked });

            }
            else
            {
                if (DMemory.rows.Count > 0)
                    DMemory.rows.Remove(DMemory.rows.SingleOrDefault(x => x.hauter == selectedHeight && x.longer == selectedWidth));
                // Save row
                DMemory.rows.Add(new RowAltar() { hauter = selectedHeight, longer = selectedWidth, itemid = itemidtext.Text, quantity = quantitytext.Text, luckrate = luckratetext.Text, annouce = announcegain.Checked, resetbonus = resetbonustext.Checked, lockscroll = lockscrolltext.Checked });

            }

            // ICON
            string path1 = "UI\\itemicon\\";
            string path2 = "data\\db\\";
            string Imgname = "";
            //find per ID

            Imgname = Readers.ReadIconName(itemidtext.Text.Trim(), path2 + "C_ItemMall.ini");

            if (Imgname == "")
                Imgname = Readers.ReadIconName(itemidtext.Text.Trim(), path2 + "C_Item.ini");

            //now we have name icon
            Imgname = path1 + Imgname + ".dds";
            if (File.Exists(Imgname))
            {
                UpdatePicture(selectedHeight, selectedWidth, Imgname);
            }
            else
            {
                ErasePicture(selectedHeight, selectedWidth);
            }

            if (showstats == 1)
            {
                UpdateStats(selectedHeight, selectedWidth);
            }

        }

        private void SaveCurrent_Click(object sender, EventArgs e)
        {
            SaveAltarToFile(currentAltar);
            string message = "Current Altar Saved !";
            MessageBox.Show(message);
        }

        private void SaveAltarToFile(int filenum)
        {
           StreamWriter writer = new StreamWriter("altar-" + filenum + ".txt");

            AltarMemory AMemory = null;
            if (filenum == 1)
                AMemory = GMemory;
            else if (filenum == 2)
                AMemory = EMemory;
            else
                AMemory = DMemory;

            string write = AMemory.altarnum + "";
            writer.Flush();
            writer.WriteLine(write);

            foreach (var x in AMemory.rows)
            {
                string wr = x.hauter + "£" + x.longer + "£" + x.itemid + "£" + x.quantity + "£" + x.luckrate + "£" + x.annouce + "£" + x.resetbonus + "£" + x.lockscroll;
                writer.Flush();
                writer.WriteLine(wr);

            }
            writer.Close();
            AMemory = null;
        }

        private void GenerateSql_Click(object sender, EventArgs e)
        {
            // generate SQL

            //generate GEM altar
            StreamWriter writer = new StreamWriter("Gem-Altar.sql");

            AltarMemory AMemory = GMemory;

            string Header = "DELETE FROM ingame_lottery;\r\nINSERT INTO ingame_lottery(round, week, item_index, item_id, item_amount, probability, bulletin, reset_bonus, cant_gonext) VALUES";
            writer.Flush();
            writer.WriteLine(Header);
            int week = 0;

            for (; week < 251; week++)
            {
                foreach (var x in AMemory.rows)
                {
                    int annouce = x.annouce ? 1 : 0;
                    int resetbonus = x.resetbonus ? 1 : 0;
                    int lockscroll = x.lockscroll ? 1 : 0;
                    string wr = "(" + x.hauter + ", " + week + ", " + x.longer + ", " + x.itemid + ", " + x.quantity + ", " + x.luckrate + ", " + annouce + ", " + resetbonus + ", " + lockscroll + "),";
                    writer.Flush();
                    writer.WriteLine(wr);
                }
            }

            string end = "(0,0,0,0,0,0,0,0,0);";
            writer.Flush();
            writer.WriteLine(end);
            writer.Close();

            // generate EC
            writer = new StreamWriter("EC-Altar.sql");

            AMemory = EMemory;

            string Header2 = "DELETE FROM lottery;\r\nINSERT INTO lottery(round, week, item_index, item_id, item_amount, probability, bulletin, reset_bonus, cant_gonext) VALUES";
            writer.Flush();
            writer.WriteLine(Header2);
            week = 0;

            for (; week < 251; week++)
            {
                foreach (var x in AMemory.rows)
                {
                    int annouce = x.annouce ? 1 : 0;
                    int resetbonus = x.resetbonus ? 1 : 0;
                    int lockscroll = x.lockscroll ? 1 : 0;
                    string wr = "(" + x.hauter + ", " + week + ", " + x.longer + ", " + x.itemid + ", " + x.quantity + ", " + x.luckrate + ", " + annouce + ", " + resetbonus + ", " + lockscroll + "),";
                    writer.Flush();
                    writer.WriteLine(wr);
                }
            }

            string end2 = "(0,0,0,0,0,0,0,0,0);";
            writer.Flush();
            writer.WriteLine(end2);
            writer.Close();


            // generate Dimaond
            writer = new StreamWriter("Diamond-Altar.sql");

            AMemory = DMemory;

            string Header3 = "DELETE FROM new_lottery;\r\nINSERT INTO new_lottery(round, week, item_index, item_id, item_amount, probability, bulletin, reset_bonus, cant_gonext) VALUES";
            writer.Flush();
            writer.WriteLine(Header3);
            week = 0;

            for (; week < 251; week++)
            {
                foreach (var x in AMemory.rows)
                {
                    int annouce = x.annouce ? 1 : 0;
                    int resetbonus = x.resetbonus ? 1 : 0;
                    int lockscroll = x.lockscroll ? 1 : 0;
                    string wr = "(" + x.hauter + ", " + week + ", " + x.longer + ", " + x.itemid + ", " + x.quantity + ", " + x.luckrate + ", " + annouce + ", " + resetbonus + ", " + lockscroll + "),";
                    writer.Flush();
                    writer.WriteLine(wr);
                }
            }

            string end3 = "(0,0,0,0,0,0,0,0,0);";
            writer.Flush();
            writer.WriteLine(end3);
            writer.Close();


            string message = "All Altar Generated in SQL !";
            MessageBox.Show(message);
        }

        private void AutoSave_CheckedChanged(object sender, EventArgs e)
        {
            // Auto save
            if (checkBox1.Checked == true)
                autosave = 1;
            else
                autosave = 0;
        }

        private void SaveAllAltar_Click(object sender, EventArgs e)
        {
            // Save all altar
            SaveAltarToFile(1);
            SaveAltarToFile(2);
            SaveAltarToFile(3);


            string message = "All Altar Saved !";
            MessageBox.Show(message);

        }

        private void ShowStats_CheckedChanged(object sender, EventArgs e)
        {
            // Show Stat
            if (ShowStat.Checked == true)
            {
                showstats = 1;
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                        UpdateStats(x, y);
                }
            }
            else
            {
                showstats = 0;
                RemoveAllStats();
            }

        }

        private void RemoveAllStats()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                    Controls.RemoveByKey(x+" - "+y);
            }
        }

        private void UpdateStats(int hauter, int longer)
        {
            string Namecase = hauter + " - " + longer;

            //Remove if exist
            Controls.RemoveByKey(Namecase);

            RowAltar Row;

            if (currentAltar == 1)
                Row = GMemory.rows.SingleOrDefault(x => x.hauter == hauter && x.longer == longer);
            else if (currentAltar == 2)
                Row = EMemory.rows.SingleOrDefault(x => x.hauter == hauter && x.longer == longer);
            else
                Row = DMemory.rows.SingleOrDefault(x => x.hauter == hauter && x.longer == longer);


            if (Row != null)
            {
                if (Row.luckrate == "")
                    return;

                string finaltext = Row.luckrate;

                if (Row.resetbonus == false)
                    finaltext += " - NR";

                if (Row.lockscroll == true)
                    finaltext += " - L";

                Point NewLocate = new Point(0,0);

                switch (Namecase)
                {
                    case "7 - 1":
                        NewLocate = new Point(pictureBox1.Location.X - 2, pictureBox1.Location.Y + 42);
                        break;
                    case "7 - 2":
                        NewLocate = new Point(pictureBox2.Location.X - 2, pictureBox2.Location.Y + 42);
                        break;
                    case "7 - 3":
                        NewLocate = new Point(pictureBox3.Location.X - 2, pictureBox3.Location.Y + 42);
                        break;
                    case "7 - 4":
                        NewLocate = new Point(pictureBox4.Location.X - 2, pictureBox4.Location.Y + 42);
                        break;
                    case "7 - 5":
                        NewLocate = new Point(pictureBox5.Location.X - 2, pictureBox5.Location.Y + 42);
                        break;
                    case "7 - 6":
                        NewLocate = new Point(pictureBox6.Location.X - 2, pictureBox6.Location.Y + 42);
                        break;
                    case "6 - 6":
                        NewLocate = new Point(pictureBox7.Location.X - 2, pictureBox7.Location.Y + 42);
                        break;
                    case "6 - 5":
                        NewLocate = new Point(pictureBox8.Location.X - 2, pictureBox8.Location.Y + 42);
                        break;
                    case "6 - 4":
                        NewLocate = new Point(pictureBox9.Location.X - 2, pictureBox9.Location.Y + 42);
                        break;
                    case "6 - 3":
                        NewLocate = new Point(pictureBox10.Location.X - 2, pictureBox10.Location.Y + 42);
                        break;
                    case "6 - 2":
                        NewLocate = new Point(pictureBox11.Location.X - 2, pictureBox11.Location.Y + 42);
                        break;
                    case "6 - 1":
                        NewLocate = new Point(pictureBox12.Location.X - 2, pictureBox12.Location.Y + 42);
                        break;
                    case "5 - 6":
                        NewLocate = new Point(pictureBox13.Location.X - 2, pictureBox13.Location.Y + 42);
                        break;
                    case "5 - 5":
                        NewLocate = new Point(pictureBox14.Location.X - 2, pictureBox14.Location.Y + 42);
                        break;
                    case "5 - 4":
                        NewLocate = new Point(pictureBox15.Location.X - 2, pictureBox15.Location.Y + 42);
                        break;
                    case "5 - 3":
                        NewLocate = new Point(pictureBox16.Location.X - 2, pictureBox16.Location.Y + 42);
                        break;
                    case "5 - 2":
                        NewLocate = new Point(pictureBox17.Location.X - 2, pictureBox17.Location.Y + 42);
                        break;
                    case "5 - 1":
                        NewLocate = new Point(pictureBox18.Location.X - 2, pictureBox18.Location.Y + 42);
                        break;
                    case "4 - 6":
                        NewLocate = new Point(pictureBox19.Location.X - 2, pictureBox19.Location.Y + 42);
                        break;
                    case "4 - 5":
                        NewLocate = new Point(pictureBox20.Location.X - 2, pictureBox20.Location.Y + 42);
                        break;
                    case "4 - 4":
                        NewLocate = new Point(pictureBox21.Location.X - 2, pictureBox21.Location.Y + 42);
                        break;
                    case "4 - 3":
                        NewLocate = new Point(pictureBox22.Location.X - 2, pictureBox22.Location.Y + 42);
                        break;
                    case "4 - 2":
                        NewLocate = new Point(pictureBox23.Location.X - 2, pictureBox23.Location.Y + 42);
                        break;
                    case "4 - 1":
                        NewLocate = new Point(pictureBox24.Location.X - 2, pictureBox24.Location.Y + 42);
                        break;
                    case "3 - 6":
                        NewLocate = new Point(pictureBox25.Location.X - 2, pictureBox25.Location.Y + 42);
                        break;
                    case "3 - 5":
                        NewLocate = new Point(pictureBox26.Location.X - 2, pictureBox26.Location.Y + 42);
                        break;
                    case "3 - 4":
                        NewLocate = new Point(pictureBox27.Location.X - 2, pictureBox27.Location.Y + 42);
                        break;
                    case "3 - 3":
                        NewLocate = new Point(pictureBox28.Location.X - 2, pictureBox28.Location.Y + 42);
                        break;
                    case "3 - 2":
                        NewLocate = new Point(pictureBox29.Location.X - 2, pictureBox29.Location.Y + 42);
                        break;
                    case "3 - 1":
                        NewLocate = new Point(pictureBox30.Location.X - 2, pictureBox30.Location.Y + 42);
                        break;
                    case "2 - 6":
                        NewLocate = new Point(pictureBox31.Location.X - 2, pictureBox31.Location.Y + 42);
                        break;
                    case "2 - 5":
                        NewLocate = new Point(pictureBox32.Location.X - 2, pictureBox32.Location.Y + 42);
                        break;
                    case "2 - 4":
                        NewLocate = new Point(pictureBox33.Location.X - 2, pictureBox33.Location.Y + 42);
                        break;
                    case "2 - 3":
                        NewLocate = new Point(pictureBox34.Location.X - 2, pictureBox34.Location.Y + 42);
                        break;
                    case "2 - 2":
                        NewLocate = new Point(pictureBox35.Location.X - 2, pictureBox35.Location.Y + 42);
                        break;
                    case "2 - 1":
                        NewLocate = new Point(pictureBox36.Location.X - 2, pictureBox36.Location.Y + 42);
                        break;
                    case "1 - 6":
                        NewLocate = new Point(pictureBox37.Location.X - 2, pictureBox37.Location.Y + 42);
                        break;
                    case "1 - 5":
                        NewLocate = new Point(pictureBox38.Location.X - 2, pictureBox38.Location.Y + 42);
                        break;
                    case "1 - 4":
                        NewLocate = new Point(pictureBox39.Location.X - 2, pictureBox39.Location.Y + 42);
                        break;
                    case "1 - 3":
                        NewLocate = new Point(pictureBox40.Location.X - 2, pictureBox40.Location.Y + 42);
                        break;
                    case "1 - 2":
                        NewLocate = new Point(pictureBox41.Location.X - 2, pictureBox41.Location.Y + 42);
                        break;
                    case "1 - 1":
                        NewLocate = new Point(pictureBox42.Location.X - 2, pictureBox42.Location.Y + 42);
                        break;
                }

                Controls.Add(new Label { Name = Namecase, Location = NewLocate, Font = new Font("Arial", 6), AutoSize = true, Text = finaltext });
            }
        }
    }
}
