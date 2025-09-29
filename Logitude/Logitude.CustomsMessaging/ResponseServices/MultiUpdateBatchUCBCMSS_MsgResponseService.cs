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
using Logitude.Customs.BL.BL;

namespace Logitude.CustomsMessaging.ResponseServices
{
    public class MultiUpdateBatchUCBCMSS_MsgResponseService : ResponseServiceBase<INF_MSG_GenericResponseData, DCAInUCBMultiUpdateWithResponseContentHeader, GenericRequestParams>
    {
        bool isFromPendingView;
        bool isUpdated;
        public override INF_MSG_GenericResponseData GetResponse(DCAInUCBMultiUpdateWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        public override void Update(DCAInUCBMultiUpdateWithResponseContentHeader customResponse, GenericRequestParams requestParams)
        {
            this.MyResponseData = new INF_MSG_GenericResponseData();
            this.MyRequestSheetParam = this.MyRequestSheetParam ?? new RequestSheetParam();
            this.MyRequestSheetParam.RequestDescription = requestParams.RequestName;

            isFromPendingView = customResponse.Declarationid == null;

            if (!isFromPendingView)
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
            isUpdated = false;
			var context = CustomContext.GetContext(requestParams.Tenant);
            var myDeclarationQueryService = new DeclarationQueryService(context);
            DeclarationPM declarationPM = myDeclarationQueryService.GetSingle(declarationId, true, false);

            if (declarationPM == null)
                return;

            LogMessagingUtil.Instance.AppendLine("declarationPM found");

            UpdateInvoiceitems(customResponse, declarationPM, context, requestParams);
            
            if(isFromPendingView)
            {
                UpdateDeclaration(customResponse, requestParams, context, declarationPM);
                UpdateSupplierInvoices(customResponse, requestParams, declarationId, context);
                UpdateDeclarationCourierStatus(requestParams, context, declarationPM);
                UpdateConsignmentPackages(customResponse, requestParams, declarationId, context);
            }


			if (declarationPM.IsCourierDeclaration && declarationPM.ChangeSetOp == ChangeSetOperation.Update)
			{
				if (isUpdated)
				{
					declarationPM.ManifestCargoStatusCode = null;
					declarationPM.ChangeSetOp = ChangeSetOperation.Update;
					DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
					declarationUpdateService.Update(declarationPM, true);
				}
			}

		}

		private void UpdateInvoiceitems(DCAInUCBMultiUpdateWithResponseContentHeader customResponse, DeclarationPM declarationPM, ICustomContext context, GenericRequestParams requestParams)
        {
            var mySupplierInvoiceItemQueryService = new SupplierInvoiceItemQueryService(context);
            var mySupplierInvioceItemCertificatQueryService = new SupplierInvioceItemCertificatQueryService(context);
            var mySupplierInvoiceItemProcesTypeQueryService = new SupplierInvoiceItemProcesTypeQueryService(context);
            SupplierInvoiceItemUpdateService updateService = new SupplierInvoiceItemUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);

            var supplierInvoiceItems = (customResponse.ClassificationCode != null && !isFromPendingView) ? // Update Classification no - will update only the recored with the same classification
                mySupplierInvoiceItemQueryService.GetSupplierInvoiceItemByClassificationCode(declarationPM.Id, declarationPM.Tenant, customResponse.ClassificationCode) :
                mySupplierInvoiceItemQueryService.GetSupplierInvoiceItemsForMultiUpdate(declarationPM.Id, declarationPM.Tenant);  // Update ALL

            foreach (var item in supplierInvoiceItems)
            {
                SupplierInvoiceItemPM supplierInvoiceItem = mySupplierInvoiceItemQueryService.GetSingleSupplierInvoicePMBySequence(item.DeclarationId, item.CounterKey, (int)item.SequenceNumeric);
                supplierInvoiceItem.SupplierInvioceItemCertificats = mySupplierInvioceItemCertificatQueryService.GetSupplierInvioceItemCertificatesForSupplierInvoiceItem(item.DeclarationId, item.CounterKey, supplierInvoiceItem.LineNumber, supplierInvoiceItem.Tenant);

                UpdateInvoiceItemProcessTypeCode(customResponse, mySupplierInvoiceItemProcesTypeQueryService, declarationPM, supplierInvoiceItem);
                UpdateInvoiceItemTaxExemptCode(customResponse, supplierInvoiceItem);

                if (isFromPendingView) // only from pendingview, dont update from invoiceview
                {
                    UpdateInvoiceItemClassificationCode(customResponse, supplierInvoiceItem);
                    UpdateInvoiceItemCurrencyTypeCode(customResponse, supplierInvoiceItem);
                    UpdateInvoiceItemPrice(customResponse, supplierInvoiceItem);
                    UpdateInvoiceItemInvoiceQuantity(customResponse, supplierInvoiceItem);
                    UpdateInvoiceItemInvoiceQuantityType(customResponse, supplierInvoiceItem);
                }

                LogMessagingUtil.Instance.AppendLine("updating invoice (CounterKey,LineNumber):" + supplierInvoiceItem.CounterKey + "," + supplierInvoiceItem.LineNumber);
               
                if (!isFromPendingView) supplierInvoiceItem.ItemAdditionalStatus = true;

                updateService.Update(supplierInvoiceItem, true);

                this.MyRequestSheetParam.CustomFileNo = declarationPM.CustomFileNo;
                this.MyResponseData.UserMessage += "";
                this.MyResponseData.ApplicationID = declarationPM.CustomFileNo;
            }
        }

