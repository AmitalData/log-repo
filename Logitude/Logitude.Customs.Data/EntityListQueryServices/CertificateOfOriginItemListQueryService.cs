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

    public partial class CertificateOfOriginItemListQueryService
    {
	    private IQueryable<CertificateOfOriginItemList> GetIqueryableList(IQueryable<CertificateOfOriginItem> iQueryable)
        {
		IQueryable<CertificateOfOriginItemList> query = (from a in iQueryable
                                            select new CertificateOfOriginItemList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          SearchFields = a.SearchFields,
					
					                          CertificateOfOriginId = a.CertificateOfOriginId,
					
					                          ItemSerial = a.ItemSerial,
					
					                          ItemId = a.ItemId,
					
					                          OriginCriterionCode = a.OriginCriterionCode,
					
					                          MarksAndNumbers = a.MarksAndNumbers,
					
					                          PackageQuantity = a.PackageQuantity,
					
					                          PackageType = a.PackageType,
					
					                          ItemDescription = a.ItemDescription,
					
					                          Weight = a.Weight,
					
					                          MeasureType = a.MeasureType,
					
					                          InvoiceConnect = a.InvoiceConnect,
					
					                          ContainerIsoCode = a.ContainerIsoCode,
					
		                    	            });
            return query;
		}

		private IQueryable<CertificateOfOriginItem> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CertificateOfOriginItem> iQueryable, int tenant)
        {
            return iQueryable;
        }
    }


}
	