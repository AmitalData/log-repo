using Logitude.Test.Base.Models.Login;

namespace Logitude.SpecFlow.Models.ChargeType
{
    public class ChargeTypeContext
    {
        public ChargeTypeContext()
        {
            ChargeTypePM = new ChargeTypePM();
        }

        public User User { get; set; }
        public ChargeTypePM ChargeTypePM { get; set; }
    }
}