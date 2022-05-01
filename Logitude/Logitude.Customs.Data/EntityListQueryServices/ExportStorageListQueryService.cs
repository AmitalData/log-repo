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

namespace Logitude.Customs.Data.EntityListQueryServices
{

    public partial class ExportStorageListQueryService
    {
        private IQueryable<ExportStorageList> GetIqueryableList(IQueryable<ExportStorage> iQueryable)
        {
            IQueryable<ExportStorageList> query = (from en in iQueryable
                                                   
                                                   join d in context.Declarations.Select(r => new { r.Id, r.DeclarationStatusTypeCode, r.CustomFileNo, r.DeclarationNumber,r.GovernmentProcedureCurrent,r.ProcedureCurrentCode })
                                                   on en.DeclarationId equals d.Id
                                                   into dj from declaration in dj.DefaultIfEmpty()

                                                   join s in context.DeclarationStatusTypes.Select(r => new { r.Code, r.LocalName })
                                                   on declaration.DeclarationStatusTypeCode equals s.Code
                                                   into sj
                                                   from status in sj.DefaultIfEmpty()

                                                   join ct in context.CargoTypes.Select(r => new { r.Code, r.LocalName})
                                                   on en.CargoType equals ct.Code
                                                   into ctj
                                                   from cargoType in ctj.DefaultIfEmpty()

                                                   join ss in context.CargoStatuses.Select( r=> new {r.Code, r.LocalName})
                                                   on en.CustomsStatus equals ss.Code
                                                   into ssj
                                                   from cargoStatus in ssj.DefaultIfEmpty()

                                                   from client in context.Clients
                                                   .Where(c => c.Code == en.ExporterID || c.Id == en.ExporterID)
                                                   .Select( r=> new {r.Id, r.FullName, r.Code})
                                                   .DefaultIfEmpty()

                                                   //join c in context.Cards.Select( r=> new {r.Id, r.LocalName, r.VatNumber})
                                                   //on en.ExporterID equals c.Id
                                                   //into cj
                                                   //from card in cj.DefaultIfEmpty()

                                                   join cs in context.CustomsShips.Select( r=> new {r.Code, r.LocalName})
                                                   on en.ShipCode equals cs.Code
                                                   into csj
                                                   from customsShip in csj.DefaultIfEmpty()

                                                   join ci in context.CargoIdentifireTypes.Select(r=> new{ r.Code, r.LocalName})
                                                   on en.CargoTypeCode equals ci.Code
                                                   into cij
                                                   from cargoIdentifireType in cij.DefaultIfEmpty()

                                                   select new ExportStorageList()
                                                   {
                                                       Id = en.Id,

                                                       Tenant = en.Tenant,

                                                       SearchFields = en.SearchFields,

                                                       DeclarationId = en.DeclarationId,

                                                       ExportFileNo = en.ExportFileNo,

                                                       StorageStatus = en.StorageStatus,

                                                       CargoTypeCode = en.CargoTypeCode,

                                                       OpenDate = en.OpenDate,

                                                       CargoType = en.CargoType,

                                                       CustomsStatus = en.CustomsCargoStatus.LocalName,

                                                       ExporterID = en.ExporterID,

                                                       ShipCode = en.ShipCode,

                                                       FirstCargoID = en.FirstCargoID,

                                                       SecondCargoID = en.SecondCargoID,

                                                       ThirdCargoID = en.ThirdCargoID,

                                                       DeclarationStatusTypeName = status.LocalName,
                                                       //DeclarationStatusTypeName = 
                                                       //(
                                                       // from status in context.DeclarationStatusTypes
                                                       // where status.Code == (from Declaration in context.Declarations where Declaration.Id == en.DeclarationId select new { Declaration.DeclarationStatusTypeCode }).FirstOrDefault().DeclarationStatusTypeCode
                                                       // select new  { status.LocalName }
                                                       //).FirstOrDefault().LocalName,
                                                       
                                                       CargoTypeName = cargoType.LocalName,

                                                       CustomStatusName = cargoStatus.LocalName,

                                                       //ExporterName = card.LocalName,
                                                       ExporterName = client.FullName,

                                                       ShipName = customsShip.LocalName,

                                                       StorErrorXML = en.StorErrorXML,

                                                       StorageNo = en.StorageNo,

                                                       ExportDealIdentification = en.ExportDealIdentification,

                                                       CargoTypeCodeName = cargoIdentifireType.LocalName,

                                                       DeclarationStatusTypeCode = status.Code,

                                                       Declaration_ID = en.DeclarationId,

                                                       DeclarationCustomFileNo = declaration.CustomFileNo,

                                                       DeclarationNumber = declaration.DeclarationNumber,

                                                       //ExporterCode = card.VatNumber
                                                       ExporterCode = client.Code,

                                                       StorageStatusIsOpen = en.StorageStatus != null && en.StorageStatus.ToLower() == "open",

                                                       ActionCode= en.ExportLogisticPermitAction.LocalName,
                                                       ProcedureCurrentName =declaration.GovernmentProcedureCurrent.LocalName
                                                   });
            return query;
        }

        private IQueryable<ExportStorage> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ExportStorage> iQueryable, int tenant)
        {
            var filter = queryOperations.QueryFilterItems.FirstOrDefault(x => x.FieldName == "DeclarationIdAndProcedureCurrentName");
            if (filter != null)
            {
                iQueryable = iQueryable.Where(x => x.DeclarationId == null || (x.DeclarationId != null  && x.DeclarationEntity.GovernmentProcedureCurrent.LocalName.Contains("המכלה")));
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

