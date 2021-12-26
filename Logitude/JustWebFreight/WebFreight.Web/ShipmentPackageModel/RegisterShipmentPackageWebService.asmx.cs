using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.DataProviders;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel;

namespace WebFreight.Web.ShipmentPackageModel
{
    /// <summary>
    /// Summary description for RegisterShipmentPackageWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class RegisterShipmentPackageWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public byte[] RegisterShipments(byte[] xmlFilters, int tenant, string partnerId)
        {
            RegisterShipmentPackageDataProvider data = this.BeginRegister(xmlFilters, tenant, partnerId);

            XmlSerializer serializer = new XmlSerializer(typeof(RegisterShipmentPackageDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, data);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        private RegisterShipmentPackageDataProvider BeginRegister(byte[] xmlFilters, int tenant, string partnerId)
        {
            RegisterShipmentPackageDataProvider totalData = new RegisterShipmentPackageDataProvider();
            totalData.Records = new List<RegisterShipmentPackageDataProvider.RegisterShipmentPackageRecord>();

            IShipmentsContext shipmentCotnext = ShipmentsContext.GetContext(tenant);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            ShipmentRepository shipmentRep = new ShipmentRepository(shipmentCotnext);
            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRep);
            ARInvoiceRepository invoiceRep = new ARInvoiceRepository(tenant);
            TenantRepository tenantRep = new TenantRepository(commonContext);
            AddressRepository addressRepository = new AddressRepository(commonContext);
            PortRepository portRepository = new PortRepository(commonContext);
            Tenant ten = tenantRep.GetSingleTenant(tenant);

            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);

            IQueryable<ShipmentJoinPackageList> shipments = shipmentQuery.GetShipmentsJoinPackagesByTenant(tenant);
            List<string> packageIds = shipments.Select(s => s.PackageId).ToList();
            List<ShipmentPackageItem> packageItems = shipmentCotnext.ShipmentPackageItems.Where(d => packageIds.Contains(d.PackageId)).ToList();

            QueryFilterItem fromDateItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CreateDateTime" && d.Operator == "GreaterThanOrEqual").FirstOrDefault();
            QueryFilterItem toDateItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CreateDateTime" && d.Operator == "LessThanOrEqual").FirstOrDefault();
            QueryFilterItem directionItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "DirectionId" && d.Operator == "Equals").FirstOrDefault();
            QueryFilterItem typeItem = queryOperations.QueryFilterItems.Where(d => d.FieldName == "ShipmentTypeId" && d.Operator == "Equals").FirstOrDefault();

            string directions = directionItem != null ? directionItem.FieldValue.ToString() : "";
            if (!string.IsNullOrEmpty(directions))
            {
                shipments = shipments.Where(d => directions.Contains(d.DirectionId));
            }

            if (fromDateItem != null)
            {
                DateTime fromDate;
                DateTime.TryParse(fromDateItem.FieldValue.ToString(), out fromDate);
                if (fromDate != null)
                {
                    shipments = shipments.Where(d => DbFunctions.TruncateTime(d.CreateDateTime) >= DbFunctions.TruncateTime(fromDate));
                }
            }

            if (toDateItem != null)
            {
                DateTime toDate;
                DateTime.TryParse(toDateItem.FieldValue.ToString(), out toDate);
                if (toDate != null)
                {
                    shipments = shipments.Where(d => DbFunctions.TruncateTime(d.CreateDateTime) <= DbFunctions.TruncateTime(toDate));
                }
            }

            if (!string.IsNullOrEmpty(partnerId))
            {
                Card card = CardRepository.GetSingleCard(partnerId, tenant, true);
                if (card.PartnerTypeId == "AG")
                {
                    shipments = shipments.Where(d => d.AgentId == partnerId);
                    totalData.Partner = "Agent Name";
                }
                else if (card.PartnerTypeId == "CS")
                {
                    shipments = shipments.Where(d => d.CustomerId == partnerId);
                    totalData.Partner = "Customer Name";
                }
            }

