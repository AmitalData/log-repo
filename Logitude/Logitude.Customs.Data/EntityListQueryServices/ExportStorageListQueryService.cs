using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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

namespace Logitude.Customs.Data.EntityListQueryServices
{

    public partial class ExportStorageListQueryService
    {
        private IQueryable<ExportStorageList> GetIqueryableList(IQueryable<ExportStorage> iQueryable)
        {
            IQueryable<ExportStorageList> query = (from en in iQueryable.Include("Declaration").Include("DeclarationStatusType").Include("CargoType").Include("CargoStatuse")
                                                   .Include("ExportLogisticPermitAction").Include("Client").Include("CustomsShip").Include("CargoIdentifireType")
                                                   .Include("CargoIdentifireType").Include("UnloadingSiteType")
                                                   join c in context.Clients
                                                   on en.ExporterID equals c.Id into cl
                                                   from client in cl.DefaultIfEmpty()


                                                   select new ExportStorageList()
                                                   {
                                                       Id = en.Id,

                                                       Tenant = en.Tenant,

                                                       SearchFields = en.SearchFields,

                                                       DeclarationId = en.DeclarationEntity!= null? en.DeclarationEntity.CustomFileNo : "",

                                                       ExportFileNo = en.ExportFileNo,

                                                       StorageStatus = en.StorageStatus,

                                                       CargoTypeCode = en.CargoTypeCode,

                                                       OpenDate = en.OpenDate,

                                                       CargoType = en.CargoType,

                                                       CustomsStatus = en.CustomsStatus,

                                                       ExporterID = en.ExporterID,

                                                       ShipCode = en.ShipCode,

                                                       FirstCargoID = en.FirstCargoID,

                                                       SecondCargoID = en.SecondCargoID,

                                                       ThirdCargoID = en.ThirdCargoID,

                                                       DeclarationStatusTypeName = en.DeclarationEntity != null && en.DeclarationEntity.DeclarationStatusType!=null ? en.DeclarationEntity.DeclarationStatusType.LocalName:"",

 
                                                       CargoTypeName = en.CargoTypeEntity!=null? en.CargoTypeEntity.LocalName:"",

                                                       CustomStatusName = en.CustomsCargoStatus.LocalName,


                                                       ShipName = en.CustomsShipCode!=null? en.CustomsShipCode.LocalName:"",

                                                       StorErrorXML = en.StorErrorXML,

                                                       StorageNo = en.StorageNo,

                                                       ExportDealIdentification = en.ExportDealIdentification,

                                                       CargoTypeCodeName = en.CargoIdentifireType.LocalName,

                                                       DeclarationStatusTypeCode = en.DeclarationEntity!=null? en.DeclarationEntity.DeclarationStatusTypeCode:"",

                                                       Declaration_ID = en.DeclarationId,

                                                       DeclarationCustomFileNo = en.DeclarationEntity != null ? en.DeclarationEntity.CustomFileNo:"",

                                                       DeclarationNumber = en.DeclarationEntity != null ? en.DeclarationEntity.DeclarationNumber:"",
                                                       ExporterName = en. ExporterID!=null ? client.FullName: en.DeclarationEntity != null && en.DeclarationEntity.Importer != null ? en.DeclarationEntity.Importer.FullName : "",

                                                       ExporterCode = en.ExporterID!=null?  client.Code : en.DeclarationEntity != null && en.DeclarationEntity.Importer != null ? en.DeclarationEntity.Importer.Code : "",//client.Code,

                                                       StorageStatusIsOpen = en.StorageStatus != null && en.StorageStatus.ToLower() == "open",

                                                       ExportLoadingPortcode = en.ExportLoadingPortcode,

                                                       ExportLoadingPortName = en.InternationalSiteS!=null? en.InternationalSiteS.LocalName :"",

                                                       ActionCode = en.ActionCode,
                                                       ActionName = en.ExportLogisticPermitAction!=null ? en.ExportLogisticPermitAction.LocalName:"",
                                                       ProcedureCurrentName = en.DeclarationEntity != null && en.DeclarationEntity.GovernmentProcedureCurrent!=null ? en.DeclarationEntity.GovernmentProcedureCurrent.LocalName :"",
                                                       StorageSiteCode = en.StorageSiteCode,
                                                       StorageStatusName = en.StorageStatus,
                                                       MarksNumbers=en.MarksNumbers,
                                                       ConnectedDeclaration=en.DeclarationId!=null?true :false,
                                                   });// ;
            return query;
        }

