using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure.AnalyticTableServices;
using System.Data.Entity;

namespace Logitude.BL.AnalyticTableServices
{
    public class QuoteAnalyticTableService : AnalyticTableService<Quote, QuoteAnalytic>
    {
        public QuoteAnalyticTableService(DbContext context) : base(context)
        {

        }


        protected override void CustomMap(Quote entity, QuoteAnalytic analyticTable)
        {
            
        }
    }
}
