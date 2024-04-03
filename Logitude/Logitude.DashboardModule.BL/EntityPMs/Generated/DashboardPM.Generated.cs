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
   public partial class DashboardPM : EntityPM
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
	  private DateTime createDate ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
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
	  private string createdByUserId ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string CreatedByUserId  
	   {
	    
	     get
		{
		   return createdByUserId;
		 }
		 set
		 {
		   if(createdByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="CreatedByUserId",OldValue=createdByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   createdByUserId=value;
		   }
			
		 }
	   }
	  private DateTime updateDate ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime UpdateDate  
	   {
	    
	     get
		{
		   return updateDate;
		 }
		 set
		 {
		   if(updateDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdateDate",OldValue=updateDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   updateDate=value;
		   }
			
		 }
	   }
	  private string updatedByUserId ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string UpdatedByUserId  
	   {
	    
	     get
		{
		   return updatedByUserId;
		 }
		 set
		 {
		   if(updatedByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="UpdatedByUserId",OldValue=updatedByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   updatedByUserId=value;
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
	  private string name ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Name  
	   {
	    
	     get
		{
		   return name;
		 }
		 set
		 {
		   if(name != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Name",OldValue=name,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   name=value;
		   }
			
		 }
	   }
	  private string description ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string Description  
	   {
	    
	     get
		{
		   return description;
		 }
		 set
		 {
		   if(description != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Description",OldValue=description,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   description=value;
		   }
			
		 }
	   }

	   private List<WidgetPM> widgets;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("DashboardWidgets", "Id","DashboardId")]
	   [DataMember]
	   public virtual List<WidgetPM> Widgets  
	   {
	        get
             {
                 if (widgets == null)
                 {
                     widgets = new List<WidgetPM>();
                 }
                 return widgets;
              }
             set { widgets = value; }
	    }
		   
	   private List<WidgetPM>  deletedWidgets;
	   public virtual List<WidgetPM> DeletedWidgets  
	   {
	        get
             {
                 if ( deletedWidgets == null)
                 {
                      deletedWidgets = new List<WidgetPM>();
                 }
                 return  deletedWidgets;
              }
             set {  deletedWidgets = value; }
	    }
	  	  private string permissionLevelCode ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public string PermissionLevelCode  
	   {
	    
	     get
		{
		   return permissionLevelCode;
		 }
		 set
		 {
		   if(permissionLevelCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PermissionLevelCode",OldValue=permissionLevelCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   permissionLevelCode=value;
		   }
			
		 }
	   }

	   private List<DashboardSharedUserPM> dashboardSharedUsers;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("DashboardSharedUsers", "Id","DashboardId")]
	   [DataMember]
	   public virtual List<DashboardSharedUserPM> DashboardSharedUsers  
	   {
	        get
             {
                 if (dashboardSharedUsers == null)
                 {
                     dashboardSharedUsers = new List<DashboardSharedUserPM>();
                 }
                 return dashboardSharedUsers;
              }
             set { dashboardSharedUsers = value; }
	    }
		   
	   private List<DashboardSharedUserPM>  deletedDashboardSharedUsers;
	   public virtual List<DashboardSharedUserPM> DeletedDashboardSharedUsers  
	   {
	        get
             {
                 if ( deletedDashboardSharedUsers == null)
                 {
                      deletedDashboardSharedUsers = new List<DashboardSharedUserPM>();
                 }
                 return  deletedDashboardSharedUsers;
              }
             set {  deletedDashboardSharedUsers = value; }
	    }
	  
	   private List<DashboardGlobalFilterPM> dashboardGlobalFilters;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("DashboardGlobalFilters", "Id","DashboardId")]
	   [DataMember]
	   public virtual List<DashboardGlobalFilterPM> DashboardGlobalFilters  
	   {
	        get
             {
                 if (dashboardGlobalFilters == null)
                 {
                     dashboardGlobalFilters = new List<DashboardGlobalFilterPM>();
                 }
                 return dashboardGlobalFilters;
              }
             set { dashboardGlobalFilters = value; }
	    }
		   
	   private List<DashboardGlobalFilterPM>  deletedDashboardGlobalFilters;
	   public virtual List<DashboardGlobalFilterPM> DeletedDashboardGlobalFilters  
	   {
	        get
             {
                 if ( deletedDashboardGlobalFilters == null)
                 {
                      deletedDashboardGlobalFilters = new List<DashboardGlobalFilterPM>();
                 }
                 return  deletedDashboardGlobalFilters;
              }
             set {  deletedDashboardGlobalFilters = value; }
	    }
	  	  private bool pinnedByDefault ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public bool PinnedByDefault  
	   {
	    
	     get
		{
		   return pinnedByDefault;
		 }
		 set
		 {
		   if(pinnedByDefault != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PinnedByDefault",OldValue=pinnedByDefault,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   pinnedByDefault=value;
		   }
			
		 }
	   }
	  private int? predefinedOrder ;
	  	  
       
	   [CustomValidation(typeof(DashboardModuleValidationClass), "ValidateClass")]
	   [DataMember]
       public int? PredefinedOrder  
	   {
	    
	     get
		{
		   return predefinedOrder;
		 }
		 set
		 {
		   if(predefinedOrder != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PredefinedOrder",OldValue=predefinedOrder,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   predefinedOrder=value;
		   }
			
		 }
	   }
	    }
   
}
	 