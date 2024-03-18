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
   public partial class CB_CustomsBookAdditionsDetailsHistoryPM : EntityPM
   {
   	  private int iD ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int ID  
	   {
	    
	     get
		{
		   return iD;
		 }
		 set
		 {
		   if(iD != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ID",OldValue=iD,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   iD=value;
		   }
			
		 }
	   }
	  private DateTime createDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime CreateDate  
	   {
	    
	     get
		{
		   return createDate;
		 }
		 set
		 {
		   if(createDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreateDate",OldValue=createDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   createDate=value;
		   }
			
		 }
	   }
	  private DateTime? updateDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string typeID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TypeID  
	   {
	    
	     get
		{
		   return typeID;
		 }
		 set
		 {
		   if(typeID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TypeID",OldValue=typeID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   typeID=value;
		   }
			
		 }
	   }
	  private string title ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string Title  
	   {
	    
	     get
		{
		   return title;
		 }
		 set
		 {
		   if(title != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Title",OldValue=title,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   title=value;
		   }
			
		 }
	   }
	  private DateTime? startDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? StartDate  
	   {
	    
	     get
		{
		   return startDate;
		 }
		 set
		 {
		   if(startDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StartDate",OldValue=startDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   startDate=value;
		   }
			
		 }
	   }
	  private DateTime? endDate ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? EndDate  
	   {
	    
	     get
		{
		   return endDate;
		 }
		 set
		 {
		   if(endDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EndDate",OldValue=endDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   endDate=value;
		   }
			
		 }
	   }
	  private string entityStatusID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityStatusID  
	   {
	    
	     get
		{
		   return entityStatusID;
		 }
		 set
		 {
		   if(entityStatusID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityStatusID",OldValue=entityStatusID,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityStatusID=value;
		   }
			
		 }
	   }
	  private string englishTitle ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string EnglishTitle  
	   {
	    
	     get
		{
		   return englishTitle;
		 }
		 set
		 {
		   if(englishTitle != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EnglishTitle",OldValue=englishTitle,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   englishTitle=value;
		   }
			
		 }
	   }
	  private int customsBookAdditionID ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int CustomsBookAdditionID  
	   {
	    
	     get
		{
		   return customsBookAdditionID;
		 }
		 set
		 {
		   if(customsBookAdditionID != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CustomsBookAdditionID",OldValue=customsBookAdditionID,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   customsBookAdditionID=value;
		   }
			
		 }
	   }
	  private int changeRequestTypePriority ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int ChangeRequestTypePriority  
	   {
	    
	     get
		{
		   return changeRequestTypePriority;
		 }
		 set
		 {
		   if(changeRequestTypePriority != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ChangeRequestTypePriority",OldValue=changeRequestTypePriority,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   changeRequestTypePriority=value;
		   }
			
		 }
	   }
   }
   
}
	 