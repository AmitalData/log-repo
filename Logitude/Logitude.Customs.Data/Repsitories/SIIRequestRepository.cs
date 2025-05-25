 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using System.Data.Entity;
using Logitude.Customs.Data.DataContracts;
using System.Diagnostics.PerformanceData;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class SIIRequestRepository:IRepository<SIIRequest>
   {
        
		public List<SIIRequest> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

       
        public SiiAgg GetAggregateForSii(int tenant, string declarationId)
        {
            return (from d in context.Declarations
                    where d.Tenant == tenant && d.Id == declarationId

                    from c in context.Clients
                             .Where(c => c.Tenant == tenant && c.Code == d.ImporterId)
                             .DefaultIfEmpty()

                    from rd in context.DeclarationReferantDatas
                             .Where(r => r.Tenant == tenant && r.DeclarationId == d.Id)
                             .DefaultIfEmpty()
                    from con in context.Consignments
                               .Where(con => con.Tenant == tenant && con.DeclarationId == d.Id)
                               .OrderBy(con => con.SequenceNumeric)
                               .Take(1)
                               .DefaultIfEmpty()

                    select new SiiAgg
                    {
                        ImporterInternalId = c == null ? null : c.Id,
                        ImporterCode = d.ImporterCode,
                        VesselLocalName = rd.Vessel != null
                                     ? (rd.VesselCode.LocalName ?? rd.VesselCode.EnglishName)
                                     : null,
                        ManifestNumber = con == null ? null : con.ManifestNumber,
                        UnloadDate = con == null ? null : (DateTime?)con.UnloadDate
                    })
                    .AsNoTracking()
                    .FirstOrDefault();
        }

        public List<SupplieInvoiceItemsForSIIRequest> GetSupplierInvoiceItems(string declarationId, int tenant)
        {
            var list =
        from itm in context.SupplierInvoiceItems
        where itm.DeclarationId == declarationId
              && itm.Tenant == tenant
              && !itm.IsParent                     
        join inv in context.SupplierInvoices
             on new { itm.DeclarationId, itm.CounterKey }
             equals new { inv.DeclarationId, CounterKey = inv.InvoiceCounterKey }
             into invJoin
        from si in invJoin.DefaultIfEmpty()

        join ta in context.TradeAgreements
            on itm.TradeAgreementCode equals ta.Code
            into taJoin
        from trade in taJoin.DefaultIfEmpty()

        join mu in context.MeasurmentUnits          
             on itm.InvoiceQuantityType equals mu.Code
             into muJoin
        from unit in muJoin.DefaultIfEmpty()

        join cc in context.CustomsCountries         
             on itm.OriginCountryCode equals cc.Code
             into ccJoin
        from country in ccJoin.DefaultIfEmpty()

        join cert in context.SupplierInvioceItemCertificats
            on new { itm.DeclarationId, itm.LineNumber,  itm.CounterKey }
            equals new { cert.DeclarationId, cert.LineNumber, CounterKey = cert.InvoiceCounterKey }
            into certJoin
        from certificate in certJoin.DefaultIfEmpty()

        join cert in context.SupplierInvoiceItemsReqLists
            on new { itm.DeclarationId, itm.LineNumber, itm.CounterKey }
            equals new { cert.DeclarationId, cert.LineNumber, CounterKey = cert.InvoiceCounterKey }
            into reqJoin
        from requestList in reqJoin.DefaultIfEmpty()

        select new SupplieInvoiceItemsForSIIRequest
        {
            InvoiceNumber = si.InvoiceNumber,
            LineNumber = itm.LineNumber,
            CounterKey = itm.LineNumber,
            ItemCode = itm.ItemCode,
            ItemDescription = itm.ItemDescription,
            ClassificationCode = itm.ClassificationCode,

            TradeAgreementCode = itm.TradeAgreementCode,
            TradeAgreementName = trade.LocalName,

            InvoiceQuantityType = itm.InvoiceQuantityType,
            InvoiceQuantityTypeName = unit.LocalName,

            InvoiceQuantity = itm.InvoiceQuantity.ToString(),
            ItemPrice = itm.ItemPrice.ToString(),
            ItemPriceCurrencyCode = itm.ItemPriceCurrencyCode,

            OriginCountryCode = itm.OriginCountryCode,
            OriginCountryName = country.LocalName,
            ReqConfirmationTypeCode = certificate.ReqConfirmationTypeCode,
            RequestRequiredStatus = requestList.RequestRequiredStatus,

        };

            return list.ToList();
        }

        public class SiiAgg
        {
            public string ImporterInternalId { get; set; }   // can be null
            public string ImporterCode { get; set; }
            public string VesselCode { get; set; }
            public string VesselLocalName { get; set; }
            public string ManifestNumber { get; set; }
            public DateTime? UnloadDate { get; set; }
        }

    }


}
   