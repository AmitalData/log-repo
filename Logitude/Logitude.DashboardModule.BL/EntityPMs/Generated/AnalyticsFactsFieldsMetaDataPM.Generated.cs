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
   public partial class AnalyticsFactsFieldsMetaDataPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
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
	  private string analyticsFactsMetaDataId ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string AnalyticsFactsMetaDataId  
	   {
	    
	     get
		{
		   return analyticsFactsMetaDataId;
		 }
		 set
		 {
		   if(analyticsFactsMetaDataId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AnalyticsFactsMetaDataId",OldValue=analyticsFactsMetaDataId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   analyticsFactsMetaDataId=value;
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
	  private bool canMeasure ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CanMeasure  
	   {
	    
	     get
		{
		   return canMeasure;
		 }
		 set
		 {
		   if(canMeasure != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CanMeasure",OldValue=canMeasure,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   canMeasure=value;
		   }
			
		 }
	   }
	  private bool canGroup ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CanGroup  
	   {
	    
	     get
		{
		   return canGroup;
		 }
		 set
		 {
		   if(canGroup != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CanGroup",OldValue=canGroup,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   canGroup=value;
		   }
			
		 }
	   }
	  private string fieldCode ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string FieldCode  
	   {
	    
	     get
		{
		   return fieldCode;
		 }
		 set
		 {
		   if(fieldCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldCode",OldValue=fieldCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fieldCode=value;
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
	  private string displayNamePlural ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string DisplayNamePlural  
	   {
	    
	     get
		{
		   return displayNamePlural;
		 }
		 set
		 {
		   if(displayNamePlural != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DisplayNamePlural",OldValue=displayNamePlural,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   displayNamePlural=value;
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
	  private string joinedTableKey ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string JoinedTableKey  
	   {
	    
	     get
		{
		   return joinedTableKey;
		 }
		 set
		 {
		   if(joinedTableKey != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="JoinedTableKey",OldValue=joinedTableKey,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   joinedTableKey=value;
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
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
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
	  private string joinedTableDBName ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string JoinedTableDBName  
	   {
	    
	     get
		{
		   return joinedTableDBName;
		 }
		 set
		 {
		   if(joinedTableDBName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="JoinedTableDBName",OldValue=joinedTableDBName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   joinedTableDBName=value;
		   }
			
		 }
	   }
	  private string objectTableName ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string ObjectTableName  
	   {
	    
	     get
		{
		   return objectTableName;
		 }
		 set
		 {
		   if(objectTableName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ObjectTableName",OldValue=objectTableName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   objectTableName=value;
		   }
			
		 }
	   }
	  private bool hasUnit ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool HasUnit  
	   {
	    
	     get
		{
		   return hasUnit;
		 }
		 set
		 {
		   if(hasUnit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HasUnit",OldValue=hasUnit,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   hasUnit=value;
		   }
			
		 }
	   }
	  private string unit ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Unit  
	   {
	    
	     get
		{
		   return unit;
		 }
		 set
		 {
		   if(unit != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Unit",OldValue=unit,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   unit=value;
		   }
			
		 }
	   }
	  private string commonFilterCode ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string CommonFilterCode  
	   {
	    
	     get
		{
		   return commonFilterCode;
		 }
		 set
		 {
		   if(commonFilterCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CommonFilterCode",OldValue=commonFilterCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   commonFilterCode=value;
		   }
			
		 }
	   }
	  private bool canSecondaryGroup ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool CanSecondaryGroup  
	   {
	    
	     get
		{
		   return canSecondaryGroup;
		 }
		 set
		 {
		   if(canSecondaryGroup != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CanSecondaryGroup",OldValue=canSecondaryGroup,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   canSecondaryGroup=value;
		   }
			
		 }
	   }
	  private bool allowTenantZeroFilter ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool AllowTenantZeroFilter  
	   {
	    
	     get
		{
		   return allowTenantZeroFilter;
		 }
		 set
		 {
		   if(allowTenantZeroFilter != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AllowTenantZeroFilter",OldValue=allowTenantZeroFilter,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   allowTenantZeroFilter=value;
		   }
			
		 }
	   }
	    }
   
}
	 