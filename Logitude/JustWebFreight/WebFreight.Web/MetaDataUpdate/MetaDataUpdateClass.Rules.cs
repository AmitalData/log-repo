using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.MetaDataUpdate.AddClasses;
using WebFreight.Web.MetaDataUpdate.DetailClasses;

namespace WebFreight.Web.MetaDataUpdate
{
    public partial class MetaDataUpdateClass
    {
        public void LoadObjectTableRulesANDFieldsValidations()
        {
            ObjectContext = WebFreightContext.GetContext(0);
            ObjectTableRuleRepository = new ObjectTableRuleRepository(ObjectContext);
            ObjectTableRuleFieldRepository = new ObjectTableRuleFieldRepository(ObjectContext);
            ObjectFieldValidationRepository = new ObjectFieldValidationRepository(ObjectContext);
            RuleConditionFieldRepository = new RuleConditionFieldRepository(ObjectContext);
            Dictionary<string, ObjectTableRule> TenantObjectTableRule = ObjectTableRuleRepository.GetObjectTableRules(0).ToDictionary(d => d.RuleCode, a => a);

            Dictionary<string, ObjectTableRuleField> TenantObjectTableRuleFields = ObjectTableRuleFieldRepository.GetObjectTableRuleFields(0).ToDictionary(d => d.ObjectTableRuleId + d.ObjectFieldCode, a => a);

            Dictionary<string, RuleConditionField> TenantRuleConditionFields = RuleConditionFieldRepository.GetRuleConditionFieldsByTenant(0).ToDictionary(d => d.ObjectTableRuleId + d.ObjectFieldCode, a => a);

            List<ObjectFieldValidation> TenantObjectFieldValidations = ObjectFieldValidationRepository.GetObjectFieldValidations(0).ToList();

            CreateShipmentRules(TenantObjectTableRule, TenantObjectTableRuleFields, TenantObjectFieldValidations, TenantRuleConditionFields);
            //CreateMasterRules(TenantObjectTableRule, TenantObjectTableRuleFields, TenantObjectFieldValidations, TenantRuleConditionFields);
            CreateQuoteRules(TenantObjectTableRule, TenantObjectTableRuleFields, TenantObjectFieldValidations, TenantRuleConditionFields);
            CreateObjectFieldValidations(TenantObjectFieldValidations);
            CreateMaintenanceTablesRules(TenantObjectTableRule, TenantObjectTableRuleFields, TenantObjectFieldValidations, TenantRuleConditionFields);
            CreateTestRules(TenantObjectTableRule, TenantObjectTableRuleFields, TenantObjectFieldValidations);
            CreateAccountingRules(TenantObjectTableRule, TenantObjectTableRuleFields, TenantObjectFieldValidations, TenantRuleConditionFields);
            CreatelosingReasonRules(TenantObjectTableRule, TenantObjectTableRuleFields, TenantObjectFieldValidations, TenantRuleConditionFields);
        }

        private void CreateTestRules(Dictionary<string, ObjectTableRule> TenantObjectTableRule, Dictionary<string, ObjectTableRuleField> TenantObjectTableRuleFields, List<ObjectFieldValidation> TenantObjectFieldValidations)
        {
            //ObjectTable AirlineTable = ObjectContext.ObjectTables.Where(f => f.Name == "Airline" && f.Tenant == 0).FirstOrDefault();
            //ObjectField airlineCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == AirlineTable.Id).FirstOrDefault();

            //ObjectTableRule AirlineDuplicationRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            //{
            //    RuleCode = "TSTD",
            //    Name = "Code Duplication",
            //    ObjectTableId = AirlineTable.Id,
            //    Tenant = 0,
            //    RuleTypeCode = "DUPL",
            //    //TriggerFieldId = airlineCode.Id,
            //    SystemLevel = true,
            //    OutputMessage = "This Test already exists",
            //    ActiveForNew = true,
            //    ActiveForUpdate = false,
            //    TriggerTypeCode = "ALLW",
            //    RuleNotificationTypeCode = "ERR",
            //}, ObjectTableRuleRepository, TenantObjectTableRule);
            //ObjectTableRuleField airlineCodeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = airlineCode.Id, ObjectTableRuleId = AirlineDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
        }

        private void CreateMaintenanceTablesRules(Dictionary<string, ObjectTableRule> TenantObjectTableRule, Dictionary<string, ObjectTableRuleField> TenantObjectTableRuleFields, List<ObjectFieldValidation> TenantObjectFieldValidations, Dictionary<string, RuleConditionField> TenantRuleConditionFields)
        {
            #region AirlineTableRules
            ObjectTable AirlineTable = ObjectContext.ObjectTables.Where(f => f.Name == "Airline" && f.Tenant == 0).FirstOrDefault();
            ObjectField airlineCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == AirlineTable.Id).FirstOrDefault();
            ObjectField checkDigit = ObjectContext.ObjectFields.Where(d => d.FieldName == "CheckDigit" && d.ObjectTableId == AirlineTable.Id).FirstOrDefault();
            ObjectField limitedLength = ObjectContext.ObjectFields.Where(d => d.FieldName == "LimitedLength" && d.ObjectTableId == AirlineTable.Id).FirstOrDefault();

            ObjectTableRule AirlineDuplicationRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "ALCD",
                Name = "Code Duplication",
                ObjectTableId = AirlineTable.Id,
                Tenant = 0,
                RuleTypeCode = "DUPL",
                SystemLevel = true,
                OutputMessage = "This airline already exists",
                ActiveForNew = true,
                ActiveForUpdate = false,
                TriggerTypeCode = "ALLW",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            ObjectTableRuleField airlineCodeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = airlineCode.Id, ObjectFieldCode = airlineCode.FieldCode, ObjectTableRuleId = AirlineDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);

            ObjectTableRule airlineSetValRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {

                RuleCode = "CheckDigitSetVal",
                Name = "Check Digit Set Value",
                ObjectTableId = AirlineTable.Id,
                Tenant = 0,
                RuleTypeCode = "SETV",
                TriggerFieldId = limitedLength.Id,
                TriggerFieldCode = limitedLength.FieldCode,
                SystemLevel = true,

                ActiveForNew = false,
                ActiveForUpdate = true,
                TriggerTypeCode = "FLDC",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            ObjectTableRuleField limitedField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails()
            {
                ObjectFieldId = checkDigit.Id,
                ObjectTableRuleId = airlineSetValRule.Id,
                SystemLevel = true,
                Tenant = 0,
                Expression = "If([LimitedLength],false,false)",
                ObjectFieldCode = checkDigit.FieldCode,
            }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);

