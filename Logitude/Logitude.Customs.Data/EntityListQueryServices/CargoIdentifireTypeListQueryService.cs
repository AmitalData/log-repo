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

    public partial class CargoIdentifireTypeListQueryService
    {
	    private IQueryable<CargoIdentifireTypeList> GetIqueryableList(IQueryable<CargoIdentifireType> iQueryable)
        {
            IQueryable<CargoIdentifireTypeList> query = (from a in iQueryable
                                                         select new CargoIdentifireTypeList()
                                                     {
                                                         Code = a.Code,
                                                         EnglishName = a.EnglishName,
                                                         LocalName = a.LocalName,
                                                         SearchFields = a.SearchFields,
                                                         Inactive = a.Inactive,
                                                         IsForDeclarationExport=a.IsForDeclarationExport,
                                                         IsForDeclarationImport=a.IsForDeclarationImport,
                                                         IsForManifest=a.IsForManifest,
                                                         IsKey2Mandatory=a.IsKey2Mandatory,
                                                         IsKey3Mandatory=a.IsKey3Mandatory,
                                                         CargoIdentifierKey1Name=a.CargoIdentifierKey1Name,
                                                         CargoIdentifierKey2Name=a.CargoIdentifierKey2Name,
                                                         CargoIdentifierKey3Name=a.CargoIdentifierKey3Name,
                                                     });
            return query;
		}

        private IQueryable<CargoIdentifireType> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CargoIdentifireType> iQueryable)
        {
            return iQueryable;
        }
	}


}
	