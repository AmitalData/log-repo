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
   public partial class WidgetPM : EntityPM
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
	  private string title ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
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
	  private string groupById ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string GroupById  
	   {
	    
	     get
		{
		   return groupById;
		 }
		 set
		 {
		   if(groupById != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GroupById",OldValue=groupById,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   groupById=value;
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
	  private string startPotistion ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string StartPotistion  
	   {
	    
	     get
		{
		   return startPotistion;
		 }
		 set
		 {
		   if(startPotistion != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="StartPotistion",OldValue=startPotistion,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   startPotistion=value;
		   }
			
		 }
	   }
	  private string endPosition ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string EndPosition  
	   {
	    
	     get
		{
		   return endPosition;
		 }
		 set
		 {
		   if(endPosition != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EndPosition",OldValue=endPosition,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   endPosition=value;
		   }
			
		 }
	   }
	  private string typeCode ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string TypeCode  
	   {
	    
	     get
		{
		   return typeCode;
		 }
		 set
		 {
		   if(typeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TypeCode",OldValue=typeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   typeCode=value;
		   }
			
		 }
	   }
	  private string entityId ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string EntityId  
	   {
	    
	     get
		{
		   return entityId;
		 }
		 set
		 {
		   if(entityId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="EntityId",OldValue=entityId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   entityId=value;
		   }
			
		 }
	   }

	   private List<WidgetMeasurePM> widgetMeasures;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("WidgetWidgetMeasure", "Id","WidgetId")]
	   [DataMember]
	   public virtual List<WidgetMeasurePM> WidgetMeasures  
	   {
	        get
             {
                 if (widgetMeasures == null)
                 {
                     widgetMeasures = new List<WidgetMeasurePM>();
                 }
                 return widgetMeasures;
              }
             set { widgetMeasures = value; }
	    }
		   
	   private List<WidgetMeasurePM>  deletedWidgetMeasures;
	   public virtual List<WidgetMeasurePM> DeletedWidgetMeasures  
	   {
	        get
             {
                 if ( deletedWidgetMeasures == null)
                 {
                      deletedWidgetMeasures = new List<WidgetMeasurePM>();
                 }
                 return  deletedWidgetMeasures;
              }
             set {  deletedWidgetMeasures = value; }
	    }
	  	  private string filters ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Filters  
	   {
	    
	     get
		{
		   return filters;
		 }
		 set
		 {
		   if(filters != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Filters",OldValue=filters,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   filters=value;
		   }
			
		 }
	   }
	  private string dateGroupCode ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string DateGroupCode  
	   {
	    
	     get
		{
		   return dateGroupCode;
		 }
		 set
		 {
		   if(dateGroupCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DateGroupCode",OldValue=dateGroupCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dateGroupCode=value;
		   }
			
		 }
	   }
	  private int? maximumGrouping ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int? MaximumGrouping  
	   {
	    
	     get
		{
		   return maximumGrouping;
		 }
		 set
		 {
		   if(maximumGrouping != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MaximumGrouping",OldValue=maximumGrouping,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   maximumGrouping=value;
		   }
			
		 }
	   }
	  private int? sortBy ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int? SortBy  
	   {
	    
	     get
		{
		   return sortBy;
		 }
		 set
		 {
		   if(sortBy != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SortBy",OldValue=sortBy,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   sortBy=value;
		   }
			
		 }
	   }
	  private string sortDirection ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string SortDirection  
	   {
	    
	     get
		{
		   return sortDirection;
		 }
		 set
		 {
		   if(sortDirection != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SortDirection",OldValue=sortDirection,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   sortDirection=value;
		   }
			
		 }
	   }
	  private string key ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Key  
	   {
	    
	     get
		{
		   return key;
		 }
		 set
		 {
		   if(key != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Key",OldValue=key,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   key=value;
		   }
			
		 }
	   }
	  private bool timeOverTime ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool TimeOverTime  
	   {
	    
	     get
		{
		   return timeOverTime;
		 }
		 set
		 {
		   if(timeOverTime != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TimeOverTime",OldValue=timeOverTime,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   timeOverTime=value;
		   }
			
		 }
	   }
	  private int? comparisonPeriod ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int? ComparisonPeriod  
	   {
	    
	     get
		{
		   return comparisonPeriod;
		 }
		 set
		 {
		   if(comparisonPeriod != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ComparisonPeriod",OldValue=comparisonPeriod,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   comparisonPeriod=value;
		   }
			
		 }
	   }
	  private string increase ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Increase  
	   {
	    
	     get
		{
		   return increase;
		 }
		 set
		 {
		   if(increase != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Increase",OldValue=increase,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   increase=value;
		   }
			
		 }
	   }
	  private string comparisonOperator ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string ComparisonOperator  
	   {
	    
	     get
		{
		   return comparisonOperator;
		 }
		 set
		 {
		   if(comparisonOperator != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ComparisonOperator",OldValue=comparisonOperator,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   comparisonOperator=value;
		   }
			
		 }
	   }
	  private string comparisonDateGroup ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string ComparisonDateGroup  
	   {
	    
	     get
		{
		   return comparisonDateGroup;
		 }
		 set
		 {
		   if(comparisonDateGroup != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ComparisonDateGroup",OldValue=comparisonDateGroup,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   comparisonDateGroup=value;
		   }
			
		 }
	   }
	  private DateTime? fromDate ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FromDate  
	   {
	    
	     get
		{
		   return fromDate;
		 }
		 set
		 {
		   if(fromDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FromDate",OldValue=fromDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fromDate=value;
		   }
			
		 }
	   }
	  private DateTime? toDate ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? ToDate  
	   {
	    
	     get
		{
		   return toDate;
		 }
		 set
		 {
		   if(toDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ToDate",OldValue=toDate,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   toDate=value;
		   }
			
		 }
	   }
	  private string globalFilters ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string GlobalFilters  
	   {
	    
	     get
		{
		   return globalFilters;
		 }
		 set
		 {
		   if(globalFilters != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GlobalFilters",OldValue=globalFilters,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   globalFilters=value;
		   }
			
		 }
	   }
	  private string secondaryGroupById ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string SecondaryGroupById  
	   {
	    
	     get
		{
		   return secondaryGroupById;
		 }
		 set
		 {
		   if(secondaryGroupById != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SecondaryGroupById",OldValue=secondaryGroupById,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   secondaryGroupById=value;
		   }
			
		 }
	   }
	  private string secondaryDateGroupCode ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string SecondaryDateGroupCode  
	   {
	    
	     get
		{
		   return secondaryDateGroupCode;
		 }
		 set
		 {
		   if(secondaryDateGroupCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SecondaryDateGroupCode",OldValue=secondaryDateGroupCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   secondaryDateGroupCode=value;
		   }
			
		 }
	   }
	  private string alignment ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Alignment  
	   {
	    
	     get
		{
		   return alignment;
		 }
		 set
		 {
		   if(alignment != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Alignment",OldValue=alignment,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   alignment=value;
		   }
			
		 }
	   }
	  private bool? thousandSeparator ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? ThousandSeparator  
	   {
	    
	     get
		{
		   return thousandSeparator;
		 }
		 set
		 {
		   if(thousandSeparator != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ThousandSeparator",OldValue=thousandSeparator,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   thousandSeparator=value;
		   }
			
		 }
	   }
	  private bool? useNumberAbbreviation ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool? UseNumberAbbreviation  
	   {
	    
	     get
		{
		   return useNumberAbbreviation;
		 }
		 set
		 {
		   if(useNumberAbbreviation != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UseNumberAbbreviation",OldValue=useNumberAbbreviation,NewValue=value,PropertyType="bool?"};
		    NotifyPropertyChanged(values);
		   useNumberAbbreviation=value;
		   }
			
		 }
	   }
	  private int? decimalPlaces ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int? DecimalPlaces  
	   {
	    
	     get
		{
		   return decimalPlaces;
		 }
		 set
		 {
		   if(decimalPlaces != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DecimalPlaces",OldValue=decimalPlaces,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   decimalPlaces=value;
		   }
			
		 }
	   }
	  private string useAbbreviationAfter ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string UseAbbreviationAfter  
	   {
	    
	     get
		{
		   return useAbbreviationAfter;
		 }
		 set
		 {
		   if(useAbbreviationAfter != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UseAbbreviationAfter",OldValue=useAbbreviationAfter,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   useAbbreviationAfter=value;
		   }
			
		 }
	   }
	  private string labelsPosition ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string LabelsPosition  
	   {
	    
	     get
		{
		   return labelsPosition;
		 }
		 set
		 {
		   if(labelsPosition != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LabelsPosition",OldValue=labelsPosition,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   labelsPosition=value;
		   }
			
		 }
	   }
	    }
   
}
	 