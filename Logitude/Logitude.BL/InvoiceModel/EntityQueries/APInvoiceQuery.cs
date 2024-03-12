using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.Security;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Accounting.Data.Repositories;
using Simplog.Data.ShipmentsModel;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using System.Windows;
using Logitude.Accounting.Data;
using Logitude.Server.Tools;
using System.Linq.Dynamic.Core;

namespace Logitude.BL.InvoiceModel.EntityQueries
{
    public class APInvoiceQuery
    {
        APInvoiceRepository repository;

        public APInvoiceQuery()
        {
            repository = new APInvoiceRepository();
        }

        public APInvoiceQuery(int tenant)
        {
            repository = new APInvoiceRepository(tenant);
        }

        public APInvoiceQuery(APInvoiceRepository apInvoiceRepository)
        {
            repository = apInvoiceRepository;
        }

        public APInvoicePM GetSinglePM(string id, int tenant)
        {
            IQueryable<APInvoicePM> invoices = GetAPInvoiceIQueryable();

            APInvoicePM invoicePM = invoices.Where(d => d.Id == id && d.Tenant == tenant).FirstOrDefault();

            APInvoicePM mappedInvoicePM = GetMappedEntity(tenant, invoicePM);

            return mappedInvoicePM;
        }

        public APInvoicePM GetSinglePMByNumber(string number, int tenant)
        {
            IQueryable<APInvoicePM> invoices = GetAPInvoiceIQueryable();

            APInvoicePM invoicePM = invoices.Where(d => d.InvoiceNumber == number && d.Tenant == tenant).FirstOrDefault();

            APInvoicePM mappedInvoicePM = GetMappedEntity(tenant, invoicePM);

            return mappedInvoicePM;
        }

        public APInvoicePM GetSinglePMByNumberAndExternalId(string number,string externalId, int tenant)
        {
            IQueryable<APInvoicePM> invoices = GetAPInvoiceIQueryable();

            APInvoicePM invoicePM = invoices.Where(d => 
            d.InvoiceNumber == number 
            && d.Tenant == tenant
            && d.ExternalAccountingEntityId == externalId).FirstOrDefault();

            APInvoicePM mappedInvoicePM = GetMappedEntity(tenant, invoicePM);

            return mappedInvoicePM;
        }

        public string GetSingleIdByNumberAndExternalId(string number, string externalId, int tenant)
        {
            APInvoice poco = null;
            poco = repository.GetSingleAPInvoiceByNumberAndExternalId(number, externalId, tenant);

            if (poco == null)
            {
                return null;
            }
            else
            {
                return poco.Id;
            }
        }



        public string GetSingleIdByNumber(string number, int tenant)
        {
            APInvoice poco = null;
            poco = repository.GetSingleAPInvoiceByNumber(number, tenant);

            if (poco == null)
            {
                return null;
            }
            else
            {
                return poco.Id;
            }
        }



        public string GetSingleIdByExternalId(string externalId, int tenant)
        {
            APInvoice poco = null;
            poco = repository.GetSingleAPInvoiceByExternalId(externalId, tenant);

            if (poco == null)
            {
                return null;
            }
            else
            {
                return poco.Id;
            }
        }



        public APInvoicePM GetSinglePMByInternalNumber(string number, int tenant)
        {
            IQueryable<APInvoicePM> invoices = GetAPInvoiceIQueryable();

            APInvoicePM invoicePM = invoices.Where(d => d.InternalNumber == number && d.Tenant == tenant).FirstOrDefault();

            APInvoicePM mappedInvoicePM = GetMappedEntity(tenant, invoicePM);

            return mappedInvoicePM;
        }

        private IQueryable<APInvoicePM> GetAPInvoiceIQueryable()
        {
            var query = (from a in repository.context.APInvoices.Include("Status").Include("LocalCurrency").Include("InvoiceCurrency").Include("TransferStatus").Include("VendorCard").Include("PaymentTerm").Include("ApprovedByUser").Include("ApprovedByUser.Contact").Include("CreatedByUser").Include("CreatedByUser.Contact")
                         select new APInvoicePM()
                         {
                             ProfitCurrencyExchangeRate = a.ProfitCurrencyExchangeRate,
                             ProfitCurrencyId = a.ProfitCurrencyId,
                             SubTotalInInvoiceCurrency = a.SubTotalInInvoiceCurrency,
                             SubTotalInLocalCurrency = a.SubTotalInLocalCurrency,
                             AmountInInvoiceCurrency = a.AmountInInvoiceCurrency,
                             AmountInLocalCurrency = a.AmountInLocalCurrency,
                             AmountInProfitCurrency = a.AmountInProfitCurrency,
                             AmountInInvoiceCurrency_Summary = a.AmountInInvoiceCurrency,
                             AmountInLocalCurrency_Summary = a.AmountInLocalCurrency,
                             AmountInProfitCurrency_Summary = a.AmountInProfitCurrency,
                             AmountDue = a.AmountDue,
                             AmountDueInLocalCurrency = a.AmountDueInLocalCurrency,
                             AmountDueInProfitCurrency = a.AmountDueInProfitCurrency,
                             CreateDate = a.CreateDate,
                             CreatedByUserId = a.CreatedByUserId,
                             DueDate = a.DueDate,
                             ExchangeRateDate = a.ExchangeRateDate,
                             Id = a.Id,
                             InternalNotes = a.InternalNotes,
                             InternalNumber = a.InternalNumber,
                             InvoiceCurrencyExchangeRate = a.InvoiceCurrencyExchangeRate,
                             InvoiceCurrencyId = a.InvoiceCurrencyId,
                             InvoiceDate = a.InvoiceDate,
                             InvoiceNumber = a.InvoiceNumber,
                             IsClosed = a.IsClosed,
                             LocalCurrencyId = a.LocalCurrencyId,
                             LocalCurrencyCode = a.LocalCurrency != null ? a.LocalCurrency.Code : null,
                             InvoiceCurrencyCode = a.InvoiceCurrency == null ? "" : a.InvoiceCurrency.Code,
                             PaymentTermId = a.PaymentTermId,
                             StatusCode = a.StatusCode,
                             StatusName = a.Status == null ? "" : a.Status.Name,
                             VendorId = a.VendorId,
                             Tenant = a.Tenant,
                             UpdateDate = a.UpdateDate,
                             UpdatedByUserId = a.UpdatedByUserId,
                             VATNumber = a.VATNumber,
                             MainEntityId = a.MainEntityId,
                             MainEntityReference = a.MainEntityReference,
                             InvoiceExpectedAmount = a.AmountInInvoiceCurrency,
                             RefundAmount = a.RefundAmount,
                             BranchId = a.BranchId,
                             MasterNumber = a.MasterNumber,
                             HouseNumber = a.HouseNumber,
                             Description = a.Description,
                             VendorName = a.VendorCard == null ? "" : a.VendorCard.EnglishName,
                             VendorContactId = a.VendorCard == null ? "" : a.VendorCard.PrimaryContactId,
                             VendorVatNumber = a.VendorCard == null ? "" : a.VendorCard.VatNumber,
                             VendorCity = a.VendorCard == null ? "" : a.VendorCard.CityName,
                             VendorCountry = a.VendorCard == null ? "" : a.VendorCard.CountryName,
                             VendorLocalName = a.VendorCard == null ? "" : a.VendorCard.LocalName,
                             VendorCode = a.VendorCard == null ? "" : a.VendorCard.Code,
                             VendorPartnerTypeId = a.VendorCard == null ? "" : a.VendorCard.PartnerTypeId,
                             PaymentTermName = a.PaymentTerm == null ? "" : a.PaymentTerm.EnglishName,
                             CreditAccount = a.CreditAccount,
                             TransferTries = a.TransferTries,
                             TransferError = a.TransferError,
                             IsTransferStarted = a.IsTransferStarted,
                             TransferStatusCode = a.TransferStatusCode,
                             TransferStatusName = a.TransferStatus == null ? "" : a.TransferStatus.Name,
                             AccountingExternalCode = a.AccountingExternalCode,
                             ReadyForTransfer = a.TransferStatusCode == "RD" ? true : false,
                             PaymentTermExternalId = a.PaymentTermExternalId,
                             IsMultipleEntities = a.IsMultipleEntities,
                             ApprovedDate = a.ApprovedDate,
                             ApprovedByUserId = a.ApprovedByUserId,
                             ApprovedByUserName = a.ApprovedByUser == null ? null : (a.ApprovedByUser.Contact == null ? null : a.ApprovedByUser.Contact.EnglishName),
                             CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                             OperationalDate = a.OperationalDate,
                             VendorGLAccountId = a.VendorGLAccountId,
                             AccountingDate = a.AccountingDate,
                             IsExternalEntity = a.IsExternalEntity,
                             IsGeneralInvoice = a.IsGeneralInvoice,
                             ExternalAccountingEntityId = a.ExternalAccountingEntityId,
                             FirstApproveDate = a.FirstApproveDate,
                             CreatedByPartner = a.CreatedByPartner,
                             TotalVATOnly = a.TotalVATOnly,
                             PaidDate = a.PaidDate,
                             ShipmentsNumbers = a.ShipmentsNumbers,
                             MasterNumbers = a.MasterNumbers,
                             MasterShipmentNumbers = a.MasterShipmentNumbers,
                             HouseNumbers = a.HouseNumbers,
                             GlobalTaxCalculation = a.GlobalTaxCalculation,
                             ConcurrencyGUID = a.ConcurrencyGUID,
                             IsEquipment = a.IsEquipment,
                             ConnectedPaymentsNumbers = a.ConnectedPaymentsNumbers,
                             TotalEquation = a.TotalEquation,
                             ConfirmationNumber = a.ConfirmationNumber,
                             ConfirmationNumberStatus = a.ConfirmationNumberStatus,

                         });          

            return query;
        }

