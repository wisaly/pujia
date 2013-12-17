namespace khfontgen
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.editInput = new System.Windows.Forms.TextBox();
            this.btnSelInput = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCharCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.editOffsetX = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.editTotWidth = new System.Windows.Forms.NumericUpDown();
            this.editTotHeight = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.editCharWidth = new System.Windows.Forms.NumericUpDown();
            this.editCharHeight = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.openInputFont = new System.Windows.Forms.OpenFileDialog();
            this.btnSelFont = new System.Windows.Forms.Button();
            this.lblFont = new System.Windows.Forms.Label();
            this.picturePreview = new System.Windows.Forms.PictureBox();
            this.btnRefreshPreview = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.saveFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.editOffsetX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editTotWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editTotHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCharWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCharHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturePreview)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "字库文件：";
            // 
            // editInput
            // 
            this.editInput.Location = new System.Drawing.Point(97, 10);
            this.editInput.Name = "editInput";
            this.editInput.Size = new System.Drawing.Size(153, 21);
            this.editInput.TabIndex = 1;
            // 
            // btnSelInput
            // 
            this.btnSelInput.Location = new System.Drawing.Point(256, 10);
            this.btnSelInput.Name = "btnSelInput";
            this.btnSelInput.Size = new System.Drawing.Size(75, 23);
            this.btnSelInput.TabIndex = 2;
            this.btnSelInput.Text = "选择文件";
            this.btnSelInput.UseVisualStyleBackColor = true;
            this.btnSelInput.Click += new System.EventHandler(this.btnSelInput_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "文字数量：";
            // 
            // lblCharCount
            // 
            this.lblCharCount.AutoSize = true;
            this.lblCharCount.Location = new System.Drawing.Point(95, 39);
            this.lblCharCount.Name = "lblCharCount";
            this.lblCharCount.Size = new System.Drawing.Size(11, 12);
            this.lblCharCount.TabIndex = 4;
            this.lblCharCount.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "字符偏移量X：";
            // 
            // editOffsetX
            // 
            this.editOffsetX.Location = new System.Drawing.Point(91, 14);
            this.editOffsetX.Name = "editOffsetX";
            this.editOffsetX.Size = new System.Drawing.Size(72, 21);
            this.editOffsetX.TabIndex = 7;
            this.editOffsetX.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "图片宽度：";
            // 
            // editTotWidth
            // 
            this.editTotWidth.Location = new System.Drawing.Point(94, 56);
            this.editTotWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.editTotWidth.Name = "editTotWidth";
            this.editTotWidth.Size = new System.Drawing.Size(72, 21);
            this.editTotWidth.TabIndex = 7;
            this.editTotWidth.Value = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            // 
            // editTotHeight
            // 
            this.editTotHeight.Location = new System.Drawing.Point(257, 56);
            this.editTotHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.editTotHeight.Name = "editTotHeight";
            this.editTotHeight.Size = new System.Drawing.Size(72, 21);
            this.editTotHeight.TabIndex = 7;
            this.editTotHeight.Value = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(186, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "图片高度：";
            // 
            // editCharWidth
            // 
            this.editCharWidth.Location = new System.Drawing.Point(91, 39);
            this.editCharWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.editCharWidth.Name = "editCharWidth";
            this.editCharWidth.Size = new System.Drawing.Size(72, 21);
            this.editCharWidth.TabIndex = 7;
            this.editCharWidth.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            // 
            // editCharHeight
            // 
            this.editCharHeight.Location = new System.Drawing.Point(241, 39);
            this.editCharHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.editCharHeight.Name = "editCharHeight";
            this.editCharHeight.Size = new System.Drawing.Size(72, 21);
            this.editCharHeight.TabIndex = 7;
            this.editCharHeight.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "字符宽度：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(173, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 8;
            this.label7.Text = "字符高度：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(42, 69);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 9;
            this.label8.Text = "字体：";
            // 
            // fontDialog
            // 
            this.fontDialog.AllowVerticalFonts = false;
            this.fontDialog.ShowEffects = false;
            // 
            // openInputFont
            // 
            this.openInputFont.DefaultExt = "txt";
            this.openInputFont.FileName = "font.txt";
            this.openInputFont.Filter = "字库文本文件|*.txt";
            // 
            // btnSelFont
            // 
            this.btnSelFont.Location = new System.Drawing.Point(238, 64);
            this.btnSelFont.Name = "btnSelFont";
            this.btnSelFont.Size = new System.Drawing.Size(75, 23);
            this.btnSelFont.TabIndex = 11;
            this.btnSelFont.Text = "选择字体";
            this.btnSelFont.UseVisualStyleBackColor = true;
            this.btnSelFont.Click += new System.EventHandler(this.btnSelFont_Click);
            // 
            // lblFont
            // 
            this.lblFont.AutoSize = true;
            this.lblFont.Location = new System.Drawing.Point(91, 70);
            this.lblFont.Name = "lblFont";
            this.lblFont.Size = new System.Drawing.Size(11, 12);
            this.lblFont.TabIndex = 12;
            this.lblFont.Text = "?";
            // 
            // picturePreview
            // 
            this.picturePreview.Location = new System.Drawing.Point(6, 95);
            this.picturePreview.Name = "picturePreview";
            this.picturePreview.Size = new System.Drawing.Size(307, 229);
            this.picturePreview.TabIndex = 13;
            this.picturePreview.TabStop = false;
            // 
            // btnRefreshPreview
            // 
            this.btnRefreshPreview.Location = new System.Drawing.Point(12, 419);
            this.btnRefreshPreview.Name = "btnRefreshPreview";
            this.btnRefreshPreview.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshPreview.TabIndex = 14;
            this.btnRefreshPreview.Text = "刷新预览";
            this.btnRefreshPreview.UseVisualStyleBackColor = true;
            this.btnRefreshPreview.Click += new System.EventHandler(this.btnRefreshPreview_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(256, 419);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "保存字库";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.editOffsetX);
            this.groupBox1.Controls.Add(this.editCharWidth);
            this.groupBox1.Controls.Add(this.picturePreview);
            this.groupBox1.Controls.Add(this.editCharHeight);
            this.groupBox1.Controls.Add(this.lblFont);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnSelFont);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Location = new System.Drawing.Point(12, 83);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(319, 330);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "字体";
            // 
            // saveFolderBrowser
            // 
            this.saveFolderBrowser.Description = "选择文件保存目录";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 454);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnRefreshPreview);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.editTotHeight);
            this.Controls.Add(this.editTotWidth);
            this.Controls.Add(this.lblCharCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSelInput);
            this.Controls.Add(this.editInput);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "王国之心HD1.5 字库文件生成器";
            ((System.ComponentModel.ISupportInitialize)(this.editOffsetX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editTotWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editTotHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCharWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCharHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturePreview)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox editInput;
        private System.Windows.Forms.Button btnSelInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCharCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown editOffsetX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown editTotWidth;
        private System.Windows.Forms.NumericUpDown editTotHeight;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown editCharWidth;
        private System.Windows.Forms.NumericUpDown editCharHeight;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.OpenFileDialog openInputFont;
        private System.Windows.Forms.Button btnSelFont;
        private System.Windows.Forms.Label lblFont;
        private System.Windows.Forms.PictureBox picturePreview;
        private System.Windows.Forms.Button btnRefreshPreview;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FolderBrowserDialog saveFolderBrowser;
    }
}

