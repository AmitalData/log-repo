using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using static Logitude.BL.InfrastructureModel.EntityQueries.ObjectFieldQuery;

namespace Logitude.Customs.BL.Messaging.U2L.ImportDeclaration
{
    public class ForiegnKeyCheck
    {
        public static bool CheckClosedTable(object pm, int tenant, bool update = true)
        {
            bool forgienKeyValidate = true;

            forgienKeyValidate = GetPMFields(pm).All(p => CheckClosedTable(p, tenant, update));

            ICustomContext objectContext = CustomContext.GetContext(tenant);

            var objectFieldQuery = new ObjectFieldQuery(tenant);

            string pmName = pm.GetType().Name;
            pmName = pmName.Remove(pmName.Length - 2);

            List<ForiegnKeyDetails> forgienKeys = GetForiegnKeyOfCloseTable(pm, tenant, objectFieldQuery, pmName);

            forgienKeys.ForEach(fk =>
            {
                object value = getValueOfForgienKey(tenant, pm, fk.tableName, fk.propName);

                forgienKeyValidate &= value != null;

                if (update && value == null)
                    SetPmProp(pm, fk.propName, null);
            });

            return forgienKeyValidate;
        }

        private static List<ForiegnKeyDetails> GetForiegnKeyOfCloseTable(object pm, int tenant, ObjectFieldQuery objectFieldQuery, string pmName)
        {
            List<ForiegnKeyDetails> forgienKeys = objectFieldQuery.GetForgienKeys(tenant, pmName).ToList();
            forgienKeys.RemoveAll(x => pm.GetType().GetProperty(x.propName) == null || GetValueFormPm(pm, x.propName) == null);
            forgienKeys.ForEach(fk =>
            {
                if (fk.tableName.StartsWith("Customs."))
                    fk.tableName = fk.tableName.Remove(0, 8);
            });
            return forgienKeys;
        }

        public static bool Check<X>(object pm, int tenant, bool update = true)
        {
            bool forgienKeyValidate = true;
            ICustomContext objectContext = CustomContext.GetContext(tenant);

            Dictionary<string, ForiegnKeyData> forgienKeys = GetForgienKeyAttribute<X>();

            var DuplicateKeys = forgienKeys.ToLookup(x => x.Value, x => x.Key).Where(x => x.Count() > 1);
            foreach (IGrouping<string, string> entry in DuplicateKeys)
            {
                string entityName = entry.Key;
                List<string> props = entry.ToList();

                object value = getValueOfForgienKey(tenant, pm, entityName, props);

                forgienKeyValidate &= value != null;

                props.ForEach(x => forgienKeys.Remove(x));

                if (update && value == null)
                    props.ForEach(prop => SetPmProp(pm, prop, null));
            }


            foreach (KeyValuePair<string, ForiegnKeyData> entry in forgienKeys)
            {
                string propKey = entry.Key;
                string entityName = entry.Value.entityName;

                object value = getValueOfForgienKey(tenant, pm, entityName, propKey);

                forgienKeyValidate &= value != null;

                if (update && value == null)
                    SetPmProp(pm, propKey, null);
            }

            return forgienKeyValidate;
        }

        private static object getValueOfForgienKey(int tenant, object pm, string entityName, string propKey) =>
            getValueOfForgienKey(tenant, pm, entityName, new List<string>() { propKey });

        private static object getValueOfForgienKey(int tenant, object pm, string entityName, List<string> propsKey)
        {
            if (pm.GetType().Name == "DeclarationPM")
            {
                //if (entityName == "CustomerId")
                //{
                //    object[] _metohdArgs = new object[] { new List<string>() { GetValueFormPm(pm, "CustomerId") as string } };
                //    var _value = GetValueOfForgienKeySimplogData(tenant, pm, entityName, "GetCustomerNamesById", _metohdArgs) as Dictionary<string, string>;
                //    return _value.Count > 0 ? _value : null;
                //}
                //else 
                if (entityName == "Card")
                {
                    object[] _metohdArgs = new object[] { new List<string>() { GetValueFormPm(pm, "CustomerId") as string } };
                    var _value = GetValueOfForgienKeySimplogData(tenant, pm, "Card", "GetCustomerNamesById", _metohdArgs) as Dictionary<string, string>;
                    return _value.Count > 0 ? _value : null;
                }
                else if (entityName == "Department" )
                {
                    object[] _metohdArgs = new object[] { GetValueFormPm(pm, "DepartmentId"), tenant };
                    return GetValueOfForgienKeySimplogData(tenant, pm, entityName, "GetSingleDepartment", _metohdArgs);
                }
                else if (entityName == "User")
                {
                    object[] _metohdArgs = new object[] { GetValueFormPm(pm, "ReferentUserId"), tenant };
                    return GetValueOfForgienKeySimplogData(tenant, pm, entityName, "GetSingleUser", _metohdArgs, 2);
                }
				else if (entityName == "TransportMode")
				{
					object[] _metohdArgs = new object[] { GetValueFormPm(pm, "TransportModeId")};
					return GetValueOfForgienKeySimplogData(tenant, pm, entityName, "GetSingleTransportMode", _metohdArgs, 1, "InfrastructureModel");
				}
				//else if (entityName == "Importer")
				//{
				//    object[] _metohdArgs = new object[] { GetValueFormPm(pm, "ImporterId"), tenant };
				//    return GetValueOfForgienKeySimplogData(tenant, pm, entityName, "GetSingle", _metohdArgs, 2);
				//}
			}


