using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class EntityGetReflectorService : IEntityGetReflectorService
    {
        public object GetEntity(EntityGetReflector entityGetReflector)
        {
            if (entityGetReflector.EntityName == "Master") entityGetReflector.EntityName = "Shipment";

            Assembly blAssembly = Assembly.Load("Logitude.BL");
            Type type = blAssembly.GetType("Logitude.BL.ShipmentsModel.EntityQueries." + entityGetReflector.EntityName + "Query");
          
            if (type == null) 
                type = blAssembly.GetType("Logitude.BL.CommonDataModel.EntityQueries." + entityGetReflector.EntityName + "Query");

            if (type == null) 
                type = blAssembly.GetType("Logitude.BL." + entityGetReflector.EntityName + "Query");

            if (type == null)
                type = blAssembly.GetType("Logitude.BL.InfrastructureModel.EntityQueries." + entityGetReflector.EntityName + "Query");

            if (type == null)
                type = blAssembly.GetType("Logitude.BL.QuoteModel.EntityQueries." + entityGetReflector.EntityName + "Query");

            if (type == null)
                type = blAssembly.GetType("Logitude.BL.InvoiceModel.EntityQueries." + entityGetReflector.EntityName + "Query");

            if (type == null)
                type = Assembly.Load("Logitude.CRM.BL").GetType("Logitude.CRM.BL.EntityQueryServices." + entityGetReflector.EntityName + "QueryService");

            if (type == null)
                type = Assembly.Load("Logitude.Customs.BL").GetType("Logitude.Customs.BL.EntityQueryServices." + entityGetReflector.EntityName + "QueryService");

            if (type == null)
                type = Assembly.Load("Logitude.BookingLib.BL").GetType("Logitude.BookingLib.BL.EntityQueryServices." + entityGetReflector.EntityName + "QueryService");

            if (type == null)
                type = Assembly.Load("Logitude.WarehouseLib.BL").GetType("Logitude.WarehouseLib.BL.EntityQueryServices." + entityGetReflector.EntityName + "QueryService");

            object resultObject = ReflectEntityObject(entityGetReflector, type);

            return resultObject;
        }

        private object ReflectEntityObject(EntityGetReflector entityGetReflector, Type queryType)
        {
            if (queryType == null) return null;

            object entityInstanceType = Activator.CreateInstance(queryType, entityGetReflector.Tenant);
            MethodInfo methodInfo = entityInstanceType.GetType().GetMethod(entityGetReflector.MethodName);
            if (methodInfo == null) throw new Exception(entityGetReflector.MethodName + "not found! :" + entityGetReflector.EntityName);

            return methodInfo.Invoke(entityInstanceType, entityGetReflector.Parameters);
        }
    }
}