using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;

namespace Logitude.BL.Helpers
{
    public class CustomFieldResolver
    {
        Dictionary<string, object> definedObjects = new Dictionary<string, object>();
        static CultureInfo en = new CultureInfo("en-US");

        public void SetFieldValue(CustomFieldResolverArgs customFieldResolverArgs)
        {
            List<ObjectField> customObjectFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName(customFieldResolverArgs.ObjectTableName, customFieldResolverArgs.Tenant).ToList();
            ObjectField customObjectField = customObjectFields.FirstOrDefault(f => f.Code.Replace(" ", "") == customFieldResolverArgs.FieldCode);
            if (customObjectField == null) return;
            PropertyInfo propInfo = customFieldResolverArgs.EntityPM.GetType().GetProperty(customObjectField.FieldName);
            if (propInfo == null) return;
            CustomFieldClass customFilterClass = new CustomFieldClass();
            string customFieldValue = customFilterClass.SetFieldDataType(customObjectField.DataTypeCode, customFieldResolverArgs.FieldValue);
            customFieldValue = ResolveCustomFieldValue(customObjectField, customFieldValue, customFieldResolverArgs.Tenant);
            propInfo.SetValue(customFieldResolverArgs.EntityPM, new CustomFieldClass(customObjectField.FieldName, customFieldResolverArgs.ObjectTableName, customFieldValue));
        }

        private string ResolveCustomFieldValue(ObjectField objectField, string fieldValue, int tenant)
        {
            //For now we handle picklist type
            if (objectField.DataTypeCode == "PickList")
            {
                return HandleCustomPickListField(objectField, fieldValue, tenant);
            }
            return fieldValue;
        }

        private string HandleCustomPickListField(ObjectField objectField, string fieldValue, int tenant)
        {
            if (fieldValue == null)
                return "";

            CustomPickListRepository customPickListRepository = new CustomPickListRepository(tenant);
            CustomPickList picklist = customPickListRepository.GetSingleCustomPickListByValue(objectField.CustomPickListCode, fieldValue, tenant);
            if (picklist != null)
                return picklist.Id;

            return fieldValue;
        }

        public void SetCustomFieldsValues(string objectTableName, int tenant, List<Object> listQuery)
        {
            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            List<ObjectField> customFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName(objectTableName, tenant).ToList();

            foreach (ObjectField field in customFields)
            {
                foreach (object list in listQuery)
                {
                    if (list != null)
                    {
                        PropertyInfo propInfo = list.GetType().GetProperty(field.FieldName);

                        object newValue = customFieldResolver.GetFieldValue(list, field, tenant);

                        propInfo.SetValue(list, newValue, null);
                    }
                }
            }
        }

        public void SetDataProviderCustomFieldsValues(string objectTableName, int tenant, Object entity, Object provider, string propertyIdientifier = null)
        {
            if (entity != null && provider != null)
            {
                CustomFieldResolver customFieldResolver = new CustomFieldResolver();
                List<ObjectField> customFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName(objectTableName, tenant).ToList();

                foreach (ObjectField field in customFields)
                {
                    PropertyInfo propInfo = entity.GetType().GetProperty(field.FieldName);

                    if (propInfo != null)
                    {
                        object newValue = customFieldResolver.GetFieldValue(entity, field, tenant);
                        PropertyInfo providerPropInfo = null;

                        if (!string.IsNullOrEmpty(propertyIdientifier))
                        {
                            providerPropInfo = provider.GetType().GetProperty(propertyIdientifier + field.FieldName);
                        }

                        else
                        {
                            providerPropInfo = provider.GetType().GetProperty(objectTableName + field.FieldName);
                        }

                        if (providerPropInfo != null)
                        {
                            providerPropInfo.SetValue(provider, newValue, null);
                        }
                    }
                }
            }
        }