        private void UpdateInvoiceItemProcessTypeCode(DCAInUCBMultiUpdateWithResponseContentHeader customResponse, SupplierInvoiceItemProcesTypeQueryService mySupplierInvoiceItemProcesTypeQueryService, DeclarationPM declarationPM, SupplierInvoiceItemPM invoice)
        {
            if (customResponse.ProcessTypeCode == null || isFromPendingView) // only from invoiceview, dont update from pendingview
                return;

            var procestypes = mySupplierInvoiceItemProcesTypeQueryService.GetSupplierInvoiceItemProcesTypesForSupplierInvoiceItem(declarationPM.Id, invoice.CounterKey, invoice.LineNumber, invoice.Tenant);
            bool procestypesExist = procestypes.Any(x => x.ProcessTypeCode == customResponse.ProcessTypeCode);

            if (!procestypesExist)
            {
				isUpdated = true;
				var entity = new SupplierInvoiceItemProcesTypePM();
                entity.ChangeSetOp = ChangeSetOperation.Insert;
                entity.Tenant = customResponse.tenant;
                entity.ProcessTypeCode = customResponse.ProcessTypeCode;
                invoice.SupplierInvoiceItemProcesTypes.Add(entity);
                invoice.SupplierInvoiceItemsProcessTypeLastLineNumber = mySupplierInvoiceItemProcesTypeQueryService.GetMaxLineNumber(invoice.DeclarationId, invoice.CounterKey, invoice.LineNumber, customResponse.tenant);
                invoice.ChangeSetOp = ChangeSetOperation.Update;
            }
        }

        private void UpdateInvoiceItemTaxExemptCode(DCAInUCBMultiUpdateWithResponseContentHeader customResponse, SupplierInvoiceItemPM invoice)
        {
            if (customResponse.TaxExemptCode == null)
                return;
			isUpdated = true;
			invoice.TaxExemptCode = customResponse.TaxExemptCode;
            invoice.ChangeSetOp = ChangeSetOperation.Update;
        }

        private void UpdateInvoiceItemClassificationCode(DCAInUCBMultiUpdateWithResponseContentHeader customResponse, SupplierInvoiceItemPM invoice)
        {
            if (customResponse.ClassificationCode == null)
                return;

			isUpdated = true;
			LogMessagingUtil.Instance.AppendLine("set ClassificationCode");

            invoice.ClassificationCode = customResponse.ClassificationCode;

            CustomsItemQueryService customsItemQueryService = new CustomsItemQueryService(customResponse.tenant);
            LogMessagingUtil.Instance.AppendLine("prev InvoiceQuantityType = " + invoice.InvoiceQuantityType);

            invoice.InvoiceQuantityType = customsItemQueryService.GetQuantityTypeByClassificationCode(customResponse.ClassificationCode, customResponse.tenant);
            LogMessagingUtil.Instance.AppendLine("set InvoiceQuantityType = " + invoice.InvoiceQuantityType);

            invoice.ChangeSetOp = ChangeSetOperation.Update;
        }

        private void UpdateInvoiceItemCurrencyTypeCode(DCAInUCBMultiUpdateWithResponseContentHeader customResponse, SupplierInvoiceItemPM supplierInvoiceItem)
        {
            if (customResponse.InvoiceCurrencyTypeCode == null)
                return;

			isUpdated = true;
			supplierInvoiceItem.ItemPriceCurrencyCode = customResponse.InvoiceCurrencyTypeCode;
            supplierInvoiceItem.ChangeSetOp = ChangeSetOperation.Update;
        }

        private void UpdateInvoiceItemPrice(DCAInUCBMultiUpdateWithResponseContentHeader customResponse, SupplierInvoiceItemPM supplierInvoiceItem)
        {
            if (customResponse.InvoiceAmount == null)
                return;

			isUpdated = true;
			supplierInvoiceItem.ItemPrice = customResponse.InvoiceAmount;
            supplierInvoiceItem.ChangeSetOp = ChangeSetOperation.Update;
        }

