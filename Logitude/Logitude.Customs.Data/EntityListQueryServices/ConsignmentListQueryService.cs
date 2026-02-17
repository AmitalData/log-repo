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

    public partial class ConsignmentListQueryService
    {
	    private IQueryable<ConsignmentList> GetIqueryableList(IQueryable<Consignment> iQueryable)
        {
            IQueryable<ConsignmentList> query = (from a in iQueryable.Include("CargoType").Include("OriginCountry").Include("ReceiverWarehouse").Include("StorageSite").Include("UnloadPort")
                                                 select new ConsignmentList()
                                                        {
                                                          DeclarationId = a.DeclarationId,
                                                          ConsignmentNumber = a.ConsignmentNumber,
                                                          CargoDescription = a.CargoDescription,
                                                          CargoTypeCode = a.CargoTypeCode,
                                                          CargoTypeName =a.CargoType.EnglishName,
                                                          IsLastReleaseFromWarehous  = a.IsLastReleaseFromWarehous,
                                                          LoadingPortCode = a.LoadingPortCode,
                                                          ManifestDate =a.ManifestDate,
                                                          ManifestNumber = a.ManifestNumber,
                                                          OriginCountryCode = a.OriginCountryCode,
                                                          OriginCountryName = a.OriginCountry.EnglishName,
                                                          ReceiverWarehouseCode = a.ReceiverWarehouseCode,
                                                          ReceiverWarehouseName  = a.RegisteredWarehouseSiteType.LocalName,
                                                          SecondCargoID =a.SecondCargoID,
                                                         // SequenceNumeric = a.SequenceNumeric,
                                                          StorageSiteCode = a.StorageSiteCode,
                                                          StorageSiteName  = a.DeliverySiteType.LocalName,
                                                          Tenant = a.Tenant,
                                                          ThirdCargoID = a.ThirdCargoID,
                                                          UnloadDate = a.UnloadDate,
                                                          UnloadPortCode = a.UnloadPortCode,
                                                          UnloadPortName = a.UnloadPort.EnglishName,

                                                          

                                                        });
            return query;
		}

        private IQueryable<Consignment> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Consignment> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	