        private IQueryable<ExportStorage> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ExportStorage> iQueryable, int tenant)
        {
            bool flag = false;
            var filter = queryOperations.QueryFilterItems.FirstOrDefault(x => x.FieldName == "DeclarationIdAndProcedureCurrentName");
            if (filter != null)
            {
                IQueryable<ConsignmentList> query = (from a in context.Consignments.Where(y => y.DeclarationId == filter.FieldValue.ToString()).Select(r => new { r.ExportStoragesId })select new ConsignmentList { ExportStoragesId=a.ExportStoragesId });
                iQueryable = iQueryable.Where(x => x.DeclarationId == null ||
                (x.DeclarationEntity.GovernmentProcedureCurrent.LocalName.Contains("המכלה") 
                    &&  !query.Any(t => t.ExportStoragesId == x.Id)));             
            }
            var filter2 = queryOperations.QueryFilterItems.FirstOrDefault(x => x.FieldName == "IsExportFileNo");
            if (filter2 != null)
            {

                flag = !iQueryable.Any(x => x.ExportFileNo == filter2.FieldValue.ToString());
                iQueryable = iQueryable.Where(x => flag || x.ExportFileNo == filter2.FieldValue.ToString());

            }
            return iQueryable;
        }
        public List<ExportStorageList> GetListForExportStorage(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            IQueryable<ExportStorage> iQueryable = (from a in context.ExportStorages

                                                    where a.Tenant == tenant
                                                    select a);
            iQueryable = ApplyCustomFilters(queryOperations, iQueryable, tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<ExportStorage>(nonListQueryOperation, iQueryable);

            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ExportStorageList> query2 = GetIqueryableList(iQueryable);

            query2 = filter.GetFilteredQuery<ExportStorageList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ExportStorageList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> DeclarationObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.ExportStorage", tenant).ToList();

                ObjectField objectField = (from a in DeclarationObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<ExportStorageList, string>(queryOperations, query2);
                    }
                    else
                    {
                        switch (objectField.DataTypeCode.ToLower())
                        {
                            case "ntext":
                            case "text":
                                {
                                    query2 = sortClass.GetSorterQuery<ExportStorageList, string>(queryOperations, query2);
                                    break;
                                }
                            case "sigdouble":
                            case "double":
                                {
                                    query2 = sortClass.GetSorterQuery<ExportStorageList, double>(queryOperations, query2);
                                    break;
                                }
                            case "date":
                            case "datetime":
                                {
                                    query2 = sortClass.GetSorterQuery<ExportStorageList, DateTime>(queryOperations, query2);
                                    break;
                                }
                            case "unsinteger":
                            case "integer":
                                {
                                    query2 = sortClass.GetSorterQuery<ExportStorageList, int>(queryOperations, query2);
                                    break;
                                }
                            case "boolean":
                                {
                                    query2 = sortClass.GetSorterQuery<ExportStorageList, bool>(queryOperations, query2);
                                    break;
                                }
                            case "unsdecimal":
                            case "decimal":
                                {
                                    query2 = sortClass.GetSorterQuery<ExportStorageList, decimal>(queryOperations, query2);
                                    break;
                                }
                            default:
                                {
                                    query2 = query2.OrderByDescending(d => d.OpenDate);
                                    break;
                                }
                        }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.OpenDate);
            }
            if (!queryOperations.GetAll)
            {
                query2 = query2.Skip(skippedPorts);
                query2 = query2.Take(queryOperations.PageSize);
            }
            return query2.ToList();


        }

    }

}

