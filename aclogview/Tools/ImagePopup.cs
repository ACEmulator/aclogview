using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace aclogview {
    public partial class ImagePopup : Form {
        public ImagePopup() {
            InitializeComponent();
        }

        public void setImage(Bitmap image) {
            pictureBox1.Image = image;
        }

        private void menuItem_Save_Click(object sender, EventArgs e) {
			using (SaveFileDialog saveDialog = new SaveFileDialog())
			{
				saveDialog.Filter = "Images|*.png;*.bmp;*.jpg";
				if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					System.Drawing.Imaging.ImageFormat imageFormat;
					switch (Path.GetExtension(saveDialog.FileName))
					{
						case ".bmp":
							imageFormat = System.Drawing.Imaging.ImageFormat.Bmp;
							break;
						case ".jpg":
							imageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;
							break;
						default:
							imageFormat = System.Drawing.Imaging.ImageFormat.Png;
							break;
					}
					pictureBox1.Image.Save(saveDialog.FileName, imageFormat);
				}
			}
        }
    }
}
