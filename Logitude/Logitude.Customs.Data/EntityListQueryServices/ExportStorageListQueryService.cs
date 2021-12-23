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
                                                   
                                                   join declaration in context.Declarations.Select(r => new { r.Id, r.DeclarationStatusTypeCode })
                                                   on en.DeclarationId equals declaration.Id
                                                   join status in context.DeclarationStatusTypes.Select(r => new { r.Code, r.LocalName })
                                                   on declaration.DeclarationStatusTypeCode equals status.Code

                                                   join cargoType in context.CargoTypes.Select(r => new { r.Code, r.LocalName})
                                                   on en.CargoType equals cargoType.Code

                                                   join storageStatus in context.StorageStatuses.Select( r=> new {r.Code, r.LocalName})
                                                   on en.StorageStatus equals storageStatus.Code

                                                   join card in context.Cards.Select( r=> new {r.Id, r.LocalName})
                                                   on en.ExporterID equals card.Id

                                                   join customsShip in context.CustomsShips.Select( r=> new {r.Code, r.LocalName})
                                                   on en.ShipCode equals customsShip.Code

                                                   join cargoIdentifireType in context.CargoIdentifireTypes.Select(r=> new{ r.Code, r.LocalName})
                                                   on en.CargoTypeCode equals cargoIdentifireType.Code

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

                                                       StorageStatusName = storageStatus.LocalName,

                                                       ExporterName = card.LocalName,

                                                       ShipName = customsShip.LocalName,

                                                       StorErrorXML = en.StorErrorXML,

                                                       StorageNo = en.StorageNo,

                                                       ExportDealIdentification = en.ExportDealIdentification,

                                                       CargoTypeCodeName = cargoIdentifireType.LocalName,

                                                       DeclarationStatusTypeCode = status.Code
                                                   });
            return query;
        }

        private IQueryable<ExportStorage> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ExportStorage> iQueryable, int tenant)
        {
            return iQueryable;
        }
    }
}
