using System;
using System.Collections.Generic;
using System.Linq;
 
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.GlobalModel;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using Logitude.BL.InfrastructureModel.EntityPMs;
using WebFreight.Web.InvoiceModel;
using WebFreight.Web.MetaDataUpdate.AddClasses;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using WebFreight.Web.QuoteModel;
using WebFreight.Web.ShipmentsModel;
using WebFreight.Web.CommonDataModel.DomainServices;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System.Data.Entity.Core.EntityClient;
using System.Configuration;
using Simplog.Server.Infrastructure;
using System.Data.Common;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL;
using Logitude.CRM.Data.Repsitories;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.BL;
using Logitude.Accounting.Data.Repositories;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.BL;
using Logitude.BookingLib.Data.Repositories;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.BL;
using Logitude.Customs.Data.Repsitories;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.BL;
using Logitude.Social.Data.Repsitories;
using Logitude.Server.Tools.CloseTablesClasses;
using Logitude.Customs.BL.ClosedTable;
using Logitude.CRM.BL.CLoseTable;
using Logitude.BookingLib.BL.CLoseTable;
using Logitude.WarehouseLib.Data.Repositories;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.BL.CLoseTable;
using Logitude.TimeManagement.Data.Repositories;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.BL.CLoseTable;
using Logitude.BL.ShipmentsModel.CloseTables;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.GlobalModel;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL;
namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.QuoteModel.EntityUpdateClasses
{
   public class QuoteUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Quote",
			      				    DBTableName =  "Quotes",
			      				    ObjectTableSingular =  "Quote",
			      				    ObjectTablePlural =  "Quotes",
			      				    HasCustomFilter =  true,
			      				    HasCustomFields =  true,
			      				    HasHelper =  true,
			      				    HasShortTitle =  true,
			      				    HasFiltersMenu =  true,
			      				    IsEditable =  true,
			      				    IsNewWizard =  true,
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  true,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  true,
			      				    IsMain =  true,
			      				    IsAutoComplete =  false,
			      				    EnableEditFromLOV =  false,
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  true,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "BR",
			      				    MaxNumberOfCustomFields =  10,
			      				    NewWizardControlName =  "Simplog.QuoteLib.NewQuoteCommand",
			      				    DefaultText =  "Quote",
			      				    Code =  "QUOT",
			      				    Name =  "Queries",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Quote",
			      				    NewWizardComponentPath =  "./Quote/Components/NewEntity/NewQuoteComponent",
			      				    NoTS =  false,
			      				    HasMenuButtons =  true,
			      				    CustomFieldsCount =  0,
			      				    SearchFields =  "Quote,Quotes,Simplog.QuoteLib.NewQuoteCommand,Id,",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EstimateProfit",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "EstimateProfit",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EstimateProfit",
					  						DefaultText =  @"Estimated Profit",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "EstimateProfit",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "EstimateProfit",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsFixedPrice",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsFixedPrice",
					  						ListPropertyPath =  "IsFixedPrice",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsFixedPrice",
					  						DefaultText =  @"Fixed Price",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "IsFixedPrice",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "IsFixedPrice",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomerContactId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Contact",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomerContactId",
					  						ListPropertyPath =  "CustomerContactId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerContactId",
					  						DefaultText =  @"Customer Contact",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "CustomerContactId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "CustomerContactId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CostTotalAmountInLocalCurrency",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CostTotalAmountInLocalCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CostTotalAmountInLocalCurrency",
					  						DefaultText =  @"Cost Total Amount in Local Currency",
					  						ShortFieldLable =  "CostTotalAmount",
					  						ShortFieldLableDefaultText =  @"Cost",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "CostTotalAmountInLocalCurrency",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "CostTotalAmountInLocalCurrency",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SaleTotalAmountInLocalCurrency",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SaleTotalAmountInLocalCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SaleTotalAmountInLocalCurrency",
					  						DefaultText =  @"Sales Total Amount in Local Currency",
					  						ShortFieldLable =  "SaleTotalAmount",
					  						ShortFieldLableDefaultText =  @"Sale",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "SaleTotalAmountInLocalCurrency",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "SaleTotalAmountInLocalCurrency",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CostTotalAmountInSaleCurrency",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CostTotalAmountInSaleCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CostTotalAmountInSaleCurrency",
					  						DefaultText =  @"Cost Total Amount in Sales Currency",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "CostTotalAmountInSaleCurrency",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "CostTotalAmountInSaleCurrency",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SaleTotalAmountInSaleCurrency",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SaleTotalAmountInSaleCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SaleTotalAmountInSaleCurrency",
					  						DefaultText =  @"Sales Total Amount in Sale Currency",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "SaleTotalAmountInSaleCurrency",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "SaleTotalAmountInSaleCurrency",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EstimateProfitInSaleCurrency",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "EstimateProfitInSaleCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EstimateProfitInSaleCurrency",
					  						DefaultText =  @"Estimated Profit In Sales Currency",
					  						ShortFieldLable =  "ProfitInSaleCurrency",
					  						ShortFieldLableDefaultText =  @"Profit In Sale Currency",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "EstimateProfitInSaleCurrency",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "EstimateProfitInSaleCurrency",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ShipperName",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ShipperName",
					  						ListPropertyPath =  "ShipperName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ShipperName",
					  						DefaultText =  @"Shipper Name",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ShipperName",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ShipperName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ConsigneeName",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ConsigneeName",
					  						ListPropertyPath =  "ConsigneeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ConsigneeName",
					  						DefaultText =  @"Consignee Name",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ConsigneeName",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ConsigneeName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DeliveryAddress",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  250,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeliveryAddress",
					  						ListPropertyPath =  "DeliveryAddress",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeliveryAddress",
					  						DefaultText =  @"Delivery Address",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "DeliveryAddress",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "DeliveryAddress",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PickUpAddress",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  250,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PickUpAddress",
					  						ListPropertyPath =  "PickUpAddress",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PickUpAddress",
					  						DefaultText =  @"Pickup Address",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PickUpAddress",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "PickUpAddress",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SaleCurrencyId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Currency",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SaleCurrencyId",
					  						ListPropertyPath =  "SaleCurrencyId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SaleCurrencyId",
					  						DefaultText =  @"Sales Currency",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "SaleCurrencyId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "SaleCurrencyId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ExchangeRate",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ExchangeRate",
					  						ListPropertyPath =  "ExchangeRate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ExchangeRate",
					  						DefaultText =  @"Exchange Rate",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ExchangeRate",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ExchangeRate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomerName",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomerName",
					  						ListPropertyPath =  "CustomerName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerName",
					  						DefaultText =  @"Customer Name",
					  						ListFieldLable =  "CustomerNameListLable",
					  						ListLableDefaultText =  @"Customer",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "CustomerName",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "CustomerName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomerNote",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  1000,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomerNote",
					  						ListPropertyPath =  "CustomerNote",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerNote",
					  						DefaultText =  @"Customer Note",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "CustomerNote",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "CustomerNote",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsCancelled",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsCancelled",
					  						ListPropertyPath =  "IsCancelled",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsCancelled",
					  						DefaultText =  @"Canceled",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "IsCancelled",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "IsCancelled",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "QuoteCostCharges",
					  						ObjectTableName =  "Quote",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "QuoteCostCharges",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "QuoteCostCharge",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "QuoteCostCharges",
					  						DefaultText =  @"Quote Cost Charges",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "QuoteCostCharges",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "QuoteCostCharges",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "QuoteSaleCharges",
					  						ObjectTableName =  "Quote",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "QuoteSaleCharges",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "QuoteSaleCharge",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "QuoteSaleCharges",
					  						DefaultText =  @"Quote Sale Charges",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "QuoteSaleCharges",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "QuoteSaleCharges",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "QuotePriceSteps",
					  						ObjectTableName =  "Quote",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "QuotePriceSteps",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "QuotePriceSteps",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "QuotePriceSteps",
					  						DefaultText =  @"Price Steps",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "QuotePriceSteps",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "QuotePriceSteps",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FollowUpOwner",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FollowUpOwner",
					  						ListPropertyPath =  "FollowUpOwner",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FollowUpOwner",
					  						DefaultText =  @"Follow-Up Owner",
					  						ListFieldLable =  "FollowUpOwnerListLable",
					  						ListLableDefaultText =  @"F/U Owner",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "FollowUpOwner",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "FollowUpOwner",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FollowUpOwnerId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FollowUpOwnerId",
					  						ListPropertyPath =  "FollowUpOwnerId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FollowUpOwnerId",
					  						DefaultText =  @"Follow-Up Owner",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "FollowUpOwnerId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "FollowUpOwnerId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TotalReceivablesAmount",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TotalReceivablesAmount",
					  						ListPropertyPath =  "TotalReceivablesAmount",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TotalReceivablesAmount",
					  						DefaultText =  @"Total Receivables Amount",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "TotalReceivablesAmount",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "TotalReceivablesAmount",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "QuoteNumber",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "QuoteNumber",
					  						ListPropertyPath =  "QuoteNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "QuoteNumber",
					  						DefaultText =  @"Quote No.",
					  						ListFieldLable =  "QuoteListLable",
					  						ListLableDefaultText =  @"Quote No.",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "QuoteNumber",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  true,
					  						CopyToDW =  false,
					  						HelpTextCode =  "QuoteNumber",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MainCarriageCarrierId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Carrier",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "MainCarriageCarrierId",
					  						ListPropertyPath =  "MainCarriageCarrierId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "MainCarriageCarrierId",
					  						DefaultText =  @"Carrier",
					  						ListFieldLable =  "MainCarriageCarrierIdLable",
					  						ListLableDefaultText =  @"Carrier",
					  						ShortFieldLable =  "CarrierId",
					  						ShortFieldLableDefaultText =  @"Carrier",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "MainCarriageCarrierId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "MainCarriageCarrierId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MainCarriageCarrierName",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  70,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "MainCarriageCarrierName",
					  						ListPropertyPath =  "MainCarriageCarrierName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "MainCarriageCarrierName",
					  						DefaultText =  @"Carrier",
					  						ListFieldLable =  "MainCarriageCarrierNameListLable",
					  						ListLableDefaultText =  @"Carrier",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "MainCarriageCarrierName",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "MainCarriageCarrierName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DirectionId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Direction",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "DirectionDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DirectionId",
					  						ListPropertyPath =  "DirectionId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						ColumnHeaderTemplateName =  "DirectionHeaderTemplate",
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DirectionId",
					  						DefaultText =  @"Direction",
					  						ListFieldLable =  "DirectionIdListLable",
					  						ListLableDefaultText =  @"Direction",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						Code =  "DirectionId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "DirectionId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransportModeId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "TransportMode",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "TransportModeTemplete",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TransportModeId",
					  						ListPropertyPath =  "TransportModeId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						ColumnHeaderTemplateName =  "TransportModeHeaderTemplate",
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransportModeId",
					  						DefaultText =  @"Transport Mode",
					  						ListFieldLable =  "TransportModeIdListLable",
					  						ListLableDefaultText =  @"Transport Mode",
					  						ShortFieldLable =  "TransportModeId",
					  						ShortFieldLableDefaultText =  @"Transport",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						Code =  "TransportModeId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "TransportModeId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DepartmentId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Department",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DepartmentId",
					  						ListPropertyPath =  "DepartmentId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DepartmentId",
					  						DefaultText =  @"Department",
					  						ListFieldLable =  "DepartmentListLable",
					  						ListLableDefaultText =  @"Department",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "DepartmentId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "DepartmentId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BranchId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Branch",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "BranchId",
					  						ListPropertyPath =  "BranchId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BranchId",
					  						DefaultText =  @"Branch",
					  						ListFieldLable =  "BranchListLable",
					  						ListLableDefaultText =  @"Branch",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "BranchId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "BranchId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ShipmentTypeId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "ShipmentType",
					  						MinLength =  0,
					  						MaxLength =  4,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ShipmentTypeId",
					  						ListPropertyPath =  "ShipmentTypeId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1Value =  "TransportModeId",
					  						DependencyFilter1Type =  "Path",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ShipmentTypeId",
					  						DefaultText =  @"Shipment Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ShipmentTypeId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ShipmentTypeId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ShipmentType",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "ShipmentType",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ShipmentType",
					  						ListPropertyPath =  "ShipmentType",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ShipmentType",
					  						DefaultText =  @"Shipment Type",
					  						ListFieldLable =  "ShipmentTypeListFieldLable",
					  						ListLableDefaultText =  @"Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ShipmentType",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ShipmentType",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "QuoteCustomerTypeCode",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "QuoteCustomerType",
					  						MinLength =  0,
					  						MaxLength =  4,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "QuoteCustomerTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "QuoteCustomerTypeCode",
					  						DefaultText =  @"Customer Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "QuoteCustomerTypeCode",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "QuoteCustomerTypeCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomerId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomerId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerId",
					  						DefaultText =  @"Customer",
					  						HelpTextCode =  "CustomerId",
					  						HelpTextDefaultText =  @"Indicates who the customer is, so that Logitude knows to refer to the relevant partner for statistics, billing and shared logistics. For Export, the Shipper is selected automatically. For Import, the Consignee is selected automatically.",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "CustomerId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ShipperId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ShipperId",
					  						ListPropertyPath =  "ShipperId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ShipperId",
					  						DefaultText =  @"Shipper",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ShipperId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ShipperId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Shipper",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  70,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Shipper",
					  						ListPropertyPath =  "Shipper",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Shipper",
					  						DefaultText =  @"Shipper",
					  						ListFieldLable =  "ShipperListLable",
					  						ListLableDefaultText =  @"Shipper",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						LookUpTableName =  "Card",
					  						Code =  "Shipper",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "Shipper",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ShipperContactId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Contact",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ShipperContactId",
					  						ListPropertyPath =  "ShipperContactId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ShipperContact",
					  						DefaultText =  @"Shipper Contact",
					  						ShortFieldLable =  "ShipperContactId",
					  						ShortFieldLableDefaultText =  @"Contact",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ShipperContactId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ShipperContact",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ShipperReference1",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  50,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ShipperReference1",
					  						ListPropertyPath =  "ShipperReference1",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ShipperReference1",
					  						DefaultText =  @"Shipper Ref. 1",
					  						ListFieldLable =  "ShipperReferenceListLable",
					  						ListLableDefaultText =  @"Shipper Ref.",
					  						ShortFieldLable =  "ShipperReference1",
					  						ShortFieldLableDefaultText =  @"Reference1",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ShipperReference1",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ShipperReference1",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ShipperReference2",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  50,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ShipperReference2",
					  						ListPropertyPath =  "ShipperReference2",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ShipperReference2",
					  						DefaultText =  @"Shipper Ref. 2",
					  						ShortFieldLable =  "ShipperReference2",
					  						ShortFieldLableDefaultText =  @"Shipper Ref 2",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ShipperReference2",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ShipperReference2",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ConsigneeId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ConsigneeId",
					  						ListPropertyPath =  "ConsigneeId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ConsigneeId",
					  						DefaultText =  @"Consignee",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ConsigneeId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ConsigneeId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Consignee",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  70,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Consignee",
					  						ListPropertyPath =  "Consignee",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Consignee",
					  						DefaultText =  @"Consignee",
					  						ListFieldLable =  "ConsigneeListLable",
					  						ListLableDefaultText =  @"Consignee",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						LookUpTableName =  "Card",
					  						Code =  "Consignee",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "Consignee",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ConsigneeContactId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Contact",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ConsigneeContactId",
					  						ListPropertyPath =  "ConsigneeContactId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ConsigneeContactId",
					  						DefaultText =  @"Consignee Contact",
					  						ShortFieldLable =  "ConsigneeContactId",
					  						ShortFieldLableDefaultText =  @"Contact",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ConsigneeContactId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ConsigneeContactId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ConsigneeReference1",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  50,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ConsigneeReference1",
					  						ListPropertyPath =  "ConsigneeReference1",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ConsigneeReference1",
					  						DefaultText =  @"Consignee Ref. 1",
					  						ShortFieldLable =  "ConsigneeReference1",
					  						ShortFieldLableDefaultText =  @"Reference1",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ConsigneeReference1",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ConsigneeReference1",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ConsigneeReference2",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  50,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ConsigneeReference2",
					  						ListPropertyPath =  "ConsigneeReference2",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ConsigneeReference2",
					  						DefaultText =  @"Consignee Ref. 2",
					  						ShortFieldLable =  "ConsigneeReference2",
					  						ShortFieldLableDefaultText =  @"Consignee Ref 2",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ConsigneeReference2",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ConsigneeReference2",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FromPortId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Port",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FromPortId",
					  						ListPropertyPath =  "FromPortId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FromPortId",
					  						DefaultText =  @"From Port",
					  						ShortFieldLable =  "FromPortId",
					  						ShortFieldLableDefaultText =  @"From Port",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "FromPortId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "FromPortId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FromPort",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FromPortCode",
					  						ListPropertyPath =  "FromPort",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FromPortCode",
					  						DefaultText =  @"From Port Code",
					  						ListFieldLable =  "FromPortCodeListLable",
					  						ListLableDefaultText =  @"From",
					  						ShortFieldLable =  "FromPortCode",
					  						ShortFieldLableDefaultText =  @"From Port Code",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "FromPort",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "FromPortCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ToPortId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Port",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ToPortId",
					  						ListPropertyPath =  "ToPortId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ToPortId",
					  						DefaultText =  @"To Port",
					  						ShortFieldLable =  "ToPortId",
					  						ShortFieldLableDefaultText =  @"To Port",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ToPortId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ToPortId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ToPort",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ToPort",
					  						ListPropertyPath =  "ToPort",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ToPortCode",
					  						DefaultText =  @"To Port Code",
					  						ListFieldLable =  "ToPortCodeListLable",
					  						ListLableDefaultText =  @"To",
					  						ShortFieldLable =  "ToPortCode",
					  						ShortFieldLableDefaultText =  @"To Port Code",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ToPort",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ToPortCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IncotermId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Incoterm",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IncotermId",
					  						ListPropertyPath =  "IncotermId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IncotermId",
					  						DefaultText =  @"Incoterm",
					  						HelpTextCode =  "IncotermId",
					  						HelpTextDefaultText =  @"Incoterm",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "IncotermId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SalesmanUserId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SalesmanUserId",
					  						ListPropertyPath =  "SalesmanUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SalesmanUserId",
					  						DefaultText =  @"Salesman",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "SalesmanUserId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3Value =  "True",
					  						DependencyFilter3Type =  "Constant",
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "SalesmanUserId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedByUserId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CreatedByUserId",
					  						ListPropertyPath =  "CreatedByUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreatedByUserId",
					  						DefaultText =  @"Opened By",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "CreatedByUserId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "CreatedByUserId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedByUser",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CreatedByUser",
					  						ListPropertyPath =  "CreatedByUser",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreatedByUser",
					  						DefaultText =  @"Opened By",
					  						ListFieldLable =  "CreatedByUserListLable",
					  						ListLableDefaultText =  @"Opened by",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "CreatedByUser",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "CreatedByUser",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OpenDate",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  true,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OpenDate",
					  						ListPropertyPath =  "OpenDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OpenDate",
					  						DefaultText =  @"Open Date",
					  						ListFieldLable =  "OpenDateListLable",
					  						ListLableDefaultText =  @"Open Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "OpenDate",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "OpenDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Notes",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  500,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "NotesDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Notes",
					  						ListPropertyPath =  "Notes",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Notes",
					  						DefaultText =  @"Notes",
					  						ListFieldLable =  "NotesListLable",
					  						ListLableDefaultText =  @"Notes",
					  						HelpTextCode =  "Notes",
					  						HelpTextDefaultText =  @"The maximum number of characters allowed in this field is 500.",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						Code =  "Notes",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DescriptionOfGoods",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  512,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DescriptionOfGoods",
					  						ListPropertyPath =  "DescriptionOfGoods",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DescriptionOfGoods",
					  						DefaultText =  @"Description of Goods",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "DescriptionOfGoods",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "DescriptionOfGoods",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ChargeableWeight",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ChargeableWeight",
					  						ListPropertyPath =  "ChargeableWeight",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ChargeableWeight",
					  						DefaultText =  @"Chargeable Weight (%ChargWeightCode)",
					  						ListFieldLable =  "ChargeableWeightListLable",
					  						ListLableDefaultText =  @"Charg. Wg.",
					  						ShortFieldLable =  "WtMsr",
					  						ShortFieldLableDefaultText =  @"Wt / Msr (%ChargWeightCode)",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ChargeableWeight",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ChargeableWeight",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "GrossWeight",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						DataTemplateName =  "GrossWeightDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "GrossWeight",
					  						ListPropertyPath =  "GrossWeight",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "GrossWeight",
					  						DefaultText =  @"Gross Weight (%GrossWeightCode)",
					  						ListFieldLable =  "GrossWeightListLable",
					  						ListLableDefaultText =  @"Gross Weight",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						Code =  "GrossWeight",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "GrossWeight",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsClosed",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsClosed",
					  						ListPropertyPath =  "IsClosed",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsClosed",
					  						DefaultText =  @"Closed",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "IsClosed",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "IsClosed",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OpenQuotes",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OpenQuotes",
					  						ListPropertyPath =  "OpenQuotes",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OpenQuotes",
					  						DefaultText =  @"Open Quotes",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "OpenQuotes",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "OpenQuotes",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ClosedQuotes",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Constant",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ClosedQuotes",
					  						DefaultText =  @"Closed Quotes",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ClosedQuotes",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ClosedQuotes",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LeadingCurrencyId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Currency",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LeadingCurrencyId",
					  						ListPropertyPath =  "LeadingCurrencyId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  true,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LeadingCurrencyId",
					  						DefaultText =  @"Leading Currency",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "LeadingCurrencyId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "LeadingCurrencyId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DimensionsUnitCode",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "DimensionsUnit",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DimensionsUnitCode",
					  						ListPropertyPath =  "DimensionsUnitCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DimensionsUnitCode",
					  						DefaultText =  @"Dimension Unit Code",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "DimensionsUnitCode",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "DimensionsUnitCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Volume",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Volume",
					  						ListPropertyPath =  "Volume",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Volume",
					  						DefaultText =  @"Volume (%VolumeCode)",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "Volume",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "Volume",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Ratio",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Ratio",
					  						ListPropertyPath =  "Ratio",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Ratio",
					  						DefaultText =  @"Ratio",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "Ratio",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "Ratio",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "NumberOfPackages",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "NumberOfPackages",
					  						ListPropertyPath =  "NumberOfPackages",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "NumberOfPackages",
					  						DefaultText =  @"Number of Packages",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "NumberOfPackages",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "NumberOfPackages",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "NumberOfContainers",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "NumberOfContainers",
					  						ListPropertyPath =  "NumberOfContainers",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "NumberOfContainers",
					  						DefaultText =  @"Number of Containers",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "NumberOfContainers",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "NumberOfContainers",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VolumeUnitCode",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "VolumeUnit",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "VolumeUnitCode",
					  						ListPropertyPath =  "VolumeUnitCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VolumeUnitCode",
					  						DefaultText =  @"Volume Unit Code",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "VolumeUnitCode",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "VolumeUnitCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsDangerous",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsDangerous",
					  						ListPropertyPath =  "IsDangerous",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsDangerous",
					  						DefaultText =  @"Dangerous Goods",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "IsDangerous",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						ListFieldLable =  "IsDangerousLable",
					  						ListLableDefaultText =  @"Dangerous Goods",
					  						HelpTextCode =  "IsDangerous",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ExpirationDays",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ExpirationDays",
					  						ListPropertyPath =  "ExpirationDays",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ExpirationDays",
					  						DefaultText =  @"Expiration Days",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ExpirationDays",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ExpirationDays",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ExpirationDate",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ExpirationDate",
					  						ListPropertyPath =  "ExpirationDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ExpirationDate",
					  						DefaultText =  @"Expiration Date",
					  						ListFieldLable =  "ExpirationDateLable",
					  						ListLableDefaultText =  @"Expiration date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ExpirationDate",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ExpirationDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsFreightBySteps",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsFreightBySteps",
					  						ListPropertyPath =  "IsFreightBySteps",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsFreightBySteps",
					  						DefaultText =  @"Freight By Steps",
					  						ShortFieldLable =  "IsFreightBySteps",
					  						ShortFieldLableDefaultText =  @"Price by break",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "IsFreightBySteps",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "IsFreightBySteps",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TotalContainers",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  250,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TotalContainers",
					  						ListPropertyPath =  "TotalContainers",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TotalContainers",
					  						DefaultText =  @"Total Containers",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "TotalContainers",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "TotalContainers",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OrderNumberOfPackages",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OrderNumberOfPackages",
					  						ListPropertyPath =  "OrderNumberOfPackages",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OrderNumberOfPackages",
					  						DefaultText =  @"Number of Packages",
					  						ListFieldLable =  "OrderNumberOfPackagesLable",
					  						ListLableDefaultText =  @"Pieces",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "OrderNumberOfPackages",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "OrderNumberOfPackages",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsAdhoc",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsAdhoc",
					  						ListPropertyPath =  "IsAdhoc",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsAdhoc",
					  						DefaultText =  @"Adhoc",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "IsAdhoc",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "IsAdhoc",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  1500,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  @"Search partners / ports / ref.# / notes",
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  @"Searching by :\n1:quote numbers\n2: References\n3: consignee and shipper names\n4: from port to port",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "SearchFields",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Reference",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Custom",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Reference",
					  						DefaultText =  @"Reference",
					  						HelpTextCode =  "Reference",
					  						HelpTextDefaultText =  @"Searching by:\n1. Shipper Reference 1\n2. Shipper Reference 2\n3. Consignee Reference 1\n4. Consignee Reference 2\n5. Quote Number",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "Reference",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FromOrToPort",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Custom",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FromOrToPort",
					  						DefaultText =  @"From or To Port",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "FromOrToPort",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "FromOrToPort",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FollowUpDate",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						DataTemplateName =  "DateTimeDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "LargerThan",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FollowUpDate",
					  						ListPropertyPath =  "FollowUpDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FUDate",
					  						DefaultText =  @"Follow-Up Date",
					  						ListFieldLable =  "FollowUpDateListLable",
					  						ListLableDefaultText =  @"F/U Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						Code =  "FollowUpDate",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "FUDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FollowUpType",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FollowUpType",
					  						ListPropertyPath =  "FollowUpType",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FUType",
					  						DefaultText =  @"Follow-Up Type",
					  						ListFieldLable =  "FollowUpTypeListLable",
					  						ListLableDefaultText =  @"F/U Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "FollowUpType",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "FUType",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FollowUpTypeId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "EventType",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FollowUpTypeId",
					  						ListPropertyPath =  "FollowUpTypeId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FollowUpTypeId",
					  						DefaultText =  @"Follow-Up Type",
					  						ListFieldLable =  "FollowUpTypeIdListLable",
					  						ListLableDefaultText =  @"F/U Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "FollowUpTypeId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "FollowUpTypeId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TodayFollowUps",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Constant",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Custom",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TodayFollowUps",
					  						DefaultText =  @"Today's Follow-Ups",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "TodayFollowUps",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "TodayFollowUps",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TomorrowFollowUps",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Constant",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Custom",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TomorrowFollowUps",
					  						DefaultText =  @"Tomorrow's Follow-Ups",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "TomorrowFollowUps",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "TomorrowFollowUps",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DueDateFollowUps",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Constant",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Custom",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DueDateFollowUps",
					  						DefaultText =  @"Due Date Follow-Ups",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "DueDateFollowUps",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "DueDateFollowUps",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AllFollowUps",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Constant",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Custom",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AllFollowUps",
					  						DefaultText =  @"All Follow-Ups",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "AllFollowUps",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "AllFollowUps",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FollowUpNotes",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  250,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  250,
					  						DisplayInList =  true,
					  						DataTemplateName =  "FollowUpNoteDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FollowUpNotes",
					  						ListPropertyPath =  "FollowUpNotes",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FUNotes",
					  						DefaultText =  @"Follow-Up Notes",
					  						ListFieldLable =  "FollowUpNotesListLable",
					  						ListLableDefaultText =  @"Notes",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						Code =  "FollowUpNotes",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "FUNotes",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PackageType1Id",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "PackageType",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PackageType1Id",
					  						ListPropertyPath =  "PackageType1Id",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PackageType1Id",
					  						DefaultText =  @"Package Type 1",
					  						ShortFieldLable =  "PackageTypeId",
					  						ShortFieldLableDefaultText =  @"Package Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PackageType1Id",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "PackageType1Id",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PackageType2Id",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "PackageType",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PackageType2Id",
					  						ListPropertyPath =  "PackageType2Id",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PackageType2Id",
					  						DefaultText =  @"Package Type 2",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PackageType2Id",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "PackageType2Id",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PackageType3Id",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "PackageType",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PackageType3Id",
					  						ListPropertyPath =  "PackageType3Id",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PackageType3Id",
					  						DefaultText =  @"Package Type 3",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PackageType3Id",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "PackageType3Id",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PackageType4Id",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "PackageType",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PackageType4Id",
					  						ListPropertyPath =  "PackageType4Id",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PackageType4Id",
					  						DefaultText =  @"Package Type 4",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PackageType4Id",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "PackageType4Id",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PackageType5Id",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "PackageType",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PackageType5Id",
					  						ListPropertyPath =  "PackageType5Id",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PackageType5Id",
					  						DefaultText =  @"Package Type 5",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PackageType5Id",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "PackageType5Id",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PackageType1Quantity",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PackageType1Quantity",
					  						ListPropertyPath =  "PackageType1Quantity",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PackageType1Quantity",
					  						DefaultText =  @"Package Type 1 Quantity",
					  						ShortFieldLable =  "PackageTypeQuantity",
					  						ShortFieldLableDefaultText =  @"Quantity",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PackageType1Quantity",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "PackageType1Quantity",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PackageType2Quantity",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PackageType2Quantity",
					  						ListPropertyPath =  "PackageType2Quantity",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PackageType2Quantity",
					  						DefaultText =  @"Package Type 2 Quantity",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PackageType2Quantity",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "PackageType2Quantity",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PackageType3Quantity",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PackageType3Quantity",
					  						ListPropertyPath =  "PackageType3Quantity",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PackageType3Quantity",
					  						DefaultText =  @"Package Type 3 Quantity",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PackageType3Quantity",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "PackageType3Quantity",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PackageType4Quantity",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PackageType4Quantity",
					  						ListPropertyPath =  "PackageType4Quantity",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PackageType4Quantity",
					  						DefaultText =  @"Package Type 4 Quantity",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PackageType4Quantity",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "PackageType4Quantity",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PackageType5Quantity",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PackageType5Quantity",
					  						ListPropertyPath =  "PackageType5Quantity",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PackageType5Quantity",
					  						DefaultText =  @"Package Type 5 Quantity",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PackageType5Quantity",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "PackageType5Quantity",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "QuoteCharges",
					  						ObjectTableName =  "Quote",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "QuoteCharges",
					  						ListPropertyPath =  "QuoteCharges",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "QuoteCharge",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "QuoteCharges",
					  						DefaultText =  @"Quote Charges",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "QuoteCharges",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "QuoteCharges",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "QuoteTypeCode",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "QuoteType",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "QuoteTypeCode",
					  						ListPropertyPath =  "QuoteTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "QuoteTypeCode",
					  						DefaultText =  @"Quote Type",
					  						HelpTextCode =  "QuoteTypeCode",
					  						HelpTextDefaultText =  @"Ad hoc: Use for quoting prices for a specific shipment with a given quantity.\nRouting Rates: Use for quoting your rates for package types, per unit or by price break levels.",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "QuoteTypeCode",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "QuoteTypeName",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "QuoteTypeName",
					  						ListPropertyPath =  "QuoteTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "QuoteTypeName",
					  						DefaultText =  @"Quote Type",
					  						ListFieldLable =  "QuoteTypeNameListLable",
					  						ListLableDefaultText =  @"Quote Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "QuoteTypeName",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "QuoteTypeName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsExpiredQuote",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsExpiredQuote",
					  						DefaultText =  @"Is Expired Quote",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "IsExpiredQuote",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "IsExpiredQuote",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "GrossWeightUnitCode",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "WeightUnit",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "GrossWeightUnitCode",
					  						ListPropertyPath =  "GrossWeightUnitCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "GrossWeightUnitCode",
					  						DefaultText =  @"Gross Weight Unit Code",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "GrossWeightUnitCode",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "GrossWeightUnitCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ChargeableWeightUnitCode",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "WeightUnit",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ChargeableWeightUnitCode",
					  						ListPropertyPath =  "ChargeableWeightUnitCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ChargeableWeightUnitCode",
					  						DefaultText =  @"Chargeable Weight Unit Code",
					  						ShortFieldLable =  "WtMsrUnitCode",
					  						ShortFieldLableDefaultText =  @"Wt / Msr Unit Code",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ChargeableWeightUnitCode",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ChargeableWeightUnitCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VolumetricWeight",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "VolumetricWeight",
					  						ListPropertyPath =  "VolumetricWeight",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VolumetricWeight",
					  						DefaultText =  @"Volumetric Weight (%ChargWeightCode)",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "VolumetricWeight",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "VolumetricWeight",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PickupLocation",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  500,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PickupLocation",
					  						ListPropertyPath =  "PickupLocation",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PickupLocation",
					  						DefaultText =  @"Pickup Location",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PickupLocation",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "PickupLocation",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DeliveryLocation",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  500,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeliveryLocation",
					  						ListPropertyPath =  "DeliveryLocation",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeliveryLocation",
					  						DefaultText =  @"Delivery Location",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "DeliveryLocation",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "DeliveryLocation",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DepartmentName",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DepartmentName",
					  						ListPropertyPath =  "DepartmentName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DepartmentName",
					  						DefaultText =  @"Department",
					  						ListFieldLable =  "DepartmentNameListLable",
					  						ListLableDefaultText =  @"Department",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "DepartmentName",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "DepartmentName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BranchName",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "BranchName",
					  						ListPropertyPath =  "BranchName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BranchName",
					  						DefaultText =  @"Branch",
					  						ListFieldLable =  "BranchNameListLable",
					  						ListLableDefaultText =  @"Branch",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "BranchName",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "BranchName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FromPartnerId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FromPartnerId",
					  						ListPropertyPath =  "FromPartnerId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FromPartnerId",
					  						DefaultText =  @"From Partner",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "FromPartnerId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "FromPartnerId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ToPartnerId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ToPartnerId",
					  						ListPropertyPath =  "ToPartnerId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ToPartnerId",
					  						DefaultText =  @"To Partner",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ToPartnerId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ToPartnerId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FromPartnerAddressId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Address",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FromPartnerAddressId",
					  						ListPropertyPath =  "FromPartnerAddressId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FromPartnerAddressId",
					  						DefaultText =  @"From Partner Address",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "FromPartnerAddressId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "FromPartnerAddressId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ToPartnerAddressId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Address",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ToPartnerAddressId",
					  						ListPropertyPath =  "ToPartnerAddressId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ToPartnerAddressId",
					  						DefaultText =  @"To Partner Address",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ToPartnerAddressId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ToPartnerAddressId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FromLocation",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FromLocation",
					  						ListPropertyPath =  "FromLocation",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FromLocation",
					  						DefaultText =  @"From Location",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "FromLocation",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "FromLocation",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ToLocation",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ToLocation",
					  						ListPropertyPath =  "ToLocation",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ToLocation",
					  						DefaultText =  @"To Location",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ToLocation",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ToLocation",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MyFollowUps",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Constant",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Custom",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "MyFollowUps",
					  						DefaultText =  @"My Follow Ups",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "MyFollowUps",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "MyFollowUps",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MyQuotes",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "MyQuotes",
					  						ListPropertyPath =  "MyQuotes",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "MyQuotes",
					  						DefaultText =  @"My Quotes",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "MyQuotes",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "MyQuotes",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FromAddressCity",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  25,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FromAddressCity",
					  						ListPropertyPath =  "FromAddressCity",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FromAddressCity",
					  						DefaultText =  @"From Address City",
					  						ShortFieldLable =  "FromAddressCity",
					  						ShortFieldLableDefaultText =  @"City",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "FromAddressCity",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "FromAddressCity",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FromAddressCountryId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Country",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FromAddressCountryId",
					  						ListPropertyPath =  "FromAddressCountryId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FromAddressCountryId",
					  						DefaultText =  @"From Address Country",
					  						ShortFieldLable =  "FromAddressCountryId",
					  						ShortFieldLableDefaultText =  @"Country",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "FromAddressCountryId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "FromAddressCountryId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FromAddressZipCode",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FromAddressZipCode",
					  						ListPropertyPath =  "FromAddressZipCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FromAddressZipCode",
					  						DefaultText =  @"From Address Zip Code",
					  						ShortFieldLable =  "FromAddressZipCode",
					  						ShortFieldLableDefaultText =  @"Zip Code",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "FromAddressZipCode",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "FromAddressZipCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ToAddressCity",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  25,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ToAddressCity",
					  						ListPropertyPath =  "ToAddressCity",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ToAddressCity",
					  						DefaultText =  @"To Address City",
					  						ShortFieldLable =  "ToAddressCity",
					  						ShortFieldLableDefaultText =  @"City",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ToAddressCity",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ToAddressCity",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ToAddressCountryId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Country",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ToAddressCountryId",
					  						ListPropertyPath =  "ToAddressCountryId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ToAddressCountryId",
					  						DefaultText =  @"To Address Country",
					  						ShortFieldLable =  "ToAddressCountryId",
					  						ShortFieldLableDefaultText =  @"Country",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ToAddressCountryId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ToAddressCountryId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ToAddressZipCode",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ToAddressZipCode",
					  						ListPropertyPath =  "ToAddressZipCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ToAddressZipCode",
					  						DefaultText =  @"To Address Zip Code",
					  						ShortFieldLable =  "ToAddressZipCode",
					  						ShortFieldLableDefaultText =  @"Zip Code",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ToAddressZipCode",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ToAddressZipCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AcceptedWithoutShipments",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AcceptedWithoutShipments",
					  						ListPropertyPath =  "AcceptedWithoutShipments",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AcceptedWithoutShipments",
					  						DefaultText =  @"Accepted Without Shipments",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "AcceptedWithoutShipments",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "AcceptedWithoutShipments",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DimFactor",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DimFactor",
					  						ListPropertyPath =  "DimFactor",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DimFactor",
					  						DefaultText =  @"Dim Factor",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "DimFactor",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "DimFactor",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsDraftQuote",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsDraftQuote",
					  						ListPropertyPath =  "IsDraftQuote",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsDraftQuote",
					  						DefaultText =  @"Is Draft Quote",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "IsDraftQuote",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "IsDraftQuote",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsSentQuote",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsSentQuote",
					  						ListPropertyPath =  "IsSentQuote",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsSentQuote",
					  						DefaultText =  @"Is Sent Quote",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "IsSentQuote",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "IsSentQuote",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsAcceptedQuote",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsAcceptedQuote",
					  						ListPropertyPath =  "IsAcceptedQuote",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsAcceptedQuote",
					  						DefaultText =  @"Is Accepted Quote",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "IsAcceptedQuote",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "IsAcceptedQuote",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IncludePickUp",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IncludePickUp",
					  						ListPropertyPath =  "IncludePickUp",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IncludePickUp",
					  						DefaultText =  @"Include PickUp",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "IncludePickUp",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "IncludePickUp",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IncludeDelivery",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IncludeDelivery",
					  						ListPropertyPath =  "IncludeDelivery",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IncludeDelivery",
					  						DefaultText =  @"Include Delivery",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "IncludeDelivery",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "IncludeDelivery",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PickUpAddressId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Address",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PickUpAddressId",
					  						ListPropertyPath =  "PickUpAddressId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PickUpAddressId",
					  						DefaultText =  @"PickUp Address",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PickUpAddressId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "PickUpAddressId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DeliveryAddressId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Address",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeliveryAddressId",
					  						ListPropertyPath =  "DeliveryAddressId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeliveryAddressId",
					  						DefaultText =  @"Delivery Address",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "DeliveryAddressId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "DeliveryAddressId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "QuoteClosingReasonCode",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "QuoteClosingReason",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "QuoteClosingReasonCode",
					  						ListPropertyPath =  "QuoteClosingReasonCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "QuoteClosingReasonCode",
					  						DefaultText =  @"Closing Reason",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "QuoteClosingReasonCode",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "QuoteClosingReasonCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SentDate",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  true,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SentDate",
					  						ListPropertyPath =  "SentDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SentDate",
					  						DefaultText =  @"Sent Date",
					  						ListFieldLable =  "SentDateListLable",
					  						ListLableDefaultText =  @"Sent Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						Code =  "SentDate",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "SentDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AcceptedDate",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  true,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AcceptedDate",
					  						ListPropertyPath =  "AcceptedDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AcceptedDate",
					  						DefaultText =  @"Accepted Date",
					  						ListFieldLable =  "AcceptedDateListLable",
					  						ListLableDefaultText =  @"Accepted Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "AcceptedDate",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "AcceptedDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DeclinedDate",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  true,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeclinedDate",
					  						ListPropertyPath =  "DeclinedDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclinedDate",
					  						DefaultText =  @"Declined Date",
					  						ListFieldLable =  "DeclinedDateListLable",
					  						ListLableDefaultText =  @"Declined Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "DeclinedDate",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "DeclinedDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UsageCount",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "UsageCount",
					  						ListPropertyPath =  "UsageCount",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UsageCount",
					  						DefaultText =  @"Usage Count",
					  						ListFieldLable =  "UsageCountListLable",
					  						ListLableDefaultText =  @"Usage Count",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "UsageCount",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "UsageCount",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LastUsageDate",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LastUsageDate",
					  						ListPropertyPath =  "LastUsageDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LastUsageDate",
					  						DefaultText =  @"Last Usage Date",
					  						ListFieldLable =  "LastUsageDateListLable",
					  						ListLableDefaultText =  @"Last Usage Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "LastUsageDate",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "LastUsageDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BusinessUnitId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "BusinessUnit",
					  						MinLength =  0,
					  						MaxLength =  50,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "BusinessUnitId",
					  						ListPropertyPath =  "BusinessUnitId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BusinessUnitId",
					  						DefaultText =  @"Business Unit",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "BusinessUnitId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "BusinessUnitId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "QuotePackages",
					  						ObjectTableName =  "Quote",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "QuotePackages",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "QuotePackage",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "QuotePackages",
					  						DefaultText =  @"Quote Packages",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "QuotePackages",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "QuotePackages",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BusinessUnitName",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "BusinessUnitName",
					  						ListPropertyPath =  "BusinessUnitName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BusinessUnitName",
					  						DefaultText =  @"Business Unit",
					  						ListFieldLable =  "BusinessUnitNameListLable",
					  						ListLableDefaultText =  @"Business Unit",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "BusinessUnitName",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "BusinessUnitName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomerReference1",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  50,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomerReference1",
					  						ListPropertyPath =  "CustomerReference1",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerReference1",
					  						DefaultText =  @"Customer Ref 1",
					  						ListFieldLable =  "CustomerReference1ListLable",
					  						ListLableDefaultText =  @"Customer Ref 1",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "CustomerReference1",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "CustomerReference1",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomerReference2",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  50,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomerReference2",
					  						ListPropertyPath =  "CustomerReference2",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerReference2",
					  						DefaultText =  @"Customer Ref 2",
					  						ListFieldLable =  "CustomerReference2ListLable",
					  						ListLableDefaultText =  @"Customer Ref 2",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "CustomerReference2",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "CustomerReference2",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Subject",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Subject",
					  						ListPropertyPath =  "Subject",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Subject",
					  						DefaultText =  @"Subject",
					  						ListFieldLable =  "SubjectLable",
					  						ListLableDefaultText =  @"Subject",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "Subject",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "Subject",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsSubjectEdited",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsSubjectEdited",
					  						ListPropertyPath =  "IsSubjectEdited",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsSubjectEdited",
					  						DefaultText =  @"Is Subject Edited",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "IsSubjectEdited",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "IsSubjectEdited",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Routing",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						DataTemplateName =  "QuoteRoutingDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Routing",
					  						ListPropertyPath =  "Routing",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Routing",
					  						DefaultText =  @"Routing",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						Code =  "Routing",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "Routing",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SalesmanName",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SalesmanName",
					  						ListPropertyPath =  "SalesmanName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SalesmanName",
					  						DefaultText =  @"Salesman",
					  						ListFieldLable =  "SalesmanNameLable",
					  						ListLableDefaultText =  @"Salesman",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "SalesmanName",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "SalesmanName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IncotermCode",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IncotermCode",
					  						ListPropertyPath =  "IncotermCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IncotermCode",
					  						DefaultText =  @"Incoterm",
					  						ListFieldLable =  "IncotermCodeLable",
					  						ListLableDefaultText =  @"Incoterm",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "IncotermCode",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "IncotermCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StageId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "QuoteStage",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StageId",
					  						ListPropertyPath =  "StageId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StageId",
					  						DefaultText =  @"Stage",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "StageId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "StageId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StageName",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "QuoteStageDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StageName",
					  						ListPropertyPath =  "StageName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StageName",
					  						DefaultText =  @"Stage",
					  						ListFieldLable =  "StageNameLable",
					  						ListLableDefaultText =  @"Stage",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						Code =  "StageName",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "StageName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StageDueDate",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StageDueDate",
					  						ListPropertyPath =  "StageDueDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StageDueDate",
					  						DefaultText =  @"Stage due date",
					  						ListFieldLable =  "StageDueDateLable",
					  						ListLableDefaultText =  @"Stage due date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "StageDueDate",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "StageDueDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RatingCode",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "QuoteRating",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "RatingCode",
					  						ListPropertyPath =  "RatingCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "RatingCode",
					  						DefaultText =  @"Rating",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "RatingCode",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "RatingCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LastActivityDate",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LastActivityDate",
					  						ListPropertyPath =  "LastActivityDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LastActivityDate",
					  						DefaultText =  @"Last Activity Date",
					  						ListFieldLable =  "LastActivityDateLable",
					  						ListLableDefaultText =  @"Last Activity Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "LastActivityDate",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "LastActivityDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LastActivitySubject",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  255,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LastActivitySubject",
					  						ListPropertyPath =  "LastActivitySubject",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LastActivitySubject",
					  						DefaultText =  @"Last Activity Subject",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "LastActivitySubject",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "LastActivitySubject",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LastActivityTypeCode",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "QuoteLastActivityTypeDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LastActivityTypeCode",
					  						ListPropertyPath =  "LastActivityTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LastActivityTypeCode",
					  						DefaultText =  @"Last Activity",
					  						ListFieldLable =  "LastActivityTypeCodeLable",
					  						ListLableDefaultText =  @"Last Activity",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						Code =  "LastActivityTypeCode",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "LastActivityTypeCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "NextActivityDate",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						DataTemplateName =  "QuoteNextActivityDateTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "NextActivityDate",
					  						ListPropertyPath =  "NextActivityDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "NextActivityDate",
					  						DefaultText =  @"Next Activity Date",
					  						ListFieldLable =  "NextActivityDateLable",
					  						ListLableDefaultText =  @"Next Activity Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						Code =  "NextActivityDate",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "NextActivityDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "NextActivitySubject",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  255,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "NextActivitySubject",
					  						ListPropertyPath =  "NextActivitySubject",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "NextActivitySubject",
					  						DefaultText =  @"Next Activity Subject",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "NextActivitySubject",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "NextActivitySubject",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "NextActivityTypeCode",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "QuoteNextActivityTypeDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "NextActivityTypeCode",
					  						ListPropertyPath =  "NextActivityTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "NextActivityTypeCode",
					  						DefaultText =  @"Next Activity",
					  						ListFieldLable =  "NextActivityTypeCodeLable",
					  						ListLableDefaultText =  @"Next Activity",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						Code =  "NextActivityTypeCode",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "NextActivityTypeCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RatingName",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "RatingName",
					  						ListPropertyPath =  "RatingName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "RatingName",
					  						DefaultText =  @"Rating",
					  						ListFieldLable =  "RatingName",
					  						ListLableDefaultText =  @"Rating",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "RatingName",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "RatingName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StageMaxDays",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StageMaxDays",
					  						ListPropertyPath =  "StageMaxDays",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StageMaxDays",
					  						DefaultText =  @"Stage max days",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "StageMaxDays",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "StageMaxDays",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RatingIndexOrder",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "RatingIndexOrder",
					  						ListPropertyPath =  "RatingIndexOrder",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "RatingIndexOrder",
					  						DefaultText =  @"Rating index order",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "RatingIndexOrder",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "RatingIndexOrder",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsAutomaticallyClosed",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsAutomaticallyClosed",
					  						ListPropertyPath =  "IsAutomaticallyClosed",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsAutomaticallyClosed",
					  						DefaultText =  @"Close automatically as declined after",
					  						ListFieldLable =  "IsAutomaticallyClosedLable",
					  						ListLableDefaultText =  @"Is Automatically Closed",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "IsAutomaticallyClosed",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "IsAutomaticallyClosed",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AutomaticallyCloseDate",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AutomaticallyCloseDate",
					  						ListPropertyPath =  "AutomaticallyCloseDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AutomaticallyCloseDate",
					  						DefaultText =  @"Close Date",
					  						ListFieldLable =  "AutomaticallyCloseDateLable",
					  						ListLableDefaultText =  @"Automatically Close Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "AutomaticallyCloseDate",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "AutomaticallyCloseDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedByUserId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "UpdatedByUserId",
					  						ListPropertyPath =  "UpdatedByUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdatedByUserId",
					  						DefaultText =  @"Updated By",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "UpdatedByUserId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "UpdatedByUserId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdateDate",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  true,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "UpdateDate",
					  						ListPropertyPath =  "UpdateDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdateDate",
					  						DefaultText =  @"Update Date",
					  						ListFieldLable =  "UpdateDateListLable",
					  						ListLableDefaultText =  @"Update Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "UpdateDate",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "UpdateDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AutomaticallyCloseDays",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AutomaticallyCloseDays",
					  						ListPropertyPath =  "AutomaticallyCloseDays",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AutomaticallyCloseDays",
					  						DefaultText =  @"Close Days",
					  						ListFieldLable =  "AutomaticallyCloseDaysLable",
					  						ListLableDefaultText =  @"Automatically Close Days",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "AutomaticallyCloseDays",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "AutomaticallyCloseDays",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "QuoteClosingReasonName",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "QuoteClosingReasonName",
					  						ListPropertyPath =  "QuoteClosingReasonName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "QuoteClosingReasonName",
					  						DefaultText =  @"Closing Reason",
					  						ListFieldLable =  "ClosingReasonNameListLable",
					  						ListLableDefaultText =  @"Closing Reason",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "QuoteClosingReasonName",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "QuoteClosingReasonName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ProductCode",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ProductCode",
					  						ListPropertyPath =  "ProductCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ProductCode",
					  						DefaultText =  @"Product Code",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ProductCode",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ProductCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsCreatedQuote",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsCreatedQuote",
					  						ListPropertyPath =  "IsCreatedQuote",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsCreatedQuote",
					  						DefaultText =  @"Is Created Quote",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "IsCreatedQuote",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "IsCreatedQuote",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ETDLabel",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  50,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ETDLabel",
					  						ListPropertyPath =  "ETDLabel",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ETDLabel",
					  						DefaultText =  @"ETD Label",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ETDLabel",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ETDLabel",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ETALabel",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  50,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ETALabel",
					  						ListPropertyPath =  "ETALabel",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ETALabel",
					  						DefaultText =  @"ETA Label",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ETALabel",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ETALabel",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransitTime",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  30,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TransitTime",
					  						ListPropertyPath =  "TransitTime",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransitTime",
					  						DefaultText =  @"Transit Time",
					  						ListFieldLable =  "TransitTimeLable",
					  						ListLableDefaultText =  @"Transit Time",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "TransitTime",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "TransitTime",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DepartureFrequency",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  30,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DepartureFrequency",
					  						ListPropertyPath =  "DepartureFrequency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DepartureFrequency",
					  						DefaultText =  @"Departure Frequency",
					  						ListFieldLable =  "DepartureFrequencyLable",
					  						ListLableDefaultText =  @"Departure Frequency",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "DepartureFrequency",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "DepartureFrequency",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ETD",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ETD",
					  						ListPropertyPath =  "ETD",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ETD",
					  						DefaultText =  @"ETD",
					  						ListFieldLable =  "ETDListLable",
					  						ListLableDefaultText =  @"ETD",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ETD",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ETD",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ETA",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ETA",
					  						ListPropertyPath =  "ETA",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ETA",
					  						DefaultText =  @"ETA",
					  						ListFieldLable =  "ETAListLable",
					  						ListLableDefaultText =  @"ETA",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ETA",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ETA",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AgentId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AgentId",
					  						ListPropertyPath =  "AgentId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1Value =  "AG",
					  						DependencyFilter1Type =  "Constant",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AgentId",
					  						DefaultText =  @"Agent",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "AgentId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "AgentId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AgentName",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  70,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AgentName",
					  						ListPropertyPath =  "AgentName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AgentName",
					  						DefaultText =  @"Agent",
					  						ListFieldLable =  "AgentNameListLable",
					  						ListLableDefaultText =  @"Agent",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						LookUpTableName =  "Card",
					  						Code =  "AgentName",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "AgentName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AgentAddressId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Address",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AgentAddressId",
					  						ListPropertyPath =  "AgentAddressId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AgentAddressId",
					  						DefaultText =  @"Agent Address",
					  						ShortFieldLable =  "AgentAddressId",
					  						ShortFieldLableDefaultText =  @"Address",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "AgentAddressId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "AgentAddressId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AgentContactId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Contact",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AgentContactId",
					  						ListPropertyPath =  "AgentContactId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AgentContactId",
					  						DefaultText =  @"Agent Contact",
					  						ShortFieldLable =  "AgentContactId",
					  						ShortFieldLableDefaultText =  @"Contact",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "AgentContactId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "AgentContactId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MoveTypeId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "MoveType",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "MoveTypeId",
					  						ListPropertyPath =  "MoveTypeId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1Value =  "TransportModeId",
					  						DependencyFilter1Type =  "Path",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "MoveTypeId",
					  						DefaultText =  @"Move Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "MoveTypeId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "MoveTypeId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TEU",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TEU",
					  						ListPropertyPath =  "TEU",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TEU",
					  						DefaultText =  @"TEU",
					  						ListFieldLable =  "TEUListLable",
					  						ListLableDefaultText =  @"TEU",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "TEU",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "TEU",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SalesTotalAmounts",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  250,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SalesTotalAmounts",
					  						ListPropertyPath =  "SalesTotalAmounts",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SalesTotalAmounts",
					  						DefaultText =  @"Sales Total Amounts",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "SalesTotalAmounts",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "SalesTotalAmounts",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "QuoteSalesTotals",
					  						ObjectTableName =  "Quote",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "QuoteSalesTotals",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "QuoteSalesTotal",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "QuoteSalesTotals",
					  						DefaultText =  @"Quote Sales Totals",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "QuoteSalesTotals",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "QuoteSalesTotals",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ValueOfGoods",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ValueOfGoods",
					  						ListPropertyPath =  "ValueOfGoods",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ValueOfGoods",
					  						DefaultText =  @"Value of Goods",
					  						ListFieldLable =  "ValueOfGoodsListLable",
					  						ListLableDefaultText =  @"Value of Goods",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ValueOfGoods",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ValueOfGoods",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ValueOfGoodsCurrencyId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Currency",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ValueOfGoodsCurrencyId",
					  						ListPropertyPath =  "ValueOfGoodsCurrencyId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ValueOfGoodsCurrencyId",
					  						DefaultText =  @"Value of Goods Currency",
					  						ShortFieldLable =  "ValueOfGoodsCurrencyId",
					  						ShortFieldLableDefaultText =  @"Value of Goods Currency",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ValueOfGoodsCurrencyId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "ValueOfGoodsCurrencyId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsChargesByVAT",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsChargesByVAT",
					  						ListPropertyPath =  "IsChargesByVAT",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsChargesByVAT",
					  						DefaultText =  @"Charges By VAT",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "IsChargesByVAT",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "IsChargesByVAT",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TotalVATPerQuote",
					  						ObjectTableName =  "Quote",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TotalVATPerQuote",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "QuoteVATsTotal",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TotalVATPerQuote",
					  						DefaultText =  @"Total VAT per quote",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "TotalVATPerQuote",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "TotalVATPerQuote",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsQuoteDataExternal",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsQuoteDataExternal",
					  						ListPropertyPath =  "IsQuoteDataExternal",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsQuoteDataExternal",
					  						DefaultText =  @"Is Quote Data External",
					  						FullLocalDefaultText =  @"IsQuoteDataExternal",
					  						ListFieldLable =  "IsQuoteDataExternalListLable",
					  						ListLableDefaultText =  @"IsQuoteDataExternal",
					  						ListLocalDefaultText =  @"IsQuoteDataExternal",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "IsQuoteDataExternal",
					  						Operator =  "Equals",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						ValidForQuerySection1 =  "Quote",
					  						IsRestrictable =  false,
					  						DigitsAfterPoint =  0,
					  						NumberOfDigits =  0,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "IsQuoteDataExternal",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsQuoteDocumentExternal",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsQuoteDocumentExternal",
					  						ListPropertyPath =  "IsQuoteDocumentExternal",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsQuoteDocumentExternal",
					  						DefaultText =  @"Is Quote Document External",
					  						FullLocalDefaultText =  @"IsQuoteDocumentExternal",
					  						ListFieldLable =  "IsQuoteDocumentExternalListLable",
					  						ListLableDefaultText =  @"IsQuoteDocumentExternal",
					  						ListLocalDefaultText =  @"IsQuoteDocumentExternal",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "IsQuoteDocumentExternal",
					  						Operator =  "Equals",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						ValidForQuerySection1 =  "Quote",
					  						IsRestrictable =  false,
					  						DigitsAfterPoint =  0,
					  						NumberOfDigits =  0,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "IsQuoteDocumentExternal",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TotalPerContainer",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TotalPerContainer",
					  						ListPropertyPath =  "TotalPerContainer",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TotalPerContainer",
					  						DefaultText =  @"Total Per Container",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "TotalPerContainer",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						ValidForQuerySection1 =  "Quote",
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HelpTextCode =  "TotalPerContainer",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "GrossWeightInKG",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Double",
					  						Code =  "GrossWeightInKG",
					  						MaxLength =  40,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "GrossWeightInKG",
					  						ListPropertyPath =  "GrossWeightInKG",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  true,
					  						IsRequired =  false,
					  						FullFieldLable =  "GrossWeightInKG",
					  						DefaultText =  @"Gross Weight (KG)",
					  						ListFieldLable =  "GrossWeightInKGListLable",
					  						ListLableDefaultText =  @"Gross Weight (KG)",
					  						HelpTextCode =  "GrossWeightInKG",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "GrossWeightPerTon",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Double",
					  						Code =  "GrossWeightPerTon",
					  						MaxLength =  40,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "GrossWeightPerTon",
					  						ListPropertyPath =  "GrossWeightPerTon",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  true,
					  						IsRequired =  false,
					  						FullFieldLable =  "GrossWeightPerTon",
					  						DefaultText =  @"Gross Weight per Ton",
					  						ListFieldLable =  "GrossWeightPerTonListLable",
					  						ListLableDefaultText =  @"Gross Weight per Ton",
					  						HelpTextCode =  "GrossWeightPerTon",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "NotifyId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						Code =  "NotifyId",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "NotifyId",
					  						ListPropertyPath =  "NotifyId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "NotifyId",
					  						DefaultText =  @"Notify",
					  						HelpTextCode =  "NotifyId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "NotifyAddressId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Address",
					  						Code =  "NotifyAddressId",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "NotifyAddressId",
					  						ListPropertyPath =  "NotifyAddressId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "NotifyAddressId",
					  						DefaultText =  @"Notify Address",
					  						HelpTextCode =  "NotifyAddressId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "NotifyContactId",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Contact",
					  						Code =  "NotifyContactId",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "NotifyContactId",
					  						ListPropertyPath =  "NotifyContactId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "NotifyContactId",
					  						DefaultText =  @"Notify Contact",
					  						HelpTextCode =  "NotifyContactId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "NotifyName",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						Code =  "NotifyName",
					  						MaxLength =  70,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  70,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "NotifyName",
					  						ListPropertyPath =  "NotifyName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "NotifyName",
					  						DefaultText =  @"Notify",
					  						ListFieldLable =  "NotifyNameListLable",
					  						ListLableDefaultText =  @"Notify",
					  						HelpTextCode =  "NotifyName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TotalSaleIncludingVATAmountInSaleCurrency",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Double",
					  						Code =  "TotalSaleIncludingVATAmountInSaleCurrency",
					  						MaxLength =  0,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "TotalSaleIncludingVATAmountInSaleCurrency",
					  						ListPropertyPath =  "TotalSaleIncludingVATAmountInSaleCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "TotalSaleIncludingVATAmountInSaleCurrency",
					  						DefaultText =  @"Total Sale Including VAT Amount In Sale Currency",
					  						HelpTextCode =  "TotalSaleIncludingVATAmountInSaleCurrency",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TotalSaleIncludingVATAmountInLocalCurrency",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Double",
					  						Code =  "TotalSaleIncludingVATAmountInLocalCurrency",
					  						MaxLength =  0,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "TotalSaleIncludingVATAmountInLocalCurrency",
					  						ListPropertyPath =  "TotalSaleIncludingVATAmountInLocalCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "TotalSaleIncludingVATAmountInLocalCurrency",
					  						DefaultText =  @"Total Sale Including VAT Amount In Local Currency",
					  						HelpTextCode =  "TotalSaleIncludingVATAmountInLocalCurrency",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "NumberOfFollowUps",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Integer",
					  						Code =  "NumberOfFollowUps",
					  						MaxLength =  0,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "NumberOfFollowUps",
					  						ListPropertyPath =  "NumberOfFollowUps",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "NumberOfFollowUps",
					  						DefaultText =  @"Follow Ups",
					  						ListFieldLable =  "NumberOfFollowUpsLable",
					  						ListLableDefaultText =  @"Number Of Follow Ups",
					  						HelpTextCode =  "NumberOfFollowUps",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MoveTypeName",
					  						ObjectTableName =  "Quote",
					  						FieldsDataType =  "Text",
					  						LookUpTableName =  "MoveType",
					  						Code =  "MoveTypeName",
					  						MaxLength =  40,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "MoveTypeName",
					  						ListPropertyPath =  "MoveTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Quote",
					  						ValidForQuerySection2 =  "QuoteFollowUp",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "MoveTypeName",
					  						DefaultText =  @"Move Type",
					  						ListFieldLable =  "MoveTypeName",
					  						ListLableDefaultText =  @"Move Type",
					  						HelpTextCode =  "MoveTypeName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup QuoteQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "QUOT", Name = "Queries" }, queryGroupRepository);
						QueryGroup QuoteQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "QFLU", Name = "Follow Ups" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable QuoteObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Quote" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> QuoteObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Quote").ToList();   

			   TextCode QuoteTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.Q.CreatedQuotes", DefaultText = @"Created Quotes",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature QuoteFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CREATEDQUOTES", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.CreatedQuotes", NameTextCodeDefaultText = "Created Quotes", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode QuoteTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.Q.DraftQuotes", DefaultText = @"Draft Quotes",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature QuoteFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DRAFTQUOTES", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.DraftQuotes", NameTextCodeDefaultText = "Draft Quotes", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode QuoteTextCode_2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.Q.SentQuotes", DefaultText = @"Sent Quotes",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature QuoteFeature_2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SENTQUOTES", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.SentQuotes", NameTextCodeDefaultText = "Sent Quotes", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode QuoteTextCode_3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.Q.ExpiredQuotes", DefaultText = @"Expired Quotes",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature QuoteFeature_3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EXPIREDQUOTES", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.ExpiredQuotes", NameTextCodeDefaultText = "Expired Quotes", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode QuoteTextCode_4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.Q.FollowUps", DefaultText = @"Follow Ups",LocalDefaultText = "Follow Ups", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature QuoteFeature_4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "FOLLOWUPS", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Master.Features.FollowUps", NameTextCodeDefaultText = "Follow Ups", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode QuoteTextCode_5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.Q.AcceptedQuotes", DefaultText = @"Accepted Quotes",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature QuoteFeature_5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCEPTEDQUOTES", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.AcceptedQuotes", NameTextCodeDefaultText = "Accepted Quotes", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode QuoteTextCode_6 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.Q.AcceptedWithoutShipmentsQuotes", DefaultText = @"Accepted Without Shipments",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature QuoteFeature_6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCEPTEDWITHOUTSHIPMENTS", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.AcceptedWithoutShipments", NameTextCodeDefaultText = "Accepted Without Shipments", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode QuoteTextCode_7 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.Q.AllQuotes", DefaultText = @"All Quotes",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature QuoteFeature_7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLQUOTES", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.AllQuotes", NameTextCodeDefaultText = "All Quotes", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode QuoteTextCode_8 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.Q.CancelledQuotes", DefaultText = @"Cancelled Quotes",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature QuoteFeature_8 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CANCELLEDQUOTES", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.CancelledQuotes", NameTextCodeDefaultText = "Cancelled Quotes", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode QuoteTextCode_9 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.Q.AllFollowUps", DefaultText = @"All Follow Ups",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature QuoteFeature_9 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QUTALLFOLLOWUPS", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.AllFollowUps", NameTextCodeDefaultText = "All FollowUps", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode QuoteTextCode_10 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.Q.MyFollowUps", DefaultText = @"My Follow Ups",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature QuoteFeature_10 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QUTMYFOLLOWUPS", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.MyFollowUps", NameTextCodeDefaultText = "My FollowUps", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode QuoteTextCode_11 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.Q.MyQuotes", DefaultText = @"My Quotes",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature QuoteFeature_11 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MYQUOTES", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.MyQuotes", NameTextCodeDefaultText = "My Quotes", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode QuoteTextCode_12 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.Q.OpenQuotes", DefaultText = @"Open Quotes",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature QuoteFeature_12 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OPENQUOTES", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.OpenQuotes", NameTextCodeDefaultText = "Open Quotes", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query CreatedQuotesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = QuoteTextCode_0.Id, Code = "Created Quotes",  QueryGroupCode = "QUOT", IndexOrder = 0, Tenant = 0, ObjectTableId = QuoteObjectTable.Id, QuerySection = "Quote", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = QuoteFeature_0.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn CreatedQuotesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CreatedQuotesQuery.Id, IndexOrder = 0, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CreatedQuotesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CreatedQuotesQuery.Id, IndexOrder = 1, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CreatedQuotesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CreatedQuotesQuery.Id, IndexOrder = 2, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "OpenDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CreatedQuotesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CreatedQuotesQuery.Id, IndexOrder = 3, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteNumber" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 84 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CreatedQuotesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CreatedQuotesQuery.Id, IndexOrder = 4, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Shipper" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CreatedQuotesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CreatedQuotesQuery.Id, IndexOrder = 5, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Consignee" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 146 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CreatedQuotesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CreatedQuotesQuery.Id, IndexOrder = 6, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "MainCarriageCarrierName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CreatedQuotesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CreatedQuotesQuery.Id, IndexOrder = 7, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FromPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CreatedQuotesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CreatedQuotesQuery.Id, IndexOrder = 8, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ToPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CreatedQuotesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CreatedQuotesQuery.Id, IndexOrder = 9, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteTypeName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CreatedQuotesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CreatedQuotesQuery.Id, IndexOrder = 10, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ShipmentType" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CreatedQuotesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CreatedQuotesQuery.Id, IndexOrder = 11, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ExpirationDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter CreatedQuotesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "IsCreatedQuote" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = CreatedQuotesQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query DraftQuotesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = QuoteTextCode_1.Id, Code = "Draft Quotes",  QueryGroupCode = "QUOT", IndexOrder = 1, Tenant = 0, ObjectTableId = QuoteObjectTable.Id, QuerySection = "Quote", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = QuoteFeature_1.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn DraftQuotesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftQuotesQuery.Id, IndexOrder = 0, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftQuotesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftQuotesQuery.Id, IndexOrder = 1, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftQuotesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftQuotesQuery.Id, IndexOrder = 2, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "OpenDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftQuotesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftQuotesQuery.Id, IndexOrder = 3, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteNumber" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 84 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftQuotesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftQuotesQuery.Id, IndexOrder = 4, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Shipper" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftQuotesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftQuotesQuery.Id, IndexOrder = 5, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Consignee" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 146 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftQuotesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftQuotesQuery.Id, IndexOrder = 6, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "MainCarriageCarrierName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftQuotesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftQuotesQuery.Id, IndexOrder = 7, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FromPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftQuotesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftQuotesQuery.Id, IndexOrder = 8, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ToPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftQuotesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftQuotesQuery.Id, IndexOrder = 9, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteTypeName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftQuotesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftQuotesQuery.Id, IndexOrder = 10, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ShipmentType" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftQuotesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftQuotesQuery.Id, IndexOrder = 11, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ExpirationDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter DraftQuotesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "IsDraftQuote" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = DraftQuotesQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query SentQuotesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = QuoteTextCode_2.Id, Code = "Sent Quotes",  QueryGroupCode = "QUOT", IndexOrder = 2, Tenant = 0, ObjectTableId = QuoteObjectTable.Id, QuerySection = "Quote", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = QuoteFeature_2.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn SentQuotesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SentQuotesQuery.Id, IndexOrder = 0, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SentQuotesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SentQuotesQuery.Id, IndexOrder = 1, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SentQuotesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SentQuotesQuery.Id, IndexOrder = 2, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "OpenDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SentQuotesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SentQuotesQuery.Id, IndexOrder = 3, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteNumber" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 84 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SentQuotesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SentQuotesQuery.Id, IndexOrder = 4, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Shipper" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SentQuotesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SentQuotesQuery.Id, IndexOrder = 5, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Consignee" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 146 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SentQuotesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SentQuotesQuery.Id, IndexOrder = 6, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "MainCarriageCarrierName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SentQuotesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SentQuotesQuery.Id, IndexOrder = 7, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FromPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SentQuotesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SentQuotesQuery.Id, IndexOrder = 8, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ToPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SentQuotesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SentQuotesQuery.Id, IndexOrder = 9, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteTypeName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SentQuotesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SentQuotesQuery.Id, IndexOrder = 10, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ShipmentType" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SentQuotesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SentQuotesQuery.Id, IndexOrder = 11, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ExpirationDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter SentQuotesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "IsSentQuote" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = SentQuotesQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query ExpiredQuotesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = QuoteTextCode_3.Id, Code = "Expired Quotes",  QueryGroupCode = "QUOT", IndexOrder = 3, Tenant = 0, ObjectTableId = QuoteObjectTable.Id, QuerySection = "Quote", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = QuoteFeature_3.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn ExpiredQuotesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExpiredQuotesQuery.Id, IndexOrder = 0, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ExpiredQuotesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExpiredQuotesQuery.Id, IndexOrder = 1, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ExpiredQuotesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExpiredQuotesQuery.Id, IndexOrder = 2, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "OpenDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ExpiredQuotesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExpiredQuotesQuery.Id, IndexOrder = 3, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ExpiredQuotesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExpiredQuotesQuery.Id, IndexOrder = 4, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteNumber" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 84 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ExpiredQuotesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExpiredQuotesQuery.Id, IndexOrder = 5, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Shipper" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ExpiredQuotesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExpiredQuotesQuery.Id, IndexOrder = 6, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Consignee" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 146 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ExpiredQuotesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExpiredQuotesQuery.Id, IndexOrder = 7, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "MainCarriageCarrierName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ExpiredQuotesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExpiredQuotesQuery.Id, IndexOrder = 8, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FromPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ExpiredQuotesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExpiredQuotesQuery.Id, IndexOrder = 9, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ToPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ExpiredQuotesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExpiredQuotesQuery.Id, IndexOrder = 10, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteTypeName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ExpiredQuotesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExpiredQuotesQuery.Id, IndexOrder = 11, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ShipmentType" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ExpiredQuotesQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExpiredQuotesQuery.Id, IndexOrder = 12, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ExpirationDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter ExpiredQuotesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "IsExpiredQuote" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = ExpiredQuotesQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query FollowUpsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = QuoteTextCode_4.Id, Code = "Follow Ups",  QueryGroupCode = "QFLU", IndexOrder = 4, Tenant = 0, ObjectTableId = QuoteObjectTable.Id, QuerySection = "QuoteFollowUp", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = QuoteFeature_4.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn FollowUpsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FollowUpsQuery.Id, IndexOrder = 1, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn FollowUpsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FollowUpsQuery.Id, IndexOrder = 2, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn FollowUpsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FollowUpsQuery.Id, IndexOrder = 3, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FollowUpDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 70 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn FollowUpsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FollowUpsQuery.Id, IndexOrder = 4, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FollowUpType" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 115 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn FollowUpsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FollowUpsQuery.Id, IndexOrder = 5, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FollowUpNotes" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn FollowUpsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FollowUpsQuery.Id, IndexOrder = 6, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Shipper" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn FollowUpsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FollowUpsQuery.Id, IndexOrder = 7, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FromPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn FollowUpsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FollowUpsQuery.Id, IndexOrder = 8, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ToPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn FollowUpsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FollowUpsQuery.Id, IndexOrder = 9, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ExpirationDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn FollowUpsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FollowUpsQuery.Id, IndexOrder = 10, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteNumber" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 84 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn FollowUpsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FollowUpsQuery.Id, IndexOrder = 11, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ShipmentType" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn FollowUpsQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FollowUpsQuery.Id, IndexOrder = 12, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FollowUpOwner" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);
  
	      

			  Query AcceptedQuotesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = QuoteTextCode_5.Id, Code = "Accepted Quotes",  QueryGroupCode = "QUOT", IndexOrder = 4, Tenant = 0, ObjectTableId = QuoteObjectTable.Id, QuerySection = "Quote", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = QuoteFeature_5.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AcceptedQuotesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedQuotesQuery.Id, IndexOrder = 0, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AcceptedQuotesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedQuotesQuery.Id, IndexOrder = 1, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AcceptedQuotesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedQuotesQuery.Id, IndexOrder = 2, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "OpenDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AcceptedQuotesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedQuotesQuery.Id, IndexOrder = 3, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteNumber" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 84 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AcceptedQuotesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedQuotesQuery.Id, IndexOrder = 4, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Shipper" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AcceptedQuotesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedQuotesQuery.Id, IndexOrder = 5, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Consignee" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 146 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AcceptedQuotesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedQuotesQuery.Id, IndexOrder = 6, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "MainCarriageCarrierName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AcceptedQuotesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedQuotesQuery.Id, IndexOrder = 7, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FromPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AcceptedQuotesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedQuotesQuery.Id, IndexOrder = 8, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ToPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AcceptedQuotesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedQuotesQuery.Id, IndexOrder = 9, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteTypeName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AcceptedQuotesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedQuotesQuery.Id, IndexOrder = 10, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ShipmentType" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AcceptedQuotesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedQuotesQuery.Id, IndexOrder = 11, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ExpirationDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter AcceptedQuotesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "IsAcceptedQuote" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = AcceptedQuotesQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query AcceptedWithoutShipmentsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = QuoteTextCode_6.Id, Code = "Accepted Without Shipments",  QueryGroupCode = "QUOT", IndexOrder = 5, Tenant = 0, ObjectTableId = QuoteObjectTable.Id, QuerySection = "Quote", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = QuoteFeature_6.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AcceptedWithoutShipmentsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedWithoutShipmentsQuery.Id, IndexOrder = 0, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AcceptedWithoutShipmentsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedWithoutShipmentsQuery.Id, IndexOrder = 1, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AcceptedWithoutShipmentsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedWithoutShipmentsQuery.Id, IndexOrder = 2, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "OpenDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AcceptedWithoutShipmentsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedWithoutShipmentsQuery.Id, IndexOrder = 3, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteNumber" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 84 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AcceptedWithoutShipmentsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedWithoutShipmentsQuery.Id, IndexOrder = 4, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Shipper" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AcceptedWithoutShipmentsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedWithoutShipmentsQuery.Id, IndexOrder = 5, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Consignee" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 146 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AcceptedWithoutShipmentsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedWithoutShipmentsQuery.Id, IndexOrder = 6, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "MainCarriageCarrierName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AcceptedWithoutShipmentsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedWithoutShipmentsQuery.Id, IndexOrder = 7, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FromPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AcceptedWithoutShipmentsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedWithoutShipmentsQuery.Id, IndexOrder = 8, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ToPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AcceptedWithoutShipmentsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedWithoutShipmentsQuery.Id, IndexOrder = 9, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteTypeName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AcceptedWithoutShipmentsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedWithoutShipmentsQuery.Id, IndexOrder = 10, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ShipmentType" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AcceptedWithoutShipmentsQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AcceptedWithoutShipmentsQuery.Id, IndexOrder = 11, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ExpirationDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter AcceptedWithoutShipmentsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "AcceptedWithoutShipments" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = AcceptedWithoutShipmentsQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query AllQuotesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = QuoteTextCode_7.Id, Code = "All Quotes",  QueryGroupCode = "QUOT", IndexOrder = 6, Tenant = 0, ObjectTableId = QuoteObjectTable.Id, QuerySection = "Quote", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = QuoteFeature_7.Id, DefaultSortName = "OpenDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
	
			 QueryColumn AllQuotesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuotesQuery.Id, IndexOrder = 0, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllQuotesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuotesQuery.Id, IndexOrder = 1, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllQuotesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuotesQuery.Id, IndexOrder = 2, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "OpenDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllQuotesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuotesQuery.Id, IndexOrder = 3, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllQuotesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuotesQuery.Id, IndexOrder = 4, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteNumber" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 84 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllQuotesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuotesQuery.Id, IndexOrder = 5, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Shipper" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllQuotesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuotesQuery.Id, IndexOrder = 6, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "MainCarriageCarrierName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllQuotesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuotesQuery.Id, IndexOrder = 7, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FromPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllQuotesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuotesQuery.Id, IndexOrder = 8, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ToPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllQuotesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuotesQuery.Id, IndexOrder = 9, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteTypeName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllQuotesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuotesQuery.Id, IndexOrder = 10, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ShipmentType" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllQuotesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllQuotesQuery.Id, IndexOrder = 11, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ExpirationDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);
  
	      

			  Query CancelledQuotesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = QuoteTextCode_8.Id, Code = "Cancelled Quotes",  QueryGroupCode = "QUOT", IndexOrder = 7, Tenant = 0, ObjectTableId = QuoteObjectTable.Id, QuerySection = "Quote", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = QuoteFeature_8.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn CancelledQuotesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledQuotesQuery.Id, IndexOrder = 0, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledQuotesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledQuotesQuery.Id, IndexOrder = 1, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledQuotesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledQuotesQuery.Id, IndexOrder = 2, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "OpenDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledQuotesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledQuotesQuery.Id, IndexOrder = 3, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledQuotesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledQuotesQuery.Id, IndexOrder = 4, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteNumber" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 84 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledQuotesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledQuotesQuery.Id, IndexOrder = 5, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Shipper" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledQuotesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledQuotesQuery.Id, IndexOrder = 6, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Consignee" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 146 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledQuotesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledQuotesQuery.Id, IndexOrder = 7, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "MainCarriageCarrierName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledQuotesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledQuotesQuery.Id, IndexOrder = 8, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FromPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledQuotesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledQuotesQuery.Id, IndexOrder = 9, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ToPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledQuotesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledQuotesQuery.Id, IndexOrder = 10, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteTypeName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledQuotesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledQuotesQuery.Id, IndexOrder = 11, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ShipmentType" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledQuotesQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledQuotesQuery.Id, IndexOrder = 12, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ExpirationDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter CancelledQuotesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "IsCancelled" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = CancelledQuotesQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query AllFollowUpsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = QuoteTextCode_9.Id, Code = "All Follow Ups",  QueryGroupCode = "QFLU", IndexOrder = 8, Tenant = 0, ObjectTableId = QuoteObjectTable.Id, QuerySection = "QuoteFollowUp", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = QuoteFeature_9.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllFollowUpsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFollowUpsQuery.Id, IndexOrder = 0, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllFollowUpsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFollowUpsQuery.Id, IndexOrder = 1, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllFollowUpsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFollowUpsQuery.Id, IndexOrder = 2, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllFollowUpsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFollowUpsQuery.Id, IndexOrder = 3, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FollowUpDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 70 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllFollowUpsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFollowUpsQuery.Id, IndexOrder = 4, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FollowUpType" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 115 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllFollowUpsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFollowUpsQuery.Id, IndexOrder = 5, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FollowUpNotes" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllFollowUpsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFollowUpsQuery.Id, IndexOrder = 6, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Shipper" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllFollowUpsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFollowUpsQuery.Id, IndexOrder = 7, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FromPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllFollowUpsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFollowUpsQuery.Id, IndexOrder = 8, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ToPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllFollowUpsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFollowUpsQuery.Id, IndexOrder = 9, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ExpirationDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllFollowUpsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFollowUpsQuery.Id, IndexOrder = 10, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteNumber" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 84 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllFollowUpsQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFollowUpsQuery.Id, IndexOrder = 11, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ShipmentType" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllFollowUpsQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFollowUpsQuery.Id, IndexOrder = 12, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FollowUpOwner" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter AllFollowUpsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "AllFollowUps" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "12/6/2018 12:00:00 AM",PredefinedValue2 = null, QueryId = AllFollowUpsQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query MyFollowUpsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = QuoteTextCode_10.Id, Code = "My Follow Ups",  QueryGroupCode = "QFLU", IndexOrder = 9, Tenant = 0, ObjectTableId = QuoteObjectTable.Id, QuerySection = "QuoteFollowUp", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = QuoteFeature_10.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn MyFollowUpsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyFollowUpsQuery.Id, IndexOrder = 0, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyFollowUpsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyFollowUpsQuery.Id, IndexOrder = 1, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyFollowUpsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyFollowUpsQuery.Id, IndexOrder = 2, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyFollowUpsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyFollowUpsQuery.Id, IndexOrder = 3, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FollowUpDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 70 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyFollowUpsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyFollowUpsQuery.Id, IndexOrder = 4, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FollowUpType" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 115 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyFollowUpsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyFollowUpsQuery.Id, IndexOrder = 5, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FollowUpNotes" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyFollowUpsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyFollowUpsQuery.Id, IndexOrder = 6, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Shipper" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyFollowUpsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyFollowUpsQuery.Id, IndexOrder = 7, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FromPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyFollowUpsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyFollowUpsQuery.Id, IndexOrder = 8, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ToPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyFollowUpsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyFollowUpsQuery.Id, IndexOrder = 9, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ExpirationDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyFollowUpsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyFollowUpsQuery.Id, IndexOrder = 10, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteNumber" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 84 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyFollowUpsQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyFollowUpsQuery.Id, IndexOrder = 11, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ShipmentType" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyFollowUpsQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyFollowUpsQuery.Id, IndexOrder = 12, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FollowUpOwner" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter MyFollowUpsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "MyFollowUps" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "12/6/2018 12:00:00 AM",PredefinedValue2 = null, QueryId = MyFollowUpsQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query MyQuotesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = QuoteTextCode_11.Id, Code = "My Quotes",  QueryGroupCode = "QUOT", IndexOrder = 10, Tenant = 0, ObjectTableId = QuoteObjectTable.Id, QuerySection = "Quote", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = QuoteFeature_11.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn MyQuotesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyQuotesQuery.Id, IndexOrder = 0, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyQuotesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyQuotesQuery.Id, IndexOrder = 1, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyQuotesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyQuotesQuery.Id, IndexOrder = 2, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "OpenDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyQuotesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyQuotesQuery.Id, IndexOrder = 3, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyQuotesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyQuotesQuery.Id, IndexOrder = 4, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteNumber" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 84 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyQuotesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyQuotesQuery.Id, IndexOrder = 5, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Shipper" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyQuotesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyQuotesQuery.Id, IndexOrder = 6, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "MainCarriageCarrierName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyQuotesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyQuotesQuery.Id, IndexOrder = 7, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FromPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyQuotesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyQuotesQuery.Id, IndexOrder = 8, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ToPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyQuotesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyQuotesQuery.Id, IndexOrder = 9, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteTypeName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyQuotesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyQuotesQuery.Id, IndexOrder = 10, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ShipmentType" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyQuotesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyQuotesQuery.Id, IndexOrder = 11, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ExpirationDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter MyQuotesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "MyQuotes" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = MyQuotesQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query OpenQuotesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = QuoteTextCode_12.Id, Code = "Open Quotes",  QueryGroupCode = "QUOT", IndexOrder = 11, Tenant = 0, ObjectTableId = QuoteObjectTable.Id, QuerySection = "Quote", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = QuoteFeature_12.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn OpenQuotesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenQuotesQuery.Id, IndexOrder = 0, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenQuotesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenQuotesQuery.Id, IndexOrder = 1, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenQuotesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenQuotesQuery.Id, IndexOrder = 2, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "OpenDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenQuotesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenQuotesQuery.Id, IndexOrder = 3, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "StageName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenQuotesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenQuotesQuery.Id, IndexOrder = 4, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteNumber" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 84 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenQuotesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenQuotesQuery.Id, IndexOrder = 5, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Shipper" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenQuotesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenQuotesQuery.Id, IndexOrder = 6, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "MainCarriageCarrierName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenQuotesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenQuotesQuery.Id, IndexOrder = 7, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "FromPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenQuotesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenQuotesQuery.Id, IndexOrder = 8, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ToPort" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 40 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenQuotesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenQuotesQuery.Id, IndexOrder = 9, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteTypeName" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 127 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenQuotesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenQuotesQuery.Id, IndexOrder = 10, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ShipmentType" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenQuotesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenQuotesQuery.Id, IndexOrder = 11, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ExpirationDate" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 92 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter OpenQuotesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "OpenQuotes" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = OpenQuotesQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable QuoteObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Quote" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> QuoteObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Quote").ToList();
		       
	      

	         Screen QuoteGeneralTabScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Quote.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = QuoteObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 9, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField QuoteQuoteGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "SalesmanUserId").FirstOrDefault().Id, ScreenId = QuoteGeneralTabScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField QuoteQuoteGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "CreatedByUserId").FirstOrDefault().Id, ScreenId = QuoteGeneralTabScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField QuoteQuoteGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "DepartmentId").FirstOrDefault().Id, ScreenId = QuoteGeneralTabScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField QuoteQuoteGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "BranchId").FirstOrDefault().Id, ScreenId = QuoteGeneralTabScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField QuoteQuoteGeneralTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 4, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ValueOfGoods").FirstOrDefault().Id, ScreenId = QuoteGeneralTabScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField QuoteQuoteGeneralTabScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 5, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ValueOfGoodsCurrencyId").FirstOrDefault().Id, ScreenId = QuoteGeneralTabScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           
	      

	         Screen QuoteHeaderScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Quote.HeaderScreen", Name = "Header Screen", ObjectTableId = QuoteObjectTable.Id, NumberOfColumns = 4, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField QuoteQuoteHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "QuoteTypeName").FirstOrDefault().Id, ScreenId = QuoteHeaderScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField QuoteQuoteHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "IncotermCode").FirstOrDefault().Id, ScreenId = QuoteHeaderScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField QuoteQuoteHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Subject").FirstOrDefault().Id, ScreenId = QuoteHeaderScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField QuoteQuoteHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Routing").FirstOrDefault().Id, ScreenId = QuoteHeaderScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField QuoteQuoteHeaderScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "LastUsageDate").FirstOrDefault().Id, ScreenId = QuoteHeaderScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField QuoteQuoteHeaderScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 1, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ExpirationDate").FirstOrDefault().Id, ScreenId = QuoteHeaderScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField QuoteQuoteHeaderScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "SalesmanName").FirstOrDefault().Id, ScreenId = QuoteHeaderScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField QuoteQuoteHeaderScreenScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 1, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "StageName").FirstOrDefault().Id, ScreenId = QuoteHeaderScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    QuoteObjectTable.HeaderScreenId = QuoteHeaderScreenScreen1.Id;
	   		  
	      

	         Screen QuoteNewQuoteScreen2 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "NewQuote", Name = "New Quote", ObjectTableId = QuoteObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 3, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField QuoteNewQuoteScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ShipperReference2").FirstOrDefault().Id, ScreenId = QuoteNewQuoteScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField QuoteNewQuoteScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "ConsigneeReference2").FirstOrDefault().Id, ScreenId = QuoteNewQuoteScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField QuoteNewQuoteScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "AgentId").FirstOrDefault().Id, ScreenId = QuoteNewQuoteScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField QuoteNewQuoteScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = QuoteObjectFields.Where(d => d.FieldName == "Notes").FirstOrDefault().Id, ScreenId = QuoteNewQuoteScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {    
			 ObjectTable QuoteObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Quote" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode QuoteOverviewTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.TH.Overview", DefaultText = "Overview",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature QuoteOverviewFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OVERVIEW", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.Overview", NameTextCodeDefaultText = "Overview", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode QuoteDetailsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.TH.Details", DefaultText = "Details",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature QuoteDetailsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DETAILS", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.Details", NameTextCodeDefaultText = "Details", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode QuotePartnersTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.TH.Partners", DefaultText = "Partners",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature QuotePartnersFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PARTNERS", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.Partners", NameTextCodeDefaultText = "Partners", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode QuotePackagesTextCode_TH3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.TH.Packages", DefaultText = "Packages",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature QuotePackagesFeature_TH3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PACKAGES", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.Packages", NameTextCodeDefaultText = "Packages", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode QuoteRoutingsTextCode_TH4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.TH.Routings", DefaultText = "Routings",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature QuoteRoutingsFeature_TH4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ROUTINGS", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.Routings", NameTextCodeDefaultText = "Routings", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode QuoteChargesTextCode_TH5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.TH.Charges", DefaultText = "Charges",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature QuoteChargesFeature_TH5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CHARGES", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.Charges", NameTextCodeDefaultText = "Charges", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode QuoteDocsOutTextCode_TH6 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.TH.DocsOut", DefaultText = "Docs Out",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature QuoteDocsOutFeature_TH6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DOCSOUT", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.DocsOut", NameTextCodeDefaultText = "DocsOut", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode QuoteDocsInTextCode_TH7 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.TH.DocsIn", DefaultText = "Docs In",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature QuoteDocsInFeature_TH7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DOCSIN", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.DocsIn", NameTextCodeDefaultText = "DocsIn", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode QuoteEventsTextCode_TH8 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature QuoteEventsFeature_TH8 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode QuoteCommunicationsTextCode_TH9 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.TH.Communications", DefaultText = "Communications",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature QuoteCommunicationsFeature_TH9 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "COMMUNICATION", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.Communication", NameTextCodeDefaultText = "Communications", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode QuoteConnectedEntitiesTextCode_TH10 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.TH.ConnectedEntities", DefaultText = "Connected Entities",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature QuoteConnectedEntitiesFeature_TH10 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CONNECTEDENTITIES", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.ConnectedEntities", NameTextCodeDefaultText = "Connected Entities", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "QTOV",HtmlComponentName = "OverviewTabComponent",HtmlComponentUrl = "./QuoteModules/QuoteTabs/Components/Overview/OverviewTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "OVERVIEW" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.QuoteLib.Views.OverView.OverViewTabControl", ObjectTableId = QuoteObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Quote.TH.Overview" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "QTOR",HtmlComponentName = "OrdersabComponent",HtmlComponentUrl = "./QuoteModules/QuoteTabs/Components/Orders/OrdersTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "DETAILS" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.QuoteLib.Views.Orders.OrdersControl", ObjectTableId = QuoteObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Quote.TH.Details" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "QTPA",HtmlComponentName = "PartnersTabComponent",HtmlComponentUrl = "./QuoteModules/QuoteTabs/Components/Partners/PartnersTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "PARTNERS" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.QuoteLib.Views.Partners.PartnersControl", ObjectTableId = QuoteObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Quote.TH.Partners" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "QTPK",HtmlComponentName = "PackagesTabComponent",HtmlComponentUrl = "./QuoteModules/QuoteTabs/Components/Packages/PackagesTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "PACKAGES" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.QuoteLib.Views.Packages.PackagesControl", ObjectTableId = QuoteObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Quote.TH.Packages" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 3 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "QTRT",HtmlComponentName = "RoutingsTabComponent",HtmlComponentUrl = "./QuoteModules/QuoteTabs/Components/Routings/RoutingsTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "ROUTINGS" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.QuoteLib.Views.Routings.RoutingsControl", ObjectTableId = QuoteObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Quote.TH.Routings" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 4 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "QTCH",HtmlComponentName = "ChargesTabComponent",HtmlComponentUrl = "./QuoteModules/QuoteCharges/Components/ChargesTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "CHARGES" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.QuoteLib.Views.Charges.ChargesUserControl", ObjectTableId = QuoteObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Quote.TH.Charges" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 5 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "QTDO",HtmlComponentName = "QuoteDocsOutTabComponent",HtmlComponentUrl = "./QuoteModules/QuoteTabs/Components/DocsOut/QuoteDocsOutTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "DOCSOUT" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.QuoteLib.Views.Documents.DocumentOutsControl", ObjectTableId = QuoteObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Quote.TH.DocsOut" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 6 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "QTDI",HtmlComponentName = "QuoteDocsInTabComponent",HtmlComponentUrl = "./QuoteModules/QuoteTabs/Components/DocsIn/QuoteDocsInTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "DOCSIN" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.QuoteLib.Views.Documents.DocumentInsControl", ObjectTableId = QuoteObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Quote.TH.DocsIn" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 7 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "QTEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "EVENTS" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = QuoteObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Quote.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 8 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "QTCM",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "COMMUNICATION" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.Views.Communications.CommunicationsControl", ObjectTableId = QuoteObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Quote.TH.Communications" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 9 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "QTCE",HtmlComponentName = "ConnectionsTabComponent",HtmlComponentUrl = "./QuoteModules/QuoteTabs/Components/Connections/ConnectionsTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "CONNECTEDENTITIES" && d.ObjectTableId == QuoteObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.QuoteLib.Views.Shipments.ShipmentsControl", ObjectTableId = QuoteObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Quote.TH.ConnectedEntities" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 10 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable QuoteObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Quote" && d.Tenant == 0).FirstOrDefault(); 
		   Feature QuoteFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature QuoteFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature QuoteFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature QuoteFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.PackageFeature", NameTextCodeDefaultText = "Quote Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes); 

		   		   //--------------> Additional Features <--------------\\

		   Feature QuoteFeature_SETTINGS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SETTINGS", FeatureTypeCode = "AREA", Packagable = false, IsBusinessUnitEnabled = false, IsOld = true, IsCoreFeature = false, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.Settings", NameTextCodeDefaultText = @"Settings" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature QuoteFeature_Quote_Followups = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Quote.Followups", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.Followups", NameTextCodeDefaultText = @"Follow ups" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature QuoteFeature_NEWQUOTE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEWQUOTE", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.NewQuote", NameTextCodeDefaultText = @"New Quote" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature QuoteFeature_OUTLOOKCONNETION = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OUTLOOKCONNETION", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.OutlookConnection", NameTextCodeDefaultText = @"Outlook Connection" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature QuoteFeature_TARIFFS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TARIFFS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.Tariffs", NameTextCodeDefaultText = @"Tariffs" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature QuoteFeature_QouteEditExchangeRate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QouteEditExchangeRate", FeatureTypeCode = "ACT", Packagable = false, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.EditExchangeRate", NameTextCodeDefaultText = @"Edit Exchange Rate" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable QuoteObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Quote" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPQT",
                ShortView =  false,
                IsManualEntry =  false,
                LocalName =  "Quote Updated",
                EnglishName =  "Quote Updated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = QuoteObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "DCQT",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Quote Disconnected",
                EnglishName =  "Quote Disconnected",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = QuoteObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "QTCN",
                ShortView =  false,
                IsManualEntry =  false,
                LocalName =  "Ticket Connected",
                EnglishName =  "Ticket Connected",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = QuoteObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "QTDC",
                ShortView =  false,
                IsManualEntry =  false,
                LocalName =  "Ticket Disconnected",
                EnglishName =  "Ticket Disconnected",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = QuoteObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "SASC",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Quote Sent",
                EnglishName =  "Quote Sent",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = QuoteObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CLQT",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Cancel Quote",
                EnglishName =  "Cancel Quote",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = QuoteObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CFAQ",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Copied from another Quote",
                EnglishName =  "Copied from another Quote",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = QuoteObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "RAQT",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Reactivate Quote",
                EnglishName =  "Reactivate Quote",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = QuoteObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRQT",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Created",
                EnglishName =  "Created",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = QuoteObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "REMF",
                ShortView =  true,
                IsManualEntry =  true,
                LocalName =  "Reminder",
                EnglishName =  "Reminder",
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  true,
                IsFollowUp =  true,
                FollowUpEnglishName =  "Reminder",
                FollowUpLocalName =  "Reminder",
                ObjectTableId = QuoteObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "QTVI",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Quote Viewed",
                EnglishName =  "Quote Viewed",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = QuoteObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "QTDS",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Quote In Discussion",
                EnglishName =  "Quote In Discussion",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = QuoteObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "QTCP",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Quote Accepted",
                EnglishName =  "Quote Accepted",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  true,
                IsFollowUp =  true,
                FollowUpEnglishName =  "Customer Accepted",
                FollowUpLocalName =  "Customer Accepted",
                ObjectTableId = QuoteObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "QTDL",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Quote Declined",
                EnglishName =  "Quote Declined",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = QuoteObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "RQTD",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Return To Draft",
                EnglishName =  "Return To Draft",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = QuoteObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "TRQR",
                ShortView =  true,
                IsManualEntry =  true,
                LocalName =  "Transportation Quote Received",
                EnglishName =  "Transportation Quote Received",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  true,
                IsFollowUp =  true,
                FollowUpEnglishName =  "Transportation Quote",
                FollowUpLocalName =  "Transportation Quote",
                ObjectTableId = QuoteObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "QEMO",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Email out sent",
                EnglishName =  "Quote email out sent",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = QuoteObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "QACR",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Activity Created",
                EnglishName =  "Activity Created",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = QuoteObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "QACM",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Activity Completed",
                EnglishName =  "Activity Completed",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = QuoteObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "QARP",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Activity Reopened",
                EnglishName =  "Activity Reopened",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = QuoteObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "QUSG",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Stage Due Date updated",
                EnglishName =  "Stage Due Date updated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = QuoteObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "EXPD",
                ShortView =  true,
                IsManualEntry =  true,
                LocalName =  "Quote Expired",
                EnglishName =  "Quote Expired",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  true,
                IsFollowUp =  true,
                FollowUpEnglishName =  "Quote Expired",
                FollowUpLocalName =  "Quote Expired",
                ObjectTableId = QuoteObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   FeatureRepository featureRepository = new FeatureRepository(0); 
		   List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable QuoteObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Quote" && d.Tenant == 0).FirstOrDefault();       
    
			   Feature QuoteFeature_MB0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QUOTEACCEPTED", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.Accepted", NameTextCodeDefaultText = "Accepted", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
			   Feature QuoteFeature_MB1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QUOTEDECLINED", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.Declined", NameTextCodeDefaultText = "Declined", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
			   Feature QuoteFeature_MB2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BUILDSHIPMENT", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.BuildShipment", NameTextCodeDefaultText = "Build Shipment", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
			   Feature QuoteFeature_MB3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QUOTEQUOTATION", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.Quotation", NameTextCodeDefaultText = "Quotation", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

			   Feature QuoteFeature_MB40 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SETASSENT", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.SetAsSent", NameTextCodeDefaultText = "Set As Sent to Customer", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature QuoteFeature_MB41 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RETURNTODRAFT", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.ReturnToDraft", NameTextCodeDefaultText = "Return Quote To Draft", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature QuoteFeature_MB42 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "COPY", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.Copy", NameTextCodeDefaultText = "Copy Quote", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature QuoteFeature_MB43 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CANCEL", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.Cancel", NameTextCodeDefaultText = "Cancel Quote", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature QuoteFeature_MB44 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "REACTIVATE", ObjectTableId = QuoteObjectTable.Id, Tenant = 0, NameTextCodeCode = "Quote.Features.Reactivate", NameTextCodeDefaultText = "Reactivate Quote", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
              

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup QuoteMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "QuoteEdit",
					Name = "QuoteEditButtonsGroup",
					ObjectTableId = QuoteObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton QuoteMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Accept",
						Index = 12, 
						IsActive = true,
						LabelTextCodeCode = "Quote.B.Accept",
						LabelTextCodeDefaultText = "Accept",
						Tenant = 0,
						MenuButtonGroupId = QuoteMenuButtonGroup.Id,
						ObjectTableId = QuoteObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = QuoteFeature_MB0.Id,
						Style = "ApproveButtonStyle",
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton QuoteMenuButton1 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Decline",
						Index = 12, 
						IsActive = true,
						LabelTextCodeCode = "Quote.B.Decline",
						LabelTextCodeDefaultText = "Decline",
						Tenant = 0,
						MenuButtonGroupId = QuoteMenuButtonGroup.Id,
						ObjectTableId = QuoteObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = QuoteFeature_MB1.Id,
						Style = "RedButtonStyle",
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton QuoteMenuButton2 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "BuildShipment",
						Index = 13, 
						IsActive = true,
						LabelTextCodeCode = "Quote.B.BuildShipment",
						LabelTextCodeDefaultText = "Build Shipment",
						Tenant = 0,
						MenuButtonGroupId = QuoteMenuButtonGroup.Id,
						ObjectTableId = QuoteObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = QuoteFeature_MB2.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton QuoteMenuButton3 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Quotation",
						Index = 13, 
						IsActive = true,
						LabelTextCodeCode = "Quote.B.Quotation",
						LabelTextCodeDefaultText = "Quotation",
						Tenant = 0,
						MenuButtonGroupId = QuoteMenuButtonGroup.Id,
						ObjectTableId = QuoteObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = QuoteFeature_MB3.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton QuoteMenuButton4 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Actions",
						Index = 100, 
						IsActive = false,
						LabelTextCodeCode = "Quote.B.Actions",
						LabelTextCodeDefaultText = "More",
						Tenant = 0,
						MenuButtonGroupId = QuoteMenuButtonGroup.Id,
						ObjectTableId = QuoteObjectTable.Id,
						MenuButtonType = "dropdownbutton",
						FeatureId = null,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

			   MenuButton QuoteMenuButton40 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "SetAsSentToCustomer",
						Index = 6, 
						IsActive = true,
						LabelTextCodeCode = "Quote.B.SetAsSent",
						LabelTextCodeDefaultText = "Set As Sent to Customer",
						Tenant = 0,
						MenuButtonGroupId = QuoteMenuButtonGroup.Id,
						ParentMenuButtonId = QuoteMenuButton4.Id,
						ObjectTableId = QuoteObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  QuoteFeature_MB40.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton QuoteMenuButton41 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "ReturnToDraft",
						Index = 7, 
						IsActive = true,
						LabelTextCodeCode = "Quote.B.ReturnToDraft",
						LabelTextCodeDefaultText = "Return Quote To Draft",
						Tenant = 0,
						MenuButtonGroupId = QuoteMenuButtonGroup.Id,
						ParentMenuButtonId = QuoteMenuButton4.Id,
						ObjectTableId = QuoteObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  QuoteFeature_MB41.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton QuoteMenuButton42 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "CopyQuote",
						Index = 8, 
						IsActive = true,
						LabelTextCodeCode = "Quote.B.CopyQuote",
						LabelTextCodeDefaultText = "Copy Quote",
						Tenant = 0,
						MenuButtonGroupId = QuoteMenuButtonGroup.Id,
						ParentMenuButtonId = QuoteMenuButton4.Id,
						ObjectTableId = QuoteObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  QuoteFeature_MB42.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton QuoteMenuButton43 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "QuoteCopySeparator",
						Index = 9, 
						IsActive = false,
						LabelTextCodeCode = "Quote.B.QuoteCopySeparator",
						LabelTextCodeDefaultText = "",
						Tenant = 0,
						MenuButtonGroupId = QuoteMenuButtonGroup.Id,
						ParentMenuButtonId = QuoteMenuButton4.Id,
						ObjectTableId = QuoteObjectTable.Id,
						MenuButtonType = "separator",
						
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton QuoteMenuButton44 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "CancelQuote",
						Index = 10, 
						IsActive = true,
						LabelTextCodeCode = "Quote.B.CancelQuote",
						LabelTextCodeDefaultText = "Cancel Quote",
						Tenant = 0,
						MenuButtonGroupId = QuoteMenuButtonGroup.Id,
						ParentMenuButtonId = QuoteMenuButton4.Id,
						ObjectTableId = QuoteObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  QuoteFeature_MB43.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton QuoteMenuButton45 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "ReactivateQuote",
						Index = 11, 
						IsActive = true,
						LabelTextCodeCode = "Quote.B.ReactivateQuote",
						LabelTextCodeDefaultText = "Reactivate Quote",
						Tenant = 0,
						MenuButtonGroupId = QuoteMenuButtonGroup.Id,
						ParentMenuButtonId = QuoteMenuButton4.Id,
						ObjectTableId = QuoteObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  QuoteFeature_MB44.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable QuoteObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Quote" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode QuoteTextCode_Quote = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote", DefaultText = "Quote",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "T", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBPartnersAddPartners = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.Partners.AddPartners", DefaultText = "Add Partners",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOFollowUps = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.FollowUps", DefaultText = "Follow Ups",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSDetailsDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Details.Details", DefaultText = "Details",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOPartnersPartners = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Partners.Partners", DefaultText = "Partners",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesCharges = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.Charges", DefaultText = "Charges",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteSelectFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.SelectFields", DefaultText = "Select Fields to Copy",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuotePartners = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.Partners", DefaultText = "Partners",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteMainCarriage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.MainCarriage", DefaultText = "Main Carriage",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteIncludePickUp = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.IncludePickUp", DefaultText = "Include PickUp",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteIncludeDelivery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.IncludeDelivery", DefaultText = "Include Delivery",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteCharges = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.Charges", DefaultText = "Charges",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteChargeType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.ChargeType", DefaultText = "Charge Type",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteCost = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.Cost", DefaultText = "Cost",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteSale = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.Sale", DefaultText = "Sale",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteSetAsMyCustomer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.SetAsMyCustomer", DefaultText = "Set as My Customer",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuotePotentialShipper = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.PotentialShipper", DefaultText = "Potential Shipper",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuotePotentialConsignee = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.PotentialConsignee", DefaultText = "Potential Consignee",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBDetailsEditAddress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.Details.EditAddress", DefaultText = "Edit Address",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBChargesByPriceBreak = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.Charges.ByPriceBreak", DefaultText = "Price by break",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBChargesAddStep = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.Charges.AddStep", DefaultText = "Add Break",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBChargesEditStep = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.Charges.EditStep", DefaultText = "Edit Break",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBChargesDeleteStep = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.Charges.DeleteStep", DefaultText = "Delete Break",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteMYouCanTypeSalePrice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.M.YouCanTypeSalePrice", DefaultText = "You can type Sale Price ex:(1000)%nOr%nYou can type Sale Markup ex:(+100) Or (+10%)",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteMUnableToDoAllIn = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.M.UnableToDoAllIn", DefaultText = "Unable to do All In..",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteMIfMatchesFrieghtCharge = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.M.IfMatchesFrieghtCharge", DefaultText = "Only if matches the Frieght Charge UOM and Currency",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSChargesProfitInLocalCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Charges.ProfitInLocalCurrency", DefaultText = "Profit in Local Currency",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesCostMinimum = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.CostMinimum", DefaultText = "Cost Minimum",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesSaleMinimum = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.SaleMinimum", DefaultText = "Sale Minimum",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBDetailsMeasurmentsSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.Details.MeasurmentsSettings", DefaultText = "Measurement Settings",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBDetailsHideMeasurmentsSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.Details.HideMeasurmentsSettings", DefaultText = "Hide Measurement Settings",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBPartnersAddShipper = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.Partners.AddShipper", DefaultText = "Add Shipper",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBPartnersAddConsignee = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.Partners.AddConsignee", DefaultText = "Add Consignee",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBPartnersAddPotentialShipper = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.Partners.AddPotentialShipper", DefaultText = "Add Potential Shipper",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBPartnersAddPotentialConsignee = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.Partners.AddPotentialConsignee", DefaultText = "Add Potential Consignee",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBChargesAddCharge = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.Charges.AddCharge", DefaultText = "Add Charge",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBChargesDeleteCharge = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.Charges.DeleteCharge", DefaultText = "Delete Charge",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBNewQuoteAddShipper = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.NewQuote.AddShipper", DefaultText = "Add Shipper",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBNewQuoteAddConsignee = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.NewQuote.AddConsignee", DefaultText = "Add Consignee",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBNewQuoteAddPotentialShipper = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.NewQuote.AddPotentialShipper", DefaultText = "Add Potential Shipper",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBNewQuoteAddPotentialConsignee = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.NewQuote.AddPotentialConsignee", DefaultText = "Add Potential Consignee",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteMDeleteThisPartner = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.M.DeleteThisPartner", DefaultText = "Delete This Partner?",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteMDeleteThisCharge = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.M.DeleteThisCharge", DefaultText = "Delete This Charge?",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSDetailsRoutingsDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Details.RoutingsDetails", DefaultText = "Routings Details",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSDetailsQuoteDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Details.QuoteDetails", DefaultText = "Quote Details",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSDetailsExpectedOrderDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Details.ExpectedOrderDetails", DefaultText = "Expected Order Details",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSDetailsQuoteContainersTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Details.QuoteContainersTypes", DefaultText = "Quote Containers Types",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSDetailsAdhoc = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Details.Adhoc", DefaultText = "Ad hoc",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSDetailsPeriodical = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Details.Periodical", DefaultText = "Periodical",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSDetailsGateway = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Details.Gateway", DefaultText = "Gateway",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSDetailsLoadingPort = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Details.LoadingPort", DefaultText = "Loading Port",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSDetailsDestination = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Details.Destination", DefaultText = "Destination",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSDetailsDischargePort = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Details.DischargePort", DefaultText = "Discharge Port",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSDetailsAirline = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Details.Airline", DefaultText = "Airline",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSDetailsShippingline = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Details.Shippingline", DefaultText = "Shipping line",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSDetailsTrucker = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Details.Trucker", DefaultText = "Trucker",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSPartnersName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Partners.Name", DefaultText = "Name",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSPartnersAddress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Partners.Address", DefaultText = "Address",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSPartnersContact = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Partners.Contact", DefaultText = "Contact",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSPartnersReference1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Partners.Reference1", DefaultText = "Reference1",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSPartnersReference2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Partners.Reference2", DefaultText = "Reference2",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSPartnersMyCustomer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Partners.MyCustomer", DefaultText = "My Customer",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSPartnersAddShipper = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Partners.AddShipper", DefaultText = "Add Shipper",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSPartnersAddConsignee = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Partners.AddConsignee", DefaultText = "Add Consignee",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSPartnersEditShipper = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Partners.EditShipper", DefaultText = "Edit Shipper",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSPartnersEditConsignee = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Partners.EditConsignee", DefaultText = "Edit Consignee",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesAddCharges = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.AddCharges", DefaultText = "Add Charges",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesEditCharges = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.EditCharges", DefaultText = "Edit Charges",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesCostQuantity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.CostQuantity", DefaultText = "Cost%nQty",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesCostPrice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.CostPrice", DefaultText = "Cost%nPrice",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesCostAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.CostAmount", DefaultText = "Cost%nAmount",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesSaleQuantity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.SaleQuantity", DefaultText = "Sale%nQty",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesSalePrice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.SalePrice", DefaultText = "Sale%nPrice (%SaleCurrencyCode)",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesSaleAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.SaleAmount", DefaultText = "Sale%nAmount (%SaleCurrencyCode)",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesSaleAmountLocal = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.SaleAmountLocal", DefaultText = "Sale%nAmount (%LocalCurrencyCode)",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesFillExchangeRate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.FillExchangeRate", DefaultText = "Fill Exchange Rate",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteRoutings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.Routings", DefaultText = "Routings",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteGeneral = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteExpectedOrderDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.ExpectedOrderDetails", DefaultText = "Expected Order Details",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteAdditionalFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.AdditionalFields", DefaultText = "Additional Fields",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteAdhoc = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.Adhoc", DefaultText = "Ad hoc",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuotePeriodical = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.Periodical", DefaultText = "Periodical",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteGateway = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.Gateway", DefaultText = "Gateway",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteLoadingPort = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.LoadingPort", DefaultText = "Loading Port",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteFrom = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.From", DefaultText = "From",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteDestination = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.Destination", DefaultText = "Destination",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteDischargePort = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.DischargePort", DefaultText = "Discharge Port",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteTo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.To", DefaultText = "To",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteCarrier = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.Carrier", DefaultText = "Carrier",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteAirline = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.Airline", DefaultText = "Airline",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteShippingline = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.Shippingline", DefaultText = "Shipping Line",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteTrucker = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.Trucker", DefaultText = "Trucker",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteQuantity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.Quantity", DefaultText = "Quantity",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuotePackageType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.PackageType", DefaultText = "Package Type",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteShipper = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.Shipper", DefaultText = "Shipper",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteConsignee = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.Consignee", DefaultText = "Consignee",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.Name", DefaultText = "Name",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteAddress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.Address", DefaultText = "Address",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteContact = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.Contact", DefaultText = "Contact",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteReference1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.Reference1", DefaultText = "Reference 1",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteReference2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.Reference2", DefaultText = "Reference 2",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteMyCustomer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.MyCustomer", DefaultText = "My Customer",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesRates = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.Rates", DefaultText = "Rates",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesCost = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.Cost", DefaultText = "Cost",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesSale = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.Sale", DefaultText = "Sale",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesSaleCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.SaleCurrency", DefaultText = "Sale Currency (%SaleCurrencyCode)",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesLocalCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.LocalCurrency", DefaultText = "Local Currency (%LocalCurrencyCode)",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSChargesMarkup = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Charges.Markup", DefaultText = "Markup",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBSetAsRejected = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.SetAsRejected", DefaultText = "Set As Rejected by Customer",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBSetAsNoAnswer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.SetAsNoAnswer", DefaultText = "Set As No Answer",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBReturnInProgress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.ReturnInProgress", DefaultText = "Return Quote in Progress",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSDetailsRoutingRates = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Details.RoutingRates", DefaultText = "Routing Rates",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteONewQuoteRoutingRates = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.NewQuote.RoutingRates", DefaultText = "Routing Rates",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBCreate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.Create", DefaultText = "Create",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBCopy = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.Copy", DefaultText = "Copy",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteRoutings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.Routings", DefaultText = "Routings",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteGeneral = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteExpectedOrderDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.ExpectedOrderDetails", DefaultText = "Expected Order Details",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteAdditionalFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.AdditionalFields", DefaultText = "Additional Fields",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteAdhoc = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.Adhoc", DefaultText = "Ad hoc",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteRoutingRates = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.RoutingRates", DefaultText = "Routing Rates",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteGateway = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.Gateway", DefaultText = "Gateway",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteLoadingPort = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.LoadingPort", DefaultText = "Loading Port",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteFrom = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.From", DefaultText = "From",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteDestination = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.Destination", DefaultText = "Destination",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteDischargePort = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.DischargePort", DefaultText = "Discharge Port",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteTo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.To", DefaultText = "To",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteCarrier = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.Carrier", DefaultText = "Carrier",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteAirline = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.Airline", DefaultText = "Airline",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteShippingline = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.Shippingline", DefaultText = "Shipping line",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteTrucker = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.Trucker", DefaultText = "Trucker",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteQuantity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.Quantity", DefaultText = "Quantity",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuotePackageType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.PackageType", DefaultText = "Package Type",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.Name", DefaultText = "Name",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteMyCustomer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.MyCustomer", DefaultText = "My Customer",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteSetAsMyCustomer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.SetAsMyCustomer", DefaultText = "Set as My Customer",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteShipperNotes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.ShipperNotes", DefaultText = "Shipper Notes",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteConsigneeNotes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.ConsigneeNotes", DefaultText = "Consignee Notes",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteSelectFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.SelectFields", DefaultText = "Select Fields to Copy",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuotePartners = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.Partners", DefaultText = "Partners",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteMainCarriage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.MainCarriage", DefaultText = "Main Carriage",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteCharges = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.Charges", DefaultText = "Charges",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteChargeType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.ChargeType", DefaultText = "Charges Types",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteCost = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.Cost", DefaultText = "Cost",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteSale = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.Sale", DefaultText = "Sale",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteORoutings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Routings", DefaultText = "Routings",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSPartnersPartners = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Partners.Partners", DefaultText = "Partners",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBPartnersAddAgent = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.Partners.AddAgent", DefaultText = "Add Agent",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSPartnersAddAgent = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Partners.AddAgent", DefaultText = "Add Agent",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSPartnersEditAgent = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Partners.EditAgent", DefaultText = "Edit Agent",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBChargesTariffs = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.Charges.Tariffs", DefaultText = "Tariffs",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesNoVat = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.NoVat", DefaultText = "No VAT for this date",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteMDeleteThisPackage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.M.DeleteThisPackage", DefaultText = "Delete this package?",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesCostMinAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.CostMinAmount", DefaultText = "Cost%nMin Amount",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesCostMaxAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.CostMaxAmount", DefaultText = "Cost%nMax Amount",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesSaleMinAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.SaleMinAmount", DefaultText = "Sale%nMin Amount",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesSaleMaxAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.SaleMaxAmount", DefaultText = "Sale%nMax Amount",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesTotalSaleAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.TotalSaleAmount", DefaultText = "Total Sale Amount",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesVATDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.VATDetails", DefaultText = "VAT Details",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesSubTotal = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.SubTotal", DefaultText = "Sub-Total",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesTotalVAT = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.TotalVAT", DefaultText = "Total VAT",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesTotalSale = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.TotalSale", DefaultText = "Total Sale",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.Details", DefaultText = "Details",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBNewQuoteAddAgent = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.NewQuote.AddAgent", DefaultText = "Add Agent",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteAddress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.Address", DefaultText = "Address",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSPackagesAddPackage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Packages.AddPackage", DefaultText = "Add Package",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSPackagesEditPackage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Packages.EditPackage", DefaultText = "Edit Package",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBAddPackage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.AddPackage", DefaultText = "Add Package",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSOverviewActivities = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Overview.Activities", DefaultText = "Activities",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSOverviewNoActivities = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Overview.NoActivities", DefaultText = "No Activities",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSDetailsGeneralDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Details.GeneralDetails", DefaultText = "General Details",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSDetailsCloseAutomatically = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Details.CloseAutomatically", DefaultText = "Close automatically as declined after",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSDetailsDays = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Details.Days", DefaultText = "days",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSDetailsExpirationTime = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Details.ExpirationTime", DefaultText = "Expiration Time",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteCreateNewQuote = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.CreateNewQuote", DefaultText = "Create New Quote",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteSpotRate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.SpotRate", DefaultText = "Spot Rate",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteExpirationTime = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.ExpirationTime", DefaultText = "Expiration Time",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteCloseAutomatically = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.CloseAutomatically", DefaultText = "Close automatically as declined after",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteDays = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.Days", DefaultText = "days",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuotePickup = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.Pickup", DefaultText = "Pickup",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuotePickupAddress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.PickupAddress", DefaultText = "Pickup Address",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteDelivery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.Delivery", DefaultText = "Delivery",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteDeliveryAddress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.DeliveryAddress", DefaultText = "Delivery Address",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteZipCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.ZipCode", DefaultText = "Zip Code",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteCity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.City", DefaultText = "City",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteCountry = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.Country", DefaultText = "Country",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteInsertTotals = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.InsertTotals", DefaultText = "Please insert totals or",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteFillDimensions = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.FillDimensions", DefaultText = "Fill Dimensions",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteSummary = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.Summary", DefaultText = "Summary",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSNewQuoteIsLocalLanguage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.NewQuote.IsLocalLanguage", DefaultText = "Is Local Language",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSWorkspaceQuoteQueries = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Workspace.QuoteQueries", DefaultText = "Quote Queries",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSWorkspaceOthers = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Workspace.Others", DefaultText = "Others",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSWorkspaceMyViews = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Workspace.MyViews", DefaultText = "My Views",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSWorkspaceRecentQuotes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Workspace.RecentQuotes", DefaultText = "Recent Quotes",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSWorkspaceTopQuotes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Workspace.TopQuotes", DefaultText = "Top Quotes",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSWorkspaceSalesFunnel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Workspace.SalesFunnel", DefaultText = "Sales Funnel",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSPartnersMainAddress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Partners.MainAddress", DefaultText = "Main Address",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSRoutingsPickup = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Routings.Pickup", DefaultText = "Pickup",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSRoutingsDelivery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Routings.Delivery", DefaultText = "Delivery",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSRoutingsMainCarriage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Routings.MainCarriage", DefaultText = "Main Carriage",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSRoutingsZipCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Routings.ZipCode", DefaultText = "Zip Code",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSRoutingsCity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Routings.City", DefaultText = "City",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSRoutingsCountry = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Routings.Country", DefaultText = "Country",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSRoutingsFromPartner = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Routings.FromPartner", DefaultText = "From Partner",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSRoutingsToPartner = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Routings.ToPartner", DefaultText = "To Partner",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSRoutingsIsLocalLanguage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Routings.IsLocalLanguage", DefaultText = "Is Local Language",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSRoutingsAddAddress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Routings.AddAddress", DefaultText = "Add Address",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSRoutingsEditAddress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Routings.EditAddress", DefaultText = "Edit Address",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesFixed = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.Fixed", DefaultText = "Fixed",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteOChargesSameAsCost = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.O.Charges.SameAsCost", DefaultText = "Same as Cost Currency",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteBPartnersAddNotify = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.B.Partners.AddNotify", DefaultText = "Add Notify",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSPartnersAddNotify = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Partners.AddNotify", DefaultText = "Add Notify",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteTextCode_QuoteSPartnersEditNotify = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.S.Partners.EditNotify", DefaultText = "Edit Notify",LocalDefaultText = null, ObjectTableId = QuoteObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 