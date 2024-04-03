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
using Logitude.DashboardModule.BL.Validators;
  
namespace Logitude.DashboardModule.BL.EntityPMs
{
   [CustomValidation(typeof(DashboardModuleClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class DashboardGlobalPresetFilterPM : EntityPM
   {
   	  private string code ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Code  
	   {
	    
	     get
		{
		   return code;
		 }
		 set
		 {
		   if(code != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Code",OldValue=code,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   code=value;
		   }
			
		 }
	   }
	  private string displayName ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string DisplayName  
	   {
	    
	     get
		{
		   return displayName;
		 }
		 set
		 {
		   if(displayName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DisplayName",OldValue=displayName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   displayName=value;
		   }
			
		 }
	   }
	  private string dataTypeCode ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string DataTypeCode  
	   {
	    
	     get
		{
		   return dataTypeCode;
		 }
		 set
		 {
		   if(dataTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DataTypeCode",OldValue=dataTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dataTypeCode=value;
		   }
			
		 }
	   }
	  private bool isDisabled ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsDisabled  
	   {
	    
	     get
		{
		   return isDisabled;
		 }
		 set
		 {
		   if(isDisabled != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsDisabled",OldValue=isDisabled,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isDisabled=value;
		   }
			
		 }
	   }
	  private bool isMultiSelect ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsMultiSelect  
	   {
	    
	     get
		{
		   return isMultiSelect;
		 }
		 set
		 {
		   if(isMultiSelect != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsMultiSelect",OldValue=isMultiSelect,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isMultiSelect=value;
		   }
			
		 }
	   }
	  private string joinedTableName ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string JoinedTableName  
	   {
	    
	     get
		{
		   return joinedTableName;
		 }
		 set
		 {
		   if(joinedTableName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="JoinedTableName",OldValue=joinedTableName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   joinedTableName=value;
		   }
			
		 }
	   }
	  private int? sort ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int? Sort  
	   {
	    
	     get
		{
		   return sort;
		 }
		 set
		 {
		   if(sort != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Sort",OldValue=sort,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   sort=value;
		   }
			
		 }
	   }
	  private string joinedTableDisplayField ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string JoinedTableDisplayField  
	   {
	    
	     get
		{
		   return joinedTableDisplayField;
		 }
		 set
		 {
		   if(joinedTableDisplayField != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="JoinedTableDisplayField",OldValue=joinedTableDisplayField,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   joinedTableDisplayField=value;
		   }
			
		 }
	   }
	  private bool canSearch ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CanSearch  
	   {
	    
	     get
		{
		   return canSearch;
		 }
		 set
		 {
		   if(canSearch != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CanSearch",OldValue=canSearch,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   canSearch=value;
		   }
			
		 }
	   }
	    }
   
}
	 