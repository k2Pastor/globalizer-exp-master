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
    public partial class MainClass : MetroFramework.Forms.MetroForm
    {
        private String TempXML = "";
        private String MpiCommand = "";
        private const String MAX_NUM_OF_POINTS = "-MaxNumOfPoints";
        private const String NUMBER_OF_TRIALS = "NumberOfTrials";
        private const String GLOBAL_OPTIMUM_FOUND = "Global optimum FOUND";


        private List<string> Results = new List<string>();
        private List<string> ActiveConfs = new List<string>();
        private List<string> TempComboXML = new List<string>();
        private List<bool> MpiList = new List<bool>();

        private int ComboSize = 0;
        private int comboT = 0;
        private bool Stop = false;
        private Process PR;

        private Excel.Application excelApp;

        private void SetMpiRun(bool _UseMpi)
        {
            if (_UseMpi)
            {
                if (TextBoxChosenDistributedFile.Text != null && TextBoxChosenDistributedFile.Text != String.Empty)
                {
                    if (File.Exists(TextBoxChosenDistributedFile.Text))
                    {
                        MpiCommand = "mpiexec -n " + TextMpiComm.Text + " -ppn 1 -hosts ";
                        String[] fileLines = File.ReadAllLines(TextBoxChosenDistributedFile.Text);
                        for (int i = 0; i < fileLines.Length; i++)
                        {
                            if (i != fileLines.Length - 1)
                            {
                                MpiCommand += fileLines[i] + ",";
                            } else
                            {
                                MpiCommand += fileLines[i];
                            }
                        }
                    } else {
                        MetroFramework.MetroMessageBox.Show(this, "Файл распределенного запуска не найден.", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                } else
                {
                    MpiCommand = "mpiexec -n " + TextMpiComm.Text;
                }
           
            } else
            {
                MpiCommand = String.Empty;
                if (TextBoxChosenDistributedFile.Text != null || TextBoxChosenDistributedFile.Text != String.Empty)
                {
                    throw new DistributedLaunchException("Для распределенного запуска необходимо использовать MPI");
                }
            }
        }
        private void SingleStartFunc(ProcessStartInfo _psi, String _Source_Config_path, bool UseMpi)
        {
            StopButton.Enabled = true;
            PR = new Process();
            PR.StartInfo = _psi;
            PR.EnableRaisingEvents = true;
            PR.Start();
            string result = PR.StandardOutput.ReadToEnd();
            String date = DateTime.Now.ToString("(HH-mm-ss)_dd.MM.yy") + "_{" + (comboT).ToString() + "}";
            AddExperiment(date, result, _Source_Config_path, UseMpi, true);
            UpdateExpJournal();
            StopButton.Enabled = false;
        }
        private async void Run_exp(String _Temp_Config_path, String _Source_Config_path, String _ChosenProgram, String commandLineData, bool UseMpi, bool SingleStart, bool openedFromParameters)
        {
            String ProgramName = "examin.exe";
            String _Config_path = _Temp_Config_path;
            if ((_Config_path != "") && !openedFromParameters && commandLineData == "")
            {
                if ((File.Exists(_Config_path)) && (File.Exists(_ChosenProgram)))
                {
                    String CurConfigName = new DirectoryInfo(_Config_path).Name;
                    DataSet ds = new DataSet();
                    ds.ReadXml(CurConfigName);
                    foreach (DataRow item in ds.Tables["exe"].Rows)
                    {
                        int n = -1;
                        foreach (object cell in item.ItemArray)
                        {
                            n++;
                            if (n < (item.ItemArray.Length / 2))
                            {
                                commandLineData += item["key" + n];
                                commandLineData += " ";
                                commandLineData += item["par" + n];
                                commandLineData += " ";
                            }

                        }

                    }
                    /** For Operation Characteristics' Graphic **/
                    if (NMaxTextBox.Text != String.Empty && !SingleStart)
                    {
                        commandLineData += MAX_NUM_OF_POINTS;
                        commandLineData += " ";
                        commandLineData += NMaxTextBox.Text;
                    }

                    SetMpiRun(UseMpi);

                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c " + MpiCommand + " " + ProgramName + " " + commandLineData,
                        // '/c' is close cmd after run
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                        CreateNoWindow = true
                    };

                    TextBoxChosenDirXML.Text = "/c " + MpiCommand + " " + ProgramName + " " + commandLineData;

                    if (SingleStart)
                    {

                        ProgressBarJour.Value = 0;
                        await TaskEx.Run(() => SingleStartFunc(psi, _Source_Config_path, UseMpi));
                        ProgressBarJour.Value = 100;

                    }
                    else
                    {
                        PR = new Process();
                        PR.StartInfo = psi;
                        PR.EnableRaisingEvents = true;
                        PR.Start();
                        Results.Add(PR.StandardOutput.ReadToEnd());

                    }

                }
                else MetroFramework.MetroMessageBox.Show(this, "XML или EXE не найден", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if ((_Config_path == "") && openedFromParameters && commandLineData != "")
            {
                SetMpiRun(UseMpi);

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c " + MpiCommand + " " + ProgramName + " " + commandLineData,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    CreateNoWindow = true
                };

                TextBoxChosenDirXML.Text = "/c " + MpiCommand + " " + ProgramName + " " + commandLineData;

                setInitialDataParams();
                ProgressBarJour.Value = 0;
                await TaskEx.Run(() => SingleStartFunc(psi, String.Empty, UseMpi));
                ProgressBarJour.Value = 100;
            }
        }


        private void CreateTempConfigs()
        {
           
            for (int i = 0; i < ConfigList.RowCount; i++)
            {
                if (Convert.ToInt32(ConfigList.Rows[i].Cells[2].Value) == 1)
                {
                    ActiveConfs.Add(ConfigList.Rows[i].Cells[1].Value.ToString());
                    MpiList.Add(Convert.ToBoolean(ConfigList.Rows[i].Cells[3].Value));
                   
                }
                

            }

            for (int i = 0; i < GenConfsGrid.RowCount; i++)
            {
                if (Convert.ToInt32(GenConfsGrid.Rows[i].Cells[2].Value) == 1)
                {
                    ActiveConfs.Add(GenConfsGrid.Rows[i].Cells[1].Value.ToString());
                    MpiList.Add(Convert.ToBoolean(GenConfsGrid.Rows[i].Cells[3].Value));

                }


            }
            ComboSize = 0;
            comboT = 0;
            foreach (String SingleConf in ActiveConfs)
            {
                gChosenXML = SingleConf;
                if (File.Exists(gChosenXML))
                {
                    String ConfigName = new DirectoryInfo(gChosenXML).Name;
                    TempXML = Directory.GetCurrentDirectory() + "\\" + ConfigName;
                    if (!File.Exists(TempXML))
                    {
                        File.Copy(gChosenXML, TempXML);
                    }
                    gTempChosenXML = TempXML;
                }

                TempComboXML.Add(gTempChosenXML);
                ComboSize++;
                comboT++;
                gTempChosenXML = TextBoxChosenXML.Text;
            }

        }
        private void StopFunc()
        {
            if (PR != null)
            {
                try
                {
                    PR.Kill();
                    PR.WaitForExit();
                }
                catch
                {

                } 
            }
            Process[] ProcessesExamin = System.Diagnostics.Process.GetProcessesByName("examin");
            foreach (Process pr in ProcessesExamin)
            {
                if (pr.HasExited == false)
                {
                    pr.Kill();
                    pr.WaitForExit();
                }
            }

            StopButton.Enabled = false;


            Stop = true;
            RunComboFin.Enabled = true;
            TextMpiComm.Enabled = true;
            ButtonChoseTargetXML.Enabled = true;
            ButtonChoseProgram.Enabled = true;
            Run.Enabled = true;
            ButOpenConfList.Enabled = true;
            ChoseDirConfBut.Enabled = true;
            TextBoxChosenDirXML.Enabled = true;
            TextBoxChosenProgram.Enabled = true;
            TextBoxChosenXML.Enabled = true;
            metroButton2.Enabled = true;
            metroButton1.Enabled = true;
            ResultsButton.Enabled = true;
            SearchButton2.Enabled = true;
            for (int i = 0; i < ComboSize; i++)
            {
                if (File.Exists(TempComboXML[i]))
                {
                    File.Delete(TempComboXML[i]);
                }
            }

            for (int i = 0; i < Convert.ToInt32(TextMpiComm.Text); i++)
            {
                if (i == 0)
                {
                    if (File.Exists(Directory.GetCurrentDirectory() + "\\" + "ExaMin.png"))
                    {
                        String TempPic = Directory.GetCurrentDirectory() + "\\" + "ExaMin.png";
                        File.Delete(TempPic);
                    }
                }
                else
                {
                    if (File.Exists(Directory.GetCurrentDirectory() + "\\" + "ExaMin_" + i + ".png"))
                    {
                        String TempPic = Directory.GetCurrentDirectory() + "\\" + "ExaMin_" + i + ".png";
                        File.Delete(TempPic);
                    }
                }

            }
        }
        private void AWFunc(String date,int i, List<string> ActiveConfs, List<string> TempComboXML)
        {
            Run_exp(TempComboXML[i], ActiveConfs[i], gChosenProgram, String.Empty, MpiList[i], false, false);
           
            AddExperiment(date, Results[i], ActiveConfs[i], MpiList[i],false);
        }
        private async void ComboFinRun(int k, List<string> ActiveConfs, List<string> TempComboXML)
        {
            SeriesNumber++;
            StopButton.Enabled = true;
            Stop = false;
            ProgressBarJour.Value = 0;
            String date = DateTime.Now.ToString("(HH-mm-ss)_dd.MM.yy");
            for (int i = 0; i < ComboSize; i++)
            {
                if (!Stop)
                {
                    if (i == ComboSize - 1)
                    {
                        ProgressBarJour.Value = 100;
                    }
                    else
                    {
                        ProgressBarJour.Value += 100 / ComboSize;
                    }

                    comboT = i + 1;
                    String ShortName = new DirectoryInfo(TempComboXML[i]).Name;
                    ProcessTextBox.Text = ShortName;
                   
                    await TaskEx.Run(() => AWFunc(date,i, ActiveConfs, TempComboXML));
                    // AWFunc(i, ActiveConfs, TempComboXML);

                    if (File.Exists(TempComboXML[i]))
                    {
                        File.Delete(TempComboXML[i]);
                    }
                   

                }
                
            }
            UpdateExpJournal();
            if (NMaxTextBox.Text != String.Empty && DeltaTextBox.Text != String.Empty)
            {
                addGraphic(date);
            }
            
            RunComboFin.Enabled = true;
                TextMpiComm.Enabled = true;
                ButtonChoseTargetXML.Enabled = true;
                ButtonChoseProgram.Enabled = true;
                Run.Enabled = true;
                ButOpenConfList.Enabled = true;
                ChoseDirConfBut.Enabled = true;
                TextBoxChosenDirXML.Enabled = true;
                TextBoxChosenProgram.Enabled = true;
                TextBoxChosenXML.Enabled = true;
            metroButton2.Enabled = true;
            metroButton1.Enabled = true;
            ResultsButton.Enabled = true;
            SearchButton2.Enabled = true;
            NMaxTextBox.Clear();
            GridJournal.Focus();
            DeltaTextBox.Clear();

            Results.Clear();
                TempComboXML.Clear();
                ActiveConfs.Clear();
                MpiList.Clear();

                ComboSize = 0;
                comboT = 0;
                MpiCommand = "";
                TempXML = "";

    }
        private void CreateSerTemplName()
        {
           
                string UserFileName = "";
                string[] words = metroComboBox1.SelectedItem.ToString().Split('*');
                if (checkBox3.Checked)
                {
                    UserFileName += words[0];
                    UserFileName += metroTextBox6.Text;
                    UserFileName += words[1] + "_";
                }


            metroTextBox7.Text = UserFileName;
            
           
        }

        private void AddExperiment(String SeriesDate,string res, string _Source_Config_path,bool useMpi,bool SingleStart)
        {
            String Prefix = metroTextBox7.Text;
            String currentPath = Directory.GetCurrentDirectory();
            
            if (!Directory.Exists(Path.Combine(currentPath, "Experiments")))
            {
                 Directory.CreateDirectory(Path.Combine(currentPath, "Experiments"));
                
            }
            if (!Directory.Exists(Path.Combine(currentPath+ "\\Experiments", Prefix+"Series" + SeriesDate + "{" + SeriesNumber + "}")))
            {
                Directory.CreateDirectory(Path.Combine(currentPath + "\\Experiments", Prefix+"Series" + SeriesDate + "{" + SeriesNumber + "}"));

            }
            String ExpNewPath = Directory.GetCurrentDirectory() + "\\Experiments" + "\\"+ Prefix + "Series"+ SeriesDate+"{" + SeriesNumber + "}";
            String date = DateTime.Now.ToString("[HH-mm-ss]_dd.MM.yy")+"_{" + (comboT).ToString() + "}";
            if (!Directory.Exists(Path.Combine(ExpNewPath, date )))
            {
                Directory.CreateDirectory(Path.Combine(ExpNewPath, date));
            }
            
            String OutFileName = date;
            String EXP = ExpNewPath + "\\" + OutFileName ;
            String LogPath = EXP + "\\Log.txt";

            //лог

            System.IO.File.AppendAllText(LogPath, res);

            //путь конфига
            if (_Source_Config_path != "")
            {
                String ConfPath = ExpNewPath + "\\" + OutFileName + "\\ConfPath.txt";
                System.IO.File.AppendAllText(ConfPath, _Source_Config_path);
            }



            //отправка точек в соответствующую папку
           
            string [] fileName = System.IO.Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dat");
            for (int i = 0; i < fileName.Length; i++)
            {
                if (File.Exists(fileName[i]))
                {
                    string Datname = System.IO.Path.GetFileNameWithoutExtension(@fileName[i]);
                     
                    String TempOptim = fileName[i];
                    String OptimLoc = EXP + "\\" + Datname + ".dat";

                    File.Copy(TempOptim, OptimLoc);
                    File.Delete(TempOptim);
                }
            }
           //линии уровня
           if (useMpi)
            {
                for (int i = 0; i < Convert.ToInt32(TextMpiComm.Text); i++)
                {
                    if (i == 0)
                    {
                        if (File.Exists(Directory.GetCurrentDirectory() + "\\" + "ExaMin.png"))
                        {
                            String TempPic = Directory.GetCurrentDirectory() + "\\" + "ExaMin.png";
                            String PicLoc = EXP + "\\" + "ExaMin.png";
                            File.Copy(TempPic, PicLoc);
                            File.Delete(TempPic);
                        }
                    }
                    else
                    {
                        if (File.Exists(Directory.GetCurrentDirectory() + "\\" + "ExaMin_" + i + ".png"))
                        {
                            String TempPic = Directory.GetCurrentDirectory() + "\\" + "ExaMin_" + i + ".png";
                            String PicLoc = EXP + "\\" + "ExaMin_" + i + ".png";
                            File.Copy(TempPic, PicLoc);
                            File.Delete(TempPic);
                        }
                    }
                }
            }
            else
            {
                if (File.Exists(Directory.GetCurrentDirectory() + "\\" + "ExaMin.png"))
                {
                    String TempPic = Directory.GetCurrentDirectory() + "\\" + "ExaMin.png";
                    String PicLoc = EXP + "\\" + "ExaMin.png";
                    File.Copy(TempPic, PicLoc);
                    File.Delete(TempPic);
                }
            }
          

        }

        private void addGraphic(String seriesDate)
        {
            String currentPath = Directory.GetCurrentDirectory();
            String prefix = metroTextBox7.Text;
            String expNewPath = currentPath + "\\Experiments" + "\\" + prefix + "Series" + seriesDate + "{" + SeriesNumber + "}";
            String logFileName = "\\Log.txt";
            if (Directory.Exists(expNewPath))
            {
                List<int> solvedTrialsList = new List<int>();
                int numberOfTrials = 0;
                DirectoryInfo directory = new DirectoryInfo(expNewPath);
                DirectoryInfo[] scopeDirectories = directory.GetDirectories();
                foreach (DirectoryInfo dinf in scopeDirectories)
                {
                    String logPath = dinf.FullName + logFileName;
                    if (File.Exists(logPath))
                    {
                        StreamReader reader = new StreamReader(logPath);
                        bool isSolved = false;
                        while (!reader.EndOfStream)
                        {
                            String line = reader.ReadLine();
                            if (line.Contains(GLOBAL_OPTIMUM_FOUND))
                            {
                                isSolved = true;
                            }
                            if (line.Contains(NUMBER_OF_TRIALS) && isSolved)
                            {
                                int.TryParse(string.Join(String.Empty, reader.ReadLine().Where(ch => char.IsDigit(ch))), out numberOfTrials);
                                solvedTrialsList.Add(numberOfTrials);
                                break;
                            }

                        }
                        reader.Close();
                    }
                }
                int c = 1;
                int delta = Convert.ToInt32(DeltaTextBox.Text);
                int NMax = Convert.ToInt32(NMaxTextBox.Text);
                int solvedCounter = 0;
                Dictionary<int, int> graphicParameters = new Dictionary<int, int>();
                
                while ((c * delta) <= NMax)
                {
                    foreach(int countOfTrials in solvedTrialsList.ToArray())
                    {
                        if ((c * delta) > countOfTrials)
                        {
                            solvedCounter++;
                            solvedTrialsList.Remove(countOfTrials);
                        }
                    }
                    int percent = Convert.ToInt32(((double)solvedCounter / ComboSize) * 100);
                    graphicParameters.Add(c * delta, percent);
                    c++;
                }
                List<int> deltaValues = graphicParameters.Keys.ToList();
                List<int> percentValues = graphicParameters.Values.ToList();

                excelApp = new Excel.Application();
                excelApp.Visible = false;
                Excel.Workbook excelWorkBook = excelApp.Workbooks.Add(Type.Missing);
                Excel.Worksheet excelWorkSheet = (Excel.Worksheet)excelWorkBook.Worksheets.Item[1];
                excelWorkSheet.Name = "Operation Characteristics";
                excelWorkSheet.Cells[1][1] = "Delta";
                excelWorkSheet.Cells[2][1] = "Solved percent";

                for (int i = 0; i < deltaValues.Count; i++)
                {
                    
                    excelWorkSheet.Cells[1][i + 2] = deltaValues[i];
                }

                for (int i = 0; i < percentValues.Count; i++)
                {
                    excelWorkSheet.Cells[2][i + 2] = percentValues[i];
                }


                Excel.Range range = (Excel.Range)excelWorkSheet.Range[excelWorkSheet.Cells[1][2], excelWorkSheet.Cells[2][percentValues.Count + 1]];
                range.Cells.Font.Name = "Arial";
                range.Cells.Font.Size = 12;
                range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                range.HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
                range.EntireColumn.AutoFit();
                range.EntireRow.AutoFit();
                range.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
                range.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                range.Borders.get_Item(Excel.XlBordersIndex.xlInsideHorizontal).LineStyle = Excel.XlLineStyle.xlContinuous;
                range.Borders.get_Item(Excel.XlBordersIndex.xlInsideVertical).LineStyle = Excel.XlLineStyle.xlContinuous;
                range.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;

                /* xlLine */
                excelWorkSheet.Activate();
                Excel.ChartObjects lineChartsObjects = (Excel.ChartObjects)excelWorkSheet.ChartObjects();
                Excel.ChartObject lineChartsObject = lineChartsObjects.Add(200, 10, 350, 250);
                lineChartsObject.Chart.ChartWizard(range, Excel.XlChartType.xlLine, 1, Excel.XlRowCol.xlColumns, 1,
                  0, false, "Operating Characterisitics", "Delta", "Percent", Type.Missing);
                String xlLineName = "\\Line Graphic.bmp";
                lineChartsObject.Chart.Export(expNewPath + xlLineName, Type.Missing, Type.Missing);

                /* xlColumn */
                Excel.ChartObjects columnChartsObjects = (Excel.ChartObjects)excelWorkSheet.ChartObjects();
                Excel.ChartObject columnChartsObject = columnChartsObjects.Add(570, 10, 350, 250);
                columnChartsObject.Chart.ChartWizard(range, Excel.XlChartType.xlColumnClustered, 1, Excel.XlRowCol.xlColumns, 1,
                  0, false, "Operating Characterisitics", "Delta", "Percent", Type.Missing);
                String xlColumnName = "\\Column Graphic.bmp";
                columnChartsObject.Chart.Export(expNewPath + xlColumnName, Type.Missing, Type.Missing);

                /* xlPie */
                //Excel.Range pieRange = (Excel.Range)excelWorkSheet.Range[excelWorkSheet.Cells[2][2], excelWorkSheet.Cells[2][percentValues.Count + 1]];
                //Excel.ChartObjects pieChartsObjects = (Excel.ChartObjects)excelWorkSheet.ChartObjects();
                //Excel.ChartObject pieChartObject = pieChartsObjects.Add(200, 10, 350, 520);
                //pieChartObject.Chart.ChartWizard(pieRange, Excel.XlChartType.xlPie, 1, Excel.XlRowCol.xlColumns, 1, 
                //    0, false, "Operating Characterisitics", "Delta", "Percent", Type.Missing);
                //String xlPieName = "\\Pie Graphic.bmp";
                //pieChartObject.Chart.Export(expNewPath + xlPieName, Type.Missing, Type.Missing);


                String fileName = "\\Operating Characteristics.xlsx";
                excelApp.Application.ActiveWorkbook.SaveAs(expNewPath + fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                                Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing,
                                                                                     Type.Missing, Type.Missing, Type.Missing);
                deallocateObject(excelWorkSheet);
                deallocateObject(excelWorkBook);
                deallocateObject(excelApp);
            }
        }

        private static void deallocateObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal
                   .ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                Console.WriteLine("Exception Occurred while releasing object" + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void ChoseXML()
        {
            if (File.Exists(TempXML))
            {
                File.Delete(TempXML);
            }
            OpenFileDialog OPF = new OpenFileDialog();
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
                gChosenXML = OPF.FileName;
            }
            if (File.Exists(gChosenXML))
            {
                String ConfigName = new DirectoryInfo(gChosenXML).Name;
                TempXML = Directory.GetCurrentDirectory() + "\\" + ConfigName;
                if (!File.Exists(TempXML))
                {
                    File.Copy(gChosenXML, TempXML);
                }
                gTempChosenXML = TempXML;
                TextBoxChosenXML.Text = gChosenXML;
            } else
            {
                MetroFramework.MetroMessageBox.Show(this, "XML не найден.", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ChooseDistributedLaunchFile()
        {
            OpenFileDialog OPF = new OpenFileDialog();
            OPF.InitialDirectory = Directory.GetCurrentDirectory();
            OPF.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                gChosenDistributedFile = OPF.FileName;
            }

            if (File.Exists(gChosenDistributedFile))
            {
                TextBoxChosenDistributedFile.Text = gChosenDistributedFile;
            } else
            {
                MetroFramework.MetroMessageBox.Show(this, "Файл распределенного запуска не найден.", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void ChoseDirXML()
        {
            if (File.Exists(TempXML))
            {
                File.Delete(TempXML);
            }

            FolderBrowserDialog FBD = new FolderBrowserDialog();
           
            if (Directory.Exists(Directory.GetCurrentDirectory() + "\\Configurations"))
            {
                FBD.SelectedPath = Directory.GetCurrentDirectory() + "\\Configurations";
            }
            else
            {
                FBD.SelectedPath = Directory.GetCurrentDirectory();
            }
            if (FBD.ShowDialog() == DialogResult.OK)
            {
                gChosenDirXML = FBD.SelectedPath;
            }
            if (Directory.Exists(gChosenDirXML))
            {
                String ConfigDirName = new DirectoryInfo(gChosenDirXML).Name;
            }
            if (Directory.Exists(gChosenDirXML))
            {
                TextBoxChosenDirXML.Text = gChosenDirXML;
            }
            else
            {
                if (!Directory.Exists(gChosenDirXML))
                {
                    MetroFramework.MetroMessageBox.Show(this, "XML не найден.", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        public void ReadConfsInDir(String DirPath)
        {
            if (Directory.Exists(DirPath))
            {
                ConfigList.Rows.Clear();
                
                String ConfigName = "";
                String[] files = Directory.GetFiles(DirPath, "*.xml");

                    for (int i = 0; i < files.Length; i++)
                    {
                        ConfigList.Rows.Add();
                        ConfigName = new DirectoryInfo(files[i]).Name;
                        ConfigList.Rows[i].Cells[0].Value = ConfigName;
                        ConfigList.Rows[i].Cells[1].Value = files[i];
                        ConfigList.Rows[i].Cells[2].Value = 0;
                        ConfigList.Rows[i].Cells[3].Value = 0;
                }
                NMaxTextBox.Focus();
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Указанная директория не найдена.", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }
        private void SettingsRun(DataGridViewCellEventArgs _e)
        {
            string ConfigFullName = ConfigList.Rows[_e.RowIndex].Cells[1].Value.ToString();
        
            using (Generator Settings = new Generator(_e, ConfigFullName))
            {
                 Settings.ShowDialog();
               // metroTabControl1.SelectTab(Generate);
            }
        }
        private void ChoseProgram()
        {
            OpenFileDialog OPF = new OpenFileDialog();
            if (OPF.ShowDialog() == DialogResult.OK)
            {
                gChosenProgram = OPF.FileName;
            }
            if (File.Exists(gChosenProgram))
            {
                  TextBoxChosenProgram.Text = gChosenProgram;
            }
            else
            {
                if (gChosenProgram != "")
                {
                    MetroFramework.MetroMessageBox.Show(this, "EXE не найден.", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void UpdateExpJournal()
        {
            string expPath = Directory.GetCurrentDirectory() + "\\Experiments";
            if (Directory.Exists(expPath))
            {
                GridJournal.Rows.Clear();
                string SeriesPath = Directory.GetCurrentDirectory() + "\\Experiments";
                if (Directory.Exists(SeriesPath) && Directory.Exists(Directory.GetCurrentDirectory() + "\\Configurations"))
                {
                    DirectoryInfo dir = new DirectoryInfo(SeriesPath);
                    DirectoryInfo[] dirs = dir.GetDirectories();
                    foreach (DirectoryInfo f in dirs)
                    {
                        DirectoryInfo directory = new DirectoryInfo(f.FullName);
                        DirectoryInfo[] scopeDirectories = directory.GetDirectories();
                        foreach (DirectoryInfo dinf in scopeDirectories)
                        {
                            String briefDescriptionPath = dinf.FullName + "\\BriefDescription.txt";
                            if (File.Exists(briefDescriptionPath))
                            {
                                String briefDescription = File.ReadAllText(briefDescriptionPath);
                                if (checkIfRowIsUnique(f.CreationTime.ToString()))
                                    GridJournal.Rows.Add(f.CreationTime, f.FullName, f.Name, briefDescription);
                            } else
                            {
                                if (checkIfRowIsUnique(f.CreationTime.ToString()))
                                    GridJournal.Rows.Add(f.CreationTime, f.FullName, f.Name);
                            }
                        }
                    }

                    // GridJournal.CurrentCell = GridJournal[0, 0];
                    if (GridJournal.Rows.Count != 0)
                    {
                        GridJournal.Rows[0].Cells[0].Selected = false;
                    }
                }
            }
        }

        private Boolean checkIfRowIsUnique(String creationTime)
        {
            for (int i = 0; i < GridJournal.RowCount; i++)
            {
                if (GridJournal.Rows[i].Cells[0].Value.ToString().Equals(creationTime))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
