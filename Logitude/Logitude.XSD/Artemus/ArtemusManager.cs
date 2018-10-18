using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.BlobServiceReference;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Logitude.XSD.Artemus
{
    public class ArtemusManager
    {
        private int tenant;
        private string shipmentId;
        private string communicationSubject;
        private TenantRepository tenantRepository;
        private Tenant tenantPOCO;
        private StateRepository stateRepository;
        private PortRepository portRepository;
        ComputingPartnerTranslationHelper computingPartnerHelper;
        ShipmentQuery shipmentQuery;
        ShipmentPM shipmentPM;
        public ArtemusManager(string shipmentId, int tenant, string communicationSubject)
        {
            this.tenant = tenant;
            this.shipmentId = shipmentId;
            shipmentQuery = new ShipmentQuery(tenant);
            shipmentPM = shipmentQuery.GetSinglePM(shipmentId, tenant);
            this.communicationSubject = communicationSubject;
            tenantRepository = new TenantRepository(tenant);
            tenantPOCO = tenantRepository.GetSingleByTenant(tenant);
            stateRepository = new StateRepository(tenant);
            portRepository = new PortRepository(tenant);
            computingPartnerHelper = new ComputingPartnerTranslationHelper(tenant);
        }

        public void SendAMS_Voyage()
        {
            var errors = "";
            ArtemusEDIVoyageRoot log = new ArtemusEDIVoyageRoot();
            log.Locations = new List<ArtemusLocationElement>();
            log.Voyage = new ArtemusVoyageElement();
            log.Voyage.PortCalls = new List<ArtemusVoyagePortCallElement>();

            ShippingLineRepository shippingLineRepository = new ShippingLineRepository(tenant);
            var shippingLine = shippingLineRepository.GetSingleShippingLine(shipmentPM.MainCarriageCarrierId, tenant);

            if (tenantPOCO != null && !string.IsNullOrEmpty(tenantPOCO.SCACCode))
            {
                log.scac = tenantPOCO.SCACCode;
            }
            else
            {
                errors += "System Defaults SCAC Code is missing;";
            }

            log.version = "1.2";

            #region Location 
            var index = 1;
            ArtemusLocationElement location = new ArtemusLocationElement();
            location.index = index;
            location.name = shipmentPM.MainCarriageFromPortName;
            if (string.IsNullOrEmpty(location.name))
            {
                errors += "Main Carriage location name is missing;";
            }
            if (string.IsNullOrEmpty(shipmentPM.MainCarriageFromPortCountryCode))
            {
                errors += "Main Carriage location country is missing;";
            }
            var port = portRepository.GetSinglePort(shipmentPM.Tenant, shipmentPM.MainCarriageFromPortId);
            location.providence = port != null ? port.State != null ? port.State.Code : "" : "";
            var unCode = port != null ? port.CombinedCode : "";
            location.unCode = unCode  == null ? "" : unCode;
            location.type = shipmentPM.TransportModeId == "O" ? "marine" : shipmentPM.TransportModeId == "I" ? "inland" : "";
            //var country = computingPartnerHelper.GetComputingPartnerCodeTranslation(shipmentPM.MainCarriageFromPortCountryCode, "Artemus", "Country");
            //if (string.IsNullOrEmpty(country))
            //{
                var country = shipmentPM.MainCarriageFromPortCountryCode;
            //}
            location.country = country;
            //var customsCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(unCode, "Artemus", "Port");
            //if (string.IsNullOrEmpty(customsCode))
            //{
            //    customsCode = "";
            //}
            //location.customsCode = customsCode;
            log.Locations.Add(location);

            if (shipmentPM.Transshipment1FromPortId != null)
            {
                location = new ArtemusLocationElement();
                index = index + 1;
                location.index = index;
                location.name = shipmentPM.Transshipment1FromPortName;
                if (string.IsNullOrEmpty(location.name))
                {
                    errors += "Transshipment1 location name is missing;";
                }
                if (string.IsNullOrEmpty(shipmentPM.Transshipment1FromPortCountryCode))
                {
                    errors += "Transshipment1 location country is missing;";
                }
                port = portRepository.GetSinglePort(shipmentPM.Tenant, shipmentPM.Transshipment1FromPortId);
                location.providence = port != null ? port.State != null ? port.State.Code : "" : "";
                unCode = port != null ? port.CombinedCode : "";
                location.unCode = unCode == null ? "" : unCode;
                location.type = shipmentPM.TransportModeId == "O" ? "marine" : shipmentPM.TransportModeId == "I" ? "inland" : "";
                //country = computingPartnerHelper.GetComputingPartnerCodeTranslation(shipmentPM.Transshipment1FromPortCountryCode, "G-Artemus", "Country");
                //if (string.IsNullOrEmpty(country))
                //{
                    country = shipmentPM.Transshipment1FromPortCountryCode;
                //}
                location.country = country;
                //customsCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(unCode, "Artemus", "Port");
                //if (string.IsNullOrEmpty(customsCode))
                //{
                //    customsCode = "";
                //}
                //location.customsCode = customsCode;
                log.Locations.Add(location);
            }

            if (shipmentPM.Transshipment2FromPortId != null)
            {
                location = new ArtemusLocationElement();
                index = index + 1;
                location.index = index;
                location.name = shipmentPM.Transshipment2FromPortName;
                if (string.IsNullOrEmpty(location.name))
                {
                    errors += "Transshipment2 location name is missing;";
                }
                if (string.IsNullOrEmpty(shipmentPM.Transshipment2FromPortCountryCode))
                {
                    errors += "Transshipment2 location country is missing;";
                }
                port = portRepository.GetSinglePort(shipmentPM.Tenant, shipmentPM.Transshipment2FromPortId);
                location.providence = port != null ? port.State != null ? port.State.Code : "" : "";
                unCode = port != null ? port.CombinedCode : "";
                location.unCode = unCode == null ? "" : unCode;
                location.type = shipmentPM.TransportModeId == "O" ? "marine" : shipmentPM.TransportModeId == "I" ? "inland" : "";
                //country = computingPartnerHelper.GetComputingPartnerCodeTranslation(shipmentPM.Transshipment2FromPortCountryCode, "G-Artemus", "Country");
                //if (string.IsNullOrEmpty(country))
                //{
                    country = shipmentPM.Transshipment2FromPortCountryCode;
                //}
                location.country = country;
                //customsCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(unCode, "Artemus", "Port");
                //if (string.IsNullOrEmpty(customsCode))
                //{
                //    customsCode = "";
                //}
                //location.customsCode = customsCode;
                log.Locations.Add(location);
            }

            if (shipmentPM.Transshipment3FromPortId != null)
            {
                location = new ArtemusLocationElement();
                index = index + 1;
                location.index = index;
                location.name = shipmentPM.Transshipment3FromPortName;
                if (string.IsNullOrEmpty(location.name))
                {
                    errors += "Transshipment3 location name is missing;";
                }
                if (string.IsNullOrEmpty(shipmentPM.Transshipment3FromPortCountryCode))
                {
                    errors += "Transshipment3 location country is missing;";
                }
                port = portRepository.GetSinglePort(shipmentPM.Tenant, shipmentPM.Transshipment3FromPortId);
                location.providence = port != null ? port.State != null ? port.State.Code : "" : "";
                unCode = port != null ? port.CombinedCode : "";
                location.unCode = unCode == null ? "" : unCode;
                location.type = shipmentPM.TransportModeId == "O" ? "marine" : shipmentPM.TransportModeId == "I" ? "inland" : "";
                //country = computingPartnerHelper.GetComputingPartnerCodeTranslation(shipmentPM.Transshipment3FromPortCountryCode, "Artemus", "Country");
                //if (string.IsNullOrEmpty(country))
                //{
                    country = shipmentPM.Transshipment3FromPortCountryCode;
                //}
                location.country = country;
                //customsCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(unCode, "Artemus", "Port");
                //if (string.IsNullOrEmpty(customsCode))
                //{
                //    customsCode = "";
                //}
                //location.customsCode = customsCode;
                log.Locations.Add(location);
            }

            if (shipmentPM.MainCarriageFinalDestinationPortId != null)
            {
                location = new ArtemusLocationElement();
                index = index + 1;
                location.index = index;
                location.name = shipmentPM.MainCarriageFinalDestinationPortName;
                if (string.IsNullOrEmpty(location.name))
                {
                    errors += "Final Destination location name is missing;";
                }
                if (string.IsNullOrEmpty(shipmentPM.MainCarriageFinalDestinationPortCountryCode))
                {
                    errors += "Final Destination location country is missing;";
                }
                port = portRepository.GetSinglePort(shipmentPM.Tenant, shipmentPM.MainCarriageFinalDestinationPortId);
                location.providence = port != null ? port.State != null ? port.State.Code : "" : "";
                unCode = port != null ? port.CombinedCode : "";
                location.unCode = unCode == null ? "" : unCode;
                location.type = shipmentPM.TransportModeId == "O" ? "marine" : shipmentPM.TransportModeId == "I" ? "inland" : "";
                //country = computingPartnerHelper.GetComputingPartnerCodeTranslation(shipmentPM.MainCarriageFinalDestinationPortCountryCode, "G-Artemus", "Country");
                //if (string.IsNullOrEmpty(country))
                //{
                    country = shipmentPM.MainCarriageFinalDestinationPortCountryCode;
                //}
                location.country = country;
                //customsCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(unCode, "Artemus", "Port");
                //if (string.IsNullOrEmpty(customsCode))
                //{
                //    customsCode = "";
                //}
                //location.customsCode = customsCode;
                log.Locations.Add(location);
            }
            #endregion

            #region  Voyage
            log.Voyage.number = shipmentPM.MainCarriageCarrierNumber;
            if (string.IsNullOrEmpty(log.Voyage.number))
            {
                errors += "Main Carriage Voyage number is missing;";
            }
            VesselRepository vesselRep = new VesselRepository(tenant);
            var vessel = vesselRep.GetSingleVessel(shipmentPM.MainCarriageVesselId, tenant);
            if (vessel == null)
            {
                errors += "Main Carriage Vessel field is missing;";
            }
            else
            {
                if (string.IsNullOrEmpty(vessel.EnglishName))
                {
                    errors += "Main Carriage Vessel field is missing;";
                }
                else
                {
                    log.Voyage.vessel = vessel.EnglishName;
                }
            }

            log.Voyage.scac = shippingLine != null ? shippingLine.SCACCode : "";

            // Main Carriage
            var portindex = 1;
            ArtemusVoyagePortCallElement portCall = new ArtemusVoyagePortCallElement();
            portCall.locationIndex = portindex;

            if (shipmentPM.MainCarriageATD == null && shipmentPM.MainCarriageETD == null)
            {
                errors += "Main Carriage ATD or Main Carriage ETD is missing;";
            }
            else
            {
                portCall.arriveDate = shipmentPM.MainCarriageATD != null ? shipmentPM.MainCarriageATD.Value : shipmentPM.MainCarriageETD.Value;
                portCall.sailDate = shipmentPM.MainCarriageATD != null ? shipmentPM.MainCarriageATD.Value : shipmentPM.MainCarriageETD.Value;
            }
            portCall.load = true;
            portCall.discharge = false;
            if (shipmentPM.Transshipment1FromPortId == null && shipmentPM.Transshipment2FromPortId == null && shipmentPM.Transshipment3FromPortId == null)
            {
                portCall.lastLoad = true;
            }
            else
            {
                portCall.lastLoad =  false;
            }
            
            log.Voyage.PortCalls.Add(portCall);

            //Transshipment1
            if (shipmentPM.Transshipment1FromPortId != null)
            {
                portCall = new ArtemusVoyagePortCallElement();
                portindex = portindex + 1;
                portCall.locationIndex = portindex;
                if (shipmentPM.MainCarriageATA == null && shipmentPM.MainCarriageETA == null)
                {
                    errors += "Main Carriage ATA or Main Carriage ETA is missing;";
                }
                else
                {
                    portCall.arriveDate = shipmentPM.MainCarriageATA != null ? shipmentPM.MainCarriageATA.Value : shipmentPM.MainCarriageETA.Value;
                }
                if (shipmentPM.Transshipment1ATD == null && shipmentPM.Transshipment1ETD == null)
                {
                    errors += "Transshipment1 ATD or Transshipment1 ETD is missing;";
                }
                else
                {
                    portCall.sailDate = shipmentPM.Transshipment1ATD != null ? shipmentPM.Transshipment1ATD.Value : shipmentPM.Transshipment1ETD.Value;
                }
                portCall.load =  true;
                portCall.discharge = true;
                portCall.lastLoad = shipmentPM.Transshipment2FromPortId == null ? true : false;
                log.Voyage.PortCalls.Add(portCall);
            }

            // Transshipment2
            if (shipmentPM.Transshipment2FromPortId != null)
            {
                portCall = new ArtemusVoyagePortCallElement();
                portindex = portindex + 1;
                portCall.locationIndex = portindex;
                if (shipmentPM.Transshipment1ATA == null && shipmentPM.Transshipment1ETA == null)
                {
                    errors += "Transshipment1 ATA or Transshipment1 ETA is missing;";
                }
                else
                {
                    portCall.arriveDate = shipmentPM.Transshipment1ATA != null ? shipmentPM.Transshipment1ATA.Value : shipmentPM.Transshipment1ETA.Value;
                }
                if (shipmentPM.Transshipment2ATD == null && shipmentPM.Transshipment2ETD == null)
                {
                    errors += "Transshipment2 ATD or Transshipment2 ETD is missing;";
                }
                else
                {
                    portCall.sailDate = shipmentPM.Transshipment2ATD != null ? shipmentPM.Transshipment2ATD.Value : shipmentPM.Transshipment2ETD.Value;
                }
                portCall.load = true;
                portCall.discharge = true;
                portCall.lastLoad = shipmentPM.Transshipment3FromPortId == null ? true : false;
                log.Voyage.PortCalls.Add(portCall);
            }

            //Transshipment3
            if (shipmentPM.Transshipment3FromPortId != null)
            {
                portCall = new ArtemusVoyagePortCallElement();
                portindex = portindex + 1;
                portCall.locationIndex = portindex;
                if (shipmentPM.Transshipment2ATA == null && shipmentPM.Transshipment2ETA == null)
                {
                    errors += "Transshipment2 ATA or Transshipment2 ETA is missing;" ;
                }
                else
                {
                    portCall.arriveDate = shipmentPM.Transshipment2ATA != null ? shipmentPM.Transshipment2ATA.Value : shipmentPM.Transshipment2ETA.Value;
                }
                if (shipmentPM.Transshipment3ATD == null && shipmentPM.Transshipment3ETD == null)
                {
                    errors += "Transshipment3 ATD or Transshipment3 ETD is missing;";
                }
                else
                {
                    portCall.sailDate = shipmentPM.Transshipment3ATD != null ? shipmentPM.Transshipment3ATD.Value : shipmentPM.Transshipment3ETD.Value;
                }
                portCall.load = true;
                portCall.discharge = true;
                portCall.lastLoad =  true;
                log.Voyage.PortCalls.Add(portCall);
            }

            // Destination Port
            if (shipmentPM.MainCarriageFinalDestinationPortId != null)
            {
                portCall = new ArtemusVoyagePortCallElement();
                portindex = portindex + 1;
                portCall.locationIndex = portindex;

                if (shipmentPM.Transshipment1FromPortId == null)
                {
                    if (shipmentPM.MainCarriageATA == null && shipmentPM.MainCarriageETA == null)
                    {
                        errors += "MainCarriage ATA or MainCarriage ETA is missing;";
                    }
                }
                else if (shipmentPM.Transshipment2FromPortId == null)
                {
                    if (shipmentPM.Transshipment1ATA == null && shipmentPM.Transshipment1ETA == null)
                    {
                        errors += "Transshipment1 ATA or Transshipment1 ETA is missing;";
                    }
                }
                else if (shipmentPM.Transshipment3FromPortId == null)
                {
                    if (shipmentPM.Transshipment2ATA == null && shipmentPM.Transshipment2ETA == null)
                    {
                        errors += "Transshipment2 ATA or Transshipment2 ETA is missing;";
                    }
                }
                else
                {
                    if (shipmentPM.Transshipment3ATA == null && shipmentPM.Transshipment3ETA == null)
                    {
                        errors += "Transshipment3 ATA or Transshipment3 ETA is missing;";
                    }
                }
                if(shipmentPM.FinalArrivalDate!= null)
                {
                    portCall.arriveDate = shipmentPM.FinalArrivalDate.Value;
                    portCall.sailDate = shipmentPM.FinalArrivalDate.Value;
                }
                   
                portCall.load = false;
                portCall.discharge = true;
                portCall.lastLoad = false;
                log.Voyage.PortCalls.Add(portCall);
            }
            #endregion 

            if (!string.IsNullOrEmpty(errors))
            {
                errors = errors.TrimEnd(';');
                throw new ApplicationException(errors);
            }
            else
            {
                this.BuildXMLFile(log, tenant, shipmentPM.Id, shipmentPM.ShipmentNumber);
            }
        }
        public void SendAMS_Bill()
        {
            var errors = "";
            ArtemusEDIBillRoot log = new ArtemusEDIBillRoot();
            log.Partys = new List<ArtemusPartyElement>();
            log.Locations = new List<ArtemusLocationElement>();
            log.BL = new ArtemusBLElement();
            log.BL.PartyRefs = new List<ArtemusPartyRefElement>();
            log.BL.locationRefs = new List<ArtemusLocationRefElement>();
            log.BL.AMSNotifys = new List<ArtemusAMSNotifyElement>();
            log.BL.Equipments = new List<ArtemusEquipmentElement>();
            log.BL.Packages = new List<ArtemusPackageElement>();
            log.BL.Cargos = new List<ArtemusCargoElement>();

            ShippingLineRepository shippingLineRepository = new ShippingLineRepository(tenant);
            var shippingLine = shippingLineRepository.GetSingleShippingLine(shipmentPM.MainCarriageCarrierId, tenant);

            if(shippingLine != null)
            {
                log.BL.masterBilllScac = shippingLine.SCACCode;
            }
            if (shipmentPM.ShipmentLevelCode == "H" && shipmentPM.MasterShipmentDataId == null)
            {
                errors += "You can't send BOL message with unconnected houses;";
            }

            else
            {
                if (tenantPOCO != null && !string.IsNullOrEmpty(tenantPOCO.SCACCode))
                {
                    log.scac = tenantPOCO.SCACCode;
                    log.BL.hblSCAC = tenantPOCO.SCACCode;
                }
                else
                {
                    errors += "System Defaults SCAC Code is missing;";
                }

                log.version = "1.2";

                #region Party 
                if (shipmentPM.ShipperId == null)
                {
                    errors += "Shipper Partner is missing;";
                }
                if (shipmentPM.ConsigneeId == null)
                {
                    errors += "Consignee Partner is missing;";
                }
                ArtemusPartyElement party = new ArtemusPartyElement();
                if (shipmentPM.ShipperId != null)
                {
                    party.index = 1;
                    party.name = shipmentPM.ShipperName;
                    party.address1 = shipmentPM.ShipperAddress1;
                    party.address2 = shipmentPM.ShipperAddress2;
                    party.postcode = shipmentPM.ShipperZipCode;
                    party.city = shipmentPM.ShipperCity;
                    party.country = computingPartnerHelper.GetComputingPartnerCodeTranslation(shipmentPM.ShipperAddressCountryCode, "G-Artemus", "Country");
                    var state = stateRepository.GetSingleState(shipmentPM.ShipperStateId, shipmentPM.Tenant);
                    party.providence = state != null ? state.Code : "";
                    log.Partys.Add(party);
                }

                if (shipmentPM.ConsigneeId != null)
                {
                    party = new ArtemusPartyElement();
                    party.index = 2;
                    if (shipmentPM.ShipperId == null) party.index = 1;
                    party.name = shipmentPM.ConsigneeName;
                    party.address1 = shipmentPM.ConsigneeAddress1;
                    party.address2 = shipmentPM.ConsigneeAddress2;
                    party.postcode = shipmentPM.ConsigneeZipCode;
                    party.city = shipmentPM.ConsigneeCity;
                    party.country = computingPartnerHelper.GetComputingPartnerCodeTranslation(shipmentPM.ConsigneeAddressCountryCode, "G-Artemus", "Country");
                    var state = stateRepository.GetSingleState(shipmentPM.ConsigneeStateId, shipmentPM.Tenant);
                    party.providence = state != null ? state.Code : "";
                    log.Partys.Add(party);
                }

                if (shipmentPM.Notify1Id != null || shipmentPM.Notify2Id != null)
                {
                    party = new ArtemusPartyElement();
                    party.index = 3;
                    party.name = shipmentPM.Notify1Name != null ? shipmentPM.Notify1Name : shipmentPM.Notify2Name;
                    party.address1 = shipmentPM.Notify1Address1 != null ? shipmentPM.Notify1Address1 : shipmentPM.Notify2Address1;
                    party.address2 = shipmentPM.Notify1Address2 != null ? shipmentPM.Notify1Address2 : shipmentPM.Notify2Address2;
                    party.postcode = shipmentPM.Notify1ZipCode != null ? shipmentPM.Notify1ZipCode : shipmentPM.Notify2ZipCode;
                    party.city = shipmentPM.Notify1City != null ? shipmentPM.Notify1City : shipmentPM.Notify2City;

                    var countryCode = shipmentPM.Notify1AddressCountryCode != null ? shipmentPM.Notify1AddressCountryCode : shipmentPM.Notify2AddressCountryCode;
                    party.country = computingPartnerHelper.GetComputingPartnerCodeTranslation(countryCode, "G-Artemus", "Country");

                    var stateId = shipmentPM.Notify1StateId != null ? shipmentPM.Notify1StateId : shipmentPM.Notify2StateId;
                    var state = stateRepository.GetSingleState(stateId, shipmentPM.Tenant);

                    party.providence = state != null ? state.Code : "";
                    log.Partys.Add(party);
                }


                #endregion

                #region Location 
                var index = 1;
                Port port = null;
                var unCode = "";
                var country = "";
                var customsCode = "";

                ArtemusLocationElement location = new ArtemusLocationElement();

                if (!string.IsNullOrEmpty(shipmentPM.PreCarriageFromPortId))
                {
                    location = new ArtemusLocationElement();
                    location.index = index;
                    location.name = shipmentPM.PreCarriageFromPortName;
                    port = portRepository.GetSinglePort(shipmentPM.Tenant, shipmentPM.PreCarriageFromPortId);
                    location.providence = port != null ? port.State != null ? port.State.Code : "" : "";
                    unCode = port != null ? port.CombinedCode : "";
                    location.unCode = unCode == null ? "" : unCode;
                    location.type = shipmentPM.TransportModeId == "O" ? "marine" : shipmentPM.TransportModeId == "I" ? "inland" : "";
                    //country = computingPartnerHelper.GetComputingPartnerCodeTranslation(shipmentPM.PreCarriageFromPortCountryCode, "Artemus", "Country");
                    //if (string.IsNullOrEmpty(country))
                    //{
                    country = shipmentPM.PreCarriageFromPortCountryCode;
                    //}
                    location.country = country;
                    //customsCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(unCode, "Artemus", "Port");
                    //if (string.IsNullOrEmpty(customsCode))
                    //{
                    //    customsCode = "";
                    //}
                    //location.customsCode = customsCode;
                    log.Locations.Add(location);
                }

                if (shipmentPM.MainCarriageFromPortId != null)
                {
                    location = new ArtemusLocationElement();
                    if (!string.IsNullOrEmpty(shipmentPM.PreCarriageFromPortId))
                    {
                        index = index + 1;
                    }
                    location.index = index;
                    location.name = shipmentPM.MainCarriageFromPortName;
                    if (string.IsNullOrEmpty(location.name))
                    {
                        errors += "Main Carriage location name is missing;";
                    }
                    if (string.IsNullOrEmpty(shipmentPM.MainCarriageFromPortCountryCode))
                    {
                        errors += "Main Carriage location country is missing;";
                    }
                    port = portRepository.GetSinglePort(shipmentPM.Tenant, shipmentPM.MainCarriageFromPortId);
                    location.providence = port != null ? port.State != null ? port.State.Code : "" : "";
                    unCode = port != null ? port.CombinedCode : "";
                    location.unCode = unCode == null ? "" : unCode;
                    location.type = shipmentPM.TransportModeId == "O" ? "marine" : shipmentPM.TransportModeId == "I" ? "inland" : "";
                    //country = computingPartnerHelper.GetComputingPartnerCodeTranslation(shipmentPM.MainCarriageFromPortCountryCode, "Artemus", "Country");
                    //if (string.IsNullOrEmpty(country))
                    //{
                    country = shipmentPM.MainCarriageFromPortCountryCode;
                    //}
                    location.country = country;
                    //customsCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(unCode, "Artemus", "Port");
                    //if (string.IsNullOrEmpty(customsCode))
                    //{
                    //    customsCode = "";
                    //}
                    //location.customsCode = customsCode;
                    log.Locations.Add(location);
                }
                if (shipmentPM.Transshipment1FromPortId != null)
                {
                    location = new ArtemusLocationElement();
                    index = index + 1;
                    location.index = index;
                    location.name = shipmentPM.Transshipment1FromPortName;
                    if (string.IsNullOrEmpty(location.name))
                    {
                        errors += "Transshipment1 location name is missing;";
                    }
                    if (string.IsNullOrEmpty(shipmentPM.Transshipment1FromPortCountryCode))
                    {
                        errors += "Transshipment1 location country is missing;";
                    }
                    port = portRepository.GetSinglePort(shipmentPM.Tenant, shipmentPM.Transshipment1FromPortId);
                    location.providence = port != null ? port.State != null ? port.State.Code : "" : "";
                    unCode = port != null ? port.CombinedCode : "";
                    location.unCode = unCode == null ? "" : unCode;
                    location.type = shipmentPM.TransportModeId == "O" ? "marine" : shipmentPM.TransportModeId == "I" ? "inland" : "";
                    country = computingPartnerHelper.GetComputingPartnerCodeTranslation(shipmentPM.Transshipment1FromPortCountryCode, "G-Artemus", "Country");
                    //if (string.IsNullOrEmpty(country))
                    //{
                    country = shipmentPM.Transshipment1FromPortCountryCode;
                    //}
                    location.country = country;
                    //customsCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(unCode, "Artemus", "Port");
                    //if (string.IsNullOrEmpty(customsCode))
                    //{
                    //    customsCode = "";
                    //}
                    //location.customsCode = customsCode;
                    log.Locations.Add(location);
                }

                if (shipmentPM.Transshipment2FromPortId != null)
                {
                    location = new ArtemusLocationElement();
                    index = index + 1;
                    location.index = index;
                    location.name = shipmentPM.Transshipment2FromPortName;
                    if (string.IsNullOrEmpty(location.name))
                    {
                        errors += "Transshipment2 location name is missing;";
                    }
                    if (string.IsNullOrEmpty(shipmentPM.Transshipment2FromPortCountryCode))
                    {
                        errors += "Transshipment2 location country is missing;";
                    }
                    port = portRepository.GetSinglePort(shipmentPM.Tenant, shipmentPM.Transshipment2FromPortId);
                    location.providence = port != null ? port.State != null ? port.State.Code : "" : "";
                    unCode = port != null ? port.CombinedCode : "";
                    location.unCode = unCode == null ? "" : unCode;
                    location.type = shipmentPM.TransportModeId == "O" ? "marine" : shipmentPM.TransportModeId == "I" ? "inland" : "";
                    // country = computingPartnerHelper.GetComputingPartnerCodeTranslation(shipmentPM.Transshipment2FromPortCountryCode, "Artemus", "Country");
                    // if (string.IsNullOrEmpty(country))
                    // {
                    country = shipmentPM.Transshipment2FromPortCountryCode;
                    //}
                    location.country = country;
                    //customsCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(unCode, "Artemus", "Port");
                    //if (string.IsNullOrEmpty(customsCode))
                    //{
                    //    customsCode = "";
                    //}
                    //location.customsCode = customsCode;
                    log.Locations.Add(location);
                }

                if (shipmentPM.Transshipment3FromPortId != null)
                {
                    location = new ArtemusLocationElement();
                    index = index + 1;
                    location.index = index;
                    location.name = shipmentPM.Transshipment3FromPortName;
                    if (string.IsNullOrEmpty(location.name))
                    {
                        errors += "Transshipment3 location name is missing;";
                    }
                    if (string.IsNullOrEmpty(shipmentPM.Transshipment3FromPortCountryCode))
                    {
                        errors += "Transshipment3 location country is missing;";
                    }
                    port = portRepository.GetSinglePort(shipmentPM.Tenant, shipmentPM.Transshipment3FromPortId);
                    location.providence = port != null ? port.State != null ? port.State.Code : "" : "";
                    unCode = port != null ? port.CombinedCode : "";
                    location.unCode = unCode == null ? "" : unCode;
                    location.type = shipmentPM.TransportModeId == "O" ? "marine" : shipmentPM.TransportModeId == "I" ? "inland" : "";
                    //country = computingPartnerHelper.GetComputingPartnerCodeTranslation(shipmentPM.Transshipment3FromPortCountryCode, "Artemus", "Country");
                    //if (string.IsNullOrEmpty(country))
                    //{
                    country = shipmentPM.Transshipment3FromPortCountryCode;
                    //}
                    location.country = country;
                    //customsCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(unCode, "Artemus", "Port");
                    //if (string.IsNullOrEmpty(customsCode))
                    //{
                    //    customsCode = "";
                    //}
                    //location.customsCode = customsCode;
                    log.Locations.Add(location);
                }

                if (shipmentPM.MainCarriageFinalDestinationPortId != null)
                {
                    location = new ArtemusLocationElement();
                    index = index + 1;
                    location.index = index;
                    location.name = shipmentPM.MainCarriageFinalDestinationPortName;
                    if (string.IsNullOrEmpty(location.name))
                    {
                        errors += "Final Destination location name is missing;";
                    }
                    if (string.IsNullOrEmpty(shipmentPM.MainCarriageFinalDestinationPortCountryCode))
                    {
                        errors += "Final Destination location country is missing;";
                    }
                    port = portRepository.GetSinglePort(shipmentPM.Tenant, shipmentPM.MainCarriageFinalDestinationPortId);
                    location.providence = port != null ? port.State != null ? port.State.Code : "" : "";
                    unCode = port != null ? port.CombinedCode : "";
                    location.unCode = unCode == null ? "" : unCode;
                    location.type = shipmentPM.TransportModeId == "O" ? "marine" : shipmentPM.TransportModeId == "I" ? "inland" : "";
                    //country = computingPartnerHelper.GetComputingPartnerCodeTranslation(shipmentPM.MainCarriageFinalDestinationPortCountryCode, "Artemus", "Country");
                    //if (string.IsNullOrEmpty(country))
                    //{
                    country = shipmentPM.MainCarriageFinalDestinationPortCountryCode;
                    //}
                    location.country = country;
                    //customsCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(unCode, "Artemus", "Port");
                    //if (string.IsNullOrEmpty(customsCode))
                    //{
                    //    customsCode = "";
                    //}
                    //location.customsCode = customsCode;
                    log.Locations.Add(location);
                }

                if (shipmentPM.OnCarriageToPortId != null)
                {
                    location = new ArtemusLocationElement();
                    index = index + 1;
                    location.index = index;
                    location.name = shipmentPM.OnCarriageToPortName;
                    if (string.IsNullOrEmpty(location.name))
                    {
                        errors += "Final Destination location name is missing;";
                    }
                    if (string.IsNullOrEmpty(shipmentPM.OnCarriageToPortCountryCode))
                    {
                        errors += "Final Destination location country is missing;";
                    }
                    port = portRepository.GetSinglePort(shipmentPM.Tenant, shipmentPM.OnCarriageToPortId);
                    location.providence = port != null ? port.State != null ? port.State.Code : "" : "";
                    unCode = port != null ? port.CombinedCode : "";
                    location.unCode = unCode == null ? "" : unCode;
                    location.type = shipmentPM.TransportModeId == "O" ? "marine" : shipmentPM.TransportModeId == "I" ? "inland" : "";
                    //country = computingPartnerHelper.GetComputingPartnerCodeTranslation(shipmentPM.OnCarriageToPortCountryCode, "Artemus", "Country");
                    //if (string.IsNullOrEmpty(country))
                    //{
                    country = shipmentPM.OnCarriageToPortCountryCode;
                    //}
                    location.country = country;
                    //customsCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(unCode, "Artemus", "Port");
                    //if (string.IsNullOrEmpty(customsCode))
                    //{
                    //    customsCode = "";
                    //}
                    //location.customsCode = customsCode;
                    log.Locations.Add(location);
                }
                #endregion

                #region BL 
                if (shipmentPM.ShipmentLevelCode == "C")
                {
                    if (shipmentPM.Master == null)
                    {
                        errors += "Master is missing;";
                    }
                    else
                    {
                        log.BL.number = shipmentPM.Master;
                    }
                }
                else
                {
                    if (shipmentPM.House == null)
                    {
                        errors += "House is missing;";
                    }
                    else
                    {
                        log.BL.number = shipmentPM.House;
                    }
                }

               // log.BL.masterBillNo = shipmentPM.Master;
                log.BL.billType = "way bill";
                VesselRepository vesselRep = new VesselRepository(tenant);
                var vessel = vesselRep.GetSingleVessel(shipmentPM.MainCarriageVesselId, tenant);
                if (vessel == null)
                {
                    errors += "Main Carriage Vessel field is missing;";
                }
                else
                {
                    if (string.IsNullOrEmpty(vessel.EnglishName))
                    {
                        errors += "Main Carriage Vessel field is missing;";
                    }
                    else
                    {
                        log.BL.vessel = vessel.EnglishName;
                    }
                }

                if (shipmentPM.MainCarriageCarrierNumber == null)
                {
                    errors += "Main Carriage Voyage Number is missing;";
                }
                else
                {
                    log.BL.voyage = shipmentPM.MainCarriageCarrierNumber;
                }
                if (shipmentPM.ShipmentTypeId == "FCL" || shipmentPM.ShipmentTypeId == "FCLD")
                {
                    log.BL.moveType = "FCL\\FCL";
                }
                else if (shipmentPM.ShipmentTypeId == "LCL" || shipmentPM.ShipmentTypeId == "LCLD")
                {
                    log.BL.moveType = "LCL\\LCL";
                }
                else if (shipmentPM.ShipmentTypeId == "MyGO")
                {
                    log.BL.moveType = "FCL\\LCL";
                }
                log.BL.nvoType = "non NVO";

                #region PartyRef 
                ArtemusPartyRefElement partyRef = new ArtemusPartyRefElement();
                if (shipmentPM.ShipperId != null)
                {
                    partyRef = new ArtemusPartyRefElement();
                    partyRef.index = 1;
                    partyRef.name = "Shipper";
                    log.BL.PartyRefs.Add(partyRef);
                }

                if (shipmentPM.ConsigneeId != null)
                {
                    partyRef = new ArtemusPartyRefElement();
                    partyRef.index = 2;
                    if (shipmentPM.ShipperId == null) partyRef.index = 1;
                    partyRef.name = "Consignee";
                    log.BL.PartyRefs.Add(partyRef);
                }

                partyRef = new ArtemusPartyRefElement();
                partyRef.index = 2;
                partyRef.name = "Ship To";
                log.BL.PartyRefs.Add(partyRef);

                partyRef = new ArtemusPartyRefElement();
                partyRef.index = 2;
                partyRef.name = "Importer";
                log.BL.PartyRefs.Add(partyRef);

                partyRef = new ArtemusPartyRefElement();
                partyRef.index = (shipmentPM.Notify1Id != null || shipmentPM.Notify2Id != null) ? 3 : 2;
                partyRef.name = "Notify";
                log.BL.PartyRefs.Add(partyRef);
                #endregion

                #region LocationRef 
                index = 1;
                ArtemusLocationRefElement locationRef = new ArtemusLocationRefElement();
                locationRef = new ArtemusLocationRefElement();
                locationRef.name = "Receipt";
                locationRef.index = index;
                log.BL.locationRefs.Add(locationRef);

                if (shipmentPM.PreCarriageFromPortId != null)
                {
                    index = index + 1;
                }

                locationRef = new ArtemusLocationRefElement();
                locationRef.name = "Loading";
                locationRef.index = index;
                log.BL.locationRefs.Add(locationRef);

                if (shipmentPM.Transshipment1FromPortId != null)
                {
                    index = index + 1;
                }
                if (shipmentPM.Transshipment2FromPortId != null)
                {
                    index = index + 1;
                }
                if (shipmentPM.Transshipment3FromPortId != null)
                {
                    index = index + 1;
                }
                if (shipmentPM.MainCarriageFinalDestinationPortId != null)
                {
                    index = index + 1;
                }

                locationRef = new ArtemusLocationRefElement();
                locationRef.name = "Discharge";
                locationRef.index = index;
                log.BL.locationRefs.Add(locationRef);

                if (shipmentPM.OnCarriageToPortId != null)
                {
                    index = index + 1;
                }

                locationRef = new ArtemusLocationRefElement();
                locationRef.name = "Delivery";
                locationRef.index = index;
                log.BL.locationRefs.Add(locationRef);
                #endregion

                #region AMSNotify 
                ArtemusAMSNotifyElement aMSNotify = new ArtemusAMSNotifyElement();
                shippingLine = shippingLineRepository.GetSingleShippingLine(shipmentPM.MainCarriageCarrierId, tenant);
                if (shippingLine == null)
                {
                    errors += "Main Carriage Shipping Line SCAC Code is missing;";
                }
                else
                {
                    if (string.IsNullOrEmpty(shippingLine.SCACCode))
                    {
                        errors += "Main Carriage Shipping Line SCAC Code is missing;";
                    }
                    else
                    {
                        aMSNotify.scac = shippingLine.SCACCode;
                        log.BL.AMSNotifys.Add(aMSNotify);
                    }
                }

                //Transshipment1
                if (shipmentPM.Transshipment1FromPortId != null && shipmentPM.Transshipment1ToPortId != null)
                {
                    //aMSNotify = new ArtemusAMSNotifyElement();
                    shippingLine = shippingLineRepository.GetSingleShippingLine(shipmentPM.Transshipment1CarrierId, tenant);
                    if (shippingLine == null)
                    {
                        errors += "Transshipment1 Shipping Line SCAC Code is missing;";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(shippingLine.SCACCode))
                        {
                            errors += "Transshipment1 Shipping Line SCAC Code is missing;";
                        }
                        else
                        {
                            //aMSNotify.scac = shippingLine.SCACCode;
                            //log.BL.AMSNotifys.Add(aMSNotify);
                        }
                    }
                }

                // Transshipment2
                if (shipmentPM.Transshipment2FromPortId != null && shipmentPM.Transshipment2ToPortId != null)
                {
                    //aMSNotify = new ArtemusAMSNotifyElement();
                    shippingLine = shippingLineRepository.GetSingleShippingLine(shipmentPM.Transshipment2CarrierId, tenant);
                    if (shippingLine == null)
                    {
                        errors += "Transshipment2 Shipping Line SCAC Code is missing;";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(shippingLine.SCACCode))
                        {
                            errors += "Transshipment2 Shipping Line SCAC Code is missing;";
                        }
                        else
                        {
                            //aMSNotify.scac = shippingLine.SCACCode;
                            //log.BL.AMSNotifys.Add(aMSNotify);
                        }
                    }
                }

                //Transshipment3
                if (shipmentPM.Transshipment3FromPortId != null && shipmentPM.Transshipment3ToPortId != null)
                {
                    //aMSNotify = new ArtemusAMSNotifyElement();
                    shippingLine = shippingLineRepository.GetSingleShippingLine(shipmentPM.Transshipment3CarrierId, tenant);
                    if (shippingLine == null)
                    {
                        errors += "Transshipment3 Shipping Line SCAC Code is missing;";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(shippingLine.SCACCode))
                        {
                            errors += "Transshipment3 Shipping Line SCAC Code is missing;";
                        }
                        else
                        {
                            //aMSNotify.scac = shippingLine.SCACCode;
                            //log.BL.AMSNotifys.Add(aMSNotify);
                        }
                    }
                }
                #endregion

                #region Package  & Cargos
                var packageIndex = 0;
                var equipmentIndex = 1;
                if (shipmentPM.ShipmentTypeId == "FCL" || shipmentPM.ShipmentTypeId == "FCLD" || shipmentPM.ShipmentTypeId == "MyGO")
                {
                    List<ShipmentPackagePM> packages = shipmentPM.ShipmentPackages.ToList();
                    ArtemusPackageElement package = new ArtemusPackageElement();
                    ArtemusAttributeElement attribute = new ArtemusAttributeElement();
                    ArtemusCargoElement cargo = new ArtemusCargoElement();
                    ArtemusEquipmentElement equipment = new ArtemusEquipmentElement();


                    if (shipmentPM.ShipmentPackages == null || (shipmentPM.ShipmentPackages != null && shipmentPM.ShipmentPackages.Count() == 0))
                    {
                        errors += "Can't send Bill of Lading message without Shipment Packages;";
                    }
                    else
                    {
                        foreach (var item in packages)
                        {
                            var grossWeigthUnit = GetGWUnit(shipmentPM.ChargeableWeightUnitCode);
                            var grossWeigthType = GetGWType(shipmentPM.ChargeableWeightUnitCode);
                            var volumeUnit = GetVolumeUnit(shipmentPM.VolumeUnitCode);
                            var volumeType = GetVolumeType(shipmentPM.VolumeUnitCode);
                            if (item.InsideShipmentPackages == null || (item.InsideShipmentPackages != null && item.InsideShipmentPackages.Count() == 0))
                            {
                                errors += "Can't send Bill of Lading message without Shipment Inside Packages;";
                            }
                            else
                            {
                                foreach (var insidePackage in item.InsideShipmentPackages)
                                {
                                    package = new ArtemusPackageElement();
                                    package.Attributes = new List<ArtemusAttributeElement>();
                                    package.index = packageIndex + 1;
                                    package.packages = insidePackage.PackageTypeName;
                                    package.equipmentIndex = equipmentIndex;
                                    package.pieceCount = insidePackage.Quantity != null ? insidePackage.Quantity + "" : "";
                                    attribute = new ArtemusAttributeElement();
                                    if (!string.IsNullOrEmpty(grossWeigthUnit))
                                    {
                                        attribute.units = grossWeigthUnit;
                                    }
                                    if (!string.IsNullOrEmpty(grossWeigthType))
                                    {
                                        attribute.type = grossWeigthType;
                                    }
                                    if (insidePackage.VolumetricWeight != null)
                                    {
                                        attribute.value =  insidePackage.VolumetricWeight + "";
                                    }
                                    package.Attributes.Add(attribute);

                                    attribute = new ArtemusAttributeElement();
                                    if (!string.IsNullOrEmpty(volumeUnit))
                                    {
                                        attribute.units = volumeUnit;
                                    }

                                    if (!string.IsNullOrEmpty(volumeType))
                                    {
                                        attribute.type = volumeType;
                                    }
                                    if (insidePackage.Volume != null)
                                    {
                                        attribute.value = insidePackage.Volume + "";
                                    }
                                    package.Attributes.Add(attribute);

                                    log.BL.Packages.Add(package);
                                    cargo = new ArtemusCargoElement();
                                    cargo.packageIndex = package.index;
                                    if (!string.IsNullOrEmpty(item.Harmonize))
                                    {
                                        cargo.harmonizedCode = item.Harmonize;
                                    }
                                    if (!string.IsNullOrEmpty(item.UnNumber))
                                    {
                                        cargo.hazardCode = item.UnNumber;
                                    }
                                    if (!string.IsNullOrEmpty(insidePackage.Description) || !string.IsNullOrEmpty(item.Description))
                                    {
                                        cargo.description = !string.IsNullOrEmpty(insidePackage.Description) ? insidePackage.Description : item.Description;
                                    }
                                 
                                    log.BL.Cargos.Add(cargo);
                                    packageIndex += 1;
                                }
                                equipment = new ArtemusEquipmentElement();
                                equipment.index = equipmentIndex;
                                equipment.equipmentNumber = item.ContainerNumber;
                                equipment.type = computingPartnerHelper.GetComputingPartnerCodeTranslation(item.PackageTypeCode, "G-Artemus", "PackageType");
                             //   equipment.seals = !string.IsNullOrEmpty(item.Seal) ? item.Seal + (!string.IsNullOrEmpty(item.Seal2) ? ("," + item.Seal2) : "") : (!string.IsNullOrEmpty(item.Seal2) ? item.Seal2 : "");
                                log.BL.Equipments.Add(equipment);
                                equipmentIndex += 1;
                            }
                        }
                    }
                }

                else if (shipmentPM.ShipmentTypeId == "LCL" || shipmentPM.ShipmentTypeId == "LCLD")
                {
                    List<ShipmentPackagePM> packages = shipmentPM.ShipmentPackages.ToList();
                    ArtemusPackageElement package = new ArtemusPackageElement();
                    ArtemusCargoElement cargo = new ArtemusCargoElement();
                    ArtemusAttributeElement attribute;
                    var grossWeigthUnit = GetGWUnit(shipmentPM.GrossWeightUnitCode);
                    var grossWeigthType = GetGWType(shipmentPM.GrossWeightUnitCode);
                    var volumeUnit = GetGWUnit(shipmentPM.VolumeUnitCode);
                    var volumeType = GetGWType(shipmentPM.VolumeUnitCode);

                    if (shipmentPM.ShipmentPackages == null || (shipmentPM.ShipmentPackages != null && shipmentPM.ShipmentPackages.Count() == 0))
                    {
                        errors += "Can't send Bill of Lading message without Shipment Packages;";
                    }
                    else
                    {
                        foreach (var item in packages)
                        {
                            package = new ArtemusPackageElement();
                            package.Attributes = new List<ArtemusAttributeElement>();
                            package.index = packageIndex + 1;
                            package.packages = item.PackageTypeName;
                            package.pieceCount = item.Quantity != null ? item.Quantity + "" : "";

                            attribute = new ArtemusAttributeElement();
                            if (!string.IsNullOrEmpty(grossWeigthUnit))
                            {
                                attribute.units = grossWeigthUnit;
                            }
                            if (!string.IsNullOrEmpty(grossWeigthType))
                            {
                                attribute.type = grossWeigthType;
                            }
                            if (item.VolumetricWeight != null)
                            {
                                attribute.value = item.VolumetricWeight + "";
                            }
                            package.Attributes.Add(attribute);

                            attribute = new ArtemusAttributeElement();
                            if (!string.IsNullOrEmpty(volumeUnit))
                            {
                                attribute.units = volumeUnit;
                            }

                            if (!string.IsNullOrEmpty(volumeType))
                            {
                                attribute.type = volumeType;
                            }
                            if (item.Volume != null)
                            {
                                attribute.value = item.Volume + "";
                            }
                            package.Attributes.Add(attribute);

                            log.BL.Packages.Add(package);

                            cargo = new ArtemusCargoElement();
                            cargo.packageIndex = package.index;
                            if (!string.IsNullOrEmpty(item.Harmonize))
                            {
                                cargo.harmonizedCode = item.Harmonize;
                            }
                            if (!string.IsNullOrEmpty(item.UnNumber))
                            {
                                cargo.hazardCode = item.UnNumber;
                            }
                            if (!string.IsNullOrEmpty(item.Description))
                            {
                                cargo.description = item.Description;
                            }

                            log.BL.Cargos.Add(cargo);
                        }
                    }
                }
                #endregion

                #endregion
            }
        
            if (!string.IsNullOrEmpty(errors))
            {
                errors = errors.TrimEnd(';');
                throw new ApplicationException(errors);
            }
            else
            {
                this.BuildXMLFile(log, tenant, shipmentPM.Id, shipmentPM.ShipmentNumber);
            }
        }
        private void BuildXMLFile(object log, int tenant, string entityId, string shipmentNumber)
        {
            Type myType = log.GetType();
            MemoryStream memstream = new MemoryStream();
            XmlSerializer ser = new XmlSerializer(myType);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "",
                OmitXmlDeclaration = true,
                NewLineChars = "",
                NewLineHandling = NewLineHandling.Replace,
            };

            XmlWriter writer = XmlTextWriter.Create(memstream, settings);
            //writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n");
            ser.Serialize(writer, log, ns);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();

            content = content.Replace(" />", "/>");

            byte[] bytearray = Encoding.ASCII.GetBytes(content);
            this.BuildCommunicationLog(bytearray, tenant, entityId, shipmentNumber);
        }
        private void BuildCommunicationLog(byte[] bytearray, int tenant, string entityId, string shipmentNumber)
        {
            ObjectTableRepository repo = new ObjectTableRepository(tenant);
            var objectTableId = repo.GetObjectTableIdByName("Shipment");

            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(commonContext);
            DocumentRepository documentRepository = new DocumentRepository(commonContext);

            string xmlTarget = "Artemus";
            string xmlSubject = communicationSubject;
            string host = "";
            string folder = "";
            string username = "";
            string password = "";


            CustomsInterfaceSettingRepository customsInterfaceSettingRepository = new CustomsInterfaceSettingRepository(tenant);
            CustomsInterfaceSetting interfaceSetting = (from d in commonContext.CustomsInterfaceSettings
                                                        where d.Tenant == tenant && d.ImportToUSAInterfaceCode == "ART"
                                                        select d).FirstOrDefault();

            if (interfaceSetting != null)
            {
                string artemusOutSettingsId = interfaceSetting.ArtemusOutSettingsId;
                FTPDetailRepository fTPDetailRepository = new FTPDetailRepository(tenant);
                FTPDetail fTPDetail = (from d in commonContext.FTPDetails
                                       where d.Tenant == tenant && d.Id == artemusOutSettingsId
                                       select d).FirstOrDefault();

                if (fTPDetail != null)
                {
                    host = fTPDetail.Host;
                    folder = fTPDetail.Folder;
                    username = fTPDetail.UserName;
                    password = fTPDetail.Password;
                }

                if (communicationSubject == "BOL")
                {
                    communicationSubject = "BL";
                }
                string filename = communicationSubject + "_" + shipmentNumber;
                var settings = new CommunicationLogSettings() { host = host, folder = folder, username = username, password = password, filename = filename };
                var settingsData = JsonConvert.SerializeObject(settings);

                Document document = new Document()
                {
                    CreateDate = DateTime.Now,
                    Extension = "xml",
                    FileSize = bytearray.Length,
                    Tenant = Convert.ToInt32(tenant),
                    Id = IdCounter.GetNumber("Document", tenant),
                    HasFile = true,
                    Folder = xmlTarget.ToLower(),
                };

                documentRepository.Add(document);
                documentRepository.SubmitChanges();

                CommunicationLog commLog = new CommunicationLog()
                {
                    Id = IdCounter.GetNumber("CommunicationLog", tenant),
                    LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    LastStatusDateUTC = DateTime.UtcNow,
                    To = xmlTarget,
                    InOut = "O",
                    EntityId = entityId,
                    ObjectTableId = objectTableId,
                    Subject = xmlSubject,
                    Tenant = tenant,
                    CommunicationLogTypeCode = "T",
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    CommunicationStatusTypeCode = "W",
                    DocumentId = document.Id,
                    CreateDateUTC = DateTime.UtcNow,
                    LogSettings = settingsData,
                    QueueName = "FTPCommunicationLogQueue"
                };

                communicationLogRepository.Add(commLog);
                communicationLogRepository.SubmitChanges();

                Logitude.Server.Tools.BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = document.Id,
                    FolderName = document.Folder,
                    Extension = document.Extension,
                    Tenant = tenant,
                    FileSize = bytearray.Length,
                };

                Logitude.Server.Tools.StorageService.IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                storageservice.Write(bytearray.ToArray(), fileInfo);

                SendCommunicationLogMessageToQueue(commLog.QueueName, commLog.Id, tenant);

                ShipmentCustomsTransmissionArgs args = new ShipmentCustomsTransmissionArgs()
                {
                    ShipmentId = shipmentId,
                    MessageType = communicationSubject == "BL" ? "ARBL" : "ASVO",
                    Status = "SENT",
                    CommunicationLogId = commLog.Id,
                };

                ShipmentCustomsTransmissionHelper transmissionHelper = new ShipmentCustomsTransmissionHelper(tenant);
                transmissionHelper.Run(args);
            }
        }

        private void SendCommunicationLogMessageToQueue(string queueName, string communicationLogId, int tenant)
        {
            try
            {
                IQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue(queueName, 0);
                queueservice.Send(new Dictionary<string, string>() { { "CommunicationLogId", communicationLogId }, { "Tenant", tenant.ToString() } });

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Send FTP CommunicationLog Queue", null, null);
            }
        }
    
        private string GetVolumeUnit(string code)
        {
            string vcode = computingPartnerHelper.GetComputingPartnerCodeTranslation(code, "G-Artemus", "VolumeUnit");
            return vcode;
        }
        private string GetVolumeType(string code)
        {
            string vcode = "";
            if (code == "CBM")
                vcode = "Ms. (M)";
            if (code == "CBF")
                vcode = "Ms. (I)";
            return vcode;
        }
        private string GetGWUnit(string code)
        {
            string vcode = computingPartnerHelper.GetComputingPartnerCodeTranslation(code, "G-Artemus", "WeightUnit");
            return vcode;
        }
        private string GetGWType(string code)
        {
            string vcode = "";
            if (code == "KG")
                vcode = "Wt. (M)";
            if (code == "LB")
                vcode = "Wt. (I)";
            return vcode;
        }
    }

    [XmlRoot("AMS")]
    public class ArtemusEDIVoyageRoot
    {
        [System.Xml.Serialization.XmlElement("Location")]
        public List<ArtemusLocationElement> Locations { get; set; }
        public ArtemusVoyageElement Voyage { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string scac { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version { get; set; }
    }
    public class ArtemusLocationElement
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int index { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string country { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string providence { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string customsCode { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string unCode { get; set; }
        //[System.Xml.Serialization.XmlAttributeAttribute()]
        //public string canadaCustomsOffice { get; set; }
    }
    public class ArtemusVoyageElement
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string number { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string vessel { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string scac { get; set; }
        [System.Xml.Serialization.XmlElement("PortCall")]
        public List<ArtemusVoyagePortCallElement> PortCalls { get; set; }
    }
    public class ArtemusVoyagePortCallElement
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int locationIndex { get; set; }
        ////[XmlElement(DataType = "date")]
        //[System.Xml.Serialization.XmlAttributeAttribute(DataType = "dateTime")]
        //public DateTime arriveDate { get; set; }
        ////[XmlElement(DataType = "date")]
        //[System.Xml.Serialization.XmlAttributeAttribute(DataType = "dateTime")]
        //public DateTime sailDate { get; set; }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public DateTime arriveDate { get; set; }
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public DateTime sailDate { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute("arriveDate")]
        public string arriveDateString
        {
            get
            {
                return this.arriveDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set
            {
                this.arriveDate = DateTime.Parse(value);
            }
        }
        [System.Xml.Serialization.XmlAttributeAttribute("sailDate")]
        public string sailDateString
        {
            get
            {
                return this.sailDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
            set
            {
                this.sailDate = DateTime.Parse(value);
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool load { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool discharge { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool lastLoad { get; set; }
    }

    [XmlRoot("AMS")]
    public class ArtemusEDIBillRoot
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string scac { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version { get; set; }
        [System.Xml.Serialization.XmlElement("Party")]
        public List<ArtemusPartyElement> Partys { get; set; }
        [System.Xml.Serialization.XmlElement("Location")]
        public List<ArtemusLocationElement> Locations { get; set; }
        public ArtemusBLElement BL { get; set; }
    }
    public class ArtemusPartyElement
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int index { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string address1 { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string address2 { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string postcode { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string city { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string country { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string providence { get; set; }
        
    }
    public class ArtemusBLElement
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string number { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string billType { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string masterBilllScac { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string vessel { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string voyage { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string moveType { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string nvoType { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string notify { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string hblSCAC { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string masterBillNo { get; set; }
        [System.Xml.Serialization.XmlElement("PartyRef",Order =1)]
        public List<ArtemusPartyRefElement> PartyRefs { get; set; }
        [System.Xml.Serialization.XmlElement("locationRef", Order = 2)]
        public List<ArtemusLocationRefElement> locationRefs { get; set; }
        [System.Xml.Serialization.XmlElement("AMSNotify", Order = 3)]
        public List<ArtemusAMSNotifyElement> AMSNotifys { get; set; }
        [System.Xml.Serialization.XmlElement("Equipment", Order = 4)]
        public List<ArtemusEquipmentElement> Equipments { get; set; }
        [System.Xml.Serialization.XmlElement("Package", Order = 5)]
        public List<ArtemusPackageElement> Packages { get; set; }
        [System.Xml.Serialization.XmlElement("Cargo", Order = 6)]
        public List<ArtemusCargoElement> Cargos { get; set; }
    }
    public class ArtemusPartyRefElement
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int index { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name { get; set; }
    }
    public class ArtemusLocationRefElement
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int index { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name { get; set; }
    }
    public class ArtemusAMSNotifyElement
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string scac { get; set; }
    }
    public class ArtemusEquipmentElement
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int index { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string equipmentNumber { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string seals { get; set; }
    }
    public class ArtemusPackageElement
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int index { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string pieceCount { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string packages { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int equipmentIndex { get; set; }
        [System.Xml.Serialization.XmlElement("Attribute")]
        public List<ArtemusAttributeElement> Attributes { get; set; }
    }
    public class ArtemusAttributeElement
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string units { get; set; }
    }
    public class ArtemusCargoElement
    {
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int packageIndex { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string harmonizedCode { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string hazardCode { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string manufacturerIndex { get; set; }
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description { get; set; }
    }

    public class CommunicationLogSettings
    {
        public string host { get; set; }
        public string folder { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string filename { get; set; }
    }
}
