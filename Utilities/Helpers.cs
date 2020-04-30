using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace HomeBudgetWf.Utilities
{
    public class Helpers
    {
        public static string GetBanckName(JToken jToken)
        {
            var banckName = "NN";

            var dateOfregistration = jToken.Value<DateTime?>("DateOfregistration") ?? new DateTime();

            banckName = dateOfregistration > new DateTime(2015, 02, 08) ? "Sparebanken Din" : "Santander Rio";

            return banckName;
        }
    }
}