        private APInvoicePM GetMappedEntity(int tenant, APInvoicePM entityPM)
        {
            if (entityPM != null)
            {
                string apinvoiceId = entityPM.Id;

                APInvoiceLineRepository invoiceLineRepository = new APInvoiceLineRepository(repository.context);
                APInvoiceEntityRepository invoiceEntityRepository = new APInvoiceEntityRepository(repository.context);
                APInvoicePaymentRepository invoicePaymentRepository = new APInvoicePaymentRepository(repository.context);
                APInvoiceTotalVATRepository myTotalVATRepository = new APInvoiceTotalVATRepository(repository.context);
                APInvoiceEntityQuery apInvoiceEntityQuery = new APInvoiceEntityQuery(invoiceEntityRepository);
                APInvoicePaymentQuery apInvoicePaymentQuery = new APInvoicePaymentQuery(invoicePaymentRepository);
                APInvoiceTotalVATQuery myTotalVATQuery = new APInvoiceTotalVATQuery(myTotalVATRepository);

                entityPM.InvoiceEntities = apInvoiceEntityQuery.GetInvoiceEntitiesPMForInvoice(apinvoiceId, tenant);
                entityPM.InvoicePayments = apInvoicePaymentQuery.GetAPInvoicePaymentPMsForInvoice(apinvoiceId, tenant);
                entityPM.TotalVATs = myTotalVATQuery.GetTotalVATs(apinvoiceId, tenant).ToList();

                List<APInvoiceLine> allInvoiceLines = null;
                List<APInvoiceLinePM> allInvoiceLinesPM = null;

                ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);
                if (entityPM.IsMultipleEntities)
                {
                    double? d1 = 0;
                    double? d2 = 0;

                    d1 = entityPM.SubTotalInInvoiceCurrency;
                    d2 = entityPM.TotalVATs.Sum(s => s.InvoiceCurrencyVATAmount);
                    d1 = (double)MethodHelper.Round(d1, 2);
                    d2 = (double)MethodHelper.Round(d2, 2);
                    entityPM.AmountInInvoiceCurrency_Summary = (double)MethodHelper.Round(d1 + d2, 2);

                    d1 = entityPM.SubTotalInLocalCurrency;
                    d2 = entityPM.TotalVATs.Sum(s => s.LocalVATAmount);
                    d1 = (double)MethodHelper.Round(d1, 2);
                    d2 = (double)MethodHelper.Round(d2, 2);
                    entityPM.AmountInLocalCurrency_Summary = (double)MethodHelper.Round(d1 + d2, 2);

                    if (entityPM.ProfitCurrencyId == entityPM.InvoiceCurrencyId)
                    {
                        entityPM.AmountInProfitCurrency_Summary = entityPM.AmountInInvoiceCurrency_Summary;
                    }

                    else if (entityPM.ProfitCurrencyId == entityPM.LocalCurrencyId)
                    {
                        entityPM.AmountInProfitCurrency_Summary = entityPM.AmountInLocalCurrency_Summary;
                    }

                    else
                    {
                        entityPM.AmountInProfitCurrency_Summary = (double)MethodHelper.Round(entityPM.AmountInLocalCurrency_Summary / entityPM.ProfitCurrencyExchangeRate, 2);

                    }

                    if (entityPM.InvoiceEntities.Count > 0)
                    {
                        List<VATTypesGroup> allVatGroups = (from d in myCommonContext.VATTypesGroups
                                                            where d.Tenant == tenant
                                                            select d).ToList();

                        VatTypeRepository vatTypeRepository = new VatTypeRepository(myCommonContext);
                        List<VatType> allVatTypes = vatTypeRepository.GetVatTypes(tenant).ToList();

                        VatTypePercentageRepository vatTypePercentageRepository = new VatTypePercentageRepository(myCommonContext);
                        VatTypePercentageQuery myVatTypePercentageQuery = new VatTypePercentageQuery(vatTypePercentageRepository);
                        List<VatTypePercentagePM> allVatPercentages = myVatTypePercentageQuery.GetVatTypePercentagePMByDate(tenant, TenantServerConfigration.GetCurrentDateTime(tenant).Date);

                        #region InvoiceMultipleShipments

                        allInvoiceLines = invoiceLineRepository.GetInvoiceLinesByInvoiceId(apinvoiceId, tenant).ToList();

                        List<string> shipmentsIds = entityPM.InvoiceEntities.Select(s => s.EntityId).ToList();
                        ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                        List<ShipmentDataView> myShipments = shipmentRepository.GetShipmentsFromIdList(shipmentsIds, tenant);

                        foreach (ShipmentDataView item in myShipments)
                        {
                            string myLongMaster = EntityFieldsHelper.GetLongMasterField(item);

                            APInvoiceMultipleShipmentPM entityShipment = new APInvoiceMultipleShipmentPM()
                            {
                                ShipmentId = item.Id,
                                APInvoiceId = entityPM.Id,
                                Tenant = tenant,
                                House = item.House,
                                Master = item.Master,
                                LongMaster = myLongMaster,
                                ShipmentNumber = item.ShipmentNumber,
                                ShipmentLevelCode = item.ShipmentLevelCode,
                                MainCarriageCarrierName = item.MainCarriageCarrierName,
                                PartnerType = item.ShipmentLevelCode == "C" ? "Agent" : "Customer",
                                PartnerName = item.ShipmentLevelCode == "C" ? item.AgentName : item.CustomerName,
                                OperationalDate = item.OperationalDate,
                                MainCarriageOrigin = item.FromPortCode,
                                MainCarriageFinalDestination = item.ToPortCode,
                                ChargeableWeight = item.ChargeableWeight,
                                TotalReceivables = item.AccountedReceivablesInProfitCurrency,
                                Profit = item.ProfitInProfitCurrency,
                                GrossWeightInKG = item.GrossWeightInKG,
                                VolumeinCBM = item.VolumeInCBM,                       
                            };

                            APInvoiceEntityPM myEntity = entityPM.InvoiceEntities.Where(d => d.EntityId == item.Id).FirstOrDefault();
                            if (myEntity != null)
                            {
                                entityShipment.IndexOrder = myEntity.IndexOrder;
                            }

                            if (entityPM.InvoiceCurrencyId == entityPM.ProfitCurrencyId)
                            {
                                entityShipment.OpenAmount = item.OpenPayablesInProfitCurrency;
                                entityShipment.AccountedAmount = item.AccountedPayablesInProfitCurrency;
                            }

                            else
                            {
                                entityShipment.OpenAmount = (double?)MethodHelper.Round(item.OpenPayablesInLocalCurrency / entityPM.InvoiceCurrencyExchangeRate, 2);
                                entityShipment.AccountedAmount = (double?)MethodHelper.Round(item.AccountedPayablesInLocalCurrency / entityPM.InvoiceCurrencyExchangeRate, 2);
                            }

                            entityShipment.ExpectedAmount = entityShipment.OpenAmount + entityShipment.AccountedAmount;

                            List<APInvoiceLine> myInvoiceLines = allInvoiceLines.Where(d => d.EntityId == item.Id).ToList();

                            if (myInvoiceLines.Count == 0)
                            {
                                entityShipment.SubTotalInLocalCurrency = 0;
                                entityShipment.SubTotalInInvoiceCurrency = 0;
                                entityShipment.TotalAmount = 0;
                                entityShipment.TotalVATAmount = 0;
                                entityShipment.TotalAmountinLocalCurrency = 0;
                            }

                            else
                            {
                                entityShipment.SubTotalInLocalCurrency = (double?)MethodHelper.Round(myInvoiceLines.Sum(s => s.LocalCurrencyAmount), 2);
                                entityShipment.SubTotalInInvoiceCurrency = (double?)MethodHelper.Round(myInvoiceLines.Sum(s => s.InvoiceCurrencyAmount), 2);

                                List<InvoiceTotalsClass> group_Source = new List<InvoiceTotalsClass>();
                                foreach (APInvoiceLine line in myInvoiceLines)
                                {
                                    #region
                                    VatType lineVatType = allVatTypes.Where(d => d.Id == line.VatTypeId).FirstOrDefault();

                                    if (lineVatType != null)
                                    {
                                        if (!lineVatType.IsMultiPercentage)
                                        {
                                            InvoiceTotalsClass newItem = new InvoiceTotalsClass()
                                            {
                                                Id = line.VatTypeId,
                                                VatTypeId = line.VatTypeId,
                                                VatTypePercentage = line.VatPercentage,
                                                LocalCurrencyAmount = line.LocalCurrencyAmount,
                                                InvoiceCurrencyAmount = line.InvoiceCurrencyAmount,
                                                ProfitCurrencyAmount = line.ProfitCurrencyAmount,
                                            };

                                            group_Source.Add(newItem);
                                        }

                                        else
                                        {
                                            List<VATTypesGroup> myVatGroups = allVatGroups.Where(d => d.GroupVATTypeId == line.VatTypeId).ToList();
                                            foreach (VATTypesGroup itemGroup in myVatGroups)
                                            {
                                                InvoiceTotalsClass newItem = new InvoiceTotalsClass()
                                                {
                                                    Id = itemGroup.SingleVATTypeId,
                                                    VatTypeId = itemGroup.SingleVATTypeId,
                                                    LocalCurrencyAmount = line.LocalCurrencyAmount,
                                                    InvoiceCurrencyAmount = line.InvoiceCurrencyAmount,
                                                    ProfitCurrencyAmount = line.ProfitCurrencyAmount,
                                                };


                                                VatTypePercentagePM myPercentagePM = allVatPercentages.Where(d => d.VatTypeId == itemGroup.SingleVATTypeId).FirstOrDefault();
                                                if (myPercentagePM != null)
                                                {
                                                    newItem.VatTypePercentage = myPercentagePM.Percentage;
                                                }

                                                group_Source.Add(newItem);
                                            }
                                        }
                                    }
                                    #endregion
                                }

                                List<InvoiceTotalsClass> group_data
                                    = (from items in group_Source
                                       group items by new { items.VatTypeId, items.VatTypePercentage } into g
                                       select new InvoiceTotalsClass()
                                       {
                                           Id = g.Key.VatTypeId,
                                           VatTypeId = g.Key.VatTypeId,
                                           VatTypePercentage = g.Key.VatTypePercentage,
                                           LocalCurrencyAmount = g.Sum(s => s.LocalCurrencyAmount),
                                           InvoiceCurrencyAmount = g.Sum(s => s.InvoiceCurrencyAmount),
                                           ProfitCurrencyAmount = g.Sum(s => s.ProfitCurrencyAmount),
                                       }).ToList();

                                entityShipment.TotalVATAmount = (double?)MethodHelper.Round(group_data.Sum(s => s.InvoiceCurrencyAmount * s.VatTypePercentage / 100), 2);
                                entityShipment.TotalAmount = (double?)MethodHelper.Round(entityShipment.SubTotalInInvoiceCurrency + entityShipment.TotalVATAmount, 2);

                                foreach (var itemGroup in group_data)
                                {
                                    VatType lineVatType = allVatTypes.Where(d => d.Id == itemGroup.VatTypeId).FirstOrDefault();

                                    if (itemGroup.VatTypePercentage == null)
                                    {
                                        itemGroup.VatTypePercentage = 0;
                                    }

                                    if (itemGroup.LocalCurrencyAmount == null)
                                    {
                                        itemGroup.LocalCurrencyAmount = 0;
                                    }

                                    if (itemGroup.InvoiceCurrencyAmount == null)
                                    {
                                        itemGroup.InvoiceCurrencyAmount = 0;
                                    }

                                    if (itemGroup.ProfitCurrencyAmount == null)
                                    {
                                        itemGroup.ProfitCurrencyAmount = 0;
                                    }

                                    string VATCell = lineVatType.EnglishName + " (" + itemGroup.VatTypePercentage + "%)";

                                    string myString = itemGroup.VatTypeId;
                                    myString += ":" + itemGroup.VatTypePercentage;
                                    myString += ":" + itemGroup.LocalCurrencyAmount;
                                    myString += ":" + itemGroup.InvoiceCurrencyAmount;
                                    myString += ":" + itemGroup.ProfitCurrencyAmount;
                                    myString += ":" + VATCell;
                                    entityShipment.TotalVatsList.Add(myString);
                                }
                            }

                            entityPM.InvoiceMultipleShipments.Add(entityShipment);
                        }
                        #endregion
                    }
                }

                else
                {
                    APInvoiceLineQuery apInvoiceLineQuery = new APInvoiceLineQuery(invoiceLineRepository);
                    allInvoiceLinesPM = apInvoiceLineQuery.GetInvoiceLinesByInvoiceId(entityPM.Id, tenant);
                    entityPM.InvoiceLines = allInvoiceLinesPM;

                    if (!string.IsNullOrEmpty(entityPM.MainEntityId))
                    {
                        IShipmentsContext iShipmentsContext = ShipmentsContext.GetContext(tenant);
                        entityPM.ShipmentConcurrencyGUID = (from d in iShipmentsContext.Shipments where d.Id == entityPM.MainEntityId select d.ConcurrencyGUID).FirstOrDefault();
                        entityPM.ShipmentNewConcurrencyGUID = Guid.NewGuid().ToString();
                    }
                }

                entityPM.NewConcurrencyGUID = Guid.NewGuid().ToString();
                entityPM.TransferLines = this.GetAPInvoiceTransferLines(entityPM, allInvoiceLines, allInvoiceLinesPM);

                //Full Accounting 
                TenantRepository tenantRepository = new TenantRepository(myCommonContext);
                Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
                if (tenantPOCO != null && tenantPOCO.AccountingActivated)
                {
                    JournalRepository rep = new JournalRepository(tenant);
                    JournalEntity journal = rep.GetJournalByAccountingEntityIdAndTypeCode(entityPM.Id, "4", tenant); // 4- apinvoice
                    if (journal != null)
                    {
                        entityPM.JournalId = journal.JournalId;
                        entityPM.JournalNumber = journal.JournalNumber;
                    }
                }

                APInvoicePM securedPM = new APInvoicePM();
                SecuredMapping.GetMappedPM(entityPM, securedPM, "APInvoice", tenant);

                if (securedPM != null)
                {
                    APInvoice entityPOCO = (from a in repository.context.APInvoices
                                            where a.Id == entityPM.Id && a.Tenant == tenant
                                            select a).FirstOrDefault();

                    securedPM.Field1 = new CustomFieldClass("Field1", "APInvoice", entityPOCO.Field1);
                    securedPM.Field2 = new CustomFieldClass("Field2", "APInvoice", entityPOCO.Field2);
                    securedPM.Field3 = new CustomFieldClass("Field3", "APInvoice", entityPOCO.Field3);
                    securedPM.Field4 = new CustomFieldClass("Field4", "APInvoice", entityPOCO.Field4);
                    securedPM.Field5 = new CustomFieldClass("Field5", "APInvoice", entityPOCO.Field5);
                    securedPM.Field6 = new CustomFieldClass("Field6", "APInvoice", entityPOCO.Field6);
                    securedPM.Field7 = new CustomFieldClass("Field7", "APInvoice", entityPOCO.Field7);
                    securedPM.Field8 = new CustomFieldClass("Field8", "APInvoice", entityPOCO.Field8);
                    securedPM.Field9 = new CustomFieldClass("Field9", "APInvoice", entityPOCO.Field9);
                    securedPM.Field10 = new CustomFieldClass("Field10", "APInvoice", entityPOCO.Field10);

                    securedPM = BranchPermitionsFilter.AddUserBranchRestrictionFilters(new QueryOperations(), securedPM, tenant);

                    if (securedPM == null)
                    {
                        throw new ApplicationException("This invoice is branch Restricted");
                    }
                }

                return securedPM;
            }
            else
            {
                return null;
            }
        }

