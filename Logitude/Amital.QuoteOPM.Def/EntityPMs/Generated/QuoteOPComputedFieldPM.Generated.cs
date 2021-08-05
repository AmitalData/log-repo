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
using Amital.QuoteOPM.Def.Validators;
  
namespace Amital.QuoteOPM.Def.EntityPMs
{
   [CustomValidation(typeof(QuoteOPMClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class QuoteOPComputedFieldPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private bool connectedToShipment ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ConnectedToShipment  
	   {
	    
	     get
		{
		   return connectedToShipment;
		 }
		 set
		 {
		   if(connectedToShipment != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConnectedToShipment",OldValue=connectedToShipment,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   connectedToShipment=value;
		   }
			
		 }
	   }
	  private bool connectedToTicket ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ConnectedToTicket  
	   {
	    
	     get
		{
		   return connectedToTicket;
		 }
		 set
		 {
		   if(connectedToTicket != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConnectedToTicket",OldValue=connectedToTicket,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   connectedToTicket=value;
		   }
			
		 }
	   }
	  private string toLocation ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ToLocation  
	   {
	    
	     get
		{
		   return toLocation;
		 }
		 set
		 {
		   if(toLocation != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToLocation",OldValue=toLocation,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   toLocation=value;
		   }
			
		 }
	   }
	  private string fromLocation ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string FromLocation  
	   {
	    
	     get
		{
		   return fromLocation;
		 }
		 set
		 {
		   if(fromLocation != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromLocation",OldValue=fromLocation,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fromLocation=value;
		   }
			
		 }
	   }
	  private string deliveryTo ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeliveryTo  
	   {
	    
	     get
		{
		   return deliveryTo;
		 }
		 set
		 {
		   if(deliveryTo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeliveryTo",OldValue=deliveryTo,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   deliveryTo=value;
		   }
			
		 }
	   }
	  private string pickupFrom ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PickupFrom  
	   {
	    
	     get
		{
		   return pickupFrom;
		 }
		 set
		 {
		   if(pickupFrom != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PickupFrom",OldValue=pickupFrom,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pickupFrom=value;
		   }
			
		 }
	   }
	  private int tenant ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private double? estimatedPayablesInSales ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? EstimatedPayablesInSales  
	   {
	    
	     get
		{
		   return estimatedPayablesInSales;
		 }
		 set
		 {
		   if(estimatedPayablesInSales != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EstimatedPayablesInSales",OldValue=estimatedPayablesInSales,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   estimatedPayablesInSales=value;
		   }
			
		 }
	   }
	  private double? estimatedPayablesInLocal ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? EstimatedPayablesInLocal  
	   {
	    
	     get
		{
		   return estimatedPayablesInLocal;
		 }
		 set
		 {
		   if(estimatedPayablesInLocal != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EstimatedPayablesInLocal",OldValue=estimatedPayablesInLocal,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   estimatedPayablesInLocal=value;
		   }
			
		 }
	   }
	  private double? estimatedReceivablesInLocal ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? EstimatedReceivablesInLocal  
	   {
	    
	     get
		{
		   return estimatedReceivablesInLocal;
		 }
		 set
		 {
		   if(estimatedReceivablesInLocal != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EstimatedReceivablesInLocal",OldValue=estimatedReceivablesInLocal,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   estimatedReceivablesInLocal=value;
		   }
			
		 }
	   }
	  private double? estimatedReceivablesInSales ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double? EstimatedReceivablesInSales  
	   {
	    
	     get
		{
		   return estimatedReceivablesInSales;
		 }
		 set
		 {
		   if(estimatedReceivablesInSales != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EstimatedReceivablesInSales",OldValue=estimatedReceivablesInSales,NewValue=value,PropertyType="double?"};
		    NotifyPropertyChanged(values);
		   estimatedReceivablesInSales=value;
		   }
			
		 }
	   }
   }
   
}
	 