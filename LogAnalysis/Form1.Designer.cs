namespace LogAnalysis
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.LogData = new System.Windows.Forms.DataGridView();
            this.AnalysisCheck = new System.Windows.Forms.CheckBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.LoadLogfile = new System.Windows.Forms.Button();
            this.Logtree = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.SaveCustom = new System.Windows.Forms.Button();
            this.SetCustom = new System.Windows.Forms.Button();
            this.LoadCustomSet = new System.Windows.Forms.Button();
            this.LoadListButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.LogData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LogData
            // 
            this.LogData.AllowUserToAddRows = false;
            this.LogData.AllowUserToDeleteRows = false;
            this.LogData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LogData.Location = new System.Drawing.Point(3, 32);
            this.LogData.MultiSelect = false;
            this.LogData.Name = "LogData";
            this.LogData.RowHeadersWidth = 24;
            this.LogData.RowTemplate.Height = 23;
            this.LogData.ShowCellErrors = false;
            this.LogData.ShowCellToolTips = false;
            this.LogData.Size = new System.Drawing.Size(824, 571);
            this.LogData.TabIndex = 1;
            this.LogData.VirtualMode = true;
            // 
            // AnalysisCheck
            // 
            this.AnalysisCheck.AutoSize = true;
            this.AnalysisCheck.Location = new System.Drawing.Point(165, 10);
            this.AnalysisCheck.Name = "AnalysisCheck";
            this.AnalysisCheck.Size = new System.Drawing.Size(72, 16);
            this.AnalysisCheck.TabIndex = 4;
            this.AnalysisCheck.Text = "单例分析";
            this.AnalysisCheck.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(81, 3);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(87, 23);
            this.progressBar.TabIndex = 3;
            // 
            // LoadLogfile
            // 
            this.LoadLogfile.Location = new System.Drawing.Point(0, 3);
            this.LoadLogfile.Name = "LoadLogfile";
            this.LoadLogfile.Size = new System.Drawing.Size(75, 23);
            this.LoadLogfile.TabIndex = 2;
            this.LoadLogfile.Text = "载入Log";
            this.LoadLogfile.UseVisualStyleBackColor = true;
            // 
            // Logtree
            // 
            this.Logtree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Logtree.Location = new System.Drawing.Point(0, 32);
            this.Logtree.Name = "Logtree";
            this.Logtree.Size = new System.Drawing.Size(171, 569);
            this.Logtree.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.progressBar);
            this.splitContainer1.Panel1.Controls.Add(this.LoadLogfile);
            this.splitContainer1.Panel1.Controls.Add(this.Logtree);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.SaveCustom);
            this.splitContainer1.Panel2.Controls.Add(this.SetCustom);
            this.splitContainer1.Panel2.Controls.Add(this.LoadCustomSet);
            this.splitContainer1.Panel2.Controls.Add(this.LoadListButton);
            this.splitContainer1.Panel2.Controls.Add(this.SaveButton);
            this.splitContainer1.Panel2.Controls.Add(this.AnalysisCheck);
            this.splitContainer1.Panel2.Controls.Add(this.LogData);
            this.splitContainer1.Size = new System.Drawing.Size(1008, 601);
            this.splitContainer1.SplitterDistance = 174;
            this.splitContainer1.TabIndex = 4;
            // 
            // SaveCustom
            // 
            this.SaveCustom.Location = new System.Drawing.Point(416, 3);
            this.SaveCustom.Name = "SaveCustom";
            this.SaveCustom.Size = new System.Drawing.Size(75, 23);
            this.SaveCustom.TabIndex = 9;
            this.SaveCustom.Text = "保存自定义";
            this.SaveCustom.UseVisualStyleBackColor = true;
            this.SaveCustom.Visible = false;
            // 
            // SetCustom
            // 
            this.SetCustom.Location = new System.Drawing.Point(330, 3);
            this.SetCustom.Name = "SetCustom";
            this.SetCustom.Size = new System.Drawing.Size(75, 23);
            this.SetCustom.TabIndex = 8;
            this.SetCustom.Text = "设定自定义";
            this.SetCustom.UseVisualStyleBackColor = true;
            this.SetCustom.Visible = false;
            // 
            // LoadCustomSet
            // 
            this.LoadCustomSet.Location = new System.Drawing.Point(244, 3);
            this.LoadCustomSet.Name = "LoadCustomSet";
            this.LoadCustomSet.Size = new System.Drawing.Size(75, 23);
            this.LoadCustomSet.TabIndex = 7;
            this.LoadCustomSet.Text = "加载自定义";
            this.LoadCustomSet.UseVisualStyleBackColor = true;
            this.LoadCustomSet.Visible = false;
            // 
            // LoadListButton
            // 
            this.LoadListButton.Location = new System.Drawing.Point(3, 3);
            this.LoadListButton.Name = "LoadListButton";
            this.LoadListButton.Size = new System.Drawing.Size(75, 23);
            this.LoadListButton.TabIndex = 6;
            this.LoadListButton.Text = "载入列表";
            this.LoadListButton.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            this.SaveButton.Enabled = false;
            this.SaveButton.Location = new System.Drawing.Point(84, 3);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 5;
            this.SaveButton.Text = "保存列表";
            this.SaveButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 601);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "Form1";
            this.Text = "日志分析";
            ((System.ComponentModel.ISupportInitialize)(this.LogData)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView LogData;
        private System.Windows.Forms.CheckBox AnalysisCheck;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button LoadLogfile;
        private System.Windows.Forms.TreeView Logtree;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button LoadListButton;
        private System.Windows.Forms.Button SaveCustom;
        private System.Windows.Forms.Button SetCustom;
        private System.Windows.Forms.Button LoadCustomSet;
    }
}

