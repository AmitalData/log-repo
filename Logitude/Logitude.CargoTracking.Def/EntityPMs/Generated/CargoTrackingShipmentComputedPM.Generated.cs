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
   public partial class CargoTrackingShipmentComputedPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private DateTime? firstPickupATD ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FirstPickupATD  
	   {
	    
	     get
		{
		   return firstPickupATD;
		 }
		 set
		 {
		   if(firstPickupATD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FirstPickupATD",OldValue=firstPickupATD,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   firstPickupATD=value;
		   }
			
		 }
	   }
	  private DateTime? finalDeliveryATA ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FinalDeliveryATA  
	   {
	    
	     get
		{
		   return finalDeliveryATA;
		 }
		 set
		 {
		   if(finalDeliveryATA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FinalDeliveryATA",OldValue=finalDeliveryATA,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   finalDeliveryATA=value;
		   }
			
		 }
	   }
	  private DateTime? finalDeliveryETA ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FinalDeliveryETA  
	   {
	    
	     get
		{
		   return finalDeliveryETA;
		 }
		 set
		 {
		   if(finalDeliveryETA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FinalDeliveryETA",OldValue=finalDeliveryETA,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   finalDeliveryETA=value;
		   }
			
		 }
	   }
   }
   
}
	 