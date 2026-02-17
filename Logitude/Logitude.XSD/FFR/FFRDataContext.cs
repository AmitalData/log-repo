using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.BL.Helpers;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Helpers;

namespace Logitude.XSD.FFR
{
    public class FFRDataContext
    {
        public int Tenant { get; set; }
        public string BookingId { get; set; }
        public Booking Booking { get; set; }

        #region Properties

        public string BookingStatusCode { get; set; }
        public string FFRStatusCode { get; set; }
        public bool WaitingForResponse { get; set; }
        public string Master { get; set; }
        public string AccountNumber { get; set; }
        public string NatureOfGoods { get; set; }
        public string BookingNumber { get; set; }
        public string DescriptionOfGoods { get; set; }
        public string DescriptionOfGoodsId { get; set; }
        public string DescriptionOfGoodsProductCode { get; set; }

        #region Carriers
        public string AirlinePrefix { get; set; }

        public string MainCarriageCarrierCode { get; set; }
        public string Transshipment1CarrierCode { get; set; }
        public string Transshipment2CarrierCode { get; set; }

        public string MainCarriageCarrierNumber { get; set; }
        public string Transshipment1CarrierNumber { get; set; }
        public string Transshipment2CarrierNumber { get; set; }

        public string MainCarriageCarrierETDDay { get; set; }        
        public string Transshipment1CarrierETDDay { get; set; }
        public string Transshipment2CarrierETDDay { get; set; }

        public string MainCarriageCarrierETDMonth { get; set; }
        public string Transshipment1CarrierETDMonth { get; set; }
        public string Transshipment2CarrierETDMonth { get; set; }

        public DateTime MainCarriageCarrierETD { get; set; }
        public DateTime Transshipment1CarrierETD { get; set; }
        public DateTime Transshipment2CarrierETD { get; set; }

        public string MainCarriageAllotmentIdentification { get; set; }
        public string Transshipment1AllotmentIdentification { get; set; }
        public string Transshipment2AllotmentIdentification { get; set; }

        public string MainCarriageSpaceAllocationCode { get; set; }
        public string Transshipment1SpaceAllocationCode { get; set; }
        public string Transshipment2SpaceAllocationCode { get; set; }
        #endregion

        #region Ports
        public string MainCarriageFromPortCode { get; set; }
        public string Transshipment1FromPortCode { get; set; }
        public string Transshipment2FromPortCode { get; set; }

        public string MainCarriageFromPortName { get; set; }
        public string MainCarriageToPortCode { get; set; }

        public string FinalDestinationPortCode { get; set; }
        public string Transshipment1ToPortCode { get; set; }
        public string Transshipment2ToPortCode { get; set; }

        public string MainCarriageFromPortId { get; set; }
        public string Transshipment1FromPortId { get; set; }
        public string Transshipment2FromPortId { get; set; }

        public string MainCarriageToPortId { get; set; }
        public string Transshipment1ToPortId { get; set; }
        public string Transshipment2ToPortId { get; set; }        
        #endregion

        #region Amounts
        public decimal Volume { get; set; }
        public decimal GrossWeight { get; set; }
        public int NumberOfPackages { get; set; }
        public string VolumeUnitCode { get; set; }
        public string DimensionsUnitCode { get; set; }
        public string GrossWeightUnitCode { get; set; }
        #endregion

        #region Shipper
        public string ShipperId { get; set; }
        public string ShipperName { get; set; }
        public string ShipperCity { get; set; }
        public string ShipperPhone { get; set; }
        public string ShipperZipCode { get; set; }
        public string ShipperAddress { get; set; }
        //public string ShipperAddress1 { get; set; }
        //public string ShipperAddress2 { get; set; }
        public string ShipperStateCode { get; set; }
        public string ShipperCountryCode { get; set; }
        #endregion

