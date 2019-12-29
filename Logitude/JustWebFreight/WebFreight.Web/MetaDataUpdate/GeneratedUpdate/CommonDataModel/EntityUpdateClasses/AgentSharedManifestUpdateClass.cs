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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.CommonDataModel.EntityUpdateClasses
{
   public class AgentSharedManifestUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "AgentSharedManifest",
			      				    DBTableName =  "AgentSharedManifests",
			      				    ObjectTableSingular =  "Agent Shared Manifest",
			      				    ObjectTablePlural =  "Agent Shared Manifests",
			      				    DefaultText =  "Agent Shared Manifest",
			      				    Name =  "Agent Shared Manifests",
			      				    IsNewWizard =  false,
			      				    HasCustomFilter =  false,
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  true,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  true,
			      				    HasCounter =  false,
			      				    EnableEditFromLOV =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  true,
			      				    SortingByObjectField =  "CreateDate",
			      				    CustomFieldsCount =  0,
			      				    HasCustomFields =  false,
			      				    InActive =  false,
			      				    SearchFields =  "AgentSharedManifest,AgentSharedManifests,,Id,CreateDate",
			      				    IsSaveButtonVisible =  true,
			      				    EnableSecurity =  true,
			      				    ObjectTableTypeCode =  "MD",
			      				    IsComposition =  false,
			      				    MaxNumberOfCustomFields =  0,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsEditable =  false,
			      				    AllowedForComputingPartners =  false,
			      				    DisableSearchBox =  false,
			      				    ClientModuleName =  "Common",
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasMenuButtons =  false,
			      				    HasFiltersMenu =  false,
			      				    AllowedInQueues =  false,
			      				    IsTabsHidden =  false,
			      				    Code =  "ASMN",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Master",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
					  						FieldCode =  "AgentSharedManifest.Master",
					  						Code =  "Master",
					  						MaxLength =  20,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
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
					  						PMPropertyPath =  "Master",
					  						ListPropertyPath =  "Master",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
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
					  						DisplayOnLookUpLocal =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "Master",
					  						DefaultText =  "Master",
					  						ListFieldLable =  "MasterListLable",
					  						ListLableDefaultText =  "Master",
					  						HelpTextCode =  "Master",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AgentReference",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
					  						FieldCode =  "AgentSharedManifest.AgentReference",
					  						Code =  "AgentReference",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
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
					  						PMPropertyPath =  "AgentReference",
					  						ListPropertyPath =  "AgentReference",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
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
					  						DisplayOnLookUpLocal =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  true,
					  						FullFieldLable =  "AgentReference",
					  						DefaultText =  "Agent Ref.",
					  						ListFieldLable =  "AgentReferenceListLable",
					  						ListLableDefaultText =  "Agent Ref.",
					  						HelpTextCode =  "AgentReference",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreateDate",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "DateTime",
					  						FieldCode =  "AgentSharedManifest.CreateDate",
					  						Code =  "CreateDate",
					  						MaxLength =  1,
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
					  						PMPropertyPath =  "CreateDate",
					  						ListPropertyPath =  "CreateDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
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
					  						DisplayOnLookUpLocal =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						HasTemplate =  true,
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  true,
					  						FullFieldLable =  "CreateDate",
					  						DefaultText =  "Create Date",
					  						ListFieldLable =  "CreateDateListLable",
					  						ListLableDefaultText =  "Create Date",
					  						HelpTextCode =  "CreateDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdateDate",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "DateTime",
					  						FieldCode =  "AgentSharedManifest.UpdateDate",
					  						Code =  "UpdateDate",
					  						MaxLength =  1,
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
					  						PMPropertyPath =  "UpdateDate",
					  						ListPropertyPath =  "UpdateDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
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
					  						DisplayOnLookUpLocal =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						HasTemplate =  true,
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  true,
					  						FullFieldLable =  "UpdateDate",
					  						DefaultText =  "Update Date",
					  						ListFieldLable =  "UpdateDateListLable",
					  						ListLableDefaultText =  "Update Date",
					  						HelpTextCode =  "UpdateDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedByUserId",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "DateTime",
					  						LookUpTableName =  "User",
					  						FieldCode =  "AgentSharedManifest.UpdatedByUserId",
					  						Code =  "UpdatedByUserId",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "UpdatedByUserId",
					  						ListPropertyPath =  "UpdatedByUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
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
					  						DisplayOnLookUpLocal =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  true,
					  						FullFieldLable =  "UpdatedByUserId",
					  						DefaultText =  "Updated By",
					  						HelpTextCode =  "UpdatedByUserId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DirectionId",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
					  						FieldCode =  "AgentSharedManifest.DirectionId",
					  						Code =  "DirectionId",
					  						MaxLength =  100,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
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
					  						PMPropertyPath =  "DirectionId",
					  						ListPropertyPath =  "DirectionId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
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
					  						DisplayOnLookUpLocal =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						ColumnHeaderTemplateName =  "DirectionHeaderTemplate",
					  						HasTemplate =  true,
					  						HtmlListComponentName =  "DirectionCellDisplayListTemplate",
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "DirectionId",
					  						DefaultText =  "Direction",
					  						ListFieldLable =  "DirectionIdListLable",
					  						ListLableDefaultText =  "Direction",
					  						HelpTextCode =  "DirectionId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransportModeId",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
					  						FieldCode =  "AgentSharedManifest.TransportModeId",
					  						Code =  "TransportModeId",
					  						MaxLength =  100,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
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
					  						PMPropertyPath =  "TransportModeId",
					  						ListPropertyPath =  "TransportModeId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
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
					  						DisplayOnLookUpLocal =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						ColumnHeaderTemplateName =  "TransportModeHeaderTemplate",
					  						HasTemplate =  true,
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "TransportModeId",
					  						DefaultText =  "Transport Mode",
					  						ListFieldLable =  "TransportModeIdListLable",
					  						ListLableDefaultText =  "Transport Mode",
					  						HelpTextCode =  "TransportModeId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PackagesQuantity",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Integer",
					  						FieldCode =  "AgentSharedManifest.PackagesQuantity",
					  						Code =  "PackagesQuantity",
					  						MaxLength =  1,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "PackagesQuantity",
					  						ListPropertyPath =  "PackagesQuantity",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
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
					  						DisplayOnLookUpLocal =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "PackagesQuantity",
					  						DefaultText =  "QTY",
					  						ListFieldLable =  "PackagesQuantityListLable",
					  						ListLableDefaultText =  "QTY",
					  						HelpTextCode =  "PackagesQuantity",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StatusName",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
					  						FieldCode =  "AgentSharedManifest.StatusName",
					  						Code =  "StatusName",
					  						MaxLength =  100,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
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
					  						PMPropertyPath =  "StatusName",
					  						ListPropertyPath =  "StatusName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
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
					  						DisplayOnLookUpLocal =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  true,
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "StatusName",
					  						DefaultText =  "Status",
					  						ListFieldLable =  "StatusNameListLable",
					  						ListLableDefaultText =  "Status",
					  						HelpTextCode =  "StatusName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ShipmentlevelCode",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
					  						LookUpTableName =  "AgentSharedManifest",
					  						FieldCode =  "AgentSharedManifest.ShipmentlevelCode",
					  						Code =  "ShipmentlevelCode",
					  						MaxLength =  100,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
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
					  						PMPropertyPath =  "ShipmentlevelCode",
					  						ListPropertyPath =  "ShipmentlevelCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
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
					  						DisplayOnLookUpLocal =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "ShipmentlevelCode",
					  						DefaultText =  "Shipment level",
					  						ListFieldLable =  "ShipmentlevelCodeListLable",
					  						ListLableDefaultText =  "Shipment level",
					  						HelpTextCode =  "ShipmentlevelCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AgentName",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
					  						FieldCode =  "AgentSharedManifest.AgentName",
					  						Code =  "AgentName",
					  						MaxLength =  100,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
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
					  						PMPropertyPath =  "AgentName",
					  						ListPropertyPath =  "AgentName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
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
					  						DisplayOnLookUpLocal =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "AgentName",
					  						DefaultText =  "Agent",
					  						ListFieldLable =  "AgentNameListLable",
					  						ListLableDefaultText =  "Agent",
					  						HelpTextCode =  "AgentName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Routing",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
					  						LookUpTableName =  "AgentSharedManifest",
					  						FieldCode =  "AgentSharedManifest.Routing",
					  						Code =  "Routing",
					  						MaxLength =  100,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
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
					  						PMPropertyPath =  "Routing",
					  						ListPropertyPath =  "Routing",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
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
					  						DisplayOnLookUpLocal =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "Routing",
					  						DefaultText =  "Routing",
					  						ListFieldLable =  "RoutingListLable",
					  						ListLableDefaultText =  "Routing",
					  						HelpTextCode =  "Routing",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StatusCode",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
					  						FieldCode =  "AgentSharedManifest.StatusCode",
					  						Code =  "StatusCode",
					  						MaxLength =  20,
					  						IsCustom =  false,
					  						MinLength =  0,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "StatusCode",
					  						ListPropertyPath =  "StatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
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
					  						DisplayOnLookUpLocal =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "StatusCode",
					  						DefaultText =  "Status Code",
					  						ListFieldLable =  "StatusCodeListLable",
					  						ListLableDefaultText =  "Status Code",
					  						HelpTextCode =  "StatusCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "GrossWeight",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Decimal",
					  						FieldCode =  "AgentSharedManifest.GrossWeight",
					  						Code =  "GrossWeight",
					  						MaxLength =  10,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "GrossWeight",
					  						ListPropertyPath =  "GrossWeight",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
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
					  						DisplayOnLookUpLocal =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "GrossWeight",
					  						DefaultText =  "Gross Weight",
					  						ListFieldLable =  "GrossWeightListLable",
					  						ListLableDefaultText =  "Gross Weight",
					  						HelpTextCode =  "GrossWeight",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TEU",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Double",
					  						FieldCode =  "AgentSharedManifest.TEU",
					  						Code =  "TEU",
					  						MaxLength =  15,
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
					  						PMPropertyPath =  "TEU",
					  						ListPropertyPath =  "TEU",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  2,
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
					  						DisplayOnLookUpLocal =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "TEU",
					  						DefaultText =  "TEU",
					  						ListFieldLable =  "TEUListLable",
					  						ListLableDefaultText =  "TEU",
					  						HelpTextCode =  "TEU",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ShipmentLevelName",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
					  						FieldCode =  "AgentSharedManifest.ShipmentLevelName",
					  						Code =  "ShipmentLevelName",
					  						MaxLength =  100,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
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
					  						PMPropertyPath =  "ShipmentLevelName",
					  						ListPropertyPath =  "ShipmentLevelName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
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
					  						DisplayOnLookUpLocal =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "ShipmentLevelName",
					  						DefaultText =  "Type",
					  						ListFieldLable =  "ShipmentLevelNameListLable",
					  						ListLableDefaultText =  "Type",
					  						HelpTextCode =  "ShipmentLevelName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup AgentSharedManifestQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "ASMN", Name = "Agent Shared Manifests" }, queryGroupRepository);
						QueryGroup AgentSharedManifestQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "AASM", Name = "Air Agent Shared Manifests" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable AgentSharedManifestObjectTable = objectContext.ObjectTables.Where(d => d.Name == "AgentSharedManifest" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> AgentSharedManifestObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "AgentSharedManifest").ToList();   

			   TextCode AgentSharedManifestTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AgentSharedManifest.Q.AgentSharedManifests", DefaultText = @"All Agent Shared Manifests",LocalDefaultText = null, ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature AgentSharedManifestFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AgentSharedManifestQ", ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, NameTextCodeCode = "AgentSharedManifest.Features.AgentSharedManifestS", NameTextCodeDefaultText = "All Agent Shared Manifests", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode AgentSharedManifestTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AgentSharedManifest.Q.AirAgentSharedManifests", DefaultText = @"Air Agent Shared Manifests",LocalDefaultText = null, ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature AgentSharedManifestFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AirAgentSharedManifestsQ", ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, NameTextCodeCode = "AgentSharedManifest.Features.AirAgentSharedManifests", NameTextCodeDefaultText = "Air Agent Shared Manifests", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode AgentSharedManifestTextCode_2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AgentSharedManifest.Q.OceanAgentSharedManifests", DefaultText = @"Ocean Agent Shared Manifests",LocalDefaultText = null, ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature AgentSharedManifestFeature_2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OceanAgentSharedManifestsQ", ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, NameTextCodeCode = "AgentSharedManifest.Features.OceanAgentSharedManifests", NameTextCodeDefaultText = "Ocean Agent Shared Manifests", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode AgentSharedManifestTextCode_3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AgentSharedManifest.Q.InlandAgentSharedManifests", DefaultText = @"Inland Agent Shared Manifests",LocalDefaultText = null, ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature AgentSharedManifestFeature_3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "InlandAgentSharedManifestsQ", ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, NameTextCodeCode = "AgentSharedManifest.Features.InlandAgentSharedManifests", NameTextCodeDefaultText = "Inland Agent Shared Manifests", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode AgentSharedManifestTextCode_4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AgentSharedManifest.Q.CancelledAgentSharedManifests", DefaultText = @"Cancelled Agent Shared Manifests",LocalDefaultText = null, ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature AgentSharedManifestFeature_4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CancelledAgentSharedManifestsQ", ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, NameTextCodeCode = "AgentSharedManifest.Features.CancelledAgentSharedManifests", NameTextCodeDefaultText = "Cancelled Agent Shared Manifests", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AgentSharedManifestsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = AgentSharedManifestTextCode_0.Id, NameTextCodeCode = AgentSharedManifestTextCode_0.Code, Code = "Agent Shared Manifests",  EditWizardName = "./Common/Components/SharedManifest/SharedManifestComponent",
			   QueryGroupCode = "ASMN", IndexOrder = 0, Tenant = 0, ObjectTableId = AgentSharedManifestObjectTable.Id, QuerySection = "AgentSharedManifest", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = AgentSharedManifestFeature_0.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AgentSharedManifestsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id, IndexOrder = 0, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id, IndexOrder = 1, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id, IndexOrder = 2, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "Master" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "Master" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id, IndexOrder = 3, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "AgentName" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "AgentName" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id, IndexOrder = 4, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "Routing" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "Routing" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id, IndexOrder = 5, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "AgentReference" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "AgentReference" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id, IndexOrder = 6, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "ShipmentLevelName" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "ShipmentLevelName" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id, IndexOrder = 7, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "GrossWeight" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "GrossWeight" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id, IndexOrder = 8, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "ShipmentlevelCode" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "ShipmentlevelCode" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id, IndexOrder = 8, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "PackagesQuantity" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "PackagesQuantity" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id, IndexOrder = 9, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id, IndexOrder = 10, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TEU" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TEU" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id, IndexOrder = 11, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_13 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id, IndexOrder = 12, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
  
	      

			  Query AirAgentSharedManifestsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = AgentSharedManifestTextCode_1.Id, NameTextCodeCode = AgentSharedManifestTextCode_1.Code, Code = "AirAgentSharedManifests",  EditWizardName = "./Common/Components/SharedManifest/SharedManifestComponent",
			   QueryGroupCode = "AASM", IndexOrder = 0, Tenant = 0, ObjectTableId = AgentSharedManifestObjectTable.Id, QuerySection = "AgentSharedManifest", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = AgentSharedManifestFeature_1.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AirAgentSharedManifestsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id, IndexOrder = 0, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id, IndexOrder = 1, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id, IndexOrder = 2, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "Master" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "Master" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id, IndexOrder = 3, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "AgentName" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "AgentName" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id, IndexOrder = 4, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "Routing" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "Routing" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id, IndexOrder = 5, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "AgentReference" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "AgentReference" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id, IndexOrder = 6, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "ShipmentLevelName" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "ShipmentLevelName" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id, IndexOrder = 7, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "GrossWeight" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "GrossWeight" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id, IndexOrder = 8, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "ShipmentlevelCode" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "ShipmentlevelCode" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id, IndexOrder = 8, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "PackagesQuantity" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "PackagesQuantity" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id, IndexOrder = 9, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TEU" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TEU" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id, IndexOrder = 10, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id, IndexOrder = 11, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter AirAgentSharedManifestsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "A",PredefinedValue2 = null, QueryId = AirAgentSharedManifestsQuery.Id, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);


             AdvancedQueryFilter AirAgentSharedManifestsQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "StatusCode" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "StatusCode" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "WAIT",PredefinedValue2 = null, QueryId = AirAgentSharedManifestsQuery.Id, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query OceanAgentSharedManifestsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = AgentSharedManifestTextCode_2.Id, NameTextCodeCode = AgentSharedManifestTextCode_2.Code, Code = "OceanAgentSharedManifests",  EditWizardName = "./Common/Components/SharedManifest/SharedManifestComponent",
			   QueryGroupCode = "OASM", IndexOrder = 0, Tenant = 0, ObjectTableId = AgentSharedManifestObjectTable.Id, QuerySection = "AgentSharedManifest", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = AgentSharedManifestFeature_2.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn OceanAgentSharedManifestsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id, IndexOrder = 0, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id, IndexOrder = 1, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id, IndexOrder = 2, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "Master" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "Master" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id, IndexOrder = 3, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "AgentName" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "AgentName" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id, IndexOrder = 4, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "Routing" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "Routing" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id, IndexOrder = 5, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "AgentReference" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "AgentReference" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id, IndexOrder = 6, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "ShipmentLevelName" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "ShipmentLevelName" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id, IndexOrder = 7, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "GrossWeight" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "GrossWeight" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id, IndexOrder = 8, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "ShipmentlevelCode" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "ShipmentlevelCode" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id, IndexOrder = 8, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "PackagesQuantity" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "PackagesQuantity" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id, IndexOrder = 9, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TEU" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TEU" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id, IndexOrder = 10, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id, IndexOrder = 11, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter OceanAgentSharedManifestsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "O",PredefinedValue2 = null, QueryId = OceanAgentSharedManifestsQuery.Id, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);


             AdvancedQueryFilter OceanAgentSharedManifestsQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "StatusCode" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "StatusCode" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "WAIT",PredefinedValue2 = null, QueryId = OceanAgentSharedManifestsQuery.Id, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query InlandAgentSharedManifestsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = AgentSharedManifestTextCode_3.Id, NameTextCodeCode = AgentSharedManifestTextCode_3.Code, Code = "InlandAgentSharedManifests",  EditWizardName = "./Common/Components/SharedManifest/SharedManifestComponent",
			   QueryGroupCode = "IASM", IndexOrder = 0, Tenant = 0, ObjectTableId = AgentSharedManifestObjectTable.Id, QuerySection = "AgentSharedManifest", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = AgentSharedManifestFeature_3.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn InlandAgentSharedManifestsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id, IndexOrder = 0, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id, IndexOrder = 1, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id, IndexOrder = 2, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "Master" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "Master" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id, IndexOrder = 3, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "AgentName" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "AgentName" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id, IndexOrder = 4, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "Routing" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "Routing" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id, IndexOrder = 5, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "AgentReference" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "AgentReference" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id, IndexOrder = 6, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "ShipmentLevelName" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "ShipmentLevelName" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id, IndexOrder = 7, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "GrossWeight" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "GrossWeight" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id, IndexOrder = 8, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "ShipmentlevelCode" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "ShipmentlevelCode" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id, IndexOrder = 8, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "PackagesQuantity" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "PackagesQuantity" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id, IndexOrder = 9, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TEU" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TEU" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id, IndexOrder = 10, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id, IndexOrder = 11, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter InlandAgentSharedManifestsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "I",PredefinedValue2 = null, QueryId = InlandAgentSharedManifestsQuery.Id, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);


             AdvancedQueryFilter InlandAgentSharedManifestsQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "StatusCode" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "StatusCode" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "WAIT",PredefinedValue2 = null, QueryId = InlandAgentSharedManifestsQuery.Id, Tenant = 0,Operator = "Equal"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query CancelledAgentSharedManifestsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = AgentSharedManifestTextCode_4.Id, NameTextCodeCode = AgentSharedManifestTextCode_4.Code, Code = "CancelledAgentSharedManifests",  EditWizardName = "./Common/Components/SharedManifest/SharedManifestComponent",
			   QueryGroupCode = "CASM", IndexOrder = 0, Tenant = 0, ObjectTableId = AgentSharedManifestObjectTable.Id, QuerySection = "AgentSharedManifest", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = AgentSharedManifestFeature_4.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn CancelledAgentSharedManifestsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id, IndexOrder = 0, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id, IndexOrder = 1, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 25 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id, IndexOrder = 2, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "Master" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "Master" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id, IndexOrder = 3, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "AgentName" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "AgentName" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id, IndexOrder = 4, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "Routing" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "Routing" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id, IndexOrder = 5, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "AgentReference" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "AgentReference" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id, IndexOrder = 6, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "ShipmentLevelName" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "ShipmentLevelName" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id, IndexOrder = 7, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "GrossWeight" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "GrossWeight" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id, IndexOrder = 8, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "ShipmentlevelCode" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "ShipmentlevelCode" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id, IndexOrder = 8, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "PackagesQuantity" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "PackagesQuantity" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id, IndexOrder = 9, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TEU" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "TEU" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id, IndexOrder = 10, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id, IndexOrder = 11, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter CancelledAgentSharedManifestsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = AgentSharedManifestObjectFields.Where(d => d.FieldName == "StatusCode" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AgentSharedManifestObjectFields.Where(d => d.FieldName == "StatusCode" && d.ObjectTableId == AgentSharedManifestObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "CANC",PredefinedValue2 = null, QueryId = CancelledAgentSharedManifestsQuery.Id, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {    

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {      
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable AgentSharedManifestObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AgentSharedManifest" && d.Tenant == 0).FirstOrDefault(); 

		   Feature AgentSharedManifestFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, NameTextCodeCode = "AgentSharedManifest.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature AgentSharedManifestFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, NameTextCodeCode = "AgentSharedManifest.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature AgentSharedManifestFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, NameTextCodeCode = "AgentSharedManifest.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature AgentSharedManifestFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, NameTextCodeCode = "AgentSharedManifest.Features.PackageFeature", NameTextCodeDefaultText = "AgentSharedManifest Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes); 

		   		   //--------------> Additional Features <--------------\\

		   Feature AgentSharedManifestFeature_UPDATESHAREDAGENT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATESHAREDAGENT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, NameTextCodeCode = "AgentSharedManifest.Features.UpdateSharedAgent", NameTextCodeDefaultText = @"Update Shared Agent" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable AgentSharedManifestObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AgentSharedManifest" && d.Tenant == 0).FirstOrDefault(); 
	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {     
	    
}

    

   }
    
}
	 