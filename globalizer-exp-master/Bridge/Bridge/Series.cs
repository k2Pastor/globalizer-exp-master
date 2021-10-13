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
using Excel = Microsoft.Office.Interop.Excel;


namespace Bridge
{
   
    public partial class Series : MetroFramework.Forms.MetroForm
    {
        public DataGridViewCellMouseEventArgs eSer = null;
        public DataGridViewCellMouseEventArgs eRes = null;

        private Excel.Application excelApp;

        public Series(DataGridViewCellMouseEventArgs _e)
        {
            InitializeComponent();
            eSer = _e;
            PrintJournalSeries();
            fillFullDescriptionTextBox();
            enableOpenXLButton();
            enableOpenGraphicGalleryButton();
            //   AddJournalRecords();
        }
        System.Windows.Forms.Form f = System.Windows.Forms.Application.OpenForms["MainClass"];

        private void enableOpenXLButton()
        {
            String exPath = ((MainClass)f).GridJournal.Rows[eSer.RowIndex].Cells[1].Value.ToString();
            String fileName = "\\Operating Characteristics.xlsx";
            if (File.Exists(exPath + fileName))
            {
                OpenXLButton.Enabled = true;
            }
        }

        private void enableOpenGraphicGalleryButton()
        {
            String exPath = ((MainClass)f).GridJournal.Rows[eSer.RowIndex].Cells[1].Value.ToString();
            String lineGraphicImageName = "\\Line Graphic.bmp";
            String columnGraphicImageName = "\\Column Graphic.bmp";
            if (File.Exists(exPath + lineGraphicImageName) || File.Exists(exPath + columnGraphicImageName))
            {
                OpenGalleryButton.Enabled = true;
            }
        }
        
