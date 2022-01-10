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
   public partial class ContainerizationPM : EntityPM
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
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private bool agentDeclaration ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool AgentDeclaration  
	   {
	    
	     get
		{
		   return agentDeclaration;
		 }
		 set
		 {
		   if(agentDeclaration != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AgentDeclaration",OldValue=agentDeclaration,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   agentDeclaration=value;
		   }
			
		 }
	   }
	  private DateTime containerizationDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime ContainerizationDate  
	   {
	    
	     get
		{
		   return containerizationDate;
		 }
		 set
		 {
		   if(containerizationDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerizationDate",OldValue=containerizationDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   containerizationDate=value;
		   }
			
		 }
	   }
	  private string containerizationNumber ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerizationNumber  
	   {
	    
	     get
		{
		   return containerizationNumber;
		 }
		 set
		 {
		   if(containerizationNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerizationNumber",OldValue=containerizationNumber,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerizationNumber=value;
		   }
			
		 }
	   }
	  private string containerizationStatus ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerizationStatus  
	   {
	    
	     get
		{
		   return containerizationStatus;
		 }
		 set
		 {
		   if(containerizationStatus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerizationStatus",OldValue=containerizationStatus,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerizationStatus=value;
		   }
			
		 }
	   }
	  private string hataraStatus ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string HataraStatus  
	   {
	    
	     get
		{
		   return hataraStatus;
		 }
		 set
		 {
		   if(hataraStatus != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HataraStatus",OldValue=hataraStatus,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   hataraStatus=value;
		   }
			
		 }
	   }
	  private string operationMode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string OperationMode  
	   {
	    
	     get
		{
		   return operationMode;
		 }
		 set
		 {
		   if(operationMode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="OperationMode",OldValue=operationMode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   operationMode=value;
		   }
			
		 }
	   }
	  private string exportFile ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ExportFile  
	   {
	    
	     get
		{
		   return exportFile;
		 }
		 set
		 {
		   if(exportFile != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ExportFile",OldValue=exportFile,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   exportFile=value;
		   }
			
		 }
	   }
	  private string importerName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ImporterName  
	   {
	    
	     get
		{
		   return importerName;
		 }
		 set
		 {
		   if(importerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ImporterName",OldValue=importerName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   importerName=value;
		   }
			
		 }
	   }
	  private string containerizationStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainerizationStatusName  
	   {
	    
	     get
		{
		   return containerizationStatusName;
		 }
		 set
		 {
		   if(containerizationStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainerizationStatusName",OldValue=containerizationStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containerizationStatusName=value;
		   }
			
		 }
	   }
	  private string hataraStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string HataraStatusName  
	   {
	    
	     get
		{
		   return hataraStatusName;
		 }
		 set
		 {
		   if(hataraStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HataraStatusName",OldValue=hataraStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   hataraStatusName=value;
		   }
			
		 }
	   }
	  private string connectedDeclarations ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ConnectedDeclarations  
	   {
	    
	     get
		{
		   return connectedDeclarations;
		 }
		 set
		 {
		   if(connectedDeclarations != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ConnectedDeclarations",OldValue=connectedDeclarations,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   connectedDeclarations=value;
		   }
			
		 }
	   }
	  private string notConnectedDeclarations ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string NotConnectedDeclarations  
	   {
	    
	     get
		{
		   return notConnectedDeclarations;
		 }
		 set
		 {
		   if(notConnectedDeclarations != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="NotConnectedDeclarations",OldValue=notConnectedDeclarations,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   notConnectedDeclarations=value;
		   }
			
		 }
	   }
	  private bool isChange ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsChange  
	   {
	    
	     get
		{
		   return isChange;
		 }
		 set
		 {
		   if(isChange != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsChange",OldValue=isChange,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isChange=value;
		   }
			
		 }
	   }
	  private string isMultiCustomers ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string IsMultiCustomers  
	   {
	    
	     get
		{
		   return isMultiCustomers;
		 }
		 set
		 {
		   if(isMultiCustomers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsMultiCustomers",OldValue=isMultiCustomers,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   isMultiCustomers=value;
		   }
			
		 }
	   }
   }
   
}
	 