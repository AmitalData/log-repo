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
   public partial class ClaimImporterDeclarsPage3BPM : EntityPM
   {
   	  private string claimId ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string ClaimId  
	   {
	    
	     get
		{
		   return claimId;
		 }
		 set
		 {
		   if(claimId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ClaimId",OldValue=claimId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   claimId=value;
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
	  private int lineNo ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public int LineNo  
	   {
	    
	     get
		{
		   return lineNo;
		 }
		 set
		 {
		   if(lineNo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="LineNo",OldValue=lineNo,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   lineNo=value;
		   }
			
		 }
	   }
	  private decimal? saleAmountAfter ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? SaleAmountAfter  
	   {
	    
	     get
		{
		   return saleAmountAfter;
		 }
		 set
		 {
		   if(saleAmountAfter != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleAmountAfter",OldValue=saleAmountAfter,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   saleAmountAfter=value;
		   }
			
		 }
	   }
	  private decimal? saleAmountClaim ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? SaleAmountClaim  
	   {
	    
	     get
		{
		   return saleAmountClaim;
		 }
		 set
		 {
		   if(saleAmountClaim != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleAmountClaim",OldValue=saleAmountClaim,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   saleAmountClaim=value;
		   }
			
		 }
	   }
	  private decimal? saleAmountBefore ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? SaleAmountBefore  
	   {
	    
	     get
		{
		   return saleAmountBefore;
		 }
		 set
		 {
		   if(saleAmountBefore != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SaleAmountBefore",OldValue=saleAmountBefore,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   saleAmountBefore=value;
		   }
			
		 }
	   }
	  private string descriptionOfGoods ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public string DescriptionOfGoods  
	   {
	    
	     get
		{
		   return descriptionOfGoods;
		 }
		 set
		 {
		   if(descriptionOfGoods != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DescriptionOfGoods",OldValue=descriptionOfGoods,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   descriptionOfGoods=value;
		   }
			
		 }
	   }
	  private decimal? inventoryAmount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? InventoryAmount  
	   {
	    
	     get
		{
		   return inventoryAmount;
		 }
		 set
		 {
		   if(inventoryAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="InventoryAmount",OldValue=inventoryAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   inventoryAmount=value;
		   }
			
		 }
	   }
	  private decimal? soldGoodsAmount ;
	  	  
       
	   [CustomValidation(typeof(CustomsValidationClass), "ValidateClass")]
	   [DataMember]
       public decimal? SoldGoodsAmount  
	   {
	    
	     get
		{
		   return soldGoodsAmount;
		 }
		 set
		 {
		   if(soldGoodsAmount != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SoldGoodsAmount",OldValue=soldGoodsAmount,NewValue=value,PropertyType="decimal?"};
		    NotifyPropertyChanged(values);
		   soldGoodsAmount=value;
		   }
			
		 }
	   }
	    }
   
}
	 