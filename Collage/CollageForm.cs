using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.UI;
using System.IO;

namespace Collage
{
    public partial class CollageForm : Form
    {
        List<Bitmap> bitmaps;
        Image backgroundImg;
        public CollageForm()
        {
            InitializeComponent();
            bitmaps = new List<Bitmap>();
            backgroundImg = new Bitmap(pictureBox.PreferredSize.Width, pictureBox.PreferredSize.Height);
            pictureBox.Image = backgroundImg;
        }

        private void btn_Background_Click(object sender, EventArgs e)
        {
            var dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                backgroundImg = new Bitmap(openFileDialog.FileName);
                pictureBox.Image = backgroundImg;
            }
            DrawCollage();
        }

        private void btn_Default_Click(object sender, EventArgs e)
        {
            Color color = Color.FromArgb(166, 229, 255);
            using (SolidBrush solidBrush = new SolidBrush(color))
            {
                
                Graphics graphics = Graphics.FromImage(pictureBox.Image);                
                graphics.FillRectangle(solidBrush, new Rectangle(0, 0, pictureBox.PreferredSize.Width, pictureBox.PreferredSize.Height));
                pictureBox.Refresh();
                backgroundImg = pictureBox.Image;
                graphics.Dispose();
            }
            DrawCollage();

        }

        private void buttonSelectImages_Click(object sender, EventArgs e)
        {
            var addForm = new AddPhotosForm(bitmaps);
            addForm.ShowDialog();
            bitmaps = addForm.Bitmaps;
            DrawCollage();
        }

        private void DrawCollage()
        {
            pictureBox.Image = new Bitmap(backgroundImg);
            int countDownImg = bitmaps.Count / 2;
            int countUpImg = bitmaps.Count - countDownImg;
            double margin = pictureBox.PreferredSize.Width * 0.05;

            int size = (int)((pictureBox.PreferredSize.Width - margin * (countUpImg + 1)) / countUpImg);

            for (int i = 0; i < countUpImg; i++)
            {
                Graphics graphics = Graphics.FromImage(pictureBox.Image);
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.DrawImage(bitmaps[i], (float)(margin * (i + 1) + size * i), 50, size, pictureBox.PreferredSize.Height / 2 - 100);
                pictureBox.Refresh();
            }

            size = (int)((pictureBox.PreferredSize.Width - margin * (countDownImg + 1)) / countDownImg);

            for (int i = countUpImg; i < bitmaps.Count; i++)
            {
                Graphics graphics = Graphics.FromImage(pictureBox.Image);
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.DrawImage(bitmaps[i], (float)(margin * (i - countUpImg + 1) + size * (i - countUpImg)), 25 + pictureBox.PreferredSize.Height / 2, size, pictureBox.PreferredSize.Height / 2 - 100);
                pictureBox.Refresh();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            var dialogResult = saveFileDialog.ShowDialog();
            ImageFormat format = ImageFormat.Png;
            if (dialogResult == DialogResult.OK)
            {
                pictureBox.Image.Save(saveFileDialog.FileName, format);
            }
            DrawCollage();
        }
    }
}
