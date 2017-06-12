using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace LogAnalysis
{
    public partial class Form1 : Form
    {
        [Serializable]
        public class LogInfo
        {
            public int Index;
            public string Data;
            public string time;
            public uint TimeSpan;
            public uint TraceSpan;
            public string Mode;
            public string OperaInfo;
            public string remark;
        }

        [Serializable]
        public class SaveInfo
        {
            public string Color;
            public string OperaInfo;
            public string remark;
        }

        public Form1()
        {
            InitializeComponent();
            var ShowList = new List<List<LogInfo>>();
            var ShowDic = new List<Dictionary<int, int>>();
            var CusTomList = new List<SaveInfo>();
            var ShowListIndex = 0;
            var StartTree = 0;
            var SavePath = "";
            var LogPath = "";
            var Custompath = new FileInfo(GetType().Assembly.Location).DirectoryName + @"\" + "custom.xml";
            Action<List<LogInfo>> ShowToDataGridView = (TempInfo) =>
            {
                if (TempInfo.Count != 0)
                {
                    var table = new DataTable();
                    table.Columns.Add("序号");//0
                    table.Columns.Add("时间");//1
                    table.Columns.Add("模式");//2
                    table.Columns.Add("时间间隔");//3
                    table.Columns.Add("Trace间隔");//4
                    table.Columns.Add("操作");//5
                    table.Columns.Add("备注");//5
                    foreach (var Tempinfo in TempInfo)
                    {
                        table.Rows.Add(Tempinfo.Index, Tempinfo.time, Tempinfo.Mode, Tempinfo.TimeSpan + "ms", Tempinfo.TraceSpan == 0 ? "" : Tempinfo.TraceSpan + "ms", Tempinfo.OperaInfo, Tempinfo.remark);
                    }
                    BeginInvoke(new MethodInvoker(() =>
                    {
                        LogData.DataSource = table;
                        //LogData.Rows[0].DefaultCellStyle.BackColor = Color.Red;
                        LogData.Columns[0].Width = 50;
                        LogData.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                        LogData.Columns[0].ReadOnly = true;
                        LogData.Columns[1].Width = 100;
                        LogData.Columns[1].ReadOnly = true;
                        LogData.Columns[2].Width = 60;
                        LogData.Columns[2].ReadOnly = true;
                        LogData.Columns[3].Width = 60;
                        LogData.Columns[3].ReadOnly = true;
                        LogData.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                        LogData.Columns[4].Width = 50;
                        LogData.Columns[4].ReadOnly = true;
                        LogData.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                        LogData.Columns[5].Width = 280;
                        LogData.Columns[5].ReadOnly = true;
                        LogData.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                        LogData.Columns[6].Width = 180;
                        LogData.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }));
                }
            };
            Action CheckColorMsecStatus = () =>
            {
                var redindex = int.Parse(LogData.Rows[0].Cells[0].Value.ToString());
                var greenindex = -1;
                TimeSpan spantime;
                for (int i = 1; i < LogData.Rows.Count; i++)
                {
                    LogData.Rows[i].Cells[7].Value = "";
                    LogData.Rows[i].Cells[8].Value = "";
                    if (LogData.Rows[i].Cells[7].Style.BackColor == Color.Red)
                    {
                        var tempindex = int.Parse(LogData.Rows[i].Cells[0].Value.ToString());
                        spantime = Convert.ToDateTime(ShowList[ShowListIndex][tempindex].time) - Convert.ToDateTime(ShowList[ShowListIndex][redindex].time);
                        LogData.Rows[i].Cells[7].Value = spantime.TotalMilliseconds + "ms";

                        if (greenindex != -1)
                        {
                            spantime = Convert.ToDateTime(ShowList[ShowListIndex][tempindex].time) - Convert.ToDateTime(ShowList[ShowListIndex][greenindex].time);
                            LogData.Rows[i].Cells[8].Value = spantime.TotalMilliseconds + "ms";
                        }
                        greenindex = -1;
                        redindex = tempindex;
                    }
                    if (LogData.Rows[i].Cells[8].Style.BackColor == Color.MediumSpringGreen)
                    {
                        var tempindex = int.Parse(LogData.Rows[i].Cells[0].Value.ToString());
                        if (greenindex == -1)
                        {
                            spantime = Convert.ToDateTime(ShowList[ShowListIndex][tempindex].time) - Convert.ToDateTime(ShowList[ShowListIndex][redindex].time);
                        }
                        else
                        {
                            spantime = Convert.ToDateTime(ShowList[ShowListIndex][tempindex].time) - Convert.ToDateTime(ShowList[ShowListIndex][greenindex].time);
                        }
                        LogData.Rows[i].Cells[8].Value = spantime.TotalMilliseconds + "ms";
                        greenindex = tempindex;
                    }
                }
                LogData.Update();
            };
            Action LoadCustomSetToData = () =>
            {
                if (LogData.Rows.Count == 0) return;
                var RowIndex = 0;
                foreach (var item in CusTomList)
                {
                    for (int i = RowIndex; i < LogData.Rows.Count; i++)
                    {
                        var String = new StringCompute();
                        String.SpeedyCompute(item.OperaInfo, LogData.Rows[i].Cells[5].Value.ToString());
                        var res = (float)String.ComputeResult.Rate;
                        if (res >= 0.8)
                        {
                            var color = Color.FromName(item.Color);
                            if (color == Color.Red)
                            {
                                LogData.Rows[i].Cells[7].Style.BackColor = color;
                            }
                            else
                            {
                                LogData.Rows[i].Cells[8].Style.BackColor = color;
                            }
                            LogData.Rows[i].Cells[6].Value = item.remark;
                            RowIndex = i + 1;
                            break;
                        }
                    }
                }
                CheckColorMsecStatus();
            };
            Action<int, int> SingleShowToDataGridView = (startindex, endindex) =>
               {
                   var table = new DataTable();
                   table.Columns.Add("序号");//0
                   table.Columns.Add("时间");//1
                   table.Columns.Add("模式");//2
                   table.Columns.Add("时间间隔");//3
                   table.Columns.Add("Trace间隔");//4
                   table.Columns.Add("操作");//5
                   table.Columns.Add("备注");//5
                   if (AnalysisCheck.Checked)
                   {
                       table.Columns.Add("检查工序");
                       table.Columns.Add("检查工作");
                   }
                   for (; startindex <= endindex; startindex++)
                   {
                       var Tempinfo = ShowList[ShowListIndex][startindex];
                       table.Rows.Add(Tempinfo.Index, Tempinfo.time, Tempinfo.Mode, Tempinfo.TimeSpan + "ms", Tempinfo.TraceSpan == 0 ? "" : Tempinfo.TraceSpan + "ms", Tempinfo.OperaInfo, Tempinfo.remark);
                   }
                   BeginInvoke(new MethodInvoker(() =>
                   {
                       LogData.DataSource = table;
                       LogData.Columns[0].Width = 50;
                       LogData.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                       LogData.Columns[0].ReadOnly = true;
                       LogData.Columns[1].Width = 100;
                       LogData.Columns[1].ReadOnly = true;
                       LogData.Columns[2].Width = 60;
                       LogData.Columns[2].ReadOnly = true;
                       LogData.Columns[3].Width = 60;
                       LogData.Columns[3].ReadOnly = true;
                       LogData.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
                       LogData.Columns[4].Width = 50;
                       LogData.Columns[4].ReadOnly = true;
                       LogData.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                       LogData.Columns[5].Width = 280;
                       LogData.Columns[5].ReadOnly = true;
                       LogData.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                       LogData.Columns[6].Width = 180;
                       LogData.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                       if (AnalysisCheck.Checked)
                       {
                           LogData.Columns[7].ReadOnly = true;
                           LogData.Columns[7].Width = 60;
                           LogData.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;
                           LogData.Columns[8].ReadOnly = true;
                           LogData.Columns[8].Width = 60;
                           LogData.Columns[8].SortMode = DataGridViewColumnSortMode.NotSortable;
                           LogData.Rows[0].Cells[7].Style.BackColor = Color.Red;
                           LogData.Rows[LogData.Rows.Count - 1].Cells[7].Style.BackColor = Color.Red;
                           if (CusTomList.Count != 0) LoadCustomSetToData();
                       }
                   }));
               };
            Action<List<LogInfo>> ShowToTree = (GetInfo) =>
            {
                if (GetInfo.Count != 0)
                {
                    var StartDic = new Dictionary<int, int>();//记录的是这次开始和上次开始之间的序列，
                    var laststartindex = -1;
                    TreeNode Root = new TreeNode();
                    TreeNode DepthStart = new TreeNode();
                    TreeNode DepthTrace = new TreeNode();
                    Root.Text = ShowList.IndexOf(GetInfo) + ":" + GetInfo[0].Data;
                    foreach (var item in GetInfo)
                    {
                        if (item.Mode == "Trace" && item.OperaInfo.StartsWith("Start Inspection"))
                        {
                            if (item.OperaInfo == "Start Inspection")
                            {
                                DepthStart = new TreeNode();
                                DepthStart.Text = item.Index + ":开始检测";
                                Root.Nodes.Add(DepthStart);
                                laststartindex = item.Index;
                                continue;
                            }
                            else if (item.OperaInfo.EndsWith("[msec]"))
                            {
                                StartDic.Add(laststartindex, item.Index);
                            }
                        }
                        if (DepthStart.Text != "")
                        {
                            if (item.Mode == "Trace")
                            {
                                DepthTrace = new TreeNode();
                                DepthTrace.Text = string.IsNullOrWhiteSpace(item.remark) ? item.Index + ":" + item.OperaInfo : item.Index + ":" + item.remark;
                                DepthStart.Nodes.Add(DepthTrace);
                                continue;
                            }
                            else
                            {
                                if (DepthTrace.Text != "")
                                {
                                    if (item.Mode != "") DepthTrace.Nodes.Add(string.IsNullOrWhiteSpace(item.remark) ? item.Index + ":" + item.Mode : item.Index + ":" + item.remark);
                                    else DepthTrace.Nodes.Add(string.IsNullOrWhiteSpace(item.remark) ? item.Index + ":" + item.OperaInfo : item.Index + ":" + item.remark);
                                }
                            }
                        }
                    }
                    ShowDic.Add(StartDic);
                    BeginInvoke(new MethodInvoker(() =>
                    {
                        Logtree.Nodes.Add(Root);
                    }));
                }
            };
            Action<Stream> StartRead = (File) =>
             {
                 using (TextReader sr = new StreamReader(File))
                 {
                     int ProgressBar = 0;
                     new Task(() =>
                     {
                         BeginInvoke(new MethodInvoker(() =>
                         {
                             progressBar.Value = 0;
                             progressBar.Maximum = (int)File.Length;
                         }));
                         do
                         {
                             BeginInvoke(new MethodInvoker(() =>
                             {
                                 progressBar.Value = (int)File.Position;
                             }));
                             Thread.Sleep(10);
                         } while (progressBar.Value != progressBar.Maximum);
                     }).Start();
                     var GetInfo = new List<LogInfo>();
                     var Regex = new Regex(@"(?<key>\d{4}-\d{2}-\d{2}).(?<key2>\d{2}:\d{2}:\d{2},\d{3})..(?<key3>(?<=\[).*?(?=\]))..(?<key4>.*)", RegexOptions.Compiled);
                     var Regex2 = new Regex(@"(?<key>\d{4}-\d{2}-\d{2}).(?<key2>\d{2}:\d{2}:\d{2},\d{3})..(?<key4>.*)", RegexOptions.Compiled);
                     LogInfo LastTrace = null;
                     LogInfo LastOpera = null;
                     while (sr.Peek() != -1)
                     {
                         var Text = sr.ReadLine();
                         GroupCollection MatchGroup = Regex.Match(Text).Groups;
                         if (MatchGroup.Count == 1)
                         {
                             MatchGroup = Regex2.Match(Text).Groups;
                             if (MatchGroup.Count == 1)
                             {
                                 continue;
                             }
                         }
                         var TempInfo = new LogInfo();
                         TempInfo.Index = ProgressBar;
                         TempInfo.Data = MatchGroup["key"].Value;
                         TempInfo.time = MatchGroup["key2"].Value.Replace(",", ".");
                         TempInfo.Mode = MatchGroup["key3"].Value;
                         TempInfo.OperaInfo = MatchGroup["key4"].Value;
                         if (TempInfo.Mode == "Trace")
                         {
                             if (LastTrace != null)
                             {
                                 TempInfo.TraceSpan = (uint)(Convert.ToDateTime(TempInfo.time) - Convert.ToDateTime(LastTrace.time)).TotalMilliseconds;
                                 TempInfo.TimeSpan = (uint)(Convert.ToDateTime(TempInfo.time) - Convert.ToDateTime(LastOpera.time)).TotalMilliseconds;
                                 LastTrace = TempInfo;
                             }
                             else
                             {
                                 LastTrace = TempInfo;
                             }
                         }
                         else
                         {
                             if (LastOpera != null)
                             {
                                 TempInfo.TimeSpan = (uint)(Convert.ToDateTime(TempInfo.time) - Convert.ToDateTime(LastOpera.time)).TotalMilliseconds;
                                 LastOpera = TempInfo;
                             }
                         }
                         LastOpera = TempInfo;
                         GetInfo.Add(TempInfo);
                         Interlocked.Increment(ref ProgressBar);
                     }
                     Thread.Sleep(100);
                     ShowList.Add(GetInfo);
                     var Mission = new Task(() => ShowToTree(GetInfo));
                     Mission.ContinueWith((obj) =>
                     {
                         if (AnalysisCheck.Checked)
                         {
                             StartTree = ShowDic[ShowListIndex].Keys.First();
                             SingleShowToDataGridView(StartTree, ShowDic[ShowListIndex][StartTree]);
                         }
                     });
                     Mission.Start();
                     if (!AnalysisCheck.Checked) new Task(() => ShowToDataGridView(GetInfo)).Start();
                     ShowListIndex = ShowList.Count - 1;
                 }
             };
            Action Save = () =>
            {
                using (var SaveDialog = new SaveFileDialog())
                {
                    SaveDialog.Title = "保存列表";
                    SaveDialog.Filter = "dat文件(*.dat)|*.dat";
                    SaveDialog.AddExtension = true;
                    if (SavePath == "" || !new FileInfo(SavePath).Exists)
                    {
                        if (LogPath != "")
                        {
                            var FileInfo = new FileInfo(LogPath);
                            SaveDialog.FileName = FileInfo.Name.Split('.')[0];
                        }
                        SaveDialog.ShowDialog();
                        if (SaveDialog.FileName == "") return;
                        SavePath = SaveDialog.FileName;
                    }
                    if (SavePath == "") return;
                    new Task(() =>
                    {
                        using (Stream Filestream = new FileStream(SavePath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            XmlSerializer ser = new XmlSerializer(typeof(List<List<LogInfo>>));
                            ser.Serialize(Filestream, ShowList);
                        }
                    }).Start();
                }
            };
            Action<List<SaveInfo>> Savecustom = (list) =>
            {
                new Task(() =>
                {
                    XmlSerializer ser = new XmlSerializer(typeof(List<SaveInfo>));
                    using (Stream Filestream = new FileStream(Custompath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        ser.Serialize(Filestream, list);
                    }
                }).Start();
            };
            Action StartChangeShow = () =>
            {
                ShowToDataGridView(ShowList[ShowListIndex]);
                BeginInvoke(new MethodInvoker(() => { LogData.CurrentCell = LogData.Rows[StartTree].Cells[0]; }));
            };
            Func<TreeNode, int> FindStartTree = Tree =>
            {
                var index = 0;
                if (Tree.Parent != null)
                {
                    while (true)
                    {
                        if (Tree.Text.EndsWith("开始检测"))
                        {
                            int.TryParse(Tree.Text.Split(':')[0], out index);
                            break;
                        }
                        else if (Tree == null)
                        {
                            break;
                        }
                        else
                        {
                            Tree = Tree.Parent;
                        }
                    }
                }
                return index;
            };
            Func<TreeNode, int> FindRootTree = Tree =>
            {
                var index = 0;
                if (Tree.Parent != null)
                {
                    while (true)
                    {
                        if (Tree.Parent == null)
                        {
                            int.TryParse(Tree.Text.Split(':')[0], out index);
                            break;
                        }
                        else
                        {
                            Tree = Tree.Parent;
                        }
                    }
                }
                else
                {
                    int.TryParse(Tree.Text.Split(':')[0], out index);
                }
                return index;
            };
            DataGridViewCellEventHandler setcustommouseopera = (s, o) =>
            {
                if (LogData.CurrentCell.ColumnIndex == 7 || LogData.CurrentCell.ColumnIndex == 8)
                {
                    if (LogData.CurrentCell.RowIndex != 0 && LogData.CurrentCell.RowIndex != LogData.Rows.Count - 1)
                    {
                        if (LogData.CurrentCell.ColumnIndex == 7)
                        {
                            if (LogData.CurrentCell.Style.BackColor == Color.Red) LogData.CurrentCell.Style.BackColor = Color.Empty;
                            else
                            {
                                LogData.CurrentCell.Style.BackColor = Color.Red;
                                LogData.Rows[LogData.CurrentCell.RowIndex].Cells[8].Style.BackColor = Color.Empty;
                            }
                        }
                        else if (LogData.Rows[LogData.CurrentCell.RowIndex].Cells[7].Style.BackColor != Color.Red)
                        {
                            if (LogData.CurrentCell.Style.BackColor == Color.MediumSpringGreen) LogData.CurrentCell.Style.BackColor = Color.Empty;
                            else LogData.CurrentCell.Style.BackColor = Color.MediumSpringGreen;
                        }
                        CheckColorMsecStatus();
                        LogData.CurrentCell = LogData.Rows[LogData.CurrentCell.RowIndex].Cells[6];
                    }
                }
            };
            LoadLogfile.Click += delegate
            {
                using (OpenFileDialog open = new OpenFileDialog())
                {
                    open.Title = "载入Log日志";
                    open.Filter = "所有文件(*.*)|*.*|Log文件(*.Log)|*.log|文本文档(*.txt)|*.txt";
                    open.ShowDialog();
                    if (open.FileName != "")
                    {
                        LogPath = open.FileName;
                        SaveButton.Enabled = true;
                        new Task(() => { StartRead(open.OpenFile()); }).Start();
                    }
                }
            };
            Logtree.AfterSelect += (s, e) =>
            {
                try
                {
                    var RootTree = FindRootTree(Logtree.SelectedNode);
                    var startTree = FindStartTree(Logtree.SelectedNode);
                    if (!AnalysisCheck.Checked)
                    {
                        if (RootTree != ShowListIndex)
                        {
                            ShowListIndex = RootTree;
                            StartTree = startTree;
                            new Task(() => StartChangeShow()).Start();
                            return;
                        }
                        else if (StartTree != startTree)
                        {
                            int.TryParse(Logtree.SelectedNode.Text.Split(':')[0], out int selectindex);
                            LogData.CurrentCell = LogData.Rows[selectindex].Cells[0];
                        }
                        {
                            int.TryParse(Logtree.SelectedNode.Text.Split(':')[0], out int selectindex);
                            LogData.CurrentCell = LogData.Rows[selectindex].Cells[0];
                        }
                    }
                    else
                    {
                        if (RootTree != ShowListIndex || StartTree != startTree)
                        {
                            ShowListIndex = RootTree;
                            StartTree = startTree;
                            SingleShowToDataGridView(StartTree, (from T in ShowDic[ShowListIndex] where T.Key == StartTree select T.Value).FirstOrDefault());
                        }
                        {
                            int.TryParse(Logtree.SelectedNode.Text.Split(':')[0], out int selectindex);
                            if (AnalysisCheck.Checked)
                            {
                                for (int i = 0; i < LogData.Rows.Count; i++)
                                {
                                    if (LogData.Rows[i].Cells[0].Value.ToString() == selectindex.ToString())
                                    {
                                        LogData.CurrentCell = LogData.Rows[i].Cells[0];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            };
            LogData.CellEndEdit += (s, e) =>
            {
                SaveButton.Enabled = true;
                int.TryParse(LogData.Rows[e.RowIndex].Cells[0].Value.ToString(), out int Index);
                if (AnalysisCheck.Checked)
                {
                    for (int i = 0; i < LogData.Rows.Count; i++)
                    {
                        if (LogData.Rows[i].Cells[0].Value.ToString() == Index.ToString())
                        {
                            Index = i;
                        }
                    }
                }
                ShowList[ShowListIndex][Index].remark = LogData.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            };
            SaveButton.Click += delegate
            {
                Save();
                SaveButton.Enabled = false;
            };
            LoadListButton.Click += delegate
              {
                  using (OpenFileDialog open = new OpenFileDialog())
                  {
                      open.Title = "载入列表";
                      open.Filter = "dat文件(*.dat)|*.dat";
                      open.AddExtension = true;
                      open.ShowDialog();
                      if (open.FileName == "") return;
                      Logtree.Nodes.Clear();
                      LoadListButton.Text = "载入中";
                      LoadLogfile.Enabled = false;
                      LoadListButton.Enabled = false;
                      SaveButton.Enabled = false;
                      SavePath = open.FileName;
                      new Task(() =>
                           {
                               using (Stream stream = new FileStream(SavePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                               {
                                   XmlSerializer ser = new XmlSerializer(typeof(List<List<LogInfo>>));
                                   ShowList = new List<List<LogInfo>>((List<List<LogInfo>>)ser.Deserialize(stream));
                                   if (ShowList.Count != 0)
                                   {
                                       foreach (var GetInfo in ShowList)
                                       {
                                           ShowToTree(GetInfo);
                                       }
                                       if (!AnalysisCheck.Checked) new Task(() => ShowToDataGridView(ShowList[0])).Start();
                                       else
                                       {
                                           StartTree = ShowDic[ShowListIndex].Keys.First();
                                           SingleShowToDataGridView(StartTree, ShowDic[ShowListIndex][StartTree]);
                                       }
                                       BeginInvoke(new MethodInvoker(() =>
                                       {
                                           LoadListButton.Text = "载入列表";
                                           LoadLogfile.Enabled = true;
                                           LoadListButton.Enabled = true;
                                       }));
                                   }
                               }
                           }).Start();
                  }
              };
            FormClosed += delegate
            {
                if (SaveButton.Enabled)
                {
                    if (MessageBox.Show("列表已经修改，是否保存?", "是否保存列表", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        Save();
                    }
                }
            };
            AnalysisCheck.CheckedChanged += delegate
            {
                try
                {
                    if (AnalysisCheck.Checked)
                    {
                        if (ShowList.Count != 0)
                        {
                            StartTree = ShowDic[ShowListIndex].Keys.First();
                            SingleShowToDataGridView(StartTree, ShowDic[ShowListIndex][StartTree]);
                        }
                        LoadCustomSet.Visible = true;
                        SetCustom.Visible = true;
                        SaveCustom.Visible = true;
                    }
                    else
                    {
                        LoadCustomSet.Visible = false;
                        SetCustom.Visible = false;
                        SaveCustom.Visible = false;
                        if (LogData.Columns.Count < 6) return;
                        new Task(() => StartChangeShow()).Start();
                        LogData.Columns.Remove("检查工序");
                        LogData.Columns.Remove("检查工作");
                    }
                }
                catch (Exception)
                { }
            };
            SetCustom.Click += delegate
            {
                if (SetCustom.Text == "设定自定义")
                {
                    LogData.CellClick += setcustommouseopera;
                    SetCustom.Text = "正在设定";
                    LoadLogfile.Enabled = false;
                    LoadListButton.Enabled = false;
                    LoadCustomSet.Enabled = false;
                    SaveCustom.Enabled = false;
                    AnalysisCheck.Enabled = false;
                }
                else
                {
                    LogData.CellClick -= setcustommouseopera;
                    SetCustom.Text = "设定自定义";
                    LoadLogfile.Enabled = true;
                    LoadListButton.Enabled = true;
                    LoadCustomSet.Enabled = true;
                    SaveCustom.Enabled = true;
                    AnalysisCheck.Enabled = true;
                    ThreadPool.QueueUserWorkItem((object state) =>
                    {
                        List<SaveInfo> cusTomList = new List<SaveInfo>();
                        foreach (DataGridViewRow item in LogData.Rows)
                        {
                            var Color7 = item.Cells[7].Style.BackColor;
                            var Color8 = item.Cells[8].Style.BackColor;
                            if (Color7 != Color.Empty || Color8 != Color.Empty)
                            {
                                var TempInfo = new SaveInfo();
                                TempInfo.Color = Color7.IsEmpty ? Color8.Name : Color7.Name;
                                TempInfo.OperaInfo = item.Cells[5].Value.ToString();
                                TempInfo.remark = item.Cells[6].Value.ToString();
                                cusTomList.Add(TempInfo);
                            }
                        }
                        if (CusTomList.Count == cusTomList.Count)
                        {
                            for (int i = 0; i < CusTomList.Count; i++)
                            {
                                if (CusTomList[i] != cusTomList[i])
                                {
                                    if (MessageBox.Show("自定义已经修改，是否保存?", "是否保存自定义", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                                    {
                                        BeginInvoke(new MethodInvoker(() => { Savecustom(cusTomList); }));
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (MessageBox.Show("自定义已经修改，是否保存?", "是否保存自定义", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                            {
                                Savecustom(cusTomList);
                            }
                        }
                        CusTomList = new List<SaveInfo>(cusTomList);
                    });
                    //增加询问是否保存
                }
            };
            LoadCustomSet.Click += delegate
            {
                using (OpenFileDialog open = new OpenFileDialog())
                {
                    open.Title = "载入自定义";
                    open.Filter = "Xml文件(*.Xml)|*.xml";
                    open.AddExtension = true;
                    open.ShowDialog();
                    if (open.FileName == "") return;
                    SavePath = open.FileName;
                    new Task(() =>
                         {
                             XmlSerializer ser = new XmlSerializer(typeof(List<SaveInfo>));
                             using (Stream stream = new FileStream(open.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                             {
                                 CusTomList = new List<SaveInfo>((List<SaveInfo>)ser.Deserialize(stream));
                                 BeginInvoke(new MethodInvoker(() =>
                                 {
                                     LoadCustomSetToData();
                                 }));
                             }
                         }).Start();
                }
            };
            SaveCustom.Click += delegate
            {
                using (var SaveDialog = new SaveFileDialog())
                {
                    SaveDialog.Title = "保存自定义";
                    SaveDialog.Filter = "Xml文件(*.Xml)|*.xml";
                    SaveDialog.InitialDirectory = new FileInfo(Custompath).DirectoryName;
                    SaveDialog.FileName = new FileInfo(Custompath).Name;
                    SaveDialog.AddExtension = true;
                    SaveDialog.ShowDialog();
                    if (SaveDialog.FileName == "") return;
                    Custompath = SaveDialog.FileName;
                    Savecustom(CusTomList);
                }
            };
            new Task(() =>
            {
                if (File.Exists(Custompath))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(List<SaveInfo>));
                    using (Stream stream = new FileStream(new FileInfo(GetType().Assembly.Location).DirectoryName + @"\" + "custom.xml", FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        CusTomList = new List<SaveInfo>((List<SaveInfo>)ser.Deserialize(stream));
                    }
                }
            }).Start();
        }
    }
}