
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.CloseTables;
using System.Text.RegularExpressions;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomsAutonomyKeywordQueryService : EntityQueryService<CustomsAutonomyKeyword, CustomsAutonomyKeywordKeys, CustomsAutonomyKeywordPM, object, CustomsAutonomyKeywordKeys>
    {
        public CustomsAutonomyKeywordPM GetByKeywordtypeCode(string KeywordtypeCode, int tenant)
        {
            var myCustomsAutonomyKeyword = this.repository.GetByKeywordtypeCode(KeywordtypeCode, tenant);
            var pm=this.GetEntityPM(myCustomsAutonomyKeyword, false);
            return pm;
        }


        public bool CheckIfsAutonomy(string city, string phone , string palestinianPrefix,  int tenant)
        {

            CustomsAutonomyKeywordDetails customsAutonomyKeywordDetails = new CustomsAutonomyKeywordDetails();
            var customsAutonomyKeywords = customsAutonomyKeywordDetails.GetAllCustomsAutonomyKeywords();

            if (CheckIfsAutonomyByType(customsAutonomyKeywords[0].Code, city, tenant)) return true;
            if (CheckIfsAutonomyByType(customsAutonomyKeywords[1].Code, phone, tenant)) return true;
            if (CheckIfsAutonomyByType(customsAutonomyKeywords[2].Code, palestinianPrefix, tenant)) return true;

            return false;



        }

        public bool CheckIfsAutonomyByType(string type ,string  valueToSearch , int tenant)
        {
            if (String.IsNullOrWhiteSpace(valueToSearch)) return false;

            valueToSearch = valueToSearch.TrimEnd();
            valueToSearch = valueToSearch.TrimStart();

            var myCustomsAutonomyKeyword = this.repository.GetByKeywordtypeCodeList(type, tenant);
            if (myCustomsAutonomyKeyword == null ) return false;
            //string[] list = myCustomsAutonomyKeyword.KeywordsList.Split(',');

            //for (int i = 0; i < list.Count(); i++)
            //{
            //    list[i]= list[i].TrimEnd();
            //    list[i] = list[i].TrimStart();
            
            //}
             
            if (myCustomsAutonomyKeyword != null && myCustomsAutonomyKeyword.Count() > 0)
            {
                if (myCustomsAutonomyKeyword.FirstOrDefault(x=>x.KeywordsList == valueToSearch)!=null)
                    return true;
            }

            return false;

        }

    }
}
