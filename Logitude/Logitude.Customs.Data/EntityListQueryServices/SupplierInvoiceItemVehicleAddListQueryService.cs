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

    public partial class SupplierInvoiceItemVehicleAddListQueryService
    {
	    private IQueryable<SupplierInvoiceItemVehicleAddList> GetIqueryableList(IQueryable<SupplierInvoiceItemVehicleAdd> iQueryable)
        {
		IQueryable<SupplierInvoiceItemVehicleAddList> query = (from a in iQueryable
                                            select new SupplierInvoiceItemVehicleAddList()
											{
                     
					                          DeclarationId = a.DeclarationId,
					
					                          InvoiceCounterKey = a.InvoiceCounterKey,
					
					                          InvoiceItemLineNumber = a.InvoiceItemLineNumber,
					
					                          LineNumber = a.LineNumber,
					
					                          VehicleModel = a.VehicleModel,
					
					                          RichbitNumber = a.RichbitNumber,
					
					                          ChassisNumber = a.ChassisNumber,
					
					                          EngineNumber = a.EngineNumber,
					
					                          VehicleValue = a.VehicleValue,
					
					                          ChassisTax = a.ChassisTax,
					
					                          ChassisPurchaseTax = a.ChassisPurchaseTax,
					
					                          ChassisVat = a.ChassisVat,
					
					                          Exempt_type = a.Exempt_type,
					
		                    	            });
            return query;
		}

		private IQueryable<SupplierInvoiceItemVehicleAdd> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<SupplierInvoiceItemVehicleAdd> iQueryable,int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	