        public void PrintJournalSeries()
        {
            string SeriesFullName = ((MainClass)f).GridJournal.Rows[eSer.RowIndex].Cells[1].Value.ToString();


            string expPath = SeriesFullName;
            string LocExpPath = SeriesFullName;
            if (Directory.Exists(LocExpPath))
            {
                string journalPath = expPath + "\\Journal.xml";
                File.WriteAllText(journalPath, string.Empty);
                string start = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<journal>\n";
                string end = "\n</journal>\n<?include somedata?>\n";
                System.IO.File.AppendAllText(journalPath, start);
                SeriesGridJournal.Rows.Clear();
                string LogPath = SeriesFullName;
                if (Directory.Exists(LogPath) && Directory.Exists(Directory.GetCurrentDirectory() + "\\Configurations"))
                {
                    int k = 0;
                    DirectoryInfo dir = new DirectoryInfo(LogPath);
                    DirectoryInfo[] dirs = dir.GetDirectories();
                    foreach (DirectoryInfo f in dirs)
                    {
                        if (File.Exists(f.FullName + "\\Log.txt"))
                        {
                            string confP = f.FullName + "\\ConfPath.txt";
                            if (File.Exists(confP))
                            {
                                string[] readText = System.IO.File.ReadAllLines(confP);
                                string ConfPath = readText[0];

                                if (ConfPath.Contains("\\Series\\"))
                                {
                                    if (ConfPath.Contains("\\Series\\Saved\\"))
                                    {
                                        string ShortConfFilename = System.IO.Path.GetFileNameWithoutExtension(@ConfPath);
                                        AddExpRecord(k, ConfPath, f.FullName + "\\Log.txt", f.CreationTime.ToString());
                                        SeriesGridJournal.Rows.Add(f.CreationTime, f.FullName, f.Name, ConfPath, ShortConfFilename);
                                        k++;


                                    }
                                    if (ConfPath.Contains("\\Series\\Temp\\"))
                                    {
                                        string ShortConfFilename = System.IO.Path.GetFileNameWithoutExtension(@ConfPath);
                                        AddExpRecord(k, "TEMP_GEN", f.FullName + "\\Log.txt", f.CreationTime.ToString());
                                        SeriesGridJournal.Rows.Add(f.CreationTime, f.FullName, f.Name, "TEMP_GEN", ShortConfFilename);
                                        k++;
                                    }
                                }
                                else
                                {
                                    if (File.Exists(ConfPath))
                                    {
                                        String ConfigName = new DirectoryInfo(ConfPath).Name;
                                        AddExpRecord(k, ConfPath, f.FullName + "\\Log.txt", f.CreationTime.ToString());
                                        SeriesGridJournal.Rows.Add(f.CreationTime, f.FullName, f.Name, ConfPath, ConfigName);
                                        k++;
                                    }
                                    else
                                    {
                                        AddExpRecord(k, "Confuguration not found", f.FullName + "\\Log.txt", f.CreationTime.ToString());
                                        SeriesGridJournal.Rows.Add(f.CreationTime, f.FullName, f.Name, "not", "Файл не найден");
                                        k++;
                                    }
                                }

                                
                            }
                            else
                            {
                                AddExpRecord(k, "Confuguration path not saved", f.FullName + "\\Log.txt", f.CreationTime.ToString());
                                SeriesGridJournal.Rows.Add(f.CreationTime, f.FullName, f.Name, "not", "Не сохранен путь");
                                k++;
                            }
                        }
                        else
                        {
                            string confP = f.FullName + "\\ConfPath.txt";
                            if (File.Exists(confP))
                            {
                                string[] readText = System.IO.File.ReadAllLines(confP);
                                string ConfPath = readText[0];

                                if (File.Exists(ConfPath))
                                {
                                    String ConfigName = new DirectoryInfo(ConfPath).Name;
                                    AddExpRecord(k, ConfPath, "Файл не найден", f.CreationTime.ToString());
                                    SeriesGridJournal.Rows.Add(f.CreationTime, "not", "Файл не найден", ConfPath, ConfigName);
                                    k++;
                                }
                                else
                                {
                                    AddExpRecord(k, "Confuguration not found", "Файл не найден", f.CreationTime.ToString());
                                    SeriesGridJournal.Rows.Add(f.CreationTime, "not", "Файл не найден", "not", "Файл не найден");
                                    k++;
                                }
                            }
                            else
                            {
                                AddExpRecord(k, "Confuguration path not saved", "Файл не найден", f.CreationTime.ToString());
                                SeriesGridJournal.Rows.Add(f.CreationTime, "not", "Файл не найден", "not", "Не сохранен путь");
                                k++;
                            }
                        }
                    }
                    System.IO.File.AppendAllText(journalPath, end);
                    SeriesGridJournal.CurrentCell = SeriesGridJournal[0, 0];
                    SeriesGridJournal.Rows[0].Cells[0].Selected = false;
                }
            }
        }

        public void AddExpRecord(int num, string confPath, string LogPath, string date)
        {
            string SeriesFullName = ((MainClass)f).GridJournal.Rows[eSer.RowIndex].Cells[1].Value.ToString();

            string date_exp = "\n <exp" + num + ">\n\n<date>\n" + date + "\n</date>";
            string experiment_Path = "\n<expPath>\n" + LogPath + "\n</expPath>\n";
            string configuration_Path = " <confPath>\n" + confPath + "\n</confPath>\n\n </exp" + num + ">\n";
            string expPath = SeriesFullName;
            string journalPath = expPath + "\\Journal.xml";
            System.IO.File.AppendAllText(journalPath, date_exp + experiment_Path + configuration_Path);
        }

        private void SeriesGridJournal_CellMouseClick(object sender, DataGridViewCellMouseEventArgs _e)
        {
            if ((_e.ColumnIndex != -1) && (_e.RowIndex != -1))
            {
                //e = _e;
                String name = Convert.ToString(SeriesGridJournal.Rows[_e.RowIndex].Cells[2].Value);
                String path = Convert.ToString(SeriesGridJournal.Rows[_e.RowIndex].Cells[1].Value);
                string filePath = path + "\\Log.txt";
                if (File.Exists(filePath))
                {
                    StreamReader file = new StreamReader(filePath);
                    string lines = file.ReadToEnd();
                }
            }
        }

