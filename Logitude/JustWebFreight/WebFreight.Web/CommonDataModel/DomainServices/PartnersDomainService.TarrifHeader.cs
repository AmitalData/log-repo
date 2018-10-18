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
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.CustomFilters;
using Logitude.BL.CommonDataModel.Tools.EntityService;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class PartnersDomainService
    {
        public IQueryable<TarrifHeaderPM> GetTarrifHeadersByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            tarrifHeaderQuery = new TarrifHeaderQuery(tenant);
            return tarrifHeaderQuery.GetTarrifHeaderPMsByTenant(tenant);
        }

        public List<TarrifHeaderPM> GetTarrifHeadersByCardIdAndTypeCode(string cardId, string typeCode, bool getAll, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            tarrifHeaderQuery = new TarrifHeaderQuery(tenant);
            return tarrifHeaderQuery.GetTarrifHeadersByCardIdAndTypeCode(cardId, typeCode, getAll, tenant);
        }

        public TarrifHeaderPM GetSingleTarrifHeader(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            tarrifHeaderQuery = new TarrifHeaderQuery(tenant);
            return tarrifHeaderQuery.GetSingleTarrifHeaderPM(id, tenant);
        }

        public TarrifHeaderList GetSingleTarrifHeaderList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            tarrifHeaderRepository = new TarrifHeaderRepository(tenant);
            TarrifHeaderList tarrifHeaderList = null;
            TarrifHeader tarrifHeader = tarrifHeaderRepository.GetSingleTarrifHeader(id);

            if (tarrifHeader != null)
            {
                List<TarrifHeader> singleEntityList = new List<TarrifHeader>();
                singleEntityList.Add(tarrifHeader);

                tarrifHeaderQuery = new TarrifHeaderQuery(tarrifHeaderRepository);
                IQueryable<TarrifHeader> iQueryable = singleEntityList.AsQueryable();
                IQueryable<TarrifHeaderList> iQueryableEntityList = tarrifHeaderQuery.GetIQueryableEntityList(iQueryable);
                tarrifHeaderList = iQueryableEntityList.FirstOrDefault();
            }
            return tarrifHeaderList;
        }

        public IQueryable<TarrifHeaderList> GetTarrifHeaderLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            tarrifHeaderRepository = new TarrifHeaderRepository(tenant);
            tarrifHeaderQuery = new TarrifHeaderQuery(tarrifHeaderRepository);

            IQueryable<TarrifHeader> iQueryable = tarrifHeaderRepository.GetTarrifHeadersByTenant(tenant);
            IQueryable<TarrifHeaderList> query2 = tarrifHeaderQuery.GetIQueryableEntityList(iQueryable);
            return query2;
        }

        [Query(HasSideEffects = true)]
        public IQueryable<TarrifHeaderList> GetTarrifHeaderFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            tarrifHeaderRepository = new TarrifHeaderRepository(tenant);
            tarrifHeaderQuery = new TarrifHeaderQuery(tarrifHeaderRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<TarrifHeader> iQueryable = tarrifHeaderRepository.GetTarrifHeadersByTenant(tenant);

            PortCustomFilter customfilters = new PortCustomFilter(tenant);
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TarrifHeader>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<TarrifHeaderList> query2 = tarrifHeaderQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<TarrifHeaderList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(TarrifHeaderList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> shipmentObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("TarrifHeader", tenant).ToList();

                ObjectField objectField = (from a in shipmentObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifHeaderList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifHeaderList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifHeaderList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifHeaderList, int>(queryOperations, query2);
                                break;
                            }
                        case "lookup":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifHeaderList, string>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<TarrifHeaderList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.CreateDate);
                                break;
                            }
                    }
                }
            }

            else
            {
                query2 = query2.OrderByDescending(d => d.CreateDate);
            }

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetTarrifHeaderFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Airline", "READ", tenant);

            tarrifHeaderRepository = new TarrifHeaderRepository(tenant);
            tarrifHeaderQuery = new TarrifHeaderQuery(tarrifHeaderRepository);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<TarrifHeader> iQueryable = tarrifHeaderRepository.GetTarrifHeadersByTenant(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<TarrifHeader>(nonListQueryOperation, iQueryable);

            IQueryable<TarrifHeaderList> query2 = tarrifHeaderQuery.GetIQueryableEntityList(iQueryable);
            query2 = filter.GetFilteredQuery<TarrifHeaderList>(listQueryOperation, query2);

            int count = query2.Count();
            return count;
        }

        //public void MapTarrifHeaderPMTarrifHeader(TarrifHeaderPM tarrifHeaderPm, TarrifHeader tarrifHeader)
        //{
        //    tarrifHeader.CardId = tarrifHeaderPm.CardId;
        //    tarrifHeader.CreateDate = tarrifHeaderPm.CreateDate;
        //    tarrifHeader.FromDate = tarrifHeaderPm.FromDate;
        //    tarrifHeader.InActive = tarrifHeaderPm.InActive;
        //    tarrifHeader.Notes = tarrifHeaderPm.Notes;
        //    tarrifHeader.TarrifTypeCode = tarrifHeaderPm.TarrifTypeCode;
        //    tarrifHeader.ToDate = tarrifHeaderPm.ToDate;
        //    tarrifHeader.TransitTimeNotes = tarrifHeaderPm.TransitTimeNotes;
        //    tarrifHeader.Tenant = tarrifHeaderPm.Tenant;
        //}

        public void InsertTarrifHeader(TarrifHeaderPM entity)
        {
            SecurityUtility.CheckContactFeature("Airline", "NEW", entity.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }
            TarrifHeaderService service = new TarrifHeaderService(objectContext , entity.Tenant);
            service.Create(entity);

            //tarrifHeaderRepository = new TarrifHeaderRepository(objectContext);
            //tarrifChargeRepository = new TarrifChargeRepository(objectContext);
            //tarrifFromToRepository = new TarrifFromToRepository(objectContext);

            //TarrifHeader newTarrifHeader = new TarrifHeader();
            //newTarrifHeader.Id = IdCounter.GetNumber("TarrifHeader", entity.Tenant).ToString();
            //entity.Id = newTarrifHeader.Id;

            //if (entity.TarrifCharges != null)
            //{
            //    foreach (TarrifChargePM charge in entity.TarrifCharges)
            //    {
            //        TarrifCharge newCharge = new TarrifCharge()
            //        {
            //            ChargesTypeId = charge.ChargesTypeId,
            //            CurrencyId = charge.CurrencyId,
            //            Id = IdCounter.GetNumber("TarrifCharge", entity.Tenant).ToString(),
            //            MaxPrice = charge.MaxPrice,
            //            MeasurementId = charge.MeasurementId,
            //            MinPrice = charge.MinPrice,
            //            TarrifHeaderId = newTarrifHeader.Id,
            //            Tenant = entity.Tenant,
            //            UnitPrice = charge.UnitPrice,

            //        };
            //        charge.Id = newCharge.Id;
            //        tarrifChargeRepository.Add(newCharge);
            //    }
            //}

            //if (entity.TarrifFromToes != null)
            //{
            //    foreach (TarrifFromToPM fromTo in entity.TarrifFromToes)
            //    {
            //        TarrifFromTo newFromTo = new TarrifFromTo()
            //        {
            //            CountryId = fromTo.CountryId,
            //            Id = IdCounter.GetNumber("TarrifFromTo", entity.Tenant).ToString(),
            //            PortId = fromTo.PortId,
            //            TarrifFromToTypeCode = fromTo.TarrifFromToTypeCode,
            //            TarrifHeaderId = newTarrifHeader.Id,
            //            Tenant = entity.Tenant,

            //        };
            //        fromTo.Id = newFromTo.Id;
            //        tarrifFromToRepository.Add(newFromTo);
            //    }
            //}

            //MapTarrifHeaderPMTarrifHeader(entity, newTarrifHeader);
            //tarrifHeaderRepository.Add(newTarrifHeader);
        }

        //public void MapTarrifChargePMTarrifCharge(TarrifChargePM tarrifChargePm, TarrifCharge tarrifCharge)
        //{
        //    tarrifCharge.ChargesTypeId = tarrifChargePm.ChargesTypeId;
        //    tarrifCharge.TarrifHeaderId = tarrifChargePm.TarrifHeaderId;
        //    tarrifCharge.Tenant = tarrifChargePm.Tenant;
        //    tarrifCharge.CurrencyId = tarrifChargePm.CurrencyId;
        //    tarrifCharge.MaxPrice = tarrifChargePm.MaxPrice;
        //    tarrifCharge.MeasurementId = tarrifChargePm.MeasurementId;
        //    tarrifCharge.MinPrice = tarrifChargePm.MinPrice;
        //    tarrifCharge.UnitPrice = tarrifChargePm.UnitPrice;
        //}

        //public void MapTarrifFromToPMTarrifFromTo(TarrifFromToPM tarrifFromToPm, TarrifFromTo tarrifFromTo)
        //{
        //    tarrifFromTo.CountryId = tarrifFromToPm.CountryId;
        //    tarrifFromTo.TarrifHeaderId = tarrifFromToPm.TarrifHeaderId;
        //    tarrifFromTo.Tenant = tarrifFromToPm.Tenant;
        //    tarrifFromTo.PortId = tarrifFromToPm.PortId;
        //    tarrifFromTo.TarrifFromToTypeCode = tarrifFromToPm.TarrifFromToTypeCode;
        //}

        public void UpdateTarrifHeader(TarrifHeaderPM currentEntity)
        {
            SecurityUtility.CheckContactFeature("Airline", "UPDATE", currentEntity.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentEntity.Tenant);
            }
          

            tarrifHeaderRepository = new TarrifHeaderRepository(objectContext);
            tarrifChargeRepository = new TarrifChargeRepository(objectContext);
            tarrifFromToRepository = new TarrifFromToRepository(objectContext);

            TarrifHeader tarrifHeader = tarrifHeaderRepository.GetSingleTarrifHeader(currentEntity.Id);

            List<TarrifChargePM> TarrifChargeChangeSet =  ChangeSet.GetAssociatedChanges(currentEntity, d => d.TarrifCharges).Cast<TarrifChargePM>().ToList();
            #region TarrifCharges
            foreach (TarrifChargePM r in TarrifChargeChangeSet)
            {
                ChangeOperation op = ChangeSet.GetChangeOperation(r);

                switch (op)
                {
                    case ChangeOperation.Insert:
                        {
                            r.ChangeSetOp = ChangeSetOperation.Insert;
                            //r.Id = IdCounter.GetNumber("TarrifCharge", currentEntity.Tenant).ToString();
                            //TarrifCharge newCh = new TarrifCharge()
                            //{
                            //    Id = r.Id,
                            //};
                            //MapTarrifChargePMTarrifCharge(r, newCh);
                            //tarrifChargeRepository.Add(newCh);
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            r.ChangeSetOp = ChangeSetOperation.Update;
                            //TarrifCharge ch = tarrifChargeRepository.GetSingleTarrifCharge(r.Id);
                            //MapTarrifChargePMTarrifCharge(r, ch);
                            //tarrifChargeRepository.Update(ch);
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            r.ChangeSetOp = ChangeSetOperation.Delete;
                            //TarrifCharge ch = tarrifChargeRepository.GetSingleTarrifCharge(r.Id);
                            //tarrifChargeRepository.Remove(ch);
                            break;
                        }

                    case ChangeOperation.None:
                        {
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
            }
            #endregion

            List<TarrifFromToPM> tarrifFromToChangeSet = ChangeSet.GetAssociatedChanges(currentEntity, d => d.TarrifFromToes).Cast<TarrifFromToPM>().ToList();

            #region TarrifFromToes
            foreach (TarrifFromToPM r in tarrifFromToChangeSet)
            {
                ChangeOperation op = ChangeSet.GetChangeOperation(r);

                switch (op)
                {
                    case ChangeOperation.Insert:
                        {
                            r.changeOp = ChangeSetOperation.Insert;
                            //r.Id = IdCounter.GetNumber("TarrifFromTo", currentEntity.Tenant).ToString();
                            //TarrifFromTo newFt = new TarrifFromTo()
                            //{
                            //    Id = r.Id,
                            //};
                            //MapTarrifFromToPMTarrifFromTo(r, newFt);
                            //tarrifFromToRepository.Add(newFt);
                            break;
                        }

                    case ChangeOperation.Update:
                        {
                            r.changeOp = ChangeSetOperation.Update;
                            //TarrifFromTo ft = tarrifFromToRepository.GetSingleTarrifFromTo(r.Id);
                            //MapTarrifFromToPMTarrifFromTo(r, ft);
                            //tarrifFromToRepository.Update(ft);
                            break;
                        }

                    case ChangeOperation.Delete:
                        {
                            r.changeOp = ChangeSetOperation.Delete;
                            //TarrifFromTo ft = tarrifFromToRepository.GetSingleTarrifFromTo(r.Id);
                            //tarrifFromToRepository.Remove(ft);
                            break;
                        }

                    case ChangeOperation.None:
                        {
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
            }
            #endregion

            //MapTarrifHeaderPMTarrifHeader(currentEntity, tarrifHeader);
            //tarrifHeaderRepository.Update(tarrifHeader);

            TarrifHeaderService service = new TarrifHeaderService(objectContext, currentEntity.Tenant);
            service.SetChangeSet(TarrifChargeChangeSet, tarrifFromToChangeSet);
            service.Update(currentEntity);
        }

        public void DeleteTarrifHeader(TarrifHeaderPM entity)
        {
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(entity.Tenant);
            }
            tarrifHeaderRepository = new TarrifHeaderRepository(objectContext);
            TarrifHeader tarrifHeader = tarrifHeaderRepository.GetSingleTarrifHeader(entity.Id);
            tarrifHeaderRepository.Remove(tarrifHeader);
        }
    }
}