	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data.EntityLists;
using Amital.QuoteOPM.Data.BL.BusinessUnitFilters;
using Amital.QuoteOPM.Data.Repsitories;

namespace Amital.QuoteOPM.Data.EntityListQueryServices
{ 

    public partial class QuoteOPListQueryService
    {
	    private IQueryable<QuoteOPList> GetIqueryableList(IQueryable<QuoteOP> iQueryable)
        {
#if true
            if (iQueryable.Count() > 0)
            {
                int tenant = iQueryable.First().Tenant;

                QuoteOPBusinessUnitFilter businessUnitFilter = new QuoteOPBusinessUnitFilter(tenant);
                iQueryable = businessUnitFilter.RunFilter(iQueryable);

#warning Logitude.BL not alowed in DATA use in controller               iQueryable = BranchPermitionsFilter.AddUserBranchRestrictionFilters<QuoteOP>(new QueryOperations(), iQueryable, tenant);
#warning Logitude.BL not alowed in DATA                iQueryable = ProductPermitionsFilter.AddUserProductRestrictionFilters<QuoteOP>(new QueryOperations(), iQueryable, tenant);
            }


#endif


            ///(ORA - 12704: character set mismatch)
            //Devart.Data.Oracle.Entity.OracleEntityProviderServices = true;
            //Multiple Includes Support Improvement in Oracle
            //We got numerous user requests concerning the ORA - 12704 error(“character set mismatch”).The reason of this error was a large number of Includes in the user code, and these Includes, in their turn, resulted in a query with a large number of UNION’s.We added a couple of workaround properties(TypedNulls and StringCastFormat) to deal with this error, which now are obsolete. We have found a possibility to fix this problem without these properties, and these queries are built correctly at the moment.
            //Devart.Data.Oracle.Entity.OracleEntityProviderServices.StringCastFormat = "TO_NCHAR({0})";
            //Devart.Data.Oracle.Entity.OracleEntityProviderServices.TypedNulls = true;
            //            Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance.CodeFirstOptions
            //.UseNonUnicodeStrings = true;
            //            Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance.CodeFirstOptions
            //            .UseNonLobStrings = true;



            IQueryable< QuoteOPList> result = from f in iQueryable

                                                  .Include("CreatedByUser.Contact")
                                                  .Include("SalesmanUser.Contact")


                                                  .Include("ShipmentSubType")
                                                  .Include("FromPartnerAddress").Include("ToPartnerAddress").Include("FromPartnerAddress.Country")
                                                  .Include("ToPort.Country").Include("FromPort.Country")


                                                  .Include("Incoterm").Include("FromPort").Include("Stage").Include("QuoteOPType")
                                                  .Include("TransportMode").Include("Direction").Include("ToPort")//.Include("ShipmentType")

                                                  .Include("MainCarriageCarrierCard").Include("Department").Include("Branch")

                                                  .Include("AgentCard").Include("NotifyCard").Include("MoveType")
                                                  .Include("ToPartnerAddress.Country").Include("FreelancerCard").Include("BusinessUnit").Include("QuoteOPClosingReason")
                                                  .Include("SalesmanUser")

                                              select new QuoteOPList()
                                           {
                                               IsClosed = f.IsClosed,
                                               EstimateProfit = f.EstimateProfit,
                                               EstimateProfitEdited = f.EstimateProfitEdited,
                                               DirectionName = f.Direction == null ? "" : f.Direction.Name,
                                               Id = f.Id,
                                               Shipper = f.ShipperName,
                                               Consignee = f.ConsigneeName,
                                               TransportModeName = f.TransportMode == null ? "" : f.TransportMode.Name,
                                               QuoteViewId = f.Id,
                                               CustomerName = f.CustomerName,
                                               ShipmentType = f.ShipmentType == null ? "" : f.ShipmentType.Name,
                                               Field1 = f.Field1,
                                               Field2 = f.Field2,
                                               Field3 = f.Field3,
                                               Field4 = f.Field4,
                                               Field5 = f.Field5,
                                               Field6 = f.Field6,
                                               Field7 = f.Field7,
                                               Field9 = f.Field9,
                                               Field8 = f.Field8,
                                               Field10 = f.Field10,
                                               LastVersionNumber = f.LastVersionNumber,
                                               ShipperReference1 = f.ShipperReference1,
                                               LastModified = f.LastModified,
                                               CreatedByUser = f.CreatedByUser != null && f.CreatedByUser.Contact != null ? f.CreatedByUser.Contact.EnglishName : "",
                                               QuoteTypeCode = f.QuoteTypeCode,
                                               DirectionId = f.DirectionId,
                                               TransportModeId = f.TransportModeId,
                                               ShipmentTypeId = f.ShipmentTypeId,
                                               ShipperId = f.ShipperId,
                                               FromPortId = f.FromPortId,
                                               ToPortId = f.ToPortId,
                                               QuoteNumber = f.QuoteNumber,
                                               OpenDate = f.OpenDate,
                                               ExpirationDate = f.ExpirationDate,
                                               StartDate = f.StartDate,
                                               MainCarriageCarrierId = f.MainCarriageCarrierId,
                                               //MainCarriageCarrierName = f.MainCarriageCarrierCard == null ? "" : f.MainCarriageCarrierCard.EnglishName,
                                               QuoteTypeName = f.QuoteOPType == null ? "" : f.QuoteOPType.Name,
                                              // CarrierName = f.MainCarriageCarrierCard == null ? "" : f.MainCarriageCarrierCard.EnglishName,
                                               ChargeableWeight = f.ChargeableWeight,
                                               ChargeableWeightInKG = f.ChargeableWeightInKG,
                                               PickupDeliveryChargeableWeight = f.PickupDeliveryChargeableWeight,
                                               PickupDeliveryVolumetricWeight = f.PickupDeliveryVolumetricWeight,
                                               GrossWeight = f.GrossWeight,
                                               GrossWeightInKG = f.GrossWeightInKG,
                                               GrossWeightPerTon = f.GrossWeightPerTon,
                                               VolumeInCBM = f.VolumeInCBM,
                                               IsCancelled = f.IsCancelled,
                                               SearchFields = f.SearchFields,
                                               Notes = f.Notes,
                                               BranchId = f.BranchId,
                                               DepartmentId = f.DepartmentId,
                                               BranchName = f.Branch == null ? null : f.Branch.EnglishName,
                                               MoveTypeName = f.MoveType == null ? null : f.MoveType.MoveTypeEnglishName,
                                               DepartmentName = f.Department == null ? null : f.Department.EnglishName,
                                               NumberOfContainers = f.NumberOfContainers,
                                               FromPartnerId = f.FromPartnerId,
                                               ToPartnerId = f.ToPartnerId,
                                               FromPartnerAddressId = f.FromPartnerAddressId,
                                               ToPartnerAddressId = f.ToPartnerAddressId,
                                               NumberOfPackages = f.NumberOfPackages,
                                               QuoteClosingReasonId = f.QuoteClosingReasonId,
                                               QuoteClosingReasonCode = f.QuoteClosingReasonCode,
                                               QuoteClosingReasonName = f.QuoteOPClosingReason == null ? null : f.QuoteOPClosingReason.Name,
                                               SentDate = f.SentDate,
                                               AcceptedDate = f.AcceptedDate,
                                               DeclinedDate = f.DeclinedDate,
                                               UsageCount = f.UsageCount,
                                               LastUsageDate = f.LastUsageDate,
                                               FreelancerId = f.FreelancerId,
                                               FreelancerAddressId = f.FreelancerAddressId,
                                               FreelancerContactId = f.FreelancerContactId,
                                               FreelancerName = f.FreelancerCard != null ? f.FreelancerCard.EnglishName : null,
                                               BusinessUnitId = f.BusinessUnitId,
                                               BusinessUnitName = f.BusinessUnit == null ? "" : f.BusinessUnit.Name,
                                               SalesmanUserId = f.SalesmanUserId,
                                               Salesman = f.SalesmanUser == null ? "" : (f.SalesmanUser.Contact == null ? "" : f.SalesmanUser.Contact.EnglishName),
                                               SalesmanName = f.SalesmanUser == null ? "" : (f.SalesmanUser.Contact == null ? "" : f.SalesmanUser.Contact.EnglishName),
                                               StageId = f.StageId,
                                               StageName = f.Stage == null ? "" : f.Stage.Name,
                                               StageMaxDays = f.Stage == null ? 0 : (f.Stage.MaxDays==null ?0: (int)f.Stage.MaxDays),
                                               StageDueDate = f.StageDueDate,
                                               RatingCode = f.RatingCode,
                                               RatingName = f.Rating == null ? "" : f.Rating.Name,
                                               RatingIndexOrder = f.Rating == null ? 0 : f.Rating.IndexOrder,
                                               LastActivityDate = f.LastActivityDate,
                                               LastActivitySubject = f.LastActivitySubject,
                                               LastActivityTypeCode = f.LastActivityTypeCode,
                                               NextActivityDate = f.NextActivityDate,
                                               NextActivitySubject = f.NextActivitySubject,
                                               NextActivityTypeCode = f.NextActivityTypeCode,
                                               LastActivityTypeName = f.LastActivityTypeCode == "CL" ? "Call" : (f.LastActivityTypeCode == "TS" ? "Task" : (f.LastActivityTypeCode == "AP" ? "Appointment" : (f.LastActivityTypeCode == "EO" ? "Email Out" : "Email In"))),
                                               NextActivityTypeName = f.NextActivityTypeCode == "CL" ? "Call" : (f.NextActivityTypeCode == "TS" ? "Task" : (f.NextActivityTypeCode == "AP" ? "Appointment" : (f.NextActivityTypeCode == "EO" ? "Email Out" : "Email In"))),
                                               OpportunityId = f.OpportunityId,
                                               IsAutomaticallyClosed = f.IsAutomaticallyClosed,
                                               AutomaticallyCloseDate = f.AutomaticallyCloseDate,
                                               AutomaticallyCloseDays = f.AutomaticallyCloseDays,
                                               UpdateDate = f.UpdateDate,
                                               UpdatedByUserId = f.UpdatedByUserId,
                                             // IncotermCode = f.Incoterm == null ? "" : f.Incoterm.Code,

                                              // FromPortName = f.FromPort == null ? "" : f.FromPort.EnglishName,
                                             //  ToPortName = f.ToPort == null ? "" : f.ToPort.EnglishName,

                                                  //12704 adress.city is NVARCHAR hile other is not !!!
                                                  //12704 FromPort = (f.TransportModeId == "I" && f.DirectionId == "D") ?(f.FromPartnerAddress != null ? f.FromPartnerAddress.City : System.Data.Entity.DbFunctions.AsUnicode( ""))                                               :                                               (f.FromPort != null ? System.Data.Entity.DbFunctions.AsUnicode(f.FromPort.Code) : System.Data.Entity.DbFunctions.AsUnicode("")),


                                                  //FromCountryCode = (f.TransportModeId == "I" && f.DirectionId == "D") ?
                                           //    ((f.FromPartnerAddress != null && f.FromPartnerAddress.Country != null ? f.FromPartnerAddress.Country.Code : "")),
                                             //  :
                                               //((f.FromPort != null && f.FromPort.Country != null ? f.FromPort.Country.Code : "")),

                                               //FromPortCountry = f.FromPort != null && f.FromPort.Country != null ? f.FromPort.Country.EnglishName : "",

                                                  //12704 ToPort = (f.TransportModeId == "I" && f.DirectionId == "D") ?(f.ToPartnerAddress != null ? f.ToPartnerAddress.City : ""):(f.ToPort != null ? f.ToPort.Code : ""),
                                                 // ToCountryCode = (f.TransportModeId == "I" && f.DirectionId == "D") ?((f.ToPartnerAddress != null && f.ToPartnerAddress.Country != null ? f.ToPartnerAddress.Country.Code : "")):((f.ToPort != null && f.ToPort.Country != null ? f.ToPort.Country.Code : "")),

                                               //   ToPortCountry = f.ToPort != null && f.ToPort.Country != null ? f.ToPort.Country.EnglishName : "",

                                                  //12704 Routing = (f.TransportModeId == "I" && f.DirectionId == "D") ?((f.FromPartnerAddress == null ? "" : f.FromPartnerAddress.City) + " > " + (f.ToPartnerAddress == null ? "" : f.ToPartnerAddress.City)):((f.FromPort == null ? "" : f.FromPort.Code) + " > " + (f.ToPort == null ? "" : f.ToPort.Code)),

                                                  Subject = f.Subject,
                                               IsSubjectEdited = f.IsSubjectEdited,
                                               IsFixedPrice = f.IsFixedPrice,
                                               ProductCode = f.ProductCode,
                                               TransitTime = f.TransitTime,
                                               DepartureFrequency = f.DepartureFrequency,
                                               ETD = f.ETD,
                                               ETA = f.ETA,
                                               AgentId = f.AgentId,
                                               AgentAddressId = f.AgentAddressId,
                                               AgentContactId = f.AgentContactId,
                                               AgentReference1 = f.AgentReference1,
                                               AgentReference2 = f.AgentReference2,
                                               AgentName = f.AgentCard == null ? null : f.AgentCard.EnglishName,
                                               LastStageDate = f.LastStageDate,
                                               TEU = f.TEU,
                                               IsSaleCurrencySameAsCost = f.IsSaleCurrencySameAsCost,
                                               IsChargesByVAT = f.IsChargesByVAT,
                                               ValueOfGoods = f.ValueOfGoods,
                                               TotalPerContainer = f.TotalPerContainer,
                                               IsQuoteDataExternal = f.IsQuoteDataExternal,
                                               IsQuoteDocumentExternal = f.IsQuoteDocumentExternal,
                                               QuotationSections = f.QuotationSections,
                                               NotifyId = f.NotifyId,
                                               NotifyAddressId = f.NotifyAddressId,
                                               NotifyContactId = f.NotifyContactId,
                                               NotifyName = f.NotifyCard == null ? null : f.NotifyCard.EnglishName,
                                               NotifyNote = f.NotifyCard == null ? null : f.NotifyCard.Notes,
                                               NumberOfFollowUps = f.NumberOfFollowUps,
                                               CustomerId = f.CustomerId,
                                               IsDangerous = f.IsDangerous,
                                               QuoteHTMLDocumentId = f.QuoteHTMLDocumentId,
                                               Field11 = f.Field11,
                                               Field12 = f.Field12,
                                               Field13 = f.Field13,
                                               Field14 = f.Field14,
                                               Field15 = f.Field15,
                                               Field16 = f.Field16,
                                               Field17 = f.Field17,
                                               Field18 = f.Field18,
                                               Field19 = f.Field19,
                                               Field20 = f.Field20,
                                               RequestDate = f.RequestDate,
                                               EstimatedProfitInLocal = f.EstimatedProfitInLocal,
                                               EstimatedProfitInProfit = f.EstimatedProfitInProfit,
                                               ShipmentSubTypeId = f.ShipmentSubTypeId,
                                               //ShipmentSubTypeName = f.ShipmentSubType == null ? null : f.ShipmentSubType.Name,
                                               RegionalTaxId = f.RegionalTaxId,
                                               RegionalTaxPercentage = f.RegionalTaxPercentage,
                                               IsMultiCurrency = f.IsMultiCurrency,

                                           };
            return result;
		}

