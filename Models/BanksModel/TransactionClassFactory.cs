using Newtonsoft.Json.Linq;

namespace HomeBudgetWf.Models.BanksModel
{
    public abstract class TransactionClassFactory
    {
        public abstract TransactionAbstractClass GetTransactionClass();
    }

    public class SantanderRioFactory : TransactionClassFactory
    {
        public JToken _jToken;

        public SantanderRioFactory(JToken jToken)
        {
            _jToken = jToken;
        }

        public override TransactionAbstractClass GetTransactionClass()
        {
            return new SantanderRioTransactionClass(_jToken);
        }
    }
    public class SparebankenDInFactory : TransactionClassFactory
    {
        public JToken _jToken;

        public SparebankenDInFactory(JToken jToken)
        {
            _jToken = jToken;
        }

        public override TransactionAbstractClass GetTransactionClass()
        {
            return new SparebankenDinTransactionClass(_jToken);
        }
    }
}
