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
using Logitude.Customs.Data.CustomFilters;

namespace Logitude.Customs.Data.EntityListQueryServices
{

    public partial class DeclarationReferantDataListQueryService
    {
        private IQueryable<DeclarationReferantDataList> GetIqueryableList(IQueryable<DeclarationReferantData> iQueryable)
        {
            IQueryable<DeclarationReferantDataList> query = (from a in iQueryable.Include("CustomsVendor")
                                                             join d in context.Declarations
                                                             /*
LEFT OUTER JOIN AMINETNXT_MAIN.Cards Extent3 ON Extent2.CustomerId = Extent3.Id
LEFT OUTER JOIN AMINETNXT_MAIN.Customers Extent7 ON Extent3.Id = Extent7.Id
due Extent7.LogBoxActivated,
*/

                                                             .Include("CustomerCard")
                                                             .Include("DeclarationOffice")
                                                             .Include("DeclarationStatusType")
                                                             .Include("Importer")
                                                             on a.DeclarationId equals d.Id
                                                             select new DeclarationReferantDataList()
                                                             {
                                                                 Tenant = a.Tenant,

                                                                 DeclarationId = a.DeclarationId,

                                                                 OrderNumber = a.OrderNumber,

                                                                 EstimatedArrivalDate = a.EstimatedArrivalDate,

                                                                 Weight = a.Weight,

                                                                 ClassificationStatus = a.ClassificationStatus,

                                                                 ControllerStatus = a.ControllerStatus,

                                                                 CollectionOfMoneyStatus = a.CollectionOfMoneyStatus,

                                                                 FollowUpDate = a.FollowUpDate,

                                                                 WithPaper = a.WithPaper,

                                                                 IsClosedForFollowUp = a.IsClosedForFollowUp,

                                                                 IsClassificationRemarks = a.IsClassificationRemarks,

                                                                 IsControllerRemarks = a.IsControllerRemarks,

                                                                 PreClassification = a.PreClassification,

                                                                 CustomFileNo = d.CustomFileNo,

                                                                 CustomerName = d.CustomerCard.LocalName,

                                                                 TransportModeId = d.TransportModeId,

                                                                 DeclarationOfficeCode = d.DeclarationOfficeCode,
                                                                 DeclarationOfficeName= d.DeclarationOffice.LocalName,

                                                                 VendorName = a.CustomsVendor.VendorName,
                                                                 ArrivalDate = a.ArrivalDate != null ? a.ArrivalDate : a.EstimatedArrivalDate,
                                                                 ATAOrETA = a.ArrivalDate != null ? "ATA" : "ETA",

                                                                 DeclarationStatusTypeName = d.DeclarationStatusType == null ? null : d.DeclarationStatusType.LocalName,

                                                                 DeclarationStatusTypeCode = d.DeclarationStatusTypeCode,
                                                                 ExceptionReasonsList = a.ExceptionReasonsList,
                                                                 ReferentUserId = d.ReferentUserId,
                                                                 DepartmentId = d.DepartmentId,
                                                                 AvailabilityDate = d.AvailabilityDate,
 
                                                                 NewFile = a.NewFile,
                                                                 Favorite = a.Favorite,
                                                                 ///itzik : 
                                                                  IsCustomerLogBoxActivated = d.CustomerCard.Customer.LogBoxActivated,
                                                                 SortedColumns = (a.NewFile && a.Favorite ? 1 : (a.NewFile ? 2 : (a.Favorite ? 3 : 4))),
                                                                 IsCancelled = d.IsCancelled,

                                                                 /*
LEFT OUTER JOIN AMINETNXT_MAIN.Contacts Extent8 ON Extent1.ClassifiedUserId = Extent8.Id
LEFT OUTER JOIN AMINETNXT_MAIN.Contacts Extent9 ON Extent9.Id = Extent1.CollectorUserId
LEFT OUTER JOIN AMINETNXT_MAIN.Contacts Extent10 ON Extent10.Id = Extent1.ControllerUserId
                                                                  */
                                                                 ClassifiedUserName = a.ClassifiedUser.Contact.LocalName,
                                                                 CollectorUserName = a.CollectorUser.Contact.LocalName,
                                                                 ControllerUserName= a.ControllerUser.Contact.LocalName, 

                                                                 LastStatusDate = a.LastStatusDate,
                                                                 LastStatusName = a.LastStatusName,
                                                                 OrderMoney = a.OrderMoney,
                                                                 StorageSiteCode=d.StorageSiteCode,
                                                                 HatraDate=d.HatraDate,
                                                                 PaymentDate=d.PaymentDate,
                                                                 TaxationDateTime=d.TaxationDateTime,
                                                                 CustomerCode = d.CustomerCard == null ? null : d.CustomerCard.Code,
                                                                 ImporterCode=d.ImporterCode,
                                                                  ProcedureCurrentCode = d.ProcedureCurrentCode,
                                                                 ImporterFile=a.ImporterFile,
                                                                 AEOImporter=d.Importer.FacilitationTypeCode,
                                                                 Team = a.ReferantTeam.LocalName,
                                                                 FileOpenDate=a.FileOpenDate,
                                                                 ProcedureCurrentName = d.GovernmentProcedureCurrent.LocalName,
                                                                 StorageSiteName=d.StorageSiteName,
                                                                 ImporterName = d.Importer != null ? d.Importer.FullName : d.ImporterName,

                                                             }) ;
                                                                
                                              return query;

        }

        private IQueryable<DeclarationReferantData> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<DeclarationReferantData> iQueryable, int tenant)
        {
            DeclarationReferantDataCustomFilters filters = new DeclarationReferantDataCustomFilters();

            var filter = queryOperations.QueryFilterItems.FirstOrDefault(x => x.FieldName == "RetrievData");
            if (filter == null)
            {

                iQueryable = filters.GetFilteredQuery(iQueryable);
            }
            iQueryable = filters.GetFreelancerDeclarationReferantDatas(queryOperations, iQueryable, tenant, context);

            return iQueryable;

        }
    }


}
