using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.QuoteModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InfrastructureModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;

using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel;

 namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{ 
   public partial class DocumentsFilingQueryService
   {
   
		ICommonDataContext  context;
		//DocumentsFilingService service; 
		
		DocumentsFilingQuery query; 

        public DocumentsFilingQueryService(int tenant)
        {
				    context = CommonDataContext.GetContext(tenant); 
			//service = new DocumentsFilingService(context, tenant); 
			query = new DocumentsFilingQuery(tenant);
        }

		
		public DocumentsFiling GetDocumentsFilingById(string Id,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePM(Id,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("DocumentsFiling with Id " + Id + " doesn't exist");

				return DocumentsFilingDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public DocumentsFiling GetDocumentsFilingByCode(string Code,int Tenant)
        { 
		    try
            {

				
				var temp = query.GetSinglePMByCode(Code,Tenant);				
				 if (temp == null)
                    throw new ApplicationException("DocumentsFiling with Code " + Code + " doesn't exist");

				return DocumentsFilingDataMapping(temp,Tenant);
			}
            catch (Exception ex)
            {

                throw ex;
            }
        }
		
		public DocumentsFiling DocumentsFilingDataMapping(DocumentsFilingPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new DocumentsFiling(); 
				   temp.Id = MyEntityPM.Id;
				   temp.Code = MyEntityPM.Code;			  
				   if(MyEntityPM.CreatedByUserId != null)
				   {
					   UserQueryService UserService0 = new UserQueryService(Tenant);
					   					   temp.CreatedByUser = UserService0.GetUserById(MyEntityPM.CreatedByUserId,Tenant); 
			       
					   				   }
				   
				   temp.EntityNumber = MyEntityPM.EntityReference;			  
				   if(MyEntityPM.ObjectTableId != null)
				   {
					   ObjectTableQueryService ObjectTableService1 = new ObjectTableQueryService(Tenant);
					   					   temp.EntityType = ObjectTableService1.GetObjectTableById(MyEntityPM.ObjectTableId,Tenant); 
			       
					   				   }
				   			  
				   if(MyEntityPM.DocumentTypeId != null)
				   {
					   DocumentTypeQueryService DocumentTypeService2 = new DocumentTypeQueryService(Tenant);
					   					   temp.DocumentType = DocumentTypeService2.GetDocumentTypeById(MyEntityPM.DocumentTypeId,Tenant); 
			       
					   				   }
				   
				   temp.BlobId = MyEntityPM.DocumentId;
				   temp.IsDigitallySigned = MyEntityPM.IsDigitallySigned;
				   temp.SignersList = MyEntityPM.SignersList;
				   temp.BlobName = MyEntityPM.FileName;
				   temp.Description = MyEntityPM.Description;
				   temp.IsSharedWithCustomer = MyEntityPM.IsSharedWithCustomer;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public DocumentsFilingPM DocumentsFilingDataMappingAndValidatin(DocumentsFiling MyEntity,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   					var temp = new DocumentsFilingPM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
					
					if (!string.IsNullOrEmpty(MyEntity.Code))
					{
						temp = query.GetSinglePMByCode(MyEntity.Code, Tenant);
					} 					   
					if(temp == null)
					{   
					    throw new ApplicationException("DocumentsFiling with Code " + MyEntity.Code + " doesn't exist");
					} 
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(MyEntity.Id))
					    {
					        throw new ApplicationException("DocumentsFiling with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = MyEntity.Id;

						//}
					}
					if(string.IsNullOrEmpty(temp.Code))
					{
					   
						temp.Code = MyEntity.Code;
					}
					UserQueryService CreatedByUserUserService = new UserQueryService(Tenant);
					if(MyEntity.CreatedByUser != null)
					{
						var myCreatedByUserPM = CreatedByUserUserService.UserDataMappingAndValidatin(MyEntity.CreatedByUser,Tenant,ComputingPartnerName);
												if(myCreatedByUserPM != null)
						{
							temp.CreatedByUserId = myCreatedByUserPM.Id;
						}
						 
					}
			
					
					temp.EntityReference = MyEntity.EntityNumber;
					ObjectTableQueryService EntityTypeObjectTableService = new ObjectTableQueryService(Tenant);
					if(MyEntity.EntityType != null)
					{
						var myEntityTypePM = EntityTypeObjectTableService.ObjectTableDataMappingAndValidatin(MyEntity.EntityType,Tenant,ComputingPartnerName);
												if(myEntityTypePM != null)
						{
							temp.ObjectTableId = myEntityTypePM.Id;
						}
						 
					}
			
					
					DocumentTypeQueryService DocumentTypeDocumentTypeService = new DocumentTypeQueryService(Tenant);
					if(MyEntity.DocumentType != null)
					{
						var myDocumentTypePM = DocumentTypeDocumentTypeService.DocumentTypeDataMappingAndValidatin(MyEntity.DocumentType,Tenant,ComputingPartnerName);
												if(myDocumentTypePM != null)
						{
							temp.DocumentTypeId = myDocumentTypePM.Id;
						}
						 
					}
			
					
					temp.DocumentId = MyEntity.BlobId;
					temp.IsDigitallySigned = MyEntity.IsDigitallySigned;
					temp.SignersList = MyEntity.SignersList;
					temp.FileName = MyEntity.BlobName;
					temp.Description = MyEntity.Description;
					temp.IsSharedWithCustomer = MyEntity.IsSharedWithCustomer;					   
					   return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}