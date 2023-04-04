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

namespace HBO_Renamer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var path = new FolderBrowserDialog())
            {
                DialogResult result = path.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(path.SelectedPath))
                {
                    textBox1.Text = path.SelectedPath;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles(textBox1.Text);
            MessageBox.Show("Please now, WAIT, don't click until he say finish after this window ! \n THAT CAN BE VERY LONG (50 min more) !", "Info !");
            // Read each file Guy !
            foreach (string filename in files) {
                string testfilename = filename;
                string newpath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                List<string> FolderPossibility = new List<string>();
                FolderPossibility.AddRange(new List<string> { "effect", "map", "model", "object", "data", "db", "char", "texture", "animation", "sky", "item", "sys" });


                for (int i = 1; i < 255; i++ )
                    for (int y = 1; y < 255; y++)
                        FolderPossibility.Add((i.ToString("X") + "_" + y.ToString("X")).ToLower());

                int havefound = 1;
                for (; havefound == 1;)
                {

                    foreach (string folder in FolderPossibility)
                    {
                        string tmpfilename = Path.GetFileName(testfilename);
                        string GetDirName = Path.GetDirectoryName(testfilename).TrimEnd(Path.DirectorySeparatorChar);
                        string LastFolder = GetDirName.Split(Path.DirectorySeparatorChar).Last();

                        //exeption list
                        if (LastFolder == "db")
                        {
                            havefound = 0;
                            continue;
                        }

                        if (LastFolder == "texture")
                        {
                            if (folder == "map" || folder == "sys")
                            {
                                havefound = 0;
                                continue;
                            }
                        }


                        if (LastFolder == "model")
                        {
                            if (folder == "map" || folder == "sys")
                            {
                                havefound = 0;
                                continue;
                            }
                        }


                        if (tmpfilename.Trim().StartsWith(folder.Trim()))
                        {


                            if (Path.GetFileNameWithoutExtension(testfilename).Trim().Length - folder.Length == 0)
                            {
                                // file have folder name
                                havefound = 0;
                                continue;
                            }

                            tmpfilename = RemoveStartWith(tmpfilename, folder);

                            newpath += "\\" + folder;

                            //create folder if not exist
                            if (!Directory.Exists(newpath))
                            {
                                Directory.CreateDirectory(newpath);
                            }

                            string OldFilePath = GetDirName + "\\" + Path.GetFileName(testfilename);
                            string NewFilePath = newpath + "\\" + tmpfilename;


                            if (!File.Exists(NewFilePath))
                            {
                                System.IO.File.Move(OldFilePath, NewFilePath);
                                /*
                                if (!OldFilePath.Contains(System.IO.Path.GetDirectoryName(Application.ExecutablePath)))
                                {
                                    System.IO.File.Copy(OldFilePath, NewFilePath);
                                }
                                else
                                {
                                    //Move because it's don't need have file with name wierd on some folder
                                    System.IO.File.Move(OldFilePath, NewFilePath);
                                }

                                */
                            }
                            testfilename = NewFilePath;

                            havefound = 1; break;
                        }
                        else
                        {
                            // close the loop
                            havefound = 0;
                        }

                    }
                }
            }
            MessageBox.Show("Operating is finish ! Enjoy !", "Success !");
        }

        public string RemoveStartWith(string value, string removeString)
        {
            int index = value.IndexOf(removeString, StringComparison.Ordinal);
            return index < 0 ? value : value.Remove(index, removeString.Length);
        }
    }
}
