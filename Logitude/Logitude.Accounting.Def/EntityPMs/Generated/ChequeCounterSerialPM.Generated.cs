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
   public partial class ChequeCounterSerialPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
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
	  private string bankId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public string BankId  
	   {
	    
	     get
		{
		   return bankId;
		 }
		 set
		 {
		   if(bankId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BankId",OldValue=bankId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   bankId=value;
		   }
			
		 }
	   }
	  private int seriesId ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int SeriesId  
	   {
	    
	     get
		{
		   return seriesId;
		 }
		 set
		 {
		   if(seriesId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SeriesId",OldValue=seriesId,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   seriesId=value;
		   }
			
		 }
	   }
	  private int chequeCounterBegin ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int ChequeCounterBegin  
	   {
	    
	     get
		{
		   return chequeCounterBegin;
		 }
		 set
		 {
		   if(chequeCounterBegin != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChequeCounterBegin",OldValue=chequeCounterBegin,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   chequeCounterBegin=value;
		   }
			
		 }
	   }
	  private int chequeCounterEnd ;
	  	  
       
	   [CustomValidation(typeof(AccountingValidationClass), "ValidateClass")]
	   [DataMember]
       public int ChequeCounterEnd  
	   {
	    
	     get
		{
		   return chequeCounterEnd;
		 }
		 set
		 {
		   if(chequeCounterEnd != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChequeCounterEnd",OldValue=chequeCounterEnd,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   chequeCounterEnd=value;
		   }
			
		 }
	   }
	    }
   
}
	 