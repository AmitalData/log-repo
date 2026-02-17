using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.CodePropertiesMapping
{
    public class CardCodePropertiesMapping
    {
        public static string GetCardIdFromCardProperties(int ImporterTenant, CodeProperties CardProperties)
        {
            ICommonDataContext commoncontext = CommonDataContext.GetContext(ImporterTenant);
            CardRepository cardsReporistory = new CardRepository(commoncontext);
            if (!string.IsNullOrEmpty(CardProperties.Id))
            {
                Card card = CardRepository.GetSingleCard(CardProperties.Id, ImporterTenant, false);
                if (card != null)
                {
                    return card.Id;
                }
                else
                {
                    return "";
                }
            }
            else if (!string.IsNullOrEmpty(CardProperties.Code))
            {
                Card card = cardsReporistory.GetSingleCardByCode(CardProperties.Code, ImporterTenant, false);
                if (card != null)
                {
                    return card.Id;
                }
                else
                {
                    return "";
                }
            }
            else// if (!string.IsNullOrEmpty(CardProperties.Code))
            {
                throw new NotImplementedException();
            }
        }
    }
}