        #region Consignee
        public string ConsigneeId { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeCity { get; set; }
        public string ConsigneePhone { get; set; }
        public string ConsigneeZipCode { get; set; }
        public string ConsigneeAddress { get; set; }
        //public string ConsigneeAddress1 { get; set; }
        //public string ConsigneeAddress2 { get; set; }
        public string ConsigneeStateCode { get; set; }
        public string ConsigneeCountryCode { get; set; }
        #endregion

        #region CustomerIdentification
        public string IssuingCarrierAgentName { get; set; }
        public string IssuingCarrierAgentCity { get; set; }
        public string IssuingCarrierIATACode { get; set; }
        public string IssuingCarrierCASSCode { get; set; }
        public bool IsIssuingCarrierIATACodeSet { get; set; }
        public bool IsIssuingCarrierCASSCodeSet { get; set; }
        #endregion

        #region ShipmentReferenceInfo
        public bool IsReferenceInfoSpecified { get; set; }
        public string ReferenceNumber { get; set; }
        public string SupplementaryShipmentInformation1 { get; set; }
        public string SupplementaryShipmentInformation2 { get; set; }
        public List<string> SupplementaryTextList { get; set; }
        #endregion

        public string CommodityItemNumber { get; set; }
        public string FirstLineOSI { get; set; }
        public string SecondLineOSI { get; set; }

        public List<BookingPackage> BookingPackages { get; set; }
        public List<string> HandlingCodesList { get; set; }
        public List<string> SSRTextList { get; set; }
        
        #endregion

        private StateRepository stateRepository;
        private AddressRepository addressRepository;

        public FFRDataContext(Booking myBooking)
        {
            this.Tenant = myBooking.Tenant;
            this.BookingId = myBooking.Id;
            this.Booking = myBooking;

            this.stateRepository = new StateRepository(Tenant);
            this.addressRepository = new AddressRepository(Tenant);

            this.BuildBaseData();
            this.BuildConsignmentData();
            this.BuildBookingData();
            this.BuildDimensionsInformationData();
            this.BuildProductInformationData();
            this.BuildAmountsData();
            this.BuildPartnersData();
            this.BuildHandlingCodesList();
            this.BuildSSRTextList();
            this.BuildShipmentReferenceInfoData();
            this.BuildOSILines();
        }

        private void BuildBaseData()
        {
            this.Master = string.IsNullOrEmpty(Booking.Master) ? null : FormatHelper.FormatInteger(8, Booking.Master);
            this.NatureOfGoods = FormatHelper.FormatString(Booking.DescriptionOfGoods, FormatHelper.PatternType.Text, 15);
            this.BookingNumber = Booking.BookingNumber;
            this.CommodityItemNumber = Booking.AWBCommodityItemNumber;
            this.DescriptionOfGoodsId = Booking.DescriptionOfGoodsId;
            this.BookingStatusCode = Booking.BookingStatusCode;
            this.FFRStatusCode = Booking.FFRStatusCode;
            this.WaitingForResponse = Booking.WaitingForResponse;

            if (!string.IsNullOrEmpty(DescriptionOfGoodsId))
            {
                AWBDescriptionOfGoodsRepository rep = new AWBDescriptionOfGoodsRepository(Tenant);
                AWBDescriptionOfGoods awbEntity = rep.GetSingleAWBDescriptionOfGoods(DescriptionOfGoodsId);

                if (awbEntity != null)
                {
                    this.DescriptionOfGoodsProductCode = awbEntity.ProductCode;
                }
            }
        }

        private void BuildConsignmentData()
        {
            this.MainCarriageFromPortId = Booking.MainCarriageFromPortId;
            this.Transshipment1FromPortId = Booking.Transshipment1FromPortId;
            this.Transshipment2FromPortId = Booking.Transshipment2FromPortId;
            this.MainCarriageToPortId = Booking.MainCarriageToPortId;
            this.Transshipment1ToPortId = Booking.Transshipment1ToPortId;
            this.Transshipment2ToPortId = Booking.Transshipment2ToPortId;
            this.AccountNumber = Booking.AccountNumber;

            #region MainCarriage
            if (!string.IsNullOrEmpty(Booking.MainCarriageFromPortId) && !string.IsNullOrEmpty(Booking.MainCarriageToPortId))
            {
                PortPM myFromPort = PortQuery.GetSinglePort(Tenant, Booking.MainCarriageFromPortId, false);
                PortPM myToPort = PortQuery.GetSinglePort(Tenant, Booking.MainCarriageToPortId, false);
                PortPM myFinalPort = PortQuery.GetSinglePort(Tenant, Booking.MainCarriageFinalDestinationPortId, false);

                this.MainCarriageFromPortCode = myFromPort.Code.ToUpper();
                this.MainCarriageFromPortName = myFromPort.EnglishName.ToUpper();
                this.MainCarriageToPortCode = myToPort.Code.ToUpper();
                this.FinalDestinationPortCode = myFinalPort.Code.ToUpper();

                this.AirlinePrefix = FormatHelper.FormatInteger(3, Booking.AirlinePrefix);

                if (!string.IsNullOrEmpty(Booking.MainCarriageCarrierId))
                {
                    Card myCard = CardRepository.GetSingleCard(Booking.MainCarriageCarrierId, Tenant, false);
                    if (myCard != null)
                    {
                        this.MainCarriageCarrierCode = myCard.Code.ToUpper();
                    }

                    if (!string.IsNullOrEmpty(Booking.MainCarriageCarrierPrefix))
                    {
                        this.MainCarriageCarrierCode = Booking.MainCarriageCarrierPrefix.Trim().ToUpper();
                    }
                }

                if (!string.IsNullOrEmpty(Booking.MainCarriageCarrierNumber))
                {
                    this.MainCarriageCarrierNumber = FormatHelper.FormatString(Booking.MainCarriageCarrierNumber, FormatHelper.PatternType.FlightNumber);
                }

                if (Booking.MainCarriageETD != null)
                {
                    int day = 0;
                    Int32.TryParse(String.Format("{0:dd}", Booking.MainCarriageETD.Value, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")), out day);

                    int month = 0;
                    Int32.TryParse(String.Format("{0:MMM}", Booking.MainCarriageETD.Value, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")), out month);

                    this.MainCarriageCarrierETDDay = FormatHelper.FormatInteger(2, day);
                    this.MainCarriageCarrierETDMonth = String.Format("{0:MMM}", Booking.MainCarriageETD.Value);
                    this.MainCarriageCarrierETD = Booking.MainCarriageETD.Value;
                }

                this.MainCarriageSpaceAllocationCode = Booking.MainCarriageSpaceAllocationCode;

                if (Booking.MainCarriageSpaceAllocationCode == "CA")
                {
                    if (!string.IsNullOrEmpty(Booking.MainCarriageAllotmentIdentification))
                    {
                        this.MainCarriageAllotmentIdentification = FormatHelper.FormatString(Booking.MainCarriageAllotmentIdentification, FormatHelper.PatternType.AllotmentIdentification);
                    }
                }
            }
            #endregion

            #region Transshipment1
            if (!string.IsNullOrEmpty(Booking.Transshipment1FromPortId) && !string.IsNullOrEmpty(Booking.Transshipment1ToPortId))
            {
                PortPM transshipment1FromPort = PortQuery.GetSinglePort(Tenant, Booking.Transshipment1FromPortId, false);
                PortPM myPort = PortQuery.GetSinglePort(Tenant, Booking.Transshipment1ToPortId, false);

                if (transshipment1FromPort != null)
                {
                    this.Transshipment1FromPortCode = transshipment1FromPort.Code.ToUpper();
                }

                if (myPort != null)
                {
                    this.Transshipment1ToPortCode = myPort.Code.ToUpper();
                }

                if (!string.IsNullOrEmpty(Booking.Transshipment1CarrierId))
                {
                    Card myCard = CardRepository.GetSingleCard(Booking.Transshipment1CarrierId, Tenant, false);
                    if (myCard != null)
                    {
                        this.Transshipment1CarrierCode = myCard.Code.ToUpper();
                    }

                    if (!string.IsNullOrEmpty(Booking.Transshipment1CarrierPrefix))
                    {
                        this.Transshipment1CarrierCode = Booking.Transshipment1CarrierPrefix.Trim().ToUpper();
                    }
                }

                if (!string.IsNullOrEmpty(Booking.Transshipment1CarrierNumber))
                {
                    this.Transshipment1CarrierNumber = FormatHelper.FormatString(Booking.Transshipment1CarrierNumber, FormatHelper.PatternType.FlightNumber);
                }

                if (Booking.Transshipment1ETD != null)
                {
                    int day = 0;
                    Int32.TryParse(String.Format("{0:dd}", Booking.Transshipment1ETD.Value, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")), out day);

                    int month = 0;
                    Int32.TryParse(String.Format("{0:MMM}", Booking.Transshipment1ETD.Value, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")), out month);

                    this.Transshipment1CarrierETDDay = FormatHelper.FormatInteger(2, day);
                    this.Transshipment1CarrierETDMonth = String.Format("{0:MMM}", Booking.Transshipment1ETD.Value);
                    this.Transshipment1CarrierETD = Booking.Transshipment1ETD.Value;
                }

                this.Transshipment1SpaceAllocationCode = Booking.Transshipment1SpaceAllocationCode;

                if (Booking.Transshipment1SpaceAllocationCode == "CA")
                {
                    if (!string.IsNullOrEmpty(Booking.Transshipment1AllotmentIdentification))
                    {
                        this.Transshipment1AllotmentIdentification = FormatHelper.FormatString(Booking.Transshipment1AllotmentIdentification, FormatHelper.PatternType.AllotmentIdentification);
                    }
                }
            }
            #endregion

            #region Transshipment2
            if (!string.IsNullOrEmpty(Booking.Transshipment2FromPortId) && !string.IsNullOrEmpty(Booking.Transshipment2ToPortId))
            {
                PortPM transshipment2FromPort = PortQuery.GetSinglePort(Tenant, Booking.Transshipment2FromPortId, false);
                PortPM myPort = PortQuery.GetSinglePort(Tenant, Booking.Transshipment2ToPortId, false);

                if (transshipment2FromPort != null)
                {
                    this.Transshipment2FromPortCode = transshipment2FromPort.Code.ToUpper();
                }
                
                if (myPort != null)
                {
                    this.Transshipment2ToPortCode = myPort.Code.ToUpper();
                }

                if (!string.IsNullOrEmpty(Booking.Transshipment2CarrierId))
                {
                    Card myCard = CardRepository.GetSingleCard(Booking.Transshipment2CarrierId, Tenant, false);
                    if (myCard != null)
                    {
                        this.Transshipment2CarrierCode = myCard.Code.ToUpper();
                    }

                    if (!string.IsNullOrEmpty(Booking.Transshipment2CarrierPrefix))
                    {
                        this.Transshipment2CarrierCode = Booking.Transshipment2CarrierPrefix.Trim().ToUpper();
                    }
                }

                if (!string.IsNullOrEmpty(Booking.Transshipment2CarrierNumber))
                {
                    this.Transshipment2CarrierNumber = FormatHelper.FormatString(Booking.Transshipment2CarrierNumber, FormatHelper.PatternType.FlightNumber);
                }

                if (Booking.Transshipment2ETD != null)
                {
                    int day = 0;
                    Int32.TryParse(String.Format("{0:dd}", Booking.Transshipment2ETD.Value, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")), out day);

                    int month = 0;
                    Int32.TryParse(String.Format("{0:MMM}", Booking.Transshipment2ETD.Value, System.Globalization.CultureInfo.CreateSpecificCulture("en-US")), out month);

                    this.Transshipment2CarrierETDDay = FormatHelper.FormatInteger(2, day);
                    this.Transshipment2CarrierETDMonth = String.Format("{0:MMM}", Booking.Transshipment2ETD.Value);
                    this.Transshipment2CarrierETD = Booking.Transshipment2ETD.Value;
                }

                this.Transshipment2SpaceAllocationCode = Booking.Transshipment2SpaceAllocationCode;

                if (Booking.Transshipment2SpaceAllocationCode == "CA")
                {
                    if (!string.IsNullOrEmpty(Booking.Transshipment2AllotmentIdentification))
                    {
                        this.Transshipment2AllotmentIdentification = FormatHelper.FormatString(Booking.Transshipment2AllotmentIdentification, FormatHelper.PatternType.AllotmentIdentification);
                    }
                }
            }
            #endregion
        }

        private void BuildBookingData()
        {
            //this.AllotmentIdentification = Booking.MainCarriageAllotmentIdentification;
            //this.SpaceAllocationCode = 
        }

        private void BuildDimensionsInformationData()
        {
            BookingPackageRepository bookingPackageRepository = new BookingPackageRepository(Tenant);
            this.BookingPackages = bookingPackageRepository.GetBookingPackagesForBookingTenant(BookingId, Tenant).ToList();
        }

        private void BuildProductInformationData()
        {
 
        }

        private void BuildAmountsData()
        {
            string myVolumeUnitCode = "";
            string myDimensionsUnitCode = string.IsNullOrEmpty(Booking.DimensionsUnitCode) ? "" : Booking.DimensionsUnitCode.ToUpper();
            string myGrossWeightUnitCode = "K";

            if (Booking.GrossWeightUnitCode == "LB")
            {
                myGrossWeightUnitCode = "L";
            }

            switch (myDimensionsUnitCode)
            {
                case "CM":
                    {
                        myDimensionsUnitCode = "CMT";
                        break;
                    }

                case "FT":
                    {
                        myDimensionsUnitCode = "FOT";
                        break;
                    }

                case "INC":
                    {
                        myDimensionsUnitCode = "INH";
                        break;
                    }
            }

            if (!string.IsNullOrEmpty(Booking.VolumeUnitCode))
            {
                switch (Booking.VolumeUnitCode.ToUpper())
                {
                    case "CBF": { myVolumeUnitCode = "CF"; break; }
                    case "CBI": { myVolumeUnitCode = "CI"; break; }
                    case "CBM": { myVolumeUnitCode = "MC"; break; }
                    default: { break; }
                }
            }

            int myPieces = Booking.NumberOfPackages == null ? 0 : Booking.NumberOfPackages.Value;
            decimal myVolume = (Booking.Volume == null) ? 0 : (decimal)Booking.Volume.Value;

            decimal myGrossWeight = 0;
            if (myGrossWeightUnitCode == "K")
            {
                myGrossWeight = (Booking.GrossWeightInKG == null) ? 0 : (decimal)Booking.GrossWeightInKG.Value;
            }

            else
            {
                myGrossWeight = (Booking.GrossWeight == null) ? 0 : (decimal)Booking.GrossWeight.Value;
            }


            this.Volume = MethodHelper.Normalize(myVolume);
            this.NumberOfPackages = myPieces;
            this.GrossWeight = MethodHelper.Normalize(myGrossWeight);
            this.VolumeUnitCode = myVolumeUnitCode;
            this.DimensionsUnitCode = myDimensionsUnitCode;
            this.GrossWeightUnitCode = myGrossWeightUnitCode;
        }

        private void BuildPartnersData()
        {
            #region Shipper
            if (!string.IsNullOrEmpty(Booking.ShipperId))
            {
                this.ShipperId = Booking.ShipperId;

                Card myCard = CardRepository.GetSingleCard(Booking.ShipperId, Tenant, false);
                if (myCard != null)
                {
                    this.ShipperName = FormatHelper.FormatString(myCard.EnglishName, FormatHelper.PatternType.Name);
                }

                if (!string.IsNullOrEmpty(Booking.ShipperAddressId))
                {
                    Address myAddress = addressRepository.GetSingleAddress(Booking.ShipperAddressId, Tenant);
                    if (myAddress != null)
                    {
                        this.ShipperAddress = FormatHelper.FormatString(myAddress.Address1 + " " + myAddress.Address2, FormatHelper.PatternType.Address);
                        //this.ShipperAddress1 = FormatHelper.FormatString(myAddress.Address1, FormatHelper.PatternType.Address);
                        //this.ShipperAddress2 = FormatHelper.FormatString(myAddress.Address2, FormatHelper.PatternType.Address);
                        this.ShipperCity = FormatHelper.FormatString(myAddress.City, FormatHelper.PatternType.City);
                        this.ShipperZipCode = FormatHelper.FormatString(myAddress.ZipCode, FormatHelper.PatternType.ZipCode);
                        this.ShipperPhone = FormatHelper.FormatString(myAddress.PhoneNumber, FormatHelper.PatternType.Phone);

                        Country myCountry = CountryRepository.GetSingleCountry(myAddress.CountryId, Tenant, false);
                        if (myCountry != null)
                        {
                            this.ShipperCountryCode = string.IsNullOrEmpty(myCountry.Code) ? "" : myCountry.Code.ToUpper();
                        }

                        if (!string.IsNullOrEmpty(myAddress.StateId))
                        {
                            State myState = stateRepository.GetSingleState(myAddress.StateId, Tenant);
                            if (myState != null)
                            {
                                this.ShipperStateCode = myState.Code.ToUpper();
                            }
                        }
                    }
                }
            }
            #endregion

            #region Consignee
            if (!string.IsNullOrEmpty(Booking.ConsigneeId))
            {
                this.ConsigneeId = Booking.ConsigneeId;

                Card myCard = CardRepository.GetSingleCard(Booking.ConsigneeId, Tenant, false);
                if (myCard != null)
                {
                    this.ConsigneeName = FormatHelper.FormatString(myCard.EnglishName, FormatHelper.PatternType.Name);
                }

                if (!string.IsNullOrEmpty(Booking.ConsigneeAddressId))
                {
                    Address myAddress = addressRepository.GetSingleAddress(Booking.ConsigneeAddressId, Tenant);
                    if (myAddress != null)
                    {
                        this.ConsigneeAddress = FormatHelper.FormatString(myAddress.Address1 + " " + myAddress.Address2, FormatHelper.PatternType.Address);
                        //this.ConsigneeAddress1 = FormatHelper.FormatString(myAddress.Address1, FormatHelper.PatternType.Address);
                        //this.ConsigneeAddress2 = FormatHelper.FormatString(myAddress.Address2, FormatHelper.PatternType.Address);
                        this.ConsigneeCity = FormatHelper.FormatString(myAddress.City, FormatHelper.PatternType.City);
                        this.ConsigneeZipCode = FormatHelper.FormatString(myAddress.ZipCode, FormatHelper.PatternType.ZipCode);
                        this.ConsigneePhone = FormatHelper.FormatString(myAddress.PhoneNumber, FormatHelper.PatternType.Phone);

                        Country myCountry = CountryRepository.GetSingleCountry(myAddress.CountryId, Tenant, false);
                        if (myCountry != null)
                        {
                            this.ConsigneeCountryCode = string.IsNullOrEmpty(myCountry.Code) ? "" : myCountry.Code.ToUpper();
                        }

                        if (!string.IsNullOrEmpty(myAddress.StateId))
                        {
                            State myState = stateRepository.GetSingleState(myAddress.StateId, Tenant);
                            if (myState != null)
                            {
                                this.ConsigneeStateCode = myState.Code.ToUpper();
                            }
                        }
                    }
                }
            }
            #endregion

            #region CustomerIdentification
            if (!string.IsNullOrEmpty(Booking.IssuingCarrierAgentId))
            {
                Card myCard = CardRepository.GetSingleCard(Booking.IssuingCarrierAgentId, Tenant, false);
                if (myCard != null)
                {
                    this.IssuingCarrierAgentName = FormatHelper.FormatString(myCard.EnglishName, FormatHelper.PatternType.Name);
                }

                if (!string.IsNullOrEmpty(Booking.IssuingCarrierAddressId))
                {
                    Address myAddress = addressRepository.GetSingleAddress(Booking.IssuingCarrierAddressId, Tenant);
                    if (myAddress != null)
                    {
                        this.IssuingCarrierAgentCity = FormatHelper.FormatString(myAddress.City, FormatHelper.PatternType.City);
                    }
                }

                if (!string.IsNullOrEmpty(Booking.IssuingCarrierIATACode))
                {
                    this.IsIssuingCarrierIATACodeSet = true;
                    this.IssuingCarrierIATACode = FormatHelper.FormatInteger(7, Booking.IssuingCarrierIATACode);
                }

                if (!string.IsNullOrEmpty(Booking.CASSCode))
                {
                    this.IsIssuingCarrierCASSCodeSet = true;
                    this.IssuingCarrierCASSCode = FormatHelper.FormatInteger(4, Booking.CASSCode);
                }
            }
            #endregion
        }

        private void BuildHandlingCodesList()
        {
            int itemLength = 3;
            string xmlField = "";
            string systemField = "";

            this.HandlingCodesList = new List<string>();

            string myFieldId = null;
            AWBSpecialHandlingCodeRepository myRepository = new AWBSpecialHandlingCodeRepository(Tenant);
            List<AWBSpecialHandlingCode> allCodes = myRepository.GetAWBHandlingCodes().ToList();

            myFieldId = Booking.AWBSpecialHandlingCodeId1;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode myCode = allCodes.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (myCode != null)
                {
                    systemField = myCode.Code;
                    xmlField = (systemField.Length <= itemLength) ? systemField : systemField.Substring(0, itemLength);
                    this.HandlingCodesList.Add(xmlField.ToUpper());
                }
            }

            myFieldId = Booking.AWBSpecialHandlingCodeId2;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode myCode = allCodes.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (myCode != null)
                {
                    systemField = myCode.Code;
                    xmlField = (systemField.Length <= itemLength) ? systemField : systemField.Substring(0, itemLength);
                    this.HandlingCodesList.Add(xmlField.ToUpper());
                }
            }

            myFieldId = Booking.AWBSpecialHandlingCodeId3;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode myCode = allCodes.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (myCode != null)
                {
                    systemField = myCode.Code;
                    xmlField = (systemField.Length <= itemLength) ? systemField : systemField.Substring(0, itemLength);
                    this.HandlingCodesList.Add(xmlField.ToUpper());
                }
            }

            myFieldId = Booking.AWBSpecialHandlingCodeId4;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode myCode = allCodes.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (myCode != null)
                {
                    systemField = myCode.Code;
                    xmlField = (systemField.Length <= itemLength) ? systemField : systemField.Substring(0, itemLength);
                    this.HandlingCodesList.Add(xmlField.ToUpper());
                }
            }

            myFieldId = Booking.AWBSpecialHandlingCodeId5;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode myCode = allCodes.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (myCode != null)
                {
                    systemField = myCode.Code;
                    xmlField = (systemField.Length <= itemLength) ? systemField : systemField.Substring(0, itemLength);
                    this.HandlingCodesList.Add(xmlField.ToUpper());
                }
            }

            myFieldId = Booking.AWBSpecialHandlingCodeId6;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode myCode = allCodes.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (myCode != null)
                {
                    systemField = myCode.Code;
                    xmlField = (systemField.Length <= itemLength) ? systemField : systemField.Substring(0, itemLength);
                    this.HandlingCodesList.Add(xmlField.ToUpper());
                }
            }

            myFieldId = Booking.AWBSpecialHandlingCodeId7;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode myCode = allCodes.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (myCode != null)
                {
                    systemField = myCode.Code;
                    xmlField = (systemField.Length <= itemLength) ? systemField : systemField.Substring(0, itemLength);
                    this.HandlingCodesList.Add(xmlField.ToUpper());
                }
            }

            myFieldId = Booking.AWBSpecialHandlingCodeId8;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode myCode = allCodes.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (myCode != null)
                {
                    systemField = myCode.Code;
                    xmlField = (systemField.Length <= itemLength) ? systemField : systemField.Substring(0, itemLength);
                    this.HandlingCodesList.Add(xmlField.ToUpper());
                }
            }

            //myFieldId = Booking.AWBSpecialHandlingCodeId9;
            //if (!string.IsNullOrEmpty(myFieldId))
            //{
            //    AWBSpecialHandlingCode myCode = allCodes.Where(d => d.Id == myFieldId).FirstOrDefault();
            //    if (myCode != null)
            //    {
            //        systemField = myCode.Code;
            //        xmlField = (systemField.Length <= itemLength) ? systemField : systemField.Substring(0, itemLength);
            //        this.HandlingCodesList.Add(xmlField.ToUpper());
            //    }
            //}

            myFieldId = Booking.BookingProductId;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                BookingProductRepository myBookingProductRepository = new BookingProductRepository(Tenant);
                BookingProduct myBookingProduct = myBookingProductRepository.GetSingle(myFieldId);
                if (myBookingProduct != null)
                {
                    systemField = myBookingProduct.Code;
                    xmlField = (systemField.Length <= itemLength) ? systemField : systemField.Substring(0, itemLength);
                    this.HandlingCodesList.Add(xmlField.ToUpper());
                }
            }
        }

        private void BuildSSRTextList()
        {
            this.SSRTextList = new List<string>();

            if (!string.IsNullOrEmpty(Booking.SpecialServicesRequest))
            {
                int itemLength = 65;
                string systemField = FormatHelper.FormatString(Booking.SpecialServicesRequest, FormatHelper.PatternType.Text);

                string xmlField = "";

                for (int i = 1; i <= 2; i++)
                {
                    systemField = systemField.Trim();

                    if (string.IsNullOrEmpty(systemField))
                    {
                        break;
                    }

                    else
                    {
                        if (systemField.Length <= itemLength)
                        {
                            xmlField = systemField.ToUpper();
                            systemField = "";
                        }

                        else
                        {
                            xmlField = systemField.Substring(0, itemLength).ToUpper();
                            systemField = systemField.Remove(0, itemLength);
                        }

                        xmlField = xmlField.Trim();
                        this.SSRTextList.Add(xmlField);
                    }
                }
            }
        }

        private void BuildOSILines()
        {
            if (!string.IsNullOrEmpty(Booking.OtherServicesInformation))
            {
                int itemLength = 65;
                string systemField = FormatHelper.FormatString(Booking.OtherServicesInformation, FormatHelper.PatternType.Text);
                
                if (systemField.Length <= itemLength)
                {
                    this.FirstLineOSI = systemField;
                }
                else
                {
                    this.FirstLineOSI = systemField.Substring(0, itemLength);
                    this.SecondLineOSI = systemField.Substring(65);
                }  
            }
        }

        private void BuildShipmentReferenceInfoData()
        {
            this.ReferenceNumber = FormatHelper.FormatString(Booking.BookingNumber, FormatHelper.PatternType.Text);
            //this.SupplementaryShipmentInformation1 = FormatHelper.FormatString(Booking.SupplementaryShipmentInformation1, FormatHelper.PatternType.Text);
            //this.SupplementaryShipmentInformation2 = FormatHelper.FormatString(Booking.SupplementaryShipmentInformation2, FormatHelper.PatternType.Text);

            this.IsReferenceInfoSpecified = false;
            this.SupplementaryTextList = new List<string>();

            if (!string.IsNullOrEmpty(ReferenceNumber))
            {
                this.IsReferenceInfoSpecified = true;
            }

            if (!string.IsNullOrEmpty(SupplementaryShipmentInformation1))
            {
                this.IsReferenceInfoSpecified = true;
                this.SupplementaryTextList.Add(SupplementaryShipmentInformation1);
            }

            if (!string.IsNullOrEmpty(SupplementaryShipmentInformation2))
            {
                this.IsReferenceInfoSpecified = true;
                this.SupplementaryTextList.Add(SupplementaryShipmentInformation2);
            }
        }
    }
}
