using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.DomainServices.Server; 
using Logitude.Server.Tools; 
using System.Runtime.Serialization;
using Simplog.Server.Infrastructure.DataContracts; 
using Logitude.TariffModule.BL.Validators;
  
namespace Logitude.TariffModule.BL.EntityPMs
{
   [CustomValidation(typeof(TariffModuleClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class TariffLinesContainersPricePM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Id  
	   {
	    
	     get
		{
		   return id;
		 }
		 set
		 {
		   if(id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Id",OldValue=id,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   id=value;
		   }
			
		 }
	   }
	  private int tenant ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int Tenant  
	   {
	    
	     get
		{
		   return tenant;
		 }
		 set
		 {
		   if(tenant != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Tenant",OldValue=tenant,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   tenant=value;
		   }
			
		 }
	   }
	  private string tariffId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string TariffId  
	   {
	    
	     get
		{
		   return tariffId;
		 }
		 set
		 {
		   if(tariffId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TariffId",OldValue=tariffId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tariffId=value;
		   }
			
		 }
	   }
	  private string tariffLineId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string TariffLineId  
	   {
	    
	     get
		{
		   return tariffLineId;
		 }
		 set
		 {
		   if(tariffLineId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TariffLineId",OldValue=tariffLineId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tariffLineId=value;
		   }
			
		 }
	   }
	  private string surchargeId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string SurchargeId  
	   {
	    
	     get
		{
		   return surchargeId;
		 }
		 set
		 {
		   if(surchargeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SurchargeId",OldValue=surchargeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   surchargeId=value;
		   }
			
		 }
	   }
	  private decimal? price1 ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Price1  
	   {
	    
	     get
		{
		   return price1;
		 }
		 set
		 {
		   if(price1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Price1",OldValue=price1,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   price1=value;
		   }
			
		 }
	   }
	  private decimal? price2 ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Price2  
	   {
	    
	     get
		{
		   return price2;
		 }
		 set
		 {
		   if(price2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Price2",OldValue=price2,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   price2=value;
		   }
			
		 }
	   }
	  private decimal? price3 ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Price3  
	   {
	    
	     get
		{
		   return price3;
		 }
		 set
		 {
		   if(price3 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Price3",OldValue=price3,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   price3=value;
		   }
			
		 }
	   }
	  private decimal? price4 ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Price4  
	   {
	    
	     get
		{
		   return price4;
		 }
		 set
		 {
		   if(price4 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Price4",OldValue=price4,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   price4=value;
		   }
			
		 }
	   }
	  private decimal? price5 ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? Price5  
	   {
	    
	     get
		{
		   return price5;
		 }
		 set
		 {
		   if(price5 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Price5",OldValue=price5,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   price5=value;
		   }
			
		 }
	   }
	  private decimal? costPrice ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? CostPrice  
	   {
	    
	     get
		{
		   return costPrice;
		 }
		 set
		 {
		   if(costPrice != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CostPrice",OldValue=costPrice,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   costPrice=value;
		   }
			
		 }
	   }
   }
   
}
	 