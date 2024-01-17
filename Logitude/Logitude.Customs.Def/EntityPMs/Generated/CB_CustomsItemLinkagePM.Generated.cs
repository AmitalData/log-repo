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
   public partial class CB_CustomsItemLinkagePM : EntityPM
   {
   	  private string iD ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ID  
	   {
	    
	     get
		{
		   return iD;
		 }
		 set
		 {
		   if(iD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ID",OldValue=iD,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   iD=value;
		   }
			
		 }
	   }
	  private string changeTypeID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ChangeTypeID  
	   {
	    
	     get
		{
		   return changeTypeID;
		 }
		 set
		 {
		   if(changeTypeID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChangeTypeID",OldValue=changeTypeID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   changeTypeID=value;
		   }
			
		 }
	   }
	  private DateTime changeDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime ChangeDate  
	   {
	    
	     get
		{
		   return changeDate;
		 }
		 set
		 {
		   if(changeDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChangeDate",OldValue=changeDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   changeDate=value;
		   }
			
		 }
	   }
	  private string customsItemDetailsHistoryID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string CustomsItemDetailsHistoryID  
	   {
	    
	     get
		{
		   return customsItemDetailsHistoryID;
		 }
		 set
		 {
		   if(customsItemDetailsHistoryID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsItemDetailsHistoryID",OldValue=customsItemDetailsHistoryID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   customsItemDetailsHistoryID=value;
		   }
			
		 }
	   }
	  private string connect_CustItemDetailsHistID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Connect_CustItemDetailsHistID  
	   {
	    
	     get
		{
		   return connect_CustItemDetailsHistID;
		 }
		 set
		 {
		   if(connect_CustItemDetailsHistID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Connect_CustItemDetailsHistID",OldValue=connect_CustItemDetailsHistID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   connect_CustItemDetailsHistID=value;
		   }
			
		 }
	   }
   }
   
}
	 