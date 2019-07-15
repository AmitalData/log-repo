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
   public partial class TariffVersionAllInChargePM : EntityPM
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
	  private int version ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int Version  
	   {
	    
	     get
		{
		   return version;
		 }
		 set
		 {
		   if(version != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Version",OldValue=version,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   version=value;
		   }
			
		 }
	   }
	  private string chargesTypeId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChargesTypeId  
	   {
	    
	     get
		{
		   return chargesTypeId;
		 }
		 set
		 {
		   if(chargesTypeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChargesTypeId",OldValue=chargesTypeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   chargesTypeId=value;
		   }
			
		 }
	   }
	  private string addedByUserId ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string AddedByUserId  
	   {
	    
	     get
		{
		   return addedByUserId;
		 }
		 set
		 {
		   if(addedByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AddedByUserId",OldValue=addedByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   addedByUserId=value;
		   }
			
		 }
	   }
	  private DateTime? addDate ;
	  	  
       
	   [CustomValidation(typeof(TariffModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? AddDate  
	   {
	    
	     get
		{
		   return addDate;
		 }
		 set
		 {
		   if(addDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AddDate",OldValue=addDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   addDate=value;
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
   }
   
}
	 