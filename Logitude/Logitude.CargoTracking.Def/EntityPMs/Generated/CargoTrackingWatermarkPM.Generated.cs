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
using Logitude.CargoTracking.Def.Validators;
  
namespace Logitude.CargoTracking.Def.EntityPMs
{
   [CustomValidation(typeof(CargoTrackingClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class CargoTrackingWatermarkPM : EntityPM
   {
   	  private string tableName ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string TableName  
	   {
	    
	     get
		{
		   return tableName;
		 }
		 set
		 {
		   if(tableName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TableName",OldValue=tableName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tableName=value;
		   }
			
		 }
	   }
	  private DateTime? lastUpdateDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LastUpdateDate  
	   {
	    
	     get
		{
		   return lastUpdateDate;
		 }
		 set
		 {
		   if(lastUpdateDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastUpdateDate",OldValue=lastUpdateDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   lastUpdateDate=value;
		   }
			
		 }
	   }
	  private DateTime? lastRun ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? LastRun  
	   {
	    
	     get
		{
		   return lastRun;
		 }
		 set
		 {
		   if(lastRun != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LastRun",OldValue=lastRun,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   lastRun=value;
		   }
			
		 }
	   }
   }
   
}
	 