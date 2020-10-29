using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HomeBudgetWf.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HomeBudgetWf
{
    public partial class TransactionForm : Form
    {
        private JArray _transactionJsonArray;
        private JArray _categoriesJsonArray;

        public TransactionForm()
        {
            InitializeComponent();
        }

        private void buttonOpenTransactions_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string fileName = openFileDialog1.FileName;
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            OutToLogTextBox(fileNameWithoutExtension);
            _transactionJsonArray = new ExcelConverter().GetJsonArrayfromExcelfile(fileName);
            if (_transactionJsonArray == null || !_transactionJsonArray.Any())
            {
                OutToLogTextBox($"cant fiend transactions in file {Path.GetFileName(openFileDialog1.FileName)}");
            }
            else
            {
                OutToLogTextBox($"number Of Transactions = {_transactionJsonArray?.Count}");
            }
        }
        private void buttonOpenCategories_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string fileName = openFileDialog1.FileName;
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            OutToLogTextBox(fileNameWithoutExtension);
            _categoriesJsonArray = new ExcelConverter().GetJsonArrayfromExcelfile(fileName);
            if (_categoriesJsonArray == null || !_categoriesJsonArray.Any())
            {
                OutToLogTextBox($"cant fiend Categories in file {Path.GetFileName(openFileDialog1.FileName)}");
            }
            else
            {
                OutToLogTextBox($"number Of categories = {_categoriesJsonArray?.Count}");
            }


            var list = _categoriesJsonArray.Select(t => t["KeyWord"]).Distinct().ToArray();

            comboBox1.DataSource = list;
            comboBox1.Refresh();
            dataGridView1.DataSource = _categoriesJsonArray;

            dataGridView1.Refresh();




        }

        internal void OutToLogTextBox(string message)
        {
            logRichTextBox.AppendText("\r\n" + message);
            logRichTextBox.ScrollToCaret();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        DataTable dt = new DataTable();

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dt = toDataTable(_categoriesJsonArray);
            if (dt != null)
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = string.Format("Category  LIKE '%{0}%'", comboBox1.SelectedItem);
                dataGridView1.DataSource = dv;
            }
        }

        private static DataTable toDataTable(JArray jArray)
        {
            var result = new DataTable();
            //Initialize the columns, If you know the row type, replace this   
            foreach (var row in jArray)
            {
                foreach (var jToken in row)
                {
                    var jproperty = jToken as JProperty;
                    if (jproperty == null) continue;
                    if (result.Columns[jproperty.Name] == null)
                        result.Columns.Add(jproperty.Name, typeof(string));
                }
            }
            foreach (var row in jArray)
            {
                var datarow = result.NewRow();
                foreach (var jToken in row)
                {
                    var jProperty = jToken as JProperty;
                    if (jProperty == null) continue;
                    datarow[jProperty.Name] = jProperty.Value.ToString();
                }
                result.Rows.Add(datarow);
            }
            return result;
        }
    }
}
