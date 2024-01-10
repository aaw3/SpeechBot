using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpeechBot
{
    public partial class Form2 : Form
    {
        Form1 form1;
        public Form2(Form1 f1Instance)
        {
            InitializeComponent();
            form1 = f1Instance;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new Size(560, 380);

            dataGridView1.Columns.Add("Command Type", "Command Type");
            dataGridView1.Columns.Add("Command Name", "Command Name");
            dataGridView1.Columns.Add("Command Value", "Command Value");
            dataGridView1.Columns.Add("Edit Command", "Edit Command");
            dataGridView1.Columns.Add("Delete Command", "Delete Command");

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            }

            //dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells; //don't know what this does (or if it affects anything)
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[3].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 16f);
            dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[4].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 16f);
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;

            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.ReadOnly = true;


            //dataGridView1.Rows.RemoveAt(0);
            dataGridView1.AllowUserToAddRows = false;
        }

        EditData ed;
        int currentlyEditing = -1;
        int newSteamID;
        //I don't re-add the tokens when the name is changed because the order doesn't really matter if we are just saving the dictionaries.
        private async void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0 || e.ColumnIndex > dataGridView1.Columns.Count - 1 || e.RowIndex > dataGridView1.Rows.Count - 1)
                return;

            if (e.ColumnIndex == 3)
            {
                currentlyEditing = e.RowIndex;
                string cmdName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                string cmdData = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                ed = await InputBox.Instance.Show("Enter new Command Name or Data", "Edit Menu", dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(), 0, dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(), 0);

                currentlyEditing = -1;

                if (ed == null)
                {
                    return; //Edit cancelled
                }

                string newName = ed.Name.ToLower();
                string newData = ed.Data;
                switch ((Form1.Category)Enum.Parse(typeof(Form1.Category), dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()))
                {
                    

                    case Form1.Category.SteamGame:

                        if (cmdName != newName && cmdData == newData)
                        {
                            form1.steamGameList.Remove(cmdName);
                            form1.steamGameList.Add(newName);

                            form1.steamGameDict.Add(newName, form1.steamGameDict[cmdName]);
                            form1.steamGameDict.Remove(cmdName);
                            dataGridView1.Rows[e.RowIndex].Cells[1].Value = newName;
                        }
                        else if (cmdData != newData)
                        {
                            if (int.TryParse(newData, out newSteamID))
                            {
                                if (cmdName != newName)
                                {
                                    form1.steamGameList.Remove(cmdName);
                                    form1.steamGameList.Add(newName);
                                }

                                MessageBox.Show("After Parse: " + newSteamID);

                                form1.steamGameDict.Remove(cmdName);
                                form1.steamGameDict.Add(newName, newSteamID);

                                dataGridView1.Rows[e.RowIndex].Cells[1].Value = newName;
                                dataGridView1.Rows[e.RowIndex].Cells[2].Value = newData;
                            }
                            else
                            {
                                form1.DisplayError("Text is not an [Integer]");
                                return;
                            }
                        }

                        break;

                    case Form1.Category.App:
                        if (cmdName != newName && cmdData == newData)
                        {
                            form1.appList.Remove(cmdName);
                            form1.appList.Add(newName);

                            form1.appDict.Add(newName, form1.appDict[cmdName]);
                            form1.appDict.Remove(cmdName);

                            dataGridView1.Rows[e.RowIndex].Cells[1].Value = newName;
                        }
                        else if (cmdData != newData)
                        {
                            if (File.Exists(newData))
                            {
                                if (cmdName != newName)
                                {
                                    form1.appList.Remove(cmdName);
                                    form1.appList.Add(newName);

                                }
                                form1.appDict.Remove(cmdName);
                                form1.appDict.Add(newName, newData);

                                dataGridView1.Rows[e.RowIndex].Cells[1].Value = newName;
                                dataGridView1.Rows[e.RowIndex].Cells[2].Value = newData;
                            }
                            else
                            {
                                form1.DisplayError("Text does not point to a [File Location]");
                                return;
                            }
                        }
                        
                        break;

                    case Form1.Category.Question: //not implemented
                        break;

                    case Form1.Category.Command: //not implemented
                        break;

                    case Form1.Category.Script:

                        if (cmdName != newName && cmdData == newData)
                        {
                            form1.scriptList.Remove(cmdName);
                            form1.scriptList.Add(newName);

                            form1.scriptDict.Add(newName, form1.scriptDict[cmdName]);
                            form1.scriptDict.Remove(cmdName);

                            form1.codeDict.Add(newName, form1.codeDict[cmdName]);
                            form1.codeDict.Remove(cmdName);

                            dataGridView1.Rows[e.RowIndex].Cells[1].Value = newName;
                        }
                        else if (cmdData != newData) //name is getting changed anyways so two seperate statements is redundant
                        {
                            string randomName = newName.Replace(" ", "_") + Form1.r.Next(10000, 100000);
                            MessageBox.Show(randomName);
                            CompilerResults results = form1.CompileCode(newData, "public class " + randomName);

                            if (results == null)
                                return;

                            if (cmdName != newName)
                            {
                                form1.scriptList.Remove(cmdName);
                                form1.scriptList.Add(newName);
                            }

                            form1.scriptDict.Remove(cmdName);
                            form1.scriptDict.Add(newName, form1.MakeScriptObject(results, randomName));

                            form1.codeDict.Remove(cmdName);
                            form1.codeDict.Add(newName, newData);

                            dataGridView1.Rows[e.RowIndex].Cells[1].Value = newName;
                            dataGridView1.Rows[e.RowIndex].Cells[2].Value = newData;
                        }

                        #region note
                        //if (cmdData != ed.Data && cmdName == ed.Name) //two seperate ways to do so, but is redundant (can do both in one)
                        //{
                        //    CompilerResults results = form1.CompileCode(ed.Data);

                        //    if (results == null)
                        //        return;

                        //    form1.scriptDict.Remove(cmdName);
                        //    form1.scriptDict.Add(cmdName, form1.MakeScriptDynamic(results));
                        //}

                        //if (cmdData != ed.Data && cmdName != ed.Name)
                        //{
                        //    CompilerResults results = form1.CompileCode(ed.Data);

                        //    if (results == null)
                        //        return;

                        //    form1.scriptDict.Remove(cmdName);
                        //    form1.scriptDict.Add(ed.Name, form1.MakeScriptDynamic(results));
                        //}
                        #endregion

                        break;

                }
                form1.ReloadCommands();


            }
            else if (e.ColumnIndex == 4)
            {
                if (MessageBox.Show($"Are you sure you want to delete {dataGridView1.Rows[e.RowIndex].Cells[0].Value}: \"{dataGridView1.Rows[e.RowIndex].Cells[1].Value}\" ?", "Command Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string cmdName = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

                    int CategoryNumber = (int)Enum.Parse(typeof(Form1.Category), dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());


                    form1.categoryList[CategoryNumber].Remove(cmdName);

                    form1.dictArray[CategoryNumber].Remove(cmdName);

                    if (CategoryNumber == 4)
                    {
                        form1.codeDict.Remove(cmdName);
                    }

                    form1.ReloadCommands();

                    dataGridView1.Rows.RemoveAt(e.RowIndex);

                    if (e.RowIndex == currentlyEditing)
                    {
                        InputBox.Instance.Cancel();
                        currentlyEditing = -1;
                    }
                }
            }
        }

        SaveData sd = new SaveData();
        public void Save(bool updateSavedTime = true)
        {
            sd.SteamGameList = form1.steamGameDict.Keys.ToList().GetRange(form1.steamStartIndex, form1.steamGameDict.Count - form1.steamStartIndex);
            sd.SteamGameToken = form1.steamGameDict.Values.ToList().GetRange(form1.steamStartIndex, form1.steamGameDict.Count - form1.steamStartIndex);

            sd.AppList = form1.appDict.Keys.ToList().GetRange(form1.appStartIndex, form1.appDict.Count - form1.appStartIndex);
            sd.AppToken = form1.appDict.Values.ToList().GetRange(form1.appStartIndex, form1.appDict.Count - form1.appStartIndex);

            sd.ScriptList = form1.codeDict.Keys.ToList();
            sd.CodeToken = form1.codeDict.Values.ToList();

            sd.Serialize<SaveData>(form1.saveLocation);

            if (updateSavedTime)
            {
                form1.SaveTimeLabel.Text = $"Last Saved:{Environment.NewLine} {DateTime.Now.ToString("MM'-'dd'-'yyyy'@'HH':'mm':'ss")}";
            }
        }

        private void dataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }

    public class SaveData
    {
        public List<string> SteamGameList;
        public List<int> SteamGameToken;

        public List<string> AppList;
        public List<string> AppToken;

        public List<string> ScriptList;
        public List<string> CodeToken;
    }


    public class EditData
    {
        public string Name;
        public string Data;
    }
}