        public string GetFieldValue(object entity, ObjectField objectField, int tenant, bool externalAPICall = false)
        {
            string resultValue = null;
            object currentEntity = entity;

            PropertyInfo propertyPathPi = currentEntity.GetType().GetProperty(objectField.FieldName);

            if (propertyPathPi != null)
            {
                object value = propertyPathPi.GetValue(currentEntity, null);

                if (value != null)
                {
                    if (value.GetType() == typeof(CustomFieldClass))
                    {
                        CustomFieldClass classvalue = value as CustomFieldClass;
                        value = classvalue.Value;
                    }
                }

                if (objectField != null)
                {
                    Assembly blAssembly = Assembly.Load("Logitude.BL");

                    if (objectField.DataTypeCode == "LookUp" && value != null)
                    {
                        string insideEntityName = ObjectTableRepository.GetSingleObjectTableById(objectField.LookUpTableId, objectField.Tenant).Name;
                        if (insideEntityName == "Carrier")
                        {
                            insideEntityName = "Card";
                        }

                        string insideTypePath = "Logitude.BL.ShipmentsModel.EntityQueries." + insideEntityName + "Query";

                        Type insideEntityType = blAssembly.GetType(insideTypePath);

                        if (insideEntityType == null)
                        {
                            insideTypePath = "Logitude.BL.CommonDataModel.EntityQueries." + insideEntityName + "Query";
                            insideEntityType = blAssembly.GetType(insideTypePath);
                        }

                        if (insideEntityType == null)
                        {
                            insideTypePath = "Logitude.BL." + insideEntityName + "Query";
                            insideEntityType = blAssembly.GetType(insideTypePath);
                        }

                        if (insideEntityType == null)
                        {
                            insideTypePath = "Logitude.BL.InfrastructureModel.EntityQueries." + insideEntityName + "Query";
                            insideEntityType = blAssembly.GetType(insideTypePath);
                        }

                        if (insideEntityType == null)
                        {
                            insideTypePath = "Logitude.BL.QuoteModel.EntityQueries." + insideEntityName + "Query";
                            insideEntityType = blAssembly.GetType(insideTypePath);
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
                                insideEntityRepository = Activator.CreateInstance(insideEntityType, tenant);

                                definedObjects.Add(insideTypePath, insideEntityRepository);
                            }
                            MethodInfo insideMethodInfo = insideEntityRepository.GetType().GetMethod("GetCustomSinglePM");
                            if (insideMethodInfo == null)
                            {
                                insideMethodInfo = insideEntityRepository.GetType().GetMethod("GetSinglePM");
                            }
                            if (insideMethodInfo == null)
                            {
                                insideMethodInfo = insideEntityRepository.GetType().GetMethod("GetSingle");
                            }
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
                                    ObjectTable lookupTable = ObjectTableRepository.GetSingleObjectTableById(objectField.LookUpTableId, objectField.Tenant);
                                    string lookupProperty = lookupTable.LookUp2 != null ? lookupTable.LookUp2 : lookupTable.LookUp1;
                                    if (externalAPICall)
                                    {
                                        lookupProperty = "Code";
                                    }

                                    PropertyInfo insidePropertyPathPi = insideEntity.GetType().GetProperty(lookupProperty);
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

                                        resultValue = (insideValue != null ? insideValue.ToString() : null);
                                    }
                                }
                            }
                        }
                    }

                    if (objectField.DataTypeCode == "PickList")
                    {
                        if (value != null)
                        {

                            CustomPickListRepository customPickListRepository = new CustomPickListRepository(tenant);
                            CustomPickList picklist = customPickListRepository.GetSingleCustomPickList(value.ToString(), tenant);
                            if (picklist != null)
                            {
                                resultValue = picklist.Value;
                            }
                        }
                    }

