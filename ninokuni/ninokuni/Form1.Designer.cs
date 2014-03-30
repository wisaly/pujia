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
            this.label4 = new System.Windows.Forms.Label();
            this.editTotWidth = new System.Windows.Forms.NumericUpDown();
            this.editTotHeight = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.editCharHeight = new System.Windows.Forms.NumericUpDown();
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
            this.label9 = new System.Windows.Forms.Label();
            this.editInputRef = new System.Windows.Forms.TextBox();
            this.btnSelInputRef = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.lblCharCountRef = new System.Windows.Forms.Label();
            this.openInputRef = new System.Windows.Forms.OpenFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.editExample = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radRenderGdi = new System.Windows.Forms.RadioButton();
            this.radRenderGdip = new System.Windows.Forms.RadioButton();
            this.comTextRenderOption = new System.Windows.Forms.ComboBox();
            this.lblTextOption = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.editTotWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editTotHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCharHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturePreview)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "图片宽度：";
            // 
            // editTotWidth
            // 
            this.editTotWidth.Location = new System.Drawing.Point(95, 104);
            this.editTotWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.editTotWidth.Name = "editTotWidth";
            this.editTotWidth.Size = new System.Drawing.Size(72, 21);
            this.editTotWidth.TabIndex = 7;
            // 
            // editTotHeight
            // 
            this.editTotHeight.Location = new System.Drawing.Point(258, 104);
            this.editTotHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.editTotHeight.Name = "editTotHeight";
            this.editTotHeight.Size = new System.Drawing.Size(72, 21);
            this.editTotHeight.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(187, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "图片高度：";
            // 
            // editCharHeight
            // 
            this.editCharHeight.Location = new System.Drawing.Point(82, 20);
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
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 8;
            this.label7.Text = "字符高度：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 49);
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
            this.lblFont.Location = new System.Drawing.Point(57, 49);
            this.lblFont.Name = "lblFont";
            this.lblFont.Size = new System.Drawing.Size(175, 38);
            this.lblFont.TabIndex = 12;
            this.lblFont.Text = "?";
            // 
            // picturePreview
            // 
            this.picturePreview.Location = new System.Drawing.Point(6, 120);
            this.picturePreview.Name = "picturePreview";
            this.picturePreview.Size = new System.Drawing.Size(307, 201);
            this.picturePreview.TabIndex = 13;
            this.picturePreview.TabStop = false;
            // 
            // btnRefreshPreview
            // 
            this.btnRefreshPreview.Location = new System.Drawing.Point(11, 524);
            this.btnRefreshPreview.Name = "btnRefreshPreview";
            this.btnRefreshPreview.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshPreview.TabIndex = 14;
            this.btnRefreshPreview.Text = "刷新预览";
            this.btnRefreshPreview.UseVisualStyleBackColor = true;
            this.btnRefreshPreview.Click += new System.EventHandler(this.btnRefreshPreview_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(255, 524);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "保存字库";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.editExample);
            this.groupBox1.Controls.Add(this.picturePreview);
            this.groupBox1.Controls.Add(this.editCharHeight);
            this.groupBox1.Controls.Add(this.lblFont);
            this.groupBox1.Controls.Add(this.btnSelFont);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Location = new System.Drawing.Point(13, 191);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(319, 327);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "字体";
            // 
            // saveFolderBrowser
            // 
            this.saveFolderBrowser.Description = "选择文件保存目录";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(24, 60);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "参考文件：";
            // 
            // editInputRef
            // 
            this.editInputRef.Location = new System.Drawing.Point(97, 57);
            this.editInputRef.Name = "editInputRef";
            this.editInputRef.Size = new System.Drawing.Size(153, 21);
            this.editInputRef.TabIndex = 1;
            // 
            // btnSelInputRef
            // 
            this.btnSelInputRef.Location = new System.Drawing.Point(256, 57);
            this.btnSelInputRef.Name = "btnSelInputRef";
            this.btnSelInputRef.Size = new System.Drawing.Size(75, 23);
            this.btnSelInputRef.TabIndex = 2;
            this.btnSelInputRef.Text = "选择文件";
            this.btnSelInputRef.UseVisualStyleBackColor = true;
            this.btnSelInputRef.Click += new System.EventHandler(this.btnSelInputRef_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(24, 86);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 3;
            this.label10.Text = "文字数量：";
            // 
            // lblCharCountRef
            // 
            this.lblCharCountRef.AutoSize = true;
            this.lblCharCountRef.Location = new System.Drawing.Point(95, 86);
            this.lblCharCountRef.Name = "lblCharCountRef";
            this.lblCharCountRef.Size = new System.Drawing.Size(11, 12);
            this.lblCharCountRef.TabIndex = 4;
            this.lblCharCountRef.Text = "0";
            // 
            // openInputRef
            // 
            this.openInputRef.DefaultExt = "cfg.bin";
            this.openInputRef.FileName = "ft_cap_rodnt36p.cfg.bin";
            this.openInputRef.Filter = "字库文件|*.cfg.bin";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "示例文字：";
            // 
            // editExample
            // 
            this.editExample.Location = new System.Drawing.Point(70, 93);
            this.editExample.Name = "editExample";
            this.editExample.Size = new System.Drawing.Size(243, 21);
            this.editExample.TabIndex = 14;
            this.editExample.Text = "示例文字";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblTextOption);
            this.groupBox2.Controls.Add(this.comTextRenderOption);
            this.groupBox2.Controls.Add(this.radRenderGdip);
            this.groupBox2.Controls.Add(this.radRenderGdi);
            this.groupBox2.Location = new System.Drawing.Point(13, 135);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(319, 50);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "渲染方式";
            // 
            // radRenderGdi
            // 
            this.radRenderGdi.AutoSize = true;
            this.radRenderGdi.Checked = true;
            this.radRenderGdi.Location = new System.Drawing.Point(13, 21);
            this.radRenderGdi.Name = "radRenderGdi";
            this.radRenderGdi.Size = new System.Drawing.Size(41, 16);
            this.radRenderGdi.TabIndex = 0;
            this.radRenderGdi.TabStop = true;
            this.radRenderGdi.Text = "gdi";
            this.radRenderGdi.UseVisualStyleBackColor = true;
            this.radRenderGdi.CheckedChanged += new System.EventHandler(this.radRenderGdi_CheckedChanged);
            // 
            // radRenderGdip
            // 
            this.radRenderGdip.AutoSize = true;
            this.radRenderGdip.Location = new System.Drawing.Point(60, 21);
            this.radRenderGdip.Name = "radRenderGdip";
            this.radRenderGdip.Size = new System.Drawing.Size(47, 16);
            this.radRenderGdip.TabIndex = 0;
            this.radRenderGdip.Text = "gdi+";
            this.radRenderGdip.UseVisualStyleBackColor = true;
            this.radRenderGdip.CheckedChanged += new System.EventHandler(this.radRenderGdip_CheckedChanged);
            // 
            // comTextRenderOption
            // 
            this.comTextRenderOption.Enabled = false;
            this.comTextRenderOption.FormattingEnabled = true;
            this.comTextRenderOption.Items.AddRange(new object[] {
            "SystemDefault",
            "SingleBitPerPixelGridFit",
            "SingleBitPerPixel",
            "AntiAliasGridFit",
            "AntiAlias",
            "ClearTypeGridFit"});
            this.comTextRenderOption.Location = new System.Drawing.Point(190, 20);
            this.comTextRenderOption.Name = "comTextRenderOption";
            this.comTextRenderOption.Size = new System.Drawing.Size(121, 20);
            this.comTextRenderOption.TabIndex = 1;
            this.comTextRenderOption.Text = "AntiAlias";
            this.comTextRenderOption.SelectedIndexChanged += new System.EventHandler(this.comTextRenderOption_SelectedIndexChanged);
            // 
            // lblTextOption
            // 
            this.lblTextOption.AutoSize = true;
            this.lblTextOption.Enabled = false;
            this.lblTextOption.Location = new System.Drawing.Point(119, 23);
            this.lblTextOption.Name = "lblTextOption";
            this.lblTextOption.Size = new System.Drawing.Size(65, 12);
            this.lblTextOption.TabIndex = 2;
            this.lblTextOption.Text = "文字选项：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 559);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnRefreshPreview);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.editTotHeight);
            this.Controls.Add(this.editTotWidth);
            this.Controls.Add(this.lblCharCountRef);
            this.Controls.Add(this.lblCharCount);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnSelInputRef);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.editInputRef);
            this.Controls.Add(this.btnSelInput);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.editInput);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "二之国 字库文件生成器";
            ((System.ComponentModel.ISupportInitialize)(this.editTotWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editTotHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editCharHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturePreview)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox editInput;
        private System.Windows.Forms.Button btnSelInput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCharCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown editTotWidth;
        private System.Windows.Forms.NumericUpDown editTotHeight;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown editCharHeight;
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
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox editInputRef;
        private System.Windows.Forms.Button btnSelInputRef;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblCharCountRef;
        private System.Windows.Forms.OpenFileDialog openInputRef;
        private System.Windows.Forms.TextBox editExample;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radRenderGdip;
        private System.Windows.Forms.RadioButton radRenderGdi;
        private System.Windows.Forms.ComboBox comTextRenderOption;
        private System.Windows.Forms.Label lblTextOption;
    }
}

