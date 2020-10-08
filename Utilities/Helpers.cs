using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using HomeBudgetWf.Models;
using HomeBudgetWf.Models.BanksModel;
using Newtonsoft.Json.Linq;

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
                keyWord = "Sport";
            }
            else if (ArrayCointainStringtring(keyWordsString, "Hermann Ivarson"))
            {
                keyWord = "Utlaie";
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
                keyWord = "Fritid";
            }
            else if (ArrayCointainStringtring(keyWordsString, "cf"))
            {
                keyWord = "House";
            }
            else if (ArrayCointainStringtring(keyWordsString, "HVASSER"))
            {
                keyWord = "Fritid";
            }
            else if (ArrayCointainStringtring(keyWordsString, "Husly"))
            {
                keyWord = "House";
            }
            else if (ArrayCointainStringtring(keyWordsString, "SANDEN CAMPING"))
            {
                keyWord = "Fritid";
            }
            else if (ArrayCointainStringtring(keyWordsString, "SKARPHEDIN"))
            {
                keyWord = "Familly";
            }
            //else if (ArrayCointainStringtring(keyWordsString, "Extra"))
            //{
            //    keyWord = "Mat";
            //}else if (ArrayCointainStringtring(keyWordsString, "h&m"))
            //{
            //    keyWord = "Klaer";
            //}

            KeyWord selectedKeyWord = null;
            if (!string.IsNullOrEmpty(keyWord))
                selectedKeyWord = keyWordMatchs.FirstOrDefault(kw => kw.Value.Equals(keyWord, StringComparison.OrdinalIgnoreCase));

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

            var isMatch = Regex.Match(input, string.Format(@"(?i)(?<= |^){0}(?= |$)", Regex.Escape(match)));
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
                var mismatchKeyWords = string.Join(",", keyWordStringList);
                keyWord = new KeyWord()
                {
                    Value = "Multiple matchs"
                };
                newTransaction.OthersDetails = mismatchKeyWords;
            }

            return newTransaction;
        }

        public List<String> GetNorwegianCities()
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
    }
}
