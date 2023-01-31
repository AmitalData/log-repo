
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
using System.Text.RegularExpressions;

namespace Logitude.Customs.Data.Repsitories
{
    public partial class PendingByKeywordRepository : IRepository<PendingByKeyword>
    {

        public List<PendingByKeyword> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }

        public List<string> GetCourierPendingReasonCodeBykeyWords(string keyWord, string SearchByFieldCode, int tenant)
        {
            if (String.IsNullOrWhiteSpace(keyWord))
            {
                return new List<string>();
            }
            keyWord = keyWord.ToLower();
            //keyWordsList = keyWordsList.Replace(" ", ",");
            // char[] BAD_CHARS = new char[] { '!', '@', '#', '$', '%', '_', ')', '(', '-', '*', '&', '^', '~', '.', '"', ';', '\'', '\\', '/', '<', '>', '{', '}', '[', ']', '\n' };
            keyWord = Regex.Replace(keyWord, @"(\-)|(\%)|(\()|(\))|(\.)", "");
            // string.Concat(keyWord.Split(BAD_CHARS, StringSplitOptions.RemoveEmptyEntries));

            List<string> pendingReasonCodeList = new List<string>();

            var PendingByKeywords = (from a in context.PendingByKeywords
                                     where a.Tenant == tenant && a.SearchByFieldCode == SearchByFieldCode
                                     select a).ToList();
            var pendingByKeyword = new List<PendingByKeyword>();

            PendingByKeywords.ForEach(r =>
            {
                if (!string.IsNullOrWhiteSpace(r.KeywordsList))
                {
                    if (r.SearchType == "2")
                    {
                        if (keyWord.Contains(r.KeywordsList.ToLower()))
                        {
                            pendingByKeyword.Add(r);
                        }
                    }
                    else
                    {
                        if (r.KeywordsList.ToLower().Contains(keyWord)) { pendingByKeyword.Add(r); }
                    }
                }
            });
            //if (pendingByKeyword != null && !String.IsNullOrWhiteSpace(pendingByKeyword.CourierPendingReasonCode)) pendingReasonCodeList.Add(pendingByKeyword.CourierPendingReasonCode);
            var courierPendingReasonCodes = pendingByKeyword.Where(r => !String.IsNullOrWhiteSpace(r.CourierPendingReasonCode)).Select(r => r.CourierPendingReasonCode).ToHashSet();
            if (courierPendingReasonCodes.Count > 0)
            {
                pendingReasonCodeList.AddRange(courierPendingReasonCodes);
            }
            return pendingReasonCodeList;


        }

    }
}
   