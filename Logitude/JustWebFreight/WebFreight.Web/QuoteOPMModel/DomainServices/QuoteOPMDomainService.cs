using Amital.QuoteOPM.BL.EntityQueryServices;
using Amital.QuoteOPM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Hosting;
using System.Web;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.QuoteOPMModel.DomainServices
{
    [EnableClientAccess()]
    public partial class QuoteOPMDomainService : LogitudeDomainService
    {

        private IQuoteOPMContext quoteOPMContext;

        public QuoteOPCustomerTypeQueryService QuoteOPCustomerTypeQuery { get; private set; }
    }
}