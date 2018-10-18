using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel;

namespace WebFreight.Web.WebServices
{
    public class WebServiceHelper
    {
        private int tenant;
        private PortRepository portRepository;
        private AddressRepository addressRepository;

        public WebServiceHelper(int tenant)
        {
            this.tenant = tenant;

            ICommonDataContext myCommonContext = CommonDataContext.GetContext(tenant);
            this.portRepository = new PortRepository(myCommonContext);
            this.addressRepository = new AddressRepository(myCommonContext);
        }

        public string GetPickUpDeliveryFromCityOrPortName(ShipmentPickUpDelivery entity)
        {
            string myResult = "";

            if (entity != null)
            {
                switch (entity.PickUpDeliveryFromTypeCode)
                {
                    case "PART":
                        {
                            if (!string.IsNullOrEmpty(entity.FromPartnerCardId))
                            {
                                Address myPartnerAddress = addressRepository.GetMainAddressByCardId(entity.FromPartnerCardId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    myResult = myPartnerAddress.City;
                                }
                            }

                            break;
                        }

                    case "PORT":
                        {
                            if (!string.IsNullOrEmpty(entity.FromPortId))
                            {
                                PortPM myPort = PortQuery.GetSinglePort(tenant, entity.FromPortId, true);
                                if (myPort != null)
                                {
                                    myResult = myPort.EnglishName;
                                }
                            }

                            break;
                        }

                    case "CASL":
                        {
                            myResult = entity.FromAddressCity;
                            break;
                        }
                }
            }

            if (myResult == null)
            {
                myResult = "";
            }

            return myResult;
        }
        public string GetPickUpDeliveryFromCityOrPortName(ShipmentPickUpPM entity)
        {
            string myResult = "";

            if (entity != null)
            {
                switch (entity.PickUpDeliveryFromTypeCode)
                {
                    case "PART":
                        {
                            if (!string.IsNullOrEmpty(entity.FromAddressId))
                            {
                                Address myPartnerAddress = addressRepository.GetSingleAddress(entity.FromAddressId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    myResult = myPartnerAddress.City;
                                }
                            }

                            break;
                        }

                    case "PORT":
                        {
                            if (!string.IsNullOrEmpty(entity.FromPortId))
                            {
                                PortPM myPort = PortQuery.GetSinglePort(tenant, entity.FromPortId, true);
                                if (myPort != null)
                                {
                                    myResult = myPort.EnglishName;
                                }
                            }

                            break;
                        }

                    case "CASL":
                        {
                            myResult = entity.FromAddressCity;
                            break;
                        }
                }
            }

            if (myResult == null)
            {
                myResult = "";
            }

            return myResult;
        }
        public string GetPlaceOfLoading(ShipmentPM shipment, ShipmentPickUpDelivery myPickup, string ShipmentPartner = null)
        {
            string myResult = "";

            if (myPickup != null)
            {
                switch (myPickup.PickUpDeliveryFromTypeCode)
                {
                    case "PART":
                        {
                            if (!string.IsNullOrEmpty(myPickup.FromPartnerCardId))
                            {
                                Address myPartnerAddress = addressRepository.GetMainAddressByCardId(myPickup.FromPartnerCardId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    myResult = myPartnerAddress.City;

                                    if (myPartnerAddress.State != null)
                                    {
                                        myResult = myResult + " , " + myPartnerAddress.State.Code;
                                    }
                                }
                            }

                            break;
                        }

                    case "PORT":
                        {
                            if (!string.IsNullOrEmpty(myPickup.FromPortId))
                            {
                                PortPM myPort = PortQuery.GetSinglePort(tenant, myPickup.FromPortId, true);
                                if (myPort != null)
                                {
                                    myResult = myPort.EnglishName;

                                    if (myPort.StateCode != null)
                                    {
                                        myResult = myResult + " , " + myPort.StateCode;
                                    }
                                }
                            }

                            break;
                        }

                    case "CASL":
                        {
                            myResult = myPickup.FromAddressCity;
                            break;
                        }
                }
            }

            else if (shipment.PreCarriageFromPortId != null && shipment.PreCarriageToPortId != null)
            {
                PortPM myPort = PortQuery.GetSinglePort(tenant, shipment.PreCarriageFromPortId, true);
                if (myPort != null)
                {
                    myResult = myPort.EnglishName;
                }
            }

            else if (shipment.DirectionId == "I")
            {
                PortPM myPort = PortQuery.GetSinglePort(tenant, shipment.MainCarriageFromPortId, true);
                if (myPort != null)
                {
                    myResult = myPort.EnglishName;
                }
            }

            else
            {
                switch (ShipmentPartner)
                {
                    case "Shipper":
                        {
                            if (shipment.ShipperAddressId != null)
                            {
                                Address myAddress = addressRepository.GetSingleAddress(shipment.ShipperAddressId, tenant);
                                if (myAddress != null)
                                {
                                    myResult = myAddress.City;
                                }
                            }

                            break;
                        }

                    default:
                        {
                            if (shipment.CustomerAddressId != null)
                            {
                                Address myAddress = addressRepository.GetSingleAddress(shipment.CustomerAddressId, tenant);
                                if (myAddress != null)
                                {
                                    myResult = myAddress.City;
                                }
                            }

                            break;
                        }
                }
            }

            return myResult;
        }
        public string GetPlaceOfLoading(ShipmentDataView shipment, ShipmentPickUpDelivery myPickup)
        {
            string myResult = "";

            if (myPickup != null)
            {
                switch (myPickup.PickUpDeliveryFromTypeCode)
                {
                    case "PART":
                        {
                            if (!string.IsNullOrEmpty(myPickup.FromPartnerCardId))
                            {
                                Address myPartnerAddress = addressRepository.GetMainAddressByCardId(myPickup.FromPartnerCardId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    myResult = myPartnerAddress.City;
                                }
                            }

                            break;
                        }

                    case "PORT":
                        {
                            if (!string.IsNullOrEmpty(myPickup.FromPortId))
                            {
                                PortPM myPort = PortQuery.GetSinglePort(tenant, myPickup.FromPortId, true);
                                if (myPort != null)
                                {
                                    myResult = myPort.EnglishName;
                                }
                            }

                            break;
                        }

                    case "CASL":
                        {
                            myResult = myPickup.FromAddressCity;
                            break;
                        }
                }
            }

            else if (shipment.PreCarriageFromPortId != null && shipment.PreCarriageToPortId != null)
            {
                PortPM myPort = PortQuery.GetSinglePort(tenant, shipment.PreCarriageFromPortId, true);
                if (myPort != null)
                {
                    myResult = myPort.EnglishName;
                }
            }

            else if (shipment.DirectionId == "I")
            {
                PortPM myPort = PortQuery.GetSinglePort(tenant, shipment.MainCarriageFromPortId, true);
                if (myPort != null)
                {
                    myResult = myPort.EnglishName;
                }
            }

            else if (shipment.CustomerAddressId != null)
            {
                Address myAddress = addressRepository.GetSingleAddress(shipment.CustomerAddressId, tenant);
                if (myAddress != null)
                {
                    myResult = myAddress.City;
                }
            }

            return myResult;
        }
        public string GetPlaceOfDelivery(ShipmentPM shipment, ShipmentPickUpDelivery myDelivery)
        {
            string myResult = "";

            if (myDelivery != null)
            {
                switch (myDelivery.PickUpDeliveryToTypeCode)
                {
                    case "PART":
                        {
                            if (!string.IsNullOrEmpty(myDelivery.ToPartnerCardId))
                            {
                                Address myPartnerAddress = addressRepository.GetMainAddressByCardId(myDelivery.ToPartnerCardId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    myResult = myPartnerAddress.City;

                                    if (myPartnerAddress.State != null)
                                    {
                                        myResult = myResult + " , " + myPartnerAddress.State.Code;
                                    }
                                }
                            }

                            break;
                        }

                    case "PORT":
                        {
                            if (!string.IsNullOrEmpty(myDelivery.ToPortId))
                            {
                                PortPM myPort = PortQuery.GetSinglePort(tenant, myDelivery.ToPortId, true);
                                if (myPort != null)
                                {
                                    myResult = myPort.EnglishName;

                                    if (myPort.StateCode != null)
                                    {
                                        myResult = myResult + " , " + myPort.StateCode;
                                    }
                                }
                            }

                            break;
                        }

                    case "CASL":
                        {
                            string myCity = myDelivery.ToAddressCity;
                            if (!string.IsNullOrEmpty(myCity))
                            {
                                myResult = myCity;
                            }

                            break;
                        }
                }
            }

            else if (shipment.OnCarriageFromPortId != null && shipment.OnCarriageToPortId != null)
            {
                PortPM myPort = PortQuery.GetSinglePort(tenant, shipment.OnCarriageToPortId, true);
                if (myPort != null)
                {
                    myResult = myPort.EnglishName;
                }
            }

            else
            {
                string myFinalPortName = "";

                if (shipment.Transshipment3ToPortId != null)
                {
                    myFinalPortName = shipment.Transshipment3ToPortName;
                }

                else if (shipment.Transshipment2ToPortId != null)
                {
                    myFinalPortName = shipment.Transshipment2ToPortName;
                }

                else if (shipment.Transshipment1ToPortId != null)
                {
                    myFinalPortName = shipment.Transshipment1ToPortName;
                }

                else if (shipment.MainCarriageToPortId != null)
                {
                    myFinalPortName = shipment.MainCarriageToPortName;
                }

                myResult = myFinalPortName;
            }

            return myResult;
        }
        public string GetPlaceOfDelivery(ShipmentDataView shipment, ShipmentPickUpDelivery myDelivery)
        {
            string myResult = "";

            if (myDelivery != null)
            {
                switch (myDelivery.PickUpDeliveryToTypeCode)
                {
                    case "PART":
                        {
                            if (!string.IsNullOrEmpty(myDelivery.ToPartnerCardId))
                            {
                                Address myPartnerAddress = addressRepository.GetMainAddressByCardId(myDelivery.ToPartnerCardId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    myResult = myPartnerAddress.City;
                                }
                            }

                            break;
                        }

                    case "PORT":
                        {
                            if (!string.IsNullOrEmpty(myDelivery.ToPortId))
                            {
                                PortPM myPort = PortQuery.GetSinglePort(tenant, myDelivery.ToPortId, true);
                                if (myPort != null)
                                {
                                    myResult = myPort.EnglishName;
                                }
                            }

                            break;
                        }

                    case "CASL":
                        {
                            string myCity = myDelivery.ToAddressCity;
                            if (!string.IsNullOrEmpty(myCity))
                            {
                                myResult = myCity;
                            }

                            break;
                        }
                }
            }

            else if (shipment.DirectionId == "D" && shipment.TransportModeId == "I")
            {
                if (!string.IsNullOrEmpty(shipment.MainCarriageToAddressId))
                {
                    Address myAddress = addressRepository.GetSingleAddress(shipment.MainCarriageToAddressId, tenant);
                    if (myAddress != null)
                    {
                        myResult = myAddress.City;
                    }
                }
            }

            else if (shipment.OnCarriageFromPortId != null && shipment.OnCarriageToPortId != null)
            {
                PortPM myPort = PortQuery.GetSinglePort(tenant, shipment.OnCarriageToPortId, true);
                if (myPort != null)
                {
                    myResult = myPort.EnglishName;
                }
            }

            else
            {
                string myFinalPortName = "";

                if (shipment.Transshipment3ToPortId != null)
                {
                    myFinalPortName = shipment.Transshipment3ToPortName;
                }

                else if (shipment.Transshipment2ToPortId != null)
                {
                    myFinalPortName = shipment.Transshipment2ToPortName;
                }

                else if (shipment.Transshipment1ToPortId != null)
                {
                    myFinalPortName = shipment.Transshipment1ToPortName;
                }

                else if (shipment.MainCarriageToPortId != null)
                {
                    myFinalPortName = shipment.MainCarriageToPortName;
                }

                myResult = myFinalPortName;
            }

            return myResult;
        }
        public string GetFromDeliveryName(ShipmentPM shipment, ShipmentPickUpDelivery myDelivery)
        {
            string myResult = "";

            if (myDelivery != null)
            {
                switch (myDelivery.PickUpDeliveryFromTypeCode)
                {
                    case "PART":
                        {
                            if (!string.IsNullOrEmpty(myDelivery.FromPartnerCardId))
                            {
                                Card myPartner = CardRepository.GetSingleCard(myDelivery.FromPartnerCardId, tenant, true);
                                if (myPartner != null)
                                {
                                    myResult = myPartner.EnglishName;
                                }
                            }

                            break;
                        }

                    case "PORT":
                        {
                            if (!string.IsNullOrEmpty(myDelivery.FromPortId))
                            {
                                PortPM myPort = PortQuery.GetSinglePort(tenant, myDelivery.FromPortId, true);
                                if (myPort != null)
                                {
                                    myResult = myPort.EnglishName;
                                }
                            }

                            break;
                        }

                    case "CASL":
                        {
                            string myCity = myDelivery.FromAddressCity;
                            if (!string.IsNullOrEmpty(myCity))
                            {
                                myResult = myCity;
                            }

                            break;
                        }
                }
            }

            return myResult;
        }
        public string GetFromDeliveryName(ShipmentDataView shipment, ShipmentPickUpDelivery myDelivery)
        {
            string myResult = "";

            if (myDelivery != null)
            {
                switch (myDelivery.PickUpDeliveryFromTypeCode)
                {
                    case "PART":
                        {
                            if (!string.IsNullOrEmpty(myDelivery.FromPartnerCardId))
                            {
                                Card myPartner = CardRepository.GetSingleCard(myDelivery.FromPartnerCardId, tenant, true);
                                if (myPartner != null)
                                {
                                    myResult = myPartner.EnglishName;
                                }
                            }

                            break;
                        }

                    case "PORT":
                        {
                            if (!string.IsNullOrEmpty(myDelivery.FromPortId))
                            {
                                PortPM myPort = PortQuery.GetSinglePort(tenant, myDelivery.FromPortId, true);
                                if (myPort != null)
                                {
                                    myResult = myPort.EnglishName;
                                }
                            }

                            break;
                        }

                    case "CASL":
                        {
                            string myCity = myDelivery.FromAddressCity;
                            if (!string.IsNullOrEmpty(myCity))
                            {
                                myResult = myCity;
                            }

                            break;
                        }
                }
            }

            return myResult;
        }
        public string GetToDeliveryName(ShipmentPM shipment, ShipmentPickUpDelivery myDelivery)
        {
            string myResult = "";

            if (myDelivery != null)
            {
                switch (myDelivery.PickUpDeliveryToTypeCode)
                {
                    case "PART":
                        {
                            if (!string.IsNullOrEmpty(myDelivery.ToPartnerCardId))
                            {
                                Card myPartner = CardRepository.GetSingleCard(myDelivery.ToPartnerCardId, tenant, true);
                                if (myPartner != null)
                                {
                                    myResult = myPartner.EnglishName;
                                }
                            }

                            break;
                        }

                    case "PORT":
                        {
                            if (!string.IsNullOrEmpty(myDelivery.ToPortId))
                            {
                                PortPM myPort = PortQuery.GetSinglePort(tenant, myDelivery.ToPortId, true);
                                if (myPort != null)
                                {
                                    myResult = myPort.EnglishName;
                                }
                            }

                            break;
                        }

                    case "CASL":
                        {
                            string myCity = myDelivery.ToAddressCity;
                            if (!string.IsNullOrEmpty(myCity))
                            {
                                myResult = myCity;
                            }

                            break;
                        }
                }
            }

            return myResult;
        }
        public string GetToDeliveryName(ShipmentDataView shipment, ShipmentPickUpDelivery myDelivery)
        {
            string myResult = "";

            if (myDelivery != null)
            {
                switch (myDelivery.PickUpDeliveryToTypeCode)
                {
                    case "PART":
                        {
                            if (!string.IsNullOrEmpty(myDelivery.ToPartnerCardId))
                            {
                                Card myPartner = CardRepository.GetSingleCard(myDelivery.ToPartnerCardId, tenant, true);
                                if (myPartner != null)
                                {
                                    myResult = myPartner.EnglishName;
                                }
                            }

                            break;
                        }

                    case "PORT":
                        {
                            if (!string.IsNullOrEmpty(myDelivery.ToPortId))
                            {
                                PortPM myPort = PortQuery.GetSinglePort(tenant, myDelivery.ToPortId, true);
                                if (myPort != null)
                                {
                                    myResult = myPort.EnglishName;
                                }
                            }

                            break;
                        }

                    case "CASL":
                        {
                            string myCity = myDelivery.ToAddressCity;
                            if (!string.IsNullOrEmpty(myCity))
                            {
                                myResult = myCity;
                            }

                            break;
                        }
                }
            }

            return myResult;
        }
        public int GetStringLinesCount(string myString)
        {
            int myResult = 0;

            if (myString != null)
            {
                myResult = 1;

                string[] strTemp = myString.Split(new string[] { "\n" }, StringSplitOptions.None);

                if (strTemp.Length > 0)
                {
                    myResult = strTemp.Length;
                }
            }

            return myResult;
        }
        public string GetFinalDestination(ShipmentPickUpDelivery myDelivery, ShipmentPM shipment)
        {
            string myFinalDestination = null;

            if (myDelivery != null)
            {
                switch (myDelivery.PickUpDeliveryToTypeCode)
                {
                    case "PART":
                        {
                            if (!string.IsNullOrEmpty(myDelivery.ToPartnerCardId))
                            {
                                Address myAddress = addressRepository.GetMainAddressByCardId(myDelivery.ToPartnerCardId, tenant);
                                if (myAddress != null)
                                {
                                    myFinalDestination = myAddress.City;
                                }
                            }

                            break;
                        }

                    case "PORT":
                        {
                            if (!string.IsNullOrEmpty(myDelivery.ToPortId))
                            {
                                Port myPort = portRepository.GetSinglePort(tenant, myDelivery.ToPortId);
                                if (myPort != null)
                                {
                                    myFinalDestination = myPort.EnglishName;
                                }
                            }

                            break;
                        }

                    case "CASL":
                        {
                            myFinalDestination = myDelivery.ToAddressCity;
                            break;
                        }
                }
            }

            else if (shipment != null)
            {
                if (shipment.OnCarriageToPortId != null)
                {
                    myFinalDestination = shipment.OnCarriageToPortName;
                }

                else if (shipment.Transshipment3ToPortId != null)
                {
                    myFinalDestination = shipment.Transshipment3ToPortName;
                }

                else if (shipment.Transshipment2ToPortId != null)
                {
                    myFinalDestination = shipment.Transshipment2ToPortName;
                }

                else if (shipment.Transshipment1ToPortId != null)
                {
                    myFinalDestination = shipment.Transshipment1ToPortName;
                }

                else if (shipment.MainCarriageToPortId != null)
                {
                    myFinalDestination = shipment.MainCarriageToPortName;
                }
            }

            return myFinalDestination;
        }
        public string GetDeliveryToCityOrPortName(ShipmentDeliveryPM entity)
        {
            string myResult = "";

            if (entity != null)
            {
                switch (entity.PickUpDeliveryToTypeCode)
                {
                    case "PART":
                        {
                            if (!string.IsNullOrEmpty(entity.ToAddressId))
                            {
                                Address myPartnerAddress = addressRepository.GetSingleAddress(entity.ToAddressId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    myResult = myPartnerAddress.City;
                                }
                            }

                            break;
                        }

                    case "PORT":
                        {
                            if (!string.IsNullOrEmpty(entity.ToPortId))
                            {
                                PortPM myPort = PortQuery.GetSinglePort(tenant, entity.ToPortId, true);
                                if (myPort != null)
                                {
                                    myResult = myPort.EnglishName;
                                }
                            }

                            break;
                        }

                    case "CASL":
                        {
                            myResult = entity.ToAddressCity;
                            break;
                        }
                }
            }

            if (myResult == null)
            {
                myResult = "";
            }

            return myResult;
        }

