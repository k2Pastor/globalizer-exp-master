using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
namespace Bridge
{
    public partial class MainClass : MetroFramework.Forms.MetroForm
    {
        private String gConfig_path = "";
        public String gProgram_name = "examin.exe";
        private String gTaskConfig_path = "";
        private String gChosenXML = "";
        private String gChosenDirXML = Directory.GetCurrentDirectory() + "\\" + "Configurations";
        private String gTempChosenXML = "";
        private String gChosenProgram;
        private String defaultChosenProgram = "examin.exe";
        private String commandLineData = "";
        private bool openedFromParameters = false;
        private int SeriesNumber = 0 ;
        private DataGridViewCellEventArgs Set_cell_e = null;
        private DataGridViewCellEventArgs cell_e = null;
        private DataGridViewCellMouseEventArgs e = null;
      

        public MainClass()
        {
            InitializeComponent();
            Сlassification classif = new Сlassification();
            InitTable(InfoTable, classif.Task);
            InitTable(metroGrid1, classif.Method);
            InitTable(metroGrid2, classif.Parallel);
            InitTable(metroGrid3, classif.Solver);
            InitTable(metroGrid4, classif.Other);

            TaskParams TaskClassif = new TaskParams();
            InitTaskTable(metroGrid8, TaskClassif.problem_With_Constraints);
            InitTaskTable(metroGrid5, TaskClassif.MCO_solver);
            InitTaskTable(metroGrid6, TaskClassif.deceptive_problem);
            InitTaskTable(metroGrid7, TaskClassif.ansys_problem);

            UpdateExpJournal();

            TextBoxChosenProgram.Text = defaultChosenProgram;
            TextBoxChosenDirXML.Text = gChosenDirXML;
            metroButton3.Enabled = false;
            metroComboBox1.SelectedIndex = 0;
            if (TextBoxPath.Text == "")
            {
                WriteConf.Enabled = false;
                AddLink.Enabled = false;
            }
            if (metroTextBox5.Text == "")
            {
                metroButton8.Enabled = false;
                metroButton9.Enabled = false;
            }
        }

        public void setInitialDataParams()
        {
            this.openedFromParameters = false;
            this.commandLineData = "";
        }

        private void MainClass_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (File.Exists(TempXML))
            {
                File.Delete(TempXML);
            }
            if(Directory.Exists(Directory.GetCurrentDirectory() + "\\Configurations\\Series\\Temp"))
                {
                DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Configurations\\Series\\Temp");
                DirectoryInfo[] dirs = dir.GetDirectories();
                foreach (DirectoryInfo file in dirs)
                {
                    if (Directory.Exists(file.FullName))
                    {
                        Directory.Delete(file.FullName, true);
                    }

                }
            }
           
        }
      
