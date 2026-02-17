using Logitude.BL.QuoteModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.Interfaces
{
    public interface IQuoteTemplateReportHelper
    {
        byte[] BuildQuoteTemplatePdfReport(string quoteId,string quoteTemplateId,string userId,int tenant, List<QuoteTemplateSectionPM> templateSections, int? userTenant = null);
    }
}