        public void GetPickUpFromAddress(ShipmentPickUpPM entity, DataProviders.PickUpDeliveryLine line, AddressRepository addressRepository, int tenant)
        {
            if (entity != null)
            {
                switch (entity.PickUpDeliveryFromTypeCode)
                {
                    case "PART":
                        {
                            if (!string.IsNullOrEmpty(entity.FromPartnerCardId))
                            {
                                Card myPartner = CardRepository.GetSingleCard(entity.FromPartnerCardId, tenant, true);
                                if(myPartner != null)
                                {
                                    line.FullAddress = myPartner.EnglishName;
                                }
                            }                            

                            if (!string.IsNullOrEmpty(entity.FromAddressId))
                            {
                                Address myPartnerAddress = addressRepository.GetSingleAddress(entity.FromAddressId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    line.Address = myPartnerAddress.City;

                                    line.FullAddress = line.FullAddress + Environment.NewLine + DataProviders.General.GetAddress(myPartnerAddress);

                                    if (myPartnerAddress.PhoneNumber != null)
                                    {
                                        line.FullAddress = line.FullAddress + Environment.NewLine + "Tel: " + myPartnerAddress.PhoneNumber;
                                    }
                                }
                            }

                            break;
                        }

                    case "PORT":
                        {
                            if (!string.IsNullOrEmpty(entity.FromPortId))
                            {
                                PortPM myPort = PortQuery.GetSinglePort(tenant, entity.FromPortId, true);
                                if (myPort != null)
                                {
                                    line.Address = myPort.EnglishName;
                                    line.FullAddress = myPort.EnglishName + ", " + myPort.CountryName;
                                        
                                    if(!string.IsNullOrEmpty(myPort.StateId))
                                    {
                                        StateRepository stateRepository = new StateRepository(tenant);
                                        State myState = stateRepository.GetSingleState(myPort.StateId, tenant);

                                        if(myState != null)
                                        {
                                            line.FullAddress  = line.FullAddress  + ", State: " + myState.EnglishName;
                                        }
                                    }                                        
                                }
                            }

                            break;
                        }

                    case "CASL":
                        {
                            line.Address = entity.FromAddressCity;
                            line.FullAddress = entity.FromAddressCountryName + ", " + entity.FromAddressCity;

                            if (!string.IsNullOrEmpty(entity.FromAddressZipCode))
                            {
                                line.FullAddress = line.FullAddress + ", " + entity.FromAddressZipCode;
                            }

                            break;
                        }
                }
            }
        }