        private void UpdateInvoiceItemInvoiceQuantity(DCAInUCBMultiUpdateWithResponseContentHeader customResponse, SupplierInvoiceItemPM supplierInvoiceItem)
        {
            if (customResponse.InvoiceQuantity == null)
                return;

			isUpdated = true;
			supplierInvoiceItem.InvoiceQuantity = customResponse.InvoiceQuantity;
            supplierInvoiceItem.ChangeSetOp = ChangeSetOperation.Update;
        }

        private void UpdateInvoiceItemInvoiceQuantityType(DCAInUCBMultiUpdateWithResponseContentHeader customResponse, SupplierInvoiceItemPM supplierInvoiceItem)
        {
            if (customResponse.InvoiceQuantityType == null)
                return;

			isUpdated = true;
			supplierInvoiceItem.InvoiceQuantityType = customResponse.InvoiceQuantityType;
            supplierInvoiceItem.ChangeSetOp = ChangeSetOperation.Update;
        }

        private void UpdateDeclaration(DCAInUCBMultiUpdateWithResponseContentHeader customResponse, GenericRequestParams requestParams, ICustomContext context, DeclarationPM declarationPM)
        {
            if (customResponse.ProcessTypeCode == null)
                return;
			isUpdated = true;
			LogMessagingUtil.Instance.AppendLine("set declaration.ProcessTypeCode");
            declarationPM.ProcedureCurrentCode = customResponse.ProcessTypeCode;
            declarationPM.ChangeSetOp = ChangeSetOperation.Update;
            DeclarationUpdateService declarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            declarationUpdateService.Update(declarationPM, true);
        }

        private void UpdateSupplierInvoices(DCAInUCBMultiUpdateWithResponseContentHeader customResponse, GenericRequestParams requestParams, string declarationId, ICustomContext context)
        {
            var supplierInvoiceQueryServices = new SupplierInvoiceQueryService(context);
            var supplierInvoiceItemUpdateService = new SupplierInvoiceUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            var supplierInvoices = supplierInvoiceQueryServices.GetSupplierInvoicesForDeclaration(declarationId, requestParams.Tenant);

            foreach (var supplierInvoice in supplierInvoices)
            {
                UpdateInvoiceCurrencyTypeCode(customResponse, supplierInvoice);
                UpdateInvoiceInvoiceAmount(customResponse, supplierInvoice);

                supplierInvoiceItemUpdateService.Update(supplierInvoice, true);
            }
        }

        private void UpdateInvoiceCurrencyTypeCode(DCAInUCBMultiUpdateWithResponseContentHeader customResponse, SupplierInvoicePM supplierInvoice)
        {
            if (customResponse.InvoiceCurrencyTypeCode == null)
                return;
			isUpdated = true;
			supplierInvoice.InvoiceCurrencyTypeCode = customResponse.InvoiceCurrencyTypeCode;
            supplierInvoice.ChangeSetOp = ChangeSetOperation.Update;
        }

        private void UpdateInvoiceInvoiceAmount(DCAInUCBMultiUpdateWithResponseContentHeader customResponse, SupplierInvoicePM supplierInvoice)
        {
            if (customResponse.InvoiceAmount == null)
                return;
			isUpdated = true;
			supplierInvoice.InvoiceAmount = customResponse.InvoiceAmount;
            supplierInvoice.ChangeSetOp = ChangeSetOperation.Update;
        }

        private void UpdateDeclarationCourierStatus(GenericRequestParams requestParams, ICustomContext context, DeclarationPM declarationPM)
        {
            var declarationCourierStatusQueryService = new DeclarationCourierStatusQueryService(context);
            var declarationCourierStatusPM = declarationCourierStatusQueryService.GetSingle(declarationPM.Id, false, false);
            new CalculateDeclarationCourierStatus(declarationPM, declarationPM.Id, requestParams.Tenant).CalcTotalInvoiceAmountInUSD(declarationCourierStatusPM);
        }

        private void UpdateConsignmentPackages(DCAInUCBMultiUpdateWithResponseContentHeader customResponse, GenericRequestParams requestParams, string declarationId, ICustomContext context)
        {
            if (customResponse.GrossMassMeasure == null)
                return;

            List<ConsignmentPackagePM> consignmentPackages = new ConsignmentPackageQueryService(context).GetConsignmentPackagesForDeclaration(declarationId);
            var consignmentPackageUpdateService = new ConsignmentPackageUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);

            foreach (var consignmentPackagePM in consignmentPackages)
            {
				isUpdated = true;
				consignmentPackagePM.GrossMassMeasure = customResponse.GrossMassMeasure;
                consignmentPackagePM.ChangeSetOp = ChangeSetOperation.Update;

                consignmentPackageUpdateService.Update(consignmentPackagePM, true);
            }
        }
    }
}
