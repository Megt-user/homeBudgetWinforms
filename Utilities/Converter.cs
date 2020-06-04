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
            if (transactions.Any())
            {
                for (int index = 0; index < transactions.Length; index++)
                {
                    Transaction transactionTemp = null;

                    KeyWord[] keyWordMatchs = null;
                    if (keyWords.Any())
                        keyWordMatchs = keyWords.Where(sub => Helpers.ExactMatch(transactions[index].Description, sub.Value).Success).ToArray();
                    if (keyWordMatchs != null && keyWordMatchs.Any())
                    {
                       //Just one matchs
                        if (keyWordMatchs.Length == 1)
                        {
                            transactionTemp = Helpers.CreateTransaction(transactions[index], keyWordMatchs.FirstOrDefault());
                        }
                        else
                        {
                            var categories = Helpers.GetCategoryFromKeywordArray(keyWordMatchs);
                            if (categories.Length == 1)
                            {
                                var keyword = keyWordMatchs.FirstOrDefault();
                                Log.Information("keyWordMatchs {keywordsMismatch}, Selected KeyWord {@keyWord}. Index:({index})", string.Join(",", keyWordMatchs.Select(kw => kw.Value)), keyword, index);
                                transactionTemp = Helpers.CreateTransaction(transactions[index], keyword);
                            }
                            else
                            {
                                var category = Helpers.ResolveExpenseCategories(categories);
                                if (category != null)
                                {
                                    var keyword = keyWordMatchs.FirstOrDefault(kw => kw.ExpenseCategory.Equals(category));
                                    Log.Information("SubCategories {categoriesMismatch}, Selected KeyWord {@keyWord}. Index:({index})", string.Join(",", categories.Select(c => c.Category)), keyword, index);
                                    transactionTemp = Helpers.CreateTransaction(transactions[index], keyword);
                                }

                                var keyWordTemp = Helpers.ResolveKeyWords(keyWordMatchs);
                                if (keyWordTemp != null)
                                {
                                    Log.Information("keyWordMatchs {keywordsMismatch}, Selected KeyWord {@keyWord}. Index:({index})", string.Join(",", keyWordMatchs.Select(kw => kw.Value)), keyWordTemp, index);
                                    transactionTemp = Helpers.CreateTransaction(transactions[index], keyWordTemp);
                                }
                                else
                                {
                                    Log.Debug("can't resolve keyWord miss matchs {keywordsMismatch}. Index:({index})", string.Join(",", keyWordMatchs.Select(kw => kw.Value)), index);
                                    transactionTemp =Helpers.CreateTransaction(transactions[index], keyWordTemp, keyWordMatchs.Select(kw => kw.Value).ToArray());
                                }
                            }
                        }
                    }
                    else
                    {
                        //TODO now match
                    }

                    transactionList.Add(transactionTemp);
                }

            }
            return transactionList;
        }
    }
}