        public void GetDeliveryToAddress(ShipmentDeliveryPM entity, DataProviders.PickUpDeliveryLine line, AddressRepository addressRepository, int tenant)
        {
            if (entity != null)
            {
                switch (entity.PickUpDeliveryToTypeCode)
                {
                    case "PART":
                        {
                            if (!string.IsNullOrEmpty(entity.ToPartnerCardId))
                            {
                                Card myPartner = CardRepository.GetSingleCard(entity.ToPartnerCardId, tenant, true);
                                if (myPartner != null)
                                {
                                    line.FullAddress = myPartner.EnglishName;
                                }
                            }

                            if (!string.IsNullOrEmpty(entity.ToAddressId))
                            {
                                Address myPartnerAddress = addressRepository.GetSingleAddress(entity.ToAddressId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    line.Address = myPartnerAddress.City;
                                    line.FullAddress = line.FullAddress + Environment.NewLine + DataProviders.General.GetAddress(myPartnerAddress);

                                    if (myPartnerAddress.PhoneNumber != null)
                                    {
                                        line.FullAddress = line.FullAddress + Environment.NewLine + "Tel: " + myPartnerAddress.PhoneNumber;
                                    }
                                }
                            }

                            break;
                        }

                    case "PORT":
                        {
                            if (!string.IsNullOrEmpty(entity.ToPortId))
                            {
                                PortPM myPort = PortQuery.GetSinglePort(tenant, entity.ToPortId, true);
                                if (myPort != null)
                                {
                                    line.Address = myPort.EnglishName;
                                    line.FullAddress = myPort.EnglishName + ", " + myPort.CountryName;

                                    if (!string.IsNullOrEmpty(myPort.StateId))
                                    {
                                        StateRepository stateRepository = new StateRepository(tenant);
                                        State myState = stateRepository.GetSingleState(myPort.StateId, tenant);

                                        if (myState != null)
                                        {
                                            line.FullAddress = line.FullAddress + ", State: " + myState.EnglishName;
                                        }
                                    }
                                }
                            }

                            break;
                        }

                    case "CASL":
                        {
                            line.Address = entity.ToAddressCity;
                            line.FullAddress = entity.ToAddressCountryName + ", " + entity.ToAddressCity;

                            if (!string.IsNullOrEmpty(entity.ToAddressZipCode))
                            {
                                line.FullAddress = line.FullAddress + ", " + entity.ToAddressZipCode;
                            }

                            break;
                        }
                }
            }
        }

