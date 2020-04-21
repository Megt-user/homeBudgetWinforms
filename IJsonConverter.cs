using System.Collections.Generic;
using HomeBudgetWf.Models;
using Newtonsoft.Json.Linq;

namespace HomeBudgetWf
{
    public interface IJsonConverter
    {
        List<Transaction> ConvertJArrayToTransactionList(JArray jsonArray);
    }
}