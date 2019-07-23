using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class CardValidating
    {

        public static void Validate(EntityPMs.CardPM entityPM)
        {

        }


        public static void ValidateCode_Unique(Simplog.Data.CommonDataModel.EntityPOCOs.Card entity, ICommonDataContext myContext)
        {
            if (!string.IsNullOrEmpty(entity.Code))
            {
                List<Simplog.Data.CommonDataModel.EntityPOCOs.Card> allMatchedCards
                    = (from a in myContext.Cards
                       where a.Tenant == entity.Tenant
                       && (a.PartnerTypeId == entity.PartnerTypeId)
                       && a.Code == entity.Code
                       && a.Id != entity.Id
                       select a).ToList();

                if (allMatchedCards != null && allMatchedCards.Count>0)
                {

                    throw new ApplicationException("Code Already Exists");

                }
            }
        }


    }
}