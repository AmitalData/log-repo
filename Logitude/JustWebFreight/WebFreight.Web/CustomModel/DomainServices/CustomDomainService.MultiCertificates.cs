using Logitude.AmitalMessaging.Customs.CustomFile;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.L2U.CustomFile;
using Logitude.Customs.Data;
using Logitude.Customs.Data.DataContracts;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using Unifreight.BL.EntityPMs.UGenerated;
//using Unifreight.BL.EntityQueryServices;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {

        public List<CertificateTicket> GetCertificateTickets(string declarationId,string reqConfirmationType, string invoiceNumber, int? invoiceCounterKey, string demandState, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);



            SupplierInvioceItemCertificatQueryService queryService = new SupplierInvioceItemCertificatQueryService(customContext);
            List<CertificateTicket> tickets = queryService.GetDeclarationCertificateTicket(declarationId,reqConfirmationType, invoiceNumber, invoiceCounterKey,demandState, tenant).ToList();



            return tickets;

        }

        //public List<SupplierInvioceItemCertificatPM> GetDeclarationCertificates(string declarationId, int tenant)
        //{
        //    customContext = CustomContext.GetContext(tenant);

        //    SupplierInvioceItemCertificatQueryService queryService = new SupplierInvioceItemCertificatQueryService(customContext);
        //    List<SupplierInvioceItemCertificatPM> certificates = queryService.GetDeclarationCertificates(declarationId, tenant);
        //    return certificates;
        //}

        public List<CertificateConnectedItems> GetTicketConnectedItems( string declarationId, string attachmentTypeCode, string reqConfirmationTypeCode, string CertificateExemptionTypeCode, string CertificateNumber, string ResConfirmationTypeCode, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);



            SupplierInvioceItemCertificatQueryService queryService = new SupplierInvioceItemCertificatQueryService(customContext);

            List<CertificateConnectedItems> connectedItems = queryService.GetCertificateConnectedItemsList(declarationId, attachmentTypeCode, reqConfirmationTypeCode, CertificateExemptionTypeCode, CertificateNumber, ResConfirmationTypeCode, tenant);

            return connectedItems;
        }

       [Query(HasSideEffects = true)]
        public List<CertificateConnectedItems> GetSupplierInvocieItemsForTicket(byte[] xmlFilters, string declarationId, string attachmentTypeCode, string reqConfirmationTypeCode, string CertificateExemptionTypeCode, string CertificateNumber, string ResConfirmationTypeCode, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);

       

            SupplierInvioceItemCertificatQueryService queryService = new SupplierInvioceItemCertificatQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            List<CertificateConnectedItems> connectedItems = queryService.GetCertificateConnectedItems(queryOperations, declarationId, attachmentTypeCode,  reqConfirmationTypeCode,  CertificateExemptionTypeCode,  CertificateNumber,  ResConfirmationTypeCode, tenant);

            return connectedItems;
        }


       public int GetCertificateTicketConnectedItemsCount(byte[] xmlFilters, string declarationId, string attachmentTypeCode, string reqConfirmationTypeCode, string CertificateExemptionTypeCode, string CertificateNumber, string ResConfirmationTypeCode, int tenant)
        {

            customContext = CustomContext.GetContext(tenant);
            SupplierInvioceItemCertificatQueryService queryService = new SupplierInvioceItemCertificatQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations,declarationId,  attachmentTypeCode,  reqConfirmationTypeCode,  CertificateExemptionTypeCode,  CertificateNumber,  ResConfirmationTypeCode,  tenant);
        }
      
        public List<CertificateConnectedItems> GetDeclarationInvoicesNumbers(string DeclarationId, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            SupplierInvioceItemCertificatQueryService queryService = new SupplierInvioceItemCertificatQueryService(customContext);
            List<CertificateConnectedItems> numbers = queryService.GetDeclarationInvoicesNumbers(DeclarationId, tenant);
            return numbers;
        }

        public void GetUnifreightCertificateForDeclaration(string declarationId, string customerCode, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            List<CertificateTicket> tickets = new List<CertificateTicket>();

            //First, Check whether the client has permission (Check Default "CIM_CERTSELFRES"- Required certifications self responsibility)
            string isRequiredCertificate = GetDefault("ISRAEL", "CIM_CERTSELFRES", "NON", customerCode, tenant);
            if (isRequiredCertificate == "Y")
            {
                return;
            }

            //Get all items (supplierInvoiceItems) that do not have certificates
            SupplierInvioceItemCertificatQueryService queryService = new SupplierInvioceItemCertificatQueryService(customContext);
            List<CertificateGroupItems> supplierInvoiceItems = queryService.GetCertificateGroupForDeclaration(declarationId, tenant);
            
            if (supplierInvoiceItems != null && supplierInvoiceItems.Count() > 0)
            {
                var myCustomCertificatesService = new CustomCertificatesService(declarationId, tenant);

                for (var i = 0; i < supplierInvoiceItems.Count(); i += 50)
                {
                    int counter = 50;
                    if (counter > supplierInvoiceItems.Count() - i)
                    {
                        counter = supplierInvoiceItems.Count() - i;
                    }
                    //Send Message to Unifreight (Groups of 50) to get certificates
                    List<CertificateGroupItems> groupList = supplierInvoiceItems.GetRange(i, counter);
                    CUSTOMCERTIFICATES_UL certificateResponseData = myCustomCertificatesService.GetUnifreightCertificateForDeclaration(groupList);

                    //Save Unifreight Certificates (= Tickets)
                    SaveUnifreightCertificates(groupList, certificateResponseData, customContext, tenant);
                }
            }

            return ;
        }

        private void SaveUnifreightCertificates(List<CertificateGroupItems> groupList, CUSTOMCERTIFICATES_UL certificateResponseData, ICustomContext customContext, int tenant)
        {
            if (certificateResponseData == null || certificateResponseData.CUSTOMCERTIFICATES_ULAMITAL == null 
                || certificateResponseData.CUSTOMCERTIFICATES_ULAMITAL.FirstOrDefault().CertificateGroupResponse == null
                || certificateResponseData.CUSTOMCERTIFICATES_ULAMITAL.FirstOrDefault().CertificateGroupResponse.Count() == 0)
            {
                return;
            }
            SupplierInvioceItemCertificatUpdateService mySupplierInvioceItemCertificatUpdateService = new SupplierInvioceItemCertificatUpdateService(customContext, new Dictionary<string, IContext>(), tenant);

            foreach (CertificateGroupResponse certificateGroupItem in certificateResponseData.CUSTOMCERTIFICATES_ULAMITAL.FirstOrDefault().CertificateGroupResponse)
            {
                CertificateGroupItems certificateGroup = (from a in groupList
                                                              where (a.ClassificationCode == certificateGroupItem.ClassificationCode
                                                                && a.ItemCode == certificateGroupItem.ItemCode
                                                                && a.OriginCountryCode == certificateGroupItem.OriginCountryCode
                                                                && a.VendorNumber == certificateGroupItem.VendorNumber)
                                                                select a).FirstOrDefault();

                foreach (var supplierItem in certificateGroup.SupplierInvoiceItems)
                {
                    foreach (var certificateItem in certificateGroupItem.CertificateDetails)
                    {
                        SupplierInvioceItemCertificatPM supplierInvioceItemCertificatPM = new SupplierInvioceItemCertificatPM();
                        supplierInvioceItemCertificatPM.ChangeSetOp = ChangeSetOperation.Update;
                        supplierInvioceItemCertificatPM.DeclarationId = supplierItem.DeclarationId;
                        supplierInvioceItemCertificatPM.Tenant = tenant;
                        supplierInvioceItemCertificatPM.InvoiceCounterKey = supplierItem.CounterKey;
                        supplierInvioceItemCertificatPM.LineNumber = supplierItem.LineNumber;
                        supplierInvioceItemCertificatPM.CertificateNumber = certificateItem.CertificateNumber;
                        supplierInvioceItemCertificatPM.ResConfirmationTypeCode = certificateItem.ResponseConfirmationTypeCode;
                        supplierInvioceItemCertificatPM.ReqConfirmationTypeCode = certificateItem.RequestConfirmationTypeCode;
                        supplierInvioceItemCertificatPM.ExternalCertificatCode = certificateItem.ExternalCertificatCode;
                        mySupplierInvioceItemCertificatUpdateService.Update(supplierInvioceItemCertificatPM,true);
                    }
                }
            }

        }

        [Invoke]
        public void UpdateCertificateTickets(List<CertificateConnectedItems> items, string declarationId, string invoiceNumber, string attachmentTypeCode, string certificateNumber, string resConfirmationTypeCode, string certificateExemptionTypeCode, string reqConfirmationTypeCode, int tenant)
        {

            customContext = CustomContext.GetContext(tenant);
            SupplierInvioceItemCertificatUpdateService updateService = new SupplierInvioceItemCertificatUpdateService(customContext);
            updateService.UpdateCertificateConnectedItems(items,declarationId, invoiceNumber, attachmentTypeCode, certificateNumber, resConfirmationTypeCode, certificateExemptionTypeCode,reqConfirmationTypeCode, tenant);

        }


        [Invoke]
        public void UpdateSuppkierInvoiceItemCatalogNumber(string declarationId, string CatalogNumber,int counterKey,int lineNumber,  int tenant)
        {
             customContext = CustomContext.GetContext(tenant);
             SupplierInvoiceItemQueryService itemQueryService = new SupplierInvoiceItemQueryService(customContext);
             SupplierInvoiceItemPM itemPM = itemQueryService.GetSingle(declarationId, counterKey, lineNumber, false, true);
             SupplierInvoiceItemUpdateService updateService = new SupplierInvoiceItemUpdateService(customContext);
             itemPM.CatalogNumber = CatalogNumber;
             updateService.Update(itemPM, true);
        }

        public void UpdateCertificateTicket(CertificateTicket ticket)
        {
            
        }

        public void UpdateCertificateConnectedItems(CertificateConnectedItems connectedItems)
        {
           
        }

        [Invoke]
        public void UpdateInvoiceCertificats(CertificateTicket oldTicket, int tenat)
        {

        }


        [Invoke]
        public bool DeclarationHasInvoices (string declarationId, int tenant)
        {
             customContext = CustomContext.GetContext(tenant);
             SupplierInvoiceQueryService itemQueryService = new SupplierInvoiceQueryService(customContext);
             SupplierInvoiceItemQueryService invoiceItemQuery = new SupplierInvoiceItemQueryService(customContext);

             List<SupplierInvoicePM> invoicePMs = itemQueryService.GetSupplierInvoicesForDeclaration(declarationId, tenant);
             List<SupplierInvoiceItemPM> invoiceItemPMs = invoiceItemQuery.GetSupplierInvoiceItemsForDeclaration(declarationId, tenant);

             if (invoicePMs.Count == 0 || invoiceItemPMs.Count == 0)
             {
                 return false;
             }
             else
             {
                 return true;
             }
           
        }

    }
}

