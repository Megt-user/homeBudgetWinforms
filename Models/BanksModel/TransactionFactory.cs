using System;
using System.Collections.Generic;
using System.Text;
using HomeBudgetWf.Utilities;
using Newtonsoft.Json.Linq;

namespace HomeBudgetWf.Models.BanksModel
{
    public class TransactionFactory
    {
        public static TransactionClass GetTransactionClassFromJtoken(JToken jToken)
        {
            var banckName = Helpers.GetBanckName(jToken);
            TransactionClass transactionFactory = null;
            switch (banckName)
            {
                case "Santander Rio":
                    transactionFactory = new SantanderRioFactory(jToken);
                    break;
                default:
                    transactionFactory = new SparebankenDinFactory(jToken);
                    break;
            }
            return transactionFactory;
        }
    }
}
