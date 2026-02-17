using System;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Reflection;
using Simplog.Data.InfrastructureModel.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Server.Tools.Helpers
{
    public class FieldValueResolver
    {
        #region GetFieldDataValue

        public static object GetFieldDataValue(ObjectField field, string value)
        {
            if (field != null && value != null)
            {
                switch (field.DataTypeCode.Trim())
                {
                    case "Text":
                    case "nText":
                    case "LookUp":
                    case "PickList":
                        {
                            return value.ToString();
                        }

                    case "DateTime":
                    case "Date":
                        {
                            if (value == "NoDate")
                            {
                                return null;
                            }
                            else
                            {
                                DateTime date = Convert.ToDateTime(value);
                                //string dateToStore = ConvertToString(date);
                                return date;
                            }
                        }

                    case "Decimal":
                        {
                            decimal d = 0;
                            decimal.TryParse(value.ToString(), out d);

                            return d;
                        }

                    case "UnsDecimal":
                        {
                            decimal d = 0;
                            decimal.TryParse(value.ToString(), out d);

                            return d;
                        }

                    case "Integer":
                        {
                            int d = 0;
                            int.TryParse(value.ToString(), out d);

                            return d;
                        }

                    case "UnsInteger":
                        {
                            int d = 0;
                            int.TryParse(value.ToString(), out d);

                            return d;
                        }

                    case "Double":
                        {
                            double d = 0;
                            double.TryParse(value.ToString(), out d);

                            return d;
                        }

                    case "SigDouble":
                        {
                            double d = 0;
                            double.TryParse(value.ToString(), out d);

                            return d;
                        }

                    case "Boolean":
                        {
                            bool f = false;
                            bool.TryParse(value, out f);
                            return f;
                        }

                    default:
                        {
                            return (value != null ? value.ToString() : null);
                        }
                }
            }

            return null;
        }

        #endregion

        #region  #endregion

        public static string GetFieldStringValue(ObjectField field, object value, int tenant = 0)
        {
            if (field != null && value != null)
            {
                if (String.IsNullOrEmpty(value.ToString()))
                {
                    return null;
                }
                switch (field.DataTypeCode.Trim())
                {
                    case "Text":
                        {
                            return value.ToString();
                        }

                    case "nText":
                        {
                            return value.ToString();
                        }
                    case "LookUp":
                    case "PickList":
                        {
                            return value.ToString();
                        }
                    case "DateTime":
                        {
                            DateTime date = Convert.ToDateTime(value);
                            string dateToStore = ConvertToString(date);
                            return dateToStore;
                        }

                    case "Date":
                        {
                            DateTime date = Convert.ToDateTime(value);
                            string dateToStore = ConvertToString(date);
                            return dateToStore;
                        }
                    case "Decimal":
                        {
                            if (String.IsNullOrEmpty(value.ToString()))
                            {
                                return null;
                            }
                            decimal d = Convert.ToDecimal(value);
                            string decimalTostore = ConvertToString(d);
                            return decimalTostore;
                        }
                    case "Integer":
                        {
                            int d = Convert.ToInt32(value);
                            string intTostore = ConvertToString(d);
                            return intTostore;
                        }
                    case "Double":
                        {
                            double d = Convert.ToDouble(value);
                            string doubleTostore = ConvertToString(d);
                            return doubleTostore;
                        }
                    case "Boolean":
                        {
                            return value.ToString();
                        }
                    default:
                        {
                            return value != null ? value.ToString() : null;
                        }
                }
            }
            return null;
        }


        public static object GetFieldObjectValue(ObjectField field, string customField)
        {
            if (field != null && customField != null)
            {
                switch (field.DataTypeCode.Trim())
                {
                    case "Text":
                    case "nText":
                    case "LookUp":
                    case "PickList":
                        {

                            return customField.ToString();

                        }
                    case "Date":
                    case "DateTime":
                        {

                            DateTime? date = ConvertToDate(customField);

                            return date;
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

                            return d;
                        }
                    case "Integer":
                    case "UnsInteger":
                        {
                            int i = 0;
                            int.TryParse(customField, out i);

                            return i;
                        }
                    case "Double":
                    case "SigDouble":
                        {
                            double d = 0;
                            if (customField.Length >= 15)
                            {
                                customField = customField.Insert(customField.Length - 3, ".");
                            }
                            double.TryParse(customField, out d);
                            return d;
                        }
                    case "Boolean":
                        {
                            bool b = false;
                            bool.TryParse(customField, out b);
                            return b;
                        }
                    default:
                        {
                            return null;
                        }
                }



            }
            return null;



        }

        #endregion

        #region ConvertToString Date

        public static string ConvertToString(DateTime date)
        {
            string time = "";
            string month, day, minuit, second, hour;

            if (date.Month < 10)
            {
                month = "0" + date.Month.ToString();
            }
            else { month = date.Month.ToString(); }

            if (date.Day < 10)
            {
                day = "0" + date.Day.ToString();
            }
            else { day = date.Day.ToString(); }

            if (date.Hour < 10)
            {
                hour = "0" + date.Hour.ToString();
            }
            else { hour = date.Hour.ToString(); }

            if (date.Minute < 10)
            {
                minuit = "0" + date.Minute.ToString();
            }
            else { minuit = date.Minute.ToString(); }

            if (date.Second < 10)
            {
                second = "0" + date.Second.ToString();
            }
            else { second = date.Second.ToString(); }

            time = date.Year.ToString() + month + day + hour + minuit + second;

            return time;
        }

        #endregion

        #region ConvertToString Integer

        public static string ConvertToString(int d)
        {
            string dstring = d.ToString();
            string s = "";
            if (dstring.Length == 1)
            {
                s = "0000000" + dstring;
            }

            if (dstring.Length == 2)
            {
                s = "000000" + dstring;
            }
            if (dstring.Length == 3)
            {
                s = "00000" + dstring;
            }
            if (dstring.Length == 4)
            {
                s = "0000" + dstring;
            }
            if (dstring.Length == 5)
            {
                s = "000" + dstring;
            }
            if (dstring.Length == 6)
            {
                s = "00" + dstring;
            }

            if (dstring.Length == 7)
            {
                s = "0" + dstring;
            }
            if (dstring.Length == 8)
            {
                s = dstring;
            }
            return s;
        }

        #endregion

        #region ConvertToString Double

        public static string ConvertToString(double d)
        {
            string dstring = d.ToString();
            string s = "";
            if (dstring.Length == 1)
            {
                s = "0000000" + dstring;
            }

            if (dstring.Length == 2)
            {
                s = "000000" + dstring;
            }
            if (dstring.Length == 3)
            {
                s = "00000" + dstring;
            }
            if (dstring.Length == 4)
            {
                s = "0000" + dstring;
            }
            if (dstring.Length == 5)
            {
                s = "000" + dstring;
            }
            if (dstring.Length == 6)
            {
                s = "00" + dstring;
            }

            if (dstring.Length == 7)
            {
                s = "0" + dstring;
            }
            if (dstring.Length == 8)
            {
                s = dstring;
            }
            return s;
        }

        #endregion

        #region ConvertToString Decimal

        public static string ConvertToString(decimal d)
        {
            string dstring = d.ToString();
            string s = "";
            if (dstring.Length == 1)
            {
                s = "0000000" + dstring;
            }

            if (dstring.Length == 2)
            {
                s = "000000" + dstring;
            }
            if (dstring.Length == 3)
            {
                s = "00000" + dstring;
            }
            if (dstring.Length == 4)
            {
                s = "0000" + dstring;
            }
            if (dstring.Length == 5)
            {
                s = "000" + dstring;
            }
            if (dstring.Length == 6)
            {
                s = "00" + dstring;
            }

            if (dstring.Length == 7)
            {
                s = "0" + dstring;
            }
            if (dstring.Length == 8)
            {
                s = dstring;
            }
            return s;
        }

        #endregion

        #region ConvertToDate

        public static DateTime? ConvertToDate(string s)
        {
            DateTime? date = null;
            if (!string.IsNullOrEmpty(s))
            {
                date = new DateTime(System.Convert.ToInt32(s.Substring(0, 4)), System.Convert.ToInt32(s.Substring(4, 2)), System.Convert.ToInt32(s.Substring(6, 2)), System.Convert.ToInt32(s.Substring(8, 2)), System.Convert.ToInt32(s.Substring(10, 2)), System.Convert.ToInt32(s.Substring(12, 2)));
            }

            return date;
        }
        #endregion

        public static string GetCustomFieldStringValueForAPI(ObjectField field, string value, int tenant = 0)
        {
            string result = null;
            if (field != null && value != null)
            {

                switch (field.DataTypeCode.Trim())
                {
                    case "Text":
                    case "nText":
                        {
                            result = value.ToString();
                            break;
                        }
                    case "LookUp":
                    case "PickList":
                        {
                            result = GetLookUpFieldValueForAPI(field, value.ToString(), tenant);
                            break;

                        }
                    case "DateTime":
                        {
                            DateTime date;
                            if (DateTime.TryParse(value, out date))
                            {
                                string dateToStore = ConvertToString(date);
                                result = dateToStore;
                            }

                            break;
                        }

                    case "Date":
                        {
                            DateTime date;
                            if (DateTime.TryParse(value, out date))
                            {
                                string dateToStore = ConvertToString(date);
                                result = dateToStore;
                            }

                            break;
                        }
                    case "Decimal":
                        {
                            decimal d;
                            if (decimal.TryParse(value, out d))
                            {
                                string decimalTostore = ConvertToString(d);
                                result = decimalTostore;
                            }
                            break;
                        }
                    case "Integer":
                        {
                            int d;
                            if (int.TryParse(value, out d))
                            {
                                string intTostore = ConvertToString(d);
                                result = intTostore;
                            }

                            break;
                        }
                    case "Double":
                        {
                            double d;
                            if (double.TryParse(value, out d))
                            {
                                string doubleTostore = ConvertToString(d);
                                result = doubleTostore;
                            }
                            break;
                        }
                    case "Boolean":
                        {
                            bool d;
                            if (bool.TryParse(value, out d))
                            {
                                result = value.ToString();
                            }
                            break;
                        }
                    default:
                        {
                            result = value != null ? value.ToString() : null;
                            break;
                        }
                }
            }
            if (string.IsNullOrEmpty(result))
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (field.DataTypeCode == "LookUp")
                    {
                        string lookupTableName = field.Code;
                        ObjectTable lookupTable = ObjectTableRepository.GetSingleObjectTableById(field.LookUpTableId, field.Tenant);
                        if (lookupTable != null)
                            lookupTableName = lookupTable.Name;

                        throw new ApplicationException("Custom Field with Code " + field.Code + " has invalid value: " + lookupTableName + " with Code " + value + " doesn't exist");
                    }
                    if (field.DataTypeCode == "PickList")
                    {
                        throw new ApplicationException("Custom Field with Code " + field.Code + " has invalid value: " + field.CustomPickListCode + " PickList" + " with Code " + value + " doesn't exist");
                    }

                    throw new ApplicationException("Custom Field with Code " + field.Code + " has invalid value");
                }
            }
            else if (field.DataTypeCode == "Text" || field.DataTypeCode == "nText")
            {
                if (!field.IsMaxLength)
                {
                    if (result.ToString().Length > field.MaxLength || result.ToString().Length < field.MinLength)
                    {
                        throw new ApplicationException("Custom Field with Code " + field.Code + " must be less than " + field.MaxLength + " and more than " + field.MinLength);
                    }
                }

            }

            return result;
        }

        public static string GetLookUpFieldValueForAPI(ObjectField objectField, string codeValue, int tenant)
        {
            Dictionary<string, object> definedObjects = new Dictionary<string, object>();
            string resultValue = null;
            if (objectField != null)
            {
                Assembly blAssembly = Assembly.Load("Logitude.BL");

                if (objectField.DataTypeCode == "LookUp" && codeValue != null)
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

                        MethodInfo insideMethodInfo = insideEntityRepository.GetType().GetMethod("GetSinglePMByCode");
                        object insideEntity = null;

                        if (insideMethodInfo != null)
                        {
                            ParameterInfo[] parametersInfo = insideMethodInfo.GetParameters();
                            object[] parameters = new object[] { };
                            switch (parametersInfo.Count())
                            {
                                case 1:
                                    parameters = new object[] { codeValue };
                                    break;
                                case 2:
                                    parameters = new object[] { codeValue, tenant };
                                    break;
                                case 3:
                                    parameters = new object[] { codeValue, tenant, false };
                                    break;
                                default:
                                    parameters = new object[] { codeValue, tenant };
                                    break;
                            }


                            insideEntity = insideMethodInfo.Invoke(insideEntityRepository, parameters);

                            if (insideEntity != null)
                            {
                                ObjectTable lookupTable = ObjectTableRepository.GetSingleObjectTableById(objectField.LookUpTableId, objectField.Tenant);
                                string lookupProperty = lookupTable.KeyPropertyPath != null ? lookupTable.KeyPropertyPath : "Id";
                                PropertyInfo insidePropertyPathPi = insideEntity.GetType().GetProperty(lookupProperty);
                                if (insidePropertyPathPi != null)
                                {
                                    object insideValue = insidePropertyPathPi.GetValue(insideEntity, null);
                                    if (insideValue != null)
                                    {
                                        //if (insideValue is DateTime)
                                        //{
                                        //    DateTime date = (DateTime)insideValue;
                                        //    insideValue = date.ToShortDateString();
                                        //}
                                    }

                                    resultValue = (insideValue != null ? insideValue.ToString() : null);
                                }
                            }
                        }
                    }
                }

                if (objectField.DataTypeCode == "PickList")
                {
                    if (codeValue != null)
                    {
                        CustomPickListRepository customPickListRepository = new CustomPickListRepository(tenant);
                        CustomPickList picklist = customPickListRepository.GetSingleCustomPickListByValue(objectField.CustomPickListCode, codeValue, tenant);
                        if (picklist != null)
                        {
                            resultValue = picklist.Id;
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
}
