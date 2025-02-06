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
   public partial class CertificateOfOriginConnectionPM : EntityPM
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
	  private string cooStatus ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CooStatus  
	   {
	    
	     get
		{
		   return cooStatus;
		 }
		 set
		 {
		   if(cooStatus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CooStatus",OldValue=cooStatus,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cooStatus=value;
		   }
			
		 }
	   }
	  private string cooReason ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CooReason  
	   {
	    
	     get
		{
		   return cooReason;
		 }
		 set
		 {
		   if(cooReason != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CooReason",OldValue=cooReason,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   cooReason=value;
		   }
			
		 }
	   }
	  private bool active ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool Active  
	   {
	    
	     get
		{
		   return active;
		 }
		 set
		 {
		   if(active != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Active",OldValue=active,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   active=value;
		   }
			
		 }
	   }
	    }
   
}
	 