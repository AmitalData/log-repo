using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.InvoiceModel.APIDataContract.ApiV1;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.APIDataContract.ApiV1;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InvoiceModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InvoiceModel.APIDataContract.ApiV1
{
     public partial class APInvoiceQueryService
    {


        public APInvoice GetAPInvoiceByInvoiceNumber(string number, int tenant)
        {
            try
            {


                var temp = query.GetSingleInvoiceByInvoiceNumber(number, tenant);
                if (temp == null)
                    throw new ApplicationException("APInvoice with number " + number + " doesn't exist");

                return APInvoiceDataMapping(temp, tenant);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void APInvoiceCustomDataMapping(APInvoice apinvoice, int tenant)
        {
            apinvoice.Tenant = tenant;



            
        }
    }
}