        //Run
        private void metroButton2_Click(object sender, EventArgs e)
        {
            GenConfsGrid.Rows.Clear();
        }
        private void metroButton3_Click(object sender, EventArgs e)
        {
            ChoseFile();
        }
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenXML(true);
        }
        private void NewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CreateXML();
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CreateXMLDefault();
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DeleteXML();
        }
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfSaveAs();
            OpenXML(false);
        }
        private void metroButton4_Click(object sender, EventArgs e)
        {
            metroContextMenu1.Show(metroButton4, 0, metroButton4.Height);
        }
        private void metroButton5_Click(object sender, EventArgs e)
        {
            metroContextMenu2.Show(metroButton5, 0, metroButton5.Height);
        }
        private void Open1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenTaskXML(true);
        }
        private void Create1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateTaskXML();
        }
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskConfSaveAs();
            OpenTaskXML(false);
        }
        private void DelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteTaskXML();
        }
        private void metroButton9_Click(object sender, EventArgs e)
        {
            TaskWriteConfing();
        }
        private void metroButton8_Click(object sender, EventArgs e)
        {
            AddLinkToTaskConf();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            CreateSerTemplName();
        }
        private void metroComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateSerTemplName();
        }
        private void StopButton_Click(object sender, EventArgs e)
        {
            StopFunc();
        }
        private void GridConfAllCheckBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (GridConfAllCheckBox1.Checked)
            {

                for (int i = 0; i < ConfigList.Rows.Count; i++)
                {
                    ConfigList.Rows[i].Cells[2].Value = 1;
                }
            }
            else
            {
                for (int i = 0; i < ConfigList.Rows.Count; i++)
                {
                    ConfigList.Rows[i].Cells[2].Value = 0;
                }
            }
        }
        private void MPIAllCheckBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (MPIAllCheckBox1.Checked)
            {

                for (int i = 0; i < ConfigList.Rows.Count; i++)
                {
                    ConfigList.Rows[i].Cells[3].Value = 1;
                }
            }
            else
            {
                for (int i = 0; i < ConfigList.Rows.Count; i++)
                {
                    ConfigList.Rows[i].Cells[3].Value = 0;
                }
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox1.Checked)
            {

                for (int i = 0; i < GenConfsGrid.Rows.Count; i++)
                {
                    GenConfsGrid.Rows[i].Cells[2].Value = 1;
                }
            }
            else
            {
                for (int i = 0; i < GenConfsGrid.Rows.Count; i++)
                {
                    GenConfsGrid.Rows[i].Cells[2].Value = 0;
                }
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {

                for (int i = 0; i < GenConfsGrid.Rows.Count; i++)
                {
                    GenConfsGrid.Rows[i].Cells[3].Value = 1;
                }
            }
            else
            {
                for (int i = 0; i < GenConfsGrid.Rows.Count; i++)
                {
                    GenConfsGrid.Rows[i].Cells[3].Value = 0;
                }
            }
        }
        private void GridJournal_CellMouseClick(object sender, DataGridViewCellMouseEventArgs _e)
        {
            if ((_e.ColumnIndex != -1) && (_e.RowIndex != -1))
            {
                e = _e;
            }
        }
        private void GridJournal_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs _e)
        {
            if ((_e.ColumnIndex != -1) && (_e.RowIndex != -1))
            {
                e = _e;
                using (Series Ser = new Series(e))
                {
                    Ser.ShowDialog();
                }
            }
        }
        private void ResultsButton_Click(object sender, EventArgs _e)
        {
            if (e != null)
            {
                using (Series Ser = new Series(e))
                {
                    Ser.ShowDialog();
                }
            }
        }
        private void RunComboFin_Click(object sender, EventArgs e)
        {
            gChosenProgram = TextBoxChosenProgram.Text;
            CreateTempConfigs();
            if (ComboSize != 0)
            {
                RunComboFin.Enabled = false;
                TextMpiComm.Enabled = false;
                ButtonChoseTargetXML.Enabled = false;
                ButtonChoseProgram.Enabled = false;
                Run.Enabled = false;
                ButOpenConfList.Enabled = false;
                ChoseDirConfBut.Enabled = false;
                TextBoxChosenDirXML.Enabled = false;
                TextBoxChosenProgram.Enabled = false;
                TextBoxChosenXML.Enabled = false;
                metroButton2.Enabled = false;
                metroButton1.Enabled = false;
                ResultsButton.Enabled = false;
                SearchButton2.Enabled = false;
                ComboFinRun(ComboSize, ActiveConfs, TempComboXML);
            }
        }

        //Editor
        private void SearchButton2_Click(object sender, EventArgs e)
        {
            Search(GridJournal, SearchTextBox2, ResLabel2);
        }
        private void ButOpenConfList_Click(object sender, EventArgs e)
        {
            ReadConfsInDir(TextBoxChosenDirXML.Text);
        }
        private void WriteConf_Click(object sender, EventArgs e)
        {
            WriteConfing();
        }
        private void AddLink_Click(object sender, EventArgs e)
        {
            AddLinkToConf();
        }
        private void SearchInfo_Click(object sender, EventArgs e)
        {
            int index = metroTabControl1.SelectedIndex;
            if (index == 0)
            {
                Search(metroGrid3, TextBoxSearch, SearchResLabel);
            }
            if (index == 1)
            {
                Search(metroGrid1, TextBoxSearch, SearchResLabel);
            }
            if (index == 2)
            {
                Search(metroGrid2, TextBoxSearch, SearchResLabel);
            }
            if (index == 3)
            {
                Search(InfoTable, TextBoxSearch, SearchResLabel);
            }
            if (index == 4)
            {
                Search(metroGrid4, TextBoxSearch, SearchResLabel);
            }

        }
        private void Run_Click(object sender, EventArgs e)
        {
            SeriesNumber++;
            gChosenProgram = TextBoxChosenProgram.Text;
            gTempChosenXML = TextBoxChosenXML.Text;
            if (gChosenProgram != "")
            {
                Run_exp(gTempChosenXML, gChosenXML, gChosenProgram, commandLineData, CheckMpiCom.Checked, true, openedFromParameters);
                openedFromParameters = false;
                TextBoxChosenXML.Clear();
                ButtonChoseTargetXML.Enabled = true;
            } else { 
                 MetroFramework.MetroMessageBox.Show(this, "Выберите программу-исполнителя!", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void ButtonChoseTargetXML_Click(object sender, EventArgs e)
        {
            ChoseXML();
        }
        private void ButtonChoseProgram_Click(object sender, EventArgs e)
        {
            ChoseProgram();
        }
        private void ConfigList_CellClick(object sender, DataGridViewCellEventArgs _e)
        {
            if ((_e.ColumnIndex != -1) && (_e.RowIndex != -1))
            {
                cell_e = _e;

                if ((cell_e.ColumnIndex == 2) || (cell_e.ColumnIndex == 3))
                {
                    if (Convert.ToInt32(ConfigList.CurrentRow.Cells[cell_e.ColumnIndex].Value) == 0)
                        ConfigList.CurrentRow.Cells[cell_e.ColumnIndex].Value = 1;
                    else
                        ConfigList.CurrentRow.Cells[cell_e.ColumnIndex].Value = 0;
                }
                if (cell_e.ColumnIndex == 4)
                {
                    if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\Configurations\\Series\\Temp"))
                    {
                        Directory.CreateDirectory((Directory.GetCurrentDirectory() + "\\Configurations\\Series\\Temp"));
                    }
                    DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory() + "\\Configurations\\Series\\Temp");
                    DirectoryInfo[] dirs = dir.GetDirectories();
                    foreach (DirectoryInfo file in dirs)
                    {
                        if (Directory.Exists(file.FullName))
                        {
                            Directory.Delete(file.FullName, true);
                        }

                    }

                    // GenConfsGrid.Rows.Clear();
                    SettingsRun(_e);
                }
            }

        }
        private void ChoseDirConfBut_Click(object sender, EventArgs e)
        {
            ChoseDirXML();
        }
        private void ConfigList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
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
        private void GenConfsGrid_CellClick(object sender, DataGridViewCellEventArgs _e)
        {
            if ((_e.ColumnIndex != -1) && (_e.RowIndex != -1))
            {
                Set_cell_e = _e;

                if ((Set_cell_e.ColumnIndex == 2) || (Set_cell_e.ColumnIndex == 3))
                {
                    if (Convert.ToInt32(GenConfsGrid.CurrentRow.Cells[Set_cell_e.ColumnIndex].Value) == 0)
                        GenConfsGrid.CurrentRow.Cells[Set_cell_e.ColumnIndex].Value = 1;
                    else
                        GenConfsGrid.CurrentRow.Cells[Set_cell_e.ColumnIndex].Value = 0;
                }
            }
        }
        private void metroButton1_Click(object sender, EventArgs e)
        {
            DeepRunSetting DeepS = new DeepRunSetting();
            DeepS.ShowDialog();
        }
        private void metroGrid3_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex != -1) && (e.RowIndex != -1))
            {
                ParNameTextBox.Text = Convert.ToString(metroGrid3.Rows[e.RowIndex].Cells[0].Value);
                ValueTextBox.Text = Convert.ToString(metroGrid3.Rows[e.RowIndex].Cells[2].Value);
            }
        }
        private void metroGrid3_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex != -1) && (e.RowIndex != -1))
            {
                if (TextBoxPath.Text != "")
                {
                    AddLinkToConf();
                }
            }
        }
        private void metroGrid1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex != -1) && (e.RowIndex != -1))
            {
                ParNameTextBox.Text = Convert.ToString(metroGrid1.Rows[e.RowIndex].Cells[0].Value);
                ValueTextBox.Text = Convert.ToString(metroGrid1.Rows[e.RowIndex].Cells[2].Value);
            }
        }
        private void metroGrid1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex != -1) && (e.RowIndex != -1))
            {
                if (TextBoxPath.Text != "")
                {
                    AddLinkToConf();
                }
            }
        }
        private void metroGrid2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex != -1) && (e.RowIndex != -1))
            {
                ParNameTextBox.Text = Convert.ToString(metroGrid2.Rows[e.RowIndex].Cells[0].Value);
                ValueTextBox.Text = Convert.ToString(metroGrid2.Rows[e.RowIndex].Cells[2].Value);
            }
        }
        private void metroGrid2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex != -1) && (e.RowIndex != -1))
            {
                if (TextBoxPath.Text != "")
                {
                    AddLinkToConf();
                }
            }
        }
        private void InfoTable_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex != -1) && (e.RowIndex != -1))
            {
                ParNameTextBox.Text = Convert.ToString(InfoTable.Rows[e.RowIndex].Cells[0].Value);
                ValueTextBox.Text = Convert.ToString(InfoTable.Rows[e.RowIndex].Cells[2].Value);

                if ((ParNameTextBox.Text == "-libPath") || (ParNameTextBox.Text == "-libConfigPath") || (ParNameTextBox.Text == "-FirstPointFilePath"))
                {
                    metroButton3.Enabled = true;
                }
                else
                {
                    metroButton3.Enabled = false;
                }
            }

        }
        private void InfoTable_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex != -1) && (e.RowIndex != -1))
            {

                if (TextBoxPath.Text != "")
                {
                    AddLinkToConf();
                }
            }
        }
        private void metroGrid4_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex != -1) && (e.RowIndex != -1))
            {
                ParNameTextBox.Text = Convert.ToString(metroGrid4.Rows[e.RowIndex].Cells[0].Value);
                ValueTextBox.Text = Convert.ToString(metroGrid4.Rows[e.RowIndex].Cells[2].Value);
            }
        }
        private void metroGrid4_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex != -1) && (e.RowIndex != -1))
            {
                if (TextBoxPath.Text != "")
                {
                    AddLinkToConf();
                }
            }
        }
        private void metroGrid5_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex != -1) && (e.RowIndex != -1))
            {

                if (metroTextBox5.Text != "")
                {
                    AddLinkToTaskConf();
                }
            }
        }
        private void metroGrid5_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex != -1) && (e.RowIndex != -1))
            {
                metroTextBox2.Text = Convert.ToString(metroGrid5.Rows[e.RowIndex].Cells[0].Value);
                metroTextBox3.Text = Convert.ToString(metroGrid5.Rows[e.RowIndex].Cells[2].Value);
            }
        }
        private void metroGrid6_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex != -1) && (e.RowIndex != -1))
            {
                if (metroTextBox5.Text != "")
                {
                    AddLinkToTaskConf();
                }

            }
        }
        private void metroGrid6_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex != -1) && (e.RowIndex != -1))
            {
                metroTextBox2.Text = Convert.ToString(metroGrid6.Rows[e.RowIndex].Cells[0].Value);
                metroTextBox3.Text = Convert.ToString(metroGrid6.Rows[e.RowIndex].Cells[2].Value);
            }
        }
        private void metroGrid7_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex != -1) && (e.RowIndex != -1))
            {
                if (metroTextBox5.Text != "")
                {
                    AddLinkToTaskConf();
                }

            }
        }
        private void metroGrid7_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex != -1) && (e.RowIndex != -1))
            {
                metroTextBox2.Text = Convert.ToString(metroGrid7.Rows[e.RowIndex].Cells[0].Value);
                metroTextBox3.Text = Convert.ToString(metroGrid7.Rows[e.RowIndex].Cells[2].Value); ;
            }
        }
        private void metroGrid8_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex != -1) && (e.RowIndex != -1))
            {
                if (metroTextBox5.Text != "")
                {
                    AddLinkToTaskConf();
                }

            }
        }
        private void metroGrid8_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.ColumnIndex != -1) && (e.RowIndex != -1))
            {
                metroTextBox2.Text = Convert.ToString(metroGrid8.Rows[e.RowIndex].Cells[0].Value);
                metroTextBox3.Text = Convert.ToString(metroGrid8.Rows[e.RowIndex].Cells[2].Value);
            }
        }

        private void processResponse(String data, bool openedFromParameters)
        {
            this.commandLineData = data;
            this.openedFromParameters = openedFromParameters;
        }

        private void AddParameters_Click(object sender, EventArgs e)
        {
            Parameters parametersDialog = new Parameters(new delegateRequestParams(processResponse));
            parametersDialog.ShowDialog();
            ButtonChoseTargetXML.Enabled = false;
        }

        private void AddDescription_Click(object sender, EventArgs e)
        {
            List<String> seriesNames = new List<String>();
            if (BriefDescriptionTextBox.Text != "")
            {
                if (GridJournal.SelectedRows.Count != 0)
                {
                    for (int i = 0; i < GridJournal.Rows.Count; i++)
                    {
                        if (GridJournal.Rows[i].Selected)
                        {
                            GridJournal.Rows[i].Cells[3].Value = BriefDescriptionTextBox.Text;
                            seriesNames.Add((GridJournal.Rows[i].Cells[1].Value).ToString());
                        }
                    }
                } else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Выберите строку", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                String briefDescription = BriefDescriptionTextBox.Text;
                foreach (String seriesName in seriesNames)
                {
                    String exPath = seriesName;
                    if (Directory.Exists(exPath))
                    {
                        DirectoryInfo directory = new DirectoryInfo(exPath);
                        DirectoryInfo[] scopeDirectories = directory.GetDirectories();
                        foreach (DirectoryInfo dinf in scopeDirectories)
                        {
                            String briefDescriptionPath = dinf.FullName + "\\BriefDescription.txt";
                            if (File.Exists(briefDescriptionPath))
                            {
                                System.IO.File.WriteAllText(briefDescriptionPath, String.Empty);
                                System.IO.File.WriteAllText(briefDescriptionPath, briefDescription);
                            } else
                            {
                                System.IO.File.WriteAllText(briefDescriptionPath, briefDescription);
                            }
                        }
                    }
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Введите текст", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void ClearDescription_Click(object sender, EventArgs e)
        {
            List<String> seriesNames = new List<String>();
            if (GridJournal.SelectedRows.Count != 0)
            {
                for (int i = 0; i < GridJournal.Rows.Count; i++)
                {
                    if (GridJournal.Rows[i].Selected)
                    {
                        GridJournal.Rows[i].Cells[3].Value = null;
                        seriesNames.Add((GridJournal.Rows[i].Cells[1].Value).ToString());
                    } 
                }
                foreach (String seriesName in seriesNames)
                {
                    String exPath = seriesName;
                    if (Directory.Exists(exPath))
                    {
                        DirectoryInfo directory = new DirectoryInfo(exPath);
                        DirectoryInfo[] scopeDirectories = directory.GetDirectories();
                        foreach (DirectoryInfo dinf in scopeDirectories)
                        {
                            String briefDescriptionPath = dinf.FullName + "\\BriefDescription.txt";
                            if (File.Exists(briefDescriptionPath))
                            {
                                File.Delete(briefDescriptionPath);
                            }
                            else
                            {
                                MetroFramework.MetroMessageBox.Show(this, "Выберите строчку с непустым значением описания", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            } else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Выберите строку", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
        }

        private void NMaxTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char entry = e.KeyChar;
            if (!Char.IsDigit(entry) && entry != 8)
            {
                e.Handled = true;
            }

        }

        private void DeltaTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char entry = e.KeyChar;
            if (!Char.IsDigit(entry) && entry != 8)
            {
                e.Handled = true;
            }
        }
    }
    public static class Exten
    {
        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
        {
            // базовый случай:
            IEnumerable<IEnumerable<T>> result = new[] { Enumerable.Empty<T>() };
            foreach (var sequence in sequences)
            {
                var s = sequence;
                result =
                  from seq in result
                  from item in s
                  select seq.Concat(new[] { item });
            }
            return result;
        }
    }
}
