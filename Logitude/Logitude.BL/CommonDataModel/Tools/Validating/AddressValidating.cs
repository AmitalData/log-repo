using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.QuoteModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace Logitude.BL.CommonDataModel.Tools.Validating
{
    public class AddressValidating
    {
        public static void Validate(AddressPM entityPM)
        {
            ValidateAddressType(entityPM);

            CountryRepository countryRepository = new CountryRepository(entityPM.Tenant);
            Country myCountry = countryRepository.GetSingleCountry(entityPM.CountryId, entityPM.Tenant);

            if (myCountry != null)
            {
                if (myCountry.IsStateRequired)
                {
                    if (string.IsNullOrEmpty(entityPM.StateId))
                    {
                        throw new ApplicationException("State is Required");
                    }
                }

                if (myCountry.HasCitiesList && !entityPM.IsHybrid)
                {
                    if (!string.IsNullOrEmpty(entityPM.City))
                    {
                        entityPM.City = entityPM.City.Trim();

                        CountryCityRepository citiesRepository = new CountryCityRepository(entityPM.Tenant);
                        IQueryable<CountryCity> allCities = citiesRepository.GetCountryCitiesByCountry(myCountry.Id, myCountry.Tenant);

                        bool isCityExists = CheckIsCityExists(entityPM.City, allCities);

                        if (!isCityExists)
                        {
                            throw new ApplicationException("This city doesn't exist in cities table");
                        }
                    }
                }

                ValidatePostalCode(entityPM, myCountry);
                
            }
        }

        private static void ValidatePostalCode(AddressPM addressPM, Country country)
        {
            if (country.Code != "MX" || string.IsNullOrEmpty(addressPM.ZipCode))
            {
                return;
            }
            PostalCodePM postalCodePM = GetPostalCodePM(addressPM);
            if (postalCodePM == null)
            {
                throw new ApplicationException("This ZipCode doesn't exist in " + country.EnglishName + " country");
            }
        }

        private static PostalCodePM GetPostalCodePM(AddressPM addressPM)
        {
            PostalCodeQuery postalCodeQuery = new PostalCodeQuery(addressPM.Tenant);
            PostalCodePM postalCodePM = postalCodeQuery.GetSinglePMByCountryCode(addressPM.ZipCode, addressPM.CountryCode);
            return postalCodePM;
        }

        private static void ValidateAddressType(AddressPM entityPM)
        {
            if (entityPM.CardId != null)
            {
                switch (entityPM.AddressTypeId)
                {
                    case "M":
                    case "B":
                    case "P":
                        {
                            ICommonDataContext context = CommonDataContext.GetContext(entityPM.Tenant);
                            bool isExists = (from d in context.Addresses where d.CardId == entityPM.CardId && d.AddressTypeId == entityPM.AddressTypeId && d.Id != entityPM.Id select d).Any();
                            if (isExists)
                            {
                                string msg = "";

                                switch (entityPM.AddressTypeId)
                                {
                                    case "M":
                                        {
                                            msg = "Partner must have one main address";
                                            break;
                                        }

                                    case "B":
                                        {
                                            msg = "Partner must have one billing address";
                                            break;
                                        }

                                    case "P":
                                        {
                                            msg = "Partner must have one pickup delivery address";
                                            break;
                                        }
                                }

                                throw new ApplicationException(msg);
                            }

                            break;
                        }
                }
            }
        }

        public static void ValidatePickUp(ShipmentPickUpPM entityPM)
        {
            string entityTypeCode = "";
            string fromTypeCode = "";
            string toTypeCode = "";

            if (!string.IsNullOrEmpty(entityPM.PickUpDeliveryTypeCode))
            {
                entityTypeCode = entityPM.PickUpDeliveryTypeCode;
            }

            if (!string.IsNullOrEmpty(entityPM.PickUpDeliveryFromTypeCode))
            {
                fromTypeCode = entityPM.PickUpDeliveryFromTypeCode;
            }

            if (!string.IsNullOrEmpty(entityPM.PickUpDeliveryToTypeCode))
            {
                toTypeCode = entityPM.PickUpDeliveryToTypeCode;
            }

            string entityType = entityTypeCode.ToUpper() == "PICK" ? "Pick up" : "Delivery";

            CountryRepository countryRepository = new CountryRepository(entityPM.Tenant);

            if (fromTypeCode.ToUpper() == "CASL")
            {
                if (!string.IsNullOrEmpty(entityPM.FromAddressCountryId))
                {
                    Country myCountry = countryRepository.GetSingleCountry(entityPM.FromAddressCountryId, entityPM.Tenant);
                    if (myCountry != null)
                    {
                        if (myCountry.HasCitiesList)
                        {
                            if (!string.IsNullOrEmpty(entityPM.FromAddressCity))
                            {
                                entityPM.FromAddressCity = entityPM.FromAddressCity.Trim();

                                CountryCityRepository citiesRepository = new CountryCityRepository(entityPM.Tenant);
                                IQueryable<CountryCity> allCities = citiesRepository.GetCountryCitiesByCountry(myCountry.Id, myCountry.Tenant);

                                bool isCityExists = CheckIsCityExists(entityPM.FromAddressCity, allCities);

                                if (!isCityExists)
                                {
                                    string msg = entityType + " from city doesn't exist in cities table";
                                    throw new ApplicationException(msg);
                                }
                            }
                        }
                    }
                }
            }

            if (toTypeCode.ToUpper() == "CASL")
            {
                if (!string.IsNullOrEmpty(entityPM.ToAddressCountryId))
                {
                    Country myCountry = countryRepository.GetSingleCountry(entityPM.ToAddressCountryId, entityPM.Tenant);
                    if (myCountry != null)
                    {
                        if (myCountry.HasCitiesList)
                        {
                            if (!string.IsNullOrEmpty(entityPM.ToAddressCity))
                            {
                                entityPM.ToAddressCity = entityPM.ToAddressCity.Trim();

                                CountryCityRepository citiesRepository = new CountryCityRepository(entityPM.Tenant);
                                IQueryable<CountryCity> allCities = citiesRepository.GetCountryCitiesByCountry(myCountry.Id, myCountry.Tenant);

                                bool isCityExists = CheckIsCityExists(entityPM.ToAddressCity, allCities);

                                if (!isCityExists)
                                {
                                    string msg = entityType + " to city doesn't exist in cities table";
                                    throw new ApplicationException(msg);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void ValidateDelivery(ShipmentDeliveryPM entityPM)
        {
            string entityTypeCode = "";
            string fromTypeCode = "";
            string toTypeCode = "";

            if (!string.IsNullOrEmpty(entityPM.PickUpDeliveryTypeCode))
            {
                entityTypeCode = entityPM.PickUpDeliveryTypeCode;
            }

            if (!string.IsNullOrEmpty(entityPM.PickUpDeliveryFromTypeCode))
            {
                fromTypeCode = entityPM.PickUpDeliveryFromTypeCode;
            }

            if (!string.IsNullOrEmpty(entityPM.PickUpDeliveryToTypeCode))
            {
                toTypeCode = entityPM.PickUpDeliveryToTypeCode;
            }

            string entityType = entityTypeCode.ToUpper() == "PICK" ? "Pick up" : "Delivery";

            CountryRepository countryRepository = new CountryRepository(entityPM.Tenant);

            if (fromTypeCode.ToUpper() == "CASL")
            {
                if (!string.IsNullOrEmpty(entityPM.FromAddressCountryId))
                {
                    Country myCountry = countryRepository.GetSingleCountry(entityPM.FromAddressCountryId, entityPM.Tenant);
                    if (myCountry != null)
                    {
                        if (myCountry.HasCitiesList)
                        {
                            if (!string.IsNullOrEmpty(entityPM.FromAddressCity))
                            {
                                entityPM.FromAddressCity = entityPM.FromAddressCity.Trim();

                                CountryCityRepository citiesRepository = new CountryCityRepository(entityPM.Tenant);
                                IQueryable<CountryCity> allCities = citiesRepository.GetCountryCitiesByCountry(myCountry.Id, myCountry.Tenant);

                                bool isCityExists = CheckIsCityExists(entityPM.FromAddressCity, allCities);

                                if (!isCityExists)
                                {
                                    string msg = entityType + " from city doesn't exist in cities table";
                                    throw new ApplicationException(msg);
                                }
                            }
                        }
                    }
                }
            }

            if (toTypeCode.ToUpper() == "CASL")
            {
                if (!string.IsNullOrEmpty(entityPM.ToAddressCountryId))
                {
                    Country myCountry = countryRepository.GetSingleCountry(entityPM.ToAddressCountryId, entityPM.Tenant);
                    if (myCountry != null)
                    {
                        if (myCountry.HasCitiesList)
                        {
                            if (!string.IsNullOrEmpty(entityPM.ToAddressCity))
                            {
                                entityPM.ToAddressCity = entityPM.ToAddressCity.Trim();

                                CountryCityRepository citiesRepository = new CountryCityRepository(entityPM.Tenant);
                                IQueryable<CountryCity> allCities = citiesRepository.GetCountryCitiesByCountry(myCountry.Id, myCountry.Tenant);

                                bool isCityExists = CheckIsCityExists(entityPM.ToAddressCity, allCities);

                                if (!isCityExists)
                                {
                                    string msg = entityType + " to city doesn't exist in cities table";
                                    throw new ApplicationException(msg);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static void ValidateQuotePickupDelivery(QuotePM entityPM)
        {
            CountryRepository countryRepository = new CountryRepository(entityPM.Tenant);

            if (entityPM.IncludePickUp)
            {
                if (string.IsNullOrEmpty(entityPM.PickUpAddressId))
                {
                    if (!string.IsNullOrEmpty(entityPM.FromAddressCountryId))
                    {
                        Country myCountry = countryRepository.GetSingleCountry(entityPM.FromAddressCountryId, entityPM.Tenant);
                        if (myCountry != null)
                        {
                            if (myCountry.HasCitiesList)
                            {
                                if (!string.IsNullOrEmpty(entityPM.FromAddressCity))
                                {
                                    entityPM.FromAddressCity = entityPM.FromAddressCity.Trim();

                                    CountryCityRepository citiesRepository = new CountryCityRepository(entityPM.Tenant);
                                    IQueryable<CountryCity> allCities = citiesRepository.GetCountryCitiesByCountry(myCountry.Id, myCountry.Tenant);

                                    bool isCityExists = CheckIsCityExists(entityPM.FromAddressCity, allCities);

                                    if (!isCityExists)
                                    {
                                        string msg = "Pickup city doesn't exist in cities table";
                                        throw new ApplicationException(msg);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (entityPM.IncludeDelivery)
            {
                if (string.IsNullOrEmpty(entityPM.DeliveryAddressId))
                {
                    if (!string.IsNullOrEmpty(entityPM.ToAddressCountryId))
                    {
                        Country myCountry = countryRepository.GetSingleCountry(entityPM.ToAddressCountryId, entityPM.Tenant);
                        if (myCountry != null)
                        {
                            if (myCountry.HasCitiesList)
                            {
                                if (!string.IsNullOrEmpty(entityPM.ToAddressCity))
                                {
                                    entityPM.ToAddressCity = entityPM.ToAddressCity.Trim();

                                    CountryCityRepository citiesRepository = new CountryCityRepository(entityPM.Tenant);
                                    IQueryable<CountryCity> allCities = citiesRepository.GetCountryCitiesByCountry(myCountry.Id, myCountry.Tenant);

                                    bool isCityExists = CheckIsCityExists(entityPM.ToAddressCity, allCities);

                                    if (!isCityExists)
                                    {
                                        string msg = "Delivery city doesn't exist in cities table";
                                        throw new ApplicationException(msg);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static bool CheckIsCityExists(string myCity, IQueryable<CountryCity> allCities)
        {
            bool isCityExists =
                 (from d in allCities
                  where
                  (d.EnglishName != null && d.EnglishName.ToLower() == myCity.ToLower())
                  ||
                  (d.LocalName != null && d.LocalName.ToLower() == myCity.ToLower())
                  select d).Any();

            return isCityExists;
        }
    }
}
