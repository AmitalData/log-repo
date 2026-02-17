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
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.CustomFilters;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class PartnersDomainService
    {
        public IQueryable<TarrifStepPM> GetTarrifStepsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TarrifStep", "READ", tenant);

            tarrifStepQuery = new TarrifStepQuery(tenant);
            return tarrifStepQuery.GetTarrifStepPMsByTenant(tenant);
        }

        public TarrifStepPM GetSingleTarrifStep(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TarrifStep", "READ", tenant);

            tarrifStepQuery = new TarrifStepQuery(tenant);
            return tarrifStepQuery.GetSingleTarrifStepPM(id);
        }

        public TarrifStepList GetSingleTarrifStepList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TarrifStep", "READ", tenant);

            tarrifStepRepository = new TarrifStepRepository(tenant);
            TarrifStepList tarrifStepList = null;
            TarrifStep tarrifStep = tarrifStepRepository.GetSingleTarrifStep(id);

            if (tarrifStep != null)
            {
                List<TarrifStep> singleEntityList = new List<TarrifStep>();
                singleEntityList.Add(tarrifStep);

                tarrifStepQuery = new TarrifStepQuery(tarrifStepRepository);
                IQueryable<TarrifStep> iQueryable = singleEntityList.AsQueryable();
                IQueryable<TarrifStepList> iQueryableEntityList = tarrifStepQuery.GetIQueryableEntityList(iQueryable);
                tarrifStepList = iQueryableEntityList.FirstOrDefault();
            }

            return tarrifStepList;
        }

        public IQueryable<TarrifStepList> GetTarrifStepLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TarrifStep", "READ", tenant);

            tarrifStepRepository = new TarrifStepRepository(tenant);
            tarrifStepQuery = new TarrifStepQuery(tarrifStepRepository);

            IQueryable<TarrifStep> iQueryable = tarrifStepRepository.GetTarrifStepsByTenant(tenant);
            IQueryable<TarrifStepList> query2 = tarrifStepQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<TarrifStepList> GetTarrifStepFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TarrifStep", "READ", tenant);

            tarrifStepRepository = new TarrifStepRepository(tenant);
            tarrifStepQuery = new TarrifStepQuery(tarrifStepRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<TarrifStep> iQueryable = tarrifStepRepository.GetTarrifStepsByTenant(tenant);

            PortCustomFilter customfilters = new PortCustomFilter(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TarrifStep>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<TarrifStepList> query2 = tarrifStepQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<TarrifStepList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(TarrifStepList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("TarrifStep", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifStepList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifStepList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifStepList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifStepList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifStepList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifStepList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetTarrifStepFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("TarrifStep", "READ", tenant);

            tarrifStepRepository = new TarrifStepRepository(tenant);
            tarrifStepQuery = new TarrifStepQuery(tarrifStepRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<TarrifStep> iQueryable = tarrifStepRepository.GetTarrifStepsByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TarrifStep>(nonListQueryOperation, iQueryable);

            IQueryable<TarrifStepList> query2 = tarrifStepQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<TarrifStepList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        //public void MapTarrifStepPMTarrifStep(TarrifStepPM tarrifStepPm, TarrifStep tarrifStep)
        //{
        //    tarrifStep.MaxPrice = tarrifStepPm.MaxPrice;
        //    tarrifStep.MinPrice = tarrifStepPm.MinPrice;
        //    tarrifStep.Step = tarrifStepPm.Step;
        //    tarrifStep.TarrifHeaderId = tarrifStepPm.TarrifHeaderId;
        //    tarrifStep.Tenant = tarrifStepPm.Tenant;
        //    tarrifStep.UnitPrice = tarrifStepPm.UnitPrice;
        //}

        public void InsertTarrifStep(TarrifStepPM entity)
        {
            SecurityUtility.CheckContactFeature("TarrifStep", "NEW", entity.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }

            TarrifStepService service = new TarrifStepService(objectContext, entity.Tenant);
            service.Create(entity);

            //tarrifStepRepository = new TarrifStepRepository(objectContext);
            //TarrifStep newTarrifStep = new TarrifStep();
            //newTarrifStep.Id = IdCounter.GetNumber("TarrifStep", entity.Tenant).ToString();
            //entity.Id = newTarrifStep.Id;
            //MapTarrifStepPMTarrifStep(entity, newTarrifStep);
            //tarrifStepRepository.Add(newTarrifStep);
        }

        public void UpdateTarrifStep(TarrifStepPM currentEntity)
        {
            SecurityUtility.CheckContactFeature("TarrifStep", "UPDATE", currentEntity.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentEntity.Tenant);
            }
            TarrifStepService service = new TarrifStepService(objectContext, currentEntity.Tenant);
            service.Update(currentEntity);

            //tarrifStepRepository = new TarrifStepRepository(objectContext);
            //TarrifStep tarrifStep = tarrifStepRepository.GetSingleTarrifStep(currentEntity.Id);
            //MapTarrifStepPMTarrifStep(currentEntity, tarrifStep);
            //tarrifStepRepository.Update(tarrifStep);
        }

        public void DeleteTarrifStep(TarrifStepPM entity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }
            tarrifStepRepository = new TarrifStepRepository(objectContext);
            TarrifStep tarrifStep = tarrifStepRepository.GetSingleTarrifStep(entity.Id);
            tarrifStepRepository.Remove(tarrifStep);
        }
    }
}