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
   public class QuoteSettingUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "QuoteSetting",
			      				    DBTableName =  "QuoteSettings",
			      				    ObjectTableSingular =  "Quote Setting",
			      				    DefaultText =  "Quote Setting",
			      				    Name =  "QuoteSetting",
			      				    IsNewWizard =  false,
			      				    HasCustomFilter =  false,
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableEditFromLOV =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  false,
			      				    IsAutoComplete =  false,
			      				    CustomFieldsCount =  0,
			      				    HasCustomFields =  false,
			      				    InActive =  false,
			      				    SearchFields =  "QuoteSetting,QuoteSettings,,Id,",
			      				    IsSaveButtonVisible =  true,
			      				    EnableSecurity =  false,
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
			      				    ClientModuleName =  "Quote",
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasMenuButtons =  false,
			      				    HasFiltersMenu =  false,
			      				    AllowedInQueues =  false,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}

        public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes, ObjectFieldRepository ObjectFieldsRepository, TextCodeRepository TextCodeRepository)
        {

            AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
            {

                FieldName = "CopyShipper",
                ObjectTableName = "QuoteSetting",
                FieldsDataType = "Boolean",
                Code = "CopyShipper",
                MaxLength = 1,
                IsCustom = false,
                MinLength = 0,
                DisplayOnLookUp = false,
                CanFilter = false,
                DisplayOnly = false,
                SystemRequired = false,
                SystemMaxLength = 0,
                DisplayInList = false,
                IsCustomFilter = false,
                Operator = "Equals",
                MultiLine = false,
                IsTimeFrameFilter = false,
                DisplayInSearchWindowList = false,
                DisplayInSearchWindowFilters = false,
                PMPropertyPath = "CopyShipper",
                DisplayInLookUpIndex = 0,
                AutomaticField = false,
                UniqueField = false,
                DisplayInSearchWindowListIndex = 0,
                DisplayInSearchWindowFiltersIndex = 0,
                IsMulti = false,
                DependencyFilter1IsList = false,
                DependencyFilter2IsList = false,
                DependencyFilter3IsList = false,
                IsRestrictable = false,
                DisplayInEntityVariables = true,
                DigitsAfterPoint = 0,
                InActive = false,
                DisplayLongName = false,
                NumberOfDigits = 0,
                IsMaxLength = false,
                AllowedinAutomationConditions = false,
                AutomationEmailRecipient = false,
                CanAutomateSetValue = false,
                AllowedInCustomerFieldsSettings = false,
                DisplayInDocumentReferences = false,
                CopyToDW = false,
                HasTemplate = false,
                IsRequired = false,
                FullFieldLable = "CopyShipper",
                DefaultText = @"Shipper",
                HelpTextCode = "CopyShipper",

            }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


            AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
            {

                FieldName = "CopyConsignee",
                ObjectTableName = "QuoteSetting",
                FieldsDataType = "Boolean",
                Code = "CopyConsignee",
                MaxLength = 1,
                IsCustom = false,
                MinLength = 0,
                DisplayOnLookUp = false,
                CanFilter = false,
                DisplayOnly = false,
                SystemRequired = false,
                SystemMaxLength = 0,
                DisplayInList = false,
                IsCustomFilter = false,
                Operator = "Equals",
                MultiLine = false,
                IsTimeFrameFilter = false,
                DisplayInSearchWindowList = false,
                DisplayInSearchWindowFilters = false,
                PMPropertyPath = "CopyConsignee",
                DisplayInLookUpIndex = 0,
                AutomaticField = false,
                UniqueField = false,
                DisplayInSearchWindowListIndex = 0,
                DisplayInSearchWindowFiltersIndex = 0,
                IsMulti = false,
                DependencyFilter1IsList = false,
                DependencyFilter2IsList = false,
                DependencyFilter3IsList = false,
                IsRestrictable = false,
                DisplayInEntityVariables = true,
                DigitsAfterPoint = 0,
                InActive = false,
                DisplayLongName = false,
                NumberOfDigits = 0,
                IsMaxLength = false,
                AllowedinAutomationConditions = false,
                AutomationEmailRecipient = false,
                CanAutomateSetValue = false,
                AllowedInCustomerFieldsSettings = false,
                DisplayInDocumentReferences = false,
                CopyToDW = false,
                HasTemplate = false,
                IsRequired = false,
                FullFieldLable = "CopyConsignee",
                DefaultText = @"Consignee",
                HelpTextCode = "CopyConsignee",

            }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


            AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
            {

                FieldName = "CopyMainCarriage",
                ObjectTableName = "QuoteSetting",
                FieldsDataType = "Boolean",
                Code = "CopyMainCarriage",
                MaxLength = 1,
                IsCustom = false,
                MinLength = 0,
                DisplayOnLookUp = false,
                CanFilter = false,
                DisplayOnly = false,
                SystemRequired = false,
                SystemMaxLength = 0,
                DisplayInList = false,
                IsCustomFilter = false,
                Operator = "Equals",
                MultiLine = false,
                IsTimeFrameFilter = false,
                DisplayInSearchWindowList = false,
                DisplayInSearchWindowFilters = false,
                PMPropertyPath = "CopyMainCarriage",
                DisplayInLookUpIndex = 0,
                AutomaticField = false,
                UniqueField = false,
                DisplayInSearchWindowListIndex = 0,
                DisplayInSearchWindowFiltersIndex = 0,
                IsMulti = false,
                DependencyFilter1IsList = false,
                DependencyFilter2IsList = false,
                DependencyFilter3IsList = false,
                IsRestrictable = false,
                DisplayInEntityVariables = true,
                DigitsAfterPoint = 0,
                InActive = false,
                DisplayLongName = false,
                NumberOfDigits = 0,
                IsMaxLength = false,
                AllowedinAutomationConditions = false,
                AutomationEmailRecipient = false,
                CanAutomateSetValue = false,
                AllowedInCustomerFieldsSettings = false,
                DisplayInDocumentReferences = false,
                CopyToDW = false,
                HasTemplate = false,
                IsRequired = false,
                FullFieldLable = "CopyMainCarriage",
                DefaultText = @"Main Carriage",
                HelpTextCode = "CopyMainCarriage",

            }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


            AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
            {

                FieldName = "CopyPickup",
                ObjectTableName = "QuoteSetting",
                FieldsDataType = "Boolean",
                Code = "CopyPickup",
                MaxLength = 1,
                IsCustom = false,
                MinLength = 0,
                DisplayOnLookUp = false,
                CanFilter = false,
                DisplayOnly = false,
                SystemRequired = false,
                SystemMaxLength = 0,
                DisplayInList = false,
                IsCustomFilter = false,
                Operator = "Equals",
                MultiLine = false,
                IsTimeFrameFilter = false,
                DisplayInSearchWindowList = false,
                DisplayInSearchWindowFilters = false,
                PMPropertyPath = "CopyPickup",
                DisplayInLookUpIndex = 0,
                AutomaticField = false,
                UniqueField = false,
                DisplayInSearchWindowListIndex = 0,
                DisplayInSearchWindowFiltersIndex = 0,
                IsMulti = false,
                DependencyFilter1IsList = false,
                DependencyFilter2IsList = false,
                DependencyFilter3IsList = false,
                IsRestrictable = false,
                DisplayInEntityVariables = true,
                DigitsAfterPoint = 0,
                InActive = false,
                DisplayLongName = false,
                NumberOfDigits = 0,
                IsMaxLength = false,
                AllowedinAutomationConditions = false,
                AutomationEmailRecipient = false,
                CanAutomateSetValue = false,
                AllowedInCustomerFieldsSettings = false,
                DisplayInDocumentReferences = false,
                CopyToDW = false,
                HasTemplate = false,
                IsRequired = false,
                FullFieldLable = "CopyPickup",
                DefaultText = @"Include Pickup",
                HelpTextCode = "CopyPickup",

            }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


            AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
            {

                FieldName = "CopyDelivery",
                ObjectTableName = "QuoteSetting",
                FieldsDataType = "Boolean",
                Code = "CopyDelivery",
                MaxLength = 1,
                IsCustom = false,
                MinLength = 0,
                DisplayOnLookUp = false,
                CanFilter = false,
                DisplayOnly = false,
                SystemRequired = false,
                SystemMaxLength = 0,
                DisplayInList = false,
                IsCustomFilter = false,
                Operator = "Equals",
                MultiLine = false,
                IsTimeFrameFilter = false,
                DisplayInSearchWindowList = false,
                DisplayInSearchWindowFilters = false,
                PMPropertyPath = "CopyDelivery",
                DisplayInLookUpIndex = 0,
                AutomaticField = false,
                UniqueField = false,
                DisplayInSearchWindowListIndex = 0,
                DisplayInSearchWindowFiltersIndex = 0,
                IsMulti = false,
                DependencyFilter1IsList = false,
                DependencyFilter2IsList = false,
                DependencyFilter3IsList = false,
                IsRestrictable = false,
                DisplayInEntityVariables = true,
                DigitsAfterPoint = 0,
                InActive = false,
                DisplayLongName = false,
                NumberOfDigits = 0,
                IsMaxLength = false,
                AllowedinAutomationConditions = false,
                AutomationEmailRecipient = false,
                CanAutomateSetValue = false,
                AllowedInCustomerFieldsSettings = false,
                DisplayInDocumentReferences = false,
                CopyToDW = false,
                HasTemplate = false,
                IsRequired = false,
                FullFieldLable = "CopyDelivery",
                DefaultText = @"Include Delivery",
                HelpTextCode = "CopyDelivery",

            }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


            AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
            {

                FieldName = "CopyChargesTypes",
                ObjectTableName = "QuoteSetting",
                FieldsDataType = "Boolean",
                Code = "CopyChargesTypes",
                MaxLength = 1,
                IsCustom = false,
                MinLength = 0,
                DisplayOnLookUp = false,
                CanFilter = false,
                DisplayOnly = false,
                SystemRequired = false,
                SystemMaxLength = 0,
                DisplayInList = false,
                IsCustomFilter = false,
                Operator = "Equals",
                MultiLine = false,
                IsTimeFrameFilter = false,
                DisplayInSearchWindowList = false,
                DisplayInSearchWindowFilters = false,
                PMPropertyPath = "CopyChargesTypes",
                DisplayInLookUpIndex = 0,
                AutomaticField = false,
                UniqueField = false,
                DisplayInSearchWindowListIndex = 0,
                DisplayInSearchWindowFiltersIndex = 0,
                IsMulti = false,
                DependencyFilter1IsList = false,
                DependencyFilter2IsList = false,
                DependencyFilter3IsList = false,
                IsRestrictable = false,
                DisplayInEntityVariables = true,
                DigitsAfterPoint = 0,
                InActive = false,
                DisplayLongName = false,
                NumberOfDigits = 0,
                IsMaxLength = false,
                AllowedinAutomationConditions = false,
                AutomationEmailRecipient = false,
                CanAutomateSetValue = false,
                AllowedInCustomerFieldsSettings = false,
                DisplayInDocumentReferences = false,
                CopyToDW = false,
                HasTemplate = false,
                IsRequired = false,
                FullFieldLable = "CopyChargesTypes",
                DefaultText = @"Charges Types",
                HelpTextCode = "CopyChargesTypes",

            }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


            AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
            {

                FieldName = "CopyChargesCost",
                ObjectTableName = "QuoteSetting",
                FieldsDataType = "Boolean",
                Code = "CopyChargesCost",
                MaxLength = 1,
                IsCustom = false,
                MinLength = 0,
                DisplayOnLookUp = false,
                CanFilter = false,
                DisplayOnly = false,
                SystemRequired = false,
                SystemMaxLength = 0,
                DisplayInList = false,
                IsCustomFilter = false,
                Operator = "Equals",
                MultiLine = false,
                IsTimeFrameFilter = false,
                DisplayInSearchWindowList = false,
                DisplayInSearchWindowFilters = false,
                PMPropertyPath = "CopyChargesCost",
                DisplayInLookUpIndex = 0,
                AutomaticField = false,
                UniqueField = false,
                DisplayInSearchWindowListIndex = 0,
                DisplayInSearchWindowFiltersIndex = 0,
                IsMulti = false,
                DependencyFilter1IsList = false,
                DependencyFilter2IsList = false,
                DependencyFilter3IsList = false,
                IsRestrictable = false,
                DisplayInEntityVariables = true,
                DigitsAfterPoint = 0,
                InActive = false,
                DisplayLongName = false,
                NumberOfDigits = 0,
                IsMaxLength = false,
                AllowedinAutomationConditions = false,
                AutomationEmailRecipient = false,
                CanAutomateSetValue = false,
                AllowedInCustomerFieldsSettings = false,
                DisplayInDocumentReferences = false,
                CopyToDW = false,
                HasTemplate = false,
                IsRequired = false,
                FullFieldLable = "CopyChargesCost",
                DefaultText = @"Cost",
                HelpTextCode = "CopyChargesCost",

            }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


            AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
            {

                FieldName = "CopyChargesSale",
                ObjectTableName = "QuoteSetting",
                FieldsDataType = "Boolean",
                Code = "CopyChargesSale",
                MaxLength = 1,
                IsCustom = false,
                MinLength = 0,
                DisplayOnLookUp = false,
                CanFilter = false,
                DisplayOnly = false,
                SystemRequired = false,
                SystemMaxLength = 0,
                DisplayInList = false,
                IsCustomFilter = false,
                Operator = "Equals",
                MultiLine = false,
                IsTimeFrameFilter = false,
                DisplayInSearchWindowList = false,
                DisplayInSearchWindowFilters = false,
                PMPropertyPath = "CopyChargesSale",
                DisplayInLookUpIndex = 0,
                AutomaticField = false,
                UniqueField = false,
                DisplayInSearchWindowListIndex = 0,
                DisplayInSearchWindowFiltersIndex = 0,
                IsMulti = false,
                DependencyFilter1IsList = false,
                DependencyFilter2IsList = false,
                DependencyFilter3IsList = false,
                IsRestrictable = false,
                DisplayInEntityVariables = true,
                DigitsAfterPoint = 0,
                InActive = false,
                DisplayLongName = false,
                NumberOfDigits = 0,
                IsMaxLength = false,
                AllowedinAutomationConditions = false,
                AutomationEmailRecipient = false,
                CanAutomateSetValue = false,
                AllowedInCustomerFieldsSettings = false,
                DisplayInDocumentReferences = false,
                CopyToDW = false,
                HasTemplate = false,
                IsRequired = false,
                FullFieldLable = "CopyChargesSale",
                DefaultText = @"Sale",
                HelpTextCode = "CopyChargesSale",

            }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


            AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
            {

                FieldName = "EditMainCarriage",
                ObjectTableName = "QuoteSetting",
                FieldsDataType = "Boolean",
                Code = "EditMainCarriage",
                MaxLength = 1,
                IsCustom = false,
                MinLength = 0,
                DisplayOnLookUp = false,
                CanFilter = false,
                DisplayOnly = false,
                SystemRequired = false,
                SystemMaxLength = 0,
                DisplayInList = false,
                IsCustomFilter = false,
                Operator = "Equals",
                MultiLine = false,
                IsTimeFrameFilter = false,
                DisplayInSearchWindowList = false,
                DisplayInSearchWindowFilters = false,
                PMPropertyPath = "EditMainCarriage",
                DisplayInLookUpIndex = 0,
                AutomaticField = false,
                UniqueField = false,
                DisplayInSearchWindowListIndex = 0,
                DisplayInSearchWindowFiltersIndex = 0,
                IsMulti = false,
                DependencyFilter1IsList = false,
                DependencyFilter2IsList = false,
                DependencyFilter3IsList = false,
                IsRestrictable = false,
                DisplayInEntityVariables = true,
                DigitsAfterPoint = 0,
                InActive = false,
                DisplayLongName = false,
                NumberOfDigits = 0,
                IsMaxLength = false,
                AllowedinAutomationConditions = false,
                AutomationEmailRecipient = false,
                CanAutomateSetValue = false,
                AllowedInCustomerFieldsSettings = false,
                DisplayInDocumentReferences = false,
                CopyToDW = false,
                HasTemplate = false,
                IsRequired = false,
                FullFieldLable = "EditMainCarriage",
                DefaultText = @"Edit Main Carriage",
                HelpTextCode = "EditMainCarriage",

            }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


            AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
            {

                FieldName = "CopyAgent",
                ObjectTableName = "QuoteSetting",
                FieldsDataType = "Boolean",
                Code = "CopyAgent",
                MaxLength = 1,
                IsCustom = false,
                MinLength = 0,
                DisplayOnLookUp = false,
                CanFilter = false,
                DisplayOnly = false,
                SystemRequired = false,
                SystemMaxLength = 0,
                DisplayInList = false,
                IsCustomFilter = false,
                Operator = "Equals",
                MultiLine = false,
                IsTimeFrameFilter = false,
                DisplayInSearchWindowList = false,
                DisplayInSearchWindowFilters = false,
                PMPropertyPath = "CopyAgent",
                DisplayInLookUpIndex = 0,
                AutomaticField = false,
                UniqueField = false,
                DisplayInSearchWindowListIndex = 0,
                DisplayInSearchWindowFiltersIndex = 0,
                IsMulti = false,
                DependencyFilter1IsList = false,
                DependencyFilter2IsList = false,
                DependencyFilter3IsList = false,
                IsRestrictable = false,
                DisplayInEntityVariables = true,
                DigitsAfterPoint = 0,
                InActive = false,
                DisplayLongName = false,
                NumberOfDigits = 0,
                IsMaxLength = false,
                AllowedinAutomationConditions = false,
                AutomationEmailRecipient = false,
                CanAutomateSetValue = false,
                AllowedInCustomerFieldsSettings = false,
                DisplayInDocumentReferences = false,
                CopyToDW = false,
                HasTemplate = false,
                IsRequired = false,
                FullFieldLable = "CopyAgent",
                DefaultText = @"Agent",
                HelpTextCode = "CopyAgent",

            }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


            AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
            {

                FieldName = "CopyNotify",
                ObjectTableName = "QuoteSetting",
                FieldsDataType = "Boolean",
                Code = "CopyNotify",
                MaxLength = 1,
                IsCustom = false,
                MinLength = 0,
                DisplayOnLookUp = false,
                CanFilter = false,
                DisplayOnly = false,
                SystemRequired = false,
                SystemMaxLength = 0,
                DisplayInList = false,
                IsCustomFilter = false,
                Operator = "Equals",
                MultiLine = false,
                IsTimeFrameFilter = false,
                DisplayInSearchWindowList = false,
                DisplayInSearchWindowFilters = false,
                PMPropertyPath = "CopyNotify",
                DisplayInLookUpIndex = 0,
                AutomaticField = false,
                UniqueField = false,
                DisplayInSearchWindowListIndex = 0,
                DisplayInSearchWindowFiltersIndex = 0,
                IsMulti = false,
                DependencyFilter1IsList = false,
                DependencyFilter2IsList = false,
                DependencyFilter3IsList = false,
                IsRestrictable = false,
                DisplayInEntityVariables = true,
                DigitsAfterPoint = 0,
                InActive = false,
                DisplayLongName = false,
                NumberOfDigits = 0,
                IsMaxLength = false,
                AllowedinAutomationConditions = false,
                AutomationEmailRecipient = false,
                CanAutomateSetValue = false,
                AllowedInCustomerFieldsSettings = false,
                DisplayInDocumentReferences = false,
                CopyToDW = false,
                HasTemplate = false,
                IsRequired = false,
                FullFieldLable = "CopyNotify",
                DefaultText = @"Notify",
                HelpTextCode = "CopyNotify",

            }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);


            AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
            {

                FieldName = "IsSaleAsCostCurrency",
                ObjectTableName = "QuoteSetting",
                FieldsDataType = "Boolean",
                Code = "IsSaleAsCostCurrency",
                MaxLength = 1,
                IsCustom = false,
                MinLength = 0,
                DisplayOnLookUp = false,
                CanFilter = false,
                DisplayOnly = false,
                SystemRequired = false,
                SystemMaxLength = 0,
                DisplayInList = false,
                IsCustomFilter = false,
                Operator = "Equals",
                MultiLine = false,
                IsTimeFrameFilter = false,
                DisplayInSearchWindowList = false,
                DisplayInSearchWindowFilters = false,
                PMPropertyPath = "IsSaleAsCostCurrency",
                DisplayInLookUpIndex = 0,
                AutomaticField = false,
                UniqueField = false,
                DisplayInSearchWindowListIndex = 0,
                DisplayInSearchWindowFiltersIndex = 0,
                IsMulti = false,
                DependencyFilter1IsList = false,
                DependencyFilter2IsList = false,
                DependencyFilter3IsList = false,
                IsRestrictable = false,
                DisplayInEntityVariables = true,
                DigitsAfterPoint = 0,
                InActive = false,
                DisplayLongName = false,
                NumberOfDigits = 0,
                IsMaxLength = false,
                AllowedinAutomationConditions = false,
                AutomationEmailRecipient = false,
                CanAutomateSetValue = false,
                AllowedInCustomerFieldsSettings = false,
                DisplayInDocumentReferences = false,
                CopyToDW = false,
                HasTemplate = false,
                IsRequired = false,
                FullFieldLable = "IsSaleAsCostCurrency",
                DefaultText = @"Quote Sale Currency Default",
                HelpTextCode = "IsSaleAsCostCurrency",

            }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

            AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
            {
                FieldName = "AutomaticallyCloseDays",
                ObjectTableName = "QuoteSetting",
                FieldsDataType = "Integer",
                Code = "AutomaticallyCloseDays",
                Operator = "Equals",
                PMPropertyPath = "AutomaticallyCloseDays",
                DisplayInEntityVariables = true,
                FullFieldLable = "AutomaticallyCloseDays",
                DefaultText = @"Automatically Close Days",
                HelpTextCode = "AutomaticallyCloseDays",

            }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        }

        public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {    

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {      
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable QuoteSettingObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "QuoteSetting" && d.Tenant == 0).FirstOrDefault(); 
		   Feature QuoteSettingFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = QuoteSettingObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteSetting.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature QuoteSettingFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = QuoteSettingObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteSetting.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature QuoteSettingFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = QuoteSettingObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteSetting.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature QuoteSettingFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = QuoteSettingObjectTable.Id, Tenant = 0, NameTextCodeCode = "QuoteSetting.Features.PackageFeature", NameTextCodeDefaultText = "QuoteSetting Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable QuoteSettingObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "QuoteSetting" && d.Tenant == 0).FirstOrDefault(); 
	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable QuoteSettingObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "QuoteSetting" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode QuoteSettingTextCode_QuoteSetting = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "QuoteSetting", DefaultText = "Quote Setting",LocalDefaultText = null, ObjectTableId = QuoteSettingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "T", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 