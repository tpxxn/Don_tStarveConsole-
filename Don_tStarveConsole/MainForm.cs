using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CCWin;
using System.IO;
using System.Text.RegularExpressions;
using log4net;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace Don_tStarveConsole
{
    public partial class MainForm : Skin_VS
    {
        public static ILog Log_0;
        public static ILog Log_error;
        private string DataPath = Directory.GetCurrentDirectory() + "\\code.txt";
        private DataTable DataSource = null;

        static MainForm()
        {
            Log_0 = LogManager.GetLogger("BootLogging");
            Log_error = LogManager.GetLogger("ErrorLogging");
        }
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Log_0.Info("程序启动");
            DataSource = GenerateData(ReadData());
            DataGridViewInit();
        }

        private List<DataActive> ReadData()
        {
            string line = null;
            string category = null;
            StreamReader file = new StreamReader(DataPath, Encoding.Default);
            List<DataActive> DataActiveList = new List<DataActive>();

            while ((line = file.ReadLine()) != null)
            {
                try
                {
                    Regex rex1 = new Regex("^[0-9{1}|DebugSpawn].*");
                    Match ma1 = rex1.Match(line);
                    if (ma1.Success)
                    {
                        DataActive da = new DataActive();
                        Regex rex = new Regex("^[0-9]{1}.*");
                        Match ma = rex.Match(line);
                        if (ma.Success)
                        {
                            line = line.Substring(2, line.Length - 2);
                            category = line;
                        }
                        else
                        {
                            line = line.Replace("DebugSpawn", "");
                            line = line.Replace("“", "");
                            line = line.Replace("”", "");
                            line = line.Replace("\"", "");
                            line = line.Replace("（", "#");
                            line = line.Replace("）", "");
                            line = line.Replace("(", "#");
                            line = line.Replace(")", "");
                            line = line.Replace(" ", "").Trim();
                            string[] sArray = line.Split('#');
                            da.Category = category;
                            da.CNName = sArray[1];
                            da.ENName = sArray[0];
                            DataActiveList.Add(da);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log_error.Error("读取数据文件异常：", ex);
                }
            }

            return DataActiveList;
        }

        private DataTable GenerateData(List<DataActive> DataActiveList)
        {
            try
            {
                DataTable dt = new DataTable("CodeTable");
                DataColumn dc1 = new DataColumn("Category", Type.GetType("System.String"));
                DataColumn dc2 = new DataColumn("CNName", Type.GetType("System.String"));
                DataColumn dc3 = new DataColumn("ENName", Type.GetType("System.String"));
                dt.Columns.Add(dc1);
                dt.Columns.Add(dc2);
                dt.Columns.Add(dc3);

                foreach (DataActive da in DataActiveList)
                {
                    DataRow dr = dt.NewRow();
                    dr["Category"] = da.Category;
                    dr["CNName"] = da.CNName;
                    dr["ENName"] = da.ENName;
                    dt.Rows.Add(dr);
                }

                return dt;
            }
            catch (Exception ex)
            {
                Log_error.Error("转换数据出错：", ex);
                return null;
            }
        }

        private DataTable DataSelect(string condition, DataTable dt)
        {
            try
            {
                DataRow[] dtr = dt.Select("Category = '" + condition + "'");
                if (dtr.Count() > 0)
                    return dt = dtr.CopyToDataTable();
                else
                {
                    DataTable nullDt = new DataTable("NullTable");
                    DataColumn dc1 = new DataColumn("Category", Type.GetType("System.String"));
                    DataColumn dc2 = new DataColumn("CNName", Type.GetType("System.String"));
                    DataColumn dc3 = new DataColumn("ENName", Type.GetType("System.String"));
                    nullDt.Columns.Add(dc1);
                    nullDt.Columns.Add(dc2);
                    nullDt.Columns.Add(dc3);
                    return nullDt;
                }
            }
            catch (Exception ex)
            {
                Log_error.Error("查询数据出错：", ex);
                return null;
            }
        }

        private DataTable DataSearch(string condition, DataTable dt)
        {
            try
            {
                DataRow[] dtr = dt.Select("CNName LIKE '%" + condition + "%' OR ENName LIKE '%" + condition + "%'");
                if (dtr.Count() > 0)
                    return dt = dtr.CopyToDataTable();
                else
                {
                    DataTable nullDt = new DataTable("NullTable");
                    DataColumn dc1 = new DataColumn("Category", Type.GetType("System.String"));
                    DataColumn dc2 = new DataColumn("CNName", Type.GetType("System.String"));
                    DataColumn dc3 = new DataColumn("ENName", Type.GetType("System.String"));
                    nullDt.Columns.Add(dc1);
                    nullDt.Columns.Add(dc2);
                    nullDt.Columns.Add(dc3);
                    return nullDt;
                }
            }
            catch (Exception ex)
            {
                Log_error.Error("查询数据出错：", ex);
                return null;
            }
        }

        private void DataGridViewInit()
        {
            if (DataSource != null)
            {
                DataGridView1.DataSource = DataSource;
            }
            else
            {
                MessageBox.Show("查询出错！");
            }
        }

        private void allBtn_Click(object sender, EventArgs e)
        {
            DataGridViewInit();
        }

        private void materialBtn_Click(object sender, EventArgs e)
        {
            DataTable dt = DataSelect("材料类", DataSource);
            if (dt != null)
            {
                DataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("查询出错！");
            }
        }

        private void weaponBtn_Click(object sender, EventArgs e)
        {
            DataTable dt = DataSelect("工具武器", DataSource);
            if (dt != null)
            {
                DataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("查询出错！");
            }
        }

        private void apparelBtn_Click(object sender, EventArgs e)
        {
            DataTable dt = DataSelect("穿戴", DataSource);
            if (dt != null)
            {
                DataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("查询出错！");
            }
        }

        private void buildBtn_Click(object sender, EventArgs e)
        {
            DataTable dt = DataSelect("建筑", DataSource);
            if (dt != null)
            {
                DataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("查询出错！");
            }
        }

        private void foodBtn_Click(object sender, EventArgs e)
        {
            DataTable dt = DataSelect("食物", DataSource);
            if (dt != null)
            {
                DataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("查询出错！");
            }
        }

        private void plantBtn_Click(object sender, EventArgs e)
        {
            DataTable dt = DataSelect("植物", DataSource);
            if (dt != null)
            {
                DataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("查询出错！");
            }
        }

        private void animalBtn_Click(object sender, EventArgs e)
        {
            DataTable dt = DataSelect("动物", DataSource);
            if (dt != null)
            {
                DataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("查询出错！");
            }
        }

        private void otherBtn_Click(object sender, EventArgs e)
        {
            DataTable dt = DataSelect("其他", DataSource);
            if (dt != null)
            {
                DataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("查询出错！");
            }
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {

            DataTable dt = DataSearch(searchText.Text, DataSource);
            if (dt != null)
            {
                DataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("查询出错！");
            }
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (DataGridView1.SelectedRows.Count > 0)
            {
                int index = DataGridView1.CurrentRow.Index; //获取选中行的行号
                textBox1.Text = DataGridView1.Rows[index].Cells[2].Value.ToString();
            }
        }

        private void CreatCode()
        {

        }
    }
}
