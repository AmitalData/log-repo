using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Services;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;

using WebFreight.Web.DataProviders;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using WebFreight.Web.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.ReportsWebServices
{
    /// <summary>  
    /// Summary description for DeliveryNoteWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DeliveryNoteWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public byte[] GetPickupData(string entityId, string entityobjecttableId, string childentityId, string childentityobjecttableId, int tenant)
        {
            DeliveryNoteDataProvider deliveryNotedataprovider = GetPickupDataProvider(entityId, entityobjecttableId, childentityId, childentityobjecttableId, tenant);
            XmlSerializer serializer = new XmlSerializer(typeof(DeliveryNoteDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, deliveryNotedataprovider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        [WebMethod]
        public byte[] GetDeliveryData(string entityId,string entityobjecttableId,string childentityId,string childentityobjecttableId, int tenant)
        {
            DeliveryNoteDataProvider deliveryNotedataprovider = GetDeliveryDataProvider(entityId, entityobjecttableId, childentityId, childentityobjecttableId, tenant);
            XmlSerializer serializer = new XmlSerializer(typeof(DeliveryNoteDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, deliveryNotedataprovider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        private DeliveryNoteDataProvider GetPickupDataProvider(string entityId, string entityobjecttableId, string childentityId, string childentityobjecttableId, int tenant)
        {
            DeliveryNoteDataProvider deliveryNotedataprovider = new DeliveryNoteDataProvider();
            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            IWebFreightContext webfreightcontext = WebFreightContext.GetContext(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
            PortRepository portRepository = new PortRepository(commonContext);
            CountryRepository countryRepository = new CountryRepository(commonContext);
            AddressRepository addressRepository = new AddressRepository(commonContext);
            CardQuery cardQuery = new CardQuery(tenant);

            ShipmentPM shipment = null;

            Tenant tenantSettings = (from a in commonContext.Tenants
                                     where a.Id == tenant
                                     select a).FirstOrDefault();

            ObjectTable entityObjectTable = (from co in webfreightcontext.ObjectTables
                                             where co.Id == entityobjecttableId
                                             select co).FirstOrDefault();
            if (entityObjectTable != null)
            {
                switch (entityObjectTable.Name)
                {
                    case "Shipment":
                        shipment = shipmentQuery.GetSinglePM(entityId, tenant);
                        break;
                }
            }

            else // Default values for now
            {
                shipment = shipmentQuery.GetSinglePM(entityId, tenant);
            }
          
            ObjectTable childEntityObjectTable = (from co in webfreightcontext.ObjectTables
                                                  where co.Id == childentityobjecttableId
                                                  select co).FirstOrDefault();

            ShipmentPickUpDelivery myPickup = (from a in shipmentsContext.ShipmentPickUpDeliveries
                                               where a.Id == childentityId && a.Tenant == tenant && a.ShipmentId == entityId && a.PickUpDeliveryTypeCode == "PICK"
                                               select a).FirstOrDefault();
            
            if (shipment != null && tenantSettings != null)
            {
                ContactQuery contactQuery = new ContactQuery(tenant);
                string email = AuthenticationUtil.GetLoggedUserEmail(tenant);
                ContactPM loggedContact = contactQuery.GetContactByEmailOnly(email, tenant);
                if (loggedContact != null)
                {
                    deliveryNotedataprovider.UserName = loggedContact.EnglishName;
                }

                deliveryNotedataprovider.ITNumber = shipment.ITNumber;
                deliveryNotedataprovider.ProjectNumber = shipment.ProjectNumber != null ? shipment.ProjectNumber : "";
                deliveryNotedataprovider.HAWB = shipment.House != null ? shipment.House : "";
                deliveryNotedataprovider.OurReferenceNumber = shipment.ShipmentNumber != null ? shipment.ShipmentNumber : "";
                deliveryNotedataprovider.DateSent = String.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date);
                deliveryNotedataprovider.DateSent_New = DateTime.Now.Date;
                deliveryNotedataprovider.ClientReferenceNumber = shipment.ShipperReference1 != null ? shipment.ShipperReference1 : "";
                deliveryNotedataprovider.MainCarriageETA = shipment.MainCarriageETA != null ? String.Format("{0:dd MMM yyyy}", shipment.MainCarriageETA) : "";
                deliveryNotedataprovider.MainCarriageETD = shipment.MainCarriageETD != null ? String.Format("{0:dd MMM yyyy}", shipment.MainCarriageETD) : "";
                deliveryNotedataprovider.MainCarriageVesselName = shipment.MainCarriageVesselName;
                deliveryNotedataprovider.MainCarriageCarrierName = shipment.MainCarriageCarrierName;                
                deliveryNotedataprovider.BookingNumber = shipment.BookingConfirmationNumber;
                deliveryNotedataprovider.MasterNumber = shipment.Master;
                deliveryNotedataprovider.ShipmentNotes = shipment.Notes;
                deliveryNotedataprovider.ConsigneeRef1 = shipment.ConsigneeReference1;
                deliveryNotedataprovider.LongMaster = shipment.LongMaster;
                deliveryNotedataprovider.ShipperReference2 = shipment.ShipperReference2;
                deliveryNotedataprovider.ConsigneeReference2 = shipment.ConsigneeReference2;
                deliveryNotedataprovider.ShipmentSalesman = shipment.SalesmanUserName;
                deliveryNotedataprovider.LastFreeDate = shipment.WarehouseLegLastFreeDate;
                deliveryNotedataprovider.FinalDestinationCode = shipment.MainCarriageFinalDestinationPortCode;
                deliveryNotedataprovider.AMSBL = shipment.AMSBL;

                if (!string.IsNullOrEmpty(shipment.FreightLocationId))
                {
                    CardPM cardPM = cardQuery.GetSinglePM(shipment.FreightLocationId, tenant);
                    if (cardPM != null)
                    {
                        deliveryNotedataprovider.FreightLocation = cardPM.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(shipment.OnCarriageCarrierId))
                {
                    deliveryNotedataprovider.OnCarriageCarrier = shipment.OnCarriageCarrierName;
                }

                if (shipment.CutoffDate != null)
                {
                    deliveryNotedataprovider.CutOffDate = String.Format("{0:dd MMM yyyy}", shipment.CutoffDate);
                    deliveryNotedataprovider.CutOffDateAsDate = shipment.CutoffDate;
                    deliveryNotedataprovider.CutOffTime = shipment.CutoffDate != null ? String.Format("{0:hh:mm:ss}", shipment.CutoffDate) : "";
                }

                if (!string.IsNullOrEmpty(shipment.ConsigneeId))
                {
                    #region
                    Card myCard = CardRepository.GetSingleCard(shipment.ConsigneeId, tenant, true);
                    if (myCard != null)
                    {
                        deliveryNotedataprovider.ConsigneeName = myCard.EnglishName;

                        if (!string.IsNullOrEmpty(shipment.ConsigneeAddressId))
                        {
                            Address myAddress = addressRepository.GetSingleAddress(shipment.ConsigneeAddressId, tenant);
                            if (myAddress != null)
                            {
                                deliveryNotedataprovider.ConsigneeAddress = DataProviders.General.GetAddress(myAddress);

                                if (!string.IsNullOrEmpty(myAddress.PhoneNumber) || !string.IsNullOrEmpty(myAddress.FaxNumber))
                                {
                                    deliveryNotedataprovider.ConsigneeAddress += Environment.NewLine;
                                }

                                if (!string.IsNullOrEmpty(myAddress.PhoneNumber))
                                {
                                    deliveryNotedataprovider.ConsigneeAddress += "Tel: " + myAddress.PhoneNumber + " ";
                                }

                                if (!string.IsNullOrEmpty(myAddress.FaxNumber))
                                {
                                    deliveryNotedataprovider.ConsigneeAddress += "Fax: " + myAddress.FaxNumber;
                                }
                            }
                        }
                    }
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.ShipperId))
                {
                    #region
                    Card myCard = CardRepository.GetSingleCard(shipment.ShipperId, tenant, true);
                    if (myCard != null)
                    {
                        deliveryNotedataprovider.ShipperName = myCard.EnglishName;

                        if (!string.IsNullOrEmpty(shipment.ShipperAddressId))
                        {
                            Address myAddress = addressRepository.GetSingleAddress(shipment.ShipperAddressId, tenant);
                            if (myAddress != null)
                            {
                                deliveryNotedataprovider.ShipperAddress = DataProviders.General.GetAddress(myAddress);

                                if (!string.IsNullOrEmpty(myAddress.PhoneNumber) || !string.IsNullOrEmpty(myAddress.FaxNumber))
                                {
                                    deliveryNotedataprovider.ShipperAddress += Environment.NewLine;
                                }

                                if (!string.IsNullOrEmpty(myAddress.PhoneNumber))
                                {
                                    deliveryNotedataprovider.ShipperAddress += "Tel: " + myAddress.PhoneNumber + " ";
                                }

                                if (!string.IsNullOrEmpty(myAddress.FaxNumber))
                                {
                                    deliveryNotedataprovider.ShipperAddress += "Fax: " + myAddress.FaxNumber;
                                }
                            }
                        }
                    }
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.IssuingCarrierAgentId))
                {
                    #region
                    Card myCard = CardRepository.GetSingleCard(shipment.IssuingCarrierAgentId, tenant, true);
                    if (myCard != null)
                    {
                        deliveryNotedataprovider.IssuingCarrierAgentName = myCard.EnglishName;

                        if (!string.IsNullOrEmpty(shipment.IssuingCarrierAddressId))
                        {
                            Address myAddress = addressRepository.GetSingleAddress(shipment.IssuingCarrierAddressId, tenant);
                            if (myAddress != null)
                            {
                                deliveryNotedataprovider.IssuingCarrierAgentAddress = DataProviders.General.GetAddress(myAddress);

                                if (!string.IsNullOrEmpty(myAddress.PhoneNumber) || !string.IsNullOrEmpty(myAddress.FaxNumber))
                                {
                                    deliveryNotedataprovider.IssuingCarrierAgentAddress += Environment.NewLine;
                                }

                                if (!string.IsNullOrEmpty(myAddress.PhoneNumber))
                                {
                                    deliveryNotedataprovider.IssuingCarrierAgentAddress += "Tel: " + myAddress.PhoneNumber + " ";
                                }

                                if (!string.IsNullOrEmpty(myAddress.FaxNumber))
                                {
                                    deliveryNotedataprovider.IssuingCarrierAgentAddress += "Fax: " + myAddress.FaxNumber;
                                }
                            }
                        }
                    }
                    #endregion
                }
                
                if (!string.IsNullOrEmpty(shipment.SalesmanUserId))
                {
                    #region
                    Contact salesmanContact = ContactRepository.GetSingleContact(shipment.SalesmanUserId, tenant, true);
                    if (salesmanContact != null)
                    {
                        deliveryNotedataprovider.SalesmanEmail = salesmanContact.Email;
                    }
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.CustomerContactId))
                {
                    Contact contact = commonContext.Contacts.Where(d => d.Id == shipment.CustomerContactId && d.Tenant == tenant).FirstOrDefault();
                    if(contact != null)
                    {
                        deliveryNotedataprovider.CustomerContactName = contact.EnglishName;
                        deliveryNotedataprovider.CustomerContactPhoneNumber = contact.BusinessPhone;                        
                    }
                }

                // tenant data
                if (tenantSettings != null)
                {
                    deliveryNotedataprovider.Signature = tenantSettings.Signature != null ? tenantSettings.Signature : "";
                    deliveryNotedataprovider.TenantName = tenantSettings.Company != null ? tenantSettings.Company : "";
                }

                if (myPickup != null)
                {
                    #region

                    deliveryNotedataprovider.DriverName = myPickup.Driver;

                    ShipmentPackage myShipmentPackage = (from a in shipmentsContext.ShipmentPackages
                                                         where a.DeliveryId == myPickup.Id && a.Tenant == tenant
                                                         select a).FirstOrDefault();

                    if (myShipmentPackage != null)
                    {
                        deliveryNotedataprovider.Reference1 = myShipmentPackage.Reference1;
                        deliveryNotedataprovider.Reference2 = myShipmentPackage.Reference2;
                        deliveryNotedataprovider.Reference3 = myShipmentPackage.Reference3;
                        deliveryNotedataprovider.Reference4 = myShipmentPackage.Reference4;
                    }

                    if (myPickup.CarrierId != null)
                    {
                        Card trucker = CardRepository.GetSingleCard(myPickup.CarrierId, tenant, true);

                        if (trucker != null)
                        {
                            deliveryNotedataprovider.To = trucker.EnglishName != null ? trucker.EnglishName : "";

                            Address address = addressRepository.GetSingleAddressByCardIdAndTypeId(trucker.Id, "M", tenant);

                            ContactRepository contactRep = new ContactRepository(commonContext);
                            Contact truckerContact = null;

                            CardContact truckerCardContact = (from cc in commonContext.CardContacts
                                                              where cc.CardId == trucker.Id
                                                              select cc).FirstOrDefault();

                            if (truckerCardContact != null)
                            {
                                truckerContact = contactRep.GetSingleContact(truckerCardContact.ContactId, myPickup.Tenant);
                            }

                            if (truckerContact != null)
                            {
                                deliveryNotedataprovider.Salesman = truckerContact.EnglishName != null ? truckerContact.EnglishName : "";
                                deliveryNotedataprovider.SalesmanEmail = truckerContact.Email != null ? truckerContact.Email : "";
                            }

                            deliveryNotedataprovider.Telephone = address != null ? (address.PhoneNumber != null ? address.PhoneNumber : "") : "";
                        }
                    }

                    #region Pickup From
                    deliveryNotedataprovider.PickupDate = myPickup.ETD != null ? String.Format("{0:dd/MMM/yyyy}", myPickup.ETD) : "";
                    deliveryNotedataprovider.PickupTime = myPickup.ETD != null ? String.Format("{0:hh:mm}", myPickup.ETD) : "";
                    deliveryNotedataprovider.PickupTime_DateTime_New = myPickup.ETD != null ? myPickup.ETD : null;
                    deliveryNotedataprovider.SpecialInstructions = myPickup.Notes != null ? myPickup.Notes : "";

                    #region From PART
                    if (myPickup.PickUpDeliveryFromTypeCode == "PART")
                    {
                        if (string.IsNullOrEmpty(myPickup.FromPartnerCardId))
                        {
                            deliveryNotedataprovider.PickupAddress = myPickup.FromAddress != null ? myPickup.FromAddress : "";
                        }

                        else
                        {
                            Card card = CardRepository.GetSingleCard(myPickup.FromPartnerCardId, tenant, true);
                            if (card != null)
                            {
                                deliveryNotedataprovider.PickupCompanyName = card.EnglishName;

                                CardContact cardContact = (from a in commonContext.CardContacts where a.CardId == card.Id select a).FirstOrDefault();
                                if (cardContact != null)
                                {
                                    Contact contact = cardContact.Contact;

                                    if (contact != null)
                                    {
                                        deliveryNotedataprovider.PickupContactPhone = contact.BusinessPhone != null ? contact.BusinessPhone : "";
                                    }
                                }

                                if (string.IsNullOrEmpty(myPickup.FromAddressId))
                                {
                                    deliveryNotedataprovider.PickupAddress = myPickup.FromAddress != null ? myPickup.FromAddress : "";
                                }

                                else
                                {
                                    Address address = addressRepository.GetSingleAddress(myPickup.FromAddressId, tenant);
                                    if (address != null)
                                    {

                                        deliveryNotedataprovider.PickupAddress = DataProviders.General.GetAddress(address)
                                            + (!string.IsNullOrEmpty(address.ATTN) ? (Environment.NewLine + "Contact : " + address.ATTN) : "")
                                            + (!string.IsNullOrEmpty(address.PhoneNumber) ? (Environment.NewLine + "Phone : " + address.PhoneNumber) : "");
                                        deliveryNotedataprovider.FromAddressDescription = address.Description != null ? address.Description : "";

                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region From PORT
                    else if (myPickup.PickUpDeliveryFromTypeCode == "PORT")
                    {
                        if (!string.IsNullOrEmpty(myPickup.FromPortId))
                        {
                            Port port = portRepository.GetSinglePort(tenant, myPickup.FromPortId);
                            if (port != null)
                            {
                                deliveryNotedataprovider.PickupCompanyName = port.EnglishName != null ? port.EnglishName : "";
                            }
                        }

                        deliveryNotedataprovider.PickupAddress = myPickup.FromAddress != null ? myPickup.FromAddress : "";
                    }
                    #endregion

                    #region From CASL
                    else
                    {
                        string location = "";

                        if (!string.IsNullOrEmpty(myPickup.FromAddressCity))
                        {
                            location = myPickup.FromAddressCity;
                        }

                        if (!string.IsNullOrEmpty(myPickup.FromAddressZipCode))
                        {
                            location = location + " " + myPickup.FromAddressZipCode;
                        }

                        if (!string.IsNullOrEmpty(myPickup.FromAddressCountryId))
                        {
                            Country country = countryRepository.GetSingleCountry(myPickup.FromAddressCountryId, tenant);
                            if (country != null)
                            {
                                location = location + Environment.NewLine + country.EnglishName;
                            }
                        }

                        deliveryNotedataprovider.PickupAddress = location;
                    }
                    #endregion

                    #endregion

                    #region Pickup To
                    deliveryNotedataprovider.DeliveryDate = myPickup.ETA != null ? String.Format("{0:dd/MMM/yyyy}", myPickup.ETA) : "";
                    deliveryNotedataprovider.DeliveryTime = myPickup.ETA != null ? String.Format("{0:hh:mm}", myPickup.ETA) : "";
                    deliveryNotedataprovider.DeliveryTime_DateTime_New = myPickup.ETA != null ? myPickup.ETA : null;

                    #region To PART
                    if (myPickup.PickUpDeliveryToTypeCode == "PART")
                    {
                        if (string.IsNullOrEmpty(myPickup.ToPartnerCardId))
                        {
                            deliveryNotedataprovider.DeliveryAddress = myPickup.ToAddress != null ? myPickup.ToAddress : "";
                        }

                        else
                        {
                            Card card = CardRepository.GetSingleCard(myPickup.ToPartnerCardId, tenant, true);
                            if (card != null)
                            {
                                deliveryNotedataprovider.DeliveryCompanyName = card.EnglishName;
                                deliveryNotedataprovider.DeliveryCompanyContactLocalName = card.LocalName;

                                CardContact cardContact = (from a in commonContext.CardContacts where a.CardId == card.Id select a).FirstOrDefault();
                                if (cardContact != null)
                                {
                                    ContactRepository contactRepository = new ContactRepository(commonContext);
                                    Contact contact = contactRepository.GetSingleContact(cardContact.ContactId, tenant);

                                    if (contact != null)
                                    {
                                        deliveryNotedataprovider.DeliveryContactPhone = contact.BusinessPhone != null ? contact.BusinessPhone : "";
                                        deliveryNotedataprovider.DeliveryContactName = contact.EnglishName != null ? contact.EnglishName : "";
                                    }
                                }

                                if (string.IsNullOrEmpty(myPickup.ToAddressId))
                                {
                                    deliveryNotedataprovider.DeliveryAddress = myPickup.ToAddress != null ? myPickup.ToAddress : "";
                                }

                                else
                                {
                                    Address address = addressRepository.GetSingleAddress(myPickup.ToAddressId, tenant);
                                    if (address != null)
                                    {

                                        deliveryNotedataprovider.DeliveryAddress = DataProviders.General.GetAddress(address)
                                            + (!string.IsNullOrEmpty(address.ATTN) ? (Environment.NewLine + "Contact : " + address.ATTN) : "")
                                            + (!string.IsNullOrEmpty(address.PhoneNumber) ? (Environment.NewLine + "Phone : " + address.PhoneNumber) : "");
                                        deliveryNotedataprovider.ToAddressDescription = address.Description != null ? address.Description : "";

                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region To PORT
                    else if (myPickup.PickUpDeliveryToTypeCode == "PORT")
                    {
                        if (!string.IsNullOrEmpty(myPickup.ToPortId))
                        {
                            Port port = portRepository.GetSinglePort(tenant, myPickup.ToPortId);
                            if (port != null)
                            {
                                deliveryNotedataprovider.DeliveryCompanyName = port.EnglishName != null ? port.EnglishName : "";
                            }
                        }

                        deliveryNotedataprovider.DeliveryAddress = myPickup.ToAddress != null ? myPickup.ToAddress : "";
                    }
                    #endregion

                    #region From CASL
                    else
                    {
                        string location = "";

                        if (!string.IsNullOrEmpty(myPickup.ToAddressCity))
                        {
                            location = myPickup.ToAddressCity;
                        }

                        if (!string.IsNullOrEmpty(myPickup.ToAddressZipCode))
                        {
                            location = location + " " + myPickup.ToAddressZipCode;
                        }

                        if (!string.IsNullOrEmpty(myPickup.ToAddressCountryId))
                        {
                            Country country = countryRepository.GetSingleCountry(myPickup.ToAddressCountryId, tenant);
                            if (country != null)
                            {
                                location = location + Environment.NewLine + country.EnglishName;
                            }
                        }

                        deliveryNotedataprovider.DeliveryAddress = location;
                    }
                    #endregion

                    #endregion

                    #region Packages
                    deliveryNotedataprovider.DescriptionOfGoods = shipment.DescriptionOfGoods != null ? shipment.DescriptionOfGoods : "";
                    List<ShipmentPickUpDeliveryPackage> packages = shipmentsContext.ShipmentPickUpDeliveryPackages.Where(d => d.ShipmentPickUpDeliveryId == myPickup.Id && d.Tenant == tenant).ToList();
                    List<string> shipmentPackagesIds = shipmentsContext.ShipmentPackages.Where(d => d.ShipmentId == shipment.Id && d.Tenant == tenant).Select(s => s.Id).ToList();

                    if (shipmentPackagesIds != null && shipmentPackagesIds.Count > 0)
                    {
                        List<InsideShipmentPackage> insidePackages = shipmentsContext.InsideShipmentPackages.Where(d => shipmentPackagesIds.Contains(d.ShipmentPackageId)).ToList();

                        if (insidePackages != null && insidePackages.Count > 0)
                        {
                            deliveryNotedataprovider.InsidePackagesLines = new List<InsidePackageLine>();

                            foreach (InsideShipmentPackage item in insidePackages)
                            {
                                InsidePackageLine insidePackageLine = new InsidePackageLine();

                                PackageType insidePackageType = (from pa in commonContext.PackageTypes
                                                                 where pa.Id == item.PackageTypeId
                                                                 select pa).FirstOrDefault();

                                insidePackageLine.PackageType = insidePackageType == null ? "" : insidePackageType.EnglishName;
                                insidePackageLine.Quantity = item.Quantity;

                                if (item.Length != null && item.Width != null && item.Height != null)
                                {
                                    insidePackageLine.Dimensions = item.Length + "x" + item.Width + "x" + item.Height;
                                }

                                insidePackageLine.Volume = item.Volume;
                                insidePackageLine.VolumetricWeight = item.VolumetricWeight;
                                insidePackageLine.Weight = item.Weight;
                                insidePackageLine.Description = item.Description;

                                #region Car Details
                                insidePackageLine.Make = item.Make;
                                insidePackageLine.Model = item.Model;
                                insidePackageLine.Year = item.Year;
                                insidePackageLine.Color = item.Color;
                                insidePackageLine.ChassisNumber = item.ChassisNumber;
                                insidePackageLine.RegistrationNumber = item.RegistrationNumber;

                                if (!string.IsNullOrEmpty(item.CountryId))
                                {
                                    Country country = CountryRepository.GetSingleCountry(item.CountryId, tenant, true);
                                    if (country != null)
                                    {
                                        insidePackageLine.CountryName = country.EnglishName;
                                    }
                                }
                                #endregion

                                deliveryNotedataprovider.InsidePackagesLines.Add(insidePackageLine);
                            }
                        }
                    }

                    deliveryNotedataprovider.PackagesLines = new List<PackageLine>();
                    deliveryNotedataprovider.AttachmentList = new List<PackageLine>();
                    int counter = 1;

                    string volumeUnitCode = shipment.VolumeUnitCode != null ? shipment.VolumeUnitCode : "";
                    int? totalQuantity = 0;
                    double? totalWeight = 0;
                    double? totalVolume = 0;

                    foreach (ShipmentPickUpDeliveryPackage package in packages)
                    {
                        counter++;
                        PackageLine packageline = new PackageLine();

                        totalQuantity += package.Quantity;
                        totalWeight += package.Weight;
                        totalVolume += package.Volume;

                        packageline.PackageDescriptionOfGoods = package.Description != null ? package.Description : "";
                        packageline.PackageGrossWeight = package.Weight != null ? (package.Weight.Value.ToString() + " " + shipment.GrossWeightUnitCode) : "";
                        packageline.PackageQuantity = package.Quantity != null ? package.Quantity.Value.ToString() : "";
                        packageline.PackageType = package.PackageType != null ? package.PackageType.EnglishName : "";
                        packageline.PackageVolume = package.Volume != null ? (package.Volume + " " + shipment.VolumeUnitCode) : "";
                        packageline.SealNumber = package.ShipperSeal;

                        packageline.Width = package.Width == null ? "" : package.Width.ToString();
                        packageline.Height = package.Height == null ? "" : package.Height.ToString();
                        packageline.Length = package.Length == null ? "" : package.Length.ToString();

                        #region Car Details
                        packageline.Make = package.Make;
                        packageline.Model = package.Model;
                        packageline.Year = package.Year;
                        packageline.Color = package.Color;
                        packageline.ChassisNumber = package.ChassisNumber;
                        packageline.RegistrationNumber = package.RegistrationNumber;

                        if (!string.IsNullOrEmpty(package.CountryId))
                        {
                            Country country = CountryRepository.GetSingleCountry(package.CountryId, tenant, true);
                            if (country != null)
                            {
                                packageline.CountryName = country.EnglishName;
                            }
                        }
                        #endregion

                        if (package.Width != null && package.Height != null && package.Length != null)
                        {
                            packageline.Dimensions = package.Length + " x " + package.Width + " x " + package.Height + " " + shipment.DimensionsUnitCode;
                        }

                        packageline.ContainerNumber = package.ContainerNumber;

                        PackageType packtype = (from pa in commonContext.PackageTypes
                                                where pa.Id == package.PackageTypeId
                                                select pa).FirstOrDefault();

                        if (packtype != null)
                        {
                            packageline.ContainerSize = packtype.ContainerSize.ToString();
                            packageline.PackageType = packtype.EnglishName != null ? packtype.EnglishName : "";
                            packageline.IsContainer = packtype.IsContainer;
                        }

                        #region Harmonize
                        if (package.IsMultiHarmonize)
                        {
                            List<PickUpDeliveryPackageHarmonize> allHarmonizes = shipmentsContext.PickUpDeliveryPackageHarmonizes.Where(d => d.PackageId == package.Id && d.Tenant == package.Tenant).ToList();
                            foreach (PickUpDeliveryPackageHarmonize itemHarmonize in allHarmonizes)
                            {
                                if (string.IsNullOrEmpty(packageline.HSCode))
                                {
                                    packageline.HSCode = itemHarmonize.Harmonize;
                                }
                                else
                                {
                                    packageline.HSCode += "," + itemHarmonize.Harmonize;
                                }
                            }
                        }
                        else
                        {
                            packageline.HSCode = package.Harmonize;
                        }
                        #endregion

                        deliveryNotedataprovider.PackagesLines.Add(packageline);
                    }

                    deliveryNotedataprovider.TotalNumberOfPackages = totalQuantity;
                    deliveryNotedataprovider.TotalGrossWeight = totalWeight;
                    deliveryNotedataprovider.TotalVolume = totalVolume;
                    #endregion

                    #region Document
                    DocumentType currentdocumentType = commonContext.DocumentTypes.Where(doc => doc.Code == "781" && doc.Tenant == tenant).FirstOrDefault();
                    if (currentdocumentType != null)
                    {
                        DocumentTypeTemplate currentTemplate = commonContext.DocumentTypeTemplates.Where(doc => doc.Id == currentdocumentType.DocumentTypeDefaultReportTemplateId && doc.Tenant == tenant).FirstOrDefault();
                        if (currentTemplate != null)
                        {
                            deliveryNotedataprovider.VerticalShift = currentTemplate.VerticalShift != null ? currentTemplate.VerticalShift.Value : 10;
                            deliveryNotedataprovider.HorizontalShift = currentTemplate.HorizontalShift != null ? currentTemplate.HorizontalShift.Value : 10;
                        }
                    }
                    #endregion

                    #region  Last Vessel
                    string vesselNameAndNumber = "";

                    if (!string.IsNullOrEmpty(shipment.Transshipment3FromPortId))
                    {
                        Vessel vessel = (from a in commonContext.Vessels
                                         where a.Id == shipment.Transshipment3VesselId
                                         select a).FirstOrDefault();
                        if (vessel != null)
                        {
                            vesselNameAndNumber = vessel.EnglishName + " \\ " + shipment.MainCarriageCarrierNumber;
                        }
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(shipment.Transshipment2FromPortId))
                        {
                            Vessel vessel = (from a in commonContext.Vessels
                                             where a.Id == shipment.Transshipment2VesselId
                                             select a).FirstOrDefault();
                            if (vessel != null)
                            {
                                vesselNameAndNumber = vessel.EnglishName + " \\ " + shipment.MainCarriageCarrierNumber;
                            }
                        }

                        else
                        {
                            if (!string.IsNullOrEmpty(shipment.Transshipment1FromPortId))
                            {
                                Vessel vessel = (from a in commonContext.Vessels
                                                 where a.Id == shipment.Transshipment1VesselId
                                                 select a).FirstOrDefault();
                                if (vessel != null)
                                {
                                    vesselNameAndNumber = vessel.EnglishName + " \\ " + shipment.MainCarriageCarrierNumber;
                                }
                            }

                            else
                            {
                                Vessel vessel = (from a in commonContext.Vessels
                                                 where a.Id == shipment.MainCarriageVesselId
                                                 select a).FirstOrDefault();
                                if (vessel != null)
                                {
                                    vesselNameAndNumber = vessel.EnglishName + " \\ " + shipment.MainCarriageCarrierNumber;
                                }
                            }
                        }
                    }

                    deliveryNotedataprovider.LastMainCarriageVesselNameAndNumber = vesselNameAndNumber;

                    if (!string.IsNullOrEmpty(shipment.MainCarriageVesselId))
                    {
                        var vesselMCNameAndNumber = ""; 
                        Vessel vessel = (from a in commonContext.Vessels
                                         where a.Id == shipment.MainCarriageVesselId
                                         select a).FirstOrDefault();
                        if (vessel != null)
                        {
                            vesselMCNameAndNumber = vessel.EnglishName + " \\ " + shipment.MainCarriageCarrierNumber;
                        }
                        deliveryNotedataprovider.MainCarriageVesselNameAndNumber = vesselMCNameAndNumber;
                    }

                    #endregion

                    #region EmptyContainer
                    deliveryNotedataprovider.EmptyContainerRef = myPickup.EmptyPickupDepotReference;

                    if (!string.IsNullOrEmpty(myPickup.EmptyPickupContainerPartnerId))
                    {
                        CardPM cardPM = cardQuery.GetSinglePM(myPickup.EmptyPickupContainerPartnerId, tenant);

                        if (cardPM != null)
                        {
                            string myEmptyContainer = null;

                            myEmptyContainer = cardPM.EnglishName != null ? cardPM.EnglishName : "";

                            if (cardPM.MainAddressId != null)
                            {
                                Address theAddress = addressRepository.GetSingleAddress(cardPM.MainAddressId, tenant);

                                if (theAddress != null)
                                {
                                    if (theAddress.IsLocalLanguage && !string.IsNullOrEmpty(cardPM.LocalName))
                                    {
                                        myEmptyContainer = cardPM.LocalName;
                                    }

                                    myEmptyContainer = myEmptyContainer + Environment.NewLine + DataProviders.General.GetAddress(theAddress);

                                    if (theAddress.PhoneNumber != null)
                                    {
                                        myEmptyContainer = myEmptyContainer + Environment.NewLine + "Phone No. : " + theAddress.PhoneNumber;
                                    }

                                    if (theAddress.FaxNumber != null)
                                    {
                                        myEmptyContainer = myEmptyContainer + "   Fax No. : " + theAddress.FaxNumber;
                                    }
                                }
                            }

                            deliveryNotedataprovider.EmptyContainer = myEmptyContainer;
                        }
                    }
                    #endregion

                    //LoadingPortName
                    if (!string.IsNullOrEmpty(shipment.MainCarriageFromPortId))
                    {
                        deliveryNotedataprovider.LoadingPortName = shipment.MainCarriageFromPortName;
                        deliveryNotedataprovider.OriginCountry = shipment.MainCarriageFromPortCountryName;
                    }

                    //DischargePortName
                    if (!string.IsNullOrEmpty(shipment.Transshipment3ToPortId))
                    {
                        deliveryNotedataprovider.DischargePortName = shipment.Transshipment3ToPortName;
                    }

                    else if (!string.IsNullOrEmpty(shipment.Transshipment2ToPortId))
                    {
                        deliveryNotedataprovider.DischargePortName = shipment.Transshipment2ToPortName;
                    }

                    else if (!string.IsNullOrEmpty(shipment.Transshipment1ToPortId))
                    {
                        deliveryNotedataprovider.DischargePortName = shipment.Transshipment1ToPortName;
                    }

                    else if (!string.IsNullOrEmpty(shipment.MainCarriageToPortId))
                    {
                        deliveryNotedataprovider.DischargePortName = shipment.MainCarriageToPortName;
                    }

                    //Dates
                    deliveryNotedataprovider.MainCarriageETD_DateTime = shipment.MainCarriageETD;
                    deliveryNotedataprovider.MainCarriageETA_DateTime = shipment.MainCarriageETA;
                    deliveryNotedataprovider.MainCarriageATA_DateTime = shipment.MainCarriageATA;

                    if (myPickup.TransportModeCode != null)
                    {
                        PickUpDeliveryTransportMode myTransportMode = shipmentsContext.PickUpDeliveryTransportModes.Where(d => d.Code == myPickup.TransportModeCode).FirstOrDefault();
                        if (myTransportMode != null)
                        {
                            deliveryNotedataprovider.TransportMode = myTransportMode.Name;
                        }
                    }

                    #endregion
                }

                CustomFieldResolver customFieldResolver = new CustomFieldResolver();
                customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, shipment, deliveryNotedataprovider);
            }

            try
            {
                Type pickupNoteType = deliveryNotedataprovider.GetType();
                PropertyInfo[] properties = pickupNoteType.GetProperties();
                foreach (PropertyInfo pi in properties)
                {
                    string propertytype = pi.PropertyType.FullName;

                    if (propertytype == "System.String")
                    {

                        if (pi.GetValue(deliveryNotedataprovider, null) == null || pi.GetValue(deliveryNotedataprovider, null).ToString() == "0" || pi.GetValue(deliveryNotedataprovider, null).ToString() == "00.00")
                        {
                            pi.SetValue(deliveryNotedataprovider, "", null);
                        }
                    }
                }
            }

            catch
            { }

            return deliveryNotedataprovider;
        }
        private DeliveryNoteDataProvider GetDeliveryDataProvider(string entityId, string entityobjecttableId, string childentityId, string childentityobjecttableId, int tenant)
        {
            DeliveryNoteDataProvider deliveryNotedataprovider = new DeliveryNoteDataProvider();
            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            IWebFreightContext webfreightcontext = WebFreightContext.GetContext(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
            CardRepository cardRepository = new CardRepository(tenant);
            PortRepository portRepository = new PortRepository(tenant);
            CountryRepository countryRepository = new CountryRepository(tenant);
            AddressRepository addressRepository = new AddressRepository(commonContext);

            ShipmentPM shipment = null;
            
            Tenant tenantSettings = (from a in commonContext.Tenants
                             where a.Id == tenant
                             select a).FirstOrDefault();

            ObjectTable entityObjectTable = (from co in webfreightcontext.ObjectTables
                                              where co.Id ==entityobjecttableId
                                              select co).FirstOrDefault();
            if (entityObjectTable != null)
            {
                switch (entityObjectTable.Name)
                {
                    case "Shipment":
                        shipment =shipmentQuery.GetSinglePM(entityId,tenant);
                        break;
                }
            }
            
            ObjectTable childEntityObjectTable = (from co in webfreightcontext.ObjectTables
                                                  where co.Id == childentityobjecttableId
                                             select co).FirstOrDefault();

            ShipmentPickUpDelivery myDelivery = (from a in shipmentsContext.ShipmentPickUpDeliveries
                                                 where a.Id == childentityId && a.Tenant == tenant && a.ShipmentId == entityId && a.PickUpDeliveryTypeCode == "DELV"
                                                 select a).FirstOrDefault();
            
            Card mainCarriageCard = (from a in commonContext.Cards
                                           where a.Id == shipment.MainCarriageCarrierId
                                           select a).FirstOrDefault();
            
            if (shipment != null && tenantSettings != null)
            {
                ContactQuery contactQuery = new ContactQuery(tenant);
                string email = AuthenticationUtil.GetLoggedUserEmail(tenant);
                ContactPM loggedContact = contactQuery.GetContactByEmailOnly(email, tenant);
                if (loggedContact != null)
                {
                    deliveryNotedataprovider.UserName = loggedContact.EnglishName;
                }

                deliveryNotedataprovider.ITNumber = shipment.ITNumber;
                deliveryNotedataprovider.ProjectNumber = shipment.ProjectNumber != null ? shipment.ProjectNumber : "";
                deliveryNotedataprovider.HAWB = shipment.House != null ? shipment.House : "";
                deliveryNotedataprovider.OurReferenceNumber = shipment.ShipmentNumber != null ? shipment.ShipmentNumber : "";
                deliveryNotedataprovider.DateSent = String.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date);
                deliveryNotedataprovider.DateSent_New = DateTime.Now.Date;
                deliveryNotedataprovider.ClientReferenceNumber = shipment.ShipperReference1 != null ? shipment.ShipperReference1 : "";
                deliveryNotedataprovider.MasterNumber = shipment.Master;
                deliveryNotedataprovider.ConsigneeRef1 = shipment.ConsigneeReference1;
                deliveryNotedataprovider.ShipmentNotes = shipment.Notes;
                deliveryNotedataprovider.MainCarriageETA = shipment.MainCarriageETA != null ? String.Format("{0:dd MMM yyyy}", shipment.MainCarriageETA) : "";
                deliveryNotedataprovider.LongMaster = shipment.LongMaster;
                deliveryNotedataprovider.MainCarriageVesselName = shipment.MainCarriageVesselName;
                deliveryNotedataprovider.MainCarriageCarrierName = shipment.MainCarriageCarrierName;
                deliveryNotedataprovider.ShipperReference2 = shipment.ShipperReference2;
                deliveryNotedataprovider.ConsigneeReference2 = shipment.ConsigneeReference2;
                deliveryNotedataprovider.ShipmentSalesman = shipment.SalesmanUserName;
                deliveryNotedataprovider.LastFreeDate = shipment.WarehouseLegLastFreeDate;
                deliveryNotedataprovider.FinalDestinationCode = shipment.MainCarriageFinalDestinationPortCode;
                deliveryNotedataprovider.AMSBL = shipment.AMSBL;

                if (!string.IsNullOrEmpty(shipment.FreightLocationId))
                {
                    Card card = cardRepository.GetSingleCard(shipment.FreightLocationId, tenant);
                    if (card != null)
                    {
                        deliveryNotedataprovider.FreightLocation = card.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(shipment.OnCarriageCarrierId))
                {
                    deliveryNotedataprovider.OnCarriageCarrier = shipment.OnCarriageCarrierName;
                }

                if (shipment.CutoffDate != null)
                {
                    deliveryNotedataprovider.CutOffDate = shipment.CutoffDate != null ? String.Format("{0:dd MMM yyyy}", shipment.CutoffDate) : "";
                    deliveryNotedataprovider.CutOffDateAsDate = shipment.CutoffDate;
                    deliveryNotedataprovider.CutOffTime = shipment.CutoffDate != null ? String.Format("{0:hh:mm:ss}", shipment.CutoffDate) : "";
                }

                if (!string.IsNullOrEmpty(shipment.CustomerContactId))
                {
                    Contact contact = commonContext.Contacts.Where(d => d.Id == shipment.CustomerContactId && d.Tenant == tenant).FirstOrDefault();
                    if (contact != null)
                    {
                        deliveryNotedataprovider.CustomerContactName = contact.EnglishName;
                        deliveryNotedataprovider.CustomerContactPhoneNumber = contact.BusinessPhone;
                    }
                }

                if (!string.IsNullOrEmpty(shipment.ConsigneeId))
                {
                    #region
                    Card myCard = CardRepository.GetSingleCard(shipment.ConsigneeId, tenant, true);
                    if (myCard != null)
                    {
                        deliveryNotedataprovider.ConsigneeName = myCard.EnglishName;

                        if (!string.IsNullOrEmpty(shipment.ConsigneeAddressId))
                        {
                            Address myAddress = addressRepository.GetSingleAddress(shipment.ConsigneeAddressId, tenant);
                            if (myAddress != null)
                            {
                                deliveryNotedataprovider.ConsigneeAddress = DataProviders.General.GetAddress(myAddress);

                                if (!string.IsNullOrEmpty(myAddress.PhoneNumber) || !string.IsNullOrEmpty(myAddress.FaxNumber))
                                {
                                    deliveryNotedataprovider.ConsigneeAddress += Environment.NewLine;
                                }

                                if (!string.IsNullOrEmpty(myAddress.PhoneNumber))
                                {
                                    deliveryNotedataprovider.ConsigneeAddress += "Tel: " + myAddress.PhoneNumber + " ";
                                }

                                if (!string.IsNullOrEmpty(myAddress.FaxNumber))
                                {
                                    deliveryNotedataprovider.ConsigneeAddress += "Fax: " + myAddress.FaxNumber;
                                }
                            }
                        }
                    }
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.ShipperId))
                {
                    #region
                    Card myCard = CardRepository.GetSingleCard(shipment.ShipperId, tenant, true);
                    if (myCard != null)
                    {
                        deliveryNotedataprovider.ShipperName = myCard.EnglishName;

                        if (!string.IsNullOrEmpty(shipment.ShipperAddressId))
                        {
                            Address myAddress = addressRepository.GetSingleAddress(shipment.ShipperAddressId, tenant);
                            if (myAddress != null)
                            {
                                deliveryNotedataprovider.ShipperAddress = DataProviders.General.GetAddress(myAddress);

                                if (!string.IsNullOrEmpty(myAddress.PhoneNumber) || !string.IsNullOrEmpty(myAddress.FaxNumber))
                                {
                                    deliveryNotedataprovider.ShipperAddress += Environment.NewLine;
                                }

                                if (!string.IsNullOrEmpty(myAddress.PhoneNumber))
                                {
                                    deliveryNotedataprovider.ShipperAddress += "Tel: " + myAddress.PhoneNumber + " ";
                                }

                                if (!string.IsNullOrEmpty(myAddress.FaxNumber))
                                {
                                    deliveryNotedataprovider.ShipperAddress += "Fax: " + myAddress.FaxNumber;
                                }
                            }
                        }
                    }
                    #endregion
                }

                if (!string.IsNullOrEmpty(shipment.IssuingCarrierAgentId))
                {
                    #region
                    Card myCard = CardRepository.GetSingleCard(shipment.IssuingCarrierAgentId, tenant, true);
                    if (myCard != null)
                    {
                        deliveryNotedataprovider.IssuingCarrierAgentName = myCard.EnglishName;

                        if (!string.IsNullOrEmpty(shipment.IssuingCarrierAddressId))
                        {
                            Address myAddress = addressRepository.GetSingleAddress(shipment.IssuingCarrierAddressId, tenant);
                            if (myAddress != null)
                            {
                                deliveryNotedataprovider.IssuingCarrierAgentAddress = DataProviders.General.GetAddress(myAddress);

                                if (!string.IsNullOrEmpty(myAddress.PhoneNumber) || !string.IsNullOrEmpty(myAddress.FaxNumber))
                                {
                                    deliveryNotedataprovider.IssuingCarrierAgentAddress += Environment.NewLine;
                                }

                                if (!string.IsNullOrEmpty(myAddress.PhoneNumber))
                                {
                                    deliveryNotedataprovider.IssuingCarrierAgentAddress += "Tel: " + myAddress.PhoneNumber + " ";
                                }

                                if (!string.IsNullOrEmpty(myAddress.FaxNumber))
                                {
                                    deliveryNotedataprovider.IssuingCarrierAgentAddress += "Fax: " + myAddress.FaxNumber;
                                }
                            }
                        }
                    }
                    #endregion
                }
                if (!string.IsNullOrEmpty(shipment.SalesmanUserId))
                {
                    #region
                    Contact salesmanContact = ContactRepository.GetSingleContact(shipment.SalesmanUserId, tenant, true);
                    if (salesmanContact != null)
                    {
                        deliveryNotedataprovider.SalesmanEmail = salesmanContact.Email;
                    }
                    #endregion
                }
                // tenant data
                if (tenantSettings != null)
                {
                    deliveryNotedataprovider.Signature = tenantSettings.Signature != null ? tenantSettings.Signature : "";
                    deliveryNotedataprovider.TenantName = tenantSettings.Company != null ? tenantSettings.Company : "";
                }

                if (myDelivery != null)
                {
                    deliveryNotedataprovider.TruckNumber = myDelivery.TruckNumber;
                    deliveryNotedataprovider.TruckerNumber = myDelivery.CarrierNumber;
                    deliveryNotedataprovider.DriverName = myDelivery.Driver;

                    if (myDelivery.ETA != null)
                    {
                        deliveryNotedataprovider.DeliveryETADate = myDelivery.ETA;
                        deliveryNotedataprovider.DeliveryETATime = myDelivery.ETA;
                    }

                    ShipmentPackage myShipmentPackage = (from a in shipmentsContext.ShipmentPackages
                                                         where a.DeliveryId == myDelivery.Id && a.Tenant == tenant
                                                         select a).FirstOrDefault();

                    if (myShipmentPackage != null)
                    {
                        deliveryNotedataprovider.Reference1 = myShipmentPackage.Reference1;
                        deliveryNotedataprovider.Reference2 = myShipmentPackage.Reference2;
                        deliveryNotedataprovider.Reference3 = myShipmentPackage.Reference3;
                        deliveryNotedataprovider.Reference4 = myShipmentPackage.Reference4;
                    }

                    #region trucker
                    if (myDelivery.CarrierId != null)
                    {
                        Card trucker = (from a in commonContext.Cards
                                        where a.Id == myDelivery.CarrierId
                                        select a).FirstOrDefault();

                        if (trucker != null)
                        {
                            deliveryNotedataprovider.To = trucker.EnglishName != null ? trucker.EnglishName : "";

                            Address address = addressRepository.GetSingleAddressByCardIdAndTypeId(trucker.Id, "M", tenant);

                            Contact truckerContact = null;

                            CardContact truckerCardContact = (from cc in commonContext.CardContacts
                                                              where cc.CardId == trucker.Id
                                                              select cc).FirstOrDefault();

                            if (truckerCardContact != null)
                            {
                                truckerContact = ContactRepository.GetSingleContact(truckerCardContact.ContactId, myDelivery.Tenant, false);
                            }
                            if (truckerContact != null)
                            {
                                deliveryNotedataprovider.Salesman = truckerContact.EnglishName != null ? truckerContact.EnglishName : "";
                                deliveryNotedataprovider.SalesmanEmail = truckerContact.Email != null ? truckerContact.Email : "";
                            }

                            deliveryNotedataprovider.Telephone = address != null ? (address.PhoneNumber != null ? address.PhoneNumber : "") : "";
                        }
                    }
                    #endregion

                    #region Delivery From
                    deliveryNotedataprovider.PickupDate = myDelivery.ETD != null ? String.Format("{0:dd/MMM/yyyy}", myDelivery.ETD) : "";
                    deliveryNotedataprovider.PickupTime = myDelivery.ETD != null ? String.Format("{0:hh:mm}", myDelivery.ETD) : "";
                    deliveryNotedataprovider.PickupTime_DateTime_New = myDelivery.ETD != null ? myDelivery.ETD : null;
                    deliveryNotedataprovider.SpecialInstructions = myDelivery.Notes != null ? myDelivery.Notes : "";

                    #region From PART
                    if (myDelivery.PickUpDeliveryFromTypeCode == "PART")
                    {
                        if (string.IsNullOrEmpty(myDelivery.FromPartnerCardId))
                        {
                            deliveryNotedataprovider.PickupAddress = myDelivery.FromAddress != null ? myDelivery.FromAddress : "";
                        }

                        else
                        {
                            Card card = cardRepository.GetSingleCard(myDelivery.FromPartnerCardId, tenant);
                            if (card != null)
                            {
                                deliveryNotedataprovider.PickupCompanyName = card.EnglishName;

                                CardContact cardContact = (from a in commonContext.CardContacts where a.CardId == card.Id select a).FirstOrDefault();
                                if (cardContact != null)
                                {
                                    Contact contact = cardContact.Contact;

                                    if (contact != null)
                                    {
                                        deliveryNotedataprovider.PickupContactPhone = contact.BusinessPhone != null ? contact.BusinessPhone : "";
                                    }
                                }

                                if (string.IsNullOrEmpty(myDelivery.FromAddressId))
                                {
                                    deliveryNotedataprovider.PickupAddress = myDelivery.FromAddress != null ? myDelivery.FromAddress : "";
                                }

                                else
                                {
                                    Address address = addressRepository.GetSingleAddress(myDelivery.FromAddressId, tenant);
                                    if (address != null)
                                    {

                                        deliveryNotedataprovider.PickupAddress = DataProviders.General.GetAddress(address)
                                            + (!string.IsNullOrEmpty(address.ATTN) ? (Environment.NewLine + "Contact : " + address.ATTN) : "")
                                            + (!string.IsNullOrEmpty(address.PhoneNumber) ? (Environment.NewLine + "Phone : " + address.PhoneNumber) : "");
                                        deliveryNotedataprovider.FromAddressDescription = address.Description != null ? address.Description : "";

                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region From PORT
                    else if (myDelivery.PickUpDeliveryFromTypeCode == "PORT")
                    {
                        if (!string.IsNullOrEmpty(myDelivery.FromPortId))
                        {
                            Port port = portRepository.GetSinglePort(tenant, myDelivery.FromPortId);
                            if (port != null)
                            {
                                deliveryNotedataprovider.PickupCompanyName = port.EnglishName != null ? port.EnglishName : "";
                            }
                        }

                        deliveryNotedataprovider.PickupAddress = myDelivery.FromAddress != null ? myDelivery.FromAddress : "";
                    }
                    #endregion

                    #region From CASL
                    else
                    {
                        string location = "";

                        if (!string.IsNullOrEmpty(myDelivery.FromAddressCity))
                        {
                            location = myDelivery.FromAddressCity;
                        }

                        if (!string.IsNullOrEmpty(myDelivery.FromAddressZipCode))
                        {
                            location = location + " " + myDelivery.FromAddressZipCode;
                        }

                        if (!string.IsNullOrEmpty(myDelivery.FromAddressCountryId))
                        {
                            Country country = countryRepository.GetSingleCountry(myDelivery.FromAddressCountryId, tenant);
                            if (country != null)
                            {
                                location = location + Environment.NewLine + country.EnglishName;
                            }
                        }

                        deliveryNotedataprovider.PickupAddress = location;
                    }
                    #endregion

                    #endregion

                    #region Delivery To
                    deliveryNotedataprovider.DeliveryDate = myDelivery.ETA != null ? String.Format("{0:dd/MMM/yyyy}", myDelivery.ETA) : "";
                    deliveryNotedataprovider.DeliveryTime = myDelivery.ETA != null ? String.Format("{0:hh:mm}", myDelivery.ETA) : "";
                    deliveryNotedataprovider.DeliveryTime_DateTime_New = myDelivery.ETA != null ? myDelivery.ETA : null;

                    #region To PART
                    if (myDelivery.PickUpDeliveryToTypeCode == "PART")
                    {
                        if (string.IsNullOrEmpty(myDelivery.ToPartnerCardId))
                        {
                            deliveryNotedataprovider.DeliveryAddress = myDelivery.ToAddress != null ? myDelivery.ToAddress : "";
                        }

                        else
                        {
                            Card card = cardRepository.GetSingleCard(myDelivery.ToPartnerCardId, tenant);
                            if (card != null)
                            {
                                deliveryNotedataprovider.DeliveryCompanyName = card.EnglishName;
                                deliveryNotedataprovider.DeliveryCompanyLocalName = card.LocalName;

                                Contact primaryContact = ContactRepository.GetSingleContact(card.PrimaryContactId, tenant, false);
                                if (primaryContact != null)
                                {
                                    deliveryNotedataprovider.DeliveryCompanyContactLocalName = primaryContact.LocalName;
                                    deliveryNotedataprovider.DeliveryCompanyContactPhone = primaryContact.BusinessPhone == null ? "" : primaryContact.BusinessPhone;
                                }

                                CardContact cardContact = (from a in commonContext.CardContacts where a.CardId == card.Id select a).FirstOrDefault();
                                if (cardContact != null)
                                {
                                    ContactRepository contactRepository = new ContactRepository(commonContext);
                                    Contact contact = contactRepository.GetSingleContact(cardContact.ContactId, tenant);

                                    if (contact != null)
                                    {
                                        deliveryNotedataprovider.DeliveryContactPhone = contact.BusinessPhone != null ? contact.BusinessPhone : "";
                                        deliveryNotedataprovider.DeliveryContactName = contact.EnglishName != null ? contact.EnglishName : "";
                                    }
                                }

                                if (string.IsNullOrEmpty(myDelivery.ToAddressId))
                                {
                                    deliveryNotedataprovider.DeliveryAddress = myDelivery.ToAddress != null ? myDelivery.ToAddress : "";
                                }

                                else
                                {
                                    Address address = addressRepository.GetSingleAddress(myDelivery.ToAddressId, tenant);
                                    if (address != null)
                                    {

                                        deliveryNotedataprovider.DeliveryAddress = DataProviders.General.GetAddress(address)
                                            + (!string.IsNullOrEmpty(address.ATTN) ? (Environment.NewLine + "Contact : " + address.ATTN) : "")
                                            + (!string.IsNullOrEmpty(address.PhoneNumber) ? (Environment.NewLine + "Phone : " + address.PhoneNumber) : "");
                                        deliveryNotedataprovider.ToAddressDescription = address.Description != null ? address.Description : "";
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region To PORT
                    else if (myDelivery.PickUpDeliveryToTypeCode == "PORT")
                    {
                        if (!string.IsNullOrEmpty(myDelivery.ToPortId))
                        {
                            Port port = portRepository.GetSinglePort(tenant, myDelivery.ToPortId);
                            if (port != null)
                            {
                                deliveryNotedataprovider.DeliveryCompanyName = port.EnglishName != null ? port.EnglishName : "";
                                deliveryNotedataprovider.DeliveryCompanyLocalName = port.LocalName != null ? port.LocalName : "";
                            }
                        }

                        deliveryNotedataprovider.DeliveryAddress = myDelivery.ToAddress != null ? myDelivery.ToAddress : "";
                    }
                    #endregion

                    #region From CASL
                    else
                    {
                        string location = "";

                        if (!string.IsNullOrEmpty(myDelivery.ToAddressCity))
                        {
                            location = myDelivery.ToAddressCity;
                        }

                        if (!string.IsNullOrEmpty(myDelivery.ToAddressZipCode))
                        {
                            location = location + " " + myDelivery.ToAddressZipCode;
                        }

                        if (!string.IsNullOrEmpty(myDelivery.ToAddressCountryId))
                        {
                            Country country = countryRepository.GetSingleCountry(myDelivery.ToAddressCountryId, tenant);
                            if (country != null)
                            {
                                location = location + Environment.NewLine + country.EnglishName;
                            }
                        }

                        deliveryNotedataprovider.DeliveryAddress = location;
                    }
                    #endregion

                    #endregion

                    #region MoveType
                    if (!string.IsNullOrEmpty(shipment.MoveTypeId))
                    {
                        MoveType moveType = webfreightcontext.MoveTypes.Where(m => m.Id == shipment.MoveTypeId).FirstOrDefault();

                        if (moveType != null)
                        {
                            deliveryNotedataprovider.MoveTypeCode = moveType.Code;
                            deliveryNotedataprovider.MoveTypeName = moveType.MoveTypeEnglishName;
                        }
                    }
                    #endregion

                    #region FreightForwarder
                    string myForwarderId = shipment.FreightForwarderId;
                    string myForwarderAddressId = shipment.FreightForwarderAddressId;
                    if (!string.IsNullOrEmpty(myForwarderId))
                    {
                        Card myPartnerCard = CardRepository.GetSingleCard(myForwarderId, tenant, true);

                        if (myPartnerCard != null)
                        {
                            deliveryNotedataprovider.ForwarderAgentCode = myPartnerCard.Code;
                            deliveryNotedataprovider.ForwarderAgentAddress = myPartnerCard.EnglishName != null ? myPartnerCard.EnglishName + Environment.NewLine : "";

                            if (!string.IsNullOrEmpty(myForwarderAddressId))
                            {
                                Address myPartnerAddress = addressRepository.GetSingleAddress(myForwarderAddressId, tenant);

                                if (myPartnerAddress != null)
                                {
                                    if (myPartnerAddress.IsLocalLanguage && !string.IsNullOrEmpty(myPartnerCard.LocalName))
                                    {
                                        deliveryNotedataprovider.ForwarderAgentAddress = myPartnerCard.LocalName + Environment.NewLine;
                                    }

                                    deliveryNotedataprovider.ForwarderAgentAddress = deliveryNotedataprovider.ForwarderAgentAddress + DataProviders.General.GetAddress(myPartnerAddress);

                                    if (myPartnerAddress.PhoneNumber != null || myPartnerAddress.FaxNumber != null)
                                    {
                                        deliveryNotedataprovider.ForwarderAgentAddress = deliveryNotedataprovider.ForwarderAgentAddress + Environment.NewLine + (myPartnerAddress.PhoneNumber != null ? "Tel: " + myPartnerAddress.PhoneNumber + " " : "") + (myPartnerAddress.FaxNumber != null ? "Fax: " + myPartnerAddress.FaxNumber + " " : "");
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region Packages
                    List<ShipmentPickUpDeliveryPackage> packages = shipmentsContext.ShipmentPickUpDeliveryPackages.Where(d => d.ShipmentPickUpDeliveryId == myDelivery.Id && d.Tenant == tenant).ToList();
                    List<ShipmentPackage> shipmentPackages = shipmentsContext.ShipmentPackages.Where(d => d.ShipmentId == myDelivery.ShipmentId && d.Tenant == tenant).ToList();
                    List<string> shipmentPackagesIds = shipmentsContext.ShipmentPackages.Where(d => d.ShipmentId == shipment.Id && d.Tenant == tenant).Select(s => s.Id).ToList();

                    if (shipmentPackagesIds != null && shipmentPackagesIds.Count > 0)
                    {
                        List<InsideShipmentPackage> insidePackages = shipmentsContext.InsideShipmentPackages.Where(d => shipmentPackagesIds.Contains(d.ShipmentPackageId)).ToList();

                        if (insidePackages != null && insidePackages.Count > 0)
                        {
                            deliveryNotedataprovider.InsidePackagesLines = new List<InsidePackageLine>();

                            foreach (InsideShipmentPackage item in insidePackages)
                            {
                                InsidePackageLine insidePackageLine = new InsidePackageLine();

                                PackageType insidePackageType = (from pa in commonContext.PackageTypes
                                                                 where pa.Id == item.PackageTypeId
                                                                 select pa).FirstOrDefault();

                                insidePackageLine.PackageType = insidePackageType == null ? "" : insidePackageType.EnglishName;
                                insidePackageLine.Quantity = item.Quantity;

                                if (item.Length != null && item.Width != null && item.Height != null)
                                {
                                    insidePackageLine.Dimensions = item.Length + "x" + item.Width + "x" + item.Height;
                                }

                                insidePackageLine.Volume = item.Volume;
                                insidePackageLine.VolumetricWeight = item.VolumetricWeight;
                                insidePackageLine.Weight = item.Weight;
                                insidePackageLine.Description = item.Description;

                                #region Car Details
                                insidePackageLine.Make = item.Make;
                                insidePackageLine.Model = item.Model;
                                insidePackageLine.Year = item.Year;
                                insidePackageLine.Color = item.Color;
                                insidePackageLine.ChassisNumber = item.ChassisNumber;
                                insidePackageLine.RegistrationNumber = item.RegistrationNumber;

                                if (!string.IsNullOrEmpty(item.CountryId))
                                {
                                    Country country = CountryRepository.GetSingleCountry(item.CountryId, tenant, true);
                                    if (country != null)
                                    {
                                        insidePackageLine.CountryName = country.EnglishName;
                                    }
                                }
                                #endregion

                                deliveryNotedataprovider.InsidePackagesLines.Add(insidePackageLine);
                            }
                        }
                    }

                    int? totalQuantity = 0;
                    double? totalWeight = 0;
                    double? totalVolume = 0;

                    deliveryNotedataprovider.PackagesLines = new List<PackageLine>();
                    deliveryNotedataprovider.AttachmentList = new List<PackageLine>();
                    int counter = 1;

                    string volumeUnitCode = shipment.VolumeUnitCode != null ? shipment.VolumeUnitCode : "";

                    foreach (ShipmentPickUpDeliveryPackage package in packages)
                    {
                        counter++;
                        PackageLine packageline = new PackageLine();

                        packageline.PackageDescriptionOfGoods = package.Description != null ? package.Description : "";
                        packageline.PackageGrossWeight = package.Weight != null ? (package.Weight.Value.ToString() + " " + shipment.GrossWeightUnitCode) : ""; //+ "  KG" : "";
                        packageline.PackageQuantity = package.Quantity != null ? package.Quantity.Value.ToString() : "";
                        packageline.PackageType = package.PackageType != null ? package.PackageType.EnglishName : "Package";
                        packageline.PackageVolume = package.Volume != null ? (package.Volume + " " + shipment.VolumeUnitCode) : "";// + "  CBM" : "";
                        packageline.SealNumber = package.ShipperSeal;
                        packageline.Width = package.Width == null ? "" : package.Width.ToString();
                        packageline.Height = package.Height == null ? "" : package.Height.ToString();
                        packageline.Length = package.Length == null ? "" : package.Length.ToString();

                        if (package.Width != null && package.Height != null && package.Length != null)
                        {
                            packageline.Dimensions = package.Length + " x " + package.Width + " x " + package.Height + " " + shipment.DimensionsUnitCode;
                        }

                        packageline.ContainerNumber = package.ContainerNumber;

                        #region Car Details
                        packageline.Make = package.Make;
                        packageline.Model = package.Model;
                        packageline.Year = package.Year;
                        packageline.Color = package.Color;
                        packageline.ChassisNumber = package.ChassisNumber;
                        packageline.RegistrationNumber = package.RegistrationNumber;

                        if (!string.IsNullOrEmpty(package.CountryId))
                        {
                            Country country = CountryRepository.GetSingleCountry(package.CountryId, tenant, true);
                            if (country != null)
                            {
                                packageline.CountryName = country.EnglishName;
                            }
                        }
                        #endregion

                        #region Harmonize
                        if (package.IsMultiHarmonize)
                        {
                            List<PickUpDeliveryPackageHarmonize> allHarmonizes = shipmentsContext.PickUpDeliveryPackageHarmonizes.Where(d => d.PackageId == package.Id && d.Tenant == package.Tenant).ToList();
                            foreach (PickUpDeliveryPackageHarmonize itemHarmonize in allHarmonizes)
                            {
                                if (string.IsNullOrEmpty(packageline.HSCode))
                                {
                                    packageline.HSCode = itemHarmonize.Harmonize;
                                }
                                else
                                {
                                    packageline.HSCode += "," + itemHarmonize.Harmonize;
                                }
                            }
                        }
                        else
                        {
                            packageline.HSCode = package.Harmonize;
                        }
                        #endregion

                        PackageType packtype = (from pa in commonContext.PackageTypes
                                                where pa.Id == package.PackageTypeId
                                                select pa).FirstOrDefault();

                        if (packtype != null)
                        {
                            packageline.ContainerSize = packtype.ContainerSize.ToString();
                            packageline.PackageType = packtype.EnglishName != null ? packtype.EnglishName : "Package";
                            packageline.IsContainer = packtype.IsContainer;
                        }

                        totalQuantity += package.Quantity;
                        totalWeight += package.Weight;
                        totalVolume += package.Volume;

                        //InsidePackages
                        packageline.InsidePackagesLines = new List<InsidePackageLine>();
                        ShipmentPackage shipmentPackage = shipmentPackages.Where(d => d.ContainerNumber == package.ContainerNumber).FirstOrDefault();
                        if (shipmentPackage != null)
                        {
                            List<InsideShipmentPackage> insidePackages = shipmentsContext.InsideShipmentPackages.Where(d => d.ShipmentPackageId == shipmentPackage.Id && d.Tenant == tenant).ToList();
                            foreach (InsideShipmentPackage insideItem in insidePackages)
                            {
                                PackageType insidePackageType = (from pa in commonContext.PackageTypes
                                                                 where pa.Id == insideItem.PackageTypeId
                                                                 select pa).FirstOrDefault();

                                InsidePackageLine insidePackage = new InsidePackageLine();

                                insidePackage.PackageType = insidePackageType == null ? "" : insidePackageType.EnglishName;
                                insidePackage.Quantity = insideItem.Quantity;

                                if (insideItem.Length != null && insideItem.Width != null && insideItem.Height != null)
                                {
                                    insidePackage.Dimensions = insideItem.Length + "x" + insideItem.Width + "x" + insideItem.Height;
                                }

                                insidePackage.Volume = insideItem.Volume;
                                insidePackage.VolumetricWeight = insideItem.VolumetricWeight;
                                insidePackage.Weight = insideItem.Weight;
                                insidePackage.Description = insideItem.Description;

                                packageline.InsidePackagesLines.Add(insidePackage);
                            }
                        }

                        deliveryNotedataprovider.PackagesLines.Add(packageline);
                    }

                    deliveryNotedataprovider.TotalNumberOfPackages = totalQuantity;
                    deliveryNotedataprovider.TotalGrossWeight = totalWeight;
                    deliveryNotedataprovider.TotalVolume = totalVolume;
                    #endregion

                    #region  Last Vessel
                    string vesselNameAndNumber = "";

                    if (!string.IsNullOrEmpty(shipment.Transshipment3FromPortId))
                    {
                        Vessel vessel = (from a in commonContext.Vessels
                                         where a.Id == shipment.Transshipment3VesselId
                                         select a).FirstOrDefault();
                        if (vessel != null)
                        {
                            vesselNameAndNumber = vessel.EnglishName + " \\ " + shipment.MainCarriageCarrierNumber;
                        }
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(shipment.Transshipment2FromPortId))
                        {
                            Vessel vessel = (from a in commonContext.Vessels
                                             where a.Id == shipment.Transshipment2VesselId
                                             select a).FirstOrDefault();
                            if (vessel != null)
                            {
                                vesselNameAndNumber = vessel.EnglishName + " \\ " + shipment.MainCarriageCarrierNumber;
                            }
                        }

                        else
                        {
                            if (!string.IsNullOrEmpty(shipment.Transshipment1FromPortId))
                            {
                                Vessel vessel = (from a in commonContext.Vessels
                                                 where a.Id == shipment.Transshipment1VesselId
                                                 select a).FirstOrDefault();
                                if (vessel != null)
                                {
                                    vesselNameAndNumber = vessel.EnglishName + " \\ " + shipment.MainCarriageCarrierNumber;
                                }
                            }

                            else
                            {
                                Vessel vessel = (from a in commonContext.Vessels
                                                 where a.Id == shipment.MainCarriageVesselId
                                                 select a).FirstOrDefault();
                                if (vessel != null)
                                {
                                    vesselNameAndNumber = vessel.EnglishName + " \\ " + shipment.MainCarriageCarrierNumber;
                                }
                            }
                        }
                    }

                    deliveryNotedataprovider.LastMainCarriageVesselNameAndNumber = vesselNameAndNumber;

                    if (!string.IsNullOrEmpty(shipment.MainCarriageVesselId))
                    {
                        var vesselMCNameAndNumber = "";
                        Vessel vessel = (from a in commonContext.Vessels
                                         where a.Id == shipment.MainCarriageVesselId
                                         select a).FirstOrDefault();
                        if (vessel != null)
                        {
                            vesselMCNameAndNumber = vessel.EnglishName + " \\ " + shipment.MainCarriageCarrierNumber;
                        }
                        deliveryNotedataprovider.MainCarriageVesselNameAndNumber = vesselMCNameAndNumber;
                    }
                    #endregion

                    #region EmptyContainer
                    deliveryNotedataprovider.EmptyContainerReturnRef = myDelivery.EmptyDeliveryDepotReference;

                    if (!string.IsNullOrEmpty(myDelivery.EmptyDeliveryContainerPartnerId))
                    {
                        Card myCard = CardRepository.GetSingleCard(myDelivery.EmptyDeliveryContainerPartnerId, tenant, true);

                        if (myCard != null)
                        {
                            string myEmptyContainer = null;

                            myEmptyContainer = myCard.EnglishName != null ? myCard.EnglishName : "";

                            Address theAddress = addressRepository.GetMainAddressByCardId(myCard.Id, tenant);

                            if (theAddress != null)
                            {
                                if (theAddress.IsLocalLanguage && !string.IsNullOrEmpty(myCard.LocalName))
                                {
                                    myEmptyContainer = myCard.LocalName;
                                }

                                myEmptyContainer = myEmptyContainer + Environment.NewLine + DataProviders.General.GetAddress(theAddress);

                                if (theAddress.PhoneNumber != null)
                                {
                                    myEmptyContainer = myEmptyContainer + Environment.NewLine + "Phone No. : " + theAddress.PhoneNumber;
                                }

                                if (theAddress.FaxNumber != null)
                                {
                                    myEmptyContainer = myEmptyContainer + "   Fax No. : " + theAddress.FaxNumber;
                                }
                            }

                            deliveryNotedataprovider.EmptyContainerReturn = myEmptyContainer;
                        }
                    }
                    #endregion

                    //LoadingPortName
                    if (!string.IsNullOrEmpty(shipment.MainCarriageFromPortId))
                    {
                        deliveryNotedataprovider.LoadingPortName = shipment.MainCarriageFromPortName;
                        deliveryNotedataprovider.OriginCountry = shipment.MainCarriageFromPortCountryName;
                    }

                    //DischargePortName
                    if (!string.IsNullOrEmpty(shipment.Transshipment3ToPortId))
                    {
                        deliveryNotedataprovider.DischargePortName = shipment.Transshipment3ToPortName;
                    }

                    else if (!string.IsNullOrEmpty(shipment.Transshipment2ToPortId))
                    {
                        deliveryNotedataprovider.DischargePortName = shipment.Transshipment2ToPortName;
                    }

                    else if (!string.IsNullOrEmpty(shipment.Transshipment1ToPortId))
                    {
                        deliveryNotedataprovider.DischargePortName = shipment.Transshipment1ToPortName;
                    }

                    else if (!string.IsNullOrEmpty(shipment.MainCarriageToPortId))
                    {
                        deliveryNotedataprovider.DischargePortName = shipment.MainCarriageToPortName;
                    }

                    //Dates
                    deliveryNotedataprovider.MainCarriageETD_DateTime = shipment.MainCarriageETD;
                    deliveryNotedataprovider.MainCarriageETA_DateTime = shipment.MainCarriageETA;
                    deliveryNotedataprovider.MainCarriageATA_DateTime = shipment.MainCarriageATA;

                    if (myDelivery.TransportModeCode != null)
                    {
                        PickUpDeliveryTransportMode myTransportMode = shipmentsContext.PickUpDeliveryTransportModes.Where(d => d.Code == myDelivery.TransportModeCode).FirstOrDefault();
                        if (myTransportMode != null)
                        {
                            deliveryNotedataprovider.TransportMode = myTransportMode.Name;
                        }
                    }
                }

                deliveryNotedataprovider.DescriptionOfGoods = shipment.DescriptionOfGoods != null ? shipment.DescriptionOfGoods : "";

                CustomFieldResolver customFieldResolver = new CustomFieldResolver();
                customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, shipment, deliveryNotedataprovider);
            }

            Type deliveryNoteType = deliveryNotedataprovider.GetType();
            PropertyInfo[] properties = deliveryNoteType.GetProperties();
            foreach (PropertyInfo pi in properties)
            {
                string propertytype = pi.PropertyType.FullName;

                if (propertytype == "System.String")
                {
                    if (pi.GetValue(deliveryNotedataprovider, null) == null || pi.GetValue(deliveryNotedataprovider, null).ToString() == "0" || pi.GetValue(deliveryNotedataprovider, null).ToString() == "00.00")
                    {
                        pi.SetValue(deliveryNotedataprovider, "", null);
                    }
                }
            }

            return deliveryNotedataprovider;
        }
    }
}
