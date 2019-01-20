using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using SilverlightExpressions;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
 
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel;
 
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
 
using Logitude.BL.ShipmentsModel;
 
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Server.Tools.Helpers;
using WebFreight.Web.ShipmentsModel.DomainServices;
using WebFreight.Web.QuoteModel.DomainServices;
using WebFreight.Web.InvoiceModel.DomainServices;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.CommonDataModel.DomainServices;
using WebFreight.Web.InfrastructureModel;
using Logitude.BL.Interfaces;


namespace WebFreight.Web.Validators
{

    public class RulesValidator : IRulesValidator
    {
        Dictionary<string, object> definedObjects = new Dictionary<string, object>();

        private GeneralDomainService generalService;
        private CommonDataDomainService commonDataDomainService;
        public WebFreightDomainService webFreightDomainService;
        private QuotesDomainService quotesDomainService;
        private ShipmentsDomainService shipmentsDomainService;
        private InvoiceDomainService invoiceDomainService;

        private List<ObjectTableRule> blockRules = new List<ObjectTableRule>();
        private List<ObjectTableRule> entityLevelRules = new List<ObjectTableRule>();
        private List<ObjectTableRule> requiredFieldRules = new List<ObjectTableRule>();
        private List<ObjectTableRule> setFieldValueRules = new List<ObjectTableRule>();
        private List<ObjectTableRule> triggeredSetFieldValueRules = new List<ObjectTableRule>();
        private List<ObjectTableRule> duplicationRules = new List<ObjectTableRule>();
        private List<ObjectTableRule> tenantRules = new List<ObjectTableRule>();

        public void Initialize(int tenant)
        {
            tenantRules = ObjectTableRuleRepository.GetObjectTableRulesByTenant(tenant).ToList();
            if (tenant == 188)
            {
                tenantRules = tenantRules.Where(r => !r.Name.Contains("Shipment_OpClosed_") && !r.Name.Contains("Master_OpClosed_")).ToList();
            }

            BlockRules = tenantRules.Where(r => r.RuleTypeCode == "BLCK" && r.InActive == false).ToList();
            entityLevelRules = tenantRules.Where(r => r.RuleTypeCode == "EVAL" && r.InActive == false).ToList();
            requiredFieldRules = tenantRules.Where(r => r.RuleTypeCode == "REQ" && r.InActive == false).ToList();
            SetFieldValueRules = tenantRules.Where(r => r.RuleTypeCode == "SETV" && r.InActive == false).ToList();
            TriggeredSetFieldValueRules = tenantRules.Where(r => r.RuleTypeCode == "SETV" && r.TriggerTypeCode == "FLDC" && r.InActive == false).ToList();
            duplicationRules = tenantRules.Where(r => r.RuleTypeCode == "DUPL" && r.InActive == false).ToList();
        }
        //public RulesValidator(int tenant)
        //{
        //    tenantRules = ObjectTableRuleRepository.GetObjectTableRulesByTenant(tenant).ToList();
        //    if (tenant == 188)
        //    {
        //        tenantRules = tenantRules.Where(r => !r.Name.Contains("Shipment_OpClosed_") && !r.Name.Contains("Master_OpClosed_")).ToList();
        //    }

        //    BlockRules = tenantRules.Where(r => r.RuleTypeCode == "BLCK" && r.InActive == false).ToList();
        //    entityLevelRules = tenantRules.Where(r => r.RuleTypeCode == "EVAL" && r.InActive == false).ToList();
        //    requiredFieldRules = tenantRules.Where(r => r.RuleTypeCode == "REQ" && r.InActive == false).ToList();
        //    SetFieldValueRules = tenantRules.Where(r => r.RuleTypeCode == "SETV" && r.InActive == false).ToList();
        //    TriggeredSetFieldValueRules = tenantRules.Where(r => r.RuleTypeCode == "SETV" && r.TriggerTypeCode == "FLDC" && r.InActive == false).ToList();
        //    duplicationRules = tenantRules.Where(r => r.RuleTypeCode == "DUPL" && r.InActive == false).ToList();
        //}

