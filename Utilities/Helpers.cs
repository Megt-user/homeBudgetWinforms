using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using HomeBudgetWf.Models;
using HomeBudgetWf.Models.BanksModel;
using Newtonsoft.Json.Linq;
using Serilog;

namespace HomeBudgetWf.Utilities
{
    public class Helpers
    {

        internal List<KeyWord> _keworsList;

        public static string GetBanckName(JToken jToken)
        {
            var banckName = "NN";

            var dateOfregistration = jToken.Value<DateTime?>("DateOfregistration") ?? new DateTime();

            banckName = dateOfregistration > new DateTime(2015, 02, 08) ? "Sparebanken Din" : "Santander Rio";

            return banckName;
        }

        internal static ExpenseCategory ResolveExpenseCategories(ExpenseCategory[] categories)
        {
            if (ArrayCointainStringtring(categories.Select(c => c.Category).Distinct().ToArray(), "Mat"))
            {
                return categories.FirstOrDefault(c => c.Category.Equals("Mat", StringComparison.CurrentCultureIgnoreCase));
            }
            if (ArrayCointainStringtring(categories.Select(c => c.Category).Distinct().ToArray(), "Vinmonopolet"))
            {
                return categories.FirstOrDefault(c => c.Category.Equals("Vinmonopolet", StringComparison.CurrentCultureIgnoreCase));
            }
            if (ArrayCointainStringtring(categories.Select(c => c.Category).Distinct().ToArray(), "Diesel"))
            {
                return categories.FirstOrDefault(c => c.Category.Equals("Diesel", StringComparison.CurrentCultureIgnoreCase));
            }
            if (ArrayCointainStringtring(categories.Select(c => c.Category).Distinct().ToArray(), "House"))
            {
                return categories.FirstOrDefault(c => c.Category.Equals("House", StringComparison.CurrentCultureIgnoreCase));
            }
            if (ArrayCointainStringtring(categories.Select(c => c.Category).Distinct().ToArray(), "klær"))
            {
                return categories.FirstOrDefault(c => c.Category.Equals("Klær", StringComparison.CurrentCultureIgnoreCase));
            }
            if (ArrayCointainStringtring(categories.Select(c => c.Category).Distinct().ToArray(), "Fritid"))
            {
                return categories.FirstOrDefault(c => c.Category.Equals("Fritid", StringComparison.CurrentCultureIgnoreCase));
            }
            return null;
        }
        internal static KeyWord ResolveKeyWords(KeyWord[] keyWordMatchs)
        {
            string keyWord = null;
            var keyWordsString = keyWordMatchs.Select(kw => kw.Value).Distinct().ToArray();
            if (ArrayCointainStringtring(keyWordsString, "ffo"))
            {
                keyWord = "ffo";
            }
            else if (ArrayCointainStringtring(keyWordsString, "Matias"))
            {
                keyWord = "Matias";
            }
            else if (ArrayCointainStringtring(keyWordsString, "Åse"))
            {
                keyWord = "Åse";
            }
            else if (ArrayCointainStringtring(keyWordsString, "Oscar"))
            {
                keyWord = "Oscar";
            }
            else if (ArrayCointainStringtring(keyWordsString, "BRUKÅS"))
            {
                keyWord = "BRUKÅS";
            }
            else if (ArrayCointainStringtring(keyWordsString, "Hermann Ivarson"))
            {
                keyWord = "Hermann Ivarson";
            }
            else if (ArrayCointainStringtring(keyWordsString, "Forsikring"))
            {
                keyWord = "Forsikring";
            }
            ////TODO verifique cómo crear privilegios para configurar la subcategoría por ejemplo Hovden / Mat subcategoría cuando Mat debe ser comida pero otras causas Fritid
            //else if (ArrayCointainString(keeWords, "yx")) 
            //{
            //    keyWord = "Diesel";
            //    subProject = "Mismatch";
            //}
            else if (ArrayCointainStringtring(keyWordsString, "Hovden"))
            {
                keyWord = "Hovden";
            }
            else if (ArrayCointainStringtring(keyWordsString, "cf"))
            {
                keyWord = "cf";
            }
            else if (ArrayCointainStringtring(keyWordsString, "HVASSER"))
            {
                keyWord = "HVASSER";
            }
            else if (ArrayCointainStringtring(keyWordsString, "Husly"))
            {
                keyWord = "Husly";
            }
            else if (ArrayCointainStringtring(keyWordsString, "SANDEN CAMPING"))
            {
                keyWord = "CAMPING";
            }
            else if (ArrayCointainStringtring(keyWordsString, "SKARPHEDIN"))
            {
                keyWord = "skarphedin";
            }
            else if (ArrayCointainStringtring(keyWordsString, "Itunes"))
            {
                keyWord = "Itunes";
            } else if (ArrayCointainStringtring(keyWordsString, "Apple"))
            {
                keyWord = "Apple";
            }else if (ArrayCointainStringtring(keyWordsString, "Vipps"))
            {
                keyWord = "Vipps";
            }
            //else if (ArrayCointainStringtring(keyWordsString, "Extra"))
            //{
            //    keyWord = "Mat";
            //}else if (ArrayCointainStringtring(keyWordsString, "h&m"))
            //{
            //    keyWord = "Klaer";
            //}

            KeyWord selectedKeyWord = null;
            if (!String.IsNullOrEmpty(keyWord))
            {
                
                var nn = keyWordMatchs.Where(kw => kw.Value.Equals(keyWord, StringComparison.OrdinalIgnoreCase)).ToArray();
                selectedKeyWord = keyWordMatchs.FirstOrDefault(kw => kw.Value.Equals(keyWord, StringComparison.OrdinalIgnoreCase));
            }

            return selectedKeyWord;

        }

