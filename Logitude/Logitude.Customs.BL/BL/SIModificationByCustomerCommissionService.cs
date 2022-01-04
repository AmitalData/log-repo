using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.BL
{
    public class SIModificationByCustomerCommissionService
    {
        //private readonly DeclarationPM declarationPM;
        //private readonly SupplierInvoicePM supplierInvoicePM;

        //public SIModificationByCustomerCommissionService()
        //{
        //    declarationPM = declaration;
        //    supplierInvoicePM = supplierInvoicePM;
        //}

        public void EnsureReductionByVendorCommission(DeclarationPM declarationPM, SupplierInvoicePM supplierInvoicePM,bool throwExceptionCheckb4SendDec)
        {
            var context = CustomContext.GetContext(supplierInvoicePM.Tenant);
            string customerId = declarationPM?.CustomerId;
            if (declarationPM == null)
            {
                var declarationRepository = new DeclarationRepository(context);
                var dec = declarationRepository.GetSingle(supplierInvoicePM.DeclarationId, supplierInvoicePM.Tenant);
                customerId = dec.CustomerId;

            }
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return;
            }

            var query = new VendorCommissionListQueryService(context);
            var commissions = query.GetCommissionsForCustomer(customerId, supplierInvoicePM.Tenant);



            if (!string.IsNullOrWhiteSpace(supplierInvoicePM.VendorId) /*&& declarationPM.CustomerId*/ &&
                supplierInvoicePM.InvoiceAmount.HasValue &&
                !string.IsNullOrWhiteSpace(supplierInvoicePM.InvoiceCurrencyTypeCode))
            {

                var commissionList = commissions.Where(d => d.VendorId == supplierInvoicePM.VendorId).ToList();

                foreach (var commission in commissionList)
                {
                    if (commission != null && commission.CommisionPercentage.HasValue)
                    {

                        //init new mod values
                        var newCurrency = supplierInvoicePM.InvoiceCurrencyTypeCode;

                        var newAmount = precisionRound(
                            number: (commission.CommisionPercentage.Value / 100) * supplierInvoicePM.InvoiceAmount.Value,
                            digitsAfterPoint: 2);

                        // if commission found:
                        // 1- update Field VendorCommisionPercentage.SupplierInvoice
                        supplierInvoicePM.VendorComissionPercentage = commission.CommisionPercentage;
                        Debug.WriteLine("[!] invoice commission changed to:", supplierInvoicePM.VendorComissionPercentage);

                        // 2- In case there’s mod record , update it
                        var modType = supplierInvoicePM.SupplierInvoiceModifications
                            .Where(r=>r.ChangeSetOp!= Simplog.Server.Infrastructure.ChangeSetOperation.Delete)//ensure !!! itzik on delete - create !!!!!
                            .FirstOrDefault(d => d.TypeCode == commission.ModificationsTypeCode)
                            ;
                        if (modType != null)
                        {
                            // 3- In case there’s record with same type 
                            //    and it's with different currency OR value ask user
                            if (modType.Amount != newAmount || modType.CurrencyTypeCode != newCurrency)
                            {
                                Debug.WriteLine("somthing changed, amount or currency ,ask user to change it ???");
                                Debug.WriteLine("No !?!?");
                            }
                            else
                            {
                                // same currency and amount
                                if (modType.Amount == newAmount && modType.CurrencyTypeCode == newCurrency)
                                {
                                    // no changes
                                }
                            }

                        }
                        else
                        {
                            if (throwExceptionCheckb4SendDec)
                            {
                                throw new Exception("הפחתות התאמות לפי אחוז עמלה לספק -חשבון ספק לא מעודכן");
                            }
                            //In case there’s no mod record I10, create a record in SupplierInvoiceModification 
                            var newMod = new SupplierInvoiceModificationPM(/*EntityPM*/);
                            newMod.Tenant = supplierInvoicePM.Tenant;
                            newMod.DeclarationId = supplierInvoicePM.DeclarationId;
                            newMod.InvoiceCounterKey = supplierInvoicePM.InvoiceCounterKey;
                            newMod.TypeCode = commission.ModificationsTypeCode;
                            newMod.TypeName = commission.ModificationsTypeName;
                            newMod.CurrencyTypeCode = newCurrency;
                            newMod.CurrencyTypeName = supplierInvoicePM.InvoiceCurrencyTypeName;
                            newMod.Amount = newAmount;
                            newMod.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

                            supplierInvoicePM.SupplierInvoiceModifications.Add(newMod);


                        }


                    }
                }
            }
        }

        private decimal precisionRound(decimal number, int digitsAfterPoint)
        {

            var factor = (decimal)Math.Pow(10, digitsAfterPoint);
            return Math.Round(number * factor) / factor;
        }


    }
}