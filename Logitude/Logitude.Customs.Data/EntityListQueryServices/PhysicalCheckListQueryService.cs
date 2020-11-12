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

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class PhysicalCheckListQueryService
    {
	    private IQueryable<PhysicalCheckList> GetIqueryableList(IQueryable<PhysicalCheck> iQueryable)
        {
            IQueryable<PhysicalCheckList> query = (from a in iQueryable.Include("CargoIdentifireType").Include("CheckSite").Include("StorageSite").Include("CheckQueueType").Include("Operation").Include("Declaration.CustomerCard").Include("CheckTypeLookup")
                                                   join d in context.Declarations
                                                   on a.DeclarationId equals d.Id into xy
                                                   from s in xy.DefaultIfEmpty() 
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
                                                              //  DeclarationId = a.DeclarationId,
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


}
	