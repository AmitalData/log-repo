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
   public partial class DeclarationStatusPM : EntityPM
   {
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
	  private string searchFields ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
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
	  private bool fieldC1 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC1  
	   {
	    
	     get
		{
		   return fieldC1;
		 }
		 set
		 {
		   if(fieldC1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC1",OldValue=fieldC1,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC1=value;
		   }
			
		 }
	   }
	  private bool fieldC2 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC2  
	   {
	    
	     get
		{
		   return fieldC2;
		 }
		 set
		 {
		   if(fieldC2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC2",OldValue=fieldC2,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC2=value;
		   }
			
		 }
	   }
	  private bool fieldC3 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC3  
	   {
	    
	     get
		{
		   return fieldC3;
		 }
		 set
		 {
		   if(fieldC3 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC3",OldValue=fieldC3,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC3=value;
		   }
			
		 }
	   }
	  private bool fieldC4 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC4  
	   {
	    
	     get
		{
		   return fieldC4;
		 }
		 set
		 {
		   if(fieldC4 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC4",OldValue=fieldC4,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC4=value;
		   }
			
		 }
	   }
	  private bool fieldC5 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC5  
	   {
	    
	     get
		{
		   return fieldC5;
		 }
		 set
		 {
		   if(fieldC5 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC5",OldValue=fieldC5,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC5=value;
		   }
			
		 }
	   }
	  private bool fieldC6 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC6  
	   {
	    
	     get
		{
		   return fieldC6;
		 }
		 set
		 {
		   if(fieldC6 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC6",OldValue=fieldC6,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC6=value;
		   }
			
		 }
	   }
	  private bool fieldC7 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC7  
	   {
	    
	     get
		{
		   return fieldC7;
		 }
		 set
		 {
		   if(fieldC7 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC7",OldValue=fieldC7,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC7=value;
		   }
			
		 }
	   }
	  private bool fieldC8 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC8  
	   {
	    
	     get
		{
		   return fieldC8;
		 }
		 set
		 {
		   if(fieldC8 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC8",OldValue=fieldC8,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC8=value;
		   }
			
		 }
	   }
	  private bool fieldC9 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC9  
	   {
	    
	     get
		{
		   return fieldC9;
		 }
		 set
		 {
		   if(fieldC9 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC9",OldValue=fieldC9,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC9=value;
		   }
			
		 }
	   }
	  private bool fieldC10 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC10  
	   {
	    
	     get
		{
		   return fieldC10;
		 }
		 set
		 {
		   if(fieldC10 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC10",OldValue=fieldC10,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC10=value;
		   }
			
		 }
	   }
	  private bool fieldC11 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC11  
	   {
	    
	     get
		{
		   return fieldC11;
		 }
		 set
		 {
		   if(fieldC11 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC11",OldValue=fieldC11,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC11=value;
		   }
			
		 }
	   }
	  private bool fieldC12 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC12  
	   {
	    
	     get
		{
		   return fieldC12;
		 }
		 set
		 {
		   if(fieldC12 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC12",OldValue=fieldC12,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC12=value;
		   }
			
		 }
	   }
	  private bool fieldC13 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC13  
	   {
	    
	     get
		{
		   return fieldC13;
		 }
		 set
		 {
		   if(fieldC13 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC13",OldValue=fieldC13,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC13=value;
		   }
			
		 }
	   }
	  private bool fieldC14 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC14  
	   {
	    
	     get
		{
		   return fieldC14;
		 }
		 set
		 {
		   if(fieldC14 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC14",OldValue=fieldC14,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC14=value;
		   }
			
		 }
	   }
	  private bool fieldC15 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC15  
	   {
	    
	     get
		{
		   return fieldC15;
		 }
		 set
		 {
		   if(fieldC15 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC15",OldValue=fieldC15,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC15=value;
		   }
			
		 }
	   }
	  private bool fieldC16 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC16  
	   {
	    
	     get
		{
		   return fieldC16;
		 }
		 set
		 {
		   if(fieldC16 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC16",OldValue=fieldC16,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC16=value;
		   }
			
		 }
	   }
	  private bool fieldC17 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC17  
	   {
	    
	     get
		{
		   return fieldC17;
		 }
		 set
		 {
		   if(fieldC17 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC17",OldValue=fieldC17,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC17=value;
		   }
			
		 }
	   }
	  private bool fieldC18 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC18  
	   {
	    
	     get
		{
		   return fieldC18;
		 }
		 set
		 {
		   if(fieldC18 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC18",OldValue=fieldC18,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC18=value;
		   }
			
		 }
	   }
	  private bool fieldC19 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC19  
	   {
	    
	     get
		{
		   return fieldC19;
		 }
		 set
		 {
		   if(fieldC19 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC19",OldValue=fieldC19,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC19=value;
		   }
			
		 }
	   }
	  private bool fieldC20 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC20  
	   {
	    
	     get
		{
		   return fieldC20;
		 }
		 set
		 {
		   if(fieldC20 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC20",OldValue=fieldC20,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC20=value;
		   }
			
		 }
	   }
	  private bool fieldC21 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC21  
	   {
	    
	     get
		{
		   return fieldC21;
		 }
		 set
		 {
		   if(fieldC21 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC21",OldValue=fieldC21,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC21=value;
		   }
			
		 }
	   }
	  private bool fieldC22 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC22  
	   {
	    
	     get
		{
		   return fieldC22;
		 }
		 set
		 {
		   if(fieldC22 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC22",OldValue=fieldC22,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC22=value;
		   }
			
		 }
	   }
	  private bool fieldC23 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC23  
	   {
	    
	     get
		{
		   return fieldC23;
		 }
		 set
		 {
		   if(fieldC23 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC23",OldValue=fieldC23,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC23=value;
		   }
			
		 }
	   }
	  private bool fieldC24 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC24  
	   {
	    
	     get
		{
		   return fieldC24;
		 }
		 set
		 {
		   if(fieldC24 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC24",OldValue=fieldC24,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC24=value;
		   }
			
		 }
	   }
	  private bool fieldC25 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC25  
	   {
	    
	     get
		{
		   return fieldC25;
		 }
		 set
		 {
		   if(fieldC25 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC25",OldValue=fieldC25,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC25=value;
		   }
			
		 }
	   }
	  private bool fieldC26 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC26  
	   {
	    
	     get
		{
		   return fieldC26;
		 }
		 set
		 {
		   if(fieldC26 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC26",OldValue=fieldC26,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC26=value;
		   }
			
		 }
	   }
	  private bool fieldC27 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC27  
	   {
	    
	     get
		{
		   return fieldC27;
		 }
		 set
		 {
		   if(fieldC27 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC27",OldValue=fieldC27,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC27=value;
		   }
			
		 }
	   }
	  private bool fieldC28 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC28  
	   {
	    
	     get
		{
		   return fieldC28;
		 }
		 set
		 {
		   if(fieldC28 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC28",OldValue=fieldC28,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC28=value;
		   }
			
		 }
	   }
	  private bool fieldC29 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC29  
	   {
	    
	     get
		{
		   return fieldC29;
		 }
		 set
		 {
		   if(fieldC29 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC29",OldValue=fieldC29,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC29=value;
		   }
			
		 }
	   }
	  private bool fieldC30 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC30  
	   {
	    
	     get
		{
		   return fieldC30;
		 }
		 set
		 {
		   if(fieldC30 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC30",OldValue=fieldC30,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC30=value;
		   }
			
		 }
	   }
	  private bool fieldC31 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC31  
	   {
	    
	     get
		{
		   return fieldC31;
		 }
		 set
		 {
		   if(fieldC31 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC31",OldValue=fieldC31,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC31=value;
		   }
			
		 }
	   }
	  private bool fieldC32 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC32  
	   {
	    
	     get
		{
		   return fieldC32;
		 }
		 set
		 {
		   if(fieldC32 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC32",OldValue=fieldC32,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC32=value;
		   }
			
		 }
	   }
	  private bool fieldC33 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC33  
	   {
	    
	     get
		{
		   return fieldC33;
		 }
		 set
		 {
		   if(fieldC33 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC33",OldValue=fieldC33,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC33=value;
		   }
			
		 }
	   }
	  private bool fieldC34 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC34  
	   {
	    
	     get
		{
		   return fieldC34;
		 }
		 set
		 {
		   if(fieldC34 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC34",OldValue=fieldC34,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC34=value;
		   }
			
		 }
	   }
	  private bool fieldC35 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC35  
	   {
	    
	     get
		{
		   return fieldC35;
		 }
		 set
		 {
		   if(fieldC35 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC35",OldValue=fieldC35,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC35=value;
		   }
			
		 }
	   }
	  private bool fieldC36 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC36  
	   {
	    
	     get
		{
		   return fieldC36;
		 }
		 set
		 {
		   if(fieldC36 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC36",OldValue=fieldC36,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC36=value;
		   }
			
		 }
	   }
	  private bool fieldC37 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC37  
	   {
	    
	     get
		{
		   return fieldC37;
		 }
		 set
		 {
		   if(fieldC37 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC37",OldValue=fieldC37,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC37=value;
		   }
			
		 }
	   }
	  private bool fieldC38 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC38  
	   {
	    
	     get
		{
		   return fieldC38;
		 }
		 set
		 {
		   if(fieldC38 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC38",OldValue=fieldC38,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC38=value;
		   }
			
		 }
	   }
	  private bool fieldC39 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC39  
	   {
	    
	     get
		{
		   return fieldC39;
		 }
		 set
		 {
		   if(fieldC39 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC39",OldValue=fieldC39,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC39=value;
		   }
			
		 }
	   }
	  private bool fieldC40 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC40  
	   {
	    
	     get
		{
		   return fieldC40;
		 }
		 set
		 {
		   if(fieldC40 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC40",OldValue=fieldC40,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC40=value;
		   }
			
		 }
	   }
	  private bool fieldC41 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC41  
	   {
	    
	     get
		{
		   return fieldC41;
		 }
		 set
		 {
		   if(fieldC41 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC41",OldValue=fieldC41,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC41=value;
		   }
			
		 }
	   }
	  private bool fieldC42 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC42  
	   {
	    
	     get
		{
		   return fieldC42;
		 }
		 set
		 {
		   if(fieldC42 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC42",OldValue=fieldC42,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC42=value;
		   }
			
		 }
	   }
	  private bool fieldC43 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC43  
	   {
	    
	     get
		{
		   return fieldC43;
		 }
		 set
		 {
		   if(fieldC43 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC43",OldValue=fieldC43,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC43=value;
		   }
			
		 }
	   }
	  private bool fieldC44 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC44  
	   {
	    
	     get
		{
		   return fieldC44;
		 }
		 set
		 {
		   if(fieldC44 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC44",OldValue=fieldC44,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC44=value;
		   }
			
		 }
	   }
	  private bool fieldC45 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC45  
	   {
	    
	     get
		{
		   return fieldC45;
		 }
		 set
		 {
		   if(fieldC45 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC45",OldValue=fieldC45,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC45=value;
		   }
			
		 }
	   }
	  private bool fieldC46 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC46  
	   {
	    
	     get
		{
		   return fieldC46;
		 }
		 set
		 {
		   if(fieldC46 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC46",OldValue=fieldC46,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC46=value;
		   }
			
		 }
	   }
	  private bool fieldC47 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC47  
	   {
	    
	     get
		{
		   return fieldC47;
		 }
		 set
		 {
		   if(fieldC47 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC47",OldValue=fieldC47,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC47=value;
		   }
			
		 }
	   }
	  private bool fieldC48 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC48  
	   {
	    
	     get
		{
		   return fieldC48;
		 }
		 set
		 {
		   if(fieldC48 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC48",OldValue=fieldC48,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC48=value;
		   }
			
		 }
	   }
	  private bool fieldC49 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC49  
	   {
	    
	     get
		{
		   return fieldC49;
		 }
		 set
		 {
		   if(fieldC49 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC49",OldValue=fieldC49,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC49=value;
		   }
			
		 }
	   }
	  private bool fieldC50 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public bool FieldC50  
	   {
	    
	     get
		{
		   return fieldC50;
		 }
		 set
		 {
		   if(fieldC50 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldC50",OldValue=fieldC50,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   fieldC50=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD1 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD1  
	   {
	    
	     get
		{
		   return fieldD1;
		 }
		 set
		 {
		   if(fieldD1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD1",OldValue=fieldD1,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD1=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD2 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD2  
	   {
	    
	     get
		{
		   return fieldD2;
		 }
		 set
		 {
		   if(fieldD2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD2",OldValue=fieldD2,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD2=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD3 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD3  
	   {
	    
	     get
		{
		   return fieldD3;
		 }
		 set
		 {
		   if(fieldD3 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD3",OldValue=fieldD3,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD3=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD4 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD4  
	   {
	    
	     get
		{
		   return fieldD4;
		 }
		 set
		 {
		   if(fieldD4 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD4",OldValue=fieldD4,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD4=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD5 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD5  
	   {
	    
	     get
		{
		   return fieldD5;
		 }
		 set
		 {
		   if(fieldD5 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD5",OldValue=fieldD5,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD5=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD6 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD6  
	   {
	    
	     get
		{
		   return fieldD6;
		 }
		 set
		 {
		   if(fieldD6 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD6",OldValue=fieldD6,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD6=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD7 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD7  
	   {
	    
	     get
		{
		   return fieldD7;
		 }
		 set
		 {
		   if(fieldD7 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD7",OldValue=fieldD7,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD7=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD8 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD8  
	   {
	    
	     get
		{
		   return fieldD8;
		 }
		 set
		 {
		   if(fieldD8 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD8",OldValue=fieldD8,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD8=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD9 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD9  
	   {
	    
	     get
		{
		   return fieldD9;
		 }
		 set
		 {
		   if(fieldD9 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD9",OldValue=fieldD9,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD9=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD10 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD10  
	   {
	    
	     get
		{
		   return fieldD10;
		 }
		 set
		 {
		   if(fieldD10 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD10",OldValue=fieldD10,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD10=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD11 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD11  
	   {
	    
	     get
		{
		   return fieldD11;
		 }
		 set
		 {
		   if(fieldD11 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD11",OldValue=fieldD11,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD11=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD12 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD12  
	   {
	    
	     get
		{
		   return fieldD12;
		 }
		 set
		 {
		   if(fieldD12 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD12",OldValue=fieldD12,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD12=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD13 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD13  
	   {
	    
	     get
		{
		   return fieldD13;
		 }
		 set
		 {
		   if(fieldD13 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD13",OldValue=fieldD13,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD13=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD14 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD14  
	   {
	    
	     get
		{
		   return fieldD14;
		 }
		 set
		 {
		   if(fieldD14 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD14",OldValue=fieldD14,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD14=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD15 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD15  
	   {
	    
	     get
		{
		   return fieldD15;
		 }
		 set
		 {
		   if(fieldD15 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD15",OldValue=fieldD15,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD15=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD16 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD16  
	   {
	    
	     get
		{
		   return fieldD16;
		 }
		 set
		 {
		   if(fieldD16 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD16",OldValue=fieldD16,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD16=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD17 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD17  
	   {
	    
	     get
		{
		   return fieldD17;
		 }
		 set
		 {
		   if(fieldD17 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD17",OldValue=fieldD17,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD17=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD18 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD18  
	   {
	    
	     get
		{
		   return fieldD18;
		 }
		 set
		 {
		   if(fieldD18 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD18",OldValue=fieldD18,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD18=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD19 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD19  
	   {
	    
	     get
		{
		   return fieldD19;
		 }
		 set
		 {
		   if(fieldD19 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD19",OldValue=fieldD19,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD19=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD20 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD20  
	   {
	    
	     get
		{
		   return fieldD20;
		 }
		 set
		 {
		   if(fieldD20 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD20",OldValue=fieldD20,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD20=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD21 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD21  
	   {
	    
	     get
		{
		   return fieldD21;
		 }
		 set
		 {
		   if(fieldD21 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD21",OldValue=fieldD21,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD21=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD22 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD22  
	   {
	    
	     get
		{
		   return fieldD22;
		 }
		 set
		 {
		   if(fieldD22 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD22",OldValue=fieldD22,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD22=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD23 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD23  
	   {
	    
	     get
		{
		   return fieldD23;
		 }
		 set
		 {
		   if(fieldD23 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD23",OldValue=fieldD23,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD23=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD24 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD24  
	   {
	    
	     get
		{
		   return fieldD24;
		 }
		 set
		 {
		   if(fieldD24 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD24",OldValue=fieldD24,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD24=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD25 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD25  
	   {
	    
	     get
		{
		   return fieldD25;
		 }
		 set
		 {
		   if(fieldD25 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD25",OldValue=fieldD25,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD25=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD26 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD26  
	   {
	    
	     get
		{
		   return fieldD26;
		 }
		 set
		 {
		   if(fieldD26 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD26",OldValue=fieldD26,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD26=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD27 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD27  
	   {
	    
	     get
		{
		   return fieldD27;
		 }
		 set
		 {
		   if(fieldD27 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD27",OldValue=fieldD27,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD27=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD28 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD28  
	   {
	    
	     get
		{
		   return fieldD28;
		 }
		 set
		 {
		   if(fieldD28 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD28",OldValue=fieldD28,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD28=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD29 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD29  
	   {
	    
	     get
		{
		   return fieldD29;
		 }
		 set
		 {
		   if(fieldD29 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD29",OldValue=fieldD29,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD29=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD30 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD30  
	   {
	    
	     get
		{
		   return fieldD30;
		 }
		 set
		 {
		   if(fieldD30 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD30",OldValue=fieldD30,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD30=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD31 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD31  
	   {
	    
	     get
		{
		   return fieldD31;
		 }
		 set
		 {
		   if(fieldD31 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD31",OldValue=fieldD31,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD31=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD32 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD32  
	   {
	    
	     get
		{
		   return fieldD32;
		 }
		 set
		 {
		   if(fieldD32 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD32",OldValue=fieldD32,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD32=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD33 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD33  
	   {
	    
	     get
		{
		   return fieldD33;
		 }
		 set
		 {
		   if(fieldD33 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD33",OldValue=fieldD33,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD33=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD34 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD34  
	   {
	    
	     get
		{
		   return fieldD34;
		 }
		 set
		 {
		   if(fieldD34 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD34",OldValue=fieldD34,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD34=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD35 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD35  
	   {
	    
	     get
		{
		   return fieldD35;
		 }
		 set
		 {
		   if(fieldD35 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD35",OldValue=fieldD35,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD35=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD36 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD36  
	   {
	    
	     get
		{
		   return fieldD36;
		 }
		 set
		 {
		   if(fieldD36 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD36",OldValue=fieldD36,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD36=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD37 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD37  
	   {
	    
	     get
		{
		   return fieldD37;
		 }
		 set
		 {
		   if(fieldD37 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD37",OldValue=fieldD37,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD37=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD38 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD38  
	   {
	    
	     get
		{
		   return fieldD38;
		 }
		 set
		 {
		   if(fieldD38 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD38",OldValue=fieldD38,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD38=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD39 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD39  
	   {
	    
	     get
		{
		   return fieldD39;
		 }
		 set
		 {
		   if(fieldD39 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD39",OldValue=fieldD39,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD39=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD40 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD40  
	   {
	    
	     get
		{
		   return fieldD40;
		 }
		 set
		 {
		   if(fieldD40 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD40",OldValue=fieldD40,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD40=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD41 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD41  
	   {
	    
	     get
		{
		   return fieldD41;
		 }
		 set
		 {
		   if(fieldD41 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD41",OldValue=fieldD41,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD41=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD42 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD42  
	   {
	    
	     get
		{
		   return fieldD42;
		 }
		 set
		 {
		   if(fieldD42 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD42",OldValue=fieldD42,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD42=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD43 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD43  
	   {
	    
	     get
		{
		   return fieldD43;
		 }
		 set
		 {
		   if(fieldD43 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD43",OldValue=fieldD43,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD43=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD44 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD44  
	   {
	    
	     get
		{
		   return fieldD44;
		 }
		 set
		 {
		   if(fieldD44 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD44",OldValue=fieldD44,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD44=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD45 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD45  
	   {
	    
	     get
		{
		   return fieldD45;
		 }
		 set
		 {
		   if(fieldD45 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD45",OldValue=fieldD45,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD45=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD46 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD46  
	   {
	    
	     get
		{
		   return fieldD46;
		 }
		 set
		 {
		   if(fieldD46 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD46",OldValue=fieldD46,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD46=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD47 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD47  
	   {
	    
	     get
		{
		   return fieldD47;
		 }
		 set
		 {
		   if(fieldD47 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD47",OldValue=fieldD47,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD47=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD48 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD48  
	   {
	    
	     get
		{
		   return fieldD48;
		 }
		 set
		 {
		   if(fieldD48 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD48",OldValue=fieldD48,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD48=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD49 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD49  
	   {
	    
	     get
		{
		   return fieldD49;
		 }
		 set
		 {
		   if(fieldD49 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD49",OldValue=fieldD49,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD49=value;
		   }
			
		 }
	   }
	  private DateTime? fieldD50 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? FieldD50  
	   {
	    
	     get
		{
		   return fieldD50;
		 }
		 set
		 {
		   if(fieldD50 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldD50",OldValue=fieldD50,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   fieldD50=value;
		   }
			
		 }
	   }
	  private string fieldR1 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FieldR1  
	   {
	    
	     get
		{
		   return fieldR1;
		 }
		 set
		 {
		   if(fieldR1 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldR1",OldValue=fieldR1,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fieldR1=value;
		   }
			
		 }
	   }
	  private string fieldR2 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FieldR2  
	   {
	    
	     get
		{
		   return fieldR2;
		 }
		 set
		 {
		   if(fieldR2 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldR2",OldValue=fieldR2,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fieldR2=value;
		   }
			
		 }
	   }
	  private string fieldR3 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FieldR3  
	   {
	    
	     get
		{
		   return fieldR3;
		 }
		 set
		 {
		   if(fieldR3 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldR3",OldValue=fieldR3,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fieldR3=value;
		   }
			
		 }
	   }
	  private string fieldR4 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FieldR4  
	   {
	    
	     get
		{
		   return fieldR4;
		 }
		 set
		 {
		   if(fieldR4 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldR4",OldValue=fieldR4,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fieldR4=value;
		   }
			
		 }
	   }
	  private string fieldR5 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FieldR5  
	   {
	    
	     get
		{
		   return fieldR5;
		 }
		 set
		 {
		   if(fieldR5 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldR5",OldValue=fieldR5,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fieldR5=value;
		   }
			
		 }
	   }
	  private string fieldR6 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FieldR6  
	   {
	    
	     get
		{
		   return fieldR6;
		 }
		 set
		 {
		   if(fieldR6 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldR6",OldValue=fieldR6,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fieldR6=value;
		   }
			
		 }
	   }
	  private string fieldR7 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FieldR7  
	   {
	    
	     get
		{
		   return fieldR7;
		 }
		 set
		 {
		   if(fieldR7 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldR7",OldValue=fieldR7,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fieldR7=value;
		   }
			
		 }
	   }
	  private string fieldR8 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FieldR8  
	   {
	    
	     get
		{
		   return fieldR8;
		 }
		 set
		 {
		   if(fieldR8 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldR8",OldValue=fieldR8,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fieldR8=value;
		   }
			
		 }
	   }
	  private string fieldR9 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FieldR9  
	   {
	    
	     get
		{
		   return fieldR9;
		 }
		 set
		 {
		   if(fieldR9 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldR9",OldValue=fieldR9,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fieldR9=value;
		   }
			
		 }
	   }
	  private string fieldR10 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FieldR10  
	   {
	    
	     get
		{
		   return fieldR10;
		 }
		 set
		 {
		   if(fieldR10 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldR10",OldValue=fieldR10,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fieldR10=value;
		   }
			
		 }
	   }
	  private string fieldR11 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FieldR11  
	   {
	    
	     get
		{
		   return fieldR11;
		 }
		 set
		 {
		   if(fieldR11 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldR11",OldValue=fieldR11,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fieldR11=value;
		   }
			
		 }
	   }
	  private string fieldR12 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FieldR12  
	   {
	    
	     get
		{
		   return fieldR12;
		 }
		 set
		 {
		   if(fieldR12 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldR12",OldValue=fieldR12,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fieldR12=value;
		   }
			
		 }
	   }
	  private string fieldR13 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FieldR13  
	   {
	    
	     get
		{
		   return fieldR13;
		 }
		 set
		 {
		   if(fieldR13 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldR13",OldValue=fieldR13,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fieldR13=value;
		   }
			
		 }
	   }
	  private string fieldR14 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FieldR14  
	   {
	    
	     get
		{
		   return fieldR14;
		 }
		 set
		 {
		   if(fieldR14 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldR14",OldValue=fieldR14,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fieldR14=value;
		   }
			
		 }
	   }
	  private string fieldR15 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FieldR15  
	   {
	    
	     get
		{
		   return fieldR15;
		 }
		 set
		 {
		   if(fieldR15 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldR15",OldValue=fieldR15,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fieldR15=value;
		   }
			
		 }
	   }
	  private string fieldR16 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FieldR16  
	   {
	    
	     get
		{
		   return fieldR16;
		 }
		 set
		 {
		   if(fieldR16 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldR16",OldValue=fieldR16,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fieldR16=value;
		   }
			
		 }
	   }
	  private string fieldR17 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FieldR17  
	   {
	    
	     get
		{
		   return fieldR17;
		 }
		 set
		 {
		   if(fieldR17 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldR17",OldValue=fieldR17,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fieldR17=value;
		   }
			
		 }
	   }
	  private string fieldR18 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FieldR18  
	   {
	    
	     get
		{
		   return fieldR18;
		 }
		 set
		 {
		   if(fieldR18 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldR18",OldValue=fieldR18,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fieldR18=value;
		   }
			
		 }
	   }
	  private string fieldR19 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FieldR19  
	   {
	    
	     get
		{
		   return fieldR19;
		 }
		 set
		 {
		   if(fieldR19 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldR19",OldValue=fieldR19,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fieldR19=value;
		   }
			
		 }
	   }
	  private string fieldR20 ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string FieldR20  
	   {
	    
	     get
		{
		   return fieldR20;
		 }
		 set
		 {
		   if(fieldR20 != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="FieldR20",OldValue=fieldR20,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   fieldR20=value;
		   }
			
		 }
	   }
	  private DateTime? sVC ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? SVC  
	   {
	    
	     get
		{
		   return sVC;
		 }
		 set
		 {
		   if(sVC != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SVC",OldValue=sVC,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   sVC=value;
		   }
			
		 }
	   }
	  private DateTime? iNA ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? INA  
	   {
	    
	     get
		{
		   return iNA;
		 }
		 set
		 {
		   if(iNA != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="INA",OldValue=iNA,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   iNA=value;
		   }
			
		 }
	   }
	  private DateTime? rSG ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? RSG  
	   {
	    
	     get
		{
		   return rSG;
		 }
		 set
		 {
		   if(rSG != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RSG",OldValue=rSG,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   rSG=value;
		   }
			
		 }
	   }
	  private DateTime? rSH ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public DateTime? RSH  
	   {
	    
	     get
		{
		   return rSH;
		 }
		 set
		 {
		   if(rSH != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RSH",OldValue=rSH,NewValue=value,PropertyType="DateTime?"};
		    NotifyPropertyChanged(values);
		   rSH=value;
		   }
			
		 }
	   }
   }
   
}
	 