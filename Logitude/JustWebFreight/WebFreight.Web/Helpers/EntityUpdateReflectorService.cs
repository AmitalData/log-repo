using Logitude.BL.Security;
using Logitude.BookingLib.Data;
using Logitude.CRM.Data;
using Logitude.Customs.Data;
using Logitude.Infrastructure.Data;
using Logitude.WarehouseLib.Data;
using Simplog.Data.CommonDataModel;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InvoiceModel;
using Simplog.Data.QuoteModel;
using Simplog.Data.ShipmentsModel;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class EntityUpdateReflectorService
    {

        public static void UpdateEntity(object entityPM, string entityName , int tenant)
        {

            Assembly blAssembly = Assembly.Load("Logitude.BL");
            object objectContext = ShipmentsContext.GetContext(tenant);
            string typePath = "Logitude.BL.ShipmentsModel.Tools.EntityService." + entityName + "Service";
            Type type = blAssembly.GetType(typePath);
            object entityService = null;

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


            var isNewModule = type == null ? true : false ;
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


            if (type != null)
            {
                if (entityName == "Shipment")
                {
                    entityService = Activator.CreateInstance(type, new object[] { objectContext, entityPM, SecurityUtility.GetAuthenticatedUser() });
                }
                else if (isNewModule)
                {
                    entityService = Activator.CreateInstance(type, new object[] { objectContext, new Dictionary<string, IContext>(), tenant });
                }
                else
                {
                    entityService = Activator.CreateInstance(type, new object[] { objectContext, tenant });
                }

                MethodInfo methodInfo;

                if (isNewModule)
                {
                    methodInfo = entityService.GetType().GetMethods().Where(d => d.Name == "InitializeEntityPM").FirstOrDefault();
                    methodInfo.Invoke(entityService, new object[] { entityPM });

                    PropertyInfo propInfo = entityPM.GetType().GetProperty("ChangeSetOp");
                    if (propInfo != null) propInfo.SetValue(entityPM, Simplog.Server.Infrastructure.ChangeSetOperation.Update, null);
                }

                methodInfo = entityService.GetType().GetMethods().Where(d => d.Name == "Update").FirstOrDefault();

                ParameterInfo[] methodParameters = methodInfo.GetParameters();
                object[] parameters = new object[] { };
                switch (methodParameters.Count())
                {
                    case 1:
                        parameters = new object[] { entityPM };

                        break;
                    case 2:
                        parameters = new object[] { entityPM, true };
                        break;
                    case 3:
                        parameters = new object[] { entityPM, true, null };
                        break;

                }

                if (methodInfo != null)
                {
                    methodInfo.Invoke(entityService, parameters);
                }
                else
                {
                    throw new Exception("Update" + entityName + "PM Service" + "not found!");
                }


            }

      

            }



        }


    }
