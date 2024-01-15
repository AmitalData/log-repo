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

    public partial class CB_TradeAgreementListQueryService
    {
	    private IQueryable<CB_TradeAgreementList> GetIqueryableList(IQueryable<CB_TradeAgreement> iQueryable)
        {
		IQueryable<CB_TradeAgreementList> query = (from a in iQueryable
                                            select new CB_TradeAgreementList()
											{
                     
					                          ID = a.ID,
					
					                          CreateDate = a.CreateDate,
					
					                          UpdateDate = a.UpdateDate,
					
					                          Title = a.Title,
					
					                          AdditionName = a.AdditionName,
					
					                          CountryGroupID = a.CountryGroupID,
					
					                          CustomsBookTypeID = a.CustomsBookTypeID,
					
					                          EntityStatusID = a.EntityStatusID,
					
					                          TradeAgreementAbbreviation = a.TradeAgreementAbbreviation,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_TradeAgreement> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_TradeAgreement> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	