                    if (objectField.DataTypeCode != "LookUp" && objectField.DataTypeCode != "PickList")
                    {
                        string valueString = value != null ? value.ToString() : null;
                        if (!String.IsNullOrEmpty(valueString))
                        {
                            resultValue = GetFieldDataType(objectField, valueString);
                        }
                    }
                }
            }

            if (resultValue == "")
            {
                resultValue = null;
            }

            return resultValue;
        }

        private static DateTime? ConvertToDate(string arg)
        {
            DateTime? date = null;

            if (!string.IsNullOrEmpty(arg) && arg.Length >= 14)
            {
                date = new DateTime(System.Convert.ToInt32(arg.Substring(0, 4)), System.Convert.ToInt32(arg.Substring(4, 2)), System.Convert.ToInt32(arg.Substring(6, 2)), System.Convert.ToInt32(arg.Substring(8, 2)), System.Convert.ToInt32(arg.Substring(10, 2)), System.Convert.ToInt32(arg.Substring(12, 2)));
            }

            return date;
        }

        private static string GetFieldDataType(ObjectField field, string customField)
        {
            if (field != null && customField != null)
            {
                switch (field.DataTypeCode.Trim())
                {
                    case "Text":
                    case "Emails":
                    case "nText":
                        {
                            return customField.ToString();
                        }

                    case "DateTime":
                        {
                            DateTime? date = ConvertToDate(customField);
                            return date != null ? date.Value.ToString(@"dd MMM yyyy HH':'mm':'ss", en.DateTimeFormat) : null;
                        }

                    case "Date":
                        {
                            DateTime? date = ConvertToDate(customField);
                            return date != null ? date.Value.ToString(@"dd MMM yyyy", en.DateTimeFormat) : null;
                        }

                    case "UnsDecimal":
                    case "Decimal":
                        {
                            decimal d = 0;

                            if (customField.Length >= 15)
                            {
                                customField = customField.Insert(customField.Length - 3, ".");
                            }

                            decimal.TryParse(customField, out d);

                            return d.ToString();
                        }

                    case "Integer":
                    case "UnsInteger":
                        {
                            if (!string.IsNullOrEmpty(customField))
                            {
                                customField = customField.Trim().Replace(" ", "");
                                customField = customField.Split('.')[0];
                            }

                            int i = 0;
                            int.TryParse(customField, out i);

                            return i.ToString();
                        }

                    case "Double":
                    case "SigDouble":
                        {
                            if (!string.IsNullOrEmpty(customField))
                            {
                                customField = customField.Trim().Replace(" ", "");
                            }


                            double d = 0;

                            if (customField.Length >= 15)
                            {
                                customField = customField.Insert(customField.Length - 3, ".");
                            }

                            double.TryParse(customField, out d);
                            return d.ToString();
                        }

                    case "Boolean":
                        {
                            bool b = false;
                            bool.TryParse(customField, out b);
                            return b.ToString();
                        }

                    default:
                        {
                            return null;
                        }
                }
            }

            return null;
        }



        public string GetFieldValue2(object value, ObjectField objectField, int tenant)
        {
            string resultValue = null;
            if (value != null)
            {
                if (objectField != null)
                {
                    Assembly blAssembly = Assembly.Load("Logitude.BL");

                    if (objectField.DataTypeCode == "LookUp" && value != null)
                    {
                        string insideEntityName = ObjectTableRepository.GetSingleObjectTableById(objectField.LookUpTableId, objectField.Tenant).Name;
                        if (insideEntityName == "Carrier")
                        {
                            insideEntityName = "Card";
                        }

                        string insideTypePath = "Logitude.BL.ShipmentsModel.EntityQueries." + insideEntityName + "Query";

                        Type insideEntityType = blAssembly.GetType(insideTypePath);

                        if (insideEntityType == null)
                        {
                            insideTypePath = "Logitude.BL.CommonDataModel.EntityQueries." + insideEntityName + "Query";
                            insideEntityType = blAssembly.GetType(insideTypePath);
                        }

                        if (insideEntityType == null)
                        {
                            insideTypePath = "Logitude.BL." + insideEntityName + "Query";
                            insideEntityType = blAssembly.GetType(insideTypePath);
                        }

                        if (insideEntityType == null)
                        {
                            insideTypePath = "Logitude.BL.InfrastructureModel.EntityQueries." + insideEntityName + "Query";
                            insideEntityType = blAssembly.GetType(insideTypePath);
                        }

                        if (insideEntityType == null)
                        {
                            insideTypePath = "Logitude.BL.QuoteModel.EntityQueries." + insideEntityName + "Query";
                            insideEntityType = blAssembly.GetType(insideTypePath);
                        }

                        if (insideEntityType == null)
                        {
                            insideTypePath = "Logitude.BL.InvoiceModel.EntityQueries." + insideEntityName + "Query";
                            insideEntityType = blAssembly.GetType(insideTypePath);
                        }





                        bool isGeneratedQuery = false;
                        if (insideEntityType == null)
                        {
                            //Logitude.CRM.BL.EntityQueryServices
                            blAssembly = Assembly.Load("Logitude.CRM.BL");
                            insideTypePath = "Logitude.CRM.BL.EntityQueryServices." + insideEntityName + "QueryService";
                            insideEntityType = blAssembly.GetType(insideTypePath);
                            if (insideEntityType != null)
                            {
                                isGeneratedQuery = true;
                            }

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
                                insideEntityRepository = Activator.CreateInstance(insideEntityType, tenant);

                                definedObjects.Add(insideTypePath, insideEntityRepository);
                            }

                            MethodInfo insideMethodInfo = insideEntityRepository.GetType().GetMethod("GetCustomSinglePM");

                            if (insideMethodInfo == null)
                            {
                                // insideMethodInfo = insideEntityRepository.GetType().GetMethod("GetSinglePM");


                                insideMethodInfo = insideEntityRepository.GetType().GetMethods().Where(d => d.Name == "GetSinglePM").FirstOrDefault();

                            }
                            if (insideMethodInfo == null)
                            {
                                insideMethodInfo = insideEntityRepository.GetType().GetMethod("GetSingle");
                            }
                            object insideEntity = null;

                            if (insideMethodInfo != null)
                            {
                                ParameterInfo[] parametersInfo = insideMethodInfo.GetParameters();
                                object[] parameters = new object[] { };
                                if (!isGeneratedQuery)
                                {
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
                                }
                                else parameters = new object[] { value, false, false };



                                insideEntity = insideMethodInfo.Invoke(insideEntityRepository, parameters);

                                if (insideEntity != null)
                                {
                                    ObjectTable lookupTable = ObjectTableRepository.GetSingleObjectTableById(objectField.LookUpTableId, objectField.Tenant);
                                    string lookupProperty = lookupTable.LookUp2 != null ? lookupTable.LookUp2 : lookupTable.LookUp1;
                                    PropertyInfo insidePropertyPathPi = insideEntity.GetType().GetProperty(lookupProperty);
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

                                        resultValue = (insideValue != null ? insideValue.ToString() : null);
                                    }
                                }
                            }
                        }
                    }

                    if (objectField.DataTypeCode == "PickList")
                    {
                        if (value != null)
                        {
                            CustomPickListRepository customPickListRepository = new CustomPickListRepository(tenant);
                            CustomPickList picklist = customPickListRepository.GetSingleCustomPickList(value.ToString(), tenant);

                            if (picklist != null)
                            {
                                resultValue = picklist.Value;
                            }
                        }
                    }

                    if (objectField.DataTypeCode != "LookUp" && objectField.DataTypeCode != "PickList")
                    {
                        string valueString = value != null ? value.ToString() : null;
                        if (!String.IsNullOrEmpty(valueString))
                        {
                            resultValue = GetFieldDataType(objectField, valueString);
                        }
                    }
                }
            }

            if (resultValue == "")
            {
                resultValue = null;
            }

            return resultValue;
        }







    }

    public class CustomFieldResolverArgs
    {
        public string ObjectTableName { get; set; }
        public object EntityPM { get; set; }
        public string FieldCode { get; set; }
        public string FieldValue { get; set; }
        public int Tenant { get; set; }
    }
}
