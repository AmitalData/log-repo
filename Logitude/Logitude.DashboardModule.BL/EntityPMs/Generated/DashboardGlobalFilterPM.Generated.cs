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
   public partial class DashboardGlobalFilterPM : EntityPM
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
	  private string dashboardId ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string DashboardId  
	   {
	    
	     get
		{
		   return dashboardId;
		 }
		 set
		 {
		   if(dashboardId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DashboardId",OldValue=dashboardId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dashboardId=value;
		   }
			
		 }
	   }
	  private bool isCommonFilter ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool IsCommonFilter  
	   {
	    
	     get
		{
		   return isCommonFilter;
		 }
		 set
		 {
		   if(isCommonFilter != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="IsCommonFilter",OldValue=isCommonFilter,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   isCommonFilter=value;
		   }
			
		 }
	   }
	  private string commonFilterField ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string CommonFilterField  
	   {
	    
	     get
		{
		   return commonFilterField;
		 }
		 set
		 {
		   if(commonFilterField != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CommonFilterField",OldValue=commonFilterField,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   commonFilterField=value;
		   }
			
		 }
	   }
	  private string dataSetId ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string DataSetId  
	   {
	    
	     get
		{
		   return dataSetId;
		 }
		 set
		 {
		   if(dataSetId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DataSetId",OldValue=dataSetId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dataSetId=value;
		   }
			
		 }
	   }
	  private string dataSetFieldId ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string DataSetFieldId  
	   {
	    
	     get
		{
		   return dataSetFieldId;
		 }
		 set
		 {
		   if(dataSetFieldId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DataSetFieldId",OldValue=dataSetFieldId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dataSetFieldId=value;
		   }
			
		 }
	   }
	  private string filterOperator ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string FilterOperator  
	   {
	    
	     get
		{
		   return filterOperator;
		 }
		 set
		 {
		   if(filterOperator != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FilterOperator",OldValue=filterOperator,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   filterOperator=value;
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
	  private int lineNumber ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
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
	    }
   
}
	 