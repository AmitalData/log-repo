using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.XSD.CW_API.ABM
{
    public class ABMDataContext
    {
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public ShipmentPM Shipment { get; set; }

        public string UserID { get; set; }
        public string Password { get; set; }
        public string CompanyID { get; set; }
        public string ApplicationID { get; set; }

        public string ShipmentNumber { get; set; }
        public string HouseNumber { get; set; }
        public string MasterNumber { get; set; }
        public double? ValueOfGoods { get; set; }
        public string ValueOfGoodsCurrencyCode { get; set; }
        public string TransportConveyance { get; set; }
        public string TransportTPMode { get; set; }

        public string MainCarriageFromPortCountryCode { get; set; }
        public string FinalDestinationPortCountryCode { get; set; }
        public string FromPortCode { get; set; }
        public string FinalDestinationPortCode { get; set; }

        //public string ShipperName { get; set; }
        //public string ShipperAddress1 { get; set; }
        //public string ShipperAddress2 { get; set; }
        //public string ShipperCity { get; set; }
        //public string ShipperZipCode { get; set; }
        //public string ShipperCountryCode { get; set; }
        //public string ShipperReference1 { get; set; }
        //public string ShipperReference2 { get; set; }

        //public string ConsigneeName { get; set; }
        //public string ConsigneeAddress1 { get; set; }
        //public string ConsigneeAddress2 { get; set; }
        //public string ConsigneeCity { get; set; }
        //public string ConsigneeZipCode { get; set; }
        //public string ConsigneeCountryCode { get; set; }
        //public string ConsigneeReference1 { get; set; }
        //public string ConsigneeReference2 { get; set; }

        public string DescriptionOfGoods { get; set; }

        public string NumberOfPackages { get; set; }
        public string GrossWeight { get; set; }
        public string NetWeight { get; set; } // net weight = gross - tare
        //public string FreightProrate { get; set; }
        //public string WeightProrate { get; set; }

        public string IncotermCode { get; set; }

        public List<ShipmentPackagePM> Containers = new List<ShipmentPackagePM>();
        private ComputingPartnerTranslationHelper computingPartnerHelper;
        private ICommonDataContext iCommonContext;
        private AddressRepository iAddressRepository;
        private CountryRepository iCountryRepository;
        private CustomerRepository iCustomerRepository;
        public ABMDataContext(string shipmentId, int tenant, ICommonDataContext commonContext)
        {
            this.Tenant = tenant;
            this.ShipmentId = shipmentId;
            this.iCommonContext = commonContext;
            this.iAddressRepository = new AddressRepository(commonContext);
            this.iCountryRepository = new CountryRepository(commonContext);
            this.computingPartnerHelper = new ComputingPartnerTranslationHelper(Tenant);
            this.iCustomerRepository = new CustomerRepository(Tenant);
            this.GetCredentialsData();
            this.GetShipmentObject();

            if (this.Shipment != null)
            {
                this.BuildGeneralData();
                this.BuildPortsData();
                this.BuildPartnersData();
                this.BuildContainersData();
                this.BuildDimenssionsData();               
            }            
        }

        private void GetCredentialsData()
        {
            this.ApplicationID = LogitudeSettings.ABMProductId;

            CustomsInterfaceSettingRepository rep = new CustomsInterfaceSettingRepository(this.Tenant);
            CustomsInterfaceSetting entity = rep.GetSingleCustomsInterfaceSetting(this.Tenant, this.Tenant);

            if (entity != null)
            {
                this.UserID = entity.LocalUserId;
                this.Password = entity.LocalPassword;
                this.CompanyID = entity.LocalCompanyId;
            }
        }

        private void GetShipmentObject()
        {
            ShipmentQuery query = new ShipmentQuery(this.Tenant);
            Shipment = query.GetSinglePMWithoutComposition(ShipmentId, Tenant);
        }
        
        private void BuildGeneralData()
        {
            this.ShipmentNumber = this.Shipment.ShipmentNumber;
            this.HouseNumber = this.Shipment.House;
            this.MasterNumber = this.Shipment.Master;
            this.IncotermCode = this.Shipment.IncotermCode;

            this.DescriptionOfGoods = this.Shipment.DescriptionOfGoods;

            if (!string.IsNullOrEmpty(this.Shipment.MainHarmonize))
            {
                string mainHarmonizeText = "MainHarmonize: " + this.Shipment.MainHarmonize;

                if (string.IsNullOrEmpty(this.DescriptionOfGoods))
                {
                    this.DescriptionOfGoods = mainHarmonizeText;
                }

                else
                {
                    this.DescriptionOfGoods += " " + mainHarmonizeText;
                }
            }

            this.ValueOfGoods = this.Shipment.ValueOfGoods;

            if (this.Shipment.ValueOfGoodsCurrencyId != null)
            {
                Currency iCurrency = (from d in iCommonContext.Currencies where d.Id == this.Shipment.ValueOfGoodsCurrencyId select d).FirstOrDefault();
                if (iCurrency != null)
                {
                    this.ValueOfGoodsCurrencyCode = iCurrency.Code;
                }
            }

            this.BuildGeneralData_TransportConveyance();
            this.BuildGeneralData_TransportTPMode();
        }
        private void BuildGeneralData_TransportTPMode()
        {
            switch (this.Shipment.TransportModeId)
            {
                case "A":
                    {
                        this.TransportTPMode = "4";
                        break;
                    }

                case "O":
                    {
                        this.TransportTPMode = "1";
                        break;
                    }

                case "I":
                    {
                        this.TransportTPMode = "";
                        break;
                    }
            }
        }
        private void BuildGeneralData_TransportConveyance()
        {
            switch (this.Shipment.TransportModeId)
            {
                case "A":
                    {
                        this.TransportConveyance = this.Shipment.MainCarriageCarrierPrefix + this.Shipment.MainCarriageCarrierNumber;
                        break;
                    }

                case "O":
                    {
                        this.TransportConveyance = this.Shipment.MainCarriageVesselName;

                        if (!string.IsNullOrEmpty(this.Shipment.MainCarriageCarrierNumber))
                        {
                            if (string.IsNullOrEmpty(this.TransportConveyance))
                            {
                                this.TransportConveyance = this.Shipment.MainCarriageCarrierNumber;
                            }

                            else
                            {
                                this.TransportConveyance = this.TransportConveyance + " " + this.Shipment.MainCarriageCarrierNumber;
                            }
                        }

                        break;
                    }

                case "I":
                    {
                        this.TransportConveyance = this.Shipment.MainCarriageCarrierNumber;
                        break;
                    }
            }
        }

        private void BuildPortsData()
        {
            string mainCarriageFromPortTranslatedCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(Shipment.MainCarriageFromPortCountryCode, "G-ABM", "Country");
            string finalDestinationPortCountryTranslatedCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(Shipment.MainCarriageFinalDestinationPortCountryCode, "G-ABM", "Country");
            string fromPortTranslatedCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(Shipment.MainCarriageFromPortCountryCode + Shipment.MainCarriageFromPortCode, "G-ABM", "Port");
            string finalDestinationPortTranslatedCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(Shipment.MainCarriageFinalDestinationPortCountryCode + Shipment.MainCarriageFinalDestinationPortCode, "G-ABM", "Port");

            if (!string.IsNullOrEmpty(mainCarriageFromPortTranslatedCode))
            {
                this.MainCarriageFromPortCountryCode = mainCarriageFromPortTranslatedCode;
            }
            else
            {
                this.MainCarriageFromPortCountryCode = Shipment.MainCarriageFromPortCountryCode;
            }

            ////////
            if (!string.IsNullOrEmpty(finalDestinationPortCountryTranslatedCode))
            {
                this.FinalDestinationPortCountryCode = finalDestinationPortCountryTranslatedCode;
            }
            else
            {
                this.FinalDestinationPortCountryCode = Shipment.MainCarriageFinalDestinationPortCountryCode;
            }

            ///////
            if (!string.IsNullOrEmpty(fromPortTranslatedCode))
            {
                this.FromPortCode = fromPortTranslatedCode;
            }
            else
            {
                this.FromPortCode = Shipment.MainCarriageFromPortCountryCode + Shipment.MainCarriageFromPortCode;
            }

            if (!string.IsNullOrEmpty(finalDestinationPortTranslatedCode))
            {
                this.FinalDestinationPortCode = finalDestinationPortTranslatedCode;
            }
            else
            {
                this.FinalDestinationPortCode = Shipment.MainCarriageFinalDestinationPortCountryCode + Shipment.MainCarriageFinalDestinationPortCode;
            }
        }

        public List<CWXSD.Party> Parties { get; set; }
        private void BuildPartnersData()
        {
            this.Parties = new List<CWXSD.Party>();

            if (this.Shipment.ShipperId != null)
            {
                this.Parties.Add(this.GetPartnerParty(this.Shipment.ShipperId, this.Shipment.ShipperAddressId, "Consignor"));
            }

            if (this.Shipment.ConsigneeId != null)
            {
                this.Parties.Add(this.GetPartnerParty(this.Shipment.ConsigneeId, this.Shipment.ConsigneeAddressId, "Consignee"));
            }

            if (this.Shipment.CustomAgentImportId != null)
            {
                this.Parties.Add(this.GetPartnerParty(this.Shipment.CustomAgentImportId, this.Shipment.CustomAgentImportAddressId , "Declarant"));
            }
        }
        private CWXSD.Party GetPartnerParty(string partnerId, string addressId, string partyType)
        {
            CWXSD.Party iParty = new CWXSD.Party()
            {
                PartyType = partyType,
                AddressLocation = new CWXSD.GPSEvent(),
            };

            if (!string.IsNullOrEmpty(partnerId))
            {
                Card iCard = CardRepository.GetSingleCard(partnerId, this.Tenant, true);

                if (iCard != null)
                {                    
                    iParty.NameAddress = new CWXSD.NameAddress()
                    {
                        Name = iCard.EnglishName,
                    };

                    if (!string.IsNullOrEmpty(addressId))
                    {
                        Address iAddress = iAddressRepository.GetSingleAddress(addressId, this.Tenant);

                        if (iAddress != null)
                        {
                            iParty.NameAddress.Address1 = iAddress.Address1;
                            iParty.NameAddress.Address2 = iAddress.Address2;
                            iParty.NameAddress.Address3 = iAddress.City;
                            iParty.NameAddress.PostCode = iAddress.ZipCode;

                            if (iAddress.CountryId != null)
                            {
                                Country iCountry = iCountryRepository.GetSingleCountry(iAddress.CountryId, this.Tenant);
                                if (iCountry != null)
                                {
                                    string iCountryCode = iCountry.Code;
                                    string translatedCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(iCountryCode, "G-ABM", "Country");
                                    if (!string.IsNullOrEmpty(translatedCode))
                                    {
                                        iCountryCode = translatedCode;
                                    }

                                    iParty.NameAddress.Country = new CWXSD.Country()
                                    {
                                        CountryType = partyType,
                                        CodeType = CWXSD.CountryCodeType.ISO,
                                        Text = new string[] { iCountryCode },
                                    };
                                }
                            }
                        }
                    }

                    List<CWXSD.Reference> partyReference = this.GetPartyReference(partyType);
                    if(partyReference.Count > 0)
                    {
                        iParty.Reference = partyReference.ToArray<CWXSD.Reference>();
                    }
                    
                }
            }

            return iParty;
        }
        private List<CWXSD.Reference> GetPartyReference(string partyType)
        {
            List<CWXSD.Reference> list = new List<CWXSD.Reference>();

            string iReference1 = null;
            string iReference2 = null;
            string BTWRefrence = null;
            bool EORIRefrence = false ;

            switch (partyType)
            {
                case "Consignor":
                    {
                        iReference1 = this.Shipment.ShipperReference1;
                        iReference2 = this.Shipment.ShipperReference2;
                        break;
                    }

                case "Consignee":
                    {
                        iReference1 = this.Shipment.ConsigneeReference1;
                        iReference2 = this.Shipment.ConsigneeReference2;
                        BTWRefrence = this.Shipment.ConsigneeVatNumber;
                        EORIRefrence = true;
                        break;
                    }

                case "Declarant":
                    {
                        iReference1 = this.Shipment.CustomAgentImportReference;
                        break;
                    }
            }

            if (!string.IsNullOrEmpty(BTWRefrence))
            {
                CWXSD.Reference iRefrenceEORI = new CWXSD.Reference()
                {
                    RefCode = "BTW",
                    RefText = BTWRefrence,
                };
                list.Add(iRefrenceEORI);
            }

            if (EORIRefrence)
            {
                Customer customer = iCustomerRepository.GetSingleCustomer(this.Shipment.ConsigneeId, Tenant, false);
                if(customer != null)
                {
                    CWXSD.Reference iRefrenceBTW = new CWXSD.Reference()
                    {
                        RefCode = "EORI",
                        RefText = customer.EORInumber,
                    };

                    list.Add(iRefrenceBTW);
                }
            }
              
        
            //if (!string.IsNullOrEmpty(iReference1) || !string.IsNullOrEmpty(iReference2))
           // {
                //if (!string.IsNullOrEmpty(iReference1))
                //{
                //    list.Add(new CWXSD.Reference()
                //    {
                //        RefCode = "reference 1",
                //        RefText = iReference1,
                //    });
                //}

                //if (!string.IsNullOrEmpty(iReference2))
                //{
                //    list.Add(new CWXSD.Reference()
                //    {
                //        RefCode = "reference 2",
                //        RefText = iReference2,
                //    });
                //}
           // }

            return list;
        }

        private void BuildContainersData()
        {
            ShipmentPackageQuery rep = new ShipmentPackageQuery(Tenant);

            this.Containers = rep.GetShipmentPackages(ShipmentId, this.ShipmentNumber, Tenant);
        }

        private void BuildDimenssionsData()
        {
            if (Shipment.NumberOfPackages != null)
            {
                this.NumberOfPackages = Shipment.NumberOfPackages.ToString();
            }
            else if (Shipment.NumberOfContainers != null)
            {
                this.NumberOfPackages = Shipment.NumberOfContainers.ToString();
            }

            if (Shipment.GrossWeight != null)
            {
                this.GrossWeight = Shipment.GrossWeight.ToString();

                if(this.Containers.Count() > 0)
                {
                    double? totalTare = 0;

                    totalTare = this.Containers.Where(d => d.Tare != null).Sum(s => s.Tare);

                    this.NetWeight = (Shipment.GrossWeight - totalTare).ToString();
                }
            }            
        }       
    }
}
