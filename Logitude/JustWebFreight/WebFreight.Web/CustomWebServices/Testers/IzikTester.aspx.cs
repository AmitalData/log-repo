using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using static Logitude.BL.InfrastructureModel.EntityQueries.ObjectFieldQuery;

namespace WebFreight.Web.CustomWebServices.Testers
{
    public partial class IzikTester : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //try
            //{
            Response.Clear();
            SupplierInvoiceFreightAmountPM pm = new SupplierInvoiceFreightAmountPM()
            {
                DeclarationId = "1-6384",
                InvoiceCounterKey = 1,
                CurrencyTypeCode = "USD"
            };

            int tenant = 0;
            //bool isValid = CheckForgienKeyClosedTable(pm, tenant);
            //bool isValid2 = CheckForgienKey<SupplierInvoiceFreightAmount>(pm, tenant);

            DeclarationPM declarationPm = new DeclarationQueryService(tenant).GetSingle("1-6801", true, true);

            //var p = declarationPm;
            //var z = p.GetType().GetProperty("Consignments")?.GetValue(p, null);

            //bool isList =
            //    z.GetType().FullName.StartsWith("System.Collections.Generic.List") &&
            //    z.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));

            //if (isList)
            //{
            //    var val = ((IEnumerable<dynamic>)z).FirstOrDefault();

            //    if (val != null && val.GetType().Name.EndsWith("PM"))
            //        CheckForgienKeyClosedTable(val, 0);
            //}

            bool isValid2 = CheckForgienKeyClosedTable(declarationPm, 0);

