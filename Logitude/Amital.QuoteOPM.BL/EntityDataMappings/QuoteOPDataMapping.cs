
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Def.EntityPMs; 
using Amital.QuoteOPM.Data;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Amital.QuoteOPM.Data.Repsitories;
using Amital.QuoteOPM.BL.EntityQueryServices;
using Logitude.BL.CommonDataModel.EntityLists;

namespace Amital.QuoteOPM.BL.EntityDataMappings
{
   
   public partial class QuoteOPDataMapping: IMapping<QuoteOPPM, QuoteOP>
   {
        

        public void CustomPMToPOCO(QuoteOPPM entityPM, QuoteOP entityPOCO)
        {
            //throw new NotImplementedException();\
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.QuoteNumber);
            
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.CreatedByUserId);
            
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.OpenDate);
            
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.DirectionId);
            
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ProductCode);

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ConcurrencyGUID);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.QuoteNumber = entityPM.QuoteNumber;
                entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
                entityPOCO.OpenDate = entityPM.OpenDate;
                entityPOCO.DirectionId = entityPM.DirectionId;
                entityPOCO.ProductCode = entityPM.ProductCode;
                entityPOCO.Id = entityPM.Id;
            }

            entityPOCO.GrossWeightInKG = entityPM.GrossWeightInKG = GetWeightInKG(entityPM.GrossWeightUnitCode, entityPM.GrossWeight);
            entityPOCO.GrossWeightPerTon = entityPM.GrossWeightPerTon = GetWeightInTon(entityPM.GrossWeightInKG);
            entityPOCO.ChargeableWeightInKG = entityPM.ChargeableWeightInKG = GetChargeableWeightInKG(entityPM.ChargeableWeightUnitCode, entityPM.ChargeableWeight);

            entityPM.ConcurrencyGUID = entityPOCO.ConcurrencyGUID = Guid.NewGuid().ToString();




            BuildSearchField(entityPM, entityPOCO);
        }
        public static double? GetWeightInKG(string weightCode, double? weight)
        {
            double? myResult = null;

            if (weight != null)
            {
                double? factorOfConvert = 1;

                if (!string.IsNullOrEmpty(weightCode))
                {
                    switch (weightCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }
                        case "MT": { factorOfConvert = 1000; break; }
                    }
                }

                myResult = weight * factorOfConvert;
            }

            if (myResult != null)
            {
                myResult = MethodHelper.Round(myResult.Value, 3);
            }

            return myResult;
        }
        public static double? GetWeightInTon(double? weightInKG)
        {
            double? myResult = null;

            if (weightInKG != null)
            {
                myResult = weightInKG / 1000;
            }

            if (myResult != null)
            {
                myResult = MethodHelper.Round(myResult.Value, 3);
            }

            return myResult;
        }

        public static double? GetChargeableWeightInKG(string weightCode, double? weight)
        {
            double? myResult = null;

            if (weight != null)
            {
                double? factorOfConvert = 1;

                if (!string.IsNullOrEmpty(weightCode))
                {
                    switch (weightCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }
                        case "MT": { factorOfConvert = 1000; break; }
                    }
                }

                myResult = weight * factorOfConvert;
            }

            if (myResult != null)
            {
                myResult = MethodHelper.Round(myResult.Value, 3);
            }

            return myResult;
        }
        public static double? GetVolumeInCBM(string volumeCode, double? volume)
        {
            double? myResult = null;

            if (volume != null)
            {
                double? factorOfConvert = 1;

                if (!string.IsNullOrEmpty(volumeCode))
                {
                    switch (volumeCode.ToUpper())
                    {
                        case "CBM": { factorOfConvert = 1; break; }
                        case "CBI": { factorOfConvert = 61024; break; }      // 1m³ = 61024in³
                        case "CBF": { factorOfConvert = 35.315; break; }     // 1m³ = 35.315ft³
                    }
                }

                myResult = volume / factorOfConvert;
            }

            if (myResult != null)
            {
                myResult = MethodHelper.Round(myResult.Value, 3);
            }

            return myResult;
        }
        private static void BuildSearchField(QuoteOPPM entityPM, QuoteOP entityPoco)
        {
            string mySearchFields = "";

            int tenant = entityPM.Tenant;

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.QuoteNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Subject);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Notes);

            #region Ports
            Logitude.BL.Helpers.QueryHelper.AddPortToSearchFields(ref mySearchFields, tenant, entityPM.FromPortId);
            QueryHelper.AddPortToSearchFields(ref mySearchFields, tenant, entityPM.ToPortId);
            #endregion

            #region Partners

            if (!string.IsNullOrEmpty(entityPM.ShipperId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.ShipperId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ShipperReference1);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ShipperReference2);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.ConsigneeId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.ConsigneeId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ConsigneeReference1);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ConsigneeReference2);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.CustomerId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.CustomerId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CustomerReference1);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CustomerReference2);
                }

                if (!string.IsNullOrEmpty(entityPM.CustomerContactId))
                {
                    Contact myContact = ContactRepository.GetSingleContact(entityPM.CustomerContactId, tenant, true);
                    if (myContact != null)
                    {
                        MethodHelper.AddToSearchFields(ref mySearchFields, myContact.Email);
                    }
                }
            }

            if (!string.IsNullOrEmpty(entityPM.NotifyId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.NotifyId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                }
            }
            #endregion

            #region Carrier
            if (!string.IsNullOrEmpty(entityPM.MainCarriageCarrierId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.MainCarriageCarrierId, entityPM.Tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.Code);
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                }
            }
            #endregion

            if (mySearchFields.Length > 1500)
            {
                mySearchFields = mySearchFields.Substring(0, 1500);
            }

            entityPM.SearchFields = mySearchFields;
            entityPoco.SearchFields = mySearchFields;
        }
        public void CustomPOCOToPM(QuoteOPPM entityPM, QuoteOP entityPOCO)
        {
            //throw new NotImplementedException();
            string myIncotermCode = null;
            string myIncotermName = null;


            if (!string.IsNullOrEmpty(entityPOCO.IncotermId))
            {
                IncotermRepository incotermRepository = new IncotermRepository(entityPOCO.Tenant);
                Incoterm incoterm = incotermRepository.GetSingleIncoterm(entityPOCO.IncotermId, entityPOCO.Tenant);//no cache !!! ??
                if (incoterm != null)
                {
                    myIncotermCode = incoterm.Code;
                    myIncotermName = incoterm.Name;
                }
            }
            this.CustomMappedPMProperties.Add(PMPropertyNames.IncotermCode);
            this.CustomMappedPMProperties.Add(PMPropertyNames.IncotermName);
            entityPM.IncotermCode = myIncotermCode;
            entityPM.IncotermName = myIncotermName;



            int tenant = entityPOCO.Tenant;
            string entityId = entityPOCO.Id;

            ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);
            IQuoteOPMContext myQuotesContext = QuoteOPMContext.GetContext(tenant);

            PortRepository portRepository = new PortRepository(myCommonContext);
            CardRepository cardsRepository = new CardRepository(myCommonContext);
            CountryRepository countryRepository = new CountryRepository(myCommonContext);
            AddressRepository addressRepository = new AddressRepository(myCommonContext);

            QuoteOPChargeRepository quoteChargeRepository = new QuoteOPChargeRepository(myQuotesContext);
            QuoteOPPackageRepository quotePackageRepository = new  QuoteOPPackageRepository (myQuotesContext);
            QuoteOPTotalVATRepository myTotalVATRepository = new  QuoteOPTotalVATRepository (myQuotesContext);
            QuoteOPChargeQueryService quoteChargeQuery = new  QuoteOPChargeQueryService(quoteChargeRepository);
            QuoteOPPackageQueryService quotePackageQuery = new  QuoteOPPackageQueryService(quotePackageRepository);
            QuoteOPTotalVATQueryService myTotalVATQuery = new  QuoteOPTotalVATQueryService(myTotalVATRepository);
           

            bool isInlandDomestic = (entityPOCO.DirectionId == "D" && entityPOCO.TransportModeId == "I");


            #region From | To Location
            if (isInlandDomestic)
            {
                if (!string.IsNullOrEmpty(entityPOCO.FromPartnerAddressId))
                {
                    Address fromAddress = addressRepository.GetSingleAddress(entityPOCO.FromPartnerAddressId, tenant);
                    if (fromAddress != null)
                    {
                        entityPM.FromCountryId = fromAddress.CountryId;

                        string fromLocation = "";

                        if (!string.IsNullOrEmpty(fromAddress.City))
                        {
                            fromLocation = fromAddress.City;
                        }

                        if (fromAddress.Country != null)
                        {
                            entityPM.FromCountryIsEC = fromAddress.Country.EC;

                            if (string.IsNullOrEmpty(fromLocation))
                            {
                                fromLocation = fromAddress.Country.Code;
                            }

                            else
                            {
                                fromLocation += " " + fromAddress.Country.Code;
                            }
                        }

                        entityPM.FromLocation = fromLocation;
                    }
                }

                if (!string.IsNullOrEmpty(entityPOCO.ToPartnerAddressId))
                {
                    Address toAddress = addressRepository.GetSingleAddress(entityPOCO.ToPartnerAddressId, tenant);
                    if (toAddress != null)
                    {
                        entityPM.ToCountryId = toAddress.CountryId;

                        string toLocation = "";

                        if (!string.IsNullOrEmpty(toAddress.City))
                        {
                            toLocation = toAddress.City;
                        }

                        if (toAddress.Country != null)
                        {
                            entityPM.ToCountryIsEC = toAddress.Country.EC;

                            if (string.IsNullOrEmpty(toLocation))
                            {
                                toLocation = toAddress.Country.Code;
                            }

                            else
                            {
                                toLocation += " " + toAddress.Country.Code;
                            }
                        }

                        entityPM.ToLocation = toLocation;
                    }
                }
            }

            else
            {
                PortQuery portQuery = new PortQuery(portRepository);

                PortPM fromPort = portQuery.GetSinglePM(entityPOCO.FromPortId, tenant);
                if (fromPort != null)
                {
                    entityPM.FromCountryId = fromPort.CountryId;
                    entityPM.FromLocation = fromPort.Code + " " + fromPort.EnglishName;

                    entityPM.FromCountryIsEC = fromPort.CountryEC;
                }

                PortPM toPort = portQuery.GetSinglePM(entityPOCO.ToPortId, tenant);
                if (toPort != null)
                {
                    entityPM.ToCountryId = toPort.CountryId;
                    entityPM.ToLocation = toPort.Code + " " + toPort.EnglishName;

                    entityPM.ToCountryIsEC = toPort.CountryEC;
                }
            }
            #endregion



            #region Routings

          /*  if (entityPOCO.FromPort != null)
            {
                entityPM.FromPort = entityPOCO.FromPort.Code;
                entityPM.FromPortName = entityPOCO.FromPort.EnglishName;

                if (entityPOCO.FromPort.Country != null)
                {
                    entityPM.FromPortCountry = entityPOCO.FromPort.Country.EnglishName;
                }
            }

            if (entityPOCO.ToPort != null)
            {
                entityPM.ToPort = entityPOCO.ToPort.Code;
                entityPM.ToPortName = entityPOCO.ToPort.EnglishName;

                if (entityPOCO.ToPort.Country != null)
                {
                    entityPM.ToPortCountry = entityPOCO.ToPort.Country.EnglishName;
                }
            }*/

            if (isInlandDomestic)
            {
                if (!string.IsNullOrEmpty(entityPOCO.FromPartnerAddressId))
                {
                    Address address = addressRepository.GetSingleAddress(entityPOCO.FromPartnerAddressId, tenant);
                    if (address != null)
                    {
                        if (!string.IsNullOrEmpty(address.CountryId))
                        {
                            Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                            if (country != null)
                            {
                                entityPM.FromCountryCode = country.Code;
                                entityPM.FromCountryName = country.EnglishName;
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(entityPOCO.ToPartnerAddressId))
                {
                    Address address = addressRepository.GetSingleAddress(entityPOCO.ToPartnerAddressId, tenant);
                    if (address != null)
                    {
                        if (!string.IsNullOrEmpty(address.CountryId))
                        {
                            Country country = countryRepository.GetSingleCountry(address.CountryId, tenant);
                            if (country != null)
                            {
                                entityPM.ToCountryCode = country.Code;
                                entityPM.ToCountryName = country.EnglishName;
                            }
                        }
                    }
                }
            }

            else
            {
                if (!string.IsNullOrEmpty(entityPOCO.FromPortId))
                {
                    Port port = portRepository.GetSinglePort(tenant, entityPOCO.FromPortId);
                    if (port != null)
                    {
                        if (!string.IsNullOrEmpty(port.CountryId))
                        {
                            Country country = countryRepository.GetSingleCountry(port.CountryId, tenant);
                            if (country != null)
                            {
                                entityPM.FromCountryCode = country.Code;
                                entityPM.FromCountryName = country.EnglishName;
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(entityPOCO.ToPortId))
                {
                    Port port = portRepository.GetSinglePort(tenant, entityPOCO.ToPortId);
                    if (port != null)
                    {
                        if (!string.IsNullOrEmpty(port.CountryId))
                        {
                            Country country = countryRepository.GetSingleCountry(port.CountryId, tenant);
                            if (country != null)
                            {
                                entityPM.ToCountryCode = country.Code;
                                entityPM.ToCountryName = country.EnglishName;
                            }
                        }
                    }
                }
            }
            #endregion

            #region Partners

            if (!string.IsNullOrEmpty(entityPOCO.CustomerId))
            {
                CustomerQuery customerQuery = new CustomerQuery(tenant);
                CustomerList list = customerQuery.GetSingleCustomerList(entityPOCO.CustomerId, tenant);

                if (list != null)
                {
                    entityPM.CustomerNote = list.Notes;
                    entityPM.CustomerRankName = list.RankName;
                }
            }

            #region Shipper


            if (!string.IsNullOrEmpty(entityPOCO.ShipperId))
            {
                Card loadedCard = CardRepository.GetSingleCard(entityPOCO.ShipperId, tenant, true);
                entityPM.ShipperNote = loadedCard.Notes;

                if (loadedCard.PartnerTypeId == "PO")
                {
                    entityPM.IsPotentialShipper = true;
                }

                Address addressMain = addressRepository.GetSingleAddressByCardIdAndTypeId(entityPOCO.ShipperId, "M", tenant);
                Address addressPick = addressRepository.GetSingleAddressByCardIdAndTypeId(entityPOCO.ShipperId, "P", tenant);

                if (addressMain != null)
                {
                    entityPM.ShipperMainAddressId = addressMain.Id;
                }

                if (addressPick != null)
                {
                    entityPM.ShipperPickAddressId = addressPick.Id;
                }
            }
            #endregion

            #region Consignee


            if (entityPOCO.ConsigneeId != null)
            {
                Card loadedCard = CardRepository.GetSingleCard(entityPOCO.ConsigneeId, tenant, false);
                entityPM.ConsigneeNote = loadedCard.Notes;

                if (loadedCard.PartnerTypeId == "PO")
                {
                    entityPM.IsPotentialConsignee = true;
                }

                Address addressMain = addressRepository.GetSingleAddressByCardIdAndTypeId(entityPOCO.ConsigneeId, "M", tenant);
                Address addressPick = addressRepository.GetSingleAddressByCardIdAndTypeId(entityPOCO.ConsigneeId, "P", tenant);

                if (addressMain != null)
                {
                    entityPM.ConsigneeMainAddressId = addressMain.Id;
                }

                if (addressPick != null)
                {
                    entityPM.ConsigneePickAddressId = addressPick.Id;
                }
            }
            #endregion

            #region Freelancer

            if (!string.IsNullOrEmpty(entityPOCO.FreelancerId))
            {
                Card loadedCard = CardRepository.GetSingleCard(entityPOCO.FreelancerId, tenant, true);
                entityPM.FreelancerName = loadedCard.EnglishName;

                Address adr = addressRepository.GetMainAddressByCardId(entityPOCO.FreelancerId, entityPOCO.Tenant);
                if (adr != null)
                {
                    entityPM.FreelancerAddressId = adr.Id;
                }
            }
            #endregion

            #region Notify
            entityPM.NotifyId = entityPOCO.NotifyId;
            entityPM.NotifyAddressId = entityPOCO.NotifyAddressId;
            entityPM.NotifyContactId = entityPOCO.NotifyContactId;
            if (!string.IsNullOrEmpty(entityPOCO.NotifyId))
            {
                Card loadedCard = CardRepository.GetSingleCard(entityPOCO.NotifyId, entityPOCO.Tenant, true);
                entityPM.NotifyName = loadedCard.EnglishName;
                entityPM.NotifyNote = loadedCard.Notes;

                if (!string.IsNullOrEmpty(entityPOCO.NotifyAddressId))
                {
                    Address notify1Address = addressRepository.GetSingleAddress(entityPM.NotifyAddressId, tenant);
                    if (notify1Address != null)
                    {
                        entityPM.NotifyAddress1 = notify1Address.Address1;
                        entityPM.NotifyAddress2 = notify1Address.Address2;
                        entityPM.NotifyCity = notify1Address.City;
                        entityPM.NotifyCountryId = notify1Address.CountryId;
                        entityPM.NotifyStateId = notify1Address.StateId;
                        entityPM.NotifyZipCode = notify1Address.ZipCode;
                    }
                }
            }
            #endregion

            #endregion

            #region Pickup Delivery
            entityPM.IncludePickUp = entityPOCO.IncludePickUp;
            entityPM.IncludeDelivery = entityPOCO.IncludeDelivery;
            entityPM.PickUpAddressId = entityPOCO.FromAddressId;
            entityPM.DeliveryAddressId = entityPOCO.ToAddressId;
            entityPM.FromAddressCity = entityPOCO.FromAddressCity;
            entityPM.FromAddressZipCode = entityPOCO.FromAddressZipCode;
            entityPM.FromAddressCountryId = entityPOCO.FromAddressCountryId;
            entityPM.ToAddressCity = entityPOCO.ToAddressCity;
            entityPM.ToAddressZipCode = entityPOCO.ToAddressZipCode;
            entityPM.ToAddressCountryId = entityPOCO.ToAddressCountryId;
            entityPM.PickupCity = entityPOCO.FromAddressCity;
            entityPM.PickupCountryId = entityPOCO.FromAddressCountryId;
            entityPM.PickupZipCode = entityPOCO.FromAddressZipCode;
            entityPM.DeliveryCity = entityPOCO.ToAddressCity;
            entityPM.DeliveryCountryId = entityPOCO.ToAddressCountryId;
            entityPM.DeliveryZipCode = entityPOCO.ToAddressZipCode;

            if (entityPM.IncludePickUp)
            {
                if (!string.IsNullOrEmpty(entityPM.PickUpAddressId))
                {
                    Address address = addressRepository.GetSingleAddress(entityPM.PickUpAddressId, tenant);
                    if (address != null)
                    {
                        entityPM.PickUpAddress = this.GetAddress(address)
                            + (!string.IsNullOrEmpty(address.ATTN) ? (Environment.NewLine + "Contact : " + address.ATTN) : "")
                            + (!string.IsNullOrEmpty(address.PhoneNumber) ? (Environment.NewLine + "Phone : " + address.PhoneNumber) : "");
                        entityPM.PickupCity = address.City;
                        entityPM.PickupCountryId = address.Country?.Id;
                        entityPM.PickupZipCode = address.ZipCode;
                    }
                }
            }

            if (entityPM.IncludeDelivery)
            {
                if (!string.IsNullOrEmpty(entityPM.DeliveryAddressId))
                {
                    Address address = addressRepository.GetSingleAddress(entityPM.DeliveryAddressId, tenant);
                    if (address != null)
                    {
                        entityPM.DeliveryAddress = this.GetAddress(address)
                            + (!string.IsNullOrEmpty(address.ATTN) ? (Environment.NewLine + "Contact : " + address.ATTN) : "")
                            + (!string.IsNullOrEmpty(address.PhoneNumber) ? (Environment.NewLine + "Phone : " + address.PhoneNumber) : "");
                        entityPM.DeliveryCity = address.City;
                        entityPM.DeliveryCountryId = address.Country?.Id;
                        entityPM.DeliveryZipCode = address.ZipCode;
                    }
                }
            }


            if (entityPOCO.IncludePickUp)
            {
                #region Pickup Location
                if (!string.IsNullOrEmpty(entityPOCO.FromAddressId))
                {
                    Address address = addressRepository.GetSingleAddress(entityPOCO.FromAddressId, tenant);
                    if (address != null)
                    {
                        string location = "";

                        if (!string.IsNullOrEmpty(address.City))
                        {
                            location = address.City;
                        }

                        if (address.Country != null)
                        {
                            if (string.IsNullOrEmpty(location))
                            {
                                location = address.Country.Code;
                            }

                            else
                            {
                                location += " " + address.Country.Code;
                            }
                        }

                        if (!string.IsNullOrEmpty(address.ZipCode))
                        {
                            if (string.IsNullOrEmpty(location))
                            {
                                location = address.ZipCode;
                            }

                            else
                            {
                                location += " - " + address.ZipCode;
                            }
                        }

                        entityPM.PickupLocation = location;
                    }
                }

                else
                {
                    string location = "";

                    if (!string.IsNullOrEmpty(entityPOCO.FromAddressCity))
                    {
                        location = entityPOCO.FromAddressCity;
                    }

                    if (!string.IsNullOrEmpty(entityPOCO.FromAddressCountryId))
                    {
                        Country country = countryRepository.GetSingleCountry(entityPOCO.FromAddressCountryId, tenant);
                        if (country != null)
                        {
                            if (string.IsNullOrEmpty(location))
                            {
                                location = country.Code;
                            }

                            else
                            {
                                location += " " + country.Code;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(entityPOCO.FromAddressZipCode))
                    {
                        if (string.IsNullOrEmpty(location))
                        {
                            location = entityPOCO.FromAddressZipCode;
                        }

                        else
                        {
                            location += " - " + entityPOCO.FromAddressZipCode;
                        }
                    }

                    entityPM.PickupLocation = location;
                }
                #endregion
            }

            if (entityPOCO.IncludeDelivery)
            {
                #region Delivery Location
                if (!string.IsNullOrEmpty(entityPOCO.ToAddressId))
                {
                    Address address = addressRepository.GetSingleAddress(entityPOCO.ToAddressId, tenant);
                    if (address != null)
                    {
                        string location = "";

                        if (!string.IsNullOrEmpty(address.City))
                        {
                            location = address.City;
                        }

                        if (address.Country != null)
                        {
                            if (string.IsNullOrEmpty(location))
                            {
                                location = address.Country.Code;
                            }

                            else
                            {
                                location += " " + address.Country.Code;
                            }
                        }


                        if (!string.IsNullOrEmpty(address.ZipCode))
                        {
                            if (string.IsNullOrEmpty(location))
                            {
                                location = address.ZipCode;
                            }

                            else
                            {
                                location += " - " + address.ZipCode;
                            }
                            if (string.IsNullOrEmpty(entityPM.ToAddressZipCode))
                            {
                                entityPM.ToAddressZipCode = address.ZipCode;
                            }
                        }

                        entityPM.DeliveryLocation = location;
                    }
                }

                else
                {
                    string location = "";

                    if (!string.IsNullOrEmpty(entityPOCO.ToAddressCity))
                    {
                        location = entityPOCO.ToAddressCity;
                    }

                    if (!string.IsNullOrEmpty(entityPOCO.ToAddressCountryId))
                    {
                        Country country = countryRepository.GetSingleCountry(entityPOCO.ToAddressCountryId, tenant);
                        if (country != null)
                        {
                            if (string.IsNullOrEmpty(location))
                            {
                                location = country.Code;
                            }

                            else
                            {
                                location += " " + country.Code;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(entityPOCO.ToAddressZipCode))
                    {
                        if (string.IsNullOrEmpty(location))
                        {
                            location = entityPOCO.ToAddressZipCode;
                        }

                        else
                        {
                            location += " - " + entityPOCO.ToAddressZipCode;
                        }
                    }

                    entityPM.DeliveryLocation = location;
                }
                #endregion
            }
            #endregion

        }

        private string GetAddress(Address address)
        {
            string resultAddress = "";

            if (address != null)
            {
                resultAddress = address.Address1 != null ? address.Address1 : "";

                if (!string.IsNullOrEmpty(address.Address2))
                {
                    resultAddress = resultAddress + Environment.NewLine + address.Address2;
                }

                if (!string.IsNullOrEmpty(address.City))
                {
                    resultAddress = resultAddress + Environment.NewLine + address.City;
                }

                if (address.State != null)
                {
                    if (address.IsLocalLanguage)
                    {
                        resultAddress = resultAddress + " " + (address.State.LocalName != null ? address.State.LocalName : "");
                    }

                    else
                    {
                        resultAddress = resultAddress + " " + (address.State.EnglishName != null ? address.State.EnglishName : "");
                    }
                }

                if (!string.IsNullOrEmpty(address.ZipCode))
                {
                    resultAddress = resultAddress + " " + address.ZipCode;
                }

                if (address.Country != null)
                {
                    if (address.IsLocalLanguage)
                    {
                        resultAddress = resultAddress + Environment.NewLine + address.Country.LocalName;
                    }

                    else
                    {
                        resultAddress = resultAddress + Environment.NewLine + address.Country.EnglishName;
                    }
                }
            }

            return resultAddress;
        }
    }


}
   