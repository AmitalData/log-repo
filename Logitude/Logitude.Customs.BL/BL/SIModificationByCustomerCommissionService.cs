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
        private readonly DeclarationPM _declarationPM;
        private readonly SupplierInvoicePM _supplierInvoicePM;

        public SIModificationByCustomerCommissionService(DeclarationPM declaration, SupplierInvoicePM supplierInvoicePM)
        {
            this._declarationPM = declaration;
            this._supplierInvoicePM = supplierInvoicePM;
        }

        internal void EnsureCommission()
        {
            var context = CustomContext.GetContext(_supplierInvoicePM.Tenant);
            string customerId = _declarationPM?.CustomerId;
            if (_declarationPM == null)
            {
                var declarationRepository = new DeclarationRepository(context);
                var dec = declarationRepository.GetSingle(_supplierInvoicePM.DeclarationId, _supplierInvoicePM.Tenant);
                customerId = dec.CustomerId;

            }
            if (string.IsNullOrWhiteSpace(customerId))
            {
                return;
            }

            var query = new VendorCommissionListQueryService(context);
            var commissions = query.GetCommissionsForCustomer(customerId, _supplierInvoicePM.Tenant);



            if (!string.IsNullOrWhiteSpace(this._supplierInvoicePM.VendorId) /*&& this.declarationPM.CustomerId*/ &&
                this._supplierInvoicePM.InvoiceAmount.HasValue &&
                !string.IsNullOrWhiteSpace(this._supplierInvoicePM.InvoiceCurrencyTypeCode))
            {

                var commissionList = commissions.Where(d => d.VendorId == this._supplierInvoicePM.VendorId).ToList();

                foreach (var commission in commissionList)
                {
                    if (commission != null && commission.CommisionPercentage.HasValue)
                    {

                        //init new mod values
                        var newCurrency = this._supplierInvoicePM.InvoiceCurrencyTypeCode;

                        var newAmount = this.precisionRound(
                            number: (commission.CommisionPercentage.Value / 100) * this._supplierInvoicePM.InvoiceAmount.Value,
                            digitsAfterPoint: 2);

                        // if commission found:
                        // 1- update Field VendorCommisionPercentage.SupplierInvoice
                        _supplierInvoicePM.VendorComissionPercentage = commission.CommisionPercentage;
                        Debug.WriteLine("[!] invoice commission changed to:", _supplierInvoicePM.VendorComissionPercentage);

                        // 2- In case there’s mod record , update it
                        var modType = _supplierInvoicePM.SupplierInvoiceModifications
                            .FirstOrDefault(d => d.TypeCode == commission.ModificationsTypeCode);
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
                            //In case there’s no mod record I10, create a record in SupplierInvoiceModification 
                            var newMod = new SupplierInvoiceModificationPM(/*this.EntityPM*/);
                            newMod.Tenant = _supplierInvoicePM.Tenant;
                            newMod.DeclarationId = _supplierInvoicePM.DeclarationId;
                            newMod.InvoiceCounterKey = _supplierInvoicePM.InvoiceCounterKey;
                            newMod.TypeCode = commission.ModificationsTypeCode;
                            newMod.TypeName = commission.ModificationsTypeName;
                            newMod.CurrencyTypeCode = newCurrency;
                            newMod.CurrencyTypeName = _supplierInvoicePM.InvoiceCurrencyTypeName;
                            newMod.Amount = newAmount;
                            newMod.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

                            _supplierInvoicePM.SupplierInvoiceModifications.Add(newMod);


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