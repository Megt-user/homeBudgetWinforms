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
using HomeBudgetWf.DataTable;
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
            dataGridView1.DataSource = DataTableConv.toDataTable(_transactionJsonArray);

            dataGridView1.Refresh();
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
            dataGridView1.DataSource = DataTableConv.toDataTable(_categoriesJsonArray);

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
        System.Data.DataTable dt = new System.Data.DataTable();

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dt = DataTableConv.toDataTable(_categoriesJsonArray);
            if (dt != null)
            {
                //DataView dv = dt.DefaultView;
                //dv.RowFilter = string.Format("Category  LIKE '%{0}%'", comboBox1.SelectedItem);
                //dataGridView1.DataSource = dv;
            }
        }

      
    }
}
