using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Xml.Serialization;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;

using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.InfrastructureModel.CustomFilters;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.Helpers;

namespace WebFreight.Web.InfrastructureModel.DomainServices
{
    public partial class WebFreightDomainService
    {
        public void UpdateIATACodeList(IATACodeList currentEntity)
        {

        }

        public IQueryable<IATACodePM> GetIATACodesByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            iAtaCodesRepository = new IATACodeRepository(tenant);
            iataCodeQuery=new IATACodeQuery(iAtaCodesRepository);
            return iataCodeQuery.GetIATACodePMs();
        }

        public IATACodeList GetSingleIATACodeList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            iAtaCodesRepository = new IATACodeRepository(tenant);
            IATACodeList iataCodeList = null;
            IATACode iataCode = iAtaCodesRepository.GetSingleIATACode(id);

            if (iataCode != null)
            {
                List<IATACode> singleEntityList = new List<IATACode>();
                singleEntityList.Add(iataCode);

                IQueryable<IATACode> iQueryable = singleEntityList.AsQueryable();
                iataCodeQuery = new IATACodeQuery(iAtaCodesRepository);
                IQueryable<IATACodeList> iQueryableEntityList = iataCodeQuery.GetIQueryableEntityList(iQueryable);
                iataCodeList = iQueryableEntityList.FirstOrDefault();
            }

            return iataCodeList;
        }

        public IATACodePM GetSingleIATACode(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          
            iAtaCodesRepository = new IATACodeRepository(tenant);
            iataCodeQuery = new IATACodeQuery(iAtaCodesRepository);
            return iataCodeQuery.GetSingleIATACodePM(id);
        }

        public IQueryable<IATACodeList> GetIATACodeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            iAtaCodesRepository = new IATACodeRepository(tenant);
            IQueryable<IATACode> iQueryable = iAtaCodesRepository.GetIATACodes();
            iataCodeQuery = new IATACodeQuery(iAtaCodesRepository);
            IQueryable<IATACodeList> query2 = iataCodeQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<IATACodeList> GetIATACodeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            iAtaCodesRepository = new IATACodeRepository(tenant);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<IATACode> iataCodes = iAtaCodesRepository.GetIATACodes();

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iataCodes = filter.GetFilteredQuery<IATACode>(nonListQueryOperation, iataCodes);

            int skippedChargregroups = queryOperations.PageIndex;//PageSize * (queryOperations.PageIndex - 1);
            iataCodeQuery = new IATACodeQuery(iAtaCodesRepository);
            IQueryable<IATACodeList> query2 = iataCodeQuery.GetIQueryableEntityList(iataCodes);
            query2 = filter.GetFilteredQuery<IATACodeList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(IATACodeList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("IATACode", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<IATACodeList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<IATACodeList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<IATACodeList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<IATACodeList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<IATACodeList, bool>(queryOperations, query2);
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

            query2 = query2.Skip(skippedChargregroups);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetIATACodeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            iAtaCodesRepository = new IATACodeRepository(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            
            IQueryable<IATACode> iataCodes = iAtaCodesRepository.GetIATACodes();

            IATACodeCustomFilter customfilters = new IATACodeCustomFilter(tenant);
            iataCodes = customfilters.GetFilteredQuery(queryOperations, iataCodes);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iataCodes = filter.GetFilteredQuery<IATACode>(nonListQueryOperation, iataCodes);

            int skippedVatTypes = queryOperations.PageIndex;

            iataCodeQuery = new IATACodeQuery(iAtaCodesRepository);
            IQueryable<IATACodeList> query2 = iataCodeQuery.GetIQueryableEntityList(iataCodes);

            query2 = filter.GetFilteredQuery<IATACodeList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertIATACodePM(IATACodePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(0);
            //SecurityUtility.CheckContactFeature("IATACode", "NEW", 0);

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(0);
            }

            IATACodeService service = new IATACodeService(objectContext, entityPM);
            service.Create();

            TableLastUpdateClass.UpdateTableHistory(0, "IATACode");
        }

        public void UpdateIATACodePM(IATACodePM entityPM)
        {
            SecurityUtility.AuthenticationOnTenant(0);
            //SecurityUtility.CheckContactFeature("IATACode", "UPDATE", 0);

            if (objectContext == null)
            {
                objectContext = WebFreightContext.GetContext(0);
            }

            IATACodeService service = new IATACodeService(objectContext, entityPM);
            service.Update();

            TableLastUpdateClass.UpdateTableHistory(0, "IATACode");
        }
    }
}