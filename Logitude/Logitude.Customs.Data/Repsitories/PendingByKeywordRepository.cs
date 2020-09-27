 
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
            foreach (string word in keyWordsList2)
            {
                string wordtemp = "," + word + ",";
                var /*PendingByKeyword*/ pendingByKeyword = (from a in context.PendingByKeywords
                                                             where a.Tenant == tenant && a.KeywordsList.ToLower().Contains(wordtemp)
                                                             where a.SearchByFieldCode == SearchByFieldCode
                                                             select a).ToList();//.FirstOrDefault();
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
   