using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using System.Xml.Linq;
namespace Bridge
{
    public partial class MainClass : MetroFramework.Forms.MetroForm
    {
        private int i = 0;
        private void InitTaskTable(MetroFramework.Controls.MetroGrid _InfoTable, Tuple<String, String,String>[] Array)
        {
            int size = 57;

            size = Array.Length;
            _InfoTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            for (int k = 0; k < 2; k++)
            {
                _InfoTable.Columns[k].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }

            for (int i = 0; i < size; i++)
            {
                _InfoTable.Rows.Add(Array[i].Item1, Array[i].Item2,Array[i].Item3);
            }
            EditorTabControl.SelectedIndex = 0;
            _InfoTable.CurrentCell = _InfoTable[0, 0];
            _InfoTable.Rows[0].Cells[0].Selected = false;
        }
        private void InitTable(MetroFramework.Controls.MetroGrid _InfoTable, Tuple<String, String, String, String>[] Array)
        {
            int size = 57;
         
            size = Array.Length;
            _InfoTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            for (int k = 0; k < 4; k++)
            {
                _InfoTable.Columns[k].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }

            for (int i = 0; i < size; i++)
            {
                _InfoTable.Rows.Add(Array[i].Item1, Array[i].Item2, Array[i].Item3, Array[i].Item4);
            }
            EditorTabControl.SelectedIndex = 0;
            _InfoTable.CurrentCell = _InfoTable[0, 0];
            _InfoTable.Rows[0].Cells[0].Selected = false;
        }
        private void Search(MetroFramework.Controls.MetroGrid Table, MetroFramework.Controls.MetroTextBox _TextBoxSearch,MetroFramework.Controls.MetroLabel ResLabel)
        {
                int count = 0;
                for (int k = 0; k < Table.RowCount; k++)
                {
                    for (int j = 0; j < Table.ColumnCount; j++)
                    {
                        if (Table.Rows[k].Cells[j].Value != null)
                        {
                        if(Table.Rows[k].Cells[j].Value.ToString().IndexOf(_TextBoxSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                           
                            {
                                count++;
                                break;
                            }
                        }
                    }
                }
                ResLabel.Text = "Найдено соответствий: " + count.ToString();


            for (; i < Table.RowCount ;)
            {
                for (int j = 0; j < Table.ColumnCount; j++)
                {
                    if (Table.Rows[i].Cells[j].Value != null)
                    {
                        if (Table.Rows[i].Cells[j].Value.ToString().IndexOf(_TextBoxSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            Table.CurrentCell = Table[0, i];
                            if (i < Table.RowCount - 2)
                            {
                                i++;
                            }
                            else
                            {
                                i = 0;
                            }
                            return;
                        }
                    }
                }
                i++;
            }
            i = 0;
        }
        private void ChoseFile()
        {
            string path = "";
            string PathDir = "";
            OpenFileDialog OPF = new OpenFileDialog();
            
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                path = OPF.FileName;
                PathDir = Path.GetDirectoryName(path);
                if ((Directory.GetCurrentDirectory() == PathDir))
                {
                    ValueTextBox.Text = System.IO.Path.GetFileName(@path);
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Файл должен лежть в одной директории с examin.exe", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
           
            
           
        }
        private static void AddConfig(TaskConf config, string name)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = name;
            myStream = openFileDialog.OpenFile();

            StreamReader read = new StreamReader(myStream);
            string xml = read.ReadToEnd();
            read.Close();

            XDocument doc = XDocument.Parse(xml);
            XElement[] mxe = doc.Root.Elements().ToArray();
            for (int i = 0; i < mxe.Length; i++)
            {
                config.Add(mxe[i]);
            }
        }
        private void OpenTaskXML(bool needDialog)
        {
            if (needDialog)
            {
                OpenFileDialog OPF = new OpenFileDialog();
                OPF.InitialDirectory = Directory.GetCurrentDirectory();
               OPF.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                //OPF.FilterIndex = 1;
                //OPF.RestoreDirectory = true;

                
                if (OPF.ShowDialog() == DialogResult.OK)
                {
                    gTaskConfig_path = OPF.FileName;
                }
            }

            if (File.Exists(gTaskConfig_path))
            {
                try
                {
                    metroGrid10.Rows.Clear();
                    metroTextBox1.Lines = File.ReadAllLines(gTaskConfig_path);
                    metroTextBox5.Text = gTaskConfig_path;

                    TaskConf conf = new TaskConf();
                    conf.Name = Path.GetFileName(gTaskConfig_path);
                    conf.Comment = TaskConf.XMLConfiguration;
                    AddConfig(conf, gTaskConfig_path);

                    for (int i = 0; i < conf.GetItems().Count; i++)
                    {
                        metroGrid10.Rows.Add(conf.GetItems()[i].Name, conf.GetItems()[i].Value);
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
            if (metroTextBox5.Text != "")
            {
                metroButton8.Enabled = true;
                metroButton9.Enabled = true;
            }
        }
        private void OpenXML(bool needDialog)
        {
            if (needDialog)
            {
                OpenFileDialog OPF = new OpenFileDialog();
                OPF.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                if (Directory.Exists(Directory.GetCurrentDirectory() + "\\Configurations"))
                {
                    OPF.InitialDirectory = Directory.GetCurrentDirectory() + "\\Configurations";
                }
                else
                {
                    OPF.InitialDirectory = Directory.GetCurrentDirectory();
                }
                
                if (OPF.ShowDialog() == DialogResult.OK)
                {
                    gConfig_path = OPF.FileName;
                }
            }

            if (File.Exists(gConfig_path))
            {
                try
                {
                    TextBoxXML.Lines = File.ReadAllLines(gConfig_path);
                    TextBoxPath.Text = gConfig_path;
                    ConfigTable.Rows.Clear();
                    DataSet ds = new DataSet();
                    ds.ReadXml(gConfig_path);
                    foreach (DataRow item in ds.Tables["exe"].Rows)
                    {
                        int n = -1;
                        foreach (object cell in item.ItemArray)
                        {
                            n++;
                            if (n < (item.ItemArray.Length / 2 ))
                            {
                                ConfigTable.Rows.Add();
                                ConfigTable.Rows[n].Cells[0].Value = item["key" + n];
                                ConfigTable.Rows[n].Cells[1].Value = item["par" + n];
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
            if (TextBoxPath.Text != "")
            {
                WriteConf.Enabled = true;
                AddLink.Enabled = true;
            }
        }
        private void TaskWritter(String _gTaskConfig_path)
        {

            string start = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<config>";
            string body = "";
            string end = "\n</config>\n";
            string Start_parameter_name = "";
            string value = "";
            string Fin_parameter_name = "";

            Dictionary<string, string> A = new Dictionary<string, string>();


            for (int i = 0; i < metroGrid10.Rows.Count - 1; i++)
            {
                if (A.ContainsKey(metroGrid10[0, i].Value.ToString()))
                {
                    A[metroGrid10[0, i].Value.ToString()] = metroGrid10[1, i].Value.ToString();
                }
                else
                {
                    A.Add(metroGrid10[0, i].Value.ToString(), metroGrid10[1, i].Value.ToString());
                }
            }
            foreach (KeyValuePair<string, string> keyValue in A)
            {
                Start_parameter_name = "\n  <" + keyValue.Key + ">";

                value = keyValue.Value;

                Fin_parameter_name = "</" + keyValue.Key + ">";

                body += Start_parameter_name + value + Fin_parameter_name;
            }
            System.IO.File.AppendAllText(_gTaskConfig_path, start + body + end);
           
        }
        private void Writter(String _gConfig_path)
        {
            
            string start = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<exe>\n";
            string program_name = " <Prog>" + gProgram_name + "</Prog>\n";
            string body = "";
            string end = "\n</exe>\n<?include somedata?>";

            for (int i = 0; i < ConfigTable.Rows.Count -1; i++)
            {
                string tagParSt = "\n  <key" + i + ">";

                string parameter_name = ConfigTable[0, i].Value.ToString();

                string tagParFin = "</key" + i + ">\n";

                string tagValSt = "   <par" + i + ">";

                string value = ConfigTable[1, i].Value.ToString();

                string tagValFin = "</par" + i + ">\n";

                if ((parameter_name == "-Separator") && ((value == ";") || (value == ".")))
                {
                    MetroFramework.MetroMessageBox.Show(this, "Недоступное значение Separator \"" + value + "\"", "Оповещение.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    value = "_";
                }
                
                
                    body += tagParSt + parameter_name + tagParFin + tagValSt + value + tagValFin;
                

               
            }
            System.IO.File.AppendAllText(_gConfig_path, start + program_name + body + end);
      

        }
        private void TaskWriteConfing()
        {
            if (gTaskConfig_path != "")
            {
                try
                {
                    if (File.Exists(gTaskConfig_path))
                    {
                        System.IO.File.WriteAllText(@gTaskConfig_path, string.Empty);
                        TaskWritter(gTaskConfig_path);
                        metroTextBox1.Lines= File.ReadAllLines(gTaskConfig_path);
                        metroTextBox5.Text = gTaskConfig_path;
                    }
                   
                    if (metroGrid10.Rows.Count > 1)
                    {
                        MessageBox.Show(this, "XML файл успешно изменен.", "Выполнено.");
                    }

                }
                catch
                {
                    MetroFramework.MetroMessageBox.Show(this, "Невозможно изменить XML файл.", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "XML файл не выбран", "Оповещение.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (metroTextBox5.Text != "")
            {
                metroButton8.Enabled = true;
                metroButton9.Enabled = true;
            }
        }
        private void WriteConfing()
        {
            if (gConfig_path != "")
            {
                try
                {
                    if (File.Exists(gConfig_path))
                    {
                     System.IO.File.WriteAllText(@gConfig_path, string.Empty);
                     Writter(gConfig_path);
                     TextBoxXML.Lines = File.ReadAllLines(gConfig_path);
                     TextBoxPath.Text = gConfig_path;
                    }
                    EditorTabControl.SelectedIndex = 0;
                    if (ConfigTable.Rows.Count > 1)
                    {
                        MessageBox.Show(this, "XML файл успешно изменен.", "Выполнено.");
                    }

                }
                catch
                {
                    MetroFramework.MetroMessageBox.Show(this, "Невозможно изменить XML файл.", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "XML файл не выбран", "Оповещение.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (TextBoxPath.Text != "")
            {
                WriteConf.Enabled = true;
                AddLink.Enabled = true;
            }
        }
        private void CreateTaskXML()
        {
            Stream myStream;
            SaveFileDialog SF = new SaveFileDialog();
            SF.Filter = "xml files (*.xml)|*.xml";
            SF.InitialDirectory = Directory.GetCurrentDirectory();
            SF.FilterIndex = 2;
            SF.RestoreDirectory = true;

            if (SF.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = SF.OpenFile()) != null)
                {
                    myStream.Close();
                }
                gTaskConfig_path = SF.FileName;

                string start = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<config>";
                string body = "";
                string end = "\n</config>\n";
                System.IO.File.AppendAllText(gTaskConfig_path, start+ body + end);

                metroTextBox1.Lines = File.ReadAllLines(gTaskConfig_path);
                metroTextBox5.Text = gTaskConfig_path;
                metroGrid10.Rows.Clear();
            }
            if (metroTextBox5.Text != "")
            {
                metroButton8.Enabled = true;
                metroButton9.Enabled = true;
            }
        }
        private void CreateXML()
        {
            Stream myStream;
            SaveFileDialog SF = new SaveFileDialog();

            SF.Filter = "xml files (*.xml)|*.xml";
            SF.InitialDirectory = Directory.GetCurrentDirectory();
            if (Directory.Exists(Directory.GetCurrentDirectory() + "\\Configurations"))
            {
                SF.InitialDirectory = Directory.GetCurrentDirectory() + "\\Configurations";
            }
            else
            {
                SF.InitialDirectory = Directory.GetCurrentDirectory();
            }
            SF.FilterIndex = 2;
            SF.RestoreDirectory = true;

            if (SF.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = SF.OpenFile()) != null)
                {
                    myStream.Close();
                }
                gConfig_path = SF.FileName;
     
                    string start = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<exe>\n";
                    string program_name = " <Prog>" + gProgram_name + "</Prog>\n";
                    string body = "";
                    string end = "\n</exe>\n<?include somedata?>";
                    System.IO.File.AppendAllText(gConfig_path, start + program_name + body + end);
            
                TextBoxXML.Lines = File.ReadAllLines(gConfig_path);
                TextBoxPath.Text = gConfig_path;
                ConfigTable.Rows.Clear();
            }
            if (TextBoxPath.Text != "")
            {
                WriteConf.Enabled = true;
                AddLink.Enabled = true;
            }
        }
        private void CreateXMLDefault()
        {

            String currentPath = Directory.GetCurrentDirectory();

            if (!Directory.Exists(Path.Combine(currentPath, "Configurations")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath, "Configurations"));
            }

            String date = DateTime.Now.ToString("[HH-mm-ss]_dd.MM.yy");
            String OutFileName = "Configure" + date;
            String FinalPath = currentPath + "\\Configurations" + "\\" + OutFileName + ".xml";
            gConfig_path = FinalPath;

                string start = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<exe>\n";
                string program_name = " <Prog>" + gProgram_name + "</Prog>\n";
                string body = "";
                string end = "\n</exe>\n<?include somedata?>";

                System.IO.File.AppendAllText(FinalPath, start + program_name + body + end);

            
            TextBoxXML.Lines = File.ReadAllLines(FinalPath);
            TextBoxPath.Text = FinalPath;
            ConfigTable.Rows.Clear();

            if (TextBoxPath.Text != "")
            {
                WriteConf.Enabled = true;
                AddLink.Enabled = true;
            }
        }
        private void DeleteTaskXML()
        {
            if (gTaskConfig_path != "")
            {
                if (File.Exists(gTaskConfig_path))
                {
                    File.Delete(gTaskConfig_path);
                    metroTextBox1.Text= "";
                    metroTextBox5.Text = "";
                    metroGrid10.Rows.Clear();
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Нет выбранного XML файла", "Оповещение.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (metroTextBox5.Text == "")
            {
                metroButton8.Enabled = false;
                metroButton9.Enabled = false;
            }
        }
        private void DeleteXML()
        {
            if (gConfig_path != "")
            {
                if (File.Exists(gConfig_path))
                {
                    File.Delete(gConfig_path);
                    TextBoxXML.Text = "";
                    TextBoxPath.Text = "";
                    ConfigTable.Rows.Clear();
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Нет выбранного XML файла", "Оповещение.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (TextBoxPath.Text == "")
            {
                WriteConf.Enabled = false;
                AddLink.Enabled = false;
            }
        }
        private void AddLinkToTaskConf()
        {
            String value = metroTextBox3.Text;
            String parameter = metroTextBox2.Text;
            
            if ((value != "") && (parameter != ""))
            {
                metroGrid10.Rows.Add(parameter, value);
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Параметр и его значение должны быть указаны.", "Оповещение.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void AddLinkToConf()
        {
            String value = ValueTextBox.Text;
            String parameter = ParNameTextBox.Text;
            if ((parameter == "-Separator") && ((value == ";")||(value == ".")))
            {
                MetroFramework.MetroMessageBox.Show(this, "Недоступное значение Separator \""+value+"\"", "Оповещение.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                value = "_";
                ValueTextBox.Text = "_";
            }
            if ((value != "") && (parameter != ""))
            {
                ConfigTable.Rows.Add(parameter, value);
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Параметр и его значение должны быть указаны.", "Оповещение.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void TaskConfSaveAs()
        {
            if (File.Exists(gTaskConfig_path))
            {
                SaveFileDialog SF = new SaveFileDialog();

                SF.FileName = "";
                SF.Filter = "xml files (*.xml)|*.xml";
                SF.InitialDirectory = Directory.GetCurrentDirectory();
                if (SF.ShowDialog() == DialogResult.OK)
                {
                    if (SF.FileName != "")
                    {
                        File.Delete(SF.FileName);
                    }
                    try
                    {
                        if (File.Exists(gTaskConfig_path))
                        {

                            gTaskConfig_path = SF.FileName;

                            TaskWritter(gTaskConfig_path);

                        }
                        MessageBox.Show(this, "XML файл успешно сохранен.", "Выполнено.");
                    }
                    catch
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Невозможно сохранить XML файл.", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Нет выбранного XML файла", "Оповещение.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void ConfSaveAs()
        {
            if (File.Exists(gConfig_path))
            {
                SaveFileDialog SF = new SaveFileDialog();

                SF.FileName = "";
                SF.Filter = "xml files (*.xml)|*.xml";
                SF.InitialDirectory = Directory.GetCurrentDirectory();
                if (Directory.Exists(Directory.GetCurrentDirectory() + "\\Configurations"))
                {
                    SF.InitialDirectory = Directory.GetCurrentDirectory() + "\\Configurations";
                }
                else
                {
                    SF.InitialDirectory = Directory.GetCurrentDirectory();
                }
                if (SF.ShowDialog() == DialogResult.OK)
                {
                    if (SF.FileName != "")
                    {
                        File.Delete(SF.FileName);
                    }
                    try
                    {
                        if (File.Exists(gConfig_path))
                        {

                            gConfig_path = SF.FileName;

                            Writter(gConfig_path);

                        }
                        MessageBox.Show(this, "XML файл успешно сохранен.", "Выполнено.");
                    }
                    catch
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Невозможно сохранить XML файл.", "Ошибка.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                    
                
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Нет выбранного XML файла", "Оповещение.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


    }
}
