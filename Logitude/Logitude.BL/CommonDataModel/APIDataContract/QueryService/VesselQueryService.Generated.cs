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
   public partial class VesselQueryService
   {
   
		ICommonDataContext  context;
		//VesselService service; 
		
		VesselQuery query; 

        public VesselQueryService(int tenant)
        {
				    context = CommonDataContext.GetContext(tenant); 
			//service = new VesselService(context, tenant); 
			query = new VesselQuery(tenant);
        }

		
		public Vessel GetVesselById(string Id,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePM(Id, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Vessel with Id " + Id + " doesn't exist");

				return VesselDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public Vessel GetVesselByCode(string Code,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePMByCode(Code, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Vessel with Code " + Code + " doesn't exist");

				return VesselDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public Vessel VesselDataMapping(VesselPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new Vessel(); 
				   temp.Id = MyEntityPM.Id;
				   temp.Code = MyEntityPM.Code;
				   temp.EnglishName = MyEntityPM.EnglishName;
				   ComputingPartnerTranslationHelper helper = new ComputingPartnerTranslationHelper(Tenant); 
				   temp.PartnerCode = helper.GetComputingPartnerCodeTranslation(MyEntityPM.Code,ComputingPartnerName,"Vessel");  					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public VesselPM VesselDataMappingAndValidatin(Vessel MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   					var temp = new VesselPM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
					
					if (!string.IsNullOrEmpty(MyEntity.Code))
					{
						temp = query.GetSinglePMByCode(MyEntity.Code, Tenant  );
					} 
					if (!string.IsNullOrEmpty(MyEntity.PartnerCode))
					{
                        if(string.IsNullOrEmpty(ComputingPartnerName))
                            throw new ApplicationException("ComputingPartnerCode is required");
						ComputingPartnerTranslationHelper helper = new ComputingPartnerTranslationHelper(Tenant);
						var MyCode = helper.GetLogitudeCodeTranslation(MyEntity.PartnerCode,ComputingPartnerName,"Vessel");
					    if(string.IsNullOrEmpty(MyCode))
						{
						  throw new ApplicationException("Vessel with Partner Code " + MyEntity.PartnerCode + " doesn't match any record");
						}
						temp = query.GetSinglePMByCode(MyCode, Tenant );
						
						
					}
					
					   					   
					if(temp == null)
					{   
					    throw new ApplicationException("Vessel with Code " + MyEntity.Code + " doesn't exist");
					} 
					
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(MyEntity.Id))
					    {
					        throw new ApplicationException("Vessel with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = MyEntity.Id;

						//} 

						
					}
					if(string.IsNullOrEmpty(temp.Code))
					{
					   
						 
						if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.Code))
						{
								//throw new ApplicationException("Code Can't be update"); 
								temp.Code = MyEntity.Code;
								
						
						}  

						
					}
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.EnglishName))
					{							//throw new ApplicationException("EnglishName Can't be update"); 
							temp.EnglishName = MyEntity.EnglishName;

										}  

					
					if(string.IsNullOrEmpty(temp.Code))
					{
					   
						 
						if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.PartnerCode))
						{
								//throw new ApplicationException("PartnerCode Can't be update"); 
								temp.Code = MyEntity.PartnerCode;
								
						
						}  

						
					}					   
					return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }
		 
   }
}