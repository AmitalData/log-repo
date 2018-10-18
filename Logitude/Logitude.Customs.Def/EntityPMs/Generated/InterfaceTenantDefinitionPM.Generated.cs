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
   public partial class InterfaceTenantDefinitionPM : EntityPM
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
	  private int tenant ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string code ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private string tenantSendOptionsCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string TenantSendOptionsCode  
	   {
	    
	     get
		{
		   return tenantSendOptionsCode;
		 }
		 set
		 {
		   if(tenantSendOptionsCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantSendOptionsCode",OldValue=tenantSendOptionsCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   tenantSendOptionsCode=value;
		   }
			
		 }
	   }
	  private int? tenantPriority ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int? TenantPriority  
	   {
	    
	     get
		{
		   return tenantPriority;
		 }
		 set
		 {
		   if(tenantPriority != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TenantPriority",OldValue=tenantPriority,NewValue=value,PropertyType="int?"};
		    NotifyPropertyChanged(values);
		   tenantPriority=value;
		   }
			
		 }
	   }
	  private bool active ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool Active  
	   {
	    
	     get
		{
		   return active;
		 }
		 set
		 {
		   if(active != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="Active",OldValue=active,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   active=value;
		   }
			
		 }
	   }
	  private bool dcaRenameFileEnable ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool DcaRenameFileEnable  
	   {
	    
	     get
		{
		   return dcaRenameFileEnable;
		 }
		 set
		 {
		   if(dcaRenameFileEnable != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DcaRenameFileEnable",OldValue=dcaRenameFileEnable,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   dcaRenameFileEnable=value;
		   }
			
		 }
	   }
	  private string dcaRenameFilePrefix ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DcaRenameFilePrefix  
	   {
	    
	     get
		{
		   return dcaRenameFilePrefix;
		 }
		 set
		 {
		   if(dcaRenameFilePrefix != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DcaRenameFilePrefix",OldValue=dcaRenameFilePrefix,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   dcaRenameFilePrefix=value;
		   }
			
		 }
	   }
   }
   
}
	 