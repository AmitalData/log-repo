using Amital.QuoteOPM.Data;
using Amital.QuoteOPM.Data.BL.BusinessUnitFilters;
using Amital.QuoteOPM.Data.EntityKeys;
using Amital.QuoteOPM.Def.EntityPMs;
using Logitude.BL.DataContracts;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amital.QuoteOPM.BL.EntityQueryServices
{
    public partial class QuoteOPQueryService
    {

        public List<ChartingDataClass> GetStageFunnelData(string ownerId, string businessUnitId, int tenant, string RecordsTypeCode)
        {
            var dataSourceQuery =
                (from d in repository.GetAllIncludeStage(tenant)
                 where d.Tenant == tenant
                 && !d.IsClosed
                 && !d.IsCancelled
                 && d.StageId != null
                 && d.Stage.Rank > 0
                 select d);

            var filter = new QuoteOPBusinessUnitFilter(tenant);
            dataSourceQuery = filter.RunFilter(dataSourceQuery);

            if (RecordsTypeCode == "C")
            {
                if (!string.IsNullOrEmpty(ownerId))
                {
                    dataSourceQuery = dataSourceQuery.Where(d => d.CreatedByUserId == ownerId);
                }
            }

            else
            {
                if (!string.IsNullOrEmpty(ownerId))
                {
                    dataSourceQuery = dataSourceQuery.Where(d => d.SalesmanUserId == ownerId);
                }

                if (!string.IsNullOrEmpty(businessUnitId))
                {
                    dataSourceQuery = dataSourceQuery.Where(d => d.BusinessUnitId == businessUnitId);
                }
            }

            List<ChartingDataClass> result =
                (from d in dataSourceQuery
                 group d by new { d.StageId, d.Stage.Name, d.Stage.Rank } into g
                 select new ChartingDataClass()
                 {
                     Id = g.Key.StageId,
                     LabelProperty = g.Key.Name,
                     DecimalProperty = g.Count(),
                     IntegerProperty = g.Key.Rank,
                     GroupedId = g.Key.StageId,
                 }).ToList();

            return result;
        }

        public override void GetComposition(EntityKeyFields entityKeys, QuoteOPPM entityPM)
        {
            IQuoteOPMContext context = MainContext as QuoteOPMContext;
            var quoteOPKeys = entityKeys as QuoteOPKeys;
            QuoteOPComputedFieldQueryService quoteOPComputedFieldQueryService= new QuoteOPComputedFieldQueryService(context);
            //entityPM.Quote = quoteOPComputedFieldQueryService.GetMulti(quoteOPKeys, true);
            QuoteOPPackageQueryService quoteOPPackageQueryService= new QuoteOPPackageQueryService(context);
            entityPM.QuotePackages = quoteOPPackageQueryService.GetMulti(quoteOPKeys, true);


            //QuoteOPChargeQueryService quoteOPChargeQueryService= new  QuoteOPChargeQueryService(context);
            //entityPM.QuoteOPCharge = quoteOPChargeQueryService.GetMulti(quoteOPKeys, true);

            base.GetComposition(entityKeys, entityPM);
        }
    }
}