		private IQueryable<QuoteOP> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOP> iQueryable, int tenant)
        {
            return iQueryable;
        }
				private IQueryable<QuoteOP> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOP> iQueryable, int tenant)
        {
			return iQueryable;
		}

        public List<QuoteOPList> GetRecentEntityLists(string ownerId, string businessUnitId, int tenant, string userId, string objectTableId)
        {
            List<QuoteOPList> entityList = new List<QuoteOPList>();

            EntityLastActivityRepository entityLastActivityRepository = new EntityLastActivityRepository(tenant);
            List<EntityLastActivity> lastActivities = entityLastActivityRepository.GetTopEntityLastActivities(tenant, userId, objectTableId).ToList();

            List<string> ids = new List<string>();
            foreach (EntityLastActivity activity in lastActivities)
            {
                ids.Add(activity.EntityId);
            }

            var repository = new QuoteOPRepository(tenant);
            var entities = repository.GetQuotes(tenant);

            QuoteOPBusinessUnitFilter businessUnitFilter = new QuoteOPBusinessUnitFilter(tenant);
            entities = businessUnitFilter.RunFilter(entities);

            if (!string.IsNullOrEmpty(ownerId))
            {
                entities = entities.Where(d => d.SalesmanUserId == ownerId);
            }

            if (!string.IsNullOrEmpty(businessUnitId))
            {
                entities = entities.Where(d => d.BusinessUnitId == businessUnitId);
            }


            var qqq = this.GetIqueryableList(entities);

            foreach (EntityLastActivity lastActivity in lastActivities)
            {
                var q = (from d in qqq///this.GetIqueryableList(entities)
                         where d.Id == lastActivity.EntityId
                         select d);
                try
                {


                    var f = q.FirstOrDefault();

                    if (f != null)
                    {


                        entityList.Add(f);
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
#if false
            if (entityList.Count > 0)
            {
                entityList = BranchPermitionsFilter.AddUserBranchRestrictionFilters<QuoteOPList>(new QueryOperations(), entityList.AsQueryable<QuoteOPList>(), tenant).ToList();
                entityList = ProductPermitionsFilter.AddUserProductRestrictionFilters<QuoteOPList>(new QueryOperations(), entityList.AsQueryable<QuoteOPList>(), tenant).ToList();
            }
#endif
            return entityList;
        }


    }


}
	