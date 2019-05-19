 
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

        public string GetCourierPendingReasonCodeBykeyWords(string keyWordsList, int tenant)
        {
            keyWordsList = keyWordsList.ToLower();
            keyWordsList.Replace(" ", ",");
            char[] BAD_CHARS = new char[] { '!', '@', '#', '$', '%', '_' , ')' , '(' , '-' , '*', '&', '^', '~', '.', '"', ';', '\'', '\\', '/', '<', '>', '{', '}', '[', ']' };
            keyWordsList = string.Concat(keyWordsList.Split(BAD_CHARS, StringSplitOptions.RemoveEmptyEntries));
            while (keyWordsList.Contains(",,"))
            {
                keyWordsList.Replace(",,", ",");
            }
           
            List<string> keyWordsList2 = keyWordsList.Split(',').ToList();
            foreach (string word in keyWordsList2)
            {
                string wordtemp = "," + word + ",";
                PendingByKeyword pendingByKeyword = (from a in context.PendingByKeywords
                                                     where a.Tenant == tenant && a.KeywordsList.Contains(wordtemp)
                                                     select a).FirstOrDefault();
                if (pendingByKeyword != null && !String.IsNullOrWhiteSpace(pendingByKeyword.CourierPendingReasonCode)) return pendingByKeyword.CourierPendingReasonCode;
            }
            return null;
        }

    }

}
   