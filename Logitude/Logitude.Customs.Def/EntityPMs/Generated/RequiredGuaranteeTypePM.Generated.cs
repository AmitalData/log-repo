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
using Logitude.Customs.Def.Validators;
  
namespace Logitude.Customs.Def.EntityPMs
{
   [CustomValidation(typeof(CustomsClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class RequiredGuaranteeTypePM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string guaranteeId ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string GuaranteeId  
	   {
	    
	     get
		{
		   return guaranteeId;
		 }
		 set
		 {
		   if(guaranteeId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GuaranteeId",OldValue=guaranteeId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   guaranteeId=value;
		   }
			
		 }
	   }
	  private string guaranteeTypeCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string GuaranteeTypeCode  
	   {
	    
	     get
		{
		   return guaranteeTypeCode;
		 }
		 set
		 {
		   if(guaranteeTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GuaranteeTypeCode",OldValue=guaranteeTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   guaranteeTypeCode=value;
		   }
			
		 }
	   }
	  private decimal? guaranteeAmount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? GuaranteeAmount  
	   {
	    
	     get
		{
		   return guaranteeAmount;
		 }
		 set
		 {
		   if(guaranteeAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GuaranteeAmount",OldValue=guaranteeAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   guaranteeAmount=value;
		   }
			
		 }
	   }
	  private string guaranteeTypeName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string GuaranteeTypeName  
	   {
	    
	     get
		{
		   return guaranteeTypeName;
		 }
		 set
		 {
		   if(guaranteeTypeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GuaranteeTypeName",OldValue=guaranteeTypeName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   guaranteeTypeName=value;
		   }
			
		 }
	   }
   }
   
}
	 