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
    public partial class DeepRunSetting : MetroFramework.Forms.MetroForm
    {
        private List<bool> CheckList = new List<bool>();
        public DeepRunSetting()
        {
            InitializeComponent();
            LoadSavedConfs();
            InitCheckList();
            
        }
        private void InitCheckList()
        {

            for (int i = 0; i < SavedConfsList.Rows.Count; i++)
            {
                CheckList.Add(false);
            }
        }
        private void LoadSavedConfs()
        {
            SavedConfsList.Rows.Clear();
            if (Directory.Exists(Directory.GetCurrentDirectory() + "\\Configurations\\Series\\Saved"))
            {
                DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Configurations\\Series\\Saved");
                DirectoryInfo[] dirs = dir.GetDirectories();
                int n = -1;
                foreach (DirectoryInfo f in dirs)
                {
                    n++;
                    SavedConfsList.Rows.Add();
                    SavedConfsList.Rows[n].Cells[0].Value = System.IO.Path.GetFileNameWithoutExtension(@f.FullName);
                    SavedConfsList.Rows[n].Cells[1].Value = f.FullName;
                    SavedConfsList.Rows[n].Cells[3].Value = "+";
                    SavedConfsList.Rows[n].Cells[4].Value = "-";
                }
            }
           
        }
        private DataGridViewCellEventArgs cell_e = null;
        private int CurrRow = -1;
        private DataGridViewCellEventArgs cell_e1 = null;
        int n = -1;
        System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["MainClass"];

        private void AddActiveSavedGenConfs()
        {
            bool Isdone = false;
            for (int i = 0; i < metroGrid1.RowCount; i++)
            {
                if (Convert.ToInt32(metroGrid1.Rows[i].Cells[2].Value) == 1)
                {
                    for (int j = 0; j < ((MainClass)f).GenConfsGrid.RowCount; j++)
                    {
                        if (((MainClass)f).GenConfsGrid.Rows[j].Cells[1].Value.ToString() == metroGrid1.Rows[i].Cells[5].Value.ToString())
                        {

                            Isdone = true;
                        }
                    }
                    if (!Isdone)
                    {
                        ((MainClass)f).GenConfsGrid.Rows.Add(
                        metroGrid1.Rows[i].Cells[0].Value,
                        metroGrid1.Rows[i].Cells[5].Value,
                        metroGrid1.Rows[i].Cells[2].Value,
                        0);
                    }
                    Isdone = false;
                }


            }

        }

        private void SavedConfsList_CellClick(object sender, DataGridViewCellEventArgs _e)
        {
            if ((_e.ColumnIndex != -1) && (_e.RowIndex != -1))
            {
                cell_e = _e;

                if (cell_e.ColumnIndex == 2)
                {
                    if (Convert.ToInt32(SavedConfsList.CurrentRow.Cells[cell_e.ColumnIndex].Value) == 0)
                    {
                        SavedConfsList.CurrentRow.Cells[cell_e.ColumnIndex].Value = 1;
                       


                        if(metroGrid1.Rows.Count!=0)
                        {
                            for(int i=0;i< metroGrid1.Rows.Count;i++)
                            {
                                if ((int)metroGrid1.Rows[i].Cells[4].Value == SavedConfsList.CurrentRow.Index)
                                {
                                    metroGrid1.Rows[i].Cells[2].Value = 1;
                                }
                            }
                        }

                    }
                    else
                    {
                        SavedConfsList.CurrentRow.Cells[cell_e.ColumnIndex].Value = 0;
                     

                        if (metroGrid1.Rows.Count != 0)
                        {
                            for (int i = 0; i < metroGrid1.Rows.Count; i++)
                            {
                                if ((int)metroGrid1.Rows[i].Cells[4].Value == SavedConfsList.CurrentRow.Index)
                                {
                                    metroGrid1.Rows[i].Cells[2].Value = 0;
                                }
                            }
                        }
                    }
                       
                }
                if (cell_e.ColumnIndex == 3)
                {
                    metroGrid1.ScrollBars = ScrollBars.Both;
                    
                    CurrRow = SavedConfsList.CurrentRow.Index;



                    if (CheckList[CurrRow] == false)
                    {
                        //metroGrid1.Rows.Clear();
                        DirectoryInfo dir = new DirectoryInfo(SavedConfsList.CurrentRow.Cells[1].Value.ToString());
                    DirectoryInfo[] dirs = dir.GetDirectories();
                   
                    foreach (DirectoryInfo f in dirs)
                    {
                        
                        n++;
                        
                        metroGrid1.Rows.Add();
                        metroGrid1.Rows[n].Cells[0].Value = System.IO.Path.GetFileNameWithoutExtension(@f.FullName);

                           
                            
                            metroGrid1.Rows[n].Cells[1].Value = f.FullName;

                            string[] files = Directory.GetFiles(metroGrid1.Rows[n].Cells[1].Value.ToString(), "*.xml");
                            if (File.Exists(files[0]))
                            {
                                metroGrid1.Rows[n].Cells[5].Value = files[0];
                            }

                            metroGrid1.Rows[n].Cells[4].Value = CurrRow;
                        
                        metroGrid1.Rows[n].Cells[2].Value = SavedConfsList.Rows[CurrRow].Cells[2].Value;
                       
                    }
                    }
                    else
                    {
                        if (metroGrid1.Rows.Count != 0)
                        {
                            for (int i = 0; i < metroGrid1.Rows.Count; i++)
                            {
                                if ((int)metroGrid1.Rows[i].Cells[4].Value == SavedConfsList.CurrentRow.Index)
                                {
                                    metroGrid1.Rows[i].Cells[2].Value = SavedConfsList.Rows[CurrRow].Cells[2].Value;
                                }
                            }
                        }
                    }

                    if (CheckList[CurrRow] == false)
                    {
                        CheckList[CurrRow] = true;
                    }
        
                }

                if (cell_e.ColumnIndex == 4)
                {
                    metroGrid1.ClearSelection();
                    metroGrid1.ScrollBars = ScrollBars.None;

                    if (metroGrid1.Rows.Count != 0)
                    {
                        for (int i = metroGrid1.Rows.Count-1; i >= 0; i--)
                        {
                            if ((int)metroGrid1.Rows[i].Cells[4].Value == SavedConfsList.CurrentRow.Index)
                            {

                                metroGrid1.FirstDisplayedScrollingRowIndex = metroGrid1.Rows.Count -1;
                                metroGrid1.Rows.RemoveAt(i);
                                n--;
                                CheckList[SavedConfsList.CurrentRow.Index] = false;
                            }
                        }
                       
                        
                    }
                    metroGrid1.ScrollBars = ScrollBars.Both;
                }


            }
        }
        private void metroGrid1_CellClick(object sender, DataGridViewCellEventArgs _e)
        {
            if ((_e.ColumnIndex != -1) && (_e.RowIndex != -1))
            {
                cell_e1 = _e;

                if (cell_e1.ColumnIndex == 2)
                {
                    if (Convert.ToInt32(metroGrid1.CurrentRow.Cells[cell_e1.ColumnIndex].Value) == 0)
                    {
                        metroGrid1.CurrentRow.Cells[cell_e1.ColumnIndex].Value = 1;
                    }
                    else
                    {
                        metroGrid1.CurrentRow.Cells[cell_e1.ColumnIndex].Value = 0;
                    }
                        
                }
                if (cell_e1.ColumnIndex == 3)
                {
                    metroGrid2.Rows.Clear();
                   
                    string[] files = Directory.GetFiles(metroGrid1.CurrentRow.Cells[1].Value.ToString(), "*.xml");
                    DataSet ds = new DataSet();
                    if (File.Exists(files[0]))
                    {
                        ds.ReadXml(files[0]);
                        metroTextBox1.Text = files[0];


                    foreach (DataRow item in ds.Tables["exe"].Rows)
                    {
                        int k = -1;
                        foreach (object cell in item.ItemArray)
                        {
                            k++;
                            if (k < (item.ItemArray.Length / 2))
                            {
                                metroGrid2.Rows.Add();
                                metroGrid2.Rows[k].Cells[0].Value = item["key" + k];
                                metroGrid2.Rows[k].Cells[1].Value = item["par" + k];
                            }

                        }

                    }
                    }
                }
            }
        }
        private void metroButton1_Click(object sender, EventArgs e)
        {
            AddActiveSavedGenConfs();
        }
        private void metroButton2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                AddActiveSavedGenConfs();
            }
            else
            {
                if (metroGrid1.RowCount != 0)
                {
                    DialogResult m = MessageBox.Show("Применить выбранные конфигурации?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (m == DialogResult.Yes)
                    {
                        AddActiveSavedGenConfs();
                        this.Close();
                    }
                    else if (m == DialogResult.No)
                    {
                        this.Close();
                    }
                }
            }

            this.Close();
        }
        private void SavedConfsList_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {

            if((e.RowIndex!=-1)&&(e.ColumnIndex==0))
            {

            
            string GenListpath = SavedConfsList.Rows[e.RowIndex].Cells[1].Value.ToString();
            if(File.Exists(GenListpath + "\\FullGenList.txt"))
            {
                string body = File.ReadAllText(GenListpath + "\\FullGenList.txt");
                SavedConfsList.Rows[e.RowIndex].Cells[0].ToolTipText = body;
            }
            }



        }
        private void metroGrid1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex != -1) && (e.ColumnIndex == 0))
            {


                string GenListpath = metroGrid1.Rows[e.RowIndex].Cells[1].Value.ToString();
                if (File.Exists(GenListpath + "\\GenList.txt"))
                {
                    string body = File.ReadAllText(GenListpath + "\\GenList.txt");
                    metroGrid1.Rows[e.RowIndex].Cells[0].ToolTipText = body;
                }
            }
        }
        private void metroGrid1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;


            if (e.ColumnIndex == 3)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.strelkaRight.Width;
                var h = Properties.Resources.strelkaRight.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.strelkaRight, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }
    }
}
