using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unifreight.Data.AmitalModel;

namespace Logitude.Customs.BL.Utils
{
    public class InsuranceFreightUtil
    {
        public void CalculateFreightForInvoice(SupplierInvoicePM invoice, DateTime? declarationTaxationDate)
        {

            decimal? totalFreightInNIS = 0;
            CustomsExchangeRateQueryService rateQuery = new CustomsExchangeRateQueryService(invoice.Tenant);

            //Calculate freight totals in NIS for invoice
            foreach (SupplierInvoiceFreightAmountPM item in invoice.SupplierInvoiceFreightAmounts)
            {
                if (item.ChangeSetOp != ChangeSetOperation.Delete)
                {
                    decimal? amountInNIS;

                    if (item.CurrencyTypeCode == "ILS")
                    {
                        amountInNIS = item.Amount;
                        totalFreightInNIS += item.Amount;
                    }
                    else
                    {
                        CustomsExchangeRatePM rate;
                        rate = rateQuery.GetCustomsExchangeRateForDateAndCurrencyTypeCode(item.CurrencyTypeCode, declarationTaxationDate, invoice.Tenant);
                        if (rate != null)
                        {
                            amountInNIS = item.Amount * rate.ExchangeRate;
                            totalFreightInNIS += amountInNIS;
                        }
                    }
                }
            }

       
            //Calculate freight totals in invoice currency 
            decimal? totalFreightInInvoice = 0;
            if (invoice.FreightCurrencyTypeCode == "ILS")
            {
                totalFreightInInvoice = totalFreightInNIS;
            }
            else
            {
                CustomsExchangeRatePM invoiceRate = rateQuery.GetCustomsExchangeRateForDateAndCurrencyTypeCode(invoice.FreightCurrencyTypeCode, declarationTaxationDate, invoice.Tenant);
                if (invoiceRate != null)
                {
                    totalFreightInInvoice = totalFreightInNIS / invoiceRate.ExchangeRate;
                }
            }


            //set values of the currency
            if (invoice.TotalFreightInFreightCurrency != totalFreightInInvoice)
                invoice.TotalFreightInFreightCurrency = totalFreightInInvoice;

            if (invoice.TotalFreightInNIS != totalFreightInNIS)
                invoice.TotalFreightInNIS = totalFreightInNIS;
        }

        public void CalculateFreightForInvoice(SupplierInvoice invoice, DateTime? declarationTaxationDate)
        {

            decimal? totalFreightInNIS = 0;
            CustomsExchangeRateQueryService rateQuery = new CustomsExchangeRateQueryService(invoice.Tenant);
            SupplierInvoiceFreightAmountRepository freightAmountRep = new Data.Repsitories.SupplierInvoiceFreightAmountRepository(invoice.Tenant);
            SupplierInvoiceKeys keys = new Data.EntityKeys.SupplierInvoiceKeys() { DeclarationId = invoice.DeclarationId, InvoiceCounterKey = invoice.InvoiceCounterKey };

            List<SupplierInvoiceFreightAmount> freightAmounts = freightAmountRep.GetMulti(keys);
            //Calculate freight totals in NIS for invoice
            foreach (SupplierInvoiceFreightAmount item in freightAmounts)
            {


                decimal? amountInNIS;

                if (item.CurrencyTypeCode == "ILS")
                {
                    amountInNIS = item.Amount;
                    totalFreightInNIS += item.Amount;
                }
                else
                {
                    CustomsExchangeRatePM rate;
                    rate = rateQuery.GetCustomsExchangeRateForDateAndCurrencyTypeCode(item.CurrencyTypeCode, declarationTaxationDate, invoice.Tenant);
                    if (rate != null)
                    {
                        amountInNIS = item.Amount * rate.ExchangeRate;
                        totalFreightInNIS += amountInNIS;
                    }
                }

            }


            //Calculate freight totals in invoice currency 
            decimal? totalFreightInInvoice = 0;
            if (invoice.FreightCurrencyTypeCode == "ILS")
            {
                totalFreightInInvoice = totalFreightInNIS;
            }
            else
            {
                CustomsExchangeRatePM invoiceRate = rateQuery.GetCustomsExchangeRateForDateAndCurrencyTypeCode(invoice.FreightCurrencyTypeCode, declarationTaxationDate, invoice.Tenant);
                if (invoiceRate != null)
                {
                    totalFreightInInvoice = totalFreightInNIS / invoiceRate.ExchangeRate;
                }
            }


            //set values of the currency
            if (invoice.TotalFreightInFreightCurrency != totalFreightInInvoice)
                invoice.TotalFreightInFreightCurrency = totalFreightInInvoice;

            if (invoice.TotalFreightInNIS != totalFreightInNIS)
                invoice.TotalFreightInNIS = totalFreightInNIS;
        }

