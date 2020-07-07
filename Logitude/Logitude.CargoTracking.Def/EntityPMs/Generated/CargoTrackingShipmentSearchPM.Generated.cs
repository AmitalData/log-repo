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
   public partial class CargoTrackingShipmentSearchPM : EntityPM
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
	  private int tenant ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
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
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string SearchFields  
	   {
	    
	     get
		{
		   return searchFields;
		 }
		 set
		 {
		   if(searchFields != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SearchFields",OldValue=searchFields,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   searchFields=value;
		   }
			
		 }
	   }
	  private string shipmentId ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentId  
	   {
	    
	     get
		{
		   return shipmentId;
		 }
		 set
		 {
		   if(shipmentId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentId",OldValue=shipmentId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentId=value;
		   }
			
		 }
	   }
	  private DateTime shipmentDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime ShipmentDate  
	   {
	    
	     get
		{
		   return shipmentDate;
		 }
		 set
		 {
		   if(shipmentDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentDate",OldValue=shipmentDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   shipmentDate=value;
		   }
			
		 }
	   }
   }
   
}
	 