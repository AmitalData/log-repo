using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
        public void UpdateIncotermList(IncotermList currentEntity)
        {
        }

        public IQueryable<Incoterm> GetIncoterms(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Incoterm", "READ", tenant);

            incotermRepository = new IncotermRepository(tenant);
            return incotermRepository.GetIncoterms(0);
        }

        public IQueryable<IncotermPM> GetIncotermsSearch(string code, string name, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Incoterm", "READ", tenant);

            incotermQuery = new IncotermQuery(tenant);
            IQueryable<IncotermPM> q = incotermQuery.GetIncotermsByCodeOrName(code, name, tenant).Where(d => d.Tenant == tenant);
            return q;
        }

        public IQueryable<IncotermPM> GetIncotermsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Incoterm", "READ", tenant);

            incotermQuery = new IncotermQuery(tenant);
            return incotermQuery.GetIncotermPMsByTenant(tenant);
        }

        public IncotermList GetSingleIncotermList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Incoterm", "READ", tenant);

            incotermRepository = new IncotermRepository(tenant);
            IncotermList incotermList = null;
            Incoterm incoterm = incotermRepository.GetSingleIncoterm(id, tenant);

            if (incoterm != null)
            {
                List<Incoterm> singleEntityList = new List<Incoterm>();
                singleEntityList.Add(incoterm);

                incotermQuery = new IncotermQuery(incotermRepository);
                IQueryable<Incoterm> iQueryable = singleEntityList.AsQueryable();
                IQueryable<IncotermList> iQueryableEntityList = incotermQuery.GetIQueryableEntityList(iQueryable);
                incotermList = iQueryableEntityList.FirstOrDefault();
            }
            return incotermList;
        }

        public IQueryable<IncotermList> GetIncotermListsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Incoterm", "READ", tenant);

            incotermRepository = new IncotermRepository(tenant);
            incotermQuery = new IncotermQuery(incotermRepository);

            IQueryable<Incoterm> incoterms = incotermRepository.GetIncoterms(tenant);
            IQueryable<IncotermList> query2 = incotermQuery.GetIQueryableEntityList(incoterms);

            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<IncotermList> GetIncotermFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Incoterm", "READ", tenant);

            incotermRepository = new IncotermRepository(tenant);
            incotermQuery = new IncotermQuery(incotermRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Incoterm> incoterms = incotermRepository.GetIncoterms(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            incoterms = filter.GetFilteredQuery<Incoterm>(nonListQueryOperation, incoterms);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<IncotermList> query2 = incotermQuery.GetIQueryableEntityList(incoterms);

            query2 = filter.GetFilteredQuery<IncotermList>(listQueryOperation, query2);
            
            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(IncotermList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Incoterm", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<IncotermList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<IncotermList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<IncotermList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<IncotermList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<IncotermList, bool>(queryOperations, query2);
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

        public int GetIncotermFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Incoterm", "READ", tenant);

            incotermRepository = new IncotermRepository(tenant);
            incotermQuery = new IncotermQuery(incotermRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Incoterm> incoterms = incotermRepository.GetIncoterms(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            incoterms = filter.GetFilteredQuery<Incoterm>(nonListQueryOperation, incoterms);

            IQueryable<IncotermList> query2 = incotermQuery.GetIQueryableEntityList(incoterms);

            query2 = filter.GetFilteredQuery<IncotermList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public IQueryable<IncotermPM> GetIncotermsByTenantInput(int tenant, string input, bool byCode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Incoterm", "READ", tenant);

            incotermQuery = new IncotermQuery(tenant);
            input = input.Trim().ToUpper();
            if (input != String.Empty)
            {

                if (byCode)
                {
                    return incotermQuery.GetIncotermPMsByTenant(tenant).Where(d => d.Tenant == tenant && d.Code.ToUpper().StartsWith(input.ToUpper()));
                }
                else
                {
                    return incotermQuery.GetIncotermPMsByTenant(tenant).Where(d => d.Tenant == tenant && d.Name.ToUpper().StartsWith(input.ToUpper()));
                }
            }
            else
            {
                return incotermQuery.GetIncotermPMsByTenant(tenant).Where(d => d.Tenant == tenant);
            }
        }

        public IQueryable<IncotermPM> GetSingleIncotermByTenantInput(int tenant, string input, bool byCode)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Incoterm", "READ", tenant);

            incotermQuery = new IncotermQuery(tenant);
            if (byCode)
            {
                return incotermQuery.GetIncotermPMsByTenant(tenant).Where(d => d.Tenant == tenant && d.Code.ToUpper() == input.ToUpper());
            }
            else
            {
                return incotermQuery.GetIncotermPMsByTenant(tenant).Where(d => d.Tenant == tenant && d.Name.ToUpper().StartsWith(input.ToUpper()));
            }
        }

        public IncotermPM GetSingleIncoterm(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Incoterm", "READ", tenant);

            incotermQuery = new IncotermQuery(tenant);
            return incotermQuery.GetSinglePM(id, tenant);
        }

        public bool DoesIncotermCodeExist(string code, int tenant)
        {
            incotermRepository = new IncotermRepository(tenant);
            return (incotermRepository.GetIncoterms(tenant).Where(d => d.Code == code && d.Tenant == tenant)).Any();
        }

        //public void MapIncotermIncotermPM(IncotermPM incotermPM, Incoterm incoterm)
        //{
        //    incoterm.AddedManually = incotermPM.AddedManually;
        //    incoterm.Code = incotermPM.Code;
        //    incoterm.Freight = incotermPM.Freight;
        //    incoterm.InActive = incotermPM.InActive;
        //    incoterm.LocalName = incotermPM.LocalName;
        //    incoterm.Name = incotermPM.Name;
        //    incoterm.OtherCharges = incotermPM.OtherCharges;
        //    incoterm.Notes = incotermPM.Notes;
        //    incoterm.Tenant = incotermPM.Tenant;
        //    incoterm.SearchFields = incotermPM.Code + "," + incotermPM.LocalName + "," + incotermPM.Name;
        //}

        public void InsertIncoterm(IncotermPM incoterm)
        {
            SecurityUtility.CheckContactFeature("Incoterm", "NEW", incoterm.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(incoterm.Tenant);
            }

            incotermRepository = new IncotermRepository(objectContext);
           
            bool exist = (from a in incotermRepository.GetIncoterms(incoterm.Tenant)
                          where a.Code == incoterm.Code && a.Tenant == incoterm.Tenant
                          select a).Any();

            if (!exist)
            {
                IncotermService service = new IncotermService(objectContext, incoterm.Tenant);
                service.Create(incoterm);

                //incoterm.Id = IdCounter.GetNumber("Incoterm", incoterm.Tenant).ToString();
                //Incoterm newIncoterm = new Incoterm();
                //newIncoterm.Id = incoterm.Id;
                //MapIncotermIncotermPM(incoterm, newIncoterm);
                //incotermRepository.Add(newIncoterm);

                //create TraceEvent
                //WebFreightDomainService webfreightService = new WebFreightDomainService();
                //ContactRepository contactsRepositorypository = new ContactRepository(objectContext);
                //ContactQuery contactQuery = new ContactQuery(contactsRepositorypository);
                //ContactPM contact = contactQuery.GetContactByNameAndTenant(SecurityUtility.GetAuthenticatedUser(), incoterm.Tenant, true);
                //if (contact != null) EventTracer.CreateTraceEvent(new TraceEvent(), "CRIT", incoterm.Tenant, contact.Id, incoterm.Id, null, "Incoterm", null, null, false);
                TableLastUpdateClass.UpdateTableHistory(incoterm.Tenant, "Incoterm");

            }
            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", incoterm.Tenant);
                msg = msg.Replace("%Entity", "Incoterm");
                throw new Exception(msg);
            }
        }

        public void UpdateIncoterm(IncotermPM currentIncoterm)
        {
            SecurityUtility.CheckContactFeature("Incoterm", "UPDATE", currentIncoterm.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentIncoterm.Tenant);
            }
         
           

            incotermRepository = new IncotermRepository(objectContext);

            bool exist = (from a in incotermRepository.GetIncoterms(currentIncoterm.Tenant)
                          where a.Code == currentIncoterm.Code && a.Id != currentIncoterm.Id && a.Tenant == currentIncoterm.Tenant
                          select a).Any();
            if (!exist)
            {
                IncotermService service = new IncotermService(objectContext, currentIncoterm.Tenant);
                service.Update(currentIncoterm);

                TableLastUpdateClass.UpdateTableHistory(currentIncoterm.Tenant, "Incoterm");
            }
            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyexists", currentIncoterm.Tenant);
                msg = msg.Replace("%Entity", "Incoterm");
                throw new Exception(msg);
            }
        }

        public void DeleteIncoterm(IncotermPM incoterm)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(incoterm.Tenant);
            }
            incotermRepository = new IncotermRepository(objectContext);

            Incoterm entity = incotermRepository.GetSingleIncoterm(incoterm.Id, incoterm.Tenant);
            incotermRepository.Remove(entity);
        }
    }
}