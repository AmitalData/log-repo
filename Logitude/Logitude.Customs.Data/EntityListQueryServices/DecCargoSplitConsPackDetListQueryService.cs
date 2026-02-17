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

    public partial class DecCargoSplitConsPackDetListQueryService
    {
	    private IQueryable<DecCargoSplitConsPackDetList> GetIqueryableList(IQueryable<DecCargoSplitConsPackDet> iQueryable)
        {
		IQueryable<DecCargoSplitConsPackDetList> query = (from a in iQueryable.Include("PackingType")
                                                          select new DecCargoSplitConsPackDetList()
											{
                                                              DeclarationCargoSplitId = a.DeclarationCargoSplitId,
                                                              DecCargoSplitConsLineNo = a.DecCargoSplitConsLineNo,
                                                              GrossMassMeasure = a.GrossMassMeasure,
                                                              DecCargoSplitConsItemLine = a.DecCargoSplitConsItemLine,
                                                              PackageLine = a.PackageLine,
                                                              ManifestNumber = a.ManifestNumber,
                                                              MarksNumbers = a.MarksNumbers,
                                                              PackageQuantity = a.PackageQuantity,
                                                              PackageTypeCode = a.PackageTypeCode,
                                                              PackageTypeName = a.PackingType.LocalName,
                                                          });
            return query;
		}

		private IQueryable<DecCargoSplitConsPackDet> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DecCargoSplitConsPackDet> iQueryable, int tenant)
        {
            return iQueryable;
        }
			}


}
	