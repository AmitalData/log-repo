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
using Logitude.Infrastructure.BL.Validators;
  
namespace Logitude.Infrastructure.BL.EntityPMs
{
   [CustomValidation(typeof(InfrastructureClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class TeamMemberBusinessRolePM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
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
	  private string teamMemberId ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string TeamMemberId  
	   {
	    
	     get
		{
		   return teamMemberId;
		 }
		 set
		 {
		   if(teamMemberId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TeamMemberId",OldValue=teamMemberId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   teamMemberId=value;
		   }
			
		 }
	   }
	  private string addedByUserId ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string AddedByUserId  
	   {
	    
	     get
		{
		   return addedByUserId;
		 }
		 set
		 {
		   if(addedByUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AddedByUserId",OldValue=addedByUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   addedByUserId=value;
		   }
			
		 }
	   }
	  private DateTime addDate ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime AddDate  
	   {
	    
	     get
		{
		   return addDate;
		 }
		 set
		 {
		   if(addDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AddDate",OldValue=addDate,NewValue=value,PropertyType="DateTime"};
		    NotifyPropertyChanged(values);
		   addDate=value;
		   }
			
		 }
	   }
	  private string businessRoleId ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string BusinessRoleId  
	   {
	    
	     get
		{
		   return businessRoleId;
		 }
		 set
		 {
		   if(businessRoleId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="BusinessRoleId",OldValue=businessRoleId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   businessRoleId=value;
		   }
			
		 }
	   }
	  private string roleName ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string RoleName  
	   {
	    
	     get
		{
		   return roleName;
		 }
		 set
		 {
		   if(roleName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RoleName",OldValue=roleName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   roleName=value;
		   }
			
		 }
	   }
	    }
   
}
	 