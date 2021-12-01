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
   public partial class PortQueryService
   {
   
		ICommonDataContext  context;
		//PortService service; 
		
		PortQuery query; 

        public PortQueryService(int tenant)
        {
				    context = CommonDataContext.GetContext(tenant); 
			//service = new PortService(context, tenant); 
			query = new PortQuery(tenant);
        }

		
		public Port GetPortById(string Id,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePM(Id, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Port with Id " + Id + " doesn't exist");

				return PortDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public Port GetPortByCombinedCode(string CombinedCode,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePMByCombinedCode(CombinedCode, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Port with CombinedCode " + CombinedCode + " doesn't exist");

				return PortDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public Port PortDataMapping(PortPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new Port(); 
				   temp.Id = MyEntityPM.Id;
				   temp.Code = MyEntityPM.CombinedCode;
				   temp.LocalName = MyEntityPM.LocalName;
				   temp.EnglishName = MyEntityPM.EnglishName;
				   ComputingPartnerTranslationHelper helper = new ComputingPartnerTranslationHelper(Tenant); 
				   temp.PartnerCode = helper.GetComputingPartnerCodeTranslation(MyEntityPM.CombinedCode,ComputingPartnerName,"Port");   

			  
				   if(MyEntityPM.CountryId != null)
				   {
					   CountryQueryService CountryService0 = new CountryQueryService(Tenant);
					   					   temp.Country = CountryService0.GetCountryById(MyEntityPM.CountryId,Tenant,ComputingPartnerName); 
			       
					   				   }
				    

			  
				   if(MyEntityPM.StateId != null)
				   {
					   StateQueryService StateService1 = new StateQueryService(Tenant);
					   					   temp.State = StateService1.GetStateById(MyEntityPM.StateId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public PortPM PortDataMappingAndValidatin(Port MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false)
        {
		    try
            {
				   					var temp = new PortPM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
					
					if (!string.IsNullOrEmpty(MyEntity.Code))
					{
						temp = query.GetSinglePMByCombinedCode(MyEntity.Code, Tenant  );
					} 
					if (!string.IsNullOrEmpty(MyEntity.PartnerCode))
					{
                        if(string.IsNullOrEmpty(ComputingPartnerName))
                            throw new ApplicationException("ComputingPartnerCode is required");
						ComputingPartnerTranslationHelper helper = new ComputingPartnerTranslationHelper(Tenant);
						var MyCode = helper.GetLogitudeCodeTranslation(MyEntity.PartnerCode,ComputingPartnerName,"Port");
					    if(string.IsNullOrEmpty(MyCode))
						{
						  throw new ApplicationException("Port with Partner Code " + MyEntity.PartnerCode + " doesn't match any record");
						}
						temp = query.GetSinglePMByCombinedCode(MyCode, Tenant );
						
						
					}
					
					   					   
					if(temp == null)
					{   
					    throw new ApplicationException("Port with Code " + MyEntity.Code + " doesn't exist");
					} 
					
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(MyEntity.Id))
					    {
					        throw new ApplicationException("Port with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = MyEntity.Id;

						//} 

						
					}
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.Code))
					{							//throw new ApplicationException("Code Can't be update"); 
							temp.CombinedCode = MyEntity.Code;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.LocalName))
					{							//throw new ApplicationException("LocalName Can't be update"); 
							temp.LocalName = MyEntity.LocalName;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.EnglishName))
					{							//throw new ApplicationException("EnglishName Can't be update"); 
							temp.EnglishName = MyEntity.EnglishName;

										}  

					
                    
					if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.PartnerCode))
					{							//throw new ApplicationException("PartnerCode Can't be update"); 
							temp.CombinedCode = MyEntity.PartnerCode;

										}  

					
					CountryQueryService CountryCountryService = new CountryQueryService(Tenant);
					if(MyEntity.Country != null)
					{
						var myCountryPM = CountryCountryService.CountryDataMappingAndValidatin(MyEntity.Country,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myCountryPM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("Country Can't be update"); 
								temp.CountryId = myCountryPM.Id;
						  
							}  

							
						} 

					}
			
					
					StateQueryService StateStateService = new StateQueryService(Tenant);
					if(MyEntity.State != null)
					{
						var myStatePM = StateStateService.StateDataMappingAndValidatin(MyEntity.State,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myStatePM != null)
						{ 

						 
							if(!IsUpdate)
							{								//throw new ApplicationException("State Can't be update"); 
								temp.StateId = myStatePM.Id;
						  
							}  

							
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