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
   public partial class DBMigrationLinePM : EntityPM
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
	  private int counterKey ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int CounterKey  
	   {
	    
	     get
		{
		   return counterKey;
		 }
		 set
		 {
		   if(counterKey != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CounterKey",OldValue=counterKey,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   counterKey=value;
		   }
			
		 }
	   }
	  private string sqlScript ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string SqlScript  
	   {
	    
	     get
		{
		   return sqlScript;
		 }
		 set
		 {
		   if(sqlScript != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SqlScript",OldValue=sqlScript,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sqlScript=value;
		   }
			
		 }
	   }
	  private string approvedRemarks ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ApprovedRemarks  
	   {
	    
	     get
		{
		   return approvedRemarks;
		 }
		 set
		 {
		   if(approvedRemarks != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ApprovedRemarks",OldValue=approvedRemarks,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   approvedRemarks=value;
		   }
			
		 }
	   }
   }
   
}
	 