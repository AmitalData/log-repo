using Logitude.Customs.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.ClosedTable
{// moran 24.7.14 - Task 6922
    public class CustomsVerificationStatusTypeDetails : CustomsVerificationStatusType, Logitude.Customs.Def.ClosedTable.ICloseTable<CustomsVerificationStatusType, CustomsVerificationStatusTypeDetails>
    {

        public CustomsVerificationStatusTypeDetails()
        {
        }
        public CustomsVerificationStatusTypeDetails(CustomsVerificationStatusType ustomsVerificationStatusType)
        {
            this.Code = ustomsVerificationStatusType.Code;
            this.EnglishName = ustomsVerificationStatusType.EnglishName;
            this.LocalName = ustomsVerificationStatusType.LocalName;

        }



        public List<CustomsVerificationStatusTypeDetails> GetAll()
        {

            var all = new List<CustomsVerificationStatusTypeDetails>();
            all.Add(new CustomsVerificationStatusTypeDetails()
            {
                Code = "4", 
                EnglishName = "Verified",
                LocalName = "אומת",
                

            });
            all.Add(new CustomsVerificationStatusTypeDetails()
            {
                Code = "5", 
                EnglishName = "Verified with Customer",
                LocalName = "אומת בנוכחות לקוח",
                
            });
            all.Add(new CustomsVerificationStatusTypeDetails()
            {
                Code = "6", 
                EnglishName = "Verify Rejected",
                LocalName = "נדחה",
                
            });
            all.Add(new CustomsVerificationStatusTypeDetails()
            {
                Code = "8", 
                EnglishName = "In Verification Process",
                LocalName = "בתהליך אימות",
                
            });

           
            return all;


        }
        public void MapPoco(CustomsVerificationStatusType poco)
        {
            poco.Code = this.Code;
            poco.EnglishName = this.EnglishName;
            poco.LocalName = this.LocalName;
            poco.SearchFields = GetSearchFields(poco);

        }




        public string GetSearchFields(CustomsVerificationStatusType rec)
        {
            return string.Concat(rec.Code + "," + rec.EnglishName + ",", rec.LocalName);
        }
    }
}