        public void CalculateInsurance(DeclarationPM declaration)
        {
            CustomsExchangeRateQueryService rateQuery = new CustomsExchangeRateQueryService(declaration.Tenant);
            SupplierInvoiceRepository invoiceRepo = new SupplierInvoiceRepository(declaration.Tenant);

            //[1] calculate FrieghtSumForAllInvoicesInNIS
            decimal FrieghtSumForAllInvoicesInNIS = 0;
            List<SupplierInvoice> invoices = invoiceRepo.GetSupplierInvoicesForDeclaration(declaration.Id, declaration.Tenant);

            if (invoices.Count > 0)
            {
                foreach (SupplierInvoice invoice in invoices)
                {
                    FrieghtSumForAllInvoicesInNIS += invoice.TotalFreightInNIS ?? 0;
                }



                //[2] get first invoice
                decimal FrieghtSumForAllInvoicesInFirstInvoiceCurrency = 0;
                if (declaration.TaxationDateTime != null)
                {
                    SupplierInvoice firstInvoice = invoices.Find(d => d.SequenceNumeric == 1);
                    if (firstInvoice != null)
                    {
                        if (firstInvoice.InvoiceCurrencyTypeCode != null && firstInvoice.InsruancePercentage != null && firstInvoice.InsruancePercentage > 0)
                        {
                            CustomsExchangeRatePM FirstInvoiceRate = rateQuery.GetCustomsExchangeRateForDateAndCurrencyTypeCode(firstInvoice.InvoiceCurrencyTypeCode, declaration.TaxationDateTime, declaration.Tenant);
                            decimal firstInvoiceRateValue = 0;
                            if (FirstInvoiceRate != null)
                            {
                                firstInvoiceRateValue = FirstInvoiceRate.ExchangeRate ?? 0; // No exchange rate in this currency
                            }

                            // calculate Frieght Sum For All Invoices In First Invoice Currency
                            if (firstInvoiceRateValue != 0)
                                FrieghtSumForAllInvoicesInFirstInvoiceCurrency = FrieghtSumForAllInvoicesInNIS / firstInvoiceRateValue;

                            else
                                FrieghtSumForAllInvoicesInFirstInvoiceCurrency = FrieghtSumForAllInvoicesInNIS;


                            //[3] calculate InvoiceAmountsInFirstInvoiceCurrency
                            decimal InvoiceAmountsInFirstInvoiceCurrency = 0;
                            foreach (SupplierInvoice invoice in invoices)
                            {
                                if (invoice.InvoiceCurrencyTypeCode != null)
                                {
                                    if (invoice.InvoiceCurrencyTypeCode != firstInvoice.InvoiceCurrencyTypeCode)
                                    {
                                        CustomsExchangeRatePM invoiceRate = rateQuery.GetCustomsExchangeRateForDateAndCurrencyTypeCode(invoice.InvoiceCurrencyTypeCode, declaration.TaxationDateTime, declaration.Tenant);
                                        //decimal invoiceRateValue = invoiceRate.ExchangeRate ?? 0; // No exchange rate in this currency
                                        decimal invoiceRateValue = invoiceRate != null ? (invoiceRate.ExchangeRate ?? 0) : 0; // No exchange rate in this currency
                                        if (invoiceRateValue > 0)
                                        {
                                            decimal invoiceAmount = invoice.InvoiceAmount ?? 0;
                                            if(firstInvoiceRateValue!=0)
                                            InvoiceAmountsInFirstInvoiceCurrency += invoiceAmount * invoiceRateValue / firstInvoiceRateValue;
                                            else
                                            {
                                                InvoiceAmountsInFirstInvoiceCurrency += invoiceAmount * invoiceRateValue;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        decimal invoiceAmount = invoice.InvoiceAmount ?? 0;
                                        InvoiceAmountsInFirstInvoiceCurrency += invoiceAmount;
                                    }
                                }
                            }



                            //[4] calculate InsuranceAmountInFirstInvoiceCurrency
                            decimal InsuranceAmountInFirstInvoiceCurrency = 0;
                            InsuranceAmountInFirstInvoiceCurrency = (FrieghtSumForAllInvoicesInFirstInvoiceCurrency + InvoiceAmountsInFirstInvoiceCurrency);

                            firstInvoice.InsuranceAmount = InsuranceAmountInFirstInvoiceCurrency * firstInvoice.InsruancePercentage / 100;


                            //[5] update first invoice
                            invoiceRepo.Update(firstInvoice);
                            invoiceRepo.SubmitChanges();
                        }
                    }
                }//2
            }


        }

        internal static decimal? CalcInvoiceAmountInUSD(DateTime? taxationDateTime, string invoiceCurrencyTypeCode, decimal? invoiceAmount, int tenant)
        {
            if (invoiceCurrencyTypeCode == "USD")
            {
                return invoiceAmount;
            }
            if (!taxationDateTime.HasValue)
            {
                return null;//  =>>>null/0 ???
            }
            if (!invoiceAmount.HasValue)
            {
                return null;
            }
            CustomsExchangeRateQueryService rateQuery = new CustomsExchangeRateQueryService(tenant);

            var rateSICurrency = rateQuery.GetCustomsExchangeRateForDateAndCurrencyTypeCode(invoiceCurrencyTypeCode, taxationDateTime, tenant);

            rateSICurrency = rateSICurrency ?? new CustomsExchangeRatePM();
            decimal amountILS = invoiceAmount.GetValueOrDefault() * rateSICurrency.ExchangeRate.GetValueOrDefault();

            var rateUSDCurrency = rateQuery.GetCustomsExchangeRateForDateAndCurrencyTypeCode("USD", taxationDateTime, tenant);
            rateUSDCurrency = rateUSDCurrency ?? new CustomsExchangeRatePM();
            if (rateUSDCurrency.ExchangeRate == 0 || rateUSDCurrency.ExchangeRate == null)
            {
                return null;
            }
            decimal amountUSD = amountILS / rateUSDCurrency.ExchangeRate.GetValueOrDefault();
            return amountUSD;


        }
    }
}
