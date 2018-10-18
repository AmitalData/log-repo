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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class DeclarationCourierStatusListQueryService
    {
	    private IQueryable<DeclarationCourierStatusList> GetIqueryableList(IQueryable<DeclarationCourierStatus> iQueryable)
        {
		IQueryable<DeclarationCourierStatusList> query = (from a in iQueryable
                                                          join d in context.Declarations.Include("GovernmentProcedureCurrent").Include("CourierCustomStatus").Include("DeclarationStatusType").Include("CustomerCard").Include("Importer").Include("AgentTalkBackType")
                                                          on a.DeclarationId equals d.Id
                                                          join c in context.CourierDeclarations
                                                          on a.DeclarationId equals c.DeclarationId
                                                          select new DeclarationCourierStatusList()
											                {
                                                                DeclarationId = a.DeclarationId,
					                                            Tenant = a.Tenant,
                                                                CourierMasterId = c.CourierMasterId,
                                                                IsDOCTab = (a.DocumentStatusCode == "M" || a.DocumentStatusCode == "X"),
                                                                IsSVGTab = a.IsCourierMissingClassification == true,
                                                                IsMNFRTab = (a.CourierManifestStatusCode == "R"),
                                                                IsDECRTab = (a.CourierDeclarationStatusCode == "R"),
                                                                IsHOLDTab = (a.CourierPendingReasonCode != null),
                                                                IsMNFTab = (a.CourierManifestStatusCode == "M" || a.CourierManifestStatusCode =="X"),
                                                                IsPAYTab = a.CourierPaymentStatusCode == "R",
                                                                IsDECTab = (a.CourierDeclarationStatusCode == "M" || a.CourierDeclarationStatusCode == "X"),
                                                                CourierManifestStatusCode = a.CourierManifestStatusCode,
                                                                CourierDeclarationStatusCode = a.CourierDeclarationStatusCode,
                                                                CourierPaymentStatusCode = a.CourierPaymentStatusCode,
                                                                IsCourierMissingClassification = a.IsCourierMissingClassification,
                                                                IsClosedForFollowUp = a.IsClosedForFollowUp,
                                                                HighLowValue = a.HighLowValue,
                                                                DocumentStatusCode = a.DocumentStatusCode,
                                                                CourierHawb = d.CourierHAWB,
                                                                ProcedureCurrentCode = d.ProcedureCurrentCode,
                                                                ProcedureCurrentName = d.GovernmentProcedureCurrent != null ? d.GovernmentProcedureCurrent.LocalName : null,
                                                                CourierCustomStatusCode = d.CourierCustomStatusCode,
                                                                CourierCustomStatusName = d.CourierCustomStatus != null ? d.CourierCustomStatus.LocalName : null,
                                                                DeclarationStatusTypeName = d.DeclarationStatusType == null ? null : d.DeclarationStatusType.LocalName,
                                                                ImporterCode = d.ImporterCode,
                                                                ImporterName = d.ImporterId != null ? d.Importer.FullName : d.ImporterName,
                                                                CustomerName = d.CustomerCard.LocalName != null ? d.CustomerCard.LocalName : d.CustomerCard.EnglishName,
                                                                CourierSearchFields = d.CourierSearchFields,
                                                                TotalInvoiceAmountInUSD = a.TotalInvoiceAmountInUSD,
                                                                DeclarationNumber = d.DeclarationNumber,
                                                                CourierPendingReasonCode = a.CourierPendingReasonCode,
                                                                CourierPendingReasonName = a.CourierPendingReason != null ? a.CourierPendingReason.LocalName : null,
                                                                PendingRemarks = a.PendingRemarks,
                                                                CourierSuspentionReasonName = d.CourierSuspentionReasonCode != null ? d.AgentTalkBackType.LocalName : null,
                                                          });
            return query;
		}

		private IQueryable<DeclarationCourierStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DeclarationCourierStatus> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	