        private static bool ArrayCointainStringtring(string[] keyWords, string name)
        {
            return keyWords.Any(sub => CultureInfo.InvariantCulture.CompareInfo.LastIndexOf(name, sub, CompareOptions.IgnoreCase) >= 0);
        }

        internal static ExpenseCategory[] GetCategoryFromKeywordArray(IEnumerable<KeyWord> keyWordMatchs)
        {
            var categories = keyWordMatchs.Select(kw => kw.ExpenseCategory).Distinct().ToArray();
            return categories;
        }

        public static Match ExactMatch(string input, string match)
        {

            var isMatch = Regex.Match(input, String.Format(@"(?i)(?<= |^){0}(?= |$)", Regex.Escape(match)));
            return isMatch;
        }

        public static Transaction CreateTransaction(TransactionAbstractClass transactionAbstract, KeyWord keyWord = null, string[] keyWordStringList = null)
        {
            var newTransaction = new Transaction()
            {
                DateOfTransaction = transactionAbstract.DateOfTransaction,
                DateOfregistration = transactionAbstract.DateOfregistration,
                Amount = transactionAbstract.Amount,
                Balance = transactionAbstract.Balance,
                Description = transactionAbstract.Description,
                OthersDetails = transactionAbstract.BankName
            };

            if (keyWordStringList == null)
            {
                if (keyWord != null)
                {
                    newTransaction.KeyWord = keyWord;
                }
            }
            else
            {
                var mismatchKeyWords = String.Join(",", keyWordStringList);
                keyWord = new KeyWord()
                {
                    Value = "Multiple matchs"
                };
                newTransaction.OthersDetails = mismatchKeyWords;
            }

            return newTransaction;
        }

        public List<string> GetNorwegianCities()
        {
            var norwegianCities = new List<string>()
            {
                "Hovden",
                "Kongsberg",
                "Oslo",
                "Seljord"
            };

            return norwegianCities;
        }