            ObjectTableRule checkDigitblockFieldsRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "CheckDigitBlock",
                Name = "CheckDigit Block",
                ObjectTableId = AirlineTable.Id,
                Tenant = 0,
                RuleTypeCode = "BLCK",
                SystemLevel = true,
                ActiveForNew = false,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);


            ObjectTableRuleField checkdigitField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = checkDigit.Id, ObjectFieldCode = checkDigit.FieldCode, ObjectTableRuleId = checkDigitblockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);

            RuleConditionField limitedLength_CondField = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = limitedLength.Id, ObjectFieldCode = limitedLength.FieldCode, ObjectTableRuleId = checkDigitblockFieldsRule.Id, Operator = "Equals", Value = "False", Tenant = checkDigitblockFieldsRule.Tenant }, RuleConditionFieldRepository, TenantRuleConditionFields);

            #endregion

            #region ShippingTableRules
            ObjectTable ShippingLineTable = ObjectContext.ObjectTables.Where(f => f.Name == "ShippingLine" && f.Tenant == 0).FirstOrDefault();
            ObjectField shippingLineCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == ShippingLineTable.Id).FirstOrDefault();

            ObjectTableRule ShippingLineDuplicationRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "SLCD",
                Name = "Code Duplication",
                ObjectTableId = ShippingLineTable.Id,
                Tenant = 0,
                RuleTypeCode = "DUPL",
                //TriggerFieldId = shippingLineCode.Id,
                SystemLevel = true,
                OutputMessage = "This shipping line already exists",
                ActiveForNew = true,
                ActiveForUpdate = false,
                TriggerTypeCode = "ALLW",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            ObjectTableRuleField shippingLineCodeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = shippingLineCode.Id, ObjectFieldCode = shippingLineCode.FieldCode, ObjectTableRuleId = ShippingLineDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);

            //ObjectTableRuleRepository.Add(ShippingLineDuplicationRule);
            //ObjectTableRuleFieldRepository.Add(shippingLineCodeField);
            #endregion

            #region TruckerRules
            ObjectTable TruckerTable = ObjectContext.ObjectTables.Where(f => f.Name == "Trucker" && f.Tenant == 0).FirstOrDefault();
            ObjectField truckerCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == TruckerTable.Id).FirstOrDefault();

            ObjectTableRule TruckerDuplicationRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "TRCD",
                Name = "Code Duplication",
                ObjectTableId = TruckerTable.Id,
                Tenant = 0,
                RuleTypeCode = "DUPL",
                //TriggerFieldId = truckerCode.Id,
                SystemLevel = true,
                OutputMessage = "This trucker already exists",
                ActiveForNew = true,
                ActiveForUpdate = false,
                TriggerTypeCode = "ALLW",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            ObjectTableRuleField truckerCodeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = truckerCode.Id, ObjectFieldCode = truckerCode.FieldCode, ObjectTableRuleId = TruckerDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleFieldRepository.Add(truckerCodeField);
            //ObjectTableRuleRepository.Add(TruckerDuplicationRule);
            #endregion

            #region PortRules
            ObjectTable PortTable = ObjectContext.ObjectTables.Where(f => f.Name == "Port" && f.Tenant == 0).FirstOrDefault();
            ObjectField portCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == PortTable.Id).FirstOrDefault();
            ObjectField portCountryId = ObjectContext.ObjectFields.Where(d => d.FieldName == "CountryId" && d.ObjectTableId == PortTable.Id).FirstOrDefault();

            ObjectTableRule PortDuplicationRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "PTCD",
                Name = "Code Duplication",
                ObjectTableId = PortTable.Id,
                Tenant = 0,
                RuleTypeCode = "DUPL",
                //TriggerFieldId = portCode.Id,
                SystemLevel = true,
                OutputMessage = "This port already exists",
                ActiveForNew = true,
                ActiveForUpdate = false,
                TriggerTypeCode = "ALLW",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            ObjectTableRuleField CountryIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = portCountryId.Id, ObjectFieldCode = portCountryId.FieldCode, ObjectTableRuleId = PortDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField portCodeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = portCode.Id, ObjectFieldCode = portCode.FieldCode, ObjectTableRuleId = PortDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);

            //ObjectTableRuleFieldRepository.Add(portCodeField);
            //ObjectTableRuleFieldRepository.Add(CountryIdField);
            //ObjectTableRuleRepository.Add(PortDuplicationRule);
            #endregion

            #region IncotermRules
            ObjectTable IncotermTable = ObjectContext.ObjectTables.Where(f => f.Name == "Incoterm" && f.Tenant == 0).FirstOrDefault();
            ObjectField incotermCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == IncotermTable.Id).FirstOrDefault();

            ObjectTableRule IncotermDuplicationRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "INCD",
                Name = "Code Duplication",
                ObjectTableId = IncotermTable.Id,
                Tenant = 0,
                RuleTypeCode = "DUPL",
                //TriggerFieldId = incotermCode.Id,
                SystemLevel = true,
                OutputMessage = "This incoterm already exists",
                ActiveForNew = true,
                ActiveForUpdate = false,
                TriggerTypeCode = "ALLW",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            ObjectTableRuleField incotermCodeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = incotermCode.Id, ObjectFieldCode = incotermCode.FieldCode, ObjectTableRuleId = IncotermDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleFieldRepository.Add(incotermCodeField);
            //ObjectTableRuleRepository.Add(IncotermDuplicationRule);
            #endregion

            #region CurrencyRules
            ObjectTable CurrencyTable = ObjectContext.ObjectTables.Where(f => f.Name == "Currency" && f.Tenant == 0).FirstOrDefault();
            ObjectField currencyCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == CurrencyTable.Id).FirstOrDefault();

            ObjectTableRule CurrencyDuplicationRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "CRCD",
                Name = "Code Duplication",
                ObjectTableId = CurrencyTable.Id,
                Tenant = 0,
                RuleTypeCode = "DUPL",
                //TriggerFieldId = currencyCode.Id,
                SystemLevel = true,
                OutputMessage = "This currency already exists",
                ActiveForNew = true,
                ActiveForUpdate = false,
                TriggerTypeCode = "ALLW",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            ObjectTableRuleField currencyCodeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = currencyCode.Id, ObjectFieldCode = currencyCode.FieldCode, ObjectTableRuleId = CurrencyDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleFieldRepository.Add(currencyCodeField);
            //ObjectTableRuleRepository.Add(CurrencyDuplicationRule);
            #endregion

            #region VatTypeRules
            ObjectTable VatTypeTable = ObjectContext.ObjectTables.Where(f => f.Name == "VatType" && f.Tenant == 0).FirstOrDefault();
            ObjectField vatTypeCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == VatTypeTable.Id).FirstOrDefault();

            ObjectTableRule VatTypeDuplicationRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "VTCD",
                Name = "Code Duplication",
                ObjectTableId = VatTypeTable.Id,
                Tenant = 0,
                RuleTypeCode = "DUPL",
                //TriggerFieldId = vatTypeCode.Id,
                SystemLevel = true,
                OutputMessage = "This vat type already exists",
                ActiveForNew = true,
                ActiveForUpdate = false,
                TriggerTypeCode = "ALLW",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            ObjectTableRuleField vatTypeCodeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = vatTypeCode.Id, ObjectFieldCode = vatTypeCode.FieldCode, ObjectTableRuleId = VatTypeDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleFieldRepository.Add(vatTypeCodeField);
            //ObjectTableRuleRepository.Add(VatTypeDuplicationRule);
            #endregion

            #region ChargeTypeRules
            ObjectTable ChargeTypeTable = ObjectContext.ObjectTables.Where(f => f.Name == "ChargesType" && f.Tenant == 0).FirstOrDefault();
            ObjectField chargeTypeCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == ChargeTypeTable.Id).FirstOrDefault();

            ObjectTableRule ChargeTypeDuplicationRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "CHCD",
                Name = "Code Duplication",
                ObjectTableId = ChargeTypeTable.Id,
                Tenant = 0,
                RuleTypeCode = "DUPL",
                //TriggerFieldId = chargeTypeCode.Id,
                SystemLevel = true,
                OutputMessage = "This charge type already exists",
                ActiveForNew = true,
                ActiveForUpdate = false,
                TriggerTypeCode = "ALLW",
                RuleNotificationTypeCode = "ERR",

            }, ObjectTableRuleRepository, TenantObjectTableRule);

            ObjectTableRuleField chargeTypeCodeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = chargeTypeCode.Id, ObjectFieldCode = chargeTypeCode.FieldCode, ObjectTableRuleId = ChargeTypeDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleFieldRepository.Add(chargeTypeCodeField);
            //ObjectTableRuleRepository.Add(ChargeTypeDuplicationRule);
            #endregion

            #region CountryRules
            ObjectTable CountryTable = ObjectContext.ObjectTables.Where(f => f.Name == "Country" && f.Tenant == 0).FirstOrDefault();
            ObjectField countryCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == CountryTable.Id).FirstOrDefault();

            ObjectTableRule CountryDuplicationRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "CNCD",
                Name = "Code Duplication",
                ObjectTableId = CountryTable.Id,
                Tenant = 0,
                RuleTypeCode = "DUPL",
                //TriggerFieldId = countryCode.Id,
                SystemLevel = true,
                OutputMessage = "This country already exists",
                ActiveForNew = true,
                ActiveForUpdate = false,
                TriggerTypeCode = "ALLW",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            ObjectTableRuleField countryCodeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = countryCode.Id, ObjectFieldCode = countryCode.FieldCode, ObjectTableRuleId = CountryDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleFieldRepository.Add(countryCodeField);
            //ObjectTableRuleRepository.Add(CountryDuplicationRule);
            #endregion

            #region GlobalZoneRules
            ObjectTable GlobalZoneTable = ObjectContext.ObjectTables.Where(f => f.Name == "GlobalZone" && f.Tenant == 0).FirstOrDefault();
            ObjectField globalZoneCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == GlobalZoneTable.Id).FirstOrDefault();

            ObjectTableRule GlobalZoneDuplicationRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "GLCD",
                Name = "Code Duplication",
                ObjectTableId = GlobalZoneTable.Id,
                Tenant = 0,
                RuleTypeCode = "DUPL",
                //TriggerFieldId = globalZoneCode.Id,
                SystemLevel = true,
                OutputMessage = "This global zone already exists",
                ActiveForNew = true,
                ActiveForUpdate = false,
                TriggerTypeCode = "ALLW",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            ObjectTableRuleField globalZoneCodeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = globalZoneCode.Id, ObjectFieldCode = globalZoneCode.FieldCode, ObjectTableRuleId = GlobalZoneDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleFieldRepository.Add(globalZoneCodeField);
            //ObjectTableRuleRepository.Add(GlobalZoneDuplicationRule);
            #endregion

            #region StateRules
            ObjectTable StateTable = ObjectContext.ObjectTables.Where(f => f.Name == "State" && f.Tenant == 0).FirstOrDefault();
            ObjectField stateCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == StateTable.Id).FirstOrDefault();
            ObjectField countryId = ObjectContext.ObjectFields.Where(d => d.FieldName == "CountryId" && d.ObjectTableId == StateTable.Id).FirstOrDefault();

            ObjectTableRule StateDuplicationRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "STCD",
                Name = "Code Duplication",
                ObjectTableId = StateTable.Id,
                Tenant = 0,
                RuleTypeCode = "DUPL",
                //TriggerFieldId = stateCode.Id,
                SystemLevel = true,
                OutputMessage = "This state already exists",
                ActiveForNew = true,
                ActiveForUpdate = false,
                TriggerTypeCode = "ALLW",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            ObjectTableRuleField SatteCountryIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = countryId.Id, ObjectFieldCode = countryId.FieldCode, ObjectTableRuleId = StateDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField stateCodeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = stateCode.Id, ObjectFieldCode = stateCode.FieldCode, ObjectTableRuleId = StateDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleFieldRepository.Add(stateCodeField);
            //ObjectTableRuleRepository.Add(StateDuplicationRule);
            //ObjectTableRuleFieldRepository.Add(SatteCountryIdField);
            #endregion

            #region DocumentTypeRules
            ObjectTable DocumentTypeTable = ObjectContext.ObjectTables.Where(f => f.Name == "DocumentType" && f.Tenant == 0).FirstOrDefault();
            ObjectField documentTypeCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == DocumentTypeTable.Id).FirstOrDefault();

            ObjectField docIsCustomerView = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsCustomerView" && d.ObjectTableId == DocumentTypeTable.Id).FirstOrDefault();
            ObjectField docIsAgentView = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsAgentView" && d.ObjectTableId == DocumentTypeTable.Id).FirstOrDefault();

            // Block
            ObjectTableRule DocTypeBlockRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "DOCTYPEBLCK",
                Name = "Block Fields",
                ObjectTableId = DocumentTypeTable.Id,
                Tenant = 0,
                RuleTypeCode = "BLCK",
                Condition = "If([Tenant] <> \"0\",True,False)",
                SystemLevel = true,
                AdvancedCondition = true,
                ActiveForNew = true,
                ActiveForUpdate = false,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = docIsCustomerView.Id, ObjectFieldCode = docIsCustomerView.FieldCode, ObjectTableRuleId = DocTypeBlockRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = docIsAgentView.Id, ObjectFieldCode = docIsAgentView.FieldCode, ObjectTableRuleId = DocTypeBlockRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);

            //Duplication
            ObjectTableRule DocumentTypeDuplicationRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "DCCD",
                Name = "Code Duplication",
                ObjectTableId = DocumentTypeTable.Id,
                Tenant = 0,
                RuleTypeCode = "DUPL",
                SystemLevel = true,
                OutputMessage = "This document type already exists",
                ActiveForNew = true,
                ActiveForUpdate = false,
                TriggerTypeCode = "ALLW",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            ObjectTableRuleField documentTypeCodeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = documentTypeCode.Id, ObjectFieldCode = documentTypeCode.FieldCode, ObjectTableRuleId = DocumentTypeDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #region EventTypeRules
            ObjectTable EventTypeTable = ObjectContext.ObjectTables.Where(f => f.Name == "EventType" && f.Tenant == 0).FirstOrDefault();
            ObjectField eventTypeCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == EventTypeTable.Id).FirstOrDefault();

            ObjectField eventIsCustomerView = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsCustomerView" && d.ObjectTableId == EventTypeTable.Id).FirstOrDefault();
            ObjectField eventIsAgentView = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsAgentView" && d.ObjectTableId == EventTypeTable.Id).FirstOrDefault();

            // Block
            ObjectTableRule EventTypeBlockRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "EVENTBLCK",
                Name = "Block Fields",
                ObjectTableId = EventTypeTable.Id,
                Tenant = 0,
                RuleTypeCode = "BLCK",
                Condition = "If([Tenant] <> \"0\",True,False)",
                SystemLevel = true,
                AdvancedCondition = true,
                ActiveForNew = true,
                ActiveForUpdate = false,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = eventIsCustomerView.Id, ObjectFieldCode = eventIsCustomerView.FieldCode, ObjectTableRuleId = EventTypeBlockRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = eventIsAgentView.Id, ObjectFieldCode = eventIsAgentView.FieldCode, ObjectTableRuleId = EventTypeBlockRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);

            //Duplication
            ObjectTableRule EventTypeDuplicationRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "EVCD",
                Name = "Code Duplication",
                ObjectTableId = EventTypeTable.Id,
                Tenant = 0,
                RuleTypeCode = "DUPL",
                SystemLevel = true,
                OutputMessage = "This event type already exists",
                ActiveForNew = true,
                ActiveForUpdate = false,
                TriggerTypeCode = "ALLW",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            ObjectTableRuleField eventTypeCodeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = eventTypeCode.Id, ObjectFieldCode = eventTypeCode.FieldCode, ObjectTableRuleId = EventTypeDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #region PackageTypeRules
            ObjectTable PackageTypeTable = ObjectContext.ObjectTables.Where(f => f.Name == "PackageType" && f.Tenant == 0).FirstOrDefault();
            ObjectField packageTypeCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == PackageTypeTable.Id).FirstOrDefault();

            ObjectTableRule PackageTypeDuplicationRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "PKCD",
                Name = "Code Duplication",
                ObjectTableId = PackageTypeTable.Id,
                Tenant = 0,
                RuleTypeCode = "DUPL",
                //TriggerFieldId = packageTypeCode.Id,
                SystemLevel = true,
                OutputMessage = "This package type already exists",
                ActiveForNew = true,
                ActiveForUpdate = false,
                TriggerTypeCode = "ALLW",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            ObjectTableRuleField packageTypeCodeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = packageTypeCode.Id, ObjectFieldCode = packageTypeCode.FieldCode, ObjectTableRuleId = PackageTypeDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleFieldRepository.Add(packageTypeCodeField);
            //ObjectTableRuleRepository.Add(PackageTypeDuplicationRule);
            #endregion

            #region VesselRules
            ObjectTable VesselTable = ObjectContext.ObjectTables.Where(f => f.Name == "Vessel" && f.Tenant == 0).FirstOrDefault();
            ObjectField vesselCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == VesselTable.Id).FirstOrDefault();

            ObjectTableRule VesselDuplicationRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "VSCD",
                Name = "Code Duplication",
                ObjectTableId = VesselTable.Id,
                Tenant = 0,
                RuleTypeCode = "DUPL",
                //TriggerFieldId = vesselCode.Id,
                SystemLevel = true,
                OutputMessage = "This vessel already exists",
                ActiveForNew = true,
                ActiveForUpdate = false,
                TriggerTypeCode = "ALLW",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            ObjectTableRuleField vesselCodeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = vesselCode.Id, ObjectFieldCode = vesselCode.FieldCode, ObjectTableRuleId = VesselDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleFieldRepository.Add(vesselCodeField);
            //ObjectTableRuleRepository.Add(VesselDuplicationRule);
            #endregion

            #region WarehouseRules
            ObjectTable WarehouseTable = ObjectContext.ObjectTables.Where(f => f.Name == "Warehouse" && f.Tenant == 0).FirstOrDefault();
            ObjectField warehouseCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == WarehouseTable.Id).FirstOrDefault();

            ObjectTableRule WarehouseDuplicationRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "WHCD",
                Name = "Code Duplication",
                ObjectTableId = WarehouseTable.Id,
                Tenant = 0,
                RuleTypeCode = "DUPL",
                TriggerFieldId = warehouseCode.Id,
                TriggerFieldCode = warehouseCode.FieldCode,
                SystemLevel = true,
                OutputMessage = "This warehouse already exists",
                ActiveForNew = true,
                ActiveForUpdate = false,
                TriggerTypeCode = "ALLW",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            ObjectTableRuleField warehouseCodeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = warehouseCode.Id, ObjectFieldCode = warehouseCode.FieldCode, ObjectTableRuleId = WarehouseDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleFieldRepository.Add(warehouseCodeField);
            //ObjectTableRuleRepository.Add(WarehouseDuplicationRule);
            #endregion

            #region ContactRules
            //ObjectTable ContactTable = ObjectContext.ObjectTables.Where(f => f.Name == "Contact" && f.Tenant == 0).FirstOrDefault();
            //ObjectField Email = ObjectContext.ObjectFields.Where(d => d.FieldName == "Email" && d.ObjectTableId == ContactTable.Id).FirstOrDefault();

            //ObjectTableRule ContactBlockFieldsRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            //{
            //    RuleCode = "Contact_Block_Fields",
            //    Name = "Contact Block Fields",
            //    ObjectTableId = ContactTable.Id,
            //    Tenant = 0,
            //    RuleTypeCode = "BLCK",
            //    SystemLevel = true,
            //    ActiveForNew = false,
            //    ActiveForUpdate = true,
            //    TriggerTypeCode = "COND",
            //    Condition = "If([Email] <> \"\",True,False)",
            //    RuleNotificationTypeCode = "ERR",
            //    AdvancedCondition = true,
            //}, ObjectTableRuleRepository, TenantObjectTableRule);

            //ObjectTableRuleField EmailBlckField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = Email.Id, ObjectTableRuleId = ContactBlockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #region UserRules
            //ObjectTable UserTable = ObjectContext.ObjectTables.Where(f => f.Name == "User" && f.Tenant == 0).FirstOrDefault();
            //ObjectField UserEmail = ObjectContext.ObjectFields.Where(d => d.FieldName == "Email" && d.ObjectTableId == UserTable.Id).FirstOrDefault();

            //ObjectTableRule UserBlockFieldsRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            //{
            //    RuleCode = "User_Block_Fields",
            //    Name = "User Block Fields",
            //    ObjectTableId = UserTable.Id,
            //    Tenant = 0,
            //    RuleTypeCode = "BLCK",
            //    SystemLevel = true,
            //    ActiveForNew = false,
            //    ActiveForUpdate = true,
            //    TriggerTypeCode = "COND",
            //    Condition = "If([Email] <> \"\",True,False)",
            //    RuleNotificationTypeCode = "ERR",
            //    AdvancedCondition = true,
            //}, ObjectTableRuleRepository, TenantObjectTableRule);

            //ObjectTableRuleField UserEmailBlckField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = UserEmail.Id, ObjectTableRuleId = UserBlockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #region AddressRules

            ObjectTable AddressTable = ObjectContext.ObjectTables.Where(f => f.Name == "Address" && f.Tenant == 0).FirstOrDefault();
            ObjectField addressStateId = ObjectContext.ObjectFields.Where(d => d.FieldName == "StateId" && d.ObjectTableId == AddressTable.Id).FirstOrDefault();
            ObjectField addressCountryId = ObjectContext.ObjectFields.Where(d => d.FieldName == "CountryId" && d.ObjectTableId == AddressTable.Id).FirstOrDefault();

            ObjectTableRule StateRequiredRule1 = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "STATE_REQ",
                Name = "State Required Rule",
                ObjectTableId = AddressTable.Id,
                Tenant = 0,
                RuleTypeCode = "REQ",
                SystemLevel = true,
                ActiveForNew = false,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                Condition = "If(([CountryId.IsStateRequired] = \"true\"),True,False)",
                RuleNotificationTypeCode = "ERR",
                AdvancedCondition = true,
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            //ObjectTableRule StateRequiredRule2 = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            //{
            //    RuleCode = "STATE_REQ2",
            //    Name = "State Required Rule2",
            //    ObjectTableId = AddressTable.Id,
            //    Tenant = 0,
            //    RuleTypeCode = "REQ",
            //    SystemLevel = true,
            //    ActiveForNew = false,
            //    ActiveForUpdate = true,
            //    TriggerTypeCode = "COND",
            //    Condition = "If([CountryId.Code] = \"CA\",True,False)",
            //    RuleNotificationTypeCode = "ERR",
            //    AdvancedCondition = true,
            //}, ObjectTableRuleRepository, TenantObjectTableRule);


            AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = addressStateId.Id, ObjectFieldCode = addressStateId.FieldCode, ObjectTableRuleId = StateRequiredRule1.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);

            //AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = addressStateId.Id, ObjectTableRuleId = StateRequiredRule2.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);


            #region Country_FieldChanged_Rule

            ObjectTableRule CountryChangedRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {

                RuleCode = "COUNTRY_STATE",
                Name = "Country state set",
                ObjectTableId = AddressTable.Id,
                Tenant = 0,
                RuleTypeCode = "SETV",
                TriggerFieldId = addressCountryId.Id,
                TriggerFieldCode = addressCountryId.FieldCode,
                SystemLevel = true,

                ActiveForNew = false,
                ActiveForUpdate = true,
                TriggerTypeCode = "FLDC",
                RuleNotificationTypeCode = "ERR",


            }, ObjectTableRuleRepository, TenantObjectTableRule);



            ObjectTableRuleField CountryIdChangedField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails()
            {
                // Id = IdCounter.GetNumber("ObjectTableRuleField").ToString(),
                ObjectFieldId = addressStateId.Id,
                ObjectFieldCode = addressStateId.FieldCode,
                ObjectTableRuleId = CountryChangedRule.Id,
                SystemLevel = true,
                Tenant = 0,
                Expression = null,
            }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);



            #endregion

            #endregion

            #region Customer Rules
            ObjectTable CustomerTable = ObjectContext.ObjectTables.Where(f => f.Name == "Customer" && f.Tenant == 0).FirstOrDefault();
            ObjectField CustomerStateId = ObjectContext.ObjectFields.Where(d => d.FieldName == "StateId_Potential" && d.ObjectTableId == CustomerTable.Id).FirstOrDefault();

            ObjectTableRule CustomerStateRequiredRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "CUS_STATE_REQ",
                Name = "Customer State Required Rule",
                ObjectTableId = CustomerTable.Id,
                Tenant = 0,
                RuleTypeCode = "REQ",
                SystemLevel = true,
                ActiveForNew = false,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                Condition = "If(([CountryId_Potential.IsStateRequired] = \"true\"),True,False)",
                RuleNotificationTypeCode = "ERR",
                AdvancedCondition = true,
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = CustomerStateId.Id, ObjectFieldCode = CustomerStateId.FieldCode, ObjectTableRuleId = CustomerStateRequiredRule.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            ObjectContext.SaveChanges();
        }

        private void CreateShipmentRules(Dictionary<string, ObjectTableRule> TenantObjectTableRule, Dictionary<string, ObjectTableRuleField> TenantObjectTableRuleFields, List<ObjectFieldValidation> TenantObjectFieldValidations, Dictionary<string, RuleConditionField> TenantRuleConditionFields)
        {
            #region ObjectFields
            ObjectTable ShipmentTable = ObjectContext.ObjectTables.Where(f => f.Name == "Shipment" && f.Tenant == 0).FirstOrDefault();

            ObjectField ShipmentTypeId = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipmentTypeId" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField HAWBDate = ObjectContext.ObjectFields.Where(d => d.FieldName == "HAWBDate" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField MainCarriageCarrierId = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainCarriageCarrierId" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField MAWBOBL = ObjectContext.ObjectFields.Where(d => d.FieldName == "Master" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField MAWBOBLDate = ObjectContext.ObjectFields.Where(d => d.FieldName == "MAWBOBLDate" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField MainCarriageCarrierNumber = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainCarriageCarrierNumber" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField MainCarriageFromPortId = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainCarriageFromPortId" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField MainCarriageToPortId = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainCarriageToPortId" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField HAWBFBLBL = ObjectContext.ObjectFields.Where(d => d.FieldName == "House" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField TransportModeId = ObjectContext.ObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            // Packages Totals Fields
            ObjectField RateClassCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "RateClassCode" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField NumberOfPackages = ObjectContext.ObjectFields.Where(d => d.FieldName == "NumberOfPackages" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField DescriptionOfGoods = ObjectContext.ObjectFields.Where(d => d.FieldName == "DescriptionOfGoods" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField GrossWeight = ObjectContext.ObjectFields.Where(d => d.FieldName == "GrossWeight" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField VolumetricWeight = ObjectContext.ObjectFields.Where(d => d.FieldName == "VolumetricWeight" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField Volume = ObjectContext.ObjectFields.Where(d => d.FieldName == "Volume" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField ChargeableWeight = ObjectContext.ObjectFields.Where(d => d.FieldName == "ChargeableWeight" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();

            ObjectField AWBFreightPrepaid = ObjectContext.ObjectFields.Where(d => d.FieldName == "AWBFreightAmountPrepaid" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField AWBFreightCollect = ObjectContext.ObjectFields.Where(d => d.FieldName == "AWBFreightAmountCollect" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField DangerousClassNumber = ObjectContext.ObjectFields.Where(d => d.FieldName == "DangerousClassNumber" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField DangerousUnNumber = ObjectContext.ObjectFields.Where(d => d.FieldName == "DangerousUnNumber" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField DangerousPackagingGroup = ObjectContext.ObjectFields.Where(d => d.FieldName == "DangerousPackagingGroup" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField DangerousFlashPoint = ObjectContext.ObjectFields.Where(d => d.FieldName == "DangerousFlashPoint" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField DangerousMaterialDescription = ObjectContext.ObjectFields.Where(d => d.FieldName == "DangerousMaterialDescription" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField MainHarmonize = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainHarmonize" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField NumberOfContainers = ObjectContext.ObjectFields.Where(d => d.FieldName == "NumberOfContainers" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();

            ObjectField DirectionId = ObjectContext.ObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField IncotermId = ObjectContext.ObjectFields.Where(d => d.FieldName == "IncotermId" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField FreightPrepaidCollectId = ObjectContext.ObjectFields.Where(d => d.FieldName == "FreightPrepaidCollectId" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField OtherPrepaidCollectId = ObjectContext.ObjectFields.Where(d => d.FieldName == "OtherPrepaidCollectId" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField SalesManUserId = ObjectContext.ObjectFields.Where(d => d.FieldName == "SalesmanUserId" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField OpenedByUserId = ObjectContext.ObjectFields.Where(d => d.FieldName == "CreatedByUserId" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField DepartmentId = ObjectContext.ObjectFields.Where(d => d.FieldName == "DepartmentId" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField BranchId = ObjectContext.ObjectFields.Where(d => d.FieldName == "BranchId" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField StatusId = ObjectContext.ObjectFields.Where(d => d.FieldName == "StatusId" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField IsOperationalClosed = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsOperationalClosed" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField ShipmentLevelCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipmentLevelCode" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField ConnectedShipments = ObjectContext.ObjectFields.Where(d => d.FieldName == "ConnectedShipments" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField MainCarriageVesselId = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainCarriageVesselId" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField MainCarriageETA = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainCarriageETA" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField MainCarriageATD = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainCarriageATD" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField MainCarriageATA = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainCarriageATA" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField AMSBL = ObjectContext.ObjectFields.Where(d => d.FieldName == "AMSBL" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            ObjectField ShipmentCustomerTypeCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipmentCustomerTypeCode" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();
            #endregion

            #region ObjectTableRules

            #region blockFieldsRule

            #region ShipTypeBlck || ShipmentBlockFieldsUpdate
            ObjectTableRule blockShipmentTypeRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "ShipTypeBlck",
                Name = "Shipment Type Block",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "BLCK",
                SystemLevel = true,
                ActiveForNew = false,
                ActiveForUpdate = true,
                TriggerTypeCode = "ALLW",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            ObjectTableRule blockedFieldsUpdateRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "ShipmentBlockFieldsUpdate",
                Name = "Block Fields Update",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "BLCK",
                SystemLevel = true,
                ActiveForNew = false,
                ActiveForUpdate = true,
                TriggerTypeCode = "ALLW",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            ObjectTableRuleField AllOpenedByUserIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = OpenedByUserId.Id, ObjectFieldCode = OpenedByUserId.FieldCode, ObjectTableRuleId = blockedFieldsUpdateRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField ShipmentTypeIdBlckField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = ShipmentTypeId.Id, ObjectFieldCode = ShipmentTypeId.FieldCode, ObjectTableRuleId = blockShipmentTypeRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #region Operational Closed
            ObjectTableRule blockFieldsRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "OPCL",
                Name = "Operational Closed",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "BLCK",
                SystemLevel = true,
                ActiveForNew = false,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField IsOperationalClosed_CondField = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = IsOperationalClosed.Id, ObjectFieldCode = IsOperationalClosed.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, Operator = "Equals", Value = "True", Tenant = blockFieldsRule.Tenant }, RuleConditionFieldRepository, TenantRuleConditionFields);

            ObjectTableRuleField ShipmentTypeIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = ShipmentTypeId.Id, ObjectFieldCode = ShipmentTypeId.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField HAWBDateField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = HAWBDate.Id, ObjectFieldCode = HAWBDate.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField MainCarriageCarrierIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageCarrierId.Id, ObjectFieldCode = MainCarriageCarrierId.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField MAWBOBLField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MAWBOBL.Id, ObjectFieldCode = MAWBOBL.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField MAWBOBLDateField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MAWBOBLDate.Id, ObjectFieldCode = MAWBOBLDate.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField HAWBFBLBLField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = HAWBFBLBL.Id, ObjectFieldCode = HAWBFBLBL.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField StatusIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = StatusId.Id, ObjectFieldCode = StatusId.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField IncotermIdField1 = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = IncotermId.Id, ObjectFieldCode = IncotermId.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField FreightPrepaidCollectIdField1 = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = FreightPrepaidCollectId.Id, ObjectFieldCode = FreightPrepaidCollectId.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField OtherPrepaidCollectIdField1 = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = OtherPrepaidCollectId.Id, ObjectFieldCode = OtherPrepaidCollectId.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField SalesManUserIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = SalesManUserId.Id, ObjectFieldCode = SalesManUserId.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField OpenedByUserIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = OpenedByUserId.Id, ObjectFieldCode = OpenedByUserId.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField DepartmentIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = DepartmentId.Id, ObjectFieldCode = DepartmentId.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField BranchIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = BranchId.Id, ObjectFieldCode = BranchId.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField MainCarriageCarrierNumberField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageCarrierNumber.Id, ObjectFieldCode = MainCarriageCarrierNumber.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField RateClassCodeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = RateClassCode.Id, ObjectFieldCode = RateClassCode.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField NumberOfPackagesField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = NumberOfPackages.Id, ObjectFieldCode = NumberOfPackages.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField DescriptionOfGoodsField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = DescriptionOfGoods.Id, ObjectFieldCode = DescriptionOfGoods.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField GrossWeightField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = GrossWeight.Id, ObjectFieldCode = GrossWeight.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField VolumetricWeightField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = VolumetricWeight.Id, ObjectFieldCode = VolumetricWeight.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField VolumeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = Volume.Id, ObjectFieldCode = Volume.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField ChargeableWeightField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = ChargeableWeight.Id, ObjectFieldCode = ChargeableWeight.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField AWBFreightPrepaidField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = AWBFreightPrepaid.Id, ObjectFieldCode = AWBFreightPrepaid.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField AWBFreightCollectField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = AWBFreightCollect.Id, ObjectFieldCode = AWBFreightCollect.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField DangerousClassNumberField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = DangerousClassNumber.Id, ObjectFieldCode = DangerousClassNumber.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField DangerousUnNumberField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = DangerousUnNumber.Id, ObjectFieldCode = DangerousUnNumber.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField DangerousPackagingGroupField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = DangerousPackagingGroup.Id, ObjectFieldCode = DangerousPackagingGroup.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField DangerousFlashPointField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = DangerousFlashPoint.Id, ObjectFieldCode = DangerousFlashPoint.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField DangerousMaterialDescriptionField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = DangerousMaterialDescription.Id, ObjectFieldCode = DangerousMaterialDescription.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField MainHarmonizeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainHarmonize.Id, ObjectFieldCode = MainHarmonize.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField NumberOfContainersField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = NumberOfContainers.Id, ObjectFieldCode = NumberOfContainers.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField MainCarriageFromPortIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageFromPortId.Id, ObjectFieldCode = MainCarriageFromPortId.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField MainCarriageToPortIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageToPortId.Id, ObjectFieldCode = MainCarriageToPortId.FieldCode, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #endregion

            #region MAWBDuplicationRule
            //ObjectTableRule MAWBDuplicationRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            //{
            //    RuleCode = "MAWB",
            //    Name = "MAWB Duplication",
            //    ObjectTableId = ShipmentTable.Id,
            //    Tenant = 0,
            //    RuleTypeCode = "DUPL",
            //    SystemLevel = true,
            //    OutputMessage = "The Master number is used by another shipment",
            //    ActiveForNew = true,
            //    ActiveForUpdate = true,
            //    TriggerTypeCode = "COND",
            //    RuleNotificationTypeCode = "ERR",
            //    Condition = "If(And([DirectionId]  =  \"E\",[TransportModeId]  =  \"A\",[Master] <> \"\",Or([ShipmentLevelCode]= \"C\",[ShipmentLevelCode]= \"D\")),True,False)",
            //    InActive = false,
            //    AdvancedCondition = true,
            //}, ObjectTableRuleRepository, TenantObjectTableRule);

            //ObjectTableRuleField DirectionIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = DirectionId.Id, ObjectTableRuleId = MAWBDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField TransportModeIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = TransportModeId.Id, ObjectTableRuleId = MAWBDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField MAWBOBLField3 = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MAWBOBL.Id, ObjectTableRuleId = MAWBDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField MainCarriageCarrierIdField3 = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageCarrierId.Id, ObjectTableRuleId = MAWBDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            ObjectContext.SaveChanges();

            #region HAWBDuplicationRule
            ObjectTableRule HAWBDuplicationRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "HAWB",
                Name = "HAWB Duplication",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "DUPL",
                SystemLevel = true,
                OutputMessage = "The HAWBFBLBL is used by another shipment",
                ActiveForNew = true,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "ERR",
                AdvancedCondition = true,
                Condition = "If(And([DirectionId]  =  \"E\",[House] <> \"\",[ShipmentLevelCode] <>\"C\",[Tenant] <>\"211\"),True,False)",
                InActive = false,
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            ObjectTableRuleField TransportModeIdField1 = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = TransportModeId.Id, ObjectFieldCode = TransportModeId.FieldCode, ObjectTableRuleId = HAWBDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField DirectionIdField1 = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = DirectionId.Id, ObjectFieldCode = DirectionId.FieldCode, ObjectTableRuleId = HAWBDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField HAWBFBLBLField1 = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = HAWBFBLBL.Id, ObjectFieldCode = HAWBFBLBL.FieldCode, ObjectTableRuleId = HAWBDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            ObjectContext.SaveChanges();

            #region Shipment Operational Closed Rules

            #region Shipment_OpClosed_Req_AE
            ObjectTableRule Shipment_OpClosed_Req_AE = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "Shipment_OpClosed_Req_AE",
                Name = "Shipment_OpClosed_Req_AE",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "REQ",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "WAR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField IsOperationalClosed_OPAE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = IsOperationalClosed.Id, ObjectFieldCode = IsOperationalClosed.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_AE.Id, Operator = "Equals", Value = "True", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField TransportModeId_OPAE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = TransportModeId.Id, ObjectFieldCode = TransportModeId.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_AE.Id, Operator = "Equals", Value = "A", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField ShipmentLevelCode_OPAE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = ShipmentLevelCode.Id, ObjectFieldCode = ShipmentLevelCode.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_AE.Id, Operator = "Equals", Value = "H", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField Direction_OPAE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = DirectionId.Id, ObjectFieldCode = DirectionId.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_AE.Id, Operator = "Equals", Value = "E", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);

            ObjectTableRuleField House_Field_AE = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = HAWBFBLBL.Id, ObjectFieldCode = HAWBFBLBL.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_AE.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField Incoterm_Field_AE = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = IncotermId.Id, ObjectFieldCode = IncotermId.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_AE.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #region Shipment_OpClosed_Req_OE
            ObjectTableRule Shipment_OpClosed_Req_OE = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "Shipment_OpClosed_Req_OE",
                Name = "Shipment_OpClosed_Req_OE",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "REQ",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "WAR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField IsOperationalClosed_OPOE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = IsOperationalClosed.Id, ObjectFieldCode = IsOperationalClosed.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_OE.Id, Operator = "Equals", Value = "True", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField TransportModeId_OPOE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = TransportModeId.Id, ObjectFieldCode = TransportModeId.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_OE.Id, Operator = "Equals", Value = "O", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField Direction_OPOE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = DirectionId.Id, ObjectFieldCode = DirectionId.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_OE.Id, Operator = "Equals", Value = "E", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField ShipmentLevelCode_OPOE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = ShipmentLevelCode.Id, ObjectFieldCode = ShipmentLevelCode.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_OE.Id, Operator = "Equals", Value = "H", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);

            ObjectTableRuleField House_Field_OE = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = HAWBFBLBL.Id, ObjectFieldCode = HAWBFBLBL.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_OE.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField Incoterm_Field_OE = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = IncotermId.Id, ObjectFieldCode = IncotermId.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_OE.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #region Shipment_OpClosed_Req_IE
            ObjectTableRule Shipment_OpClosed_Req_IE = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "Shipment_OpClosed_Req_IE",
                Name = "Shipment_OpClosed_Req_IE",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "REQ",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "WAR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField IsOperationalClosed_OPIE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = IsOperationalClosed.Id, ObjectFieldCode = IsOperationalClosed.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_IE.Id, Operator = "Equals", Value = "True", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField TransportModeId_OPIE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = TransportModeId.Id, ObjectFieldCode = TransportModeId.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_IE.Id, Operator = "Equals", Value = "I", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField Direction_OPIE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = DirectionId.Id, ObjectFieldCode = DirectionId.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_IE.Id, Operator = "Equals", Value = "E", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField ShipmentLevelCode_OPIE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = ShipmentLevelCode.Id, ObjectFieldCode = ShipmentLevelCode.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_IE.Id, Operator = "Equals", Value = "H", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);

            ObjectTableRuleField House_Field_IE = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = HAWBFBLBL.Id, ObjectFieldCode = HAWBFBLBL.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_IE.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField Incoterm_Field_IE = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = IncotermId.Id, ObjectFieldCode = IncotermId.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_IE.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #region Shipment_OpClosed_Req_AI
            ObjectTableRule Shipment_OpClosed_Req_AI = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "Shipment_OpClosed_Req_AI",
                Name = "Shipment_OpClosed_Req_AI",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "REQ",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "WAR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField IsOperationalClosed_OPAI = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = IsOperationalClosed.Id, ObjectFieldCode = IsOperationalClosed.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_AI.Id, Operator = "Equals", Value = "True", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField TransportModeId_OPAI = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = TransportModeId.Id, ObjectFieldCode = TransportModeId.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_AI.Id, Operator = "Equals", Value = "A", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField Direction_OPAI = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = DirectionId.Id, ObjectFieldCode = DirectionId.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_AI.Id, Operator = "Equals", Value = "I", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField ShipmentLevelCode_OPAI = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = ShipmentLevelCode.Id, ObjectFieldCode = ShipmentLevelCode.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_AI.Id, Operator = "Equals", Value = "H", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);

            ObjectTableRuleField House_Field_AI = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = HAWBFBLBL.Id, ObjectFieldCode = HAWBFBLBL.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_AI.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField Incoterm_Field_AI = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = IncotermId.Id, ObjectFieldCode = IncotermId.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_AI.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #region Shipment_OpClosed_Req_OI
            ObjectTableRule Shipment_OpClosed_Req_OI = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "Shipment_OpClosed_Req_OI",
                Name = "Shipment_OpClosed_Req_OI",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "REQ",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "WAR",

            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField IsOperationalClosed_OPOI = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = IsOperationalClosed.Id, ObjectFieldCode = IsOperationalClosed.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_OI.Id, Operator = "Equals", Value = "True", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField TransportModeId_OPOI = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = TransportModeId.Id, ObjectFieldCode = TransportModeId.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_OI.Id, Operator = "Equals", Value = "O", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField Direction_OPOI = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = DirectionId.Id, ObjectFieldCode = DirectionId.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_OI.Id, Operator = "Equals", Value = "I", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField ShipmentLevelCode_OPOI = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = ShipmentLevelCode.Id, ObjectFieldCode = ShipmentLevelCode.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_OI.Id, Operator = "Equals", Value = "H", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);

            ObjectTableRuleField House_Field_OI = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = HAWBFBLBL.Id, ObjectFieldCode = HAWBFBLBL.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_OI.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField Incoterm_Field_OI = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = IncotermId.Id, ObjectFieldCode = IncotermId.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_OI.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #region Shipment_OpClosed_Req_II
            ObjectTableRule Shipment_OpClosed_Req_II = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "Shipment_OpClosed_Req_II",
                Name = "Shipment_OpClosed_Req_II",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "REQ",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "WAR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField IsOperationalClosed_OPII = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = IsOperationalClosed.Id, ObjectFieldCode = IsOperationalClosed.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_II.Id, Operator = "Equals", Value = "True", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField TransportModeId_OPII = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = TransportModeId.Id, ObjectFieldCode = TransportModeId.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_II.Id, Operator = "Equals", Value = "I", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField Direction_OPII = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = DirectionId.Id, ObjectFieldCode = DirectionId.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_II.Id, Operator = "Equals", Value = "I", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField ShipmentLevelCode_OPII = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = ShipmentLevelCode.Id, ObjectFieldCode = ShipmentLevelCode.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_II.Id, Operator = "Equals", Value = "H", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);

            ObjectTableRuleField House_Field_II = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = HAWBFBLBL.Id, ObjectFieldCode = HAWBFBLBL.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_II.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField Incoterm_Field_II = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = IncotermId.Id, ObjectFieldCode = IncotermId.FieldCode, ObjectTableRuleId = Shipment_OpClosed_Req_II.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #region Master_OpClosed_Req_AE
            ObjectTableRule Master_OpClosed_Req_AE = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "Master_OpClosed_Req_AE",
                Name = "Master_OpClosed_Req_AE",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "REQ",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "WAR",
                //Condition = "If(And([TransportModeId] = \"A\",[DirectionId]  =  \"E\",[IsOperationalClosed],Or([ShipmentLevelCode]= \"C\",[ShipmentLevelCode]= \"D\")),True,False)",
                //AdvancedCondition = true,
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField TransportModeId_MOPAE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = TransportModeId.Id, ObjectFieldCode = TransportModeId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AE.Id, Operator = "Equals", Value = "A", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField Direction_MOPAE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = DirectionId.Id, ObjectFieldCode = DirectionId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AE.Id, Operator = "Equals", Value = "E", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField IsOperationalClosed_MOPAE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = IsOperationalClosed.Id, ObjectFieldCode = IsOperationalClosed.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AE.Id, Operator = "Equals", Value = "True", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField ShipmentLevelCode_MOPAE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = ShipmentLevelCode.Id, ObjectFieldCode = ShipmentLevelCode.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AE.Id, Operator = "Equals", Value = "C", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
           

            ObjectTableRuleField Carrier_Field_AE = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageCarrierId.Id, ObjectFieldCode = MainCarriageCarrierId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AE.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField Master_Field_AE = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MAWBOBL.Id, ObjectFieldCode = MAWBOBL.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AE.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField ATD_Field_AE = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageATD.Id, ObjectFieldCode = MainCarriageATD.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AE.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField FreightPP_Field_AE = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = FreightPrepaidCollectId.Id, ObjectFieldCode = FreightPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AE.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField OtherPP_Field_AE = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = OtherPrepaidCollectId.Id, ObjectFieldCode = OtherPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AE.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField CarrierNo_Field_AE = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageCarrierNumber.Id, ObjectFieldCode = MainCarriageCarrierNumber.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AE.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);


            ObjectTableRule Master_OpClosed_Req_AE_D = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "Master_OpClosed_Req_AE_D",
                Name = "Master_OpClosed_Req_AE_D",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "REQ",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "WAR",
                //Condition = "If(And([TransportModeId] = \"A\",[DirectionId]  =  \"E\",[IsOperationalClosed],Or([ShipmentLevelCode]= \"C\",[ShipmentLevelCode]= \"D\")),True,False)",
                //AdvancedCondition = true,
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField TransportModeId_MOPAED = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = TransportModeId.Id, ObjectFieldCode = TransportModeId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AE_D.Id, Operator = "Equals", Value = "A", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField Direction_MOPAED = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = DirectionId.Id, ObjectFieldCode = DirectionId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AE_D.Id, Operator = "Equals", Value = "E", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField IsOperationalClosed_MOPAED = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = IsOperationalClosed.Id, ObjectFieldCode = IsOperationalClosed.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AE_D.Id, Operator = "Equals", Value = "True", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField ShipmentLevelCode_MOPAED = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = ShipmentLevelCode.Id, ObjectFieldCode = ShipmentLevelCode.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AE_D.Id, Operator = "Equals", Value = "D", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);


            ObjectTableRuleField Carrier_Field_AED = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageCarrierId.Id, ObjectFieldCode = MainCarriageCarrierId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AE_D.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField Master_Field_AED = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MAWBOBL.Id, ObjectFieldCode = MAWBOBL.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AE_D.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField ATD_Field_AED = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageATD.Id, ObjectFieldCode = MainCarriageATD.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AE_D.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField FreightPP_Field_AED = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = FreightPrepaidCollectId.Id, ObjectFieldCode = FreightPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AE_D.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField OtherPP_Field_AED = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = OtherPrepaidCollectId.Id, ObjectFieldCode = OtherPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AE_D.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField CarrierNo_Field_AED = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageCarrierNumber.Id, ObjectFieldCode = MainCarriageCarrierNumber.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AE_D.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #region Master_OpClosed_Req_OE
            ObjectTableRule Master_OpClosed_Req_OE = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "Master_OpClosed_Req_OE",
                Name = "Master_OpClosed_Req_OE",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "REQ",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "WAR",
                //Condition = "If(And([TransportModeId] = \"O\",[DirectionId]  =  \"E\",[IsOperationalClosed],Or([ShipmentLevelCode]= \"C\",[ShipmentLevelCode]= \"D\")),True,False)",
                //AdvancedCondition = true,
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField TransportModeId_MOPOE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = TransportModeId.Id, ObjectFieldCode = TransportModeId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OE.Id, Operator = "Equals", Value = "O", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField Direction_MOPOE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = DirectionId.Id, ObjectFieldCode = DirectionId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OE.Id, Operator = "Equals", Value = "E", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField IsOperationalClosed_MOPOE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = IsOperationalClosed.Id, ObjectFieldCode = IsOperationalClosed.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OE.Id, Operator = "Equals", Value = "True", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField ShipmentLevelCode_MOPOE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = ShipmentLevelCode.Id, ObjectFieldCode = ShipmentLevelCode.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OE.Id, Operator = "Equals", Value = "C", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
           

            ObjectTableRuleField Carrier_Field_OE = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageCarrierId.Id, ObjectFieldCode = MainCarriageCarrierId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OE.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField ATD_Field_OE = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageATD.Id, ObjectFieldCode = MainCarriageATD.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OE.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField FreightPP_Field_OE = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = FreightPrepaidCollectId.Id, ObjectFieldCode = FreightPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OE.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField OtherPP_Field_OE = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = OtherPrepaidCollectId.Id, ObjectFieldCode = OtherPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OE.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField CarrierNo_Field_OE = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageCarrierNumber.Id, ObjectFieldCode = MainCarriageCarrierNumber.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OE.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField Vessel__Field_OE = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageVesselId.Id, ObjectFieldCode = MainCarriageVesselId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OE.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);



            ObjectTableRule Master_OpClosed_Req_OED = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "Master_OpClosed_Req_OE_D",
                Name = "Master_OpClosed_Req_OE_D",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "REQ",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "WAR",
                //Condition = "If(And([TransportModeId] = \"O\",[DirectionId]  =  \"E\",[IsOperationalClosed],Or([ShipmentLevelCode]= \"C\",[ShipmentLevelCode]= \"D\")),True,False)",
                //AdvancedCondition = true,
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField TransportModeId_MOPOED = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = TransportModeId.Id, ObjectFieldCode = TransportModeId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OED.Id, Operator = "Equals", Value = "O", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField Direction_MOPOED = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = DirectionId.Id, ObjectFieldCode = DirectionId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OED.Id, Operator = "Equals", Value = "E", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField IsOperationalClosed_MOPOED = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = IsOperationalClosed.Id, ObjectFieldCode = IsOperationalClosed.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OED.Id, Operator = "Equals", Value = "True", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField ShipmentLevelCode_MOPOED = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = ShipmentLevelCode.Id, ObjectFieldCode = ShipmentLevelCode.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OED.Id, Operator = "Equals", Value = "D", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);


            ObjectTableRuleField Carrier_Field_OED = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageCarrierId.Id, ObjectFieldCode = MainCarriageCarrierId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OED.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField ATD_Field_OED = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageATD.Id, ObjectFieldCode = MainCarriageATD.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OED.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField FreightPP_Field_OED = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = FreightPrepaidCollectId.Id, ObjectFieldCode = FreightPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OED.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField OtherPP_Field_OED = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = OtherPrepaidCollectId.Id, ObjectFieldCode = OtherPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OED.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField CarrierNo_Field_OED = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageCarrierNumber.Id, ObjectFieldCode = MainCarriageCarrierNumber.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OED.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField Vessel__Field_OED = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageVesselId.Id, ObjectFieldCode = MainCarriageVesselId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OED.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #region Master_OpClosed_Req_AI
            ObjectTableRule Master_OpClosed_Req_AI = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "Master_OpClosed_Req_AI",
                Name = "Master_OpClosed_Req_AI",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "REQ",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "WAR",
                //Condition = "If(And([TransportModeId] = \"A\",[DirectionId]  =  \"I\",[IsOperationalClosed],Or([ShipmentLevelCode]= \"C\",[ShipmentLevelCode]= \"D\")),True,False)",
                //AdvancedCondition = true,
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField TransportModeId_MOPAI = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = TransportModeId.Id, ObjectFieldCode = TransportModeId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AI.Id, Operator = "Equals", Value = "A", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField Direction_MOPAI = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = DirectionId.Id, ObjectFieldCode = DirectionId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AI.Id, Operator = "Equals", Value = "I", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField IsOperationalClosed_MOPAI = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = IsOperationalClosed.Id, ObjectFieldCode = IsOperationalClosed.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AI.Id, Operator = "Equals", Value = "True", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField ShipmentLevelCode_MOPAI = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = ShipmentLevelCode.Id, ObjectFieldCode = ShipmentLevelCode.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AI.Id, Operator = "Equals", Value = "C", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
           
            ObjectTableRuleField Carrier_Field_AI = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageCarrierId.Id, ObjectFieldCode = MainCarriageCarrierId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AI.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField Master_Field_AI = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MAWBOBL.Id, ObjectFieldCode = MAWBOBL.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AI.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField ATA_Field_AI = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageATA.Id, ObjectFieldCode = MainCarriageATA.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AI.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField FreightPP_Field_AI = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = FreightPrepaidCollectId.Id, ObjectFieldCode = FreightPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AI.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField OtherPP_Field_AI = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = OtherPrepaidCollectId.Id, ObjectFieldCode = OtherPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AI.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);

            ObjectTableRule Master_OpClosed_Req_AID = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "Master_OpClosed_Req_AI_D",
                Name = "Master_OpClosed_Req_AI_D",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "REQ",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "WAR",
                //Condition = "If(And([TransportModeId] = \"A\",[DirectionId]  =  \"I\",[IsOperationalClosed],Or([ShipmentLevelCode]= \"C\",[ShipmentLevelCode]= \"D\")),True,False)",
                //AdvancedCondition = true,
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField TransportModeId_MOPAID = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = TransportModeId.Id, ObjectFieldCode = TransportModeId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AID.Id, Operator = "Equals", Value = "A", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField Direction_MOPAID = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = DirectionId.Id, ObjectFieldCode = DirectionId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AID.Id, Operator = "Equals", Value = "I", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField IsOperationalClosed_MOPAID = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = IsOperationalClosed.Id, ObjectFieldCode = IsOperationalClosed.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AID.Id, Operator = "Equals", Value = "True", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField ShipmentLevelCode_MOPAID = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = ShipmentLevelCode.Id, ObjectFieldCode = ShipmentLevelCode.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AID.Id, Operator = "Equals", Value = "D", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);

            ObjectTableRuleField Carrier_Field_AID = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageCarrierId.Id, ObjectFieldCode = MainCarriageCarrierId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AID.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField Master_Field_AID = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MAWBOBL.Id, ObjectFieldCode = MAWBOBL.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AID.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField ATA_Field_AID = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageATA.Id, ObjectFieldCode = MainCarriageATA.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AID.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField FreightPP_Field_AID = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = FreightPrepaidCollectId.Id, ObjectFieldCode = FreightPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AID.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField OtherPP_Field_AID = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = OtherPrepaidCollectId.Id, ObjectFieldCode = OtherPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_AID.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #region Master_OpClosed_Req_OI
            ObjectTableRule Master_OpClosed_Req_OI = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "Master_OpClosed_Req_OI",
                Name = "Master_OpClosed_Req_OI",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "REQ",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "WAR",
                //Condition = "If(And([TransportModeId] = \"O\",[DirectionId]  =  \"I\",[IsOperationalClosed],Or([ShipmentLevelCode]= \"C\",[ShipmentLevelCode]= \"D\")),True,False)",
                //AdvancedCondition = true,
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField TransportModeId_MOPOI = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = TransportModeId.Id, ObjectFieldCode = TransportModeId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OI.Id, Operator = "Equals", Value = "O", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField Direction_MOPOI = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = DirectionId.Id, ObjectFieldCode = DirectionId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OI.Id, Operator = "Equals", Value = "I", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField IsOperationalClosed_MOPOI = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = IsOperationalClosed.Id, ObjectFieldCode = IsOperationalClosed.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OI.Id, Operator = "Equals", Value = "True", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField ShipmentLevelCode_MOPOI = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = ShipmentLevelCode.Id, ObjectFieldCode = ShipmentLevelCode.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OI.Id, Operator = "Equals", Value = "C", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
     

            ObjectTableRuleField Carrier_Field_OI = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageCarrierId.Id, ObjectFieldCode = MainCarriageCarrierId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OI.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField Master_Field_OI = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MAWBOBL.Id, ObjectFieldCode = MAWBOBL.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OI.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField ATA_Field_OI = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageATA.Id, ObjectFieldCode = MainCarriageATA.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OI.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField FreightPP_Field_OI = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = FreightPrepaidCollectId.Id, ObjectFieldCode = FreightPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OI.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField OtherPP_Field_OI = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = OtherPrepaidCollectId.Id, ObjectFieldCode = OtherPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OI.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField Vessel__Field_OI = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageVesselId.Id, ObjectFieldCode = MainCarriageVesselId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OI.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);


            ObjectTableRule Master_OpClosed_Req_OID = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "Master_OpClosed_Req_OI_D",
                Name = "Master_OpClosed_Req_OI_D",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "REQ",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "WAR",
                //Condition = "If(And([TransportModeId] = \"O\",[DirectionId]  =  \"I\",[IsOperationalClosed],Or([ShipmentLevelCode]= \"C\",[ShipmentLevelCode]= \"D\")),True,False)",
                //AdvancedCondition = true,
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField TransportModeId_MOPOID = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = TransportModeId.Id, ObjectFieldCode = TransportModeId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OID.Id, Operator = "Equals", Value = "O", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField Direction_MOPOID = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = DirectionId.Id, ObjectFieldCode = DirectionId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OID.Id, Operator = "Equals", Value = "I", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField IsOperationalClosed_MOPOID = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = IsOperationalClosed.Id, ObjectFieldCode = IsOperationalClosed.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OID.Id, Operator = "Equals", Value = "True", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField ShipmentLevelCode_MOPOID = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = ShipmentLevelCode.Id, ObjectFieldCode = ShipmentLevelCode.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OID.Id, Operator = "Equals", Value = "D", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);


            ObjectTableRuleField Carrier_Field_OID = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageCarrierId.Id, ObjectFieldCode = MainCarriageCarrierId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OID.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField Master_Field_OID = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MAWBOBL.Id, ObjectFieldCode = MAWBOBL.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OID.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField ATA_Field_OID = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageATA.Id, ObjectFieldCode = MainCarriageATA.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OID.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField FreightPP_Field_OID = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = FreightPrepaidCollectId.Id, ObjectFieldCode = FreightPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OID.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField OtherPP_Field_OID = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = OtherPrepaidCollectId.Id, ObjectFieldCode = OtherPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OID.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField Vessel__Field_OID = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageVesselId.Id, ObjectFieldCode = MainCarriageVesselId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_OID.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            
            #endregion

            #region Master_OpClosed_Req_II
            ObjectTableRule Master_OpClosed_Req_II = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "Master_OpClosed_Req_II",
                Name = "Master_OpClosed_Req_II",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "REQ",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "WAR",
                //Condition = "If(And([TransportModeId] = \"I\",[DirectionId]  =  \"I\",[IsOperationalClosed],Or([ShipmentLevelCode]= \"C\",[ShipmentLevelCode]= \"D\")),True,False)",
                //AdvancedCondition = true,
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField TransportModeId_MOPII = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = TransportModeId.Id, ObjectFieldCode = TransportModeId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_II.Id, Operator = "Equals", Value = "I", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField Direction_MOPII = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = DirectionId.Id, ObjectFieldCode = DirectionId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_II.Id, Operator = "Equals", Value = "I", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField IsOperationalClosed_MOPII = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = IsOperationalClosed.Id, ObjectFieldCode = IsOperationalClosed.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_II.Id, Operator = "Equals", Value = "True", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField ShipmentLevelCode_MOPII = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = ShipmentLevelCode.Id, ObjectFieldCode = ShipmentLevelCode.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_II.Id, Operator = "Equals", Value = "C", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
     

            ObjectTableRuleField Carrier_Field_II = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageCarrierId.Id, ObjectFieldCode = MainCarriageCarrierId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_II.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField ATA_Field_II = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageATA.Id, ObjectFieldCode = MainCarriageATA.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_II.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField FreightPP_Field_II = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = FreightPrepaidCollectId.Id, ObjectFieldCode = FreightPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_II.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField OtherPP_Field_II = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = OtherPrepaidCollectId.Id, ObjectFieldCode = OtherPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_II.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);


            ObjectTableRule Master_OpClosed_Req_IID = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "Master_OpClosed_Req_II_D",
                Name = "Master_OpClosed_Req_II_D",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "REQ",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "WAR",
                //Condition = "If(And([TransportModeId] = \"I\",[DirectionId]  =  \"I\",[IsOperationalClosed],Or([ShipmentLevelCode]= \"C\",[ShipmentLevelCode]= \"D\")),True,False)",
                //AdvancedCondition = true,
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField TransportModeId_MOPIID = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = TransportModeId.Id, ObjectFieldCode = TransportModeId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_IID.Id, Operator = "Equals", Value = "I", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField Direction_MOPIID = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = DirectionId.Id, ObjectFieldCode = DirectionId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_IID.Id, Operator = "Equals", Value = "I", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField IsOperationalClosed_MOPIID = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = IsOperationalClosed.Id, ObjectFieldCode = IsOperationalClosed.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_IID.Id, Operator = "Equals", Value = "True", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField ShipmentLevelCode_MOPIID = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = ShipmentLevelCode.Id, ObjectFieldCode = ShipmentLevelCode.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_IID.Id, Operator = "Equals", Value = "D", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);


            ObjectTableRuleField Carrier_Field_IID = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageCarrierId.Id, ObjectFieldCode = MainCarriageCarrierId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_IID.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField ATA_Field_IID = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageATA.Id, ObjectFieldCode = MainCarriageATA.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_IID.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField FreightPP_Field_IID = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = FreightPrepaidCollectId.Id, ObjectFieldCode = FreightPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_IID.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField OtherPP_Field_IID = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = OtherPrepaidCollectId.Id, ObjectFieldCode = OtherPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_IID.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
           
            
            #endregion

            #region Master_OpClosed_Req_IE
            ObjectTableRule Master_OpClosed_Req_IE = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "Master_OpClosed_Req_IE",
                Name = "Master_OpClosed_Req_IE",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "REQ",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "WAR",
                //Condition = "If(And([TransportModeId] = \"I\",[DirectionId]  =  \"E\",[IsOperationalClosed],Or([ShipmentLevelCode]= \"C\",[ShipmentLevelCode]= \"D\")),True,False)",
                //AdvancedCondition = true,
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField TransportModeId_MOPIE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = TransportModeId.Id, ObjectFieldCode = TransportModeId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_IE.Id, Operator = "Equals", Value = "I", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField Direction_MOPIE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = DirectionId.Id, ObjectFieldCode = DirectionId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_IE.Id, Operator = "Equals", Value = "E", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField IsOperationalClosed_MOPIE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = IsOperationalClosed.Id, ObjectFieldCode = IsOperationalClosed.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_IE.Id, Operator = "Equals", Value = "True", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField ShipmentLevelCode_MOPIE = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = ShipmentLevelCode.Id, ObjectFieldCode = ShipmentLevelCode.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_IE.Id, Operator = "Equals", Value = "C", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
     

            ObjectTableRuleField Carrier_Field_IE = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageCarrierId.Id, ObjectFieldCode = MainCarriageCarrierId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_IE.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField FreightPP_Field_IE = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = FreightPrepaidCollectId.Id, ObjectFieldCode = FreightPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_IE.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField OtherPP_Field_IE = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = OtherPrepaidCollectId.Id, ObjectFieldCode = OtherPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_IE.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);

            ObjectTableRule Master_OpClosed_Req_IE_D = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "Master_OpClosed_Req_IED",
                Name = "Master_OpClosed_Req_IED",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "REQ",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "WAR",
                //Condition = "If(And([TransportModeId] = \"I\",[DirectionId]  =  \"E\",[IsOperationalClosed],Or([ShipmentLevelCode]= \"C\",[ShipmentLevelCode]= \"D\")),True,False)",
                //AdvancedCondition = true,
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField TransportModeId_MOPIED = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = TransportModeId.Id, ObjectFieldCode = TransportModeId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_IE_D.Id, Operator = "Equals", Value = "I", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField Direction_MOPIED = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = DirectionId.Id, ObjectFieldCode = DirectionId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_IE_D.Id, Operator = "Equals", Value = "E", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField IsOperationalClosed_MOPIED = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = IsOperationalClosed.Id, ObjectFieldCode = IsOperationalClosed.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_IE_D.Id, Operator = "Equals", Value = "True", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField ShipmentLevelCode_MOPIED = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = ShipmentLevelCode.Id, ObjectFieldCode = ShipmentLevelCode.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_IE_D.Id, Operator = "Equals", Value = "D", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);


            ObjectTableRuleField Carrier_Field_IED = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageCarrierId.Id, ObjectFieldCode = MainCarriageCarrierId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_IE_D.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField FreightPP_Field_IED = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = FreightPrepaidCollectId.Id, ObjectFieldCode = FreightPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_IE_D.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField OtherPP_Field_IED = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = OtherPrepaidCollectId.Id, ObjectFieldCode = OtherPrepaidCollectId.FieldCode, ObjectTableRuleId = Master_OpClosed_Req_IE_D.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "ERR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
           
            
            #endregion

            #region Master_OpClosed_War_Connected Shipments
            ObjectTableRule Master_OpClosed_War_Connected = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "Master_OpClosed_War",
                Name = "Master_OpClosed_War",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "REQ",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "WAR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField IsOperationalClosed_OPWAR = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = IsOperationalClosed.Id, ObjectFieldCode = IsOperationalClosed.FieldCode, ObjectTableRuleId = Master_OpClosed_War_Connected.Id, Operator = "Equals", Value = "True", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);
            RuleConditionField ShipmentLevelCode_OPWAR = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = ShipmentLevelCode.Id, ObjectFieldCode = ShipmentLevelCode.FieldCode, ObjectTableRuleId = Master_OpClosed_War_Connected.Id, Operator = "Equals", Value = "C", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);

            ObjectTableRuleField ConnectedShipments_Field_WAR = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = ConnectedShipments.Id, ObjectFieldCode = ConnectedShipments.FieldCode, ObjectTableRuleId = Master_OpClosed_War_Connected.Id, SystemLevel = true, Tenant = 0, RuleNotificationTypeCode = "WAR" }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #region InActive Rules
            //ObjectTableRule OPClosedRequiredFieldsRule2 = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            //{
            //    RuleCode = "OPRQM",
            //    Name = "Operational Closed Req. (Master)",
            //    ObjectTableId = ShipmentTable.Id,
            //    Tenant = 0,
            //    RuleTypeCode = "REQ",
            //    Condition = "If(And([IsOperationalClosed],[TransportModeId] = \"A\"),True,False)",
            //    SystemLevel = true,
            //    ActiveForNew = false,
            //    ActiveForUpdate = true,
            //    TriggerTypeCode = "COND",
            //    RuleNotificationTypeCode = "ERR",
            //    InActive = true,
            //}, ObjectTableRuleRepository, TenantObjectTableRule);

            //ObjectTableRuleField MAWBOBLField2 = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MAWBOBL.Id, ObjectTableRuleId = OPClosedRequiredFieldsRule2.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);

            //ObjectTableRuleField MainCarriageCarrierIdField2 = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageCarrierId.Id, ObjectTableRuleId = OPClosedRequiredFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #endregion

            #region Incoterm_FieldChanged_Rule
            ObjectTableRule IncotermRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "INCO",
                Name = "Incoterm",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "SETV",
                TriggerFieldId = IncotermId.Id,
                TriggerFieldCode = IncotermId.FieldCode,
                SystemLevel = true,
                OutputMessage = "The HAWBFBLBL is used by another shipment",
                ActiveForNew = false,
                ActiveForUpdate = true,
                TriggerTypeCode = "FLDC",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField IncotermRule_Driection = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = DirectionId.Id, ObjectFieldCode = DirectionId.FieldCode, ObjectTableRuleId = IncotermRule.Id, Operator = "Equals", Value = "E", Tenant = 0 }, RuleConditionFieldRepository, TenantRuleConditionFields);

            ObjectTableRuleField FreightPrepaidCollectIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails()
            {
                ObjectFieldId = FreightPrepaidCollectId.Id,
                ObjectFieldCode = FreightPrepaidCollectId.FieldCode,
                ObjectTableRuleId = IncotermRule.Id,
                SystemLevel = true,
                Tenant = 0,
                Expression = "[IncotermId.Freight.Id]",
            }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);

            ObjectTableRuleField OtherPrepaidCollectIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails()
            {
                ObjectFieldId = OtherPrepaidCollectId.Id,
                ObjectFieldCode = OtherPrepaidCollectId.FieldCode,
                ObjectTableRuleId = IncotermRule.Id,
                SystemLevel = true,
                Tenant = 0,
                Expression = "[IncotermId.OtherCharges.Id]",
            }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #region AMSBL Rule
            ObjectTableRule AMSBLBlockRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "AMSBLBLCK",
                Name = "Block AMSBL Field",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "BLCK",
                Condition = "If(Or([TransportModeId] = \"A\", [TransportModeId] = \"I\" ),True,False)",
                SystemLevel = true,
                AdvancedCondition = true,
                ActiveForNew = true,
                ActiveForUpdate = false,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = AMSBL.Id, ObjectFieldCode = AMSBL.FieldCode, ObjectTableRuleId = AMSBLBlockRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #region SetCustomer_Rules

            #region Export Direction
            ObjectTableRule SetExportCustomer_Rule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "CUD1",
                Name = "Set Customer on Export",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "SETV",
                TriggerTypeCode = "COND",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = false,
                RuleNotificationTypeCode = "WAR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField SetExportCustomer_Condition = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails()
            {
                ObjectFieldId = DirectionId.Id,
                ObjectFieldCode = DirectionId.FieldCode,
                ObjectTableRuleId = SetExportCustomer_Rule.Id,
                Operator = "Equals",
                Value = "E",
                Tenant = 0
            }, RuleConditionFieldRepository, TenantRuleConditionFields);

            ObjectTableRuleField SetExportCustomer_Field = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails()
            {
                ObjectFieldId = ShipmentCustomerTypeCode.Id,
                ObjectFieldCode = ShipmentCustomerTypeCode.FieldCode,
                ObjectTableRuleId = SetExportCustomer_Rule.Id,
                SystemLevel = true,
                Tenant = 0,
                Expression = "SHI",
            }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #region Domestic Direction
            ObjectTableRule SetDomesticCustomer_Rule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "CUD2",
                Name = "Set Customer on Domestic",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "SETV",
                TriggerTypeCode = "COND",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = false,
                RuleNotificationTypeCode = "WAR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField SetDomesticCustomer_Condition = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails()
            {
                ObjectFieldId = DirectionId.Id,
                ObjectFieldCode = DirectionId.FieldCode,
                ObjectTableRuleId = SetDomesticCustomer_Rule.Id,
                Operator = "Equals",
                Value = "D",
                Tenant = 0
            }, RuleConditionFieldRepository, TenantRuleConditionFields);

            ObjectTableRuleField SetDomesticCustomer_Field = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails()
            {
                ObjectFieldId = ShipmentCustomerTypeCode.Id,
                ObjectFieldCode = ShipmentCustomerTypeCode.FieldCode,
                ObjectTableRuleId = SetDomesticCustomer_Rule.Id,
                SystemLevel = true,
                Tenant = 0,
                Expression = "SHI",
            }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #region Drop Direction
            ObjectTableRule SetDropCustomer_Rule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "CUD3",
                Name = "Set Customer on Drop",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "SETV",
                TriggerTypeCode = "COND",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = false,
                RuleNotificationTypeCode = "WAR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField SetDropCustomer_Condition = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails()
            {
                ObjectFieldId = DirectionId.Id,
                ObjectFieldCode = DirectionId.FieldCode,
                ObjectTableRuleId = SetDropCustomer_Rule.Id,
                Operator = "Equals",
                Value = "R",
                Tenant = 0
            }, RuleConditionFieldRepository, TenantRuleConditionFields);

            ObjectTableRuleField SetDropCustomer_Field = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails()
            {
                ObjectFieldId = ShipmentCustomerTypeCode.Id,
                ObjectFieldCode = ShipmentCustomerTypeCode.FieldCode,
                ObjectTableRuleId = SetDropCustomer_Rule.Id,
                SystemLevel = true,
                Tenant = 0,
                Expression = "SHI",
            }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #region Import Direction
            ObjectTableRule SetImportCustomer_Rule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "CUD4",
                Name = "Set Customer on Import",
                ObjectTableId = ShipmentTable.Id,
                Tenant = 0,
                RuleTypeCode = "SETV",
                TriggerTypeCode = "COND",
                SystemLevel = true,
                ActiveForNew = true,
                ActiveForUpdate = false,
                RuleNotificationTypeCode = "WAR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField SetImportCustomer_Condition = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails()
            {
                ObjectFieldId = DirectionId.Id,
                ObjectFieldCode = DirectionId.FieldCode,
                ObjectTableRuleId = SetImportCustomer_Rule.Id,
                Operator = "Equals",
                Value = "I",
                Tenant = 0
            }, RuleConditionFieldRepository, TenantRuleConditionFields);

            ObjectTableRuleField SetImportCustomer_Field = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails()
            {
                ObjectFieldId = ShipmentCustomerTypeCode.Id,
                ObjectFieldCode = ShipmentCustomerTypeCode.FieldCode,
                ObjectTableRuleId = SetImportCustomer_Rule.Id,
                SystemLevel = true,
                Tenant = 0,
                Expression = "CON",
            }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #endregion

            #endregion

            #region Testing

            //============= Just For Testing ============= 
            if (Testing.General.IsTesting)
            {
                ObjectTableRule Test_blockFieldsRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
                {
                    RuleCode = "TTTT",
                    Name = "Operational Closed",
                    ObjectTableId = ShipmentTable.Id,
                    Tenant = 0,
                    RuleTypeCode = "BLCK",
                    Condition = "If([IsOperationalClosed],True,False)",
                    SystemLevel = true,
                    ActiveForNew = false,
                    ActiveForUpdate = true,
                    TriggerTypeCode = "COND",
                    RuleNotificationTypeCode = "ERR",
                }, ObjectTableRuleRepository, TenantObjectTableRule);

                ObjectTableRuleField Test_ShipmentTypeIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = ShipmentTypeId.Id, ObjectFieldCode = ShipmentTypeId.FieldCode, ObjectTableRuleId = Test_blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);

                ObjectTableRule Update_blockFieldsRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
                {
                    RuleCode = "OPCL",
                    Name = "Test_Update_Operational Closed",
                    ObjectTableId = ShipmentTable.Id,
                    Tenant = 0,
                    RuleTypeCode = "BLCK",
                    Condition = "If([IsOperationalClosed],True,False)",
                    SystemLevel = true,
                    ActiveForNew = false,
                    ActiveForUpdate = true,
                    TriggerTypeCode = "COND",
                    RuleNotificationTypeCode = "ERR",
                }, ObjectTableRuleRepository, TenantObjectTableRule);
            }
            //============================================

            #endregion

            ObjectContext.SaveChanges();
        }

        private void CreateQuoteRules(Dictionary<string, ObjectTableRule> TenantObjectTableRule, Dictionary<string, ObjectTableRuleField> TenantObjectTableRuleFields, List<ObjectFieldValidation> TenantObjectFieldValidations, Dictionary<string, RuleConditionField> TenantRuleConditionFields)
        {
            ObjectTable QuoteTable = ObjectContext.ObjectTables.Where(f => f.Name == "Quote" && f.Tenant == 0).FirstOrDefault();

            ObjectField OpenedByUserId = ObjectContext.ObjectFields.Where(d => d.FieldName == "CreatedByUserId" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            ObjectField IsCancelled = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsCancelled" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();

            ObjectTableRule blockedFieldsUpdateRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "QuoteBlockFieldsUpdate",
                Name = "Block Fields Update",
                ObjectTableId = QuoteTable.Id,
                Tenant = 0,
                RuleTypeCode = "BLCK",
                SystemLevel = true,
                ActiveForNew = false,
                ActiveForUpdate = true,
                TriggerTypeCode = "ALLW",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            ObjectTableRuleField AllOpenedByUserIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = OpenedByUserId.Id, ObjectFieldCode = OpenedByUserId.FieldCode, ObjectTableRuleId = blockedFieldsUpdateRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);

            #region Block after cancel
            //ObjectTableRule blockFieldsRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            //{
            //    RuleCode = "BLCKCancel",
            //    Name = "Block Cancelled Fields",
            //    ObjectTableId = QuoteTable.Id,
            //    Tenant = 0,
            //    RuleTypeCode = "BLCK",
            //    SystemLevel = true,
            //    ActiveForNew = false,
            //    ActiveForUpdate = true,
            //    TriggerTypeCode = "COND",
            //    RuleNotificationTypeCode = "ERR",
            //}, ObjectTableRuleRepository, TenantObjectTableRule);

            //RuleConditionField IsCancelled_CondField = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = IsCancelled.Id, ObjectTableRuleId = blockFieldsRule.Id, Operator = "Equals", Value = "True", Tenant = blockFieldsRule.Tenant }, RuleConditionFieldRepository, TenantRuleConditionFields);

            #region ObjectFields
            //ObjectField FromPortId = ObjectContext.ObjectFields.Where(d => d.FieldName == "FromPortId" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField ToPortId = ObjectContext.ObjectFields.Where(d => d.FieldName == "ToPortId" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField MainCarriageCarrierId = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainCarriageCarrierId" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField IncludePickUp = ObjectContext.ObjectFields.Where(d => d.FieldName == "IncludePickUp" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField FromAddressId = ObjectContext.ObjectFields.Where(d => d.FieldName == "FromAddressId" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField IncludeDelivery = ObjectContext.ObjectFields.Where(d => d.FieldName == "IncludeDelivery" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField ToAddressId = ObjectContext.ObjectFields.Where(d => d.FieldName == "ToAddressId" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField IsAdhoc = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsAdhoc" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField IncotermId = ObjectContext.ObjectFields.Where(d => d.FieldName == "IncotermId" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField ExpirationDays = ObjectContext.ObjectFields.Where(d => d.FieldName == "ExpirationDays" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField ExpirationDate = ObjectContext.ObjectFields.Where(d => d.FieldName == "ExpirationDate" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();

            //ObjectField RateClassCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "RateClassCode" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField NumberOfPackages = ObjectContext.ObjectFields.Where(d => d.FieldName == "NumberOfPackages" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField DescriptionOfGoods = ObjectContext.ObjectFields.Where(d => d.FieldName == "DescriptionOfGoods" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField GrossWeight = ObjectContext.ObjectFields.Where(d => d.FieldName == "GrossWeight" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField VolumetricWeight = ObjectContext.ObjectFields.Where(d => d.FieldName == "VolumetricWeight" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField Volume = ObjectContext.ObjectFields.Where(d => d.FieldName == "Volume" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField ChargeableWeight = ObjectContext.ObjectFields.Where(d => d.FieldName == "ChargeableWeight" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField AWBFreightPrepaid = ObjectContext.ObjectFields.Where(d => d.FieldName == "AWBFreightAmountPrepaid" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField AWBFreightCollect = ObjectContext.ObjectFields.Where(d => d.FieldName == "AWBFreightAmountCollect" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField DangerousClassNumber = ObjectContext.ObjectFields.Where(d => d.FieldName == "DangerousClassNumber" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField DangerousUnNumber = ObjectContext.ObjectFields.Where(d => d.FieldName == "DangerousUnNumber" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField DangerousPackagingGroup = ObjectContext.ObjectFields.Where(d => d.FieldName == "DangerousPackagingGroup" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField DangerousFlashPoint = ObjectContext.ObjectFields.Where(d => d.FieldName == "DangerousFlashPoint" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField DangerousMaterialDescription = ObjectContext.ObjectFields.Where(d => d.FieldName == "DangerousMaterialDescription" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField MainHarmonize = ObjectContext.ObjectFields.Where(d => d.FieldName == "MainHarmonize" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField NumberOfContainers = ObjectContext.ObjectFields.Where(d => d.FieldName == "NumberOfContainers" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField DirectionId = ObjectContext.ObjectFields.Where(d => d.FieldName == "DirectionId" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField IncotermId = ObjectContext.ObjectFields.Where(d => d.FieldName == "IncotermId" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField FreightPrepaidCollectId = ObjectContext.ObjectFields.Where(d => d.FieldName == "FreightPrepaidCollectId" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField OtherPrepaidCollectId = ObjectContext.ObjectFields.Where(d => d.FieldName == "OtherPrepaidCollectId" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField SalesManUserId = ObjectContext.ObjectFields.Where(d => d.FieldName == "SalesmanUserId" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField DepartmentId = ObjectContext.ObjectFields.Where(d => d.FieldName == "DepartmentId" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField BranchId = ObjectContext.ObjectFields.Where(d => d.FieldName == "BranchId" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField StatusId = ObjectContext.ObjectFields.Where(d => d.FieldName == "StatusId" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField IsOperationalClosed = ObjectContext.ObjectFields.Where(d => d.FieldName == "IsOperationalClosed" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            //ObjectField ShipmentLevelCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "ShipmentLevelCode" && d.ObjectTableId == QuoteTable.Id).FirstOrDefault();
            #endregion

            #region blockFieldsRule RuleFields
            //ObjectTableRuleField ShipmentTypeIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = ShipmentTypeId.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField HAWBDateField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = HAWBDate.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField MainCarriageCarrierIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageCarrierId.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField MAWBOBLField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MAWBOBL.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField MAWBOBLDateField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MAWBOBLDate.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField HAWBFBLBLField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = HAWBFBLBL.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField StatusIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = StatusId.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField IncotermIdField1 = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = IncotermId.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField FreightPrepaidCollectIdField1 = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = FreightPrepaidCollectId.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField OtherPrepaidCollectIdField1 = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = OtherPrepaidCollectId.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField SalesManUserIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = SalesManUserId.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField OpenedByUserIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = OpenedByUserId.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField DepartmentIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = DepartmentId.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField BranchIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = BranchId.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField MainCarriageCarrierNumberField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageCarrierNumber.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField RateClassCodeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = RateClassCode.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField NumberOfPackagesField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = NumberOfPackages.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField DescriptionOfGoodsField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = DescriptionOfGoods.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField GrossWeightField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = GrossWeight.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField VolumetricWeightField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = VolumetricWeight.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField VolumeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = Volume.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField ChargeableWeightField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = ChargeableWeight.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField AWBFreightPrepaidField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = AWBFreightPrepaid.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField AWBFreightCollectField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = AWBFreightCollect.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField DangerousClassNumberField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = DangerousClassNumber.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField DangerousUnNumberField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = DangerousUnNumber.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField DangerousPackagingGroupField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = DangerousPackagingGroup.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField DangerousFlashPointField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = DangerousFlashPoint.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField DangerousMaterialDescriptionField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = DangerousMaterialDescription.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField MainHarmonizeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainHarmonize.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField NumberOfContainersField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = NumberOfContainers.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField MainCarriageFromPortIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageFromPortId.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            //ObjectTableRuleField MainCarriageToPortIdField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = MainCarriageToPortId.Id, ObjectTableRuleId = blockFieldsRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            #endregion

            #endregion

            ObjectContext.SaveChanges();
        }

        private void CreateAccountingRules(Dictionary<string, ObjectTableRule> TenantObjectTableRule, Dictionary<string, ObjectTableRuleField> TenantObjectTableRuleFields, List<ObjectFieldValidation> TenantObjectFieldValidations, Dictionary<string, RuleConditionField> TenantRuleConditionFields)
        {
            ObjectTable ARInvoiceTable = ObjectContext.ObjectTables.Where(f => f.Name == "ARInvoice" && f.Tenant == 0).FirstOrDefault();
            ObjectField ARInvoicePrintNotes = ObjectContext.ObjectFields.Where(d => d.FieldName == "PrintNotes" && d.ObjectTableId == ARInvoiceTable.Id).FirstOrDefault();

            ObjectTableRule blockARInvoicePrintNotesRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "ARInvoice_BlockPrintNotes",
                Name = "ARInvoice Block Print Notes",
                ObjectTableId = ARInvoiceTable.Id,
                Tenant = 0,
                RuleTypeCode = "BLCK",
                SystemLevel = true,
                ActiveForNew = false,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                Condition = "If(Or([StatusCode] = \"PR\", [StatusCode] = \"VD\" ),True,False)",
                RuleNotificationTypeCode = "ERR",
                AdvancedCondition = true,
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            ObjectTableRuleField ARInvoicePrintNotesField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = ARInvoicePrintNotes.Id, ObjectFieldCode = ARInvoicePrintNotes.FieldCode, ObjectTableRuleId = blockARInvoicePrintNotesRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectContext.SaveChanges();
        }

        private void CreateObjectFieldValidations(List<ObjectFieldValidation> TenantObjectFieldValidations)
        {
            ObjectTable MasterTable = ObjectContext.ObjectTables.Where(f => f.Name == "Master" && f.Tenant == 0).FirstOrDefault();
            ObjectTable ShipmentTable = ObjectContext.ObjectTables.Where(f => f.Name == "Shipment" && f.Tenant == 0).FirstOrDefault();
            ObjectField MasterMAWBOBL = ObjectContext.ObjectFields.Where(d => d.FieldName == "Master" && d.ObjectTableId == MasterTable.Id).FirstOrDefault();
            ObjectField ShipmentMAWBOBL = ObjectContext.ObjectFields.Where(d => d.FieldName == "Master" && d.ObjectTableId == ShipmentTable.Id).FirstOrDefault();

            #region Shipment ObjectFieldValidations
            DeleteObjectFieldValidations.DeleteObjectFieldValidation(TenantObjectFieldValidations, ObjectFieldValidationRepository);

            ObjectFieldValidation MAWBFieldLengthValidation = new ObjectFieldValidation()
            {
                Id = IdCounter.GetNumber("ObjectFieldValidation", 0).ToString(),
                Tenant = 0,
                ObjectFieldId = ShipmentMAWBOBL.Id,
                ObjectFieldCode = ShipmentMAWBOBL.FieldCode,
                ValidationOrder = 0,
                Code = "MAFL",
                ValidationExpression = "If(Or(Len(value) = 8,[TransportModeId] <> \"A\" ),True,False)",
                ErrorMessage = "MAWB number length must be 8 digits",


            };
            if (Testing.General.IsTesting)
            {
                //============= Just For Testing ============= 
                ObjectFieldValidation TSTMAWBFieldLengthValidation = new ObjectFieldValidation()
                {
                    Id = IdCounter.GetNumber("ObjectFieldValidation", 0).ToString(),
                    Tenant = 0,
                    ObjectFieldId = ShipmentMAWBOBL.Id,
                    ObjectFieldCode = ShipmentMAWBOBL.FieldCode,
                    ValidationOrder = 0,
                    Code = "TMAF",
                    ValidationExpression = "1",
                    ErrorMessage = "MAWB number length must be 8 digits",


                };
                ObjectFieldValidationRepository.Add(TSTMAWBFieldLengthValidation);
            }
            //============================================


            ObjectFieldValidation MAWBFieldCheckDigitValidation = new ObjectFieldValidation()
            {
                Id = IdCounter.GetNumber("ObjectFieldValidation", 0).ToString(),
                Tenant = 0,
                ObjectFieldId = ShipmentMAWBOBL.Id,
                ObjectFieldCode = ShipmentMAWBOBL.FieldCode,
                ValidationOrder = 1,
                Condition = "If(And([TransportModeId] = \"A\",Len(value) = 8),True,False)",
                ValidationExpression = "If(Or([TransportModeId] <> \"A\",Mod(Mid(value,1,7),7)= Int(Mid(value,8,1))),True,False)",
                ErrorMessage = "MAWB check digit is not valid",
                Code = "MACD",

            };

            ObjectFieldValidationRepository.Add(MAWBFieldLengthValidation);
            ObjectFieldValidationRepository.Add(MAWBFieldCheckDigitValidation);




            #endregion

            #region Master ObjectFieldValidations


            ObjectFieldValidation MasterMAWBFieldLengthValidation = new ObjectFieldValidation()
            {
                Id = IdCounter.GetNumber("ObjectFieldValidation", 0).ToString(),
                Tenant = 0,
                ObjectFieldId = MasterMAWBOBL.Id,
                ObjectFieldCode = MasterMAWBOBL.FieldCode,
                ValidationOrder = 0,
                Code = "JMAF",
                ValidationExpression = "If(Or(Len(value) = 8,[TransportModeId] <> \"A\" ),True,False)",
                ErrorMessage = "MAWB number length must be 8 digits",


            };
            if (Testing.General.IsTesting)
            {
                //============= Just For Testing ============= 
                ObjectFieldValidation TSTMAWBFieldLengthValidation = new ObjectFieldValidation()
                {
                    Id = IdCounter.GetNumber("ObjectFieldValidation", 0).ToString(),
                    Tenant = 0,
                    ObjectFieldId = MasterMAWBOBL.Id,
                    ObjectFieldCode = MasterMAWBOBL.FieldCode,
                    ValidationOrder = 0,
                    Code = "JTMA",
                    ValidationExpression = "1",
                    ErrorMessage = "MAWB number length must be 8 digits",


                };
                ObjectFieldValidationRepository.Add(TSTMAWBFieldLengthValidation);
            }
            //============================================


            ObjectFieldValidation MasterMAWBFieldCheckDigitValidation = new ObjectFieldValidation()
            {
                Id = IdCounter.GetNumber("ObjectFieldValidation", 0).ToString(),
                Tenant = 0,
                ObjectFieldId = MasterMAWBOBL.Id,
                ObjectFieldCode = MasterMAWBOBL.FieldCode,
                ValidationOrder = 1,
                Condition = "If(Len(value) = 8,True,False)",
                ValidationExpression = "If(Or([TransportModeId] <> \"A\",Mod(Mid(value,1,7),7)= Int(Mid(value,8,1))),True,False)",
                ErrorMessage = "MAWB check digit is not valid",
                Code = "JMAC",

            };

            ObjectFieldValidationRepository.Add(MasterMAWBFieldLengthValidation);
            ObjectFieldValidationRepository.Add(MasterMAWBFieldCheckDigitValidation);




            #endregion

            ObjectContext.SaveChanges();
        }

        private void CreatelosingReasonRules(Dictionary<string, ObjectTableRule> TenantObjectTableRule, Dictionary<string, ObjectTableRuleField> TenantObjectTableRuleFields, List<ObjectFieldValidation> TenantObjectFieldValidations, Dictionary<string, RuleConditionField> TenantRuleConditionFields)
        {
            ObjectTable closingReasonTable = ObjectContext.ObjectTables.Where(f => f.Name == "OpportunityClosingReason" && f.Tenant == 0).FirstOrDefault();

            ObjectField addedManuallyField = ObjectContext.ObjectFields.Where(d => d.FieldName == "AddedManually" && d.ObjectTableId == closingReasonTable.Id).FirstOrDefault();
            ObjectField closingReasonNameField = ObjectContext.ObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == closingReasonTable.Id).FirstOrDefault();
            ObjectField closingReasonLocalNameField = ObjectContext.ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == closingReasonTable.Id).FirstOrDefault();

            ObjectTableRule blockNamesRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            {
                RuleCode = "Closing_BlockNames",
                Name = "Block Closing Reason Names",
                ObjectTableId = closingReasonTable.Id,
                Tenant = 0,
                RuleTypeCode = "BLCK",
                SystemLevel = true,
                ActiveForNew = false,
                ActiveForUpdate = true,
                TriggerTypeCode = "COND",
                RuleNotificationTypeCode = "ERR",
            }, ObjectTableRuleRepository, TenantObjectTableRule);

            RuleConditionField IsBlocked_CondField = AddObjectTableRules.AddRuleConditionField(new RuleConditionFieldDetails() { ObjectFieldId = addedManuallyField.Id, ObjectFieldCode = addedManuallyField.FieldCode, ObjectTableRuleId = blockNamesRule.Id, Operator = "Equals", Value = "False", Tenant = blockNamesRule.Tenant }, RuleConditionFieldRepository, TenantRuleConditionFields);

            ObjectTableRuleField ClosingReasonNameRuleField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = closingReasonNameField.Id, ObjectFieldCode = closingReasonNameField.FieldCode, ObjectTableRuleId = blockNamesRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);
            ObjectTableRuleField ClosingReasonLocalNameRuleField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = closingReasonLocalNameField.Id, ObjectFieldCode = closingReasonLocalNameField.FieldCode, ObjectTableRuleId = blockNamesRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);

            ObjectContext.SaveChanges();
        }
    }
}