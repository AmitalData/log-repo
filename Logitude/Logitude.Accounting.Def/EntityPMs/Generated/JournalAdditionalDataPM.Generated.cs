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
using Logitude.Accounting.Def.Validators;
  
namespace Logitude.Accounting.Def.EntityPMs
{
   [CustomValidation(typeof(AccountingClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class JournalAdditionalDataPM : EntityPM
   {
   	  private int tenant ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  private string journalId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string JournalId  
	   {
	    
	     get
		{
		   return journalId;
		 }
		 set
		 {
		   if(journalId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="JournalId",OldValue=journalId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   journalId=value;
		   }
			
		 }
	   }
	  private string taxReportId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string TaxReportId  
	   {
	    
	     get
		{
		   return taxReportId;
		 }
		 set
		 {
		   if(taxReportId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaxReportId",OldValue=taxReportId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   taxReportId=value;
		   }
			
		 }
	   }
	  private string taxReportStatusCode ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string TaxReportStatusCode  
	   {
	    
	     get
		{
		   return taxReportStatusCode;
		 }
		 set
		 {
		   if(taxReportStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TaxReportStatusCode",OldValue=taxReportStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   taxReportStatusCode=value;
		   }
			
		 }
	   }
   }
   
}
	 