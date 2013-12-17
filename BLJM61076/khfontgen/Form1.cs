using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace khfontgen
{
    public partial class Form1 : Form
    {
        Font _fontSel = null;
        List<UInt16> _charset = null;
        public Form1()
        {
            InitializeComponent();
            _fontSel = this.Font;
        }

        private void btnSelInput_Click(object sender, EventArgs e)
        {
            if(openInputFont.ShowDialog() == DialogResult.OK)
            {
                editInput.Text = openInputFont.FileName;

                try
                {
                    FontGen.GetCharset(openInputFont.FileName, out _charset);
                    lblCharCount.Text = _charset.Count.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnSelFont_Click(object sender, EventArgs e)
        {
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                _fontSel = fontDialog.Font;
                lblFont.Text = _fontSel.FontFamily.Name + ","
                    + _fontSel.Size;

                btnRefreshPreview_Click(null, null);
            }
        }

        private void btnRefreshPreview_Click(object sender, EventArgs e)
        {
            Bitmap bmp = null;
            List<UInt16> chars = new List<UInt16>(){
                '示',
                '示',
                '示',
                '　',
                '例',
                '例',
                '例',
                '　',
                '文',
                '文',
                '文',
                '　',
                '字',
                '字',
                '字',
                '　'};
            if(FontGen.Generate(
                chars,(int)editOffsetX.Value,
                (int)editCharWidth.Value * 2 + 1, (int)editCharHeight.Value * 2 + 1,
                (int)editCharWidth.Value, (int)editCharHeight.Value,
                _fontSel,true,out bmp,null))
            {
                picturePreview.Image = bmp;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_charset == null || _charset.Count == 0)
            {
                MessageBox.Show("没有选择字符集");
                return;
            }

            if (saveFolderBrowser.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = null;
                FontGen.Generate(_charset, (int)editOffsetX.Value,
                    (int)editTotWidth.Value, (int)editTotHeight.Value,
                    (int)editCharWidth.Value, (int)editCharHeight.Value,
                    _fontSel, false, out bmp, saveFolderBrowser.SelectedPath);
                MessageBox.Show("已保存");
            }
        }
    }
}
