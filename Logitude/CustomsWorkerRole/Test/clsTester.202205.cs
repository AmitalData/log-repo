using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomsWorkerRole.Test
{
    public partial class clsTester
    {

       

        public static void ReAnalyze2470_CustomsWithheld(int tenant,int maxretry)//מעוכב מכס	
        {

            var objectTableDecId = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
            var qs = new DeclarationCourierStatusQueryService(tenant);
            var customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(tenant);
            var q = qs.GetQCustomsWithheld(tenant);
            var decs = q.ToList();
            Debug.WriteLine($"GetQCustomsWithheld({decs.Count})");
            var crsList = new List<string>();
            foreach (var decId in decs)
            {
                Debug.WriteLine($"currentdecId={decId}");
                var list = customsRequestsSheetQueryService.GetRequestByInterfaceTypeCode(tenant, "2470", objectTableDecId, decId, null);
                if (list?.Count > 0)
                {
                    Debug.WriteLine($"GetRequestByInterfaceTypeCode(2470).count={list?.Count}");
                    var customsRequestsSheetPM = list.FirstOrDefault(x => x.RequestStatusCode == "30");

                    if (customsRequestsSheetPM != null)
                    {

                        Debug.WriteLine($"customsRequestsSheetPM.id={customsRequestsSheetPM.Id}");
                        string mess = null;
                        try
                        {
                            if (maxretry > 0)
                            {
                                maxretry--;

                                ChangeAnalyzeFailAndReQueue(tenant, customsRequestsSheetPM);
                            }
                            mess = null;
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.ToString());
                            
                        }
                    }


                }

            }

        }
        public static void RequeByID(int tenant, string customsRequestsSheetId)
        {

            
            var customsRequestsSheetQueryService = new CustomsRequestsSheetQueryService(tenant);
            var customsRequestsSheetPM = customsRequestsSheetQueryService.GetSingle(customsRequestsSheetId,false,false);
            
            
                ChangeAnalyzeFailAndReQueue(tenant, customsRequestsSheetPM);
            
        }

        public static void ChangeAnalyzeFailAndReQueue(int tenant, Logitude.Customs.Def.EntityPMs.CustomsRequestsSheetPM customsRequestsSheetPM)
        {

            if (customsRequestsSheetPM?.RequestStatusEnum != Logitude.CustomsMessaging.Common.ResponseData.SheetStatusEnum.Analyzed)
            {
                Debug.WriteLine($"status not Analyzed-abort {customsRequestsSheetPM?.Id}");
                return;
            }
                
            using (var scopeNewCRS = TransactionFactory.GetNewTransaction())
            {

                var customContext = CustomContext.GetContext(tenant);
                var customsRequestsSheetUpdateService = new CustomsRequestsSheetUpdateService(customContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);

                //customsRequestsSheetPM.RequestStatusCode = "25";
                customsRequestsSheetPM.RequestStatusEnum= Logitude.CustomsMessaging.Common.ResponseData.SheetStatusEnum.AnalyzeFailed;
                customsRequestsSheetPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                customsRequestsSheetUpdateService.Update(customsRequestsSheetPM, true);

                MessagingServiceFactoryHelper
                .ResolveAndReQueue(customsRequestsSheetPM.InterfaceTypeCode, customsRequestsSheetPM.Tenant, customsRequestsSheetPM.Id);
                Debug.WriteLine($"customsRequestsSheetPM.id={customsRequestsSheetPM.Id}:done");
                scopeNewCRS.Complete();

            }
        }

        public static void CheckCustomsContext()
        {
            var customContext =CustomContext.GetContext(6) as ICustomContext;
            customContext.DecisionTypes.FirstOrDefault();
            var properties = customContext.GetType().GetProperties();
            foreach (var prop in properties)
            {
                if (prop.PropertyType.AssemblyQualifiedName.Contains("DbSet"))
                {
                     dynamic d =prop.GetValue(customContext);
                    try
                    {
                        d.FirstOrDefault();
                    }
                    catch (Exception)
                    {


                    }
                }
            }


        }
    }
}
