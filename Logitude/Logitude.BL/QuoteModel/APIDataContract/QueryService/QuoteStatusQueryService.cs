using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityQueries;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.QuoteModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.APIDataContract.QueryService
{
    public partial class QuoteStatusQueryService
    {
        private IQuotesContext quotesContext;
        private QuoteQuery quoteQuery;

        public QuoteStatusQueryService(int tenant)
        {
            quotesContext = QuotesContext.GetContext(tenant);
            quoteQuery = new QuoteQuery(tenant);
        }

        public QuoteStatus GetQuoteStatusById(string Id, int tenant)
        {
            try
            {
                QuotePM quotePM = quoteQuery.GetSinglePM(Id, tenant);
                QuoteStatus myEntity = this.QuoteStatusDataMapping(quotePM, tenant);

                return myEntity;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public QuoteStatus GetQuoteStatusByNumber(string quoteNumber, int tenant)
        {
            try
            {
                QuotePM quotePM = quoteQuery.GetSinglePMByQuoteNumber(quoteNumber, tenant);
                QuoteStatus myEntity = this.QuoteStatusDataMapping(quotePM, tenant);

                return myEntity;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        private QuoteStatus QuoteStatusDataMapping(QuotePM entityPM, int tenant)
        {
            try
            {
                QuoteStatus myEntity = new QuoteStatus();
                myEntity.QuoteNumber = entityPM.QuoteNumber;
                myEntity.IsQuoteCancel = entityPM.IsCancelled;
                myEntity.Stage.Id = entityPM.StageId;
                myEntity.QuoteDeclineReason.Code = entityPM.QuoteClosingReasonCode;
                myEntity.QuoteAcceptDate = entityPM.AcceptedDate;

                return myEntity;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
