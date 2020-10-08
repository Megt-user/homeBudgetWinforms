using HomeBudgetWf.Utilities;
using Newtonsoft.Json.Linq;

namespace HomeBudgetWf.Models.BanksModel
{
    
    public abstract class TransactionClass
    {
        public abstract TransactionAbstractClass CreateTransactionClass();
    }

    public class SantanderRioFactory : TransactionClass
    {
        public JToken JToken;

        public SantanderRioFactory(JToken jToken)
        {
            JToken = jToken;
        }

        public override TransactionAbstractClass CreateTransactionClass()
        {
            return new SantanderRioTransactionClass(JToken);
        }
    }
    public class SparebankenDinFactory : TransactionClass
    {
        public JToken _jToken;

        public SparebankenDinFactory(JToken jToken)
        {
            _jToken = jToken;
        }

        public override TransactionAbstractClass CreateTransactionClass()
        {
            return new SparebankenDinTransactionClass(_jToken);
        }
    }
}
