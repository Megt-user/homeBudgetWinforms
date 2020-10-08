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
            Dictionary<string, int> needToCategorize = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
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
                                else
                                {
                                    var keyWordTemp = Helpers.ResolveKeyWords(keyWordMatchs);
                                    if (keyWordTemp != null)
                                    {
                                        Log.Information("keyWordMatchs {keywordsMismatch}, Selected KeyWord {@keyWord}. Index:({index})", string.Join(",", keyWordMatchs.Select(kw => kw.Value)), keyWordTemp, index);
                                        transactionTemp = Helpers.CreateTransaction(transactions[index], keyWordTemp);
                                    }
                                    else
                                    {
                                        Log.Error("can't resolve keyWord miss matchs {keywordsMismatch}. Index:({index}), description ({description})", string.Join(",", keyWordMatchs.Select(kw => kw.Value)), index, transactions[index].Description);
                                        transactionTemp = Helpers.CreateTransaction(transactions[index], keyWordTemp, keyWordMatchs.Select(kw => kw.Value).ToArray());
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //TODO now match
                        var keyWordNowMatch = new KeyWord()
                        {
                            Value = "Now match",
                            ExpenseCategory = new ExpenseCategory()
                            {
                                Category = "Abra Cadabra"
                            }
                        };
                        transactionTemp = Helpers.CreateTransaction(transactions[index], keyWordNowMatch);

                        string[] values = transactions[index].Description.Split(' ');
                        var descriptionsWords = values.GroupBy<string, string, int>(k => k, e => 1)
                            .Select(f => new KeyValuePair<string, int>(f.Key, f.Sum()))
                            .ToDictionary(k => k.Key, e => e.Value);
                        foreach (var word in descriptionsWords)
                        {
                            if (needToCategorize.ContainsKey(word.Key))
                            {
                                needToCategorize[word.Key] = needToCategorize[word.Key] + word.Value;
                            }
                            else
                            {
                                needToCategorize.Add(word.Key, word.Value);
                            }
                        }

                    }

                    if (transactionTemp == null)
                    {
                        //Log.Error("cant create transaction item {@transaction}",transactions[index]);
                    }
                    else
                    {
                        transactionList.Add(transactionTemp);
                    }
                }

                var array = needToCategorize.Where(k => !string.IsNullOrEmpty(k.Key)).Where(k => k.Value > 10).ToDictionary(x => x.Key, y => y.Value);
                foreach (var wordToCategorize in array)
                {
                    Log.Debug("this word need to be categorize {WordToCategorize} {WordToCategorizaCount}", wordToCategorize.Key, wordToCategorize.Value);
                }

            }
            return transactionList;
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