        public APInvoicePM GetSingleInvoiceByInvoiceNumber(string invoiceNumber, int tenant)
        {
            APInvoicePM entityPM = (from a in repository.context.APInvoices.Include("Status").Include("LocalCurrency").Include("InvoiceCurrency").Include("TransferStatus").Include("VendorCard").Include("PaymentTerm").Include("ApprovedByUser").Include("ApprovedByUser.Contact").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("Branch")
                                    where a.InvoiceNumber == invoiceNumber && a.Tenant == tenant
                                    select new APInvoicePM()
                                    {
                                        ProfitCurrencyExchangeRate = a.ProfitCurrencyExchangeRate,
                                        ProfitCurrencyId = a.ProfitCurrencyId,
                                        SubTotalInInvoiceCurrency = a.SubTotalInInvoiceCurrency,
                                        SubTotalInLocalCurrency = a.SubTotalInLocalCurrency,
                                        AmountInInvoiceCurrency = a.AmountInInvoiceCurrency,
                                        AmountInLocalCurrency = a.AmountInLocalCurrency,
                                        AmountInProfitCurrency = a.AmountInProfitCurrency,
                                        AmountInInvoiceCurrency_Summary = a.AmountInInvoiceCurrency,
                                        AmountInLocalCurrency_Summary = a.AmountInLocalCurrency,
                                        AmountInProfitCurrency_Summary = a.AmountInProfitCurrency,
                                        AmountDue = a.AmountDue,
                                        AmountDueInLocalCurrency = a.AmountDueInLocalCurrency,
                                        AmountDueInProfitCurrency = a.AmountDueInProfitCurrency,
                                        CreateDate = a.CreateDate,
                                        CreatedByUserId = a.CreatedByUserId,
                                        DueDate = a.DueDate,
                                        ExchangeRateDate = a.ExchangeRateDate,
                                        Id = a.Id,
                                        InternalNotes = a.InternalNotes,
                                        InternalNumber = a.InternalNumber,
                                        InvoiceCurrencyExchangeRate = a.InvoiceCurrencyExchangeRate,
                                        InvoiceCurrencyId = a.InvoiceCurrencyId,
                                        InvoiceDate = a.InvoiceDate,
                                        InvoiceNumber = a.InvoiceNumber,
                                        IsClosed = a.IsClosed,
                                        LocalCurrencyId = a.LocalCurrencyId,
                                        LocalCurrencyCode = a.LocalCurrency != null ? a.LocalCurrency.Code : null,
                                        InvoiceCurrencyCode = a.InvoiceCurrency == null ? "" : a.InvoiceCurrency.Code,
                                        PaymentTermId = a.PaymentTermId,
                                        StatusCode = a.StatusCode,
                                        StatusName = a.Status == null ? "" : a.Status.Name,
                                        VendorId = a.VendorId,
                                        Tenant = a.Tenant,
                                        UpdateDate = a.UpdateDate,
                                        UpdatedByUserId = a.UpdatedByUserId,
                                        VATNumber = a.VATNumber,
                                        MainEntityId = a.MainEntityId,
                                        MainEntityReference = a.MainEntityReference,
                                        InvoiceExpectedAmount = a.AmountInInvoiceCurrency,
                                        RefundAmount = a.RefundAmount,
                                        BranchId = a.BranchId,
                                        MasterNumber = a.MasterNumber,
                                        HouseNumber = a.HouseNumber,
                                        Description = a.Description,
                                        VendorName = a.VendorCard == null ? "" : (a.VendorCard.LocalName != null ? a.VendorCard.LocalName : a.VendorCard.EnglishName),
                                        VendorContactId = a.VendorCard == null ? "" : a.VendorCard.PrimaryContactId,
                                        VendorVatNumber = a.VendorCard == null ? "" : a.VendorCard.VatNumber,
                                        VendorCity = a.VendorCard == null ? "" : a.VendorCard.CityName,
                                        VendorCountry = a.VendorCard == null ? "" : a.VendorCard.CountryName,
                                        VendorCode = a.VendorCard == null ? "" : a.VendorCard.Code,
                                        VendorPartnerTypeId = a.VendorCard == null ? "" : a.VendorCard.PartnerTypeId,
                                        PaymentTermName = a.PaymentTerm == null ? "" : a.PaymentTerm.EnglishName,
                                        CreditAccount = a.CreditAccount,
                                        TransferTries = a.TransferTries,
                                        TransferError = a.TransferError,
                                        IsTransferStarted = a.IsTransferStarted,
                                        TransferStatusCode = a.TransferStatusCode,
                                        TransferStatusName = a.TransferStatus == null ? "" : a.TransferStatus.Name,
                                        AccountingExternalCode = a.AccountingExternalCode,
                                        ReadyForTransfer = a.TransferStatusCode == "RD" ? true : false,
                                        PaymentTermExternalId = a.PaymentTermExternalId,
                                        IsMultipleEntities = a.IsMultipleEntities,
                                        ApprovedDate = a.ApprovedDate,
                                        ApprovedByUserId = a.ApprovedByUserId,
                                        ApprovedByUserName = a.ApprovedByUser == null ? null : (a.ApprovedByUser.Contact == null ? null : a.ApprovedByUser.Contact.EnglishName),
                                        CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                                        OperationalDate = a.OperationalDate,
                                        VendorGLAccountId = a.VendorGLAccountId,
                                        AccountingDate = a.AccountingDate,
                                        IsExternalEntity = a.IsExternalEntity,
                                        IsGeneralInvoice = a.IsGeneralInvoice,
                                        ExternalAccountingEntityId = a.ExternalAccountingEntityId,
                                        FirstApproveDate = a.FirstApproveDate,
                                        BranchName = a.Branch == null ? null : a.Branch.EnglishName,
                                        CreatedByPartner = a.CreatedByPartner,
                                        TotalVATOnly = a.TotalVATOnly,
                                        PaidDate = a.PaidDate,
                                        ShipmentsNumbers = a.ShipmentsNumbers, 
                                        MasterNumbers = a.MasterNumbers,
                                        MasterShipmentNumbers = a.MasterShipmentNumbers, 
                                        HouseNumbers = a.HouseNumbers,
                                        GlobalTaxCalculation = a.GlobalTaxCalculation,
                                        ConnectedPaymentsNumbers = a.ConnectedPaymentsNumbers,
                                        TotalEquation = a.TotalEquation,
                                    }).FirstOrDefault();
            return entityPM;
        }

