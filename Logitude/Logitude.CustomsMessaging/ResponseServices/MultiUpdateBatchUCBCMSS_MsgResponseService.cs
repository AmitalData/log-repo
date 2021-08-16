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
            var mess = new StringBuilder();
            var context = CustomContext.GetContext(requestParams.Tenant);
            this.MyResponseData = new INF_MSG_GenericResponseData();
            var myDeclarationQueryService = new DeclarationQueryService(context);
            var mySupplierInvoiceItemQueryService = new SupplierInvoiceItemQueryService(context);
            var mySupplierInvoiceItemProcesTypeQueryService = new SupplierInvoiceItemProcesTypeQueryService(context);
            SupplierInvoiceItemUpdateService updateService = new SupplierInvoiceItemUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);

            var procestypesExist = false;
            DeclarationPM declarationPM = myDeclarationQueryService.GetSingle(customResponse.Declarationid, true, false);
            if (declarationPM != null)
            {
                var invoiceitems=new List<SupplierInvoiceItemPM>();
                if (customResponse.ClassificationCode != null) // Update Classification no - will update only the recored with the same classification
                {
                    invoiceitems = mySupplierInvoiceItemQueryService.GetSupplierInvoiceItemByClassificationCode(declarationPM.Id, declarationPM.Tenant, customResponse.ClassificationCode);
                }
                else // Update ALL
                {
                    invoiceitems = mySupplierInvoiceItemQueryService.GetSupplierInvoiceItemsForDeclaration(declarationPM.Id, declarationPM.Tenant);
                }
                foreach (var invoice in invoiceitems)
                {
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
                            invoice.SupplierInvoiceItemsProcessTypeLastLineNumber= mySupplierInvoiceItemProcesTypeQueryService.GetMaxLineNumber(invoice.DeclarationId, invoice.CounterKey, invoice.LineNumber, customResponse.tenant);
                            invoice.ChangeSetOp = ChangeSetOperation.Update;
                        }
                    }
                    if (customResponse.TaxExemptCode != null)
                    {
                        invoice.TaxExemptCode = customResponse.TaxExemptCode;
                        invoice.ChangeSetOp = ChangeSetOperation.Update;

                    }

                    updateService.Update(invoice, true);
                    this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
                    this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;
                    this.MyRequestSheetParam.CustomFileNo = declarationPM.CustomFileNo;
                    this.MyRequestSheetParam.ObjectTableId1 = ObjectTableRepository.GetObjectTableByName("Customs.Declaration");
                    this.MyResponseData.UserMessage = mess.ToString();
                    this.MyResponseData.Succeeded = true;
                    this.MyResponseData.ApplicationID= declarationPM.CustomFileNo;
                }
            }

        }

    }

}
