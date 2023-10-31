using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Metadata;
using Logitude.Customs.Data.EntityMapping;
using Logitude.Server.Tools.Helpers;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomsDocumentsTicketQueryService
    {
        public override void GetComposition(Simplog.Server.Infrastructure.EntityKeyFields entityKeys, CustomsDocumentsTicketPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            CustomsDocumentsTicketKeys customsDocumentsTicketKeys = entityKeys as CustomsDocumentsTicketKeys;
            CustomsDocumentPointerQueryService pointerQueryService = new CustomsDocumentPointerQueryService(context);

            entityPM.CustomsDocumentPointers = pointerQueryService.GetPointersForTicket(entityPM.Id, entityPM.Tenant);

        }
 
       
        public int CheckRequestedCustomsDocIdsByEntityIdAndChilds(string entityId , int tenant, string parentEntityCode, string requestedCustomsDocId)
        {


            List<CustomsDocumentsTicketPM> customsDocumentsTicketPMList = GetCustomsDocumentsTicketPMsByEntityIdAndChilds(entityId, "", "", "", tenant, "Declaration");
            //var test = customsDocumentsTicketPMList.Where(x => string.IsNullOrEmpty(x.VerificationStatusTypeCode) && !string.IsNullOrEmpty(x.RequestedCustomsDocId) && x.RequestedCustomsDocId != requestedCustomsDocId);
            if (customsDocumentsTicketPMList.Count(x=>string.IsNullOrEmpty(x.VerificationStatusTypeCode) && !string.IsNullOrEmpty(x.RequestedCustomsDocId) && x.RequestedCustomsDocId != requestedCustomsDocId) >0 )
            {
                return 1;
            }


            return 0;
        }

         public List<CustomsDocumentsTicketPM> GetCustomsDocumentsTicketPMsByEntityIdAndChilds(string entityId, string child1EntityId, string child2EntityId, string child3EntityId, int tenant, string parentEntityCode, bool isAir = false)
        {
            LogMessagingUtil.Instance.AppendLine("GetCustomsDocumentsTicketPMsByEntityIdAndChilds" );

            List<CustomsDocumentsTicket> tickets = repository.GetCustomsDocumentsTicketPMsByEntityIdAndChilds(entityId, child1EntityId, child2EntityId, child3EntityId,tenant,parentEntityCode,isAir);
            ICustomContext context = MainContext as CustomContext;
             CustomsDocumentPointerQueryService pointerQueryService = new CustomsDocumentPointerQueryService(context);
            
            List<CustomsDocumentsTicketPM> ticketPMs = (from a in tickets
                                                        //select new CustomsDocumentsTicketPM()
                                                        //{
                                                        //    Id = a.Id,
                                                        //    DocumentTypeCode = a.DocumentTypeCode,
                                                        //    Tenant = a.Tenant,
                                                        //    DocumentsFilingId = a.DocumentsFilingId,
                                                        //    RequestedCustomsDocId = a.RequestedCustomsDocId,
                                                        //    Remarks=a.Remarks,
                                                        //     VerificationRemarks=a.VerificationRemarks,
                                                        //     VerificationStatusTypeCode=a.VerificationStatusTypeCode,
                                                        //      UserRemarks = a.UserRemarks,
                                                        //}
                                                        select this.GetEntityPM(a)
                                                        ).ToList();
            List<string> ids=(from a in ticketPMs
                              select a.Id).ToList();

            List<CustomsDocumentPointerPM> pointerPMs = pointerQueryService.GetPointersForMultipleTickets(ids, tenant);
            List<int> supplierInvoiceKeys = new List<int>();
            List<int> supplierInvoiceItemKeys = new List<int>();
            List<int> supplierInvoiceKeysForItems = new List<int>();
            List<int> claimsRelatedEntityKeys = new List<int>();
            foreach (CustomsDocumentPointerPM pointer in pointerPMs)
            {
                LogMessagingUtil.Instance.AppendLine("pointer.DocumentTypeCode" + pointer.DocumentTypeCode);

                if (pointer.ParentEntityCode == "Declaration")
                {
                    if (!string.IsNullOrEmpty(pointer.Child1EntityId) && string.IsNullOrEmpty(pointer.Child2EntityId))
                    {
                        int invoiceKey;
                        int.TryParse(pointer.Child1EntityId, out invoiceKey);
                        supplierInvoiceKeys.Add(invoiceKey);
                    }

                    else if (!string.IsNullOrEmpty(pointer.Child1EntityId) && !string.IsNullOrEmpty(pointer.Child2EntityId))
                    {
                        int invoiceKey;
                        int.TryParse(pointer.Child1EntityId, out invoiceKey);
                        int invoiceItemKey;
                        int.TryParse(pointer.Child2EntityId, out invoiceItemKey);

                        supplierInvoiceItemKeys.Add(invoiceItemKey);
                        supplierInvoiceKeysForItems.Add(invoiceKey);
                    }
                }
                else if(pointer.ParentEntityCode=="Claim")
                {
                    if (!string.IsNullOrEmpty(pointer.Child1EntityId))
                    {
                        int creKey;
                        int.TryParse(pointer.Child1EntityId, out creKey);
                        claimsRelatedEntityKeys.Add(creKey);
                    }
                }
               
            }


            List<SupplierInvoicePM> supplierInvoicePMs = null;
            List<SupplierInvoiceItemPM> supplierInvoiceItemPMs = null;
            List<ClaimsRelatedEntityPM> crePMs = null;
            if (supplierInvoiceItemKeys.Count > 0)
            {
                SupplierInvoiceItemQueryService invoiceItemQueryService = new SupplierInvoiceItemQueryService(context);
                supplierInvoiceItemPMs = invoiceItemQueryService.GetSupplierInvoiceItemsByCounterKeys(entityId, supplierInvoiceKeysForItems, supplierInvoiceItemKeys, tenant);
                supplierInvoiceKeys = supplierInvoiceKeys.Concat(supplierInvoiceKeysForItems).ToList();
            }

            if (supplierInvoiceKeys.Count > 0)
            {
                LogMessagingUtil.Instance.AppendLine("GetSupplierInvoicesByCounterKeys(entityId, supplierInvoiceKeys, tenant);" + entityId+ supplierInvoiceKeys);

                SupplierInvoiceQueryService invoiceQueryService = new SupplierInvoiceQueryService(context);
                supplierInvoicePMs = invoiceQueryService.GetSupplierInvoicesByCounterKeys(entityId, supplierInvoiceKeys, tenant);
            }

            if (claimsRelatedEntityKeys.Count > 0)
            {
                ClaimsRelatedEntityQueryService creQueryService = new ClaimsRelatedEntityQueryService(context);
                crePMs = creQueryService.GetCREsByCounterKeys(entityId, claimsRelatedEntityKeys, tenant);
            }

            foreach (CustomsDocumentsTicketPM ticket in ticketPMs)
            {
                ticket.CustomsDocumentPointers = (from a in pointerPMs
                                                  where a.CustomsDocumentsTicketId == ticket.Id
                                                  select a).ToList();

                if (supplierInvoicePMs != null || supplierInvoiceItemPMs!=null)
                {
                    foreach (CustomsDocumentPointerPM pointerPM in ticket.CustomsDocumentPointers)
                    {
                        if (supplierInvoicePMs != null)
                        {
                            if (string.IsNullOrEmpty(pointerPM.Child2EntityId))
                            {
                                SupplierInvoicePM invoicepm = supplierInvoicePMs.FirstOrDefault(d => d.InvoiceCounterKey.ToString() == pointerPM.Child1EntityId);
                                if (invoicepm != null)
                                {
                                    ticket.ConnectedInvoicesSequences = ticket.ConnectedInvoicesSequences + "," + invoicepm.SequenceNumeric;
                                }
                            }
                        }
                        if (supplierInvoiceItemPMs != null)
                        {
                            SupplierInvoiceItemPM invoiceItempm = supplierInvoiceItemPMs.FirstOrDefault(d => d.CounterKey.ToString() == pointerPM.Child1EntityId && d.LineNumber.ToString() == pointerPM.Child2EntityId);
                          
                            if (invoiceItempm != null)
                            {
                                SupplierInvoicePM invoicepm = supplierInvoicePMs.FirstOrDefault(d => d.InvoiceCounterKey == invoiceItempm.CounterKey);

                                ticket.ConnectedInvoiceItemsSequences = ticket.ConnectedInvoiceItemsSequences + "," + invoiceItempm.SequenceNumeric;
                                bool exists = false;
                                if (invoicepm != null)
                                {
                                    if (ticket.ConnectedInvoicesSequences != null)
                                    {
                                        string[] connectedInvoices = ticket.ConnectedInvoicesSequences.Split(',');
                                        if (connectedInvoices.Contains(invoicepm.SequenceNumeric.ToString()))
                                        {
                                            exists = true;
                                        }
                                    }
                                    if (!exists && ticket.ConnectedInvoicesSequences!=null && !ticket.ConnectedInvoicesSequences.Contains("," + invoicepm.SequenceNumeric + ",") && !ticket.ConnectedInvoicesSequences.StartsWith(invoicepm.SequenceNumeric+",") && !ticket.ConnectedInvoicesSequences.EndsWith(","+invoicepm.SequenceNumeric))
                                    {
                                        ticket.ConnectedInvoicesSequences = ticket.ConnectedInvoicesSequences + "," + invoicepm.SequenceNumeric;
                                    }
                                    else if (ticket.ConnectedInvoicesSequences == null)
                                    {
                                        ticket.ConnectedInvoicesSequences = invoicepm.SequenceNumeric.ToString();
                                    }
                                }
                            }
                        }
                        pointerPM.DocumentTypeCode = ticket.DocumentTypeCode;
                    }
                    
                }
                else if (crePMs != null)
                {
                    foreach (CustomsDocumentPointerPM pointerPM in ticket.CustomsDocumentPointers)
                    {
                        ClaimsRelatedEntityPM crepm = crePMs.FirstOrDefault(d => d.EntityCounterKey.ToString() == pointerPM.Child1EntityId);
                        if (crepm != null)
                        {
                            ticket.ConnectedCREsSequences = ticket.ConnectedCREsSequences + "," + crepm.EntityCounterKey;
                        }
                        pointerPM.DocumentTypeCode = ticket.DocumentTypeCode;
                    }
                }
                if (!string.IsNullOrEmpty(ticket.ConnectedInvoicesSequences))
                {
                    ticket.ConnectedInvoicesSequences = ticket.ConnectedInvoicesSequences.TrimStart(',');
                }
                if (!string.IsNullOrEmpty(ticket.ConnectedInvoiceItemsSequences))
                {
                    ticket.ConnectedInvoiceItemsSequences = ticket.ConnectedInvoiceItemsSequences.TrimStart(',');
                }
                if (!string.IsNullOrEmpty(ticket.ConnectedCREsSequences))
                {
                    ticket.ConnectedCREsSequences = ticket.ConnectedCREsSequences.TrimStart(',');
                }

                if (!string.IsNullOrEmpty(ticket.DocumentsFilingId))
                {
                    CustomsDocumentQueryService customsDocumentQueryService = new CustomsDocumentQueryService(tenant);
                    CustomsDocumentPM document = customsDocumentQueryService.GetSingle(ticket.DocumentsFilingId, false, false);
                    if (document != null)
                    {
                        ticket.DocumentRemarks = document.DocumentRemarks;
                        ticket.DocumentStatusCode = document.DocumentStatusCode;
                        ticket.DocumentStatusName = document.DocumentStatusName;
                        ticket.Extension = document.Extension;
                        ticket.FileSize = document.FileSize;
                        ticket.Name = document.Name;
                        ticket.IsMetaDataReady = document.IsMetaDataReady;
                        ticket.CustomsDocId = document.CustomsDocId;
                        ticket.ExternalAttachmentId = document.ExternalAttachmentId;
                        ticket.IsDigitallySigned = document.IsDigitallySigned;
                        ticket.SignersList = document.SignersList;
                    }
                }
              

            }
            return ticketPMs;
        }

        public List<CustomsDocumentsTicketPM> GetCustomsDocumentsTickets(GetTicketsParams parameters, int tenant)
        {
            List<CustomsDocumentsTicket> tickets = repository.GetCustomsDocumentTickets(parameters, tenant);
            
            List<CustomsDocumentsTicketPM> ticketPMs = (from a in tickets
                                                        //select new CustomsDocumentsTicketPM()
                                                        //{
                                                        //    Id = a.Id,
                                                        //    DocumentTypeCode = a.DocumentTypeCode,
                                                        //    Tenant = a.Tenant,
                                                        //    DocumentsFilingId = a.DocumentsFilingId,
                                                        //    RequestedCustomsDocId = a.RequestedCustomsDocId,
                                                        //}
                                                        select this.GetEntityPM(a)
                                                        ).ToList();
            return ticketPMs;
        }

        public List<CustomsDocumentsTicketPM> GetCustomsDocumentsTicketsByDocumentsFilingId(string documentsFilingId, int tenant)
        {
            List<CustomsDocumentsTicket> tickets = repository.GetCustomsDocumentsTicketsByDocumentsFilingId(documentsFilingId, tenant);

            ICustomContext context = MainContext as CustomContext;
            CustomsDocumentPointerQueryService pointerQueryService = new CustomsDocumentPointerQueryService(context);
            List<CustomsDocumentsTicketPM> ticketPMs = (from a in tickets
                                                        select new CustomsDocumentsTicketPM()
                                                        {
                                                            Id = a.Id,
                                                            DocumentTypeCode = a.DocumentTypeCode,
                                                            Tenant = a.Tenant,
                                                            DocumentsFilingId = a.DocumentsFilingId,
                                                            RequestedCustomsDocId = a.RequestedCustomsDocId,
                                                            Remarks = a.Remarks,
                                                            VerificationRemarks = a.VerificationRemarks,
                                                            VerificationStatusTypeCode = a.VerificationStatusTypeCode,
                                                            UserRemarks = a.UserRemarks
                                                        }).ToList();
            List<string> ids = (from a in ticketPMs
                                select a.Id).ToList();

            List<CustomsDocumentPointerPM> pointerPMs = pointerQueryService.GetPointersForMultipleTickets(ids, tenant);
            foreach (CustomsDocumentsTicketPM ticket in ticketPMs)
            {
                ticket.CustomsDocumentPointers = (from a in pointerPMs
                                                  where a.CustomsDocumentsTicketId == ticket.Id
                                                  select a).ToList();
                if (!string.IsNullOrEmpty(ticket.DocumentsFilingId))
                {
                    CustomsDocumentQueryService customsDocumentQueryService = new CustomsDocumentQueryService(tenant);
                    CustomsDocumentPM document = customsDocumentQueryService.GetSingle(ticket.DocumentsFilingId, false, false);
                    if (document != null)
                    {
                        ticket.DocumentRemarks = document.DocumentRemarks;
                        ticket.DocumentStatusCode = document.DocumentStatusCode;
                        ticket.DocumentStatusName = document.DocumentStatusName;
                        ticket.Extension = document.Extension;
                        ticket.FileSize = document.FileSize;
                        ticket.Name = document.Name;
                        ticket.IsMetaDataReady = document.IsMetaDataReady;
                        ticket.CustomsDocId = document.CustomsDocId;
                        ticket.ExternalAttachmentId = document.ExternalAttachmentId;
                    }
                }

            }
            return ticketPMs;
        }

        public List<CustomsDocumentsTicketPM> GetCustomsDocumentsTicketsIds(string ticketIds, int tenant)
        {
            List<CustomsDocumentsTicket> tickets = repository.GetCustomsDocumentsTicketsIds(ticketIds, tenant);
            
            List<CustomsDocumentsTicketPM> ticketPMs = (from a in tickets
                                                        select new CustomsDocumentsTicketPM()
                                                        {
                                                            Id = a.Id,
                                                            DocumentTypeCode = a.DocumentTypeCode,
                                                            Tenant = a.Tenant,
                                                            DocumentsFilingId = a.DocumentsFilingId,
                                                            RequestedCustomsDocId = a.RequestedCustomsDocId,
                                                            Remarks = a.Remarks,
                                                            VerificationRemarks = a.VerificationRemarks,
                                                            VerificationStatusTypeCode = a.VerificationStatusTypeCode,
                                                            UserRemarks = a.UserRemarks
                                                        }).ToList();
        
            return ticketPMs;
        }
        public List<string> GetIsConnectDec(string documentsfilingid, string entityId)
        {
          return  repository.GetIsConnectDec(documentsfilingid, entityId);
        }
        public List<string> GetDocConnectTicket(string documentsfilingid, string entityId, int tenant)
        {
            return repository.GetDocConnectTicket(documentsfilingid, entityId, tenant);
        }

        public bool IsSendToCustomsAndNotConnectTicket(string documentsfilingid, string entityId, int tenant)
        {
            return repository.IsSendToCustomsAndNotConnectTicket(documentsfilingid, entityId, tenant);
        }
        public bool GetIfThereRequestDocumentDocIdNotVerifiedByDeclarationId(string declarationId)
        {
            return repository.GetIfThereRequestDocumentDocIdNotVerifiedByDeclarationId(declarationId);
        }

        public int GetCountOfTicketsByDocFilingId(string documentsfilingid, int tenant)
        {
            if(string.IsNullOrEmpty(documentsfilingid)) {  return 0; }
            return repository.GetCountOfTicketsByDocFilingId(documentsfilingid, tenant);
        }
    }
}
