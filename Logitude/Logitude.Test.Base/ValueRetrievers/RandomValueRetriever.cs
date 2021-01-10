using Logitude.Test.Base.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow.Assist;

namespace Logitude.Test.Base.ValueRetrievers
{
    public class RandomValueRetriever : IValueRetriever
    {
        protected string RandomType;

        public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            if (RegularExpressions.RandomStringRegex.IsMatch(keyValuePair.Value))
            {
                RandomType = "String";
                return true;
            }
            if (RegularExpressions.RandomNumberRegex.IsMatch(keyValuePair.Value))
            {
                RandomType = "Number";
                return true;
            }
            return false;
        }

        public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            try
            {
                if (RandomType == "String")
                {
                    int stringLength = GetStringLengthFromRandomStringRegex(keyValuePair.Value);
                    string randomString = GenerateRandomString(stringLength);
                    return randomString;
                }
                else if (RandomType == "Number")
                {
                    int minNumber = GetDataInFromRandomNumberRegex(keyValuePair.Value, "min");
                    int maxNumber = GetDataInFromRandomNumberRegex(keyValuePair.Value, "max");
                    int randomNumber = GenerateRandomNumber(minNumber, maxNumber);
                    return randomNumber;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected int GetStringLengthFromRandomStringRegex(string tableValue)
        {
            try
            {
                int stringLength = Int32.Parse(tableValue?.Split('(')[1]?.Split(')')[0]);
                if(stringLength == 0)
                {
                    return 1;
                }
                return stringLength;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        protected int GetDataInFromRandomNumberRegex(string tableValue, string data)
        {
            try
            {
                int number = Int32.Parse(tableValue?.Split('(')[1]?.Split(')')[0]?.Split(',')[data == "min" ? 0 : 1]);
                return number;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        protected string GenerateRandomString(int stringLength)
        {
            Random random = new Random();
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(characters, stringLength).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        protected int GenerateRandomNumber(int minNumber, int maxNumber)
        {
            Random random = new Random();
            int randomNumber = random.Next(minNumber, maxNumber);
            return randomNumber;
        }
    }
}