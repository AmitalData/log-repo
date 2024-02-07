using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using System.Data.Common;
using System.Data.SqlClient;
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using System;
using Unifreight.Data.AmitalModel;
using Logitude.Customs.BL.BL;
using UnifreightIIG.Common.TransshipmentDeclarationRequestServiceReference;
using Logitude.Customs.BL.Utils;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class SaveDF_MSG2751_2757_TransshipmentDeclarationRequestRequestService :
        RequestServiceBase<DF_NG_2751_MSG10000_ExportDeclaration, GenericRequestParams>
    {
        private DF_NG_2751_MSG10000_ExportDeclarationRequestService exportDeclarationRequestService = new DF_NG_2751_MSG10000_ExportDeclarationRequestService();

        public override void OnRequestFail(GenericRequestParams requestParams)
        {
            if (!String.IsNullOrWhiteSpace(requestParams.AppicationId))
            {
                CalculateDeclarationCourierStatus.UpdateCourierDeclarationStatusCode(requestParams.Tenant, requestParams.AppicationId);
            }

            base.OnRequestFail(requestParams);
        }

        public override void ManipulateRequestParams(GenericRequestParams requestParams)
        {
            if (requestParams.RequestVIA == SendRequestVIA.DCABatch)
            {
                return;
            }
            var settings = CustomsSettingQueryService.GetSettingByTenant(requestParams.Tenant);
            var maxItemsSendInteractive = settings.MaxItemsSendInteractive ?? 100;
            var maxSISendInteractive = settings.MaxSISendInteractive ?? 15;
            int countItems = 0;
            int countSI = 0;
            int backgroundcountItems = 0;
            var fast = true;
            var sw = Stopwatch.StartNew();
            bool onlyAlwaysAccumulate = false;
            int existSupplierInvoiceItemsWithoutHash = 0;
            int existSupplierInvoiceItemsWithParent = 0;
            int SItoAccumulate = 0;
            try
            {
                var siqs = new SupplierInvoiceQueryService(requestParams.Tenant);
                SItoAccumulate = siqs.GetSupplierInvoiceToAccumulateCount(requestParams.Tenant, requestParams.AppicationId);
                var ssiqs = new SupplierInvoiceItemQueryService(requestParams.Tenant);
                bool noAccumulateForNow = true;//itzik +ihab 
                if (noAccumulateForNow)
                {
                    countSI = siqs.GetSupplierInvoiceCountForDeclaration(requestParams.AppicationId, requestParams.Tenant);
                    countItems = ssiqs.GetDeclarationCountOfSupplierInvoiceItems(requestParams.Tenant, requestParams.AppicationId, true);
                    LogMessagingUtil.Instance.AppendLine("GetDeclarationCountOfSupplierInvoiceItems: " + countItems.ToString());
                }
                else
                {
                    countItems = siqs.GetDeclarationCountOfSupplierInvoiceItemsForAccumulation(requestParams.Tenant, requestParams.AppicationId);
                    LogMessagingUtil.Instance.AppendLine("GetDeclarationCountOfSupplierInvoiceItemsForAccumulation: " + countItems.ToString());
                }

                backgroundcountItems = countItems;
                existSupplierInvoiceItemsWithParent = ssiqs.ExistSupplierInvoiceItemsWithParent(requestParams.Tenant, requestParams.AppicationId);
                if ((countItems > maxItemsSendInteractive || SItoAccumulate > 0) && existSupplierInvoiceItemsWithParent > 0)
                {
                    //existSupplierInvoiceItemsWithoutHash = qs.ExistSupplierInvoiceItemsWithoutHash(requestParams.Tenant, requestParams.AppicationId
                    if (countItems < 999 && SItoAccumulate > 0) onlyAlwaysAccumulate = true;
                    existSupplierInvoiceItemsWithoutHash = siqs.ExistSupplierInvoiceItemsWithoutHashForAccumulation(requestParams.Tenant, requestParams.AppicationId, onlyAlwaysAccumulate);
                    if (existSupplierInvoiceItemsWithParent > 0 && existSupplierInvoiceItemsWithoutHash < 1)
                    {
                        if (countItems > 998)
                        {
                            countItems = existSupplierInvoiceItemsWithParent;
                        }
                        if (backgroundcountItems > maxItemsSendInteractive)
                        {
                            backgroundcountItems = existSupplierInvoiceItemsWithParent;
                        }
                    }
                }

#if false
1>                This take All SupplierInvoiceItems  include parent !!!

2>               **maybe** if >998 and all SIItems have  accurate itemHash 
                - Then not need to ReCalcAccumulation & to send InterActive 
                - Ask Yaron 
#endif

            }
            finally
            {
                LogMessagingUtil.Instance.AppendLine("LogitudeSettings.LogitudeURL = " + LogitudeSettings.LogitudeURL);
                LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams:requestParams.RequestVIA = " + requestParams.RequestVIA.ToString());
                if (countItems > 998)
                {
                    if (LogitudeSettings.LogitudeURL.Contains("http://192.116.221.103/Oracle"))
                    {
                        requestParams.RequestVIA = SendRequestVIA.WebServiceBatch;
                    }
                    else
                    {
                        requestParams.RequestVIA = SendRequestVIA.DCABatch;
                    }

                    requestParams.RequestVIAChangeDue = ("הצהרה זו מכילה מעל 998 פרטי מכס ולכן תשלח לכספת");
                    LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams:requestParams.RequestVIA = " + requestParams.RequestVIA.ToString());
                    LogMessagingUtil.Instance.AppendLine("הצהרה זו מכילה מעל 998 פרטי מכס ולכן תשלח לכספת");
                }
                else
                {
                    if (backgroundcountItems >= maxItemsSendInteractive)
                    {
                        if (requestParams.RequestVIA == SendRequestVIA.DCABatch)
                        {
                            LogMessagingUtil.Instance.AppendLine("***User**** Send this request VIA DCABatch-- no need to change !!!");
                            LogMessagingUtil.Instance.AppendLine(string.Format("הצהרה זו מכילה מעל {0} פרטי מכס ולכן תשודר ברקע",maxItemsSendInteractive));
                        }
                        else
                        {
                            requestParams.RequestVIA = SendRequestVIA.WebServiceBatch;
                            LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams:requestParams.RequestVIA = SendRequestVIA.WebServiceBatch");
                            LogMessagingUtil.Instance.AppendLine(string.Format("הצהרה זו מכילה מעל {0} פרטי מכס ולכן תשודר ברקע", maxItemsSendInteractive));
                            requestParams.RequestVIAChangeDue = string.Format("הצהרה זו מכילה מעל {0} פרטי מכס ולכן תשודר ברקע", maxItemsSendInteractive);
                        }

                    }
                    if (
                        (requestParams.RequestVIA == SendRequestVIA.WebServiceInteractive
                        || requestParams.RequestVIA == SendRequestVIA.Default)
                        && countSI > maxSISendInteractive)
                    {
                        requestParams.RequestVIA = SendRequestVIA.WebServiceBatch;
                        LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams:requestParams.RequestVIA = SendRequestVIA.WebServiceBatch");
                        LogMessagingUtil.Instance.AppendLine(string.Format("הצהרה זו מכילה מעל {0} חן ספק ולכן תשודר ברקע", maxSISendInteractive));
                        requestParams.RequestVIAChangeDue = (string.Format("הצהרה זו מכילה מעל {0} חן ספק ולכן תשודר ברקע", maxSISendInteractive));


                    }
                    if (SItoAccumulate > 0)
                    {
                        requestParams.RequestVIAChangeDue = ("בהצהרה זו יש חשבון ספק שמסומן לצבירה ולכן ההצהרה תיצבר");
                        LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams:requestParams.RequestVIA = " + requestParams.RequestVIA.ToString());
                        LogMessagingUtil.Instance.AppendLine("בהצהרה זו יש חשבון ספק שמסומן לצבירה ולכן ההצהרה תיצבר");
                    }
                }

                LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams:requestParams.RequestVIA = " + requestParams.RequestVIA.ToString());
                LogMessagingUtil.Instance.AppendLine("ManipulateRequestParams:fast=" + fast.ToString() + ":Took:" + sw.ElapsedMilliseconds);
            }
        }

        public override void PostGetRequest(DF_NG_2751_MSG10000_ExportDeclaration customRequest, GenericRequestParams requestParams)
        {
            UnifreightIIG.Common.ExportDeclarationServiceReference.DF_NG_2751_MSG10000_ExportDeclaration castCustomRequest =
             Serializer.CastXML<UnifreightIIG.Common.ExportDeclarationServiceReference.DF_NG_2751_MSG10000_ExportDeclaration, DF_NG_2751_MSG10000_ExportDeclaration>(customRequest);

            exportDeclarationRequestService.PostGetRequest(castCustomRequest, requestParams);
        }

        public override DF_NG_2751_MSG10000_ExportDeclaration GetRequest(GenericRequestParams requestParams)
        {
            UnifreightIIG.Common.ExportDeclarationServiceReference.DF_NG_2751_MSG10000_ExportDeclaration req = exportDeclarationRequestService.GetRequest(requestParams);

            DF_NG_2751_MSG10000_ExportDeclaration castReq =
             Serializer.CastXML<DF_NG_2751_MSG10000_ExportDeclaration, UnifreightIIG.Common.ExportDeclarationServiceReference.DF_NG_2751_MSG10000_ExportDeclaration>(req);

            return castReq;
        }
    }
}
