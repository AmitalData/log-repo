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

    public partial class ExportStorgeCargoListQueryService
    {
	    private IQueryable<ExportStorgeCargoList> GetIqueryableList(IQueryable<ExportStorgeCargo> iQueryable)
        {
		IQueryable<ExportStorgeCargoList> query = (from a in iQueryable
                                            select new ExportStorgeCargoList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          SearchFields = a.SearchFields,
					
					                          CargoTypeCode = a.CargoTypeCode,
					
					                          Manifest = a.Manifest,
					
					                          SecondCargoID = a.SecondCargoID,
					
					                          ThirdCargoID = a.ThirdCargoID,
					
					                          CargoDescription = a.CargoDescription,
					
					                          CargoType = a.CargoType,
					
					                          HandlingCode = a.HandlingCode,
					
					                          DangerousGoodsIndication = a.DangerousGoodsIndication,
					
					                          CodeBreaksIndication = a.CodeBreaksIndication,
					
					                          DamageCode = a.DamageCode,
					
					                          ForeignCurrencyType = a.ForeignCurrencyType,
					
					                          ForeignCurrencyAmoun = a.ForeignCurrencyAmoun,
					
					                          GoodsValueNIS = a.GoodsValueNIS,
					
					                          PackageType = a.PackageType,
					
					                          Quantity = a.Quantity,
					
					                          MarksNumbers = a.MarksNumbers,
					
					                          WeightInPortMandatory = a.WeightInPortMandatory,
					
					                          Weight = a.Weight,
					
					                          VolumeSize = a.VolumeSize,
					
					                          LicensePlateNumber = a.LicensePlateNumber,
					
					                          CustomsItem = a.CustomsItem,
					
		                    	            });
            return query;
		}

		private IQueryable<ExportStorgeCargo> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ExportStorgeCargo> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	