        public List<ObjectTableRule> BlockRules
        {
            get { return blockRules; }
            set { blockRules = value; }
        }

        public List<ObjectTableRule> SetFieldValueRules
        {
            get { return setFieldValueRules; }
            set { setFieldValueRules = value; }
        }

        public List<ObjectTableRule> TriggeredSetFieldValueRules
        {
            get { return triggeredSetFieldValueRules; }
            set { triggeredSetFieldValueRules = value; }
        }

        #region ValidateRequiredFieldRules

        public List<ObjectTableRuleField> ValidateAllRequiredFieldRules(object entity, string objectTableName, int tenant)
        {
            List<ObjectTableRuleField> requiredFields = new List<ObjectTableRuleField>();

            ObjectTablePM table = ObjectTableQuery.GetObjectTableByCode(objectTableName, tenant);
            if (table != null)
            {
                List<ObjectTableRule> tableRules = (from a in requiredFieldRules
                                                    where a.ObjectTableId == table.Id && (a.Tenant == tenant || a.Tenant == 0)
                                                    select a).ToList();

                foreach (ObjectTableRule rule in tableRules)
                {
                    ValidateRequiedFieldRule(rule.RuleCode, entity, requiredFields, tenant, objectTableName);
                }
            }

            return requiredFields;
        }

        private void ValidateRequiedFieldRule(string ruleCode, object entity, List<ObjectTableRuleField> requiredFields, int tenant, string objectTableName)
        {
            ObjectTableRule rule = (from a in requiredFieldRules
                                    where a.RuleCode == ruleCode && (a.Tenant == tenant || a.Tenant == 0) && a.InActive == false
                                    select a).FirstOrDefault();

            if (rule != null)
            {
                List<ObjectTableRuleField> ruleFields = GetRuleFields(rule, tenant);
                List<RuleConditionField> RuleConditionFields = RuleConditionFieldRepository.GetObjectRuleConditionFieldsByTenant(tenant).Where(f=>f.ObjectTableRuleId == rule.Id).ToList();

                if (rule.AdvancedCondition && rule.Condition != null)
                {
                    bool required = ValidateConditionExpression(rule.Condition, entity, objectTableName, tenant);

                    if (required)
                    {
                        GenerateRuleErrors(ruleFields, entity, requiredFields, tenant, objectTableName);
                    }
                }

                if (rule.AdvancedCondition == false && RuleConditionFields.Count > 0)
                {
                    bool required = ValidateConditionFieldsRule(entity, RuleConditionFields.ToList(), tenant, objectTableName);

                    if (required)
                    {
                        GenerateRuleErrors(ruleFields, entity, requiredFields, tenant, objectTableName);

                    }
                }
            }
        }

        #endregion

        #region GenerateRuleErrors

