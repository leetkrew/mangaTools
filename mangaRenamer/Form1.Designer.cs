namespace mangaRenamer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtPathFrom = new System.Windows.Forms.TextBox();
            this.txtPathTo = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnAddQueue = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnBrowseFrom = new System.Windows.Forms.Button();
            this.btnBrowseTo = new System.Windows.Forms.Button();
            this.btnExportPdf = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.btnExploreFrom = new System.Windows.Forms.Button();
            this.btnExploreTo = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtChapterNumberRegExGroup = new System.Windows.Forms.TextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.cboChapterNumberRegEx = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cboChapterTitleRegEx = new System.Windows.Forms.ComboBox();
            this.btnGenerateTitleRegEx = new System.Windows.Forms.Button();
            this.txtChapterTitleRegExGroup = new System.Windows.Forms.TextBox();
            this.chkChapterTitleEnabled = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPathFrom
            // 
            this.txtPathFrom.Location = new System.Drawing.Point(12, 43);
            this.txtPathFrom.Name = "txtPathFrom";
            this.txtPathFrom.Size = new System.Drawing.Size(712, 22);
            this.txtPathFrom.TabIndex = 0;
            // 
            // txtPathTo
            // 
            this.txtPathTo.Location = new System.Drawing.Point(12, 97);
            this.txtPathTo.Name = "txtPathTo";
            this.txtPathTo.Size = new System.Drawing.Size(712, 22);
            this.txtPathTo.TabIndex = 3;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(11, 287);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(962, 430);
            this.dataGridView1.TabIndex = 2;
            // 
            // btnAddQueue
            // 
            this.btnAddQueue.Location = new System.Drawing.Point(11, 258);
            this.btnAddQueue.Name = "btnAddQueue";
            this.btnAddQueue.Size = new System.Drawing.Size(135, 23);
            this.btnAddQueue.TabIndex = 9;
            this.btnAddQueue.Text = "Add to Queue";
            this.btnAddQueue.UseVisualStyleBackColor = true;
            this.btnAddQueue.Click += new System.EventHandler(this.btnAddQueue_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(293, 258);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(135, 23);
            this.btnCopy.TabIndex = 11;
            this.btnCopy.Text = "Copy Files";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(152, 258);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(135, 23);
            this.btnClear.TabIndex = 10;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnBrowseFrom
            // 
            this.btnBrowseFrom.Location = new System.Drawing.Point(730, 43);
            this.btnBrowseFrom.Name = "btnBrowseFrom";
            this.btnBrowseFrom.Size = new System.Drawing.Size(135, 23);
            this.btnBrowseFrom.TabIndex = 1;
            this.btnBrowseFrom.Text = "Browse";
            this.btnBrowseFrom.UseVisualStyleBackColor = true;
            this.btnBrowseFrom.Click += new System.EventHandler(this.btnBrowseFrom_Click);
            // 
            // btnBrowseTo
            // 
            this.btnBrowseTo.Location = new System.Drawing.Point(730, 96);
            this.btnBrowseTo.Name = "btnBrowseTo";
            this.btnBrowseTo.Size = new System.Drawing.Size(135, 23);
            this.btnBrowseTo.TabIndex = 4;
            this.btnBrowseTo.Text = "Browse";
            this.btnBrowseTo.UseVisualStyleBackColor = true;
            this.btnBrowseTo.Click += new System.EventHandler(this.btnBrowseTo_Click);
            // 
            // btnExportPdf
            // 
            this.btnExportPdf.Location = new System.Drawing.Point(434, 258);
            this.btnExportPdf.Name = "btnExportPdf";
            this.btnExportPdf.Size = new System.Drawing.Size(135, 23);
            this.btnExportPdf.TabIndex = 12;
            this.btnExportPdf.Text = "Export as PDF";
            this.btnExportPdf.UseVisualStyleBackColor = true;
            this.btnExportPdf.Click += new System.EventHandler(this.btnExportPdf_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 723);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(961, 23);
            this.progressBar1.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "Source Directory";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 17);
            this.label2.TabIndex = 11;
            this.label2.Text = "Destination Directory";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(744, 749);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(229, 17);
            this.linkLabel1.TabIndex = 12;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "RJ Regalado (www.rjregalado.com)";
            // 
            // btnExploreFrom
            // 
            this.btnExploreFrom.Location = new System.Drawing.Point(871, 43);
            this.btnExploreFrom.Name = "btnExploreFrom";
            this.btnExploreFrom.Size = new System.Drawing.Size(129, 23);
            this.btnExploreFrom.TabIndex = 2;
            this.btnExploreFrom.Text = "Explore";
            this.btnExploreFrom.UseVisualStyleBackColor = true;
            this.btnExploreFrom.Click += new System.EventHandler(this.btnExploreFrom_Click);
            // 
            // btnExploreTo
            // 
            this.btnExploreTo.Location = new System.Drawing.Point(871, 96);
            this.btnExploreTo.Name = "btnExploreTo";
            this.btnExploreTo.Size = new System.Drawing.Size(133, 23);
            this.btnExploreTo.TabIndex = 5;
            this.btnExploreTo.Text = "Explore";
            this.btnExploreTo.UseVisualStyleBackColor = true;
            this.btnExploreTo.Click += new System.EventHandler(this.btnExploreTo_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(727, 130);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 17);
            this.label4.TabIndex = 20;
            this.label4.Text = "ReEx Group";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(239, 17);
            this.label3.TabIndex = 19;
            this.label3.Text = "Chapter Number Regular Expression";
            // 
            // txtChapterNumberRegExGroup
            // 
            this.txtChapterNumberRegExGroup.Location = new System.Drawing.Point(730, 150);
            this.txtChapterNumberRegExGroup.Name = "txtChapterNumberRegExGroup";
            this.txtChapterNumberRegExGroup.Size = new System.Drawing.Size(135, 22);
            this.txtChapterNumberRegExGroup.TabIndex = 7;
            this.txtChapterNumberRegExGroup.Text = "0";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(871, 149);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(133, 23);
            this.btnGenerate.TabIndex = 8;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // cboChapterNumberRegEx
            // 
            this.cboChapterNumberRegEx.FormattingEnabled = true;
            this.cboChapterNumberRegEx.Location = new System.Drawing.Point(11, 150);
            this.cboChapterNumberRegEx.Name = "cboChapterNumberRegEx";
            this.cboChapterNumberRegEx.Size = new System.Drawing.Size(713, 24);
            this.cboChapterNumberRegEx.TabIndex = 6;
            this.cboChapterNumberRegEx.SelectedIndexChanged += new System.EventHandler(this.cboRegEx_SelectedIndexChanged);
            this.cboChapterNumberRegEx.TextChanged += new System.EventHandler(this.cboRegEx_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 184);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(226, 17);
            this.label5.TabIndex = 21;
            this.label5.Text = "Chapter\'s Title Regular Expression";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(727, 184);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 17);
            this.label6.TabIndex = 22;
            this.label6.Text = "RegEx Group";
            // 
            // cboChapterTitleRegEx
            // 
            this.cboChapterTitleRegEx.FormattingEnabled = true;
            this.cboChapterTitleRegEx.Location = new System.Drawing.Point(32, 203);
            this.cboChapterTitleRegEx.Name = "cboChapterTitleRegEx";
            this.cboChapterTitleRegEx.Size = new System.Drawing.Size(692, 24);
            this.cboChapterTitleRegEx.TabIndex = 23;
            this.cboChapterTitleRegEx.SelectedIndexChanged += new System.EventHandler(this.cboChapterTitleRegEx_SelectedIndexChanged);
            // 
            // btnGenerateTitleRegEx
            // 
            this.btnGenerateTitleRegEx.Location = new System.Drawing.Point(871, 204);
            this.btnGenerateTitleRegEx.Name = "btnGenerateTitleRegEx";
            this.btnGenerateTitleRegEx.Size = new System.Drawing.Size(129, 23);
            this.btnGenerateTitleRegEx.TabIndex = 24;
            this.btnGenerateTitleRegEx.Text = "Generate";
            this.btnGenerateTitleRegEx.UseVisualStyleBackColor = true;
            this.btnGenerateTitleRegEx.Click += new System.EventHandler(this.btnGenerateTitleRegEx_Click);
            // 
            // txtChapterTitleRegExGroup
            // 
            this.txtChapterTitleRegExGroup.Location = new System.Drawing.Point(730, 204);
            this.txtChapterTitleRegExGroup.Name = "txtChapterTitleRegExGroup";
            this.txtChapterTitleRegExGroup.Size = new System.Drawing.Size(135, 22);
            this.txtChapterTitleRegExGroup.TabIndex = 25;
            this.txtChapterTitleRegExGroup.Text = "0";
            // 
            // chkChapterTitleEnabled
            // 
            this.chkChapterTitleEnabled.AutoSize = true;
            this.chkChapterTitleEnabled.Checked = true;
            this.chkChapterTitleEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkChapterTitleEnabled.Location = new System.Drawing.Point(12, 207);
            this.chkChapterTitleEnabled.Name = "chkChapterTitleEnabled";
            this.chkChapterTitleEnabled.Size = new System.Drawing.Size(18, 17);
            this.chkChapterTitleEnabled.TabIndex = 26;
            this.chkChapterTitleEnabled.UseVisualStyleBackColor = true;
            this.chkChapterTitleEnabled.CheckedChanged += new System.EventHandler(this.chkChapterTitleEnabled_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 775);
            this.Controls.Add(this.chkChapterTitleEnabled);
            this.Controls.Add(this.txtChapterTitleRegExGroup);
            this.Controls.Add(this.btnGenerateTitleRegEx);
            this.Controls.Add(this.cboChapterTitleRegEx);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cboChapterNumberRegEx);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtChapterNumberRegExGroup);
            this.Controls.Add(this.btnExploreTo);
            this.Controls.Add(this.btnExploreFrom);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnExportPdf);
            this.Controls.Add(this.btnBrowseTo);
            this.Controls.Add(this.btnBrowseFrom);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnAddQueue);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.txtPathTo);
            this.Controls.Add(this.txtPathFrom);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manga Tools by RJ Regalado";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPathFrom;
        private System.Windows.Forms.TextBox txtPathTo;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnAddQueue;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnBrowseFrom;
        private System.Windows.Forms.Button btnBrowseTo;
        private System.Windows.Forms.Button btnExportPdf;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button btnExploreFrom;
        private System.Windows.Forms.Button btnExploreTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtChapterNumberRegExGroup;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.ComboBox cboChapterNumberRegEx;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboChapterTitleRegEx;
        private System.Windows.Forms.Button btnGenerateTitleRegEx;
        private System.Windows.Forms.TextBox txtChapterTitleRegExGroup;
        private System.Windows.Forms.CheckBox chkChapterTitleEnabled;
    }
}

