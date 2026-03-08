namespace FortuneBag
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnLoad = new System.Windows.Forms.ToolStripButton();
            this.btnRes = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnNewBag = new System.Windows.Forms.ToolStripButton();
            this.btnDelBag = new System.Windows.Forms.ToolStripButton();
            this.btnDupBag = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAddRow = new System.Windows.Forms.ToolStripButton();
            this.btnDelRow = new System.Windows.Forms.ToolStripButton();
            this.btnMoveUp = new System.Windows.Forms.ToolStripButton();
            this.btnMoveDown = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this._statusFile = new System.Windows.Forms.ToolStripStatusLabel();
            this._statusSchema = new System.Windows.Forms.ToolStripStatusLabel();
            this._statusRows = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._searchBox = new System.Windows.Forms.TextBox();
            this._boxList = new System.Windows.Forms.ListBox();
            this._grid = new System.Windows.Forms.DataGridView();
            this.gridContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addRowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeRowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.moveUpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            this.gridContextMenu.SuspendLayout();
            this.SuspendLayout();
            //
            // toolStrip1
            //
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLoad,
            this.btnRes,
            this.btnSave,
            this.toolStripSeparator1,
            this.btnNewBag,
            this.btnDelBag,
            this.btnDupBag,
            this.toolStripSeparator2,
            this.btnAddRow,
            this.btnDelRow,
            this.btnMoveUp,
            this.btnMoveDown});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1100, 25);
            this.toolStrip1.TabIndex = 0;
            //
            // btnLoad
            //
            this.btnLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(57, 22);
            this.btnLoad.Text = "Load SQL";
            this.btnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            //
            // btnRes
            //
            this.btnRes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRes.Name = "btnRes";
            this.btnRes.Size = new System.Drawing.Size(93, 22);
            this.btnRes.Text = "Load Resources";
            this.btnRes.Click += new System.EventHandler(this.BtnRes_Click);
            //
            // btnSave
            //
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(35, 22);
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            //
            // toolStripSeparator1
            //
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            //
            // btnNewBag
            //
            this.btnNewBag.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnNewBag.Name = "btnNewBag";
            this.btnNewBag.Size = new System.Drawing.Size(56, 22);
            this.btnNewBag.Text = "New Bag";
            this.btnNewBag.Click += new System.EventHandler(this.BtnNewBag_Click);
            //
            // btnDelBag
            //
            this.btnDelBag.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDelBag.Name = "btnDelBag";
            this.btnDelBag.Size = new System.Drawing.Size(67, 22);
            this.btnDelBag.Text = "Delete Bag";
            this.btnDelBag.Click += new System.EventHandler(this.BtnDelBag_Click);
            //
            // btnDupBag
            //
            this.btnDupBag.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDupBag.Name = "btnDupBag";
            this.btnDupBag.Size = new System.Drawing.Size(83, 22);
            this.btnDupBag.Text = "Duplicate Bag";
            this.btnDupBag.Click += new System.EventHandler(this.BtnDupBag_Click);
            //
            // toolStripSeparator2
            //
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            //
            // btnAddRow
            //
            this.btnAddRow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnAddRow.Name = "btnAddRow";
            this.btnAddRow.Size = new System.Drawing.Size(58, 22);
            this.btnAddRow.Text = "Add Row";
            this.btnAddRow.Click += new System.EventHandler(this.BtnAdd_Click);
            //
            // btnDelRow
            //
            this.btnDelRow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDelRow.Name = "btnDelRow";
            this.btnDelRow.Size = new System.Drawing.Size(80, 22);
            this.btnDelRow.Text = "Remove Row";
            this.btnDelRow.Click += new System.EventHandler(this.BtnDel_Click);
            //
            // btnMoveUp
            //
            this.btnMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(23, 22);
            this.btnMoveUp.Text = "Up";
            this.btnMoveUp.Click += new System.EventHandler(this.BtnMoveUp_Click);
            //
            // btnMoveDown
            //
            this.btnMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(42, 22);
            this.btnMoveDown.Text = "Down";
            this.btnMoveDown.Click += new System.EventHandler(this.BtnMoveDown_Click);
            //
            // statusStrip1
            //
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._statusFile,
            this._statusSchema,
            this._statusRows});
            this.statusStrip1.Location = new System.Drawing.Point(0, 678);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1100, 22);
            this.statusStrip1.TabIndex = 1;
            //
            // _statusFile
            //
            this._statusFile.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this._statusFile.Name = "_statusFile";
            this._statusFile.Size = new System.Drawing.Size(87, 17);
            this._statusFile.Text = "No file loaded";
            //
            // _statusSchema
            //
            this._statusSchema.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this._statusSchema.Name = "_statusSchema";
            this._statusSchema.Size = new System.Drawing.Size(4, 17);
            //
            // _statusRows
            //
            this._statusRows.Name = "_statusRows";
            this._statusRows.Size = new System.Drawing.Size(994, 17);
            this._statusRows.Spring = true;
            this._statusRows.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // splitContainer1
            //
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            //
            // splitContainer1.Panel1
            //
            this.splitContainer1.Panel1.Controls.Add(this._boxList);
            this.splitContainer1.Panel1.Controls.Add(this._searchBox);
            //
            // splitContainer1.Panel2
            //
            this.splitContainer1.Panel2.Controls.Add(this._grid);
            this.splitContainer1.Size = new System.Drawing.Size(1100, 653);
            this.splitContainer1.SplitterDistance = 200;
            this.splitContainer1.TabIndex = 2;
            //
            // _searchBox
            //
            this._searchBox.Dock = System.Windows.Forms.DockStyle.Top;
            this._searchBox.Location = new System.Drawing.Point(0, 0);
            this._searchBox.Name = "_searchBox";
            this._searchBox.Size = new System.Drawing.Size(200, 20);
            this._searchBox.TabIndex = 0;
            this._searchBox.TextChanged += new System.EventHandler(this.SearchBox_TextChanged);
            //
            // _boxList
            //
            this._boxList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._boxList.FormattingEnabled = true;
            this._boxList.IntegralHeight = false;
            this._boxList.Location = new System.Drawing.Point(0, 20);
            this._boxList.Name = "_boxList";
            this._boxList.Size = new System.Drawing.Size(200, 633);
            this._boxList.TabIndex = 1;
            this._boxList.SelectedIndexChanged += new System.EventHandler(this.BoxList_SelectedIndexChanged);
            //
            // _grid
            //
            this._grid.AllowDrop = true;
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            this._grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._grid.ContextMenuStrip = this.gridContextMenu;
            this._grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystrokeOrF2;
            this._grid.Location = new System.Drawing.Point(0, 0);
            this._grid.Name = "_grid";
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._grid.Size = new System.Drawing.Size(896, 653);
            this._grid.TabIndex = 0;
            this._grid.DragDrop += new System.Windows.Forms.DragEventHandler(this.Grid_DragDrop);
            this._grid.DragOver += new System.Windows.Forms.DragEventHandler(this.Grid_DragOver);
            this._grid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseDown);
            this._grid.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Grid_MouseMove);
            //
            // gridContextMenu
            //
            this.gridContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addRowMenuItem,
            this.removeRowMenuItem,
            this.toolStripSeparator3,
            this.moveUpMenuItem,
            this.moveDownMenuItem});
            this.gridContextMenu.Name = "gridContextMenu";
            this.gridContextMenu.Size = new System.Drawing.Size(145, 98);
            //
            // addRowMenuItem
            //
            this.addRowMenuItem.Name = "addRowMenuItem";
            this.addRowMenuItem.Size = new System.Drawing.Size(144, 22);
            this.addRowMenuItem.Text = "Add Row";
            this.addRowMenuItem.Click += new System.EventHandler(this.BtnAdd_Click);
            //
            // removeRowMenuItem
            //
            this.removeRowMenuItem.Name = "removeRowMenuItem";
            this.removeRowMenuItem.Size = new System.Drawing.Size(144, 22);
            this.removeRowMenuItem.Text = "Remove Row";
            this.removeRowMenuItem.Click += new System.EventHandler(this.BtnDel_Click);
            //
            // toolStripSeparator3
            //
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(141, 6);
            //
            // moveUpMenuItem
            //
            this.moveUpMenuItem.Name = "moveUpMenuItem";
            this.moveUpMenuItem.Size = new System.Drawing.Size(144, 22);
            this.moveUpMenuItem.Text = "Move Up";
            this.moveUpMenuItem.Click += new System.EventHandler(this.BtnMoveUp_Click);
            //
            // moveDownMenuItem
            //
            this.moveDownMenuItem.Name = "moveDownMenuItem";
            this.moveDownMenuItem.Size = new System.Drawing.Size(144, 22);
            this.moveDownMenuItem.Text = "Move Down";
            this.moveDownMenuItem.Click += new System.EventHandler(this.BtnMoveDown_Click);
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "FortuneBag Editor";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            this.gridContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnLoad;
        private System.Windows.Forms.ToolStripButton btnRes;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnNewBag;
        private System.Windows.Forms.ToolStripButton btnDelBag;
        private System.Windows.Forms.ToolStripButton btnDupBag;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnAddRow;
        private System.Windows.Forms.ToolStripButton btnDelRow;
        private System.Windows.Forms.ToolStripButton btnMoveUp;
        private System.Windows.Forms.ToolStripButton btnMoveDown;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel _statusFile;
        private System.Windows.Forms.ToolStripStatusLabel _statusSchema;
        private System.Windows.Forms.ToolStripStatusLabel _statusRows;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox _searchBox;
        private System.Windows.Forms.ListBox _boxList;
        private System.Windows.Forms.DataGridView _grid;
        private System.Windows.Forms.ContextMenuStrip gridContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addRowMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeRowMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem moveUpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveDownMenuItem;
    }
}
