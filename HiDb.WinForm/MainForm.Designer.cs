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
            menuStrip1 = new MenuStrip();
            数据源ToolStripMenuItem = new ToolStripMenuItem();
            设置ToolStripMenuItem = new ToolStripMenuItem();
            关于ToolStripMenuItem = new ToolStripMenuItem();
            版本信息ToolStripMenuItem = new ToolStripMenuItem();
            hiDbToolStripMenuItem = new ToolStripMenuItem();
            添加数据源ToolStripMenuItem = new ToolStripMenuItem();
            splitContainerMain = new SplitContainer();
            treeView1 = new TreeView();
            splitContainerBody = new SplitContainer();
            toolStripContainer1 = new ToolStripContainer();
            richTextBox1 = new RichTextBox();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            statusStrip1 = new StatusStrip();
            dataGridView1 = new DataGridView();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainerBody).BeginInit();
            splitContainerBody.Panel1.SuspendLayout();
            splitContainerBody.Panel2.SuspendLayout();
            splitContainerBody.SuspendLayout();
            toolStripContainer1.ContentPanel.SuspendLayout();
            toolStripContainer1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { 数据源ToolStripMenuItem, 设置ToolStripMenuItem, 关于ToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // 数据源ToolStripMenuItem
            // 
            数据源ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 添加数据源ToolStripMenuItem });
            数据源ToolStripMenuItem.Name = "数据源ToolStripMenuItem";
            数据源ToolStripMenuItem.Size = new Size(68, 24);
            数据源ToolStripMenuItem.Text = "数据源";
            // 
            // 设置ToolStripMenuItem
            // 
            设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            设置ToolStripMenuItem.Size = new Size(53, 24);
            设置ToolStripMenuItem.Text = "设置";
            // 
            // 关于ToolStripMenuItem
            // 
            关于ToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { 版本信息ToolStripMenuItem, hiDbToolStripMenuItem });
            关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            关于ToolStripMenuItem.Size = new Size(53, 24);
            关于ToolStripMenuItem.Text = "关于";
            // 
            // 版本信息ToolStripMenuItem
            // 
            版本信息ToolStripMenuItem.Name = "版本信息ToolStripMenuItem";
            版本信息ToolStripMenuItem.Size = new Size(224, 26);
            版本信息ToolStripMenuItem.Text = "版本信息";
            // 
            // hiDbToolStripMenuItem
            // 
            hiDbToolStripMenuItem.Name = "hiDbToolStripMenuItem";
            hiDbToolStripMenuItem.Size = new Size(224, 26);
            hiDbToolStripMenuItem.Text = "HiDb";
            // 
            // 添加数据源ToolStripMenuItem
            // 
            添加数据源ToolStripMenuItem.Name = "添加数据源ToolStripMenuItem";
            添加数据源ToolStripMenuItem.Size = new Size(224, 26);
            添加数据源ToolStripMenuItem.Text = "添加数据源";
            // 
            // splitContainerMain
            // 
            splitContainerMain.Dock = DockStyle.Fill;
            splitContainerMain.Location = new Point(0, 28);
            splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(treeView1);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(splitContainerBody);
            splitContainerMain.Size = new Size(800, 422);
            splitContainerMain.SplitterDistance = 266;
            splitContainerMain.TabIndex = 1;
            // 
            // treeView1
            // 
            treeView1.Dock = DockStyle.Fill;
            treeView1.Location = new Point(0, 0);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(266, 422);
            treeView1.TabIndex = 0;
            // 
            // splitContainerBody
            // 
            splitContainerBody.Dock = DockStyle.Fill;
            splitContainerBody.Location = new Point(0, 0);
            splitContainerBody.Name = "splitContainerBody";
            splitContainerBody.Orientation = Orientation.Horizontal;
            // 
            // splitContainerBody.Panel1
            // 
            splitContainerBody.Panel1.Controls.Add(toolStripContainer1);
            // 
            // splitContainerBody.Panel2
            // 
            splitContainerBody.Panel2.Controls.Add(tabControl1);
            splitContainerBody.Size = new Size(530, 422);
            splitContainerBody.SplitterDistance = 176;
            splitContainerBody.TabIndex = 0;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            toolStripContainer1.ContentPanel.Controls.Add(richTextBox1);
            toolStripContainer1.ContentPanel.Size = new Size(530, 151);
            toolStripContainer1.Dock = DockStyle.Fill;
            toolStripContainer1.Location = new Point(0, 0);
            toolStripContainer1.Name = "toolStripContainer1";
            toolStripContainer1.Size = new Size(530, 176);
            toolStripContainer1.TabIndex = 0;
            toolStripContainer1.Text = "toolStripContainer1";
            // 
            // richTextBox1
            // 
            richTextBox1.Dock = DockStyle.Fill;
            richTextBox1.Location = new Point(0, 0);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(530, 151);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(530, 242);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(dataGridView1);
            tabPage1.Controls.Add(statusStrip1);
            tabPage1.Location = new Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(522, 209);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(242, 92);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Location = new Point(3, 184);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(516, 22);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(516, 181);
            dataGridView1.TabIndex = 1;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(splitContainerMain);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "HiDb";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            splitContainerBody.Panel1.ResumeLayout(false);
            splitContainerBody.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerBody).EndInit();
            splitContainerBody.ResumeLayout(false);
            toolStripContainer1.ContentPanel.ResumeLayout(false);
            toolStripContainer1.ResumeLayout(false);
            toolStripContainer1.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem 数据源ToolStripMenuItem;
        private ToolStripMenuItem 添加数据源ToolStripMenuItem;
        private ToolStripMenuItem 设置ToolStripMenuItem;
        private ToolStripMenuItem 关于ToolStripMenuItem;
        private ToolStripMenuItem 版本信息ToolStripMenuItem;
        private ToolStripMenuItem hiDbToolStripMenuItem;
        private SplitContainer splitContainerMain;
        private TreeView treeView1;
        private SplitContainer splitContainerBody;
        private ToolStripContainer toolStripContainer1;
        private RichTextBox richTextBox1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private DataGridView dataGridView1;
        private StatusStrip statusStrip1;
        private TabPage tabPage2;
    }
}