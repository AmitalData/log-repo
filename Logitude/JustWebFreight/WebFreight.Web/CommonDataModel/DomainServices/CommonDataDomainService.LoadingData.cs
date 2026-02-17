using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {

        int counter = 0;
        public void loadOceanPorts(string file)
        {
            List<Country> countries = countryRepository.GetCountries(0).ToList<Country>();
            List<Port> ports = portRepository.GetPorts(0).ToList();
            List<GlobalZone> globalZones = globalZoneRepository.GetGlobalZones(0).ToList();
            int count = 0;
            string[] mynewline = null;

            string[] readData2 = null;
            string countryCode = "";
            string portCode = "";
            string iata = "";
            string portName = "";

            mynewline = file.Split('\n');

            foreach (string line in mynewline)
            {
                counter++;
                if (line != "\r" && line != "")
                {
                    readData2 = line.Split(',');

                    portCode = readData2[0].Trim();
                    countryCode = readData2[2].Trim();
                    portName = readData2[1].TrimEnd();
                    iata = readData2[3].Trim();

                    if (!string.IsNullOrEmpty(portCode))
                    {


                        //by jalal 17/7

                        if (!string.IsNullOrEmpty(iata))
                        {
                            portCode = iata;
                        }

                        bool portexists = (from a in ports
                                           where a.Code == portCode && a.Country.Code == countryCode
                                           select a).Any();

                        if (!portexists)
                        {
                            Port newPort = new Port();
                            newPort.Id = IdCounter.GetNumber("Port", 0).ToString();

                            newPort.Code = portCode;

                            if (portName.Count() >= 40)
                            {
                                newPort.EnglishName = portName.Substring(0, 40);
                                newPort.LocalName = portName.Substring(0, 40);
                            }
                            else
                            {
                                newPort.EnglishName = portName;
                                newPort.LocalName = portName;
                            }
                            newPort.IsAir = false;
                            newPort.IsInland = true;
                            newPort.IsOcean = true;
                            newPort.InActive = false;
                            newPort.AddedManually = false;
                            newPort.Tenant = 0;

                            //if (read_data.Length == 3) 
                            // if there is no code in column 4 take country code from col3
                            Country country = countries.Where(c => c.Code == countryCode).FirstOrDefault();

                            if (country != null)
                            {
                                newPort.CountryId = country.Id;
                                newPort.Country = country;
                            }

                            if (newPort.Code.Length == 3 && newPort.EnglishName.Length >= 1 && country != null)
                            {
                                portRepository.Add(newPort);
                            }
                        }
                        else
                        {

                            Port oldport = (from a in ports
                                            where a.Code == portCode && a.Country.Code == countryCode
                                            select a).FirstOrDefault();
                            if (oldport != null)
                            {
                                oldport.IsOcean = true;
                                oldport.IsInland = true;
                                oldport.EnglishName = portName;
                            }
                            portRepository.Update(oldport);
                        }
                    }

                    if (counter == 100)
                    {
                        counter = 0;

                        portRepository.SubmitChanges();

                    }
                }
            }
            portRepository.SubmitChanges();
        }

        public void LoadDemoPartners(int tenant)
        {

        }

        public void LoadWarehouse(int tenant)
        {

        }

        public void LoadPortsToTenantZero(string portsStringOfLines)
        {
            List<Country> countries = countryRepository.GetCountries(0).ToList();

            int count = 0;
            string[] stringLineArray = portsStringOfLines.Split('\n');
            //portLinesCount = stringLineArray.Length;
            string[] readData = null;
            for (int i = 0; i < stringLineArray.Length; i++)
            {
                if (!string.IsNullOrEmpty(stringLineArray[i]))
                {
                    readData = stringLineArray[i].Split(',');

                    if (readData[0].Length == 3 && readData[1].Length >= 1)
                    {
                        Port newPort = new Port();
                        newPort.Code = readData[0].Trim();
                        if (readData[1].Trim().Count() >= 40)
                        {
                            newPort.EnglishName = readData[1].Trim().Substring(0, 40);
                            newPort.LocalName = readData[1].Trim().Substring(0, 40);
                        }
                        else
                        {
                            newPort.EnglishName = readData[1].Trim();
                            newPort.LocalName = readData[1].Trim();
                        }

                        newPort.IsAir = true;
                        newPort.IsInland = false;
                        newPort.IsOcean = false;
                        newPort.InActive = false;
                        newPort.AddedManually = false;
                        newPort.Tenant = 0;
                        newPort.Id = IdCounter.GetNumber("Port", 0).ToString();

                        string countryCode;
                        if (readData.Length == 4)
                            countryCode = readData[3].Trim();
                        else
                            countryCode = readData[2].Trim();


                        //if (read_data.Length == 3) 
                        // if there is no code in column 4 take country code from col3
                        Country country = countries.Where(c => c.Code == countryCode).FirstOrDefault();

                        if (country != null)
                        {
                            newPort.CountryId = country.Id;
                            //newPort.Country = country;
                        }

                        if (newPort.Code.Length == 3 && newPort.EnglishName.Length >= 1 && country != null)
                        {
                            portRepository.Add(newPort);
                        }
                        count++;
                    }
                }
                if (count == 100)
                {
                    count = 0;

                    //CountriesRepository.SubmitChanges();
                    portRepository.SubmitChanges();
                }
            }
            portRepository.SubmitChanges();
        }

        public void LoadPortLocations(string locations)
        {
            List<Port> ports = portRepository.GetPorts(0).ToList();
            string[] stringLineArray = locations.Split('\n');
            string[] readData = null;

            foreach (string location in stringLineArray)
            {
                if (!string.IsNullOrEmpty(location))
                {
                    readData = location.Split(',');

                    if (readData[0].Length == 3 && readData[1].Length == 3 && readData.Count() == 14)
                    {
                        double latitude = ConvertLocation(readData[11]);
                        double longtitude = ConvertLocation(readData[12]);
                        Port port = (from a in ports
                                     where a.Code == readData[0]
                                     select a).FirstOrDefault();

                        if (port != null)
                        {
                            port.Latitude = latitude;
                            port.Longtitude = longtitude;
                            portRepository.Update(port);
                        }
                    }
                }
            }
            portRepository.SubmitChanges();
        }

        public double ConvertLocation(string toConvertString)
        {
            try
            {
                string[] stringsArray = toConvertString.Split('.');
                string second = stringsArray[2].Substring(0, 2);
                string sign = stringsArray[2].Substring(2, 1);
                double basic = Convert.ToDouble(stringsArray[0]);
                double mins = Convert.ToDouble(stringsArray[1]);
                double secs = Convert.ToDouble(second);
                double sum = (((mins * 60) + secs) / 3600) + basic;

                if (sign == "S" || sign == "W")
                {
                    sum = sum * -1;
                }
                return sum;
            }
            catch (Exception ex)
            {

            }
            return 0;
        }

        //public List<CardView> GetCardViewsByTenant(int tenant, string type)
        //{
        //    CommonDataContext context = new CommonDataContext();
        //    List<Card> list = context.Cards.Where(c => c.Tenant == tenant && c.PartnerType.Id == type).ToList();
        //    List<Address> Addresses = context.Addresses.Where(c => c.Tenant== tenant).ToList();
        //    var result = from card in list
        //                 select new CardView()
        //                 {
        //                     Id = card.Id,
        //                     EnglishName = card.EnglishName,
        //                     CreateDate = card.CreateDate,
        //                     CardViewId = card.Id,
        //                     LocalName = card.LocalName,
        //                     InActive = card.InActive,
        //                     VatNumber = card.VatNumber,
        //                     Tenant = card.Tenant,
        //                     Code = card.Code,
        //                     Type = card.PartnerTypeId,
        //                     Country = ((Addresses != null ? Addresses.Where(a => a.AddressType.Id == "M").FirstOrDefault() : null) != null ? Addresses.Where(a => a.AddressType.Id == "M").FirstOrDefault().Country : null),
        //                     CityName = ((Addresses != null ? Addresses.Where(a => a.AddressType.Id == "M").FirstOrDefault() : null) != null ? Addresses.Where(a => a.AddressType.Id == "M").FirstOrDefault().City : null),
        //                     CountryId = ((Addresses != null ? Addresses.Where(a => a.AddressType.Id == "M").FirstOrDefault() : null) != null ? Addresses.Where(a => a.AddressType.Id == "M").FirstOrDefault().Country.Id : null)
        //                 };

        //    return result.ToList();
        //}

        Dictionary<string, string> countryDect = new Dictionary<string, string>();

        public void LoadCountries(string countriesStringOfLine)
        {
            string[] stringLineArray = countriesStringOfLine.Split('\n');

            //portLinesCount = stringLineArray.Length;
            string[] readCountryData = null;

            for (int i = 0; i < stringLineArray.Length; i++)
            {
                if (!string.IsNullOrEmpty(stringLineArray[i]))
                {
                    readCountryData = stringLineArray[i].Split(',');

                    if (readCountryData.Length >= 2)
                    {
                        if (readCountryData[4].Length > 119)
                            readCountryData[4] = readCountryData[4].Substring(0, 119);

                        if (readCountryData[0].Length == 2 && readCountryData[1].Length == 2 && readCountryData[4].Trim() != String.Empty)
                        {
                            if (!countryDect.Keys.Contains(readCountryData[1]))
                            {
                                string gzc = readCountryData[0];
                                Country newcountry = new Country();
                                newcountry.Code = readCountryData[1];
                                newcountry.Id = IdCounter.GetNumber("Country", 0).ToString();
                                newcountry.EnglishName = readCountryData[4];
                                newcountry.LocalName = readCountryData[4];
                                newcountry.Tenant = 0;
                                newcountry.AddedManually = false;
                                newcountry.InActive = false;
                                newcountry.GlobalZoneId = globalZoneRepository.GetGlobalZones(0).Where(d => d.Code == gzc).FirstOrDefault().Id;
                                newcountry.EC = readCountryData[5] == "TRUE" ? true : false;
                                countryDect.Add(readCountryData[1], readCountryData[1]);
                                countryRepository.Add(newcountry);
                            }
                        }
                    }
                }
            }
            countryRepository.SubmitChanges();
        }

        //----global zones-----
        Dictionary<string, string> globalZonesDect = new Dictionary<string, string>();
        public void LoadGlobalZones(string globalZonesStringOfLine)
        {
            string[] stringLineArray = globalZonesStringOfLine.Split('\n');
            //portLinesCount = stringLineArray.Length;
            string[] readGlobalZoneData = null;

            for (int i = 0; i < stringLineArray.Length; i++)
            {
                if (!string.IsNullOrEmpty(stringLineArray[i]))
                {
                    readGlobalZoneData = stringLineArray[i].Split(',');

                    if (readGlobalZoneData.Length >= 2)
                    {
                        if (readGlobalZoneData[1].Length > 119)
                            readGlobalZoneData[1] = readGlobalZoneData[1].Substring(0, 119);

                        if (readGlobalZoneData[0].Length == 2 && readGlobalZoneData[1].Trim() != String.Empty)
                        {
                            if (!globalZonesDect.Keys.Contains(readGlobalZoneData[0]))
                            {
                                GlobalZone newGlobalZone = new GlobalZone();
                                newGlobalZone.Code = readGlobalZoneData[0];
                                newGlobalZone.Id = IdCounter.GetNumber("GlobalZone", 0).ToString();
                                newGlobalZone.EnglishName = readGlobalZoneData[1];
                                newGlobalZone.LocalName = readGlobalZoneData[1];
                                newGlobalZone.Tenant = 0;
                                newGlobalZone.InActive = false;
                                globalZonesDect.Add(readGlobalZoneData[0], readGlobalZoneData[0]);
                                globalZoneRepository.Add(newGlobalZone);
                            }
                        }
                    }
                }
            }
            globalZoneRepository.SubmitChanges();
        }

        public void LoadAirlines(string airlinesStringOfLines)
        {
            CardRepository CardsRepository = new CardRepository(objectContext);
            AirlineRepository airlinesRepository = new AirlineRepository(objectContext);
            string[] stringLineArray = airlinesStringOfLines.Split('\n');
            //portLinesCount = stringLineArray.Length;
            string[] readData = null;
            List<Country> countries = countryRepository.GetCountries(0).ToList();
            List<State> states = stateRepository.GetStates(0).ToList();
            for (int i = 0; i < stringLineArray.Length; i++)
            {
                if (!string.IsNullOrEmpty(stringLineArray[i]))
                {
                    readData = stringLineArray[i].Split(',');
                    if (readData[0].Length >= 2)
                    {
                        Card carrier = new Card()
                        {
                            Tenant = 0,
                            //CarrierCode = read_data[0],
                            EnglishName = readData[1],
                            LocalName = readData[1],
                            PartnerTypeId = "AL",
                            CreateDate = DateTime.Now,
                            InActive = false,
                            Id = IdCounter.GetNumber("Card", 0).ToString(),
                            Code = readData[0],//CodeCounter.GetNumber("Airline", 0).ToString(),
                            Website = readData[12],

                        };
                        Country country = countries.Where(c => c.Code == readData[6]).FirstOrDefault();
                        State state = states.Where(d => d.Code == readData[7]).FirstOrDefault();
                        if (country != null && !string.IsNullOrEmpty(readData[5]))
                        {
                            Address address = new Address()
                            {
                                Address1 = readData[3],
                                Address2 = readData[4],
                                City = readData[5],
                                CountryId = country.Id,
                                StateId = state != null ? state.Id : null,
                                ZipCode = readData[8],
                                PhoneNumber = readData[9],
                                FaxNumber = readData[10],
                                ATTN = readData[11],
                                Id = IdCounter.GetNumber("Address", 0).ToString(),
                                CardId = carrier.Id,
                                AddressTypeId = "M",
                                Name = readData[2],
                                Tenant = 0,
                                Description = readData[2],
                            };
                            addressRepository.Add(address);
                        }
                        Airline airline = new Airline()
                        {
                            Tenant = 0,
                            Prefix = readData[2],
                            AddedManually = false,
                            Id = carrier.Id,
                            CheckDigit = true,
                        };
                        CardsRepository.Add(carrier);
                        airlinesRepository.Add(airline);
                    }
                }
                if (counter == 100)
                {
                    CardsRepository.SubmitChanges();
                    addressRepository.SubmitChanges();
                    airlinesRepository.SubmitChanges();
                }
            }
            CardsRepository.SubmitChanges();
            addressRepository.SubmitChanges();
            airlinesRepository.SubmitChanges();
        }

        public void LoadStates(string statesStringOfLines)
        {
            List<Country> countries = countryRepository.GetCountries(0).ToList();
            string[] stringLineArray = statesStringOfLines.Split('\n');
            //portLinesCount = stringLineArray.Length;
            string[] readData = null;

            for (int i = 0; i < stringLineArray.Length; i++)
            {
                if (!string.IsNullOrEmpty(stringLineArray[i]))
                {
                    readData = stringLineArray[i].Split(',');

                    if (readData[0] != String.Empty && readData[1].Length == 2 && readData[2] != String.Empty)
                    {
                        string countryCode;
                        if (readData[2] == "USA")
                        {
                            countryCode = "US";
                        }

                        else
                        {
                            countryCode = "CA";
                        }

                        Country country = countries.Where(c => c.Code == countryCode).FirstOrDefault();

                        State state = new State()
                        {
                            EnglishName = readData[0],
                            LocalName = readData[0],
                            Code = readData[1],
                            Id = IdCounter.GetNumber("State", 0).ToString(),
                            Tenant = 0,
                            InActive = false,
                            CountryId = country.Id
                        };

                        stateRepository.Add(state);
                    }
                }
            }

            stateRepository.SubmitChanges();
        }

        //----PackageTypes
        Dictionary<string, string> packageTypesDect = new Dictionary<string, string>();
        public void LoadPackageTypes(string packageTypesStringOfLine)
        {
            string[] stringLineArray = packageTypesStringOfLine.Split('\n');
            //portLinesCount = stringLineArray.Length;
            string[] readPackageTypesData = null;

            for (int i = 0; i < stringLineArray.Length; i++)
            {
                if (!string.IsNullOrEmpty(stringLineArray[i]))
                {
                    readPackageTypesData = stringLineArray[i].Split(',');

                    if (readPackageTypesData.Length >= 2)
                    {
                        if (readPackageTypesData[1].Length > 119)
                            readPackageTypesData[1] = readPackageTypesData[1].Substring(0, 119);

                        if (readPackageTypesData[1].Trim() != String.Empty)
                        {
                            if (!packageTypesDect.Keys.Contains(readPackageTypesData[0]))
                            {
                                PackageType newPackageType = new PackageType();
                                newPackageType.Id = IdCounter.GetNumber("PackageType", 0).ToString();
                                newPackageType.EnglishName = readPackageTypesData[0];
                                newPackageType.LocalName = readPackageTypesData[0];
                                newPackageType.Code = readPackageTypesData[1];
                                if (readPackageTypesData[4] == "Y")
                                {
                                    newPackageType.TEU = Convert.ToDouble(readPackageTypesData[2]);
                                    newPackageType.ContainerSize = Convert.ToInt32(readPackageTypesData[3]);
                                    newPackageType.IsContainer = true;
                                    newPackageType.IsOcean = true;
                                    newPackageType.IsAir = false;
                                    newPackageType.IsInland = true;
                                    Measurement newMeasurement = new Measurement();
                                    newMeasurement.Id = IdCounter.GetNumber("Measurement", 0).ToString();
                                    newMeasurement.IsContainer = true;
                                    newMeasurement.Name = newPackageType.EnglishName;
                                    newMeasurement.Code = newPackageType.Code;
                                    newMeasurement.ShortName = newPackageType.EnglishName;
                                    newPackageType.MeasurementId = newMeasurement.Id;
                                    measurementRepository.Add(newMeasurement);
                                }
                                else
                                {
                                    newPackageType.IsInland = true;
                                    newPackageType.IsOcean = true;
                                    newPackageType.IsAir = true;
                                }

                                packageTypesDect.Add(readPackageTypesData[1], readPackageTypesData[1]);
                                packageTypeRepository.Add(newPackageType);
                            }
                        }
                    }
                }
            }

            packageTypeRepository.SubmitChanges();
            measurementRepository.SubmitChanges();
        }

        //----ChargesTypes
        Dictionary<string, string> chargesTypesDect = new Dictionary<string, string>();
        public void LoadChargesTypes(string chargesTypesStringOfLine)
        {
            string[] stringLineArray = chargesTypesStringOfLine.Split('\n');
            //portLinesCount = stringLineArray.Length;
            string[] readChargesTypesData = null;
            List<Measurement> measurementList = measurementRepository.GetMeasurementsByTenant(0).ToList();
            List<VatType> vatTypeList = vatTypeRepository.GetVatTypes(0).ToList();

            IATACodeRepository iATACodeRepository = new IATACodeRepository(0);
            List<IATACode> allIATACodes = iATACodeRepository.GetIATACodes().ToList();

            for (int i = 0; i < stringLineArray.Length; i++)
            {
                if (!string.IsNullOrEmpty(stringLineArray[i]))
                {
                    readChargesTypesData = stringLineArray[i].Split(',');

                    if (readChargesTypesData.Length >= 2)
                    {
                        if (!chargesTypesDect.Keys.Contains(readChargesTypesData[0].Trim()))
                        {
                            ChargesType newChargesType = new ChargesType()
                            {
                                Id = IdCounter.GetNumber("ChargesType", 0).ToString(),
                                EnglishName = readChargesTypesData[0].Trim(),
                                Code = readChargesTypesData[1].Trim(),
                                LocalName = readChargesTypesData[2].Trim(),
                            };

                            if (!string.IsNullOrEmpty(readChargesTypesData[3].Trim()))
                            {
                                newChargesType.Description = readChargesTypesData[3].Trim();
                            }
                            if (!string.IsNullOrEmpty(readChargesTypesData[4].Trim()))
                            {
                                newChargesType.ContainerMeasurementId = measurementList.Where(d => d.Code == readChargesTypesData[4].Trim()).FirstOrDefault().Id;
                            }
                            if (!string.IsNullOrEmpty(readChargesTypesData[5].Trim()))
                            {
                                newChargesType.MeasurementId = measurementList.Where(d => d.Code == readChargesTypesData[5].Trim()).FirstOrDefault().Id;
                            }
                            newChargesType.ChargesGroupCode = readChargesTypesData[6].Trim();

                            if (!string.IsNullOrEmpty(readChargesTypesData[7].Trim()))
                            {
                                newChargesType.VatTypeId = vatTypeList.Where(d => d.Code == readChargesTypesData[7].Trim()).FirstOrDefault().Id;
                            }
                            if (!string.IsNullOrEmpty(readChargesTypesData[8].Trim()))
                            {
                                string myCode = readChargesTypesData[8].Trim();
                                IATACode myIATAItem = allIATACodes.Where(d => d.Code == myCode).FirstOrDefault();
                                if (myIATAItem != null)
                                {
                                    newChargesType.IATACodeId = myIATAItem.Id;
                                }
                            }
                            newChargesType.DueTypeCode = readChargesTypesData[9].Trim();
                            newChargesType.IsReceivable = Convert.ToBoolean(readChargesTypesData[10].Trim());
                            newChargesType.IsPayable = Convert.ToBoolean(readChargesTypesData[11].Trim());
                            newChargesType.IsAir = Convert.ToBoolean(readChargesTypesData[12].Trim());
                            newChargesType.IsOcean = Convert.ToBoolean(readChargesTypesData[13].Trim());
                            newChargesType.IsInland = Convert.ToBoolean(readChargesTypesData[14].Trim());
                            newChargesType.IsAutoDisplayInShipment = Convert.ToBoolean(readChargesTypesData[15].Trim());
                            newChargesType.IsAutoDisplayInQuote = Convert.ToBoolean(readChargesTypesData[20].Trim());
                            newChargesType.IsAutoDisplayInConsolidation = Convert.ToBoolean(readChargesTypesData[16].Trim());
                            newChargesType.ViewOrder = Convert.ToInt32(readChargesTypesData[21].Trim());
                            newChargesType.ReceivableAccountId = null;
                            newChargesType.PayableAccountId = null;
                            chargesTypeRepository.Add(newChargesType);
                        }
                    }
                }
            }
            chargesTypeRepository.SubmitChanges();
        }

        //---Shipping Lines
        public void LoadShippingLines(string shippingLinesStringOfLines)
        {
            CardRepository CardsRepository = new CardRepository(objectContext);
            ShippingLineRepository shippingLinesRepository = new ShippingLineRepository(objectContext);
            string[] stringLineArray = shippingLinesStringOfLines.Split('\n');
            //portLinesCount = stringLineArray.Length;
            string[] readData = null;

            for (int i = 0; i < stringLineArray.Length; i++)
            {
                if (!string.IsNullOrEmpty(stringLineArray[i]))
                {
                    readData = stringLineArray[i].Split(',');
                    if (readData.Length == 2)
                    {
                        if (readData[0].Length == 4 && readData[1] != String.Empty)
                        {
                            Card carrier = new Card()
                            {
                                Tenant = 0,
                                Code = readData[0],
                                EnglishName = readData[1],
                                LocalName = readData[1],
                                PartnerTypeId = "SL",

                                InActive = false,
                                Id = IdCounter.GetNumber("Card", 0).ToString(),
                                //Code=CodeCounter.GetNumber("ShippingLine",0).ToString(),
                                CreateDate = DateTime.Now,
                            };
                            ShippingLine shippingLine = new ShippingLine()
                            {
                                Tenant = 0,
                                SCACCode = readData[0],
                                AddedManually = false,
                                Id = carrier.Id,
                            };
                            CardsRepository.Add(carrier);
                            shippingLinesRepository.Add(shippingLine);
                        }
                    }
                }
                CardsRepository.SubmitChanges();
                shippingLinesRepository.SubmitChanges();
            }
        }

        public void loadPorts(string file)
        {
            //0         1          2       3      4       5         6
            //Code,EnglishName,LocalName,IsAir,IsOcean,IsInland,CountryCode

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(0);
            }
            countryRepository = new CountryRepository(objectContext);
            portRepository = new PortRepository(objectContext);
            globalZoneRepository = new GlobalZoneRepository(objectContext);

            List<Country> countries = countryRepository.GetCountries(0).ToList<Country>();
            List<Port> ports = portRepository.GetPorts(0).ToList();
            List<GlobalZone> globalZones = globalZoneRepository.GetGlobalZones(0).ToList();

            int count = 0;
            string[] mynewline = null;
            string[] readData2 = null;
            string countryCode = "";
            string portCode = "";
            string iata = "";
            string portEnglishName = "";
            bool isAir;
            bool isOcean;
            bool isinland;

            mynewline = file.Split('\n');

            foreach (string line in mynewline)
            {
                counter++;
                if (line != "\r" && line != "")
                {
                    readData2 = line.Split(',');
                    portCode = readData2[0].Trim();
                    portEnglishName = readData2[1].TrimEnd();
                    isAir = readData2[3] == "0" ? false : true;
                    isOcean = readData2[4] == "0" ? false : true;
                    isinland = readData2[5] == "0" ? false : true;
                    countryCode = readData2[6].Trim();
                    
                    //iata = readData2[3].Trim();                    

                    if (!string.IsNullOrEmpty(portCode))
                    {
                        if (!string.IsNullOrEmpty(iata))
                        {
                            portCode = iata;
                        }

                        bool portexists = (from a in ports
                                           where a.Code == portCode && a.Country.Code == countryCode
                                           select a).Any();

                        if (!portexists)
                        {
                            Port newPort = new Port();
                            newPort.Id = IdCounter.GetNumber("Port", 0).ToString();

                            newPort.Code = portCode;

                            if (portEnglishName.Count() >= 40)
                            {
                                newPort.EnglishName = portEnglishName.Substring(0, 40);
                                newPort.LocalName = portEnglishName.Substring(0, 40);
                            }
                            else
                            {
                                newPort.EnglishName = portEnglishName;
                                newPort.LocalName = portEnglishName;
                            }
                            //newPort.IsAir = false;
                            //newPort.IsInland = true;
                            //newPort.IsOcean = true;
                            newPort.InActive = false;
                            newPort.AddedManually = false;
                            newPort.Tenant = 0;
                            newPort.Latitude = 0;
                            newPort.Longtitude = 0;

                            Country country = countries.Where(c => c.Code == countryCode).FirstOrDefault();

                            if (country != null)
                            {
                                newPort.CountryId = country.Id;
                                newPort.Country = country;
                            }

                            if (newPort.Code.Length == 3 && newPort.EnglishName.Length >= 1 && country != null)
                            {
                                portRepository.Add(newPort);
                            }
                        }
                        else
                        {

                            Port oldport = (from a in ports
                                            where a.Code == portCode && a.Country.Code == countryCode
                                            select a).FirstOrDefault();
                            if (oldport != null)
                            {
                                oldport.IsOcean = true;
                                oldport.IsInland = true;
                                oldport.EnglishName = portEnglishName;
                            }
                            portRepository.Update(oldport);
                        }
                    }

                    if (counter == 100)
                    {
                        counter = 0;
                        portRepository.SubmitChanges();
                    }
                }
            }
            portRepository.SubmitChanges();
        }
    }
}