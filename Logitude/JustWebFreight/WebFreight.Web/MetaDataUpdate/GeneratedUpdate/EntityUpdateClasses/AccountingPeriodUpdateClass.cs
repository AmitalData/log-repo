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
using Logitude.Workflow.Data.Repositories;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.CLoseTable;
using Logitude.DashboardModule.Data.Repositories;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.BL;

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses
{
   public class AccountingPeriodUpdateClass
   {  		
		public const string HashString = "871517f59037c3ed1bb101b8e1ebbad9";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                      


            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "AccountingPeriod",
			      				    IsNew =  false,
			      				    DBTableName =  "AccountingPeriods",
			      				    ObjectTableSingular =  "Accounting Period",
			      				    ObjectTablePlural =  "Accounting Periods",
			      				    DescriptionDefaultText =  "Define your accounting periods",
			      				    DescriptionLocalDefaultText =  "דרך תפריט זה ניתן לפתוח ולסגור חודשים ",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  false,
			      				    IsNewWizard =  false,
			      				    LookUp1 =  "Year",
			      				    LookUp2 =  "PeriodTypeName",
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
			      				    SortingByObjectField =  "Year",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "BR",
			      				    MaxNumberOfCustomFields =  0,
			      				    LocalDefaultText =  "הגדרות תקופות חשבונאיות",
			      				    DefaultText =  "Accounting Period",
			      				    Code =  "ca8b",
			      				    Name =  "AccountingPeriod Query Group",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Accounting",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    HashString =  AccountingPeriodUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Id",
					  						ObjectTableName =  "AccountingPeriod",
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
					  						SystemMaxLength =  15,
					  						DisplayInList =  true,
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
					  						ValidForQuerySection1 =  "AccountingPeriod",
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
					  						FullLocalDefaultText =  "Id",
					  						ListFieldLable =  "IdListLable",
					  						ListLableDefaultText =  "Id",
					  						ListLocalDefaultText =  "Id",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  true,
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
					 
					 						FieldName =  "Tenant",
					  						ObjectTableName =  "AccountingPeriod",
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
					  						DisplayInList =  true,
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
					  						ValidForQuerySection1 =  "AccountingPeriod",
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
					  						FullLocalDefaultText =  "tenant",
					  						ListFieldLable =  "TenantListLable",
					  						ListLableDefaultText =  "Tenant",
					  						ListLocalDefaultText =  "tenant",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  true,
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
					 
					 						FieldName =  "Year",
					  						ObjectTableName =  "AccountingPeriod",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  0,
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "Year",
					  						ListPropertyPath =  "Year",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AccountingPeriod",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Year",
					  						DefaultText =  "Year",
					  						FullLocalDefaultText =  "שנה",
					  						ListFieldLable =  "YearListLable",
					  						ListLableDefaultText =  "Year",
					  						ListLocalDefaultText =  "שנה",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
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
					 
					 						FieldName =  "PeriodTypeCode",
					  						ObjectTableName =  "AccountingPeriod",
					  						FieldsDataType =  "LookUp",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PeriodTypeCode",
					  						ListPropertyPath =  "PeriodTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AccountingPeriod",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PeriodTypeCode",
					  						DefaultText =  "Type",
					  						FullLocalDefaultText =  "סוג תקופה",
					  						ListFieldLable =  "PeriodTypeCodeListLable",
					  						ListLableDefaultText =  "Type",
					  						ListLocalDefaultText =  "סוג תקופה",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "PeriodType",
					  						NavigationPropertyName =  "PeriodType",
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
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
					 
					 						FieldName =  "PeriodTypeName",
					  						ObjectTableName =  "AccountingPeriod",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  50,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  50,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "PeriodTypeName",
					  						ListPropertyPath =  "PeriodTypeName",
					  						DisplayInLookUpIndex =  1,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  1,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AccountingPeriod",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PeriodTypeName",
					  						DefaultText =  "Period Type",
					  						FullLocalDefaultText =  "סוג תקופה",
					  						ListFieldLable =  "PeriodTypeNameListLable",
					  						ListLableDefaultText =  "Period Type",
					  						ListLocalDefaultText =  "סוג תקופה",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
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
					 
					 						FieldName =  "OpenMonth",
					  						ObjectTableName =  "AccountingPeriod",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
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
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "OpenMonth",
					  						ListPropertyPath =  "OpenMonth",
					  						DisplayInLookUpIndex =  2,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  2,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AccountingPeriod",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OpenMonth",
					  						DefaultText =  "Open Month",
					  						FullLocalDefaultText =  "חודש פתוח",
					  						ListFieldLable =  "OpenMonthListLable",
					  						ListLableDefaultText =  "OpenMonth",
					  						ListLocalDefaultText =  "חודש פתוח",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
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
					 
					 						FieldName =  "ClosedMonth",
					  						ObjectTableName =  "AccountingPeriod",
					  						FieldsDataType =  "Integer",
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
					  						PMPropertyPath =  "ClosedMonth",
					  						ListPropertyPath =  "ClosedMonth",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AccountingPeriod",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ClosedMonth",
					  						DefaultText =  "Closed Month",
					  						FullLocalDefaultText =  "חודש סגור",
					  						ListFieldLable =  "ClosedMonthListLable",
					  						ListLableDefaultText =  "Closed Month",
					  						ListLocalDefaultText =  "חודש סגור",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
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
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable AccountingPeriodObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AccountingPeriod" && d.Tenant == 0).FirstOrDefault();
		   //List<ObjectField> AccountingPeriodObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "AccountingPeriod").ToList();
		       
	      

	         Screen AccountingPeriodAccountingPeriodHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "AccountingPeriod.HeaderScreen", Name = "AccountingPeriodHeaderScreen", ObjectTableId = AccountingPeriodObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      	
		    AccountingPeriodObjectTable.HeaderScreenId = AccountingPeriodAccountingPeriodHeaderScreenScreen0.Id;
		    AccountingPeriodObjectTable.HeaderScreenCode = AccountingPeriodAccountingPeriodHeaderScreenScreen0.Code;

	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable AccountingPeriodObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AccountingPeriod" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode AccountingPeriodGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriod.TH.General", DefaultText = "General",LocalDefaultText = "כללי", ObjectTableId = AccountingPeriodObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature AccountingPeriodGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = AccountingPeriodObjectTable.Id, Tenant = 0, NameTextCodeCode = "AccountingPeriod.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,AccountingPeriodObjectTable);
 
                 
			   TextCode AccountingPeriodEventsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriod.TH.Events", DefaultText = "Events",LocalDefaultText = "אירועים", ObjectTableId = AccountingPeriodObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature AccountingPeriodEventsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = AccountingPeriodObjectTable.Id, Tenant = 0, NameTextCodeCode = "AccountingPeriod.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,AccountingPeriodObjectTable);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "APGC",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = AccountingPeriodGeneralFeature_TH0.Id,FeatureUniqeCode = AccountingPeriodGeneralFeature_TH0.FeatureUniqeCode, ControlPath = "Logitude.Accounting.Views.Tabs.ACC.AccountingPeriodGeneralTabControl", ObjectTableId = AccountingPeriodObjectTable.Id, TabNameTextCodeId = AccountingPeriodGeneralTextCode_TH0.Id, TabNameTextCodeCode = AccountingPeriodGeneralTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "APEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = AccountingPeriodEventsFeature_TH1.Id,FeatureUniqeCode = AccountingPeriodEventsFeature_TH1.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = AccountingPeriodObjectTable.Id, TabNameTextCodeId = AccountingPeriodEventsTextCode_TH1.Id, TabNameTextCodeCode = AccountingPeriodEventsTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable AccountingPeriodObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AccountingPeriod" && d.Tenant == 0).FirstOrDefault(); 

		   Feature AccountingPeriodFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = AccountingPeriodObjectTable.Id, Tenant = 0, NameTextCodeCode = "AccountingPeriod.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,AccountingPeriodObjectTable);
		   Feature AccountingPeriodFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = AccountingPeriodObjectTable.Id, Tenant = 0, NameTextCodeCode = "AccountingPeriod.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,AccountingPeriodObjectTable);
		   Feature AccountingPeriodFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = AccountingPeriodObjectTable.Id, Tenant = 0, NameTextCodeCode = "AccountingPeriod.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,AccountingPeriodObjectTable);
		   Feature AccountingPeriodFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = false, ObjectTableId = AccountingPeriodObjectTable.Id, Tenant = 0, NameTextCodeCode = "AccountingPeriod.Features.PackageFeature", NameTextCodeDefaultText = "AccountingPeriod Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,AccountingPeriodObjectTable); 

		   		   //--------------> Additional Features <--------------\\

		   Feature AccountingPeriodFeature_ACCOUNTINGPERIODS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCOUNTINGPERIODS", FeatureTypeCode = "QUER", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = AccountingPeriodObjectTable.Id, Tenant = 0, NameTextCodeCode = "AccountingPeriod.Features.AccountingPeriods", NameTextCodeDefaultText = @"Accounting Periods" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,AccountingPeriodObjectTable);

		   Feature AccountingPeriodFeature_ACCOUNTINGPERIODSMENU = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCOUNTINGPERIODSMENU", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = AccountingPeriodObjectTable.Id, Tenant = 0, NameTextCodeCode = "AccountingPeriod.Features.AccountingPeriodsMenu", NameTextCodeDefaultText = @"Accounting Periods" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,AccountingPeriodObjectTable);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable AccountingPeriodObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AccountingPeriod" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "OPEN",
                EnglishName =  "Open New Month",
                LocalName =  "Open New Month",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = AccountingPeriodObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CLOS",
                EnglishName =  "Close Month",
                LocalName =  "Close Month",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = AccountingPeriodObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "OPCN",
                EnglishName =  "Cancel Open Month",
                LocalName =  "Cancel Open Month",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = AccountingPeriodObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CREV",
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
                ObjectTableId = AccountingPeriodObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPEV",
                EnglishName =  "Updated",
                LocalName =  "Updated",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = AccountingPeriodObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "PUPD",
                EnglishName =  "Changed",
                LocalName =  "Changed",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = AccountingPeriodObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "PCR",
                EnglishName =  "Created",
                LocalName =  "Created",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = AccountingPeriodObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "PDL",
                EnglishName =  "Deleted",
                LocalName =  "Deleted",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = AccountingPeriodObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable AccountingPeriodObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AccountingPeriod" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode AccountingPeriodTextCode_AccountingPeriodOCantCancelOpenMonth = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriod.O.CantCancelOpenMonth", DefaultText = "Can’t cancel opened month, There are transactions that already registered for this month",LocalDefaultText = @"לא ניתן לבטל את פתיחת החודש משום שנרשמו תנועות עליו", ObjectTableId = AccountingPeriodObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode AccountingPeriodTextCode_AccountingPeriodOCantOpenInvoiceMonth = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriod.O.CantOpenInvoiceMonth", DefaultText = "Invoice month cannot be opened, The accounting month must first be opened",LocalDefaultText = @"לא ניתן לפתוח חודש חשבונית , יש לפתוח ראשית את החודש החשבונאי", ObjectTableId = AccountingPeriodObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode AccountingPeriodTextCode_AccountingPeriodOCantCancelInvoiceClosedMonth = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriod.O.CantCancelInvoiceClosedMonth", DefaultText = "Cannot open an invoice's closed month which is less than accounting period's closed month.",LocalDefaultText = @"לא ניתן לפתוח חודש חשבונית לתקופה קודמת לחודש חשבונאי", ObjectTableId = AccountingPeriodObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode AccountingPeriodTextCode_AccountingPeriodOCantOpenInterestInvoiceMonth = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriod.O.CantOpenInterestInvoiceMonth", DefaultText = "Interest Invoice month cannot be opened, The accounting month must first be opened",LocalDefaultText = @"לא ניתן לפתוח תקופה חשבונאית לחשבוניות ריבית , יש לפתוח תחילה את התקופה החשבונאית לרישום חשבונאי", ObjectTableId = AccountingPeriodObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode AccountingPeriodTextCode_AccountingPeriodOCantCancelInterestInvoiceClosedMonth = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriod.O.CantCancelInterestInvoiceClosedMonth", DefaultText = "Cannot open an invoice's closed month which is less than accounting period's closed month.",LocalDefaultText = @"לא ניתן לפתוח חודש חשבונית לתקופה קודמת לחודש חשבונאי", ObjectTableId = AccountingPeriodObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode AccountingPeriodTextCode_GeneralMCACCAccountingPeriods = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.AccountingPeriods", DefaultText = "Accounting Periods",LocalDefaultText = @"תקופות חשבונאיות", ObjectTableId = AccountingPeriodObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode AccountingPeriodTextCode_AccountingPeriodsQAccountingPeriods = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriods.Q.AccountingPeriods", DefaultText = "Accounting Periods",LocalDefaultText = @"תקופות חשבונאיות", ObjectTableId = AccountingPeriodObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode AccountingPeriodTextCode_AccountingPeriodsQAccountingPeriodMng = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriods.Q.AccountingPeriodMng", DefaultText = "Accounting Period",LocalDefaultText = @"ניהול תקופה חשבונאית", ObjectTableId = AccountingPeriodObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode AccountingPeriodTextCode_AccountingPeriodsFYear = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriods.F.Year", DefaultText = "Year",LocalDefaultText = @"שנה", ObjectTableId = AccountingPeriodObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode AccountingPeriodTextCode_AccountingPeriodsFYearLabel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriods.F.YearLabel", DefaultText = "Year: ",LocalDefaultText = @"שנה: ", ObjectTableId = AccountingPeriodObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode AccountingPeriodTextCode_AccountingPeriodTHNew = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingPeriod.TH.New", DefaultText = "New",LocalDefaultText = @"חדש", ObjectTableId = AccountingPeriodObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 