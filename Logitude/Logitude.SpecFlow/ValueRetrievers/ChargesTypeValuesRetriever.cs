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
    public class ChargesTypeValuesRetriever : IValueRetriever
    {
        protected readonly UserData User;
        protected Regex GetIdRegex;

        public ChargesTypeValuesRetriever(UserData user)
        {
            User = user;
            GetIdRegex = new Regex(@"^get id from code \{(.*?)\}", RegexOptions.IgnoreCase);
        }

        public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            if (GetIdRegex.IsMatch(keyValuePair.Value))
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
                    string measurementCode = GetCodeFromId(keyValuePair.Value);
                    MeasurementRepository measurementRepository = new MeasurementRepository();
                    Measurement measurement = measurementRepository.GetMeasurementbyCode(measurementCode, User.Tenant);
                    return measurement.Id;
                case "chargesgroupid":
                    string chargesGroupCode = GetCodeFromId(keyValuePair.Value);
                    ChargesGroupRepository chargesGroupRepository = new ChargesGroupRepository();
                    ChargesGroup chargesGroup = chargesGroupRepository.GetSingleChargesGroupByCode(chargesGroupCode, User.Tenant);
                    return chargesGroup.Id;
            }
            return null;
        }

        protected string GetCodeFromId(string valueFromTable)
        {
            return GetIdRegex.Match(valueFromTable).ToString().Split('{')[1].Split('}')[0].Trim();
        }
    }
}