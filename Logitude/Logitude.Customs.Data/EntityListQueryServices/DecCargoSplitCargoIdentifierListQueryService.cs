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

    public partial class DecCargoSplitCargoIdentifierListQueryService
    {
	    private IQueryable<DecCargoSplitCargoIdentifierList> GetIqueryableList(IQueryable<DecCargoSplitCargoIdentifier> iQueryable)
        {
		IQueryable<DecCargoSplitCargoIdentifierList> query = (from a in iQueryable
                                            select new DecCargoSplitCargoIdentifierList()
											{
                                                DeclarationCargoSplitId = a.DeclarationCargoSplitId,
                                                LineNumber = a.LineNumber,
                                                CargoIdentifierKey1 = a.CargoIdentifierKey1,
                                                CargoIdentifierKey2 = a.CargoIdentifierKey2,
                                                CargoIdentifierKey3 = a.CargoIdentifierKey3,
                                            });
            return query;
		}

		private IQueryable<DecCargoSplitCargoIdentifier> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DecCargoSplitCargoIdentifier> iQueryable, int tenant)
        {
            return iQueryable;
        }
			}


}
	