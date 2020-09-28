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
using Logitude.CRM.BL.Validators;
  
namespace Logitude.CRM.BL.EntityPMs
{
   [CustomValidation(typeof(CRMClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class TicketEscalationPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
	  private DateTime? createDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? CreateDate  
	   {
	    
	     get
		{
		   return createDate;
		 }
		 set
		 {
		   if(createDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreateDate",OldValue=createDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   createDate=value;
		   }
			
		 }
	   }
	  private string ticketId ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TicketId  
	   {
	    
	     get
		{
		   return ticketId;
		 }
		 set
		 {
		   if(ticketId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TicketId",OldValue=ticketId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   ticketId=value;
		   }
			
		 }
	   }
	  private int lineNumber ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public int LineNumber  
	   {
	    
	     get
		{
		   return lineNumber;
		 }
		 set
		 {
		   if(lineNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LineNumber",OldValue=lineNumber,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   lineNumber=value;
		   }
			
		 }
	   }
	  private string escalationFor ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string EscalationFor  
	   {
	    
	     get
		{
		   return escalationFor;
		 }
		 set
		 {
		   if(escalationFor != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EscalationFor",OldValue=escalationFor,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   escalationFor=value;
		   }
			
		 }
	   }
	  private string recepients ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string Recepients  
	   {
	    
	     get
		{
		   return recepients;
		 }
		 set
		 {
		   if(recepients != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Recepients",OldValue=recepients,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   recepients=value;
		   }
			
		 }
	   }
	  private bool isClose ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsClose  
	   {
	    
	     get
		{
		   return isClose;
		 }
		 set
		 {
		   if(isClose != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsClose",OldValue=isClose,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isClose=value;
		   }
			
		 }
	   }
	  private bool isSLAViolated ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsSLAViolated  
	   {
	    
	     get
		{
		   return isSLAViolated;
		 }
		 set
		 {
		   if(isSLAViolated != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsSLAViolated",OldValue=isSLAViolated,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isSLAViolated=value;
		   }
			
		 }
	   }
	  private DateTime? dueDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? DueDate  
	   {
	    
	     get
		{
		   return dueDate;
		 }
		 set
		 {
		   if(dueDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DueDate",OldValue=dueDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   dueDate=value;
		   }
			
		 }
	   }
	  private DateTime? closeDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? CloseDate  
	   {
	    
	     get
		{
		   return closeDate;
		 }
		 set
		 {
		   if(closeDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CloseDate",OldValue=closeDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   closeDate=value;
		   }
			
		 }
	   }
	  private DateTime? updateDate ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? UpdateDate  
	   {
	    
	     get
		{
		   return updateDate;
		 }
		 set
		 {
		   if(updateDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateDate",OldValue=updateDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   updateDate=value;
		   }
			
		 }
	   }
	  private string escalationForName ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
	   [DataMember]
       public string EscalationForName  
	   {
	    
	     get
		{
		   return escalationForName;
		 }
		 set
		 {
		   if(escalationForName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EscalationForName",OldValue=escalationForName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   escalationForName=value;
		   }
			
		 }
	   }
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(CRMValidationClass), "ValidateClass")]
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
   }
   
}
	 