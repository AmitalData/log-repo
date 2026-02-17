using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{
   public partial class CardQueryService
    {
        public CardPM CardCustomDataMappingAndValidatin(Card MyEntity, int Tenant, string ComputingPartnerName = "")
        {

            try
            {
                var temp = new CardPM();

                if (!string.IsNullOrEmpty(MyEntity.Id))
                {
                    temp = query.GetSinglePM(MyEntity.Id, Tenant);
                }

                else
                {
                    temp = query.GetSinglePMByCode(MyEntity.Code, Tenant);
                }


                if (temp == null)
                {
                    throw new ApplicationException("Card with Id " + MyEntity.Id + " doesn't exist");
                }
                if (string.IsNullOrEmpty(temp.Id))
                {
                    temp.Id = MyEntity.Id;
                }
                if (string.IsNullOrEmpty(temp.Code))
                {
                    temp.Code = MyEntity.Code;
                }
                if (string.IsNullOrEmpty(temp.EnglishName))
                {
                    temp.EnglishName = MyEntity.EnglishName;
                }
                if (string.IsNullOrEmpty(temp.LocalName))
                {
                    temp.LocalName = MyEntity.LocalName;
                }
                return temp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        public Card CardCustomDataMapping(string Id, int Tenant)
        {
            try
            {

                CardQueryService CardService0 = new CardQueryService(Tenant);
                var ChargeType = CardService0.GetCardById(Id, Tenant);
                return ChargeType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
