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
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
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
using Microsoft.Data.OData.Query.SemanticAst;

namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{ 
   public partial class CardQueryService
   {
   
		ICommonDataContext  context;
		//CardService service; 
		
		CardQuery query; 

        public CardQueryService(int tenant)
        {
				    context = CommonDataContext.GetContext(tenant); 
			//service = new CardService(context, tenant); 
			query = new CardQuery(tenant);
        }

		
		public Card GetCardById(string Id,int Tenant,  string ComputingPartnerName = "",bool IncludeAddress = true)
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePM(Id, Tenant, IncludeAddress);				
				 if (temp == null)
                    throw new ApplicationException("Card with Id " + Id + " doesn't exist");

				return CardDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public Card GetCardByCode(string Code,int Tenant,  string ComputingPartnerName = "")
        { 
		    try
            {
				 
				
				var temp = query.GetSinglePMByCode(Code, Tenant);				
				 if (temp == null)
                    throw new ApplicationException("Card with Code " + Code + " doesn't exist");

				return CardDataMapping(temp,Tenant,ComputingPartnerName);
			}

            catch (Exception ex)
            {
                throw ex;
            }
        }
		
		public Card CardDataMapping(CardPM MyEntityPM,int Tenant,string ComputingPartnerName = "")
        {
		    try
            {
				   
				   var temp = new Card(); 
				   temp.Id = MyEntityPM.Id;
				   temp.EnglishName = MyEntityPM.EnglishName;
				   temp.LocalName = MyEntityPM.LocalName;
				   temp.Code = MyEntityPM.Code; 

			  
				   if(MyEntityPM.MainAddressId != null)
				   {
					   AddressQueryService AddressService0 = new AddressQueryService(Tenant);
					   					   temp.MainAddress = AddressService0.GetAddressById(MyEntityPM.MainAddressId,Tenant,ComputingPartnerName); 
			       
					   				   }
				   
				   temp.VatNumber = MyEntityPM.VatNumber;
				   ComputingPartnerTranslationHelper helper = new ComputingPartnerTranslationHelper(Tenant); 
				   temp.PartnerCode = helper.GetComputingPartnerCodeTranslation(MyEntityPM.Code,ComputingPartnerName,"Card");  
				   temp.IsDisconnectedFromGLAccount = MyEntityPM.IsDisconnectedFromGLAccount;
				   temp.ReceivablesAccountingCard = MyEntityPM.ReceivablesAccountingCard;
				   temp.PayablesAccountingCard = MyEntityPM.PayablesAccountingCard;
				   temp.ICAO = MyEntityPM.ICAO;
				   temp.AllowUnassignedEntry = MyEntityPM.AllowUnassignedEntry;					
				   return temp;
			}
            catch (Exception ex)
            {

                throw ex;
            }
        } 

		public CardPM CardDataMappingAndValidatin(Card MyEntity,int Tenant,string ComputingPartnerName = "",bool IsUpdate = false, List<string> partnerTypes = null)
        {
		    try
            {
				   					var temp = new CardPM();								  
					if (!string.IsNullOrEmpty(MyEntity.Id))
					{
						temp = query.GetSinglePM(MyEntity.Id, Tenant);
					} 
					
					if (!string.IsNullOrEmpty(MyEntity.Code))
					{
						if (partnerTypes != null && partnerTypes.Any())
							temp = query.GetSinglePMByCodePartnerTypes(MyEntity.Code, Tenant, partnerTypes);
						else
							temp = query.GetSinglePMByCode(MyEntity.Code, Tenant);
					} 


					if (!string.IsNullOrEmpty(MyEntity.PartnerCode))
					{
                        if(string.IsNullOrEmpty(ComputingPartnerName))
                            throw new ApplicationException("ComputingPartnerCode is required");
						ComputingPartnerTranslationHelper helper = new ComputingPartnerTranslationHelper(Tenant);
						var MyCode = helper.GetLogitudeCodeTranslation(MyEntity.PartnerCode,ComputingPartnerName,"Card");
					    if(string.IsNullOrEmpty(MyCode))
						{
						  throw new ApplicationException("Card with Partner Code " + MyEntity.PartnerCode + " doesn't match any record");
						}
						temp = query.GetSinglePMByCode(MyCode, Tenant );
						
						
					}
					
					
			  	   if(temp == null)
					{   
					    throw new ApplicationException("Card with Code " + MyEntity.Code + " doesn't exist");
					} 
				 
					
					if(string.IsNullOrEmpty(temp.Id))
					{
					   
					    if(!string.IsNullOrEmpty(MyEntity.Id))
					    {
					        throw new ApplicationException("Card with provided key doesn't exist");
						
						}
						//else
						//{
						//    temp.Id = MyEntity.Id;

						//} 

						
					}
                    
					if(!IsUpdate)
					{							
						temp.EnglishName = MyEntity.EnglishName;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.LocalName = MyEntity.LocalName;

										}  

					
					if(string.IsNullOrEmpty(temp.Code))
					{
					   
						 
						if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.Code))
						{								
							temp.Code = MyEntity.Code;
								
						
						}  

						
					}
					AddressQueryService MainAddressAddressService = new AddressQueryService(Tenant);
					if(MyEntity.MainAddress != null)
					{
						var myMainAddressPM = MainAddressAddressService.AddressDataMappingAndValidatin(MyEntity.MainAddress,Tenant,ComputingPartnerName,IsUpdate);
						
						if(myMainAddressPM != null)
						{ 

						 
							if(!IsUpdate)
							{								
								temp.MainAddressId = myMainAddressPM.Id;
						  
							}  

							
						} 

					}
			
					
                    
					if(!IsUpdate)
					{							
						temp.VatNumber = MyEntity.VatNumber;

										}  

					
					if(string.IsNullOrEmpty(temp.Code))
					{
					   
						 
						if(!IsUpdate)// && !string.IsNullOrEmpty(MyEntity.PartnerCode))
						{								
							temp.Code = MyEntity.PartnerCode;
								
						
						}  

						
					}
                    
					if(!IsUpdate)
					{							
						temp.IsDisconnectedFromGLAccount = MyEntity.IsDisconnectedFromGLAccount;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.ReceivablesAccountingCard = MyEntity.ReceivablesAccountingCard;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.PayablesAccountingCard = MyEntity.PayablesAccountingCard;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.ICAO = MyEntity.ICAO;

										}  

					
                    
					if(!IsUpdate)
					{							
						temp.AllowUnassignedEntry = MyEntity.AllowUnassignedEntry;

										}  

										   
					return temp;
		    }
            catch (Exception ex)
            {

                throw ex;
            } 
        }

 		public List<CardPM> GetAllLocalCards(int tenant)
		{
			List<CardPM> clientsPMList = query.GetAllLocalCards(tenant);
			return clientsPMList;
		}

 


						   
	}
}
