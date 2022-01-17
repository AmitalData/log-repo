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

    public partial class DeclarationCargoSplitListQueryService
    {
        private IQueryable<DeclarationCargoSplitList> GetIqueryableList(IQueryable<DeclarationCargoSplit> iQueryable)
        {
            IQueryable<DeclarationCargoSplitList> query = (from a in iQueryable
                                                           .Include("CargoIdentifireType").Include("ActionCode").Include("SplitOrMergeReason").Include("CargoSplitRequestStatus")
                                                           join db_dec in context.Declarations
                                                            on a.DeclarationId equals db_dec.Id into decs
                                                           from declarationCouldBeNull in decs.DefaultIfEmpty()

                                                           select new DeclarationCargoSplitList()
                                                           {
                                                               Id = a.Id,
                                                               Tenant = a.Tenant,
                                                               RequestDate = a.RequestDate,
                                                               SearchFields = a.SearchFields,
                                                               ActionTypeCode = a.ActionTypeCode,
                                                               ActionTypeName = a.ActionCode.LocalName != null ? a.ActionCode.LocalName : a.ActionCode.EnglishName,
                                                               CargoTypeCode = a.CargoTypeCode,
                                                               CargoTypeName = a.CargoIdentifireType.EnglishName,
                                                               DeclarationId = a.DeclarationId,
                                                               IsClosed = a.IsClosed,
                                                               ManifestNumber = a.ManifestNumber,
                                                               RequestNumber = a.RequestNumber,
                                                               RequestReason = a.RequestReason,
                                                               RequestReasonName = a.SplitOrMergeReason.LocalName != null ? a.SplitOrMergeReason.LocalName : a.SplitOrMergeReason.EnglishName,
                                                               RequestRemarks = a.RequestRemarks,
                                                               ResponseStatusCode = a.ResponseStatusCode,
                                                               ResponseStatusName = a.CargoSplitRequestStatus.LocalName != null ? a.CargoSplitRequestStatus.LocalName : a.CargoSplitRequestStatus.EnglishName,
                                                               SecondCargoID = a.SecondCargoID,
                                                               ThirdCargoID = a.ThirdCargoID,
                                                               CustomFileNo = declarationCouldBeNull != null ? declarationCouldBeNull.CustomFileNo : "",
                                                               TransportModeId = declarationCouldBeNull.TransportModeId,
                                                               Direction = declarationCouldBeNull.Direction,
                                                           });
            return query;
        }

        private IQueryable<DeclarationCargoSplit> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<DeclarationCargoSplit> iQueryable, int tenant)
        {
            return iQueryable;
        }
    }


}