            Response.Write(declarationPm);
            //}
            //catch (Exception eee)
            //{
            //    Response.Clear();
            //    Response.Write(eee.ToString());
            //}
        }

        private bool CheckForgienKeyClosedTable(object pm, int tenant, bool update = true)
        {
            bool forgienKeyValidate = true;

            forgienKeyValidate = GetPMFields(pm).All(p => CheckForgienKeyClosedTable(p, tenant, update));

            ICustomContext objectContext = CustomContext.GetContext(tenant);

            var objectFieldQuery = new ObjectFieldQuery(tenant);

            string pmName = pm.GetType().Name;
            pmName = pmName.Remove(pmName.Length - 2);
            List<ForiegnKeyDetails> forgienKeys = objectFieldQuery.GetForgienKeys(tenant, pmName).ToList();
            forgienKeys.RemoveAll(x => pm.GetType().GetProperty(x.propName) == null || GetValueFormPm(pm, x.propName) == null);
            forgienKeys.ForEach(fk =>
            {
                if (fk.tableName.StartsWith("Customs."))
                    fk.tableName = fk.tableName.Remove(0, 8);
            });

            forgienKeys.ForEach(fk =>
            {
                object value = getValueOfForgienKey(tenant, pm, fk.tableName, fk.propName);

                forgienKeyValidate &= value != null;

                if (update && value == null)
                    SetPmProp(pm, fk.propName, null);
            });

            return forgienKeyValidate;
        }

        private bool CheckForgienKey<X>(object pm, int tenant, bool update = true)
        {
            bool forgienKeyValidate = true;
            ICustomContext objectContext = CustomContext.GetContext(tenant);

            Dictionary<string, string> forgienKeys = GetForgienKeyAttribute<X>();

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


            foreach (KeyValuePair<string, string> entry in forgienKeys)
            {
                string entityName = entry.Value;
                string prop = entry.Key;

                object value = getValueOfForgienKey(tenant, pm, entityName, prop);

                forgienKeyValidate &= value != null;

                if (update && value == null)
                    SetPmProp(pm, prop, null);
            }

            return forgienKeyValidate;
        }

        private object getValueOfForgienKey(int tenant, object pm, string entityName, string prop) =>
            getValueOfForgienKey(tenant, pm, entityName, new List<string>() { prop });

        private object getValueOfForgienKey(int tenant, object pm, string entityName, List<string> props)
        {
            if (entityName == "Card" && pm.GetType().Name == "DeclarationPM")
            {
                object[] _metohdArgs = new object[] { new List<string>() { GetValueFormPm(pm, "CustomerId") as string } };
                var _value = GetValueOfForgienKeySimplogData(tenant, pm, entityName, "GetCustomerNamesById", _metohdArgs) as Dictionary<string, string>;
                return _value.Count > 0 ? _value : null;
            }
            else if (entityName == "Department" && pm.GetType().Name == "DeclarationPM")
            {
                object[] _metohdArgs = new object[] { GetValueFormPm(pm, "DepartmentId"), tenant };
                return GetValueOfForgienKeySimplogData(tenant, pm, entityName, "GetSingleDepartment", _metohdArgs);
            }
            else if (entityName == "User" && pm.GetType().Name == "DeclarationPM")
            {
                object[] _metohdArgs = new object[] { GetValueFormPm(pm, "ReferentUserId"), tenant };
                return GetValueOfForgienKeySimplogData(tenant, pm, entityName, "GetSingleUser", _metohdArgs, 2);
            }


            Type repositoryType = GetRepositoryType(entityName);
            MethodInfo methodInfos = GetSingaleMethod(repositoryType);

            var repoArgs = new object[] { tenant };
            object repositoryInstance = Activator.CreateInstance(repositoryType, repoArgs);
            var keys = CreateRepositoryKey(entityName, props, pm);
            object[] metohdArgs = new object[] { keys };

            var value = methodInfos.Invoke(repositoryInstance, metohdArgs);
            return value;
        }

        public object GetValueFormPm(object pm, string prop) =>
            pm.GetType().GetProperty(prop)?.GetValue(pm, null);

        public MethodInfo ReflectionRepositoryGetSingleMethod(string entityName)
        {
            Type repositoryType = GetRepositoryType(entityName);
            MethodInfo methodInfos = GetSingaleMethod(repositoryType);

            return methodInfos;
        }

        private static Type GetRepositoryType(string entityName) =>
            GetRepositoryType("Logitude.Customs.Data", "Repsitories." + entityName + "Repository");

        private static Type GetRepositoryType(string assemblyName, string path)
        {
            string repositoryName = assemblyName + "." + path;
            Assembly assembly = Assembly.Load(assemblyName);
            return assembly.GetType(repositoryName);
        }

        public MethodInfo GetSingaleMethod(Type type) =>
            type.GetMethods()
            .Where((m) =>
                m.Name == "GetSingle"
                && m.GetParameters().Any(x => x.ParameterType == typeof(EntityKeyFields)))
            .FirstOrDefault();

        public object CreateRepositoryKey(string entityName, string prop, object pm) =>
            CreateRepositoryKey(entityName, new List<string>() { prop }, pm);

        public object CreateRepositoryKey(string entityName, List<string> props, object pm)
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

        public object CreateCardRepositoryKey(string entityName, List<string> props, object pm)
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

        public Dictionary<string, string> GetForgienKeyAttribute<T>()
        {
            Dictionary<string, string> _dict = new Dictionary<string, string>();

            typeof(T).GetProperties().ToList().ForEach(prop =>
                prop.GetCustomAttributes(true).ToList().ForEach(attr =>
                {
                    var foreignKeyAttribute = attr as ForeignKeyAttribute;
                    if (foreignKeyAttribute != null)
                    {
                        string fieldName = prop.Name;
                        string forgienTable = foreignKeyAttribute.Name;

                        _dict.Add(fieldName, forgienTable);
                    }
                })
            );

            return _dict;
        }

        private void SetPmProp(object pm, string prop, object value) =>
            pm.GetType().GetProperty(prop).SetValue(pm, value, null);


        //private List<object> GetPMFields<T>(object pm) =>
        //    typeof(T).GetProperties().ToList().FindAll(prop =>
        //        prop.GetCustomAttributes(true).ToList().Count() == 0)
        //        .Select(x => GetValueFormPm(pm, x.Name))
        //        .ToList().FindAll(x => x != null);

        //private object GetValueOfForgienKeyCard(int tenant, object pm, string entityName)
        //{
        //    Type repositoryType = GetSimplogDataRepositoryType(entityName);
        //    MethodInfo methodInfos = GetMethod(repositoryType, "GetCustomerNamesById");

        //    object repositoryInstance = Activator.CreateInstance(repositoryType, new object[] { tenant });
        //    object[] metohdArgs = new object[] { new List<string>() { GetValueFormPm(pm, "CustomerId") as string } };
        //    Dictionary<string, string> value = methodInfos.Invoke(repositoryInstance, metohdArgs) as Dictionary<string, string>;

        //    return value.Count > 0 ? value : null;
        //}
        //private object GetValueOfForgienKeyDeprtment(int tenant, object pm, string entityName)
        //{
        //    object[] metohdArgs = new object[] { GetValueFormPm(pm, "DepartmentId"), tenant };
        //    GetValueOfForgienKeySimplogData(tenant, pm, entityName, "GetSingleDepartment", metohdArgs);
        //    Type repositoryType = GetSimplogDataRepositoryType(entityName);
        //    MethodInfo methodInfos = GetMethod(repositoryType, "GetSingleDepartment");

        //    object repositoryInstance = Activator.CreateInstance(repositoryType, new object[] { tenant });
        //    Department value = methodInfos.Invoke(repositoryInstance, metohdArgs) as Department;

        //    return value;
        //}        

        private object GetValueOfForgienKeySimplogData(int tenant, object pm, string entityName, string methodName, object[] metohdArgs, int countParameter = -1)
        {
            Type repositoryType = GetSimplogDataRepositoryType(entityName);
            MethodInfo methodInfos = GetMethod(repositoryType, methodName, countParameter);

            object repositoryInstance = Activator.CreateInstance(repositoryType, new object[] { tenant });
            var value = methodInfos.Invoke(repositoryInstance, metohdArgs);

            return value;
        }

        private static Type GetSimplogDataRepositoryType(string entityName) =>
            GetRepositoryType("Simplog.Data", "CommonDataModel.Repositories." + entityName + "Repository");

        public MethodInfo GetMethod(Type type, string methodName, int countParameter = -1) =>
           type.GetMethods()
           .Where((m) => m.Name == methodName
           && (countParameter == -1 || m.GetParameters().Length == countParameter))
           .FirstOrDefault();

        private List<object> GetPMFields(object pm)
        {
            var res = new List<object>();

            var b = pm.GetType().GetProperty("DeclarationConsignments")?.GetValue(pm, null) as List<DeclarationConsignmentPM>;

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
    }
}