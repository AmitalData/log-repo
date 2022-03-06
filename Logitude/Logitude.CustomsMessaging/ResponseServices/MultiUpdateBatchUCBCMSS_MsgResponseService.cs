using Logitude.AmitalMessaging.Utils;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.Models;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.Testers.Messages;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Models;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel.Repsitories;
using Microsoft.Practices.Unity;
using Logitude.CustomsMessaging.Utils;
using Logitude.Customs.Data.EntityListQueryServices;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class MultiUpdateBatchUCBCMSS_MsgResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, DCAInUCBMultiUpdateWithResponseContentHeader, GenericRequestParams>
    {

        public override INF_MSG_GenericResponseData GetResponse(DCAInUCBMultiUpdateWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DCAInUCBMultiUpdateWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
            if (customResponse.Declarationid != null)
            {
                LogMessagingUtil.Instance.AppendLine("customResponse.Declarationid: " + customResponse.Declarationid + " = from DeclarationInvoiceView");
                Do_Update(customResponse, requestParams, customResponse.Declarationid);
                this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                this.MyResponseData.Succeeded = true;
            }
            else
            {
                LogMessagingUtil.Instance.AppendLine("customResponse.Declarationid is null = from CourierPendingView");
                ICustomContext customContext = CustomContext.GetContext(customResponse.tenant);
                LogMessagingUtil.Instance.AppendLine("select declarationIdsList to update");
                List<string> declarationIdsList = customResponse.checkboxAll ?
                   new DeclarationCourierStatusListQueryService(customContext).GetDeclarationCourierStatusListPendingBulk(customResponse.queryOperations, customResponse.tenant).Select(x => x.DeclarationId).ToList() :
                    customResponse.DeclarationIds.ToList();


                if (customResponse.checkboxAll && customResponse.allWithoutdeclarationIdsList != null && customResponse.allWithoutdeclarationIdsList.Count() > 0)
                    declarationIdsList.RemoveAll(x => customResponse.allWithoutdeclarationIdsList.Contains(x));

                foreach (var id in declarationIdsList)
                {
                    LogMessagingUtil.Instance.AppendLine("update declarationid: " + id);
                    Do_Update(customResponse, requestParams, id);
                }
                this.MyResponseData.UserMessage = "ההצהרות עודכנו";
                this.MyRequestSheetParam.CustomFileNo = null;
                this.MyResponseData.ApplicationID = customResponse.CourierMasterId;

            }

        }

        private void Do_Update(DCAInUCBMultiUpdateWithResponseContentHeader customResponse, GenericRequestParams requestParams, string declarationId)
        {
            var mess = new StringBuilder();
            var context = CustomContext.GetContext(requestParams.Tenant);

            var myDeclarationQueryService = new DeclarationQueryService(context);
            var mySupplierInvoiceItemQueryService = new SupplierInvoiceItemQueryService(context);
            var mySupplierInvioceItemCertificatQueryService = new SupplierInvioceItemCertificatQueryService(context);
            var mySupplierInvoiceItemProcesTypeQueryService = new SupplierInvoiceItemProcesTypeQueryService(context);
            SupplierInvoiceItemUpdateService updateService = new SupplierInvoiceItemUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);

            var procestypesExist = false;

            DeclarationPM declarationPM = myDeclarationQueryService.GetSingle(declarationId, true, false);

            if (declarationPM != null)
            {
                LogMessagingUtil.Instance.AppendLine("declarationPM found");
                var invoiceitems = new List<SupplierInvoiceItemPM>();
                if (customResponse.ClassificationCode != null && customResponse.Declarationid != null) // Update Classification no - will update only the recored with the same classification
                {
                    invoiceitems = mySupplierInvoiceItemQueryService.GetSupplierInvoiceItemByClassificationCode(declarationPM.Id, declarationPM.Tenant, customResponse.ClassificationCode);
                }
                else // Update ALL
                {
                    invoiceitems = mySupplierInvoiceItemQueryService.GetSupplierInvoiceItemsForMultiUpdate(declarationPM.Id, declarationPM.Tenant);
                }
                foreach (var item in invoiceitems)
                {

                    var invoice = mySupplierInvoiceItemQueryService.GetSingleSupplierInvoicePMBySequence(item.DeclarationId, item.CounterKey, (int)item.SequenceNumeric);
                    invoice.SupplierInvioceItemCertificats = mySupplierInvioceItemCertificatQueryService.GetSupplierInvioceItemCertificatesForSupplierInvoiceItem(item.DeclarationId, item.CounterKey, invoice.LineNumber, invoice.Tenant);
                    procestypesExist = false;
                    if (customResponse.ProcessTypeCode != null)
                    {
                        var procestypes = mySupplierInvoiceItemProcesTypeQueryService.GetSupplierInvoiceItemProcesTypesForSupplierInvoiceItem(declarationPM.Id, invoice.CounterKey, invoice.LineNumber, invoice.Tenant);
                        foreach (var proces in procestypes)
                        {
                            if (proces.ProcessTypeCode == customResponse.ProcessTypeCode)
                            {
                                procestypesExist = true;
                            }
                        }
                        if (!procestypesExist)
                        {
                            var entity = new SupplierInvoiceItemProcesTypePM();
                            entity.ChangeSetOp = ChangeSetOperation.Insert;
                            entity.Tenant = customResponse.tenant;
                            entity.ProcessTypeCode = customResponse.ProcessTypeCode;
                            invoice.SupplierInvoiceItemProcesTypes.Add(entity);
                            invoice.SupplierInvoiceItemsProcessTypeLastLineNumber = mySupplierInvoiceItemProcesTypeQueryService.GetMaxLineNumber(invoice.DeclarationId, invoice.CounterKey, invoice.LineNumber, customResponse.tenant);
                            invoice.ChangeSetOp = ChangeSetOperation.Update;
                        }
                    }
                    if (customResponse.TaxExemptCode != null)
                    {
                        invoice.TaxExemptCode = customResponse.TaxExemptCode;
                        invoice.ChangeSetOp = ChangeSetOperation.Update;
                    }
                    if(customResponse.ClassificationCode != null && customResponse.Declarationid == null)
                    {
                        LogMessagingUtil.Instance.AppendLine("set ClassificationCode");

                        invoice.ClassificationCode = customResponse.ClassificationCode;
                        
                        CustomsItemQueryService customsItemQueryService = new CustomsItemQueryService(customResponse.tenant);
                        LogMessagingUtil.Instance.AppendLine("prev InvoiceQuantityType = " + invoice.InvoiceQuantityType);

                        invoice.InvoiceQuantityType = customsItemQueryService.GetQuantityTypeByClassificationCode(customResponse.ClassificationCode, customResponse.tenant);
                        LogMessagingUtil.Instance.AppendLine("set InvoiceQuantityType = " + invoice.InvoiceQuantityType);

                        invoice.ChangeSetOp = ChangeSetOperation.Update;
                    }
                    LogMessagingUtil.Instance.AppendLine("updating invoice (CounterKey,LineNumber):" + invoice.CounterKey + "," + invoice.LineNumber);
                    updateService.Update(invoice, true);
                    this.MyRequestSheetParam.CustomFileNo = declarationPM.CustomFileNo;
                    this.MyResponseData.UserMessage += mess.ToString();
                    this.MyResponseData.ApplicationID = declarationPM.CustomFileNo;
                }
            }
        }

    }

}
