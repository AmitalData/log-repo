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
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class PartnersDomainService
    {
        public void UpdateShippingLine(ShippingLineList currentEntity)
        {
        }

        public IQueryable<ShippingLine> GetShippingLines(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ShippingLine", "READ", tenant);

            shippingLineRepository = new ShippingLineRepository(tenant);
            return shippingLineRepository.GetShippinngLines(0);
        }

        public IQueryable<ShippingLinePM> GetShippingLinesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ShippingLine", "READ", tenant);

            shippingLineQuery = new ShippingLineQuery(tenant);
            return shippingLineQuery.GetShippinngLinePMsByTenant(tenant);
        }

        public bool DoesShippingLineCodeExist(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            shippingLineRepository = new ShippingLineRepository(tenant);
            return (shippingLineRepository.GetShippinngLines(tenant).Where(d => d.Card.Code == code && d.Tenant == tenant)).Any();
        }

        public ShippingLinePM GetShippingLineById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ShippingLine", "READ", tenant);

            shippingLineQuery = new ShippingLineQuery(tenant);
            return shippingLineQuery.GetSinglePM(id, tenant);
        }

        public ShippingLinePM GetShippingLineByCode(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ShippingLine", "READ", tenant);

            shippingLineQuery = new ShippingLineQuery(tenant);
            return shippingLineQuery.GetSinglePMByCode(code, tenant);
        }

        public ShippingLineList GetSingleShippingLineList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ShippingLine", "READ", tenant);

            shippingLineRepository = new ShippingLineRepository(tenant);
            ShippingLineList shippingLineList = null;
            ShippingLine shippingLine = shippingLineRepository.GetSingleShippingLine(id, tenant);

            if (shippingLine != null)
            {
                List<ShippingLine> singleEntityList = new List<ShippingLine>();
                singleEntityList.Add(shippingLine);

                shippingLineQuery = new ShippingLineQuery(shippingLineRepository);
                IQueryable<ShippingLine> iQueryable = singleEntityList.AsQueryable();
                IQueryable<ShippingLineList> iQueryableEntityList = shippingLineQuery.GetIQueryableEntityList(iQueryable);
                shippingLineList = iQueryableEntityList.FirstOrDefault();
            }

            if (shippingLine.ShippingAgent != null)
            {
                if (shippingLine.ShippingAgent.Card != null)
                {
                    shippingLineList.ShippingAgentEnglishName = shippingLine.ShippingAgent.Card.EnglishName;
                }
            }

            return shippingLineList;
        }

        public IQueryable<ShippingLineList> GetShippingLineLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ShippingLine", "READ", tenant);

            shippingLineRepository = new ShippingLineRepository(tenant);
            shippingLineQuery = new ShippingLineQuery(shippingLineRepository);

            IQueryable<ShippingLine> shippingLines = shippingLineRepository.GetShippinngLines(tenant);
            IQueryable<ShippingLineList> query2 = shippingLineQuery.GetIQueryableEntityList(shippingLines);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ShippingLineList> GetShippingLineFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ShippingLine", "READ", tenant);

            shippingLineRepository = new ShippingLineRepository(tenant);
            shippingLineQuery = new ShippingLineQuery(shippingLineRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<ShippingLine> shippingLines = shippingLineRepository.GetShippinngLines(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            ShippingLineCustomFilter customfilters = new ShippingLineCustomFilter(tenant);
            shippingLines = customfilters.GetFilteredQuery(queryOperations, shippingLines);

            shippingLines = filter.GetFilteredQuery<ShippingLine>(nonListQueryOperation, shippingLines);
            int skippedLines = queryOperations.PageIndex;
            IQueryable<ShippingLineList> query2 = shippingLineQuery.GetIQueryableEntityList(shippingLines);
            query2 = filter.GetFilteredQuery<ShippingLineList>(listQueryOperation, query2);
            
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ShippingLineList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ShippingLine", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ShippingLineList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ShippingLineList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ShippingLineList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ShippingLineList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<ShippingLineList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ShippingLineList, bool>(queryOperations, query2);
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

        public int GetShippingLineFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ShippingLine", "READ", tenant);

            shippingLineRepository = new ShippingLineRepository(tenant);
            shippingLineQuery = new ShippingLineQuery(shippingLineRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<ShippingLine> shippingLines = shippingLineRepository.GetShippinngLines(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            ShippingLineCustomFilter customfilters = new ShippingLineCustomFilter(tenant);
            shippingLines = customfilters.GetFilteredQuery(queryOperations, shippingLines);

            shippingLines = filter.GetFilteredQuery<ShippingLine>(nonListQueryOperation, shippingLines);
            int skippedLines = queryOperations.PageIndex;

            IQueryable<ShippingLineList> query2 = shippingLineQuery.GetIQueryableEntityList(shippingLines);
            query2 = filter.GetFilteredQuery<ShippingLineList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        public IQueryable<ShippingLinePM> GetShippingLineSearch(string code, string name, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ShippingLine", "READ", tenant);

            shippingLineQuery = new ShippingLineQuery(tenant);
            IQueryable<ShippingLinePM> q = shippingLineQuery.GetShippingLinesByNameOrCode(code, name, tenant).Where(d => d.Tenant == tenant);
            return q;
        }

        public void InsertShippingLine(ShippingLinePM shippingLine)
        {
            SecurityUtility.AuthenticationOnTenant(shippingLine.Tenant);
            SecurityUtility.CheckContactFeature("ShippingLine", "NEW", shippingLine.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(shippingLine.Tenant);
            }
        
           
            shippingLineRepository = new ShippingLineRepository(objectContext);
            CardRepository = new CardRepository(objectContext);
            ContactRepository = new ContactRepository(objectContext);

            bool exist = (from a in shippingLineRepository.GetShippinngLines(shippingLine.Tenant)
                          where a.Card.Code == shippingLine.Code && a.Tenant == shippingLine.Tenant
                          select a).Any();
            if (!exist)
            {
                ShippingLineService service = new ShippingLineService(objectContext, shippingLine.Tenant);
                service.Create(shippingLine);
            }

            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", shippingLine.Tenant);
                msg = msg.Replace("%Entity", "Shipping line");
                throw new Exception(msg);
            }
        }

        public void UpdateShippingLine(ShippingLinePM currentShippingLine)
        {
            SecurityUtility.AuthenticationOnTenant(currentShippingLine.Tenant);
            SecurityUtility.CheckContactFeature("ShippingLine", "UPDATE", currentShippingLine.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentShippingLine.Tenant);
            }
  

            shippingLineRepository = new ShippingLineRepository(objectContext);
            CardRepository = new CardRepository(objectContext);
            cardQuery = new CardQuery(CardRepository);
            ContactRepository = new ContactRepository(objectContext);

            List<CardExternalCodeByCurrencyPM> cardExternalCodeByCurrenciesChangeSet = ChangeSet.GetAssociatedChanges(currentShippingLine, d => d.CardExternalCodeByCurrencies).Cast<CardExternalCodeByCurrencyPM>().ToList();
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

            CardPM c = cardQuery.GetSinglePM(currentShippingLine.Id, currentShippingLine.Tenant);

            bool exist = (from a in shippingLineRepository.GetShippinngLines(currentShippingLine.Tenant)
                          where a.Card.Code == c.Code
                          && a.Id != currentShippingLine.Id && a.Tenant == currentShippingLine.Tenant
                          select a).Any();

            if (!exist)
            {
                ShippingLineService service = new ShippingLineService(objectContext, currentShippingLine.Tenant);
                service.SetChangeSet(cardExternalCodeByCurrenciesChangeSet);
                service.Update(currentShippingLine);
            }

            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", currentShippingLine.Tenant);
                msg = msg.Replace("%Entity", "Shipping line");
                throw new Exception(msg);
            }
        }

        public void DeleteShippingLine(ShippingLine shippingLine)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(shippingLine.Tenant);
            }
            CardRepository = new CardRepository(objectContext);
            ShippingLine entity = shippingLineRepository.GetSingleShippingLine(shippingLine.Id, shippingLine.Tenant);
            shippingLineRepository.Remove(entity);
        }
    }
}
