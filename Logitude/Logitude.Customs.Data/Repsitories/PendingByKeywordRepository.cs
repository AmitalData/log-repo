 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class PendingByKeywordRepository:IRepository<PendingByKeyword>
   {
        
		public List<PendingByKeyword> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public List<string> GetCourierPendingReasonCodeBykeyWords(string keyWordsList,string SearchByFieldCode, int tenant)
        {
            if (String.IsNullOrWhiteSpace(keyWordsList))
            {
                return new List<string>();
            }
            keyWordsList = keyWordsList.ToLower();
            keyWordsList = keyWordsList.Replace(" ", ",");
            char[] BAD_CHARS = new char[] { '!', '@', '#', '$', '%', '_' , ')' , '(' , '-' , '*', '&', '^', '~', '.', '"', ';', '\'', '\\', '/', '<', '>', '{', '}', '[', ']','\n' };
            keyWordsList = string.Concat(keyWordsList.Split(BAD_CHARS, StringSplitOptions.RemoveEmptyEntries));
            while (keyWordsList.Contains(",,"))
            {
                keyWordsList = keyWordsList.Replace(",,", ",");
            }
            List<string> pendingReasonCodeList = new List<string>();
            List<string> keyWordsList2 = keyWordsList.Split(',').ToList();
            var PendingByKeywords = (from a in context.PendingByKeywords
                                     where a.Tenant == tenant && a.SearchByFieldCode == SearchByFieldCode
                                     select a).ToList();
            foreach (string word in keyWordsList2)
            {
                string wordtemp = "," + word + ",";
                var pendingByKeyword = new List<PendingByKeyword>();
                PendingByKeywords.ForEach(r => {
                    if (r.SearchType == "2")
                    {
                        var tempList = r.KeywordsList.ToLower().Split(',').ToList();
                        if(tempList.FirstOrDefault(x=>word.Contains(x)) != null){
                            pendingByKeyword.Add(r);
                        }
                    }
                    else
                    {
                        if(r.KeywordsList.ToLower().Contains(wordtemp)) { pendingByKeyword.Add(r); }
                    }
                });
                //if (pendingByKeyword != null && !String.IsNullOrWhiteSpace(pendingByKeyword.CourierPendingReasonCode)) pendingReasonCodeList.Add(pendingByKeyword.CourierPendingReasonCode);
                var courierPendingReasonCodes= pendingByKeyword.Where(r => !String.IsNullOrWhiteSpace(r.CourierPendingReasonCode)).Select(r => r.CourierPendingReasonCode).ToHashSet();
                if (courierPendingReasonCodes.Count > 0)
                {
                    pendingReasonCodeList.AddRange(courierPendingReasonCodes);
                }



            }
            return pendingReasonCodeList;
        }



    }

}
   