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
using Amital.QuoteOPM.Def.Validators;
  
namespace Amital.QuoteOPM.Def.EntityPMs
{
   [CustomValidation(typeof(QuoteOPMClassLevelValidator), "ValidateClass")]
   [DataContract]
   public partial class QuoteOPTemplateSettingPM : EntityPM
   {
   	  private string id ;
	  
       [Key]
	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
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
	  private bool showTotalInSaleCurrencyPackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowTotalInSaleCurrencyPackages  
	   {
	    
	     get
		{
		   return showTotalInSaleCurrencyPackages;
		 }
		 set
		 {
		   if(showTotalInSaleCurrencyPackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowTotalInSaleCurrencyPackages",OldValue=showTotalInSaleCurrencyPackages,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showTotalInSaleCurrencyPackages=value;
		   }
			
		 }
	   }
	  private bool showTotalInSaleCurrencyContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowTotalInSaleCurrencyContainers  
	   {
	    
	     get
		{
		   return showTotalInSaleCurrencyContainers;
		 }
		 set
		 {
		   if(showTotalInSaleCurrencyContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowTotalInSaleCurrencyContainers",OldValue=showTotalInSaleCurrencyContainers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showTotalInSaleCurrencyContainers=value;
		   }
			
		 }
	   }
	  private bool showTotalInLocalCurrencyPackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowTotalInLocalCurrencyPackages  
	   {
	    
	     get
		{
		   return showTotalInLocalCurrencyPackages;
		 }
		 set
		 {
		   if(showTotalInLocalCurrencyPackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowTotalInLocalCurrencyPackages",OldValue=showTotalInLocalCurrencyPackages,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showTotalInLocalCurrencyPackages=value;
		   }
			
		 }
	   }
	  private bool showTotalInLocalCurrencyContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowTotalInLocalCurrencyContainers  
	   {
	    
	     get
		{
		   return showTotalInLocalCurrencyContainers;
		 }
		 set
		 {
		   if(showTotalInLocalCurrencyContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowTotalInLocalCurrencyContainers",OldValue=showTotalInLocalCurrencyContainers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showTotalInLocalCurrencyContainers=value;
		   }
			
		 }
	   }
	  private bool showPricesTablePackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowPricesTablePackages  
	   {
	    
	     get
		{
		   return showPricesTablePackages;
		 }
		 set
		 {
		   if(showPricesTablePackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowPricesTablePackages",OldValue=showPricesTablePackages,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showPricesTablePackages=value;
		   }
			
		 }
	   }
	  private bool showPricesTableContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowPricesTableContainers  
	   {
	    
	     get
		{
		   return showPricesTableContainers;
		 }
		 set
		 {
		   if(showPricesTableContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowPricesTableContainers",OldValue=showPricesTableContainers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showPricesTableContainers=value;
		   }
			
		 }
	   }
	  private bool showChargeCodePackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowChargeCodePackages  
	   {
	    
	     get
		{
		   return showChargeCodePackages;
		 }
		 set
		 {
		   if(showChargeCodePackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowChargeCodePackages",OldValue=showChargeCodePackages,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showChargeCodePackages=value;
		   }
			
		 }
	   }
	  private bool showChargeDescriptionPackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowChargeDescriptionPackages  
	   {
	    
	     get
		{
		   return showChargeDescriptionPackages;
		 }
		 set
		 {
		   if(showChargeDescriptionPackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowChargeDescriptionPackages",OldValue=showChargeDescriptionPackages,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showChargeDescriptionPackages=value;
		   }
			
		 }
	   }
	  private bool showChargeDescriptionContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowChargeDescriptionContainers  
	   {
	    
	     get
		{
		   return showChargeDescriptionContainers;
		 }
		 set
		 {
		   if(showChargeDescriptionContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowChargeDescriptionContainers",OldValue=showChargeDescriptionContainers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showChargeDescriptionContainers=value;
		   }
			
		 }
	   }
	  private bool showChargeCodeContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowChargeCodeContainers  
	   {
	    
	     get
		{
		   return showChargeCodeContainers;
		 }
		 set
		 {
		   if(showChargeCodeContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowChargeCodeContainers",OldValue=showChargeCodeContainers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showChargeCodeContainers=value;
		   }
			
		 }
	   }
	  private bool showChargeNamePackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowChargeNamePackages  
	   {
	    
	     get
		{
		   return showChargeNamePackages;
		 }
		 set
		 {
		   if(showChargeNamePackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowChargeNamePackages",OldValue=showChargeNamePackages,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showChargeNamePackages=value;
		   }
			
		 }
	   }
	  private bool showChargeNameContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowChargeNameContainers  
	   {
	    
	     get
		{
		   return showChargeNameContainers;
		 }
		 set
		 {
		   if(showChargeNameContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowChargeNameContainers",OldValue=showChargeNameContainers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showChargeNameContainers=value;
		   }
			
		 }
	   }
	  private bool showMeasurementPackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowMeasurementPackages  
	   {
	    
	     get
		{
		   return showMeasurementPackages;
		 }
		 set
		 {
		   if(showMeasurementPackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowMeasurementPackages",OldValue=showMeasurementPackages,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showMeasurementPackages=value;
		   }
			
		 }
	   }
	  private bool showMeasurementContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowMeasurementContainers  
	   {
	    
	     get
		{
		   return showMeasurementContainers;
		 }
		 set
		 {
		   if(showMeasurementContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowMeasurementContainers",OldValue=showMeasurementContainers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showMeasurementContainers=value;
		   }
			
		 }
	   }
	  private bool showFixedPriceContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowFixedPriceContainers  
	   {
	    
	     get
		{
		   return showFixedPriceContainers;
		 }
		 set
		 {
		   if(showFixedPriceContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowFixedPriceContainers",OldValue=showFixedPriceContainers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showFixedPriceContainers=value;
		   }
			
		 }
	   }
	  private bool showUnitsPackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowUnitsPackages  
	   {
	    
	     get
		{
		   return showUnitsPackages;
		 }
		 set
		 {
		   if(showUnitsPackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowUnitsPackages",OldValue=showUnitsPackages,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showUnitsPackages=value;
		   }
			
		 }
	   }
	  private bool showUnitPricePackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowUnitPricePackages  
	   {
	    
	     get
		{
		   return showUnitPricePackages;
		 }
		 set
		 {
		   if(showUnitPricePackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowUnitPricePackages",OldValue=showUnitPricePackages,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showUnitPricePackages=value;
		   }
			
		 }
	   }
	  private bool showLocalCurrencyColumnPackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowLocalCurrencyColumnPackages  
	   {
	    
	     get
		{
		   return showLocalCurrencyColumnPackages;
		 }
		 set
		 {
		   if(showLocalCurrencyColumnPackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowLocalCurrencyColumnPackages",OldValue=showLocalCurrencyColumnPackages,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showLocalCurrencyColumnPackages=value;
		   }
			
		 }
	   }
	  private bool showLocalCurrencyColumnContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowLocalCurrencyColumnContainers  
	   {
	    
	     get
		{
		   return showLocalCurrencyColumnContainers;
		 }
		 set
		 {
		   if(showLocalCurrencyColumnContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowLocalCurrencyColumnContainers",OldValue=showLocalCurrencyColumnContainers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showLocalCurrencyColumnContainers=value;
		   }
			
		 }
	   }
	  private bool showSaleCurrencyColumnPackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowSaleCurrencyColumnPackages  
	   {
	    
	     get
		{
		   return showSaleCurrencyColumnPackages;
		 }
		 set
		 {
		   if(showSaleCurrencyColumnPackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowSaleCurrencyColumnPackages",OldValue=showSaleCurrencyColumnPackages,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showSaleCurrencyColumnPackages=value;
		   }
			
		 }
	   }
	  private bool showSaleCurrencyColumnContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowSaleCurrencyColumnContainers  
	   {
	    
	     get
		{
		   return showSaleCurrencyColumnContainers;
		 }
		 set
		 {
		   if(showSaleCurrencyColumnContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowSaleCurrencyColumnContainers",OldValue=showSaleCurrencyColumnContainers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showSaleCurrencyColumnContainers=value;
		   }
			
		 }
	   }
	  private bool showLocalLanguage ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowLocalLanguage  
	   {
	    
	     get
		{
		   return showLocalLanguage;
		 }
		 set
		 {
		   if(showLocalLanguage != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowLocalLanguage",OldValue=showLocalLanguage,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showLocalLanguage=value;
		   }
			
		 }
	   }
	  private bool splitChargesbyGroupsPackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool SplitChargesbyGroupsPackages  
	   {
	    
	     get
		{
		   return splitChargesbyGroupsPackages;
		 }
		 set
		 {
		   if(splitChargesbyGroupsPackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SplitChargesbyGroupsPackages",OldValue=splitChargesbyGroupsPackages,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   splitChargesbyGroupsPackages=value;
		   }
			
		 }
	   }
	  private bool splitChargesbyGroupsContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool SplitChargesbyGroupsContainers  
	   {
	    
	     get
		{
		   return splitChargesbyGroupsContainers;
		 }
		 set
		 {
		   if(splitChargesbyGroupsContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SplitChargesbyGroupsContainers",OldValue=splitChargesbyGroupsContainers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   splitChargesbyGroupsContainers=value;
		   }
			
		 }
	   }
	  private bool showContainerNameInsteadOfCodeContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowContainerNameInsteadOfCodeContainers  
	   {
	    
	     get
		{
		   return showContainerNameInsteadOfCodeContainers;
		 }
		 set
		 {
		   if(showContainerNameInsteadOfCodeContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowContainerNameInsteadOfCodeContainers",OldValue=showContainerNameInsteadOfCodeContainers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showContainerNameInsteadOfCodeContainers=value;
		   }
			
		 }
	   }
	  private bool alignRight ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool AlignRight  
	   {
	    
	     get
		{
		   return alignRight;
		 }
		 set
		 {
		   if(alignRight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="AlignRight",OldValue=alignRight,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   alignRight=value;
		   }
			
		 }
	   }
	  private bool showPriceByContainerColumn ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowPriceByContainerColumn  
	   {
	    
	     get
		{
		   return showPriceByContainerColumn;
		 }
		 set
		 {
		   if(showPriceByContainerColumn != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowPriceByContainerColumn",OldValue=showPriceByContainerColumn,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showPriceByContainerColumn=value;
		   }
			
		 }
	   }
	  private bool showCodeChargeSaleMinMaxContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowCodeChargeSaleMinMaxContainers  
	   {
	    
	     get
		{
		   return showCodeChargeSaleMinMaxContainers;
		 }
		 set
		 {
		   if(showCodeChargeSaleMinMaxContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowCodeChargeSaleMinMaxContainers",OldValue=showCodeChargeSaleMinMaxContainers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showCodeChargeSaleMinMaxContainers=value;
		   }
			
		 }
	   }
	  private bool showCodeChargeSaleMinMaxPackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowCodeChargeSaleMinMaxPackages  
	   {
	    
	     get
		{
		   return showCodeChargeSaleMinMaxPackages;
		 }
		 set
		 {
		   if(showCodeChargeSaleMinMaxPackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowCodeChargeSaleMinMaxPackages",OldValue=showCodeChargeSaleMinMaxPackages,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showCodeChargeSaleMinMaxPackages=value;
		   }
			
		 }
	   }
	  private int quoteTemplatePDFMarginLeft ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int QuoteTemplatePDFMarginLeft  
	   {
	    
	     get
		{
		   return quoteTemplatePDFMarginLeft;
		 }
		 set
		 {
		   if(quoteTemplatePDFMarginLeft != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteTemplatePDFMarginLeft",OldValue=quoteTemplatePDFMarginLeft,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   quoteTemplatePDFMarginLeft=value;
		   }
			
		 }
	   }
	  private int quoteTemplatePDFMarginRight ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int QuoteTemplatePDFMarginRight  
	   {
	    
	     get
		{
		   return quoteTemplatePDFMarginRight;
		 }
		 set
		 {
		   if(quoteTemplatePDFMarginRight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteTemplatePDFMarginRight",OldValue=quoteTemplatePDFMarginRight,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   quoteTemplatePDFMarginRight=value;
		   }
			
		 }
	   }
	  private string pageHeaderArea1Type ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageHeaderArea1Type  
	   {
	    
	     get
		{
		   return pageHeaderArea1Type;
		 }
		 set
		 {
		   if(pageHeaderArea1Type != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderArea1Type",OldValue=pageHeaderArea1Type,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageHeaderArea1Type=value;
		   }
			
		 }
	   }
	  private string pageHeaderArea2Type ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageHeaderArea2Type  
	   {
	    
	     get
		{
		   return pageHeaderArea2Type;
		 }
		 set
		 {
		   if(pageHeaderArea2Type != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderArea2Type",OldValue=pageHeaderArea2Type,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageHeaderArea2Type=value;
		   }
			
		 }
	   }
	  private string pageHeaderArea3Type ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageHeaderArea3Type  
	   {
	    
	     get
		{
		   return pageHeaderArea3Type;
		 }
		 set
		 {
		   if(pageHeaderArea3Type != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderArea3Type",OldValue=pageHeaderArea3Type,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageHeaderArea3Type=value;
		   }
			
		 }
	   }
	  private string pageHeaderArea1ImageDetailId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageHeaderArea1ImageDetailId  
	   {
	    
	     get
		{
		   return pageHeaderArea1ImageDetailId;
		 }
		 set
		 {
		   if(pageHeaderArea1ImageDetailId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderArea1ImageDetailId",OldValue=pageHeaderArea1ImageDetailId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageHeaderArea1ImageDetailId=value;
		   }
			
		 }
	   }
	  private string pageHeaderArea2ImageDetailId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageHeaderArea2ImageDetailId  
	   {
	    
	     get
		{
		   return pageHeaderArea2ImageDetailId;
		 }
		 set
		 {
		   if(pageHeaderArea2ImageDetailId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderArea2ImageDetailId",OldValue=pageHeaderArea2ImageDetailId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageHeaderArea2ImageDetailId=value;
		   }
			
		 }
	   }
	  private string pageHeaderArea3ImageDetailId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageHeaderArea3ImageDetailId  
	   {
	    
	     get
		{
		   return pageHeaderArea3ImageDetailId;
		 }
		 set
		 {
		   if(pageHeaderArea3ImageDetailId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderArea3ImageDetailId",OldValue=pageHeaderArea3ImageDetailId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageHeaderArea3ImageDetailId=value;
		   }
			
		 }
	   }
	  private string pageHeaderArea1FreeText ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageHeaderArea1FreeText  
	   {
	    
	     get
		{
		   return pageHeaderArea1FreeText;
		 }
		 set
		 {
		   if(pageHeaderArea1FreeText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderArea1FreeText",OldValue=pageHeaderArea1FreeText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageHeaderArea1FreeText=value;
		   }
			
		 }
	   }
	  private string pageHeaderArea2FreeText ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageHeaderArea2FreeText  
	   {
	    
	     get
		{
		   return pageHeaderArea2FreeText;
		 }
		 set
		 {
		   if(pageHeaderArea2FreeText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderArea2FreeText",OldValue=pageHeaderArea2FreeText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageHeaderArea2FreeText=value;
		   }
			
		 }
	   }
	  private string pageHeaderArea3FreeText ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageHeaderArea3FreeText  
	   {
	    
	     get
		{
		   return pageHeaderArea3FreeText;
		 }
		 set
		 {
		   if(pageHeaderArea3FreeText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderArea3FreeText",OldValue=pageHeaderArea3FreeText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageHeaderArea3FreeText=value;
		   }
			
		 }
	   }
	  private string pageHeaderArea1FreeTextDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageHeaderArea1FreeTextDesignId  
	   {
	    
	     get
		{
		   return pageHeaderArea1FreeTextDesignId;
		 }
		 set
		 {
		   if(pageHeaderArea1FreeTextDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderArea1FreeTextDesignId",OldValue=pageHeaderArea1FreeTextDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageHeaderArea1FreeTextDesignId=value;
		   }
			
		 }
	   }
	  private string pageHeaderArea2FreeTextDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageHeaderArea2FreeTextDesignId  
	   {
	    
	     get
		{
		   return pageHeaderArea2FreeTextDesignId;
		 }
		 set
		 {
		   if(pageHeaderArea2FreeTextDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderArea2FreeTextDesignId",OldValue=pageHeaderArea2FreeTextDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageHeaderArea2FreeTextDesignId=value;
		   }
			
		 }
	   }
	  private string pageHeaderArea3FreeTextDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageHeaderArea3FreeTextDesignId  
	   {
	    
	     get
		{
		   return pageHeaderArea3FreeTextDesignId;
		 }
		 set
		 {
		   if(pageHeaderArea3FreeTextDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderArea3FreeTextDesignId",OldValue=pageHeaderArea3FreeTextDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageHeaderArea3FreeTextDesignId=value;
		   }
			
		 }
	   }
	  private double pageHeaderArea1Width ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double PageHeaderArea1Width  
	   {
	    
	     get
		{
		   return pageHeaderArea1Width;
		 }
		 set
		 {
		   if(pageHeaderArea1Width != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderArea1Width",OldValue=pageHeaderArea1Width,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   pageHeaderArea1Width=value;
		   }
			
		 }
	   }
	  private double pageHeaderArea2Width ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double PageHeaderArea2Width  
	   {
	    
	     get
		{
		   return pageHeaderArea2Width;
		 }
		 set
		 {
		   if(pageHeaderArea2Width != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderArea2Width",OldValue=pageHeaderArea2Width,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   pageHeaderArea2Width=value;
		   }
			
		 }
	   }
	  private double pageHeaderArea3Width ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double PageHeaderArea3Width  
	   {
	    
	     get
		{
		   return pageHeaderArea3Width;
		 }
		 set
		 {
		   if(pageHeaderArea3Width != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderArea3Width",OldValue=pageHeaderArea3Width,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   pageHeaderArea3Width=value;
		   }
			
		 }
	   }
	  private int pageHeaderImage1Width ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int PageHeaderImage1Width  
	   {
	    
	     get
		{
		   return pageHeaderImage1Width;
		 }
		 set
		 {
		   if(pageHeaderImage1Width != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderImage1Width",OldValue=pageHeaderImage1Width,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   pageHeaderImage1Width=value;
		   }
			
		 }
	   }
	  private int pageHeaderImage2Width ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int PageHeaderImage2Width  
	   {
	    
	     get
		{
		   return pageHeaderImage2Width;
		 }
		 set
		 {
		   if(pageHeaderImage2Width != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderImage2Width",OldValue=pageHeaderImage2Width,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   pageHeaderImage2Width=value;
		   }
			
		 }
	   }
	  private int pageHeaderImage3Width ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int PageHeaderImage3Width  
	   {
	    
	     get
		{
		   return pageHeaderImage3Width;
		 }
		 set
		 {
		   if(pageHeaderImage3Width != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderImage3Width",OldValue=pageHeaderImage3Width,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   pageHeaderImage3Width=value;
		   }
			
		 }
	   }
	  private int pageFooterImage1Width ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int PageFooterImage1Width  
	   {
	    
	     get
		{
		   return pageFooterImage1Width;
		 }
		 set
		 {
		   if(pageFooterImage1Width != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterImage1Width",OldValue=pageFooterImage1Width,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   pageFooterImage1Width=value;
		   }
			
		 }
	   }
	  private int pageFooterImage2Width ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int PageFooterImage2Width  
	   {
	    
	     get
		{
		   return pageFooterImage2Width;
		 }
		 set
		 {
		   if(pageFooterImage2Width != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterImage2Width",OldValue=pageFooterImage2Width,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   pageFooterImage2Width=value;
		   }
			
		 }
	   }
	  private int pageFooterImage3Width ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int PageFooterImage3Width  
	   {
	    
	     get
		{
		   return pageFooterImage3Width;
		 }
		 set
		 {
		   if(pageFooterImage3Width != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterImage3Width",OldValue=pageFooterImage3Width,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   pageFooterImage3Width=value;
		   }
			
		 }
	   }
	  private int pageHeaderAreaHeight ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int PageHeaderAreaHeight  
	   {
	    
	     get
		{
		   return pageHeaderAreaHeight;
		 }
		 set
		 {
		   if(pageHeaderAreaHeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderAreaHeight",OldValue=pageHeaderAreaHeight,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   pageHeaderAreaHeight=value;
		   }
			
		 }
	   }
	  private int pageFooterAreaHeight ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int PageFooterAreaHeight  
	   {
	    
	     get
		{
		   return pageFooterAreaHeight;
		 }
		 set
		 {
		   if(pageFooterAreaHeight != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterAreaHeight",OldValue=pageFooterAreaHeight,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   pageFooterAreaHeight=value;
		   }
			
		 }
	   }
	  private int pageHeaderArea1Height ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int PageHeaderArea1Height  
	   {
	    
	     get
		{
		   return pageHeaderArea1Height;
		 }
		 set
		 {
		   if(pageHeaderArea1Height != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderArea1Height",OldValue=pageHeaderArea1Height,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   pageHeaderArea1Height=value;
		   }
			
		 }
	   }
	  private int pageHeaderArea2Height ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int PageHeaderArea2Height  
	   {
	    
	     get
		{
		   return pageHeaderArea2Height;
		 }
		 set
		 {
		   if(pageHeaderArea2Height != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderArea2Height",OldValue=pageHeaderArea2Height,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   pageHeaderArea2Height=value;
		   }
			
		 }
	   }
	  private int pageHeaderArea3Height ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int PageHeaderArea3Height  
	   {
	    
	     get
		{
		   return pageHeaderArea3Height;
		 }
		 set
		 {
		   if(pageHeaderArea3Height != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderArea3Height",OldValue=pageHeaderArea3Height,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   pageHeaderArea3Height=value;
		   }
			
		 }
	   }
	  private string pageHeaderArea1ImageAlignment ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageHeaderArea1ImageAlignment  
	   {
	    
	     get
		{
		   return pageHeaderArea1ImageAlignment;
		 }
		 set
		 {
		   if(pageHeaderArea1ImageAlignment != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderArea1ImageAlignment",OldValue=pageHeaderArea1ImageAlignment,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageHeaderArea1ImageAlignment=value;
		   }
			
		 }
	   }
	  private string pageHeaderArea2ImageAlignment ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageHeaderArea2ImageAlignment  
	   {
	    
	     get
		{
		   return pageHeaderArea2ImageAlignment;
		 }
		 set
		 {
		   if(pageHeaderArea2ImageAlignment != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderArea2ImageAlignment",OldValue=pageHeaderArea2ImageAlignment,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageHeaderArea2ImageAlignment=value;
		   }
			
		 }
	   }
	  private string pageHeaderArea3ImageAlignment ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageHeaderArea3ImageAlignment  
	   {
	    
	     get
		{
		   return pageHeaderArea3ImageAlignment;
		 }
		 set
		 {
		   if(pageHeaderArea3ImageAlignment != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderArea3ImageAlignment",OldValue=pageHeaderArea3ImageAlignment,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageHeaderArea3ImageAlignment=value;
		   }
			
		 }
	   }
	  private string pageFooterArea1Type ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageFooterArea1Type  
	   {
	    
	     get
		{
		   return pageFooterArea1Type;
		 }
		 set
		 {
		   if(pageFooterArea1Type != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterArea1Type",OldValue=pageFooterArea1Type,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageFooterArea1Type=value;
		   }
			
		 }
	   }
	  private string pageFooterArea2Type ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageFooterArea2Type  
	   {
	    
	     get
		{
		   return pageFooterArea2Type;
		 }
		 set
		 {
		   if(pageFooterArea2Type != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterArea2Type",OldValue=pageFooterArea2Type,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageFooterArea2Type=value;
		   }
			
		 }
	   }
	  private string pageFooterArea3Type ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageFooterArea3Type  
	   {
	    
	     get
		{
		   return pageFooterArea3Type;
		 }
		 set
		 {
		   if(pageFooterArea3Type != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterArea3Type",OldValue=pageFooterArea3Type,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageFooterArea3Type=value;
		   }
			
		 }
	   }
	  private string pageFooterArea1ImageDetailId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageFooterArea1ImageDetailId  
	   {
	    
	     get
		{
		   return pageFooterArea1ImageDetailId;
		 }
		 set
		 {
		   if(pageFooterArea1ImageDetailId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterArea1ImageDetailId",OldValue=pageFooterArea1ImageDetailId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageFooterArea1ImageDetailId=value;
		   }
			
		 }
	   }
	  private string pageFooterArea2ImageDetailId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageFooterArea2ImageDetailId  
	   {
	    
	     get
		{
		   return pageFooterArea2ImageDetailId;
		 }
		 set
		 {
		   if(pageFooterArea2ImageDetailId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterArea2ImageDetailId",OldValue=pageFooterArea2ImageDetailId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageFooterArea2ImageDetailId=value;
		   }
			
		 }
	   }
	  private string pageFooterArea3ImageDetailId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageFooterArea3ImageDetailId  
	   {
	    
	     get
		{
		   return pageFooterArea3ImageDetailId;
		 }
		 set
		 {
		   if(pageFooterArea3ImageDetailId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterArea3ImageDetailId",OldValue=pageFooterArea3ImageDetailId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageFooterArea3ImageDetailId=value;
		   }
			
		 }
	   }
	  private string pageFooterArea1FreeText ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageFooterArea1FreeText  
	   {
	    
	     get
		{
		   return pageFooterArea1FreeText;
		 }
		 set
		 {
		   if(pageFooterArea1FreeText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterArea1FreeText",OldValue=pageFooterArea1FreeText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageFooterArea1FreeText=value;
		   }
			
		 }
	   }
	  private string pageFooterArea2FreeText ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageFooterArea2FreeText  
	   {
	    
	     get
		{
		   return pageFooterArea2FreeText;
		 }
		 set
		 {
		   if(pageFooterArea2FreeText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterArea2FreeText",OldValue=pageFooterArea2FreeText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageFooterArea2FreeText=value;
		   }
			
		 }
	   }
	  private string pageFooterArea3FreeText ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageFooterArea3FreeText  
	   {
	    
	     get
		{
		   return pageFooterArea3FreeText;
		 }
		 set
		 {
		   if(pageFooterArea3FreeText != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterArea3FreeText",OldValue=pageFooterArea3FreeText,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageFooterArea3FreeText=value;
		   }
			
		 }
	   }
	  private string pageFooterArea1FreeTextDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageFooterArea1FreeTextDesignId  
	   {
	    
	     get
		{
		   return pageFooterArea1FreeTextDesignId;
		 }
		 set
		 {
		   if(pageFooterArea1FreeTextDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterArea1FreeTextDesignId",OldValue=pageFooterArea1FreeTextDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageFooterArea1FreeTextDesignId=value;
		   }
			
		 }
	   }
	  private string pageFooterArea2FreeTextDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageFooterArea2FreeTextDesignId  
	   {
	    
	     get
		{
		   return pageFooterArea2FreeTextDesignId;
		 }
		 set
		 {
		   if(pageFooterArea2FreeTextDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterArea2FreeTextDesignId",OldValue=pageFooterArea2FreeTextDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageFooterArea2FreeTextDesignId=value;
		   }
			
		 }
	   }
	  private string pageFooterArea3FreeTextDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageFooterArea3FreeTextDesignId  
	   {
	    
	     get
		{
		   return pageFooterArea3FreeTextDesignId;
		 }
		 set
		 {
		   if(pageFooterArea3FreeTextDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterArea3FreeTextDesignId",OldValue=pageFooterArea3FreeTextDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageFooterArea3FreeTextDesignId=value;
		   }
			
		 }
	   }
	  private double pageFooterArea1Width ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double PageFooterArea1Width  
	   {
	    
	     get
		{
		   return pageFooterArea1Width;
		 }
		 set
		 {
		   if(pageFooterArea1Width != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterArea1Width",OldValue=pageFooterArea1Width,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   pageFooterArea1Width=value;
		   }
			
		 }
	   }
	  private double pageFooterArea2Width ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double PageFooterArea2Width  
	   {
	    
	     get
		{
		   return pageFooterArea2Width;
		 }
		 set
		 {
		   if(pageFooterArea2Width != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterArea2Width",OldValue=pageFooterArea2Width,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   pageFooterArea2Width=value;
		   }
			
		 }
	   }
	  private double pageFooterArea3Width ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double PageFooterArea3Width  
	   {
	    
	     get
		{
		   return pageFooterArea3Width;
		 }
		 set
		 {
		   if(pageFooterArea3Width != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterArea3Width",OldValue=pageFooterArea3Width,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   pageFooterArea3Width=value;
		   }
			
		 }
	   }
	  private int pageFooterArea1Height ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int PageFooterArea1Height  
	   {
	    
	     get
		{
		   return pageFooterArea1Height;
		 }
		 set
		 {
		   if(pageFooterArea1Height != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterArea1Height",OldValue=pageFooterArea1Height,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   pageFooterArea1Height=value;
		   }
			
		 }
	   }
	  private int pageFooterArea2Height ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int PageFooterArea2Height  
	   {
	    
	     get
		{
		   return pageFooterArea2Height;
		 }
		 set
		 {
		   if(pageFooterArea2Height != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterArea2Height",OldValue=pageFooterArea2Height,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   pageFooterArea2Height=value;
		   }
			
		 }
	   }
	  private int pageFooterArea3Height ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int PageFooterArea3Height  
	   {
	    
	     get
		{
		   return pageFooterArea3Height;
		 }
		 set
		 {
		   if(pageFooterArea3Height != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterArea3Height",OldValue=pageFooterArea3Height,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   pageFooterArea3Height=value;
		   }
			
		 }
	   }
	  private string pageFooterArea1ImageAlignment ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageFooterArea1ImageAlignment  
	   {
	    
	     get
		{
		   return pageFooterArea1ImageAlignment;
		 }
		 set
		 {
		   if(pageFooterArea1ImageAlignment != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterArea1ImageAlignment",OldValue=pageFooterArea1ImageAlignment,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageFooterArea1ImageAlignment=value;
		   }
			
		 }
	   }
	  private string pageFooterArea2ImageAlignment ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageFooterArea2ImageAlignment  
	   {
	    
	     get
		{
		   return pageFooterArea2ImageAlignment;
		 }
		 set
		 {
		   if(pageFooterArea2ImageAlignment != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterArea2ImageAlignment",OldValue=pageFooterArea2ImageAlignment,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageFooterArea2ImageAlignment=value;
		   }
			
		 }
	   }
	  private string pageFooterArea3ImageAlignment ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageFooterArea3ImageAlignment  
	   {
	    
	     get
		{
		   return pageFooterArea3ImageAlignment;
		 }
		 set
		 {
		   if(pageFooterArea3ImageAlignment != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterArea3ImageAlignment",OldValue=pageFooterArea3ImageAlignment,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageFooterArea3ImageAlignment=value;
		   }
			
		 }
	   }
	  private bool showHeaderQuoteDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowHeaderQuoteDate  
	   {
	    
	     get
		{
		   return showHeaderQuoteDate;
		 }
		 set
		 {
		   if(showHeaderQuoteDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowHeaderQuoteDate",OldValue=showHeaderQuoteDate,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showHeaderQuoteDate=value;
		   }
			
		 }
	   }
	  private bool showHeaderExpirationDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowHeaderExpirationDate  
	   {
	    
	     get
		{
		   return showHeaderExpirationDate;
		 }
		 set
		 {
		   if(showHeaderExpirationDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowHeaderExpirationDate",OldValue=showHeaderExpirationDate,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showHeaderExpirationDate=value;
		   }
			
		 }
	   }
	  private bool showHeaderQuoteNumber ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowHeaderQuoteNumber  
	   {
	    
	     get
		{
		   return showHeaderQuoteNumber;
		 }
		 set
		 {
		   if(showHeaderQuoteNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowHeaderQuoteNumber",OldValue=showHeaderQuoteNumber,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showHeaderQuoteNumber=value;
		   }
			
		 }
	   }
	  private bool showHeaderCustomer ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowHeaderCustomer  
	   {
	    
	     get
		{
		   return showHeaderCustomer;
		 }
		 set
		 {
		   if(showHeaderCustomer != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowHeaderCustomer",OldValue=showHeaderCustomer,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showHeaderCustomer=value;
		   }
			
		 }
	   }
	  private bool showDetailsExpirationDate ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsExpirationDate  
	   {
	    
	     get
		{
		   return showDetailsExpirationDate;
		 }
		 set
		 {
		   if(showDetailsExpirationDate != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsExpirationDate",OldValue=showDetailsExpirationDate,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsExpirationDate=value;
		   }
			
		 }
	   }
	  private bool showDetailsExpirationDays ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsExpirationDays  
	   {
	    
	     get
		{
		   return showDetailsExpirationDays;
		 }
		 set
		 {
		   if(showDetailsExpirationDays != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsExpirationDays",OldValue=showDetailsExpirationDays,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsExpirationDays=value;
		   }
			
		 }
	   }
	  private bool showDetailsShipperName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsShipperName  
	   {
	    
	     get
		{
		   return showDetailsShipperName;
		 }
		 set
		 {
		   if(showDetailsShipperName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsShipperName",OldValue=showDetailsShipperName,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsShipperName=value;
		   }
			
		 }
	   }
	  private bool showDetailsShipperAddress ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsShipperAddress  
	   {
	    
	     get
		{
		   return showDetailsShipperAddress;
		 }
		 set
		 {
		   if(showDetailsShipperAddress != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsShipperAddress",OldValue=showDetailsShipperAddress,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsShipperAddress=value;
		   }
			
		 }
	   }
	  private bool showDetailsShipperContact ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsShipperContact  
	   {
	    
	     get
		{
		   return showDetailsShipperContact;
		 }
		 set
		 {
		   if(showDetailsShipperContact != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsShipperContact",OldValue=showDetailsShipperContact,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsShipperContact=value;
		   }
			
		 }
	   }
	  private bool showDetailsShipperReferences ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsShipperReferences  
	   {
	    
	     get
		{
		   return showDetailsShipperReferences;
		 }
		 set
		 {
		   if(showDetailsShipperReferences != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsShipperReferences",OldValue=showDetailsShipperReferences,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsShipperReferences=value;
		   }
			
		 }
	   }
	  private bool showDetailsConsigneeName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsConsigneeName  
	   {
	    
	     get
		{
		   return showDetailsConsigneeName;
		 }
		 set
		 {
		   if(showDetailsConsigneeName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsConsigneeName",OldValue=showDetailsConsigneeName,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsConsigneeName=value;
		   }
			
		 }
	   }
	  private bool showDetailsConsigneeAddress ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsConsigneeAddress  
	   {
	    
	     get
		{
		   return showDetailsConsigneeAddress;
		 }
		 set
		 {
		   if(showDetailsConsigneeAddress != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsConsigneeAddress",OldValue=showDetailsConsigneeAddress,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsConsigneeAddress=value;
		   }
			
		 }
	   }
	  private bool showDetailsConsigneeContact ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsConsigneeContact  
	   {
	    
	     get
		{
		   return showDetailsConsigneeContact;
		 }
		 set
		 {
		   if(showDetailsConsigneeContact != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsConsigneeContact",OldValue=showDetailsConsigneeContact,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsConsigneeContact=value;
		   }
			
		 }
	   }
	  private bool showDetailsConsigneeReferences ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsConsigneeReferences  
	   {
	    
	     get
		{
		   return showDetailsConsigneeReferences;
		 }
		 set
		 {
		   if(showDetailsConsigneeReferences != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsConsigneeReferences",OldValue=showDetailsConsigneeReferences,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsConsigneeReferences=value;
		   }
			
		 }
	   }
	  private bool showDetailsPickupFrom ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsPickupFrom  
	   {
	    
	     get
		{
		   return showDetailsPickupFrom;
		 }
		 set
		 {
		   if(showDetailsPickupFrom != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsPickupFrom",OldValue=showDetailsPickupFrom,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsPickupFrom=value;
		   }
			
		 }
	   }
	  private bool showDetailsDeliveryTo ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsDeliveryTo  
	   {
	    
	     get
		{
		   return showDetailsDeliveryTo;
		 }
		 set
		 {
		   if(showDetailsDeliveryTo != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsDeliveryTo",OldValue=showDetailsDeliveryTo,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsDeliveryTo=value;
		   }
			
		 }
	   }
	  private bool showDetailsFromPort ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsFromPort  
	   {
	    
	     get
		{
		   return showDetailsFromPort;
		 }
		 set
		 {
		   if(showDetailsFromPort != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsFromPort",OldValue=showDetailsFromPort,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsFromPort=value;
		   }
			
		 }
	   }
	  private bool showDetailsToPort ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsToPort  
	   {
	    
	     get
		{
		   return showDetailsToPort;
		 }
		 set
		 {
		   if(showDetailsToPort != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsToPort",OldValue=showDetailsToPort,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsToPort=value;
		   }
			
		 }
	   }
	  private bool showDetailsIncoterms ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsIncoterms  
	   {
	    
	     get
		{
		   return showDetailsIncoterms;
		 }
		 set
		 {
		   if(showDetailsIncoterms != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsIncoterms",OldValue=showDetailsIncoterms,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsIncoterms=value;
		   }
			
		 }
	   }
	  private bool showDetailsService ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsService  
	   {
	    
	     get
		{
		   return showDetailsService;
		 }
		 set
		 {
		   if(showDetailsService != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsService",OldValue=showDetailsService,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsService=value;
		   }
			
		 }
	   }
	  private bool showDetailsSalesMan ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsSalesMan  
	   {
	    
	     get
		{
		   return showDetailsSalesMan;
		 }
		 set
		 {
		   if(showDetailsSalesMan != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsSalesMan",OldValue=showDetailsSalesMan,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsSalesMan=value;
		   }
			
		 }
	   }
	  private bool showDetailsDescriptionOfGoods ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsDescriptionOfGoods  
	   {
	    
	     get
		{
		   return showDetailsDescriptionOfGoods;
		 }
		 set
		 {
		   if(showDetailsDescriptionOfGoods != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsDescriptionOfGoods",OldValue=showDetailsDescriptionOfGoods,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsDescriptionOfGoods=value;
		   }
			
		 }
	   }
	  private bool showDetailsDangerousGoods ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsDangerousGoods  
	   {
	    
	     get
		{
		   return showDetailsDangerousGoods;
		 }
		 set
		 {
		   if(showDetailsDangerousGoods != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsDangerousGoods",OldValue=showDetailsDangerousGoods,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsDangerousGoods=value;
		   }
			
		 }
	   }
	  private bool showDetailsCarrier ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsCarrier  
	   {
	    
	     get
		{
		   return showDetailsCarrier;
		 }
		 set
		 {
		   if(showDetailsCarrier != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsCarrier",OldValue=showDetailsCarrier,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsCarrier=value;
		   }
			
		 }
	   }
	  private bool showDetailsCustomerName ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsCustomerName  
	   {
	    
	     get
		{
		   return showDetailsCustomerName;
		 }
		 set
		 {
		   if(showDetailsCustomerName != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsCustomerName",OldValue=showDetailsCustomerName,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsCustomerName=value;
		   }
			
		 }
	   }
	  private bool showDetailsCustomerAddress ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsCustomerAddress  
	   {
	    
	     get
		{
		   return showDetailsCustomerAddress;
		 }
		 set
		 {
		   if(showDetailsCustomerAddress != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsCustomerAddress",OldValue=showDetailsCustomerAddress,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsCustomerAddress=value;
		   }
			
		 }
	   }
	  private bool showDetailsCustomerContact ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsCustomerContact  
	   {
	    
	     get
		{
		   return showDetailsCustomerContact;
		 }
		 set
		 {
		   if(showDetailsCustomerContact != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsCustomerContact",OldValue=showDetailsCustomerContact,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsCustomerContact=value;
		   }
			
		 }
	   }
	  private bool showDetailsCustomerReferences ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowDetailsCustomerReferences  
	   {
	    
	     get
		{
		   return showDetailsCustomerReferences;
		 }
		 set
		 {
		   if(showDetailsCustomerReferences != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowDetailsCustomerReferences",OldValue=showDetailsCustomerReferences,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showDetailsCustomerReferences=value;
		   }
			
		 }
	   }
	  private bool showTitleQuoteDetails ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowTitleQuoteDetails  
	   {
	    
	     get
		{
		   return showTitleQuoteDetails;
		 }
		 set
		 {
		   if(showTitleQuoteDetails != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowTitleQuoteDetails",OldValue=showTitleQuoteDetails,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showTitleQuoteDetails=value;
		   }
			
		 }
	   }
	  private bool showTitlePricingPackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowTitlePricingPackages  
	   {
	    
	     get
		{
		   return showTitlePricingPackages;
		 }
		 set
		 {
		   if(showTitlePricingPackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowTitlePricingPackages",OldValue=showTitlePricingPackages,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showTitlePricingPackages=value;
		   }
			
		 }
	   }
	  private bool showTitlePricingContainsers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowTitlePricingContainsers  
	   {
	    
	     get
		{
		   return showTitlePricingContainsers;
		 }
		 set
		 {
		   if(showTitlePricingContainsers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowTitlePricingContainsers",OldValue=showTitlePricingContainsers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showTitlePricingContainsers=value;
		   }
			
		 }
	   }
	  private string packagesTableDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PackagesTableDesignId  
	   {
	    
	     get
		{
		   return packagesTableDesignId;
		 }
		 set
		 {
		   if(packagesTableDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PackagesTableDesignId",OldValue=packagesTableDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   packagesTableDesignId=value;
		   }
			
		 }
	   }
	  private string containserTableDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string ContainserTableDesignId  
	   {
	    
	     get
		{
		   return containserTableDesignId;
		 }
		 set
		 {
		   if(containserTableDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ContainserTableDesignId",OldValue=containserTableDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   containserTableDesignId=value;
		   }
			
		 }
	   }
	  private string totalsPackagesLabelDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TotalsPackagesLabelDesignId  
	   {
	    
	     get
		{
		   return totalsPackagesLabelDesignId;
		 }
		 set
		 {
		   if(totalsPackagesLabelDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalsPackagesLabelDesignId",OldValue=totalsPackagesLabelDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   totalsPackagesLabelDesignId=value;
		   }
			
		 }
	   }
	  private string totalsContainsersLabelDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TotalsContainsersLabelDesignId  
	   {
	    
	     get
		{
		   return totalsContainsersLabelDesignId;
		 }
		 set
		 {
		   if(totalsContainsersLabelDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalsContainsersLabelDesignId",OldValue=totalsContainsersLabelDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   totalsContainsersLabelDesignId=value;
		   }
			
		 }
	   }
	  private string totalsPackagesValueDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TotalsPackagesValueDesignId  
	   {
	    
	     get
		{
		   return totalsPackagesValueDesignId;
		 }
		 set
		 {
		   if(totalsPackagesValueDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalsPackagesValueDesignId",OldValue=totalsPackagesValueDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   totalsPackagesValueDesignId=value;
		   }
			
		 }
	   }
	  private string totalsContainsersValueDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TotalsContainsersValueDesignId  
	   {
	    
	     get
		{
		   return totalsContainsersValueDesignId;
		 }
		 set
		 {
		   if(totalsContainsersValueDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalsContainsersValueDesignId",OldValue=totalsContainsersValueDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   totalsContainsersValueDesignId=value;
		   }
			
		 }
	   }
	  private bool rightToLeft ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool RightToLeft  
	   {
	    
	     get
		{
		   return rightToLeft;
		 }
		 set
		 {
		   if(rightToLeft != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="RightToLeft",OldValue=rightToLeft,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   rightToLeft=value;
		   }
			
		 }
	   }
	  private string groupByPackagesLabelDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string GroupByPackagesLabelDesignId  
	   {
	    
	     get
		{
		   return groupByPackagesLabelDesignId;
		 }
		 set
		 {
		   if(groupByPackagesLabelDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GroupByPackagesLabelDesignId",OldValue=groupByPackagesLabelDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   groupByPackagesLabelDesignId=value;
		   }
			
		 }
	   }
	  private string groupByPackagesValueDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string GroupByPackagesValueDesignId  
	   {
	    
	     get
		{
		   return groupByPackagesValueDesignId;
		 }
		 set
		 {
		   if(groupByPackagesValueDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GroupByPackagesValueDesignId",OldValue=groupByPackagesValueDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   groupByPackagesValueDesignId=value;
		   }
			
		 }
	   }
	  private string groupByContainsersLabelDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string GroupByContainsersLabelDesignId  
	   {
	    
	     get
		{
		   return groupByContainsersLabelDesignId;
		 }
		 set
		 {
		   if(groupByContainsersLabelDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GroupByContainsersLabelDesignId",OldValue=groupByContainsersLabelDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   groupByContainsersLabelDesignId=value;
		   }
			
		 }
	   }
	  private string groupByContainsersValueDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string GroupByContainsersValueDesignId  
	   {
	    
	     get
		{
		   return groupByContainsersValueDesignId;
		 }
		 set
		 {
		   if(groupByContainsersValueDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="GroupByContainsersValueDesignId",OldValue=groupByContainsersValueDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   groupByContainsersValueDesignId=value;
		   }
			
		 }
	   }
	  private string detailsTableDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string DetailsTableDesignId  
	   {
	    
	     get
		{
		   return detailsTableDesignId;
		 }
		 set
		 {
		   if(detailsTableDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DetailsTableDesignId",OldValue=detailsTableDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   detailsTableDesignId=value;
		   }
			
		 }
	   }
	  private bool detailsSectionHasTwoColumns ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool DetailsSectionHasTwoColumns  
	   {
	    
	     get
		{
		   return detailsSectionHasTwoColumns;
		 }
		 set
		 {
		   if(detailsSectionHasTwoColumns != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DetailsSectionHasTwoColumns",OldValue=detailsSectionHasTwoColumns,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   detailsSectionHasTwoColumns=value;
		   }
			
		 }
	   }
	  private string headerTableDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string HeaderTableDesignId  
	   {
	    
	     get
		{
		   return headerTableDesignId;
		 }
		 set
		 {
		   if(headerTableDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HeaderTableDesignId",OldValue=headerTableDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   headerTableDesignId=value;
		   }
			
		 }
	   }
	  private bool headerSectionHasTwoColumns ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool HeaderSectionHasTwoColumns  
	   {
	    
	     get
		{
		   return headerSectionHasTwoColumns;
		 }
		 set
		 {
		   if(headerSectionHasTwoColumns != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HeaderSectionHasTwoColumns",OldValue=headerSectionHasTwoColumns,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   headerSectionHasTwoColumns=value;
		   }
			
		 }
	   }
	  private string detailsTitleDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string DetailsTitleDesignId  
	   {
	    
	     get
		{
		   return detailsTitleDesignId;
		 }
		 set
		 {
		   if(detailsTitleDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DetailsTitleDesignId",OldValue=detailsTitleDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   detailsTitleDesignId=value;
		   }
			
		 }
	   }
	  private string pricingPackagesTitleDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PricingPackagesTitleDesignId  
	   {
	    
	     get
		{
		   return pricingPackagesTitleDesignId;
		 }
		 set
		 {
		   if(pricingPackagesTitleDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PricingPackagesTitleDesignId",OldValue=pricingPackagesTitleDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pricingPackagesTitleDesignId=value;
		   }
			
		 }
	   }
	  private string pricingContainsersTitleDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PricingContainsersTitleDesignId  
	   {
	    
	     get
		{
		   return pricingContainsersTitleDesignId;
		 }
		 set
		 {
		   if(pricingContainsersTitleDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PricingContainsersTitleDesignId",OldValue=pricingContainsersTitleDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pricingContainsersTitleDesignId=value;
		   }
			
		 }
	   }
	  private string pageHeaderBorderTypeCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageHeaderBorderTypeCode  
	   {
	    
	     get
		{
		   return pageHeaderBorderTypeCode;
		 }
		 set
		 {
		   if(pageHeaderBorderTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderBorderTypeCode",OldValue=pageHeaderBorderTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageHeaderBorderTypeCode=value;
		   }
			
		 }
	   }
	  private string pageHeaderBorderColor ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageHeaderBorderColor  
	   {
	    
	     get
		{
		   return pageHeaderBorderColor;
		 }
		 set
		 {
		   if(pageHeaderBorderColor != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderBorderColor",OldValue=pageHeaderBorderColor,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageHeaderBorderColor=value;
		   }
			
		 }
	   }
	  private int pageHeaderBorderThickness ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int PageHeaderBorderThickness  
	   {
	    
	     get
		{
		   return pageHeaderBorderThickness;
		 }
		 set
		 {
		   if(pageHeaderBorderThickness != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageHeaderBorderThickness",OldValue=pageHeaderBorderThickness,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   pageHeaderBorderThickness=value;
		   }
			
		 }
	   }
	  private string pageFooterBorderTypeCode ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageFooterBorderTypeCode  
	   {
	    
	     get
		{
		   return pageFooterBorderTypeCode;
		 }
		 set
		 {
		   if(pageFooterBorderTypeCode != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterBorderTypeCode",OldValue=pageFooterBorderTypeCode,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageFooterBorderTypeCode=value;
		   }
			
		 }
	   }
	  private string pageFooterBorderColor ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageFooterBorderColor  
	   {
	    
	     get
		{
		   return pageFooterBorderColor;
		 }
		 set
		 {
		   if(pageFooterBorderColor != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterBorderColor",OldValue=pageFooterBorderColor,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageFooterBorderColor=value;
		   }
			
		 }
	   }
	  private int pageFooterBorderThickness ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int PageFooterBorderThickness  
	   {
	    
	     get
		{
		   return pageFooterBorderThickness;
		 }
		 set
		 {
		   if(pageFooterBorderThickness != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageFooterBorderThickness",OldValue=pageFooterBorderThickness,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   pageFooterBorderThickness=value;
		   }
			
		 }
	   }
	  private string headerTableColumWidthType ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string HeaderTableColumWidthType  
	   {
	    
	     get
		{
		   return headerTableColumWidthType;
		 }
		 set
		 {
		   if(headerTableColumWidthType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HeaderTableColumWidthType",OldValue=headerTableColumWidthType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   headerTableColumWidthType=value;
		   }
			
		 }
	   }
	  private string detailsTableColumWidthType ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string DetailsTableColumWidthType  
	   {
	    
	     get
		{
		   return detailsTableColumWidthType;
		 }
		 set
		 {
		   if(detailsTableColumWidthType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DetailsTableColumWidthType",OldValue=detailsTableColumWidthType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   detailsTableColumWidthType=value;
		   }
			
		 }
	   }
	  private double detailsTableColumn1LabelWidth ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double DetailsTableColumn1LabelWidth  
	   {
	    
	     get
		{
		   return detailsTableColumn1LabelWidth;
		 }
		 set
		 {
		   if(detailsTableColumn1LabelWidth != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DetailsTableColumn1LabelWidth",OldValue=detailsTableColumn1LabelWidth,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   detailsTableColumn1LabelWidth=value;
		   }
			
		 }
	   }
	  private double detailsTableColumn1ValueWidth ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double DetailsTableColumn1ValueWidth  
	   {
	    
	     get
		{
		   return detailsTableColumn1ValueWidth;
		 }
		 set
		 {
		   if(detailsTableColumn1ValueWidth != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DetailsTableColumn1ValueWidth",OldValue=detailsTableColumn1ValueWidth,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   detailsTableColumn1ValueWidth=value;
		   }
			
		 }
	   }
	  private double detailsTableColumn2LabelWidth ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double DetailsTableColumn2LabelWidth  
	   {
	    
	     get
		{
		   return detailsTableColumn2LabelWidth;
		 }
		 set
		 {
		   if(detailsTableColumn2LabelWidth != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DetailsTableColumn2LabelWidth",OldValue=detailsTableColumn2LabelWidth,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   detailsTableColumn2LabelWidth=value;
		   }
			
		 }
	   }
	  private double detailsTableColumn2ValueWidth ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double DetailsTableColumn2ValueWidth  
	   {
	    
	     get
		{
		   return detailsTableColumn2ValueWidth;
		 }
		 set
		 {
		   if(detailsTableColumn2ValueWidth != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="DetailsTableColumn2ValueWidth",OldValue=detailsTableColumn2ValueWidth,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   detailsTableColumn2ValueWidth=value;
		   }
			
		 }
	   }
	  private double headerTableColumn1LabelWidth ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double HeaderTableColumn1LabelWidth  
	   {
	    
	     get
		{
		   return headerTableColumn1LabelWidth;
		 }
		 set
		 {
		   if(headerTableColumn1LabelWidth != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HeaderTableColumn1LabelWidth",OldValue=headerTableColumn1LabelWidth,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   headerTableColumn1LabelWidth=value;
		   }
			
		 }
	   }
	  private double headerTableColumn1ValueWidth ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double HeaderTableColumn1ValueWidth  
	   {
	    
	     get
		{
		   return headerTableColumn1ValueWidth;
		 }
		 set
		 {
		   if(headerTableColumn1ValueWidth != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HeaderTableColumn1ValueWidth",OldValue=headerTableColumn1ValueWidth,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   headerTableColumn1ValueWidth=value;
		   }
			
		 }
	   }
	  private double headerTableColumn2LabelWidth ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double HeaderTableColumn2LabelWidth  
	   {
	    
	     get
		{
		   return headerTableColumn2LabelWidth;
		 }
		 set
		 {
		   if(headerTableColumn2LabelWidth != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HeaderTableColumn2LabelWidth",OldValue=headerTableColumn2LabelWidth,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   headerTableColumn2LabelWidth=value;
		   }
			
		 }
	   }
	  private double headerTableColumn2ValueWidth ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public double HeaderTableColumn2ValueWidth  
	   {
	    
	     get
		{
		   return headerTableColumn2ValueWidth;
		 }
		 set
		 {
		   if(headerTableColumn2ValueWidth != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HeaderTableColumn2ValueWidth",OldValue=headerTableColumn2ValueWidth,NewValue=value,PropertyType="double"};
		    NotifyPropertyChanged(values);
		   headerTableColumn2ValueWidth=value;
		   }
			
		 }
	   }
	  private bool showTotalPerChargeGroupPackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowTotalPerChargeGroupPackages  
	   {
	    
	     get
		{
		   return showTotalPerChargeGroupPackages;
		 }
		 set
		 {
		   if(showTotalPerChargeGroupPackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowTotalPerChargeGroupPackages",OldValue=showTotalPerChargeGroupPackages,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showTotalPerChargeGroupPackages=value;
		   }
			
		 }
	   }
	  private bool showTotalPerChargeGroupContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowTotalPerChargeGroupContainers  
	   {
	    
	     get
		{
		   return showTotalPerChargeGroupContainers;
		 }
		 set
		 {
		   if(showTotalPerChargeGroupContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowTotalPerChargeGroupContainers",OldValue=showTotalPerChargeGroupContainers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showTotalPerChargeGroupContainers=value;
		   }
			
		 }
	   }
	  private bool showPageBreakBeforeTotalPerContainersTable ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowPageBreakBeforeTotalPerContainersTable  
	   {
	    
	     get
		{
		   return showPageBreakBeforeTotalPerContainersTable;
		 }
		 set
		 {
		   if(showPageBreakBeforeTotalPerContainersTable != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowPageBreakBeforeTotalPerContainersTable",OldValue=showPageBreakBeforeTotalPerContainersTable,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showPageBreakBeforeTotalPerContainersTable=value;
		   }
			
		 }
	   }
	  private string totalPerContainersAdditionalTextDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TotalPerContainersAdditionalTextDesignId  
	   {
	    
	     get
		{
		   return totalPerContainersAdditionalTextDesignId;
		 }
		 set
		 {
		   if(totalPerContainersAdditionalTextDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalPerContainersAdditionalTextDesignId",OldValue=totalPerContainersAdditionalTextDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   totalPerContainersAdditionalTextDesignId=value;
		   }
			
		 }
	   }
	  private string totalPerContainersTableDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TotalPerContainersTableDesignId  
	   {
	    
	     get
		{
		   return totalPerContainersTableDesignId;
		 }
		 set
		 {
		   if(totalPerContainersTableDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalPerContainersTableDesignId",OldValue=totalPerContainersTableDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   totalPerContainersTableDesignId=value;
		   }
			
		 }
	   }
	  private string totalPerContainersCurrencyType ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string TotalPerContainersCurrencyType  
	   {
	    
	     get
		{
		   return totalPerContainersCurrencyType;
		 }
		 set
		 {
		   if(totalPerContainersCurrencyType != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="TotalPerContainersCurrencyType",OldValue=totalPerContainersCurrencyType,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   totalPerContainersCurrencyType=value;
		   }
			
		 }
	   }
	  private bool showTitleTotalPerContainersTable ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowTitleTotalPerContainersTable  
	   {
	    
	     get
		{
		   return showTitleTotalPerContainersTable;
		 }
		 set
		 {
		   if(showTitleTotalPerContainersTable != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowTitleTotalPerContainersTable",OldValue=showTitleTotalPerContainersTable,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showTitleTotalPerContainersTable=value;
		   }
			
		 }
	   }
	  private bool showChargeNotePackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowChargeNotePackages  
	   {
	    
	     get
		{
		   return showChargeNotePackages;
		 }
		 set
		 {
		   if(showChargeNotePackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowChargeNotePackages",OldValue=showChargeNotePackages,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showChargeNotePackages=value;
		   }
			
		 }
	   }
	  private bool showChargeNoteContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowChargeNoteContainers  
	   {
	    
	     get
		{
		   return showChargeNoteContainers;
		 }
		 set
		 {
		   if(showChargeNoteContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowChargeNoteContainers",OldValue=showChargeNoteContainers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showChargeNoteContainers=value;
		   }
			
		 }
	   }
	  private bool showSaleMaxMinAmountPackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowSaleMaxMinAmountPackages  
	   {
	    
	     get
		{
		   return showSaleMaxMinAmountPackages;
		 }
		 set
		 {
		   if(showSaleMaxMinAmountPackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowSaleMaxMinAmountPackages",OldValue=showSaleMaxMinAmountPackages,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showSaleMaxMinAmountPackages=value;
		   }
			
		 }
	   }
	  private bool showSaleMaxMinAmountContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowSaleMaxMinAmountContainers  
	   {
	    
	     get
		{
		   return showSaleMaxMinAmountContainers;
		 }
		 set
		 {
		   if(showSaleMaxMinAmountContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowSaleMaxMinAmountContainers",OldValue=showSaleMaxMinAmountContainers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showSaleMaxMinAmountContainers=value;
		   }
			
		 }
	   }
	  private bool showHeaderLabelsPackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowHeaderLabelsPackages  
	   {
	    
	     get
		{
		   return showHeaderLabelsPackages;
		 }
		 set
		 {
		   if(showHeaderLabelsPackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowHeaderLabelsPackages",OldValue=showHeaderLabelsPackages,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showHeaderLabelsPackages=value;
		   }
			
		 }
	   }
	  private bool showHeaderLabelsContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowHeaderLabelsContainers  
	   {
	    
	     get
		{
		   return showHeaderLabelsContainers;
		 }
		 set
		 {
		   if(showHeaderLabelsContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowHeaderLabelsContainers",OldValue=showHeaderLabelsContainers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showHeaderLabelsContainers=value;
		   }
			
		 }
	   }
	  private int spaceLinesBeforeContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int SpaceLinesBeforeContainers  
	   {
	    
	     get
		{
		   return spaceLinesBeforeContainers;
		 }
		 set
		 {
		   if(spaceLinesBeforeContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SpaceLinesBeforeContainers",OldValue=spaceLinesBeforeContainers,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   spaceLinesBeforeContainers=value;
		   }
			
		 }
	   }
	  private int spaceLinesBeforePackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int SpaceLinesBeforePackages  
	   {
	    
	     get
		{
		   return spaceLinesBeforePackages;
		 }
		 set
		 {
		   if(spaceLinesBeforePackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SpaceLinesBeforePackages",OldValue=spaceLinesBeforePackages,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   spaceLinesBeforePackages=value;
		   }
			
		 }
	   }
	  private int spaceLinesBeforeQuoteHeaders ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int SpaceLinesBeforeQuoteHeaders  
	   {
	    
	     get
		{
		   return spaceLinesBeforeQuoteHeaders;
		 }
		 set
		 {
		   if(spaceLinesBeforeQuoteHeaders != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SpaceLinesBeforeQuoteHeaders",OldValue=spaceLinesBeforeQuoteHeaders,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   spaceLinesBeforeQuoteHeaders=value;
		   }
			
		 }
	   }
	  private int spaceLinesBeforeQuoteDetails ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int SpaceLinesBeforeQuoteDetails  
	   {
	    
	     get
		{
		   return spaceLinesBeforeQuoteDetails;
		 }
		 set
		 {
		   if(spaceLinesBeforeQuoteDetails != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SpaceLinesBeforeQuoteDetails",OldValue=spaceLinesBeforeQuoteDetails,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   spaceLinesBeforeQuoteDetails=value;
		   }
			
		 }
	   }
	  private int spaceLinesBeforeHeaders ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int SpaceLinesBeforeHeaders  
	   {
	    
	     get
		{
		   return spaceLinesBeforeHeaders;
		 }
		 set
		 {
		   if(spaceLinesBeforeHeaders != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SpaceLinesBeforeHeaders",OldValue=spaceLinesBeforeHeaders,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   spaceLinesBeforeHeaders=value;
		   }
			
		 }
	   }
	  private int spaceLinesBeforeFooters ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int SpaceLinesBeforeFooters  
	   {
	    
	     get
		{
		   return spaceLinesBeforeFooters;
		 }
		 set
		 {
		   if(spaceLinesBeforeFooters != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SpaceLinesBeforeFooters",OldValue=spaceLinesBeforeFooters,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   spaceLinesBeforeFooters=value;
		   }
			
		 }
	   }
	  private int spaceLinesBeforePerContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int SpaceLinesBeforePerContainers  
	   {
	    
	     get
		{
		   return spaceLinesBeforePerContainers;
		 }
		 set
		 {
		   if(spaceLinesBeforePerContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="SpaceLinesBeforePerContainers",OldValue=spaceLinesBeforePerContainers,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   spaceLinesBeforePerContainers=value;
		   }
			
		 }
	   }
	  private int quoteTemplatePDFMarginTop ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int QuoteTemplatePDFMarginTop  
	   {
	    
	     get
		{
		   return quoteTemplatePDFMarginTop;
		 }
		 set
		 {
		   if(quoteTemplatePDFMarginTop != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteTemplatePDFMarginTop",OldValue=quoteTemplatePDFMarginTop,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   quoteTemplatePDFMarginTop=value;
		   }
			
		 }
	   }
	  private int quoteTemplatePDFMarginBottom ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public int QuoteTemplatePDFMarginBottom  
	   {
	    
	     get
		{
		   return quoteTemplatePDFMarginBottom;
		 }
		 set
		 {
		   if(quoteTemplatePDFMarginBottom != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="QuoteTemplatePDFMarginBottom",OldValue=quoteTemplatePDFMarginBottom,NewValue=value,PropertyType="int"};
		    NotifyPropertyChanged(values);
		   quoteTemplatePDFMarginBottom=value;
		   }
			
		 }
	   }
	  private bool showIncludedChargesPerContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowIncludedChargesPerContainers  
	   {
	    
	     get
		{
		   return showIncludedChargesPerContainers;
		 }
		 set
		 {
		   if(showIncludedChargesPerContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowIncludedChargesPerContainers",OldValue=showIncludedChargesPerContainers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showIncludedChargesPerContainers=value;
		   }
			
		 }
	   }
	  private bool showIncludedChargesPackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowIncludedChargesPackages  
	   {
	    
	     get
		{
		   return showIncludedChargesPackages;
		 }
		 set
		 {
		   if(showIncludedChargesPackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowIncludedChargesPackages",OldValue=showIncludedChargesPackages,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showIncludedChargesPackages=value;
		   }
			
		 }
	   }
	  private bool showIncludedChargesContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowIncludedChargesContainers  
	   {
	    
	     get
		{
		   return showIncludedChargesContainers;
		 }
		 set
		 {
		   if(showIncludedChargesContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowIncludedChargesContainers",OldValue=showIncludedChargesContainers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showIncludedChargesContainers=value;
		   }
			
		 }
	   }
	  private bool showVATTypePackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowVATTypePackages  
	   {
	    
	     get
		{
		   return showVATTypePackages;
		 }
		 set
		 {
		   if(showVATTypePackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowVATTypePackages",OldValue=showVATTypePackages,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showVATTypePackages=value;
		   }
			
		 }
	   }
	  private bool showVATTypeContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowVATTypeContainers  
	   {
	    
	     get
		{
		   return showVATTypeContainers;
		 }
		 set
		 {
		   if(showVATTypeContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowVATTypeContainers",OldValue=showVATTypeContainers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showVATTypeContainers=value;
		   }
			
		 }
	   }
	  private bool showVATPercentagePackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowVATPercentagePackages  
	   {
	    
	     get
		{
		   return showVATPercentagePackages;
		 }
		 set
		 {
		   if(showVATPercentagePackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowVATPercentagePackages",OldValue=showVATPercentagePackages,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showVATPercentagePackages=value;
		   }
			
		 }
	   }
	  private bool showVATPercentageContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowVATPercentageContainers  
	   {
	    
	     get
		{
		   return showVATPercentageContainers;
		 }
		 set
		 {
		   if(showVATPercentageContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowVATPercentageContainers",OldValue=showVATPercentageContainers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showVATPercentageContainers=value;
		   }
			
		 }
	   }
	  private bool hidePageNumber ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool HidePageNumber  
	   {
	    
	     get
		{
		   return hidePageNumber;
		 }
		 set
		 {
		   if(hidePageNumber != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="HidePageNumber",OldValue=hidePageNumber,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   hidePageNumber=value;
		   }
			
		 }
	   }
	  private string pageNumberingTextDesignId ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public string PageNumberingTextDesignId  
	   {
	    
	     get
		{
		   return pageNumberingTextDesignId;
		 }
		 set
		 {
		   if(pageNumberingTextDesignId != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="PageNumberingTextDesignId",OldValue=pageNumberingTextDesignId,NewValue=value,PropertyType="string"};
		    NotifyPropertyChanged(values);
		   pageNumberingTextDesignId=value;
		   }
			
		 }
	   }
	  private bool showRegionalTAXPackages ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowRegionalTAXPackages  
	   {
	    
	     get
		{
		   return showRegionalTAXPackages;
		 }
		 set
		 {
		   if(showRegionalTAXPackages != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowRegionalTAXPackages",OldValue=showRegionalTAXPackages,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showRegionalTAXPackages=value;
		   }
			
		 }
	   }
	  private bool showRegionalTAXContainers ;
	  	  
       
	   [CustomValidation(typeof(QuoteOPMValidationClass), "ValidateClass")]
	   [DataMember]
       public bool ShowRegionalTAXContainers  
	   {
	    
	     get
		{
		   return showRegionalTAXContainers;
		 }
		 set
		 {
		   if(showRegionalTAXContainers != value)
		  {
		    NotifyPropertyChangeValues values=new NotifyPropertyChangeValues(){PropertyName="ShowRegionalTAXContainers",OldValue=showRegionalTAXContainers,NewValue=value,PropertyType="bool"};
		    NotifyPropertyChanged(values);
		   showRegionalTAXContainers=value;
		   }
			
		 }
	   }
   }
   
}
	 