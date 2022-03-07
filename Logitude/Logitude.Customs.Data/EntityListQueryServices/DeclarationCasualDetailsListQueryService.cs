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

    public partial class DeclarationCasualDetailsListQueryService
    {
	    private IQueryable<DeclarationCasualDetailsList> GetIqueryableList(IQueryable<DeclarationCasualDetails> iQueryable)
        {
		IQueryable<DeclarationCasualDetailsList> query = (from a in iQueryable
                                            select new DeclarationCasualDetailsList()
											{
                     
					                          DeclarationId = a.DeclarationId,
					
					                          Tenant = a.Tenant,
					
					                          //CasualSupplierName = a.CasualSupplierName,
					
					                          //CasualSupplierAddress = a.CasualSupplierAddress,
					
					                          CasualImporterAddress1 = a.CasualImporterAddress1,
					
					                          CasualImporterAddress2 = a.CasualImporterAddress2,
					
					                          CasualImporterCity = a.CasualImporterCity,
					
					                          CasualImporterZipCode = a.CasualImporterZipCode,
					
					                          CasualImporterFax = a.CasualImporterFax,
					
					                          CasualImporterEmail = a.CasualImporterEmail,
					
					                          CasualImporterTel = a.CasualImporterTel,
					
					                          CasualImporterContact = a.CasualImporterContact,
											  ImporterCode = a.ImporterCode,
											  ImporterAddress = a.ImporterAddress,
											  ImporterName = a.ImporterName,
											  EntitleImporterAddress = a.EntitleImporterAddress,
											  EntitleImporterCode = a.EntitleImporterCode,
											  EntitleImporterName = a.EntitleImporterName,
											  EntitlePassportNumber = a.EntitlePassportNumber,
											  ImporterPassportNumber = a.ImporterPassportNumber,
											  TransferImporterAddress = a.TransferImporterAddress,
											  TransferImporterName = a.TransferImporterName,
											  TransferPassportNumber = a.TransferPassportNumber
					
		                    	            });
            return query;
		}

		private IQueryable<DeclarationCasualDetails> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DeclarationCasualDetails> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	