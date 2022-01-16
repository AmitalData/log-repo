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

using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.EntityLists;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.Helpers;
using Logitude.TariffModule.Data.Repositories;

namespace Logitude.TariffModule.Data.EntityListQueryServices
{

    public partial class TariffListQueryService
    {
        TariffTypeRepository tariffTypeRepository;
        TariffRepository tariffRepository;

        private IQueryable<TariffList> GetIqueryableList(IQueryable<Tariff> iQueryable)
        {
            IQueryable<TariffList> query = (from a in iQueryable
                                            select new TariffList()
                                            {
                                                Id = a.Id,
                                                Tenant = a.Tenant,
                                                CreateDate = a.CreateDate,
                                                CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact != null ? a.CreatedByUser.Contact.EnglishName : "" : "",
                                                SearchFields = a.SearchFields,
                                                StartDate = a.StartDate,
                                                ExpirationDate = a.ExpirationDate,
                                                Name = a.Name,
                                                InActive = a.InActive,
                                                Notes = a.Notes,
                                                SellerName = a.Seller != null ? a.Seller.EnglishName : "",
                                                UpdateDate = a.UpdateDate,
                                                UpdatedByUserName = a.UpdatedByUser != null ? a.UpdatedByUser.Contact != null ? a.UpdatedByUser.Contact.EnglishName : "" : "",
                                                TariffNumber = a.TariffNumber,
                                                LastUsedDate = a.LastUsedDate,
                                                TypeCode = a.TypeCode,
                                                CustomsBrokerName = a.CustomsBroker != null ? a.CustomsBroker.EnglishName : "",
                                            });
            return query;
        }

        public List<TariffList> GetRecentTariffs(string userId, int tenant)
        {
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Tariff", 0, true);
            
            IQueryable<TariffList> myResult = this.GetRecentEntityLists(tenant, userId, objectTable.Id).AsQueryable();

            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            customFieldResolver.SetCustomFieldsValues("Tariffs", tenant, myResult.Cast<object>().ToList());
            return myResult.ToList();
        }
        public List<TariffList> GetRecentEntityLists(int tenant, string userId, string objectTableId)
        {
            List<TariffList> entityList = new List<TariffList>();
            List<EntityLastActivity> lastActivities = GetLastActivityList(tenant, userId, objectTableId);
            List<string> lastActivetyIds = GetLastActivityIds(lastActivities);
            IQueryable<Tariff> entities = GetTariffsEntitysFromIds(lastActivetyIds, tenant);

            foreach (EntityLastActivity lastActivity in lastActivities)
            {
                Tariff a = (from d in entities.Include("UpdatedByUser.Contact").Include("CreatedByUser.Contact").Include("Seller").Include("CustomsBroker")
                            where d.Id == lastActivity.EntityId
                            select d).FirstOrDefault();

                if (a != null)
                {
                    TariffList list = new TariffList()
                    {
                        Id = a.Id,
                        Tenant = a.Tenant,
                        CreateDate = a.CreateDate,
                        CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact != null ? a.CreatedByUser.Contact.EnglishName : "" : "",
                        SearchFields = a.SearchFields,
                        StartDate = a.StartDate,
                        ExpirationDate = a.ExpirationDate,
                        Name = a.Name,
                        InActive = a.InActive,
                        Notes = a.Notes,
                        SellerName = a.Seller != null ? a.Seller.EnglishName : "",
                        UpdateDate = a.UpdateDate,
                        UpdatedByUserName = a.UpdatedByUser != null ? a.UpdatedByUser.Contact != null ? a.UpdatedByUser.Contact.EnglishName : "" : "",
                        TariffNumber = a.TariffNumber,
                        ContractNumber = a.ContractNumber,
                        TypeCode = a.TypeCode,
                        PriceSteps = a.PriceSteps,
                        LastActivityDate = lastActivity.ActivityDate,
                        LastActivityTypeName = lastActivity.ActivityType.Name,
                        LastActivityByUserName = lastActivity.User.Contact.EnglishName,
                        CurrencyId = a.CurrencyId,
                        LastUsedDate = a.LastUsedDate,
                        CustomsBrokerName = a.CustomsBroker != null ? a.CustomsBroker.EnglishName : "",
                    };

                    TariffType tariffType = GetTariffType(a.TypeCode, tenant);
                    list.TypeName = tariffType.Name != null ? tariffType.Name : "";
                    list.TransportModeCode = tariffType.TransportModeCode != null ? tariffType.TransportModeCode : "";
                    list.TransportModeName = GetTransportModeName(tariffType.TransportModeCode, tenant);
                    entityList.Add(list);
                }
            }
            return entityList;
        }

        private string GetTransportModeName(string code, int tenant)
        {
            TransportModeRepository transportModeRepository = new TransportModeRepository(tenant);
            TransportMode transportMode = transportModeRepository.GetSingleTransportMode(code);
            string transportModeName = "";
            if (transportMode != null)
            {
                transportModeName = transportMode.Name != null ? transportMode.Name : ""; 
            }
            return transportModeName;
        }

        private TariffType GetTariffType(string typeCode, int tenant)
        {
            tariffTypeRepository = new TariffTypeRepository(tenant);
            return tariffTypeRepository.GetSingle(typeCode);
        }
        
        private List<string> GetLastActivityIds(List<EntityLastActivity> lastActivities)
        {
            List<string> ids = new List<string>();
            foreach (EntityLastActivity activity in lastActivities)
            {
                ids.Add(activity.EntityId);
            }
            return ids;
        }
        private IQueryable<Tariff> GetTariffsEntitysFromIds(List<string> lastActivetyIds, int tenant)
        {
            tariffRepository = new TariffRepository(tenant);
            return tariffRepository.GetAllFromIdList(lastActivetyIds, tenant);
        }
        private List<EntityLastActivity> GetLastActivityList(int tenant, string userId, string objectTableId)
        {
            EntityLastActivityRepository entityLastActivityRepository = new EntityLastActivityRepository(tenant);
            List<EntityLastActivity> lastActivities = entityLastActivityRepository.GetTopEntityLastActivities(tenant, userId, objectTableId).ToList();
            return lastActivities;
        }
        private IQueryable<Tariff> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Tariff> iQueryable, int tenant)
        {
            return iQueryable;
        }

        private IQueryable<Tariff> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<Tariff> iQueryable, int tenant)
        {
            return iQueryable;
        }
    }
}
	