using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FortuneBag
{
    public partial class Form1 : Form
    {
        List<BagRow> _allRows = new List<BagRow>();
        Dictionary<int, string> _itemNames = new Dictionary<int, string>();
        GameSchema _schema = GameSchema.Unknown;
        string _loadedFilePath = "";
        int _currentBagId = -1;

        Rectangle _dragBox = Rectangle.Empty;
        int _dragRowIndex = -1;

        public Form1()
        {
            InitializeComponent();
        }

        void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S) { BtnSave_Click(sender, e); e.Handled = true; }
            if (e.Control && e.KeyCode == Keys.O) { BtnLoad_Click(sender, e); e.Handled = true; }
            if (e.KeyCode == Keys.Delete && _grid.Focused) { BtnDel_Click(sender, e); e.Handled = true; }
        }

        void UpdateStatus()
        {
            _statusFile.Text = string.IsNullOrEmpty(_loadedFilePath) ? "No file loaded" : Path.GetFileName(_loadedFilePath);
            _statusSchema.Text = _schema == GameSchema.Unknown ? "" : _schema.ToString();
            _statusRows.Text = _allRows.Count > 0 ? _allRows.Count + " rows / " + _allRows.Select(r => r.Id).Distinct().Count() + " bags" : "";
        }

        void BtnLoad_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog { Filter = "SQL files|*.sql;*.txt|All files|*.*" })
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                _loadedFilePath = dlg.FileName;
                var parser = new SqlParser();
                _allRows = parser.Parse(_loadedFilePath);
                _schema = parser.Schema;
                _itemNames.Clear();
                _currentBagId = -1;
                _grid.Columns.Clear();
                _grid.Rows.Clear();
                RefreshBoxList("");
                UpdateStatus();
            }
        }

        void RefreshBoxList(string filter)
        {
            var ids = _allRows.Select(r => r.Id).Distinct().OrderBy(x => x);
            if (!string.IsNullOrWhiteSpace(filter))
                ids = ids.Where(x => x.ToString().Contains(filter)).OrderBy(x => x);

            _boxList.Items.Clear();
            foreach (var id in ids) _boxList.Items.Add(id);
        }

        void SearchBox_TextChanged(object sender, EventArgs e)
        {
            RefreshBoxList(_searchBox.Text);
        }

        void BoxList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_boxList.SelectedItem == null) return;
            SyncCurrentBag();
            int newId = (int)_boxList.SelectedItem;
            _currentBagId = newId;
            ShowBox(newId);
        }

        void ShowBox(int boxId)
        {
            var rows = _allRows.Where(r => r.Id == boxId).ToList();
            _grid.Columns.Clear();
            _grid.Rows.Clear();

            foreach (var col in GetColumnNames())
                _grid.Columns.Add(col, col);

            if (_itemNames.Count > 0)
                _grid.Columns.Add(new DataGridViewTextBoxColumn { Name = "name", HeaderText = "name", ReadOnly = true });

            foreach (var row in rows)
            {
                var values = row.ToColumns().Cast<object>().ToList();
                if (_itemNames.Count > 0)
                {
                    string name;
                    _itemNames.TryGetValue(row.ItemId, out name);
                    values.Add(name ?? "");
                }
                _grid.Rows.Add(values.ToArray());
            }
        }

        string[] GetColumnNames()
        {
            switch (_schema)
            {
                case GameSchema.LaPlace: return new[] { "id", "sequence", "set", "item_id", "item_num", "probability", "ach_probability", "item_counter", "bulletin", "note" };
                case GameSchema.EdenEternal: return new[] { "id", "sequence", "set", "item_id", "item_num", "probability", "item_counter", "bulletin", "embedded", "bind", "note" };
                case GameSchema.GrandFantasia: return new[] { "id", "sequence", "set", "item_id", "item_num", "probability", "bulletin", "white", "green", "blue", "yellow", "note" };
                default: return new[] { "id", "sequence", "set", "item_id", "item_num", "probability", "note" };
            }
        }

        void BtnAdd_Click(object sender, EventArgs e)
        {
            if (_boxList.SelectedItem == null) return;
            int boxId = (int)_boxList.SelectedItem;
            var cols = GetColumnNames();
            var values = new object[cols.Length + (_itemNames.Count > 0 ? 1 : 0)];
            values[0] = boxId.ToString();
            int nextSeq = 1;
            if (_grid.Rows.Count > 0)
            {
                int lastSeq;
                if (int.TryParse(_grid.Rows[_grid.Rows.Count - 1].Cells["sequence"]?.Value?.ToString(), out lastSeq))
                    nextSeq = lastSeq + 1;
            }
            values[1] = nextSeq.ToString();
            for (int i = 2; i < cols.Length; i++) values[i] = "0";
            if (cols[cols.Length - 1] == "note") values[cols.Length - 1] = "";
            if (_itemNames.Count > 0) values[cols.Length] = "";
            _grid.Rows.Add(values);
        }

        void BtnDel_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in _grid.SelectedRows)
                if (!row.IsNewRow) _grid.Rows.Remove(row);
        }

        void BtnNewBag_Click(object sender, EventArgs e)
        {
            if (_schema == GameSchema.Unknown)
            {
                MessageBox.Show("Load a SQL file first to detect the schema.");
                return;
            }

            string input = ShowInputDialog("New Bag ID:", "New Bag");
            if (input == null) return;

            int newId;
            if (!int.TryParse(input, out newId))
            {
                MessageBox.Show("Invalid ID.");
                return;
            }

            if (_allRows.Any(r => r.Id == newId))
            {
                MessageBox.Show("Bag " + newId + " already exists.");
                return;
            }

            var row = new BagRow { Schema = _schema, Id = newId, Sequence = 1 };
            _allRows.Add(row);
            RefreshBoxList(_searchBox.Text);
            SelectBag(newId);
            UpdateStatus();
        }

        void BtnDelBag_Click(object sender, EventArgs e)
        {
            if (_boxList.SelectedItem == null) return;
            int boxId = (int)_boxList.SelectedItem;

            if (MessageBox.Show("Delete bag " + boxId + " and all its rows?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            _allRows.RemoveAll(r => r.Id == boxId);
            _currentBagId = -1;
            _grid.Columns.Clear();
            _grid.Rows.Clear();
            RefreshBoxList(_searchBox.Text);
            UpdateStatus();
        }

        void BtnDupBag_Click(object sender, EventArgs e)
        {
            if (_boxList.SelectedItem == null) return;
            int srcId = (int)_boxList.SelectedItem;
            SyncCurrentBag();

            string input = ShowInputDialog("New Bag ID for copy of " + srcId + ":", "Duplicate Bag");
            if (input == null) return;

            int newId;
            if (!int.TryParse(input, out newId))
            {
                MessageBox.Show("Invalid ID.");
                return;
            }

            if (_allRows.Any(r => r.Id == newId))
            {
                MessageBox.Show("Bag " + newId + " already exists.");
                return;
            }

            var srcRows = _allRows.Where(r => r.Id == srcId).ToList();
            foreach (var src in srcRows)
            {
                var copy = new BagRow
                {
                    Schema = src.Schema, Id = newId, Sequence = src.Sequence, Set = src.Set,
                    ItemId = src.ItemId, ItemNum = src.ItemNum, Probability = src.Probability,
                    AchProbability = src.AchProbability, ItemCounter = src.ItemCounter,
                    Bulletin = src.Bulletin, Embedded = src.Embedded, Bind = src.Bind,
                    White = src.White, Green = src.Green, Blue = src.Blue, Yellow = src.Yellow,
                    Note = src.Note
                };
                _allRows.Add(copy);
            }

            RefreshBoxList(_searchBox.Text);
            SelectBag(newId);
            UpdateStatus();
        }

        void BtnMoveUp_Click(object sender, EventArgs e)
        {
            if (_grid.SelectedRows.Count != 1) return;
            int idx = _grid.SelectedRows[0].Index;
            if (idx <= 0) return;
            SwapGridRows(idx, idx - 1);
            _grid.ClearSelection();
            _grid.Rows[idx - 1].Selected = true;
        }

        void BtnMoveDown_Click(object sender, EventArgs e)
        {
            if (_grid.SelectedRows.Count != 1) return;
            int idx = _grid.SelectedRows[0].Index;
            if (idx >= _grid.Rows.Count - 1) return;
            SwapGridRows(idx, idx + 1);
            _grid.ClearSelection();
            _grid.Rows[idx + 1].Selected = true;
        }

        void SwapGridRows(int a, int b)
        {
            for (int c = 0; c < _grid.Columns.Count; c++)
            {
                var tmp = _grid.Rows[a].Cells[c].Value;
                _grid.Rows[a].Cells[c].Value = _grid.Rows[b].Cells[c].Value;
                _grid.Rows[b].Cells[c].Value = tmp;
            }
        }

        void Grid_MouseDown(object sender, MouseEventArgs e)
        {
            var hitTest = _grid.HitTest(e.X, e.Y);
            if (hitTest.RowIndex >= 0)
            {
                _dragRowIndex = hitTest.RowIndex;
                Size dragSize = SystemInformation.DragSize;
                _dragBox = new Rectangle(new Point(e.X - dragSize.Width / 2, e.Y - dragSize.Height / 2), dragSize);
            }
            else
            {
                _dragBox = Rectangle.Empty;
                _dragRowIndex = -1;
            }
        }

        void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || _dragBox == Rectangle.Empty) return;
            if (!_dragBox.Contains(e.X, e.Y))
                _grid.DoDragDrop(_dragRowIndex, DragDropEffects.Move);
        }

        void Grid_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        void Grid_DragDrop(object sender, DragEventArgs e)
        {
            Point clientPoint = _grid.PointToClient(new Point(e.X, e.Y));
            int destIndex = _grid.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            if (destIndex < 0 || destIndex == _dragRowIndex) return;

            var values = new object[_grid.Columns.Count];
            for (int c = 0; c < _grid.Columns.Count; c++)
                values[c] = _grid.Rows[_dragRowIndex].Cells[c].Value;

            _grid.Rows.RemoveAt(_dragRowIndex);
            _grid.Rows.Insert(destIndex, values);
            _grid.ClearSelection();
            _grid.Rows[destIndex].Selected = true;
        }

        void BtnRes_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog { Description = "Select folder with C_Item.ini or C_ItemMall.ini" })
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                _itemNames = LoadItemNames(dlg.SelectedPath);
                MessageBox.Show("Loaded " + _itemNames.Count + " item names.");
                if (_boxList.SelectedItem != null) ShowBox((int)_boxList.SelectedItem);
            }
        }

        Dictionary<int, string> LoadItemNames(string folder)
        {
            var result = new Dictionary<int, string>();
            Encoding big5 = Encoding.GetEncoding("big5");
            foreach (string fname in new[] { "C_Item.ini", "C_ItemMall.ini" })
            {
                string path = Path.Combine(folder, fname);
                if (!File.Exists(path)) continue;
                bool first = true;
                foreach (string line in File.ReadAllLines(path, big5))
                {
                    if (first) { first = false; continue; }
                    if (!line.Contains("|")) continue;
                    string[] parts = line.Split('|');
                    if (parts.Length < 3) continue;
                    int id;
                    if (!int.TryParse(parts[0], out id)) continue;
                    if (!result.ContainsKey(id)) result[id] = parts[2];
                }
            }
            return result;
        }

        void BtnSave_Click(object sender, EventArgs e)
        {
            if (_allRows.Count == 0)
            {
                MessageBox.Show("Nothing to save.");
                return;
            }

            SyncCurrentBag();

            string folder = string.IsNullOrEmpty(_loadedFilePath)
                ? AppDomain.CurrentDomain.BaseDirectory
                : Path.GetDirectoryName(_loadedFilePath);

            string outPath = Path.Combine(folder, "fortune_bag_" + DateTime.Now.ToString("dd_MM_yyyy_HH_mm") + ".sql");
            Encoding big5 = Encoding.GetEncoding("big5");

            using (var sw = new StreamWriter(outPath, false, big5))
            {
                sw.WriteLine(GetDropCreate());
                foreach (var row in _allRows.OrderBy(r => r.Id).ThenBy(r => r.Sequence))
                    sw.WriteLine(ToInsert(row));
            }

            MessageBox.Show("Saved:\n" + outPath);
        }

        void SyncCurrentBag()
        {
            _grid.EndEdit();
            if (_currentBagId < 0 || _grid.Columns.Count == 0) return;
            _allRows.RemoveAll(r => r.Id == _currentBagId);

            var cols = GetColumnNames();
            int seq = 1;
            foreach (DataGridViewRow gridRow in _grid.Rows)
            {
                if (gridRow.IsNewRow) continue;
                var row = new BagRow { Schema = _schema };
                for (int i = 0; i < cols.Length && i < gridRow.Cells.Count; i++)
                    SetField(row, cols[i], gridRow.Cells[i].Value?.ToString() ?? "");
                row.Id = _currentBagId;
                row.Sequence = seq++;
                _allRows.Add(row);
            }
        }

        void SetField(BagRow row, string col, string val)
        {
            var inv = System.Globalization.CultureInfo.InvariantCulture;
            switch (col)
            {
                case "id": int.TryParse(val, out row.Id); break;
                case "sequence": int.TryParse(val, out row.Sequence); break;
                case "set": int.TryParse(val, out row.Set); break;
                case "item_id": int.TryParse(val, out row.ItemId); break;
                case "item_num": int.TryParse(val, out row.ItemNum); break;
                case "probability": float.TryParse(val, System.Globalization.NumberStyles.Any, inv, out row.Probability); break;
                case "ach_probability": float.TryParse(val, System.Globalization.NumberStyles.Any, inv, out row.AchProbability); break;
                case "item_counter": float.TryParse(val, System.Globalization.NumberStyles.Any, inv, out row.ItemCounter); break;
                case "bulletin": int.TryParse(val, out row.Bulletin); break;
                case "embedded": int.TryParse(val, out row.Embedded); break;
                case "bind": int.TryParse(val, out row.Bind); break;
                case "white": float.TryParse(val, System.Globalization.NumberStyles.Any, inv, out row.White); break;
                case "green": float.TryParse(val, System.Globalization.NumberStyles.Any, inv, out row.Green); break;
                case "blue": float.TryParse(val, System.Globalization.NumberStyles.Any, inv, out row.Blue); break;
                case "yellow": float.TryParse(val, System.Globalization.NumberStyles.Any, inv, out row.Yellow); break;
                case "note": row.Note = val; break;
            }
        }

        string GetDropCreate()
        {
            switch (_schema)
            {
                case GameSchema.LaPlace:
                    return "DROP TABLE fortune_bag;\r\nCREATE TABLE fortune_bag (\r\nid integer NOT NULL,\r\nsequence smallint NOT NULL,\r\nset integer NOT NULL,\r\nitem_id integer NOT NULL,\r\nitem_num integer NOT NULL,\r\nprobability real NOT NULL,\r\nach_probability real NOT NULL,\r\nitem_counter real NOT NULL,\r\nbulletin integer NOT NULL,\r\nnote text DEFAULT ''\r\n);";
                case GameSchema.EdenEternal:
                    return "DROP TABLE fortune_bag;\r\nCREATE TABLE fortune_bag (\r\nid integer NOT NULL,\r\nsequence smallint NOT NULL,\r\nset integer NOT NULL,\r\nitem_id integer NOT NULL,\r\nitem_num integer NOT NULL,\r\nprobability real NOT NULL,\r\nitem_counter integer NOT NULL,\r\nbulletin integer NOT NULL,\r\nembedded integer NOT NULL,\r\nbind integer NOT NULL,\r\nnote text DEFAULT ''\r\n);";
                case GameSchema.GrandFantasia:
                    return "DROP TABLE public.fortune_bag;\r\nCREATE TABLE public.fortune_bag (\r\n    id integer NOT NULL,\r\n    sequence smallint NOT NULL,\r\n    set integer NOT NULL,\r\n    item_id integer NOT NULL,\r\n    item_num integer NOT NULL,\r\n    probability real NOT NULL,\r\n    bulletin integer NOT NULL,\r\n    white real NOT NULL,\r\n    green real NOT NULL,\r\n    blue real NOT NULL,\r\n    yellow real NOT NULL,\r\n    note text DEFAULT ''::text\r\n);";
                default:
                    return "DROP TABLE fortune_bag;\r\nCREATE TABLE fortune_bag ();";
            }
        }

        string ToInsert(BagRow row)
        {
            var cols = row.ToColumns();
            var colNames = GetColumnNames();
            var sb = new StringBuilder("INSERT INTO fortune_bag VALUES (");
            for (int i = 0; i < cols.Length; i++)
            {
                if (i > 0) sb.Append(",");
                if (i < colNames.Length && colNames[i] == "note")
                    sb.Append("'" + cols[i].Replace("'", "''") + "'");
                else
                    sb.Append(cols[i]);
            }
            sb.Append(");");
            return sb.ToString();
        }

        void SelectBag(int id)
        {
            for (int i = 0; i < _boxList.Items.Count; i++)
            {
                if ((int)_boxList.Items[i] == id)
                {
                    _boxList.SelectedIndex = i;
                    return;
                }
            }
        }

        string ShowInputDialog(string text, string caption)
        {
            var prompt = new Form
            {
                Width = 300, Height = 140,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false, MinimizeBox = false
            };
            var label = new Label { Left = 12, Top = 12, Text = text, AutoSize = true };
            var textBox = new TextBox { Left = 12, Top = 36, Width = 260 };
            var ok = new Button { Text = "OK", Left = 120, Top = 66, Width = 70, DialogResult = DialogResult.OK };
            var cancel = new Button { Text = "Cancel", Left = 200, Top = 66, Width = 70, DialogResult = DialogResult.Cancel };
            prompt.Controls.AddRange(new Control[] { label, textBox, ok, cancel });
            prompt.AcceptButton = ok;
            prompt.CancelButton = cancel;
            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text.Trim() : null;
        }
    }
}
