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
    public partial class Generator : MetroFramework.Forms.MetroForm
    {
        private DataGridViewCellEventArgs eSet = null;
        private DataGridViewCellEventArgs cell_e = null;
        private string ConfigFullName = "";
        System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["MainClass"];
        private List<string[]> WordsList = new List<string[]>();
        private List<string> BigSubList = new List<string>();

        public Generator(DataGridViewCellEventArgs _e, string _ConfigFullName)
        {
            InitializeComponent();
            eSet = _e;
            ConfigFullName = _ConfigFullName;
            CreateSettingsTable(SettingsConfigTable, ConfigFullName);
            metroComboBox1.SelectedIndex = 0;
            metroTextBox3.Text = System.IO.Path.GetFileNameWithoutExtension(@ConfigFullName);
            CreateTemplName();
            checkBox2.Checked = true;
            checkBox3.Checked = true;
        }
        private void CreateSettingsTable(MetroFramework.Controls.MetroGrid Table ,string _ConfigFullName)
        {
                 if (File.Exists(_ConfigFullName))
            {
                try
                {

                    Table.Rows.Clear();
                    DataSet ds = new DataSet();
                    ds.ReadXml(_ConfigFullName);
                    foreach (DataRow item in ds.Tables["exe"].Rows)
                    {
                        int n = -1;
                        foreach (object cell in item.ItemArray)
                        {
                            n++;
                            if (n < (item.ItemArray.Length / 2))
                            {
                                Table.Rows.Add();
                                Table.Rows[n].Cells[0].Value = item["key" + n];
                                Table.Rows[n].Cells[1].Value = item["par" + n];
                            }

                        }

                    }
                }
                catch
                {
                    MetroFramework.MetroMessageBox.Show(this, "Некорректный XML файл.", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "XML файл не найден.", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }
        private void ReadStrValues()
        {
            string str = "";
            string par = "";
            for(int j=0;j< SettingsConfigTable.Rows.Count-1;j++)
            {
                str = SettingsConfigTable.Rows[j].Cells[1].Value.ToString();
                par = SettingsConfigTable.Rows[j].Cells[0].Value.ToString();
                string[] words = str.Split(';');
                WordsList.Add(words);
                if(words.Length>1)
                {
                    BigSubList.Add(par);
                }
            }
        }
        private void SerialWritter(string _SerialConfigPath, int k, ref List<string[]> CList)
        {
            string start = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<exe>\n";
            string program_name = " <Prog>" + ((MainClass)f).gProgram_name + "</Prog>\n";
            string body = "";
            string end = "\n</exe>\n<?include somedata?>";
            
            for (int i = 0; i < SettingsConfigTable.Rows.Count - 1; i++)
            {
                string tagParSt = "\n  <key" + i + ">";

                string parameter_name = SettingsConfigTable[0, i].Value.ToString();

                string tagParFin = "</key" + i + ">\n";

                string tagValSt = "   <par" + i + ">";

                string value = CList[k][i];

                string tagValFin = "</par" + i + ">\n";

                body += tagParSt + parameter_name + tagParFin + tagValSt + value + tagValFin;
            }
            System.IO.File.AppendAllText(_SerialConfigPath, start + program_name + body + end);


        }
        private string CreateSeriesSettingConf(string nameTempl,bool UseAllTemplName)
        {
            string ShortConfFilename = "";
            ShortConfFilename =  metroTextBox1.Text;
            
            ReadStrValues();
            string DirPath = Path.Combine(Directory.GetCurrentDirectory() + "\\Configurations\\Series\\Temp", ShortConfFilename);
            if (Directory.Exists(DirPath))
            {
                Directory.Delete(DirPath, true);
            }
            if (!Directory.Exists(DirPath))
            {
                Directory.CreateDirectory(DirPath);
            }

            var product = Exten.CartesianProduct(WordsList);
            List<string[]> CarList = product.Select(innerEnumerable => innerEnumerable.ToArray()).ToList();
            //metroTextBox1.Clear();
            //for (int i = 0; i < CarList.Count; i++)
            //{

            //    metroTextBox1.Text += " {";
            //    for (int j = 0; j < CarList[i].Length; j++)
            //    {
            //        metroTextBox1.Text += CarList[i][j] + " ";
            //    }
            //    metroTextBox1.Text += "} " + Environment.NewLine;
            //}

            for (int i = 0; i < CarList.Count; i++)
            {
                string subDirPath = Path.Combine(Directory.GetCurrentDirectory() + "\\Configurations\\Series\\Temp", ShortConfFilename + "\\" + ShortConfFilename + "_" + i);
                if (Directory.Exists(subDirPath))
                {
                    Directory.Delete(subDirPath, true);
                }
                if (!Directory.Exists(subDirPath))
                {
                    Directory.CreateDirectory(subDirPath);
                }
                string SerialConfigPath = subDirPath + "\\" + ShortConfFilename + "_" + i +  ".xml";
                SerialWritter(SerialConfigPath, i, ref CarList);
                WriteGenComb(ShortConfFilename, i);


                

            }
            string FullGenListPath = Directory.GetCurrentDirectory() + "\\Configurations\\Series\\Temp\\" + ShortConfFilename + "\\" + "FullGenList.txt";
            string fullGenBody = "";
            string str = "";
            string par = "";
            for (int j = 0; j < SettingsConfigTable.Rows.Count - 1; j++)
            {
                str = SettingsConfigTable.Rows[j].Cells[1].Value.ToString();
                par = SettingsConfigTable.Rows[j].Cells[0].Value.ToString();

                fullGenBody += par + " = " + str + Environment.NewLine;

            }
            System.IO.File.AppendAllText(FullGenListPath, fullGenBody);

            BigSubList.Clear();
            WordsList.Clear();

            return ShortConfFilename;

        }
        private void WriteGenComb(string _ShortConfFilename,int i)
        {
            

            string DirPath = Directory.GetCurrentDirectory() + "\\Configurations\\Series\\Temp\\" + _ShortConfFilename + "\\" + _ShortConfFilename + "_" + i;
            string FilePath = DirPath + "\\" + _ShortConfFilename + "_" + i + ".xml";
            if (Directory.Exists(DirPath)&&File.Exists(FilePath))
            {
                try
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(FilePath);
                    foreach (DataRow item in ds.Tables["exe"].Rows)
                    {
                        int n = -1;
                        string body = "";
                        foreach (object cell in item.ItemArray)
                        {
                            n++;
                            if (n < (item.ItemArray.Length / 2))
                            {

                                for(int j = 0;j< BigSubList.Count;j++)
                                {
                                    if (item["key" + n].ToString()==(BigSubList[j]))
                                    {
                                        body += item["key" + n].ToString() +" = "+item["par" + n].ToString() + Environment.NewLine;
                                    }
                                }
                            }
                        }
                        System.IO.File.AppendAllText(DirPath + "\\" + "GenList.txt", body);
                        body = "";
                    }
                   
                }
                catch
                {
                    MetroFramework.MetroMessageBox.Show(this, "Некорректный XML файл.", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "XML файл не найден.", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
        }
        private void SaveGen()
        {
            if(SettingConfigList.RowCount!=0)
            {

           

            string SavedPath = Directory.GetCurrentDirectory() + "\\Configurations\\Series\\Saved";
            if (!Directory.Exists(SavedPath))
            {
                Directory.CreateDirectory(SavedPath);
            }
            string[] PreWords = Path.GetDirectoryName(SettingConfigList.Rows[0].Cells[1].Value.ToString()).Split('\\');
            if (Directory.Exists(SavedPath + "\\" + PreWords[PreWords.Length - 2]))
            {
                Directory.Delete(SavedPath + "\\" + PreWords[PreWords.Length - 2], true);
            }

            for (int i = 0; i < SettingConfigList.RowCount; i++)
            {
                if (Convert.ToInt32(SettingConfigList.Rows[i].Cells[2].Value) == 1)
                {

                    string FullDirPath = Path.GetDirectoryName(SettingConfigList.Rows[i].Cells[1].Value.ToString());
                    string shortName = System.IO.Path.GetFileNameWithoutExtension(FullDirPath) + ".xml";

                    string lastDir =   Path.GetFileName(FullDirPath);
                    


                    string[] words = FullDirPath.Split('\\');
                    string back = "";
                    for (int j = words.Length-2;j< words.Length;j++)
                    {
                        back += words[j]+"\\";
                    }
                    

                    if (Directory.Exists(SavedPath + "\\" + back))
                    {
                        Directory.Delete(SavedPath + "\\" + back,true);
                    }

                    if (!Directory.Exists(SavedPath +"\\"+ back))
                    {
                        Directory.CreateDirectory(SavedPath + "\\" + back);
                    }

                    if(File.Exists(Path.Combine(SavedPath, back + "\\" + shortName)))
                    {
                        File.Delete(Path.Combine(SavedPath, back + "\\" + shortName));
                    }
                    File.Copy(SettingConfigList.Rows[i].Cells[1].Value.ToString(), Path.Combine(SavedPath, back + "\\" + shortName));

                    if (File.Exists(Path.Combine(SavedPath, back + "\\" + "GenList.txt")))
                    {
                        File.Delete(Path.Combine(SavedPath, back + "\\" + "GenList.txt"));
                    }
                    File.Copy(FullDirPath + "\\"+ "GenList.txt", Path.Combine(SavedPath, back + "\\" + "GenList.txt"));



                    string[] GenWords = FullDirPath.Split('\\');
                    string GenBack = "";
                    for (int j = 0; j < GenWords.Length - 1; j++)
                    {
                        GenBack += GenWords[j] + "\\";
                    }

                    if (File.Exists(Path.Combine(SavedPath, GenWords[GenWords.Length - 2] + "\\" + "FullGenList.txt")))
                    {
                        File.Delete(Path.Combine(SavedPath, GenWords[GenWords.Length - 2] + "\\" + "FullGenList.txt"));
                    }
                    File.Copy(GenBack + "\\" + "FullGenList.txt", Path.Combine(SavedPath, GenWords[GenWords.Length - 2] + "\\" + "FullGenList.txt"));
                }
            }
            }
        }
        private void AddSerToRun()
        {

            ((MainClass)f).ReadConfsInDir(((MainClass)f).TextBoxChosenDirXML.Text);
            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\Configurations\\Series\\Temp"))
            {
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Configurations\\Series\\Temp");
            }
            string _ShortConfFilename = CreateSeriesSettingConf(metroTextBox1.Text, checkBox2.Checked);
            SettingConfigList.Rows.Clear();
            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Configurations\\Series\\Temp\\" + _ShortConfFilename);
            DirectoryInfo[] dirs = dir.GetDirectories();
            foreach (DirectoryInfo file in dirs)
            {

                string[] fileName = System.IO.Directory.GetFiles(file.FullName, "*.xml");
                for (int i = 0; i < fileName.Length; i++)
                {
                    if (File.Exists(fileName[i]))
                    {
                        string Shortname = System.IO.Path.GetFileNameWithoutExtension(@fileName[i]);

                        DataGridViewRow rowToAdd = (DataGridViewRow)((MainClass)f).ConfigList.Rows[0].Clone();
                        rowToAdd.Cells[0].Value = Shortname + ".xml";//short name
                        rowToAdd.Cells[1].Value = fileName[i];//full name
                        rowToAdd.Cells[2].Value = 1;//use
                        rowToAdd.Cells[3].Value = 0;//mpi
                        SettingConfigList.Rows.Add(rowToAdd);
                    }
                }

            }
        }
        private void CreateTemplName()
        {
            if (metroTextBox3.Text != "")
            {
                string UserFileName = "";
                string[] words = metroComboBox1.SelectedItem.ToString().Split('*');
                if (checkBox3.Checked)
                {
                    UserFileName += words[0];
                    UserFileName += metroTextBox2.Text;
                    UserFileName += words[1] + "_";
                }
                if (checkBox2.Checked)
                {
                    UserFileName += metroTextBox3.Text;
                }

                metroTextBox1.Text = UserFileName;
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Пустое имя в шаблоне", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            SaveGen();
        }
        private void SettingConfigList_CellClick(object sender, DataGridViewCellEventArgs _e)
        {
            if ((_e.ColumnIndex != -1) && (_e.RowIndex != -1))
            {
                cell_e = _e;

                if ((cell_e.ColumnIndex == 2) || (cell_e.ColumnIndex == 3))
                {
                    if (Convert.ToInt32(SettingConfigList.CurrentRow.Cells[cell_e.ColumnIndex].Value) == 0)
                        SettingConfigList.CurrentRow.Cells[cell_e.ColumnIndex].Value = 1;
                    else
                        SettingConfigList.CurrentRow.Cells[cell_e.ColumnIndex].Value = 0;
                }
                if (cell_e.ColumnIndex == 4)
                {
                    CreateSettingsTable(metroGrid1, SettingConfigList.CurrentRow.Cells[1].Value.ToString());
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            AddSerToRun();
        }
        private void metroButton1_Click_1(object sender, EventArgs e)
        {


            if (checkBox1.Checked)
            {
                SaveGen();
            }
            else
            {
                if (SettingConfigList.RowCount != 0)
                {
                    DialogResult m = MessageBox.Show("Сохранить выбранные конфигурации?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (m == DialogResult.Yes)
                    {
                        SaveGen();
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
        private void SettingConfigList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
           
                if (e.RowIndex < 0)
                    return;

             
                if (e.ColumnIndex == 4)
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
        private void button2_Click(object sender, EventArgs e)
        {
            CreateTemplName();
        }
        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateTemplName();
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            CreateTemplName();
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            CreateTemplName();
        }
    }
}
