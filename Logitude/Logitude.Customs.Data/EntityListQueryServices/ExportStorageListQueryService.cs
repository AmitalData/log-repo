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

           

            IQueryable<ExportStorageList> query = (from a in iQueryable
                                                    //DeclarationStatusTypeName = (
                                                    //    from status in context.DeclarationStatusTypes
                                                    //    where status.Code == (from Declaration in context.Declarations where Declaration.Id == a.DeclarationId select Declaration).FirstOrDefault().DeclarationStatusTypeCode
                                                    //    select status
                                                    //   ).FirstOrDefault().LocalName,
                                                    join d in context.Declarations.Select( r=> new { r.Id,r.DeclarationStatusTypeCode })
                                                    on a.DeclarationId  equals d.Id
                                                    join s in context.DeclarationStatusTypes
                                                    on d.DeclarationStatusTypeCode  equals s.Code


                                                   select new ExportStorageList()
                                                   {

                                                       Id = a.Id,

                                                       Tenant = a.Tenant,

                                                       SearchFields = a.SearchFields,

                                                       DeclarationId = a.DeclarationId,

                                                       ExportFileNo = a.ExportFileNo,

                                                       //StorageNo = a.StorageNo,

                                                       StorageStatus = a.StorageStatus,

                                                       CargoTypeCode = a.CargoTypeCode,
                                                       
                                                       OpenDate = a.OpenDate,

                                                       CargoType = a.CargoType,

                                                       CustomsStatus = a.CustomsStatus,

                                                       ExporterID = a.ExporterID,
                                                       
                                                       ShipCode = a.ShipCode,

                                                       FirstCargoID = a.FirstCargoID,

                                                       SecondCargoID = a.SecondCargoID,

                                                       ThirdCargoID = a.ThirdCargoID,

                                                       //ExportDealIdentification = a.ExportDealIdentification,

                                                       DeclarationStatusTypeName = s.LocalName
                                                       //(
                                                       // from status in context.DeclarationStatusTypes
                                                       // where status.Code == (from Declaration in context.Declarations where Declaration.Id == a.DeclarationId select Declaration).FirstOrDefault().DeclarationStatusTypeCode
                                                       // select status
                                                       //).FirstOrDefault().LocalName,




                                                   });
            return query;
        }

        private IQueryable<ExportStorage> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ExportStorage> iQueryable, int tenant)
        {
            return iQueryable;
        }
    }
}
