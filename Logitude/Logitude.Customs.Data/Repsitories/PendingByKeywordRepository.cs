
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
    using static System.Net.Mime.MediaTypeNames;
using Logitude.Infrastructure.Data.Repsitories;

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
            // split the input "keyword"
            keyWord = keyWord.ToLower();
            keyWord = keyWord.Replace('\n', ' ');
            var splittedkeyWord = keyWord;
            FeatureToggleRepository featureToggleRepository = new FeatureToggleRepository(tenant);
            var RemoveBadChars=featureToggleRepository.HasFeatureToggle("RBC", tenant);
            if (RemoveBadChars)
            {
                char[] BAD_CHARS = new char[] { '!', '@', '#', '$', '%', '_', ')', '(', '-', '*', '&', '^', '~', '.', '"', ';', '\'', '\\', '/', '<', '>', '{', '}', '[', ']', '\n' };
                splittedkeyWord = string.Concat(splittedkeyWord.Split(BAD_CHARS, StringSplitOptions.RemoveEmptyEntries));
            }
            List<string> keyWordSplittedIntoList = splittedkeyWord.Split(' ').ToList();

            List<string> pendingReasonCodeList = new List<string>();
            var PendingByKeywords = (from a in context.PendingByKeywords
                                     where a.Tenant == tenant && a.SearchByFieldCode == SearchByFieldCode
                                     select a).ToList();
            var pendingByKeyword = new List<PendingByKeyword>();

            foreach (string word in keyWordSplittedIntoList)
            {
                pendingByKeyword = new List<PendingByKeyword>();
                PendingByKeywords.ForEach(r =>
                {
                    if (!string.IsNullOrWhiteSpace(r.KeywordsList))
                    {
                        if (r.SearchType != "2")
                        {
                            if (r.KeywordsList.ToLower().Equals(word))
                            {
                                if (r.ExceptKeywords == null || !keyWord.Contains(r.ExceptKeywords.ToLower()))
                                {
                                    pendingByKeyword.Add(r);
                                }
                            }
                        }

                    }
                });

                var courierPendingReasonCodes = pendingByKeyword.Where(r => !String.IsNullOrWhiteSpace(r.CourierPendingReasonCode)).Select(r => r.CourierPendingReasonCode).ToHashSet();
                if (courierPendingReasonCodes.Count > 0)
                {
                    pendingReasonCodeList.AddRange(courierPendingReasonCodes);
                }
            }



            pendingByKeyword = new List<PendingByKeyword>();
            PendingByKeywords.ForEach(r =>
            {
                if (!string.IsNullOrWhiteSpace(r.KeywordsList))
                {
                    if (r.SearchType == "2")
                    {
                        if (splittedkeyWord.Contains(r.KeywordsList.ToLower()))
                        {
                            if (r.ExceptKeywords == null || !keyWord.Contains(r.ExceptKeywords.ToLower()))
                            {
                                pendingByKeyword.Add(r);
                            }
                        }
                    }


                }
            });

            var courierPendingReasonCodes2 = pendingByKeyword.Where(r => !String.IsNullOrWhiteSpace(r.CourierPendingReasonCode)).Select(r => r.CourierPendingReasonCode).ToHashSet();
            if (courierPendingReasonCodes2.Count > 0)
            {
                pendingReasonCodeList.AddRange(courierPendingReasonCodes2);
            }


            return pendingReasonCodeList;


        }

    }
}
