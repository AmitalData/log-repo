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

    public partial class DeclarationReferantDataListQueryService
    {
	    private IQueryable<DeclarationReferantDataList> GetIqueryableList(IQueryable<DeclarationReferantData> iQueryable)
        {
		IQueryable<DeclarationReferantDataList> query = (from a in iQueryable
                                            select new DeclarationReferantDataList()
											{
                     
					                          DeclarationId = a.DeclarationId,
					
					                          OrderNumber = a.OrderNumber,
					
					                          ArrivalDate = a.ArrivalDate,
					
					                          EstimatedArrivalDate = a.EstimatedArrivalDate,
					
					                          Weight = a.Weight,
					
					                          ClassificationStatus = a.ClassificationStatus,
					
					                          ControllerStatus = a.ControllerStatus,
					
					                          CollectionOfMoneyStatus = a.CollectionOfMoneyStatus,
					
					                          FollowUpDate = a.FollowUpDate,
					
					                          IsExceptional = a.IsExceptional,
					
					                          WithPaper = a.WithPaper,
					
					                          IsClosedForFollowUp = a.IsClosedForFollowUp,
					
					                          IsClassificationRemarks = a.IsClassificationRemarks,
					
					                          IsControllerRemarks = a.IsControllerRemarks,
					
					                          PreClassification = a.PreClassification,
					
		                    	            });
            return query;
		}

		private IQueryable<DeclarationReferantData> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DeclarationReferantData> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	