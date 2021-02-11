namespace Logitude.SpecFlow.Models.ChargeType
{
    public class ChargeTypeContext
    {
        public ChargeTypeContext()
        {
            ChargeTypePM = new ChargeTypePM();
        }

        public ChargeTypePM ChargeTypePM { get; set; }
    }
}