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

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class Aur_ItemListQueryService
    {
	    private IQueryable<Aur_ItemList> GetIqueryableList(IQueryable<Aur_Item> iQueryable)
        {
		IQueryable<Aur_ItemList> query = (from a in iQueryable
                                            select new Aur_ItemList()
											{
                     
					                          Line = a.Line,
					
					                          SalesOrderid = a.SalesOrderid,
					
					                          RelatedContract = a.RelatedContract,
					
					                          ProductNumber = a.ProductNumber,
					
					                          ProductName = a.ProductName,
					
					                          PricePerUnit = a.PricePerUnit,
					
					                          Quantity = a.Quantity,
					
					                          Discount = a.Discount,
					
					                          BaseAmount = a.BaseAmount,
					
					                          Tax = a.Tax,
					
					                          ExtendedAmount = a.ExtendedAmount,
					
		                    	            });
            return query;
		}

		private IQueryable<Aur_Item> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Aur_Item> iQueryable, int tenant)
        {
            return iQueryable;
        }
				private IQueryable<Aur_Item> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<Aur_Item> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	