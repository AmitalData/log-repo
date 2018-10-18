using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class PartnersDomainService
    {
        public void UpdateTruckerList(TruckerList currentEntity)
        {
        }

        public IQueryable<Trucker> GetTruckers(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Trucker", "READ", tenant);

            truckerRepository = new TruckerRepository(tenant);
            return truckerRepository.GetTruckers(0);
        }

        public IQueryable<TruckerPM> GetTruckersByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Trucker", "READ", tenant);

            truckerQuery = new TruckerQuery(tenant);
            return truckerQuery.GetTruckerPMsByTenant(tenant);
        }

        public bool DoesTruckerCodeExist(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            truckerRepository = new TruckerRepository(tenant);
            return (truckerRepository.GetTruckers(tenant).Where(d => d.Card.Code == code && d.Tenant == tenant)).Any();
        }

        public TruckerPM GetTruckerById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Trucker", "READ", tenant);

            truckerQuery = new TruckerQuery(tenant);
            return truckerQuery.GetSinglePM(id, tenant);
        }

        public TruckerList GetSingleTruckerList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Trucker", "READ", tenant);

            truckerRepository = new TruckerRepository(tenant);
            TruckerList truckerList = null;
            Trucker trucker = truckerRepository.GetSingleTrucker(id, tenant);

            if (trucker != null)
            {
                List<Trucker> singleEntityList = new List<Trucker>();
                singleEntityList.Add(trucker);

                truckerQuery = new TruckerQuery(truckerRepository);
                IQueryable<Trucker> iQueryable = singleEntityList.AsQueryable();
                IQueryable<TruckerList> iQueryableEntityList = truckerQuery.GetIQueryableEntityList(iQueryable);
                truckerList = iQueryableEntityList.FirstOrDefault();
            }
            return truckerList;
        }

        public TruckerPM GetTruckerByCode(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Trucker", "READ", tenant);

            truckerQuery = new TruckerQuery(tenant);
            return truckerQuery.GetSinglePMByCode(code, tenant);
        }

        public IQueryable<TruckerList> GetTruckerLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Trucker", "READ", tenant);

            truckerRepository = new TruckerRepository(tenant);
            truckerQuery = new TruckerQuery(truckerRepository);

            IQueryable<Trucker> truckers = truckerRepository.GetTruckers(tenant);
            IQueryable<TruckerList> query2 = truckerQuery.GetIQueryableEntityList(truckers);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<TruckerList> GetTruckerFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Trucker", "READ", tenant);

            truckerRepository = new TruckerRepository(tenant);
            truckerQuery = new TruckerQuery(truckerRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Trucker> truckers = truckerRepository.GetTruckers(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            TruckerCustomFilter customfilters = new TruckerCustomFilter(tenant);
            truckers = customfilters.GetFilteredQuery(queryOperations, truckers);

            truckers = filter.GetFilteredQuery<Trucker>(nonListQueryOperation, truckers);
            int skippedLines = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);

            IQueryable<TruckerList> query2 = truckerQuery.GetIQueryableEntityList(truckers);
            query2 = filter.GetFilteredQuery<TruckerList>(listQueryOperation, query2);
            
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(TruckerList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Trucker", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "ntext":
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<TruckerList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<TruckerList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<TruckerList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<TruckerList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<TruckerList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<TruckerList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Code);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Code);
            }
            
            query2 = query2.Skip(skippedLines);
            query2 = query2.Take(queryOperations.PageSize);

            return query2;
        }

        public int GetTruckerFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Trucker", "READ", tenant);

            truckerRepository = new TruckerRepository(tenant);
            truckerQuery = new TruckerQuery(truckerRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Trucker> truckers = truckerRepository.GetTruckers(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            
            TruckerCustomFilter customfilters = new TruckerCustomFilter(tenant);
            truckers = customfilters.GetFilteredQuery(queryOperations, truckers);

            truckers = filter.GetFilteredQuery<Trucker>(nonListQueryOperation, truckers);

            IQueryable<TruckerList> query2 = truckerQuery.GetIQueryableEntityList(truckers);
            query2 = filter.GetFilteredQuery<TruckerList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public IQueryable<TruckerPM> GetTruckerSearch(string code, string name, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Trucker", "READ", tenant);

            truckerQuery = new TruckerQuery(tenant);
            IQueryable<TruckerPM> q = truckerQuery.GetTruckersByNameOrCode(code, name, tenant).Where(d => d.Tenant == tenant);
            return q;
        }

        public void InsertTrucker(TruckerPM trucker)
        {
            SecurityUtility.AuthenticationOnTenant(trucker.Tenant);
            SecurityUtility.CheckContactFeature("Trucker", "NEW", trucker.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(trucker.Tenant);
            }
           
            truckerRepository = new TruckerRepository(objectContext);
            AddressRepository = new AddressRepository(objectContext);
            CardRepository = new CardRepository(objectContext);
            ContactRepository = new ContactRepository(objectContext);
            CardContactRepository = new CardContactRepository(objectContext);

            bool exist = (from a in truckerRepository.GetTruckers(trucker.Tenant)
                          where a.Card.Code == trucker.Code && a.Tenant == trucker.Tenant
                          select a).Any();
            if (!exist)
            {
                TruckerService service = new TruckerService(objectContext , trucker.Tenant);
                service.Create(trucker);
            }

            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", trucker.Tenant);
                msg = msg.Replace("%Entity", "Trucker");
                throw new Exception(msg);
            }
        }

        public void UpdateTrucker(TruckerPM currentTrucker)
        {
            SecurityUtility.AuthenticationOnTenant(currentTrucker.Tenant);
            SecurityUtility.CheckContactFeature("Trucker", "UPDATE", currentTrucker.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentTrucker.Tenant);
            }

            truckerRepository = new TruckerRepository(objectContext);
            CardRepository = new CardRepository(objectContext);
            cardQuery = new CardQuery(CardRepository);
            ContactRepository = new ContactRepository(objectContext);

            List<CardExternalCodeByCurrencyPM> cardExternalCodeByCurrenciesChangeSet = ChangeSet.GetAssociatedChanges(currentTrucker, d => d.CardExternalCodeByCurrencies).Cast<CardExternalCodeByCurrencyPM>().ToList();
            foreach (CardExternalCodeByCurrencyPM itemPM in cardExternalCodeByCurrenciesChangeSet)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert: { itemPM.ChangeSetOp = ChangeSetOperation.Insert; break; }
                    case ChangeOperation.Delete: { itemPM.ChangeSetOp = ChangeSetOperation.Delete; break; }

                    case ChangeOperation.Update:
                        {
                            itemPM.ChangeSetOp = ChangeSetOperation.Update;
                            break;
                        }

                    default: { itemPM.ChangeSetOp = ChangeSetOperation.None; break; }
                }
            }


            CardPM c = cardQuery.GetSinglePM(currentTrucker.Id, currentTrucker.Tenant);

            bool exist = (from a in truckerRepository.GetTruckers(currentTrucker.Tenant)
                          where a.Card.Code == c.Code
                          && a.Id != currentTrucker.Id && a.Tenant == currentTrucker.Tenant
                          select a).Any();
            if (!exist)
            {

                TruckerService service = new TruckerService(objectContext, currentTrucker.Tenant);
                service.SetChangeSet(cardExternalCodeByCurrenciesChangeSet);
                service.Update(currentTrucker);
            }

            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", currentTrucker.Tenant);
                msg = msg.Replace("%Entity", "Trucker");
                throw new Exception(msg);
            }
        }

        public void DeleteTrucker(TruckerPM trucker)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(trucker.Tenant);
            }
            truckerRepository = new TruckerRepository(objectContext);
            Trucker entity = truckerRepository.GetSingleTrucker(trucker.Id, trucker.Tenant);
            truckerRepository.Remove(entity);
        }
    }
}
