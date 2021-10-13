using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Bridge
{
    public partial class GraphicsGallery : MetroFramework.Forms.MetroForm
    {
        private String exPath;

        public GraphicsGallery(String exPath)
        {
            InitializeComponent();
            this.exPath = exPath;
        }

        private void lineGraphicRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            String lineGraphicImageName = "\\Line Graphic.bmp";
            String imagePath = exPath + lineGraphicImageName;
            if (File.Exists(imagePath))
            {
                Bitmap image = new Bitmap(imagePath);
                graphicPictureBox.Size = image.Size;
                graphicPictureBox.Image = image;
                graphicPictureBox.Invalidate();
            } else
            {
                lineGraphicRadioButton.Enabled = false;
            }
        }

        private void columnGraphicRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            String lineGraphicImageName = "\\Column Graphic.bmp"; ;
            String imagePath = exPath + lineGraphicImageName;
            if (File.Exists(imagePath))
            {
                Bitmap image = new Bitmap(imagePath);
                graphicPictureBox.Size = image.Size;
                graphicPictureBox.Image = image;
                graphicPictureBox.Invalidate();
            } else
            {
                columnGraphicRadioButton.Enabled = false;
            }
        }
    }
}
