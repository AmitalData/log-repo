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
   public partial class LBPTeamMemberPM : EntityPM
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
	  private string memberUserId ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string MemberUserId  
	   {
	    
	     get
		{
		   return memberUserId;
		 }
		 set
		 {
		   if(memberUserId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MemberUserId",OldValue=memberUserId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   memberUserId=value;
		   }
			
		 }
	   }
	  private string teamId ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string TeamId  
	   {
	    
	     get
		{
		   return teamId;
		 }
		 set
		 {
		   if(teamId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TeamId",OldValue=teamId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   teamId=value;
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
	  private string memberTeamId ;
	  	  
       
	   [CustomValidation(typeof(InfrastructureValidationClass), "ValidateClass")]
	   [DataMember]
       public string MemberTeamId  
	   {
	    
	     get
		{
		   return memberTeamId;
		 }
		 set
		 {
		   if(memberTeamId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MemberTeamId",OldValue=memberTeamId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   memberTeamId=value;
		   }
			
		 }
	   }

	   private List<TeamMemberBusinessRolePM> businessRolesList;
	    
       [Composition]
 
		     
	   [Include]
	   [Association("LBPTeamMemberTeamMemberBusinessRole", "Id","TeamMemberId")]
	   [DataMember]
	   public virtual List<TeamMemberBusinessRolePM> BusinessRolesList  
	   {
	        get
             {
                 if (businessRolesList == null)
                 {
                     businessRolesList = new List<TeamMemberBusinessRolePM>();
                 }
                 return businessRolesList;
              }
             set { businessRolesList = value; }
	    }
		   
	   private List<TeamMemberBusinessRolePM>  deletedBusinessRolesList;
	   public virtual List<TeamMemberBusinessRolePM> DeletedBusinessRolesList  
	   {
	        get
             {
                 if ( deletedBusinessRolesList == null)
                 {
                      deletedBusinessRolesList = new List<TeamMemberBusinessRolePM>();
                 }
                 return  deletedBusinessRolesList;
              }
             set {  deletedBusinessRolesList = value; }
	    }
	  	    }
   
}
	 