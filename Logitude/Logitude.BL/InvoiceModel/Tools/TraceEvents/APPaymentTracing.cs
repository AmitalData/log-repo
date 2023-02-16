using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.APIDataContract.ApiV1;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.BL.Security;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System;

namespace Logitude.BL.InvoiceModel.Tools.TraceEvents
{
    public class APPaymentTracing
    {
        public static void Trace(EntityPMs.APPaymentPM entityPM, APPayment payment, bool isNewState)
        {
            string myEntityName = "APPayment";
            APPaymentRepository repository = new APPaymentRepository(entityPM.Tenant);
            ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(entityPM.Tenant);
            bool showLocals = !loggedContact.DontShowLocal;
            var oldValue = TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPM.Tenant, showLocals);
            var newValue = TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPM.Tenant, showLocals);
            var notes = "";

            if (isNewState)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CRAP",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = myEntityName,
                });
                return;
            }


            if (entityPM.SetApproved)
            {
                if (payment.StatusCode != "AD" && entityPM.StatusCode == "AD")
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "APPA",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = myEntityName,
                    });
                }
            }

            else if (entityPM.SetVoided)
            {
                if (payment.StatusCode != "VD" && entityPM.StatusCode == "VD")
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "APPV",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = myEntityName,
                    });
                }
            }

            else if (entityPM.SetCancelApproval)
            {
                if (payment.StatusCode != "DR" && entityPM.StatusCode == "DR")
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "APPC",
                        UserId = loggedContact.Id,
                        EntityId = entityPM.Id,
                        ObjectTableName = myEntityName,
                    });
                }
            }
            else if (entityPM.InternalNotes != payment.InternalNotes || entityPM.PrintNotes != payment.PrintNotes)
            {
                var isInternalNotesChanged = entityPM.InternalNotes != payment.InternalNotes;
                var isPrintNotesChanged = entityPM.PrintNotes != payment.PrintNotes;
                var isBothChanged = isInternalNotesChanged && isPrintNotesChanged;
                var InternalNotes = "Internal Notes Updated: " + oldValue + payment.InternalNotes + newValue + entityPM.InternalNotes;
                var PrintNotes = "Print Notes Updated: " + oldValue + payment.PrintNotes + newValue + entityPM.PrintNotes;
                if (isBothChanged)
                {
                    notes = InternalNotes + ", " + PrintNotes;
                }
                else if (isInternalNotesChanged)
                {
                    notes = InternalNotes;
                }
                else
                {
                    notes = PrintNotes;
                }
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPAP",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = myEntityName,
                    Notes = notes
                });
            }
            if (entityPM.PaymentCurrencyExchangeRate != payment.PaymentCurrencyExchangeRate)
            {
                var isPaymentCurrencyExchangeRateChanged = entityPM.PaymentCurrencyExchangeRate != payment.PaymentCurrencyExchangeRate;
                var PaymentCurrencyExchangeRate = "Payment Currency Exchange Rate Updated: " + oldValue + payment.PaymentCurrencyExchangeRate + newValue + entityPM.PaymentCurrencyExchangeRate;
                if (isPaymentCurrencyExchangeRateChanged)
                {
                    notes = PaymentCurrencyExchangeRate;
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPAP",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = myEntityName,
                    Notes = notes
                });
            }
            if (entityPM.RegisterDate != payment.RegisterDate)
            {
                var isRegisterDateChanged = entityPM.RegisterDate != payment.RegisterDate;
                var RegisterDate = "Register Date Updated: " + oldValue + payment.RegisterDate + newValue + entityPM.RegisterDate;
                if (isRegisterDateChanged)
                {
                    notes = RegisterDate;
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPAP",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = myEntityName,
                    Notes = notes
                });
            }
            if (entityPM.PaymentCurrencyId != payment.PaymentCurrencyId)
            {
                Currency currency = CurrencyRepository.GetSingleCurrency(payment.PaymentCurrencyId, payment.Tenant, true);
                var isPaymentCurrencyIdChanged = entityPM.PaymentCurrencyId != payment.PaymentCurrencyId;
                var PaymentCurrencyId = "Payment Currency Updated: " + oldValue + currency.Code + newValue + entityPM.PaymentCurrencyCode;
                if (isPaymentCurrencyIdChanged)
                {
                    notes = PaymentCurrencyId;
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPAP",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = myEntityName,
                    Notes = notes
                });
            }
            if (entityPM.BranchId != payment.BranchId)
            {
                BranchRepository rep = new BranchRepository(entityPM.Tenant);

                Branch oldBranch = rep.GetSingleBranch(payment.BranchId, payment.Tenant);
                Branch newBranch = rep.GetSingleBranch(entityPM.BranchId, entityPM.Tenant);
                var isBranchIdChanged = entityPM.BranchId != payment.BranchId;
                var BranchId = "Branch Updated: " + oldValue + oldBranch?.EnglishName + newValue + newBranch.EnglishName;
                if (isBranchIdChanged)
                {
                    notes = BranchId;
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPAP",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = myEntityName,
                    Notes = notes
                });
            }

            if (entityPM.AmountInPaymentCurrency != payment.AmountInPaymentCurrency)
            {
                var isAmountInPaymentCurrencyChanged = entityPM.AmountInPaymentCurrency != payment.AmountInPaymentCurrency;
                var AmountInPaymentCurrency = "Amount In Payment Currency Updated: " + oldValue + payment.AmountInPaymentCurrency + newValue + entityPM.AmountInPaymentCurrency;
                if (isAmountInPaymentCurrencyChanged)
                {
                    notes = AmountInPaymentCurrency;
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPAP",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = myEntityName,
                    Notes = notes
                });
            }

            if (entityPM.AccountingPaymentMethodId != payment.AccountingPaymentMethodId)
            {

                AccountingPaymentMethodRepository paymentMethodRep = new AccountingPaymentMethodRepository(repository.context);
                var oldMethod = paymentMethodRep.GetSingleAccountingPaymentMethod(payment.AccountingPaymentMethodId, payment.Tenant);
                var newMethod = paymentMethodRep.GetSingleAccountingPaymentMethod(entityPM.AccountingPaymentMethodId, entityPM.Tenant);
                var isAccountingPaymentMethodIdChanged = entityPM.AccountingPaymentMethodId != payment.AccountingPaymentMethodId;
                var AccountingPaymentMethodId = "Payment Method Updated: " + oldValue + oldMethod.Name + newValue + newMethod.Name;
                if (isAccountingPaymentMethodIdChanged)
                {
                    notes = AccountingPaymentMethodId;
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPAP",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = myEntityName,
                    Notes = notes
                });
            }
            if (entityPM.VendorId != payment.VendorId)
            {
                CardRepository cardRep = new CardRepository(entityPM.Tenant);
                Card oldCard = cardRep.GetSingleCard(payment.VendorId, entityPM.Tenant);
                Card newCard = cardRep.GetSingleCard(entityPM.VendorId, entityPM.Tenant);
                var isVendorIdChanged = entityPM.VendorId != payment.VendorId;
                var VendorId = "Vendor Updated: " + oldValue + (oldCard.EnglishName ?? oldCard.LocalName) + newValue + (newCard.EnglishName ?? newCard.LocalName);
                if (isVendorIdChanged)
                {
                    notes = VendorId;
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPAP",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = myEntityName,
                    Notes = notes
                });
            }
            if (entityPM.VendorAddressId != payment.VendorAddressId)
            {
                AddressRepository addressRepository = new AddressRepository(payment.Tenant);
                Address oldAddress = addressRepository.GetSingleAddress(payment.VendorAddressId, payment.Tenant);
                Address newAddress = addressRepository.GetSingleAddress(entityPM.VendorAddressId, payment.Tenant);
                var isVendorAddressIdChanged = entityPM.VendorAddressId != payment.VendorAddressId;
                var VendorAddressId = "Vendor Address Updated: " + oldValue + oldAddress.Description + newValue + newAddress.Description;
                if (isVendorAddressIdChanged)
                {
                    notes = VendorAddressId;
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPAP",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = myEntityName,
                    Notes = notes
                });
            }
            if (entityPM.TaxDeductionLocalAmount != payment.TaxDeductionLocalAmount)
            {
                var isTaxDeductionLocalAmountChanged = entityPM.TaxDeductionLocalAmount != payment.TaxDeductionLocalAmount;
                var TaxDeductionLocalAmount = "Tax Deduction  Updated: " + oldValue + payment.TaxDeductionLocalAmount + newValue + entityPM.TaxDeductionLocalAmount;
                if (isTaxDeductionLocalAmountChanged)
                {
                    notes = TaxDeductionLocalAmount;
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPAP",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = myEntityName,
                    Notes = notes
                });
            }
            if (entityPM.TaxDeductionPercentage != payment.TaxDeductionPercentage)
            {
                var isTaxDeductionPercentageChanged = entityPM.TaxDeductionPercentage != payment.TaxDeductionPercentage;
                var TaxDeductionPercentage = "Tax Deduction Percentage Updated: " + oldValue + payment.TaxDeductionPercentage + newValue + entityPM.TaxDeductionPercentage;
                if (isTaxDeductionPercentageChanged)
                {
                    notes = TaxDeductionPercentage;
                }

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPAP",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = myEntityName,
                    Notes = notes
                });
            }
            else if (entityPM.DontIncludeInDeductionReport != payment.DontIncludeInDeductionReport)
            {
                notes = TranslateTextsClass.Translate("Accounting.General.O.OldValue", entityPM.Tenant) + payment.DontIncludeInDeductionReport + TranslateTextsClass.Translate("Accounting.General.O.NewValue", entityPM.Tenant) + entityPM.DontIncludeInDeductionReport;

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "DIDR",
                    UserId = loggedContact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = myEntityName,
                    Notes = notes
                });
            }

            TraceExternalPayment(entityPM, payment, loggedContact.Id);
        }




        private static void TraceExternalPayment(APPaymentPM entityPM, APPayment payment, string loggedContactId)
        {
            if (entityPM.ExternalPaymentAmount != null && entityPM.ExternalPaymentDate != null)
            {
                if (entityPM.ExternalPaymentAmount != payment.ExternalPaymentAmount || entityPM.ExternalPaymentDate != payment.ExternalPaymentDate)
                {
                    string notes = "";
                    notes += "Amount: " + String.Format("{0:0,0.00}", entityPM.ExternalPaymentAmount.Value);
                    notes += "\nDate: " + String.Format("{0:dd MMM yyyy}", entityPM.ExternalPaymentDate);
                    notes += "\nNotes : " + entityPM.ExternalPaymentNotes;

                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "PXTR",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "APPayment",
                        Notes = notes,
                    });
                }
            }
        }
    }
}
