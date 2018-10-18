
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityQueryServices;


namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class CustomsDocumentsTicketDataMapping: IMapping<CustomsDocumentsTicketPM, CustomsDocumentsTicket>
   {

        public void CustomPMToPOCO(CustomsDocumentsTicketPM entityPM, CustomsDocumentsTicket entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.Id);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }
        }

        public void CustomPOCOToPM(CustomsDocumentsTicketPM entityPM, CustomsDocumentsTicket entityPOCO)
        {

            if (!string.IsNullOrEmpty(entityPOCO.DocumentsFilingId))
            {
                // CustomDocument
                CustomsDocumentQueryService customsDocumentQueryService = new CustomsDocumentQueryService(entityPOCO.Tenant);
                CustomsDocumentPM document = customsDocumentQueryService.GetSingle(entityPOCO.DocumentsFilingId, false, false);
                if (document != null)
                {
                    entityPM.DocumentRemarks = document.DocumentRemarks;
                    entityPM.DocumentStatusCode = document.DocumentStatusCode;
                    entityPM.DocumentStatusName = document.DocumentStatusName;
                    entityPM.Extension = document.Extension;
                    entityPM.FileSize = document.FileSize;
                    entityPM.Name = document.Name;
                    entityPM.IsMetaDataReady = document.IsMetaDataReady;
                    entityPM.CustomsDocId = document.CustomsDocId;
                    entityPM.ExternalAttachmentId = document.ExternalAttachmentId;
                    entityPM.CustomsDocId = document.CustomsDocId;
                }

                ////DocumentFiling
                //DocumentsFilingQuery filingQuery = new DocumentsFilingQuery(entityPOCO.Tenant);
                //DocumentsFilingPM documentFiling = filingQuery.GetSinglePM(entityPOCO.DocumentsFilingId, entityPM.Tenant);
                //if (documentFiling != null)
                //{
                //    entityPM.IsDigitallySigned = documentFiling.IsDigitallySigned;
                //}
            }

            if (entityPOCO.VerificationStatusTypeCode != null)
            {
                CustomsVerificationStatusTypeQueryService customsVerificationStatusTypeQueryService = new CustomsVerificationStatusTypeQueryService(entityPOCO.Tenant);
                CustomsVerificationStatusTypePM customsDocumentStatusType = customsVerificationStatusTypeQueryService.GetSingle(entityPOCO.VerificationStatusTypeCode, false, true);
                entityPM.DocumentStatusName = customsDocumentStatusType.LocalName;
            }

            List<int> supplierInvoiceKeys = new List<int>();
            List<int> supplierInvoiceItemKeys = new List<int>();
            List<int> supplierInvoiceKeysForItems = new List<int>();
            List<int> claimsRelatedEntityKeys = new List<int>();
            string entityId="";
            foreach (CustomsDocumentPointerPM pointer in entityPM.CustomsDocumentPointers)
            {
                if (string.IsNullOrEmpty(entityId))
                {
                    entityId = pointer.ParentEntityId;
                }
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
                else if (pointer.ParentEntityCode == "Claim")
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
                SupplierInvoiceItemQueryService invoiceItemQueryService = new SupplierInvoiceItemQueryService(entityPOCO.Tenant);
                supplierInvoiceItemPMs = invoiceItemQueryService.GetSupplierInvoiceItemsByCounterKeys(entityId, supplierInvoiceKeysForItems, supplierInvoiceItemKeys, entityPOCO.Tenant);
                supplierInvoiceKeys = supplierInvoiceKeys.Concat(supplierInvoiceKeysForItems).ToList();
            }

            if (supplierInvoiceKeys.Count > 0)
            {
                SupplierInvoiceQueryService invoiceQueryService = new SupplierInvoiceQueryService(entityPOCO.Tenant);
                supplierInvoicePMs = invoiceQueryService.GetSupplierInvoicesByCounterKeys(entityId, supplierInvoiceKeys, entityPOCO.Tenant);
            }

            if (claimsRelatedEntityKeys.Count > 0)
            {
                ClaimsRelatedEntityQueryService creQueryService = new ClaimsRelatedEntityQueryService(entityPOCO.Tenant);
                crePMs = creQueryService.GetCREsByCounterKeys(entityId, claimsRelatedEntityKeys, entityPOCO.Tenant);
            }

            foreach (CustomsDocumentPointerPM pointerPM in entityPM.CustomsDocumentPointers)
            {
                if (supplierInvoicePMs != null)
                {
                    if (string.IsNullOrEmpty(pointerPM.Child2EntityId))
                    {
                        SupplierInvoicePM invoicepm = supplierInvoicePMs.FirstOrDefault(d => d.InvoiceCounterKey.ToString() == pointerPM.Child1EntityId);
                        if (invoicepm != null)
                        {
                            entityPM.ConnectedInvoicesSequences = entityPM.ConnectedInvoicesSequences + "," + invoicepm.SequenceNumeric;
                        }
                    }
                }
                if (supplierInvoiceItemPMs != null)
                {

                    SupplierInvoiceItemPM invoiceItempm = supplierInvoiceItemPMs.FirstOrDefault(d => d.CounterKey.ToString() == pointerPM.Child1EntityId && d.LineNumber.ToString() == pointerPM.Child2EntityId);

                    if (invoiceItempm != null)
                    {
                        SupplierInvoicePM invoicepm = supplierInvoicePMs.FirstOrDefault(d => d.InvoiceCounterKey == invoiceItempm.CounterKey);

                        entityPM.ConnectedInvoiceItemsSequences = entityPM.ConnectedInvoiceItemsSequences + "," + invoiceItempm.SequenceNumeric;
                        if (invoicepm != null)
                        {
                            if (entityPM.ConnectedInvoicesSequences != null && !entityPM.ConnectedInvoicesSequences.Contains("," + invoicepm.SequenceNumeric + ","))
                            {
                                entityPM.ConnectedInvoicesSequences = entityPM.ConnectedInvoicesSequences + "," + invoicepm.SequenceNumeric;
                            }
                            else if (entityPM.ConnectedInvoicesSequences == null)
                            {
                                entityPM.ConnectedInvoicesSequences = invoicepm.SequenceNumeric.ToString();
                            }
                        }
                    }
                }
                if (crePMs != null)
                {

                    ClaimsRelatedEntityPM crepm = crePMs.FirstOrDefault(d => d.EntityCounterKey.ToString() == pointerPM.Child1EntityId);
                    if (crepm != null)
                    {
                        entityPM.ConnectedCREsSequences = entityPM.ConnectedCREsSequences + "," + crepm.EntityCounterKey;
                    }
                    pointerPM.DocumentTypeCode = entityPM.DocumentTypeCode;

                }
            }

            if (!string.IsNullOrEmpty(entityPM.ConnectedInvoicesSequences))
            {
                entityPM.ConnectedInvoicesSequences = entityPM.ConnectedInvoicesSequences.TrimStart(',');
            }
            if (!string.IsNullOrEmpty(entityPM.ConnectedInvoiceItemsSequences))
            {
                entityPM.ConnectedInvoiceItemsSequences = entityPM.ConnectedInvoiceItemsSequences.TrimStart(',');
            }
            if (!string.IsNullOrEmpty(entityPM.ConnectedCREsSequences))
            {
                entityPM.ConnectedCREsSequences = entityPM.ConnectedCREsSequences.TrimStart(',');
            }
        }
   }


}
   