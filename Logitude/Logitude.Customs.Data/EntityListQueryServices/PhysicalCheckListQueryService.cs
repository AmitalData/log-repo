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
using Logitude.Customs.Data.Repsitories;
using System.Globalization;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Customs.Data.Utils;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class PhysicalCheckListQueryService
    {
	    private IQueryable<PhysicalCheckList> GetIqueryableList(IQueryable<PhysicalCheck> iQueryable)
        {
            var qJoin =
(from p in context.CourierDeclarations


 join sts1 in context.CourierMasters.Include("Card")
                   on p.CourierMasterId equals sts1.Id
                   into CourierMasterJoin



from myCourierMasterJoin in CourierMasterJoin

 select new { p.CourierMaster,p.DeclarationId,p.CourierMaster.Card}
);


            var qMyJoin =
                 (
                 from rec in qJoin
                 select new MyCourierMasterPhysicalCheckJoin
                 {
                     DeclarationId = rec.DeclarationId,
                      IntegratorName = rec.CourierMaster.Card != null ? rec.CourierMaster.Card.LocalName: null,
                      IntegratorCode = rec.CourierMaster != null ? rec.CourierMaster.IntegratorCode: null,

                 });
            int tenant = 1;
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                tenant = authToken.Tenant;
            }
            catch (Exception)
            {

                // throw;
            }
            bool isCourierEnv = context.CustomsSettings.FirstOrDefault(r => r.Tenant == tenant).CompanyType == "B";
            if (!isCourierEnv)
            {
                qMyJoin = (from rec in context.CourierDeclarations.Where(r => r.DeclarationId == "-1")
                           select new MyCourierMasterPhysicalCheckJoin()
                           {
                               DeclarationId = rec.DeclarationId,
                               IntegratorCode = "",
                               IntegratorName="",
                           });
            }

            IQueryable<PhysicalCheckList> query = (from a in iQueryable.Include("CargoIdentifireType").Include("CheckSite").Include("StorageSite").Include("CheckQueueType").Include("Operation").Include("Declaration.CustomerCard").Include("CheckTypeLookup")
                                                   join d in context.Declarations
                                                   on a.DeclarationId equals d.Id into xy
                                                   from s in xy.DefaultIfEmpty()

                                                   join recJoin in qMyJoin
                                                                on a.DeclarationId equals recJoin.DeclarationId
                                                                into qrecJoin
                                                   from myJoin in qrecJoin.DefaultIfEmpty()

                                                   select new PhysicalCheckList()
                                                            {
                                                              
                                                                Id = a.Id,
                                                                CargoIdentifierKey1 =a.CargoIdentifierKey1,
                                                                CargoIdentifierKey2 = a.CargoIdentifierKey2,
                                                                CargoIdentifierKey3 = a.CargoIdentifierKey3,
                                                                CargoIdentifierTypeCode = a.CargoIdentifierTypeCode,
                                                                CargoIdentifierTypeName = a.CargoIdentifireType.EnglishName,
                                                                CargoTypeCode = a.CargoTypeCode,
                                                                CheckEssence = a.CheckEssence,
                                                                CheckId = a.CheckId,
                                                                CheckSiteCode = a.CheckSiteCode,
                                                                CheckSiteName = a.CheckSite.LocalName!=null?a.CheckSite.LocalName:a.CheckSite.EnglishName,
                                                                ContainerNubmer = a.ContainerNubmer,
                                                                DeclarationId = a.DeclarationId,
                                                               DeclarationNo = s.DeclarationNumber,
                                                                ImporterNumber = a.ImporterNumber,
                                                                InitiatorTypeCode = a.InitiatorTypeCode,
                                                                IsClosed = a.IsClosed,
                                                                LimitDate = a.LimitDate,
                                                                Tenant = a.Tenant,
                                                               StorageSiteName = a.StorageSite.LocalName!=null?a.StorageSite.LocalName:a.StorageSite.EnglishName,
                                                                StorageSiteCode = a.StorageSiteCode,
                                                                StatusMessageCode = a.StatusMessageCode,
                                                                RowNumber = a.RowNumber,
                                                                QueueTypeName = a.CheckQueueType.LocalName != null ? a.CheckQueueType.LocalName : a.CheckQueueType.EnglishName,
                                                                QueueTypeCode= a.QueueTypeCode,
                                                               OperationName= a.Operation.LocalName,
                                                                OperationCode = a.OperationCode,
                                                                OpenDate = a.OpenDate,
                                                                SearchFields = a.SearchFields,
                                                                 CustomerName=s.CustomerCard.LocalName!=null?s.CustomerCard.LocalName:s.CustomerCard.EnglishName,
                                                                 CustomerCode= s.CustomerCard != null? s.CustomerCard.Code : null,
                                                                 CustomFileNo= s.CustomFileNo,
                                                                StatusMessageName = a.StatusMessage == null ? null : a.StatusMessage.LocalName,
                                                                IsComprehensiveCheck = a.IsComprehensiveCheck,
                                                                CheckTypeCode = a.CheckTypeCode,
                                                                CheckTypeName = a.CheckTypeLookup != null? a.CheckTypeLookup.LocalName : null,
                                                                VehicleChassisNumber = a.VehicleChassisNumber,
                                                                CustomerId = a.CustomerId,
                                                                NoEscortRequired=a.NoEscortRequired,
                                                               DeclarationOfficeName = s.DeclarationOffice == null ? null : s.DeclarationOffice.LocalName,
                                                       IntegratorCode = myJoin != null ? myJoin.IntegratorCode : null,
                                                       IntegratorName = myJoin != null ? myJoin.IntegratorName : null,
                                                       AvailabilityDate = s.AvailabilityDate,

                                                   });
        
            return query;
		}

        private IQueryable<PhysicalCheck> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<PhysicalCheck> iQueryable, int tenant)
        {
            iQueryable = GetFreelancerQuery(iQueryable, tenant);
            return iQueryable;
        }

        public IQueryable<PhysicalCheck> GetFreelancerQuery(IQueryable<PhysicalCheck> queryableData, int tenant)
        {
            FreelancerCustomersUtil frlUtil = new FreelancerCustomersUtil(tenant);
            if (frlUtil.user.IsFreelancer)
            {
                List<string> customersIds = frlUtil.GetConnectedCustomersIds(tenant);
                if (customersIds.Count > 0)
                {
                    queryableData = queryableData.Where(d => customersIds.Contains(d.CustomerId));
                }
            }

            return queryableData;
        }

    }

    public class MyCourierMasterPhysicalCheckJoin
    {
        public string IntegratorCode { get; set; }
        public string IntegratorName { get; set; }

        public string DeclarationId { get; set; }

    }
}
	