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
using Logitude.Infrastructure.BL.Validators;
  
namespace Logitude.Infrastructure.BL.EntityPMs
{
   [CustomValidation(typeof(InfrastructureClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class ContainerSettingPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
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
	  private int? emptyReturnClosingDays ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public int? EmptyReturnClosingDays  
	   {
	    
	     get
		{
		   return emptyReturnClosingDays;
		 }
		 set
		 {
		   if(emptyReturnClosingDays != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EmptyReturnClosingDays",OldValue=emptyReturnClosingDays,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   emptyReturnClosingDays=value;
		   }
			
		 }
	   }
	  private int? shipmentATAClosingDays ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public int? ShipmentATAClosingDays  
	   {
	    
	     get
		{
		   return shipmentATAClosingDays;
		 }
		 set
		 {
		   if(shipmentATAClosingDays != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentATAClosingDays",OldValue=shipmentATAClosingDays,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   shipmentATAClosingDays=value;
		   }
			
		 }
	   }
	  private string shipmentATADateIndicator ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string ShipmentATADateIndicator  
	   {
	    
	     get
		{
		   return shipmentATADateIndicator;
		 }
		 set
		 {
		   if(shipmentATADateIndicator != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShipmentATADateIndicator",OldValue=shipmentATADateIndicator,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   shipmentATADateIndicator=value;
		   }
			
		 }
	   }
	  private bool isExport ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsExport  
	   {
	    
	     get
		{
		   return isExport;
		 }
		 set
		 {
		   if(isExport != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsExport",OldValue=isExport,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isExport=value;
		   }
			
		 }
	   }
	  private bool isDomestic ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsDomestic  
	   {
	    
	     get
		{
		   return isDomestic;
		 }
		 set
		 {
		   if(isDomestic != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsDomestic",OldValue=isDomestic,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isDomestic=value;
		   }
			
		 }
	   }
	  private bool isImport ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsImport  
	   {
	    
	     get
		{
		   return isImport;
		 }
		 set
		 {
		   if(isImport != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsImport",OldValue=isImport,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isImport=value;
		   }
			
		 }
	   }
	  private bool isDrop ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsDrop  
	   {
	    
	     get
		{
		   return isDrop;
		 }
		 set
		 {
		   if(isDrop != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsDrop",OldValue=isDrop,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isDrop=value;
		   }
			
		 }
	   }
	  private bool addedManually ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public bool AddedManually  
	   {
	    
	     get
		{
		   return addedManually;
		 }
		 set
		 {
		   if(addedManually != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AddedManually",OldValue=addedManually,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   addedManually=value;
		   }
			
		 }
	   }
	  private DateTime? activationDate ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ActivationDate  
	   {
	    
	     get
		{
		   return activationDate;
		 }
		 set
		 {
		   if(activationDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ActivationDate",OldValue=activationDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   activationDate=value;
		   }
			
		 }
	   }
	    }
   
}
	 