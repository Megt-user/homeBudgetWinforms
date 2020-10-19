using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HomeBudgetWf.Models;
using HomeBudgetWf.Models.BanksModel;
using Serilog;

namespace HomeBudgetWf.Utilities
{
    public class Converter
    {
        public static List<Transaction> GetTransactionListToSave(TransactionAbstractClass[] transactions, KeyWord[] keyWords)
        {
            var transactionList = new List<Transaction>();
            if (transactions == null || !transactions.Any() && keyWords == null || !keyWords.Any())
                return transactionList;
            string[] keywordsList = keyWords.Select(k => k.Value).ToArray();

            for (int index = 0; index < transactions.Length; index++)
            {
                Transaction transactionTemp = null;

                var transactionsDescription = transactions[index].Description;
                // Get key words exact match to get transaction
                var keyWordMatchsList = keywordsList.Where(sub => Helpers.ExactMatch(transactionsDescription, sub).Success).ToList();

                // create transaction from keyWord foun in exactMatch
                transactionTemp = TransactionTemp(transactions[index], keyWords, keyWordMatchsList);

                //If transaction didn't have a match tray with Contein
                if (transactionTemp == null)
                {
                    var newKeyWordMatchs = keywordsList.Where(sub => transactionsDescription.Contains(sub, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    transactionTemp = TransactionTemp(transactions[index], keyWords, newKeyWordMatchs);
                }

                if (transactionTemp == null)
                    transactionTemp = Helpers.CreateTransactionWithoutKeyWordMatch(transactions[index]);

                transactionList.Add(transactionTemp);

            }

            return transactionList;
        }

        public static Transaction TransactionTemp(TransactionAbstractClass transactions, KeyWord[] keyWords, List<string> keyWordExactMatchList)
        {
            Transaction transactionTemp = null;
            if (keyWordExactMatchList != null)
            {
                List<KeyWord> keyWordExactMatch = new List<KeyWord>();
                foreach (var key in keyWordExactMatchList)
                {
                    keyWordExactMatch.Add(keyWords.First(k => k.Value.Equals(key)));
                }

                transactionTemp = Helpers.GetTransactionKeyWordMatch(transactions, keyWordExactMatch.ToArray());
            }

            return transactionTemp;
        }


        public static List<TransactionWithCategory> ConvertTrasactionToTransactionWithCategories(List<Transaction> transactions)
        {
            List<TransactionWithCategory> transactionWithCategories = new List<TransactionWithCategory>();
            for (int i = 0; i < transactions.Count; i++)
            {
                try
                {
                    if (transactions[i] == null)
                    {
                        Log.Error("ConvertTrasactionToTransactionWithCategories index of transaction is null {index}", i);
                        continue;
                    }

                    TransactionWithCategory transactionWithCategory = new TransactionWithCategory().GetTransactionWithCategory(transactions[i]);
                    transactionWithCategories.Add(transactionWithCategory);
                }
                catch (Exception e)
                {
                    Log.Error(e, "cant create TransactionWithCategory from Transaction, transaction index:{index}", i);
                }
            }

            return transactionWithCategories;
        }
    }
}
