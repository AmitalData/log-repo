using Logitude.Test.Base.Constants;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow.Assist;

namespace Logitude.Test.Base.ValueRetrievers
{
    public class ForeignEntityValueRetriever : IValueRetriever
    {
        protected User User;

        public ForeignEntityValueRetriever(User user)
        {
            User = user;
        }

        public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            if (RegularExpressions.ForeignEntityRegex.IsMatch(keyValuePair.Value))
            {
                return true;
            }
            return false;
        }

        public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            try
            {
                string requiredPropertyName = GetDataInForeignEntityRegex(keyValuePair.Value, 0);
                string filterPropertyName = GetDataInForeignEntityRegex(keyValuePair.Value, 1);
                string filterPropertyValue = GetDataInForeignEntityRegex(keyValuePair.Value, 2);
                string entityControllerName = GetDataInForeignEntityRegex(keyValuePair.Value, 3);

                string requestUrl = entityControllerName + "/GetByFilters?ForceCacheRefresh=false&GetAll=false&GetCount=false&PageIndex=0&PageSize=1" +
                                          "&Filter1Name=" + filterPropertyName + "&Filter1Operator=equals&Filter1Value=" + filterPropertyValue;

                dynamic requestResult = APICaller.CallGet(requestUrl, User.Token, "Result");

                if (requestResult != null)
                {
                    string requiredPropertyValue = Convert.ToString(requestResult[0][requiredPropertyName]);
                    return requiredPropertyValue;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected string GetDataInForeignEntityRegex(string tableValue, int index)
        {
            MatchCollection matchCollection = RegularExpressions.CurlyBracketsRegex.Matches(tableValue ?? "");
            if(matchCollection.Count > 0)
            {
                return matchCollection[index]?.ToString()?.Replace("{", "")?.Replace("}", "")?.Trim();
            }
            return null;
        }
    }
}