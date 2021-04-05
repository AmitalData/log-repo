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

    public partial class ExportStorgeListQueryService
    {
	    private IQueryable<ExportStorgeList> GetIqueryableList(IQueryable<ExportStorge> iQueryable)
        {
		IQueryable<ExportStorgeList> query = (from a in iQueryable
                                            select new ExportStorgeList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          SearchFields = a.SearchFields,
					
					                          DeclarationId = a.DeclarationId,
					
					                          ExportFileNo = a.ExportFileNo,
					
					                          OrderNo = a.OrderNo,
					
					                          CustomFileNo = a.CustomFileNo,
					
					                          StorageNo = a.StorageNo,
					
					                          VoyageNo = a.VoyageNo,
					
					                          StorageDate = a.StorageDate,
					
					                          StorageStatus = a.StorageStatus,
					
					                          OperationCode = a.OperationCode,
					
					                          SenderCodeID = a.SenderCodeID,
					
					                          MessageFromForm = a.MessageFromForm,
					
					                          ReplyPhoneNumeric = a.ReplyPhoneNumeric,
					
					                          OperatorID = a.OperatorID,
					
					                          InformedParty = a.InformedParty,
					
					                          DeclarationNumber = a.DeclarationNumber,
					
					                          DeclarationsInContainer = a.DeclarationsInContainer,
					
					                          ExportManifestNumber = a.ExportManifestNumber,
					
					                          ReceivingSite = a.ReceivingSite,
					
					                          StuffingSiteType = a.StuffingSiteType,
					
					                          LoadingSite = a.LoadingSite,
					
		                    	            });
            return query;
		}

		private IQueryable<ExportStorge> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ExportStorge> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	