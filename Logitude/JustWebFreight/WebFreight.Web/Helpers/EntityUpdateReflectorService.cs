using Logitude.BL.Interfaces;
using Logitude.BL.Security;
using Logitude.BookingLib.Data;
using Logitude.CRM.Data;
using Logitude.Customs.Data;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools.Helpers;
using Logitude.WarehouseLib.Data;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InvoiceModel;
using Simplog.Data.QuoteModel;
using Simplog.Data.ShipmentsModel;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class EntityUpdateReflectorService: IEntityUpdateReflectorService
    {

        int tenant = 0;
        public  void UpdateEntity(object entityPM, string entityName, int tenant)
        {
            UpdateEntityByArgs(new UpdateEntityArgs
            {
                EntityPM = entityPM,
                EntityName = entityName,
                Tenant = tenant
            });
        }

        public void UpdateEntity(UpdateEntityArgs updateEntityArgs)
        {
            UpdateEntityByArgs(updateEntityArgs);
        }

        private void UpdateEntityByArgs(UpdateEntityArgs updateEntityArgs)
        {
            this.tenant = updateEntityArgs.Tenant;
            UpdateEntityServiceParameter prepareUpdateEntityResult = GetUpdateEntityServiceParameter(updateEntityArgs.EntityName, tenant);
            if (prepareUpdateEntityResult.Type == null) return;
            object entityUpdateService = GentNewInStanceFromEntityUpdateService(updateEntityArgs, prepareUpdateEntityResult);
            if (prepareUpdateEntityResult.IsNewModule) InitializeEntityUpdateService(updateEntityArgs.EntityPM, entityUpdateService);
            MethodInfo updateMethodInfo = entityUpdateService.GetType().GetMethods().Where(d => d.Name == "Update").FirstOrDefault();
            object[] parameters = GetUpdateMethodParameters(updateEntityArgs.EntityPM, updateEntityArgs.EntityName, updateMethodInfo);
            if (updateMethodInfo != null) updateMethodInfo.Invoke(entityUpdateService, parameters);
            else throw new Exception("Update" + updateEntityArgs.EntityName + "Service" + "not found!");
        }

        private  void InitializeEntityUpdateService(object entityPM, object entityService)
        {
            MethodInfo methodInfo = entityService.GetType().GetMethods().Where(d => d.Name == "InitializeEntityPM").FirstOrDefault();
            methodInfo.Invoke(entityService, new object[] { entityPM });
            PropertyInfo propInfo = entityPM.GetType().GetProperty("ChangeSetOp");
            if (propInfo != null) propInfo.SetValue(entityPM, Simplog.Server.Infrastructure.ChangeSetOperation.Update, null);
        }

        private UpdateEntityServiceParameter GetUpdateEntityServiceParameter(string entityName, int tenant)
        {
            Assembly blAssembly = Assembly.Load("Logitude.BL");
            object objectContext = ShipmentsContext.GetContext(tenant);
            string typePath = "Logitude.BL.ShipmentsModel.Tools.EntityService." + entityName + "Service";
            Type type = blAssembly.GetType(typePath);
            if (type == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
                typePath = "Logitude.BL.CommonDataModel.Tools.EntityService." + entityName + "Service";
                type = blAssembly.GetType(typePath);

            }

            if (type == null)
            {
                objectContext = WebFreightContext.GetContext(tenant);
                typePath = "Logitude.BL.InfrastructureModel.Tools.EntityService." + entityName + "Service";
                type = blAssembly.GetType(typePath);
            }

            if (type == null)
            {
                objectContext = QuotesContext.GetContext(tenant);
                typePath = "Logitude.BL.QuoteModel.Tools.EntityService." + entityName + "Service";
                type = blAssembly.GetType(typePath);
            }


            if (type == null)
            {
                objectContext = InvoiceContext.GetContext(tenant);
                typePath = "Logitude.BL.InvoiceModel.Tools.EntityService." + entityName + "Service";
                type = blAssembly.GetType(typePath);
            }


            bool isNewModule = type == null ? true : false;
            if (type == null)
            {
                objectContext = CRMContext.GetContext(tenant);
                Assembly assembly = Assembly.Load("Logitude.CRM.BL");
                typePath = "Logitude.CRM.BL.EntityUpdateServices." + entityName + "UpdateService";
                type = assembly.GetType(typePath);
            }

            if (type == null)
            {
                objectContext = CustomContext.GetContext(tenant);

                Assembly assembly = Assembly.Load("Logitude.Customs.BL");
                typePath = "Logitude.Customs.BL.EntityUpdateServices." + entityName + "UpdateService";
                type = assembly.GetType(typePath);

            }

            if (type == null)
            {
                objectContext = BookingContext.GetContext(tenant);

                Assembly assembly = Assembly.Load("Logitude.BookingLib.BL");
                typePath = "Logitude.BookingLib.BL.EntityUpdateServices." + entityName + "UpdateService";
                type = assembly.GetType(typePath);

            }

            if (type == null)
            {
                objectContext = WarehouseContext.GetContext(tenant);
                Assembly assembly = Assembly.Load("Logitude.WarehouseLib.BL");
                typePath = "Logitude.WarehouseLib.BL.EntityUpdateServices." + entityName + "UpdateService";
                type = assembly.GetType(typePath);
            }

            return new UpdateEntityServiceParameter(){Type = type,ObjectContext = objectContext,IsNewModule = isNewModule,Tenant = tenant};

        }

        private  object[] GetUpdateMethodParameters(object entityPM, string entityName, MethodInfo methodInfo)
        {
            ParameterInfo[] methodParameters = methodInfo.GetParameters();
            object[] parameters = new object[] { };
            switch (methodParameters.Count())
            {
                case 1:
                    parameters = entityName == "Shipment" ? new object[] { true } : new object[] { entityPM };
     
                    break;
                case 2:
                    parameters = new object[] { entityPM, true };
                    break;
                case 3:
                    parameters = new object[] { entityPM, true, null };
                    break;

            }

            return parameters;
        }

        private  object GentNewInStanceFromEntityUpdateService(UpdateEntityArgs updateEntityArgs, UpdateEntityServiceParameter PrepareUpdateEntityResult)
        {
            object entityService = null;
            if (updateEntityArgs.EntityName == "Shipment")
            {
                string loggedUserEmail = !string.IsNullOrEmpty(updateEntityArgs.LoggedUserEmail) ? updateEntityArgs.LoggedUserEmail : AuthenticationUtil.GetLoggedUserEmail(this.tenant);
                entityService = Activator.CreateInstance(PrepareUpdateEntityResult.Type, new object[] { PrepareUpdateEntityResult.ObjectContext, updateEntityArgs.EntityPM, loggedUserEmail });
            }
            else if (PrepareUpdateEntityResult.IsNewModule)
            {
                entityService = Activator.CreateInstance(PrepareUpdateEntityResult.Type, new object[] { PrepareUpdateEntityResult.ObjectContext, new Dictionary<string, IContext>(), PrepareUpdateEntityResult.Tenant });
            }
            else
            {
                entityService = Activator.CreateInstance(PrepareUpdateEntityResult.Type, new object[] { PrepareUpdateEntityResult.ObjectContext, PrepareUpdateEntityResult.Tenant });
            }

            return entityService;
        }


    }



    public class UpdateEntityServiceParameter
    {
        public object ObjectContext { get; set; }
        public Type Type { get; set; }
        public bool IsNewModule { get; set; }
        public int Tenant { get; set; }

        


    }

}


