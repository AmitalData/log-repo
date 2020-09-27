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
   public partial class CargoTrackingIncrementalStatPM : EntityPM
   {
   	  private DateTime startDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime StartDate  
	   {
	    
	     get
		{
		   return startDate;
		 }
		 set
		 {
		   if(startDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StartDate",OldValue=startDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   startDate=value;
		   }
			
		 }
	   }
	  private DateTime endDate ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime EndDate  
	   {
	    
	     get
		{
		   return endDate;
		 }
		 set
		 {
		   if(endDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EndDate",OldValue=endDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   endDate=value;
		   }
			
		 }
	   }
	  private int shipments ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public int Shipments  
	   {
	    
	     get
		{
		   return shipments;
		 }
		 set
		 {
		   if(shipments != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Shipments",OldValue=shipments,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   shipments=value;
		   }
			
		 }
	   }
	  private int cards ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public int Cards  
	   {
	    
	     get
		{
		   return cards;
		 }
		 set
		 {
		   if(cards != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Cards",OldValue=cards,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   cards=value;
		   }
			
		 }
	   }
	  private int ports ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public int Ports  
	   {
	    
	     get
		{
		   return ports;
		 }
		 set
		 {
		   if(ports != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Ports",OldValue=ports,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   ports=value;
		   }
			
		 }
	   }
	  private int countries ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public int Countries  
	   {
	    
	     get
		{
		   return countries;
		 }
		 set
		 {
		   if(countries != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Countries",OldValue=countries,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   countries=value;
		   }
			
		 }
	   }
	  private int transportModes ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public int TransportModes  
	   {
	    
	     get
		{
		   return transportModes;
		 }
		 set
		 {
		   if(transportModes != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TransportModes",OldValue=transportModes,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   transportModes=value;
		   }
			
		 }
	   }
	  private int shipmentComputedFields ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public int ShipmentComputedFields  
	   {
	    
	     get
		{
		   return shipmentComputedFields;
		 }
		 set
		 {
		   if(shipmentComputedFields != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentComputedFields",OldValue=shipmentComputedFields,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   shipmentComputedFields=value;
		   }
			
		 }
	   }
	  private int shipmentMasterDatas ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public int ShipmentMasterDatas  
	   {
	    
	     get
		{
		   return shipmentMasterDatas;
		 }
		 set
		 {
		   if(shipmentMasterDatas != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentMasterDatas",OldValue=shipmentMasterDatas,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   shipmentMasterDatas=value;
		   }
			
		 }
	   }
	  private int id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public int Id  
	   {
	    
	     get
		{
		   return id;
		 }
		 set
		 {
		   if(id != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Id",OldValue=id,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   id=value;
		   }
			
		 }
	   }
	  private string errorLog ;
	  	  
       
	   [CustomValidation(typeof(CargoTrackingValidationClass), "ValidateClass")]
	   [DataMember]
       public string ErrorLog  
	   {
	    
	     get
		{
		   return errorLog;
		 }
		 set
		 {
		   if(errorLog != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ErrorLog",OldValue=errorLog,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   errorLog=value;
		   }
			
		 }
	   }
   }
   
}
	 