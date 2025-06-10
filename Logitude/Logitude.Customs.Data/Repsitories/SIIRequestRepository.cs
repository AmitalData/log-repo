 
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
    public partial class SIIRequestRepository : IRepository<SIIRequest>
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
                        UnloadDate = con == null ? null : (DateTime?)con.UnloadDate,
                        CustomerId = d.CustomerId,
                    })
                    .AsNoTracking()
                    .FirstOrDefault();
        }

        public List<SupplieInvoiceItemsForSIIRequest> GetSupplierInvoiceItems(string declarationId, string siiRequestId, int tenant)
        {

            var validCodes = new[] { "401", "402", "403" };


            var rawList =
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
         on new { itm.DeclarationId, itm.LineNumber, itm.CounterKey }
            equals new
            {
                cert.DeclarationId,
                cert.LineNumber,
                CounterKey = cert.InvoiceCounterKey
            }
         into certGroup

        join cert in context.SupplierInvoiceItemsReqLists
            on new { itm.DeclarationId, itm.LineNumber, itm.CounterKey, SIIRequestID = siiRequestId }
            equals new { cert.DeclarationId, cert.LineNumber, CounterKey = cert.InvoiceCounterKey, cert.SIIRequestID }
            into reqJoin
        from requestList in reqJoin.DefaultIfEmpty()

        select new SupplieInvoiceItemsForSIIRequest
        {
            InvoiceNumber = si.InvoiceNumber,
            InvoiceLineNumber = itm.LineNumber,
            InvoiceCounterKey = itm.CounterKey,
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
            HasDemandState = certGroup.Any(c => validCodes.Contains(c.ReqConfirmationTypeCode)),
            RequestRequiredStatus = String.IsNullOrEmpty(requestList.RequestRequiredStatus) ? "0" : requestList.RequestRequiredStatus,
            LineNumber = requestList != null ? (int)requestList.LineNumber : 0,
        };
            var list = rawList.ToList();

            var result = list.Select((x, index) => new SupplieInvoiceItemsForSIIRequest
            {
                InvoiceNumber = x.InvoiceNumber,
                InvoiceLineNumber = x.LineNumber,
                InvoiceCounterKey = x.InvoiceCounterKey,
                ItemCode = x.ItemCode,
                ItemDescription = x.ItemDescription,
                ClassificationCode = x.ClassificationCode,
                TradeAgreementCode = x.TradeAgreementCode,
                TradeAgreementName = x.TradeAgreementName,
                InvoiceQuantityType = x.InvoiceQuantityType,
                InvoiceQuantityTypeName = x.InvoiceQuantityTypeName,
                InvoiceQuantity = x.InvoiceQuantity,
                ItemPrice = x.ItemPrice,
                ItemPriceCurrencyCode = x.ItemPriceCurrencyCode,
                OriginCountryCode = x.OriginCountryCode,
                OriginCountryName = x.OriginCountryName,
                HasDemandState = x.HasDemandState,
                RequestRequiredStatus = x.RequestRequiredStatus,
                LineNumber = index + 1,
            }).ToList();

            return result;
        }

        public int GetSIIFormApplicationMaxNumber(int tenant)
        {
            var ids = context.SIIRequests
                .Where(s => s.Tenant == tenant)
                .Select(s => s.FromApplicationId)
                .ToList();
            if (ids.Count == 0)
                return 0;
            var maxNumber = ids
                .Select(id =>
                {
                    var parts = id.Split('-');
                    var last = parts.Last();
                    return int.TryParse(last, out var num) ? num : 0;
                })
                .Max();
            return maxNumber;
        }

        public class SiiAgg
        {
            public string ImporterInternalId { get; set; }   // can be null
            public string ImporterCode { get; set; }
            public string VesselCode { get; set; }
            public string VesselLocalName { get; set; }
            public string ManifestNumber { get; set; }
            public DateTime? UnloadDate { get; set; }
            public string CustomerId { get; set; }
            public string OriginCountryCode { get; set; } 
            public string UnloadPortCode { get; set; }

        }
       
    }


}
   