            Type repositoryType = GetRepositoryType(entityName);
            MethodInfo methodInfos = GetSingaleMethod(repositoryType);

            var repoArgs = new object[] { tenant };
            object repositoryInstance = Activator.CreateInstance(repositoryType, repoArgs);
            var keys = CreateRepositoryKey(entityName, propsKey, pm);
            object[] metohdArgs = new object[] { keys };

            var value = methodInfos.Invoke(repositoryInstance, metohdArgs);
            return value;
        }

        private static object GetValueFormPm(object pm, string prop) =>
            pm.GetType().GetProperty(prop)?.GetValue(pm, null);


        private static Type GetRepositoryType(string entityName) =>
            GetRepositoryType("Logitude.Customs.Data", "Repsitories." + entityName + "Repository");

        private static Type GetRepositoryType(string assemblyName, string path)
        {
            string repositoryName = assemblyName + "." + path;
            Assembly assembly = Assembly.Load(assemblyName);
            return assembly.GetType(repositoryName);
        }

        private static MethodInfo GetSingaleMethod(Type type) =>
            type.GetMethods()
            .Where((m) =>
                m.Name == "GetSingle"
                && m.GetParameters().Any(x => x.ParameterType == typeof(EntityKeyFields)))
            .FirstOrDefault();

        private static object CreateRepositoryKey(string entityName, List<string> props, object pm)
        {
            string keysName = "Logitude.Customs.Data.EntityKeys." + entityName + "Keys";
            Assembly assembly = Assembly.Load("Logitude.Customs.Data");
            var key = assembly.GetType(keysName);
            object instance = Activator.CreateInstance(key, null);

            if (props.Count > 1)
                props.ForEach(p => key.GetProperty(p).SetValue(instance, GetValueFormPm(pm, p), null));
            else
                key.GetProperties()[0].SetValue(instance, GetValueFormPm(pm, props[0]), null);

            return instance;
        }

        private static Dictionary<string, ForiegnKeyData> GetForgienKeyAttribute<T>()
        {
            var _dict = new Dictionary<string, ForiegnKeyData> ();

            typeof(T).GetProperties().ToList().ForEach(prop =>
                prop.GetCustomAttributes(true).ToList().ForEach(attr =>
                {
                    var foreignKeyAttribute = attr as ForeignKeyAttribute;
                    if (foreignKeyAttribute != null)
                    {
                        string fieldName = prop.Name;
                        var foriegnKeyData = new ForiegnKeyData()
                        {
                            forgienfield = foreignKeyAttribute.Name,
                            entityName = typeof(T).GetProperty(foreignKeyAttribute.Name).PropertyType.Name
                        };

                        _dict.Add(fieldName, foriegnKeyData);
                    }
                })
            );

            return _dict;
        }

        private static void SetPmProp(object pm, string prop, object value) =>
            pm.GetType().GetProperty(prop).SetValue(pm, value, null);

        private static object GetValueOfForgienKeySimplogData(int tenant, object pm, string entityName, string methodName, object[] metohdArgs, int countParameter = -1,string model = "CommonDataModel")
        {
            Type repositoryType = GetSimplogDataRepositoryType(entityName, model);
            MethodInfo methodInfos = GetMethod(repositoryType, methodName, countParameter);

            object repositoryInstance = Activator.CreateInstance(repositoryType, new object[] { tenant });
            var value = methodInfos.Invoke(repositoryInstance, metohdArgs);

            return value;
        }

        private static Type GetSimplogDataRepositoryType(string entityName,string model) =>
            GetRepositoryType("Simplog.Data", model + ".Repositories." + entityName + "Repository");

        private static MethodInfo GetMethod(Type type, string methodName, int countParameter = -1) =>
           type.GetMethods()
           .Where((m) => m.Name == methodName
           && (countParameter == -1 || m.GetParameters().Length == countParameter))
           .FirstOrDefault();

        private static List<object> GetPMFields(object pm)
        {
            var res = new List<object>();

            pm.GetType().GetProperties().ToList()
                .ForEach(prop =>
                {
                    var propVal = GetValueFormPm(pm, prop.Name);
                    if (propVal == null) return;

                    bool isList =
                        propVal.GetType().FullName.StartsWith("System.Collections.Generic.List") &&
                        propVal.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));

                    if (isList)
                    {
                        var val = ((IEnumerable<dynamic>)propVal).FirstOrDefault();

                        if (val != null && val.GetType().Name.EndsWith("PM"))
                            res.AddRange((IEnumerable<dynamic>)propVal);
                    }
                    else if (propVal != null && propVal.GetType().Name.EndsWith("PM"))
                        res.Add(propVal);
                });

            return res;
        }

        private class ForiegnKeyData
        {
            public string forgienfield { get; set; }
            public string entityName { get; set; }
        }
    }
}
