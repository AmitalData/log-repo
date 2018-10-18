using Logitude.BL.QuoteModel.EntityPMs;
using Simplog.Data.QuoteModel.EntityPOCOs;

namespace Logitude.BL.QuoteModel.Tools.DataMapping
{
    public class QuoteTypeMapping
    {
        public static void MapEntity(QuoteTypePM entityPM, QuoteType poco, bool isNewState)
        {
            poco.Code = entityPM.Code;
            poco.Name = entityPM.Name; 
            poco.SearchFields = entityPM.SearchFields; 

        }
    }
}