        private void SeriesGridJournal_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs _e)
        {
            if ((_e.ColumnIndex != -1) && (_e.RowIndex != -1))
            {
                eRes = _e;
                using (Results Res = new Results(eRes))
                {
                    Res.ShowDialog();
                }
            }
        }

        private void SaveDescription_Click(object sender, EventArgs e)
        {
            String fullDescription = fullDescriptionTextBox.Text;
            if (fullDescription != "")
            {
                String exPath = ((MainClass)f).GridJournal.Rows[eSer.RowIndex].Cells[1].Value.ToString();
                if (Directory.Exists(exPath))
                {
                    DirectoryInfo directory = new DirectoryInfo(exPath);
                    DirectoryInfo[] scopeDirectories = directory.GetDirectories();
                    foreach (DirectoryInfo dinf in scopeDirectories)
                    {
                        String fullDescriptionPath = dinf.FullName + "\\FullDescription.txt";
                        if (File.Exists(fullDescriptionPath))
                        {
                            File.WriteAllText(fullDescriptionPath, String.Empty);
                            File.WriteAllText(fullDescriptionPath, fullDescription);
                        } else
                        {
                            File.WriteAllText(fullDescriptionPath, fullDescription);
                        }
                    }
                }
            } else
            {
                MetroFramework.MetroMessageBox.Show(this, "Заполните описание", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DeleteDescription_Click(object sender, EventArgs e)
        {
            String fullDescription = fullDescriptionTextBox.Text;
            if (fullDescription != "")
            {
                String exPath = ((MainClass)f).GridJournal.Rows[eSer.RowIndex].Cells[1].Value.ToString();
                if (Directory.Exists(exPath))
                {
                    DirectoryInfo directory = new DirectoryInfo(exPath);
                    DirectoryInfo[] scopeDirectories = directory.GetDirectories();
                    foreach (DirectoryInfo dinf in scopeDirectories)
                    {
                        String fullDescriptionPath = dinf.FullName + "\\FullDescription.txt";
                        if (File.Exists(fullDescriptionPath))
                        {
                            fullDescriptionTextBox.Clear();
                            File.Delete(fullDescriptionPath);
                        }
                    }
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Заполните описание", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void fillFullDescriptionTextBox()
        {
            String exPath = ((MainClass)f).GridJournal.Rows[eSer.RowIndex].Cells[1].Value.ToString();
            if (Directory.Exists(exPath)) {
                DirectoryInfo directory = new DirectoryInfo(exPath);
                DirectoryInfo[] scopeDirectories = directory.GetDirectories();
                foreach (DirectoryInfo dinf in scopeDirectories)
                {
                    String fullDescriptionPath = dinf.FullName + "\\FullDescription.txt";
                    if (File.Exists(fullDescriptionPath))
                    {
                        fullDescriptionTextBox.Text = File.ReadAllText(fullDescriptionPath);
                    }
                }
            }
        }

        private void OpenGraphicButton_Click(object sender, EventArgs e)
        {
            String exPath = ((MainClass)f).GridJournal.Rows[eSer.RowIndex].Cells[1].Value.ToString();
            String fileName = "\\Operating Characteristics.xlsx";
            excelApp = new Excel.Application();
            excelApp.Visible = true;
            excelApp.Workbooks.Open(exPath + fileName,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                            Type.Missing, Type.Missing);
        }

        private void OpenGalleryButton_Click(object sender, EventArgs e)
        {
            String exPath = ((MainClass)f).GridJournal.Rows[eSer.RowIndex].Cells[1].Value.ToString();
            GraphicsGallery graphicsGalleryDialog = new GraphicsGallery(exPath);
            graphicsGalleryDialog.ShowDialog();
        }
    }
}
