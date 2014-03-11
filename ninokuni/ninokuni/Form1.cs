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
        List<char> _charset = null;
        int _width = 0;
        int _height = 0;
        int _charHeight = 0;
        int _charCount = 0;
        string _refFileName = "";

        public Form1()
        {
            InitializeComponent();
            _fontSel = this.Font;
        }

        private void btnSelInput_Click(object sender, EventArgs e)
        {
            if (openInputFont.ShowDialog() == DialogResult.OK)
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

        private void btnSelInputRef_Click(object sender, EventArgs e)
        {
            if (openInputRef.ShowDialog() == DialogResult.OK)
            {
                _refFileName = openInputRef.SafeFileName;

                editInputRef.Text = openInputRef.FileName;
                try
                {
                    FontGen.GetRefInfo(openInputRef.FileName, out _width, out _height, out _charHeight, out _charCount);
                    lblCharCountRef.Text = _charCount.ToString();
                    editTotWidth.Value = _width;
                    editTotHeight.Value = _height;
                    editCharHeight.Value = _charHeight;
                    _fontSel = new Font("宋体", _charHeight, GraphicsUnit.Pixel);
                    fontDialog.Font = _fontSel;
                    lblFont.Text = _fontSel.ToString();
                    btnRefreshPreview_Click(null, null);
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
                //lblFont.Text = _fontSel.FontFamily.Name + "," + _fontSel.Size;

                lblFont.Text = _fontSel.ToString();
                btnRefreshPreview_Click(null, null);
            }
        }

        private void btnRefreshPreview_Click(object sender, EventArgs e)
        {
            Bitmap bmp = null;
            List<char> chars = new List<char>();
            for (int i = 0; i < editExample.Text.Length; i++)
            {
                chars.Add(editExample.Text[i]);
            }
            if (FontGen.Generate(
                chars, radRenderGdi.Checked,getTextRenderingHint(),
                (int)editCharHeight.Value * 2 + 10, (int)editCharHeight.Value * 2 + 10,
                (int)editCharHeight.Value, (int)editCharHeight.Value,
                _fontSel, true, out bmp, null, null))
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
                FontGen.Generate(_charset, radRenderGdi.Checked,getTextRenderingHint(),
                    (int)editTotWidth.Value, (int)editTotHeight.Value,
                    0, (int)editCharHeight.Value,
                    _fontSel, false, out bmp,
                    saveFolderBrowser.SelectedPath, _refFileName);
                MessageBox.Show("已保存");
            }
        }

        private void radRenderGdi_CheckedChanged(object sender, EventArgs e)
        {
            lblTextOption.Enabled = false;
            comTextRenderOption.Enabled = false;
        }

        private void radRenderGdip_CheckedChanged(object sender, EventArgs e)
        {
            lblTextOption.Enabled = true;
            comTextRenderOption.Enabled = true;
        }

        private System.Drawing.Text.TextRenderingHint getTextRenderingHint()
        {
            if (comTextRenderOption.Text == "SingleBitPerPixelGridFit")
            {
                return System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
            }
            else if (comTextRenderOption.Text == "SingleBitPerPixel")
            {
                return System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
            }
            else if (comTextRenderOption.Text == "AntiAliasGridFit")
            {
                return System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            }
            else if (comTextRenderOption.Text == "AntiAlias")
            {
                return System.Drawing.Text.TextRenderingHint.AntiAlias;
            }
            else if (comTextRenderOption.Text == "ClearTypeGridFit")
            {
                return System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            }
            else
            {
                return System.Drawing.Text.TextRenderingHint.SystemDefault;
            }
        }

        private void comTextRenderOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRefreshPreview_Click(null, null);
        }
    }
}
