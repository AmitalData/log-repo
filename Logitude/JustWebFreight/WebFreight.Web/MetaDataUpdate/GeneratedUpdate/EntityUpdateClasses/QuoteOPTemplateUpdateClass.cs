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
using Logitude.TariffModule.Data.Repositories;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.CLoseTable;
using Logitude.CargoTracking.Data.Repositories;
using Logitude.CargoTracking.BL;
using Logitude.CargoTracking.Data.EntityPOCOs;

//using Amital.QuoteOPM.BL.CLoseTable;
using Amital.QuoteOPM.Data.Repsitories;
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.BL;


namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses
{
   public class QuoteOPTemplateUpdateClass
   {  		
		public const string HashString = "69d14c78ca94c200d2d3633c55fdba1d";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "QuoteOPTemplate",
			      				    IsNew =  false,
			      				    DBTableName =  "QuoteOPTemplates",
			      				    ObjectTableSingular =  "Quote Template",
			      				    ObjectTablePlural =  "Quote Templates",
			      				    DescriptionDefaultText =  "Create new quote templates and maintain existing ones.",
			      				    HasCustomFilter =  true,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  true,
			      				    IsNewWizard =  true,
			      				    LookUp1 =  "Name",
			      				    DependencyFilter1 =  "TemplateTypeCode",
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  true,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  true,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  true,
			      				    EnableEditFromLOV =  false,
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    NewWizardControlName =  "Simplog.QuoteLib.NewQuoteOPTemplateCommand",
			      				    DefaultText =  "Quote Template",
			      				    Code =  "QUTE",
			      				    Name =  "QuoteOPTemplates",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "QuoteOPM",
			      				    NewWizardComponentPath =  "./QuoteOPMModules/QuoteOPTemplates/Components/NewQuoteOPTemplateComponent",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    SearchFields =  "QuoteOPTemplate,QuoteOPTemplates,Simplog.QuoteLib.NewQuoteOPTemplateCommand,Id,",
			      				    HashString =  QuoteOPTemplateUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "HeaderDocId",
					  						ObjectTableName =  "QuoteOPTemplate",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "HeaderDocId",
					  						ListPropertyPath =  "HeaderDocId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "HeaderDocId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "HeaderDocId",
					  						DefaultText =  "HeaderDocId",
					  						ListFieldLable =  "HeaderDocIdListLable",
					  						ListLableDefaultText =  "HeaderDocId",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "HeaderDocId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FooterDocId",
					  						ObjectTableName =  "QuoteOPTemplate",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FooterDocId",
					  						ListPropertyPath =  "FooterDocId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "FooterDocId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FooterDocId",
					  						DefaultText =  "FooterDocId",
					  						ListFieldLable =  "FooterDocIdListLable",
					  						ListLableDefaultText =  "FooterDocId",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "FooterDocId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "QuoteOPTemplateSettingId",
					  						ObjectTableName =  "QuoteOPTemplate",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "QuoteOPTemplateSettingId",
					  						ListPropertyPath =  "QuoteOPTemplateSettingId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "QuoteOPTemplateSettingId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "QuoteOPTemplateSettingId",
					  						DefaultText =  "QuoteOPTemplateSettingId",
					  						ListFieldLable =  "QuoteOPTemplateSettingIdListLable",
					  						ListLableDefaultText =  "QuoteOPTemplateSettingId",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "QuoteOPTemplateSettingId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Name",
					  						ObjectTableName =  "QuoteOPTemplate",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "Name",
					  						ListPropertyPath =  "Name",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "QuoteOPTemplate",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Name",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Name",
					  						DefaultText =  "Name",
					  						ListFieldLable =  "NameListLable",
					  						ListLableDefaultText =  "Name",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Name",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsTemplate",
					  						ObjectTableName =  "QuoteOPTemplate",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsTemplate",
					  						ListPropertyPath =  "IsTemplate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsTemplate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsTemplate",
					  						DefaultText =  "Is Template",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsTemplate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OriginalQuoteOPTemplateId",
					  						ObjectTableName =  "QuoteOPTemplate",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OriginalQuoteOPTemplateId",
					  						ListPropertyPath =  "OriginalQuoteOPTemplateId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "OriginalQuoteOPTemplateId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OriginalQuoteOPTemplateId",
					  						DefaultText =  "Original Quote Template Id",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "OriginalQuoteOPTemplateId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreateDate",
					  						ObjectTableName =  "QuoteOPTemplate",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
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
					  						DefaultText =  "Create Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "CreateDate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdateDate",
					  						ObjectTableName =  "QuoteOPTemplate",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
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
					  						DefaultText =  "Update Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "UpdateDate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedByUserId",
					  						ObjectTableName =  "QuoteOPTemplate",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
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
					  						DefaultText =  "Created By UserId",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "CreatedByUserId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedByUserId",
					  						ObjectTableName =  "QuoteOPTemplate",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
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
					  						DefaultText =  "Updated By UserId",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "UpdatedByUserId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "QuoteOPTemplate",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  1000,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SearchFields",
					  						ListPropertyPath =  "SearchFields",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "SearchFields",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search..",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "SearchFields",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TemplateTypeCode",
					  						ObjectTableName =  "QuoteOPTemplate",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  true,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TemplateTypeCode",
					  						ListPropertyPath =  "TemplateTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "TemplateTypeCode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TemplateTypeCode",
					  						DefaultText =  "Template Type Code",
					  						ListFieldLable =  "TemplateTypeCodeListLable",
					  						ListLableDefaultText =  "TemplateTypeCode",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "TemplateTypeCode",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TemplateTypeName",
					  						ObjectTableName =  "QuoteOPTemplate",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TemplateTypeName",
					  						ListPropertyPath =  "TemplateTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "QuoteOPTemplate",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "TemplateTypeName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TemplateTypeName",
					  						DefaultText =  "Template Type",
					  						ListFieldLable =  "TemplateTypeNameListLable",
					  						ListLableDefaultText =  "Template Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "TemplateTypeName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsDefault",
					  						ObjectTableName =  "QuoteOPTemplate",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsDefault",
					  						ListPropertyPath =  "IsDefault",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "QuoteOPTemplate",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsDefault",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsDefault",
					  						DefaultText =  "Is Default",
					  						ListFieldLable =  "IsDefaultListLable",
					  						ListLableDefaultText =  "Is Default",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsDefault",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ShowLocalLanguage",
					  						ObjectTableName =  "QuoteOPTemplate",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ShowLocalLanguage",
					  						ListPropertyPath =  "ShowLocalLanguage",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "QuoteOPTemplate",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ShowLocalLanguage",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ShowLocalLanguage",
					  						DefaultText =  "Show Local Language",
					  						ListFieldLable =  "ShowLocalLanguageListLable",
					  						ListLableDefaultText =  "Show Local Language",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ShowLocalLanguage",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InActive",
					  						ObjectTableName =  "QuoteOPTemplate",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Inactive",
					  						ListPropertyPath =  "Inactive",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "QuoteOPTemplate",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "InActive",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InActive",
					  						DefaultText =  "In Active",
					  						ListFieldLable =  "InActiveListLable",
					  						ListLableDefaultText =  "In Active",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "InActive",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsCopiedAtSignup",
					  						ObjectTableName =  "QuoteOPTemplate",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsCopiedAtSignup",
					  						ListPropertyPath =  "IsCopiedAtSignup",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsCopiedAtSignup",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsCopiedAtSignup",
					  						DefaultText =  "Copy at Signup",
					  						ListFieldLable =  "IsCopiedAtSignupListLable",
					  						ListLableDefaultText =  "IsCopiedAtSignup",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsCopiedAtSignup",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsEnabledForCustomers",
					  						ObjectTableName =  "QuoteOPTemplate",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsEnabledForCustomers",
					  						ListPropertyPath =  "IsEnabledForCustomers",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsEnabledForCustomers",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsEnabledForCustomers",
					  						DefaultText =  "Enabled for Customers",
					  						ListFieldLable =  "IsEnabledForCustomersListLable",
					  						ListLableDefaultText =  "IsEnabledForCustomers",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsEnabledForCustomers",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TenantName",
					  						ObjectTableName =  "QuoteOPTemplate",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
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
					  						ValidForQuerySection1 =  "QuoteOPTemplate",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TenantName",
					  						DefaultText =  "Tenant",
					  						ListFieldLable =  "TenantNameListLable",
					  						ListLableDefaultText =  "Tenant",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters,Dictionary<string, QueryGroup> tenantQueryGroups )
	    {  
	        //FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
	        QueryGroup QuoteOPTemplateQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "QUTE", Name = "QuoteOPTemplates" }, queryGroupRepository,tenantQueryGroups);
						QueryGroup QuoteOPTemplateQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "db63", Name = " Query Group" }, queryGroupRepository,tenantQueryGroups);
				        queryGroupRepository.SubmitChanges();
	        ObjectTable QuoteOPTemplateObjectTable = objectTables.ContainsKey("QuoteOPTemplate") ? objectTables["QuoteOPTemplate"] : null;
            if (QuoteOPTemplateObjectTable == null)
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(0);  

                QuoteOPTemplateObjectTable = objectContext.ObjectTables.Where(d => d.Name == "QuoteOPTemplate" && d.Tenant == 0).FirstOrDefault();
            }

	         
			List<Feature> addedFeatures = new List<Feature>();
			List<TextCode> addedTextCodes = new List<TextCode>();
			List<Query> addedQueries = new List<Query>();
			List<QueryColumn> addedQueryColumns = new List<QueryColumn>();
			List<AdvancedQueryFilter> addedQueryFilters = new List<AdvancedQueryFilter>();
   

			   TextCode QuoteOPTemplateTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.Q.QuoteOPTemplates", DefaultText = @"Quote Templates",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature QuoteOPTemplateFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QuoteOPTemplateS", ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteOPTemplate.Features.QuoteOPTemplate", NameTextCodeDefaultText = "Quote Templates", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,QuoteOPTemplateObjectTable, addedFeatures, addedTextCodes);

	        //TextCodeRepository.SubmitChanges();
	        //FeaturesRepository.SubmitChanges();    
	      

			  Query QuoteOPTemplatesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = QuoteOPTemplateTextCode_0.Id, NameTextCodeCode = QuoteOPTemplateTextCode_0.Code, ObjectTableName = "QuoteOPTemplate", Code = "QuoteOPTemplates",  EditWizardName = "Simplog.QuoteLib.Views.QuoteOPTemplateViews.QuoteOPTemplateWizardEditControl",
			   QueryGroupCode = "QUTE", IndexOrder = 0, Tenant = 0, ObjectTableId = QuoteOPTemplateObjectTable.Id, QuerySection = "QuoteOPTemplate", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = QuoteOPTemplateFeature_0.Id,FeatureUniqeCode= QuoteOPTemplateFeature_0.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, addedQueries);
	
			 QueryColumn QuoteOPTemplatesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = QuoteOPTemplatesQuery.Id,QueryCode = QuoteOPTemplatesQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "QuoteOPTemplate.Name" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn QuoteOPTemplatesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = QuoteOPTemplatesQuery.Id,QueryCode = QuoteOPTemplatesQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "QuoteOPTemplate.CreateDate" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn QuoteOPTemplatesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = QuoteOPTemplatesQuery.Id,QueryCode = QuoteOPTemplatesQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "QuoteOPTemplate.UpdateDate" , ColumnWidth = 100 }, addedQueryColumns);
			SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
			SqlBulkInsert.BulkInsert("Features", addedFeatures);
			SqlBulkInsert.BulkInsert("Queries", addedQueries);
			SqlBulkInsert.BulkInsert("QueryColumns", addedQueryColumns);
			SqlBulkInsert.BulkInsert("AdvancedQueryFilters", addedQueryFilters);	 
  
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable QuoteOPTemplateObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "QuoteOPTemplate" && d.Tenant == 0).FirstOrDefault();
		   //List<ObjectField> QuoteOPTemplateObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "QuoteOPTemplate").ToList();
		       
	      

	         Screen QuoteOPTemplateQuoteOPTemplateHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "QuoteOPTemplate.HeaderScreen", Name = "QuoteOPTemplateHeaderScreen", ObjectTableId = QuoteOPTemplateObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      	
		    QuoteOPTemplateObjectTable.HeaderScreenId = QuoteOPTemplateQuoteOPTemplateHeaderScreenScreen0.Id;
		    QuoteOPTemplateObjectTable.HeaderScreenCode = QuoteOPTemplateQuoteOPTemplateHeaderScreenScreen0.Code;

	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable QuoteOPTemplateObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "QuoteOPTemplate" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode QuoteOPTemplateGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature QuoteOPTemplateGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteOPTemplate.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,QuoteOPTemplateObjectTable);
 
                 
			   TextCode QuoteOPTemplateSettingsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.TH.Settings", DefaultText = "Settings",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature QuoteOPTemplateSettingsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SETTINGS", ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteOPTemplate.Features.Settings", NameTextCodeDefaultText = "Settings", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,QuoteOPTemplateObjectTable);
 
                 
			   TextCode QuoteOPTemplateEventsTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature QuoteOPTemplateEventsFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteOPTemplate.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,QuoteOPTemplateObjectTable);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "QEGC",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = QuoteOPTemplateGeneralFeature_TH0.Id,FeatureUniqeCode = QuoteOPTemplateGeneralFeature_TH0.FeatureUniqeCode, ControlPath = "Simplog.QuoteLib.Views.General.GeneralControl", ObjectTableId = QuoteOPTemplateObjectTable.Id, TabNameTextCodeId = QuoteOPTemplateGeneralTextCode_TH0.Id, TabNameTextCodeCode = QuoteOPTemplateGeneralTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "QESE",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = QuoteOPTemplateSettingsFeature_TH1.Id,FeatureUniqeCode = QuoteOPTemplateSettingsFeature_TH1.FeatureUniqeCode, ControlPath = "Simplog.QuoteLib.Views.Settings.SettingsControl", ObjectTableId = QuoteOPTemplateObjectTable.Id, TabNameTextCodeId = QuoteOPTemplateSettingsTextCode_TH1.Id, TabNameTextCodeCode = QuoteOPTemplateSettingsTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "QEEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = QuoteOPTemplateEventsFeature_TH2.Id,FeatureUniqeCode = QuoteOPTemplateEventsFeature_TH2.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = QuoteOPTemplateObjectTable.Id, TabNameTextCodeId = QuoteOPTemplateEventsTextCode_TH2.Id, TabNameTextCodeCode = QuoteOPTemplateEventsTextCode_TH2.Code, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable QuoteOPTemplateObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "QuoteOPTemplate" && d.Tenant == 0).FirstOrDefault(); 

		   Feature QuoteOPTemplateFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteOPTemplate.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,QuoteOPTemplateObjectTable);
		   Feature QuoteOPTemplateFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteOPTemplate.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,QuoteOPTemplateObjectTable);
		   Feature QuoteOPTemplateFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteOPTemplate.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,QuoteOPTemplateObjectTable);
		   Feature QuoteOPTemplateFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = false, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteOPTemplate.Features.PackageFeature", NameTextCodeDefaultText = "QuoteOPTemplate Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,QuoteOPTemplateObjectTable); 

		   		   //--------------> Additional Features <--------------\\

		   Feature QuoteOPTemplateFeature_COPYATSIGNUP = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "COPYATSIGNUP", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteOPTemplate.Features.CopyAtSignup", NameTextCodeDefaultText = @"Copy At Signup" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,QuoteOPTemplateObjectTable);

		   Feature QuoteOPTemplateFeature_ENABLEDFORCUSTOMERS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ENABLEDFORCUSTOMERS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteOPTemplate.Features.EnabledForCustomers", NameTextCodeDefaultText = @"Enabled For Customers" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,QuoteOPTemplateObjectTable);

		   Feature QuoteOPTemplateFeature_FROMLIBRARY = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "FROMLIBRARY", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteOPTemplate.Features.FromLibrary", NameTextCodeDefaultText = @"Add From Library" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,QuoteOPTemplateObjectTable);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable QuoteOPTemplateObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "QuoteOPTemplate" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CREV",
                EnglishName =  "Created",
                LocalName =  "Created",
                IsManualEntry =  false,
                ShortView =  true,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = QuoteOPTemplateObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPEV",
                EnglishName =  "Updated",
                LocalName =  "Updated",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = QuoteOPTemplateObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable QuoteOPTemplateObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "QuoteOPTemplate" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowTotalPerChargeGroup = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowTotalPerChargeGroup", DefaultText = "ShowTotalPerChargeGroup",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSSearch = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Search", DefaultText = "Search",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSPageHeader = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.PageHeader", DefaultText = "Page Header",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSQuoteHeader = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.QuoteHeader", DefaultText = "Quote Header",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSQuoteIntroduction = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.QuoteIntroduction", DefaultText = "Quote Introduction",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSQuoteDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.QuoteDetails", DefaultText = "Quote Details",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSPricingPackages = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.PricingPackages", DefaultText = "Pricing Packages",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSPricingContainers = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.PricingContainers", DefaultText = "Pricing Containers",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSPageFooter = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.PageFooter", DefaultText = "Page Footer",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSSampleText = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.SampleText", DefaultText = "Sample Text",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSSections = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Sections", DefaultText = "Sections",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSTranslation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Translation", DefaultText = "Translation",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSRighttoleft = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Righttoleft", DefaultText = "Right-to-left",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowLocalLanguage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowLocalLanguage", DefaultText = "Show Local Language",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSPricingTableDesign = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.PricingTableDesign", DefaultText = "Pricing Table Design",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSPreviewTemplate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.PreviewTemplate", DefaultText = "Preview Template",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSTotal = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Total", DefaultText = "Total",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSRows = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Rows", DefaultText = "Rows",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowPageBreakBeforeTable = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowPageBreakBeforeTable", DefaultText = "Show Page Break Before Table",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSBorderColor = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.BorderColor", DefaultText = "Border Color",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowFixedPrice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowFixedPrice", DefaultText = "Show Fixed Price",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowPriceByContainer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowPriceByContainer", DefaultText = "Show Price By Container",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSGeneral = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSTotalsDesign = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.TotalsDesign", DefaultText = "Totals  Design",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSTitleDesign = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.TitleDesign", DefaultText = "Title Design",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSPageHeaderSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.PageHeaderSettings", DefaultText = "Page Header Settings",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSQuoteDetailsSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.QuoteDetailsSettings", DefaultText = "Quote Details Settings",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSPricingPackagesSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.PricingPackagesSettings", DefaultText = "Pricing Packages Settings",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSQuoteHeaderSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.QuoteHeaderSettings", DefaultText = "Quote Header Settings",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSPageFooterSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.PageFooterSettings", DefaultText = "Page Footer Settings",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSPricingContainersSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.PricingContainersSettings", DefaultText = "Pricing Containers Settings",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSEditSection = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.EditSection", DefaultText = "Edit Section",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSAddSection = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.AddSection", DefaultText = "Add Section",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSBorderThickness = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.BorderThickness", DefaultText = "Border Thickness",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSColumn1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Column1", DefaultText = "Column 1",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSColumn2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Column2", DefaultText = "Column 2",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSColumn3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Column3", DefaultText = "Column 3",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSNone = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.None", DefaultText = "None",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSLogo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Logo", DefaultText = "Logo",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSText = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Text", DefaultText = "Text",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSColumnsWidthOptions = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ColumnsWidthOptions", DefaultText = "Columns Width Options",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSWidth = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Width", DefaultText = "Width",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSBorderType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.BorderType", DefaultText = "Border Type",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSAll = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.All", DefaultText = "All",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSAvaiLabelFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.AvaiLabelFields", DefaultText = "AvaiLabel Fields",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSBox = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Box", DefaultText = "Box",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSHorizontalOnly = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.HorizontalOnly", DefaultText = "Horizontal Only",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSVerticalOnly = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.VerticalOnly", DefaultText = "Vertical Only",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSCm = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Cm", DefaultText = "Cm",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSHeight = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Height", DefaultText = "Height",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSTextColor = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.TextColor", DefaultText = "Text Color",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSBackgroundColor = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.BackgroundColor", DefaultText = "Background Color",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSDesignTable = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.DesignTable", DefaultText = "Design Table",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSEditArea1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.EditArea1", DefaultText = "Edit Area 1",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSEditArea2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.EditArea2", DefaultText = "Edit Area 2",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSEditArea3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.EditArea3", DefaultText = "Edit Area 3",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSpx = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.px", DefaultText = "px",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSFont = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Font", DefaultText = "Font",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSTablecolumns = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Tablecolumns", DefaultText = "Table columns",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSLabels = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Labels", DefaultText = "Labels",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSTableDesign = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.TableDesign", DefaultText = "Table Design",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSColumnwidthType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ColumnwidthType", DefaultText = "Column width Type",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSFixed = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Fixed", DefaultText = "Fixed",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSAuto = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Auto", DefaultText = "Auto",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSColumn1Labels = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Column1Labels", DefaultText = "Column 1 Labels",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSColumn2Labels = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Column2Labels", DefaultText = "Column 2 Labels",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSPercentage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Percentage", DefaultText = "%",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSColumn1Values = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Column1Values", DefaultText = "Column 1 Values",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSColumn2Values = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Column2Values", DefaultText = "Column 2 Values",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSQuoteLabels = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.QuoteLabels", DefaultText = "Quote Labels",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSEnglishLabel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.EnglishLabel", DefaultText = "English Label",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSLocalLabel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.LocalLabel", DefaultText = "Local Label",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSLabel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Label", DefaultText = "Label",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSValue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Value", DefaultText = "Value",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSBorder = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Border", DefaultText = "Border",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowTitleGeneralDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowTitle(GeneralDetails)", DefaultText = "Show Title ( General Details )",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSNewQuoteOPTemplate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.NewQuoteOPTemplate", DefaultText = "New Quote Template",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Name", DefaultText = "Name",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSPricingTable = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.PricingTable", DefaultText = "Pricing Table",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSTotals = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Totals", DefaultText = "Totals",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowTitle = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowTitle", DefaultText = "Show Title",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowPricesTable = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowPricesTable", DefaultText = "Show Prices Table",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowChargeCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowChargeCode", DefaultText = "Show Charge Code",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowChargeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowChargeName", DefaultText = "Show Charge Name",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowMeasurement = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowMeasurement", DefaultText = "Show Measurement",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowUnits = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowUnits", DefaultText = "Show Units",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowUnitPrice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowUnitPrice", DefaultText = "Show Unit Price",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowSaleCurrencyColumn = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowSaleCurrencyColumn", DefaultText = "Show Sale Currency Column",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowLocalCurrencyColumn = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowLocalCurrencyColumn", DefaultText = "Show Local Currency Column",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowChargeDescription = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowChargeDescription", DefaultText = "Show Charge Description",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowChargeNote = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowChargeNote", DefaultText = "Show Charge Notes",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowSaleMinMax = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowSaleMinMax", DefaultText = "Show Sale Min/Max",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSSplitbychargegroup = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Splitbychargegroup", DefaultText = "Split by charge group",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowTotalInLocalCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowTotalInLocalCurrency", DefaultText = "Show Total In Local Currency",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowTotalInSaleCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowTotalInSaleCurrency", DefaultText = "Show Total In Sale Currency",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSHeader = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Header", DefaultText = "Header",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSTitle = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Title", DefaultText = "Title",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSGroupbyDesign = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.GroupbyDesign", DefaultText = "Group by Design",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSTotalPerContainerSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.TotalPerContainerSettings", DefaultText = "Total Per Container Settings",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSTotalPerContainerTable = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.TotalPerContainerTable", DefaultText = "Total Per Container Table",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSTotalPerContainerDesign = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.TotalPerContainerDesign", DefaultText = "Total Per Container Design",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSQuote = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Quote", DefaultText = "Quote",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateFTemplate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.F.Template", DefaultText = "Template",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "F", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateFLastUpdate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.F.LastUpdate", DefaultText = "LastUpdate",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "F", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateBSection = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.B.Section", DefaultText = "Section",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateBAdd = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.B.Add", DefaultText = "Add",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateBPageBreak = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.B.PageBreak", DefaultText = "Page Break",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateBEdit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.B.Edit", DefaultText = "Edit",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateBSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.B.Settings", DefaultText = "Settings",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateBGeneralSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.B.GeneralSettings", DefaultText = "General Settings",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateBPreview = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.B.Preview", DefaultText = "Preview",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateBCancel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.B.Cancel", DefaultText = "Cancel",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateBSave = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.B.Save", DefaultText = "Save",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateBClose = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.B.Close", DefaultText = "Close",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateBUp = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.B.Up", DefaultText = "Up",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateBDown = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.B.Down", DefaultText = "Down",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateBNew = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.B.New", DefaultText = "New",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateBCopy = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.B.Copy", DefaultText = "Copy",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateBAdvanced = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.B.Advanced", DefaultText = "Advanced",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateBAddDataField = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.B.AddDataField", DefaultText = "Add Data Field",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateBNext = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.B.Next", DefaultText = "Next",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateBFromAllTenant = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.B.FromAllTenant", DefaultText = "From All Tenant",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateBAddtemplatefromlibrary = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.B.Addtemplatefromlibrary", DefaultText = "Add template from library",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateMClicktoaddthephoto = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.M.Clicktoaddthephoto", DefaultText = "Click to add the photo",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateMDeleteSectionConfirmMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.M.DeleteSectionConfirmMessage", DefaultText = "Are you sure you want to delete this section?",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateMPageHeaderDescriptionMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.M.PageHeaderDescriptionMessage", DefaultText = "the header of each page in the quote",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateMQuoteHeaderDescriptionMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.M.QuoteHeaderDescriptionMessage", DefaultText = "the header of the quote",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateMQuoteDetailsDescriptionMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.M.QuoteDetailsDescriptionMessage", DefaultText = "general details of the quote",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateMPricingTableDescriptionMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.M.PricingTableDescriptionMessage", DefaultText = "the sales prices for the quote",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateMPageFooterDescriptionMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.M.PageFooterDescriptionMessage", DefaultText = "the footer of each page in the quote",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateMSaving = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.M.Saving", DefaultText = "Saving...",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateMLoading = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.M.Loading", DefaultText = "Loading...",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateMNoActiveTemplatesFoundMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.M.NoActiveTemplatesFoundMessage", DefaultText = "No active templates found ",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateMValueEditedByUserMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.M.ValueEditedByUserMessage", DefaultText = "Value edited by user",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteQuotationMFileUploadedManuallyMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.Quotation.M.FileUploadedManuallyMessage", DefaultText = "File uploaded manually",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteQuotationMHaveAllTheQuoteOPTemplatesMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Quote.Quotation.M.HaveAllTheQuoteOPTemplatesMessage", DefaultText = "You have all the Quote Templates",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowHeaderLabels = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowHeaderLabels", DefaultText = "Show Header Labels",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowTotalSplitToMultipleCurrencies = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowTotalSplitToMultipleCurrencies", DefaultText = "Show Total Split To Multiple Currencies",LocalDefaultText = @"", ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSSpaceLinesBefore = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.SpaceLinesBefore", DefaultText = "Space Lines Before",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSQuoteOPTemplatePDFMarginTop = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.QuoteOPTemplatePDFMarginTop", DefaultText = "Margin Top",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSQuoteOPTemplatePDFMarginBottom = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.QuoteOPTemplatePDFMarginBottom", DefaultText = "Margin Bottom",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowIncludedCharges = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowIncludedCharges", DefaultText = "Show Included Charges",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateMAddNewQuoteOPTemplateMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.M.AddNewQuoteOPTemplateMessage", DefaultText = "Here you can add new Quote template  from Logitude's Quote template list",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSDesignAreaFreeText = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.DesignAreaFreeText", DefaultText = "Design Area Free Text",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowVATType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowVATType", DefaultText = "Show VAT Type",LocalDefaultText = @"Show VAT Type", ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowVATPercentage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowVATPercentage", DefaultText = "Show VAT Percentage",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSDesign = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.Design", DefaultText = "Design",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSColumnsSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ColumnsSettings", DefaultText = "Columns Settings",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSPageNumbering = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.PageNumbering", DefaultText = "Page Numbering",LocalDefaultText = null, ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode QuoteOPTemplateTextCode_QuoteOPTemplateSShowRegionalTax = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteOPTemplate.S.ShowRegionalTax", DefaultText = "Show Regional Tax",LocalDefaultText = @"Show Regional Tax", ObjectTableId = QuoteOPTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 