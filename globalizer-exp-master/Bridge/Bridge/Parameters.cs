using Bridge;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Bridge
{
    public partial class Parameters : MetroFramework.Forms.MetroForm
    {
        private String commandLine = "";
        private bool _openedFromParameters = false;
        private int comboT = 0;
        private DataGridViewCellMouseEventArgs e = null;
        private delegateRequestParams dataParams;
        public Parameters(delegateRequestParams _dataParams)
        {
            InitializeComponent();
            this.dataParams = _dataParams;
            _openedFromParameters = true;
            Сlassification classification = new Сlassification();
            InitTable(SolverTable, classification.Solver);
            InitTable(MethodTable, classification.Method);
            InitTable(ParallelTable, classification.Parallel);
            InitTable(TaskTable, classification.Task);
            InitTable(OtherTable, classification.Other);
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
            _InfoTable.CurrentCell = _InfoTable[0, 0];
            _InfoTable.Rows[0].Cells[0].Selected = false;
        }

        private void Return_Click(object sender, EventArgs e)
        {   
            commandLine = commandLine + getParametersFromTable(SolverTable);
            commandLine = commandLine + getParametersFromTable(MethodTable);
            commandLine = commandLine + getParametersFromTable(ParallelTable);
            commandLine = commandLine + getParametersFromTable(TaskTable);
            commandLine = commandLine + getParametersFromTable(OtherTable);
            
            if (!commandLine.Equals(""))
            {
                this.dataParams(commandLine, _openedFromParameters);
                this.Close();
            } else MetroFramework.MetroMessageBox.Show(this, "Ни один параметр не задан", "Оповещение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private String getParametersFromTable(MetroFramework.Controls.MetroGrid metroGridTable)
        {
            String commandLine = "";
            for (int i = 0; i < metroGridTable.Rows.Count; i++)
            {
                if (metroGridTable.Rows[i].Cells[4].Value != null)
                {
                    for (int j = 0; j < metroGridTable.ColumnCount; j += 4)
                    { 
                        commandLine += metroGridTable.Rows[i].Cells[j].Value.ToString();
                        commandLine += " ";
                    }
                }

            }

            return commandLine;
        }

    }
}
