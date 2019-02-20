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

    public partial class ConsignmentPackageListQueryService
    {
	    private IQueryable<ConsignmentPackageList> GetIqueryableList(IQueryable<ConsignmentPackage> iQueryable)
        {
            IQueryable<ConsignmentPackageList> query = (from a in iQueryable.Include("PackageMeasureQualifier").Include("PackingType").Include("GrossMassMeasurmentUnit")
                                                        select new ConsignmentPackageList()
                                                  {
                                                     ConsignmentNumber = a.ConsignmentNumber,
                                                     DeclarationId = a.DeclarationId,
                                                     GrossMassMeasure= a.GrossMassMeasure,
                                                     LineNumber = a.LineNumber,
                                                     MarksNumbers = a.MarksNumbers,
                                                    PackageMeasureQualifierCode = a.PackageMeasureQualifierCode,
                                                    PackageMeasureQualifierName = a.PackageMeasureQualifier.EnglishName,
                                                    PackageQuantity= a.PackageQuantity,
                                                    PackageTypeCode = a.PackageTypeCode,
                                                    PackageTypeName= a.PackingType.EnglishName,
                                                    GrossMassMeasureTypeCode = a.GrossMassMeasureTypeCode,
                                                    GrossMassMeasureTypeName = a.GrossMassMeasurmentUnit.EnglishName,
                                                    Tenant = a.Tenant,



                                                  });
            return query;
		}

        private IQueryable<ConsignmentPackage> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ConsignmentPackage> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	