        public PlaceOfReceiptData GetPlaceOfReceiptData(ShipmentPickUpDelivery entity)
        {
            PlaceOfReceiptData myResult = new PlaceOfReceiptData();

            if (entity != null)
            {
                switch (entity.PickUpDeliveryFromTypeCode)
                {
                    case "PART":
                        {
                            if (!string.IsNullOrEmpty(entity.FromPartnerCardId))
                            {
                                Address myPartnerAddress = addressRepository.GetMainAddressByCardId(entity.FromPartnerCardId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    myResult.City = myPartnerAddress.City;
                                    myResult.CountryCode = myPartnerAddress.Country == null ? "" : myPartnerAddress.Country.Code;
                                    myResult.CountryName = myPartnerAddress.Country == null ? "" : myPartnerAddress.Country.EnglishName;
                                    myResult.StateCode = myPartnerAddress.State == null ? "" : myPartnerAddress.State.Code;
                                }
                            }

                            break;
                        }

                    case "PORT":
                        {
                            if (!string.IsNullOrEmpty(entity.FromPortId))
                            {
                                PortPM myPort = PortQuery.GetSinglePort(tenant, entity.FromPortId, true);
                                if (myPort != null)
                                {
                                    myResult.City = myPort.EnglishName;
                                    myResult.CountryCode = myPort.CountryCode;
                                    myResult.CountryName = myPort.CountryName;
                                    myResult.StateCode = myPort.StateCode;
                                }
                            }

                            break;
                        }

                    case "CASL":
                        {
                            myResult.City = entity.FromAddressCity;
                            CountryRepository countryRepository = new CountryRepository(tenant);
                            Country country = countryRepository.GetSingleCountry(entity.FromAddressCountryId, tenant);
                            if (country != null)
                            {
                                myResult.CountryCode = country.Code;
                                myResult.CountryName = country.EnglishName;
                            }
                            break;
                        }
                }
            }

            if (myResult == null)
            {
                myResult.City = "";
                myResult.CountryCode = "";
                myResult.CountryName = "";
                myResult.StateCode = "";
            }

            return myResult;
        }