        public APInvoicePM GetSingleInvoiceByExternlaEntityId(string externalEntityId, int tenant)
        {
            APInvoicePM entityPM = (from a in repository.context.APInvoices.Include("Branch")
                                    where a.ExternalAccountingEntityId == externalEntityId && a.Tenant == tenant
                                    select new APInvoicePM()
                                    {
                                        ProfitCurrencyExchangeRate = a.ProfitCurrencyExchangeRate,
                                        ProfitCurrencyId = a.ProfitCurrencyId,
                                        SubTotalInInvoiceCurrency = a.SubTotalInInvoiceCurrency,
                                        SubTotalInLocalCurrency = a.SubTotalInLocalCurrency,
                                        AmountInInvoiceCurrency = a.AmountInInvoiceCurrency,
                                        AmountInLocalCurrency = a.AmountInLocalCurrency,
                                        AmountInProfitCurrency = a.AmountInProfitCurrency,
                                        AmountInInvoiceCurrency_Summary = a.AmountInInvoiceCurrency,
                                        AmountInLocalCurrency_Summary = a.AmountInLocalCurrency,
                                        AmountInProfitCurrency_Summary = a.AmountInProfitCurrency,
                                        AmountDue = a.AmountDue,
                                        AmountDueInLocalCurrency = a.AmountDueInLocalCurrency,
                                        AmountDueInProfitCurrency = a.AmountDueInProfitCurrency,
                                        CreateDate = a.CreateDate,
                                        CreatedByUserId = a.CreatedByUserId,
                                        DueDate = a.DueDate,
                                        ExchangeRateDate = a.ExchangeRateDate,
                                        Id = a.Id,
                                        InternalNotes = a.InternalNotes,
                                        InternalNumber = a.InternalNumber,
                                        InvoiceCurrencyExchangeRate = a.InvoiceCurrencyExchangeRate,
                                        InvoiceCurrencyId = a.InvoiceCurrencyId,
                                        InvoiceDate = a.InvoiceDate,
                                        InvoiceNumber = a.InvoiceNumber,
                                        IsClosed = a.IsClosed,
                                        LocalCurrencyId = a.LocalCurrencyId,
                                        LocalCurrencyCode = a.LocalCurrency != null ? a.LocalCurrency.Code : null,
                                        InvoiceCurrencyCode = a.InvoiceCurrency == null ? "" : a.InvoiceCurrency.Code,
                                        PaymentTermId = a.PaymentTermId,
                                        StatusCode = a.StatusCode,
                                        StatusName = a.Status == null ? "" : a.Status.Name,
                                        VendorId = a.VendorId,
                                        Tenant = a.Tenant,
                                        UpdateDate = a.UpdateDate,
                                        UpdatedByUserId = a.UpdatedByUserId,
                                        VATNumber = a.VATNumber,
                                        MainEntityId = a.MainEntityId,
                                        MainEntityReference = a.MainEntityReference,
                                        InvoiceExpectedAmount = a.AmountInInvoiceCurrency,
                                        RefundAmount = a.RefundAmount,
                                        BranchId = a.BranchId,
                                        MasterNumber = a.MasterNumber,
                                        HouseNumber = a.HouseNumber,
                                        Description = a.Description,
                                        VendorName = a.VendorCard == null ? "" : (a.VendorCard.LocalName != null ? a.VendorCard.LocalName : a.VendorCard.EnglishName),
                                        VendorContactId = a.VendorCard == null ? "" : a.VendorCard.PrimaryContactId,
                                        VendorVatNumber = a.VendorCard == null ? "" : a.VendorCard.VatNumber,
                                        VendorCity = a.VendorCard == null ? "" : a.VendorCard.CityName,
                                        VendorCountry = a.VendorCard == null ? "" : a.VendorCard.CountryName,
                                        VendorCode = a.VendorCard == null ? "" : a.VendorCard.Code,
                                        VendorPartnerTypeId = a.VendorCard == null ? "" : a.VendorCard.PartnerTypeId,
                                        PaymentTermName = a.PaymentTerm == null ? "" : a.PaymentTerm.EnglishName,
                                        CreditAccount = a.CreditAccount,
                                        TransferTries = a.TransferTries,
                                        TransferError = a.TransferError,
                                        IsTransferStarted = a.IsTransferStarted,
                                        TransferStatusCode = a.TransferStatusCode,
                                        TransferStatusName = a.TransferStatus == null ? "" : a.TransferStatus.Name,
                                        AccountingExternalCode = a.AccountingExternalCode,
                                        ReadyForTransfer = a.TransferStatusCode == "RD" ? true : false,
                                        PaymentTermExternalId = a.PaymentTermExternalId,
                                        IsMultipleEntities = a.IsMultipleEntities,
                                        ApprovedDate = a.ApprovedDate,
                                        ApprovedByUserId = a.ApprovedByUserId,
                                        ApprovedByUserName = a.ApprovedByUser == null ? null : (a.ApprovedByUser.Contact == null ? null : a.ApprovedByUser.Contact.EnglishName),
                                        CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                                        OperationalDate = a.OperationalDate,
                                        VendorGLAccountId = a.VendorGLAccountId,
                                        AccountingDate = a.AccountingDate,
                                        IsExternalEntity = a.IsExternalEntity,
                                        IsGeneralInvoice = a.IsGeneralInvoice,
                                        ExternalAccountingEntityId = a.ExternalAccountingEntityId,
                                        FirstApproveDate = a.FirstApproveDate,
                                        BranchName = a.Branch == null ? null : a.Branch.EnglishName,
                                        CreatedByPartner = a.CreatedByPartner,
                                        TotalVATOnly = a.TotalVATOnly,
                                        PaidDate = a.PaidDate,
                                        ShipmentsNumbers = a.ShipmentsNumbers,
                                        MasterNumbers = a.MasterNumbers,
                                        MasterShipmentNumbers = a.MasterShipmentNumbers,
                                        HouseNumbers = a.HouseNumbers,
                                        GlobalTaxCalculation = a.GlobalTaxCalculation,
                                        ConnectedPaymentsNumbers = a.ConnectedPaymentsNumbers,
                                        TotalEquation = a.TotalEquation,
                                    }).FirstOrDefault();

            if(entityPM != null)
            {

                entityPM.InvoiceLines = GetAPInvoiceLineByInvoiceId(entityPM);

            }
            return entityPM;
        }
        private List<APInvoiceLinePM> GetAPInvoiceLineByInvoiceId(APInvoicePM invoice)
        {
            APInvoiceLineRepository invoiceLineRepository = new APInvoiceLineRepository(invoice.Tenant);
            APInvoiceLineQuery apInvoiceLineQuery = new APInvoiceLineQuery(invoiceLineRepository);
            return apInvoiceLineQuery.GetInvoiceLinesByInvoiceId(invoice.Id, invoice.Tenant);
        }
        private List<APTransferLinePM> GetAPInvoiceTransferLines(APInvoicePM entityPM, List<APInvoiceLine> allInvoiceLines, List<APInvoiceLinePM> allInvoiceLinesPM)
        {
            List<APTransferLinePM> myTransferLines = new List<APTransferLinePM>();

            int index = 0;
            string myTable = null;
            string myFieldValue = null;
            string myFieldExternalId = null;
            int tenant = entityPM.Tenant;

            ExternalSystemsTablesCodeRepository externalTablesRepository = new ExternalSystemsTablesCodeRepository(tenant);
            List<ExternalSystemsTablesCode> externalTables = externalTablesRepository.GetExternalSystemsTablesCodes(tenant).ToList();

            AccountingSettingRepository accountingSettingRepository = new AccountingSettingRepository(tenant);
            AccountingSetting accountingSetting = accountingSettingRepository.GetSingleAccountSetting(tenant);

            AccountingSystemRepository accountingSystemRepository = new AccountingSystemRepository(tenant);
            AccountingSystem accountingSystem = accountingSystemRepository.GetSingleAccountingSystem(accountingSetting.AccountingSystemCode);

            bool isJournal = accountingSystem.IsJournalMode;
            bool isExternal = accountingSystem.IsExternalCodesFromTable;
            bool isTaxItemManaged = accountingSystem.IsTaxItemManaged;
            bool isSingleTaxPerInvoice = accountingSystem.IsSingleTaxPerInvoice;

            #region Card
            index++;
            myTable = "Card";
            myFieldValue = entityPM.CreditAccount;
            myFieldExternalId = this.GetExternalId(myTable, myFieldValue, externalTables);
            myTransferLines.Add(new APTransferLinePM()
            {
                Id = index,
                Table = myTable,
                APInvoiceId = entityPM.Id,
                FieldName = "CreditAccount",
                FieldValue = myFieldValue,
                Description = "Vendor",
                DescriptionValue = entityPM.VendorName,
                DescriptionHelp = "Please enter Credit Account",
                FieldExternalTableId = myFieldExternalId,
            });
            #endregion

            #region Currency
            index++;
            myTable = "Currency";
            myFieldValue = entityPM.AccountingExternalCode;
            myFieldExternalId = this.GetExternalId(myTable, myFieldValue, externalTables);
            myTransferLines.Add(new APTransferLinePM()
            {
                Id = index,
                Table = myTable,
                APInvoiceId = entityPM.Id,
                FieldName = "AccountingExternalCode",
                FieldValue = myFieldValue,
                Description = "Invoice Currency",
                DescriptionValue = entityPM.InvoiceCurrencyCode,
                DescriptionHelp = "Please enter external code for " + entityPM.InvoiceCurrencyCode,
                FieldExternalTableId = myFieldExternalId,
            });
            #endregion

            #region PaymentTerm
            index++;
            myTable = "PaymentTerm";
            myFieldValue = entityPM.PaymentTermExternalId;
            myFieldExternalId = this.GetExternalId(myTable, myFieldValue, externalTables);
            myTransferLines.Add(new APTransferLinePM()
            {
                Id = index,
                Table = myTable,
                APInvoiceId = entityPM.Id,
                FieldName = "PaymentTermExternalId",
                FieldValue = myFieldValue,
                Description = "Payment Term",
                DescriptionValue = entityPM.PaymentTermName,
                DescriptionHelp = "Please enter external Payment Term",
                FieldExternalTableId = myFieldExternalId,
            });
            #endregion

            if (entityPM.IsMultipleEntities)
            {
                if (allInvoiceLines != null)
                {
                    if (allInvoiceLines.Count > 0)
                    {
                        #region Charges
                        List<string> myChargeTypesIds = allInvoiceLines.GroupBy(g => g.ChargesTypeId).Select(s => s.Key).ToList();

                        foreach (string itemChargeTypeId in myChargeTypesIds)
                        {
                            APInvoiceLine myInvoiceLine = allInvoiceLines.Where(d => d.ChargesTypeId == itemChargeTypeId).FirstOrDefault();

                            if (myInvoiceLine != null)
                            {
                                index++;

                                string myDescriptionValue = myInvoiceLine.Description;
                                if (string.IsNullOrEmpty(myDescriptionValue))
                                {
                                    ChargesType myChargesType = ChargesTypeRepository.GetSingleChargesType(itemChargeTypeId, entityPM.Tenant, true);
                                    if (myChargesType != null)
                                    {
                                        myDescriptionValue = myChargesType.EnglishName;
                                    }
                                }

                                myTable = "ChargesType";
                                myFieldValue = myInvoiceLine.DebitAccount;
                                myFieldExternalId = this.GetExternalId(myTable, myFieldValue, externalTables);
                                myTransferLines.Add(new APTransferLinePM()
                                {
                                    Id = index,
                                    Table = myTable,
                                    APInvoiceId = entityPM.Id,
                                    FieldName = "DebitAccount",
                                    FieldValue = myFieldValue,
                                    Description = "Charge Type",
                                    DescriptionValue = myDescriptionValue,
                                    DescriptionHelp = "Please enter Debit Account",
                                    FieldExternalTableId = myFieldExternalId,
                                });
                            }
                        }
                        #endregion

                        #region Vats
                        List<string> myVatTypesIds = allInvoiceLines.Where(d => d.VatPercentage != null).GroupBy(g => g.VatTypeId).Select(s => s.Key).ToList();

                        foreach (string itemVatTypeId in myVatTypesIds)
                        {
                            APInvoiceLine myInvoiceLine = allInvoiceLines.Where(d => d.VatTypeId == itemVatTypeId && d.VatPercentage != null).FirstOrDefault();
                            if (myInvoiceLine != null)
                            {
                                string myVatTypeName = null;
                                VatType myVatType = VatTypeRepository.GetSingleVatType(itemVatTypeId, tenant, true);
                                if (myVatType != null)
                                {
                                    myVatTypeName = myVatType.EnglishName;
                                }

                                if (!isSingleTaxPerInvoice)
                                {
                                    if (myInvoiceLine.VatPercentage != 0)
                                    {
                                        //ObsList.Add(new APTransferLineViewModel(linePM, this, "TAX"));
                                        myTable = "VatType";
                                        myFieldValue = myInvoiceLine.DebitAccount;
                                        myFieldExternalId = this.GetExternalId(myTable, myFieldValue, externalTables);
                                        myTransferLines.Add(new APTransferLinePM()
                                        {
                                            Id = index,
                                            Table = myTable,
                                            APInvoiceId = entityPM.Id,
                                            FieldName = "DebitAccount",
                                            FieldValue = myFieldValue,
                                            Description = "Charge Type",
                                            DescriptionValue = myVatTypeName,
                                            DescriptionHelp = "Please enter Debit Account",
                                            FieldExternalTableId = myFieldExternalId,
                                        });
                                    }
                                }

                                else
                                {
                                    //ObsList.Add(new APTransferLineViewModel(linePM, this, "TAX"));

                                    if (isTaxItemManaged)
                                    {
                                        //ObsList.Add(new APTransferLineViewModel(linePM, this, "VAT"));
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
            }

            else
            {
                if (allInvoiceLinesPM != null)
                {
                    if (allInvoiceLinesPM.Count > 0)
                    {

                    }
                }
            }

            return myTransferLines;
        }

        private string GetExternalId(string myLogitudeTable, string myFieldValue, List<ExternalSystemsTablesCode> externalTables)
        {
            string myResult = null;

            if (externalTables.Count > 0)
            {
                if (!string.IsNullOrEmpty(myFieldValue))
                {
                    if (!string.IsNullOrEmpty(myLogitudeTable))
                    {
                        ExternalSystemsTablesCode itemExternalTable = externalTables.Where(d => d.LogitudeTable == myLogitudeTable && d.Code == myFieldValue).FirstOrDefault();
                        if (itemExternalTable != null)
                        {
                            myResult = itemExternalTable.Id;
                        }
                    }
                }
            }

            return myResult;
        }

        public APInvoiceMultipleShortPM GetSingleAPInvoiceShortPM(string id, string shipmentId, int tenant)
        {
            APInvoiceMultipleShortPM entityPM = (from a in repository.context.APInvoices
                                                 where a.Id == id && a.Tenant == tenant
                                                 select new APInvoiceMultipleShortPM()
                                                 {
                                                     Id = id,
                                                     Tenant = tenant,
                                                     ShipmentId = shipmentId,
                                                     StatusCode = a.StatusCode,
                                                     VendorId = a.VendorId,
                                                     ProfitCurrencyId = a.ProfitCurrencyId,
                                                     InvoiceCurrencyId = a.InvoiceCurrencyId,
                                                     InvoiceCurrencyExchangeRate = a.InvoiceCurrencyExchangeRate,
                                                     ProfitCurrencyExchangeRate = a.ProfitCurrencyExchangeRate,
                                                 }).FirstOrDefault();

            APInvoiceLineRepository invoiceLineRepository = new APInvoiceLineRepository(repository.context);
            APInvoiceLineQuery apInvoiceLineQuery = new APInvoiceLineQuery(invoiceLineRepository);

            entityPM.InvoiceLines = apInvoiceLineQuery.GetAPInvoiceLinesByEntityId(id, shipmentId, tenant);

            entityPM.SubTotalInLocalCurrency = entityPM.InvoiceLines.Sum(s => s.LocalCurrencyAmount);
            entityPM.SubTotalInInvoiceCurrency = entityPM.InvoiceLines.Sum(s => s.InvoiceCurrencyAmount);

            return entityPM;
        }

        public List<CreditorsClass> GetDebtorsExposureForGridControl(int tenant, int currencyIndex, bool isBranchRestricted)
        {
            if (FeatureToggleHelper.HasFeatureToggle("QPI", tenant))
                return GetDebtorsExposureForGridControl_NewStyle(tenant, currencyIndex, isBranchRestricted);
            else
                return GetDebtorsExposureForGridControl_OldStyle(tenant, currencyIndex);            
        }
        private List<CreditorsClass> GetDebtorsExposureForGridControl_NewStyle(int tenant, int currencyIndex, bool isBranchRestricted)
        {
            var currentDate = DateTime.Now.AddMonths(-3);
            if (!repository.context.APPayments.Any(x => x.CreateDate >= currentDate)) return new List<CreditorsClass>();

            List<string> allowedBranchesIds = new List<string>();
            if (isBranchRestricted) allowedBranchesIds = BranchPermitionsFilter.GetAllowedLoggedUserBranches(tenant);

            return (from invoice in repository.context.APInvoices.Include("VendorCard")
                    where invoice.Tenant == tenant
                    && invoice.StatusCode != "VD" && invoice.StatusCode != "PD" && invoice.StatusCode != "WA"
                    && !invoice.IsClosed
                    && (!isBranchRestricted || allowedBranchesIds.Contains(invoice.BranchId))
                    group invoice by new
                    {
                        VendorEnglishName = invoice.VendorCard.EnglishName,
                        invoice.VendorId,
                        VendorPartnerTypeId = invoice.VendorCard.PartnerTypeId,
                    } into gr
                    orderby gr.Key.VendorEnglishName
                    select new CreditorsClass()
                    {
                        Outstanding = currencyIndex == 1 ? gr.Sum(d => (d.AmountDueInLocalCurrency)) : gr.Sum(d => (d.AmountDueInProfitCurrency)),
                        CreditorName = gr.Key.VendorEnglishName,
                        Overdue = (from s in gr where s.DueDate <= DateTime.Today.Date select new { overDue1 = currencyIndex == 1 ? s.AmountDueInLocalCurrency : s.AmountDueInProfitCurrency }).Sum(ss => ss.overDue1),
                        CreditorId = gr.Key.VendorId,
                        CreditorType = gr.Key.VendorPartnerTypeId,
                    }).OrderByDescending(d => d.Outstanding).Take(10).ToList();
        }
        private List<CreditorsClass> GetDebtorsExposureForGridControl_OldStyle(int tenant, int currencyIndex)
        {
            List<APInvoicePM> invoiceList = (from a in repository.context.APInvoices
                                             where a.Tenant == tenant && (a.StatusCode != "VD" && a.StatusCode != "PD" && a.StatusCode != "WA" && a.IsClosed == false)
                                             select new APInvoicePM()
                                             {
                                                 AmountDueInLocalCurrency = a.AmountDueInLocalCurrency,
                                                 AmountDueInProfitCurrency = a.AmountDueInProfitCurrency,
                                                 VendorId = a.VendorId,
                                                 Id = a.Id,
                                                 Tenant = a.Tenant,
                                                 InvoiceCurrencyExchangeRate = a.InvoiceCurrencyExchangeRate,
                                                 DueDate = a.DueDate,
                                                 BranchId = a.BranchId,
                                             }).ToList();

            invoiceList = BranchPermitionsFilter.AddUserBranchRestrictionFilters<APInvoicePM>(new QueryOperations(), invoiceList.AsQueryable<APInvoicePM>(), tenant).ToList();


            foreach (APInvoicePM invoice in invoiceList)
            {
                Card vendor = CardRepository.GetSingleCard(invoice.VendorId, invoice.Tenant, true);
                invoice.VendorName = vendor.EnglishName;
                invoice.VendorType = vendor.PartnerTypeId;
            }

            List<CreditorsClass> datalist = (from a in invoiceList
                                             where a.Tenant == tenant
                                             group a by new
                                             {
                                                 a.VendorName,
                                                 a.VendorId,
                                                 a.VendorType,

                                             } into gr
                                             orderby gr.Key.VendorName
                                             select new CreditorsClass()
                                             {
                                                 Outstanding = currencyIndex == 1 ? gr.Sum(d => (d.AmountDueInLocalCurrency)) : gr.Sum(d => (d.AmountDueInProfitCurrency)),
                                                 CreditorName = gr.Key.VendorName,
                                                 Overdue = currencyIndex == 1 ? gr.Where(d => d.DueDate <= DateTime.Today.Date).Sum(s => (s.AmountDueInLocalCurrency)) : gr.Where(d => d.DueDate <= DateTime.Today.Date).Sum(s => (s.AmountDueInProfitCurrency)),
                                                 CreditorId = gr.Key.VendorId,
                                                 CreditorType = gr.Key.VendorType,
                                             }).ToList();

            datalist = datalist.OrderByDescending(d => d.Outstanding).Take(10).ToList();

            return datalist;
        }


        public IQueryable<APInvoiceList> GetIQueryableEntityList(IQueryable<APInvoice> iQueryable)
        {
            int tenant = 0;
            if (iQueryable != null && iQueryable.Count() > 0)
            {
                tenant = iQueryable.First().Tenant;
            }
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            bool isFullAccounting = false;
            Tenant myTenant = TenantRepository.GetSingleTenant(tenant, true);
            if (myTenant != null)
            {
                isFullAccounting = myTenant.AccountingActivated;
            }
            var result = from a in iQueryable.Include("Status").Include("LocalCurrency").Include("InvoiceCurrency").Include("ProfitCurrency").Include("VendorCard").Include("PaymentTerm").Include("TransferStatus").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("UpdatedByUser").Include("UpdatedByUser.Contact").Include("ApprovedByUser").Include("ApprovedByUser.Contact").Include("Branch")
                         select new APInvoiceList()
                         {
                             CreateDate = a.CreateDate,
                             CreatedByUserId = a.CreatedByUserId,
                             DueDate = a.DueDate,
                             ExchangeRateDate = a.ExchangeRateDate,
                             AmountInInvoiceCurrency = a.AmountInInvoiceCurrency,
                             AmountInLocalCurrency = a.AmountInLocalCurrency,
                             AmountInProfitCurrency = a.AmountInProfitCurrency,
                             Id = a.Id,
                             InternalNotes = a.InternalNotes,
                             InternalNumber = a.InternalNumber,
                             InvoiceCurrencyExchangeRate = a.InvoiceCurrencyExchangeRate,
                             InvoiceCurrencyId = a.InvoiceCurrencyId,
                             InvoiceDate = a.InvoiceDate,
                             InvoiceNumber = a.InvoiceNumber,
                             IsClosed = a.IsClosed,
                             LocalCurrencyId = a.LocalCurrencyId,
                             LocalCurrencyCode = a.LocalCurrency == null ? null : a.LocalCurrency.Code,
                             PaymentTermId = a.PaymentTermId,
                             ProfitCurrencyExchangeRate = a.ProfitCurrencyExchangeRate,
                             ProfitCurrencyId = a.ProfitCurrencyId,
                             StatusCode = a.StatusCode,
                             SubTotalInInvoiceCurrency = a.SubTotalInInvoiceCurrency,
                             SubTotalInLocalCurrency = a.SubTotalInLocalCurrency,
                             VendorId = a.VendorId,
                             VendorName = a.VendorCard == null ? "" : isFullAccounting == false ? a.VendorCard.EnglishName : (a.VendorCard.LocalName != null ? a.VendorCard.LocalName : a.VendorCard.EnglishName),
                             VendorCity = a.VendorCard == null ? "" : a.VendorCard.CityName,
                             VendorCountry = a.VendorCard == null ? "" : a.VendorCard.CountryName,
                             VendorCode = a.VendorCard == null ? null : a.VendorCard.Code,
                             Tenant = a.Tenant,
                             UpdateDate = a.UpdateDate,
                             UpdatedByUserId = a.UpdatedByUserId,
                             VATNumber = a.VATNumber,
                             SearchFields = a.SearchFields,
                             InvoiceCurrencyCode = a.InvoiceCurrency == null ? null : a.InvoiceCurrency.Code,
                             ProfitCurrencyCode = a.ProfitCurrency == null ? null : a.ProfitCurrency.Code,
                             StatusName = a.Status == null ? null : a.Status.Name,
                             AmountDue = a.AmountDue,
                             MainEntityReference = a.MainEntityReference,
                             MainEntityId = a.MainEntityId,
                             CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                             UpdatedByUserName = a.UpdatedByUser == null ? null : (a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.EnglishName),
                             PaymentTermName = a.PaymentTerm == null ? null : a.PaymentTerm.EnglishName,
                             MasterNumber = a.MasterNumber,
                             HouseNumber = a.HouseNumber,
                             Description = a.Description,
                             AmountDueInLocalCurrency = a.AmountDueInLocalCurrency,
                             AmountDueInProfitCurrency = a.AmountDueInProfitCurrency,
                             CreditAccount = a.CreditAccount,
                             TransferTries = a.TransferTries,
                             TransferError = a.TransferError,
                             IsTransferStarted = a.IsTransferStarted,
                             TransferStatusCode = a.TransferStatusCode,
                             TransferStatusName = a.TransferStatus == null ? "" : a.TransferStatus.Name,
                             AccountingExternalCode = a.AccountingExternalCode,
                             ReadyForTransfer = a.TransferStatusCode == "RD" ? true : false,
                             PaymentTermExternalId = a.PaymentTermExternalId,
                             IsDueDateColorRed = (a.DueDate == null || a.StatusCode == "PD") ? false : (a.DueDate.Value < todayDate ? true : false),
                             IsMultipleEntities = a.IsMultipleEntities,
                             ApprovedDate = a.ApprovedDate,
                             ApprovedByUserId = a.ApprovedByUserId,
                             ApprovedByUserName = a.ApprovedByUser == null ? null : (a.ApprovedByUser.Contact == null ? null : a.ApprovedByUser.Contact.EnglishName),
                             OperationalDate = a.OperationalDate,
                             VendorGLAccountId = a.VendorGLAccountId,
                             AccountingDate = a.AccountingDate,
                             IsExternalEntity = a.IsExternalEntity,
                             IsGeneralInvoice = a.IsGeneralInvoice,
                             FirstApproveDate = a.FirstApproveDate,
                             BranchName = a.Branch == null ? null : a.Branch.EnglishName,
                             CreatedByPartner = a.CreatedByPartner,
                             Field1 = a.Field1,
                             Field2 = a.Field2,
                             Field3 = a.Field3,
                             Field4 = a.Field4,
                             Field5 = a.Field5,
                             Field6 = a.Field6,
                             Field7 = a.Field7,
                             Field8 = a.Field8,
                             Field9 = a.Field9,
                             Field10 = a.Field10,
                             TotalVATOnly = a.TotalVATOnly,
                             PaidDate = a.PaidDate,
                             ShipmentsNumbers = a.ShipmentsNumbers,
                             MasterNumbers = a.MasterNumbers,
                             MasterShipmentNumbers = a.MasterShipmentNumbers,
                             HouseNumbers = a.HouseNumbers,
                             GlobalTaxCalculation = a.GlobalTaxCalculation,
                             IsEquipment = a.IsEquipment,
                             ConnectedPaymentsNumbers = a.ConnectedPaymentsNumbers,
                         };

            return result;
        }

        public IQueryable<APInvoiceList> GetUnpaidAPInvoices(int tenant)
        {
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;

            var query = from a in repository.context.APInvoices.Include("InvoiceCurrency").Include("LocalCurrency").Include("TransferStatus").Include("ApprovedByUser").Include("ApprovedByUser.Contact").Include("Branch")
                        where a.Tenant == tenant && a.StatusCode != "WA" && a.StatusCode != "VD" && a.StatusCode != "LL" && !a.IsClosed
                        select new APInvoiceList()
                        {
                            MainEntityId = a.MainEntityId,
                            ProfitCurrencyExchangeRate = a.ProfitCurrencyExchangeRate,
                            ProfitCurrencyId = a.ProfitCurrencyId,
                            SubTotalInInvoiceCurrency = a.SubTotalInInvoiceCurrency,
                            SubTotalInLocalCurrency = a.SubTotalInLocalCurrency,
                            AmountInInvoiceCurrency = a.AmountInInvoiceCurrency,
                            AmountInLocalCurrency = a.AmountInLocalCurrency,
                            AmountInProfitCurrency = a.AmountInProfitCurrency,
                            AmountDue = a.AmountDue,
                            AmountDueInLocalCurrency = a.AmountDueInLocalCurrency,
                            AmountDueInProfitCurrency = a.AmountDueInProfitCurrency,
                            CreateDate = a.CreateDate,
                            CreatedByUserId = a.CreatedByUserId,
                            DueDate = a.DueDate,
                            ExchangeRateDate = a.ExchangeRateDate,
                            Id = a.Id,
                            InternalNotes = a.InternalNotes,
                            InternalNumber = a.InternalNumber,
                            InvoiceCurrencyExchangeRate = a.InvoiceCurrencyExchangeRate,
                            InvoiceCurrencyId = a.InvoiceCurrencyId,
                            InvoiceDate = a.InvoiceDate,
                            InvoiceNumber = a.InvoiceNumber,
                            IsClosed = a.IsClosed,
                            LocalCurrencyId = a.LocalCurrencyId,
                            LocalCurrencyCode = a.LocalCurrency != null ? a.LocalCurrency.Code : null,
                            PaymentTermId = a.PaymentTermId,
                            StatusCode = a.StatusCode,
                            VendorId = a.VendorId,
                            Tenant = a.Tenant,
                            UpdateDate = a.UpdateDate,
                            UpdatedByUserId = a.UpdatedByUserId,
                            VATNumber = a.VATNumber,
                            MainEntityReference = a.MainEntityReference,
                            SearchFields = a.SearchFields,
                            RefundAmount = a.RefundAmount,
                            BranchId = a.BranchId,
                            MasterNumber = a.MasterNumber,
                            HouseNumber = a.HouseNumber,
                            Description = a.Description,
                            InvoiceCurrencyCode = a.InvoiceCurrency == null ? null : a.InvoiceCurrency.Code,
                            CreditAccount = a.CreditAccount,
                            TransferTries = a.TransferTries,
                            TransferError = a.TransferError,
                            IsTransferStarted = a.IsTransferStarted,
                            TransferStatusCode = a.TransferStatusCode,
                            TransferStatusName = a.TransferStatus == null ? "" : a.TransferStatus.Name,
                            AccountingExternalCode = a.AccountingExternalCode,
                            ReadyForTransfer = a.TransferStatusCode == "RD" ? true : false,
                            PaymentTermExternalId = a.PaymentTermExternalId,
                            IsDueDateColorRed = (a.DueDate == null || a.StatusCode == "PD") ? false : (a.DueDate.Value < todayDate ? true : false),
                            IsMultipleEntities = a.IsMultipleEntities,
                            ApprovedDate = a.ApprovedDate,
                            ApprovedByUserId = a.ApprovedByUserId,
                            ApprovedByUserName = a.ApprovedByUser == null ? null : (a.ApprovedByUser.Contact == null ? null : a.ApprovedByUser.Contact.EnglishName),
                            OperationalDate = a.OperationalDate,
                            VendorGLAccountId = a.VendorGLAccountId,
                            AccountingDate = a.AccountingDate,
                            IsExternalEntity = a.IsExternalEntity,
                            IsGeneralInvoice = a.IsGeneralInvoice,
                            FirstApproveDate = a.FirstApproveDate,
                            BranchName = a.Branch == null ? null : a.Branch.EnglishName,
                            CreatedByPartner = a.CreatedByPartner,
                            TotalVATOnly = a.TotalVATOnly,
                            PaidDate = a.PaidDate,
                            ConnectedPaymentsNumbers = a.ConnectedPaymentsNumbers,
                        };

            return query;
        }

        public List<APInvoiceTransferHistoryPM> GetAPInvoiceTransferHistory(string entityId, int tenant)
        {
            AccountingTransferHeaderRepository transferHeaderRepository = new AccountingTransferHeaderRepository(tenant);
            AccountingTransferLineRepository transferLineRepository = new AccountingTransferLineRepository(tenant);
            List<AccountingTransferLine> transferLines = transferLineRepository.GetAPInvoiceTransferLines(entityId, tenant).ToList();
            List<APInvoiceTransferHistoryPM> TransferHistoryList = new List<APInvoiceTransferHistoryPM>();

            foreach (AccountingTransferLine item in transferLines)
            {
                AccountingTransferHeader transferHeader = transferHeaderRepository.GetSingleEntity(item.AccountingTransferHeaderId, tenant);
                if (transferHeader != null)
                {
                    if (transferHeader.AccountingTransferTypeCode == "APIN")
                    {
                        APInvoiceTransferHistoryPM invoiceTransfer = new APInvoiceTransferHistoryPM()
                        {
                            Id = transferHeader.Id,
                            APInvoiceId = entityId,
                            FileName = transferHeader.FileName,
                            TransferDate = transferHeader.TransferDate,
                            TransferNumber = transferHeader.TransferNumber
                        };

                        TransferHistoryList.Add(invoiceTransfer);
                    }
                }
            }
            return TransferHistoryList;
        }

        public IQueryable<APInvoiceList> GetInvoiceListByTenant(int tenant)
        {
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;

            var result = from a in repository.context.APInvoices.Include("Status").Include("LocalCurrency").Include("InvoiceCurrency").Include("ProfitCurrency").Include("VendorCard").Include("PaymentTerm").Include("TransferStatus").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("UpdatedByUser").Include("UpdatedByUser.Contact").Include("ApprovedByUser").Include("ApprovedByUser.Contact").Include("Branch")
                         where a.Tenant == tenant
                         select new APInvoiceList()
                         {
                             MainEntityId = a.MainEntityId,
                             CreateDate = a.CreateDate,
                             CreatedByUserId = a.CreatedByUserId,
                             DueDate = a.DueDate,
                             ExchangeRateDate = a.ExchangeRateDate,
                             AmountInInvoiceCurrency = a.AmountInInvoiceCurrency,
                             AmountInLocalCurrency = a.AmountInLocalCurrency,
                             AmountInProfitCurrency = a.AmountInProfitCurrency,
                             Id = a.Id,
                             InternalNotes = a.InternalNotes,
                             InternalNumber = a.InternalNumber,
                             InvoiceCurrencyExchangeRate = a.InvoiceCurrencyExchangeRate,
                             InvoiceCurrencyId = a.InvoiceCurrencyId,
                             InvoiceDate = a.InvoiceDate,
                             InvoiceNumber = a.InvoiceNumber,
                             IsClosed = a.IsClosed,
                             LocalCurrencyId = a.LocalCurrencyId,
                             LocalCurrencyCode = a.LocalCurrency == null ? null : a.LocalCurrency.Code,
                             PaymentTermId = a.PaymentTermId,
                             ProfitCurrencyExchangeRate = a.ProfitCurrencyExchangeRate,
                             ProfitCurrencyId = a.ProfitCurrencyId,
                             StatusCode = a.StatusCode,
                             SubTotalInInvoiceCurrency = a.SubTotalInInvoiceCurrency,
                             SubTotalInLocalCurrency = a.SubTotalInLocalCurrency,
                             VendorId = a.VendorId,
                             VendorName = a.VendorCard == null ? "" : (a.VendorCard.LocalName != null ? a.VendorCard.LocalName : a.VendorCard.EnglishName),
                             VendorCity = a.VendorCard == null ? "" : a.VendorCard.CityName,
                             VendorCountry = a.VendorCard == null ? "" : a.VendorCard.CountryName,
                             VendorCode = a.VendorCard == null ? null : a.VendorCard.Code,
                             Tenant = a.Tenant,
                             UpdateDate = a.UpdateDate,
                             UpdatedByUserId = a.UpdatedByUserId,
                             VATNumber = a.VATNumber,
                             SearchFields = a.SearchFields,
                             InvoiceCurrencyCode = a.InvoiceCurrency == null ? null : a.InvoiceCurrency.Code,
                             ProfitCurrencyCode = a.ProfitCurrency == null ? null : a.ProfitCurrency.Code,
                             StatusName = a.Status == null ? null : a.Status.Name,
                             AmountDue = a.AmountDue,
                             MainEntityReference = a.MainEntityReference,
                             CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                             UpdatedByUserName = a.UpdatedByUser == null ? null : (a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.EnglishName),
                             PaymentTermName = a.PaymentTerm == null ? null : a.PaymentTerm.EnglishName,
                             MasterNumber = a.MasterNumber,
                             HouseNumber = a.HouseNumber,
                             Description = a.Description,
                             AmountDueInLocalCurrency = a.AmountDueInLocalCurrency,
                             AmountDueInProfitCurrency = a.AmountDueInProfitCurrency,
                             CreditAccount = a.CreditAccount,
                             TransferTries = a.TransferTries,
                             TransferError = a.TransferError,
                             IsTransferStarted = a.IsTransferStarted,
                             TransferStatusCode = a.TransferStatusCode,
                             TransferStatusName = a.TransferStatus == null ? "" : a.TransferStatus.Name,
                             AccountingExternalCode = a.AccountingExternalCode,
                             ReadyForTransfer = a.TransferStatusCode == "RD" ? true : false,
                             PaymentTermExternalId = a.PaymentTermExternalId,
                             IsDueDateColorRed = (a.DueDate == null || a.StatusCode == "PD") ? false : (a.DueDate.Value < todayDate ? true : false),
                             IsMultipleEntities = a.IsMultipleEntities,
                             BranchId = a.BranchId,
                             ApprovedDate = a.ApprovedDate,
                             ApprovedByUserId = a.ApprovedByUserId,
                             ApprovedByUserName = a.ApprovedByUser == null ? null : (a.ApprovedByUser.Contact == null ? null : a.ApprovedByUser.Contact.EnglishName),
                             OperationalDate = a.OperationalDate,
                             VendorGLAccountId = a.VendorGLAccountId,
                             AccountingDate = a.AccountingDate,
                             IsExternalEntity = a.IsExternalEntity,
                             IsGeneralInvoice = a.IsGeneralInvoice,
                             FirstApproveDate = a.FirstApproveDate,
                             BranchName = a.Branch == null ? null : a.Branch.EnglishName,
                             CreatedByPartner = a.CreatedByPartner,
                             Field1 = a.Field1,
                             Field2 = a.Field2,
                             Field3 = a.Field3,
                             Field4 = a.Field4,
                             Field5 = a.Field5,
                             Field6 = a.Field6,
                             Field7 = a.Field7,
                             Field8 = a.Field8,
                             Field9 = a.Field9,
                             Field10 = a.Field10,
                             TotalVATOnly = a.TotalVATOnly,
                             PaidDate = a.PaidDate,
                             ConnectedPaymentsNumbers = a.ConnectedPaymentsNumbers,
                         };

            return result;
        }
        public List<APInvoicePM> GetAllAPInvoicesByIds(List<string> Ids, int tenant)
        {
            List<APInvoicePM> invoicePMs = (from a in repository.context.APInvoices
                                            where Ids.Contains(a.Id) && a.Tenant == tenant
                                            select new APInvoicePM()
                                            {
                                                Id = a.Id,
                                                IsEquipment = a.IsEquipment,
                                                InvoiceNumber = a.InvoiceNumber,
                                                ConfirmationNumber = a.ConfirmationNumber,
                                                VATNumber = a.VATNumber,
                                            }).ToList();

            return invoicePMs;
        }

        public List<APInvoicePM> GetVoidedAPInvoicesByIds(List<string> Ids, int tenant, DateTime taxReportDate)
        {
            DateTime beginOfMonthOfTaxReportDate = new DateTime(taxReportDate.Year, taxReportDate.Month, 1);
            DateTime endOfMonthOfTaxReportDate = new DateTime(taxReportDate.Year, taxReportDate.Month, DateTime.DaysInMonth(taxReportDate.Year, taxReportDate.Month));

            List<APInvoicePM> invoicePMs = (from a in repository.context.APInvoices.Include("Branch")
                                            where Ids.Contains(a.Id) && a.Tenant == tenant 
                                            && a.StatusCode == "VD"
                                            select new APInvoicePM()
                                            {
                                                ProfitCurrencyExchangeRate = a.ProfitCurrencyExchangeRate,
                                                ProfitCurrencyId = a.ProfitCurrencyId,
                                                SubTotalInInvoiceCurrency = a.SubTotalInInvoiceCurrency,
                                                SubTotalInLocalCurrency = a.SubTotalInLocalCurrency,
                                                AmountInInvoiceCurrency = a.AmountInInvoiceCurrency,
                                                AmountInLocalCurrency = a.AmountInLocalCurrency,
                                                AmountInProfitCurrency = a.AmountInProfitCurrency,
                                                AmountInInvoiceCurrency_Summary = a.AmountInInvoiceCurrency,
                                                AmountInLocalCurrency_Summary = a.AmountInLocalCurrency,
                                                AmountInProfitCurrency_Summary = a.AmountInProfitCurrency,
                                                AmountDue = a.AmountDue,
                                                AmountDueInLocalCurrency = a.AmountDueInLocalCurrency,
                                                AmountDueInProfitCurrency = a.AmountDueInProfitCurrency,
                                                CreateDate = a.CreateDate,
                                                CreatedByUserId = a.CreatedByUserId,
                                                DueDate = a.DueDate,
                                                ExchangeRateDate = a.ExchangeRateDate,
                                                Id = a.Id,
                                                InternalNotes = a.InternalNotes,
                                                InternalNumber = a.InternalNumber,
                                                InvoiceCurrencyExchangeRate = a.InvoiceCurrencyExchangeRate,
                                                InvoiceCurrencyId = a.InvoiceCurrencyId,
                                                InvoiceDate = a.InvoiceDate,
                                                InvoiceNumber = a.InvoiceNumber,
                                                IsClosed = a.IsClosed,
                                                LocalCurrencyId = a.LocalCurrencyId,
                                                LocalCurrencyCode = a.LocalCurrency != null ? a.LocalCurrency.Code : null,
                                                InvoiceCurrencyCode = a.InvoiceCurrency == null ? "" : a.InvoiceCurrency.Code,
                                                PaymentTermId = a.PaymentTermId,
                                                StatusCode = a.StatusCode,
                                                StatusName = a.Status == null ? "" : a.Status.Name,
                                                VendorId = a.VendorId,
                                                Tenant = a.Tenant,
                                                UpdateDate = a.UpdateDate,
                                                UpdatedByUserId = a.UpdatedByUserId,
                                                VATNumber = a.VATNumber,
                                                MainEntityId = a.MainEntityId,
                                                MainEntityReference = a.MainEntityReference,
                                                InvoiceExpectedAmount = a.AmountInInvoiceCurrency,
                                                RefundAmount = a.RefundAmount,
                                                BranchId = a.BranchId,
                                                MasterNumber = a.MasterNumber,
                                                HouseNumber = a.HouseNumber,
                                                Description = a.Description,
                                                VendorName = a.VendorCard == null ? "" : (a.VendorCard.LocalName != null ? a.VendorCard.LocalName : a.VendorCard.EnglishName),
                                                VendorContactId = a.VendorCard == null ? "" : a.VendorCard.PrimaryContactId,
                                                VendorVatNumber = a.VendorCard == null ? "" : a.VendorCard.VatNumber,
                                                VendorCity = a.VendorCard == null ? "" : a.VendorCard.CityName,
                                                VendorCountry = a.VendorCard == null ? "" : a.VendorCard.CountryName,
                                                VendorCode = a.VendorCard == null ? "" : a.VendorCard.Code,
                                                VendorPartnerTypeId = a.VendorCard == null ? "" : a.VendorCard.PartnerTypeId,
                                                PaymentTermName = a.PaymentTerm == null ? "" : a.PaymentTerm.EnglishName,
                                                CreditAccount = a.CreditAccount,
                                                TransferTries = a.TransferTries,
                                                TransferError = a.TransferError,
                                                IsTransferStarted = a.IsTransferStarted,
                                                TransferStatusCode = a.TransferStatusCode,
                                                TransferStatusName = a.TransferStatus == null ? "" : a.TransferStatus.Name,
                                                AccountingExternalCode = a.AccountingExternalCode,
                                                ReadyForTransfer = a.TransferStatusCode == "RD" ? true : false,
                                                PaymentTermExternalId = a.PaymentTermExternalId,
                                                IsMultipleEntities = a.IsMultipleEntities,
                                                ApprovedDate = a.ApprovedDate,
                                                ApprovedByUserId = a.ApprovedByUserId,
                                                ApprovedByUserName = a.ApprovedByUser == null ? null : (a.ApprovedByUser.Contact == null ? null : a.ApprovedByUser.Contact.EnglishName),
                                                CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                                                OperationalDate = a.OperationalDate,
                                                VendorGLAccountId = a.VendorGLAccountId,
                                                AccountingDate = a.AccountingDate,
                                                IsExternalEntity = a.IsExternalEntity,
                                                IsGeneralInvoice = a.IsGeneralInvoice,
                                                ExternalAccountingEntityId = a.ExternalAccountingEntityId,
                                                FirstApproveDate = a.FirstApproveDate,
                                                BranchName = a.Branch == null ? null : a.Branch.EnglishName,
                                                CreatedByPartner = a.CreatedByPartner,
                                                TotalVATOnly = a.TotalVATOnly,
                                                PaidDate = a.PaidDate,
                                                ConnectedPaymentsNumbers = a.ConnectedPaymentsNumbers,
                                                TotalEquation = a.TotalEquation,
                                                ConfirmationNumber=a.ConfirmationNumber,
                                            }).ToList();
            return invoicePMs;
        }

        public List<APInvoice_LT_DTO> GetAPInvoicesForStatusUpdate(IAccountingContext accContext, ref GetNextAPInvoicesArgs args)
        {
            int tenant = args.Tenant;
            string invoiceNumber = args.InvoiceNumber;
            string lastCheckedId = args.LastCheckedId;
            DateTime fromInvoiceDate = args.FromInvoiceDate;
            DateTime toInvoiceDate = args.ToInvoiceDate;

            IEnumerable<APInvoice_LT_DTO> query = null;

            IQueryable<APInvoice> dbQuery = null;

            // Invoices 

            if (!string.IsNullOrWhiteSpace(invoiceNumber))
            {
                dbQuery = repository.GetAPInvoices(args.Tenant).Where(rec => rec.InvoiceNumber == invoiceNumber
                     && (rec.StatusCode == "AD" || rec.StatusCode == "PD" || rec.StatusCode == "PP")); 
            }

            else if (!string.IsNullOrWhiteSpace(lastCheckedId))
            {
                dbQuery = repository.GetAPInvoices(args.Tenant).Where(rec => rec.InvoiceDate >= fromInvoiceDate && rec.InvoiceDate <= toInvoiceDate
                     && String.Compare(rec.Id, lastCheckedId) > 0
                     && (rec.StatusCode == "AD" || rec.StatusCode == "PD" || rec.StatusCode == "PP")).OrderBy(r => r.Id);
            }

            else 
            {
                dbQuery = repository.GetAPInvoices(args.Tenant).Where(rec => rec.InvoiceDate >= fromInvoiceDate && rec.InvoiceDate <= toInvoiceDate
                     && (rec.StatusCode == "AD" || rec.StatusCode == "PD" || rec.StatusCode == "PP")).OrderBy(r => r.Id);
            }

            var invQuery = dbQuery.Select(dbInv =>
                new APInvoice_DTO
                {
                    Id = dbInv.Id,
                    Tenant = dbInv.Tenant,
                    InvoiceNumber = dbInv.InvoiceNumber,
                    StatusCode = dbInv.StatusCode,
                    IsClosed = dbInv.IsClosed,
                }).ToArray();

                    
            string long_text = "";
            bool isInDebuggingMode = false;
            bool isOk = true;

            if (!string.IsNullOrWhiteSpace(invoiceNumber))
            {
                var oneInvList = dbQuery.ToList();
                if (oneInvList != null && oneInvList.Count == 1)
                {
                    fromInvoiceDate = oneInvList.FirstOrDefault().InvoiceDate.Value;
                    toInvoiceDate = fromInvoiceDate;
                }
                else
                {
                    isOk = false;
                }
            }


            if (invQuery != null && invQuery.Length > 0 && isOk)
            {
                JournalRepository journalRepository = new JournalRepository(accContext);
                JournalLineRepository journalLineRepository = new JournalLineRepository(accContext);
                LedgerTransactionRepository ledgerTransactionRepository = new LedgerTransactionRepository(accContext);

                // Journals and Journal Lines
                var j_l_prequery = from jl in journalLineRepository.GetAll(tenant).Where(rec => rec.ActionCode == "1").Where(rec => rec.AccountingDate >= fromInvoiceDate && rec.AccountingDate <= toInvoiceDate)
                                    join
                                    j in journalRepository.GetAll(tenant).Where(rec => rec.AccountingEntityCode == CloseTables.AccountingEntityValues.APInvoice && rec.StatusCode == "2" 
                                                                                    && rec.AccountingDate >= fromInvoiceDate && rec.AccountingDate <= toInvoiceDate)
                                    on jl.JournalId equals j.Id
                                    select new J_L_DTO()
                                    {
                                        JournalId = j.Id,
                                        JournalLineNumber = jl.Line,
                                        AccountingEntityId = j.AccountingEntityId,
                                    };

                // Journals and Journal Lines and Transactions 
                var j_lt_query = from jl in j_l_prequery
                                 join lt in ledgerTransactionRepository.GetAll(tenant)
                                 on jl.JournalId equals lt.JournalId
                                 where lt.JournalLineNumber == jl.JournalLineNumber
                                 select new J_LT_DTO()
                                 {
                                     AccountingEntityId = jl.AccountingEntityId,
                                     JournalId = jl.JournalId,
                                     JournalLineNumber = jl.JournalLineNumber,
                                     LT_Id = lt.Id,
                                     LT_LocalAmountCredit = lt.LocalAmountCredit,
                                     LT_LocalAmountDebit = lt.LocalAmountDebit,
                                     LT_OpenAmount = lt.OpenAmount,
                                 };


                J_LT_DTO[] j_l_query_arry = j_lt_query.ToArray();

                // All together, without additional conditions 
                var j_inv_l_query = j_l_query_arry 
                               .Join(invQuery, jrn => jrn.AccountingEntityId, inv => inv.Id, (jrn, inv) =>
                               new APInvoice_LT_DTO()
                               {
                                   APInvoiceId = inv.Id,
                                   APInvoiceStatusCode = inv.StatusCode,
                                   APInvoiceIsClosed = inv.IsClosed,
                                   JournalId = jrn.JournalId,
                                   JournalLineNumber = jrn.JournalLineNumber,
                                   LT_LocalAmountCredit = jrn.LT_LocalAmountCredit,
                                   LT_LocalAmountDebit = jrn.LT_LocalAmountDebit,
                                   LT_OpenAmount = jrn.LT_OpenAmount,
                                   LT_Id = jrn.LT_Id,
                               });



                // All together, with additional conditions 
                query = j_inv_l_query.Where(rec => (rec.APInvoiceStatusCode != "AD" && ((rec.LT_LocalAmountDebit != 0m && rec.LT_OpenAmount == rec.LT_LocalAmountDebit)
                            || (rec.LT_LocalAmountDebit == 0m && rec.LT_OpenAmount == rec.LT_LocalAmountCredit * -1)))
                            || (rec.APInvoiceStatusCode != "PP" && rec.LT_OpenAmount != 0m)
                            || (rec.APInvoiceStatusCode != "PD" && rec.LT_OpenAmount == 0m)).ToList();


            }
            else
            {
                args.Stop = true;
            }

            List<APInvoice_LT_DTO> ret_list = null;
            if (query != null)
                ret_list = query.ToList();
            else
                ret_list = new List<APInvoice_LT_DTO>();
            long_text = "";
            isInDebuggingMode = false;
            if (isInDebuggingMode)
            {
                int ctr = 1;
                ret_list.ForEach(item => long_text += "#" + ctr++ + "," + item.APInvoiceId + "," + item.APInvoiceIsClosed + "," + item.JournalId + "," + item.JournalLineNumber + "," + item.LT_Id + "," + item.LT_LocalAmountCredit + "," + item.LT_LocalAmountDebit + "," + item.LT_OpenAmount + "\n");
            }

            return ret_list;
        }








        private class APInvoice_DTO
        {
            public string Id { get; set; }
            public int Tenant { get; set; }
            public string InvoiceNumber { get; set; }
            public string StatusCode { get; set; }
            public bool IsClosed { get; set; }
        }

        private class J_L_DTO
        {
            public J_L_DTO()
            {
            }

            public string JournalId { get; set; }
            public int JournalLineNumber { get; set; }
            public string AccountingEntityId { get; set; }
        }


        private class J_LT_DTO
        {
            public J_LT_DTO()
            {
            }

            public string AccountingEntityId { get; set; }
            public string JournalId { get; set; }
            public int JournalLineNumber { get; set; }
            public string LT_Id { get; set; }
            public decimal LT_LocalAmountCredit { get; set; }
            public decimal LT_LocalAmountDebit { get; set; }
            public decimal LT_OpenAmount { get; set; }
        }
    }

    public class GetNextAPInvoicesArgs
    {
        public int Tenant { get; set; }
        public string InvoiceNumber { get; set; }
        public string LastCheckedId { get; set; }
        public DateTime FromInvoiceDate { get; set; }
        public DateTime ToInvoiceDate { get; set; }
        public int ThisTimeMadeCount { get; set; }
        public bool Stop { get; set; }


    }

    public class APInvoice_LT_DTO
    {
        public string APInvoiceId { get; set; }
        public string APInvoiceStatusCode { get; set; }
        public bool APInvoiceIsClosed { get; set; }
        public string JournalId { get; set; }
        public int JournalLineNumber { get; set; }
        public string LT_Id { get; set; }
        public decimal LT_LocalAmountDebit { get; set; }
        public decimal LT_LocalAmountCredit { get; set; }
        public decimal LT_OpenAmount { get; set; }
    }



}