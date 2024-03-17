using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.BL.CommonDataModel.APIDataContract;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.APIDataContract.ApiV1;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.APIDataContract.ApiV1
{
     public partial class ARInvoiceQueryService
    {

        public void SetInvoiceLinesEntityId(ARInvoicePM entity, int tenant)
        {
            HouseQueryService queryService = new HouseQueryService(tenant);
            string shipmentId = queryService.GetShipmentIdByNumber(entity.MainEntityReference, tenant);

            if (entity.InvoiceLines != null && entity.InvoiceLines.Count > 0)
            {
                foreach(ARInvoiceLinePM item in entity.InvoiceLines)
                {
                    item.EntityId = shipmentId;
                    
                }
            }
        }

        public void SetBillToGLAccountId(ARInvoicePM entity)
        {
            if (entity.BillToGLAccountId != null)
            {
                IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
                Logitude.Accounting.Def.EntityPMs.GLAccountPM gLAccount = glAccountQuery.GetGLAccountByInternalNumber(entity.BillToGLAccountId, entity.Tenant);
                if (gLAccount != null)
                {

                    entity.BillToGLAccountId = gLAccount.Id;
                }
                else
                {
                    throw new Exception("GLAccount with internal number " + entity.BillToGLAccountId + "does not exist");
                }
            }
        }

        public ARInvoicePM UpdateCreditInvoice(ARInvoicePM invoice, int tenant)
        {
            ARInvoicePM invoicePM = null;
            if (String.IsNullOrEmpty(invoice.Id))
            {
                invoice.Id = IdCounter.GetNumber("ARInvoice", invoice.Tenant).ToString();
            }
            if (invoice.CreditARInvoice != null)
            {
                invoicePM = query.GetSingleInvoiceByInvoiceNumber(invoice.CreditARInvoice, tenant);
            
                if (invoicePM != null && invoicePM.StatusCode == "AD")
                {
                    if (invoicePM.BillToId != invoice.BillToId)
                    {
                        throw new ApplicationException("The bill to is different than the credited invoice bill to");
                    }
                    if (Math.Abs(invoicePM.AmountInLocalCurrency.Value) != Math.Abs(invoice.AmountInLocalCurrency.Value))
                    {
                        throw new ApplicationException("The total amount is different than the credited invoice total amount");
                    }

                    ARInvoiceRepository invoiceRepository = new ARInvoiceRepository(tenant);

                    ARInvoiceService service = new ARInvoiceService(context, tenant);

                    if (invoicePM.StatusCode == "AR")
                    {
                        throw new ApplicationException("this invoice is already auto credited");
                    }

                    else
                    {


                        invoice.StatusCode = "AC";
                        invoice.IsAutoCredit = true;

                        //  invoice.InvoiceDate = DateTime.Today;

                        invoice.SubTotalInInvoiceCurrency = invoice.SubTotalInInvoiceCurrency;
                        invoice.SubTotalInLocalCurrency = invoice.SubTotalInLocalCurrency;
                        invoice.AmountInInvoiceCurrency = invoice.AmountInInvoiceCurrency;
                        invoice.AmountInLocalCurrency = invoice.AmountInLocalCurrency;
                        invoice.AmountInProfitCurrency = invoice.AmountInProfitCurrency;
                        invoice.AmountDue = 0;
                        invoice.AmountDueInLocalCurrency = 0;
                        invoice.AmountDueInProfitCurrency = 0;
                        invoice.CreditedByARInvoiceId = invoicePM.Id;



                        int i = 1;
                        foreach (ARInvoiceLinePM item in invoice.InvoiceLines)
                        {
                            item.UnitPrice = item.UnitPrice;
                            item.ForiegnCurrencyAmount = item.ForiegnCurrencyAmount;
                            item.LocalCurrencyAmount = item.LocalCurrencyAmount;
                            item.ProfitCurrencyAmount = item.ProfitCurrencyAmount;
                            item.InvoiceCurrencyAmount = item.InvoiceCurrencyAmount;
                        }

                        //  service.Update(invoicePM);



                    
                      
                        invoicePM.IsCancelled = true;
                        invoicePM.CancelledByARInvoiceId = invoice.Id;
                        invoicePM.StatusCode = "AR";
                        invoicePM.AmountDue = 0;
                        invoicePM.AmountDueInLocalCurrency = 0;
                        invoicePM.AmountDueInProfitCurrency = 0;
                      //  service.Update(invoicePM, true);


                    }
                }
                }

            return invoicePM;
          
         }

        public ARInvoice SetARInvoiceSystemUser(ARInvoice entity)
        {
            String systemUserId = "";
            UserPM myCreatedByUserPM = null;
            User systemUser = null;
            if (entity.CreatedByUser != null)
            {
                UserQuery query = new UserQuery(entity.Tenant);
                myCreatedByUserPM = query.UserCustomDataMappingAndValidatin(entity.CreatedByUser, entity.Tenant);
            }



            if (entity.CreatedByUser == null || myCreatedByUserPM == null)
            {
                systemUserId = AuthenticationUtil.ResolveSystemUserId(entity.Tenant);
                if (!String.IsNullOrEmpty(systemUserId))
                {
                    entity.CreatedByUser = null;

                    UserQueryService userQueryService = new UserQueryService(entity.Tenant);
                    systemUser = userQueryService.GetUserById(systemUserId, entity.Tenant);
                    if (systemUser != null)
                    {
                        entity.CreatedByUser = new User();
                        entity.CreatedByUser.Id = systemUser.Id;
                        entity.CreatedByUser.EnglishName = systemUser.EnglishName;
                        entity.CreatedByUser.ExternalCode = systemUser.ExternalCode;
                        entity.CreatedByUser.LocalName = systemUser.LocalName;

                    }
                }
            }


            UserPM myIssuedByUserPM = null;
            if (entity.IssuedByUser != null)
            {
                UserQuery query = new UserQuery(entity.Tenant);
                myIssuedByUserPM = query.UserCustomDataMappingAndValidatin(entity.IssuedByUser, entity.Tenant);
            }



            if (entity.IssuedByUser == null || myIssuedByUserPM == null)
            {
                entity.IssuedByUser = null;
                if (systemUser != null)
                {
                    entity.IssuedByUser = new User();
                    entity.IssuedByUser.Id = systemUser.Id;
                    entity.IssuedByUser.EnglishName = systemUser.EnglishName;
                    entity.IssuedByUser.ExternalCode = systemUser.ExternalCode;
                    entity.IssuedByUser.LocalName = systemUser.LocalName;

                }
            }



            return entity;
        }



        public JournalPM GetARInvoiceExistingJournalPM(ARInvoice entity)
        {
            JournalPM journalPM = null;
            if (String.IsNullOrWhiteSpace(entity.ExistingExternalJournal) && entity.DoNotCreateJournal.HasValue && entity.DoNotCreateJournal.Value == true)
            {
                throw new ApplicationException("ExistingExternalJournal is required when DoNotCreateJournal==true");
            }

            if (!String.IsNullOrWhiteSpace(entity.ExistingExternalJournal) && !(entity.DoNotCreateJournal.HasValue && entity.DoNotCreateJournal.Value == true))
            {
                throw new ApplicationException("DoNotCreateJournal required to be true when ExistingExternalJournal is present");
            }

            if (!String.IsNullOrWhiteSpace(entity.ExistingExternalJournal) && entity.DoNotCreateJournal.HasValue && entity.DoNotCreateJournal.Value == true)
            {
                IJournalQueryServiceExt journalQuery = ContainerAccessor.Container.Resolve(typeof(IJournalQueryServiceExt), "JournalQueryServiceExt", new ParameterOverride("", 1)) as IJournalQueryServiceExt;
                journalPM = GetSingleJournalByExternalNoAndExternalSystem(entity.ExistingExternalJournal, "AMITAL", entity.Tenant);

                if (journalPM == null)
                {
                    throw new ApplicationException("ExistingExternalJournal " + entity.ExistingExternalJournal + " from AMITAL is not found");
                }
            }
            return journalPM;
        }



        public ARInvoice ARInvoiceDataMappingAndValidatin(ARInvoicePM MyEntity, int Tenant, string ComputingPartnerName = "")
        {
            try
            {
                var temp = new ARInvoice();
                //if (!string.IsNullOrEmpty(MyEntity.Id))
                //{
                //    temp = query.GetSinglePM(MyEntity.Id, Tenant);
                //}

                //if (temp == null)
                //{
                //    throw new ApplicationException("ARInvoice with Id " + MyEntity.Id + " Isn't exist");
                //}
                if (string.IsNullOrEmpty(temp.Id))
                {
                    temp.Id = MyEntity.Id;
                }
                ARInvoiceTypeQueryService InvoiceTypeARInvoiceTypeService = new ARInvoiceTypeQueryService(Tenant);
                if (MyEntity.ARInvoiceTypeCode != null)
                {
                    var myInvoiceType = InvoiceTypeARInvoiceTypeService.GetARInvoiceTypeByCode(MyEntity.ARInvoiceTypeCode, Tenant);
                    if (myInvoiceType != null)
                    {
                       
                        temp.InvoiceType = new ARInvoiceType();
                        temp.InvoiceType.Code = myInvoiceType.Code;
                        temp.InvoiceType.Name = myInvoiceType.Name;
                    }
                }
                CardQueryService BillToCardService = new CardQueryService(Tenant);
                if (MyEntity.BillToId != null)
                {
                    var myBillTo = BillToCardService.GetCardById(MyEntity.BillToId, Tenant);
                    if (myBillTo != null)
                    {

                        temp.BillTo = new Card();
                        temp.BillTo.Id = myBillTo.Id;
                        temp.BillTo.EnglishName = myBillTo.EnglishName;
                        temp.BillTo.LocalName = myBillTo.LocalName;
                        temp.BillTo.Code = myBillTo.Code;
                    }
                }
                temp.InvoiceNumber = MyEntity.InvoiceNumber;
                temp.InvoiceDate = MyEntity.InvoiceDate;
                temp.PrintDate = MyEntity.PrintDate;
                temp.IsPrinted = MyEntity.IsPrinted;
                temp.MainEntityReference = MyEntity.MainEntityReference;
                temp.IsConstituentInvoice = MyEntity.IsConstituentInvoice;
                temp.IsConsolidationInvoice = MyEntity.IsConsolidationInvoice;
                CurrencyQueryService InvoiceCurrencyCurrencyService = new CurrencyQueryService(Tenant);
                if (MyEntity.InvoiceCurrencyId != null)
                {
                    var myInvoiceCurrency = InvoiceCurrencyCurrencyService.GetCurrencyById(MyEntity.InvoiceCurrencyId, Tenant);
                    if (myInvoiceCurrency != null)
                    {
                        temp.InvoiceCurrency = new Currency();
                        temp.InvoiceCurrency.Id = myInvoiceCurrency.Id;
                        temp.InvoiceCurrency.Code = myInvoiceCurrency.Code;
                        temp.InvoiceCurrency.EnglishName = myInvoiceCurrency.EnglishName;
                        temp.InvoiceCurrency.LocalName = myInvoiceCurrency.LocalName;

                    }
                }
                temp.AmountInLocalCurrency = MyEntity.AmountInLocalCurrency;
                temp.CancelledByARInvoice = MyEntity.CancelledByARInvoiceId;
                UserQueryService CreatedByUserUserService = new UserQueryService(Tenant);
                if (MyEntity.CreatedByUserId != null)
                {
                    var myCreatedByUser = CreatedByUserUserService.GetUserById(MyEntity.CreatedByUserId, Tenant);
                    if (myCreatedByUser != null)
                    {

                        temp.CreatedByUser = new User();
                        temp.CreatedByUser.Id = myCreatedByUser.Id;
                        temp.CreatedByUser.EnglishName = myCreatedByUser.EnglishName;
                        temp.CreatedByUser.LocalName = myCreatedByUser.LocalName;
                        temp.CreatedByUser.ExternalCode = myCreatedByUser.ExternalCode;
                    }
                }
                temp.VATNumber = MyEntity.VatNumber;
                AddressQueryService BillToAddressAddressService = new AddressQueryService(Tenant);
                if (MyEntity.BillToAddressId != null)
                {
                    var myBillToAddress = BillToAddressAddressService.GetAddressById(MyEntity.BillToAddressId, Tenant);
                    if (myBillToAddress != null)
                    {


                        temp.BillToAddress = new Address();
                        temp.BillToAddress.ExternalId = myBillToAddress.ExternalId;
                        temp.BillToAddress.Name = myBillToAddress.Name;
                        temp.BillToAddress.Address1 = myBillToAddress.Address1;
                        temp.BillToAddress.Address2 = myBillToAddress.Address2;

                     
                    }
                }
                temp.PrintNotes = MyEntity.PrintNotes;
                //  temp.IssuedByUser = MyEntity.IssuedByUserId;
                UserQueryService IssuedByUserService = new UserQueryService(Tenant);
                if (MyEntity.IssuedByUserId != null)
                {
                    var issuedByUser = IssuedByUserService.GetUserById(MyEntity.IssuedByUserId, Tenant);
                    if (issuedByUser != null)
                    {

                        temp.IssuedByUser = new User();
                        temp.IssuedByUser.Id = issuedByUser.Id;
                        temp.IssuedByUser.EnglishName = issuedByUser.EnglishName;
                        temp.IssuedByUser.LocalName = issuedByUser.LocalName;
                        temp.IssuedByUser.ExternalCode = issuedByUser.ExternalCode;
                    }

                    
                }
                else
                {
                    temp.IssuedByUser = new User();
                    temp.IssuedByUser.Id =temp.CreatedByUser.Id;
                    temp.IssuedByUser.EnglishName = temp.CreatedByUser.EnglishName;
                    temp.IssuedByUser.LocalName = temp.CreatedByUser.LocalName;
                    temp.IssuedByUser.ExternalCode = temp.CreatedByUser.ExternalCode;
                }
                temp.InvoiceCurrencyExchangeRate = MyEntity.InvoiceCurrencyExchangeRate;
                if (MyEntity.InvoiceLines != null && MyEntity.InvoiceLines.Count > 0)
                {
                    ARInvoiceLineQueryService ARInvoiceLineService9 = new ARInvoiceLineQueryService(Tenant);
                    temp.ARInvoiceLines = ARInvoiceLineService9.ARInvoiceLineCustomDataMapping(MyEntity,MyEntity.InvoiceLines, Tenant);
                }


                temp.DueDate = MyEntity.DueDate;
                temp.SubTotalInInvoiceCurrency = MyEntity.SubTotalInInvoiceCurrency;
                temp.SubTotalInLocalCurrency = MyEntity.SubTotalInLocalCurrency;
                temp.AmountInInvoiceCurrency = MyEntity.AmountInInvoiceCurrency;
                ARInvoiceStatusQueryService StatusARInvoiceStatusService = new ARInvoiceStatusQueryService(Tenant);
                //if (MyEntity.StatusCode != null)
                //{
                //    var myStatus = StatusARInvoiceStatusService.GetARInvoiceStatusByCode(MyEntity.StatusCode, Tenant);
                //    if (myStatus != null)
                //    {

                //        temp.Status = new ARInvoiceStatus();
                //        temp.Status.Code = myStatus.Code;
                //        temp.Status.Name = myStatus.Name;
                //    }
                //}
                /*if(MyEntity.isDr)*/
                temp.ProfitCurrencyExchangeRate = MyEntity.ProfitCurrencyExchangeRate;
                temp.AmountInProfitCurrency = MyEntity.AmountInProfitCurrency;
                ARInvoiceTransferStatusQueryService TransferStatusARInvoiceTransferStatusService = new ARInvoiceTransferStatusQueryService(Tenant);
                if (MyEntity.TransferStatusCode != null)
                {
                    var myTransferStatus = TransferStatusARInvoiceTransferStatusService.GetARInvoiceTransferStatusByCode(MyEntity.TransferStatusCode, Tenant);
                    if (myTransferStatus != null)
                    {


                        temp.TransferStatus = new ARInvoiceTransferStatus();
                        temp.TransferStatus.Code = myTransferStatus.Code;
                        temp.TransferStatus.Name = myTransferStatus.Name;
                    }
                }
                BranchQueryService BranchBranchService = new BranchQueryService(Tenant);
                if (MyEntity.BranchId != null)
                {
                    var myBranch = BranchBranchService.GetBranchById(MyEntity.BranchId, Tenant);
                    if (myBranch != null)
                    {

                        temp.Branch = new Branch();
                        temp.Branch.Id = myBranch.Id;
                        temp.Branch.EnglishName = myBranch.EnglishName;
                        temp.Branch.Code = myBranch.Code;
                        temp.Branch.LocalName = myBranch.LocalName;
                    }
                }
                CurrencyQueryService LocalCurrencyCurrencyService = new CurrencyQueryService(Tenant);
                if (MyEntity.LocalCurrencyId != null)
                {
                    var myLocalCurrency = LocalCurrencyCurrencyService.GetCurrencyById(MyEntity.LocalCurrencyId, Tenant);
                    if (myLocalCurrency != null)
                    {

                        temp.LocalCurrency = new Currency();
                        temp.LocalCurrency.Id = myLocalCurrency.Id;
                        temp.LocalCurrency.Code = myLocalCurrency.Code;
                        temp.LocalCurrency.EnglishName = myLocalCurrency.EnglishName;
                        temp.LocalCurrency.LocalName = myLocalCurrency.LocalName;
                    }
                }
                temp.Tenant = MyEntity.Tenant;
                temp.IsDraft = MyEntity.IsDraft;
                temp.ExternalAccountingEntityId = MyEntity.ExternalAccountingEntityId;
                if(MyEntity.BillToGLAccountId != null)
                temp.BillToGLAccount = GetGLAccountNumberById(MyEntity.BillToGLAccountId, MyEntity.Tenant);
                temp.MasterNumber = MyEntity.MasterNumber;

                return temp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ARInvoice GetARInvoiceByInvoiceNumber(string number, int tenant)
        {
            try
            {


                var temp = query.GetSingleInvoiceByInvoiceNumber(number, tenant);
                if (temp == null)
                    throw new ApplicationException("ARInvoice with number " + number + " doesn't exist");

                return ARInvoiceDataMapping(temp, tenant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ARInvoiceLite GetARInvoiceLiteByInvoiceNumber(string number, int tenant)
        {
            try
            {


                string aRInvoiceId = query.GetSingleInvoiceIdByInvoiceNumber(number, tenant);

                return new ARInvoiceLite() { Id = aRInvoiceId };
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public ARInvoiceLite GetARInvoiceLiteById(string invoiceId, int tenant, string ComputingPartnerName = "")
        {
            try
            {
                string aRInvoiceId = query.GetCheckInvoiceId(invoiceId, tenant);

                return new ARInvoiceLite() { Id = aRInvoiceId };
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string GetGLAccountNumberById(string id, int tenant )
        {
            IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
            Logitude.Accounting.Def.EntityPMs.GLAccountPM gLAccount = glAccountQuery.GetSingleGLAccountPM(id,tenant);
            if (gLAccount != null)
            {
                return gLAccount.DisplayNumber;

            }
            else return null;
        }

        public ARInvoice ARInvoiceCustomDataMapping(string Id, int Tenant)
        {
            try
            {

                ARInvoiceQueryService ARInvoiceService0 = new ARInvoiceQueryService(Tenant);
                var invoice = GetARInvoiceByInvoiceNumber(Id, Tenant);
                return invoice;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ValidateAccountingExternalEntityId(ARInvoice invoice)
        {
            if (invoice.ExternalAccountingEntityId != null)
            {
                ARInvoiceQuery invoiceQuery = new ARInvoiceQuery(invoice.Tenant);
                bool exist=  invoiceQuery.CheckARInvoiceByExternalAccountingEnityId(invoice.ExternalAccountingEntityId, invoice.Tenant);
                if (exist)
                {

                    throw new Exception("ARInvoice with the same ExternalEntityId already exist");
                }
            }
        }

        private JournalPM GetSingleJournalByExternalNoAndExternalSystem(string externalNo, string externalSystem, int tenant)
        {
            IJournalQueryServiceExt journalQuery = ContainerAccessor.Container.Resolve(typeof(IJournalQueryServiceExt), "JournalQueryServiceExt", new ParameterOverride("", 1)) as IJournalQueryServiceExt;
            return journalQuery.GetSingleJournalByExternalNoAndExternalSystem(externalNo, externalSystem, tenant);


        }


    }
}
