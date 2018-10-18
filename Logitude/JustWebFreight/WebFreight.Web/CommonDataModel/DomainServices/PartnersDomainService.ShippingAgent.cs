using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Transactions;
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
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure;
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
        public void UpdateShippingAgentList(ShippingAgentList currentEntity)
        {
        }

        public bool DoesShippingAgentCodeExist(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            shippingAgentRepository = new ShippingAgentRepository(tenant);
            return (shippingAgentRepository.GetShippingAgents(tenant).Where(d => d.Card.Code == code && d.Tenant == tenant)).Any();
        }

        public IQueryable<ShippingAgent> GetShippingAgents(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ShippingAgent", "READ", tenant);

            shippingAgentRepository = new ShippingAgentRepository(tenant);
            return shippingAgentRepository.GetShippingAgents(0);
        }

        public IQueryable<ShippingAgentPM> GetShippingAgentsSearch(string code, string name, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ShippingAgent", "READ", tenant);

            shippingAgentQuery = new ShippingAgentQuery(tenant);
            IQueryable<ShippingAgentPM> q = shippingAgentQuery.GetShippingAgentsByNameOrCode(code, name, tenant).Where(d => d.Tenant == tenant);
            return q;
        }

        public IQueryable<ShippingAgentPM> GetShippingAgentsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ShippingAgent", "READ", tenant);

            shippingAgentQuery = new ShippingAgentQuery(tenant);
            return shippingAgentQuery.GetShippingAgentPMsByTenant(tenant).Where(d => d.Tenant == tenant);
        }

        public ShippingAgentPM GetShippingAgentById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ShippingAgent", "READ", tenant);

            shippingAgentQuery = new ShippingAgentQuery(tenant);
            return shippingAgentQuery.GetSinglePM(id,tenant);
        }

        public ShippingAgentList GetSingleShippingAgentList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ShippingAgent", "READ", tenant);

            shippingAgentQuery = new ShippingAgentQuery(tenant);
            return shippingAgentQuery.GetSingleShippingAgentList(id, tenant);
        }

        public IQueryable<ShippingAgentList> GetShippingAgentLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ShippingAgent", "READ", tenant);

            shippingAgentRepository = new ShippingAgentRepository(tenant);
            shippingAgentQuery = new ShippingAgentQuery(shippingAgentRepository);

            IQueryable<ShippingAgent> shippingAgents = shippingAgentRepository.GetShippingAgents(tenant);
            IQueryable<ShippingAgentList> query2 = shippingAgentQuery.GetIQueryableEntityList(shippingAgents);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<ShippingAgentList> GetShippingAgentFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ShippingAgent", "READ", tenant);

            shippingAgentRepository = new ShippingAgentRepository(tenant);
            shippingAgentQuery = new ShippingAgentQuery(shippingAgentRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<ShippingAgent> shippingAgents = shippingAgentRepository.GetShippingAgents(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            ShippingAgentCustomFilter customfilters = new ShippingAgentCustomFilter(tenant);
            shippingAgents = customfilters.GetFilteredQuery(queryOperations, shippingAgents);

            shippingAgents = filter.GetFilteredQuery<ShippingAgent>(nonListQueryOperation, shippingAgents);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ShippingAgentList> query2 = shippingAgentQuery.GetIQueryableEntityList(shippingAgents);
            query2 = filter.GetFilteredQuery<ShippingAgentList>(listQueryOperation, query2);
            
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ShippingAgentList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("ShippingAgent", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ShippingAgentList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ShippingAgentList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ShippingAgentList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ShippingAgentList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<ShippingAgentList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ShippingAgentList, bool>(queryOperations, query2);
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
            
            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetShippingAgentFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("ShippingAgent", "READ", tenant);

            shippingAgentRepository = new ShippingAgentRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<ShippingAgent> agents = shippingAgentRepository.GetShippingAgents(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            ShippingAgentCustomFilter customfilters = new ShippingAgentCustomFilter(tenant);
            agents = customfilters.GetFilteredQuery(queryOperations, agents);
            agents = filter.GetFilteredQuery<ShippingAgent>(nonListQueryOperation, agents);
            shippingAgentQuery = new ShippingAgentQuery(shippingAgentRepository);
            IQueryable<ShippingAgentList> query2 = shippingAgentQuery.GetIQueryableEntityList(agents);
            query2 = filter.GetFilteredQuery<ShippingAgentList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        //[Invoke]
        //public ShippingAgent CreateShippingAgent(ShippingAgent shippingAgent, Card card, int tenant)
        //{
        //    SecurityUtility.AuthenticationOnTenant(tenant);
        //    SecurityUtility.CheckContactFeature("ShippingAgent", "NEW", tenant);

        //    shippingAgentRepository = new ShippingAgentRepository(tenant);
        //    card.Id = IdCounter.GetNumber("Card", tenant).ToString();
        //    card.CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            
        //    using (TransactionScope scope = TransactionFactory.GetTransaction())//TransactionFactory.GetTransaction())
        //    {
        //        shippingAgent.Card = card;
        //        shippingAgent.Card.Code = CodeCounter.GetNumber("ShippingAgent", tenant).ToString();
        //        shippingAgentRepository.Add(shippingAgent);
        //        this.objectContext.SaveChanges();
        //        scope.Complete();
        //        return shippingAgent;
        //    }
        //}

        public void MapShippingAgentPMShippingAgent(ShippingAgentPM entityPm, ShippingAgent entity, Card card)
        {
            entity.Tenant = entityPm.Tenant;
            entity.ForwarderCreditNumber = entityPm.ForwarderCreditNumber;
            entity.ForwarderAccountNumber = entityPm.ForwarderAccountNumber;
            card.ReceivablesAccountingCard = entityPm.ReceivablesAccountingCard;
            card.PayablesAccountingCard = entityPm.PayablesAccountingCard;
            card.CreateDate = entityPm.CreateDate;
            card.EnglishName = entityPm.EnglishName;
            card.InActive = entityPm.InActive;
            card.LocalName = entityPm.LocalName;
            card.Notes = entityPm.Notes;
            card.PartnerTypeId = entityPm.PartnerTypeId;
            card.PaymentTermId = entityPm.PaymentTermId;
            card.Tenant = entityPm.Tenant;
            card.VatNumber = entityPm.VatNumber;
            card.Website = entityPm.Website;
            card.SearchFields = entityPm.Code + "," + entityPm.EnglishName + "," + entityPm.LocalName + "," + entityPm.VatNumber + "," + entityPm.ReceivablesAccountingCard + "," + entityPm.PayablesAccountingCard;
            card.VatTypeId = entityPm.VatTypeId;

            AddressRepository = new AddressRepository(objectContext);
            addressQuery = new AddressQuery(AddressRepository);
            List<AddressPM> addresses = addressQuery.GetAddressesByCardId(card.Id, card.Tenant);

            if (addresses != null)
            {
                AddressPM add = addresses.Where(a => a.AddressTypeId == "M").FirstOrDefault();
                if (add != null)
                {
                    card.CityName = add.City;

                    if (add.CountryId != null)
                    {
                        Country country = CountryRepository.GetSingleCountry(add.CountryId, card.Tenant, true);
                        if (country != null)
                        {
                            card.CountryName = country.EnglishName;
                        }
                    }
                }
            }
            card.InvoiceCurrencyId = entityPm.InvoiceCurrencyId;
        }

        public void InsertShippingAgent(ShippingAgentPM entityPm)
        {
            SecurityUtility.AuthenticationOnTenant(entityPm.Tenant);
            SecurityUtility.CheckContactFeature("ShippingAgent", "NEW", entityPm.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entityPm.Tenant);
            }

            ShippingAgentService service = new ShippingAgentService(objectContext, entityPm.Tenant);
            service.Create(entityPm);            
        }

        public void UpdateShippingAgent(ShippingAgentPM currentEntity)
        {
            SecurityUtility.AuthenticationOnTenant(currentEntity.Tenant);
            SecurityUtility.CheckContactFeature("ShippingAgent", "UPDATE", currentEntity.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentEntity.Tenant);
            }

            List<CardExternalCodeByCurrencyPM> cardExternalCodeByCurrenciesChangeSet = ChangeSet.GetAssociatedChanges(currentEntity, d => d.CardExternalCodeByCurrencies).Cast<CardExternalCodeByCurrencyPM>().ToList();
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

            ShippingAgentService service = new ShippingAgentService(objectContext, currentEntity.Tenant);
            service.SetChangeSet(cardExternalCodeByCurrenciesChangeSet);
            service.Update(currentEntity);
        }

        public void DeleteShippingAgent(ShippingAgentPM shippingAgent)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(shippingAgent.Tenant);
            }
            shippingAgentRepository = new ShippingAgentRepository(objectContext);
            ShippingAgent entity = shippingAgentRepository.GetSingleShippingAgent(shippingAgent.Tenant, shippingAgent.Id);
            shippingAgentRepository.Remove(entity);
        }
    }
}
