using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Bridge
{
    public partial class Results :  MetroFramework.Forms.MetroForm 
    {
        public DataGridViewCellMouseEventArgs e;


        public Results(DataGridViewCellMouseEventArgs _eRes)
        {
            InitializeComponent();
            e = _eRes;
            PrintTables();
        }
        System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["Series"];

        private void PrintTables()
        {
            String EXpath = Convert.ToString(((Series)f).SeriesGridJournal.Rows[e.RowIndex].Cells[1].Value);
            string EXfilePath = EXpath + "\\Log.txt";
            if (File.Exists(EXfilePath))
            {
                NameLog.Clear();
                NameLog.Text = EXpath;
                textBoxLog.Clear();
                StreamReader file = new StreamReader(EXfilePath);
                string lines = file.ReadToEnd();
                textBoxLog.Text = lines;
            }

            string[] fileName = System.IO.Directory.GetFiles(EXpath, "*.dat");
            if (fileName.Length != 0)
            {
                if (File.Exists(fileName[0]))
                {
                    string Datname = System.IO.Path.GetFileNameWithoutExtension(@fileName[0]);

                    String OptimPath = EXpath + "\\" + Datname + ".dat";


                    if (File.Exists(OptimPath))
                    {
                        OptimName.Text = OptimPath;
                        TextOptimPath.Clear();
                        TextOptimPath.Lines = File.ReadAllLines(OptimPath);
                    }
                }
            }

            String CONFpath = Convert.ToString(((Series)f).SeriesGridJournal.Rows[e.RowIndex].Cells[3].Value);
            if (CONFpath == "TEMP_GEN")
            {
               
                    ConfName.Clear();
                    ConfName.Text = CONFpath;
                    textBoxConf.Clear();
                    textBoxConf.Text = "Generated configuration not saved";
                
            }
            if (CONFpath.Contains("\\Series\\Saved\\"))
            {
                    if (File.Exists(CONFpath))
                    {
                        ConfName.Clear();
                        ConfName.Text = CONFpath;
                        textBoxConf.Clear();
                        textBoxConf.Lines = File.ReadAllLines(CONFpath);
                    }

            }
            else
            {
                if (File.Exists(CONFpath))
                {
                    ConfName.Clear();
                    ConfName.Text = CONFpath;
                    textBoxConf.Clear();
                    textBoxConf.Lines = File.ReadAllLines(CONFpath);
                }
            }
                

            try
            {
            
            String PicLoc = EXpath + "\\Examin.png";
            if (File.Exists(PicLoc))
            {
                Bitmap image1 = new Bitmap(PicLoc);

                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                pictureBox1.Image = image1;

            }
            else
            {
                trackBar1.Visible = false;
                trackBar2.Visible = false;
                pictureBox1.Visible = false;

                metroLabel1.Left = metroLabel1.Location.X - 300;
                metroLabel2.Left = metroLabel2.Location.X - 300;
                metroLabel3.Left = metroLabel3.Location.X - 300;
                NameLog.Left = NameLog.Location.X - 300;
                ConfName.Left = ConfName.Location.X - 300;
                OptimName.Left = OptimName.Location.X - 300;
                textBoxLog.Left = textBoxLog.Location.X - 300;
                textBoxConf.Left = textBoxConf.Location.X - 300;
                TextOptimPath.Left = TextOptimPath.Location.X - 300;
            }
            }
            catch
            {
                MetroFramework.MetroMessageBox.Show(this, "Изображение повреждено.", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                e.Graphics.DrawImage(pictureBox1.Image, new Rectangle(0, 0, trackBar1.Value, trackBar2.Value));
            }
        }

        private void Results_Load(object sender, EventArgs e)
        {
            trackBar1.Value = pictureBox1.Size.Width;
            trackBar2.Value = pictureBox1.Size.Height;

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            pictureBox1.Size = new Size(trackBar1.Value, pictureBox1.Size.Height);

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            pictureBox1.Size = new Size(pictureBox1.Size.Width, trackBar2.Value);

        }
    }
}
