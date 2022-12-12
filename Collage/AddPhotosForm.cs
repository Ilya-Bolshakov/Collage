using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Collage
{
    public partial class AddPhotosForm : Form
    {
        private readonly List<Bitmap> bitmaps;
        Bitmap current;
        public AddPhotosForm(List<Bitmap> bitmaps)
        {
            InitializeComponent();
            this.bitmaps = bitmaps;
            if (bitmaps.Count > 0)
            {
                current = bitmaps[0];
                pictureBoxCurrent.Image = current;
            }
            UpdateControls(0);
            
        }

        public List<Bitmap> Bitmaps
        {
            get => bitmaps;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var img = new Bitmap(openFileDialog.FileName);
                bitmaps.Add(img);
                current = img;
                UpdatePhoto();
                if (bitmaps.Count == 6)
                {
                    buttonAdd.Enabled = false;
                    buttonAddFromCamera.Enabled = false;
                }
                UpdateControls(bitmaps.Count - 1);
                buttonDelete.Enabled = true;
            }
        }

        private void UpdatePhoto()
        {
            pictureBoxCurrent.Image = current;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            var index = bitmaps.IndexOf(current);
            bitmaps.Remove(current);
            if (index == bitmaps.Count)
            {
                if (index == 0)
                {
                    current = null;
                }
                else
                {
                    current = bitmaps[--index];
                }
                
            }
            else
            {
                current = bitmaps[index];
            }
            UpdateControls(index);
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            var index = bitmaps.IndexOf(current);
            current = bitmaps[++index];
            UpdateControls(index);
            
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            var index = bitmaps.IndexOf(current);
            current = bitmaps[--index];
            UpdateControls(index);
        }

        private void UpdateControls(int index)
        {
            if (index == 0)
            {
                buttonPrev.Enabled = false;
            }
            if (index > 0)
            {
                buttonPrev.Enabled = true;
            }
            if (bitmaps.Count == index + 1)
            {
                buttonNext.Enabled = false;
            }
            else
            {
                buttonNext.Enabled = true;
            }
            if (bitmaps.Count == 0)
            {
                buttonNext.Enabled = false;
                buttonDelete.Enabled = false;
            }

            pictureBoxCurrent.Image = current;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonAddFromCamera_Click(object sender, EventArgs e)
        {
            Capture capture = new Capture(); 
            Bitmap img = capture.QueryFrame().Bitmap;
            

            bitmaps.Add(img);
            current = img;
            UpdatePhoto();
            if (bitmaps.Count == 6)
            {
                buttonAdd.Enabled = false;
                buttonAddFromCamera.Enabled = false;
            }
            UpdateControls(bitmaps.Count - 1);
            buttonDelete.Enabled = true;
            capture.Dispose();
        }
    }
}
