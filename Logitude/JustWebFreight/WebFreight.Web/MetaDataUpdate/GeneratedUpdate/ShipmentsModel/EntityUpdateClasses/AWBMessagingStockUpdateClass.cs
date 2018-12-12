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
namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.ShipmentsModel.EntityUpdateClasses
{
   public class AWBMessagingStockUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "AWBMessagingStock",
			      				    IsNew =  false,
			      				    DBTableName =  "AWBMessagingStocks",
			      				    OldDBTableName =  "AWBMessagingStocks",
			      				    ObjectTableSingular =  "AWB Messaging Stock",
			      				    ObjectTablePlural =  "AWB Messaging Stocks",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  true,
			      				    IsNewWizard =  true,
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  false,
			      				    EnableEditFromLOV =  false,
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    NewWizardControlName =  "Simplog.ShipmentLib.NewMessagingStockCommand",
			      				    DefaultText =  "AWB Messaging Stock",
			      				    Code =  "AWMS",
			      				    Name =  "AWB Messaging Stocks",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Shipment",
			      				    NewWizardComponentPath =  "./ShipmentModules/ShipmentStock/Components/AWBMessagingStock/StockNewWizardComponent",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    SearchFields =  "AWBMessagingStock,AWBMessagingStocks,Simplog.ShipmentLib.NewMessagingStockCommand,Id,",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Amount",
					  						OldFieldName =  "Amount",
					  						ObjectTableName =  "AWBMessagingStock",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  true,
					  						CopyToDW =  false,
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
					  						PMPropertyPath =  "Amount",
					  						ListPropertyPath =  "Amount",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AWBMessagingStock",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Amount",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Amount",
					  						DefaultText =  @"Amount",
					  						ListFieldLable =  "AmountListLable",
					  						ListLableDefaultText =  @"Amount",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Amount",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Remaining",
					  						OldFieldName =  "Remaining",
					  						ObjectTableName =  "AWBMessagingStock",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
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
					  						PMPropertyPath =  "Remaining",
					  						ListPropertyPath =  "Remaining",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AWBMessagingStock",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Remaining",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Remaining",
					  						DefaultText =  @"Remaining",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Remaining",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StartDate",
					  						OldFieldName =  "StartDate",
					  						ObjectTableName =  "AWBMessagingStock",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "StartDateDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StartDate",
					  						ListPropertyPath =  "StartDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AWBMessagingStock",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "StartDate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StartDate",
					  						DefaultText =  @"Start Date",
					  						ListFieldLable =  "StartDateListLable",
					  						ListLableDefaultText =  @"Start Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "StartDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EndDate",
					  						OldFieldName =  "EndDate",
					  						ObjectTableName =  "AWBMessagingStock",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "EndDateDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "EndDate",
					  						ListPropertyPath =  "EndDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AWBMessagingStock",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "EndDate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EndDate",
					  						DefaultText =  @"End Date",
					  						ListFieldLable =  "EndDateListLable",
					  						ListLableDefaultText =  @"End Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "EndDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreateDate",
					  						OldFieldName =  "CreateDate",
					  						ObjectTableName =  "AWBMessagingStock",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
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
					  						PMPropertyPath =  "CreateDate",
					  						ListPropertyPath =  "CreateDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AWBMessagingStock",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "CreateDate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreateDate",
					  						DefaultText =  @"Create Date",
					  						ListFieldLable =  "CreateDateListLable",
					  						ListLableDefaultText =  @"Create Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "CreateDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdateDate",
					  						OldFieldName =  "UpdateDate",
					  						ObjectTableName =  "AWBMessagingStock",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
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
					  						ValidForQuerySection1 =  "AWBMessagingStock",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "UpdateDate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
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
					  						HelpTextCode =  "UpdateDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedByUserId",
					  						OldFieldName =  "CreatedByUserId",
					  						ObjectTableName =  "AWBMessagingStock",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
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
					  						ValidForQuerySection1 =  "AWBMessagingStock",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "CreatedByUserId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreatedByUserId",
					  						DefaultText =  @"Created By",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "CreatedByUserId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedByUserId",
					  						OldFieldName =  "UpdatedByUserId",
					  						ObjectTableName =  "AWBMessagingStock",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
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
					  						ValidForQuerySection1 =  "AWBMessagingStock",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "UpdatedByUserId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
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
					  						HelpTextCode =  "UpdatedByUserId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsCancelled",
					  						OldFieldName =  "IsCancelled",
					  						ObjectTableName =  "AWBMessagingStock",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
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
					  						ValidForQuerySection1 =  "AWBMessagingStock",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsCancelled",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsCancelled",
					  						DefaultText =  @"Is Cancelled",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsCancelled",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Status",
					  						OldFieldName =  "Status",
					  						ObjectTableName =  "AWBMessagingStock",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  10,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "AWBStockStatusDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Status",
					  						ListPropertyPath =  "Status",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AWBMessagingStock",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Status",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Status",
					  						DefaultText =  @"Status",
					  						ListFieldLable =  "StatusListLable",
					  						ListLableDefaultText =  @"Status",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Status",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Notes",
					  						OldFieldName =  "Notes",
					  						ObjectTableName =  "AWBMessagingStock",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  250,
					  						IsRequired =  false,
					  						CopyToDW =  false,
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
					  						ValidForQuerySection1 =  "AWBMessagingStock",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Notes",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Notes",
					  						DefaultText =  @"Notes",
					  						ListFieldLable =  "NotesListLable",
					  						ListLableDefaultText =  @"Notes",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Notes",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TenantNumber",
					  						OldFieldName =  "TenantNumber",
					  						ObjectTableName =  "AWBMessagingStock",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  true,
					  						CopyToDW =  false,
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
					  						PMPropertyPath =  "TenantNumber",
					  						ListPropertyPath =  "TenantNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AWBMessagingStock",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "TenantNumber",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TenantNumber",
					  						DefaultText =  @"Tenant Number",
					  						ListFieldLable =  "TenantNumberListLable",
					  						ListLableDefaultText =  @"Tenant Number",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "TenantNumber",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TenantName",
					  						OldFieldName =  "TenantName",
					  						ObjectTableName =  "AWBMessagingStock",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
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
					  						PMPropertyPath =  "TenantName",
					  						ListPropertyPath =  "TenantName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AWBMessagingStock",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "TenantName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TenantName",
					  						DefaultText =  @"Tenant Name",
					  						ListFieldLable =  "TenantNameListLable",
					  						ListLableDefaultText =  @"Tenant Name",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "TenantName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TotalPrice",
					  						OldFieldName =  "TotalPrice",
					  						ObjectTableName =  "AWBMessagingStock",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
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
					  						PMPropertyPath =  "TotalPrice",
					  						ListPropertyPath =  "TotalPrice",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AWBMessagingStock",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "TotalPrice",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TotalPrice",
					  						DefaultText =  @"Total Price",
					  						ListFieldLable =  "TotalPriceListLable",
					  						ListLableDefaultText =  @"Total Price",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "TotalPrice",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup AWBMessagingStockQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "AWMS", Name = "AWB Messaging Stocks" }, queryGroupRepository);
						QueryGroup AWBMessagingStockQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "4e67", Name = " Query Group" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable AWBMessagingStockObjectTable = objectContext.ObjectTables.Where(d => d.Name == "AWBMessagingStock" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> AWBMessagingStockObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "AWBMessagingStock").ToList();   

			   TextCode AWBMessagingStockTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AWBMessagingStock.Q.AllAWBMessagingStocks", DefaultText = @"All AWB Messaging Stocks",LocalDefaultText = null, ObjectTableId = AWBMessagingStockObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature AWBMessagingStockFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AWBMessagingStock.Q.AllQuery", ObjectTableId = AWBMessagingStockObjectTable.Id, Tenant = 0, NameTextCodeCode = "AWBMessagingStock.Features.AllAWBMessagingStocks", NameTextCodeDefaultText = "All AWB Messaging Stocks", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AllAWBMessagingStocksQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = AWBMessagingStockTextCode_0.Id, Code = "All AWB Messaging Stocks",  QueryGroupCode = "AWMS", IndexOrder = 0, Tenant = 0, ObjectTableId = AWBMessagingStockObjectTable.Id, QuerySection = "AWBMessagingStock", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = AWBMessagingStockFeature_0.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllAWBMessagingStocksQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAWBMessagingStocksQuery.Id, IndexOrder = 0, ObjectFieldId = AWBMessagingStockObjectFields.Where(d => d.FieldName == "TenantNumber" && d.ObjectTableId == AWBMessagingStockObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllAWBMessagingStocksQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAWBMessagingStocksQuery.Id, IndexOrder = 1, ObjectFieldId = AWBMessagingStockObjectFields.Where(d => d.FieldName == "StartDate" && d.ObjectTableId == AWBMessagingStockObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllAWBMessagingStocksQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAWBMessagingStocksQuery.Id, IndexOrder = 2, ObjectFieldId = AWBMessagingStockObjectFields.Where(d => d.FieldName == "EndDate" && d.ObjectTableId == AWBMessagingStockObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllAWBMessagingStocksQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAWBMessagingStocksQuery.Id, IndexOrder = 3, ObjectFieldId = AWBMessagingStockObjectFields.Where(d => d.FieldName == "Amount" && d.ObjectTableId == AWBMessagingStockObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllAWBMessagingStocksQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAWBMessagingStocksQuery.Id, IndexOrder = 4, ObjectFieldId = AWBMessagingStockObjectFields.Where(d => d.FieldName == "Remaining" && d.ObjectTableId == AWBMessagingStockObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllAWBMessagingStocksQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAWBMessagingStocksQuery.Id, IndexOrder = 5, ObjectFieldId = AWBMessagingStockObjectFields.Where(d => d.FieldName == "Status" && d.ObjectTableId == AWBMessagingStockObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllAWBMessagingStocksQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAWBMessagingStocksQuery.Id, IndexOrder = 6, ObjectFieldId = AWBMessagingStockObjectFields.Where(d => d.FieldName == "Notes" && d.ObjectTableId == AWBMessagingStockObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 350 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllAWBMessagingStocksQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAWBMessagingStocksQuery.Id, IndexOrder = 7, ObjectFieldId = AWBMessagingStockObjectFields.Where(d => d.FieldName == "TotalPrice" && d.ObjectTableId == AWBMessagingStockObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable AWBMessagingStockObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AWBMessagingStock" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> AWBMessagingStockObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "AWBMessagingStock").ToList();
		       
	      

	         Screen AWBMessagingStockHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "AWBMessagingStock.HeaderScreen", Name = "Header Screen", ObjectTableId = AWBMessagingStockObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField AWBMessagingStockAWBMessagingStockHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = AWBMessagingStockObjectFields.Where(d => d.FieldName == "StartDate").FirstOrDefault().Id, ScreenId = AWBMessagingStockHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField AWBMessagingStockAWBMessagingStockHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = AWBMessagingStockObjectFields.Where(d => d.FieldName == "EndDate").FirstOrDefault().Id, ScreenId = AWBMessagingStockHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField AWBMessagingStockAWBMessagingStockHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = AWBMessagingStockObjectFields.Where(d => d.FieldName == "Amount").FirstOrDefault().Id, ScreenId = AWBMessagingStockHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField AWBMessagingStockAWBMessagingStockHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = AWBMessagingStockObjectFields.Where(d => d.FieldName == "Remaining").FirstOrDefault().Id, ScreenId = AWBMessagingStockHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    AWBMessagingStockObjectTable.HeaderScreenId = AWBMessagingStockHeaderScreenScreen0.Id;
	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {    
			 ObjectTable AWBMessagingStockObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AWBMessagingStock" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode AWBMessagingStockGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AWBMessagingStock.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = AWBMessagingStockObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature AWBMessagingStockGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AWBMessagingStock.Tab.General", ObjectTableId = AWBMessagingStockObjectTable.Id, Tenant = 0, NameTextCodeCode = "AWBMessagingStock.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode AWBMessagingStockEventsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AWBMessagingStock.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = AWBMessagingStockObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature AWBMessagingStockEventsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AWBMessagingStock.Tab.Events", ObjectTableId = AWBMessagingStockObjectTable.Id, Tenant = 0, NameTextCodeCode = "AWBMessagingStock.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "MSGN",HtmlComponentName = "",HtmlComponentUrl = "./ShipmentModules/ShipmentStock/Components/AWBMessagingStock/StockGeneralTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "AWBMessagingStock.Tab.General" && d.ObjectTableId == AWBMessagingStockObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.ShipmentLib.Views.AWBMessagingStock.MessagingStockGeneralTabControl", ObjectTableId = AWBMessagingStockObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "AWBMessagingStock.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "MSEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "AWBMessagingStock.Tab.Events" && d.ObjectTableId == AWBMessagingStockObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = AWBMessagingStockObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "AWBMessagingStock.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable AWBMessagingStockObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AWBMessagingStock" && d.Tenant == 0).FirstOrDefault(); 
		   Feature AWBMessagingStockFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = AWBMessagingStockObjectTable.Id, Tenant = 0, NameTextCodeCode = "AWBMessagingStock.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature AWBMessagingStockFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = AWBMessagingStockObjectTable.Id, Tenant = 0, NameTextCodeCode = "AWBMessagingStock.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature AWBMessagingStockFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = AWBMessagingStockObjectTable.Id, Tenant = 0, NameTextCodeCode = "AWBMessagingStock.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature AWBMessagingStockFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = AWBMessagingStockObjectTable.Id, Tenant = 0, NameTextCodeCode = "AWBMessagingStock.Features.PackageFeature", NameTextCodeDefaultText = "AWBMessagingStock Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes); 

		   		   //--------------> Additional Features <--------------\\

		   Feature AWBMessagingStockFeature_AWBMessagingStock_M_AWBMessagingStocks = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AWBMessagingStock.M.AWBMessagingStocks", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = AWBMessagingStockObjectTable.Id, Tenant = 0, NameTextCodeCode = "AWBMessagingStock.Features.AWBMessagingStocks", NameTextCodeDefaultText = @"AWB Messaging Stocks" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable AWBMessagingStockObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AWBMessagingStock" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPMS",
                EnglishName =  "AWB Stock Updated",
                LocalName =  "AWB Stock Updated",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = AWBMessagingStockObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRMS",
                EnglishName =  "Created",
                LocalName =  "Created",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = AWBMessagingStockObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   FeatureRepository featureRepository = new FeatureRepository(0); 
		   List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable AWBMessagingStockObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AWBMessagingStock" && d.Tenant == 0).FirstOrDefault(); 			   Feature AWBMessagingStockFeature_MB00 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AWBMessagingStock.Action.Cancel", ObjectTableId = AWBMessagingStockObjectTable.Id, Tenant = 0, NameTextCodeCode = "AWBMessagingStock.Features.Cancel", NameTextCodeDefaultText = "Cancel", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
              

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup AWBMessagingStockMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "AWBMessagingStockEdit",
					Name = "AWBMessagingStockEditButtonsGroup",
					ObjectTableId = AWBMessagingStockObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton AWBMessagingStockMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Actions",
						Index = 100, 
						IsActive = false,
						LabelTextCodeCode = "AWBMessagingStock.B.More",
						LabelTextCodeDefaultText = "More",
						Tenant = 0,
						MenuButtonGroupId = AWBMessagingStockMenuButtonGroup.Id,
						ObjectTableId = AWBMessagingStockObjectTable.Id,
						MenuButtonType = "dropdownbutton",
						FeatureId = null,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

			   MenuButton AWBMessagingStockMenuButton00 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Cancel",
						Index = 2, 
						IsActive = true,
						LabelTextCodeCode = "AWBMessagingStock.B.Cancel",
						LabelTextCodeDefaultText = "Cancel",
						Tenant = 0,
						MenuButtonGroupId = AWBMessagingStockMenuButtonGroup.Id,
						ParentMenuButtonId = AWBMessagingStockMenuButton0.Id,
						ObjectTableId = AWBMessagingStockObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  AWBMessagingStockFeature_MB00.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable AWBMessagingStockObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AWBMessagingStock" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode AWBMessagingStockTextCode_AWBMessagingStock = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AWBMessagingStock", DefaultText = "AWB Messaging Stock",LocalDefaultText = null, ObjectTableId = AWBMessagingStockObjectTable.Id, Tenant = 0, TextCodeTypeCode = "T", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 