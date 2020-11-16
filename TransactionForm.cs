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
using HomeBudgetWf.Models;
using HomeBudgetWf.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JsonConverter = HomeBudgetWf.Json.JsonConverter;

namespace HomeBudgetWf
{
    public partial class TransactionForm : Form
    {
        private JArray _transactionJsonArray;
        private JArray _categoriesJsonArray;
        private List<TransactionWithCategory> _transactionWithCategories;

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
            //var years = _transactionJsonArray.Select(j => j.SelectToken("DateOfTransaction")).Distinct().ToArray();
            //comboBoxYear.DataSource = years;
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


            var list = _categoriesJsonArray.Select(t => t["Category"]).Distinct().ToArray();

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
            var transactionWithCategories = JArray.Parse(JsonConvert.SerializeObject(_transactionWithCategories));
            dt = DataTableConv.toDataTable(transactionWithCategories);
            if (dt != null)
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = string.Format("Category  LIKE '%{0}%'", comboBox1.SelectedItem);
                dataGridView1.DataSource = dv;
            }
        }

        private void comboBoxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            var noko = _transactionWithCategories.Where(t => t.DateOfTransaction.Year == comboBox1.SelectedIndex).ToArray();
            var transactionWithCategories = JArray.Parse(JsonConvert.SerializeObject(noko));
           
            dt = DataTableConv.toDataTable(transactionWithCategories);
            dataGridView1.DataSource = dt;
            if (dt != null)
            {
                //DataView dv = dt.DefaultView;
                //dv.RowFilter = string.Format("Category  LIKE '%{0}%'", comboBox1.SelectedItem);
               
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var transactionList = JsonConverter.ConvetJsonArrayToListTransaction(_transactionJsonArray);
            var keyWords = JsonConverter.ConvertJsonArrayToListKeyWords(_categoriesJsonArray);
            var transactionListToSave = Converter.GetTransactionListToSave(transactionList, keyWords);
            var noko = transactionListToSave.Where(t => string.IsNullOrEmpty(t.KeyWord?.Value)).ToArray();
            var noko2 = transactionListToSave.Where(t => string.IsNullOrEmpty(t.KeyWord?.ExpenseCategory?.Category)).ToArray();
            _transactionWithCategories = Converter.ConvertTrasactionToTransactionWithCategories(transactionListToSave);
            var years = _transactionWithCategories.Select(mov => mov.DateOfTransaction.Year).Distinct().ToArray();
            comboBoxYear.DataSource = years;
            dataGridView1.DataSource = _transactionWithCategories;
            dataGridView1.Refresh();
        }
    }
}
