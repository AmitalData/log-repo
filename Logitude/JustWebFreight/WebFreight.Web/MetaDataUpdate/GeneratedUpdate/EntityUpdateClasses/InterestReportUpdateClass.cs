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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses
{
   public class InterestReportUpdateClass
   {  		
		public const string HashString = "3fab2ff2396c343cd706a0429e07b061";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "InterestReport",
			      				    IsNew =  true,
			      				    DBTableName =  "InterestReports",
			      				    ObjectTableSingular =  "InterestReport",
			      				    ObjectTablePlural =  "InterestReports",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  true,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  false,
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
			      				    SortingByObjectField =  "CreateDateTime",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    NewWizardControlName =  "./Accounting/Components/NewEntity/NewInterestReportComponent",
			      				    LocalDefaultText =  "דוח ריבית",
			      				    DefaultText =  "Interest Report",
			      				    Code =  "25a2",
			      				    Name =  " Query Group",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Accounting",
			      				    NewWizardComponentPath =  "./Accounting/Components/NewEntity/NewInterestReportComponent",
			      				    NoTS =  false,
			      				    HasMenuButtons =  true,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    HashString =  InterestReportUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Id",
					  						ObjectTableName =  "InterestReport",
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
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Id",
					  						ListPropertyPath =  "Id",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "InterestReports",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Id",
					  						DefaultText =  "Id",
					  						FullLocalDefaultText =  "מזהה פנימי",
					  						ListFieldLable =  "IdListLable",
					  						ListLableDefaultText =  "Id",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  true,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Tenant",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "Integer",
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
					  						PMPropertyPath =  "Tenant",
					  						ListPropertyPath =  "Tenant",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "InterestReports",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Tenant",
					  						DefaultText =  "Tenant",
					  						FullLocalDefaultText =  "דייר",
					  						ListFieldLable =  "TenantListLable",
					  						ListLableDefaultText =  "Tenant",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  true,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreateDateTime",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "DateTime",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "GreaterThanOrEqual",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CreateDateTime",
					  						ListPropertyPath =  "CreateDateTime",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "InterestReports",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreateDateTime",
					  						DefaultText =  "Create Date",
					  						FullLocalDefaultText =  "שעת יצירה",
					  						ListFieldLable =  "CreateDateTimeListLable",
					  						ListLableDefaultText =  "Create Date",
					  						ListLocalDefaultText =  "שעת יצירה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  true,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedByUserId",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
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
					  						ValidForQuerySection1 =  "InterestReport",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreatedByUserId",
					  						DefaultText =  "Created By",
					  						FullLocalDefaultText =  "מזהה פנימי של היוצר",
					  						ListFieldLable =  "CreatedByUserIdListLable",
					  						ListLableDefaultText =  "Created By",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdateDateTime",
					  						ObjectTableName =  "InterestReport",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "UpdateDateTime",
					  						ListPropertyPath =  "UpdateDateTime",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "InterestReports",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdateDateTime",
					  						DefaultText =  "Update Date",
					  						FullLocalDefaultText =  "שעת עדכון",
					  						ListFieldLable =  "UpdateDateTimeListLable",
					  						ListLableDefaultText =  "Update Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  true,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedByUserId",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
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
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdatedByUserId",
					  						DefaultText =  "Updated By",
					  						FullLocalDefaultText =  "מזהה פנימי של היוצר",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "GLAccountId",
					  						ObjectTableName =  "InterestReport",
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
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "GLAccountId",
					  						ListPropertyPath =  "GLAccountId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "InterestReport",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "GLAccountId",
					  						DefaultText =  "GLAccount",
					  						FullLocalDefaultText =  "מזהה פנימי לכרטיס",
					  						ListFieldLable =  "GLAccountIdListLable",
					  						ListLableDefaultText =  "GLAccount",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ReportNumber",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ReportNumber",
					  						ListPropertyPath =  "ReportNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "InterestReport",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ReportNumber",
					  						DefaultText =  "Report Number",
					  						FullLocalDefaultText =  "דוח מספר",
					  						ListFieldLable =  "ReportNumberListLable",
					  						ListLableDefaultText =  "Report Number",
					  						ListLocalDefaultText =  "דוח מספר",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InterestCalculationDate",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  true,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "GreaterThanOrEqual",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "InterestCalculationDate",
					  						ListPropertyPath =  "InterestCalculationDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "InterestReport",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InterestCalculationDate",
					  						DefaultText =  "Calculation Date",
					  						FullLocalDefaultText =  "תאריך חישוב ריבית",
					  						ListFieldLable =  "InterestCalculationDateListLable",
					  						ListLableDefaultText =  "Calculation Date",
					  						ListLocalDefaultText =  "תאריך חישוב ריבית",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TotalAmount",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "Decimal",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TotalAmount",
					  						ListPropertyPath =  "TotalAmount",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "InterestReport",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  18,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TotalAmount",
					  						DefaultText =  "Total Interest Amount",
					  						FullLocalDefaultText =  "סכום ריבית מחושב",
					  						ListFieldLable =  "TotalAmountListLable",
					  						ListLableDefaultText =  "Total Interest Amount",
					  						ListLocalDefaultText =  "סכום ריבית מחושב",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OpenBalance",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "Decimal",
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
					  						PMPropertyPath =  "OpenBalance",
					  						ListPropertyPath =  "OpenBalance",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  18,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OpenBalance",
					  						DefaultText =  "Open Balance",
					  						FullLocalDefaultText =  "יתרת פתיחה",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CloseBalance",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "Decimal",
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
					  						PMPropertyPath =  "CloseBalance",
					  						ListPropertyPath =  "CloseBalance",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  18,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CloseBalance",
					  						DefaultText =  "Close Balance",
					  						FullLocalDefaultText =  "יתרת סגירה",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ARinvoiceId",
					  						ObjectTableName =  "InterestReport",
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
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ARinvoiceId",
					  						ListPropertyPath =  "ARinvoiceId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ARinvoiceId",
					  						DefaultText =  "ARinvoice",
					  						FullLocalDefaultText =  "מזהה פנימי לחשבונית",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvoiceAmount",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "Decimal",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "InvoiceAmount",
					  						ListPropertyPath =  "InvoiceAmount",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "InterestReport",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  18,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InvoiceAmount",
					  						DefaultText =  "Invoice Amount Including VAT",
					  						FullLocalDefaultText =  "סכום חשבונית כולל מעמ",
					  						ListFieldLable =  "InvoiceAmountListLable",
					  						ListLableDefaultText =  "Invoice Amount Including VAT",
					  						ListLocalDefaultText =  "סכום חשבונית כולל מעמ",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "GLAccountInterestCreditLimit",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "Decimal",
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
					  						PMPropertyPath =  "GLAccountInterestCreditLimit",
					  						ListPropertyPath =  "GLAccountInterestCreditLimit",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  18,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "GLAccountInterestCreditLimit",
					  						DefaultText =  "Credit Limit",
					  						FullLocalDefaultText =  "מסגרת אשראי של הכרטיס",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InterestReportStatusCode",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "InterestReportStatuse",
					  						MinLength =  0,
					  						MaxLength =  4,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "InterestReportStatusCode",
					  						ListPropertyPath =  "InterestReportStatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "InterestReport",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InterestReportStatusCode",
					  						DefaultText =  "Status Code",
					  						FullLocalDefaultText =  "קוד סטאטוס של הדוח",
					  						ListFieldLable =  "InterestReportStatusCodeListLable",
					  						ListLableDefaultText =  "Status Code",
					  						ListLocalDefaultText =  "קוד סטאטוס של הדוח",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedByLocalName",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  200,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  200,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CreatedByLocalName",
					  						ListPropertyPath =  "CreatedByLocalName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "InterestReport",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreatedByLocalName",
					  						DefaultText =  "Created By",
					  						FullLocalDefaultText =  "נוצר ע\"י משתמש",
					  						ListFieldLable =  "CreatedByLocalNameListLable",
					  						ListLableDefaultText =  "Created By",
					  						ListLocalDefaultText =  "נוצר ע\"י משתמש",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "GLAccountDisplayNumber",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "GLAccountDisplayNumber",
					  						ListPropertyPath =  "GLAccountDisplayNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "InterestReport",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "GLAccountDisplayNumber",
					  						DefaultText =  "Account No.",
					  						FullLocalDefaultText =  "מספר כרטיס",
					  						ListFieldLable =  "GLAccountDisplayNumberListLable",
					  						ListLableDefaultText =  "Account No.",
					  						ListLocalDefaultText =  "מספר כרטיס",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "GLAccountLocalName",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  105,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  105,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "GLAccountLocalName",
					  						ListPropertyPath =  "GLAccountLocalName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "InterestReport",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "GLAccountLocalName",
					  						DefaultText =  "Local Name",
					  						FullLocalDefaultText =  "שם מקומי",
					  						ListFieldLable =  "GLAccountLocalNameListLable",
					  						ListLableDefaultText =  "Local Name",
					  						ListLocalDefaultText =  "שם מקומי",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ARInvoiceNumber",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  20,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  20,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ARInvoiceNumber",
					  						ListPropertyPath =  "ARInvoiceNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ARInvoiceNumber",
					  						DefaultText =  "Invoice Number",
					  						FullLocalDefaultText =  "מספר חשבונית",
					  						ListFieldLable =  "ARInvoiceNumberListLable",
					  						ListLableDefaultText =  "Invoice No.",
					  						ListLocalDefaultText =  "מספר חשבונית",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedByLocalName",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  200,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  200,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "UpdatedByLocalName",
					  						ListPropertyPath =  "UpdatedByLocalName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "InterestReport",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdatedByLocalName",
					  						DefaultText =  "Updated By",
					  						FullLocalDefaultText =  "עודכן על ידי",
					  						ListFieldLable =  "UpdatedByLocalNameListLable",
					  						ListLableDefaultText =  "Updated By",
					  						ListLocalDefaultText =  "עודכן על ידי",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InterestReportStatusName",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  200,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  200,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "InterestReportStatusName",
					  						ListPropertyPath =  "InterestReportStatusName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "InterestReport",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InterestReportStatusName",
					  						DefaultText =  "Status Name",
					  						FullLocalDefaultText =  "סטטוס של הדוח",
					  						ListFieldLable =  "InterestReportStatusNameListLable",
					  						ListLableDefaultText =  "Status Name",
					  						ListLocalDefaultText =  "סטטוס של הדוח",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  1000,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1000,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
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
					  						ValidForQuerySection1 =  "InterestReport",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search ...",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  "Search ...",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InterestReportStatusLocalName",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  200,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  200,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "InterestReportStatusLocalName",
					  						ListPropertyPath =  "InterestReportStatusLocalName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "InterestReport",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InterestReportStatusLocalName",
					  						DefaultText =  "Status Name",
					  						FullLocalDefaultText =  "סטטוס של הדוח",
					  						ListFieldLable =  "InterestReportStatusLocalNameListLable",
					  						ListLableDefaultText =  "Status Name",
					  						ListLocalDefaultText =  "סטטוס של הדוח",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomerId",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  true,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomerId",
					  						ListPropertyPath =  "CustomerId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerId",
					  						DefaultText =  "Customer",
					  						FullLocalDefaultText =  "לקוח",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InterestReportLinesByDates",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "List",
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
					  						PMPropertyPath =  "InterestReportLinesByDates",
					  						ListPropertyPath =  "InterestReportLinesByDates",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "InterestReportLinesByDate",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InterestReportLinesByDates",
					  						DefaultText =  "Interest Report Lines By Date",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomerName",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
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
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerName",
					  						DefaultText =  "Customer",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "GLAccountMinimumInterest",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "Integer",
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
					  						PMPropertyPath =  "GLAccountMinimumInterest",
					  						ListPropertyPath =  "GLAccountMinimumInterest",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "GLAccountMinimumInterest",
					  						DefaultText =  "Minimum Interest Invoice billing",
					  						FullLocalDefaultText =  "מינימום חיוב בחשבונית ריבית",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomerLocalName",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomerLocalName",
					  						ListPropertyPath =  "CustomerLocalName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerLocalName",
					  						DefaultText =  "Customer",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EnableInvoiceing",
					  						ObjectTableName =  "InterestReport",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "EnableInvoiceing",
					  						ListPropertyPath =  "EnableInvoiceing",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EnableInvoiceing",
					  						DefaultText =  "EnableInvoiceing",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsFirstReport",
					  						ObjectTableName =  "InterestReport",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsFirstReport",
					  						ListPropertyPath =  "IsFirstReport",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsFirstReport",
					  						DefaultText =  "Is First Report",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvoiceFailureReason",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  1024,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1024,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "InvoiceFailureReason",
					  						ListPropertyPath =  "InvoiceFailureReason",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "InterestReports",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InvoiceFailureReason",
					  						DefaultText =  "InvoiceFailureReason",
					  						ListFieldLable =  "InvoiceFailureReasonListLable",
					  						ListLableDefaultText =  "InvoiceFailureReason",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsCreatedFromBatch",
					  						ObjectTableName =  "InterestReport",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsCreatedFromBatch",
					  						ListPropertyPath =  "IsCreatedFromBatch",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsCreatedFromBatch",
					  						DefaultText =  "Is Created From Batch",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BatchReportUserEmail",
					  						ObjectTableName =  "InterestReport",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "BatchReportUserEmail",
					  						ListPropertyPath =  "BatchReportUserEmail",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BatchReportUserEmail",
					  						DefaultText =  "BatchReportUserEmail",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RecalculateData",
					  						ObjectTableName =  "InterestReport",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "RecalculateData",
					  						ListPropertyPath =  "RecalculateData",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "RecalculateData",
					  						DefaultText =  "Recalculate Data",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsUpdatedFromBatch",
					  						ObjectTableName =  "InterestReport",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsUpdatedFromBatch",
					  						ListPropertyPath =  "IsUpdatedFromBatch",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsUpdatedFromBatch",
					  						DefaultText =  "Is Updated From Batch",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CanRecalculate",
					  						ObjectTableName =  "InterestReport",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CanRecalculate",
					  						ListPropertyPath =  "CanRecalculate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CanRecalculate",
					  						DefaultText =  "Can Recalculate",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvoiceDate",
					  						ObjectTableName =  "InterestReport",
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
					  						PMPropertyPath =  "InvoiceDate",
					  						ListPropertyPath =  "InvoiceDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InvoiceDate",
					  						DefaultText =  "InvoiceDate",
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
	        QueryGroup InterestReportQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "25a2", Name = " Query Group" }, queryGroupRepository,tenantQueryGroups);
						QueryGroup InterestReportQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "2123", Name = " Query Group" }, queryGroupRepository,tenantQueryGroups);
				        queryGroupRepository.SubmitChanges();
	        ObjectTable InterestReportObjectTable = objectTables.ContainsKey("InterestReport") ? objectTables["InterestReport"] : null;
            if (InterestReportObjectTable == null)
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(0);  

                InterestReportObjectTable = objectContext.ObjectTables.Where(d => d.Name == "InterestReport" && d.Tenant == 0).FirstOrDefault();
            }

	         
			List<Feature> addedFeatures = new List<Feature>();
			List<TextCode> addedTextCodes = new List<TextCode>();
			List<Query> addedQueries = new List<Query>();
			List<QueryColumn> addedQueryColumns = new List<QueryColumn>();
			List<AdvancedQueryFilter> addedQueryFilters = new List<AdvancedQueryFilter>();
   

			   TextCode InterestReportTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.Q.InterestReport", DefaultText = @"All Reports",LocalDefaultText = "כל הדוחות", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature InterestReportFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "InterestReport.Q.InterestReport", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "InterestReportFeatures.InterestReport", NameTextCodeDefaultText = "Interest Report", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,InterestReportObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode InterestReportTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.Q.DraftReports", DefaultText = @"Draft Reports",LocalDefaultText = "דוחות טיוטה", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature InterestReportFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "InterestReport.Q.DraftReports", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "InterestReportFeatures.DraftReports", NameTextCodeDefaultText = "DraftReports", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,InterestReportObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode InterestReportTextCode_2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.Q.InvoicedReports", DefaultText = @"Invoiced Reports",LocalDefaultText = "דוחות עם חשבונית", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature InterestReportFeature_2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "InterestReport.Q.InvoicedReports", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "InterestReportFeatures.InvoicedReports", NameTextCodeDefaultText = "InvoicedReports", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,InterestReportObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode InterestReportTextCode_3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.Q.ClosedWithoutInvoice", DefaultText = @"Closed Without Invoice",LocalDefaultText = "דוחות שנסגרו ללא חשבונית", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature InterestReportFeature_3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "InterestReport.Q.ClosedWithoutInvoice", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "InterestReportFeatures.ClosedWithoutInvoice", NameTextCodeDefaultText = "ClosedWithoutInvoice", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,InterestReportObjectTable, addedFeatures, addedTextCodes);

	        //TextCodeRepository.SubmitChanges();
	        //FeaturesRepository.SubmitChanges();    
	      

			  Query InterestReportQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = InterestReportTextCode_0.Id, NameTextCodeCode = InterestReportTextCode_0.Code, ObjectTableName = "InterestReport", Code = "Interest Report",  QueryGroupCode = "25a2", IndexOrder = 0, Tenant = 0, ObjectTableId = InterestReportObjectTable.Id, QuerySection = "InterestReport", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = InterestReportFeature_0.Id,FeatureUniqeCode= InterestReportFeature_0.FeatureUniqeCode, DefaultSortName = "CreateDateTime", DefaultSortDirection = "Desending", Perspective = null }, addedQueries);
	
			 QueryColumn InterestReportQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InterestReportQuery.Id,QueryCode = InterestReportQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "InterestReport.ReportNumber" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn InterestReportQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InterestReportQuery.Id,QueryCode = InterestReportQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "InterestReport.CreateDateTime" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn InterestReportQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InterestReportQuery.Id,QueryCode = InterestReportQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "InterestReport.CreatedByLocalName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn InterestReportQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InterestReportQuery.Id,QueryCode = InterestReportQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "InterestReport.GLAccountDisplayNumber" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn InterestReportQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InterestReportQuery.Id,QueryCode = InterestReportQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "InterestReport.GLAccountLocalName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn InterestReportQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InterestReportQuery.Id,QueryCode = InterestReportQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "InterestReport.InterestReportStatusName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn InterestReportQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InterestReportQuery.Id,QueryCode = InterestReportQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "InterestReport.TotalAmount" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn InterestReportQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InterestReportQuery.Id,QueryCode = InterestReportQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "InterestReport.ARInvoiceNumber" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn InterestReportQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InterestReportQuery.Id,QueryCode = InterestReportQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "InterestReport.InvoiceAmount" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn InterestReportQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InterestReportQuery.Id,QueryCode = InterestReportQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "InterestReport.InterestCalculationDate" , ColumnWidth = 130 }, addedQueryColumns);
  
	      

			  Query DraftReportsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = InterestReportTextCode_1.Id, NameTextCodeCode = InterestReportTextCode_1.Code, ObjectTableName = "InterestReport", Code = "DraftReports",  QueryGroupCode = "25a2", IndexOrder = 1, Tenant = 0, ObjectTableId = InterestReportObjectTable.Id, QuerySection = "InterestReport", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = InterestReportFeature_1.Id,FeatureUniqeCode= InterestReportFeature_1.FeatureUniqeCode, DefaultSortName = "CreateDateTime", DefaultSortDirection = "Desending", Perspective = null }, addedQueries);
	
			 QueryColumn DraftReportsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftReportsQuery.Id,QueryCode = DraftReportsQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "InterestReport.ReportNumber" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn DraftReportsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftReportsQuery.Id,QueryCode = DraftReportsQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "InterestReport.CreateDateTime" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn DraftReportsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftReportsQuery.Id,QueryCode = DraftReportsQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "InterestReport.CreatedByLocalName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn DraftReportsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftReportsQuery.Id,QueryCode = DraftReportsQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "InterestReport.GLAccountDisplayNumber" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn DraftReportsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftReportsQuery.Id,QueryCode = DraftReportsQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "InterestReport.GLAccountLocalName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn DraftReportsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftReportsQuery.Id,QueryCode = DraftReportsQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "InterestReport.InterestReportStatusName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn DraftReportsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftReportsQuery.Id,QueryCode = DraftReportsQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "InterestReport.TotalAmount" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn DraftReportsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftReportsQuery.Id,QueryCode = DraftReportsQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "InterestReport.ARInvoiceNumber" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn DraftReportsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftReportsQuery.Id,QueryCode = DraftReportsQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "InterestReport.InvoiceAmount" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn DraftReportsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftReportsQuery.Id,QueryCode = DraftReportsQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "InterestReport.InterestCalculationDate" , ColumnWidth = 130 }, addedQueryColumns);

             AdvancedQueryFilter DraftReportsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "InterestReport.InterestReportStatusCode", PredefinedValue = "1",PredefinedValue2 = null, QueryId = DraftReportsQuery.Id,QueryCode = DraftReportsQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query InvoicedReportsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = InterestReportTextCode_2.Id, NameTextCodeCode = InterestReportTextCode_2.Code, ObjectTableName = "InterestReport", Code = "InvoicedReports",  QueryGroupCode = "25a2", IndexOrder = 2, Tenant = 0, ObjectTableId = InterestReportObjectTable.Id, QuerySection = "InterestReport", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = InterestReportFeature_2.Id,FeatureUniqeCode= InterestReportFeature_2.FeatureUniqeCode, DefaultSortName = "CreateDateTime", DefaultSortDirection = "Desending", Perspective = null }, addedQueries);
	
			 QueryColumn InvoicedReportsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InvoicedReportsQuery.Id,QueryCode = InvoicedReportsQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "InterestReport.ReportNumber" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn InvoicedReportsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InvoicedReportsQuery.Id,QueryCode = InvoicedReportsQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "InterestReport.CreateDateTime" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn InvoicedReportsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InvoicedReportsQuery.Id,QueryCode = InvoicedReportsQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "InterestReport.CreatedByLocalName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn InvoicedReportsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InvoicedReportsQuery.Id,QueryCode = InvoicedReportsQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "InterestReport.GLAccountDisplayNumber" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn InvoicedReportsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InvoicedReportsQuery.Id,QueryCode = InvoicedReportsQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "InterestReport.GLAccountLocalName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn InvoicedReportsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InvoicedReportsQuery.Id,QueryCode = InvoicedReportsQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "InterestReport.InterestReportStatusName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn InvoicedReportsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InvoicedReportsQuery.Id,QueryCode = InvoicedReportsQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "InterestReport.TotalAmount" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn InvoicedReportsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InvoicedReportsQuery.Id,QueryCode = InvoicedReportsQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "InterestReport.ARInvoiceNumber" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn InvoicedReportsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InvoicedReportsQuery.Id,QueryCode = InvoicedReportsQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "InterestReport.InvoiceAmount" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn InvoicedReportsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InvoicedReportsQuery.Id,QueryCode = InvoicedReportsQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "InterestReport.InterestCalculationDate" , ColumnWidth = 130 }, addedQueryColumns);

             AdvancedQueryFilter InvoicedReportsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "InterestReport.InterestReportStatusCode", PredefinedValue = "2",PredefinedValue2 = null, QueryId = InvoicedReportsQuery.Id,QueryCode = InvoicedReportsQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query ClosedWithoutInvoiceQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = InterestReportTextCode_3.Id, NameTextCodeCode = InterestReportTextCode_3.Code, ObjectTableName = "InterestReport", Code = "ClosedWithoutInvoice",  QueryGroupCode = "25a2", IndexOrder = 3, Tenant = 0, ObjectTableId = InterestReportObjectTable.Id, QuerySection = "InterestReport", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = InterestReportFeature_3.Id,FeatureUniqeCode= InterestReportFeature_3.FeatureUniqeCode, DefaultSortName = "CreateDateTime", DefaultSortDirection = "Desending", Perspective = null }, addedQueries);
	
			 QueryColumn ClosedWithoutInvoiceQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedWithoutInvoiceQuery.Id,QueryCode = ClosedWithoutInvoiceQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "InterestReport.ReportNumber" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ClosedWithoutInvoiceQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedWithoutInvoiceQuery.Id,QueryCode = ClosedWithoutInvoiceQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "InterestReport.CreateDateTime" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ClosedWithoutInvoiceQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedWithoutInvoiceQuery.Id,QueryCode = ClosedWithoutInvoiceQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "InterestReport.CreatedByLocalName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ClosedWithoutInvoiceQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedWithoutInvoiceQuery.Id,QueryCode = ClosedWithoutInvoiceQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "InterestReport.GLAccountDisplayNumber" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ClosedWithoutInvoiceQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedWithoutInvoiceQuery.Id,QueryCode = ClosedWithoutInvoiceQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "InterestReport.GLAccountLocalName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ClosedWithoutInvoiceQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedWithoutInvoiceQuery.Id,QueryCode = ClosedWithoutInvoiceQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "InterestReport.InterestReportStatusName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ClosedWithoutInvoiceQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedWithoutInvoiceQuery.Id,QueryCode = ClosedWithoutInvoiceQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "InterestReport.TotalAmount" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ClosedWithoutInvoiceQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedWithoutInvoiceQuery.Id,QueryCode = ClosedWithoutInvoiceQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "InterestReport.ARInvoiceNumber" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ClosedWithoutInvoiceQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedWithoutInvoiceQuery.Id,QueryCode = ClosedWithoutInvoiceQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "InterestReport.InvoiceAmount" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ClosedWithoutInvoiceQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedWithoutInvoiceQuery.Id,QueryCode = ClosedWithoutInvoiceQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "InterestReport.InterestCalculationDate" , ColumnWidth = 130 }, addedQueryColumns);

             AdvancedQueryFilter ClosedWithoutInvoiceQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "InterestReport.InterestReportStatusCode", PredefinedValue = "4",PredefinedValue2 = null, QueryId = ClosedWithoutInvoiceQuery.Id,QueryCode = ClosedWithoutInvoiceQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

			SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
			SqlBulkInsert.BulkInsert("Features", addedFeatures);
			SqlBulkInsert.BulkInsert("Queries", addedQueries);
			SqlBulkInsert.BulkInsert("QueryColumns", addedQueryColumns);
			SqlBulkInsert.BulkInsert("AdvancedQueryFilters", addedQueryFilters);	 
  
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable InterestReportObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "InterestReport" && d.Tenant == 0).FirstOrDefault();
		   //List<ObjectField> InterestReportObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "InterestReport").ToList();
		       
	      

	         Screen InterestReportInterestReportHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "InterestReport.HeaderScreen", Name = "InterestReportHeaderScreen", ObjectTableId = InterestReportObjectTable.Id, NumberOfColumns = 3, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
             ScreenField InterestReportInterestReportHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ScreenId = InterestReportInterestReportHeaderScreenScreen0.Id,ScreenCode = InterestReportInterestReportHeaderScreenScreen0.Code, ObjectFieldCode = "InterestReport.ReportNumber", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField InterestReportInterestReportHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ScreenId = InterestReportInterestReportHeaderScreenScreen0.Id,ScreenCode = InterestReportInterestReportHeaderScreenScreen0.Code, ObjectFieldCode = "InterestReport.InterestCalculationDate", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField InterestReportInterestReportHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ScreenId = InterestReportInterestReportHeaderScreenScreen0.Id,ScreenCode = InterestReportInterestReportHeaderScreenScreen0.Code, ObjectFieldCode = "InterestReport.InterestReportStatusLocalName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField InterestReportInterestReportHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ScreenId = InterestReportInterestReportHeaderScreenScreen0.Id,ScreenCode = InterestReportInterestReportHeaderScreenScreen0.Code, ObjectFieldCode = "InterestReport.ARInvoiceNumber", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField InterestReportInterestReportHeaderScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ScreenId = InterestReportInterestReportHeaderScreenScreen0.Id,ScreenCode = InterestReportInterestReportHeaderScreenScreen0.Code, ObjectFieldCode = "InterestReport.CreatedByLocalName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField InterestReportInterestReportHeaderScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 1, ScreenId = InterestReportInterestReportHeaderScreenScreen0.Id,ScreenCode = InterestReportInterestReportHeaderScreenScreen0.Code, ObjectFieldCode = "InterestReport.UpdatedByLocalName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          	
		    InterestReportObjectTable.HeaderScreenId = InterestReportInterestReportHeaderScreenScreen0.Id;
		    InterestReportObjectTable.HeaderScreenCode = InterestReportInterestReportHeaderScreenScreen0.Code;

	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable InterestReportObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "InterestReport" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode InterestReportGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.TH.General", DefaultText = "General",LocalDefaultText = "כללי", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature InterestReportGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "InterestReport.Tab.General", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "InterestReportFeatures.IRGN", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,InterestReportObjectTable);
 
                 
			   TextCode InterestReportEventsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.TH.Events", DefaultText = "Events",LocalDefaultText = "אירועים", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature InterestReportEventsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "InterestReport.Tab.Events", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "InterestReportFeatures.IREV", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,InterestReportObjectTable);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "IRGN",HtmlComponentName = "",HtmlComponentUrl = "./Accounting/Components/EditTabs/InterestReport/GeneralTab/InterestReportGeneralTabComponent", FeatureId = InterestReportGeneralFeature_TH0.Id,FeatureUniqeCode = InterestReportGeneralFeature_TH0.FeatureUniqeCode, ControlPath = "./Accounting/Components/EditTabs/InterestReport/GeneralTab/InterestReportGeneralTabComponent", ObjectTableId = InterestReportObjectTable.Id, TabNameTextCodeId = InterestReportGeneralTextCode_TH0.Id, TabNameTextCodeCode = InterestReportGeneralTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "IREV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = InterestReportEventsFeature_TH1.Id,FeatureUniqeCode = InterestReportEventsFeature_TH1.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = InterestReportObjectTable.Id, TabNameTextCodeId = InterestReportEventsTextCode_TH1.Id, TabNameTextCodeCode = InterestReportEventsTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable InterestReportObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "InterestReport" && d.Tenant == 0).FirstOrDefault(); 

		   Feature InterestReportFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "InterestReport.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,InterestReportObjectTable);
		   Feature InterestReportFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "InterestReport.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,InterestReportObjectTable);
		   Feature InterestReportFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "InterestReport.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,InterestReportObjectTable);
		   Feature InterestReportFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = false, ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "InterestReport.Features.PackageFeature", NameTextCodeDefaultText = "InterestReport Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,InterestReportObjectTable);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable InterestReportObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "InterestReport" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = InterestReportObjectTable.Id,
				 
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
                ObjectTableId = InterestReportObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "IRCN",
                EnglishName =  "Cancelled",
                LocalName =  "Cancelled",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = InterestReportObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "IRIN",
                EnglishName =  "Invoiced",
                LocalName =  "Invoiced",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = InterestReportObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "IRCW",
                EnglishName =  "Closed without Invoice",
                LocalName =  "Closed without Invoice",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = InterestReportObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "IRIF",
                EnglishName =  "Invoicing Failed",
                LocalName =  "Invoicing Failed",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = InterestReportObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "IRFD",
                EnglishName =  "Failed",
                LocalName =  "Failed",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = InterestReportObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "IRCD",
                EnglishName =  "Created",
                LocalName =  "Created",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = InterestReportObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "IRUP",
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
                ObjectTableId = InterestReportObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   //FeatureRepository featureRepository = new FeatureRepository(0); 
		   //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable InterestReportObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "InterestReport" && d.Tenant == 0).FirstOrDefault();       
    
			   Feature InterestReportFeature_MB0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CreateInvoice", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "InterestReport.Features.CreateInvoice", NameTextCodeDefaultText = "Create Invoice", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,InterestReportObjectTable);

			   Feature InterestReportFeature_MB10 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "InterestPrint", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "InterestReport.Features.Print", NameTextCodeDefaultText = "Print", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,InterestReportObjectTable);
             			   Feature InterestReportFeature_MB11 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "IRCN", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "InterestReport.Features.Cancel", NameTextCodeDefaultText = "Cancel", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,InterestReportObjectTable);
             			   Feature InterestReportFeature_MB12 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CloseWithoutInvoice", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "InterestReport.Features.CloseWithoutInvoice", NameTextCodeDefaultText = "Close Without Invoice", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,InterestReportObjectTable);
             			   Feature InterestReportFeature_MB13 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RecalculateReport", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "InterestReport.Features.RecalculateReport", NameTextCodeDefaultText = "Recalculate Report", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,InterestReportObjectTable);
              

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup InterestReportMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "InterestReportEdit",
					Name = "InterestReportEditButtonsGroup",
					ObjectTableId = InterestReportObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton InterestReportMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "CreateInvoice",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "InterestReport.B.CreateInvoice",
						LabelTextCodeDefaultText = "Create Invoice",
						Tenant = 0,
						MenuButtonGroupId = InterestReportMenuButtonGroup.Id,
						ObjectTableId = InterestReportObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = InterestReportFeature_MB0.Id,
						Style = "ApproveButtonStyle",
						LocalDefaultText = "הפק חשבונית",
						FeatureUniqeCode = InterestReportFeature_MB0.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton InterestReportMenuButton1 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "More",
						Index = 100, 
						IsActive = false,
						LabelTextCodeCode = "InterestReport.B.More",
						LabelTextCodeDefaultText = "More",
						Tenant = 0,
						MenuButtonGroupId = InterestReportMenuButtonGroup.Id,
						ObjectTableId = InterestReportObjectTable.Id,
						MenuButtonType = "dropdownbutton",
						FeatureId = null,
						Style = null,
						LocalDefaultText = "נוספים",
						FeatureUniqeCode = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

			   MenuButton InterestReportMenuButton10 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "InterestPrint",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "InterestReport.B.Print",
						LabelTextCodeDefaultText = "Print",
						Tenant = 0,
						MenuButtonGroupId = InterestReportMenuButtonGroup.Id,
						ParentMenuButtonId = InterestReportMenuButton1.Id,
						ObjectTableId = InterestReportObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  InterestReportFeature_MB10.Id,
						Style = null,
						LocalDefaultText = "הדפסה",
						FeatureUniqeCode=  InterestReportFeature_MB10.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton InterestReportMenuButton11 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "IRCN",
						Index = 1, 
						IsActive = true,
						LabelTextCodeCode = "InterestReport.B.Cancel",
						LabelTextCodeDefaultText = "Cancel",
						Tenant = 0,
						MenuButtonGroupId = InterestReportMenuButtonGroup.Id,
						ParentMenuButtonId = InterestReportMenuButton1.Id,
						ObjectTableId = InterestReportObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  InterestReportFeature_MB11.Id,
						Style = null,
						LocalDefaultText = "ביטול דוח",
						FeatureUniqeCode=  InterestReportFeature_MB11.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton InterestReportMenuButton12 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "CloseWithoutInvoice",
						Index = 2, 
						IsActive = true,
						LabelTextCodeCode = "InterestReport.B.CloseWithoutInvoice",
						LabelTextCodeDefaultText = "Close Without Invoice",
						Tenant = 0,
						MenuButtonGroupId = InterestReportMenuButtonGroup.Id,
						ParentMenuButtonId = InterestReportMenuButton1.Id,
						ObjectTableId = InterestReportObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  InterestReportFeature_MB12.Id,
						Style = null,
						LocalDefaultText = "סגירה ללא חשבונית",
						FeatureUniqeCode=  InterestReportFeature_MB12.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton InterestReportMenuButton13 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "RecalculateReport",
						Index = 3, 
						IsActive = true,
						LabelTextCodeCode = "InterestReport.B.RecalculateReport",
						LabelTextCodeDefaultText = "Recalculate Report",
						Tenant = 0,
						MenuButtonGroupId = InterestReportMenuButtonGroup.Id,
						ParentMenuButtonId = InterestReportMenuButton1.Id,
						ObjectTableId = InterestReportObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  InterestReportFeature_MB13.Id,
						Style = null,
						LocalDefaultText = "חישוב מחדש של הדוח",
						FeatureUniqeCode=  InterestReportFeature_MB13.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable InterestReportObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "InterestReport" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode InterestReportTextCode_InterestReportOCustomerisnotdefined = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.Customerisnotdefined", DefaultText = "Customer is not defined to interest",LocalDefaultText = @"לקוח לא מוגדר לריבית", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOCustomerisnotconnected = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.Customerisnotconnected", DefaultText = "Customer is not connected to GLAccount",LocalDefaultText = @"הלקוח לא מחובר לכרטיס הנה''ח", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOCustomeralreadyhasaDraftinterest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.CustomeralreadyhasaDraftinterest", DefaultText = "Customer already has a Draft interest report  number",LocalDefaultText = @"ללקוח כבר קיים דוח ריבית בסטטוס טיוטה", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOShowDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.ShowDetails", DefaultText = "Show Details",LocalDefaultText = @"פירוט", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOCustomeralreadyhasarecent = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.Customeralreadyhasarecent", DefaultText = "Customer already has a recent interest report date number",LocalDefaultText = @"ללקוח כבר קיים דוח ריבית מתאריך מאוחר יותר- מספר דוח", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOReportTotalAmountIslowerthanGLAccountMinimumamount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.ReportTotalAmountIslowerthanGLAccountMinimumamount", DefaultText = "Report total amount is lower than GLAccount Minimum amount definition  , Close the report without Invoice ?",LocalDefaultText = @"סכום הדוח קטן מהגדרת סכום מינימום לחיוב ריבית בכרטיס , לסגור את הדוח ללא חשבונית ?", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOCancel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.Cancel", DefaultText = "Cancel",LocalDefaultText = @"ביטול", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOApprove = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.Approve", DefaultText = "Approve",LocalDefaultText = @"אישור", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOInterestForDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.InterestForDate ", DefaultText = "Interest For Date ",LocalDefaultText = @"חישוב ריבית לתאריך", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOClosingBalance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.ClosingBalance", DefaultText = "Closing Balance",LocalDefaultText = @"יתרת סגירה", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOCantCancel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.CantCancel", DefaultText = "Cant cancel this report , there’s a recent (with Higher interestReportDate) report for this customer , please cancel it first",LocalDefaultText = @"לא ניתן לבטל את הדוח מכיוון שקיים דוח מאוחר יותר ללקוח זה , אנא בטל אותו קודם", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOConfirmCancelling = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.ConfirmCancelling", DefaultText = "Please confirm canceling the report",LocalDefaultText = @"הדוח הנ”ל יבוטל , האם להמשיך", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOCancelingInvoicedReportMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.CancelingInvoicedReportMessage", DefaultText = "ARinvoice already issued for this report , cancelling the report will create an Auto Credit Invoice , Continue ?",LocalDefaultText = @"לדוח זה כבר הופקה חשבונית , ביטול הדוח יבטל את החשבונית , האם להמשיך ?", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOTheReportisinProgress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.TheReportisinProgress", DefaultText = "The Report is in Progress, Can't Cancel until it Finishes",LocalDefaultText = @"הדוח בתהליך , לא ניתן לבטל כרגע", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOCustomeralreadyhasaninprogress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.Customeralreadyhasaninprogress", DefaultText = "Customer already has an in progress interest report number",LocalDefaultText = @"ללקוח כבר קיים דוח בתהליך , נא להמתין לסיום ולנסות שנית", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestInvoiceOBatchInterest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestInvoice.O.BatchInterest", DefaultText = "Batch Interest",LocalDefaultText = @"הפקה מרוכזת", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOBatchInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.BatchInvoice", DefaultText = "Batch Invoice",LocalDefaultText = @"הפקת חשבונית ריבית מרוכזת", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOCreateInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.CreateInvoice", DefaultText = "Create Invoice",LocalDefaultText = @"צור חשבונית", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOSelectAtLeastOnLine = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.SelectAtLeastOnLine", DefaultText = "Please select at least one line",LocalDefaultText = @"אנא בחר שורה אחת לפחות", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOShowInvoicingInProgressReports = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.ShowInvoicingInProgressReports", DefaultText = "Show Invoicing In Progress Reports",LocalDefaultText = @"הצג דוחות בתהליך הפקת חשבונית", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOReportIsBeingInvoiced = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.ReportIsBeingInvoiced", DefaultText = "The Report is being Invoiced, Can't Cancel until it Finishes",LocalDefaultText = @" הדוח בתהליך הפקת חשבונית , ניתן יהיה לבטל בסיום", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOReportinProgress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.ReportinProgress", DefaultText = "Report In Progress",LocalDefaultText = @"הדוח נמצא בתהליך בניה", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOReportCreationFailed = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.ReportCreationFailed", DefaultText = "Report Creation Failed",LocalDefaultText = @"בנית הדוח נכשלה", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOEditOpenBalance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.EditOpenBalance", DefaultText = "Edit Open Balance",LocalDefaultText = @"עדכן יתרת פתיחה", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOSelectedReportsWillNotHaveAnInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.SelectedReportsWillNotHaveAnInvoice", DefaultText = "selected reports will not have an invoice created for them because they do not meet the minimum billing requirements. They will be closed without invoices",LocalDefaultText = @"הדוחות שנבחרו לא תיווצר עבורם חשבונית מכיוון שהם לא עומדים בהגדרת החיוב המינמלית שבכרטיס . ", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOOutOf = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.OutOf", DefaultText = "out of",LocalDefaultText = @"מתוך", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOAnotherBatchInvoiceStillInProgress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.AnotherBatchInvoiceStillInProgress", DefaultText = "Please wait until all invoices that are being created have completed before creating more batch invoices",LocalDefaultText = @"קיימות חשבוניות בתהליך הפקה , לא ניתן להפיק נוספות עד שיסתיימו", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOBatchReport = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.BatchReport", DefaultText = "Batch Report",LocalDefaultText = @"הפקת דוחות ריבית מרוכזת ", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOAnotherBatchReportStillInProgress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.AnotherBatchReportStillInProgress", DefaultText = "Please wait until all reports that are being created have completed before creating more batch reports.",LocalDefaultText = @"קיימים דוחות בתהליך הפקה , לא ניתן להפיק נוספים עד שיסתיימו", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportONoGlAccountPeriod = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.NoGlAccountPeriod", DefaultText = "There is no GL Account Interest period in the dates provided",LocalDefaultText = @"לא קיימת הגדרת ריבית בכרטיס בתאריכים אלו", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportONoStandardBasePeriod = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.NoStandardBasePeriod", DefaultText = "there is no Interest Base period in the dates provided for the Standard Rate Base",LocalDefaultText = @"לא קיימת הגדרת ריבית רגילה  בכרטיס בתאריכים אלו", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportONoExceptionalBasePeriod = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.NoExceptionalBasePeriod", DefaultText = "there is no Interest Base period in the dates provided for the Exceptional Rate Base",LocalDefaultText = @"לא קיימת הגדרת ריבית חריגה בכרטיס בתאריכים אלו", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportONoCreditBasePeriod = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.NoCreditBasePeriod", DefaultText = "there is no Interest Base period in the dates provided for the Credit Rate Base",LocalDefaultText = @"לא קיימת הגדרת ריבית זכות בכרטיס בתאריכים אלו", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOBatchPrint = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.BatchPrint", DefaultText = "Batch Print",LocalDefaultText = @"הדפסה ברצף", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOShowReportsWithPrintedInvoices = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.ShowReportsWithPrintedInvoices", DefaultText = "Show Reports with Printed Invoices",LocalDefaultText = @"הצג דוחות בתהליך הפקת חשבונית", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOInvoicesnovalidcopiestoprint = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.Invoicesnovalidcopiestoprint", DefaultText = " were not printed because there are no valid copies to print.",LocalDefaultText = @"לא הודפסו מכיוון שלא נמצאו עותקים להדפסה", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOReportsnovalidcopiestoprint = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.Reportsnovalidcopiestoprint", DefaultText = " were not printed because there are no valid copies to print.",LocalDefaultText = @"לא הודפסו מכיוון שלא נמצאו עותקים להדפסה", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOInvoices = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.Invoices", DefaultText = "Invoices ",LocalDefaultText = @"חשבוניות", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOReports = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.Reports", DefaultText = "Reports ",LocalDefaultText = @"דוחות", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOEditCalculationDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.EditCalculationDate", DefaultText = "Edit Calculation Date",LocalDefaultText = @"עדכן תאריך חישוב", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportODownloadorView = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.DownloadorView", DefaultText = "Would you like to Download or View all selected interest invoices as one document?",LocalDefaultText = @"", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportODownload = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.Download", DefaultText = "Download",LocalDefaultText = @"הורדה", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOView = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.View", DefaultText = "View",LocalDefaultText = @"לצפות", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOCreatingInvoicepermitted = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.CreatingInvoicepermitted", DefaultText = "Creating an Invoice is not permitted unless the report is 'Draft' or 'Invoicing Failed'",LocalDefaultText = @"ניתן להפיק חשבונית רק לדוח שבסטטוס טיוטה או נכשל", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOEntertheInvoiceDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.EntertheInvoiceDate", DefaultText = "Enter the Invoice Date to be used for the invoices for all selected reports",LocalDefaultText = null, ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOCannotCreateReportWithCalculationDateLess = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.CannotCreateReportWithCalculationDateLess", DefaultText = "Can't create report. [Report ",LocalDefaultText = @"לא ניתן להכין את הדוח , ישנו דוח בתאריכים", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOon = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.on", DefaultText = "on",LocalDefaultText = @"עד", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOAlreadyExists = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.AlreadyExists", DefaultText = "] already exists",LocalDefaultText = null, ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOConfirmClosingWithoutInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.ConfirmClosingWithoutInvoice", DefaultText = "Confirm closing the report without invoice?",LocalDefaultText = @"נא לאשר סגירת דוחות ללא הפקת חשבוניות", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOAttachReportWithEachInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.AttachReportWithEachInvoice", DefaultText = "Attach Report with Each Invoice",LocalDefaultText = @"צרף פירוט ריבית להדפסה", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOLineWasCreatedByTheSystem = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.LineWasCreatedByTheSystem", DefaultText = "This line was created by the system in order to calculate the interest on the Open Balance",LocalDefaultText = @"ריבית מחושבת על יתרת חובה", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOUpdatingInvoicepermitted = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.UpdatingInvoicepermitted", DefaultText = "Update a report is not permitted unless the report is 'Draft' or 'Invoicing Failed'",LocalDefaultText = "ניתן לעדכן דוח רק כאשר הסטאטוס של הדוח ''הוא ''טיוטה'' או ''נכשל", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOConfirmRecalculateReport = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.ConfirmRecalculateReport", DefaultText = "Confirm recalculating the report",LocalDefaultText = @"יש לאשר חישוב מחדש של הדוח", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOEnterCreditInvoiceDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.EnterCreditInvoiceDate", DefaultText = "Enter Credit Invoice Date",LocalDefaultText = @"הקלד תאריך לחשבונית זיכוי", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportORequiedCreditInvoiceDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.RequiedCreditInvoiceDate", DefaultText = "The credit invoice date is required",LocalDefaultText = @"חובה להקליד תאריך לחשבונית זיכוי", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOCreditInvoiceDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.CreditInvoiceDate", DefaultText = "Credit Invoice Date",LocalDefaultText = @" תאריך לחשבונית זיכוי", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode InterestReportTextCode_InterestReportOInterestReports = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "InterestReport.O.InterestReports", DefaultText = "Interest Report",LocalDefaultText = @"דוח ריבית", ObjectTableId = InterestReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 