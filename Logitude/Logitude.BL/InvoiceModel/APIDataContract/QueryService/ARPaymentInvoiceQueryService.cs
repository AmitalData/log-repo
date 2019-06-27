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
        IInvoiceContext context;
        public ARPaymentInvoiceQueryService(int tenant)
        {
            context = InvoiceContext.GetContext(tenant);

            query = new ARInvoicePaymentQuery(tenant);
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
                    temp.ForeignCurrencyId = item.ForeignCurrencyId;
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

        public List<ARPaymentInvoicePM> ARPaymentInvoiceDataMappingAndValidatin(List<ARPaymentInvoice> ARPaymentInvoices, int Tenant, string ComputingPartnerName = "")
        {
            try
            {

                var ARPaymentInvoicePMs = new List<ARPaymentInvoicePM>();
                foreach (var item in ARPaymentInvoices)
                {
                    var temp = new ARPaymentInvoicePM();
                    if (!string.IsNullOrEmpty(item.ARInvoiceNumber))
                    {
                        temp = query.GetARPaymentInvoicePMsByInvoiceNumber(item.ARInvoiceNumber, Tenant);
                    }

                    if (temp == null)
                    {
                        throw new ApplicationException("ARPaymentInvoice with invoice number " + item.ARInvoiceNumber + " doesn't exist");
                    }

                    temp.ARInvoiceNumber = item.ARInvoiceNumber;
                    temp.LocalAmount = item.LocalAmount;
                    temp.ForeignAmount = item.ForeignAmount;
                    temp.ForeignCurrencyId = item.ForeignCurrencyId;
                    ARPaymentInvoicePMs.Add(temp);

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
