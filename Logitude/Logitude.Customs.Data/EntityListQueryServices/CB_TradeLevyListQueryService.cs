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

    public partial class CB_TradeLevyListQueryService
    {
	    private IQueryable<CB_TradeLevyList> GetIqueryableList(IQueryable<CB_TradeLevy> iQueryable)
        {
		IQueryable<CB_TradeLevyList> query = (from a in iQueryable
                                            select new CB_TradeLevyList()
											{
                     
					                          ID = a.ID,
					
					                          CreateDate = a.CreateDate,
					
					                          UpdateDate = a.UpdateDate,
					
					                          CustomsBookTypeID = a.CustomsBookTypeID,
					
					                          LevyNumber = a.LevyNumber,
					
					                          EndOfInquiryDate = a.EndOfInquiryDate,
					
					                          EndOfLevyDate = a.EndOfLevyDate,
					
					                          InceptionCodeID = a.InceptionCodeID,
					
					                          StartDate = a.StartDate,
					
					                          TradeLevyStatusID = a.TradeLevyStatusID,
					
					                          ComputationMethodDataID = a.ComputationMethodDataID,
					
					                          LevyTrustID = a.LevyTrustID,
					
					                          ParagraphTypeID = a.ParagraphTypeID,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_TradeLevy> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_TradeLevy> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	