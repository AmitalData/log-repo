using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityQueries;
using Logitude.BL.QuoteModel.Tools.EntityService;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Service
{
   public class QuoteUpdateService
    {

        public void UpdateQuoteStatusToSend(string quoteId, string tickeNumber, int tenant)
        {
            IQuotesContext context = QuotesContext.GetContext(tenant);
            QuoteService quoteService = new QuoteService(context, tenant);
            QuoteQuery quoteQuery = new QuoteQuery(tenant);
            QuotePM quotePM = quoteQuery.GetSinglePM(quoteId, tenant);
            if (quotePM != null)
            {
                if (IsQuoteStageCreateOrDraft(quotePM))
                {
                    quotePM.ActionType = "SetAsSentToCustomer";
                    quotePM.ExternalEntityNumber = tickeNumber;
                    quoteService.SetChangeSet(new List<QuoteChargePM>(), new List<QuoteFollowUpPM>(), new List<QuotePackagePM>(), new List<QuoteDocumentVersionPM>());
                    quoteService.Update(quotePM);
                }
            }
        }


        private bool IsQuoteStageCreateOrDraft(QuotePM quotePM )
        {
            bool result = false;
            QuoteStageRepository myQuoteStageRepository = new QuoteStageRepository(quotePM.Tenant);
            List<QuoteStage> quoteStages = myQuoteStageRepository.GetQuoteStages(quotePM.Tenant).Where(d => d.Code == "QTDR" || d.Code == "QTCR").ToList();
            QuoteStage myCreateStage = quoteStages.Where(d => d.Code == "QTCR").FirstOrDefault();
            QuoteStage myDraftStage = quoteStages.Where(d => d.Code == "QTDR").FirstOrDefault();
            if (myCreateStage != null && myDraftStage != null)
            {
                if (quotePM.StageId == myDraftStage.Id || quotePM.StageId == myCreateStage.Id) result = true;
            }
            return result;
        }

    }
}
