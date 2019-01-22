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
   public partial class DeclarationMamanSpecialActionPM : EntityPM
   {
   	  private string declarationId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DeclarationId  
	   {
	    
	     get
		{
		   return declarationId;
		 }
		 set
		 {
		   if(declarationId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DeclarationId",OldValue=declarationId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   declarationId=value;
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
	  private string mamanSpecialActionCode ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MamanSpecialActionCode  
	   {
	    
	     get
		{
		   return mamanSpecialActionCode;
		 }
		 set
		 {
		   if(mamanSpecialActionCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MamanSpecialActionCode",OldValue=mamanSpecialActionCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mamanSpecialActionCode=value;
		   }
			
		 }
	   }
	  private string mamanLabelText1 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MamanLabelText1  
	   {
	    
	     get
		{
		   return mamanLabelText1;
		 }
		 set
		 {
		   if(mamanLabelText1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MamanLabelText1",OldValue=mamanLabelText1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mamanLabelText1=value;
		   }
			
		 }
	   }
	  private string mamanLabelText2 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MamanLabelText2  
	   {
	    
	     get
		{
		   return mamanLabelText2;
		 }
		 set
		 {
		   if(mamanLabelText2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MamanLabelText2",OldValue=mamanLabelText2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mamanLabelText2=value;
		   }
			
		 }
	   }
	  private string mamanLabelText3 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MamanLabelText3  
	   {
	    
	     get
		{
		   return mamanLabelText3;
		 }
		 set
		 {
		   if(mamanLabelText3 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MamanLabelText3",OldValue=mamanLabelText3,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mamanLabelText3=value;
		   }
			
		 }
	   }
	  private string mamanSpecialActionName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MamanSpecialActionName  
	   {
	    
	     get
		{
		   return mamanSpecialActionName;
		 }
		 set
		 {
		   if(mamanSpecialActionName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MamanSpecialActionName",OldValue=mamanSpecialActionName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mamanSpecialActionName=value;
		   }
			
		 }
	   }
	  private string mamanLabelText4 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MamanLabelText4  
	   {
	    
	     get
		{
		   return mamanLabelText4;
		 }
		 set
		 {
		   if(mamanLabelText4 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MamanLabelText4",OldValue=mamanLabelText4,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mamanLabelText4=value;
		   }
			
		 }
	   }
	  private string mamanLabelText5 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MamanLabelText5  
	   {
	    
	     get
		{
		   return mamanLabelText5;
		 }
		 set
		 {
		   if(mamanLabelText5 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MamanLabelText5",OldValue=mamanLabelText5,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mamanLabelText5=value;
		   }
			
		 }
	   }
	  private string mamanSpecialActionStatusCode ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MamanSpecialActionStatusCode  
	   {
	    
	     get
		{
		   return mamanSpecialActionStatusCode;
		 }
		 set
		 {
		   if(mamanSpecialActionStatusCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MamanSpecialActionStatusCode",OldValue=mamanSpecialActionStatusCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mamanSpecialActionStatusCode=value;
		   }
			
		 }
	   }
	  private string mamanSpecialActionStatusName ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MamanSpecialActionStatusName  
	   {
	    
	     get
		{
		   return mamanSpecialActionStatusName;
		 }
		 set
		 {
		   if(mamanSpecialActionStatusName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MamanSpecialActionStatusName",OldValue=mamanSpecialActionStatusName,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mamanSpecialActionStatusName=value;
		   }
			
		 }
	   }
	  private string mamanSpecialActionsErrorXml ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string MamanSpecialActionsErrorXml  
	   {
	    
	     get
		{
		   return mamanSpecialActionsErrorXml;
		 }
		 set
		 {
		   if(mamanSpecialActionsErrorXml != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="MamanSpecialActionsErrorXml",OldValue=mamanSpecialActionsErrorXml,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   mamanSpecialActionsErrorXml=value;
		   }
			
		 }
	   }
   }
   
}
	 