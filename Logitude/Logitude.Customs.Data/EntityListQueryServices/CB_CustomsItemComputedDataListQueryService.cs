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

    public partial class CB_CustomsItemComputedDataListQueryService
    {
	    private IQueryable<CB_CustomsItemComputedDataList> GetIqueryableList(IQueryable<CB_CustomsItemComputedData> iQueryable)
        {
		IQueryable<CB_CustomsItemComputedDataList> query = (from a in iQueryable
                                            select new CB_CustomsItemComputedDataList()
											{
                     
					                          CB_ID = a.CB_ID,
					
					                          ID = a.ID,
					
					                          CustomsItemID = a.CustomsItemID,
					
					                          FullClassification = a.FullClassification,
					
					                          IsLeaf = a.IsLeaf,
					
					                          CustomsItemDetailsHistoryID = a.CustomsItemDetailsHistoryID,
					
					                          PropertiesDetailsHistoryID = a.PropertiesDetailsHistoryID,
					
					                          PH_MeasurementUnitID = a.PH_MeasurementUnitID,
					
					                          IsHistoryExists = a.IsHistoryExists,
					
					                          IsRulesExists = a.IsRulesExists,
					
					                          StartDate = a.StartDate,
					
					                          EndDate = a.EndDate,
					
					                          CI_Parent_CustomsItemIDNum = a.CI_Parent_CustomsItemIDNum,
					
					                          CI_BaseFullClassification = a.CI_BaseFullClassification,
					
					                          CI_ComputedCheckDigit = a.CI_ComputedCheckDigit,
					
					                          CI_CustomsBookTypeIDNum = a.CI_CustomsBookTypeIDNum,
					
					                          CI_CustomsItemCategoryIDNum = a.CI_CustomsItemCategoryIDNum,
					
					                          ItemHierarchicLocationID = a.ItemHierarchicLocationID,
					
					                          CIH_Title = a.CIH_Title,
					
					                          CIH_GoodsDescription = a.CIH_GoodsDescription,
					
					                          CustomsItemEntityStatusIDNum = a.CustomsItemEntityStatusIDNum,
					
					                          PH_IsCarItem = a.PH_IsCarItem,
					
					                          FullGoodsDescription = a.FullGoodsDescription,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_CustomsItemComputedData> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_CustomsItemComputedData> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	