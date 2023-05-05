using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NfsExtractor
{

    public partial class Form1 : Form
    {
        private NFSManager loadedDrive;
        internal NFSManager LoadedDrive { get => loadedDrive; set => loadedDrive = value; }
        public DataTable MemoryTable { get; private set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Open crap box for txt
            this.openFileDialog1.Filter = "packageindex(*.*)|*.*|nfs.idx (*.idx)|*.idx";
            this.openFileDialog1.Multiselect = false;
            this.openFileDialog1.Title = "Find any Package index from xLeg game";
            this.openFileDialog1.FileName = "packageindex";
            DialogResult txt_file = this.openFileDialog1.ShowDialog();
            if (txt_file == System.Windows.Forms.DialogResult.OK)
            {
                string nameoutput = openFileDialog1.FileName;
                textBox1.Text = nameoutput;
            }
        }

        private void UpdateGrid1(uint one, uint two, uint tree, uint four)
        {
            // let's grow a tree =D
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new MethodInvoker(() => dataGridView1.Rows.Add(one.ToString("X"), two, tree.ToString("X"), four)));
            }
            else
            {
                dataGridView1.Rows.Add(one.ToString("X"), two, tree.ToString("X"), four);
            }
        }


        private void EnableFindFile(bool enable)
        {
            // step 2
            if (button2.InvokeRequired)
            {
                button2.Invoke(new MethodInvoker(() => button2.Enabled = enable));
                button3.Invoke(new MethodInvoker(() => button3.Enabled = enable));
            }
            else
            {
                button2.Enabled = enable;
                button3.Enabled = enable;
            }
        }

        private void RenameFindFile()
        {
            // step 2
            if (button2.InvokeRequired)
            {
                button2.Invoke(new MethodInvoker(() => button2.Text = "Add More FileList"));
            }
            else
            {
                button2.Text = "Add More FileList";
            }
        }

        private void EnableExtract(bool enable)
        {
            // step 3
            if (button9.InvokeRequired)
            {
                button9.Invoke(new MethodInvoker(() => button9.Enabled = enable));
                button6.Invoke(new MethodInvoker(() => button6.Enabled = enable));
                button4.Invoke(new MethodInvoker(() => button4.Enabled = enable));
                button3.Invoke(new MethodInvoker(() => button3.Enabled = enable));
            }
            else
            {
                button9.Enabled = enable;
                button6.Enabled = enable;
                button4.Enabled = enable;
                button3.Enabled = enable;
            }

            if (enable == true)
            {
                ToolStrip toolStrip = toolStripStatusLabel1.GetCurrentParent();

                if (toolStrip.InvokeRequired)
                {
                    toolStrip.Invoke(new Action(() => toolStripStatusLabel1.Text = "FileList Loaded !"));
                }
                else
                {
                    toolStripStatusLabel1.Text = "FileList Loaded !";
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Open FileList and do the cooking
            toolStripStatusLabel1.Text = "Loading PackageIndex ....";
            if (textBox1.Text == "")
            {
                string boxmsg = "No FileList selected !";
                string boxtitle = "ERROR";
                MessageBox.Show(boxmsg, boxtitle);
                return;
            }

            string fileListPath = System.IO.Path.GetDirectoryName(this.textBox1.Text);
            NFSManager package = new NFSManager(this.textBox1.Text, 1);
            LoadedDrive = package;

            this.dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            MemoryTable = new DataTable("NFSDATA");
            // Add Coll
            DataColumn TMPCOL = new DataColumn();
            TMPCOL.DataType = typeof(String);
            TMPCOL.ColumnName = "Hash";
            TMPCOL.Caption = "Hash";
            MemoryTable.Columns.Add(TMPCOL);

            TMPCOL = new DataColumn();
            TMPCOL.DataType = typeof(String);
            TMPCOL.ColumnName = "Offset";
            TMPCOL.Caption = "Offset";
            MemoryTable.Columns.Add(TMPCOL);

            TMPCOL = new DataColumn();
            TMPCOL.DataType = typeof(String);
            TMPCOL.ColumnName = "SizeZip";
            TMPCOL.Caption = "SizeZip";
            MemoryTable.Columns.Add(TMPCOL);

            TMPCOL = new DataColumn();
            TMPCOL.DataType = typeof(String);
            TMPCOL.ColumnName = "Size";
            TMPCOL.Caption = "Size";
            MemoryTable.Columns.Add(TMPCOL);

            TMPCOL = new DataColumn();
            TMPCOL.DataType = typeof(String);
            TMPCOL.ColumnName = "ChecksumZip";
            TMPCOL.Caption = "ChecksumZip";
            MemoryTable.Columns.Add(TMPCOL);

            TMPCOL = new DataColumn();
            TMPCOL.DataType = typeof(String);
            TMPCOL.ColumnName = "Checksum";
            TMPCOL.Caption = "Checksum";
            MemoryTable.Columns.Add(TMPCOL);

            TMPCOL = new DataColumn();
            TMPCOL.DataType = typeof(String);
            TMPCOL.ColumnName = "Time";
            TMPCOL.Caption = "Time";
            MemoryTable.Columns.Add(TMPCOL);

            TMPCOL = new DataColumn();
            TMPCOL.DataType = typeof(String);
            TMPCOL.ColumnName = "PathFile";
            TMPCOL.Caption = "PathFile";
            MemoryTable.Columns.Add(TMPCOL);

            TMPCOL = new DataColumn();
            TMPCOL.DataType = typeof(String);
            TMPCOL.ColumnName = "RealName";
            TMPCOL.Caption = "RealName";
            MemoryTable.Columns.Add(TMPCOL);

            foreach (var Data in package._chunks.ToArray())
            {
                DataRow NewRow = MemoryTable.NewRow();
                NewRow["Hash"] = Data.Value.Hash.ToString("X");
                NewRow["Offset"] = Data.Value.Offset.ToString("X");
                NewRow["Size"] = Data.Value.Size;
                NewRow["Checksum"] = Data.Value.Checksum.ToString("X");
                NewRow["Time"] = Data.Value.Time.ToString("X");
                MemoryTable.Rows.Add(NewRow);
            }

            DataView dataView = new DataView(MemoryTable);
            this.dataGridView1.DataSource = dataView;

            EnableFindFile(true);
            toolStripStatusLabel1.Text = "PackageIndex loaded !";
        }

        public void CreateFoldersForPath(string filePath)
        {
            string directoryPath = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrEmpty(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Task.Run(() => ExtractAllFiles());
        }

        private void ExtractAllFiles()
        {
            if (textBox1.Text == "")
            {
                string boxmsg = "No FileList selected !";
                string boxtitle = "ERROR";
                MessageBox.Show(boxmsg, boxtitle);
                return;
            }

            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                UpdateStatus("Extracting ... ");

                string basepath = System.IO.Path.GetDirectoryName(this.textBox1.Text);
                string NFSFile = basepath + "\\" + (string)r.Cells[7].Value;
                string OutFile = basepath + "\\" + (string)r.Cells[0].Value;
                uint Size = Convert.ToUInt32((string)r.Cells[2].Value);
                uint Offset = Convert.ToUInt32((string)r.Cells[1].Value, 16);
                ulong Hash = Convert.ToUInt64((string)r.Cells[0].Value, 16);

                //Scratter file - Sorry about name i'm too mutch do android crap stuff
                if (r.Cells[8].Value != DBNull.Value)
                {
                    if ((string)r.Cells[8].Value != "")
                    {
                        OutFile = (string)r.Cells[8].Value;
                        OutFile = basepath + "\\" + OutFile;
                        CreateFoldersForPath(OutFile);
                    }
                }

                LoadedDrive.ExtractChunkToFile(NFSFile, Hash, Offset, Size, OutFile);
                UpdateStatus("File Extracted : " + OutFile);
            }
            // Update the uzer
            UpdateStatus("HUMMM was good - Extract Finish.");
        }

        private void UpdateStatus(string status)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(UpdateStatus), status);
            }
            else
            {
                toolStripStatusLabel1.Text = status;
            }
        }


        private void button9_Click(object sender, EventArgs e)
        {
            // Unpack selected chunk

            if (textBox1.Text == "")
            {
                string boxmsg = "No FileList selected !";
                string boxtitle = "ERROR";
                MessageBox.Show(boxmsg, boxtitle);
                return;
            }

            foreach (DataGridViewRow r in dataGridView1.SelectedRows)
            {
                toolStripStatusLabel1.Text = "Extracting ... ";
                string basepath = System.IO.Path.GetDirectoryName(this.textBox1.Text);
                string NFSFile = basepath + "\\" + (string)r.Cells[7].Value;
                string OutFile = basepath + "\\" + (string)r.Cells[0].Value;
                uint Size = Convert.ToUInt32((string)r.Cells[2].Value);
                uint Offset = Convert.ToUInt32((string)r.Cells[1].Value, 16);
                ulong Hash = Convert.ToUInt64((string)r.Cells[0].Value, 16);

                //Scratter file
                if (r.Cells[8].Value != DBNull.Value)
                { 
                    if ((string)r.Cells[8].Value != "")
                    {
                        OutFile = (string)r.Cells[8].Value;
                        OutFile = basepath + "\\" + OutFile;
                        CreateFoldersForPath(OutFile);
                    }
                }

                LoadedDrive.ExtractChunkToFile(NFSFile, Hash, Offset, Size, OutFile);
                toolStripStatusLabel1.Text = "File Extracted : " + OutFile;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // load the file list

            if (textBox1.Text == "" || LoadedDrive.valid == false)
            {
                string boxmsg = "No FileList selected/loaded !";
                string boxtitle = "ERROR";
                MessageBox.Show(boxmsg, boxtitle);
                return;
            }
            toolStripStatusLabel1.Text = "Loading FileList ... ";


            // OLD WAY too mutch HDD killer :( - RIP Mauris 2016-2018, Gilbert 2022-2023
            // i just keep it here for remember that way exist lol
            /*
            string basepath = System.IO.Path.GetDirectoryName(this.textBox1.Text) + "\\nfs";
            if (!Directory.Exists(basepath))
            {
                basepath = System.IO.Path.GetDirectoryName(this.textBox1.Text) + "\\..\\nfs";
                if (!Directory.Exists(basepath))
                {
                    string boxmsg = "Folder NFS is not findable close of the FileIndex !";
                    string boxtitle = "ERROR";
                    MessageBox.Show(boxmsg, boxtitle);
                    return;
                }
            }

            Thread ThreadList = new Thread(() => PrintFilePathForExtract(basepath));
            ThreadList.Start();
            DataView dataView = new DataView(MemoryTable);
            this.dataGridView1.DataSource = dataView;
            */

            this.openFileDialog1.Filter = "FileList (*.txt)|*.txt";
            this.openFileDialog1.Multiselect = false;
            this.openFileDialog1.Title = "Find any FileList from xLeg game";
            this.openFileDialog1.FileName = "FileList.txt";
            DialogResult txt_file = this.openFileDialog1.ShowDialog();
            if (txt_file == System.Windows.Forms.DialogResult.OK)
            {
                EnableFindFile(false);
                string fileListPCPath = openFileDialog1.FileName;
                List<FileEntry> fileEntries = FileListPCParser.Parse(fileListPCPath);

                Thread ThreadList = new Thread(() => MergeFileListAndPackageIndex(fileEntries));
                ThreadList.Start();
            }

            
        }
        private void MergeFileListAndPackageIndex(List<FileEntry> fileEntries)
        {
            string nfspath = "nfs";
            string basepath = System.IO.Path.GetDirectoryName(this.textBox1.Text) + "\\" + nfspath;
            if (!Directory.Exists(basepath))
            {
                nfspath = "..\\nfs";
                basepath = System.IO.Path.GetDirectoryName(this.textBox1.Text) + "\\" + nfspath;
                if (!Directory.Exists(basepath))
                {
                    string boxmsg2 = "Folder NFS is not findable close of the FileIndex !";
                    string boxtitle2 = "ERROR";
                    MessageBox.Show(boxmsg2, boxtitle2);
                    return;
                }
            }

            int row = 0;
            foreach (DataRow r in MemoryTable.Rows)
            {
                ulong hash = Convert.ToUInt64(r["Hash"].ToString(), 16);
                var data = fileEntries.Find(x => x.Hash == hash);
                if (data != null)
                {
                    MemoryTable.Rows[row]["SizeZip"] = data.CompressedSize;
                    MemoryTable.Rows[row]["ChecksumZip"] = data.CheckSumCompress.ToString("X");
                    MemoryTable.Rows[row]["PathFile"] = nfspath + "\\" + data.NFSPath + data.NFSFile;
                }

                row++;
            }
            RenameFindFile();
            EnableExtract(true);
            EnableFindFile(true);
        }

        /*
        private void PrintFilePathForExtract(string basepath)
        {
            int row = 0;
            foreach (DataRow r in MemoryTable.Rows)
            {
                Chunk chunk = new Chunk();

                chunk.Offset = Convert.ToUInt32(r[0].ToString(), 16);
                chunk.Size = Convert.ToUInt32(r[1].ToString());
                chunk.Checksum = Convert.ToUInt32(r[2].ToString(), 16);
                chunk.Time = Convert.ToUInt32(r[3].ToString());
              
                string data = LoadedDrive.FindFileForChunk(basepath, chunk);
                if (data != null)
                {
                    MemoryTable.Rows[row][4] = data;
                }
                
                row++;
            }
            //EnableFindFile(true);
        }
        */

        private void UpdateGrid1RowEdit(int id, string data)
        {
            // let's grow a tree =D
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.Invoke(new MethodInvoker(() => dataGridView1.Rows[id].Cells[4].Value = data));
            }
            else
            {
                dataGridView1.Rows[id].Cells[4].Value = data;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }



        public class FileScatter
        {
            public ulong Hash { get; set; }
            public string RealName { get; set; }
        }

        private void button4_Click(object sender, EventArgs e)
        {
 
            // Ok boyz let me explain, this is HASH so there is no way to recover without know it (i use brutefore but for sure better way exist)
            this.openFileDialog1.Filter = "Map_List (*.txt)|*.txt";
            this.openFileDialog1.Multiselect = false;
            this.openFileDialog1.Title = "Select your Map_List";
            this.openFileDialog1.FileName = "Map_List.txt";
            DialogResult txt_file = this.openFileDialog1.ShowDialog();
            if (txt_file == System.Windows.Forms.DialogResult.OK)
            {
                toolStripStatusLabel1.Text = "Loading Scatter File ...";
                List<FileScatter> FileWithHashAndRealName = new List<FileScatter>();
                string[] lines = File.ReadAllLines(this.openFileDialog1.FileName);
                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 2)
                    {
                        ulong hash = Convert.ToUInt64(parts[0], 16);
                        string realName = parts[1];
                        FileWithHashAndRealName.Add(new FileScatter { Hash = hash, RealName = realName });
                    }
                    else if (parts.Length == 3)
                    {
                        uint hash = Convert.ToUInt32(parts[0], 16);
                        string realName = parts[2];
                        FileWithHashAndRealName.Add(new FileScatter { Hash = hash, RealName = realName });
                    }
                }

                Thread ThreadList = new Thread(() => MergeScatterAndDataTable(FileWithHashAndRealName));
                ThreadList.Start();

            }
            toolStripStatusLabel1.Text = "Scatter Loaded !";
        }

        private void MergeScatterAndDataTable(List<FileScatter> FileWithHashAndRealName)
        {
            int row = 0;
            foreach (DataRow r in MemoryTable.Rows)
            {
                ulong hash = Convert.ToUInt64(r["Hash"].ToString(), 16);
                var data = FileWithHashAndRealName.Find(x => x.Hash == hash);
                if (data != null)
                {
                    MemoryTable.Rows[row]["RealName"] = data.RealName;
                }

                row++;
            }
            ToolStrip toolStrip = toolStripStatusLabel1.GetCurrentParent();

            if (toolStrip.InvokeRequired)
            {
                toolStrip.Invoke(new Action(() => toolStripStatusLabel1.Text = "Scatter File Loaded !"));
            }
            else
            {
                toolStripStatusLabel1.Text = "Scatter File Loaded !";
            }
        }


        public void SaveDataGridViewToCSV(DataGridView dataGridView, string filePath, string separator = ",")
        {
            StringBuilder csvContent = new StringBuilder();

            //Ok maybe i abuse
            //separator = "|";

            for (int i = 0; i < dataGridView.Columns.Count; i++)
            {
                csvContent.Append(dataGridView.Columns[i].HeaderText);
                if (i < dataGridView.Columns.Count - 1)
                {
                    csvContent.Append(separator);
                }
            }
            csvContent.AppendLine();

            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView.Columns.Count; j++)
                {
                    csvContent.Append(dataGridView.Rows[i].Cells[j].Value);
                    if (j < dataGridView.Columns.Count - 1)
                    {
                        csvContent.Append(separator);
                    }
                }
                csvContent.AppendLine();
            }

            File.WriteAllText(filePath, csvContent.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (textBox1.Text == "" || LoadedDrive.valid == false)
            {
                string boxmsg = "No FileList selected/loaded !";
                string boxtitle = "ERROR";
                MessageBox.Show(boxmsg, boxtitle);
                return;
            }
            toolStripStatusLabel1.Text = "Start Write List.csv ...";
            string basepath = System.IO.Path.GetDirectoryName(this.textBox1.Text) + "\\" ;
            string path = basepath + "List.csv";
            SaveDataGridViewToCSV(dataGridView1, path);
            toolStripStatusLabel1.Text = "List saved at : " + path;
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }
    }

}