        public static object GetPropertyValue(object model, string propertyName)
        {
            try
            {
                var properties = GetPropertiesNamesFromObject(model);
                if (properties.Contains(propertyName))
                {
                    object result = model.GetType().GetProperty(propertyName).GetValue(model, null);
                    return result;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        public static List<string> GetPropertiesNamesFromObject(object model)
        {
            var properties = model?.GetType().GetProperties();
            return properties?.Select(prop => prop.Name).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="transactionWithCategories"></param>
        /// <returns></returns>
        public static List<string> GetCategoriesFromTransactions(List<TransactionWithCategory> transactionWithCategories, bool? extraction = null)
        {
            List<string> categoryList = new List<string>();

            var categories = transactionWithCategories.Select(m => m.Category).Distinct().ToList();
            if (extraction.HasValue)
            {
                if (extraction.GetValueOrDefault())
                {
                    categoryList = categories.Where(category => transactionWithCategories.Where(mv => mv.Category == category).Any(mv => mv.Amount < 0)).ToList();
                }
                else
                {
                    categoryList = categories.Where(category => transactionWithCategories.Where(mv => mv.Category == category).Any(mv => mv.Amount > 0)).ToList();
                }
            }
            else
            {
                categoryList = categories;
            }

            return categoryList;
        }

        public static List<TransactionWithCategory> GetMonthYearTransaction(List<TransactionWithCategory> transactionWithCategories, int? year = null, int? month = null, bool? extraction = null, string category = null)
        {
            List<TransactionWithCategory> selectedTransactionWithCategories = new List<TransactionWithCategory>();

            if (transactionWithCategories == null || !transactionWithCategories.Any())
                return transactionWithCategories;

            selectedTransactionWithCategories = category != null ? transactionWithCategories.Where(t => t.Category == category).ToList() : transactionWithCategories;


            if (year.HasValue)
            {
                if (month.HasValue)
                {
                    selectedTransactionWithCategories = selectedTransactionWithCategories.Where(mov => !String.IsNullOrEmpty(mov.Category) && mov.DateOfTransaction.Year == year && mov.DateOfTransaction.Month == month).ToList();
                }
                else
                {
                    selectedTransactionWithCategories = selectedTransactionWithCategories.Where(mov => !String.IsNullOrEmpty(mov.Category) && mov.DateOfTransaction.Year == year).ToList();
                }
            }
            else
            {

                if (month.HasValue)
                {
                    selectedTransactionWithCategories = selectedTransactionWithCategories.Where(mov => !String.IsNullOrEmpty(mov.Category) && mov.DateOfTransaction.Month == month).ToList();
                }
                else
                {
                    selectedTransactionWithCategories = selectedTransactionWithCategories.Where(mov => !String.IsNullOrEmpty(mov.Category)).ToList();
                }
            }

            if (extraction.HasValue)
            {
                List<TransactionWithCategory> newSelectedTransactionWithCategories;
                if (extraction.GetValueOrDefault())
                    newSelectedTransactionWithCategories = selectedTransactionWithCategories.Where(mov => mov.Amount < 0).ToList();
                else
                    newSelectedTransactionWithCategories = selectedTransactionWithCategories.Where(mov => mov.Amount > 0).ToList();

                selectedTransactionWithCategories = newSelectedTransactionWithCategories;
            }
            return selectedTransactionWithCategories;
        }

        public static Transaction CreateTransactionWithoutKeyWordMatch(TransactionAbstractClass transaction)
        {

            var keyWordNowMatch = new KeyWord()
            {
                Value = "Now match",
                ExpenseCategory = new ExpenseCategory()
                {
                    Category = "Abra Cadabra"
                }
            };
            var transactionTemp = Helpers.CreateTransaction(transaction, keyWordNowMatch);
            return transactionTemp;
        }
        public static Transaction GetTransactionKeyWordMatch(TransactionAbstractClass transaction, KeyWord[] keyWordMatchs)
        {
            Transaction transactionTemp = null;
            if (keyWordMatchs != null && keyWordMatchs.Any())
            {
                //Just one matchs
                if (keyWordMatchs.Length == 1)
                {
                    transactionTemp = Helpers.CreateTransaction(transaction, keyWordMatchs.FirstOrDefault());
                }
                else
                {
                    var categories = Helpers.GetCategoryFromKeywordArray(keyWordMatchs);
                    if (categories.Length == 1)
                    {
                        var keyword = keyWordMatchs.FirstOrDefault();
                        Log.Information("keyWordMatchs {keywordsMismatch}, Selected KeyWord {@keyWord}.", String.Join(",", keyWordMatchs.Select(kw => kw.Value)), keyword);
                        transactionTemp = Helpers.CreateTransaction(transaction, keyword);
                    }
                    else
                    {
                        var category = Helpers.ResolveExpenseCategories(categories);
                        if (category != null)
                        {
                            var keyword = keyWordMatchs.FirstOrDefault(kw => kw.ExpenseCategory.Equals(category));
                            Log.Information("SubCategories {categoriesMismatch}, Selected KeyWord {@keyWord}.", String.Join(",", categories.Select(c => c.Category)), keyword);
                            transactionTemp = Helpers.CreateTransaction(transaction, keyword);
                        }
                        else
                        {
                            var keyWordTemp = Helpers.ResolveKeyWords(keyWordMatchs);
                            if (keyWordTemp != null)
                            {
                                Log.Information("keyWordMatchs {keywordsMismatch}, Selected KeyWord {@keyWord}.", String.Join(",", keyWordMatchs.Select(kw => kw.Value)), keyWordTemp);
                                transactionTemp = Helpers.CreateTransaction(transaction, keyWordTemp);
                            }
                            else
                            {
                                Log.Error("can't resolve keyWord miss matchs {keywordsMismatch}. description ({description})", String.Join(",", keyWordMatchs.Select(kw => kw.Value)), transaction.Description);
                                transactionTemp = Helpers.CreateTransaction(transaction, keyWordTemp, keyWordMatchs.Select(kw => kw.Value).ToArray());
                            }
                        }
                    }
                }
            }
            else
            {
                //string[] values = transaction.Description.Split(' ');
                //var descriptionsWords = values.GroupBy<string, string, int>(k => k, e => 1)
                //    .Select(f => new KeyValuePair<string, int>(f.Key, f.Sum()))
                //    .ToDictionary(k => k.Key, e => e.Value);
                //foreach (var word in descriptionsWords)
                //{
                //    if (needToCategorize.ContainsKey(word.Key))
                //    {
                //        needToCategorize[word.Key] = needToCategorize[word.Key] + word.Value;
                //    }
                //    else
                //    {
                //        needToCategorize.Add(word.Key, word.Value);
                //    }
                //}
            }

            return transactionTemp;
        }
    }


}
