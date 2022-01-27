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
                                                   
                                                   join d in context.Declarations.Select(r => new { r.Id, r.DeclarationStatusTypeCode, r.CustomFileNo, r.DeclarationNumber })
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

                                                   join c in context.Cards.Select( r=> new {r.Id, r.LocalName, r.VatNumber})
                                                   on en.ExporterID equals c.Id
                                                   into cj
                                                   from card in cj.DefaultIfEmpty()

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

                                                       CustomsStatus = en.CustomsStatus,

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

                                                       ExporterName = card.LocalName,

                                                       ShipName = customsShip.LocalName,

                                                       StorErrorXML = en.StorErrorXML,

                                                       StorageNo = en.StorageNo,

                                                       ExportDealIdentification = en.ExportDealIdentification,

                                                       CargoTypeCodeName = cargoIdentifireType.LocalName,

                                                       DeclarationStatusTypeCode = status.Code,

                                                       Declaration_ID = en.DeclarationId,

                                                       DeclarationCustomFileNo = declaration.CustomFileNo,

                                                       DeclarationNumber = declaration.DeclarationNumber,

                                                       ExporterCode = card.VatNumber
                                                   });
            return query;
        }

        private IQueryable<ExportStorage> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ExportStorage> iQueryable, int tenant)
        {
            return iQueryable;
        }
    }
}
