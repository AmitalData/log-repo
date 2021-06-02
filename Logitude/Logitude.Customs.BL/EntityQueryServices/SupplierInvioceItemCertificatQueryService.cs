using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.DataContracts;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class SupplierInvioceItemCertificatQueryService
    {
        public List<SupplierInvioceItemCertificatPM> GetSupplierInvioceItemCertificatesForSupplierInvoice(string declarationId, int invoiceCounterKey, int tenant, List<int> FilterLine = null)
        {
            List<SupplierInvioceItemCertificat> supplierInvioceItemCertificates = repository.GetSupplierInvioceItemCertificatesForSupplierInvoice(declarationId, invoiceCounterKey, tenant, FilterLine);
            List<SupplierInvioceItemCertificatPM> supplierInvioceItemCertificatPMs = (from a in supplierInvioceItemCertificates
                                                                                      select new SupplierInvioceItemCertificatPM()
                                                                          {
                                                                              DeclarationId = a.DeclarationId,
                                                                              AttachmentTypeCode = a.AttachmentTypeCode,
                                                                              AttachmentTypeName = a.AttachmentType != null ? a.AttachmentType.LocalName : null,
                                                                              CertificateExemptionTypeCode = a.CertificateExemptionTypeCode,
                                                                              CertificateExemptionTypeName = a.CertificateExemptionType != null ? a.CertificateExemptionType.LocalName : null,
                                                                              CertificateNumber = a.CertificateNumber,
                                                                              CustomsAttachmentID = a.CustomsAttachmentID,
                                                                              ItemCertificateCounterKey = a.ItemCertificateCounterKey,
                                                                              ReqConfirmationTypeCode = a.ReqConfirmationTypeCode,
                                                                              ReqConfirmationTypeName = a.RequestConfirmationType != null ? a.RequestConfirmationType.LocalName : null,
                                                                              ResConfirmationTypeCode = a.ResConfirmationTypeCode,
                                                                              ResConfirmationTypeName = a.ResponseConfirmationType != null ? a.ResponseConfirmationType.LocalName : null,
                                                                              InvoiceCounterKey = a.InvoiceCounterKey,
                                                                              LineNumber = a.LineNumber,
                                                                              Tenant = a.Tenant,
                                                                              SequenceNumeric = a.SequenceNumeric,
                                                                              ApprovalRequestNumber = a.ApprovalRequestNumber,
                                                                              ExternalRequestTypeCode = a.ExternalRequestTypeCode

                                                                          }).ToList();
            return supplierInvioceItemCertificatPMs;

        }

        public List<SupplierInvioceItemCertificatPM> GetSupplierInvioceItemCertificatesForSupplierInvoiceItem(string declarationId, int invoiceCounterKey, int lineNumber, int tenant)
        {
            List<SupplierInvioceItemCertificat> supplierInvioceItemCertificates = repository.GetMulti(new SupplierInvoiceItemKeys() { DeclarationId = declarationId, CounterKey = invoiceCounterKey, LineNumber = lineNumber });
            List<SupplierInvioceItemCertificatPM> supplierInvioceItemCertificatPMs = (from a in supplierInvioceItemCertificates
                                                                                      select new SupplierInvioceItemCertificatPM()
                                                                                      {
                                                                                          DeclarationId = a.DeclarationId,
                                                                                          AttachmentTypeCode = a.AttachmentTypeCode,
                                                                                          AttachmentTypeName = a.AttachmentType != null ? a.AttachmentType.LocalName : null,
                                                                                          CertificateExemptionTypeCode = a.CertificateExemptionTypeCode,
                                                                                          CertificateExemptionTypeName = a.CertificateExemptionType != null ? a.CertificateExemptionType.LocalName : null,
                                                                                          CertificateNumber = a.CertificateNumber,
                                                                                          CustomsAttachmentID = a.CustomsAttachmentID,
                                                                                          ItemCertificateCounterKey = a.ItemCertificateCounterKey,
                                                                                          ReqConfirmationTypeCode = a.ReqConfirmationTypeCode,
                                                                                          ReqConfirmationTypeName = a.RequestConfirmationType != null ? a.RequestConfirmationType.LocalName : null,
                                                                                          ResConfirmationTypeCode = a.ResConfirmationTypeCode,
                                                                                          ResConfirmationTypeName = a.ResponseConfirmationType != null ? a.ResponseConfirmationType.LocalName : null,
                                                                                          InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                          LineNumber = a.LineNumber,
                                                                                          Tenant = a.Tenant,
                                                                                          SequenceNumeric = a.SequenceNumeric,
                                                                                          ApprovalRequestNumber = a.ApprovalRequestNumber,
                                                                                          ExternalRequestTypeCode = a.ExternalRequestTypeCode

                                                                                      }).ToList();
            return supplierInvioceItemCertificatPMs;

        }

        public List<SupplierInvioceItemCertificatPM> GetSupplierInvioceItemCertificatesForSupplierInvoiceWithSpecificKeys(string declarationId, int invoiceCounterKey, List<int> itemsLineNumbers, int tenant)
        {
            List<SupplierInvioceItemCertificat> supplierInvioceItemCertificates = repository.GetSupplierInvioceItemCertificatesForSupplierInvoiceWithSpecificKeys(declarationId, invoiceCounterKey, itemsLineNumbers, tenant);
            List<SupplierInvioceItemCertificatPM> supplierInvioceItemCertificatPMs = (from a in supplierInvioceItemCertificates
                                                                                      select new SupplierInvioceItemCertificatPM()
                                                                                      {
                                                                                          DeclarationId = a.DeclarationId,
                                                                                          AttachmentTypeCode = a.AttachmentTypeCode,
                                                                                          AttachmentTypeName = a.AttachmentType != null ? a.AttachmentType.LocalName : null,
                                                                                          CertificateExemptionTypeCode = a.CertificateExemptionTypeCode,
                                                                                          CertificateExemptionTypeName = a.CertificateExemptionType != null ? a.CertificateExemptionType.LocalName : null,
                                                                                          CertificateNumber = a.CertificateNumber,
                                                                                          CustomsAttachmentID = a.CustomsAttachmentID,
                                                                                          ItemCertificateCounterKey = a.ItemCertificateCounterKey,
                                                                                          ReqConfirmationTypeCode = a.ReqConfirmationTypeCode,
                                                                                          ReqConfirmationTypeName = a.RequestConfirmationType != null ? a.RequestConfirmationType.LocalName : null,
                                                                                          ResConfirmationTypeCode = a.ResConfirmationTypeCode,
                                                                                          ResConfirmationTypeName = a.ResponseConfirmationType != null ? a.ResponseConfirmationType.LocalName : null,
                                                                                          InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                          LineNumber = a.LineNumber,
                                                                                          Tenant = a.Tenant,
                                                                                          SequenceNumeric = a.SequenceNumeric,
                                                                                          ApprovalRequestNumber = a.ApprovalRequestNumber,
                                                                                          ExternalRequestTypeCode = a.ExternalRequestTypeCode

                                                                                      }).ToList();
            return supplierInvioceItemCertificatPMs;

        }

        public List<SupplierInvioceItemCertificatPM> GetCertificatesBySearchFields(string declarationId, int invoiceCounterKey, string externalRequestTypeCode,string approvalRequestNumber, int tenant)
        {
            List<SupplierInvioceItemCertificat> supplierInvioceItemCertificates = 
                repository.GetCertificatesBySearchFields(declarationId, invoiceCounterKey,externalRequestTypeCode, approvalRequestNumber, tenant);

            List<SupplierInvioceItemCertificatPM> supplierInvioceItemCertificatPMs = (from a in supplierInvioceItemCertificates
                                                                                      select new SupplierInvioceItemCertificatPM()
                                                                                      {
                                                                                          DeclarationId = a.DeclarationId,
                                                                                          AttachmentTypeCode = a.AttachmentTypeCode,
                                                                                          AttachmentTypeName = a.AttachmentType != null ? a.AttachmentType.LocalName : null,
                                                                                          CertificateExemptionTypeCode = a.CertificateExemptionTypeCode,
                                                                                          CertificateExemptionTypeName = a.CertificateExemptionType != null ? a.CertificateExemptionType.LocalName : null,
                                                                                          CertificateNumber = a.CertificateNumber,
                                                                                          CustomsAttachmentID = a.CustomsAttachmentID,
                                                                                          ItemCertificateCounterKey = a.ItemCertificateCounterKey,
                                                                                          ReqConfirmationTypeCode = a.ReqConfirmationTypeCode,
                                                                                          ReqConfirmationTypeName = a.RequestConfirmationType != null ? a.RequestConfirmationType.LocalName : null,
                                                                                          ResConfirmationTypeCode = a.ResConfirmationTypeCode,
                                                                                          ResConfirmationTypeName = a.ResponseConfirmationType != null ? a.ResponseConfirmationType.LocalName : null,
                                                                                          InvoiceCounterKey = a.InvoiceCounterKey,
                                                                                          LineNumber = a.LineNumber,
                                                                                          Tenant = a.Tenant,
                                                                                          SequenceNumeric = a.SequenceNumeric,
                                                                                          ApprovalRequestNumber = a.ApprovalRequestNumber,
                                                                                          ExternalRequestTypeCode = a.ExternalRequestTypeCode

                                                                                      }).ToList();
            return supplierInvioceItemCertificatPMs;

        }

        public List<CertificateConnectedItems> GetCertificateConnectedItems(QueryOperations queryOperations, string declarationId, string attachmentTypeCode, string reqConfirmationTypeCode, string CertificateExemptionTypeCode, string CertificateNumber, string ResConfirmationTypeCode, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            string invoiceNumber = null;
            int skippedItems = queryOperations.PageIndex;
            QueryFilterItem invoicenumberFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "InvoiceNumber").FirstOrDefault();
            if (invoicenumberFilter != null)
            {
                invoiceNumber = invoicenumberFilter.FieldValue.ToString();
            }
            string ClassificationCode = null;
            QueryFilterItem ClassificationCodeFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ClassificationCode").FirstOrDefault();
            if (ClassificationCodeFilter != null)
            {
                ClassificationCode = ClassificationCodeFilter.FieldValue.ToString();
            }
            string SearchFields = null;
            QueryFilterItem SearchFieldsFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "SearchFields").FirstOrDefault();
            if (SearchFieldsFilter != null)
            {
                SearchFields = SearchFieldsFilter.FieldValue.ToString();
            }
            IQueryable<CertificateConnectedItems> query2 = repository.GetCertificateConnectedItems(declarationId, attachmentTypeCode, reqConfirmationTypeCode, CertificateExemptionTypeCode, CertificateNumber, ResConfirmationTypeCode, tenant, invoiceNumber, ClassificationCode, SearchFields, skippedItems, queryOperations.PageSize, false);





            query2 = filter.GetFilteredQuery<CertificateConnectedItems>(queryOperations, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(CertificateConnectedItems).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> objectFields = new List<ObjectField>();
                objectFields.Add(new ObjectField() { FieldName = "InvoiceNumber", DataTypeCode = "text" });
                objectFields.Add(new ObjectField() { FieldName = "SequenceNumeric", DataTypeCode = "integer" });
                objectFields.Add(new ObjectField() { FieldName = "ItemCode", DataTypeCode = "text" });
                objectFields.Add(new ObjectField() { FieldName = "TradeAgreementName", DataTypeCode = "ntext" });
                objectFields.Add(new ObjectField() { FieldName = "ClassificationCode", DataTypeCode = "ntext" });
                objectFields.Add(new ObjectField() { FieldName = "OriginCountryName", DataTypeCode = "text" });
                objectFields.Add(new ObjectField() { FieldName = "CatalogNumber", DataTypeCode = "text" });
                objectFields.Add(new ObjectField() { FieldName = "SearchFields", DataTypeCode = "ntext" });

                ObjectField objectField = (from a in objectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();
                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                        case "ntext":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateConnectedItems, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateConnectedItems, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateConnectedItems, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateConnectedItems, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<CertificateConnectedItems, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderBy(d => d.ReqConfirmationTypeCode).ThenBy(d => d.AttachmentTypeCode);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderBy(d => d.ReqConfirmationTypeCode).ThenBy(d => d.AttachmentTypeCode);
            }

           




            return query2.ToList();

   

        
        }


        public List<CertificateConnectedItems> GetCertificateConnectedItemsList(string declarationId, string attachmentTypeCode, string reqConfirmationTypeCode, string CertificateExemptionTypeCode, string CertificateNumber, string ResConfirmationTypeCode, int tenant)
        {
            List<CertificateConnectedItems> conntectedItems = repository.GetCertificateConnectedItems(declarationId, attachmentTypeCode, reqConfirmationTypeCode, CertificateExemptionTypeCode, CertificateNumber, ResConfirmationTypeCode, tenant, null, null, null, 0, 0, true).ToList();
            return conntectedItems;
        }


        public int GetListCount(QueryOperations queryOperations, string declarationId, string attachmentTypeCode, string reqConfirmationTypeCode, string CertificateExemptionTypeCode, string CertificateNumber, string ResConfirmationTypeCode, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            string invoiceNumber = null;
            int skippedItems = queryOperations.PageIndex;
            QueryFilterItem invoicenumberFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "InvoiceNumber").FirstOrDefault();
            if (invoicenumberFilter != null)
            {
                invoiceNumber = invoicenumberFilter.FieldValue.ToString();
            }
            string ClassificationCode = null;
            QueryFilterItem ClassificationCodeFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ClassificationCode").FirstOrDefault();
            if (ClassificationCodeFilter != null)
            {
                ClassificationCode = ClassificationCodeFilter.FieldValue.ToString();
            }
            string SearchFields = null;
            QueryFilterItem SearchFieldsFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "SearchFields").FirstOrDefault();
            if (SearchFieldsFilter != null)
            {
                SearchFields = SearchFieldsFilter.FieldValue.ToString();
            }
            int count = repository.GetCertificateConnectedItemsCount(declarationId, attachmentTypeCode, reqConfirmationTypeCode, CertificateExemptionTypeCode, CertificateNumber, ResConfirmationTypeCode, tenant, invoiceNumber, ClassificationCode, SearchFields);
            
            return count;
        }

        public List<CertificateTicket> GetDeclarationCertificateTicket(string declarationId,string reqConfirmationType, string invoiceNumber, int? invoiceCounterKey, string demandState, int tenant)
        {
            List<CertificateTicket> tickets = repository.GetDeclarationCertificateTicket(declarationId,reqConfirmationType, invoiceNumber, invoiceCounterKey, demandState, tenant);


            return tickets;



        }

        public List<CertificateConnectedItems> GetDeclarationInvoicesNumbers(string declarationId, int tenant)
        {
            List<CertificateConnectedItems> numbers = repository.GetDeclarationInvoicesNumbers(declarationId, tenant);
            return numbers;
        }

        public List<CertificateGroupItems> GetCertificateGroupForDeclaration(string declarationId, int tenant)
        {
            List<CertificateGroupItems> tickets = repository.GetCertificateGroupForDeclaration(declarationId, tenant);

            return tickets;
        }

        public int? GetMaxCounterKey(string declarationId,int invoiceCounterKey,int invoiceItemLineNum, int tenant)
        {
            return repository.GetMaxCounterKey(declarationId, invoiceCounterKey, invoiceItemLineNum, tenant);
        }
        public SupplierInvioceItemCertificatPM GetSupplierInvioceItemCertificatWithExternalRequestTypeCode(string code, string decId, int lineNumber)
        {
            SupplierInvioceItemCertificat supplierInvioceItemCertificates = repository.GetSupplierInvioceItemCertificatWithExternalRequestTypeCode(code,decId,lineNumber);
            if (supplierInvioceItemCertificates != null)
            {
                SupplierInvioceItemCertificatPM supplierInvioceItemCertificatPM = new SupplierInvioceItemCertificatPM()
                {
                    DeclarationId = supplierInvioceItemCertificates.DeclarationId,
                    AttachmentTypeCode = supplierInvioceItemCertificates.AttachmentTypeCode,
                    AttachmentTypeName = supplierInvioceItemCertificates.AttachmentType != null ? supplierInvioceItemCertificates.AttachmentType.LocalName : null,
                    CertificateExemptionTypeCode = supplierInvioceItemCertificates.CertificateExemptionTypeCode,
                    CertificateExemptionTypeName = supplierInvioceItemCertificates.CertificateExemptionType != null ? supplierInvioceItemCertificates.CertificateExemptionType.LocalName : null,
                    CertificateNumber = supplierInvioceItemCertificates.CertificateNumber,
                    CustomsAttachmentID = supplierInvioceItemCertificates.CustomsAttachmentID,
                    ItemCertificateCounterKey = supplierInvioceItemCertificates.ItemCertificateCounterKey,
                    ReqConfirmationTypeCode = supplierInvioceItemCertificates.ReqConfirmationTypeCode,
                    ReqConfirmationTypeName = supplierInvioceItemCertificates.RequestConfirmationType != null ? supplierInvioceItemCertificates.RequestConfirmationType.LocalName : null,
                    ResConfirmationTypeCode = supplierInvioceItemCertificates.ResConfirmationTypeCode,
                    ResConfirmationTypeName = supplierInvioceItemCertificates.ResponseConfirmationType != null ? supplierInvioceItemCertificates.ResponseConfirmationType.LocalName : null,
                    InvoiceCounterKey = supplierInvioceItemCertificates.InvoiceCounterKey,
                    LineNumber = supplierInvioceItemCertificates.LineNumber,
                    Tenant = supplierInvioceItemCertificates.Tenant,
                    SequenceNumeric = supplierInvioceItemCertificates.SequenceNumeric,
                    ApprovalRequestNumber = supplierInvioceItemCertificates.ApprovalRequestNumber,
                    ExternalRequestTypeCode = supplierInvioceItemCertificates.ExternalRequestTypeCode
                };
                return supplierInvioceItemCertificatPM;
            }
            return null;
        }

    }
}
