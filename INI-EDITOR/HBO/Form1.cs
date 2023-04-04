using Model_Converter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ini_Editor
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private const int WM_SETREDRAW = 11;
        private int _keyValue;
        private Boolean _checkKeyValue = false;
        private List<string> ListOfRow;
        Find findwindows;

        public static string[] ColumnsHeader;

        public string HeaderCopy;
        public string FilenameOriginal;

        GeneralFunc BasicFunc = new GeneralFunc();
        FileThinker FileThink = new FileThinker();

        public Form1()
        {
            BasicFunc.CreatePath("header");
            InitializeComponent();
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            if (!System.Windows.Forms.SystemInformation.TerminalServerSession)
            {
              Type dgvType = dataGridView1.GetType();
              PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
              BindingFlags.Instance | BindingFlags.NonPublic);
              pi.SetValue(dataGridView1, true, null);
            }
        }

        public class MemoryFind
        {
            public int Row { set; get; }
            public int Coll { set; get; }
        }

        //All Exel like stuff
        private void CopyToClipboard()
        {
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        private void PasteClipboardValue()
        {
            if (dataGridView1.SelectedCells.Count == 0)
                return;

            DataGridViewCell startCell = GetStartCell(dataGridView1);
            Dictionary<int, Dictionary<int, string>> cbValue =
                    ClipBoardValues(Clipboard.GetText());

            int iRowIndex = startCell.RowIndex;
            foreach (int rowKey in cbValue.Keys)
            {
                int iColIndex = startCell.ColumnIndex;
                foreach (int cellKey in cbValue[rowKey].Keys)
                {
                    if (iColIndex <= dataGridView1.Columns.Count - 1
                    && iRowIndex <= dataGridView1.Rows.Count - 1)
                    {
                        DataGridViewCell cell = dataGridView1[iColIndex, iRowIndex];
                        if (cell.Selected)
                            cell.Value = cbValue[rowKey][cellKey];
                    }
                    iColIndex++;
                }
                iRowIndex++;
            }
        }

        private DataGridViewCell GetStartCell(DataGridView dgView)
        {
            if (dgView.SelectedCells.Count == 0)
                return null;

            int rowIndex = dgView.Rows.Count - 1;
            int colIndex = dgView.Columns.Count - 1;

            foreach (DataGridViewCell dgvCell in dgView.SelectedCells)
            {
                if (dgvCell.RowIndex < rowIndex)
                    rowIndex = dgvCell.RowIndex;
                if (dgvCell.ColumnIndex < colIndex)
                    colIndex = dgvCell.ColumnIndex;
            }

            return dgView[colIndex, rowIndex];
        }

        private Dictionary<int, Dictionary<int, string>> ClipBoardValues(string clipboardValue)
        {
            Dictionary<int, Dictionary<int, string>>
            copyValues = new Dictionary<int, Dictionary<int, string>>();

            String[] lines = clipboardValue.Split('\n');

            for (int i = 0; i <= lines.Length - 1; i++)
            {
                copyValues[i] = new Dictionary<int, string>();
                String[] lineContent = lines[i].Split('\t');
                if (lineContent.Length == 0)
                    copyValues[i][0] = string.Empty;
                else
                {
                    for (int j = 0; j <= lineContent.Length - 1; j++)
                        copyValues[i][j] = lineContent[j];
                }
            }
            return copyValues;
        }
        private DataTable GetDataGridViewAsDataTable(DataGridView _DataGridView)
        {
            try
            {
                if (_DataGridView.ColumnCount == 0) return null;
                DataTable dtSource = new DataTable();
                foreach (DataGridViewColumn col in _DataGridView.Columns)
                {
                    if (col.ValueType == null) dtSource.Columns.Add(col.Name, typeof(string));
                    else dtSource.Columns.Add(col.Name, col.ValueType);
                    dtSource.Columns[col.Name].Caption = col.HeaderText;
                }
                foreach (DataGridViewRow row in _DataGridView.Rows)
                {
                    DataRow drNewRow = dtSource.NewRow();
                    foreach (DataColumn col in dtSource.Columns)
                    {
                        drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value;
                    }
                    dtSource.Rows.Add(drNewRow);
                }
                return dtSource;
            }
            catch
            {
                return null;
            }
        }
        private void FastAutoSizeColumns(DataGridView targetGrid)
        {
           var gridTable = GetDataGridViewAsDataTable(targetGrid);
           using (var gfx = targetGrid.CreateGraphics())
            {

                for (int i = 0; i < gridTable.Columns.Count; i++)
                {
                    string[] colStringCollection = gridTable.AsEnumerable().Where(r => r.Field<object>(i) != null).Select(r => r.Field<object>(i).ToString()).ToArray();
                    colStringCollection = colStringCollection.OrderBy((x) => x.Length).ToArray();
                    string longestColString = colStringCollection.Last();
                    var colWidth = gfx.MeasureString(longestColString, targetGrid.Font);
                    if (colWidth.Width > targetGrid.Columns[i].HeaderCell.Size.Width)
                    {
                        targetGrid.Columns[i].Width = (int)colWidth.Width;
                    }
                    else 
                    {
                        targetGrid.Columns[i].Width = targetGrid.Columns[i].HeaderCell.Size.Width;
                    }
                }
            }
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
                dataGridView1.ContextMenuStrip = contextMenuStrip2;
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyToClipboard();
        }

        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CopyToClipboard();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteClipboardValue();
        }


        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PasteClipboardValue();
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CopyToClipboard();
            foreach (DataGridViewCell dgvCell in dataGridView1.SelectedCells)
                dgvCell.Value = string.Empty;
        }
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyToClipboard();
            foreach (DataGridViewCell dgvCell in dataGridView1.SelectedCells)
                dgvCell.Value = string.Empty;
        }

        private delegate void UpdateRowDelegate(object value);

        private void AddRowSafe(object row)
        {
            if (this.dataGridView1.InvokeRequired)
            {
                // Delegate my brother
                this.dataGridView1.Invoke(new UpdateRowDelegate(this.AddRowSafe), row);
            }
            else
            {
                // Do the work HERE!
                SendMessage(dataGridView1.Handle, WM_SETREDRAW, false, 0);
				
                this.dataGridView1.Rows.Add(BasicFunc.CutLineVertical(row.ToString()));
				//this.dataGridView1.Rows.AddRange(rows.ToArray());
				
                SendMessage(dataGridView1.Handle, WM_SETREDRAW, true, 0);
                dataGridView1.Refresh();
            }
        }

		private delegate void UpdateRowsDelegate(List<string>  value);
		
        private void AddRowsSafe(List<string> rows)
        {
            if (this.dataGridView1.InvokeRequired)
            {
                // Delegate my brother
                this.dataGridView1.Invoke(new UpdateRowsDelegate(this.AddRowsSafe), rows);

            }
            else
            {
                // Do the work HERE!
                SendMessage(dataGridView1.Handle, WM_SETREDRAW, false, 0);
                List<DataGridViewRow> OutputRows = new List<DataGridViewRow>();

                foreach (string RowsIni in rows)
                {
                    DataGridViewRow RowNew = new DataGridViewRow();
                    RowNew.CreateCells(dataGridView1);
                    string[] lines = BasicFunc.CutLineVertical(RowsIni);

                    for (int y = 0; y < lines.Count(); y++)
                    {
                        if(lines[y] != "")
                        RowNew.Cells[y].Value = lines[y];
                    }

                    OutputRows.Add(RowNew);
                }
                
                this.dataGridView1.Rows.AddRange(OutputRows.ToArray());
                
                SendMessage(dataGridView1.Handle, WM_SETREDRAW, true, 0);
                dataGridView1.Refresh();
                FastAutoSizeColumns(dataGridView1);
            }
        }
		
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (_checkKeyValue)
            {
                _checkKeyValue = false;

                if (_keyValue != -1)
                {
                    cell.Value = _keyValue;
                }
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 1)
            {
                _checkKeyValue = true;
                _keyValue = (int)e.KeyValue;
                dataGridView1.BeginEdit(false);
            }
        }

        public void FocusOnCell(int cell, int Column)
        {
           dataGridView1.CurrentCell = dataGridView1.Rows[cell].Cells[Column];
        }

        public List<MemoryFind> FindAllInCell(string searchValue , string Column, bool casecheck = false)
        {
            List<MemoryFind> Return = new List<MemoryFind>();
            int rowIndex = -1;
            if (Column == "All Columns")
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int y = 0; y < dataGridView1.ColumnCount; y++)
                    {
                        if (dataGridView1.Rows[i].Cells[y].Value != null)
                        {
                            if (casecheck == true)
                            {
                                if (dataGridView1.Rows[i].Cells[y].Value.ToString().Contains(searchValue))
                                {
                                    rowIndex = i;
                                    Return.Add(new MemoryFind { Row = rowIndex, Coll = y });
                                }
                            }
                            else
                            {
                                if (dataGridView1.Rows[i].Cells[y].Value.ToString().ToLower().Contains(searchValue.ToLower()))
                                {
                                    rowIndex = i;
                                    Return.Add(new MemoryFind { Row = rowIndex, Coll = y });
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells[Column].Value != null) // Find With coll
                    {
                        if (casecheck == true)
                        {
                            if (row.Cells[Column].Value.ToString().Contains(searchValue))
                            {
                                rowIndex = row.Index;
                                Return.Add(new MemoryFind { Row = rowIndex, Coll = row.Cells[Column].ColumnIndex });
                            }
                        }
                        else
                        {
                            if (row.Cells[Column].Value.ToString().ToLower().Contains(searchValue.ToLower()))
                            {
                                rowIndex = row.Index;
                                Return.Add(new MemoryFind { Row = rowIndex, Coll = row.Cells[Column].ColumnIndex });
                            }
                        }
                    }

                }
            }
            return Return;
        }

        //Buttons 
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenIniFile();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        //Functionality 

        private void AddsRows()
        {
            //SLOOWWW
            //foreach (string RowsIni in ListOfRow)
            //    AddRowSafe(RowsIni);
            AddRowsSafe(ListOfRow);
            ListOfRow.Clear();
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            //test button for now :)
            /*
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "Random";
            dataGridView1.Columns[1].Name = "Random";
            dataGridView1.Columns[2].Name = "Random";

            string[] row = new string[] { "1", "random", "1000" };
            dataGridView1.Rows.Add(row);
            row = new string[] { "2", "Random", "2000" };
            dataGridView1.Rows.Add(row);
            row = new string[] { "3", "Random", "3000" };
            dataGridView1.Rows.Add(row);
            row = new string[] { "4", "Random", "4000" };
            dataGridView1.Rows.Add(row);
            */
        }

        private void OpenIniFile()
        {
            // open dialog here
            this.openFileDialog1.Filter = "HBO ini (*.ini)|*.ini";
            this.openFileDialog1.Multiselect = false;
            this.openFileDialog1.Title = "Find ini file from HBO";
            this.openFileDialog1.FileName = "Any HBO ini file";
            string line = "";
            int fileerror = 0;
            Encoding big5 = Encoding.GetEncoding("big5");

            DialogResult ininode_files = this.openFileDialog1.ShowDialog();

            if (ininode_files == System.Windows.Forms.DialogResult.OK)
            {
                //clean old stuff
                this.dataGridView1.DataSource = null;
                this.dataGridView1.Rows.Clear();
                HeaderCopy = "";

                string nameoutput = Path.GetFileName(openFileDialog1.FileName);
                FilenameOriginal = openFileDialog1.FileName;
                string safefilename = nameoutput;

                if (safefilename.Split('_').Count() > 1)
                    safefilename = safefilename.Split('_')[1];

                this.Text = "Ini Editor - " + nameoutput;

                // Ok it's time to work

                //openFileDialog1.FileName
                List<string> filedata = FileThink.GetFileRow(openFileDialog1.FileName);
                ListOfRow = new List<string>();
                int lignenumber = 1;
                for (int z = 0; z < filedata.Count; z++)
                {
                    line = filedata[z];

                    if (lignenumber != 1 && line.Contains('|') == true)
                    {
                        ListOfRow.Add(line);
                    }
                    else if (lignenumber == 1)
                    {
                        HeaderCopy = line;
                        bool FindMatch = false;
                        string[] returndata = BasicFunc.CutLineVertical(line);
                        //check if exist
                        DirectoryInfo d = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\header");
                        FileInfo[] Files = d.GetFiles("*.col");
                        foreach (FileInfo FileListed in Files)
                        {
                            string extractedname = FileListed.Name;

                            if (FileListed.Name.Split('_').Count() > 1)
                                extractedname = FileListed.Name.Split('_')[1];

                            if (extractedname.Replace(".col", "").ToLower() == safefilename.Replace(".ini", "").ToLower())
                            {
                                // if yes put header nice
                                FindMatch = true;
                                string[] ColName = System.IO.File.ReadAllLines(FileListed.FullName);
                                ColumnsHeader = new string[0];
                                int ColNumber = Int32.Parse(returndata[3]);
                                dataGridView1.ColumnCount = ColNumber;
                                for (int i = 0; i < ColNumber; i++)
                                {
                                    if (ColName.ElementAtOrDefault(i) != null)
                                    {
                                        dataGridView1.Columns[i].Name = ColName[i];
                                        Array.Resize(ref ColumnsHeader, ColumnsHeader.Length + 1);
                                        ColumnsHeader[ColumnsHeader.Length - 1] = ColName[i];
                                    }
                                    else
                                    {
                                        dataGridView1.Columns[i].Name = "Unknow " + i;
                                        Array.Resize(ref ColumnsHeader, ColumnsHeader.Length + 1);
                                        ColumnsHeader[ColumnsHeader.Length - 1] = "Unknow " + i;
                                    }
                                }

                            }

                        }
                        //if not create new header
                        if (FindMatch == false)
                        {
                            using (System.IO.StreamWriter NewFile = new System.IO.StreamWriter("header\\" + safefilename.Replace(".ini", "") + ".col"))
                            {
                                ColumnsHeader = new string[0];
                                int ColNumber = Int32.Parse(returndata[3]);
                                dataGridView1.ColumnCount = ColNumber;
                                for (int i = 0; i < ColNumber; i++)
                                {
                                    dataGridView1.Columns[i].Name = "Unknow " + i;
                                    NewFile.WriteLine(dataGridView1.Columns[i].Name);
                                    Array.Resize(ref ColumnsHeader, ColumnsHeader.Length + 1);
                                    ColumnsHeader[ColumnsHeader.Length - 1] = "Unknow " + i;
                                }
                            }
                        }


                        foreach (DataGridViewColumn c in dataGridView1.Columns)
                        {
                            c.SortMode = DataGridViewColumnSortMode.NotSortable;
                            c.Selected = false;
                        }
                    }

                    lignenumber++;
                }


                if (fileerror == 1)
                {
                    this.dataGridView1.DataSource = null;
                    this.dataGridView1.Rows.Clear();
                    HeaderCopy = "";
                }
                else
                {
                    //enable save button
                   // saveFileToolStripMenuItem.Enabled = true;

                    var tasks = Task.Factory.StartNew(() =>
                    {
                        AddsRows();
                    }, TaskCreationOptions.LongRunning);
                }
            }
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Find>().Count() == 1)
                findwindows.Activate();
            else
            {
                findwindows = new Find(this);
                findwindows.Show();
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

        private void rToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<Find>().Count() == 1)
                findwindows.Activate();
            else
            {
                findwindows = new Find(this);
                findwindows.Show();
            }
        }

        private void toolStripDropDownButton2_Click(object sender, EventArgs e)
        {

        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Save part ...
            string path = Path.GetDirectoryName(FilenameOriginal) + "\\edit";
            string nameoutput = Path.GetFileName(FilenameOriginal);
            string outputfilename = "";

            if (safeModeToolStripMenuItem.Checked == true)
            {
                if (BasicFunc.CreatePath(path) == false)
                {
                    string boxmsg = "I can't save - Impossible to create path. \nSorry !";
                    string boxtitle = "Permission ERROR";
                    MessageBox.Show(boxmsg, boxtitle);
                    return;
                }

                if (File.Exists(path + "\\" + nameoutput))
                {
                    File.Delete(path + "\\" + nameoutput);
                }

                outputfilename = path + "\\" + nameoutput;
            }
            else
            {
                if (File.Exists(FilenameOriginal))
                {
                    File.Delete(FilenameOriginal);
                }
                outputfilename = FilenameOriginal;
            }




            Encoding big5 = Encoding.GetEncoding("big5");

            using (System.IO.StreamWriter fileoutput = new System.IO.StreamWriter(outputfilename, true, big5))
            {
                fileoutput.WriteLine(HeaderCopy);

                string line = "";
                for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                {
                    line = "";
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        if (dataGridView1.Rows[i].Cells[j].Value != null)
                            line += dataGridView1.Rows[i].Cells[j].Value.ToString() + "|";
                        else
                            line += "|";
                    }

                    if (i + 1 < dataGridView1.RowCount - 1)
                        fileoutput.WriteLine(line);
                    else
                        fileoutput.Write(line);
                }

            }

            string box_msg = "";
            if (safeModeToolStripMenuItem.Checked == true)
                box_msg = "File : \\edit\\" + nameoutput + " have been saved (SafeMode).";
            else
                box_msg = "File : " + nameoutput + " have been saved.";

            string box_title = "File saved";
            MessageBox.Show(box_msg, box_title);

        }

        private void safeModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (safeModeToolStripMenuItem.Checked == true)
                safeModeToolStripMenuItem.Checked = false;
            else
                safeModeToolStripMenuItem.Checked = true;
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dataGridView1.ClearSelection();
            DataGridViewCell cell = dataGridView1.Rows[0].Cells[e.ColumnIndex];
            dataGridView1.CurrentCell = cell;
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                dataGridView1.Rows[i].Cells[e.ColumnIndex].Selected = true;
            }

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];

            foreach (DataGridViewCell cells in dataGridView1.SelectedCells)
                dataGridView1.Rows[cells.RowIndex].Cells[cells.ColumnIndex].Value = cell.EditedFormattedValue;

        }


        private void toolStripDropDownButton3_Click(object sender, EventArgs e)
        {
            string box_msg = "This function is not allowed on this version ! \nSorry !";
            string box_title = "Registration Error";
            MessageBox.Show(box_msg, box_title);
        }
    }
}
