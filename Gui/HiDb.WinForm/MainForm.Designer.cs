namespace HiDb.WinForm
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            menuStrip_Main = new MenuStrip();
            打开ToolStripMenuItem = new ToolStripMenuItem();
            数据库连接ToolStripMenuItem = new ToolStripMenuItem();
            关于ToolStripMenuItem = new ToolStripMenuItem();
            splitContainer1 = new SplitContainer();
            panel_Db = new Panel();
            treeView_Db = new TreeView();
            panel1 = new Panel();
            toolStrip_Menu = new ToolStrip();
            toolStripComboBox1 = new ToolStripComboBox();
            toolStripButton1 = new ToolStripButton();
            splitContainer2 = new SplitContainer();
            panel4 = new Panel();
            richTextBox_Search = new RichTextBox();
            panel2 = new Panel();
            toolStrip_Search = new ToolStrip();
            toolStripButton2 = new ToolStripButton();
            toolStripButton3 = new ToolStripButton();
            toolStripButton4 = new ToolStripButton();
            panel6 = new Panel();
            dataGridView_Result = new DataGridView();
            panel5 = new Panel();
            menuStrip_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel_Db.SuspendLayout();
            panel1.SuspendLayout();
            toolStrip_Menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            panel4.SuspendLayout();
            panel2.SuspendLayout();
            toolStrip_Search.SuspendLayout();
            panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView_Result).BeginInit();
            SuspendLayout();
            // 
            // menuStrip_Main
            // 
            menuStrip_Main.ImageScalingSize = new Size(20, 20);
            menuStrip_Main.Items.AddRange(new ToolStripItem[] { 打开ToolStripMenuItem, 关于ToolStripMenuItem });
            menuStrip_Main.Location = new Point(0, 0);
            menuStrip_Main.Name = "menuStrip_Main";
            menuStrip_Main.Size = new Size(846, 28);
            menuStrip_Main.TabIndex = 0;
            menuStrip_Main.Text = "menuStrip1";
            // 
            // 打开ToolStripMenuItem
            // 
            打开ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 数据库连接ToolStripMenuItem });
            打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            打开ToolStripMenuItem.Size = new Size(53, 24);
            打开ToolStripMenuItem.Text = "打开";
            // 
            // 数据库连接ToolStripMenuItem
            // 
            数据库连接ToolStripMenuItem.Name = "数据库连接ToolStripMenuItem";
            数据库连接ToolStripMenuItem.Size = new Size(167, 26);
            数据库连接ToolStripMenuItem.Text = "数据库连接";
            // 
            // 关于ToolStripMenuItem
            // 
            关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            关于ToolStripMenuItem.Size = new Size(53, 24);
            关于ToolStripMenuItem.Text = "关于";
            关于ToolStripMenuItem.Click += 关于ToolStripMenuItem_Click;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 28);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(panel_Db);
            splitContainer1.Panel1.Controls.Add(panel1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new Size(846, 422);
            splitContainer1.SplitterDistance = 281;
            splitContainer1.TabIndex = 1;
            // 
            // panel_Db
            // 
            panel_Db.Controls.Add(treeView_Db);
            panel_Db.Dock = DockStyle.Fill;
            panel_Db.Location = new Point(0, 34);
            panel_Db.Name = "panel_Db";
            panel_Db.Size = new Size(281, 388);
            panel_Db.TabIndex = 1;
            // 
            // treeView_Db
            // 
            treeView_Db.Dock = DockStyle.Fill;
            treeView_Db.Location = new Point(0, 0);
            treeView_Db.Name = "treeView_Db";
            treeView_Db.Size = new Size(281, 388);
            treeView_Db.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(toolStrip_Menu);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(281, 34);
            panel1.TabIndex = 0;
            // 
            // toolStrip_Menu
            // 
            toolStrip_Menu.ImageScalingSize = new Size(20, 20);
            toolStrip_Menu.Items.AddRange(new ToolStripItem[] { toolStripComboBox1, toolStripButton1 });
            toolStrip_Menu.Location = new Point(0, 0);
            toolStrip_Menu.Name = "toolStrip_Menu";
            toolStrip_Menu.Size = new Size(281, 28);
            toolStrip_Menu.TabIndex = 0;
            toolStrip_Menu.Text = "toolStrip1";
            // 
            // toolStripComboBox1
            // 
            toolStripComboBox1.Name = "toolStripComboBox1";
            toolStripComboBox1.Size = new Size(121, 28);
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton1.Image = (Image)resources.GetObject("toolStripButton1.Image");
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(29, 25);
            toolStripButton1.Text = "toolStripButton1";
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(panel4);
            splitContainer2.Panel1.Controls.Add(panel2);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(panel6);
            splitContainer2.Panel2.Controls.Add(panel5);
            splitContainer2.Size = new Size(561, 422);
            splitContainer2.SplitterDistance = 175;
            splitContainer2.TabIndex = 0;
            // 
            // panel4
            // 
            panel4.Controls.Add(richTextBox_Search);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(0, 34);
            panel4.Name = "panel4";
            panel4.Size = new Size(561, 141);
            panel4.TabIndex = 1;
            // 
            // richTextBox_Search
            // 
            richTextBox_Search.BorderStyle = BorderStyle.FixedSingle;
            richTextBox_Search.Dock = DockStyle.Fill;
            richTextBox_Search.Location = new Point(0, 0);
            richTextBox_Search.Name = "richTextBox_Search";
            richTextBox_Search.Size = new Size(561, 141);
            richTextBox_Search.TabIndex = 0;
            richTextBox_Search.Text = "";
            // 
            // panel2
            // 
            panel2.Controls.Add(toolStrip_Search);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(561, 34);
            panel2.TabIndex = 0;
            // 
            // toolStrip_Search
            // 
            toolStrip_Search.ImageScalingSize = new Size(20, 20);
            toolStrip_Search.Items.AddRange(new ToolStripItem[] { toolStripButton2, toolStripButton3, toolStripButton4 });
            toolStrip_Search.Location = new Point(0, 0);
            toolStrip_Search.Name = "toolStrip_Search";
            toolStrip_Search.Size = new Size(561, 27);
            toolStrip_Search.TabIndex = 0;
            toolStrip_Search.Text = "toolStrip2";
            // 
            // toolStripButton2
            // 
            toolStripButton2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton2.Image = (Image)resources.GetObject("toolStripButton2.Image");
            toolStripButton2.ImageTransparentColor = Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new Size(29, 24);
            toolStripButton2.Text = "toolStripButton2";
            // 
            // toolStripButton3
            // 
            toolStripButton3.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton3.Image = (Image)resources.GetObject("toolStripButton3.Image");
            toolStripButton3.ImageTransparentColor = Color.Magenta;
            toolStripButton3.Name = "toolStripButton3";
            toolStripButton3.Size = new Size(29, 24);
            toolStripButton3.Text = "toolStripButton3";
            // 
            // toolStripButton4
            // 
            toolStripButton4.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton4.Image = (Image)resources.GetObject("toolStripButton4.Image");
            toolStripButton4.ImageTransparentColor = Color.Magenta;
            toolStripButton4.Name = "toolStripButton4";
            toolStripButton4.Size = new Size(29, 24);
            toolStripButton4.Text = "toolStripButton4";
            // 
            // panel6
            // 
            panel6.Controls.Add(dataGridView_Result);
            panel6.Dock = DockStyle.Fill;
            panel6.Location = new Point(0, 0);
            panel6.Name = "panel6";
            panel6.Size = new Size(561, 211);
            panel6.TabIndex = 1;
            // 
            // dataGridView_Result
            // 
            dataGridView_Result.BackgroundColor = SystemColors.Control;
            dataGridView_Result.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView_Result.Dock = DockStyle.Fill;
            dataGridView_Result.Location = new Point(0, 0);
            dataGridView_Result.Name = "dataGridView_Result";
            dataGridView_Result.RowHeadersWidth = 51;
            dataGridView_Result.Size = new Size(561, 211);
            dataGridView_Result.TabIndex = 0;
            // 
            // panel5
            // 
            panel5.Dock = DockStyle.Bottom;
            panel5.Location = new Point(0, 211);
            panel5.Name = "panel5";
            panel5.Size = new Size(561, 32);
            panel5.TabIndex = 0;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(846, 450);
            Controls.Add(splitContainer1);
            Controls.Add(menuStrip_Main);
            MainMenuStrip = menuStrip_Main;
            Name = "MainForm";
            Text = "HiDb";
            Load += MainForm_Load;
            menuStrip_Main.ResumeLayout(false);
            menuStrip_Main.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel_Db.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            toolStrip_Menu.ResumeLayout(false);
            toolStrip_Menu.PerformLayout();
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            toolStrip_Search.ResumeLayout(false);
            toolStrip_Search.PerformLayout();
            panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView_Result).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip_Main;
        private ToolStripMenuItem 打开ToolStripMenuItem;
        private SplitContainer splitContainer1;
        private Panel panel_Db;
        private Panel panel1;
        private SplitContainer splitContainer2;
        private Panel panel4;
        private Panel panel2;
        private Panel panel5;
        private TreeView treeView_Db;
        private ToolStrip toolStrip_Menu;
        private ToolStripComboBox toolStripComboBox1;
        private ToolStripButton toolStripButton1;
        private RichTextBox richTextBox_Search;
        private ToolStrip toolStrip_Search;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton3;
        private ToolStripButton toolStripButton4;
        private Panel panel6;
        private DataGridView dataGridView_Result;
        private ToolStripMenuItem 数据库连接ToolStripMenuItem;
        private ToolStripMenuItem 关于ToolStripMenuItem;
    }
}