        private void GenerateRuleErrors(List<ObjectTableRuleField> ruleFields, object entity, List<ObjectTableRuleField> requiredFields, int tenant, string objectTableName)
        {
            Type type1 = entity.GetType();
            PropertyInfo propertyInf = null;
            object propertyValue = null;

            List<ObjectField> objectFieldList = GetObjectFieldsList(objectTableName, tenant);

            foreach (ObjectTableRuleField ruleField in ruleFields)
            {
                ObjectField objectField = (from a in objectFieldList
                                           where a.Id == ruleField.ObjectFieldId && (a.Tenant == tenant || a.Tenant == 0)
                                           select a).FirstOrDefault();
                if (ruleField.RuleNotificationTypeCode == "ERR")
                {
                    propertyInf = type1.GetProperty(objectField.FieldName);
                    if (propertyInf != null)
                    {
                        propertyValue = propertyInf.GetValue(entity, null);
                        if (objectField.DataTypeCode == "Text")
                        {
                            if (propertyValue == null)
                            {
                                if (!requiredFields.Contains(ruleField))
                                {
                                    requiredFields.Add(ruleField);
                                }

                            }
                            else
                            {
                                if (String.IsNullOrEmpty(propertyValue.ToString().Trim()))
                                {
                                    if (!requiredFields.Contains(ruleField))
                                    {
                                        requiredFields.Add(ruleField);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (propertyValue == null)
                            {
                                if (!requiredFields.Contains(ruleField))
                                {
                                    requiredFields.Add(ruleField);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region GetRuleFields

        private List<ObjectTableRuleField> GetRuleFields(ObjectTableRule rule, int tenant)
        {
            List<ObjectTableRuleField> ruleFields;
            ruleFields = ObjectTableRuleFieldRepository.GetTenantRuleFields(tenant).Where(f => f.ObjectTableRuleId == rule.Id).ToList();

            return ruleFields;
        }

        #endregion

        #region ValidateEntityRules

        public bool ValidateEntityRules(object entity, string objectTableName, int tenant, ref string outputMessage)
        {
            string theOutputMessage = "";
            Dictionary<string, object> conditionFieldsDic = new Dictionary<string, object>();
            bool isValid = true;
            ExpressionValidation expressionValidation = null;

            ObjectTablePM table = ObjectTableQuery.GetObjectTableByCode(objectTableName, tenant);
            if (table != null)
            {
                List<ObjectTableRule> tableRules = (from a in entityLevelRules
                                                    where a.ObjectTableId == table.Id && (a.Tenant == tenant || a.Tenant == 0)
                                                    select a).ToList();

                foreach (ObjectTableRule rule in tableRules)
                {
                    if (expressionValidation == null)
                    {
                        expressionValidation = new ExpressionValidation();
                    }

                    List<RuleConditionField> RuleConditionFields = RuleConditionFieldRepository.GetObjectRuleConditionFieldsByTenant(tenant).ToList(); ;

                    bool haserror = false;
                    if (rule.AdvancedCondition && rule.Condition != null)
                    {
                        haserror = ValidateConditionExpression(rule.Condition, entity, objectTableName, tenant);
                    }

                    if (rule.AdvancedCondition == false && RuleConditionFields.Count > 0)
                    {
                        haserror = ValidateConditionFieldsRule(entity, RuleConditionFields.ToList(), tenant, objectTableName);
                    }

                    if (haserror)
                    {
                        if (!string.IsNullOrEmpty(rule.OutputMessage))
                        {
                            theOutputMessage = rule.OutputMessage + ",";
                        }

                        isValid = false;

                        break;
                    }
                }
            }
            if (isValid)
            {
                outputMessage = "";
                return true;
            }
            else
            {
                outputMessage = theOutputMessage;
                return false;
            }
        }

        #endregion

        #region ApplyDuplicationRules

        public bool ApplyDuplicationRules(object entity, string objectTableName, int tenant, ref string outputMessage)
        {
            bool isValid = true;
            ObjectTablePM table = ObjectTableQuery.GetObjectTableByCode(objectTableName, tenant);
            if (table == null)
            {
                return true;
            }

            string objectTableId = table.Id;
            Dictionary<string, object> conditionFieldsDic = new Dictionary<string, object>();
            List<ObjectTableRule> entityTableRules = duplicationRules.Where(r => r.ObjectTableId == objectTableId && (r.Tenant == tenant || r.Tenant == 0) && r.RuleTypeCode == "DUPL").ToList();

            if (entityTableRules.Count > 0)
            {
                foreach (ObjectTableRule rule in entityTableRules)
                {
                    List<ObjectTableRuleField> ruleFields = GetRuleFields(rule, tenant);
                    List<RuleConditionField> RuleConditionFields = RuleConditionFieldRepository.GetObjectRuleConditionFieldsByTenant(tenant).ToList(); ;
                    bool enableRun = true;
                    if (rule.TriggerTypeCode == "COND")
                    {
                        if (rule.AdvancedCondition && rule.Condition != null)
                        {
                            enableRun = ValidateConditionExpression(rule.Condition, entity, objectTableName, tenant);
                        }

                        if (rule.AdvancedCondition == false && RuleConditionFields.Count > 0)
                        {
                            enableRun = ValidateConditionFieldsRule(entity, RuleConditionFields.ToList(), tenant, objectTableName);
                        }
                    }
                    if (enableRun)
                    {
                        string methodName = "Get" + objectTableName + "FiltersCount";

                        object context = null;
                        if (commonDataDomainService == null)
                        {
                            commonDataDomainService = new CommonDataDomainService(CommonDataContext.GetContext(tenant));
                        }

                        MethodInfo insideMethodInfo = commonDataDomainService.GetType().GetMethod(methodName);

                        if (insideMethodInfo != null)
                        {
                            context = commonDataDomainService;
                        }

                        if (generalService == null)
                        {
                            generalService = new GeneralDomainService(WebFreightContext.GetContext(tenant));
                        }

                        if (insideMethodInfo == null)
                        {
                            insideMethodInfo = generalService.GetType().GetMethod(methodName);

                            if (insideMethodInfo != null)
                            {
                                context = generalService;
                            }
                        }

                        if (webFreightDomainService == null)
                        {
                            webFreightDomainService = new WebFreightDomainService(WebFreightContext.GetContext(tenant));
                        }

                        if (insideMethodInfo == null)
                        {
                            insideMethodInfo = webFreightDomainService.GetType().GetMethod(methodName);

                            if (insideMethodInfo != null)
                            {
                                context = webFreightDomainService;
                            }
                        }

                        if (quotesDomainService == null)
                        {
                            quotesDomainService = new QuotesDomainService();
                        }

                        if (insideMethodInfo == null)
                        {
                            insideMethodInfo = quotesDomainService.GetType().GetMethod(methodName);

                            if (insideMethodInfo != null)
                            {
                                context = quotesDomainService;
                            }
                        }

                        if (shipmentsDomainService == null)
                        {
                            shipmentsDomainService = new ShipmentsDomainService(ShipmentsContext.GetContext(tenant));
                        }
                        if (insideMethodInfo == null)
                        {
                            insideMethodInfo = shipmentsDomainService.GetType().GetMethod(methodName);

                            if (insideMethodInfo != null)
                            {
                                context = shipmentsDomainService;
                            }
                        }

                        if (invoiceDomainService == null)
                        {
                            invoiceDomainService = new InvoiceDomainService();
                        }
                        if (insideMethodInfo == null)
                        {
                            insideMethodInfo = invoiceDomainService.GetType().GetMethod(methodName);

                            if (insideMethodInfo != null)
                            {
                                context = invoiceDomainService;
                            }
                        }

                        if (insideMethodInfo != null)
                        {
                            QueryOperations queryOperations = new QueryOperations();
                            bool jump = false;
                            foreach (ObjectTableRuleField ruleField in ruleFields)
                            {
                                object value = GetObjectFieldValue(ruleField.ObjectField.FieldName, objectTableId, entity, tenant);
                                if (value == null)
                                {
                                    jump = true;
                                    isValid = true;
                                    break;
                                }

                                queryOperations.SetFilter(ruleField.ObjectField.FieldName, value, ruleField.ObjectField.IsCustom, "Equals", null, ruleField.ObjectField.DisplayInList);
                            }
                            if (jump)
                            {
                                continue;
                            }

                            byte[] arrayOfBytes = SerializeFilterItems(queryOperations);
                            object[] parameters = new object[] { arrayOfBytes, tenant };

                            int count = Convert.ToInt32(insideMethodInfo.Invoke(context, parameters));
                            if (count == 0)
                            {
                                isValid = true;
                            }
                            else
                            {
                                if (count == 1)
                                {
                                    PropertyInfo idInfo = entity.GetType().GetProperty("Id");
                                    object idValue = idInfo.GetValue(entity, null);

                                    if (idValue == null) // new entity
                                    {
                                        outputMessage = rule.OutputMessage;
                                        return false;
                                    }

                                    queryOperations.SetFilter("Id", idValue, false, "Equals", null, false);
                                    arrayOfBytes = SerializeFilterItems(queryOperations);
                                    parameters = new object[] { arrayOfBytes, tenant };
                                    count = Convert.ToInt32(insideMethodInfo.Invoke(context, parameters));
                                    if (count == 1)
                                    {
                                        isValid = true;
                                    }
                                    else
                                    {
                                        outputMessage = rule.OutputMessage;
                                        return false;
                                    }
                                }
                                else
                                {
                                    outputMessage = rule.OutputMessage;
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            isValid = true;
                        }
                    }
                    else
                    {
                        isValid = true;
                    }
                }
            }
            else
            {
                isValid = true;
            }

            return isValid;
        }

        #endregion

        #region CheckDuplication

        private object CheckDuplication(string insideEntityName, int tenant, object entity, string objectTableId)
        {
            PropertyInfo idInfo = entity.GetType().GetProperty("Id");
            object value = idInfo.GetValue(entity, null);
            object result = null;
            string insideTypePath = "Logitude.BL.ShipmentsModel.Repositories." + insideEntityName + "Repository";

            Type insideEntityType = Type.GetType(insideTypePath);
            if (insideEntityType == null)
            {
                insideTypePath = "Logitude.BL.CommonDataModel.Repositories." + insideEntityName + "Repository";
                insideEntityType = Type.GetType(insideTypePath);
            }

            if (insideEntityType == null)
            {
                insideTypePath = "Logitude.BL." + insideEntityName + "Repository";
                insideEntityType = Type.GetType(insideTypePath);
            }

            if (insideEntityType == null)
            {
                insideTypePath = "Logitude.BL.InfrastructureModel.Repositories." + insideEntityName + "Repository";
                insideEntityType = Type.GetType(insideTypePath);
            }

            if (insideEntityType == null)
            {
                insideTypePath = "Logitude.BL.QuoteModel.Repositories." + insideEntityName + "Repository";
                insideEntityType = Type.GetType(insideTypePath);
            }

            object insideEntityRepository = null;
            if (insideEntityType != null)
            {
                if (definedObjects.Keys.Contains(insideTypePath))
                {
                    insideEntityRepository = definedObjects[insideTypePath];
                }

                if (insideEntityRepository == null)
                {
                    insideEntityRepository = Activator.CreateInstance(insideEntityType, tenant);///??????
                    ///
                    definedObjects.Add(insideTypePath, insideEntityRepository);
                }

                MethodInfo insideMethodInfo = insideEntityRepository.GetType().GetMethod("GetSinglePM");
                object insideEntity = null;
                if (insideMethodInfo != null)
                {
                    ParameterInfo[] parametersInfo = insideMethodInfo.GetParameters();
                    object[] parameters = new object[] { };
                    switch (parametersInfo.Count())
                    {
                        case 1:
                            parameters = new object[] { value };
                            break;
                        case 2:
                            parameters = new object[] { value, tenant };
                            break;
                        case 3:
                            parameters = new object[] { value, tenant, false };
                            break;
                        default:
                            parameters = new object[] { value, tenant };
                            break;
                    }

                    insideEntity = insideMethodInfo.Invoke(insideEntityRepository, parameters);
                    if (insideEntity != null)
                    {
                        result = insideEntity;
                    }
                }
            }

            return result;
        }

        #endregion

        #region ValidateConditionExpression

        public bool ValidateConditionExpression(string condition, object entity, string objectTableName, int tenant)
        {
            bool validcondition = false;
            Type type1;
            type1 = entity.GetType();
            PropertyInfo propertyInf = null;
            object propertyValue = null;
            Dictionary<string, object> conditionFieldsDic = new Dictionary<string, object>();

            string[] fieldNames = { };
            string ruleCondition = condition;
            string[] level1 = ruleCondition.Split('[');

            foreach (string s in level1)
            {
                if (s.Contains("]"))
                {
                    string[] level2 = s.Split(']');
                    string fieldName = level2[0].Trim();
                    string key = fieldName.Replace(".", "") + System.Guid.NewGuid().ToString("N");

                    if (!fieldName.Contains("."))
                    {
                        ruleCondition = ruleCondition.Replace("[" + fieldName + "]", key);

                        propertyInf = type1.GetProperty(fieldName);
                        if (propertyInf != null)
                        {
                            propertyValue = propertyInf.GetValue(entity, null);
                            propertyValue = (propertyValue != null ? propertyValue.ToString() : null);
                            conditionFieldsDic.Add(key, propertyValue);
                        }
                    }

                    else
                    {
                        ruleCondition = ruleCondition.Replace("[" + fieldName + "]", key);
                        propertyValue = GetObjectFieldValue(fieldName, objectTableName, entity, tenant);
                        conditionFieldsDic.Add(key, propertyValue);
                    }
                }
            }

            if (conditionFieldsDic.Count != 0)
            {
                ExpressionValidation expressionValidation = new ExpressionValidation();
                validcondition = expressionValidation.ExecuteExpression(ruleCondition, conditionFieldsDic);
            }

            return validcondition;
        }

        #endregion

        #region ValidateConditionFieldsRule

        public bool ValidateConditionFieldsRule(object entity, List<RuleConditionField> ruleConditionFields, int tenant, string objectTableName)
        {
            bool validcondition = true;
            Type type1 = entity.GetType();
            PropertyInfo propertyInf = null;

            foreach (RuleConditionField condfield in ruleConditionFields)
            {
                propertyInf = type1.GetProperty(condfield.ObjectField.FieldName);
                if (propertyInf != null)
                {
                    string value = FieldValueResolver.GetFieldStringValue(condfield.ObjectField, propertyInf.GetValue(entity, null));

                    if (condfield.Value != value)
                    {
                        validcondition = false;
                        break;
                    }
                }
            }

            return validcondition;
        }

        #endregion

        public List<ObjectField> GetObjectFieldsList(string objectTableName, int tenant)
        {
            List<ObjectField> objectFieldList = new List<ObjectField>();
            objectFieldList = ObjectFieldRepository.GetObjectFieldsByObjectTableName(objectTableName, tenant).ToList(); //Context.Where(d => d.ObjectTable.Name == objectTableName && (d.Tenant == tenant || d.Tenant == 0)).ToList();

            return objectFieldList;
        }

        #region SerializeFilterItems

        public byte[] SerializeFilterItems(QueryOperations filterItems)
        {
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(typeof(QueryOperations));

            ser.Serialize(memstream, filterItems);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        #endregion

        #region GetObjectFieldValue

        private object GetObjectFieldValue(string propertyName, string objectTableName, object entity, int tenant)
        {
            object value = null;

            if (propertyName.Contains("."))
            {
                string[] fields = propertyName.Split('.');
                List<ObjectField> entityObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName(objectTableName, tenant).ToList();
                value = GetInsideEntityFieldValue(entity, entityObjectFields, fields, tenant);
            }

            else
            {
                value = GetEntityFieldValue(entity, propertyName);
            }

            return value;
        }

        #endregion

        #region GetInsideEntityFieldValue

        public string GetInsideEntityFieldValue(object entity, List<ObjectField> entityObjectFields, string[] fields, int tenant)
        {
            int i = 0;
            string resultValue = " ";
            object currentEntity = entity;
            List<ObjectField> currentEntityObjectFields = entityObjectFields;

            while (true)
            {
                PropertyInfo propertyPathPi = currentEntity.GetType().GetProperty(fields[i].Trim());
                if (propertyPathPi != null)
                {
                    object value = propertyPathPi.GetValue(currentEntity, null);

                    if (value == null)
                    {
                        break;
                    }
                    ObjectField objectField = currentEntityObjectFields.Where(f => f.FieldName == fields[i]).FirstOrDefault();
                    if (objectField != null)
                    {
                        string insideEntityName = objectField.ObjectTable_LookUpTable.Name;
                        if (insideEntityName == "Carrier")
                        {
                            insideEntityName = "Card";
                        }

                        List<ObjectField> insideEntityObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName(insideEntityName, tenant).ToList();
                        ObjectField insideObjectField = insideEntityObjectFields.Where(f => f.FieldName == fields[i + 1]).FirstOrDefault();
                        string insideTypePath = "Logitude.BL.ShipmentsModel.EntityQueries." + insideEntityName + "Query";

                        Type insideEntityType = Type.GetType(insideTypePath);
                        if (insideEntityType == null)
                        {
                            insideTypePath = "Logitude.BL.CommonDataModel.EntityQueries." + insideEntityName + "Query";
                            insideEntityType = Type.GetType(insideTypePath);
                        }

                        if (insideEntityType == null)
                        {
                            insideTypePath = "Logitude.BL." + insideEntityName + "Query";
                            insideEntityType = Type.GetType(insideTypePath);
                        }

                        if (insideEntityType == null)
                        {
                            insideTypePath = "Logitude.BL.InfrastructureModel.EntityQueries." + insideEntityName + "Query";
                            insideEntityType = Type.GetType(insideTypePath);
                        }

                        if (insideEntityType == null)
                        {
                            insideTypePath = "Logitude.BL.QuoteModel.EntityQueries." + insideEntityName + "Query";
                            insideEntityType = Type.GetType(insideTypePath);
                        }

                        object insideEntityRepository = null;
                        if (insideEntityType != null)
                        {
                            if (definedObjects.Keys.Contains(insideTypePath))
                            {
                                insideEntityRepository = definedObjects[insideTypePath];
                            }

                            if (insideEntityRepository == null)
                            {
                                insideEntityRepository = Activator.CreateInstance(insideEntityType, tenant);///??????
                                ///
                                definedObjects.Add(insideTypePath, insideEntityRepository);
                            }

                            MethodInfo insideMethodInfo = insideEntityRepository.GetType().GetMethod("GetSinglePM");
                            object insideEntity = null;
                            if (insideMethodInfo != null)
                            {
                                ParameterInfo[] parametersInfo = insideMethodInfo.GetParameters();
                                object[] parameters = new object[] { };
                                switch (parametersInfo.Count())
                                {
                                    case 1:
                                        parameters = new object[] { value };
                                        break;
                                    case 2:
                                        parameters = new object[] { value, tenant };
                                        break;
                                    case 3:
                                        parameters = new object[] { value, tenant, false };
                                        break;
                                    default:
                                        parameters = new object[] { value, tenant };
                                        break;
                                }

                                insideEntity = insideMethodInfo.Invoke(insideEntityRepository, parameters);
                                if (insideEntity != null)
                                {
                                    if ((i + 1) < fields.Count())
                                    {
                                        PropertyInfo insidePropertyPathPi = insideEntity.GetType().GetProperty(fields[i + 1].Trim());
                                        if (insidePropertyPathPi != null)
                                        {
                                            object insideValue = insidePropertyPathPi.GetValue(insideEntity, null);
                                            if (insideValue != null)
                                            {
                                                if (insideValue is DateTime)
                                                {
                                                    DateTime date = (DateTime)insideValue;
                                                    insideValue = date.ToShortDateString();
                                                }
                                            }

                                            resultValue = (insideValue != null ? insideValue.ToString() : " ");
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    else
                                    {

                                        break;
                                    }

                                    if (insideObjectField != null)
                                    {
                                        if (insideObjectField.DataTypeCode == "LookUp")
                                        {
                                            currentEntity = insideEntity;
                                            currentEntityObjectFields = insideEntityObjectFields;
                                            i++;
                                        }
                                        else
                                        {
                                            // check if its a multi value
                                            // ResolveObjectFieldValue
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            return resultValue;
        }


        #endregion

        #region GetEntityFieldValue

        private object GetEntityFieldValue(object entity, string propertyName)
        {
            object resultValue = " ";
            PropertyInfo propertyPathPi = entity.GetType().GetProperty(propertyName.Trim());
            if (propertyPathPi != null)
            {
                object value = propertyPathPi.GetValue(entity, null);
                resultValue = value;
            }

            return resultValue;
        }

        #endregion

        #region SetEntityFieldValue

        private void SetEntityFieldValue(object entity, string propertyName, object value)
        {
            object resultValue = " ";
            PropertyInfo propertyPathPi = entity.GetType().GetProperty(propertyName.Trim());
            if (propertyPathPi != null)
            {
                propertyPathPi.SetValue(entity, value, null);
            }
        }

        #endregion


    }
}
 