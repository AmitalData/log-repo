using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebFreight.Web.Helpers.AutomationModel.SendInterface
{
    public class SendInterfaceDataContract
    {
        public string GetPropertyValueFromObject(string propertyName, object entity)
        {
            string propertyValue = string.Empty;
            PropertyInfo propertyInfo = entity.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
            {
                propertyValue = propertyInfo.GetValue(entity).ToString();
            }
            return propertyValue;
        }
        public static object GetMethodValue(object queryService, string methodName, object[] methodParameters)
        {
            MethodInfo methodInfo = queryService.GetType().GetMethods().Where(d => d.Name == methodName).FirstOrDefault();
            return methodInfo.Invoke(queryService, methodParameters);
        }
        public static Type GetInstanceAssemblyType(string assemblyValue, string typePath)
        {
            Assembly assembly = Assembly.Load(assemblyValue);
            return assembly.GetType(typePath);
        }
    }

    public class SendInterfaceDataContractObjectArgs
    {
        public int Tenant { get; set; }
        public AutomationSendInterface AutomationSendInterface { get; set; }
    }

    public class SendInterfaceDataContractFileNameArgs
    {
        public int Tenant { get; set; }
        public string Format { get; set; }
    }
}
