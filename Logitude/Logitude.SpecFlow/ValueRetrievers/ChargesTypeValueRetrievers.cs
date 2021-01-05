using Logitude.SpecFlow.Models;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow.Assist;

namespace Logitude.SpecFlow.ValueRetrievers
{
    public class ChargesTypeValueRetrievers : IValueRetriever
    {
        protected UserData User;
        protected Regex IdRegex;

        public ChargesTypeValueRetrievers(UserData user)
        {
            User = user;
            IdRegex = new Regex(@"^Get id from code \{[^\{\}]*\}$", RegexOptions.IgnoreCase);
        }
        
        public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            if (IdRegex.IsMatch(keyValuePair.Value))
            {
                switch (keyValuePair.Key.ToLower())
                {
                    case "measurementid":
                        return true;
                    case "chargesgroupid":
                        return true;
                }
            }
            return false;
        }
        
        public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            switch (keyValuePair.Key.ToLower())
            {
                case "measurementid":
                    string measurementCode = GetCodeFromIdRegex(keyValuePair.Value);
                    MeasurementRepository measurementRepository = new MeasurementRepository();
                    Measurement measurement = measurementRepository.GetMeasurementbyCode(measurementCode, User.Tenant);
                    return measurement.Id;
                case "chargesgroupid":
                    string chargesGroupCode = GetCodeFromIdRegex(keyValuePair.Value);
                    ChargesGroupRepository chargesGroupRepository = new ChargesGroupRepository();
                    ChargesGroup chargesGroup = chargesGroupRepository.GetSingleChargesGroupByCode(chargesGroupCode, User.Tenant);
                    return chargesGroup.Id;
            }
            return keyValuePair.Value;
        }

        protected string GetCodeFromIdRegex(string valueFromTable)
        {
            return valueFromTable.Split('{')[1].Split('}')[0].Trim();
        }
    }
}