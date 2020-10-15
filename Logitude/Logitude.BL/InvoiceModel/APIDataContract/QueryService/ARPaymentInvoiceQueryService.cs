using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.InvoiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.APIDataContract.ApiV1
{
    public class ARPaymentInvoiceQueryService
    {

        ARInvoicePaymentQuery query;
        ARInvoiceQuery invoiceQuery;
        IInvoiceContext context;
        public ARPaymentInvoiceQueryService(int tenant)
        {
            context = InvoiceContext.GetContext(tenant);

            query = new ARInvoicePaymentQuery(tenant);
            invoiceQuery = new ARInvoiceQuery(tenant);
        }


        public List<ARPaymentInvoice> GetARPaymentInvoiceById(string Id, int Tenant)
        {
            try
            {


                var temp = query.GetARPaymentInvoicePMsForPayment(Id, Tenant);
                if (temp == null)
                    throw new ApplicationException("ARPaymentInvoice with paymentId " + Id + " doesn't exist");

                return ARPaymentInvoiceDataMapping(temp, Tenant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ARPaymentInvoice> ARPaymentInvoiceDataMapping(List<ARPaymentInvoicePM> paymentInvoices, int Tenant, string ComputingPartnerName = "")
        {
            try
            {

                var ARPaymentInvoices = new List<ARPaymentInvoice>();
                foreach (var item in paymentInvoices)
                {

                    var temp = new ARPaymentInvoice();
                   
                    temp.ARInvoiceNumber = item.ARInvoiceNumber;
                    temp.ForeignAmount = item.ForeignAmount;
                    if (item.ForeignCurrencyId != null)
                    {
                        CurrencyQueryService CurrencyService1 = new CurrencyQueryService(Tenant);
                        temp.ForeignCurrency = CurrencyService1.CurrencyCustomDataMapping(item.ForeignCurrencyId, Tenant);

                    }
                    temp.LocalAmount = item.LocalAmount;
                    ARPaymentInvoices.Add(temp);
                }
                return ARPaymentInvoices;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ARPaymentInvoicePM> ARPaymentInvoiceDataMappingAndValidatin(List<ARPaymentInvoice> ARPaymentInvoices, int Tenant, string ComputingPartnerName = "", bool IsUpdate = false)
        {
            try
            {

                var ARPaymentInvoicePMs = new List<ARPaymentInvoicePM>();
                foreach (var item in ARPaymentInvoices)
                {
                    var temp = new ARPaymentInvoicePM();
                    ARInvoicePM invoicePM = new ARInvoicePM();
                    if (!string.IsNullOrEmpty(item.ARInvoiceNumber))
                    {
                        invoicePM = invoiceQuery.GetSingleInvoiceByInvoiceNumber(item.ARInvoiceNumber, Tenant);
                    }

                    if (invoicePM != null)
                    {
                        //  throw new ApplicationException("ARInvoice with invoice number " + item.ARInvoiceNumber + " doesn't exist");

                        temp.ARInvoiceId = invoicePM.Id;

                        temp.ARInvoiceNumber = invoicePM.InvoiceNumber;
                        temp.LocalAmount = item.LocalAmount;
                        temp.ForeignAmount = item.ForeignAmount;
                        CurrencyQueryService ForiegnCurrencyCurrencyService = new CurrencyQueryService(Tenant);
                        if (item.ForeignCurrency != null)
                        {
                            var ForiegnCurrencyPM = ForiegnCurrencyCurrencyService.CurrencyCustomDataMappingAndValidatin(item.ForeignCurrency, Tenant);
                            if (ForiegnCurrencyPM != null)
                            {
                                temp.ForeignCurrencyId = ForiegnCurrencyPM.Id;
                            }
                        }
                        temp.Tenant = Tenant;
                        ARPaymentInvoicePMs.Add(temp);
                    }
                    else
                    {   throw new ApplicationException("ARInvoice with invoice number " + item.ARInvoiceNumber + " doesn't exist");
                    }
                }
                return ARPaymentInvoicePMs;
            }



            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
