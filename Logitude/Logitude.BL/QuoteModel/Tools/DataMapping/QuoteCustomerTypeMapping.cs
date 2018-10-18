using Logitude.BL.QuoteModel.EntityPMs;
using Simplog.Data.QuoteModel.EntityPOCOs;

namespace Logitude.BL.QuoteModel.Tools.DataMapping
{
    public class QuoteCustomerTypeMapping
    {
        public static void MapEntity(QuoteCustomerTypePM entityPM, QuoteCustomerType poco, bool isNewState)
        {
            poco.Code = entityPM.Code;
            poco.Name = entityPM.Name; 
            poco.SearchFields = entityPM.SearchFields; 

        }
    }
}