            else
            {
                totalData.Partner = "Partner";
            }

            string type = typeItem != null ? typeItem.FieldValue.ToString() : null;
            if (!string.IsNullOrEmpty(type))
            {
                shipments = shipments.Where(d => d.ShipmentTypeId == type);
            }

            totalData.Logo = DataProviders.General.GetLogo(tenant);
            totalData.TenantName = ten != null ? ten.Company : "";

            shipments = shipments.Where(d => d.IsCancelled == false);

            foreach (ShipmentJoinPackageList shipment in shipments)
            {                
                ShipmentPickUpDelivery myLastDelivery = (from d in shipmentCotnext.ShipmentPickUpDeliveries
                                                         where d.ShipmentId == shipment.ShipmentId && d.PickUpDeliveryTypeCode == "DELV"
                                                         select d).OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();

                ShipmentPickUpDelivery myRailDelivery = (from d in shipmentCotnext.ShipmentPickUpDeliveries
                                                         where d.ShipmentId == shipment.ShipmentId && d.PickUpDeliveryTypeCode == "DELV"
                                                         && d.TransportModeCode == "BYRA"
                                                         select d).FirstOrDefault();

                List<ARInvoice> invoices = invoiceRep.GetInvoicesByMainEntityId(shipment.ShipmentId, tenant)
                                                                       .Where(d => d.BillToId == shipment.CustomerId && d.StatusCode != "LL")
                                                                       .ToList();

                ShipmentPickUpDelivery myFirstPickUp = (from d in shipmentCotnext.ShipmentPickUpDeliveries
                                                        where d.ShipmentId == shipment.ShipmentId && d.PickUpDeliveryTypeCode == "PICK"
                                                        select d).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();

                List<ShipmentPackageItem> myPackageItems = packageItems.Where(d => d.PackageId == shipment.PackageId).ToList();

                WebFreight.Web.ShipmentPackageModel.RegisterShipmentPackageDataProvider.RegisterShipmentPackageRecord provider = new RegisterShipmentPackageDataProvider.RegisterShipmentPackageRecord();

                provider.ShipmentNumber = shipment.ShipmentNumber != null ? shipment.ShipmentNumber : "";
                provider.ETD = shipment.MainCarriageETD != null ? String.Format("{0:dd MMM yyyy}", shipment.MainCarriageETD) : "";
                provider.ETA = shipment.MainCarriageFinalDestinationETA != null ? String.Format("{0:dd MMM yyyy}", shipment.MainCarriageFinalDestinationETA) : "";
                provider.MasterNumber = !string.IsNullOrEmpty(shipment.MasterNumber) ? shipment.MasterNumber : "";
                provider.Status = !string.IsNullOrEmpty(shipment.StatusName) ? shipment.StatusName : "";
                provider.AgentName = !string.IsNullOrEmpty(shipment.AgentName) ? shipment.AgentName : "";
                provider.CustomerName = !string.IsNullOrEmpty(shipment.CustomerName) ? shipment.CustomerName : "";
                provider.POL = !string.IsNullOrEmpty(shipment.MainCarriageFromPortName) ? shipment.MainCarriageFromPortName : "";
                provider.POD = !string.IsNullOrEmpty(shipment.MainCarriageFinalDestinationPortName) ? shipment.MainCarriageFinalDestinationPortName : "";
                provider.ShipperName = !string.IsNullOrEmpty(shipment.ShipperName) ? shipment.ShipperName : "";
                provider.ShippingLine = !string.IsNullOrEmpty(shipment.MainCarriageCarrierName) ? shipment.MainCarriageCarrierName : "";
                provider.ContainerNumber = !string.IsNullOrEmpty(shipment.ContainerNumber) ? shipment.ContainerNumber : "";
                provider.ETDAsDateTime = shipment.MainCarriageETD;
                provider.ETAAsDateTime = shipment.MainCarriageFinalDestinationETA;
                provider.IsCancelled = shipment.IsCancelled;
                provider.ShipmentDescriptionofGoods = shipment.DescriptionofGoods;
                provider.House = shipment.House;
                provider.ConsigneeName = shipment.ConsigneeName;
                provider.ShipmentPackageReference1 = shipment.ShipmentPackageReference1;
                provider.ShipmentPackageReference2 = shipment.ShipmentPackageReference2;
                provider.ShipmentPackageReference3 = shipment.ShipmentPackageReference3;
                provider.ShipmentPackageReference4 = shipment.ShipmentPackageReference4;
                provider.ContainerTypeName = shipment.ContainerTypeName;
                provider.OnCarriageTo = shipment.OnCarriageTo;
                provider.OnForwardingTo = shipment.OnForwardingTo;
                provider.ATD = shipment.ATD;
                provider.ATA = shipment.ATA;
                provider.OnCarriageATD = shipment.OnCarriageATD;
                provider.OnCarriageATA = shipment.OnCarriageATA;
                provider.OnCarriageETA = shipment.OnCarriageETA;
                provider.OnForwardingATD = shipment.OnForwardingATD;
                provider.OnForwardingATA = shipment.OnForwardingATA;
                provider.OnForwardingETA = shipment.OnForwardingETA;
                provider.ContainerNotes = shipment.ContainerNotes;
                provider.GrossWeight = String.Format("{0:0,0.00}", shipment.PackagesGrossWeight);
                provider.Flagged = shipment.ContainerFollowUp;
                provider.GrossWeightAsDouble = shipment.PackagesGrossWeight;
                provider.Ramp = shipment.ShipmentLevelCode == "H" ? shipment.OnForwardingToPortCode : shipment.OnCarriageToPortCode;
                provider.BookingConfirmationNumber = shipment.BookingConfirmationNumber;
                provider.Volume = shipment.Volume;
                provider.ContainerVolume = shipment.PackageVolume;
                provider.TotalNumberOfContainers = shipment.NumberOfContainers;

                if (!string.IsNullOrEmpty(shipment.MainCarriageCarrierId))
                {
                    ShippingLine myShippingLine = (from d in commonContext.ShippingLines where d.Id == shipment.MainCarriageCarrierId select d).FirstOrDefault();
                    if (myShippingLine != null)
                    {
                        provider.ShippingLineSCAC = myShippingLine.SCACCode;
                    }
                }

                if (!string.IsNullOrEmpty(shipment.IncotermId))
                {
                    Incoterm myIncoterm = (from d in commonContext.Incoterms where d.Id == shipment.IncotermId select d).FirstOrDefault();
                    if (myIncoterm != null)
                    {
                        provider.Incoterm = myIncoterm.Name;
                        provider.IncotermCode = myIncoterm.Code;
                    }
                }

                if (!string.IsNullOrEmpty(shipment.ShipperAddressId))
                {
                    Address shipperAddress = addressRepository.GetSingleAddress(shipment.ShipperAddressId, tenant);
                    if (shipperAddress != null)
                    {
                        if (shipperAddress.Country != null)
                        {
                            provider.ShipperCityAndCountry = shipperAddress.City + ", " + shipperAddress.Country.EnglishName;
                        }
                        else
                        {
                            provider.ShipperCityAndCountry = shipperAddress.City;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(shipment.ConsigneeAddressId))
                {
                    Address consigneeAddress = addressRepository.GetSingleAddress(shipment.ConsigneeAddressId, tenant);
                    if (consigneeAddress != null)
                    {
                        if (consigneeAddress.Country != null)
                        {
                            provider.ConsigneeCityAndCountry = consigneeAddress.City + ", " + consigneeAddress.Country.EnglishName;
                        }
                        else
                        {
                            provider.ConsigneeCityAndCountry = consigneeAddress.City;
                        }
                    }
                }

                if (myFirstPickUp != null)
                {
                    provider.PickupATD = myFirstPickUp.ATD;
                    provider.PickupATA = myFirstPickUp.ATA;
                }

                if (myPackageItems.Count > 0)
                {
                    string desc = null;
                    string value = null;
                    string quantity = null;
                    foreach (ShipmentPackageItem item in myPackageItems)
                    {
                        if (!string.IsNullOrEmpty(item.Description))
                        {
                            if (string.IsNullOrEmpty(desc))
                            {
                                desc = item.Description;
                            }
                            else
                            {
                                desc = desc + Environment.NewLine + item.Description;
                            }
                        }
                        if (item.GoodsValue != null)
                        {
                            if (string.IsNullOrEmpty(value))
                            {
                                value = item.GoodsValue.Value.ToString();
                            }
                            else
                            {
                                value = value + Environment.NewLine + item.GoodsValue.Value.ToString();
                            }
                        }
                        if (item.Quantity != null)
                        {
                            if (string.IsNullOrEmpty(quantity))
                            {
                                quantity = item.Quantity.Value.ToString();
                            }
                            else
                            {
                                quantity = quantity + Environment.NewLine + item.Quantity.Value.ToString();
                            }
                        }
                    }
                    provider.ContainerPackageItemsDescription = desc;
                    provider.ContainerPackageItemsQuantity = quantity;
                    provider.ContainerPackageItemsValue = value;
                }

                if (!string.IsNullOrEmpty(shipment.Transshipment3FromPortId))
                {
                    provider.LastATA = shipment.Transshipment3ATA;
                    provider.LastETA = shipment.Transshipment3ETA;
                    provider.LastVessel = shipment.Transshipment3VesselName;

                }

                else if (!string.IsNullOrEmpty(shipment.Transshipment2FromPortId))
                {
                    provider.LastVessel = shipment.Transshipment2VesselName;
                    provider.LastATA = shipment.Transshipment2ATA;
                    provider.LastETA = shipment.Transshipment2ETA;
                }

                else if (!string.IsNullOrEmpty(shipment.Transshipment1FromPortId))
                {
                    provider.LastVessel = shipment.Transshipment1VesselName;
                    provider.LastATA = shipment.Transshipment1ATA;
                    provider.LastETA = shipment.Transshipment1ETA;
                }

                else if (!string.IsNullOrEmpty(shipment.MainCarriageFromPortId))
                {
                    provider.LastVessel = shipment.MainCarriageVesselName;
                    provider.LastATA = shipment.MainCarriageATA;
                    provider.LastETA = shipment.MainCarriageETA;
                }

                if (shipment.SplitOnCarriage == true)
                {
                    provider.ATARamp = shipment.PackageOnCarriageATA;
                    provider.ATDRamp = shipment.PackageOnCarriageATD;
                    provider.ETARamp = shipment.PackageOnCarriageETA;
                }

                else
                {
                    if (shipment.ShipmentLevelCode == "H")
                    {
                        provider.ATARamp = shipment.OnForwardingATA;
                        provider.ATDRamp = shipment.OnForwardingATD;
                        provider.ETARamp = shipment.OnForwardingETA;
                    }

                    else
                    {
                        provider.ATARamp = shipment.OnCarriageATA;
                        provider.ATDRamp = shipment.OnCarriageATD;
                        provider.ETARamp = shipment.OnCarriageETA;
                    }
                }

                if (!string.IsNullOrEmpty(shipment.PackageDliveryId))
                {
                    ShipmentPickUpDelivery myDelivery = (from d in shipmentCotnext.ShipmentPickUpDeliveries
                                                         where d.Id == shipment.PackageDliveryId
                                                         select d).FirstOrDefault();

                    if (myDelivery != null)
                    {
                        provider.ATADoor = myDelivery.ATA;
                    }
                }

                else if (shipment.ContainerNumber != null)
                {
                    List<ShipmentPickUpDelivery> allDeliveries = (from d in shipmentCotnext.ShipmentPickUpDeliveries
                                                                  where d.ShipmentId == shipment.ShipmentId
                                                                  && d.PickUpDeliveryTypeCode == "DELV"
                                                                  select d).ToList();

                    foreach (ShipmentPickUpDelivery myDelivery in allDeliveries)
                    {
                        bool hasContainer = (from d in shipmentCotnext.ShipmentPickUpDeliveryPackages
                                             where d.ShipmentPickUpDeliveryId == myDelivery.Id
                                             && d.ContainerNumber == shipment.ContainerNumber
                                             select d).Any();

                        if (hasContainer)
                        {
                            provider.ATADoor = myDelivery.ATA;
                            break;
                        }
                    }
                }

                if (myRailDelivery != null)
                {
                    provider.RailATA = myRailDelivery.ATA;
                    provider.RailATD = myRailDelivery.ATD;

                    switch (myRailDelivery.PickUpDeliveryToTypeCode)
                    {
                        case "PART":
                            {
                                if (!string.IsNullOrEmpty(myRailDelivery.ToAddressId))
                                {
                                    Address myPartnerAddress = addressRepository.GetSingleAddress(myRailDelivery.ToAddressId, tenant);
                                    if (myPartnerAddress != null)
                                    {
                                        provider.RailTo = myPartnerAddress.City;
                                    }
                                }

                                break;
                            }

                        case "PORT":
                            {
                                if (!string.IsNullOrEmpty(myRailDelivery.ToPortId))
                                {
                                    Port myPort = portRepository.GetSinglePort(tenant, myRailDelivery.ToPortId);
                                    if (myPort != null)
                                    {
                                        provider.RailTo = myPort.EnglishName;
                                    }
                                }

                                break;
                            }

                        case "CASL":
                            {
                                string myCity = myRailDelivery.ToAddressCity;
                                if (!string.IsNullOrEmpty(myCity))
                                {
                                    provider.RailTo = myCity;
                                }

                                break;
                            }
                    }
                }

                if (myLastDelivery != null)
                {
                    provider.DeliveryATD = myLastDelivery.ATD;
                    provider.DeliveryATA = myLastDelivery.ATA;
                    provider.DeliveryETA = myLastDelivery.ETA;

                    switch (myLastDelivery.PickUpDeliveryToTypeCode)
                    {
                        case "PART":
                            {
                                if (!string.IsNullOrEmpty(myLastDelivery.ToAddressId))
                                {
                                    Address myPartnerAddress = addressRepository.GetSingleAddress(myLastDelivery.ToAddressId, tenant);
                                    if (myPartnerAddress != null)
                                    {
                                        provider.FinalDestination = myPartnerAddress.City;
                                    }
                                }

                                break;
                            }

                        case "PORT":
                            {
                                if (!string.IsNullOrEmpty(myLastDelivery.ToPortId))
                                {
                                    Port myPort = portRepository.GetSinglePort(tenant, myLastDelivery.ToPortId);
                                    if (myPort != null)
                                    {
                                        provider.FinalDestination = myPort.EnglishName;
                                    }
                                }

                                break;
                            }

                        case "CASL":
                            {
                                string myCity = myLastDelivery.ToAddressCity;
                                if (!string.IsNullOrEmpty(myCity))
                                {
                                    provider.FinalDestination = myCity;
                                }

                                break;
                            }
                    }
                }

                else if (!string.IsNullOrEmpty(shipment.OnForwardingToPortId))
                {
                    provider.FinalDestination = shipment.OnForwardingTo;
                }

                else if (!string.IsNullOrEmpty(shipment.OnCarriageToPortId))
                {
                    provider.FinalDestination = shipment.OnCarriageTo;
                }

                else
                {
                    if (shipment.Transshipment3ToPortId != null)
                    {
                        provider.FinalDestination = shipment.Transshipment3ToPortName;
                    }
                    else if (shipment.Transshipment2ToPortId != null)
                    {
                        provider.FinalDestination = shipment.Transshipment2ToPortName;
                    }
                    else if (shipment.Transshipment1ToPortId != null)
                    {
                        provider.FinalDestination = shipment.Transshipment1ToPortName;
                    }
                    else if (shipment.MainCarriageToPortId != null)
                    {
                        provider.FinalDestination = shipment.MainCarriageToPortName;
                    }
                }

                if (totalData.Partner == "Agent Name")
                {
                    provider.PartnerName = !string.IsNullOrEmpty(shipment.AgentName) ? shipment.AgentName : "";
                }
                else if (totalData.Partner == "Customer Name")
                {
                    provider.PartnerName = !string.IsNullOrEmpty(shipment.CustomerName) ? shipment.CustomerName : "";
                }

                if (partnerId == null)
                {
                    if (shipment.ShipmentLevelCode == "D")
                    {
                        provider.PartnerName = !string.IsNullOrEmpty(shipment.CustomerName) ? shipment.CustomerName : "";
                    }
                    else if (shipment.ShipmentLevelCode == "C")
                    {
                        provider.PartnerName = !string.IsNullOrEmpty(shipment.AgentName) ? shipment.AgentName : "";
                    }

                }

                if (!string.IsNullOrEmpty(shipment.ShipmentTypeName))
                {
                    if (shipment.ShipmentTypeName == "FCL")
                    {
                        provider.Type = string.IsNullOrEmpty(shipment.ContainerCode) ? "" : shipment.ContainerCode;
                    }
                    else
                    {
                        provider.Type = shipment.ShipmentTypeName;
                    }
                }
                else
                {
                    provider.Type = "";
                }

                provider.Vessel_Voyage = shipment.Voyage;

                if (!string.IsNullOrEmpty(shipment.VesselName))
                {
                    provider.VesselName = shipment.VesselName;

                    if (string.IsNullOrEmpty(provider.Vessel_Voyage))
                    {
                        provider.Vessel_Voyage = shipment.VesselName;
                    }

                    else
                    {
                        provider.Vessel_Voyage = shipment.VesselName + " - " + shipment.Voyage;
                    }
                }

                if (!string.IsNullOrEmpty(shipment.AgentReference1))
                {
                    if (!string.IsNullOrEmpty(shipment.AgentReference2))
                    {
                        provider.AgentReference = shipment.AgentReference1 + " , " + shipment.AgentReference2;
                    }
                    else
                    {
                        provider.AgentReference = shipment.AgentReference1;
                    }
                }
                else if (!string.IsNullOrEmpty(shipment.AgentReference2))
                {
                    provider.AgentReference = shipment.AgentReference2;
                }
                else
                {
                    provider.AgentReference = "";
                }

                if (!string.IsNullOrEmpty(shipment.CustomerReference1))
                {
                    if (!string.IsNullOrEmpty(shipment.CustomerReference2))
                    {
                        provider.CustomerReference = shipment.CustomerReference1 + " , " + shipment.CustomerReference2;
                    }
                    else
                    {
                        provider.CustomerReference = shipment.CustomerReference1;
                    }
                }
                else if (!string.IsNullOrEmpty(shipment.CustomerReference2))
                {
                    provider.CustomerReference = shipment.CustomerReference2;
                }
                else
                {
                    provider.CustomerReference = "";
                }

                string inv = "";
                foreach (ARInvoice item in invoices)
                {
                    inv += item.InvoiceNumber + " - ";
                }

                if (inv.EndsWith(" - "))
                {
                    inv = inv.Remove(inv.Length - 3, 3);
                }

                provider.OurInvoice = inv;

                CustomFieldResolver customFieldResolver = new CustomFieldResolver();
                customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, shipment, provider);

                if (string.IsNullOrEmpty(provider.ContainerNumber))
                {
                    if (!totalData.Records.Where(d => d.ShipmentNumber == provider.ShipmentNumber && string.IsNullOrEmpty(d.ContainerNumber) && d.Type == provider.Type).Any())
                    {
                        totalData.Records.Add(provider);
                    }
                }
                else
                {
                    totalData.Records.Add(provider);
                }
            }

            return totalData;
        }
    }
}