        public string GetPickUpAddress(ShipmentPickUpDelivery myPickup)
        {
            string myResult = "";

            switch (myPickup.PickUpDeliveryFromTypeCode)
            {
                case "PART":
                    {
                        if (!string.IsNullOrEmpty(myPickup.FromPartnerCardId))
                        {
                            Address myPartnerAddress = addressRepository.GetMainAddressByCardId(myPickup.FromPartnerCardId, tenant);
                            if (myPartnerAddress != null)
                            {
                                myResult = myPartnerAddress.City;

                                if (myPartnerAddress.State != null)
                                {
                                    myResult = myResult + " , " + myPartnerAddress.State.Code;
                                }
                            }
                        }

                        break;
                    }

                case "PORT":
                    {
                        if (!string.IsNullOrEmpty(myPickup.FromPortId))
                        {
                            PortPM myPort = PortQuery.GetSinglePort(tenant, myPickup.FromPortId, true);
                            if (myPort != null)
                            {
                                myResult = myPort.EnglishName;

                                if (myPort.StateCode != null)
                                {
                                    myResult = myResult + " , " + myPort.StateCode;
                                }
                            }
                        }

                        break;
                    }

                case "CASL":
                    {
                        myResult = myPickup.FromAddressCity;
                        break;
                    }
            }

            return myResult;
        }
        public string GetDeliveryAddress(ShipmentPickUpDelivery myDelivery)
        {
            string myResult = "";

            switch (myDelivery.PickUpDeliveryToTypeCode)
            {
                case "PART":
                    {
                        if (!string.IsNullOrEmpty(myDelivery.ToPartnerCardId))
                        {
                            Address myPartnerAddress = addressRepository.GetMainAddressByCardId(myDelivery.ToPartnerCardId, tenant);
                            if (myPartnerAddress != null)
                            {
                                myResult = myPartnerAddress.City;

                                if (myPartnerAddress.State != null)
                                {
                                    myResult = myResult + " , " + myPartnerAddress.State.Code;
                                }
                            }
                        }

                        break;
                    }

                case "PORT":
                    {
                        if (!string.IsNullOrEmpty(myDelivery.ToPortId))
                        {
                            PortPM myPort = PortQuery.GetSinglePort(tenant, myDelivery.ToPortId, true);
                            if (myPort != null)
                            {
                                myResult = myPort.EnglishName;

                                if (myPort.StateCode != null)
                                {
                                    myResult = myResult + " , " + myPort.StateCode;
                                }
                            }
                        }

                        break;
                    }

                case "CASL":
                    {
                        myResult = myDelivery.ToAddressCity;
                        break;
                    }
            }

            return myResult;
        }
    }

    public class PlaceOfReceiptData
    {
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string StateCode { get; set; }
    }
}